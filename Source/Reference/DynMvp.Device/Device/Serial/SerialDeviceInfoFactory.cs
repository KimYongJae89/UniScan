using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Serial
{
    public class SerialDeviceInfoFactory
    {
        public static SerialDeviceInfo CreateSerialDeviceInfo(ESerialDeviceType deviceType, ESerialSensorType sensorType)
        {
            switch (deviceType)
            {
                case ESerialDeviceType.SerialEncoder:
                    return new SerialEncoderInfo();

                case ESerialDeviceType.SerialSensor:
                    return SerialSensorInfo.Create(sensorType);
                default:
                    return null;
            }
        }
    }



    //public class SerialDeviceFactory
    //{
    //   
    //}
}
