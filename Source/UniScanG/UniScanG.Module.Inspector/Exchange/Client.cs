using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanG.Common.Exchange;
using UniScanG.Module.Inspector.Settings.Inspector;

namespace UniScanG.Module.Inspector.Exchange
{
    class Client : TcpIpMachineIfClient
    {
        ExchangeProtocolList exchangeProtocolList;

        public Client(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            exchangeProtocolList = (ExchangeProtocolList)InspectorSystemSettings.Instance().ClientSetting.MachineIfProtocolList;

            Initialize();
        }

        public override void Initialize()
        {
            AddExecuter(new CommExecuter());
            AddExecuter(new InspectExecuter());
            AddExecuter(new ModelExecuter());
            AddExecuter(new VisitExecuter());

            base.Initialize();
        }

        protected override TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new ExchangePacketParser(exchangeProtocolList);
        }

        protected override void ClientSocket_OnConnectAsyncCompleted()
        {
            int camId = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            int clientId = SystemManager.Instance().ExchangeOperator.GetClientIndex();

            //SendCommand(ExchangeCommand.C_CONNECTED, camId.ToString(), clientId.ToString());
        }

        public void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            SendCommand(exchangeProtocolList, exchangeCommand, args);
        }
    }
}
