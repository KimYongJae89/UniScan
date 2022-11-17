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
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Data.Model;
using UniScanG.Gravure.Device;
using UniScanG.Module.Controller.MachineIF;

namespace UniScanG.Module.Controller.Exchange
{
    class CommExecuter: Common.Exchange.MachineIfExecuter
    {
        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.C_CONNECTED,
                ExchangeCommand.C_DISCONNECTED,
                ExchangeCommand.C_SPD,
                ExchangeCommand.C_LICENSE,
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand commCommand = IsExcutable(splitCommand[0]);
            bool result = commCommand != ExchangeCommand.None;
            if (!result)
                return false;

            switch (commCommand)
            {
                case ExchangeCommand.C_CONNECTED:
                    {
                        int camId = int.Parse(splitCommand[1]);
                        int clientId = int.Parse(splitCommand[2]);
                        string version = splitCommand.ElementAtOrDefault(3)?.ToString();
                        string build = splitCommand.ElementAtOrDefault(4)?.ToString();

                        InspectorObj inspectorObj = ((IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspector(camId, clientId);
                        inspectorObj.CommState = CommState.CONNECTED;
                        inspectorObj.InspectState = UniEye.Base.Data.InspectState.Done;
                        inspectorObj.OpState = UniEye.Base.Data.OpState.Idle;
                        inspectorObj.ResetJobState();
                        inspectorObj.ModelManager.Init(System.IO.Path.Combine(inspectorObj.Info.Path, "Model"));

                        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(build))
                        {
                            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected,
                                ErrorLevel.Info, inspectorObj.Info.GetName(), "Inspector Connected.", null, ""));
                        }
                        else
                        {
                            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected,
                                                     ErrorLevel.Info, inspectorObj.Info.GetName(), "Inspector Connected. (V{0}, B{1})", new string[] { version, build }, ""));
                        }

                        if (SystemManager.Instance().CurrentModel != null)
                        {
                            Task.Run(() =>
                            {
                                Common.Data.ModelDescription modelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
                                ((MonitorOperator)SystemManager.Instance().ExchangeOperator).SelectRemoteModel(camId, clientId, modelDescription);
                            });
                        }
                    }
                    break;

                case ExchangeCommand.C_DISCONNECTED:
                    {
                        int camId = int.Parse(splitCommand[1]);
                        int clientId = int.Parse(splitCommand[2]);
                        string ip = splitCommand[3];
                        string mode = splitCommand[4];
                        InspectorObj inspectorObj = ((IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspector(camId, clientId);
                        if (inspectorObj != null)
                        {
                            inspectorObj.CommState = Common.Data.CommState.DISCONNECTED;
                            inspectorObj.InspectState = UniEye.Base.Data.InspectState.Done;
                            inspectorObj.OpState = UniEye.Base.Data.OpState.Idle;
                            inspectorObj.ResetJobState();
                            string inspectorObjName = inspectorObj.Info.GetName();
                            ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Comms.Disconnected, ErrorLevel.Info,
                                inspectorObjName, "IM has wrong setting. (Camera: {0}, Client: {1}, IP: {2}, Mode: {3})", 
                                new object[] { camId, clientId, ip, string.IsNullOrEmpty(mode) ? "None" : mode });
                        }
                    }
                    break;


                case ExchangeCommand.C_SPD:
                    MachineIfData machineIfData = ((DeviceControllerG)SystemManager.Instance().DeviceController).MachineIfMonitor?.MachineIfData as MachineIfData;
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.C_SPD, machineIfData.GET_START_GRAVURE_INSPECTOR.ToString(), machineIfData.GET_PRESENT_SPEED_REAL.ToString());
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
