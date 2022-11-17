using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Devices.Daq;
using DynMvp.Base;
using DynMvp.Data.Forms;

namespace UniEye.Base.UI.ParamControl
{
    public partial class DaqParamControl : UserControl, IAlgorithmParamControl
    {
        public ValueChangedDelegate ValueChanged = null;

        Model model;
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        private CommandManager commandManager;
        public CommandManager CommandManager
        {
            set { commandManager = value; }
        }

        DaqProbe selectedProbe;
        bool onValueUpdate = false;

        public DaqParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin DaqParamControl-Ctor");

            InitializeComponent();

            //change language
            labelDaqChannel.Text = StringManager.GetString(this.GetType().FullName, labelDaqChannel.Text);
            labelRange.Text = StringManager.GetString(this.GetType().FullName, labelRange.Text);
            labelNumSample.Text = StringManager.GetString(this.GetType().FullName, labelNumSample.Text);
            inverseResult.Text = StringManager.GetString(this.GetType().FullName, inverseResult.Text);
            modelVerification.Text = StringManager.GetString(this.GetType().FullName, modelVerification.Text);
            useLocalScaleFactor.Text = StringManager.GetString(this.GetType().FullName, useLocalScaleFactor.Text);
            labelFilterType.Text = StringManager.GetString(this.GetType().FullName, labelFilterType.Text);

            DaqChannelManager.Instance().FillComboDaqChannel(daqSelector);

            LogHelper.Debug(LoggerType.Operation, "End DaqParamControl-Ctor");
        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - SetSelectedProbe");

            selectedProbe = (DaqProbe)probe;
            if (selectedProbe != null)
                UpdateData();
            else
            {
                EnableControls(false);
            }
        }

        private void EnableControls(bool enable)
        {
            daqSelector.Enabled = enable;
            numSample.Enabled = enable;
            upperValue.Enabled = enable;
            lowerValue.Enabled = enable;
            inverseResult.Enabled = enable;
            modelVerification.Enabled = enable;
            useLocalScaleFactor.Enabled = enable;
            localScaleFactor.Enabled = enable && useLocalScaleFactor.Enabled;
            valueOffset.Enabled = enable && useLocalScaleFactor.Enabled;
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (selectedProbe.DaqChannel == null)
            {
                daqSelector.SelectedIndex = 0;
                return;
            }

            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - UpdateData");

            onValueUpdate = true;

            daqSelector.Text = selectedProbe.DaqChannel.Name;
            measureType.Text = selectedProbe.MeasureType.ToString();
           // filterType.Text = selectedProbe.FilterType.ToString();
            numSample.Text = selectedProbe.NumSample.ToString();
            upperValue.Text = selectedProbe.UpperValue.ToString();
            lowerValue.Text = selectedProbe.LowerValue.ToString();
            inverseResult.Checked = selectedProbe.InverseResult;
            modelVerification.Checked = selectedProbe.ModelVerification;
            useLocalScaleFactor.Checked = selectedProbe.UseLocalScaleFactor;
            localScaleFactor.Text = selectedProbe.LocalScaleFactor.ToString();
            localScaleFactor.Enabled = useLocalScaleFactor.Checked;
            valueOffset.Text = selectedProbe.ValueOffset.ToString();
            valueOffset.Enabled = useLocalScaleFactor.Checked;

            List<Target> targetList = new List<Target>();
            model.GetTargets(targetList);

            target1.Items.Clear();
            target2.Items.Clear();
            foreach (Target target in targetList)
            {
                string targetName = target.Name;
                if (targetName != null || targetName != "")
                {
                    target1.Items.Add(targetName);
                    target2.Items.Add(targetName);
                }
            }

            target1.Text = selectedProbe.Target1Name;
            target2.Text = selectedProbe.Target2Name;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation,"VisionParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, true);
            }
        }

        private void portSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProbe.DaqChannel = (DaqChannel)DaqChannelManager.Instance().GetDaqChannel((string)daqSelector.Text);
        }

        private void lowerValue_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - lowerValue_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                selectedProbe.LowerValue = (float)Convert.ToDouble(lowerValue.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void upperValue_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - upperValue_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                selectedProbe.UpperValue = (float)Convert.ToDouble(upperValue.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == numSample)
                valueName = StringManager.GetString(this.GetType().FullName, labelNumSample.Text);
            else if (sender == lowerValue)
                valueName = StringManager.GetString(this.GetType().FullName, "Normal Range Lower");
            else if (sender == upperValue)
                valueName = StringManager.GetString(this.GetType().FullName, "Normal Range Upper");
            else if (sender == localScaleFactor)
                valueName = StringManager.GetString(this.GetType().FullName, "Local Scale Factor");
            else if (sender == valueOffset)
                valueName = StringManager.GetString(this.GetType().FullName, "Value Offset");

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - inverseResult_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InverseResult = inverseResult.Checked;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void modelVerification_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - modelVerification_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.ModelVerification = modelVerification.Checked;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void numSample_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - numSample_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.NumSample = Convert.ToInt32(numSample.Text);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void useScaleValue_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - useScaleValue_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            localScaleFactor.Enabled = useLocalScaleFactor.Checked;
            valueOffset.Enabled = useLocalScaleFactor.Checked;

            selectedProbe.UseLocalScaleFactor = useLocalScaleFactor.Checked;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void localScaleFactor_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - scaleValue_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                selectedProbe.LocalScaleFactor = Convert.ToSingle(localScaleFactor.Text);
                ParamControl_ValueChanged(ValueChangedType.None);

            }
            catch (InvalidCastException)
            {

            }
            catch (FormatException)
            {

            }
        }

        private void labelValueOffset_Click(object sender, EventArgs e)
        {

        }

        private void valueOffset_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - scaleValue_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                selectedProbe.ValueOffset = Convert.ToSingle(valueOffset.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch (InvalidCastException)
            {

            }
            catch (FormatException)
            {

            }
        }

        private void measureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (measureType.SelectedIndex == 0)
            {
                panelProbeSelector.Hide();
                panelMeasureParam.Show();
            }
            else
            {
                panelMeasureParam.Hide();
                panelProbeSelector.Location = panelMeasureParam.Location;
                panelProbeSelector.Show();
            }

            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - measureType_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.MeasureType = (DaqMeasureType)Enum.Parse(typeof(DaqMeasureType), measureType.Text);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void target1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - target1_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Target1Name = target1.Text;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void target2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation,"DaqParamControl - target2_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "DaqParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Target2Name = target2.Text;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void filterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - filterType_SelectedIndexChanged");

           // selectedProbe.FilterType = (DaqFilterType)Enum.Parse(typeof(DaqFilterType), filterType.Text);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return "DAQ Probe";
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {

        }

        public void SetTargetGroupImage(ImageD image)
        {

        }
    }
}
