using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.MachineIF;

namespace UniScanM.Operation
{
    public abstract class InspectRunnerExtender: UniEye.Base.Inspect.InspectRunnerExtender
    {
        public InspectRunnerExtender() : base()
        {
           
        }

        public override InspectionResult BuildInspectionResult(string extendInfo = "")
        {
            LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");

            Data.Production production = SystemManager.Instance().ProductionManager.CurProduction;
            Data.InspectionResult inspectionResult = (Data.InspectionResult)CreateInspectionResult();

            inspectionResult.ModelName = production.Name;
            inspectionResult.LotNo = production.LotNo;
            inspectionResult.Wroker = production.Worker;
            inspectionResult.InspectionTime = new TimeSpan(0);
            inspectionResult.ExportTime = new TimeSpan(0);
            inspectionResult.InspectionStartTime = DateTime.Now;
            inspectionResult.InspectionEndTime = DateTime.Now;

            inspectionResult.InspectionNo = GetInspectionNo();
            //inspectionResult.InspectionNo = "0";

            if (true)
            {
                // 롤의 현재 위치
                inspectionResult.RollDistance = SystemManager.Instance().InspectStarter.GetPosition();
            }
            else
            {
                // 롤의 현재 위치 (추정)
                TimeSpan time = inspectionResult.InspectionStartTime - SystemManager.Instance().ProductionManager.CurProduction.StartTime;
                double lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
                inspectionResult.RollDistance = (int)(SystemManager.Instance().ProductionManager.CurProduction.StartPosition + (time.TotalSeconds * lineSpeed / 60));
            }

            string autoManual = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto ? "Auto" : "Manual";
            string productionName = SystemManager.Instance().ProductionManager.CurProduction.Name;
            
            //todo production name....
            inspectionResult.ResultPath = Path.Combine(
                PathSettings.Instance().Result,
                SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyyMMdd"),
                SystemManager.Instance().ProductionManager.CurProduction.Name,
                autoManual,
                SystemManager.Instance().ProductionManager.CurProduction.LotNo);

            Directory.CreateDirectory(inspectionResult.ResultPath);

            inspectionResult.ReportPath = inspectionResult.ResultPath.Replace(@"Result\", @"Report\");
            Directory.CreateDirectory(inspectionResult.ReportPath);

            LogHelper.Debug(LoggerType.Inspection, String.Format("Create Inspection Result: {0}", inspectionResult.InspectionNo));

            return inspectionResult;
        }

        //protected override string GetInspectionNo()
        //{
        //    ProductionBase productionBase = SystemManager.Instance().ProductionManager.CurProduction;
        //    if (productionBase == null)
        //        return base.GetInspectionNo();

        //    string inspectionNo = productionBase.Total.ToString();
        //    productionBase.AddTotal();

        //    return inspectionNo;
        //}

        public ImageD GetBinningImage(ImageD srcImage)
        {
            Image2D binningImage = new Image2D(srcImage.Width, srcImage.Height * 2, srcImage.NumBand);
            Image2D srcImage2D = srcImage as Image2D;
            if (srcImage2D.IsUseIntPtr())
                srcImage2D.ConvertFromPtr();

            int width = srcImage2D.Width;
            int height = srcImage2D.Height;

            int size = width * height * srcImage.NumBand;

            byte[] imageBuf = srcImage2D.Data;
            byte[] binningBuf = binningImage.Data;

            Parallel.For(0, height, y =>
            {
                Array.Copy(imageBuf, srcImage2D.Pitch * y, binningBuf, width * y * 2, width);
                Array.Copy(imageBuf, srcImage2D.Pitch * y, binningBuf, width * y * 2 + width, width);
            });

            return binningImage;
        }
    }
}
