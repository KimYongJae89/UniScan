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
using DynMvp.Devices.Light;


namespace UniScanM.CGInspector.UI
{
    public partial class InspectionPanelRight_M : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }
        Operation.InspectRunner inspectrunner;
        public Operation.InspectRunner InspectRunner { get => inspectrunner; set => inspectrunner = value; }
        
        public InspectionPanelRight_M()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.startPosValue.Maximum = this.endPosValue.Maximum = int.MaxValue;
            this.startPosValue.Minimum = this.endPosValue.Minimum = int.MinValue;

            this.startPosValue.Value = -10000;
            this.endPosValue.Value = 170000;
            //if (SystemManager.Instance().DeviceController.RobotStage != null)
            //{
            //    AxisPosition[] limitPosition = SystemManager.Instance().DeviceController.RobotStage?.GetLimitPos();

            //    trackBarPos.Minimum = (int)limitPosition[0].Position[0];
            //    trackBarPos.Maximum = (int)limitPosition[1].Position[0];

            //    robotPosUpdateTimer.Start();
            //}

            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;
            SystemManager.Instance().OnModelChanged += SystemManager_OnModelChanged; 
            SystemState.Instance().AddOpListener(this);

            StringManager.AddListener(this);
        }

        private void SystemManager_OnModelChanged()
        {
            UpdateParamControl();
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


        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        public void Initialize() { }
        public void ClearPanel()
        {
            //ResetPrintingLengthGridView();
        }
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

            LightParam lightParam = SystemManager.Instance().CurrentModel?.LightParamSet.LightParamList.FirstOrDefault();
            if (lightParam == null || lightParam.LightValue.NumLight == 0)
            {
                numericUpDownLight.Enabled = this.lightSaveButton.Enabled = false;
            }
            else
            {
                numericUpDownLight.Enabled = this.lightSaveButton.Enabled = true;
                numericUpDownLight.Value = (decimal)lightParam.LightValue.Value[0];
            }
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
                //UpdateContol(labelState, null, Color.Black, Color.White);
                //UpdateContol(state, "", Color.Black, Color.White);
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
            if (text != null)
                control.Text = text;
            control.BackColor = backColor;
            control.ForeColor = foreColor;
            UiHelper.ResumeDrawing(control);
        }

        public void GrabButton_Click(object sender, EventArgs e)
        {
            
        }

        private void numericUpDownLight_ValueChanged(object sender, EventArgs e)
        {
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            Model model = SystemManager.Instance().CurrentModel;
            if (model != null)
            {
                LightParamSet lightParamSet = model.LightParamSet;
                LightValue lightvalue = lightParamSet.LightParamList[0].LightValue;
                lightvalue.Value[0] = (int)numericUpDownLight.Value;
            }
        }

        private void InspectionPanelRight_M_Load(object sender, EventArgs e)
        {

        }

        private void cycleOnce_Click(object sender, EventArgs e)
        {
            if (!SystemState.Instance().IsInspection)
                return;

            Cycle(1);          
        }

        private void cycleMulti_Click(object sender, EventArgs e)
        {
            if (!SystemState.Instance().IsInspection)
                return;

            Cycle(10);
        }

        private void cycleStop_Click(object sender, EventArgs e)
        {
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        private void Cycle(int count)
        {
            UniScanM.CGInspector.Operation.InspectRunner inspectRunner = SystemManager.Instance().InspectRunner as UniScanM.CGInspector.Operation.InspectRunner;
            if (inspectRunner != null)
            {
                float startPos = (float)this.startPosValue.Value;
                float endPos = (float)this.endPosValue.Value;
                inspectRunner.RunRunRun(startPos, endPos, count, cancellationTokenSource.Token);
            }
        }

        private void lightSaveButton_Click(object sender, EventArgs e)
        {
            Model currentModel = SystemManager.Instance().CurrentModel;
            SystemManager.Instance().ModelManager.SaveModel(currentModel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
