using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Device.Dio.UI
{
    public partial class SerialDigitalIoInfoForm : Form
    {
        public SerialDigitalIoInfo SerialDigitalIoInfo => serialDigitalIoInfo;
        SerialDigitalIoInfo serialDigitalIoInfo;

        public SerialDigitalIoInfoForm(SerialDigitalIoInfo serialDigitalIoInfo)
        {
            InitializeComponent();
            SerialPortManager.FillComboAllPort(comPort);

            this.serialDigitalIoInfo = serialDigitalIoInfo;
        }

        private void SerialDigitalIoInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = serialDigitalIoInfo.Name;
            comPort.Text = serialDigitalIoInfo.SerialPortInfo.PortName;

            this.doPort0RTS.Checked = ((serialDigitalIoInfo.RtsPort & 0x01 )> 0);
            this.doPort1RTS.Checked = ((serialDigitalIoInfo.RtsPort & 0x02) > 0);

            this.doPort0DTR.Checked = ((serialDigitalIoInfo.DtrPort & 0x01) > 0);
            this.doPort1DTR.Checked = ((serialDigitalIoInfo.DtrPort & 0x02) > 0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Apply(this.serialDigitalIoInfo);
        }

        private void Apply(SerialDigitalIoInfo  serialDigitalIoInfo )
        {
            serialDigitalIoInfo.SerialPortInfo.PortName = comPort.Text;
            serialDigitalIoInfo.RtsPort = (this.doPort0RTS.Checked ? 1 : 0) + (this.doPort1RTS.Checked ? 2 : 0);
            serialDigitalIoInfo.DtrPort = (this.doPort0DTR.Checked ? 1 : 0) + (this.doPort1DTR.Checked ? 2 : 0);
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            SerialDigitalIoInfo clone = (SerialDigitalIoInfo )this.serialDigitalIoInfo.Clone();
            Apply(clone);

            DigitalIoSerial digitalIoSerial = new DigitalIoSerial("Test");
            if(digitalIoSerial.Initialize(clone) ==false)
            {
                MessageBox.Show("Port initialize Fail");
                return;
            }

            ThreadHandler threadHandler = null;
            threadHandler = new ThreadHandler("Text", new System.Threading.Thread(() =>
            {
                int i = 0;

                while(!threadHandler.RequestStop)
                {
                    digitalIoSerial.WriteOutputPort(0, 0, (i == 1 || i == 2));
                    digitalIoSerial.WriteOutputPort(0, 1, (i == 2 || i == 3));
                    i = (i + 1) % 4;
                    Thread.Sleep(500);
                }
            }));
            threadHandler.Start();

            MessageBox.Show("Check the signal...");

            threadHandler.Stop();
            digitalIoSerial.Release();
        }

        private void doPort_CheckedChanged(object sender, EventArgs e)
        {
            if(doPort0DTR.Checked)
                doPort1DTR.Checked = doPort1DTR.Enabled = false;
            else if(doPort1DTR.Checked)
                doPort0DTR.Checked = doPort0DTR.Enabled = false;
            else
                doPort0DTR.Enabled = doPort1DTR.Enabled = true;

            if (doPort0RTS.Checked)
                doPort1RTS.Checked = doPort1RTS.Enabled = false;
            else if (doPort1RTS.Checked)
                doPort0RTS.Checked = doPort0RTS.Enabled = false;
            else
                doPort0RTS.Enabled = doPort1RTS.Enabled = true;

        }
    }
}
