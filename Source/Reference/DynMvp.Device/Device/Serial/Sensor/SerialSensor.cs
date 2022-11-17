using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Device.Serial.Sensor;
using DynMvp.Device.Device.Serial.Sensor.CD22_15_485;
using DynMvp.Device.Device.Serial.Sensor.GT2_70;
using DynMvp.Device.Device.Serial.Sensor.SC_HG1_485;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;

namespace DynMvp.Device.Serial.Sensor
{
    public enum ESerialSensorType
    {
        None, GT2_70, SC_HG1_485, CD22_15_485
    }

    public abstract class SerialSensorInfo : SerialDeviceInfo
    {
        ESerialSensorType sensorType;
        public ESerialSensorType SensorType { get => sensorType; set => sensorType = value; }

        public override string GetDeviceString()
        {
            return $"{this.DeviceType};{this.sensorType}";
        }

        public SerialSensorInfo() : base(ESerialDeviceType.SerialSensor) { }

        public static SerialSensorInfo Create(ESerialSensorType sensorType)
        {
            switch (sensorType)
            {
                case ESerialSensorType.CD22_15_485:
                    return new SerialSensorInfoCD22_15_485();

                case ESerialSensorType.GT2_70:
                    return new SerialSensorInfoGT2_70();

                case ESerialSensorType.SC_HG1_485:
                    return new SerialSensorInfoSC_HG1_485();
            }

            return null;
        }

        public override SerialDevice CreateSerialDevice(bool virtualMode)
        {
            if (virtualMode)
                this.SerialPortInfo.PortName = "Virtual";

            return SerialSensor.Create(this);
        }

        public override SerialDeviceInfo Clone()
        {
            SerialSensorInfo serialSensorInfo = SerialSensorInfo.Create(this.SensorType);
            serialSensorInfo.CopyFrom(this);
            return serialSensorInfo;
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            SerialSensorInfo serialSensorInfo = (SerialSensorInfo)serialDeviceInfo;
            base.CopyFrom(serialDeviceInfo);

            this.sensorType = serialSensorInfo.sensorType;
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            XmlHelper.SetValue(xmlElement, "SerialSensorType", this.sensorType.ToString());
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            this.sensorType = XmlHelper.GetValue(xmlElement, "SerialSensorType", ESerialSensorType.GT2_70);

            //resolution = Convert.ToDouble(XmlHelper.GetValue(xmlElement, "Resolution", resolution.ToString()));
        }
    }

    public abstract class SerialSensor : SerialDevice
    {
        public SerialSensor(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public static SerialSensor Create(SerialDeviceInfo deviceInfo)
        {
            SerialSensorInfo serialSensorInfo = (SerialSensorInfo)deviceInfo;

            switch (serialSensorInfo.SensorType)
            {
                case ESerialSensorType.GT2_70:
                    return SerialSensorGT2_70.Create(serialSensorInfo);
                case ESerialSensorType.SC_HG1_485:
                    return SerialSensorSC_HG1_485.Create(serialSensorInfo);
                case ESerialSensorType.CD22_15_485:
                    return SerialSensorCD22_15_485.Create(serialSensorInfo);
                default:
                    return null;
            }
        }

        public abstract bool GetData(int count, float[] dataArray);
    }


}
