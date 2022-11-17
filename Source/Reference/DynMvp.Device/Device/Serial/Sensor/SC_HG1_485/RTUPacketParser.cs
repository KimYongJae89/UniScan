using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial.Sensor.SC_HG1_485
{
    public class RTUPacketParser : PacketParser
    {
        List<byte> byteList;
        
        public RTUPacketParser()
        {
            this.byteList = new List<byte>();
        }

        public override PacketParser Clone()
        {
            throw new NotImplementedException();
        }

        public override string DecodePacket(byte[] packet)
        {
            return BitConverter.ToString(packet);
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }

        public override bool ParsePacket(byte[] PacketContents, out int processedPacketCnt)
        {
            byteList.AddRange(PacketContents);
            processedPacketCnt = PacketContents.Length;

            if (byteList.Count >= 4)
            {
                byte slave = byteList[0];
                byte code = byteList[1];
                if ((code & 0x80) == 0)
                // 정상응답
                {
                    //데이터 처리 완료 전에 다음 데이터가 들어오면 망함...
                    byte[] data = byteList.GetRange(2, byteList.Count - 2).ToArray();
                    byte[] crc = byteList.GetRange(byteList.Count - 2, 2).ToArray();

                    byte[] crcBytes = byteList.GetRange(0, byteList.Count - 2).ToArray();
                    byte[] calcCrc = Frame.CalcCRC(crcBytes);
                    if (crc[0] == calcCrc[0] && crc[1] == calcCrc[1])
                    {
                        ReceivedPacket receivedPacket = new ReceivedPacket(byteList.ToArray());
                        this.OnDataReceived(receivedPacket);
                        byteList.Clear();
                    }
                }
                else
                {
                    ReceivedPacket receivedPacket = new ReceivedPacket(byteList.ToArray(), code.ToString());
                    this.OnDataReceived(receivedPacket);
                    byteList.Clear();
                }
            }
            return true;
        }
    }
}
