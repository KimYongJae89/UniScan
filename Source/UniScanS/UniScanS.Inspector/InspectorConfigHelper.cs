using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
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
using UniEye.Base.Settings.UI;
using UniScanS.Common;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.Inspector.UI;
using UniScanS.Common.Settings.UI;
using UniScanS.Inspector.UI;
using UniScanS.Data;

namespace UniScanS.Inspector
{
    public class InspectorConfigHelper : ConfigHelper
    {
        static InspectorConfigHelper _instance;
        public static InspectorConfigHelper Instance()
        {
            if (_instance == null)
                _instance = new InspectorConfigHelper();

            return _instance;
        }

        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new InspectorSystemSettingPanel());
        }

        public override Form GetMainForm()
        {
            return new UniScanS.UI.InspectorMainForm();
        }

        public override void BuildSystemManager()
        {
            SystemManager systemManager = CreateScreenSystemManager();
            SystemManager.SetInstance(systemManager);

            UniScanS.Data.ProductionManagerS productionManager = new ProductionManagerS(PathSettings.Instance().Result);
            productionManager.Load(PathSettings.Instance().Result);

            systemManager.ProductionManager = productionManager;
            systemManager.ExchangeOperator = new InspectorOperator();
            systemManager.UiController = new InspectorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);
            
        }

        private SystemManager CreateScreenSystemManager()
        {
            SystemManager systemManager = new UniScanS.Screen.InspectorSystemManagerS();

            systemManager.Init(new UniScanS.Data.Model.ModelManager(), new UniScanS.Screen.UI.InspectorUiChangerS(), new UniScanS.Screen.AlgorithmArchiver(),
                new UniScanS.Common.Device.DeviceBox(new UniEye.Base.Device.PortMap()), new UniEye.Base.Device.DeviceController(), null, null);

            return systemManager;
        }
    }
}
