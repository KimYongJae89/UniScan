using DynMvp.Device.Device.Serial.Sensor.SC_HG1_485;
using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEDMS_SENSOR_TEST
{
    public partial class Form1 : Form
    {
        SerialSensorSC_HG1_485 serialSensor;

        public Form1()
        {
            InitializeComponent();

            SerialSensorInfo serialSensorInfo = SerialSensorInfo.Create(ESerialSensorType.SC_HG1_485);
            serialSensorInfo.DeviceName = "SerialSensorSC_HG1_485";
            serialSensorInfo.SensorType = ESerialSensorType.SC_HG1_485;
            serialSensorInfo.SerialPortInfo.CopyFrom(new DynMvp.Devices.Comm.SerialPortInfo("COM3", 19200));

            this.serialSensor = serialSensorInfo.CreateSerialDevice(false) as SerialSensorSC_HG1_485;

            //this.serialSensor = SerialSensorSC_HG1_485.Create(serialSensorInfo);
            this.serialSensor.Initialize();

            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float[] datas = new float[2];
            this.serialSensor.GetData(2, datas);

            value1.Text = datas[0].ToString();
            value2.Text = datas[1].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            float[] datas = new float[2];
            this.serialSensor.GetData(2, datas);

            value1.Text = datas[0].ToString();
            value2.Text = datas[1].ToString();
        }
    }
}
