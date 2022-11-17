using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using System.Threading;

namespace DynMvp.Devices.UI
{
    public partial class SerialPortSettingForm : Form
    {
        private ISerialDeviceInfo serialDeviceInfo;
        public ISerialDeviceInfo SerialDeviceInfo
        {
            get { return serialDeviceInfo; }
            set { serialDeviceInfo = value; }
        }

        bool enablePortNo = false;
        public bool EnablePortNo
        {
            get { return enablePortNo; }
            set { enablePortNo = value; }
        }

        string portName = "";
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        
        public SerialPortSettingForm()
        {
            InitializeComponent();

            comboDeviceType.DataSource = Enum.GetValues(typeof(ESerialDeviceType));
            comboSensorType.DataSource = Enum.GetValues(typeof(ESerialSensorType));
            
            groupBoxProperty.Text = StringManager.GetString(this.GetType().FullName,groupBoxProperty.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
            labelBaudRate.Text = StringManager.GetString(this.GetType().FullName,labelBaudRate.Text);
            labelDataBits.Text = StringManager.GetString(this.GetType().FullName,labelDataBits.Text);
            labelParity.Text = StringManager.GetString(this.GetType().FullName,labelParity.Text);
            labelStopBits.Text = StringManager.GetString(this.GetType().FullName,labelStopBits.Text);
            labelHandshake.Text = StringManager.GetString(this.GetType().FullName,labelHandshake.Text);
            checkRtsEnable.Text = StringManager.GetString(this.GetType().FullName,checkRtsEnable.Text);
            checkDtrEnable.Text = StringManager.GetString(this.GetType().FullName,checkDtrEnable.Text);
            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);
        }

        private void SerialPortSettingForm_Load(object sender, EventArgs e)
        {
            if (serialDeviceInfo == null)
            {
                this.groupBoxProperty.Visible = false;
                return;
            }

            textBoxName.Text = serialDeviceInfo.DeviceName;

            comboDeviceType.SelectedItem = serialDeviceInfo.DeviceType;
            comboSensorType.Visible = (serialDeviceInfo is SerialSensorInfo);
            if (serialDeviceInfo is SerialSensorInfo)
                comboSensorType.SelectedItem = ((SerialSensorInfo)serialDeviceInfo).SensorType;
            comboDeviceType.Enabled = comboSensorType.Enabled = false;

            SerialPortManager.FillComboAllPort(comboPortName);
            if (string.IsNullOrEmpty(PortName))
                comboPortName.Text = serialDeviceInfo.SerialPortInfo.PortName;
            else
            {
                comboPortName.Text = portName;
                serialDeviceInfo.SerialPortInfo.PortName = comboPortName.Text;
            }

            //comboPortName.Enabled = enablePortNo;

            comboBaudRate.Text = serialDeviceInfo.SerialPortInfo.BaudRate.ToString();
            comboParity.Text = serialDeviceInfo.SerialPortInfo.Parity.ToString();

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            comboStopBits.SelectedIndex = stopBitsValues.ToList().IndexOf(serialDeviceInfo.SerialPortInfo.StopBits);

            comboDataBits.Text = serialDeviceInfo.SerialPortInfo.DataBits.ToString();

            Handshake[] handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            comboHandshake.SelectedIndex = handShakes.ToList().IndexOf(serialDeviceInfo.SerialPortInfo.Handshake);

            checkRtsEnable.Checked = serialDeviceInfo.SerialPortInfo.RtsEnable;
            checkDtrEnable.Checked = serialDeviceInfo.SerialPortInfo.DtrEnable;
        }

        public void EditableDevice(bool editable)
        {
            panel2.Visible = editable;
        }

        private void comboDeviceType_SelectedValueChanged(object sender, EventArgs e)
        {
            ESerialDeviceType serialDeviceType = (ESerialDeviceType)comboDeviceType.SelectedItem;
            comboSensorType.Visible = (serialDeviceType == ESerialDeviceType.SerialSensor);

            InitDeviceName();
        }

        private void comboSensorType_SelectedValueChanged(object sender, EventArgs e)
        {
            InitDeviceName();
        }

        private void InitDeviceName()
        {
            ESerialDeviceType serialDeviceType = (ESerialDeviceType)comboDeviceType.SelectedItem;
            string deviceName = serialDeviceType.ToString();
            if (serialDeviceType == ESerialDeviceType.SerialSensor)
            {
                ESerialSensorType serialSensorType = (ESerialSensorType)comboSensorType.SelectedItem;
                deviceName = serialSensorType.ToString();
            }
            textBoxName.Text = deviceName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (serialDeviceInfo == null)
            {
                ESerialDeviceType serialDeviceType = (ESerialDeviceType)comboDeviceType.SelectedItem;
                ESerialSensorType serialSensorType = (ESerialSensorType)comboSensorType.SelectedItem;

                this.serialDeviceInfo = SerialDeviceInfoFactory.CreateSerialDeviceInfo(serialDeviceType, serialSensorType);
                serialDeviceInfo.DeviceName = textBoxName.Text;
            }
            else
            {
                serialDeviceInfo.DeviceName = textBoxName.Text;
                ApplyPortInfoData(serialDeviceInfo.SerialPortInfo);
                
            }
            Close();
        }

        private void ApplyPortInfoData(SerialPortInfo serialPortInfo)
        {
            serialDeviceInfo.SerialPortInfo.PortName = comboPortName.Text;
            serialDeviceInfo.SerialPortInfo.BaudRate = Convert.ToInt32(comboBaudRate.Text);
            serialDeviceInfo.SerialPortInfo.Parity = (Parity)Enum.Parse(typeof(Parity), comboParity.Text);
            serialDeviceInfo.SerialPortInfo.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboStopBits.Text);
            serialDeviceInfo.SerialPortInfo.DataBits = Convert.ToInt32(comboDataBits.Text);

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            serialDeviceInfo.SerialPortInfo.StopBits = stopBitsValues[comboStopBits.SelectedIndex];

            Handshake[] handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            serialDeviceInfo.SerialPortInfo.Handshake = handShakes[comboHandshake.SelectedIndex];

            serialDeviceInfo.SerialPortInfo.RtsEnable = checkRtsEnable.Checked;
            serialDeviceInfo.SerialPortInfo.DtrEnable = checkDtrEnable.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFindPort_Click(object sender, EventArgs e)
        {
            ApplyPortInfoData(this.SerialDeviceInfo.SerialPortInfo);

            List<string> comPortList = new List<string>();
            foreach (string portName in comboPortName.Items)
                comPortList.Add(portName);
            comPortList.Remove("Virtual");

            List<int> baudRateList = new List<int>();
            foreach (string baudRate in comboBaudRate.Items)
                baudRateList.Add(int.Parse(baudRate));

            SerialPortFindForm form = new SerialPortFindForm(this.SerialDeviceInfo, comPortList.ToArray(), baudRateList.ToArray());
            if(form.ShowDialog(this) == DialogResult.OK)
            {
                this.comboPortName.Text = form.SelectedPort;
                this.comboBaudRate.Text = form.SelectedBaudRate.ToString();
            }
        }
    }
}
