using DynMvp.Authentication;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanS.Common;
using UniScanS.Common.Exchange;

namespace UniScanS.Inspector.Exchange
{
    public class ModelExecuter : MachineIfExecuter
    {
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand modelCommand = ExchangeCommand.None;
            bool result = Enum.TryParse(splitCommand[0], out modelCommand);
            
            switch (modelCommand)
            {
                case ExchangeCommand.M_SELECT:
                    SystemManager.Instance().ExchangeOperator.SelectModel(splitCommand.Skip(1).ToArray());
                    break;
                case ExchangeCommand.M_REFRESH:
                    SystemManager.Instance().ModelManager.Refresh();
                    break;
                case ExchangeCommand.M_CLOSE:
                    SystemManager.Instance().ModelManager.CloseModel();
                    break;
                case ExchangeCommand.M_PARAM:
                    ((IClientExchangeOperator)SystemManager.Instance().ExchangeOperator).AlgorithmSettingChanged();
                    break;
                case ExchangeCommand.M_RESET:
                    SystemManager.Instance().ProductionManager.CurProduction?.Reset();
                    SystemManager.Instance().ProductionManager.Save(PathSettings.Instance().Result);
                    break;
                //귀찮아서.........휴
                case ExchangeCommand.U_CHANGE:
                    UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser(splitCommand[1]);
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
