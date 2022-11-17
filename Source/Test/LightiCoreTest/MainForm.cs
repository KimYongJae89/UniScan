using DynMvp.Device.Device.Light;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using DynMvp.Devices.Comm;
using System.Diagnostics;
using DynMvp.Device.Device.Light.SerialLigth.iCore;

// 순서 : 통신 > 읽기 > 폼 출력 > 라디오버튼 입력 >  
namespace LightiCoreTest
{
    public partial class MainForm : Form  //view
    {
        SerialLightCtrlInfoIPulse serialLightInfoIPulse;
        SerialLightIPulse serialLightIPulse;

        public MainForm()
        {
            InitializeComponent();

            this.serialLightInfoIPulse = new SerialLightCtrlInfoIPulse();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            OffRadio.Checked = (this.serialLightInfoIPulse.OperationMode == OperationMode.Off);
            ContinuousRadio.Checked = (this.serialLightInfoIPulse.OperationMode == OperationMode.Continuous);
            PulseRadio.Checked = (this.serialLightInfoIPulse.OperationMode == OperationMode.Pulse);
        }

        private void bntOpenAndSearch_Click(object sender, EventArgs e)
        {
            if (this.serialLightIPulse == null)
            {
                string comPort = this.serialComPort.Text;
                int baudRate = int.Parse(this.serialBaud.Text);
                byte slaveId = byte.Parse(this.slaveId.Text);

                SerialLightCtrlInfoIPulse serialLightInfoIPulse = new SerialLightCtrlInfoIPulse();
                serialLightInfoIPulse.NumChannel = 4;
                serialLightInfoIPulse.ResponceTimeoutMs = 1000;
                serialLightInfoIPulse.SerialPortInfo.CopyFrom(new SerialPortInfo(comPort, baudRate));
                serialLightInfoIPulse.SlaveId = slaveId;
                this.serialLightInfoIPulse = serialLightInfoIPulse;

                this.serialLightIPulse = new SerialLightIPulse(serialLightInfoIPulse);
                bool init = this.serialLightIPulse.Initialize();
                if (!init)
                {
                    this.serialLightIPulse.Dispose();
                    return;
                }
            }
            else
            {
                this.serialLightIPulse.TurnOff();
                this.serialLightIPulse.Dispose();
                this.serialLightIPulse = null;
            }
        }

        private void OffRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                this.serialLightInfoIPulse.OperationMode = (sender == ContinuousRadio ? OperationMode.Continuous : sender == PulseRadio ? OperationMode.Pulse : OperationMode.Off);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LightValue lv = new LightValue(4);
            lv.Value[0] = (int)numericUpDown16.Value;
            lv.Value[1] = (int)numericUpDown17.Value;
            lv.Value[2] = (int)numericUpDown18.Value;
            lv.Value[3] = (int)numericUpDown19.Value;

            this.serialLightIPulse.TurnOn(lv);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.serialLightIPulse.TurnOff();
        }
    }
}
