using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynMvp.Device.Device.Serial.Sensor.CD22_15_485
{
    public class CD22PacketParser : PacketParser
    {
        List<byte> byteList;

        public CD22PacketParser()
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
            //System.Diagnostics.Debug.WriteLine(string.Format("PacketContents.Length = {0}", PacketContents.Length));
            byteList.AddRange(PacketContents);
            processedPacketCnt = PacketContents.Length;

            if (byteList.Count >= 6)
            {
                if (byteList.Count > 20)
                {
                    byteList.Clear();
                    return false;
                }
                int stxIndex = byteList.FindIndex(x => x.ToString() == "2");
                //int etxIndex = byteList.FindIndex(x => x.ToString() == "3");
                if (stxIndex < 0)
                    return false;

                List<byte> tempByteList = byteList.GetRange(stxIndex, 6);
                byteList.RemoveRange(stxIndex, 6);

                byte stx = tempByteList[0];
                byte code = tempByteList[1];
                byte[] data = tempByteList.GetRange(2, 2).ToArray();
                byte etx = tempByteList[4];
                byte[] bcc = tempByteList.GetRange(5, 1).ToArray();

                byte[] bccBytes = tempByteList.GetRange(1, 3).ToArray();
                byte[] calcBcc = Frame.CalcBCC(bccBytes);

                if (bcc[0] == calcBcc?[0])
                {
                    ReceivedPacket receivedPacket = new ReceivedPacket(tempByteList.ToArray());
                    this.OnDataReceived(receivedPacket);
                    tempByteList.Clear();
                }
                else
                {
                    ReceivedPacket receivedPacket = new ReceivedPacket(tempByteList.ToArray(), code.ToString());
                    this.OnDataReceived(receivedPacket);
                    tempByteList.Clear();
                }
                return true;
            }
            return false;
        }
    }
}
