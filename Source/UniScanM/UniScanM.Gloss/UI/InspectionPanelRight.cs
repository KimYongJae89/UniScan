using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Device.Serial.Sensor.SC_HG1_485;
using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanM.Gloss.Operation;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        #region 생성자
        public InspectionPanelRight()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
            GlossSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;
            UpdateComboBox();
        }
        #endregion

        #region 대리자
        private delegate void ProductInspectedDelegate(DynMvp.InspData.InspectionResult inspectionResult);

        private delegate void ExitWaitInspectionDelegate();

        private delegate void ClearPanelDelegate();
        #endregion

        #region 이벤트
        private void comboScanWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            string name = comboBox.SelectedValue.ToString();
            GlossSettings.Instance().SelectedGlossScanWidth = GlossSettings.Instance().GlossScanWidthList.Find(x => x.Name == name);
        }

        private void comboCalParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string name = comboBox.SelectedValue.ToString();
            GlossSettings.Instance().SelectedGlossCalibrationParam = GlossSettings.Instance().GlossCalibrationParamList.Find(x => x.Name == name);
        }
        #endregion

        #region 인터페이스
        public void EnterWaitInspection() { }

        public void ExitWaitInspection() { }

        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            Gloss.Data.InspectionResult glossInspectionResult = (Gloss.Data.InspectionResult)inspectionResult;
            SuspendLayout();

            UiHelper.SuspendDrawing(labelState);

            if (SystemState.Instance().OpState != OpState.Idle)
            {
                labelState.Text = StringManager.GetString("Measure");
                labelState.BackColor = Color.Green;
                labelState.ForeColor = Color.White;
            }
            UiHelper.ResumeDrawing(labelState);
            ResumeLayout();
        }

        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }
            labelState.Text = StringManager.GetString("State");
            labelState.BackColor = Color.Black;
        }

        public void Initialize() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            if (curOpState == OpState.Idle)
            {
                labelState.ForeColor = SystemColors.ControlText;
                labelState.BackColor = SystemColors.Control;
                labelState.Text = "";
                progressBarZeroing.Value = 0;
                comboScanWidth.Enabled = true;
                comboCalParam.Enabled = true;
                return;
            }
            else
            {
                comboScanWidth.Enabled = false;
                comboCalParam.Enabled = false;
            }
        }
        #endregion

        #region 속성
        private AxisHandler AxisHandler => SystemManager.Instance().DeviceController.RobotStage;
        #endregion

        #region 메서드
        public void UpdateComboBox()
        {
            var widthNameList = new List<string>();
            var widthList = GlossSettings.Instance().GlossScanWidthList;
            foreach (var scanWidth in widthList)
            {
                widthNameList.Add(scanWidth.Name);
            }
            comboScanWidth.DataSource = widthNameList.ToArray();

            var calParamNameList = new List<string>();
            var paramList = GlossSettings.Instance().GlossCalibrationParamList;
            foreach (var param in paramList)
            {
                calParamNameList.Add(param.Name);
            }
            comboCalParam.DataSource = calParamNameList.ToArray();
        }

        private void UpdatePanel()
        {
            UpdateComboBox();
        }
        #endregion

        private void buttonSafety_Click(object sender, EventArgs e)
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as InspectRunner;
            inspectRunner.GlossMotionController.MoveSafetyPos();
        }

        private void buttonCalibration_Click(object sender, EventArgs e)
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as InspectRunner;
            inspectRunner.GlossMotionController.MoveCalPos();
        }

        private void textLotNo_TextChanged(object sender, EventArgs e)
        {
            var control = sender as TextBox;
            GlossSettings.Instance().ManualLotNo = control.Text;
        }
    }
}
