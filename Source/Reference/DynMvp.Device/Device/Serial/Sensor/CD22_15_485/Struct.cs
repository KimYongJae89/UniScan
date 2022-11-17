using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial.Sensor.CD22_15_485
{
    public class Frame
    {
        //얘는 stx etx 붙여주는 애
        //BitArray stxBitArray = new BitArray(Convert.ToString(Convert.ToInt32("02", 16), 2).PadLeft(8, '0').Select(c => c == '1').ToArray());
        //BitArray etxBitArray = new BitArray(Convert.ToString(Convert.ToInt32("03", 16), 2).PadLeft(8, '0').Select(c => c == '1').ToArray());

        byte stx = 0x02;
        byte etx = 0x03;
        //byte stx = Convert.ToByte("02", 16);
        //byte etx = Convert.ToByte("03", 16);

        Command commandFrame;

        public Frame(Command command)
        {
            this.commandFrame = command;
        }

        public static byte[] CalcBCC(byte[] bytes)
        {
            if (bytes.Count() != 3)
                return null;

            byte result = (byte)(bytes[0] ^ bytes[1]^ bytes[2]);
            
            Byte[]  byteArray = BitConverter.GetBytes(result).Take(1).ToArray();
            return byteArray;
        }

        public byte[] CalcBCC()
        {
            List<byte> bccByteRange = new List<byte>();
            bccByteRange.AddRange(this.commandFrame.ToByte());
            return CalcBCC(bccByteRange.ToArray());
        }

        public byte[] ToBytes()
        {
            List<byte> command = new List<byte>();

            command.Add(stx);
            command.AddRange(this.commandFrame.ToByte());
            command.Add(etx);
            command.AddRange(CalcBCC());

            return command.ToArray();
        }
    }

    public abstract class Command
    {
        byte cmdCode;
        protected byte[] cmdData = null;

        public Command(byte cmdCode)
        {
            this.cmdCode = cmdCode;
        }

        public byte[] ToByte()
        {
            List<byte> byteList = new List<byte>();
            byteList.Add(cmdCode);
            byteList.AddRange(cmdData);
            return byteList.ToArray();
        }
    }

    public class IndividualFunctionCommand : Command
    {
        public IndividualFunctionCommand(short startAddr) : base(0x43)
        {
            byte[] data = BitConverter.GetBytes(startAddr);

            this.cmdData = new byte[2];
            this.cmdData[0] = data[1];
            this.cmdData[1] = data[0];
        }
    }

    public class WriteSetting : Command { WriteSetting() : base(0x57) { } }
    public class ReadSetting : Command { ReadSetting() : base(0x52) { } }

    //public abstract class CommandFrame
    //{
    //    byte cmdCode;
    //    byte data1;
    //    byte data2;

    //    byte ConvertToByte(BitArray bits)
    //    {
    //        if (bits.Count != 8)
    //        {
    //            throw new ArgumentException("bits");
    //        }
    //        byte[] bytes = new byte[1];

    //        bits.CopyTo(bytes, 0);
    //        return bytes[0];
    //    }

    //    public CommandFrame(string cmdCode, string data1, string data2)
    //    {
    //        BitArray cmdBitArray = new BitArray(Convert.ToString(Convert.ToInt32(cmdCode, 16), 2).PadLeft(8, '0').Select(c => c == '1').ToArray());
    //        BitArray data1BitArray = new BitArray(Convert.ToString(Convert.ToInt32(data1, 16), 2).PadLeft(8, '0').Select(c => c == '1').ToArray());
    //        BitArray data2BitArray = new BitArray(Convert.ToString(Convert.ToInt32(data2, 16), 2).PadLeft(8, '0').Select(c => c == '1').ToArray());

    //        //this.cmdCode = Convert.ToByte(cmdCode);
    //        //this.data1 = Convert.ToByte(data1);
    //        //this.data2 = Convert.ToByte(data2);

    //        this.cmdCode = ConvertToByte(cmdBitArray);
    //        this.data1 = ConvertToByte(data1BitArray);
    //        this.data2 = ConvertToByte(data2BitArray);
    //    }

    //    public byte[] ToByte()
    //    {
    //        List<byte> byteList = new List<byte>();
    //        byteList.Add(cmdCode);
    //        byteList.Add(data1);
    //        byteList.Add(data2);

    //        return byteList.ToArray();
    //    }
    //}

    //public class FuntionCommand : CommandFrame { public FuntionCommand(string data1, string data2) : base("43", data1, data2) { } }
    //public class Initialize : FuntionCommand { public Initialize() : base("40", "00") { } }
    //public class ReadMeasurementValue : FuntionCommand { public ReadMeasurementValue() : base("B0", "01") { } }
    //public class WriteLaserOff : FuntionCommand { public WriteLaserOff() : base("A0", "02") { } }
    //public class WriteLaserOn : FuntionCommand { public WriteLaserOn() : base("A0", "03") { } }
}