// custom
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanG.Common;
using UniScanG.Common.Settings;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.MachineIF;
using UniScanG.UI;
using UniScanG.UI.Teach;

namespace UniScanG
{
    public abstract class SystemManager : UniScanG.Common.SystemManager
    {
        HardwareMonitor hardwareMonitor;

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new UniScanG.Device.DeviceController DeviceController => (UniScanG.Device.DeviceController)deviceController;
        public UniScanG.Gravure.Device.DeviceControllerG DeviceControllerG => (UniScanG.Gravure.Device.DeviceControllerG)deviceController;
        
        public new UiChanger UiChanger
        {
            get { return (UiChanger)uiChanger; }
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
            set { currentModel = value; }
        }

        public UniScanG.Inspect.InspectRunner InspectRunnerG
        {
            get { return (UniScanG.Inspect.InspectRunner)inspectRunner; }
            set { inspectRunner = value; }
        }

        public new ProductionManager ProductionManager
        {
            get { return (ProductionManager)this.productionManager; }
        }

        public override UniScanG.Common.Data.ModelManager CreateModelManager()
        {
            return new ModelManager();
        }

        public override string[] GetSystemTypeNames()
        {
            return base.GetSystemTypeNames();
            return new string[]
            {
                "Gravure",
                "TesT"
            };
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();

            try
            {
                this.hardwareMonitor = new HardwareMonitor(PathSettings.Instance().HWMonitor, true);
                this.hardwareMonitor.Start(5000);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("SystemManager::InitializeAdditionalUnits - {0}", ex.ToString()));
            }
        }

        public override void Release()
        {
            base.Release();
            this.hardwareMonitor?.Stop();
        }
        public static bool IsUserAdministrator()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();

            this.inspectRunner.ExitWaitInspection();
            IVncContainer[] vncContainers= Array.ConvertAll(Array.FindAll(this.UiChanger.TabPages, f => f is IVncContainer), f => (IVncContainer)f);
            Array.ForEach(vncContainers, f => f.ExitAllVncProcess(true));
        }
    }
}
