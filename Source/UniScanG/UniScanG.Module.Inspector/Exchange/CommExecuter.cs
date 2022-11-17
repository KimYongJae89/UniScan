using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniScanG.Data.Model;
using UniScanG.Gravure.Device;

namespace UniScanG.Module.Inspector.Exchange
{
    class CommExecuter : Common.Exchange.MachineIfExecuter
    {
        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.C_CONNECTED,
                //ExchangeCommand.C_DISCONNECTED,
                //ExchangeCommand.C_SPD,
                //ExchangeCommand.C_LICENSE,
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand commCommand = IsExcutable(splitCommand[0]);
            bool result = commCommand != ExchangeCommand.None;
            if (!result)
                return false;

            string ip = SystemManager.Instance().ExchangeOperator.GetRemoteIpAddress();
            switch (commCommand)
            {
                case ExchangeCommand.C_CONNECTED:
                    if (ip == splitCommand[3])
                    {
                        ExchangeOperator o = SystemManager.Instance().ExchangeOperator;
                        ExchangeOperator exchangeOperator = SystemManager.Instance().ExchangeOperator;
                        string camIndex = exchangeOperator.GetCamIndex().ToString();
                        string clientIndex = exchangeOperator.GetClientIndex().ToString();
                        object[] locals = new object[] { int.Parse(camIndex), int.Parse(clientIndex), ip, CameraConfiguration.ConfigFlag };
                        object[] remotes = new object[] { int.Parse(splitCommand[1]), int.Parse(splitCommand[2]), splitCommand[3], splitCommand[4] };
                        if (remotes.SequenceEqual(locals))
                        {
                            string version = VersionHelper.Instance().VersionString;
                            string build = VersionHelper.Instance().BuildString;
                            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.C_CONNECTED, camIndex, clientIndex, version, build);
                        }
                        else
                        {
                            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.C_DISCONNECTED, locals.Select(f => f.ToString()).ToArray());

                        }
                    }
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
