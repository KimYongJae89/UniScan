using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.MachineIF;

namespace UniScanG.Inspect
{
    public abstract class InspectRunner : UnitBaseInspectRunner
    {
        public ProcessBufferManager ProcessBufferManager { get => processBufferManager; }
        protected ProcessBufferManager processBufferManager = null;

        public const int MAX_TRANSFER_BUFFER_SIZE = 100;

        public List<Task> TransferTaskList { get => this.trasnferTaskList; }
        protected List<Task> trasnferTaskList = new List<Task>();

        public InspectStarter InspectStarter { get => this.inspectStarter; }
        protected InspectStarter inspectStarter;

        public GrabProcesserG GrabProcesser { get => grabProcesser; }
        protected GrabProcesserG grabProcesser = null;

        public IInspectObserver InspectObserver { get => inspectObserver; }
        protected IInspectObserver inspectObserver = null;

        public AlarmManager AlarmManager => this.alarmManager;
        protected AlarmManager alarmManager = null;

        public InspectRunner() : base()
        {
            this.processBufferManager = new ProcessBufferManager();

            this.inspectStarter = new InspectStarter();
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf != null)
                this.inspectStarter.Start();
        }

        public override void Dispose()
        {
            this.inspectStarter.Stop();
            base.Dispose();
        }

        public abstract GrabProcesserG BuildSheetGrabProcesser(bool useLengthFilter);

        protected override void SetupUnitManager() { }

        protected override InspectionResult BuildInspectionResult(string inspectionNo = null)
        {
            //UniScanG.Data.Inspect.InspectionResult inspectionResult = new UniScanG.Data.Inspect.InspectionResult();
            InspectionResult inspectionResult = base.BuildInspectionResult(inspectionNo);

            Production productionG = (Production)SystemManager.Instance().ProductionManager.CurProduction;
            lock (productionG)
            {
                inspectionResult.InspectionNo = string.IsNullOrEmpty(inspectionNo) ? productionG.Total.ToString() : inspectionNo;
                inspectionResult.ResultPath = Path.Combine(productionG.ResultPath, inspectionResult.InspectionNo);
                productionG.AddTotal();
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunner::BuildInspectionResult - InspectionNo {0}", inspectionResult.InspectionNo));
            return inspectionResult;
        }

        protected string GetResultPath(ProductionG productionG, string inspectionNo)
        {
            return Path.Combine(
                PathSettings.Instance().Result,
                productionG.StartTime.ToString("yy-MM-dd"),
                productionG.Name,
                productionG.Thickness.ToString(),
                productionG.Paste,
                productionG.LotNo,
                string.IsNullOrEmpty(inspectionNo) ? productionG.Total.ToString() : inspectionNo);
        }

        //public override void InspectDone(UnitInspectItem unitInspectItem)
        //{
        //    if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName) == false)
        //        return;

        //    Production production = (Production)SystemManager.Instance().ProductionManager.CurProduction;
        //    production.Update(((SheetResult)unitInspectItem.InspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName]));

        //    base.InspectDone(unitInspectItem);
        //}

        public int GetTransferTaskLisCount()
        {
            lock (this.trasnferTaskList)
                return this.trasnferTaskList.Count;
        }

        public virtual bool IsRunable()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::IsRunable");

            bool isCleared = ErrorManager.Instance().IsCleared();
            if (!isCleared)
                throw new Exception("Alram is not cleared.");

            Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                throw new Exception("Model is not selected.");

            if(!curModel.IsTrained)
                throw new Exception("Model is not teached.");

            return true;
        }

        public override void InspectDone(InspectionResult inspectionResult)
        {
            this.alarmManager?.AddResult(inspectionResult);
            base.InspectDone(inspectionResult);
        }
    }
}
