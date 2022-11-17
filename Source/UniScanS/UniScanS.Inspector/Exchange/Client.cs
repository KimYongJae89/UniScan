using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.Inspector;

namespace UniScanS.Inspector.Exchange
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
            AddExecuter(new InspectExecuter());
            AddExecuter(new ModelExecuter());
            AddExecuter(new VisitExecuter());

            base.Initialize();
            //Start();
        }

        protected override TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new ExchangePacketParser(exchangeProtocolList);
        }
        
        public void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            SendCommand(exchangeProtocolList, exchangeCommand, args);
        }
    }
}
