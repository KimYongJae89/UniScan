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
using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;
using UniScanG.Common.UI;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor.UI;
using UniScanG.Module.Controller.UI;
using UniScanG.Data;
using UniScanG.MachineIF;

namespace UniScanG.Module.Controller
{
    public class MonitorConfigHelper : Common.ConfigHelper
    {
        public static void SetInstance()
        {
            if (ConfigHelper.instance == null)
                ConfigHelper.instance = new MonitorConfigHelper();
        }

        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new MonitorSystemSettingPanel());
        }

        public override Form GetMainForm()
        {
            IUiControlPanel uiControlPanel = SystemManager.Instance().UiChanger.CreateUiControlPanel();
            SystemManager.Instance().UiController.SetUiControlPanel(uiControlPanel);

            //if (OperationSettings.Instance().SystemType == "Gravure")
            {
                if (CustomizeSettings.Instance().ProgramTitle != "UniScanG.CM")
                {
                    CustomizeSettings.Instance().Title = StringManager.GetString(typeof(MonitorMainform).FullName, "MLCC Gravure Print Inspector");
                    CustomizeSettings.Instance().ProgramTitle = "UniScanG.CM";
                    CustomizeSettings.Instance().Save();
                }
            }

            string title = CustomizeSettings.Instance().Title;
            this.mainForm = new MonitorMainform(uiControlPanel, title);
            return this.mainForm;
        }

        public override void BuildSystemManager()
        {
            MachineIfProtocolList.Set(new MachineIF.MachineProtocolList());

            SystemManager systemManager = CreateGravureSystemManager();
            SystemManager.SetInstance(systemManager);

            //SystemManager.Instance().ProductionManager.Load(PathSettings.Instance().Result);
            systemManager.ExchangeOperator = new MonitorOperator();
            systemManager.UiController = new MonitorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);
        }

        private SystemManager CreateGravureSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerG());
            SystemManager systemManager = new MonitorSystemManagerG();

            new SimpleProgressForm().Show(() =>
            {
            systemManager.Init(
                new UniScanG.Data.Model.ModelManager(),
                new MonitorUiChangerG(),
                new UniScanG.Gravure.AlgorithmArchiver(),
                new UniScanG.Gravure.Device.DeviceBox(new UniScanG.Module.Controller.Device.PortMap()),
                new UniScanG.Module.Controller.Device.DeviceController(),
                new UniScanG.Gravure.Data.ProductionManagerG(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());
            });

            SheetCombiner.SetCollector(new UniScanG.Gravure.Data.ResultCollector());
            return systemManager;
        }
    }
}
