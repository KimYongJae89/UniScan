using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class LightParamSimpleForm : Form, ILightParamForm
    {
        Model curModel;
        InspectionStep curInspectionStep;
        TargetGroup curTargetGroup;

        LightParamSet lightParamSet;
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        bool onValueUpdate = false;
        ImageDeviceHandler imageDeviceHandler;

        public LightTypeChangedDelegate LightTypeChanged;
        public LightValueChangedDelegate LightValueChanged;

        public int LightTypeIndex
        {
            get { return lightTypeCombo.SelectedIndex; }
        }

        public LightParamSimpleForm()
        {
            InitializeComponent();

            labelLightSource.Text = StringManager.GetString(this.GetType().FullName,labelLightSource.Text);
            labelLightType.Text = StringManager.GetString(this.GetType().FullName,labelLightType.Text);
            labelExposure.Text = StringManager.GetString(this.GetType().FullName,labelExposure.Text);

            applyLightButton.Text = StringManager.GetString(this.GetType().FullName,applyLightButton.Text);
            applyAllLightButton.Text = StringManager.GetString(this.GetType().FullName,applyAllLightButton.Text);
        }

        public void Initialize(ImageDeviceHandler imageDeviceHandler)
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize");

            this.imageDeviceHandler = imageDeviceHandler;

            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize End");
        }

        public void InitControls()
        {
            InitLightList();
            UpdateLightTypeCombo(LightSettings.Instance().LightParamSet);
        }

        private void UpdateLightTypeCombo(LightParamSet lightParamSet)
        {
            LogHelper.Debug(LoggerType.Operation, "UpdateLightTypeCombo");

            int numLightType = LightSettings.Instance().NumLightType;
            int curLightType = lightTypeCombo.SelectedIndex;

            onValueUpdate = true;

            lightTypeCombo.Items.Clear();
            for (int i = 0; i < LightSettings.Instance().NumLightType; i++)
            {
                string lightTypeName = lightParamSet.LightParamList[i].Name;
                lightTypeCombo.Items.Add(lightTypeName);
            }

            lightTypeCombo.SelectedIndex = MathHelper.Bound(curLightType, 0, lightTypeCombo.Items.Count);

            onValueUpdate = false;

            if (lightTypeCombo.Items.Count == 1)
                lightTypeCombo.Enabled = false;
        }

        private void InitLightList()
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightList");

            List<string> lightNameList = LightSettings.Instance().LightNameList;

            lightValueGrid.Rows.Clear();
            for (int i = 0; i < LightSettings.Instance().NumLight; i++)
                lightValueGrid.Rows.Add(lightNameList[i], 0);
        }

        private void LightParamPanel_Load(object sender, EventArgs e)
        {

        }

        public void SetLightValues(Model model, InspectionStep inspectionStep, TargetGroup targetGroup)
        {
            curModel = model;
            curInspectionStep = inspectionStep;
            curTargetGroup = targetGroup;

            lightParamSet = targetGroup.GetLightParamSet().Clone();
            UpdateLightTypeCombo(lightParamSet);
            UpdateLightValues();
        }

        public void SetLightValues()
        {
            curModel = null;
            curInspectionStep = null;
            curTargetGroup = null;
            comboLightParamSource.Enabled = false;
            applyAllLightButton.Enabled = false;

            lightParamSet = LightSettings.Instance().LightParamSet.Clone();
            UpdateLightTypeCombo(lightParamSet);
            UpdateLightValues();
        }

        private void UpdateLightValues()
        {
            onValueUpdate = true;

            if (curTargetGroup != null)
                comboLightParamSource.SelectedIndex = (int)curTargetGroup.LightParamSource;

            if (lightParamSet.LightParamList.Count <= 0)
                return;

            int lightTypeIndex = lightTypeCombo.SelectedIndex;

            LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];
            if (lightValueGrid.Rows.Count > 0)
            {
                for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                    lightValueGrid.Rows[i].Cells[1].Value = lightParam.LightValue.Value[i];
            }

            exposureTimeMs.Value = lightParam.ExposureTimeUs / 1000;
            if (exposureTimeMs.Value == 0)
                exposureTimeMs.Value = MachineSettings.Instance().DefaultExposureTimeMs;

            onValueUpdate = false;
        }

        private void comboLightParamSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            curTargetGroup.LightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;

            LightParamSet lightParamSet = curTargetGroup.GetLightParamSet().Clone();
            UpdateLightTypeCombo(lightParamSet);
            UpdateLightValues();
        }

        public void SetLightTypeIndex(int lightTypeIndex)
        {
            lightTypeCombo.SelectedIndex = lightTypeIndex;
        }

        private void lightTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            UpdateLightValues();

            LightTypeChanged?.Invoke();
        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = "Light param is invailed";
                MessageBox.Show(message);
                return;
            }

            LightValueChanged?.Invoke(true);
        }

        private void applyAllLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = StringManager.GetString(this.GetType().FullName, "Light param is invailed");
                MessageForm.Show(this.ParentForm, message, MessageFormType.OK);
                return;
            }

            string message2 = StringManager.GetString(this.GetType().FullName, "Apply this light parameter into every step?");
            DialogResult res = MessageForm.Show(this.ParentForm, message2, MessageFormType.YesNo);
            if (res == DialogResult.No)
            {
                return;
            }

            if (curInspectionStep.UseInspectionStepLight)
                return;

            foreach (InspectionStep inspectionStep in curModel.InspectionStepList)
            {
                if (curInspectionStep.StepType == inspectionStep.StepType && inspectionStep.UseInspectionStepLight)
                {
                    inspectionStep.LightParamSet = lightParamSet.Clone();
                }
            }

            LightValueChanged?.Invoke(false);
        }

        private bool ApplyLight()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - exposureTime_ValueChanged");

            try
            {
                int lightTypeIndex = lightTypeCombo.SelectedIndex;

                LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];
                lightParam.Name = lightTypeCombo.Text;
                lightParam.LightParamType = LightParamType.Value;
                lightParam.LightStableTimeMs = MachineSettings.Instance().DefaultLightStableTimeMs;
                lightParam.ExposureTimeUs = Convert.ToInt32(exposureTimeMs.Value * 1000);
                lightParam.ExposureTime3dUs = 0;

                List<string> lightNameList = LightSettings.Instance().LightNameList;
                for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                {
                    lightParam.LightValue.Value[i] = Convert.ToInt32(lightValueGrid.Rows[i].Cells[1].Value);
                    lightNameList[i] = (string)lightValueGrid.Rows[i].Cells[0].Value;
                }

                if (curTargetGroup != null)
                {
                    LightParamSource lightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;

                    switch (lightParamSource)
                    {
                        case LightParamSource.Model:
                            curModel.LightParamSet = lightParamSet.Clone();
                            break;
                        case LightParamSource.InspectionStep:
                            curInspectionStep.LightParamSet = lightParamSet.Clone();
                            break;
                        case LightParamSource.TargetGroup:
                            curTargetGroup.LightParamSet = lightParamSet.Clone();
                            break;
                    }
                }

                LightSettings.Instance().LightParamSet = lightParamSet.Clone();
                LightSettings.Instance().Save();

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        private void editTypeNameButton_Click(object sender, EventArgs e)
        {
            InputForm form = new InputForm(StringManager.GetString(this.GetType().FullName, "Edit Light Type Name"), lightTypeCombo.Items[lightTypeCombo.SelectedIndex].ToString());
            form.StartPosition = FormStartPosition.CenterParent;
            form.TopLevel = true;
            form.TopMost = true;

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.lightParamSet.LightParamList[lightTypeCombo.SelectedIndex].Name = form.InputText;
                UpdateLightTypeCombo(this.lightParamSet);
                ApplyLight();
                //lightTypeCombo.Items[lightTypeCombo.SelectedIndex] = form.InputText;
                //UpdateLightTypeCombo(this.lightParamSet);
            }
        }

        private void panelValue_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LightParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void ToggleVisible()
        {
            Visible = !Visible;
        }
    }
}
