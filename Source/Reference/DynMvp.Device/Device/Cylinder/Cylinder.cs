using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices;

namespace DynMvp.Device.Device.Cylinder
{
    public enum SensorLogic
    {
        And, Or
    }

    public class Cylinder
    {
        string name;
        DigitalIoHandler digitalIoHandler;
        List<IoPort> injectionPortList = new List<IoPort>();
        List<IoPort> ejectPortList = new List<IoPort>();
        List<IoPort> injectionSensorPortList = new List<IoPort>();
        List<IoPort> ejectSensorPortList = new List<IoPort>();
        bool injectSensorDetected = false;
        bool ejectSensorDetected = false;
        IsActionError isActionError;
        public IsActionError IsActionError
        {
            get { return isActionError; }
            set { isActionError = value; }
        }

        SensorLogic ejectSensorLogic = SensorLogic.And;
        public SensorLogic EjectSensorLogic
        {
            get { return ejectSensorLogic; }
            set { ejectSensorLogic = value; }
        }

        SensorLogic injectSensorLogic = SensorLogic.And;
        public SensorLogic InjectSensorLogic
        {
            get { return injectSensorLogic; }
            set { injectSensorLogic = value; }
        }

        static int actionDoneWaitS = 1000;
        public static int ActionDoneWaitS
        {
            get { return Cylinder.actionDoneWaitS; }
            set { Cylinder.actionDoneWaitS = value; }
        }

        static int airActionStableTimeMs;
        public static int AirActionStableTimeMs
        {
            get { return Cylinder.airActionStableTimeMs; }
            set { Cylinder.airActionStableTimeMs = value; }
        }

        bool useInjectionDoneCheck = true;
        public bool UseInjectionDoneCheck
        {
            get { return useInjectionDoneCheck; }
            set { useInjectionDoneCheck = value; }
        }

        bool useEjectionDoneCheck = true;
        public bool UseEjectionDoneCheck
        {
            get { return useEjectionDoneCheck; }
            set { useEjectionDoneCheck = value; }
        }

        bool virtualMode = false;
        public Cylinder(string name)
        {
            this.name = name;
            virtualMode = true;
        }

        public Cylinder(string name, DigitalIoHandler digitalIoHandler, IoPort injectionPort, IoPort ejectPort, IoPort injectionSensorPort = null, IoPort ejectSensorPort = null)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            if (injectionPort != null)
                injectionPortList.Add(injectionPort);
            if (ejectPort != null)
                ejectPortList.Add(ejectPort);
            if (injectionSensorPort != null)
                injectionSensorPortList.Add(injectionSensorPort);
            if (ejectSensorPort != null)
                ejectSensorPortList.Add(ejectSensorPort);

            ResetSensorDetectedFlags();
        }

        public void AddActuatorPort(IoPort injectionPort, IoPort ejectPort)
        {
            if (injectionPort != null)
                injectionPortList.Add(injectionPort);

            if (ejectPort != null)
                ejectPortList.Add(ejectPort);
        }

        public void AddSensorPort(IoPort injectionSensorPort, IoPort ejectSensorPort)
        {
            if (injectionSensorPort != null)
                injectionSensorPortList.Add(injectionSensorPort);

            if (ejectSensorPort != null)
                ejectSensorPortList.Add(ejectSensorPort);
        }

        public void ResetSensorDetectedFlags()
        {
            injectSensorDetected = false;
            ejectSensorDetected = false;
        }

        public bool IsInjected(bool enableLog = true)
        {
            if (virtualMode == true)
                return true;

            if (injectionSensorPortList.Count == 0)
            {
                if (enableLog)
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} inject sensor is inactivated", name));
                return true;
            }

            ResetSensorDetectedFlags();

            if (injectSensorDetected == false)
            {
                if (injectSensorLogic == SensorLogic.Or)
                    injectSensorDetected = false;
                else
                    injectSensorDetected = true;

                foreach (IoPort ioPort in injectionSensorPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    if (injectSensorLogic == SensorLogic.Or)
                        injectSensorDetected |= digitalIoHandler.ReadInput(ioPort);
                    else
                        injectSensorDetected &= digitalIoHandler.ReadInput(ioPort);
                }

                if (injectSensorDetected && enableLog)
                {
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} inject sensor is detected", name));
                }
            }

            return injectSensorDetected;
        }

        public bool IsEjected(bool enableLog = true)
        {
            if (virtualMode == true)
                return true;

            if (ejectSensorPortList.Count == 0)
            {
                if (enableLog == true)
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} Eject Sensor is inactivated", name));
                return true;
            }

            ResetSensorDetectedFlags();

            if (ejectSensorDetected == false)
            {
                if (ejectSensorLogic == SensorLogic.Or)
                    ejectSensorDetected = false;
                else
                    ejectSensorDetected = true;

                foreach (IoPort ioPort in ejectSensorPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    if (ejectSensorLogic == SensorLogic.Or)
                        ejectSensorDetected |= digitalIoHandler.ReadInput(ioPort);
                    else
                        ejectSensorDetected &= digitalIoHandler.ReadInput(ioPort);
                }

                if (ejectSensorDetected && enableLog)
                {
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} Eject Sensor is detected", name));
                }
            }

            return ejectSensorDetected;
        }

        public bool Inject()
        {
            LogHelper.Debug(LoggerType.IO, String.Format("Inject {0}", name));

            if (Act(true, false) == true)
                return true;

            LogHelper.Debug(LoggerType.IO, String.Format("Inject - Retry {0}", name));

            return Act(true, true);
        }

        public bool Eject()
        {
            LogHelper.Debug(LoggerType.IO, String.Format("Eject {0}", name));

            if (Act(false, false) == true)
                return true;

            LogHelper.Debug(LoggerType.IO, String.Format("Eject - Retry {0}", name));

            return Act(false, true);
        }

        public void ActAsync(bool inject)
        {
            ErrorManager.Instance().ThrowIfAlarm();

            ResetSensorDetectedFlags();

            //            DioValue outputValue = digitalIoHandler.ReadOutput(true);
            //if (inject)
            //    Thread.Sleep(10000);
            //else
            //    Thread.Sleep(5000);

            if (inject)
            {
                foreach (IoPort injectionPort in injectionPortList)
                {
                    if (injectionPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    digitalIoHandler.WriteOutput(injectionPort, true);

                    //                    outputValue.UpdateBitFlag(injectionPort, true);
                }

                foreach (IoPort ejectPort in ejectPortList)
                {
                    if (ejectPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    digitalIoHandler.WriteOutput(ejectPort, false);

                    //                    outputValue.UpdateBitFlag(ejectPort, false);
                }
            }
            else
            {
                foreach (IoPort injectionPort in injectionPortList)
                {
                    if (injectionPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    digitalIoHandler.WriteOutput(injectionPort, false);

                    //                    outputValue.UpdateBitFlag(injectionPort, false);
                }

                foreach (IoPort ejectPort in ejectPortList)
                {
                    if (ejectPort.PortNo == IoPort.UNUSED_PORT_NO)
                        continue;

                    digitalIoHandler.WriteOutput(ejectPort, true);
                    //                    outputValue.UpdateBitFlag(ejectPort, true);
                }
            }

            //            digitalIoHandler.WriteOutput(outputValue, true);
        }

        public bool Act(bool inject, bool reportError)
        {
            ActAsync(inject);

            Thread.Sleep(100);

            ActionDoneChecker actionDoneChecker = new ActionDoneChecker();
            if (inject)
            {
                if (useInjectionDoneCheck)
                    actionDoneChecker.IsActionDone = IsInjected;
            }
            else
            {
                if (useEjectionDoneCheck)
                    actionDoneChecker.IsActionDone = IsEjected;
            }

            actionDoneChecker.IsActionError = isActionError;

            isActionError = null;

            if (actionDoneChecker.IsActionDone != null)
            {
                if (actionDoneChecker.WaitActionDone(actionDoneWaitS * 1000) == false)
                {
                    if (reportError == true && ActionDoneChecker.StopDoneChecker == false)
                    {
                        ErrorCode errorCode = inject ? ErrorCodeCylinder.Instance.Inject : ErrorCodeCylinder.Instance.Eject;

                        ErrorManager.Instance().Report(errorCode, ErrorLevel.Error,
                            this.name, "Cylinder Error : {0}", new object[] { name });
                    }

                    return false;
                }
            }

            Thread.Sleep(airActionStableTimeMs);

            return true;
        }
    }
}
