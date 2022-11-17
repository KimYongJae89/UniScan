using DynMvp.Device.Daq.Sensor.UsbSensorGloss_60;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DynMvp.Devices.Daq
{
    public class DaqUSBGloss60 : DaqChannel
    {
        public DaqUSBGloss60() : base(DaqChannelType.USB)
        {
        }

        public override void Initialize(DaqChannelProperty daqChannelProperty)
        {
            List<GlossSensorInfo> deviceList = UsbSensorGloss60.ListDevices();

            if (deviceList == null || deviceList.Count == 0)
            {
                MessageBox.Show("Gloss Device를 찾을 수 없습니다.");
                return;
            }

            foreach (var device in deviceList)
                UsbSensorGloss60.Open(device.PortNo);
        }

        public override double[] ReadVoltage(int numSamples)
        {
            return null;
        }

        public override double[] ReadData(int numSamples = 1)
        {
            double[] tempResult = new double[numSamples];

            for (int count = 0; count < numSamples; count++)
                tempResult[count] = UsbSensorGloss60.Measure();

            return tempResult;
        }

        public bool Calibration()
        {
            UsbSensorGloss60.Calibrate();
            if (UsbSensorGloss60.GetCalibrateStatusFlag() == "0")
                return true;
            else
                return false;
        }
    }

}