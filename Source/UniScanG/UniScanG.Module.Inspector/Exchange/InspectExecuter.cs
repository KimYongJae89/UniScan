using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Common;
using UniEye.Base.MachineInterface;
using UniScanG.Common.Exchange;
using UniEye.Base.Data;
using UniScanG.Data;
using DynMvp.Base;

namespace UniScanG.Module.Inspector.Exchange
{
    public class InspectExecuter : UniScanG.Common.Exchange.MachineIfExecuter
    {
        Task asyncTask = null;

        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.I_LOTCHANGE,
                ExchangeCommand.I_PAUSE,
                ExchangeCommand.I_READY,
                ExchangeCommand.I_START,
                ExchangeCommand.I_TEACH,
                ExchangeCommand.I_LIGHT,
                ExchangeCommand.I_STOP
            });
        }


        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;

            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand inspectCommand = IsExcutable(splitCommand[0]);
            bool result = inspectCommand != ExchangeCommand.None;
            if (!result)
                return false;

            switch (inspectCommand)
            {
                case ExchangeCommand.I_LOTCHANGE:
                    UniScanG.Data.ProductionManager productionManager = SystemManager.Instance().ProductionManager as UniScanG.Data.ProductionManager;
                    if (splitCommand.Length == 4)
                        productionManager.LotChange(SystemManager.Instance().CurrentModel, DateTime.Now, splitCommand[1], (RewinderZone)Enum.Parse(typeof(RewinderZone), splitCommand[2]), Convert.ToSingle(splitCommand[3]));
                    else
                        productionManager.LotChange(SystemManager.Instance().CurrentModel, DateTime.Now, splitCommand[1], RewinderZone.Unknowen, 0);
                    break;

                case ExchangeCommand.I_PAUSE:
                    SystemManager.Instance().InspectRunner.EnterPauseInspection();
                    break;

                case ExchangeCommand.I_READY:
                    //client.PreparePanel(ExchangeCommand.V_INSPECT);
                    SystemManager.Instance().ProductionManager.CurProduction?.Reset();
                    //ProductionBase curProduction = null;
                    //if (splitCommand.Length > 2)
                    //    curProduction = productionManager.LotChange(SystemManager.Instance().CurrentModel, DateTime.Now, splitCommand[1], Convert.ToSingle(splitCommand[2]));
                    //else
                    //    curProduction = productionManager.LotChange(SystemManager.Instance().CurrentModel, DateTime.Now, splitCommand[1], 0);

                    SystemManager.Instance().InspectRunner.EnterWaitInspection();
                    break;
                case ExchangeCommand.I_START:
                    SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                    break;
                case ExchangeCommand.I_TEACH:
                    if (asyncTask == null || asyncTask.IsCompleted)
                        asyncTask = Task.Run(() =>
                        {
                            if (splitCommand.Length == 1)
                                SystemManager.Instance().ModellerPageExtender.AutoTeachProcess(0);
                            else
                                SystemManager.Instance().ModellerPageExtender.AutoTeachProcess(float.Parse(splitCommand[1]));
                        });
                    break;
                case ExchangeCommand.I_LIGHT:
                    if (asyncTask == null || asyncTask.IsCompleted)
                        asyncTask = Task.Run(() =>
                        {
                            float[] histog = SystemManager.Instance().ModellerPageExtender.AutoLigthProcess(float.Parse(splitCommand[1]));
                            List<string> argList = new List<string>();
                            argList.Add(ExchangeCommand.I_LIGHT.ToString());
                            argList.Add(client.GetCamIndex().ToString());
                            argList.Add(client.GetClientIndex().ToString());
                            argList.AddRange(Array.ConvertAll(histog, f => f.ToString("F2")));

                            client.SendCommand(ExchangeCommand.J_DONE, argList.ToArray());
                        });
                    break;
                case ExchangeCommand.I_STOP:
                    if (SystemState.Instance().OpState != OpState.Idle)
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    //SystemManager.Instance().InspectRunnerG.InspectStarter.ExitWaitInspection();
                    ErrorManager.Instance().ResetAllAlarm();
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
