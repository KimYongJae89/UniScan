using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Common;
using UniScanG.Common.Exchange;

namespace UniScanG.Module.Controller.Exchange
{
    internal class ModelExecuter : Common.Exchange.MachineIfExecuter
    {
        static object modelSaveLocker = new object();

        public ModelExecuter() : base() { }

        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.M_TEACH_DONE
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
            float chipShare = 0;

            if (splitCommand.Length > 3)
                float.TryParse(splitCommand[3], out chipShare);

            // Master IM만 동기화 함.
            if (cliendId > 0)
                return true;

            switch (stateCommand)
            {
                case ExchangeCommand.M_TEACH_DONE:
                    Task.Run(() =>
                    {
                        IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                        server.SyncModel(camId);
                        lock (modelSaveLocker)
                        {
                            if (camId == 0 && chipShare > 0)
                                SystemManager.Instance().CurrentModel.ChipShare100p = chipShare;
                            SystemManager.Instance().ExchangeOperator.SaveModel();
                            SystemManager.Instance().ExchangeOperator.ModelTeachDone(camId);
                        }
                    });
                    return true;

                default:
                    return false;
            }
        }
    }
}
