using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.MachineIF;

using UniScanM.RVMS.Settings;
using UniScanM.RVMS.MachineIF;
using UniScanM.Data;

namespace UniScanM.RVMS.Operation
{
    public class PLCInspectStarter : UniScanM.Operation.PLCInspectStarter
    {
        public PLCInspectStarter() : base()
        {

        }
        
        public override void SetReadySignal(bool imReady = true) //0: off or manual, 1: on and Auto-mode
        {
            //if(Auto-mode)
            //string mode = automode ? "1" : "0";
            //PLC가 읽고 clear 함으로 매뉴얼 모드일때는 굳이 쓸필요없음.
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfRVMSCommand.SET_RVMS_READY, "1");  // Update PLC
        }

        public override void SetRunningSignal(bool running = true) //0: idle, 1: run@ only auto-mode, Not Manual-Mode
        {
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfRVMSCommand.SET_RVMS_RUN, "1");  // Update PLC
        }                                         

        public override bool GetAutoStartSignal()
        {
            return melsecMonitor.State.RvmsOnStart;
        }

        protected override void SetFirstValue()
        {
            string mergeString =
                string.Format("{0}{1}{2}{3}{4}{5}",
                string.Format("{0:X04}", (short)(0))
                , string.Format("{0:X04}", (short)(0))
                , string.Format("{0:X04}", (short)(0))
                , "0000"
                , MelsecDataConverter.WInt((int)(0))
                , MelsecDataConverter.WInt((int)(0))
                );
                                                                      
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfRVMSCommand.SET_RVMS, mergeString);

        }
    }
}