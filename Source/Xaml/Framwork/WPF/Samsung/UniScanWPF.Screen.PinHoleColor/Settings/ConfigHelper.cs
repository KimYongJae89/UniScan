using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Data;

namespace UniScanWPF.Screen.PinHoleColor.Settings
{
    class ConfigHelper : UniScanWPF.Settings.ConfigHelper
    {
        protected override void BuildSystemManager()
        {
            SystemManager systemManager = new SystemManager();

            systemManager.Init(
                new ModelManager(),
                null, 
                new DynMvp.Vision.AlgorithmArchiver(), 
                new UniEye.Base.Device.DeviceBox(new Device.PortMap()), 
                new UniEye.Base.Device.DeviceController(),
                new MultipleProductionManager(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());

            systemManager.WpfUiChanger = new UI.WPFUiChanger();
            systemManager.OnSetupDone = OnSetupDone;
            SystemManager.SetInstance(systemManager);
        }

        protected bool OnSetupDone()
        {
            SystemManager.Instance().ModelManager.InitPreset();
            return true;
        }
    }
}
