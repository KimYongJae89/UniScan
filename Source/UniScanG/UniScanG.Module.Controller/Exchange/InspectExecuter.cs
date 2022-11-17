using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspData;
using DynMvp.Inspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Module.Controller.Exchange
{
    public class InspectExcuter : Common.Exchange.MachineIfExecuter
    {
        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.I_DONE,
                ExchangeCommand.I_LOADFACTOR,
                ExchangeCommand.F_FOUNDED
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand inspectCommand = IsExcutable(splitCommand[0]);
            bool result = inspectCommand != ExchangeCommand.None;
            if (!result)
                return false;

            switch (inspectCommand)
            {
                case ExchangeCommand.I_DONE:
                    if (splitCommand.Count() != 6)
                        return false;

                    InspectionOption inspectionOption = new InspectionOption();
                    InspectionResult inspectionResult = new InspectionResult();
                    inspectionResult.AddExtraResult("Cam", splitCommand[1]);
                    inspectionResult.AddExtraResult("Client", splitCommand[2]);
                    inspectionResult.AddExtraResult("No", splitCommand[3]);
                    inspectionResult.AddExtraResult("Path", splitCommand[4]);
                    inspectionResult.AddExtraResult("Judgment", splitCommand[5]);
                    SystemManager.Instance().InspectRunner?.Inspect(null, IntPtr.Zero, inspectionResult);
                    break;

                case ExchangeCommand.F_FOUNDED:
                    break;

                case ExchangeCommand.I_LOADFACTOR:
                    try
                    {
                        int camera = int.Parse(splitCommand[1]);
                        int client = int.Parse(splitCommand[2]);
                        float loadFactor = float.Parse(splitCommand[3]);

                        InspectorObj inspectorObj = ((IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspector(camera, client);
                        if (inspectorObj != null)
                            inspectorObj.AddLoadFactor(loadFactor);
                    }
                    catch { }
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
