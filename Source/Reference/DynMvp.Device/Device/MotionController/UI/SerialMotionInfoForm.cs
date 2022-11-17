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
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Comm;
using DynMvp.Base;

namespace DynMvp.Devices.UI
{
    public partial class SerialMotionInfoForm : Form
    {
        private SerialMotionInfo serialMotionInfo;
        public SerialMotionInfo SerialMotionInfo
        {
            get { return serialMotionInfo; }
            set { serialMotionInfo = value; }
        }

        public SerialMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            groupBoxProperty.Text = StringManager.GetString(this.GetType().FullName,groupBoxProperty.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
            labelBaudRate.Text = StringManager.GetString(this.GetType().FullName,labelBaudRate.Text);
            labelDataBits.Text = StringManager.GetString(this.GetType().FullName,labelDataBits.Text);
            labelParity.Text = StringManager.GetString(this.GetType().FullName,labelParity.Text);
            label1.Text = StringManager.GetString(this.GetType().FullName,label1.Text);
            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);

        }

        private void SerialMotionInfoForm_Load(object sender, EventArgs e)
        {
            SerialPortManager.FillComboAllPort(comboPortName);

            txtName.Text = serialMotionInfo.Name;
            numAxis.Value = serialMotionInfo.NumAxis;

            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            comboPortName.Text = serialPortInfo.PortName;
            baudRate.Text = serialPortInfo.BaudRate.ToString();
            parity.Text = serialPortInfo.Parity.ToString();

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            stopBits.SelectedIndex = stopBitsValues.ToList().IndexOf(serialPortInfo.StopBits);

            dataBits.Text = serialPortInfo.DataBits.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            serialMotionInfo.Name = txtName.Text;
            serialMotionInfo.NumAxis = (int)numAxis.Value;

            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            serialPortInfo.PortName = comboPortName.Text;
            serialPortInfo.BaudRate = Convert.ToInt32(baudRate.Text);
            serialPortInfo.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
            serialPortInfo.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits.Text);
            serialPortInfo.DataBits = Convert.ToInt32(dataBits.Text);

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            serialPortInfo.StopBits = stopBitsValues[stopBits.SelectedIndex];

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
