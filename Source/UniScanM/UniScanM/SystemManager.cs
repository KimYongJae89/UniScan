using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.Data;
using UniScanM.Operation;
using UniScanM.UI;

namespace UniScanM
{
    public interface IModelChangedListner
    {
        void OnModelChanged(Model model);
    }

    public abstract class SystemManager : UniEye.Base.SystemManager
    {
        List<IModelChangedListner> modelChangedListnerList = new List<IModelChangedListner>();

        protected InspectParamManager inspectParamManager;
        public InspectParamManager InspectParamManager { get => inspectParamManager; set => inspectParamManager = value; }

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
        }

        public new UiChanger UiChanger
        {
            get { return (UiChanger)uiChanger; }
        }

        public new ProductionManager ProductionManager
        {
            get { return (ProductionManager)productionManager; }
        }

        public void AddModelListner(IModelChangedListner modelChangedListner)
        {
            modelChangedListnerList.Add(modelChangedListner);
        }
        
        protected InspectStarter inspectStarter;
        public InspectStarter InspectStarter { get => inspectStarter; set => inspectStarter = value; }

        public abstract void CreateInspectStarter();

        public virtual void LoadDefaultModel()
        {
            if (ModelManager == null)
                return;

            ModelDescription md = ModelManager.GetModelDescription("DefaultModel", "Unknown");
            if (md == null)
            {
                md = (ModelDescription)modelManager.CreateModelDescription();
                md.Name = "DefaultModel";
                md.Paste = "Unknown";
                modelManager.AddModel(md);
                Model model = (Model)modelManager.LoadModel(md, null);
                modelManager.SaveModel(model);
            }

            //모델이 널일때가 있네.
            if (this.currentModel?.ModelDescription == md)
                return;

            SystemManager.Instance().LoadModel(md);
        }
        
        public override void InitializeResultManager()
        {
            int resultStoringDays = OperationSettings.Instance().ResultStoringDays;
            int minimumFreeSpace = OperationSettings.Instance().MinimumFreeSpace;

            dataManagerList.Add(new UniScanM.Data.DataRemover(resultStoringDays, minimumFreeSpace, SystemManager.Instance().ProductionManager, new System.IO.DirectoryInfo(PathSettings.Instance().Log)));
            //dataManagerList.Add(new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Seq, resultPath, resultStoringDays, "yyyyMMdd", true));

            //string reportPath = PathSettings.Instance().Result.Replace(@"Result\", @"Report\");
            //dataManagerList.Add(new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Seq, reportPath, resultStoringDays, "yyyyMMdd", true));
        }
    }
}
