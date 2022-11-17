using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.Device;
using UniEye.Base;
using DynMvp.Devices;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.InspData;
using UniScanM.Pinhole.Data;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;
using System.Reflection;
using DynMvp.Devices.Light;
using UniScanM.Pinhole.UI.MenuPage;
using System.Threading;
using UniScanM.Pinhole.Settings;
using UniScanM.Pinhole.MachineIF;
using UniScanM.Data;
using UniScanM.UI.MenuPage.AutoTune;
using UniScanM.Operation;
using UniEye.Base.Data;
using System.Diagnostics;

namespace UniScanM.Pinhole.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        ResultTable resultTable;
        
        public InspectionPanelRight()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            InitResultTable();
            cmbNGType.SelectedIndex = 0;
            StringManager.AddListener(this);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        void InitResultTable()
        {
            resultTable = new ResultTable();
            this.resultTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTable.Location = new System.Drawing.Point(3, 3);
            this.resultTable.Name = string.Format("inspectMap");
            this.resultTable.Size = new System.Drawing.Size(409, 523);
            panelDefectMap.Controls.Add(resultTable);
        }
        
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            //Debug.WriteLine("Right panel ProductInspected...111.");
            resultTable.UpdateQuary.Set();
            //Debug.WriteLine("Right panel ProductInspected...222.");
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            SetModel();
            GetModel();
        }

        void SetModel()
        {
            Data.Model curModel = (Data.Model)SystemManager.Instance().CurrentModel;
            //curModel.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)numLightValue.Value;
            //curModel.LightParamSet.LightParamList[0].LightValue.Value[0] = LightValueHelper.Get_PercentageToLightValue((int)numLightValue.Value);

            curModel.LightParamSet.LightParamList[0].LightValue.Value[0] = PinholeSettings.Instance().DefalutLightValue;

            //PinholeSettings.Instance().SkipLength = (int)numSheetLenght.Value;
            PinholeSettings.Instance().SkipLength = Convert.ToInt32(numSheetLenght.Value * 1000 / PinholeSettings.Instance().PixelResolution);
            curModel.InspectParam.PinholeParamValue1.EdgeThreshold = (int)numEdgeThreshold1.Value;
            curModel.InspectParam.PinholeParamValue1.DefectThreshold = (int)numDefectThreshold1.Value;
            curModel.InspectParam.PinholeParamValue2.EdgeThreshold = (int)numEdgeThreshold2.Value;
            curModel.InspectParam.PinholeParamValue2.DefectThreshold = (int)numDefectThreshold2.Value;
            curModel.InspectParam.NGType = (NGType)Enum.Parse(typeof(NGType), cmbNGType.Text);

            Data.ModelManager modelManager = (Data.ModelManager)SystemManager.Instance().ModelManager;
            modelManager.SaveModel(curModel);
            PinholeSettings.Instance().Save();
        }

        void GetModel()
        {
            UniScanM.Pinhole.Data.Model curModel = (UniScanM.Pinhole.Data.Model)SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return;

            //numLightValue.Value = curModel.LightParamSet.LightParamList[0].LightValue.Value[0];
            int lightValue = curModel.LightParamSet.LightParamList[0].LightValue.Value[0];
            //numLightValue.Value = LightValueHelper.Get_LightValueToPercentage(lightValue);
            //labelValueOfLight.Text = numLightValue.Value.ToString();
            labelPercent.Text = LightValueHelper.Get_LightValueToPercentage(PinholeSettings.Instance().DefalutLightValue).ToString();
            labelRealValue.Text = string.Format("({0})", PinholeSettings.Instance().DefalutLightValue);

            //numSheetLenght.Value = PinholeSettings.Instance().SkipLength;
            numSheetLenght.Value = (decimal)(PinholeSettings.Instance().SkipLength * PinholeSettings.Instance().PixelResolution / 1000.0f);
            numEdgeThreshold1.Value = (int)curModel.InspectParam.PinholeParamValue1.EdgeThreshold;
            numDefectThreshold1.Value = (int)curModel.InspectParam.PinholeParamValue1.DefectThreshold;
            numEdgeThreshold2.Value = (int)curModel.InspectParam.PinholeParamValue2.EdgeThreshold;
            numDefectThreshold2.Value = (int)curModel.InspectParam.PinholeParamValue2.DefectThreshold;

            cmbNGType.SelectedIndex = (int)curModel.InspectParam.NGType;
            curModel.InspectParam.NGType = (NGType)Enum.Parse(typeof(NGType), cmbNGType.Text);
        }

        void UpdateJobInfo()
        {
            labelPassWidth.Text = string.Format("{0:f} mm", PinholeSettings.Instance().SmallSize.Width);
            labelPassHeight.Text = string.Format("{0:f} mm", PinholeSettings.Instance().SmallSize.Height);
            labelPercent.Text = LightValueHelper.Get_LightValueToPercentage(PinholeSettings.Instance().DefalutLightValue).ToString();
        }
        
        private void btnAutoLight_Click(object sender, EventArgs e)
        {
            ((Operation.InspectRunner)SystemManager.Instance().InspectRunner).ToggleAutoTuneMode();
        }

        private void process1_Exited(object sender, EventArgs e)
        {

        }

        private void panelStatus_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
                return;

            UpdateJobInfo();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        public void Initialize() { }
        public void ClearPanel() { }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(DynMvp.Data.Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string marginW = MelsecDataConverter.WInt(111111);
            string marginL = MelsecDataConverter.WInt(2222);
            string blotW = MelsecDataConverter.WInt(33);
            string blotL = MelsecDataConverter.WInt(444);

            string defectW = MelsecDataConverter.WInt(55555);//MarginW
            string defectL = MelsecDataConverter.WInt(6);//MarginW

            string judge = "0003";

            //string sendData = string.Format("{0}{1}{2}{3}{4}{5}",
            //    judge, marginL, marginW, blotW, blotL, defectW, defectL
            //    );

            string sendData = string.Format("{0}0000{1}{2}{3}{4}{5}{6}",
                judge, marginW, marginL, blotW, blotL, defectW, defectL
                );

            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE, sendData);
        }
        
        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

           ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();
        }

        private void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");

            UpdateJobInfo();

            groupModelSettings.Enabled = !flag;
            groupModelSettings.Expanded = !flag;
        }

        private void groupModelSettings_ExpandedStateChanging(object sender, CancelEventArgs e)
        {
            GetModel();
        }
    }
}
