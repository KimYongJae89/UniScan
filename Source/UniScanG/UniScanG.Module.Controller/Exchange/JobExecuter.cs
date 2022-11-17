using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Data.Model;

namespace UniScanG.Module.Controller.Exchange
{
    public class JobExecuter : Common.Exchange.MachineIfExecuter
    {
        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.J_RUNNING,
                ExchangeCommand.J_DONE,
                ExchangeCommand.J_ERROR
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand jobCommand = IsExcutable(splitCommand[0]);
            bool result = jobCommand != ExchangeCommand.None;
            if (!result)
                return false;

            result = true;
            JobState jobState = JobState.Idle;
            switch (jobCommand)
            {
                case ExchangeCommand.J_RUNNING:
                    jobState = JobState.Run;
                    break;
                case ExchangeCommand.J_DONE:
                    jobState = JobState.Done;
                    break;
                case ExchangeCommand.J_ERROR:
                    jobState = JobState.Error;
                    break;
                default:
                    result = false;
                    break;
            }

            if (result)
            {
                ExchangeCommand comm;
                Enum.TryParse(splitCommand[1], out comm);
                int camId = int.Parse(splitCommand[2]);
                int clientId = int.Parse(splitCommand[3]);
                object[] jobResult = splitCommand.Skip(4).ToArray();

                Common.IServerExchangeOperator serverExchangeOperator = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                InspectorObj inspectorObj = serverExchangeOperator.GetInspector(camId, clientId);
                if (inspectorObj != null)
                {
                    inspectorObj.SetJobState(jobState, comm, jobResult);

                    if (clientId < 0)
                    {
                        List<InspectorObj> objList;
                        if (SystemTypeSettings.Instance().LocalExchangeMode)
                            objList = serverExchangeOperator.GetInspectorList().FindAll(f => f.Info.CamIndex == inspectorObj.Info.CamIndex);
                        else
                            objList = serverExchangeOperator.GetInspectorList().FindAll(f => f.Info.IpAddress == inspectorObj.Info.IpAddress);

                        objList.Remove(inspectorObj);
                        objList.ForEach(f => f.SetJobState(jobState, comm, jobResult));
                    }
                }
            }

            return result;
        }
    }
}
