using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanG.Settings;
using UniScanG.UI.Model;
using System.Windows.Forms;
using UniScanG.Common.UI;
using UniScanG.Common.Settings.UI;
using UniScanG.Common;
using System;
using System.Collections.Generic;
using UniScanG.UI.Teach.Monitor;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.UI.Inspect;
using System.Drawing;
using System.IO;
using UniEye.Base.Settings;
using DynMvp.Base;
using DynMvp.InspData;

namespace UniScanG.UI
{
    public enum MainTabKey
    {
        Inspect, Model, Teach, Report, Setting, Log, Exit
    }

    public interface IModelImagePanel
    {
        void SetPreview(ModelDescription modelDescription);
    }

    public interface IVncContainer
    {
        void ProcessStarted(IVncControl startVncViewer);
        void ProcessExited();
        void ExitAllVncProcess(bool terminateProgram);
    }

    public delegate void UpdateResultDelegate1(InspectionResult inspectionResult);
    public delegate void UpdateResultDelegate2(Bitmap image, List<DataGridViewRow> dataGridViewRowList);
    public delegate void ResetDelegate();

    public interface IInspectDefectPanel
    {
        void AddDelegate(UpdateResultDelegate1 UpdateResultDelegate);
        void AddDelegate(UpdateResultDelegate2 UpdateResultDelegate);
        bool BlockUpdate { get; set; }
        void Reset();
    }

    public interface IInspectExtraPanel
    {
        void UpdateResult(InspectionResult inspectionResult);
        void UpdateVisible();
    }

    public abstract class MonitorUiChanger : UiChanger
    {
         public abstract Control CreateTeachSettingPanel();
        
        public override IMainTabPage CreateTeachPage()
        {
            return new UI.Teach.Monitor.TeachPage();
        }

        public override IMainTabPage CreateInspectPage()
        {
            this.inspect = new UI.Inspect.InspectPage();

            return this.inspect;
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
        public override IMainTabPage CreateInspectPage()
        {
            this.inspect = new UI.Inspect.InspectPage();
            return this.inspect;
        }

        public override ISettingPage CreateSettingPage()
        {
            this.setting = null;
            return (ISettingPage)this.setting;
        }

        public override List<IStatusStripPanel> GetStatusStrip()
        {
            return new List<IStatusStripPanel>();
        }

        public abstract IModellerControl CreateImageController();
        public abstract IModellerControl CreateParamController();

        public abstract IModellerControl CreateTeachToolBar();
    }

    public abstract class UiChanger : UniEye.Base.UI.UiChanger
    {
        public IMainForm MainForm { get => mainForm; }
        public IMainTabPage InspectControl { get => inspect; }
        public IMainTabPage ModelControl { get => model; }
        public IMainTabPage TeachControl { get => teach; }
        public IMainTabPage SettingControl { get => setting; }
        public IMainTabPage ReportControl { get => report; }
        public IMainTabPage LogControl { get => log; }
        public IMainTabPage[] TabPages => new IMainTabPage[] { this.inspect, this.model, this.teach, this.setting, this.report, this.log };
                
        protected IUiControlPanel uiControlPanel;

        protected IMainForm mainForm = null;
        protected IMainTabPage inspect = null;
        protected IMainTabPage model = null;
        protected IMainTabPage teach = null;
        protected IMainTabPage report = null;
        protected IMainTabPage setting = null;
        protected IMainTabPage log = null;


        //public override IMainForm CreateMainForm()
        //{
        //    this.mainForm = new MonitorMainform(uiControlPanel, "");
        //    return this.mainForm;
        //}

        public IMainTabPage CreateLogPage()
        {
            this.log = new UniScanG.UI.Log.LogPage();
            return this.log;
        }

        public string GetMainformTitle()
        {
            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;

            string mainformTitle = string.Format("{0} @ {1}, Version {2} Build {3}", title, copyright, VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString);
            if (SystemManager.IsUserAdministrator())
                mainformTitle += " (Administrator)";
            return mainformTitle;
        }

        public IMainTabPage CreateModelPage()
        {
            this.model = new UI.Model.ModelPage();
            return this.model;
        }

        public abstract IMainTabPage CreateReportPage();
        //{
        //    this.report = new UI.Report.ReportPage();
        //    return this.report;
        //}

        public abstract IMainTabPage CreateInspectPage();
        public abstract Control CreateInfoBufferPanel();
        public abstract IInspectDefectPanel CreateDefectPanel();
        public abstract IInspectExtraPanel CreateExtraInfoPanel();
        public abstract IInspectExtraPanel CreateExtraImagePanel();
        public abstract IInspectExtraPanel CreateExtraLengthPanel();

        public abstract IMainTabPage CreateTeachPage();

        public abstract List<IStatusStripPanel> GetStatusStrip();

        public override IReportPanel CreateReportPanel() { return null; }
        public override void EnableTargetParamControls(TargetParamControl targetParamControl) { }
        public override void SetupVisionParamControl(VisionParamControl visionParamControl) { }
        public override void ChangeModellerMenu(ModellerPage modellerPage) { }
        public override string[] GetProbeNames() { return null; }
        public override string[] GetStepTypeNames() { return null; }
        public override IDefectReportPanel CreateDefectReportPanel() { return null; }
        public override void BuildAdditionalAlgorithmTypeMenu(ModellerPage modellerPage, ToolStripItemCollection dropDownItems) { }
    }}
