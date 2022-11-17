using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Data.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI.ParamControl;

namespace UniEye.Base.UI
{
    public partial class TargetParamControl : UserControl
    {
        public ValueChangedDelegate ValueChanged = null;

        bool onValueUpdate = false;

        Probe selectedProbe;

        private CommandManager commandManager;
        public CommandManager CommandManager
        {
            set
            {
                commandManager = value;
                this.visionParamControl.CommandManager = commandManager;
                this.daqParamControl.CommandManager = commandManager;
                this.markerParamControl.CommandManager = commandManager;
            }
        }

        PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            set
            {
                positionAligner = value;
                this.visionParamControl.PositionAligner = positionAligner;
            }
        }

        private TeachHandlerProbe teachHandlerProbe;
        public TeachHandlerProbe TeachHandlerProbe
        {
            set { teachHandlerProbe = value; }
        }

        private VisionParamControl visionParamControl;
        public VisionParamControl VisionParamControl
        {
            get { return visionParamControl; }
        }

        private UserControl selectedAlgorithmParamControl = null;
        public IAlgorithmParamControl SelectedAlgorithmParamControl
        {
            get { return (IAlgorithmParamControl)selectedAlgorithmParamControl; }
        }

        private ComputeParamControl computeParamControl;
        private DaqParamControl daqParamControl;
        private MarkerParamControl markerParamControl;
        private TensionChekcerParamControl tensionChekcerParamcontrol;
        
        private ImageD targetGroupImage;
        private int lightTypeIndex = 0;

        public TargetParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin TargetParamControl-Ctor");

            InitializeComponent();

            this.visionParamControl = new VisionParamControl();
            this.computeParamControl = new ComputeParamControl();
            this.daqParamControl = new DaqParamControl();
            this.markerParamControl = new MarkerParamControl();
            this.tensionChekcerParamcontrol = new TensionChekcerParamControl();
            this.SuspendLayout();

            this.visionParamControl.Name = "visionParamControl";
            this.visionParamControl.Location = new System.Drawing.Point(0, 0);
            this.visionParamControl.Size = new System.Drawing.Size(5, 5);
            this.visionParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionParamControl.TabIndex = 26;
            this.visionParamControl.Hide();
            this.visionParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);

            this.daqParamControl.Name = "daqParamControl";
            this.daqParamControl.Location = new System.Drawing.Point(0, 0);
            this.daqParamControl.Size = new System.Drawing.Size(5, 5);
            this.daqParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daqParamControl.TabIndex = 26;
            this.daqParamControl.Hide();
            this.daqParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);

            this.tensionChekcerParamcontrol.Name = "TensionParamControl";
            this.tensionChekcerParamcontrol.Location = new System.Drawing.Point(0, 0);
            this.tensionChekcerParamcontrol.Size = new System.Drawing.Size(5, 5);
            this.tensionChekcerParamcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tensionChekcerParamcontrol.TabIndex = 26;
            this.tensionChekcerParamcontrol.Hide();
            this.tensionChekcerParamcontrol.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);

            this.markerParamControl.Name = "markerParamControl";
            this.markerParamControl.Location = new System.Drawing.Point(0, 0);
            this.markerParamControl.Size = new System.Drawing.Size(5, 5);
            this.markerParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.markerParamControl.TabIndex = 26;
            this.markerParamControl.Hide();
            this.markerParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);

            this.ResumeLayout(false);
            this.PerformLayout();

            //change language
            labelProbeId.Text = StringManager.GetString(this.GetType().FullName, labelProbeId.Text);
            probeId.Text = StringManager.GetString(this.GetType().FullName, probeId.Text);
            labelProbeType.Text = StringManager.GetString(this.GetType().FullName, labelProbeType.Text);
            probeType.Text = StringManager.GetString(this.GetType().FullName, probeType.Text);
            labelProbe.Text = StringManager.GetString(this.GetType().FullName, labelProbe.Text);
            labelProbeName.Text = StringManager.GetString(this.GetType().FullName, labelProbeName.Text);
            buttonRefresh.Text = StringManager.GetString(this.GetType().FullName, buttonRefresh.Text);
            labelTargetName.Text = StringManager.GetString(this.GetType().FullName, labelTargetName.Text);
            UpdateProbeNameCombo();

            LogHelper.Debug(LoggerType.Operation, "End TargetParamControl-Ctor");
        }
        
        private void UpdateProbeNameCombo()
        {
            string[] probeNames = SystemManager.Instance().UiChanger?.GetProbeNames();

            if (probeNames != null)
            {
                foreach (string name in probeNames)
                    comboBoxProbeName.Items.Add(name);
            }
            else
            {
                probeNamePanel.Visible = false;
            }

            //if (OperationSettings.Instance().SystemType == SystemType.FPCBAligner)
            //{
            //    probeNamePanel.Visible = true;

            //    comboBoxProbeName.Items.Add("Fid1_Glass");
            //    comboBoxProbeName.Items.Add("Fid1_Fpcb");
            //    comboBoxProbeName.Items.Add("Fid2_Glass");
            //    comboBoxProbeName.Items.Add("Fid2_Fpcb");
            //}
            //else if (OperationSettings.Instance().SystemType == SystemType.DrugPackaging)
            //{
            //    probeNamePanel.Visible = true;

            //    comboBoxProbeName.Items.Add("BarcodeReader");
            //    comboBoxProbeName.Items.Add("OcrReader");
            //}
            //else if (OperationSettings.Instance().SystemType == SystemType.Calinar)
            //{
            //    probeNamePanel.Visible = true;

            //    comboBoxProbeName.Items.Add("Fiducial");
            //    comboBoxProbeName.Items.Add("Calibration");
            //    comboBoxProbeName.Items.Add("Offset_XY");
            //    comboBoxProbeName.Items.Add("Offset_Z");
            //}
            //else if (OperationSettings.Instance().SystemType == SystemType.ShampooBarcode)
            //{
            //    probeNamePanel.Visible = true;

            //    comboBoxProbeName.Items.Add("카메라1_앞면1");
            //    comboBoxProbeName.Items.Add("카메라1_앞면2");
            //    comboBoxProbeName.Items.Add("카메라1_뒷면1");
            //    comboBoxProbeName.Items.Add("카메라1_뒷면2");
            //    comboBoxProbeName.Items.Add("카메라2_앞면1");
            //    comboBoxProbeName.Items.Add("카메라2_앞면2");
            //    comboBoxProbeName.Items.Add("카메라2_뒷면1");
            //    comboBoxProbeName.Items.Add("카메라2_뒷면2");
            //}
            //else
            //{
            //    probeNamePanel.Visible = false;
            //}
        }

        public void UpdateTargetGroupImage(ImageD targetGroupImage, int lightTypeIndex)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - UpdateTargetImage(Bitmap targetImage)");

            this.targetGroupImage = targetGroupImage;
            this.lightTypeIndex = lightTypeIndex;
            //visionParamControl.TargetGroupImage = targetGroupImage;
            visionParamControl.UpdateTargetGroupImage(targetGroupImage);

            LogHelper.Debug(LoggerType.Operation, "End TargetParamControl - UpdateTargetImage(Bitmap targetImage)");
        }

        public void SelectObject(ITeachObject teachObject)
        {
            if (teachObject is Target)
            {
                Target target = (Target)teachObject;
                txtTargetName.Text = target.Name;

                UpdateTargetImage(target.Image);

                if (selectedAlgorithmParamControl != null)
                    selectedAlgorithmParamControl.Hide();

                SelectTarget(target);
             }
            else if (teachObject is Probe)
            {
                Probe probe = (Probe)teachObject;
                txtTargetName.Text = probe.Target.Name;

                UpdateTargetImage(probe.Target.Image);

                SelectProbe(probe);
                
                SelectTarget(probe.Target);
            }
            //txtLocationNumber.Text = txtTargetName.Text;
        }

        public void SelectProbe(Probe probe)
        {
            if (panelCommonParam.Visible == true)
            {
                probeFullId.Text = probe.FullId.ToString();
                probeId.Text = probe.Id.ToString();
                probeType.Text = StringManager.GetString(this.GetType().FullName, probe.GetProbeTypeDetailed());
            }

            if (panelMaskInspecter.Visible == true)
            {
                padId.Text = probe.Id.ToString();
                txtLocationNumber.Text = probe.Name;
                padType.Text = StringManager.GetString(this.GetType().FullName, probe.GetProbeTypeDetailed());
            }

            onValueUpdate = true;

            comboBoxProbeName.Text = probe.Name;
            txtTargetName.Enabled = true;

            onValueUpdate = false;

            ShowAlgorithmParamControl(probe);
        }

        public void SelectTarget(Target target)
        {
            txtTargetName.Text = target.Name;
//            txtLocationNumber.Text = "";

            UpdateTargetImage(target.Image);
        }

        private void UpdateTargetImage(ImageD image)
        { 
            if (targetPictureBox.Image != null)
                targetPictureBox.Image.Dispose();

            if (image != null)
            {
                targetPictureBox.Image = image.ToBitmap();
            }
            else
            {
                targetPictureBox.Image = Properties.Resources.Image; // 아무것도 등록이 안된 경우는 이 이미지를 넣어 준다
            }
        }

        bool fClearProbeData = false;
        public void ClearProbeData()
        {
            fClearProbeData = true;
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ClearProbeData");

            padId.Text = probeId.Text = "";
            padType.Text = probeType.Text = "";
            probeFullId.Text = "";

            selectedProbe = null;
            comboBoxProbeName.Text = "";

            targetPictureBox.Image = Properties.Resources.Image; // 아무것도 등록이 안된 경우는 이 이미지를 넣어 준다

            txtTargetName.Text = "";
            txtLocationNumber.Text = "";

            if (selectedAlgorithmParamControl != null)
            {
                selectedAlgorithmParamControl.Hide();
                ((IAlgorithmParamControl)selectedAlgorithmParamControl).ClearSelectedProbe();
                selectedAlgorithmParamControl = null;
            }

            fClearProbeData = false;
        }

        public void ClearTargetData()
        {

        }

        public void ShowAlgorithmParamControl(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ShowAlgorithmParamControl");

            UserControl preSelectedAlgorithmParamControl = selectedAlgorithmParamControl;

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    selectedAlgorithmParamControl = visionParamControl;                    
                    break;
                case ProbeType.Compute:
                    selectedAlgorithmParamControl = computeParamControl;            
                    break;
                case ProbeType.Daq:
                    selectedAlgorithmParamControl = daqParamControl;
                    daqParamControl.Model = SystemManager.Instance().CurrentModel;
                    break;
                case ProbeType.Marker:
                    selectedAlgorithmParamControl = markerParamControl;
                    break;
                case ProbeType.Tension:
                    selectedAlgorithmParamControl = tensionChekcerParamcontrol;
                    break;

                default:
                    throw new InvalidTypeException();
            }

            if (selectedAlgorithmParamControl != preSelectedAlgorithmParamControl)
            {
                if (preSelectedAlgorithmParamControl != null)
                {
                    preSelectedAlgorithmParamControl.Hide();
                    ((IAlgorithmParamControl)preSelectedAlgorithmParamControl).ClearSelectedProbe();
                }

                this.panelParam.Controls.Clear();
                this.panelParam.Controls.Add(selectedAlgorithmParamControl);

                selectedAlgorithmParamControl.Show();
            }

            ((IAlgorithmParamControl)selectedAlgorithmParamControl).AddSelectedProbe(probe);
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ParamControl_ValueChanged");

            if (onValueUpdate == false)
            {
                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);
            }
        }

        private void comboBoxProbeName_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            if (fClearProbeData == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - comboBoxProbeName_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "TargetParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Name = comboBoxProbeName.Text;

            ParamControl_ValueChanged(ValueChangedType.Position, true);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
            ((IAlgorithmParamControl)selectedAlgorithmParamControl)?.PointSelected(clickPos, ref processingCancelled);
        }

        private void comboBoxProbeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - comboBoxProbeName_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "TargetParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Name = comboBoxProbeName.Text;

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject != null)
            {
                List<Target> targetList = teachHandlerProbe.GetTargetList();

                ImageD targetImage = targetGroupImage.ClipImage(targetList[0].BaseRegion);

                teachObject.UpdateTargetImage((Image2D)targetImage, lightTypeIndex);

                if (targetPictureBox.Image != null)
                    targetPictureBox.Image.Dispose();

                targetPictureBox.Image = targetImage.ToBitmap();
            }
        }

        void ChangeTargetName(string targetName)
        {
            if (teachHandlerProbe == null)
                return;
            if (teachHandlerProbe.GetTargetList().Count != 1)
                return;
            if (String.IsNullOrEmpty(targetName))
                return;

            List<Target> targetList = teachHandlerProbe.GetTargetList();
            foreach (Target target in targetList)
            {
                target.Name = targetName;
            }

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void txtTargetName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ChangeTargetName(txtTargetName.Text);
        }

        private void txtPartName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ChangeTargetName(txtLocationNumber.Text);
        }

        private void txtPartName_TextChanged(object sender, EventArgs e)
        {
            ChangeTargetName(txtLocationNumber.Text);
        }
    }
}
