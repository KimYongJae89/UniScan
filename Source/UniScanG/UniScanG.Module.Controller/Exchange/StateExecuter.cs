using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Module.Controller.Exchange
{
    public class StateExecuter : Common.Exchange.MachineIfExecuter
    {
        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.S_IDLE,
                ExchangeCommand.S_OpWait,
                ExchangeCommand.S_InspWAIT,
                ExchangeCommand.S_RUN,
                ExchangeCommand.S_INSPECT,
                ExchangeCommand.S_PAUSE,
                ExchangeCommand.S_TEACH,
                ExchangeCommand.S_DONE,
                ExchangeCommand.S_ALARM,
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand stateCommand = IsExcutable(splitCommand[0]);
            if(stateCommand == ExchangeCommand.None)
                return false;

            int camId = int.Parse(splitCommand[1]);
            int cliendId = int.Parse(splitCommand[2]);
            string message = splitCommand.Length > 3 ? splitCommand[3] : "";

            switch (stateCommand)
            {
                case ExchangeCommand.S_IDLE:
                case ExchangeCommand.S_OpWait:
                case ExchangeCommand.S_InspWAIT:
                case ExchangeCommand.S_RUN:
                case ExchangeCommand.S_INSPECT:
                case ExchangeCommand.S_PAUSE:
                case ExchangeCommand.S_TEACH:
                case ExchangeCommand.S_DONE:
                    StateChanged(camId, cliendId, stateCommand, message);
                    return true;

                case ExchangeCommand.S_ALARM:
                    StateChanged(camId, cliendId, stateCommand, message);
                    if(SystemManager.Instance().InspectRunner.IsOnInspect())
                        AlarmReport(ErrorLevel.Error, message);
                    return true;

                default:
                    return false;
            }
        }

        private void StateChanged(int camId, int clientId, ExchangeCommand command, string message)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f=>f.Info.CamIndex == camId);

            foreach (InspectorObj inspector in inspectorList)
            {
                if (clientId < 0 || inspector.Info.ClientIndex == clientId)
                {
                    inspector.OpMessage = message;
                    switch (command)
                    {
                        case ExchangeCommand.S_IDLE:
                            inspector.OpState = UniEye.Base.Data.OpState.Idle;
                            break;
                        case ExchangeCommand.S_OpWait:
                            inspector.OpState = UniEye.Base.Data.OpState.Wait;
                            break;
                        case ExchangeCommand.S_TEACH:
                            inspector.OpState = UniEye.Base.Data.OpState.Teach;
                            break;
                        case ExchangeCommand.S_INSPECT:
                            inspector.OpState = UniEye.Base.Data.OpState.Inspect;
                            break;
                        case ExchangeCommand.S_ALARM:
                            inspector.OpState = UniEye.Base.Data.OpState.Alarm;
                            UniEye.Base.Data.SystemState.Instance().SetAlarm(message);
                            break;

                        case ExchangeCommand.S_PAUSE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Pause;
                            break;
                        case ExchangeCommand.S_RUN:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Run;
                            break;
                        case ExchangeCommand.S_InspWAIT:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Wait;
                            break;
                        case ExchangeCommand.S_DONE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                            break;
                    }
                }
            }
        }

        private void AlarmReport(ErrorLevel errorLevel, string message)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f => f.OpState == UniEye.Base.Data.OpState.Alarm);
            inspectorList.ForEach(f =>
            {
                ErrorManager.Instance().Report(UniScanG.Data.ErrorCodeInspect.Instance.InvalidState, errorLevel, f.Info.GetName(), message, null, "");

            });
        }
    }
}