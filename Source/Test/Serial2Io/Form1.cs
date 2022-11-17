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

namespace Serial2Io
{
    public partial class Form1 : Form
    {
        bool run = false;
        Thread[] runThread = null;

        DigitalIo[] digitalIos = null;
       

        public Form1()
        {
            InitializeComponent();

            this.digitalIos = new DigitalIo[2];

            try
            {
                SerialDigitalIoInfo serialDigitalIoInfo = new SerialDigitalIoInfo(0, 0, 0, 0, 1, 0, 2);
                serialDigitalIoInfo.SerialPortInfo = new DynMvp.Devices.Comm.SerialPortInfo("COM1", 9600);
                DigitalIo digitalIoSerial = DigitalIoFactory.Create(serialDigitalIoInfo);
                this.digitalIos[0] = digitalIoSerial;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("dd");

            try
            {
                SlaveDigitalIoInfo pciDigitalIoInfo = new SlaveDigitalIoInfo (DigitalIoType.TmcAexxx, 0, 1, 0, 16, 1, 0, 16);
                DigitalIo digitalIoAlpha = DigitalIoFactory.Create(pciDigitalIoInfo);
                this.digitalIos[1] = digitalIoAlpha;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.runThread = new Thread[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.serialPort1.IsOpen)
                {
                    this.serialPort1.Close();
                    button1.Text = "Closed";
                }
                else
                {
                    this.serialPort1.Open();
                    button1.Text = "Opened";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(run)
            {
                for (int i = 0; i < this.runThread.Length; i++)
                {
                    this.runThread[i].Abort();
                    this.digitalIos[i]?.WriteOutputGroup(0, 0x00);
                }
                this.run = false;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    this.runThread[i] = new Thread(new ParameterizedThreadStart(IoProc));
                    this.runThread[i].Start(i);
                }
                this.run = true;
            }
        }

        private void IoProc(object obj)
        {
            int index = (int)obj;
            int a = 0;
            while(true)
            {
                if (this.digitalIos[index] != null)
                {
                    this.digitalIos[index].WriteOutputGroup(0, 0xff);
                    //for (int i = 0; i < 1000; i++) a++;
                    this.digitalIos[index].WriteOutputGroup(0, 0x00);
                    //for (int i = 0; i < 1000; i++) a++;
                }
            }
        }
    }
}
