using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniScanG.MachineIF;
using UniScanG.Module.Monitor.MachineIF;

namespace UniScanG.Module.Monitor.Config
{
    class ConfigHelper : UniScanWPF.Settings.ConfigHelper
    {
        protected override void BuildSystemManager()
        {
            Settings.AdditionalSettings.CreateInstance();

            string portMapXmlFilePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);

            MachineIfProtocolList.Set(new UniScanG.Module.Monitor.MachineIF.MelsecProtocolList());
            SystemManager systemManager = new SystemManager();
            systemManager.Init(
                new Data.ModelManager(),
                null,
                new Data.AlgorithmArchiver(),
                new Device.DeviceBox(new PortMap(portMapXmlFilePath)),
                new Device.DeviceController(),
                new Data.ProductionManager(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager()
                );

            SystemManager.SetInstance(systemManager);
        }

        public override bool SplashSetupAction(IReportProgress reportProgress)
        {
            if (base.SplashSetupAction(reportProgress) == false)
                return false;

            return true;
        }

        protected override ICustomConfigPage CreateCustomConfigPage()
        {
            return new CustomConfigPage();
        }
    }
}
