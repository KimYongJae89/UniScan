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
    public partial class BrightnessCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        List<BrightnessChecker> brightnessCheckerList = new List<BrightnessChecker>();

        bool onValueUpdate = false;


        public BrightnessCheckerParamControl()
        {
            InitializeComponent();

            //change language
            labelBrightnessRange.Text = StringManager.GetString(this.GetType().FullName,labelBrightnessRange.Text);
        }

        public void ClearSelectedProbe()
        {
            brightnessCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BrightnessCheckerParamControl - SetSelectedProbe");

            VisionProbe selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == BrightnessChecker.TypeName)
            {
                brightnessCheckerList.Add((BrightnessChecker)selectedProbe.InspAlgorithm);
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
            if (brightnessCheckerList.Count == 0)
                return;

            LogHelper.Debug(LoggerType.Operation, "BrightnessCheckerParamControl - UpdateData");

            onValueUpdate = true;

            BrightnessChecker brightnessChecker = brightnessCheckerList[0];

            lowerValue.Value = ((BrightnessCheckerParam)brightnessChecker.Param).LowerValue;
            upperValue.Value = ((BrightnessCheckerParam)brightnessChecker.Param).UpperValue;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void lowerValue_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BrightnessCheckerParamControl - lowerValue_ValueChanged");

            if (brightnessCheckerList.Count == 0)
                return;

            foreach (BrightnessChecker brightnessChecker in brightnessCheckerList)
            {
                AlgorithmParam newParam = brightnessChecker.Param.Clone();
                ((BrightnessCheckerParam)newParam).LowerValue = (int)lowerValue.Value;

                ParamControl_ValueChanged(ValueChangedType.None, brightnessChecker, newParam);
            }
        }

        private void upperValue_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BrightnessCheckerParamControl - upperValue_ValueChanged");

            if (brightnessCheckerList.Count == 0)
                return;

            foreach (BrightnessChecker brightnessChecker in brightnessCheckerList)
            {
                AlgorithmParam newParam = brightnessChecker.Param.Clone();
                ((BrightnessCheckerParam)newParam).UpperValue = (int)upperValue.Value;

                ParamControl_ValueChanged(ValueChangedType.None, brightnessChecker, newParam);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == lowerValue)
                valueName = "Brightness Lower";
            else if (sender == upperValue)
                valueName = "Brightness Upper";

            UpDownControl.ShowControl(StringManager.GetString(this.GetType().FullName,valueName), (Control)sender);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BrightnessChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD targetGroupImage)
        {

        }
    }
}
