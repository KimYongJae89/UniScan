using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanS.Data;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision.Detector;

namespace UniScanS.Inspect
{
    public abstract class InspectRunner : UnitBaseInspectRunner
    {
        protected ProcessBufferManager processBufferManager = null;
        protected GrabProcesser grabProcesser = null;

        public InspectRunner() : base()
        {
            this.processBufferManager = new ProcessBufferManager();
        }
        protected override void SetupUnitManager() { }
        

        protected override InspectionResult BuildInspectionResult(string inspectionNo = null)
        {
            InspectionResult inspectionResult = new InspectionResult();

            inspectionResult.ModelName = SystemManager.Instance().CurrentModel.Name;

            inspectionResult.InspectionTime = new TimeSpan(0);
            inspectionResult.ExportTime = new TimeSpan(0);
            inspectionResult.InspectionStartTime = DateTime.Now;
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.JobOperator = UserHandler.Instance().CurrentUser.Id;
            inspectionResult.GrabImageList = new List<ImageD>();

            ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
            lock (SystemManager.Instance().ProductionManager.CurProduction)
                productionG.SheetIndex++;

            inspectionResult.InspectionNo = productionG.SheetIndex.ToString();
            inspectionResult.ResultPath = GetResultPath(productionG);

            return inspectionResult;
        }
        protected string GetResultPath(ProductionS productionG)
        {
            return Path.Combine(
                PathSettings.Instance().Result,
                productionG.StartTime.ToString("yy-MM-dd"),
                productionG.Name,
                productionG.Thickness,
                productionG.Paste,
                productionG.LotNo,
                productionG.SheetIndex.ToString());
        }

        public override void InspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetInspector.TypeName) == false)
                return;

            ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG != null)
            {
                productionG.Update((SheetResult)inspectionResult.AlgorithmResultLDic[SheetInspector.TypeName]);
                SaveCurrentProduction(SystemManager.Instance().ProductionManager.CurProduction);
            }

            base.InspectDone(inspectionResult);
        }

        public void SaveCurrentProduction(ProductionBase production)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement productionElement = xmlDocument.CreateElement("", "Production", "");
            production.Save(productionElement);
            xmlDocument.AppendChild(productionElement);

            try
            {
                string fileName = Path.Combine(PathSettings.Instance().Result, "CurProduction.bak");
                string readFileName = Path.Combine(PathSettings.Instance().Result, "CurProduction.csv");

                xmlDocument.Save(fileName);
                File.Move(fileName, readFileName);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, e.Message);
            }
        }

        public void LoadCurrentProduction(ProductionBase production, string filePath)
        {
            if (filePath == null)
                return;

            if (production == null)
                production = (ProductionS)SystemManager.Instance().ProductionManager.CreateProduction(null, "");

            try
            {
                string fileName = Path.Combine(filePath, "CurProduction.csv");

                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement xmlElement = xmlDocument.DocumentElement;
                production.Load(xmlElement);

                File.Delete(fileName);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, e.Message);
            }
        }
    }
}
