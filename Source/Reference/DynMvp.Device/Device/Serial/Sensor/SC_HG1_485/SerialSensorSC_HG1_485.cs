using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial.Sensor.SC_HG1_485
{
    public class SerialSensorInfoSC_HG1_485 : SerialSensorInfo
    {
        public SerialSensorInfoSC_HG1_485() : base()
        {
            this.SensorType = ESerialSensorType.SC_HG1_485;
        }

        public override PacketParser CreatePacketParser()
        {
            RTUPacketParser rtuUPacketParser = new RTUPacketParser();
            return rtuUPacketParser;
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

    public class SerialSensorSC_HG1_485 : SerialSensor
    {
        int responceTimeout = 1000;

        public SerialSensorSC_HG1_485(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo) { }

        public static SerialSensorSC_HG1_485 Create(SerialSensorInfo serialSensorInfo)
        {
            if (serialSensorInfo.SerialPortInfo.IsVirtual)
                return new SerialSensorSC_HG1_485Virtual(serialSensorInfo);

            return new SerialSensorSC_HG1_485(serialSensorInfo);
        }

        public byte[] ExcuteCommand(Frame frame)
        {
            lock (this)
            {
                this.SendCommand(frame.ToBytes());
                this.waitResponce.WaitOne(responceTimeout);
                return this.lastResponce.ReceivedData;
            }
        }

        public override bool GetData(int count, float[] dataArray)
        {
            FnReadHoldingRegisters func = new FnReadHoldingRegisters(0x0064, (short)(count * 2));
            Frame frame = new Frame(1, func);
            this.ExcuteCommand(frame);

            byte slaveId = this.lastResponce.ReceivedData[0];
            byte funcCode = this.lastResponce.ReceivedData[1];
            byte dataSize = this.lastResponce.ReceivedData[2];
            byte[] data = this.lastResponce.ReceivedData.Skip(3).Take(dataSize).ToArray();

            if (dataSize != count * 4)
                return false;

            for (int i = 0; i < count; i++)
            {
                byte[] firstByte = data.Skip(i * 4).Take(2).ToArray();
                byte[] secondByte = data.Skip(i * 4 + 2).Take(2).ToArray();

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(firstByte);
                    Array.Reverse(secondByte);
                }

                byte[] totalByte = new byte[firstByte.Length + secondByte.Length];

                Array.Copy(firstByte, 0, totalByte, 0, firstByte.Length);
                Array.Copy(secondByte, 0, totalByte, firstByte.Length, secondByte.Length);

                dataArray[i] = BitConverter.ToInt32(totalByte, 0) / 10000f;
            }
            //dataArray[0] = float.Parse(responceData1);
            //dataArray[1] = float.Parse(responceData2);

            return true;
        }

        public bool GetSensitivityUseInfo(int count)
        {
            FnReadHoldingRegisters func = new FnReadHoldingRegisters(0x09F0, (short)count);

            Frame frame = new Frame(1, func);

            //TODO : [jena] data result값 잘써줬는지랑 데이터 사이즈 맞는지
            byte[] result = this.ExcuteCommand(frame);

            byte slaveId = this.lastResponce.ReceivedData[0];
            byte funcCode = this.lastResponce.ReceivedData[1];
            byte dataSize = this.lastResponce.ReceivedData[2];
            byte[] data = this.lastResponce.ReceivedData.Skip(3).Take(dataSize).ToArray();

            if (data[1] == 0)
                return false;
            else
                return true;
        }

        public bool GetSensitivityValueInfo(int count)
        {
            FnReadHoldingRegisters func = new FnReadHoldingRegisters(0x09F2, (short)count);

            Frame frame = new Frame(1, func);

            this.ExcuteCommand(frame);

            byte slaveId = this.lastResponce.ReceivedData[0];
            byte funcCode = this.lastResponce.ReceivedData[1];
            byte dataSize = this.lastResponce.ReceivedData[2];
            byte[] data = this.lastResponce.ReceivedData.Skip(3).Take(dataSize).ToArray();

            if (data[1] == 0)
                return false;
            else
                return true;
        }

        public bool UseSensitivity(bool master, bool on)
        {
            // 감도설정 사용자 설정으로 사용하게끔,,, 402545 --> 0: 디폴트(25%) 1: 사용자 설정
            FnWriteSingleRegister funcSetHandler = new FnWriteSingleRegister(0x03E8, Convert.ToInt16(!master));
            FnWriteSingleRegister func = new FnWriteSingleRegister(0x09F0, Convert.ToInt16(on));

            Frame frameSetHandler = new Frame(1, funcSetHandler);
            Frame frame = new Frame(1, func);

            byte[] handlerResult = this.ExcuteCommand(frameSetHandler);
            byte[] result = null;

            //TODO : [jena] data result값 유효성 체크
            if (handlerResult?.Count() == 8)
                result = this.ExcuteCommand(frame);

            if (result?.Count() == 8)
                return true;
            else
                return false;
        }

        public bool UserSensitivitySet(bool master, int percent)
        {
            if (percent < 10 || percent > 90)
                return false;

            // 감도설정 설정범위 : 10~90(%)
            FnWriteSingleRegister funcSetHandler = new FnWriteSingleRegister(0x03E8, Convert.ToInt16(!master));
            FnWriteSingleRegister func = new FnWriteSingleRegister(0x09F2, Convert.ToInt16(percent));

            Frame frameSetHandler = new Frame(1, funcSetHandler);
            Frame frame = new Frame(1, func);

            byte[] handlerResult = this.ExcuteCommand(frameSetHandler);
            byte[] result = null;
            if (handlerResult?.Count() == 8)
                result = this.ExcuteCommand(frame);

            if (result?.Count() == 8)
                return true;
            else
                return false;
        }

        public bool Preset(bool master, bool on)
        {
            FnWriteSingleRegister funcSetHandler = new FnWriteSingleRegister(0x03E8, Convert.ToInt16(!master));
            FnWriteSingleRegister func = new FnWriteSingleRegister(0x0468, Convert.ToInt16(on));

            Frame frameSetHandler = new Frame(1, funcSetHandler);
            Frame frame = new Frame(1, func);

            byte[] handlerResult = this.ExcuteCommand(frameSetHandler);
            byte[] result = null;


            if (handlerResult?.Count() == 8)
                result = this.ExcuteCommand(frame);

            if (result?.Count() == 8)
                return true;
            else
                return false;
        }

        public bool GetPresetInfo(int count)
        {
            FnReadHoldingRegisters func = new FnReadHoldingRegisters(0x0468, (short)count);

            Frame frame = new Frame(1, func);

            this.ExcuteCommand(frame);

            byte slaveId = this.lastResponce.ReceivedData[0];
            byte funcCode = this.lastResponce.ReceivedData[1];
            byte dataSize = this.lastResponce.ReceivedData[2];
            byte[] data = this.lastResponce.ReceivedData.Skip(3).Take(dataSize).ToArray();

            if (data[1] == 0)
                return false;
            else
                return true;
        }
    }

    public class SerialSensorSC_HG1_485Virtual : SerialSensorSC_HG1_485
    {
        public SerialSensorSC_HG1_485Virtual(SerialSensorInfo serialSensorInfo) : base(serialSensorInfo)
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

