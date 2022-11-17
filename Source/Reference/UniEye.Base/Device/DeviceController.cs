using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Device;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System.Collections.Generic;
using UniEye.Base.Settings;
using System;
using DynMvp.InspData;

namespace UniEye.Base.Device
{
    public class DeviceController
    {
        private IoMonitor ioMonitor;
        public IoMonitor IoMonitor
        {
            get { return ioMonitor; }
        }

        protected List<MotionEventHandler> motionEventHandlerList;

        protected List<IoEventHandler> ioEventHandlerList;

        MotionMonitor motionMonitor;
        public MotionMonitor MotionMonitor
        {
            get { return motionMonitor; }
        }

        private AxisHandler robotStage;
        public AxisHandler RobotStage
        {
            get { return robotStage; }
            set { robotStage = value; }
        }

        private AxisHandler convayor;
        public AxisHandler Convayor
        {
            get { return convayor; }
            set { convayor = value; }
        }

        TowerLamp towerLamp;
        public TowerLamp TowerLamp
        {
            get { return towerLamp; }
        }

        //private List<IoEventHandler> doorOpenedList;
        //public List<IoEventHandler> DoorOpenedList
        //{
        //    get { return doorOpenedList; }
        //}

        //IoButtonEventHandler emergencyButton;
        //public IoButtonEventHandler EmergencyButton
        //{
        //    get { return emergencyButton; }
        //}

        //IoEventHandler airPressure;
        //public IoEventHandler AirPressure
        //{
        //    get { return airPressure; }
        //}

        BarcodeScanner barcodeScanner;

        public DeviceController()
        {
            this.motionEventHandlerList = new List<MotionEventHandler>();
            this.ioEventHandlerList = new List<IoEventHandler>();
        }

        public BarcodeScanner BarcodeScanner
        {
            get { return barcodeScanner; }
        }

        public virtual void Initialize(DeviceBox deviceBox)
        {
            MachineSettings machineSettings = MachineSettings.Instance();

            InitializeIoEventHandler(deviceBox);

            ioMonitor = new IoMonitor(deviceBox.DigitalIoHandler, ioEventHandlerList);
            ioMonitor.Start();

            InitializeMotionEventHandler(deviceBox);

            if (deviceBox.MotionList.Count > 0)
            {
                motionMonitor = new MotionMonitor(deviceBox.MotionList, motionEventHandlerList);
                motionMonitor.Start();
            }

            convayor = deviceBox.AxisConfiguration.Find(f => f.HandlerType == AxisHandlerType.Converyor);
            if (convayor != null)
            {
                convayor.TurnOnServo(true);
                //convayor.AxisList.ForEach(f => f.HomeFound = true); // Convayor는 Home을 잡지 않는다.
            }

            robotStage = deviceBox.AxisConfiguration.Find(f => f.HandlerType == AxisHandlerType.RobotStage);
            if (robotStage != null)
            //if (machineSettings.UseRobotStage)
            {
                //    robotStage = deviceBox.AxisConfiguration.GetAxisHandler(AxisHandlerName.RobotStage);
                robotStage.RobotAligner.Load(PathSettings.Instance().Config);

                robotStage.TurnOnServo(true);
                //    motionMonitor = new MotionMonitor(deviceBox.MotionList);
                //    motionMonitor.Start();
            }

            if (machineSettings.UseTowerLamp)
            {
                IoPort[] towerLampIoPort = deviceBox.PortMap.GetTowerLampPort();
                if (towerLampIoPort != null)
                {
                    towerLamp = new TowerLamp();
                    towerLamp.Setup(deviceBox.DigitalIoHandler, TimeSettings.Instance().TowerLampUpdataIntervalMs);
                    towerLamp.SetupPort(towerLampIoPort);
                    towerLamp.Load();
                    towerLamp.GetDynamicState = towerLamp_GetDynamicState;
                    towerLamp.Start();
                }
            }
        }

        public virtual TowerLampState towerLamp_GetDynamicState()
        {
            return towerLamp.GetState();
        }

        private void InitializeMotionEventHandler(DeviceBox deviceBox)
        {
            //motionEventHandlerList = new List<MotionEventHandler>();
        }

        public void AddMotionEventHandler(MotionEventHandler motionEventHandler)
        {
            this.motionEventHandlerList.Add(motionEventHandler);
        }

        private bool IoMonitor_ProcessIdle(DioValue value)
        {
            throw new NotImplementedException();
        }

        private void InitializeIoEventHandler(DeviceBox deviceBox)
        {
            DigitalIoHandler digitalIoHandler = deviceBox.DigitalIoHandler;
            PortMap portMap = (PortMap)deviceBox.PortMap;
            MachineSettings machineSettings = MachineSettings.Instance();

            //ioEventHandlerList = new List<DynMvp.Device.Device.IoEventHandler>();

            if (machineSettings.UseOpPanel)
            {
                IoPort portEmg = portMap.InEmergency;
                if (portEmg == null)
                {
                    LogHelper.Error(LoggerType.Error, "\"UseOpPanel\" is on, but IO Port \"Emergency\" is not defined");
                }
                else
                {
                    IoButtonEventHandler emergencyButton = new IoButtonEventHandler("Emergency", digitalIoHandler, portMap.InEmergency, null);
                    emergencyButton.OnActivate += emergencyButton_ButtonPushed;
                    emergencyButton.OnDeactivate += emergencyButton_ButtonPulled;
                    ioEventHandlerList.Add(emergencyButton);
                }
            }

            if (machineSettings.UseAirPressure)
            {
                IoPort portAir = portMap.InAirPressureLow;
                if (portAir == null)
                {
                    LogHelper.Error(LoggerType.Error, "\"UseAirPressure\" is on, but IO Port \"Air Pressure\" is not defined");
                }
                else
                {
                    IoEventHandler airPressure = new IoEventHandler("Air Pressure", digitalIoHandler, portMap.InAirPressureLow, IoEventHandlerDirection.InBound);
                    airPressure.OnActivate += airPressure_OnInputOn;
                    airPressure.OnDeactivate = null;
                    ioEventHandlerList.Add(airPressure);
                }
            }

            if (machineSettings.UseDoorSensor)
            {
                List<IoPort> doorPorts = new List<IoPort>();
                portMap.GetInDoorPorts(doorPorts);
                if (doorPorts.Count == 0)
                {
                    LogHelper.Error(LoggerType.Error, "\"UseDoorSensor\" is on, but there is no door port");
                }
                else
                {
                    /*foreach (IoPort ioPort in doorPorts)
                    {
                        IoEventHandler doorOpened = new IoEventHandler("Door Opened", digitalIoHandler, ioPort);
                        doorOpened.OnInputOn += doorOpened_OnInputOn;
                        doorOpened.OnInputOff += doorOpened_OnInputOff;
                        doorOpened.Update();
                        //doorOpenedList.Add(doorOpened);
                        ioEventHandlerList.Add(doorOpened);
                    }*/
                }
            }
        }

        public void AddIoEventHandler(IoEventHandler ioEventHandler)
        {
            lock (this.ioEventHandlerList)
                this.ioEventHandlerList.Add(ioEventHandler);
            ioEventHandler.Update();
        }

        public IoEventHandler GetIoEventHandler(IoPort ioPort)
        {
            return GetIoEventHandler(ioPort.Name);
        }

        public IoEventHandler GetIoEventHandler(string name)
        {
            return ioEventHandlerList?.Find(f => f.Name == name);
        }

        //private bool IoMonitor_ProcessInputChanged(DioValue oldValue, DioValue newValue)
        //{
        //    uint oldVal = oldValue.GetValue(0, 0);
        //    uint newVal = newValue.GetValue(0, 0);
        //    uint diff = oldVal ^ newVal;

        //    int iter = 0;
        //    while(diff!=0)
        //    {
        //        if ((diff & 0x01) == 1)
        //        {
        //            bool curValue = ((newVal >> iter) & 0x01) == 1;
        //            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(0, 0, iter);
        //            if(ioPort.EventHandler!=null)
        //                ioPort.EventHandler(curValue);
        //        }
        //        iter++;
        //        diff >>= 1;
        //    }

        //    return true;
        //}

        public virtual void Release()
        {
            if (robotStage != null)
            {
                robotStage.StopMove();
                robotStage.WaitMoveDone();
            }

            if (convayor != null)
            {
                convayor.StopMove();
                convayor.WaitMoveDone();
            }

            if (towerLamp != null)
            {
                towerLamp.Stop();
                towerLamp.Release();
            }

            if (ioMonitor != null)
                ioMonitor.Stop();

            if (motionMonitor != null)
                motionMonitor.Stop();
        }

        public virtual void ResetState()
        {
        }

        private bool doorOpened_OnInputOn(IoEventHandler eventSource)
        {
            /*IoEventHandler doorLockHandler = ioEventHandlerList.Find(f => f.Name == "DoorLock");
            if(doorLockHandler!=null && (doorLockHandler.GetCurrentValue() == true))
                return true;

            ErrorManager.Instance().Report(ErrorSections.Instance().Safety, (int)SafetyError.DoorOpen, ErrorLevel.Error,
                ErrorSections.Instance().Safety.ToString(), SafetyError.DoorOpen.ToString(), "Door is opened");
            */
            return true;
        }

        private bool doorOpened_OnInputOff(IoEventHandler eventSource)
        {
            ErrorManager.Instance().ResetAllAlarm();
            return true;
        }

        private bool airPressure_OnInputOn(IoEventHandler eventSource)
        {
            ErrorManager.Instance().Report(ErrorCodeUtility.Instance.Air.Information, ErrorLevel.Error,
                eventSource.Name, "Air Pressure is not supplied", null);
            return true;
        }

        private bool emergencyButton_ButtonPushed(IoEventHandler eventSource)
        {
            ErrorManager.Instance().Report(ErrorSectionSafety.Instance.EmergencySwitch.Information, ErrorLevel.Error,
                eventSource.Name, "Emergency stop button is pressed", null);
            return true;
        }

        private bool emergencyButton_ButtonPulled(IoEventHandler eventSource)
        {
            //ErrorManager.Instance().ResetAlarm();
            return true;
        }

        public virtual void OnModelLoaded(Model curModel)
        {

        }

        public virtual void OnModelClose(Model curModel)
        {

        }

        public virtual bool OnEnterWaitInspection(params object[] args)
        {
            return true;
        }

        internal virtual void OnStartInspection()
        {

        }

        internal virtual void OnStopInspection()
        {

        }

        public virtual void OnProductInspected(InspectionResult inspectionResult)
        {
            //LogHelper.Debug(LoggerType.Operation, "DeviceController::OnProductInspected");
        }

        public virtual bool OnExitWaitInspection()
        {
            return true;
        }

        internal virtual bool ProcessInputChanged(DioValue inputValue)
        {
            //if (emergencyButton.CheckState(inputValue) == true)
            //    return true;

            return false;
        }

        internal void Stop()
        {

        }
    }
}
