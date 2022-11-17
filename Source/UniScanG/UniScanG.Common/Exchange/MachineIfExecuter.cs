using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Common.Exchange
{
    public abstract class MachineIfExecuter : UniEye.Base.MachineInterface.MachineIfExecuter
    {
        protected List<ExchangeCommand> exchangeCommandList = new List<ExchangeCommand>();

        //public abstract bool Execute(string command);

        protected abstract void AddExchangeCommand();

        public MachineIfExecuter()
        {
            AddExchangeCommand();
        }


        protected ExchangeCommand IsExcutable(string command)
        {
            ExchangeCommand exchangeCommand;
            if(Enum.TryParse(command,out exchangeCommand))
            {
                if (this.exchangeCommandList.Contains(exchangeCommand))
                    return exchangeCommand;
            }

            return ExchangeCommand.None;
        }

    }
}
