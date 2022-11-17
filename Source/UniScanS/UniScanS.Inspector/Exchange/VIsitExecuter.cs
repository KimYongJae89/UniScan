using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanS.Common;
using UniScanS.Common.Exchange;

namespace UniScanS.Inspector.Exchange
{
    public class VisitExecuter : MachineIfExecuter
    {
        char separator = ';';
        
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand visitCommand;
            bool result = Enum.TryParse(splitCommand[0], out visitCommand);

            InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;

            switch (visitCommand)
            {
                case ExchangeCommand.V_INSPECT:
                case ExchangeCommand.V_MODEL:
                case ExchangeCommand.V_REPORT:
                case ExchangeCommand.V_SETTING:
                case ExchangeCommand.V_TEACH:
                    client.PreparePanel(visitCommand);
                    break;
                case ExchangeCommand.V_DONE:
                    client.ClearPanel();
                    break;
                default:
                    result = false;
                    break;
            }
            
            return result;
        }
    }
}
