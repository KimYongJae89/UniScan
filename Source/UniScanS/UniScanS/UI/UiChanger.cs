using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanS.Settings;
using UniScanS.UI.Model;
using System.Windows.Forms;
using UniScanS.Common.UI;
using UniScanS.Common.Settings.UI;
using UniScanS.Common;
using System;
using System.Collections.Generic;
using UniScanS.UI.Teach.Monitor;
using UniScanS.Common.Data;
using UniScanS.Screen.UI;
using UniScanS.Common.Util;
using UniScanS.Common.Exchange;
using UniScanS.UI.Etc;
using UniScanS.UI.Teach.Inspector;
using UniScanS.UI.Teach;
using UniScanS.UI.Inspect;
using System.Drawing;
using UniScanS.Screen.Data;

namespace UniScanS.UI
{
    public interface IModelImagePanel
    {
        void SetPreview(ModelDescription modelDescription);
    }

    public interface IVncContainer
    {
        void ProcessStarted(IVncControl startVncViewer);
        void ProcessExited();
    }

    public delegate void UpdateResultDelegate(SheetResult sheetResult, List<DataGridViewRow> dataGridViewRowList);

    public interface IInspectDefectPanel
    {
        void AddDelegate(UpdateResultDelegate UpdateResultDelegate);
    }

    public abstract class MonitorUiChanger : UiChanger
    {
        public abstract Control CreateTeachSettingPanel();

        //public override IInspectionPanel CreateInspectionPanel() { return null; }

        public override Control CreateTeachPage()
        {
            return new UI.Teach.Monitor.TeachPage();
        }

        public override Control CreateInspectPage()
        {
            return new UI.Inspect.InspectPage();
        }
        
        public abstract List<Control> GetTeachButtons(IVncContainer teachPage);

        public List<Control> GetReportButtons(IVncContainer reportPage)
        {
            List<Control> controlList = new List<Control>();
            
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_REPORT, inspector, reportPage.ProcessStarted, reportPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }

        public List<Control> GetInspectButtons(IVncContainer inspectPage)
        {
            List<Control> controlList = new List<Control>();
            
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_INSPECT, inspector, inspectPage.ProcessStarted, inspectPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }
    }

    public abstract class InspectorUiChanger : UiChanger
    {
        public override Control CreateInspectPage()
        {
            return new UI.Inspect.InspectPage();
        }

        public override ISettingPage CreateSettingPage()
        {
            return null;
        }

        public abstract IModellerControl CreateImageController();
        public abstract IModellerControl CreateParamController();

        public abstract IModellerControl CreateTeachToolBar();
    }

    public abstract class UiChanger : UniEye.Base.UI.UiChanger
    {
        IUiControlPanel uiControlPanel;

        public override IMainForm CreateMainForm()
        {
            return new MonitorMainform(uiControlPanel);
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            if (uiControlPanel == null)
            {
                Control Inspect = CreateInspectPage();
                Control model = CreateModelPage();
                Control teach = CreateTeachPage();
                Control report = CreateReportPage();

                uiControlPanel = new MainTabPanel(Inspect, model, teach, report);
            }

            return uiControlPanel;
        }

        public Control CreateModelPage()
        {
            return new UI.Model.ModelPage();
        }

        public Control CreateReportPage()
        {
            return new UI.Report.ReportPage();
        }

        public abstract Control CreateInspectPage();
        public abstract Control CreateDefectInfoPanel();
        public abstract IInspectDefectPanel CreateDefectPanel();

        public abstract Control CreateTeachPage();
        
        //public override IInspectionPanel CreateInspectionPanel() { return null; }
        public override IReportPanel CreateReportPanel() { return null; }
        public override void EnableTargetParamControls(TargetParamControl targetParamControl) { }
        public override void SetupVisionParamControl(VisionParamControl visionParamControl) { }
        public override void ChangeModellerMenu(ModellerPage modellerPage) { }
        public override string[] GetProbeNames() { return null; }
        public override string[] GetStepTypeNames() { return null; }
        public override IDefectReportPanel CreateDefectReportPanel() { return null; }
        public override void BuildAdditionalAlgorithmTypeMenu(ModellerPage modellerPage, ToolStripItemCollection dropDownItems) { }
    }}
