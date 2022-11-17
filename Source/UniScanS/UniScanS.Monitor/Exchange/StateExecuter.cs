using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;

namespace UniScan.Inspector.Exchange
{
    public class StateExecuter : MachineIfExecuter
    {
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand stateCommand;
            bool result = Enum.TryParse(splitCommand[0], out stateCommand);

            if (result == false)
                return false;
            
            switch (stateCommand)
            {
                case ExchangeCommand.S_IDLE:
                case ExchangeCommand.S_WAIT:
                case ExchangeCommand.S_RUN:
                case ExchangeCommand.S_INSPECT:
                case ExchangeCommand.S_PAUSE:
                case ExchangeCommand.S_TEACH:
                case ExchangeCommand.S_DONE:
                case ExchangeCommand.S_ALARM:
                    StateChanged(splitCommand[1], stateCommand);
                    break;
                case ExchangeCommand.M_TEACH_DONE:
                    SystemManager.Instance().ExchangeOperator.ModelTeachDone(splitCommand[1]);
                    break;
            }

            return true;
        }

        private void StateChanged(string indexString, ExchangeCommand command)
        {
            int index = Convert.ToInt32(indexString);
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList();

            foreach (InspectorObj inspector in inspectorList)
            {
                if (inspector.Info.CamIndex == index)
                {
                    switch (command)
                    {
                        case ExchangeCommand.S_IDLE:
                            inspector.OpState = UniEye.Base.Data.OpState.Idle;
                            break;
                        case ExchangeCommand.S_TEACH:
                            inspector.OpState = UniEye.Base.Data.OpState.Teach;
                            break;
                        case ExchangeCommand.S_INSPECT:
                            inspector.OpState = UniEye.Base.Data.OpState.Inspect;
                            break;
                        case ExchangeCommand.S_PAUSE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Pause;
                            break;
                        case ExchangeCommand.S_RUN:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Run;
                            break;
                        case ExchangeCommand.S_WAIT:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Wait;
                            break;
                        case ExchangeCommand.S_DONE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                            break;
                    }
                }
            }
        }
    }
}