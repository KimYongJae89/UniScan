using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Settings;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    public class ResultCombiner : IInspectedListner, INotifyPropertyChanged
    {
        static ResultCombiner _instance;
        public static ResultCombiner Instance()
        {
            if (_instance == null)
                _instance = new ResultCombiner();

            return _instance;
        }

        List<InspectResult> resultList = new List<InspectResult>();
        Dictionary<ImageDevice, Int32Rect> targetRegionDictionary = new Dictionary<ImageDevice, Int32Rect>();

        private List<IInspectedListner> inspectedListnerList = new List<IInspectedListner>();
        bool running = false;

        public int ListCount { get => resultList.Count; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ResultCombiner()
        {
            SystemManager.Instance().AddInspectedIListner(this);
            Load();
        }

        public InspectResult Combine(InspectResult inspectResult)
        {
            if (inspectResult.DetectorResult is PinHoleDetectorResult == false)
                return null;

            InspectResult foundResult = null;
            lock (this)
            {
                foundResult = resultList.Find(result => result.Index == inspectResult.Index);

                if (foundResult == null)
                {
                    resultList.Add(inspectResult);

                    OnPropertyChanged("ListCount");
                    return null;
                }

                resultList.Remove(foundResult);
            }

            OnPropertyChanged("ListCount");

            InspectResult combineResult = new InspectResult(PinHoleColor.Data.DetectorType.PinHole);
            combineResult.StartTime = inspectResult.StartTime < foundResult.StartTime ? inspectResult.StartTime : foundResult.StartTime;
            combineResult.EndTime = inspectResult.StartTime > foundResult.StartTime ? inspectResult.StartTime : foundResult.StartTime;

            combineResult.Index = inspectResult.Index;
            combineResult.DetectorResult = CombineResult(new List<InspectResult>() { inspectResult, foundResult }, out BitmapSource bitmapSource);
            combineResult.TargetImage = bitmapSource;

            Production production = SystemManager.Instance().ProductionManager.CurProduction.PinHoleProduction;

            if (inspectResult.Judgment == DynMvp.InspData.Judgment.Skip || foundResult.Judgment == DynMvp.InspData.Judgment.Skip)
            {
                combineResult.DetectorResult.Judgment = DynMvp.InspData.Judgment.Skip;
                production.AddPass();
            }
            else if (inspectResult.Judgment == DynMvp.InspData.Judgment.Accept && foundResult.Judgment == DynMvp.InspData.Judgment.Accept)
            {
                combineResult.DetectorResult.Judgment = DynMvp.InspData.Judgment.Accept;
                production.AddGood();
            }
            else
            {
                production.AddNG();
            }

            production.AddTotal();
            
            combineResult.ResultPath = Path.Combine(SystemManager.Instance().ProductionManager.DefaultPath, production.StartTime.ToString("yyyyMMdd"), production.LotNo, "PinHole", combineResult.Index.ToString());

            return combineResult;
        }
        
        public DetectorResult CombineResult(List<InspectResult> resultList ,out BitmapSource combineImage)
        {
            int imageWidth = (int)(targetRegionDictionary.Sum(pair => pair.Value.Width));
            int imageHeight = (int)resultList.Min(result => result.TargetImage.Height);
            int interestX = int.MaxValue;
            int interestEndX = 0;

            byte[] destArray = new byte[imageWidth * imageHeight];
            
            PinHoleDetectorResult combineResult = new PinHoleDetectorResult();
            foreach (KeyValuePair<ImageDevice, Int32Rect> pair in targetRegionDictionary)
            {
                List<KeyValuePair<ImageDevice, Int32Rect>> prevList = targetRegionDictionary.TakeWhile(tempPair => tempPair.Key.Index < pair.Key.Index).ToList();
                int prevWidth = prevList.Count == 0 ? 0 : prevList.Sum(prev => prev.Value.Width);

                InspectResult inspectResult = resultList.Find(result => result.TargetDevice == pair.Key);

                if (inspectResult == null)
                    continue;

                PinHoleDetectorResult detectorResult = (PinHoleDetectorResult)inspectResult.DetectorResult;

                if (prevWidth + detectorResult.InterestRegion.X < interestX)
                    interestX = prevWidth + detectorResult.InterestRegion.X;

                if (prevWidth + detectorResult.InterestRegion.Width > interestEndX)
                    interestEndX = prevWidth + detectorResult.InterestRegion.Width;

                int srcImageWidth = (int)Math.Min(inspectResult.TargetImage.Width, pair.Value.Width);
                int srcImageHeight = imageHeight;

                Int32Rect rect = new Int32Rect(pair.Value.X, pair.Value.Y, (int)Math.Min(inspectResult.TargetImage.Width, pair.Value.Width), srcImageHeight);

                byte[] sourceArray = new byte[rect.Width * rect.Height];
                inspectResult.TargetImage.CopyPixels(rect, sourceArray, rect.Width, rect.X);
                int resizeWidth = (int)(prevWidth);

                for (int y = 0; y < imageHeight; y++)
                {
                    for (int x = 0; x < srcImageWidth; x++)
                    {
                        int ySrcIndex = y * srcImageWidth;
                        int yDestIndex = y * imageWidth;
                        destArray[yDestIndex + resizeWidth + x] = sourceArray[ySrcIndex + x];
                    }
                }

                detectorResult.DefectList.ForEach(defect => defect.Rectangle = new Rectangle(defect.Rectangle.X + prevWidth, defect.Rectangle.Y, defect.Rectangle.Width, defect.Rectangle.Height));
                combineResult.DefectList.AddRange(detectorResult.DefectList);

                combineResult.Average = combineResult.Average != 0 ? (combineResult.Average + detectorResult.Average) / 2.0f : detectorResult.Average;
                combineResult.EdgeValueList.AddRange(detectorResult.EdgeValueList.ToArray());
            }

            combineImage = BitmapSource.Create(
                   imageWidth, imageHeight, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, destArray, imageWidth);

            //if (combineResult.DefectList.Count > PinHoleSettings.Instance().MaxDefectNum)
            //    combineResult.DefectList = combineResult.DefectList.Take(PinHoleSettings.Instance().MaxDefectNum).ToList();
                
            combineImage.Freeze();
            combineResult.InterestRegion = new Rectangle(interestX, 0, interestEndX - interestX, imageHeight);

            return combineResult;
        }

        public void Reset(int milliSecond = 5000)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (running == true && stopwatch.ElapsedMilliseconds < milliSecond)
                Thread.Sleep(50);

            running = false;

            lock (resultList)
                resultList.Clear();
        }

        public void Load()
        {
            targetRegionDictionary.Clear();

            List<ImageDevice> imageDeviceList = new List<ImageDevice>();

            string path = Path.Combine(PathSettings.Instance().Config, "ResultCombiner.xml");
            bool ok = false;
            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(path);
                if (xmlDocument == null)
                    return;

                XmlElement paramElement = xmlDocument["CombinerParam"];
                if (paramElement == null)
                    return;

                ok = true;

                foreach (ImageDevice device in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                {
                    XmlElement deviceElement = paramElement[string.Format("Camera{0}", device.Index)];

                    if (deviceElement == null)
                        continue;

                    int x = Convert.ToInt32(XmlHelper.GetValue(deviceElement, "X", "0"));
                    int y = Convert.ToInt32(XmlHelper.GetValue(deviceElement, "Y", "0"));
                    int width = Convert.ToInt32(XmlHelper.GetValue(deviceElement, "Width", "0"));
                    int height = Convert.ToInt32(XmlHelper.GetValue(deviceElement, "Height", "0"));

                    targetRegionDictionary.Add(device, new Int32Rect(x, y, width, height));
                }
            }
            finally
            {
                if (ok == false)
                {
                    foreach (ImageDevice device in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                        targetRegionDictionary.Add(device, new Int32Rect(0, 0, 2048, 2048));

                    Save();
                }
            }
        }

        public void Save()
        {
            if (Directory.Exists(PathSettings.Instance().Config) == false)
                Directory.CreateDirectory(PathSettings.Instance().Config);

            string path = Path.Combine(PathSettings.Instance().Config, "ResultCombiner.xml");

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement paramElement = xmlDocument.CreateElement("CombinerParam");
            xmlDocument.AppendChild(paramElement);

            foreach (KeyValuePair<ImageDevice, Int32Rect> pair in targetRegionDictionary)
            {
                XmlElement deviceElement = xmlDocument.CreateElement("", string.Format("Camera{0}", pair.Key.Index), "");
                paramElement.AppendChild(deviceElement);

                XmlHelper.SetValue(deviceElement, "X", pair.Value.X.ToString());
                XmlHelper.SetValue(deviceElement, "Y", pair.Value.Y.ToString());
                XmlHelper.SetValue(deviceElement, "Width", pair.Value.Width.ToString());
                XmlHelper.SetValue(deviceElement, "Height", pair.Value.Height.ToString());
            }

            xmlDocument.Save(path);
        }

        internal void AddInspectedIListner(IInspectedListner listner)
        {
            lock (inspectedListnerList)
                inspectedListnerList.Add(listner);
        }
        
        public void Inspected(InspectResult inspectResult)
        {
            if (inspectResult.DetectorResult is Color.Inspect.ColorDetectorResult)
                return;

            running = true;

            InspectResult combineResult = Combine(inspectResult);
            if (combineResult == null)
            {
                running = false;
                return;
            }

            if (SystemState.Instance().OpState != OpState.Teach)
            {
                if (combineResult.Judgment != DynMvp.InspData.Judgment.Accept)
                {
                    SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPinHole), true);
                    Thread.Sleep(PinHoleSettings.Instance().SignalTime);
                    SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPinHole), false);

                    ResultExportManager.Instance().ExportResult(combineResult);
                }
            }

            inspectedListnerList.ForEach(listner => listner.Inspected(combineResult));

            running = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
