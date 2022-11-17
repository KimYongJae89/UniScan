using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniScanWPF.Table;
using UniScanWPF.Table.Operation;

namespace UniScanWPF.Table.Settings
{
    class ConfigHelper : UniScanWPF.Settings.ConfigHelper
    {
        protected override void BuildSystemManager()
        {
            SystemManager systemManager = new SystemManager();

            systemManager.Init(
                new Table.Data.ModelManager(),
                null, 
                new DynMvp.Vision.AlgorithmArchiver(),
                new UniEye.Base.Device.DeviceBox(new Device.PortMap()), 
                new UniEye.Base.Device.DeviceController(),
                new UniScanWPF.Table.Data.ProductionManager(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());

            SystemManager.SetInstance(systemManager);
        }

        public override bool SplashSetupAction(IReportProgress reportProgress)
        {
            if (base.SplashSetupAction(reportProgress) == false)
                return false;

            BufferManager.Instance().Init();

            return true;
        }

        protected override ICustomConfigPage CreateCustomConfigPage()
        {
            return null;
        }
    }
}
