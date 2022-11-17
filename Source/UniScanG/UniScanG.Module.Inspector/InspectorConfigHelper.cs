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
using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;
using UniScanG.Module.Inspector.Settings.Inspector;
using UniScanG.Module.Inspector.Settings.Inspector.UI;
using UniScanG.Module.Inspector.UI;
using UniScanG.Data;

namespace UniScanG.Module.Inspector
{
    public class InspectorConfigHelper : Common.ConfigHelper
    {
        public static void SetInstance()
        {
            if (ConfigHelper.instance == null)
                ConfigHelper.instance = new InspectorConfigHelper();
        }

        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new InspectorSystemSettingPanel());
        }

        public override Form GetMainForm()
        {
            //if (OperationSettings.Instance().SystemType == "Gravure")
            {
                if (CustomizeSettings.Instance().ProgramTitle != "UniScanG.IM")
                {
                    CustomizeSettings.Instance().Title = "";
                    CustomizeSettings.Instance().ProgramTitle = "UniScanG.IM";
                    CustomizeSettings.Instance().Save();
                }
            }

            this.mainForm = new InspectorMainForm();
            //SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.C_CONNECTED, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
            return this.mainForm;
        }

        public override void BuildSystemManager()
        {
            SystemManager systemManager = CreateGravureSystemManager();
            SystemManager.SetInstance(systemManager);

            //SystemManager.Instance().ProductionManager.Load(PathSettings.Instance().Result);

            systemManager.ExchangeOperator = new InspectorOperator();
            systemManager.UiController = new InspectorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);

            systemManager.InitalizeModellerPageExtender();
        }

        private SystemManager CreateGravureSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerG());
            SystemManager systemManager = new InspectorSystemManagerG();

            systemManager.Init(
                new UniScanG.Data.Model.ModelManager(),
                new InspectorUiChangerG(),
                new UniScanG.Gravure.AlgorithmArchiver(),
                new UniScanG.Gravure.Device.DeviceBox(new UniScanG.Module.Inspector.Device.PortMap()),
                new UniScanG.Module.Inspector.Device.DeviceController(),
                new UniScanG.Gravure.Data.ProductionManagerG(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());

            return systemManager;
        }
    }
}
