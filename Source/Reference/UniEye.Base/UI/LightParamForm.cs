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
    public interface ILightParamForm
    {
        LightParamSet LightParamSet { get; }
        int LightTypeIndex { get; }

        void InitControls();
        void Initialize(ImageDeviceHandler imageDeviceHandler);
        void SetLightValues(Model currentModel, InspectionStep curInspectionStep, TargetGroup curTargetGroup);
        void ToggleVisible();
    }

    public delegate void LightTypeChangedDelegate();
    public delegate void LightValueChangedDelegate(bool imageUpdateRequired);

    public partial class LightParamForm : Form, ILightParamForm
    {
        ImageDeviceHandler imageDeviceHandler;

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

        public LightTypeChangedDelegate LightTypeChanged;
        public LightValueChangedDelegate LightValueChanged;

        public int LightTypeIndex
        {
            get { return lightTypeCombo.SelectedIndex; }
        }

        public LightParamForm()
        {
            InitializeComponent();

            labelLightSource.Text = StringManager.GetString(this.GetType().FullName,labelLightSource.Text);
            labelLightType.Text = StringManager.GetString(this.GetType().FullName,labelLightType.Text);
            labelExposure.Text = StringManager.GetString(this.GetType().FullName,labelExposure.Text);
            labelExposure3D.Text = StringManager.GetString(this.GetType().FullName,labelExposure3D.Text);
            labelLightStable.Text = StringManager.GetString(this.GetType().FullName,labelLightStable.Text);

            applyLightButton.Text = StringManager.GetString(this.GetType().FullName,applyLightButton.Text);
            applyAllLightButton.Text = StringManager.GetString(this.GetType().FullName,applyAllLightButton.Text);
            advanceButton.Text = StringManager.GetString(this.GetType().FullName,advanceButton.Text);

            saveToDefaultToolStripMenuItem.Text = StringManager.GetString(this.GetType().FullName,saveToDefaultToolStripMenuItem.Text);
            loadFromDefaultToolStripMenuItem.Text = StringManager.GetString(this.GetType().FullName,loadFromDefaultToolStripMenuItem.Text);
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
            LogHelper.Debug(LoggerType.Operation, "InitLightTypeList");

            int numLightType = LightSettings.Instance().NumLightType;

            int curLightType = lightTypeCombo.SelectedIndex;
            int curCompositeSrc1 = comboCompositeSrc1.SelectedIndex;
            int curCompositeSrc2 = comboCompositeSrc2.SelectedIndex;

            onValueUpdate = true;

            comboCompositeSrc1.Items.Clear();
            comboCompositeSrc2.Items.Clear();
            lightTypeCombo.Items.Clear();
            for (int i = 0; i < LightSettings.Instance().NumLightType; i++)
            {
                string lightTypeName = lightParamSet.LightParamList[i].Name;
                lightTypeCombo.Items.Add(lightTypeName);

                if (numLightType >= 3)
                {
                    comboCompositeSrc1.Items.Add(lightTypeName);
                    comboCompositeSrc2.Items.Add(lightTypeName);
                }
            }

            lightTypeCombo.SelectedIndex = MathHelper.Bound(curLightType, 0, lightTypeCombo.Items.Count);
            if (comboCompositeSrc1.Items.Count > 0)
                comboCompositeSrc1.SelectedIndex = MathHelper.Bound(curCompositeSrc1, 0, comboCompositeSrc1.Items.Count);
            if (comboCompositeSrc2.Items.Count > 0)
                comboCompositeSrc2.SelectedIndex = MathHelper.Bound(curCompositeSrc2, 0, comboCompositeSrc2.Items.Count);

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
            if (imageDeviceHandler.IsDepthScannerExist() == false)
            {
                labelExposure3dMs.Visible = false;
                labelExposure3D.Visible = false;
                exposureTime3dMs.Visible = false;
            }
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

        private void UpdateLightValues()
        {
            onValueUpdate = true;

            comboLightParamSource.SelectedIndex = (int)curTargetGroup.LightParamSource;

            if (lightParamSet.LightParamList.Count <= 0)
                return;

            int lightTypeIndex = lightTypeCombo.SelectedIndex;

            LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];
            paramTab.SelectedTab = paramTab.Tabs[(int)lightParam.LightParamType];

            if (lightParam.LightParamType == LightParamType.Value)
            {
                if (lightValueGrid.Rows.Count > 0)
                {
                    for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                        lightValueGrid.Rows[i].Cells[1].Value = lightParam.LightValue.Value[i];
                }

                exposureTimeMs.Value = lightParam.ExposureTimeUs / 1000;
                if (exposureTimeMs.Value == 0)
                    exposureTimeMs.Value = MachineSettings.Instance().DefaultExposureTimeMs;

                exposureTime3dMs.Value = lightParam.ExposureTime3dUs / 1000;
            }
            else
            {
                comboCompositeSrc1.SelectedIndex = lightParam.FirstImageIndex;
                comboCompositeSrc2.SelectedIndex = lightParam.SecondImageIndex;
                comboCompositeType.SelectedIndex = (int)lightParam.OperationType;
            }

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
            if (curTargetGroup == null)
                return;

            bool ok = ApplyLight();
            if (!ok)
            {
                string message = "Light param is invailed";
                MessageBox.Show(message);
                return;
            }

            UpdateLightTypeCombo(this.lightParamSet);

            LightParamSource lightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;
            curTargetGroup.LightParamSource = lightParamSource;

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

            LightValueChanged?.Invoke(true);
        }

        private void applyAllLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = StringManager.GetString(this.GetType().FullName, "Light param is invailed");
                MessageForm.Show(this.ParentForm, message);
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

            if (curTargetGroup == null)
                return false;

            try
            {
                int lightTypeIndex = lightTypeCombo.SelectedIndex;

                LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];
                lightParam.Name = lightTypeCombo.Text;
                lightParam.LightParamType = (LightParamType)paramTab.ActiveTab.Index;
                lightParam.LightStableTimeMs = Convert.ToInt32(lightStableTimeMs.Value);
                lightParam.ExposureTimeUs = Convert.ToInt32(exposureTimeMs.Value * 1000);
                lightParam.ExposureTime3dUs = Convert.ToInt32(exposureTime3dMs.Value * 1000);

                if (lightParam.LightParamType == LightParamType.Value)
                {
                    List<string> lightNameList = LightSettings.Instance().LightNameList;
                    for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                    {
                        lightParam.LightValue.Value[i] = Convert.ToInt32(lightValueGrid.Rows[i].Cells[1].Value);
                        lightNameList[i] = (string)lightValueGrid.Rows[i].Cells[0].Value;
                    }

                }
                else
                {
                    if (comboCompositeSrc1.SelectedIndex < 0
                        || comboCompositeSrc2.SelectedIndex < 0
                        || lightTypeIndex == lightParam.FirstImageIndex || lightTypeIndex == lightParam.SecondImageIndex)
                    {
                        return false;
                    }
                    lightParam.FirstImageIndex = comboCompositeSrc1.SelectedIndex;
                    lightParam.SecondImageIndex = comboCompositeSrc2.SelectedIndex;
                    lightParam.OperationType = (ImageOperationType)comboCompositeType.SelectedIndex;
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

        private void comboComposite_SelectedIndexChanged(object sender, EventArgs e)
        {
            int lightTypeIndex = lightTypeCombo.SelectedIndex;

            if (this.LightParamSet.LightParamList[lightTypeIndex].LightParamType == LightParamType.Composite)
            {
                ComboBox comboBox = (ComboBox)sender;
                if (comboBox.SelectedIndex == lightTypeCombo.SelectedIndex)
                {
                    string message = "Can not select itself.";
                    MessageBox.Show(message);
                    comboBox.SelectedIndex = -1;
                }
            }
        }

        private void advanceButton_Click(object sender, EventArgs e)
        {
            //Point pt = this.PointToScreen(new Point(advanceButton.Bounds.Left, advanceButton.Bounds.Bottom));
            //advancedContextMenuStrip.Show(pt);
            
            advancedContextMenuStrip.Show(Cursor.Position);
        }

        private void saveToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString(this.GetType().FullName, "Save parameters to default setting?");
            DialogResult res = MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo);
            if (res == DialogResult.Yes)
            {
                LightSettings.Instance().LightParamSet = LightParamSet.Clone();
                LightSettings.Instance().Save();
            }
        }

        private void loadFromDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString(this.GetType().FullName, "Load parameters from default setting?");
            DialogResult res = MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo);
            if (res == DialogResult.Yes)
            {
                LightParamSet = LightSettings.Instance().LightParamSet.Clone();
            }
        }

        public void ToggleVisible()
        {
            Visible = !Visible;
        }
    }
}
