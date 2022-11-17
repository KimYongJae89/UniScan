using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using DynMvp.Devices.Comm;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DynMvp.Device.Device.Serial.Sensor.CD22_15_485
{
    public class SerialSensorInfoCD22_15_485 : SerialSensorInfo
    {
        public SerialSensorInfoCD22_15_485() : base()
        {
            this.SensorType = ESerialSensorType.CD22_15_485;
        }

        public override PacketParser CreatePacketParser()
        {
            CD22PacketParser cd22PacketParser = new CD22PacketParser();
            return cd22PacketParser;
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

    public class SerialSensorCD22_15_485 : SerialSensor
    {
        int responceTimeout = 1000;

        public SerialSensorCD22_15_485(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo) { }

        public static SerialSensorCD22_15_485 Create(SerialSensorInfo serialSensorInfo)
        {
            if (serialSensorInfo.SerialPortInfo.IsVirtual)
                return new SerialSensorCD22_15_485Virtual(serialSensorInfo);

            return new SerialSensorCD22_15_485(serialSensorInfo);
        }

        public byte[] ExcuteCommand(Frame cmd)
        {
            lock (this)
            {
                var temp = cmd.ToBytes();
                this.SendCommand(temp);
                this.waitResponce.WaitOne(responceTimeout);
                return this.lastResponce?.ReceivedData;
            }
        }

        public bool LaserOn()
        {
            byte[] data = new byte[2];

            data[0] = 0xA0;
            data[1] = 0x03;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand laserOn = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(laserOn);
            byte[] result = this.ExcuteCommand(frame);

            if (CheckAck(result))
                return true;
            else
                return false;
        }

        public bool LaserOff()
        {
            byte[] data = new byte[2];

            data[0] = 0xA0;
            data[1] = 0x02;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand laserOFf = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(laserOFf);
            byte[] result = this.ExcuteCommand(frame);

            if (CheckAck(result))
                return true;
            else
                return false;
        }

        public bool ExecuteZeroReset()
        {
            byte[] data = new byte[2];

            data[0] = 0xA1;
            data[1] = 0x00;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand executeZeroReset = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(executeZeroReset);
            byte[] result = this.ExcuteCommand(frame);

            if (CheckAck(result))
                return true;
            else
                return false;
        }

        public bool ReleaseZeroReset()
        {
            byte[] data = new byte[2];

            data[0] = 0xA1;
            data[1] = 0x01;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand releaseZeroReset = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(releaseZeroReset);
            byte[] result = this.ExcuteCommand(frame);

            if (CheckAck(result))
                return true;
            else
                return false;
        }

        public bool ExcuteInitiallize()
        {
            byte[] data = new byte[2];

            data[0] = 0x40;
            data[1] = 0x00;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand initialize = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(initialize);
            byte[] result = this.ExcuteCommand(frame);

            if (CheckAck(result))
                return true;
            else
                return false;
        }

        public override bool GetData(int count, float[] dataArray)
        {
            //[Transimission data]   stx    command data1       data2       etx     bcc
            //[Incoming data]        stx    ack     response1   response2   etx     bcc
            //[Incoming data(error)] stx    nck     errorcode   00(H)       etx     bcc

            byte[] data = new byte[2];

            data[0] = 0xB0;
            data[1] = 0x01;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            short datas = BitConverter.ToInt16(data, 0);

            IndividualFunctionCommand measure = new IndividualFunctionCommand(datas);

            Frame frame = new Frame(measure);
            byte[] result = this.ExcuteCommand(frame);

            if (result != null && CheckAck(result))
            {
                byte[] receiveData = result.Skip(2).Take(2).ToArray();
                if (receiveData[0] == 127 && receiveData[1] == 255)
                    return false;

                var hexData = BitConverter.ToString(receiveData).Replace("-", "");
                float dec = Convert.ToInt16(hexData, 16);

                dataArray[0] = dec / 1000f;

                return true;
            }
            return false;
        }

        public bool CheckAck(byte[] resultData)
        {
            if (resultData == null || resultData.Count() != 6)
                return false;

            byte ack = resultData[1];
            byte res = resultData[2];

            if (ack == 0x15)
            {
                switch (res)
                {
                    case 0x02:
                        //MessageBox.Show("Address is invalid");
                        DynMvp.Base.LogHelper.Error(Base.LoggerType.Device, "Address is invalid");
                        break;
                    case 0x04:
                        //MessageBox.Show("Bcc value is invalid");
                        DynMvp.Base.LogHelper.Error(Base.LoggerType.Device, "Bcc value is invalid");
                        break;
                    case 0x05:
                        //MessageBox.Show("Invalid command is issued except 'C' 'W' 'R' is invalid");
                        DynMvp.Base.LogHelper.Error(Base.LoggerType.Device, "Invalid command is issued except 'C' 'W' 'R' is invalid");
                        break;
                    case 0x06:
                        //MessageBox.Show("Setting valuue is invalid (out of specifications)");
                        DynMvp.Base.LogHelper.Error(Base.LoggerType.Device, "Setting valuue is invalid (out of specifications)");
                        break;
                    case 0x07:
                        //MessageBox.Show("Setting value is invalid (out of range)");
                        DynMvp.Base.LogHelper.Error(Base.LoggerType.Device, "Setting value is invalid (out of range)");
                        break;
                }
                return false;
            }
            else if (ack == 0x06)
                return true;

            return false;
        }
    }

    public class SerialSensorCD22_15_485Virtual : SerialSensorCD22_15_485
    {
        public SerialSensorCD22_15_485Virtual(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo)
        {
        }

        public override bool GetData(int count, float[] dataArray)
        {
            double coefficient = 0;
            double[] data = new double[] { 0, 0 };

            if (coefficient > 4)
                coefficient = 0;

            if (coefficient > 2)
            {
                data[0] = 0.03 * Math.Sin(coefficient * Math.PI);
                data[1] = 0.03 * Math.Cos(coefficient * Math.PI);
            }
            coefficient += 0.1;

            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = (float)data[i];

            return true;
        }
    }
}

