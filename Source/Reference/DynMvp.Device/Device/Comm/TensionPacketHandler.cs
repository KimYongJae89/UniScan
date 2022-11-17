using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using System.Diagnostics;

namespace DynMvp.Devices.Comm
{
    public class TensionPacketHandler : PacketHandler
    {
        public TensionPacketHandler()
        {
            TensionPacketPaser tensionPacketPaser = new TensionPacketPaser();
            tensionPacketPaser.EndChar = Encoding.ASCII.GetBytes("\r");
            packetParser = tensionPacketPaser;
        }
    }

    public class TensionReceivedPacket : ReceivedPacket
    {
        string resultValue;
        public string ResultValue
        {
            get { return resultValue; }
            set { resultValue = value; }
        }
    }
    
    public class TensionPacketPaser : SimplePacketParser
    {
        string lastString;

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            processedPacketCnt = packetContents.Length;
            return false;
            //string currentString = System.Text.Encoding.Default.GetString(PacketContents.ToArray());

            //LogHelper.Debug(LoggerType.Serial, String.Format("Received Valid Packet : {0}", currentString));
            //lastString = currentString;
            //try
            //{
            //    if (OnDataReceived != null)
            //    {
            //        TensionReceivedPacket receivedPacket = new TensionReceivedPacket();
            //        string result = lastString; //.Remove(0 , 3);
            //        float convertResult = Convert.ToSingle(result);

            //        receivedPacket.ResultValue = convertResult.ToString();
            //        OnDataReceived(receivedPacket);
            //    }
            //    else
            //    {
            //        LogHelper.Debug(LoggerType.Serial, "DataReceived is Empty");
            //    }
            //}
            //catch (FormatException ex)
            //{
            //    LogHelper.Debug(LoggerType.Serial, String.Format("Invalid string format"));
            //}

            //return true;
        }
    }
}
