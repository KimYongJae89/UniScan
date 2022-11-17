using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial.Sensor.GT2_70
{
    public class SerialSensorInfoGT2_70 : SerialSensorInfo
    {
        public SerialSensorInfoGT2_70() : base()
        {
            this.SensorType = ESerialSensorType.GT2_70;
        }

        public override PacketParser CreatePacketParser()
        {
            SimplePacketParser simplePacketParser = new SimplePacketParser();
            simplePacketParser.EndChar = new byte[2] { (byte)'\r', (byte)'\n' };
            return simplePacketParser;
        }

        public override bool IsPortFound(byte[] responce)
        {
            throw new NotImplementedException();
        }

        public override string GetPortFindString()
        {
            throw new NotImplementedException();
        }
    }

    public class SerialSensorGT2_70 : SerialSensor
    {
        public SerialSensorGT2_70(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo)
        {
            //deviceInfo.DeviceName = "GT2_70";
        }

        public static SerialSensor Create(SerialSensorInfo serialSensorInfo)
        {
            if (serialSensorInfo.SerialPortInfo.IsVirtual)
                return new SerialSensorGT2_70Virtual(serialSensorInfo);

            return new SerialSensorGT2_70(serialSensorInfo);
        }

        public override bool GetData(int count, float[] dataArray)
        {
            string excuteResult = ExcuteCommand("M0");
            string[] tokens = excuteResult.Split(',');
            if (tokens == null || tokens.Length < count + 1)
                return false;

            if (tokens[0] == "M0")
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        dataArray[i] = float.Parse(tokens[i + 1]);
                    }
                    catch (FormatException)
                    {
                        var msg = string.Format("SerialSensorGT2_70.GetData():FormatException[{0}]", excuteResult);
                        LogHelper.Debug(LoggerType.Serial,msg);
                        return false;
                    }
                    catch (OverflowException)
                    {
                        var msg = string.Format("SerialSensorGT2_70.GetData():OverflowException[{0}]", excuteResult);
                        LogHelper.Debug(LoggerType.Serial, msg);
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }

    public class SerialSensorGT2_70Virtual : SerialSensorGT2_70
    {
        public SerialSensorGT2_70Virtual(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo)
        {
        }

        protected override bool SendCommand(string packetString)
        {
            //if (this.isAlarmed)
            //    return false;

            byte[] packet = serialPortEx.PacketHandler.PacketParser.EncodePacket(packetString);
            byte[] virtualResponce = BuildVirtualResponce();
            Task.Run(() => serialPortEx.ProcessPacket(virtualResponce));
            return true;
        }

        double coefficient = 0.000;
        private byte[] BuildVirtualResponce()
        {
            double[] data = new double[] { 0.022, 0.022 };
            if (true)
            {
                if (coefficient > 3)
                    coefficient = 0;

                if (coefficient > 2)
                {
                    //data[0] += 0.03 * Math.Sin(coefficient * Math.PI);
                    //data[1] += 0.03 * Math.Cos(coefficient * Math.PI);
                    data[0] += 0.03 * Math.Sin(coefficient * Math.PI);
                    data[1] += 0.03 * Math.Sin(coefficient * Math.PI);
                }
                coefficient += 0.1;
            }
            else
            {
                data[0] = data[1] = 0.0200;
            }

            List<string> list = new List<string>();
            list.Add("M0");
            Array.ForEach(data, f => list.Add(f.ToString("F6")));

            byte[] dummyData = serialPortEx.PacketHandler.PacketParser.EncodePacket(string.Join(",", list));
            return dummyData;
        }
    }
}
