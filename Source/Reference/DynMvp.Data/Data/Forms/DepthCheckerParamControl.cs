using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Data.Forms;

namespace DynMvp.Data.Forms
{
    public partial class DepthCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        DepthChecker depthChecker = null;
        VisionProbe selectedProbe = null;
        Calibration calibration = null;

        bool onValueUpdate = false;

        Image3D targetGroupImage = null;
        public Image3D TargetGroupImage
        {
            set
            {
                LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - Set Target Image");
                targetGroupImage = value;
            }
        }

        public DepthCheckerParamControl()
        {
            InitializeComponent();

            labelMeasureType.Text = StringManager.GetString(this.GetType().FullName,labelMeasureType.Text);
            labelValueRange.Text = StringManager.GetString(this.GetType().FullName,labelValueRange.Text);
            labelMinValue.Text = StringManager.GetString(this.GetType().FullName,labelMinValue.Text);
            labelMaxValue.Text = StringManager.GetString(this.GetType().FullName,labelMaxValue.Text);
            buttonGetDepthValue.Text = StringManager.GetString(this.GetType().FullName,buttonGetDepthValue.Text);
            //change language
        }

        public void SetCameraCalibration(Calibration calibration)
        {
            this.calibration = calibration;
        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
            depthChecker = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - SetSelectedProbe");

            selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == DepthChecker.TypeName)
            {
                depthChecker = (DepthChecker)selectedProbe.InspAlgorithm;
                UpdateData();
            }
            else
                throw new InvalidOperationException();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - UpdateData");

            onValueUpdate = true;

            lowerValue.Text = ((DepthCheckerParam)depthChecker.Param).LowerValue.ToString();
            upperValue.Text = ((DepthCheckerParam)depthChecker.Param).UpperValue.ToString();
            comboBoxMeasureType.SelectedIndex = (int)((DepthCheckerParam)depthChecker.Param).Type;
            UpdateValueUnit();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void lowerValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
                return;

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - lowerValue_ValueChanged");

            if (depthChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "DepthCheckerParamControl - depthChecker instance is null.");
                return;
            }

            AlgorithmParam newParam = depthChecker.Param.Clone();
            ((DepthCheckerParam)newParam).LowerValue = (float)lowerValue.Value;

            ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
        }

        private void upperValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
                return;

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - upperValue_ValueChanged");

            if (depthChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "DepthCheckerParamControl - depthChecker instance is null.");
                return;
            }

            AlgorithmParam newParam = depthChecker.Param.Clone();
            ((DepthCheckerParam)newParam).UpperValue = (float)upperValue.Value;

            ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == lowerValue)
                valueName = "Depth Lower";
            else if (sender == upperValue)
                valueName = "Depth Upper";

            UpDownControl.ShowControl(StringManager.GetString(this.GetType().FullName,valueName), (Control)sender);
        }

        private void comboBoxMeasureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
                return;

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - comboBoxValue_ValueChanged");

            if (depthChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "DepthCheckerParamControl - brightnessChecker instance is null.");
                return;
            }

            AlgorithmParam newParam = depthChecker.Param.Clone();
            ((DepthCheckerParam)newParam).Type = (DepthCheckType)comboBoxMeasureType.SelectedIndex;
            
            UpdateValueUnit();

            ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
        }

        void UpdateValueUnit()
        {
            switch ((DepthCheckType)comboBoxMeasureType.SelectedIndex)
            {
                case DepthCheckType.Volume:
                    labelMaxUnit.Text = "㎟";
                    labelMinUnit.Text = "㎟";
                    break;
                default:
                    labelMaxUnit.Text = "㎜";
                    labelMinUnit.Text = "㎜";
                    break;
            }
        }

        private void buttonGetDepthValue_Click(object sender, EventArgs e)
        {
            if (targetGroupImage == null)
            {
                MessageBox.Show("Please, scan image first.");
                return;
            }

            ImageD clipImage = targetGroupImage.ClipImage(Rectangle.Truncate(selectedProbe.BaseRegion.ToRectangleF()));
            float upperPct = (100 + (float)marginPercent.Value) / 100;
            float lowerPct = (100 - (float)marginPercent.Value) / 100;
            float averageHeight = clipImage.GetAverage();
            float area = (selectedProbe.BaseRegion.Width * 0.3f) * (selectedProbe.BaseRegion.Width * 0.3f);
            switch ((DepthCheckType)comboBoxMeasureType.SelectedIndex)
            {
                case DepthCheckType.Volume:
                    upperValue.Value = (decimal)(averageHeight * area * upperPct);
                    lowerValue.Value = (decimal)(averageHeight * area * lowerPct);
                    break;
                case DepthCheckType.HeightMax:
                    {
                        float maxValue = clipImage.GetMax();
                        upperValue.Value = (decimal)(maxValue * upperPct);
                        lowerValue.Value = (decimal)(maxValue * lowerPct);
                    }
                    break;
                case DepthCheckType.HeightMin:
                    {
                        float minValue = clipImage.GetMin();
                        upperValue.Value = (decimal)(minValue * upperPct);
                        lowerValue.Value = (decimal)(minValue * lowerPct);
                    }
                    break;
                case DepthCheckType.HeightAverage:
                    {
                        upperValue.Value = (decimal)(averageHeight * upperPct);
                        lowerValue.Value = (decimal)(averageHeight * lowerPct);
                    }
                    break;
                default:
                    break;
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return DepthChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD targetGroupImage)
        {
            if (targetGroupImage is Image3D)
                this.targetGroupImage = (Image3D)targetGroupImage;
        }
    }
}
