using System.Collections.Generic;
using DynMvp.Devices.Dio;
using UniEye.Base.Settings;
using System;
using System.IO;

namespace UniEye.Base.Device
{
    public class PortMap : PortMapBase
    {
        public enum IoPortNameBase
        {
            InEmergency, InStartSw, InStopSw, InResetSw, InDoorOpen1, InDoorOpen2, InDoorOpen3, InAirPressure,
            OutInspStart, OutInspEnd, InInspTrig, InInspTrig2, Done, OutStartLamp, OutStopLamp, OutResetLamp, OutTowerRed, OutTowerYellow, OutTowerGreen, OutTowerBuzzer, OutIoLight1, OutIoLight2, OutIoLight3,
            OutVaccum, OutIonizer, OutIonizerSol,
            OutVisionReady, OutCommandWait, OutComplete, OutResultNg, OutOnWorking
        }

        //protected IoPort inEmergency = new IoPort("Emergency");
        public IoPort InEmergency { get { return GetInPort(IoPortNameBase.InEmergency); } }

        //protected IoPort inStartSw = new IoPort("Start Switch");
        public IoPort InStartSw { get { return GetInPort(IoPortNameBase.InStartSw); } }

        //protected IoPort inStopSw = new IoPort("Stop Switch");
        public IoPort InStopSw { get { return GetInPort(IoPortNameBase.InStopSw); } }

        //protected IoPort inResetSw = new IoPort("Reset Switch");
        public IoPort InResetSw { get { return GetInPort(IoPortNameBase.InResetSw); } }

        //protected IoPort inDoorOpen1 = new IoPort("Door Open1");
        public IoPort InDoorOpen1 { get { return GetInPort(IoPortNameBase.InDoorOpen1); } }

        //protected IoPort inDoorOpen2 = new IoPort("Door Open2");
        public IoPort InDoorOpen2 { get { return GetInPort(IoPortNameBase.InDoorOpen2); } }

        //protected IoPort inDoorOpen3 = new IoPort("Door Open3");
        public IoPort InDoorOpen3 { get { return GetInPort(IoPortNameBase.InDoorOpen3); } }

        //protected IoPort inAirPressureLow = new IoPort("Air Pressure Low");
        public IoPort InAirPressureLow { get { return GetInPort(IoPortNameBase.InAirPressure); } }

        //protected IoPort outStartLamp = new IoPort("Start Lamp");
        public IoPort OutStartLamp { get { return GetOutPort(IoPortNameBase.OutStartLamp); } }

        //protected IoPort outStopLamp = new IoPort("Stop Lamp");
        public IoPort OutStopLamp { get { return GetOutPort(IoPortNameBase.OutStopLamp); } }

        //protected IoPort outResetLamp = new IoPort("Reset Lamp");
        public IoPort OutResetLamp { get { return GetOutPort(IoPortNameBase.OutResetLamp); } }

        //protected IoPort outTowerLampRed = new IoPort("Tower Lamp Red");
        public IoPort OutTowerLampRed { get { return GetOutPort(IoPortNameBase.OutTowerRed); } }

        //protected IoPort outTowerLampYellow = new IoPort("Tower Lamp Yellow");
        public IoPort OutTowerLampYellow { get { return GetOutPort(IoPortNameBase.OutTowerYellow); } }

        //protected IoPort outTowerLampGreen = new IoPort("Tower Lamp Green");
        public IoPort OutTowerLampGreen { get { return GetOutPort(IoPortNameBase.OutTowerGreen); } }

        //protected IoPort outTowerBuzzer = new IoPort("Tower Buzzer");
        public IoPort OutTowerBuzzer { get { return GetOutPort(IoPortNameBase.OutTowerBuzzer); } }

        //protected IoPort outLight1 = new IoPort("Light 1");
        public IoPort OutLight1 { get { return GetOutPort(IoPortNameBase.OutIoLight1); } }

        //protected IoPort outLight2 = new IoPort("Light 2");
        public IoPort OutLight2 { get { return GetOutPort(IoPortNameBase.OutIoLight2); } }

        //protected IoPort outLight3 = new IoPort("Light 3");
        public IoPort OutLight3 { get { return GetOutPort(IoPortNameBase.OutIoLight3); } }

        //protected IoPort outVisionReady = new IoPort("Vision Ready");
        public IoPort OutVisionReady { get { return GetOutPort(IoPortNameBase.OutVisionReady); } }

        public IoPort OutOnWorking { get { return GetOutPort(IoPortNameBase.OutOnWorking); } }

        //protected IoPort outCommandWait = new IoPort("Command Wait");
        public IoPort OutCommandWait { get { return GetOutPort(IoPortNameBase.OutCommandWait); } }

        //protected IoPort inInspStart = new IoPort("Insp Start");
        public IoPort InInspStart { get { return GetInPort(IoPortNameBase.OutInspStart); } }

        //protected IoPort inInspEnd = new IoPort("Insp End");
        public IoPort InInspEnd { get { return GetInPort(IoPortNameBase.OutInspEnd); } }

        //protected IoPort inTrigger = new IoPort("Trigger");
        public IoPort InTrigger { get { return GetInPort(IoPortNameBase.InInspTrig); } }

        //protected IoPort inTriggerCh1 = new IoPort("Trigger Ch1");
        //public IoPort InTriggerCh1 { get { return inTriggerCh1; } }

        //protected IoPort inTriggerCh2 = new IoPort("Trigger Ch2");
        public IoPort InTriggerCh2 { get { return GetInPort(IoPortNameBase.InInspTrig2); } }

        //protected IoPort outComplete = new IoPort("Complete");
        public IoPort OutComplete { get { return GetOutPort(IoPortNameBase.OutComplete); } }

        //protected IoPort inCommandDone = new IoPort("Command Done");
        public IoPort InCommandDone { get { return GetInPort(IoPortNameBase.Done); } }

        //protected IoPort outResultNg = new IoPort("Result NG");
        public IoPort OutResultNg { get { return GetOutPort(IoPortNameBase.OutResultNg); } }

        public PortMap(string xmlFilePath = null) : base(xmlFilePath)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                this.xmlFilePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
                
            Initialize(typeof(IoPortNameBase));
        }

        public void Clear()
        {
            this.InPortList.ClearPort();
            this.OutPortList.ClearPort();
        }

        public virtual void SetupPorts()
        {
            string fileName = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            Load(fileName);
        }

        public override void GetIoLightPorts(List<IoPort> lightPortList)
        {
            if (OutLight1.PortNo != -1)
                lightPortList.Add(OutLight1);
            if (OutLight2.PortNo != -1)
                lightPortList.Add(OutLight2);
            if (OutLight3.PortNo != -1)
                lightPortList.Add(OutLight3);

            lightPortList.AddRange(InPortList.GetIoPorts(IoGroup.Light));
        }
    }

    public class PortMapFactory
    {
        public static PortMap Create()
        {
            string xmlFilePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            return new PortMap(xmlFilePath);
        }
    }
}
