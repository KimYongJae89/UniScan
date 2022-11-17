using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanS.Common.Exchange;

namespace UniScan.Inspector.Exchange
{
    public class JobExecuter : MachineIfExecuter
    {
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand jobCommand;
            bool result = Enum.TryParse(splitCommand[0], out jobCommand);

            if (result == false)
                return false;

            switch (jobCommand)
            {
                case ExchangeCommand.J_DONE:
                    break;
                case ExchangeCommand.J_ERROR:
                    break;
            }

            return true;
        }
    }
}
