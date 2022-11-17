using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanS.Settings;
using UniScanS.UI.Model;
using System.Windows.Forms;
using UniScanS.Common;
using System;
using UniScanS.UI;
using System.Collections.Generic;
using UniScanS.Common.Data;
using UniScanS.UI.Teach.Monitor;
using UniScanS.Screen.UI.Teach.Monitor;
using UniScanS.Screen.UI.Inspect;
using UniScanS.Screen.UI.Teach.Inspector;
using UniScanS.Common.Exchange;
using UniScanS.UI.Etc;
using UniScanS.UI.Teach.Inspector;
using UniScanS.UI.Teach;
using UniScanS.Screen.UI.Teach;
using UniScanS.Common.Util;

namespace UniScanS.Screen.UI
{
    public class MonitorUiChangerS : MonitorUiChanger
    {
        public override Control CreateDefectInfoPanel()
        {
            return new InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new DefectPanel();
        }
        
        public override List<Control> GetTeachButtons(IVncContainer teachPage)
        {
            List<Control> controlList = new List<Control>();
            
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_TEACH, inspector, teachPage.ProcessStarted, teachPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }
        
        public override IReportPanel CreateReportPanel()
        {
            return new Screen.UI.Report.ReportPanel();
        }

        public override Control CreateTeachSettingPanel()
        {
            return new Teach.Monitor.SettingPanel();
        }
    }

    public class InspectorUiChangerS : InspectorUiChanger
    {
        public override Control CreateDefectInfoPanel()
        {
            return new InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new DefectPanel();
        }
        
        public override IModellerControl CreateImageController()
        {
            return new ImageController();
        }

        public override UniEye.Base.UI.ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtenderS();
        }

        public override IModellerControl CreateParamController()
        {
            return new ParamController();
        }
        
        public override Control CreateTeachPage()
        {
            return new UniScanS.UI.Teach.Inspector.TeachPage();
        }

        public override IModellerControl CreateTeachToolBar()
        {
            return new TeachToolBar();
        }
    }
}
