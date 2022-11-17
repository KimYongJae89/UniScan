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

using UniScanM.Pinhole.Settings;
using UniScanM.Pinhole.MachineIF;

namespace UniScanM.Pinhole.Operation
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
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE_READY, "1");  // Update PLC
        }

        public override void SetRunningSignal(bool running = true) //0: idle, 1: run@ only auto-mode, Not Manual-Mode
        {
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE_RUN, "1");  // Update PLC
        }

        //public override void SetResultJudgement(bool NG) //0:OK, 1:NG
        //{
        //    string strNG = NG ? "1" : "0";
        //    SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE_GOOD, 500, "1");  // Update PLC
        //}

        public override bool GetAutoStartSignal()
        {
            return melsecMonitor.State.PinholeOnStart;
        }
                         
        protected override void SetFirstValue()
        {
            string data = string.Format("{0:X04}{1:X04}", 0, 0);
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE, data);
        }
    }
}