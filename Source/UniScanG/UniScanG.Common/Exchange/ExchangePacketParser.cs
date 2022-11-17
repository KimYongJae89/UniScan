using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;

namespace UniScanG.Common.Exchange
{
    public class ExchangePacketParser : TcpIpMachineIfPacketParser
    {
        MachineIfProtocolList machineIfProtocolList; 

        public ExchangePacketParser(MachineIfProtocolList machineIfProtocolList)
        {
            this.machineIfProtocolList = machineIfProtocolList;
        }

        public override MachineIfProtocolWithArguments BreakPacket(string packet)
        {
            string trimString = packet.Trim('\0');
            if (trimString.Length < startChar.Length)
                return null;
            trimString =trimString.Substring(startChar.Length);
            trimString =trimString.Substring(0, trimString.Length - endChar.Length);

            string[] token = trimString.Split(',');

            Enum e = (Enum)Enum.Parse(typeof(ExchangeCommand), token[0]);

            TcpIpMachineIfProtocol tcpIpMachineIfProcotol = (TcpIpMachineIfProtocol)machineIfProtocolList.GetProtocol(e);
            token = token.Skip(1).ToArray();
            return new MachineIfProtocolWithArguments(tcpIpMachineIfProcotol, token);
        }
    }
}
