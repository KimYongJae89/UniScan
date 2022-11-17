using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Comm
{
    public class RobotListeningParser : PacketParser
    {
        public override PacketParser Clone()
        {
            return new RobotListeningParser();
        }

        public override string DecodePacket(byte[] packet)
        {
            throw new NotImplementedException();
        }

        public override byte[] EncodePacket(string protocol)
        {
            throw new NotImplementedException();
        }

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            processedPacketCnt = packetContents.Length;
            return false;
            //string contentsString = System.Text.Encoding.Default.GetString(packetContents.ToArray());

            //ReceivedPacket receivedPacket = new ReceivedPacket();
            //receivedPacket.ReceivedData = contentsString;

            //if (OnDataReceived != null)
            //    OnDataReceived(receivedPacket);

            //return true;
        }
    }
}
