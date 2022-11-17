using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public class InspectResult : TargetResult
    {
        string resultPath;

        Data.DetectorType type;

        ImageDevice targetDevice;
        DetectorResult detectorResult;

        int index;

        DateTime startTime;
        DateTime endTime;

        BitmapSource targetImage;

        public DetectorResult DetectorResult { get => detectorResult; set => detectorResult = value; }
        public ImageDevice TargetDevice { get => targetDevice; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public int Index { get => index; set => index = value; }
        
        public string ResultPath { get => resultPath; set => resultPath = value; }
        public string IndexStr { get => string.Format("{0:00000}", index); }
        
        public Judgment Judgment { get => detectorResult.Judgment; }
        public BitmapSource TargetImage
        {
            get
            {
                if (targetImage == null && resultPath != null)
                {
                    string imageFilePath = String.Format("{0}\\Target.jpg", resultPath);
                    if (File.Exists(imageFilePath))
                        targetImage = WPFImageHelper.LoadBitmapSource(imageFilePath);
                }

                return targetImage;
            }

            set => targetImage = value;
        }

        
        public object Brush
        {
            get
            {
                if (detectorResult == null)
                    return Brushes.White;

                switch (Judgment)
                {
                    case Judgment.Accept:
                        return App.Current.Resources["LightGreenBrush"];
                    case Judgment.Reject:
                    case Judgment.FalseReject:
                        return App.Current.Resources["LightRedBrush"];
                    case Judgment.Skip:
                        return App.Current.Resources["LightYellowBrush"];
                }
                
                return Brushes.Black;
            }
        }

        private static Dictionary<ImageDevice, int> inspectSectionDictionary;
        public static void Reset()
        {
            inspectSectionDictionary = new Dictionary<ImageDevice, int>();
            
            foreach (ImageDevice device in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                inspectSectionDictionary.Add(device, 0);
        }
        
        protected override BitmapSource GetBitmapImage()
        {
            return TargetImage;
        }

        public override List<UIElement> GetFigureList()
        {
            if (detectorResult == null)
                return null;

            Rectangle rect = new Rectangle();
            switch (detectorResult.Judgment)
            {
                case Judgment.Accept:
                    rect.Stroke = new SolidColorBrush(Colors.Green);
                    break;
                case Judgment.Reject:
                case Judgment.FalseReject:
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    break;
                case Judgment.Skip:
                    rect.Stroke = new SolidColorBrush(Colors.Yellow);
                    break;
            }

            List<UIElement> detectorFigures = detectorResult.GetFigures();
            if (targetImage != null && detectorFigures != null)
            {
                rect.StrokeThickness = TargetImage.Width / 100;
                rect.Width = TargetImage.Width;
                rect.Height = TargetImage.Height;
                detectorFigures.Add(rect);
            }

            return detectorFigures;
        }

        internal InspectResult(DetectorType type)
        {
            startTime = DateTime.Now;
            this.type = type;
        }

        internal InspectResult(ImageDevice targetDevice)
        {
            if (inspectSectionDictionary.ContainsKey(targetDevice) == false)
            {
                LogHelper.Debug(LoggerType.Operation, "InspectResult - Dictionary device null");
                return;
            }
            
            this.targetDevice = targetDevice;
            index = inspectSectionDictionary[targetDevice]++;
            startTime = DateTime.Now;
        }

        internal void ExportResult()
        {
            if (Directory.Exists(resultPath) == false)
                Directory.CreateDirectory(resultPath);

            string resultFilePath = String.Format("{0}\\Result.xml", resultPath);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement resultElement = xmlDocument.CreateElement("", "Result", "");
            xmlDocument.AppendChild(resultElement);
            
            XmlHelper.SetValue(resultElement, "Index", index.ToString());
            XmlHelper.SetValue(resultElement, "StartTime", startTime.ToString());
            XmlHelper.SetValue(resultElement, "EndTime", endTime.ToString());

            XmlElement detectorElement = xmlDocument.CreateElement("", "Detector", "");
            resultElement.AppendChild(detectorElement);
            detectorResult.ExportResult(resultPath, detectorElement);

            XmlHelper.Save(xmlDocument, resultFilePath);
            if (targetImage != null)
            {
                string imageFilePath = String.Format("{0}\\Target.jpg", resultPath);
                WPFImageHelper.SaveBitmapSource(imageFilePath, targetImage);
            }
        }

        internal void ImportResult()
        {
            if (Directory.Exists(resultPath) == false)
                return;

            string resultFilePath = String.Format("{0}\\Result.xml", resultPath);
            if (File.Exists(resultFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(resultFilePath);
                XmlElement resultElement = xmlDocument["Result"];

                if (resultElement != null)
                {
                    index = Convert.ToInt32(XmlHelper.GetValue(resultElement, "Index", "0"));
                    startTime = Convert.ToDateTime(XmlHelper.GetValue(resultElement, "StartTime", DateTime.Now.ToString()));
                    endTime = Convert.ToDateTime(XmlHelper.GetValue(resultElement, "EndTime", DateTime.Now.ToString()));

                    switch (type)
                    {
                        case DetectorType.PinHole:
                            detectorResult = new PinHoleDetectorResult();
                            break;
                        case DetectorType.Color:
                            detectorResult = new ColorDetectorResult();
                            break;
                    }

                    XmlElement detectorElement = resultElement["Detector"];
                    if (detectorElement != null)
                        detectorResult.ImportResult(resultPath, detectorElement);
                }
            }
        }
    }
}
