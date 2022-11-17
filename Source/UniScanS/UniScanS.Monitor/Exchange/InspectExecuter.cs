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
using UniScanS.Common;
using UniScanS.Common.Exchange;

namespace UniScanS.Monitor.Exchange
{
    public class InspectExcuter : MachineIfExecuter
    {
        //ImageDevice imageDevice = new CameraVirtual();
        
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand inspectCommand;
            bool result = Enum.TryParse(splitCommand[0], out inspectCommand);

            if (result == false)
                return false;

            switch (inspectCommand)
            {
                case ExchangeCommand.I_DONE:
                    if (splitCommand.Count() < 3)
                        return false;
                    InspectionOption inspectionOption = new InspectionOption();
                    InspectionResult inspectionResult = new InspectionResult();
                    inspectionResult.AddExtraResult("Cam", splitCommand[1]);
                    inspectionResult.AddExtraResult("No", splitCommand[2]);
                    inspectionResult.AddExtraResult("Time", splitCommand[3]);
                    LogHelper.Info(LoggerType.Inspection, string.Format("Cam : {0}, No : {1}", splitCommand[1], splitCommand[2]));
                    SystemManager.Instance().InspectRunner?.Inspect(null, IntPtr.Zero, inspectionResult);
                    break;
                case ExchangeCommand.F_FOUNDED:
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.F_SET, splitCommand[1], splitCommand[2], splitCommand[3]);
                    break;
            }

            return true;
        }
    }
}
