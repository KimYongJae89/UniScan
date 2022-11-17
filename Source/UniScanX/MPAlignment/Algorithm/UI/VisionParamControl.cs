using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Data;
using DynMvp.Base;
using DynMvp.Device;
using DynMvp.Device.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Vision.Cognex;
using DynMvp.Data.Forms;
using UniEye.Forms;
using System.Collections.Generic;

namespace UniAoi.A.UI.InspectParamControl
{
    public partial class VisionParamControl : UserControl, ProbeContainable
    {
        private VisionProbe selectedProbe;

        public ValueChangedDelegate ValueChanged = null;

        Target selectedTarget;
        public Target SelectedTarget
        {
            set
            {
                selectedTarget = value;
                UpdateData();
            }
        }

        Camera selectedCamera;
        public Camera SelectedCamera
        {
            get { return selectedCamera; }
            set { selectedCamera = value; }
        }

        Bitmap targetImage;
        public Bitmap TargetImage
        {
            set
            {
                targetImage = value;
                patternMatchingParamControl.TargetImage = targetImage;                
                simpleColorCheckerParamControl.TargetImage = targetImage;
            }
                

        }

        // 컨트롤의 값을 프로그램적으로 갱신하고 있는 동안, 부품 이미지의 갱신을 하지 않도록 하기 위해 사용하는 파라미터
        // 갑이 갱신될 때, 각 갱신 이벤트에 이미지 갱신을 하는 함수를 호출하고 있어, 이 플랙이 없을 경우 반복적으로 갱신이 수행됨.
        bool onValueUpdate = false;

        private UserControl selectedAlgorithmParamControl = null;
        private PatternMatchingParamControl patternMatchingParamControl;
        private BinaryCounterParamControl binaryCounterParamControl;
        private BrightnessCheckerParamControl brightnessCheckerParamControl;
        private SimpleColorCheckerParamControl simpleColorCheckerParamControl;
        private UniAoi.A.UI.InspectParamControl.BlobCheckerParamControl blobCheckerParamControl;
        //private NNBoltCheckerControl boltCheckerControl;
        //private NNBoltSegmentationControl boltSegmentationControl;

        //private BarcodeReaderParamControl 

        public VisionParamControl()
        {
            InitializeComponent();

            this.patternMatchingParamControl = new PatternMatchingParamControl();
            this.binaryCounterParamControl = new BinaryCounterParamControl();
            this.brightnessCheckerParamControl = new BrightnessCheckerParamControl();
            this.simpleColorCheckerParamControl = new SimpleColorCheckerParamControl();
            this.blobCheckerParamControl = new UniAoi.A.UI.InspectParamControl.BlobCheckerParamControl();
            this.SuspendLayout();

            this.patternMatchingParamControl.Name = "patternMatchingParamControl";
            this.patternMatchingParamControl.Location = new System.Drawing.Point(0, 0);
            this.patternMatchingParamControl.Size = new System.Drawing.Size(10, 10);
            this.patternMatchingParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patternMatchingParamControl.TabIndex = 26;
            this.patternMatchingParamControl.Hide();
            this.patternMatchingParamControl.ValueChanged = new ValueChangedDelegate(VisionParamControl_ValueChanged);

            this.binaryCounterParamControl.Name = "binaryCounterParamControl";
            this.binaryCounterParamControl.Location = new System.Drawing.Point(0, 0);
            this.binaryCounterParamControl.Size = new System.Drawing.Size(10, 10);
            this.binaryCounterParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binaryCounterParamControl.TabIndex = 26;
            this.binaryCounterParamControl.Hide();
            this.binaryCounterParamControl.ValueChanged = new ValueChangedDelegate(VisionParamControl_ValueChanged);

            this.brightnessCheckerParamControl.Name = "brightnessCheckerParamControl";
            this.brightnessCheckerParamControl.Location = new System.Drawing.Point(0, 0);
            this.brightnessCheckerParamControl.Size = new System.Drawing.Size(10, 10);
            this.brightnessCheckerParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brightnessCheckerParamControl.TabIndex = 26;
            this.brightnessCheckerParamControl.Hide();
            this.brightnessCheckerParamControl.ValueChanged = new ValueChangedDelegate(VisionParamControl_ValueChanged);

            this.simpleColorCheckerParamControl.Name = "simpleColorCheckerParamControl";
            this.simpleColorCheckerParamControl.Location = new System.Drawing.Point(0, 0);
            this.simpleColorCheckerParamControl.Size = new System.Drawing.Size(10, 10);
            this.simpleColorCheckerParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleColorCheckerParamControl.TabIndex = 26;
            this.simpleColorCheckerParamControl.Hide();
            this.simpleColorCheckerParamControl.ValueChanged = new ValueChangedDelegate(VisionParamControl_ValueChanged);


            //this.patternMatchingParamControl.Show();

            this.ResumeLayout(false);
            this.PerformLayout();

            //change language
            labelPos.Text = StringManager.GetString(labelPos.Text);
            useHistogramEqualization.Text = StringManager.GetString(useHistogramEqualization.Text);
            useBinarization.Text = StringManager.GetString(useBinarization.Text);
            radioAutoThreshold.Text = StringManager.GetString(radioAutoThreshold.Text);
            useEdgeExtraction.Text = StringManager.GetString(useEdgeExtraction.Text);
            inverseResult.Text = StringManager.GetString(inverseResult.Text);
            modelVerification.Text = StringManager.GetString(modelVerification.Text);
            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);
            stepBlocker.Text = StringManager.GetString(stepBlocker.Text);
            radioSingleThreshold.Text = StringManager.GetString(radioSingleThreshold.Text);
            radioDoubleThreshold.Text = StringManager.GetString(radioDoubleThreshold.Text);

            //ShowAlgorithmParamControl(PatternMatching.TypeName);
        }

        public void SetSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - SetSelectedProbe");

            selectedProbe = (VisionProbe)probe;

            if (selectedProbe != null)
            {
                probeId.Text = probe.Id.ToString();
                probeType.Text = probe.GetProbeTypeDetailed();

                UpdateData();
            }
            else
            {
               EnableControls(false);
            }
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (selectedProbe == null)
                return;

            EnableControls(true);

            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - UpdateData");

            onValueUpdate = true;

            probePosX.Maximum = 5000; // selectedCamera.GetFrameSize().Width;
            probePosX.Value = (int)selectedProbe.Region.X;

            probePosY.Maximum = 5000; //  selectedCamera.GetFrameSize().Height;
            probePosY.Value = (int)selectedProbe.Region.Y;

            probeWidth.Maximum = 2000;
            probeWidth.Value = (int)selectedProbe.Region.Width;

            probeHeight.Maximum = 2000;
            probeHeight.Value = (int)selectedProbe.Region.Height;

            probeAngle.Value = (int)selectedProbe.Region.Angle;

            Algorithm inspAlgorithm = selectedProbe.InspAlgorithm;
            useHistogramEqualization.Checked = inspAlgorithm.UseHistogramEqualization;
            useBinarization.Checked = inspAlgorithm.UseBinarization;
            thresholdLower.Value = inspAlgorithm.ThresholdLower;
            thresholdUpper.Value = inspAlgorithm.ThresholdUpper;
            useEdgeExtraction.Checked = inspAlgorithm.UseEdgeExtraction;
            numErode.Value = inspAlgorithm.NumErode;
            numDilate.Value = inspAlgorithm.NumDilate;

            inverseResult.Checked = selectedProbe.InverseResult;
            modelVerification.Checked = selectedProbe.ModelVerification;
            stepBlocker.Checked = selectedProbe.StepBlocker;
            stepBlockType.Text = selectedProbe.StepBlockerType.ToString();

            switch (inspAlgorithm.BinarizationType)
            {
                case BinarizationType.SingleThreshold:
                    radioSingleThreshold.Checked = true;
                    break;
                case BinarizationType.AutoThreshold:
                    radioAutoThreshold.Checked = true;
                    break;
                case BinarizationType.DoubleThreshold:
                    radioDoubleThreshold.Checked = true;
                    break;
            }

            imageBand.Text = inspAlgorithm.ImageBand.ToString();

            if (inspAlgorithm.SourceImageType == ImageType.Grey)
            {
                imageBand.SelectedIndex = 0;
                imageBand.Enabled = true;
                EnableBinarizationControls(true);
            }
            else
            {
                if (inspAlgorithm.IsColorAlgorithm)
                {
                    imageBand.SelectedIndex = 0;
                    imageBand.Enabled = true;
                    EnableBinarizationControls(false);
                }
                else
                {
                    imageBand.Text = inspAlgorithm.ImageBand.ToString();
                    imageBand.Enabled = true;
                    EnableBinarizationControls(true);
                }
            }

            ShowAlgorithmParamControl(selectedProbe.InspAlgorithm.GetAlgorithmType());
            
            onValueUpdate = false;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing, false);
        }

        private void EnableControls(bool enable)
        {
            probePosX.Enabled = enable;
            probePosY.Enabled = enable;
            probeWidth.Enabled = enable;
            probeHeight.Enabled = enable;
            useHistogramEqualization.Enabled = enable;
            useEdgeExtraction.Enabled = enable;
            inverseResult.Enabled = enable;
            modelVerification.Enabled = enable;
            stepBlocker.Enabled = enable;

            if (enable == false)
            {
                useBinarization.Enabled = false;
                radioSingleThreshold.Enabled = false;
                radioAutoThreshold.Enabled = false;
                radioDoubleThreshold.Enabled = false;
                numErode.Enabled = false;
                numDilate.Enabled = false;
                thresholdLower.Enabled = false;
                thresholdUpper.Enabled = false;

                if (selectedAlgorithmParamControl != null)
                    selectedAlgorithmParamControl.Hide();
            }
        }

        private void EnableBinarizationControls(bool enable)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - EnableBinarizationControls");

            useBinarization.Enabled = enable;

            enable &= selectedProbe.InspAlgorithm.UseBinarization;
            radioSingleThreshold.Enabled = enable;
            radioAutoThreshold.Enabled = enable;
            radioDoubleThreshold.Enabled = enable;
            numErode.Enabled = enable;
            numDilate.Enabled = enable;

            switch (selectedProbe.InspAlgorithm.BinarizationType)
            {
                case BinarizationType.AutoThreshold:
                    thresholdLower.Enabled = false;
                    thresholdUpper.Enabled = false;
                    break;
                case BinarizationType.DoubleThreshold:
                    thresholdLower.Enabled = enable;
                    thresholdUpper.Enabled = enable;
                    break;
                case BinarizationType.SingleThreshold:
                    thresholdLower.Enabled = enable;
                    thresholdUpper.Enabled = false;
                    break;
            }
        }

        private void ShowAlgorithmParamControl(string algorithmType)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - ShowAlgorithmParamControl");

            if (selectedAlgorithmParamControl != null)
                selectedAlgorithmParamControl.Hide();

            if (algorithmType == PatternMatching.TypeName)
                selectedAlgorithmParamControl = patternMatchingParamControl;
            else if (algorithmType == BinaryCounter.TypeName)
                selectedAlgorithmParamControl = binaryCounterParamControl;
            else if (algorithmType == BrightnessChecker.TypeName)
                selectedAlgorithmParamControl = brightnessCheckerParamControl;
            else if (algorithmType == SimpleColorChecker.TypeName)
                selectedAlgorithmParamControl = simpleColorCheckerParamControl;
            else if (algorithmType == BlobChecker.TypeName)
                selectedAlgorithmParamControl = blobCheckerParamControl;
            else
                throw new InvalidTypeException();

            this.algorithmParamPanel.Controls.Clear();
            this.algorithmParamPanel.Controls.Add(selectedAlgorithmParamControl);

            selectedAlgorithmParamControl.Dock = DockStyle.Fill;
            selectedAlgorithmParamControl.Show();
            if (selectedProbe != null)
            {
                ((ProbeContainable)selectedAlgorithmParamControl).SetSelectedProbe(selectedProbe);
            }
        }

        public void VisionParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified = true)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);
            }
        }

        private void useHistogramEqualization_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - useHistogramEqualization_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.UseHistogramEqualization = useHistogramEqualization.Checked;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void useBinarization_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - useBinarization_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.UseBinarization = useBinarization.Checked;
            
            EnableBinarizationControls(true);

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void radioSingleThreshold_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - radioSingleThreshold_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.BinarizationType = BinarizationType.SingleThreshold;

            EnableBinarizationControls(true);
            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void radioDoubleThreshold_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - radioDoubleThreshold_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.BinarizationType = BinarizationType.DoubleThreshold;
            if (thresholdLower.Value > thresholdUpper.Value)
            {
                thresholdUpper.Value = thresholdLower.Value;
                selectedProbe.InspAlgorithm.ThresholdUpper = (int)thresholdUpper.Value;
            }

            EnableBinarizationControls(true);
            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void useEdgeExtraction_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - useEdgeExtraction_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.UseEdgeExtraction = useEdgeExtraction.Checked;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - inverseResult_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InverseResult = inverseResult.Checked;

            VisionParamControl_ValueChanged(ValueChangedType.None);
       }

        private void modelVerification_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - modelVerification_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.ModelVerification = modelVerification.Checked;

            VisionParamControl_ValueChanged(ValueChangedType.None);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == probePosX)
                valueName = StringManager.GetString("Position X");
            else if (sender == probePosY)
                valueName = StringManager.GetString("Position Y");
            else if (sender == probeWidth)
                valueName = StringManager.GetString("Width");
            else if (sender == probeHeight)
                valueName = StringManager.GetString("Height");
            else if (sender == thresholdLower)
                valueName = StringManager.GetString("Threshold Lower");
            else if (sender == thresholdUpper)
                valueName = StringManager.GetString("Threshold Upper");

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void colorBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - colorBand_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), imageBand.Text);

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void thresholdLower_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - thresholdLower_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.ThresholdLower = (int)thresholdLower.Value;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void thresholdUpper_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug,"VisionParamControl - thresholdUpper_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.ThresholdUpper = (int)thresholdUpper.Value;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void radioAutoThreshold_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - radioAutoThreshold_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.BinarizationType = BinarizationType.AutoThreshold;

            EnableBinarizationControls(true);

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void probePosX_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - probePosX_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.X = (int)probePosX.Value;
            VisionParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void probePosY_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - probePosY_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Y = (int)probePosY.Value;
            VisionParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void probeWidth_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - probeWidth_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Width = (int)probeWidth.Value;
            VisionParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void probeHeight_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - probeHeight_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Height = (int)probeHeight.Value;
            VisionParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void numErode_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - numErode_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.NumErode = (int)numErode.Value;
            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void numDilate_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - numDilate_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InspAlgorithm.NumDilate = (int)numDilate.Value;
            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }

        private void stepBlocker_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - stepBlocker_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.StepBlocker = stepBlocker.Checked;
            //selectedProbe.StepBlockerType = (StepBlockerType)stepBlockType.SelectedIndex;
            VisionParamControl_ValueChanged(ValueChangedType.None);
        }

        private void stepBlockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - stepBlocker_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }
            selectedProbe.StepBlockerType = (StepBlockerType)stepBlockType.SelectedIndex;
            VisionParamControl_ValueChanged(ValueChangedType.None);
        }

        private void probeAngle_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - probeAngle_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Angle = (int)probeAngle.Value;
            VisionParamControl_ValueChanged(ValueChangedType.Position);
            UpdateData();
        }
    }
}
