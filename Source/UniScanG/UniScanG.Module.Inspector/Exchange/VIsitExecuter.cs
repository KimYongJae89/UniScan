using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Common;
using UniScanG.Common.Exchange;

namespace UniScanG.Module.Inspector.Exchange
{
    public class VisitExecuter : UniScanG.Common.Exchange.MachineIfExecuter
    {
        char separator = ';';

        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.V_SHOW,
                ExchangeCommand.V_HIDE,
                ExchangeCommand.V_INSPECT,
                ExchangeCommand.V_MODEL,
                ExchangeCommand.V_REPORT,
                ExchangeCommand.V_SETTING,
                ExchangeCommand.V_LOG,
                ExchangeCommand.V_TEACH,
                ExchangeCommand.V_DONE
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(separator);

            ExchangeCommand visitCommand = IsExcutable(splitCommand[0]);
            bool result = visitCommand != ExchangeCommand.None;
            if (!result)
                return false;

            InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;

            switch (visitCommand)
            {
                case ExchangeCommand.V_SHOW:
                case ExchangeCommand.V_HIDE:

                case ExchangeCommand.V_INSPECT:
                case ExchangeCommand.V_MODEL:
                case ExchangeCommand.V_TEACH:
                case ExchangeCommand.V_REPORT:
                case ExchangeCommand.V_LOG:
                case ExchangeCommand.V_SETTING:
                    client.PreparePanel(visitCommand);
                    break;
                case ExchangeCommand.V_DONE:    // VNC 접속 종료
                    client.PreparePanel(ExchangeCommand.V_INSPECT);
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
