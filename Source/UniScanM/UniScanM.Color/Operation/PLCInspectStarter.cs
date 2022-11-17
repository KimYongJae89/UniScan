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

using UniScanM.ColorSens.Settings;
using UniScanM.ColorSens.MachineIF;

namespace UniScanM.ColorSens.Operation
{
    public class PLCInspectStarter : UniScanM.Operation.PLCInspectStarter
    {
        public PLCInspectStarter() : base()
        {

        }

        public override bool GetAutoStartSignal()
        {
            return this.melsecMonitor.State.ColorSensorOnStart;
        }

        void SetBrightnessToPLC(ref double Brightness) //실시간 result 전달
        {
           MachineIfProtocolResponce protocolResponce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfColorSensorCommand.SET_SHEET_BRIGHTNESS);
        }

        //**********override Color-Sensor *************************************************//
        public override void SetReadySignal(bool imReady = true) //0: off or manual, 1: on and Auto-mode
        {
            //if(Auto-mode)
            //string mode = automode ? "1" : "0";
            //PLC가 읽고 clear 함으로 매뉴얼 모드일때는 굳이 쓸필요없음.
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_READY, "1");  // Update PLC
        }

        public override void SetRunningSignal(bool running = true) //0: idle, 1: run@ only auto-mode, Not Manual-Mode
        {
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_RUN, "1");  // Update PLC
        }

        protected override void SetFirstValue()
        {                                         
            string good = "0000";     
            string sheetBrightness = string.Format("{0:X04}", 0);//sheetBrightness
            string sendData = string.Format("{0}{1}", good, sheetBrightness);
        }
    }
}