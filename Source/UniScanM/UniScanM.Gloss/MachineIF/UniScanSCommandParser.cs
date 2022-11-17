using DynMvp.Devices.Comm;
using System;
using System.Text;

namespace UniScanM.Gloss.MachineIF
{
    public class UniScanSCommandParser
    {
        public CommandInfo Parse(ReceivedPacket packet)
        {
            var CommandInfo = new CommandInfo();
            CommandInfo.Sender = packet.SenderInfo;
            string receivedDataStrting = Encoding.Default.GetString(packet.ReceivedData);
            string[] tokens = receivedDataStrting.Split(',');
            int tokenIndex = 0;
            if (Enum.TryParse<EUniScanSCommand>(tokens[tokenIndex++], out EUniScanSCommand command))
            {
                CommandInfo.Command = command;

                for (int i = tokenIndex; i < tokens.Length; i++)
                    CommandInfo.Parameters.Add(tokens[i]);
            }
            else
            {
                CommandInfo.Command = EUniScanSCommand.Unknown;
            }

            return CommandInfo;
        }
    }
}
