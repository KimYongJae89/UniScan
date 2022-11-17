using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Calculator.V2;

namespace UniScanG.Gravure.Vision.Extender
{
    public enum WatcherLockFile { DateTime, Lot, PelWidth, PelHeight }
    public class Watcher : AlgorithmG
    {
        public static string TypeName { get { return "Watcher"; } }

        public static WatcherParam WatcherParam => AlgorithmSetting.Instance().WatcherParam;

        int period = 0;
        int iterate = 0;

        public static string LockFileName => "Watcher.lock";

        public static string MonitoringPath => Path.Combine(PathSettings.Instance().Result, "Monitoring");

        public static string MonitoringLockFile => Path.Combine(MonitoringPath, LockFileName);

        public Watcher()
        {
            this.AlgorithmName = TypeName;
            this.param = null;
        }

        #region Abstract
        public override AlgorithmParam CreateParam()
        {
            return new WatcherParam(true);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            Watcher clone = new Watcher();
            clone.CopyFrom(this);

            return clone;
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }
        #endregion

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new WatcherResult();
        }

        public override void PrepareInspection()
        {
            base.PrepareInspection();

            int clientIndex = SystemManager.Instance().ExchangeOperator.GetClientIndex();

            if (WatcherParam.MonitoringPeriod % 2 == 0)
                // 주기가 짝수: Master에서만 검사함. Slave는 검사 안 함.
            {
                this.period =
                    (clientIndex < 0 ? WatcherParam.MonitoringPeriod :
                    (clientIndex == 0 ? WatcherParam.MonitoringPeriod / 2 :
                    0));
                this.iterate = 0;
            }
            else
            // 주기가 홀수: Master와 Slave에서 번갈아가며 검사함. Salve 최초 시작은 주기/2 에서 시작.
            {
                this.period = WatcherParam.MonitoringPeriod;
                this.iterate = (clientIndex <= 0 ? 0 : WatcherParam.MonitoringPeriod / 2 + 1) % this.period;
            }

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            Array.ForEach(model.WatcherModelParam.Collections, f => f.PrepareInspection());
            //model.WatcherModelParam.MarginCollection.PrepareInspection();

            if (File.Exists(MonitoringLockFile))
                File.Delete(MonitoringLockFile);
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            DebugContextG debugContextG = algorithmInspectParam.DebugContext as DebugContextG;

            SheetInspectParam inspectParam = algorithmInspectParam as SheetInspectParam;
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            ProcessBufferSetG processBufferSetG = inspectParam.ProcessBufferSet as ProcessBufferSetG;
            CancellationToken cancellationToken = inspectParam.CancellationToken;
            bool isTestMode = inspectParam.TestInspect;

            AlgoImage algoImage = processBufferSetG == null ? inspectParam.AlgoImage : processBufferSetG.AlgoImage;
            Point patternOffset = processBufferSetG == null ? Point.Empty : processBufferSetG.OffsetStructSet.PatternOffset.Offset;
            Calibration calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            SizeF pelSize = calibration == null ? new SizeF(14, 14) : calibration.PelSize;

            List<FoundedObjInPattern> foundedObjInPatternList = new List<FoundedObjInPattern>();

            WatcherResult watcherResult = (WatcherResult)CreateAlgorithmResult();

            if (Directory.Exists(MonitoringPath) == false)
                Directory.CreateDirectory(MonitoringPath);

            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance() as AlgorithmSetting;
            if (this.period > 0)
            {
                if (isTestMode || iterate == 0)
                {
                    // Observe
                    if (!File.Exists(MonitoringLockFile))
                    {
                        debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Extend_StopImg, 0);
                        model.WatcherModelParam.MonitorChipCollection.Inspect(inspectParam);
                        model.WatcherModelParam.MonitorFPCollection.Inspect(inspectParam);
                        model.WatcherModelParam.MonitorIndexCollection.Inspect(inspectParam);
                        CreateLockFile();
                    }

                    // Save Chip Image
                    debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Extend_StopImg, 0);
                    foundedObjInPatternList.AddRange(model.WatcherModelParam.StopImgCollection.Inspect(inspectParam));

                    // Measure Margin
                    debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Extend_Margin, 0);
                    foundedObjInPatternList.AddRange(model.WatcherModelParam.MarginCollection.Inspect(inspectParam));

                    // Measure Transform
                    debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Extend_Transform, 0);
                    foundedObjInPatternList.AddRange(model.WatcherModelParam.TransformCollection.Inspect(inspectParam));
                }
                this.iterate = (this.iterate + 1) % this.period;
            }

            watcherResult.SheetSubResultList.AddRange(foundedObjInPatternList);
            watcherResult.UpdateSpandTime();
            watcherResult.OffsetFound = processBufferSetG == null ? SizeF.Empty : new SizeF(processBufferSetG.OffsetStructSet.PatternOffset.Offset);
            watcherResult.Good = true;
            return watcherResult;
        }


        //private FiducialInPattern[] MeasureTransform(SheetInspectParam inspectParam)
        //{
        //    WatcherParam watcherParam = this.param as WatcherParam;
        //    Calibration calibration = inspectParam.CameraCalibration;
        //    ProcessBufferSetG2 processBufferSetG = inspectParam.ProcessBufferSet as ProcessBufferSetG2;
        //    DebugContextG debugContextG = inspectParam.DebugContext as DebugContextG;

        //    Stopwatch sw = Stopwatch.StartNew();
        //    Point patternOffset = processBufferSetG.OffsetStructSet.PatternOffset.Offset;
        //    AlgoImage algoImage = processBufferSetG.AlgoImage;
        //    FiducialInPattern[] watcherResultElements = processBufferSetG.MeasureShrinkAndExtend.Measure(algoImage, patternOffset, watcherParam.MeasureTransformScore, calibration, debugContextG);
        //    sw.Stop();
        //    debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Monitoring, sw.ElapsedMilliseconds);

        //    return watcherResultElements;
        //}

        private void SaveImage(AlgoImage algoImage, ExtItem watchItem, Point offset)
        {
            if (watchItem == null)
                return;

            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);

            string imageFileName = watchItem.GetFileName();
            string lockFileName = Path.GetFileNameWithoutExtension(imageFileName);
            string lockFilePath = Path.Combine(MonitoringPath, lockFileName);

            if (!File.Exists(lockFilePath))
            {
                RectangleF adjustRectR = watchItem.ClipRectangleUm;
                Rectangle adjustRect = Rectangle.Round(DrawingHelper.Mul(adjustRectR, imageRect.Size));
                adjustRect.Offset(offset);

                if (Rectangle.Intersect(imageRect, adjustRect) == adjustRect)
                {
                    AlgoImage subImage = algoImage.GetSubImage(adjustRect);
                    subImage.Save(Path.Combine(MonitoringPath, imageFileName));
                    subImage.Dispose();

                    // Create Lock File
                    CreateLockFile(lockFilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
        }

        private void CreateLockFile()
        {
            FileStream fileStream = new FileStream(MonitoringLockFile, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.DateTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.Lot.ToString(), SystemManager.Instance().ProductionManager.CurProduction?.LotNo));

            SizeF pelSize = new SizeF(14, 14);
            Calibration calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                pelSize = calibration.PelSize;

            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.PelWidth.ToString(), pelSize.Width));
            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.PelHeight.ToString(), pelSize.Height));

            streamWriter.Close();
            fileStream.Close();
        }

        private void CreateLockFile(string filePath, string contents)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.WriteLine(contents);

            streamWriter.Close();
            fileStream.Close();
        }
    }
}
