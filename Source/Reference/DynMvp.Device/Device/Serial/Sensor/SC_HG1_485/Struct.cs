using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial.Sensor.SC_HG1_485
{
    public class Frame
    {
        byte start = (byte)':';
        byte slaveAdd;
        Function function;
        byte[] end = new byte[2] { (byte)'\r', (byte)'\n' };

        public Frame(byte slaveAdd, Function function)
        {
            this.slaveAdd = slaveAdd;
            this.function = function;
        }

        public static byte[] CalcCRC(byte[] bytes)
        {
            //● CRC 체크
            //RTU 모드에서는 CRC에 의한 체크를 합니다.
            //아래와 같은 방법으로 CRC를 계산하십시오.

            //1. 16비트 레지스터의 CRC 레지스터에 모두 1의 값을 로드합니다.
            ushort crc = 0xffff;

            foreach (byte crcByte in bytes)
            {
                //2.CRC 레지스터의 하위 바이트와 메시지 프레임의 첫 번째 8비트 데이터의 배타적 논리합(XOR)을 계산합니다.
                crc = (ushort)(crc ^ (ushort)crcByte);

                for (int i = 0; i < 8; i++)
                {
                    //3.최하위 비트가 0인 경우, 스텝 4를 실행합니다. 1인 경우, 스텝 5를 실행합니다.
                    if ((crc & 0x0001) == 0)
                    {
                        //4.결과를 1bit 오른쪽에 시프트합니다.그런 후에 스텝 6을 실행합니다.
                        crc >>= 1;
                    }
                    else
                    {
                        // 5.결과를 1bit 오른쪽에 시프트합니다. 
                        // 그런후에 다항식값 0xA001(1010000000000001)과 시프트한 결과의 배타적 논리합(XOR)을 계산합니다.
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    //6.스텝 3~스텝 5를 8회 반복합니다.
                }
                //7.다음 메시지 프레임의 8비트 데이터를 사용하여 스텝 2부터 스텝 6까지 반복합니다.
            }

            //8.마지막으로 얻어진 값이 CRC입니다.C
            return BitConverter.GetBytes(crc);
        }

        public byte[] CalcCRC()
        {
            List<byte> crcByteRange = new List<byte>();
            crcByteRange.Add(slaveAdd);
            crcByteRange.AddRange(this.function.ToByte());
            return CalcCRC(crcByteRange.ToArray());
        }

        public byte[] ToBytes()
        {
            List<byte> command = new List<byte>();

            command.Add(slaveAdd);
            command.AddRange(function.ToByte());

            command.AddRange(CalcCRC());

            return command.ToArray();
        }
    }

    public abstract class Function
    {
        byte fnCode;
        protected byte[] fnData = null;

        public Function(byte fnCode)
        {
            this.fnCode = fnCode;
        }

        public byte[] ToByte()
        {
            List<byte> byteList = new List<byte>();
            byteList.Add(fnCode);
            byteList.AddRange(fnData);
            return byteList.ToArray();
        }
    }

    public class FnReadCoils : Function
    {
        public FnReadCoils() : base(01) { }
    }

    public class FnReadHoldingRegisters : Function
    {
        public FnReadHoldingRegisters(short startAddr, short count) : base(03)
        {
            byte[] addr = BitConverter.GetBytes(startAddr);
            byte[] cnt = BitConverter.GetBytes(count);

            this.fnData = new byte[4];
            this.fnData[0] = addr[1];
            this.fnData[1] = addr[0];
            this.fnData[2] = cnt[1];
            this.fnData[3] = cnt[0];
        }
    }


    public class FnWriteSingleCoil : Function { public FnWriteSingleCoil() : base(05) { } }

    public class FnWriteSingleRegister : Function
    {
        public FnWriteSingleRegister(short startAddr, short writeData) : base(06)
        {
            byte[] addr = BitConverter.GetBytes(startAddr);
            byte[] data = BitConverter.GetBytes(writeData);

            this.fnData = new byte[4];
            this.fnData[0] = addr[1];
            this.fnData[1] = addr[0];
            this.fnData[2] = data[1];
            this.fnData[3] = data[0];
        }
    }

    public class FnGetComeventcounter : Function { public FnGetComeventcounter() : base(11) { } }
    public class FnGetComeventLog : Function { public FnGetComeventLog() : base(12) { } }
    public class FnWriteMultipleCoils : Function { public FnWriteMultipleCoils() : base(15) { } }
    public class FnWriteMultipleregisters : Function { public FnWriteMultipleregisters() : base(16) { } }
    public class FnReportServerID : Function { public FnReportServerID() : base(17) { } }
    public class FnMaskWriteRegister : Function { public FnMaskWriteRegister() : base(22) { } }
    public class FnReadAndWriteMultipleRegisters : Function { public FnReadAndWriteMultipleRegisters() : base(23) { } }


    public abstract class SubFunction : Function
    {
        byte[] subFnCode = new byte[2];

        protected SubFunction(int subFnCode) : base(08)
        {
            this.subFnCode = BitConverter.GetBytes(subFnCode);
        }
    }

    public class SubFnReturnQueryData : SubFunction { public SubFnReturnQueryData() : base(00) { } }
    public class SubFnRestartCommunicationsOption : SubFunction { public SubFnRestartCommunicationsOption() : base(01) { } }
    public class SubFnForceListenOnlyMode : SubFunction { public SubFnForceListenOnlyMode() : base(04) { } }
    public class SubFnClearCountersandDiagnosticRegister : SubFunction { public SubFnClearCountersandDiagnosticRegister() : base(10) { } }
    public class SubFnReturnBusMessageCount : SubFunction { public SubFnReturnBusMessageCount() : base(11) { } }
    public class SubFnReturnBusCommunicationErrorCount : SubFunction { public SubFnReturnBusCommunicationErrorCount() : base(12) { } }
    public class SubFnReturnBusExceptionCount : SubFunction { public SubFnReturnBusExceptionCount() : base(13) { } }
    public class SubFnReturnServerMessageCount : SubFunction { public SubFnReturnServerMessageCount() : base(14) { } }
    public class SubFnReturnServerNoResponseCount : SubFunction { public SubFnReturnServerNoResponseCount() : base(15) { } }
    public class SubFnReturnServerNAKCount : SubFunction { public SubFnReturnServerNAKCount() : base(16) { } }
    public class SubFnReturnServerBusyCount : SubFunction { public SubFnReturnServerBusyCount() : base(17) { } }
    public class SubFnReturnBusCharacterOverrunCount : SubFunction { public SubFnReturnBusCharacterOverrunCount() : base(18) { } }
    public class SubFnClearOverrunCounterandFlag : SubFunction { public SubFnClearOverrunCounterandFlag() : base(20) { } }

}