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


// 순서 : 통신 > 읽기 > 폼 출력 > 라디오버튼 입력 >  
namespace UniScanM.CGInspector.UI
{
    public partial class iCoreLightControll : Form  //view
    {
        public iCoreLightControll()
        {
            InitializeComponent();

        }
        List<IPulseFrame> buffer = new List<IPulseFrame>();
        Received received = new Received();


        private void OffRadio_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.WriteLine(string.Format("{0}{1:00}{2:0000}{3:00}", 9, 0x06, 0x0030, 0x00));
            richTextBox1.AppendText(serialPort1.ReadLine());
            serialPort1.DiscardOutBuffer();
        }

        private void ContinuousRadio_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.WriteLine(string.Format("{0}{1:00}{2:0000}{3:00}", 9, 0x06, 0x0030, 0x01));
        }

        private void PulseRadio_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.WriteLine(string.Format("{0}{1:00}{2:0000}{3:00}", 9, 0x06, 0x0030, 0x02));
        }
        
        #region SerialProperty
        private void SerialProperty(string portname, Parity parity, int baudrate, StopBits stopbits, int databits)
        {
            portname = serialPort1.PortName;
            parity = serialPort1.Parity;
            baudrate = serialPort1.BaudRate;
            stopbits = serialPort1.StopBits;
            databits = serialPort1.DataBits;

        }
        #endregion


        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int Rx =  serialPort1.Read(received.ReceiveBuffer, 0, 8);
            richTextBox1.AppendText(string.Format("{0}", Rx.ToString()));
        }

        private void iCoreLightControll_Load(object sender, EventArgs e)
        {
            if(!serialPort1.IsOpen)
            {
                serialPort1.Open();
                
            }
        }

        private void iCoreLightControll_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Dispose();
        }

        private void LED1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.LED1CheckBox.Checked)
            {
                serialPort1.WriteLine(string.Format("{0}{1:00}{2:0000}{3:00},{4:0000}", 9, 0x06, 0x0030, 0x02, 0x0101));
            }
        }

        private void LED1Current_ValueChanged(object sender, EventArgs e)
        {
            serialPort1.Write(string.Format("{0}{1:00}{2:0000}{3:00},{4:0000}", 9, 0x06, 0x0340 , (ushort)LED1Current.Value, 0x0101));
        }

        private void LED1ContinuousNumeric_ValueChanged(object sender, EventArgs e)
        {
            serialPort1.Write(string.Format("{0}{1:00}{2:0000}{3:00},{4:0000}", 9, 0x06, 0x0330, (ushort)LED1ContinuousNumeric.Value, 0x0101));
        }
    }


    public abstract class Property : SerialLightIPulse
    {
        
        public delegate void PacketLengthDelegate(byte length, ushort argument);
       
        public Property(SerialLightCtrlInfo serialLightCtrlInfo) : base(serialLightCtrlInfo)
        {
            serialLightCtrlInfo.Clone();
        }

    }

    public abstract class PulseFrame : IPulseFrame
    {
        IPulseFunction PulseFunction;
        IPulseFrame pulseFrame;
        iCoreLightControll ControlForm;
        SerialPort serialport = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
        
        public PulseFrame(byte[] sizebytes) : base(sizebytes)
        {
            List<byte> productID = new List<byte>();
            productID.Add(9);

            sizebytes = ToBytes();
        }

        public bool Connected()
        {
            if (!serialport.IsOpen)
            {
                MessageBox.Show("connection error");
                return false;
            }
            else
            {
                serialport.WriteLine(string.Format("{0:00}{1:00}{2:0000}{3:00000}", 9,Function.Write,Address.WriteEnable_RW,(ushort)0x0000));
            }
            return true;
        }
    }

    public class Received
    {
       
        byte[] receiveBuffer;
        public byte[] ReceiveBuffer
        {
            get { return receiveBuffer; }
            set { receiveBuffer = value; }
        }

        public static void stringParser(byte[] ReceiveData )
        {

        }

    }

    public class Sending
    {
        string sendbuffer;
        public string SendBuffer
        {
            get { return sendbuffer; }
            set { sendbuffer = value; }
        }
    }
}
