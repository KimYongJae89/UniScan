using DynMvp.Base;
using DynMvp.Devices.MotionController;
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
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Device;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.UI;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table
{
    public interface IModelListner
    {
        void ModelChanged(Model curModel);
    }

    public interface IOperatorListner
    {
        void Processed(OperatorResult operatorResult);
        void Completed(OperatorResult operatorResult);
    }

    public class SystemManager : UniEye.Base.SystemManager
    {
        MachineObserver machineObserver;
        OperatorManager operatorManager;

        public MachineObserver MachineObserver { get => machineObserver; }
        public OperatorManager OperatorManager { get => operatorManager; }
        
        List<IOperatorListner> operatorListnerList = new List<IOperatorListner>();
        List<IModelListner> modelListnerList = new List<IModelListner>();

        MainWindow mainWindow;
        public MainWindow MainPage { get => mainWindow; set => mainWindow = value; }

        public bool OnMachinRun
        {
            get
            {
                return operatorManager.IsRun && machineObserver.MotionBox.OnMove;
            }
        }

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }
        
        public new Data.ModelManager ModelManager
        {
            get { return (Data.ModelManager)modelManager; }
        }

        public new ProductionManager ProductionManager
        {
            get { return (ProductionManager)productionManager; }
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
            set
            {
                if (currentModel != value)
                {
                    currentModel = value;
                    productionManager.LotChange(null, null);

                    InfoBox.Instance.LastStartTime = DateTime.Now;

                    if (currentModel != null)
                        LogHelper.Info(LoggerType.Operation, string.Format("Model Changed - {0}", currentModel.Name));

                    modelListnerList.ForEach(listner => listner.ModelChanged(value));
                    InfoBox.Instance.ModelChanged();
                }
            }
        }
        
        public void Close()
        {
            operatorManager.Cancle(null);
            operatorManager.ResultCombiner.Clear();

            BufferManager.Instance().Dispose();
        }

        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new UniEye.Base.Inspect.DirectTriggerInspectRunner();
        }

        public override void InitializeDataExporter()
        {

        }

        protected override void LoadAdditialSettings()
        {
            
        }

        public void InitTableUnit()
        {
            machineObserver = new MachineObserver();
            operatorManager = new OperatorManager();
        }

        public void AddOperatorListnerList(IOperatorListner listner)
        {
            lock (operatorListnerList)
                operatorListnerList.Add(listner);
        }
        
        public void AddModelListnerList(IModelListner listner)
        {
            lock (modelListnerList)
                modelListnerList.Add(listner);
        }

        public void OperatorProcessed(OperatorResult result)
        {
            Debug.WriteLine(string.Format("SystemManager::OperatorProcessed({0})", result.Type));
            operatorListnerList.ForEach(listner => listner.Processed(result));
        }

        public void OperatorCompleted(OperatorResult result)
        {
            Debug.WriteLine(string.Format("SystemManager::OperatorCompleted({0})", result?.Type));
            operatorListnerList.ForEach(listner => listner.Completed(result));
        }
    }
}