using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Operation;
using UniScanM.StillImage.MachineIF;

namespace UniScanM.StillImage.Operation
{
    class PLCInspectStarter : UniScanM.Operation.PLCInspectStarter
    {
        public PLCInspectStarter() : base()
        {

        }

        public override void SetReadySignal(bool imReady = true) //0: off or manual, 1: on and Auto-mode
        {
            //if(Auto-mode)
            //string mode = automode ? "1" : "0";
            //PLC가 읽고 clear 함으로 매뉴얼 모드일때는 굳이 쓸필요없음.
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_READY, "1");  // Update PLC
        }

        public override void SetRunningSignal(bool running = true) //0: idle, 1: run@ only auto-mode, Not Manual-Mode
        {
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_RUN, "1");  // Update PLC
        }                          

        public override bool GetAutoStartSignal()
        {
            return melsecMonitor.State.StillImageOnStart;
        }

        protected override void SetFirstValue()
        {
            string judge = "0000";

            string sendData = string.Format("{0}0000{1}{2}{3}{4}{5}{6}",
                judge, 0, 0, 0, 0, 0, 0
                );
            if (SystemManager.Instance().DeviceBox.MachineIf != null)
            {
                if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected && SystemManager.Instance().DeviceBox.MachineIf.IsIdle)
                    SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE, sendData);
            }
        }
    }
}
