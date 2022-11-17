using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
//using UniEye.Base.Settings;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Extender
{
    public abstract class ClipAndSave : ExtItem
    {
        public ClipAndSave(ExtType extType, DynMvp.Base.LicenseManager.ELicenses licenseKey) : this(extType, licenseKey.ToString()) { }
        public ClipAndSave(ExtType extType, string licenseKey) : base(extType, licenseKey) { }

        public void SaveImage(AlgoImage algoImage, Point offset, Calibration calibration, string path)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);

            string imageFileName = GetFileName();
            string lockFileName = Path.GetFileNameWithoutExtension(imageFileName);

            string imageFIle = Path.Combine(path, imageFileName);
            string lockFIle = Path.Combine(path, lockFileName);

            if (!File.Exists(lockFIle))
            {
                RectangleF adjustRectUm = this.ClipRectangleUm;
                Rectangle adjustRect = Rectangle.Round(calibration.WorldToPixel(adjustRectUm));
                adjustRect.Offset(offset);

                if (Rectangle.Intersect(imageRect, adjustRect) == adjustRect)
                {
                    AlgoImage subImage = algoImage.GetSubImage(adjustRect);
                    subImage.Save(imageFIle);
                    subImage.Dispose();

                    // Create Lock File
                    CreateLockFile(lockFIle);
                }
                else
                {
                    LogHelper.Error(LoggerType.Error, $"ClipAndSave::SaveImage - imageRect: {imageRect}, adjustRect: {adjustRect}");
                }
            }
        }


        private void CreateLockFile(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            string contents = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            streamWriter.WriteLine(contents);

            streamWriter.Close();
            fileStream.Close();
        }
    }

    public class ClipAndSaveParam : ExtParam
    {
        public override bool Use => true;

        public int Count { get => this.count; set => this.count = value; }
        private int count;

        public ClipAndSaveParam(bool available) : base(available)
        {
            this.count = 3;
        }

        public override ExtParam Clone()
        {
            return new ClipAndSaveParam(this.Available)
            {
                count = this.count
            };
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);
            this.count = XmlHelper.GetValue(xmlElement, "Count", this.count);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "Count", this.count);
        }
    }

    public abstract class ClipAndSaveCollection : ExtCollection
    {
        public static string MonitoringPath => Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Result, "Monitoring");

        public ClipAndSaveParam Param => (ClipAndSaveParam)this.param;

        int chipIndex = 0;
        bool isRolling = true;

        public ClipAndSaveCollection(ExtType extType, bool isRolling, DynMvp.Base.LicenseManager.ELicenses licenseKey) : this(extType, isRolling, licenseKey.ToString()) { }

        public ClipAndSaveCollection(ExtType extType, bool isRolling, string licenseKey) : base(extType)
        {
            this.isRolling = isRolling;
            this.param = new ClipAndSaveParam(DynMvp.Base.LicenseManager.Exist(licenseKey));
        }

        public override void PrepareInspection()
        {
            base.PrepareInspection();
            this.chipIndex = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(MonitoringPath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();

            this.items.ForEach(f =>
            {
                string imageFileName = f.GetFileName();
                string lockFileName = Path.GetFileNameWithoutExtension(imageFileName);
                FileInfo[] fileInfos = directoryInfo.GetFiles(lockFileName);
                Array.ForEach(fileInfos, g => g.Delete());
            });
            //FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
            //Array.ForEach(fileInfos, f => f.Delete());
        }

        public override FoundedObjInPattern[] Inspect(SheetInspectParam inspectParam)
        {
            if (!this.param.Active)
                return new FoundedObjInPattern[0];

            ProcessBufferSetG processBufferSetG = inspectParam.ProcessBufferSet as ProcessBufferSetG;

            AlgoImage algoImage = inspectParam.AlgoImage;
            Point patternOffset = Point.Empty;
            if (processBufferSetG != null)
            {
                patternOffset = processBufferSetG.OffsetStructSet.PatternOffset.Offset;
                algoImage = processBufferSetG.AlgoImage;
            }

            DebugContextG debugContextG = inspectParam.DebugContext as DebugContextG;
            int totalCount = this.Count;

            List<ClipAndSave> list = new List<ClipAndSave>();
            if (inspectParam.TestInspect || !this.isRolling)
                list.AddRange(this.items.ConvertAll<ClipAndSave>(f => (ClipAndSave)f));
            else
                list.AddRange(this.items.FindAll(f => f.Index == this.chipIndex).ConvertAll<ClipAndSave>(f => (ClipAndSave)f));

            Stopwatch sw = Stopwatch.StartNew();

            // Save Images
            list.ForEach(f =>
            {
                int containerIndex = f.ContainerIndex;
                PointF localOffsetF = PointF.Empty;
                if (processBufferSetG != null)
                    localOffsetF = processBufferSetG.OffsetStructSet.GetLocalOffset(containerIndex);
                Point localOffset = Point.Round(localOffsetF);
                LogHelper.Debug(LoggerType.Inspection, string.Format("ClipAndSaveCollection::Inspect - Type: {0}, ID: {1}, PatternOffset: {2}, LocalOffset: {3}",
                    f.ExtType, f.Index, patternOffset, localOffset));
                f.SaveImage(algoImage, DrawingHelper.Add(patternOffset, localOffset),inspectParam.CameraCalibration, MonitoringPath);
            });

            if (totalCount > 0)
                this.chipIndex = (this.chipIndex + 1) % totalCount;

            sw.Stop();
            debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Extend_StopImg, sw.ElapsedMilliseconds);

            return new FoundedObjInPattern[0];
        }
    }
}
