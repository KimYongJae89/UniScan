using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.Comm;
using DynMvp.Base;
using DynMvp.Device.Serial;

namespace DynMvp.Devices.UI
{
    public partial class LightConfigForm : Form
    {
        DigitalIoHandler digitalIoHandler;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
            set { digitalIoHandler = value; }
        }

        LightCtrlInfo lightCtrlInfo;
        public LightCtrlInfo LightCtrlInfo
        {
            get { return lightCtrlInfo; }
            set { lightCtrlInfo = value; }
        }

        string lightCtrlName;
        public string LightCtrlName
        {
            get { return lightCtrlName; }
            set { lightCtrlName = value; }
        }

        //SerialPortInfo serialPortInfo = new SerialPortInfo();

        bool initialized = false;

        public LightConfigForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName, labelName.Text);
            labelNumLight.Text = StringManager.GetString(this.GetType().FullName, labelNumLight.Text);
            useIoLightCtrl.Text = StringManager.GetString(this.GetType().FullName, useIoLightCtrl.Text);
            useSerialLightCtrl.Text = StringManager.GetString(this.GetType().FullName, useSerialLightCtrl.Text);
            buttonEditLightCtrlPort.Text = StringManager.GetString(this.GetType().FullName, buttonEditLightCtrlPort.Text);
            buttonTestLightController.Text = StringManager.GetString(this.GetType().FullName, buttonTestLightController.Text);

            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;

            labelSerialPortInfo.Text = "";
            lightCtrlName = "LightController";

            comboLightControllerVender.DataSource = Enum.GetValues(typeof(LightControllerVender));
        }

        private void LightConfigForm_Load(object sender, EventArgs e)
        {
            if (this.lightCtrlInfo != null)
            {
                UpdateInfo();
            }
            else
            {
                txtName.Text = lightCtrlName;
                numLight.Value = 1;
                useIoLightCtrl.Checked = true;
            }

            initialized = true;
        }

        private void UpdateInfo()
        { 
            if (this.lightCtrlInfo.ControllerType == LightCtrlType.Serial)
            {
                SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)lightCtrlInfo;

                useSerialLightCtrl.Checked = true;
                checkResponceTimeout.Value = serialLightCtrlInfo.ResponceTimeoutMs;

                comboLightControllerVender.Enabled = true;
                buttonEditLightCtrlPort.Enabled = true;

                comboLightControllerVender.SelectedItem = serialLightCtrlInfo.ControllerVender;

                //serialPortInfo = serialLightCtrlInfo.SerialPortInfo;
                //labelSerialPortInfo.Text = serialPortInfo.ToString();
                labelSerialPortInfo.Text = serialLightCtrlInfo.SerialPortInfo.ToString();
            }
            else if (this.lightCtrlInfo.ControllerType == LightCtrlType.IO)
            {
                useIoLightCtrl.Checked = true;
            }

            txtName.Text = this.lightCtrlInfo.Name;
            numLight.Value = this.lightCtrlInfo.NumChannel;
        }

        private void useIoLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
                return;

            this.lightCtrlInfo = CreateLightCtrlInfo();

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;
            checkResponceTimeout.Enabled = false;

            UpdateInfo();
        }

        private void buttonEditLightCtrlPort_Click(object sender, EventArgs e)
        {
            SerialLightCtrlInfo lightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;

            SerialPortSettingForm form = new SerialPortSettingForm()
            {
                SerialDeviceInfo = (ISerialDeviceInfo)lightCtrlInfo.Clone(),                
                EnablePortNo = true
            };
            form.EditableDevice(false);

            if (form.ShowDialog() == DialogResult.OK)
            {
                lightCtrlInfo.SerialPortInfo.CopyFrom(form.SerialDeviceInfo.SerialPortInfo);
                labelSerialPortInfo.Text = lightCtrlInfo.SerialPortInfo.ToString();
            }
        }

        private LightCtrlInfo CreateLightCtrlInfo()
        {
            LightCtrlInfo lightCtrlInfo = null;
            if (useIoLightCtrl.Checked)
            {
                lightCtrlInfo = LightCtrlInfoFactory.Create(LightCtrlType.IO, LightControllerVender.Unknown);
            }
            else if (useSerialLightCtrl.Checked)
            {
                LightControllerVender controllerVender = (LightControllerVender)comboLightControllerVender.SelectedItem;
                SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)LightCtrlInfoFactory.Create(LightCtrlType.Serial, controllerVender);
                serialLightCtrlInfo.ResponceTimeoutMs = (int)this.checkResponceTimeout.Value;
                lightCtrlInfo = serialLightCtrlInfo;
            }

            if (lightCtrlInfo != null)
            {
                lightCtrlInfo.Name = txtName.Text;
                lightCtrlInfo.NumChannel = (int)numLight.Value;
            }

            return lightCtrlInfo;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            //lightCtrlInfo = CreateLightCtrlInfo();
        }

        private void useSerialLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
                return;

            this.lightCtrlInfo = CreateLightCtrlInfo();

            buttonEditLightCtrlPort.Enabled = true;
            comboLightControllerVender.Enabled = true;
            this.checkResponceTimeout.Enabled = true;

            UpdateInfo();
        }

        private void buttonTestLightController_Click(object sender, EventArgs e)
        {
            LightCtrl lightCtrl = LightCtrlFactory.Create(this.lightCtrlInfo, digitalIoHandler, false);
            if (lightCtrl == null)
                return;

            LightValue lightValue = new LightValue(this.lightCtrlInfo.NumChannel, lightCtrl.GetMaxLightLevel());
            lightCtrl.TurnOn(lightValue);

            MessageBox.Show("Please, check the light");

            lightCtrl.TurnOff();

            lightCtrl.Release();
        }

        private void buttonAdvance_Click(object sender, EventArgs e)
        {
            Form advForm = this.lightCtrlInfo.GetAdvancedConfigForm();
            if (advForm == null)
                return;

            advForm.ShowDialog();
        }

        private void comboLightControllerVender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initialized)
                return;

            this.lightCtrlInfo = CreateLightCtrlInfo();
            UpdateInfo();
        }

        private void numLight_ValueChanged(object sender, EventArgs e)
        {
            if (lightCtrlInfo == null) return;
            this.lightCtrlInfo.NumChannel = (int)this.numLight.Value;
        }

        private void checkResponceTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (lightCtrlInfo == null) return;
            ((SerialLightCtrlInfo)this.lightCtrlInfo).ResponceTimeoutMs = (int)this.checkResponceTimeout.Value;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (lightCtrlInfo == null) return;
            this.lightCtrlInfo.Name = txtName.Text;
        }
    }
}
