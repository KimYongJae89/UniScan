using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.MachineInterface;
using UniScanS.Common.Exchange;
using UniScanS.Data;

namespace UniScanS.Inspector.Exchange
{
    public class InspectExecuter : MachineIfExecuter
    {
        char separator = ';';
        
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand inspectCommand;
            bool result = Enum.TryParse(splitCommand[0], out inspectCommand);
            
            switch (inspectCommand)
            {
                case ExchangeCommand.I_LOTCHANGE:
                    break;
                case ExchangeCommand.I_ENTER_PAUSE:
                    SystemManager.Instance().InspectRunner.EnterPauseInspection();
                    break;
                case ExchangeCommand.I_EXIT_PAUSE:
                    //SystemManager.Instance().InspectRunner.ExitPauseInspection();
                    break;
                case ExchangeCommand.I_START:
                    ((ProductionManagerS)SystemManager.Instance().ProductionManager).LotChange(SystemManager.Instance().CurrentModel, splitCommand);
                    SystemManager.Instance().InspectRunner.EnterWaitInspection();
                    break;
                case ExchangeCommand.I_STOP:
                    SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    break;
                case ExchangeCommand.F_SET:
                    ((UniScanS.Screen.Inspect.InspectRunnerInspectorS)SystemManager.Instance().InspectRunner).AdjustFidOffset(Convert.ToBoolean(splitCommand[1]), Convert.ToSingle(splitCommand[2]), Convert.ToSingle(splitCommand[3]));
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
