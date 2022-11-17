using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using UniScanM.CGInspector.Operation;
using DynMvp.UI.Touch;
using System.Threading;
using Infragistics.Win.Misc;
using Infragistics.Win;
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniScanM.Operation;

namespace UniScanM.CGInspector.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelRight()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            //if (SystemManager.Instance().DeviceController.RobotStage != null)
            //{
            //    AxisPosition[] limitPosition = SystemManager.Instance().DeviceController.RobotStage?.GetLimitPos();
                
            //    trackBarPos.Minimum = (int)limitPosition[0].Position[0];
            //    trackBarPos.Maximum = (int)limitPosition[1].Position[0];

            //    robotPosUpdateTimer.Start();
            //}

            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
        }

        private void ErrorManager_OnStartAlarm()
        {

        }

        delegate void ProductInspectedDelegate(InspectionResult inspectionResult);
        public void ProductInspected(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            UiHelper.ResumeDrawing(state);
            UiHelper.ResumeDrawing(labelState);
        }

        private void UpdateSummary()
        {
            UniScanM.Data.Production Production = SystemManager.Instance().ProductionManager.CurProduction;
            Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as Data.Production;
            if (production != null)
            {
                this.summaryProduction.Text = production.Total.ToString();
                this.summaryGood.Text = production.Good.ToString();
                this.summaryNG.Text = production.Ng.ToString();
                this.summarySkip.Text = production.Pass.ToString();
            }
            else
            {
                this.summaryProduction.Text = "";
                this.summaryGood.Text = "";
                this.summaryNG.Text = "";
                this.summarySkip.Text = "";
                this.summaryMargin.Text = "";
                this.summaryBlot.Text = "";
                this.summaryPinhole.Text = "";
            }
        }

        delegate void UpdateResultValueDelgate1(Color color);
        private void UpdateResultValue(Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateResultValueDelgate1(UpdateResultValue), color);
                return;
            }

            this.marginW.Text = "---.-";
            this.marginW.ForeColor = color;

            this.marginL.Text = "---.-";
            this.marginL.ForeColor = color;

            this.blotW.Text = "--.-";
            this.blotW.ForeColor = color;

            this.blotL.Text = "--.-";
            this.blotL.ForeColor = color;

            this.defectW.Text = "---";
            this.defectW.ForeColor = color;

            this.defectH.Text = "---";
            this.defectH.ForeColor = color;

            this.defectC.Text = "-";
            this.defectC.ForeColor = color;
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (SystemManager.Instance().DeviceController.RobotStage != null)
                trackBarPos.Value = Math.Max(trackBarPos.Minimum, Math.Min(trackBarPos.Maximum , (int)SystemManager.Instance().DeviceController.RobotStage.GetActualPos().Position[0]));
        }

        public void Initialize() { }
        public void ClearPanel() { }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
        
        private void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

            ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (curOpState == OpState.Idle)
            {
                UpdateContol(labelState, null, Color.Black, Color.White);
                UpdateContol(state, "", Color.Black, Color.White);
                return;
            }
        }

        private delegate void UpdateContolDelegate(Control control, string text, Color backColor, Color foreColor);
        private void UpdateContol(Control control, string text, Color backColor, Color foreColor)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateContolDelegate(UpdateContol), control, text, backColor, foreColor);
                return;
            }

            UiHelper.SuspendDrawing(control);
            if(text!=null)
                control.Text = text;
            control.BackColor = backColor;
            control.ForeColor = foreColor;
            UiHelper.ResumeDrawing(control);
        }

        private void summaryUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateSummary();
        }
    }
}
