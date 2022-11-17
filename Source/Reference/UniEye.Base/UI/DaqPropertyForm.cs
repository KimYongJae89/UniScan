using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Devices.Daq;

namespace UniEye.Base.UI
{
    public partial class DaqPropertyForm : Form
    {
        DaqChannelProperty daqChannelProperty;
        public DaqChannelProperty DaqChannelProperty
        {
            get { return daqChannelProperty; }
            set { daqChannelProperty = value; }
        }

        double tempScaleFactor;
        double tempOffset;

        public DaqPropertyForm()
        {
            InitializeComponent();

            labelDaqName.Text = StringManager.GetString(this.GetType().FullName,labelDaqName.Text);
            labelDeviceName.Text = StringManager.GetString(this.GetType().FullName,labelDeviceName.Text);
            labelLeftDaqChannel.Text = StringManager.GetString(this.GetType().FullName,labelLeftDaqChannel.Text);
            labelMinValue.Text = StringManager.GetString(this.GetType().FullName,labelMinValue.Text);
            labelMaxValue.Text = StringManager.GetString(this.GetType().FullName,labelMaxValue.Text);
            labelSamplingHz.Text = StringManager.GetString(this.GetType().FullName,labelSamplingHz.Text);
            label1.Text = StringManager.GetString(this.GetType().FullName,label1.Text);
            buttonCalculateScale.Text = StringManager.GetString(this.GetType().FullName,buttonCalculateScale.Text);
            labelScaleFactor.Text = StringManager.GetString(this.GetType().FullName,labelScaleFactor.Text);
            checkUseCutomScaleFactor.Text = StringManager.GetString(this.GetType().FullName,checkUseCutomScaleFactor.Text);
            buttonUseThisScaleFactor.Text = StringManager.GetString(this.GetType().FullName,buttonUseThisScaleFactor.Text);
            labelValueOffset.Text = StringManager.GetString(this.GetType().FullName,labelValueOffset.Text);
            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void DaqPropertyForm_Load(object sender, EventArgs e)
        {
            txtName.Text = daqChannelProperty.Name;
            deviceName.Text = daqChannelProperty.DeviceName;
            daqChannelName.Text = daqChannelProperty.ChannelName;

            minValue.Value = (Decimal)daqChannelProperty.MinValue;
            maxValue.Value = (Decimal)daqChannelProperty.MaxValue;
            samplingHz.Value = (Decimal)daqChannelProperty.SamplingHz;
            scaleFactor.Text = daqChannelProperty.ScaleFactor.ToString();
            valueOffset.Text = daqChannelProperty.ValueOffset.ToString();
            resisterValue.Text = daqChannelProperty.ResisterValue.ToString();
            checkUseCutomScaleFactor.Checked = daqChannelProperty.UseCustomScale;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            daqChannelProperty.Name = txtName.Text;
            daqChannelProperty.DeviceName = deviceName.Text;
            daqChannelProperty.ChannelName = daqChannelName.Text;

            daqChannelProperty.MinValue = (double)minValue.Value;
            daqChannelProperty.MaxValue = (double)maxValue.Value;
            daqChannelProperty.SamplingHz = (double)samplingHz.Value;
            daqChannelProperty.ScaleFactor = Convert.ToDouble(scaleFactor.Text);
            daqChannelProperty.ValueOffset = Convert.ToDouble(valueOffset.Text);
            daqChannelProperty.ResisterValue = Convert.ToInt32(resisterValue.Text);
            daqChannelProperty.UseCustomScale = checkUseCutomScaleFactor.Checked;
        }

        private void buttonCalculateScale_Click(object sender, EventArgs e)
        {
            if (resisterValue.Text == "")
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Resister Value is empty. Please, input the value."));
                return;
            }

            double value = Convert.ToDouble(resisterValue.Text);

            double minValue = 0.004F * value;
            double maxValue = 0.02F * value;

            scaleFactor.Text = (70 / (maxValue - minValue)).ToString("0.00");
        }

        private void scaleFactor_TextChanged(object sender, EventArgs e)
        {
            daqChannelProperty.ScaleFactor = Convert.ToDouble(scaleFactor.Text);
        }

        private void buttonCalcScaleFactor_Click(object sender, EventArgs e)
        {
            CalcCustomScaleFactor();
        }

        private void CalcCustomScaleFactor()
        {
            if (string.IsNullOrEmpty(txtMinDistance.Text) || string.IsNullOrEmpty(txtMaxDistance.Text) || string.IsNullOrEmpty(txtMinVoltage.Text) || string.IsNullOrEmpty(txtMaxVoltage.Text))
                return;

            double distance = Convert.ToDouble(txtMinDistance.Text) - Convert.ToDouble(txtMaxDistance.Text);
            double voltage = Convert.ToDouble(txtMaxVoltage.Text) - Convert.ToDouble(txtMinVoltage.Text);

            double scaleFactor = voltage / distance; // use this
            tempScaleFactor = scaleFactor;

            double maxVoltage = Convert.ToDouble(txtMaxVoltage.Text) + Convert.ToDouble(txtMinVoltage.Text);
            double midVoltage = maxVoltage / 2;

            double midDistance = Convert.ToDouble(txtMaxDistance.Text) - Convert.ToDouble(txtMinDistance.Text);

            double offset = midVoltage + (scaleFactor * midDistance); // use this
            tempOffset = offset;
            this.buttonUseThisScaleFactor.Text = tempScaleFactor.ToString();
        }

        private void buttonUseThisScaleFactor_Click(object sender, EventArgs e)
        {
            scaleFactor.Text = tempScaleFactor.ToString();
            valueOffset.Text = tempOffset.ToString();
        }

        private void checkUseCutomScaleFactor_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
