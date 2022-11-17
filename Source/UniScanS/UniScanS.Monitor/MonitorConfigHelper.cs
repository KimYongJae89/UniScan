using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Serial;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.Monitor.UI;
using UniScanS.Common.Settings.UI;
using UniScanS.Common.UI;
using UniScanS.Monitor.Exchange;
using UniScanS.Monitor.UI;
using UniScanS.Data;

namespace UniScanS.Monitor
{
    public class MonitorConfigHelper : ConfigHelper
    {
        static MonitorConfigHelper _instance;
        public static MonitorConfigHelper Instance()
        {
            if (_instance == null)
                _instance = new MonitorConfigHelper();

            return _instance;
        }
        
        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new MonitorSystemSettingPanel());
        }

        public override Form GetMainForm()
        {
            IUiControlPanel uiControlPanel = SystemManager.Instance().UiChanger.CreateUiControlPanel();
            SystemManager.Instance().UiController.SetUiControlPanel(uiControlPanel);
            return new UniScanS.UI.MonitorMainform(uiControlPanel);
        }

        public override void BuildSystemManager()
        {
            SystemManager systemManager = CreateScreenSystemManager();
            SystemManager.SetInstance(systemManager);

            ProductionManagerS productionManager = new ProductionManagerS(PathSettings.Instance().Result);
            productionManager.Load(PathSettings.Instance().Result);

            systemManager.ProductionManager = productionManager;
            systemManager.ExchangeOperator = new MonitorOperator();
            systemManager.UiController = new MonitorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);
        }

        private SystemManager CreateScreenSystemManager()
        {
            SystemManager systemManager = new UniScanS.Screen.MonitorSystemManagerS();

            systemManager.Init(new UniScanS.Data.Model.ModelManager(), new UniScanS.Screen.UI.MonitorUiChangerS(), new UniScanS.Screen.AlgorithmArchiver(),
                new UniScanS.Screen.Device.DeviceBox(new UniScanS.Screen.Device.PortMap()), new UniEye.Base.Device.DeviceController(), null, null);
            
            return systemManager;
        }
    }
}
