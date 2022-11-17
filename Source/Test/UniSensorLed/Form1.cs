using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace UniSensorLed
{

    public partial class LED_Driver : Form
    {
        enum STATUS { INIT, OPEN, CLOSE };

        string Receivedstring = "";
        ManualResetEvent eventReceived;


        public LED_Driver()
        {
            InitializeComponent();
            initComboBox_com();
            //SetState(STATUS.INIT);
            eventReceived = new ManualResetEvent(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            open();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            close();
        }
        private void initComboBox_com()
        {
            string[] ports = SerialPort.GetPortNames();

            for (int i = 0; i < ports.Length; i++)
            {
                if (!comboBox_Com.Items.Contains(ports[i]))
                {
                    comboBox_Com.Items.Add(ports[i]);
                }
            }
            comboBox_Com.SelectionStart = 0;
        }
        void open()
        {
            string ComPortNum = (string)comboBox_Com.SelectedItem;

            if (ComPortNum != null && !serialPort1.IsOpen)
            {
                // Allow the user to set the appropriate properties.
                serialPort1.PortName = ComPortNum;
                serialPort1.BaudRate = 115200;
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;

                // Set the read/write timeouts
                serialPort1.ReadTimeout = 500;
                serialPort1.WriteTimeout = 500;

                serialPort1.PinChanged += SerialPinChangedEventHandler;
                serialPort1.DataReceived += SerialDataReceivedEventHandler;
                serialPort1.Open();
                SetState(STATUS.OPEN);
            }
        }
        void close()
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.PinChanged -= SerialPinChangedEventHandler;
                serialPort1.DataReceived -= SerialDataReceivedEventHandler;
                serialPort1.Close();
                SetState(STATUS.CLOSE);
            }
        }

        private void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //if (e.EventType == SerialData.Chars)
            //{
            //    int byteToRead = serialPort1.BytesToRead;

            //    byte[] dataByte = new byte[byteToRead];
            //    serialPort1.Read(dataByte, 0, byteToRead);
            //    if (Array.IndexOf<byte>(dataByte, 0x0A) >= 0)
            //        eventReceived.Set();
            //}
            SerialPort sp = (SerialPort)sender;
            Receivedstring = sp.ReadExisting();
            if (Receivedstring.Contains("\n") )
                eventReceived.Set();
        }

        static bool RIon = true;
        private void SerialPinChangedEventHandler(object sender, SerialPinChangedEventArgs e)
        {
            //+12v 입력시 On으로 표시됨.
            SerialPort serialport = sender as SerialPort;

            if (e.EventType == SerialPinChange.Break)
                SetColorControl(label_Break, serialport.BreakState);

            else if (e.EventType == SerialPinChange.CDChanged)
                SetColorControl(label_DCD, serialPort1.CDHolding);

            else if (e.EventType == SerialPinChange.CtsChanged)
                SetColorControl(label_CTS, serialPort1.CtsHolding);

            else if (e.EventType == SerialPinChange.DsrChanged)
                SetColorControl(label_DSR, serialPort1.DsrHolding);

            else if (e.EventType == SerialPinChange.Ring)//Rising Falling 일때 검지됨.. 핀상태는 알수가 없나?
                SetColorControl(label_RI, RIon ^= true);
        }

        private void SetColorControl(Control ctrl, bool on)
        {  //+12v 입력시 On으로 표시됨.
            ctrl.BackColor = on ? Color.Lime : Color.DarkGreen;
        }

        private void SetState(STATUS status)
        {
            switch (status)
            {
                case STATUS.INIT:
                case STATUS.CLOSE:
                    comboBox_Com.Enabled = true;
                    button_Open.Enabled = true;
                    button_Close.Enabled = false;
                    trackBar1.Enabled = false;
                    trackBar2.Enabled = false;
                    trackBar3.Enabled = false;
                    trackBar4.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox2.Enabled = false;
                    checkBox3.Enabled = false;
                    checkBox4.Enabled = false;
                    break;

                case STATUS.OPEN:
                    comboBox_Com.Enabled = false;
                    button_Open.Enabled = false;
                    button_Close.Enabled = true;
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    trackBar4.Enabled = true;
                    checkBox1.Enabled = true;
                    checkBox2.Enabled = true;
                    checkBox3.Enabled = true;
                    checkBox4.Enabled = true;
                    break;
            }
            checkBox_RTS.Enabled = button_Close.Enabled;
            checkBox_DTR.Enabled = button_Close.Enabled;
            label_DCD.Enabled = button_Close.Enabled;
            label_RI.Enabled = button_Close.Enabled;
            label_DSR.Enabled = button_Close.Enabled;
            label_CTS.Enabled = button_Close.Enabled;
            label_Break.Enabled = button_Close.Enabled;
        }

        private void checkBox_RTS_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.RtsEnable = checkBox_RTS.Checked; 
            SetColorControl(checkBox_RTS, checkBox_RTS.Checked);
        }

        private void checkBox_DTR_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.DtrEnable = checkBox_DTR.Checked;
            SetColorControl(checkBox_DTR, checkBox_DTR.Checked);
        }

        private void Form_RS232c_FormClosing(object sender, FormClosingEventArgs e)
        {
            close();
        }

    
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = sender as TrackBar;
            int channel = 0;
            
            switch(tb.Name)
            {
                case "trackBar1":
                    label1.Text = trackBar1.Value.ToString();
                    channel = 0;
                    break;

                case "trackBar2":
                    label2.Text = trackBar2.Value.ToString();
                    channel = 1;
                    break;

                case "trackBar3":
                    label3.Text = trackBar3.Value.ToString();
                    channel = 2;
                    break;

                case "trackBar4":
                    label4.Text = trackBar4.Value.ToString();
                    channel = 3;
                    break;
            }

            int value = tb.Value;
            string strcommand = string.Format("I{0},{1}\r\n", channel, value); //value 0~1000;
            string retstr = sendCommand( strcommand);
        }
 
        private string sendCommand(string strcommand)
        {
            byte[] command = Encoding.ASCII.GetBytes(strcommand);
            eventReceived.Reset();
            serialPort1.Write(command, 0, command.Length);
            if( eventReceived.WaitOne(500) == true ) //success
            {
                return Receivedstring;
            }
            else //fail
            {
                return "";
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            string checkbx = ((CheckBox)(sender)).Text;
            int nChecked = ((CheckBox)(sender)).Checked? 1:0;
            
            int channel = 0;
            switch (checkbx)
            {
                case "CH0":
                    //label1.Enabled = bChecked;
                    channel = 0;

                    break;

                case "CH1":
                    channel = 1;

                    //label2.Enabled = bChecked;
                    break;

                case "CH2":
                    channel = 2;

                    //label3.Enabled = bChecked;
                    break;

                case "CH3":
                    channel = 3;
                    //label4.Enabled = bChecked;
                    break;

                default:
                    break;
            }
            string strcommand = string.Format("P{0},{1}\r\n", channel, nChecked);
            string retstr = sendCommand(strcommand);
        }
    }
}
