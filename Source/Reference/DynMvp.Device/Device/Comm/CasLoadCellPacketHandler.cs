using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public class CasLoadCellReceivedPacket : ReceivedPacket
    {
        float resultValue;
        public float ResultValue
        {
            get { return resultValue; }
            set { resultValue = value; }
        }
    }
    


    public class CasLoadCellPacketParser : SimplePacketParser
    {
        string lastString;

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            processedPacketCnt = packetContents.Length;
            return false;
            //string packetString = System.Text.Encoding.Default.GetString(dataByte.ToArray());
            //LogHelper.Debug(LoggerType.Serial, String.Format("Received Packet : {0}", packetString));

            //if (dataByte.Length >= 21)
            //{
            //    string currentString = System.Text.Encoding.Default.GetString(dataByte.ToArray());
            //    //if (lastString == currentString)
            //    //    return true;

            //    LogHelper.Debug(LoggerType.Serial, String.Format("Received Valid Packet : {0}", currentString));
            //    lastString = currentString;

            //    int startPos = GetValidStartPos(currentString);

            //    if (startPos > -1)
            //    {
            //        string onePacket = currentString.Substring(startPos);
            //        string startString = onePacket.Substring(0, 2);

            //        char sign = onePacket[8];
            //        string weightString = onePacket.Substring(9, 8).Trim();

            //        try
            //        {
            //            float resultValue = (float)Convert.ToDouble(weightString);

            //            if (sign == '-')
            //                resultValue *= -1;

            //            if (OnDataReceived != null)
            //            {
            //                CasLoadCellReceivedPacket receivedPacket = new CasLoadCellReceivedPacket();
            //                receivedPacket.ResultValue = resultValue;
            //                OnDataReceived(receivedPacket);
            //            }
            //            else
            //            {
            //                LogHelper.Debug(LoggerType.Serial, "DataReceived is Empty");
            //            }
            //        }
            //        catch (FormatException ex)
            //        {
            //            LogHelper.Debug(LoggerType.Serial, String.Format("Invalid string format : {0} - {1}", weightString, ex.Message));
            //        }
            //    }
            //}

            //return true;
        }

        private int GetValidStartPos(string fullString)
        {
            string[] validStarters = new string[] { "ST", "US", "OL" };

            foreach (string starter in validStarters)
            {
                int startPos = fullString.IndexOf(starter);
                if (startPos > -1)
                    return startPos;
            }

            return -1;
        }
    }

    public class CasLoadCellPacketHandler : PacketHandler
    {
        public CasLoadCellPacketHandler()
        {
            CasLoadCellPacketParser casLoadCellPacketParser = new CasLoadCellPacketParser();
            casLoadCellPacketParser.EndChar = Encoding.ASCII.GetBytes("\n");

            packetParser = casLoadCellPacketParser;
        }
    }
}
