using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniScanG.Module.Controller.MachineIF;

namespace UniScanG.Module.Controller.Device.Stage
{
    internal class StageState
    {
        int servoOnCnt = 0;
        int servoRunCnt = 0;

        public bool ServoFault { get => this.servoFault; set => this.servoFault = value; }
        bool servoFault = false;

        public bool Emergency { get => this.emergency; set => this.emergency = value; }
        bool emergency = false;

        public bool AirPressure { get => this.airPressure; set => this.airPressure = value; }
        bool airPressure = false;

        public bool DoorOpenL { get => this.doorOpenL; set => this.doorOpenL = value; }
        bool doorOpenL = false;

        public bool DoorOpenR { get => this.doorOpenR; set => this.doorOpenR = value; }
        bool doorOpenR = false;

        public bool DoorOpen => this.doorOpenL || this.doorOpenR;

        public bool DoorLocked { get => this.doorLocked; set => this.doorLocked = value; }
        bool doorLocked;

        public bool Ionizer { get => this.ionizer; set => this.ionizer = value; }
        bool ionizer;

        public bool IonizerSol { get => this.ionizerSol; set => this.ionizerSol = value; }
        bool ionizerSol;

        public bool Vaccum { get => this.vaccum; set => this.vaccum = value; }
        bool vaccum;

        public bool AirFanOn { get => this.airFanOn; set => this.airFanOn = value; }
        bool airFanOn;


        public bool RoomLightOn { get => this.roomLightOn; set => this.roomLightOn = value; }
        bool roomLightOn;

        public bool ServoOn => this.servoOnCnt > 0;
        public bool ServoRun => this.servoRunCnt > 0;

        public void IncServeOnCnt() { servoOnCnt++; }
        public void DecServeOnCnt() { if (servoOnCnt > 0) servoOnCnt--; }
        public void IncServeRunCnt() { servoRunCnt++; }
        public void DecServeRunCnt() { if (servoRunCnt > 0) servoRunCnt--; }

        public string[] GetOpenedDoorName()
        {
            List<string> openedDoorList = new List<string>();
            if (this.doorOpenL)
                openedDoorList.Add(StringManager.GetString(this.GetType().FullName, "Left"));

            if (this.doorOpenR)
                openedDoorList.Add(StringManager.GetString(this.GetType().FullName, "Right"));

            return openedDoorList.ToArray();
        }
    }

    public class TestbedStage : DeviceControllerExtender
    {
        StageState stageState = null;

        public MovingParam MovingParam => this.movingParam;
        MovingParam movingParam = null;

        public TestbedStage(DeviceController deviceController) : base(deviceController)
        {
            this.movingParam = new MovingParam();

            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmState;
        }

        private void ErrorManager_OnResetAlarmState()
        {
            deviceController.Convayor.ResetAlarm();
        }

        public override void Initialize(DeviceBox deviceBox)
        {
            this.stageState = new StageState();
            
            AxisHandler axisHandler = deviceController.Convayor;
            if (axisHandler != null)
            {
                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
                float pelSize = calibration == null ? 14.0f : Math.Min(calibration.PelSize.Width, calibration.PelSize.Height);

                axisHandler.AxisList.ForEach(f =>
                {
                    float pulse = f.ToPulse(pelSize);
                    f.Motion.StartCmp(f.AxisNo, 0, pulse, true);
                });
            }

            SetConvSpeed(30);

            InitializeIoEventHandler(deviceBox);
            InitializeMotionEventHandler(deviceBox);
        }

        public void SetConvSpeed(float speelMpm)
        {
            float defaultUps = speelMpm / 60.0f * 1E6f;
            float accTime = Math.Max(1000, (float)(speelMpm / 20f * 1000f));
            float decTime = accTime / 2;

            Axis axis = deviceController.Convayor?.AxisList.FirstOrDefault();
            if (axis != null)
            {
                this.movingParam.AccelerationTimeMs = accTime;
                this.movingParam.DecelerationTimeMs = decTime;
                this.movingParam.MaxVelocity = axis.ToPulse(defaultUps);
            }
        }

        private void InitializeIoEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            PortMap portMap = (PortMap)deviceBox.PortMap;

            IoPort ioPort = portMap.GetOutPort(PortMap.IoPortName.OutDoorOpen);
            if (ioPort != null)
                deviceBox.DigitalIoHandler.SetOutputDeactive(ioPort);

            List<IoEventHandler> ioEventHandlerList = new List<IoEventHandler>();

            deviceController.AddIoEventHandler(new IoEventHandler("Emergency", deviceBox.DigitalIoHandler, portMap.GetInPort(PortMap.IoPortName.InEmergency), IoEventHandlerDirection.InBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.Emergency = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("AirPressure", deviceBox.DigitalIoHandler, portMap.GetInPort(PortMap.IoPortName.InAirPressure), IoEventHandlerDirection.InBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.AirFanOn = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("DoorOpenL", deviceBox.DigitalIoHandler, portMap.GetInPort(PortMap.IoPortName.InDoorOpenL), IoEventHandlerDirection.InBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.DoorOpenL = f.IsActivate;
                    CheckDoorState();
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("DoorOpenR", deviceBox.DigitalIoHandler, portMap.GetInPort(PortMap.IoPortName.InDoorOpenR), IoEventHandlerDirection.InBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.DoorOpenR = f.IsActivate;
                    CheckDoorState();
                    return true;
                })
            });

            deviceController.AddIoEventHandler(new IoEventHandler("DoorLock", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutDoorOpen), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.DoorLocked = f.IsActivate;
                    CheckDoorState();
                    return true;
                })
            });

            deviceController.AddIoEventHandler(new IoEventHandler("Ionizer", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutIonizer), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.Ionizer = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("IonizerSol", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutIonizerSol), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.IonizerSol = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("Vaccum", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutVaccumOn), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.Vaccum = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("AirFan", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutAirFan), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.AirFanOn = f.IsActivate;
                    return true;
                })
            });
            deviceController.AddIoEventHandler(new IoEventHandler("RoomLight", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutRoomLight), IoEventHandlerDirection.OutBound)
            {
                OnChanged = new IoEvent(f =>
                {
                    this.stageState.RoomLightOn = f.IsActivate;
                    return true;
                })
            });
        }

        private void InitializeMotionEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            foreach (AxisHandler axisHandler in deviceBox.AxisConfiguration)
            {
                MotionEventHandler motionEventHandler = new MotionEventHandler(axisHandler.Name, axisHandler, -1);
                motionEventHandler.OnServoOn = motionEventHandler_OnServoOn;
                motionEventHandler.OnServoOff = motionEventHandler_OnServoOff;
                motionEventHandler.OnStartMove = motionEventHandler_OnStartMove;
                motionEventHandler.OnMoveDone = motionEventHandler_OnMoveDone;
                motionEventHandler.OnFault = motionEventHandler_OnFault;
                deviceController.AddMotionEventHandler(motionEventHandler);
            }
        }

        private bool motionEventHandler_OnMoveDone(MotionEventHandler eventSource)
        {
            stageState.DecServeRunCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnStartMove(MotionEventHandler eventSource)
        {
            stageState.IncServeRunCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnServoOn(MotionEventHandler eventSource)
        {
            stageState.IncServeOnCnt();
            return true;
        }

        private bool motionEventHandler_OnServoOff(MotionEventHandler eventSource)
        {
            stageState.DecServeOnCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnFault(MotionEventHandler eventSource)
        {
            //motionState.DecServeOnCnt();
            //throw new NotImplementedException();
            ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteValue, ErrorLevel.Error,
                eventSource.Name, "Motion is Faulted", null);
            return true;
        }

        public TowerLampState GetTowerlampState()
        {
            if (!ErrorManager.Instance().IsCleared() || stageState.ServoFault)
                return deviceController.TowerLamp.GetState(TowerLampStateType.Alarm);
            if (stageState.ServoRun)
                return deviceController.TowerLamp.GetState(TowerLampStateType.Working);
            if (stageState.ServoOn)
                return deviceController.TowerLamp.GetState(TowerLampStateType.Wait);

            return deviceController.TowerLamp.GetState(TowerLampStateType.Idle);
        }

        public bool LockDoor(bool enable)
        {
            if (enable)
            {
                // 문이 열려있는지 확인
                if (this.stageState.DoorOpen)
                {
                    string openedDoor = string.Join(", ", this.stageState.GetOpenedDoorName());
                    throw new AlarmException(ErrorSectionSafety.Instance.DoorOpen.Information, ErrorLevel.Error,
                        openedDoor, "[{0}] Door is Opened", new object[] { openedDoor }, "");
                }

                // 문 잠금
                IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutDoorOpen);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(ioPort);

                return true;
            }
            else
            {
                IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutDoorOpen);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(ioPort);
                return true;
            }
        }

        public bool UpdateUtility(bool enable)
        {
            if (enable)
            {
                // 진공 흡수. 이오나이저 작동. 펜 작동
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutVaccumOn));

                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutIonizer));
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutIonizerSol));

                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutAirFan));
            }
            else
            // 반대 동작
            {
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutVaccumOn));

                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutIonizer));
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutIonizerSol));

                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutAirFan));
            }

            return true;
        }

        public bool CheckDoorState()
        {
            bool good = true;
            if (this.stageState.DoorOpen)
            {
                if (this.stageState.DoorLocked)
                {
                    string openedDoor = string.Join(", ", this.stageState.GetOpenedDoorName());
                    ErrorManager.Instance().Report(new AlarmException(ErrorSectionSafety.Instance.DoorOpen.Information, ErrorLevel.Error,
                        openedDoor, "[{0}] Door is Opened", new object[] { openedDoor }, ""));
                    good = false;
                }
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRoomLight));
            }
            else
            {
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRoomLight));
            }
            return good;
        }

        public bool UpdateAxisHandler(bool enable)
        {
            // StandAlone인 경우 컨베이어 굴리기
            AxisHandler axisHandler = deviceController.Convayor;
            if (axisHandler != null)
            {
                bool wait = true;
                if (enable)
                {
                    wait = true;
                    bool ok = axisHandler.ContinuousMove(this.movingParam, false);
                    if (!ok)
                        throw new AlarmException(ErrorCodeMotion.Instance.FailToWriteValue, ErrorLevel.Error,
            axisHandler.Name, "Testbed Stage Running Failure.", null, "");
                }
                else
                {
                    if (axisHandler.AxisList.Max(f => f.GetActualVel()) == 0)
                        wait = false;
                    SystemManager.Instance().DeviceController.Convayor?.StopMove();
                }

                if (wait)
                {
                    int sleepTime = (int)(this.movingParam.AccelerationTimeMs * 1.2);
                    int sleepStep = (sleepTime + 99) / 100;
                    while (sleepStep > 0)
                    {
                        Thread.Sleep(100);
                        System.Windows.Forms.Application.DoEvents();
                        sleepStep--;
                    }
                }
                return true;
            }
            return false;
        }

        public float GetActualVelMpm()
        {
            AxisHandler axisHandler = deviceController.Convayor;
            float ups = axisHandler.AxisList.Max(f =>f.ToPosition(f.GetActualVel()));
            return ups / 1E6f * 60;
        }

        public float GetCommandVelMpm()
        {
            AxisHandler axisHandler = deviceController.Convayor;
            float ups = axisHandler.AxisList.Max(f => f.ToPosition(this.movingParam.MaxVelocity));
            return ups / 1E6f * 60;
        }

        public override void Update(MachineIfData machineIfData)
        {
            throw new NotImplementedException();
        }

        public override void Apply(MachineIfData machineIfData)
        {
            throw new NotImplementedException();
        }
    }
}
