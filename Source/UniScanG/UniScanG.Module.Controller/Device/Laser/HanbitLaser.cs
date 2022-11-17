using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Settings;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor;

namespace UniScanG.Module.Controller.Device.Laser
{
    public class HanbitLaser : DeviceControllerExtender
    {
        protected const int HeartbeatIntervalMs = 1000;
        protected const int HeartbeatIntervalTimeoutMs = 3500;

        public HanbitLaserExtender HanbitLaserExtender => hanbitLaserExtender;
        protected HanbitLaserExtender hanbitLaserExtender = null;

        // CM => Laser
        protected IoPort c2lAlive = null;
        protected IoPort c2lEmg = null;
        protected IoPort c2lRst = null;
        protected IoPort c2lMark = null;
        protected IoPort c2lNotUse = null;
        protected IoPort c2lLotClear = null;

        // Laser => CM
        protected IoPort l2cAlive = null;
        protected IoPort l2cReady = null;
        protected IoPort l2cDone = null;
        protected IoPort l2cError = null;
        protected IoPort l2cOutOfRange = null;
        protected IoPort l2cLotClearDone = null;
        protected IoPort l2cNotMeanderDetect = null;
        protected IoPort l2cDecelMarkFault = null;

        Timer c2lAliveTimer = null;
        TimeOutTimer aliveTimeoutCheckerCmSide = null;
        bool isConnected = false;

        public bool IsSetAlive { get => this.c2lAliveTimer.Enabled; }

        public bool IsSetEmergency { get => isSetEmergency; }
        bool isSetEmergency;

        public bool IsSetReset { get => isSetReset; }
        bool isSetReset;

        public bool IsSetNG { get => isSetNG; }
        bool isSetNG;

        public bool IsSetNotUse { get => isSetNotUse; }
        bool isSetNotUse;

        public bool IsSetLotClear { get => isSetLotClear; }
        bool isSetLotClear;

        public bool IsConnected { get => this.isConnected; }

        public bool IsReady { get => isReady; }
        bool isReady;

        public bool IsDone { get => isDone; }
        bool isDone;

        public bool IsError { get => isError; }
        bool isError;

        public bool IsOutofMeanderRange { get => isOutOfRange; }
        bool isOutOfRange;

        public bool IsLotClearDone { get => isLotClearDone; }
        bool isLotClearDone;

        public bool IsMGood { get => isMGood; }
        bool isMGood;

        public bool IsDecelMarkFault { get => isDecelMarkFault; }
        bool isDecelMarkFault;

        public bool IsAlarmed { get => isError || isOutOfRange || isDecelMarkFault; }

        public virtual bool IsVirtual { get => false; }

        public static HanbitLaser Create(DeviceController deviceController)
        {
            if (UniEye.Base.Settings.MachineSettings.Instance().RunningMode != UniEye.Base.Settings.RunningMode.Real)
                return new HanbitLaserVirtual(deviceController);

            if (MonitorSystemSettings.Instance().UseLaserBurner == LaserMode.Virtual)
                return new HanbitLaserVirtual(deviceController);

            return new HanbitLaser(deviceController);
        }

        public HanbitLaser(DeviceController deviceController) : base(deviceController)
        {
            LogHelper.Debug(LoggerType.StartUp, "HanbitLaser Created");
            this.hanbitLaserExtender = new HanbitLaserExtender(this);
            this.hanbitLaserExtender.UseFromLocal = AdditionalSettings.Instance().LaserSetting.Use;
        }

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.c2lAliveTimer = new Timer();
            this.c2lAliveTimer.Interval = HeartbeatIntervalMs;
            this.c2lAliveTimer.Tick += C2LAliveTimer_Tick;
            this.c2lAliveTimer.Start();

            this.aliveTimeoutCheckerCmSide = new TimeOutTimer(this);
            this.aliveTimeoutCheckerCmSide.OnTimeout = AliveTimeoutCheckerCmSide_OnTimeout;
            this.aliveTimeoutCheckerCmSide.Start(HeartbeatIntervalTimeoutMs);

            InitializeIoEventHandler(deviceBox);
        }

        public override void Update(MachineIfData machineIfData)
        {
            this.hanbitLaserExtender.UseFromRemote = machineIfData.GET_START_GRAVURE_ERASER;
            this.hanbitLaserExtender.SetPNG(machineIfData.GET_FORCE_GRAVURE_ERASER);
        }

        public override void Apply(MachineIfData machineIfData)
        {
            machineIfData.SET_VISION_GRAVURE_ERASER_READY = this.IsConnected;
            machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING = this.hanbitLaserExtender.IsSetRun;

            machineIfData.SET_VISION_GRAVURE_ERASER_CNT_ERASE = Math.Min(0, this.hanbitLaserExtender.DoneCount);
        }

        private void C2LAliveTimer_Tick(object sender, EventArgs e)
        {
            // CM -> Laser Alive
            if (this.c2lAlive != null)
            {
                bool curValue = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(this.c2lAlive);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lAlive, !curValue);
            }
        }

        private void InitializeIoEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.c2lAlive = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserAlive);
            this.c2lEmg = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserEmergency);
            this.c2lRst = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserReset);
            this.c2lMark = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserMark);
            this.c2lNotUse = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserNotuse);
            this.c2lLotClear = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserLotClear);

            this.l2cAlive = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserAlive);
            this.l2cReady = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserReady);
            this.l2cDone = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserMarkDone);
            this.l2cError = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserError);
            this.l2cOutOfRange = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserOutOfMeanderRange);
            this.l2cLotClearDone = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserLotClearDone);
            this.l2cNotMeanderDetect = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserMarkGood);
            this.l2cDecelMarkFault = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserDecelMarkFault);

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lEmg, IoEventHandlerDirection.OutBound) { OnChanged = C2LEmergency_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lRst, IoEventHandlerDirection.OutBound) { OnChanged = C2LReset_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lMark, IoEventHandlerDirection.OutBound) { OnChanged = C2LNG_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lNotUse, IoEventHandlerDirection.OutBound) { OnChanged = C2LNotUse_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lLotClear, IoEventHandlerDirection.OutBound) { OnChanged = C2LLotClear_OnChanged });

            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cAlive, IoEventHandlerDirection.InBound, false) { OnChanged = L2CAlive_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cReady, IoEventHandlerDirection.InBound) { OnChanged = L2CReady_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cDone, IoEventHandlerDirection.InBound) { OnChanged = L2CDone_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cError, IoEventHandlerDirection.InBound) { OnChanged = L2CError_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cOutOfRange, IoEventHandlerDirection.InBound) { OnChanged = L2COutOfRange_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cLotClearDone, IoEventHandlerDirection.InBound) { OnChanged = L2CLotClearDone_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cNotMeanderDetect, IoEventHandlerDirection.InBound) { OnChanged = L2CNotMeanderDetect_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cDecelMarkFault, IoEventHandlerDirection.InBound) { OnChanged = L2CDecelMarkFault_OnChanged });
        }

        private void AliveTimeoutCheckerCmSide_OnTimeout(object sender, EventArgs e)
        {
            this.isConnected = false;
            ErrorManager.Instance().Report(ErrorCodeLaser.Instance.Information, ErrorLevel.Info, "Laser Device disconnected", null);
            this.hanbitLaserExtender?.OnStateChanged();
        }

        private bool C2LEmergency_OnChanged(IoEventHandler eventSource)
        {
            this.isSetEmergency = eventSource.IsActivate;
            //this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool C2LReset_OnChanged(IoEventHandler eventSource)
        {
            this.isSetReset = eventSource.IsActivate;
            //this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool C2LNG_OnChanged(IoEventHandler eventSource)
        {
            this.isSetNG = eventSource.IsActivate;
            //this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool C2LNotUse_OnChanged(IoEventHandler eventSource)
        {
            this.isSetNotUse = eventSource.IsActivate;
            //this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool C2LLotClear_OnChanged(IoEventHandler eventSource)
        {
            this.isSetLotClear = eventSource.IsActivate;
            //this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool L2CAlive_OnChanged(IoEventHandler eventSource)
        {
            this.aliveTimeoutCheckerCmSide.Stop();
            bool isNewConnected = !this.isConnected;
            if (isNewConnected)
            {
                ErrorManager.Instance().Report(ErrorCodeLaser.Instance.Information, ErrorLevel.Info, "Laser Device Connected", null);
                this.hanbitLaserExtender?.OnStateChanged();
            }
            this.isConnected = true;
            this.aliveTimeoutCheckerCmSide.Start(HeartbeatIntervalTimeoutMs);

            return true;
        }

        private bool L2CReady_OnChanged(IoEventHandler eventSource)
        {
            this.isReady = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnStateChanged();
            this.hanbitLaserExtender?.AppendLog(this.isReady ? "Auto" : "Manual");
            return true;
        }

        private bool L2CDone_OnChanged(IoEventHandler eventSource)
        {
            this.isDone = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnDoneChanged();

            return true;
        }

        private bool L2CError_OnChanged(IoEventHandler eventSource)
        {
            this.isError = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnStateChanged();
            return true;
        }

        private bool L2COutOfRange_OnChanged(IoEventHandler eventSource)
        {
            this.isOutOfRange = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnStateChanged();

            return true;
        }

        private bool L2CLotClearDone_OnChanged(IoEventHandler eventSource)
        {
            this.isLotClearDone = eventSource.IsActivate;
            if (this.isSetLotClear && this.isLotClearDone)
                this.SetLotClear(false);

            return true;
        }

        private bool L2CNotMeanderDetect_OnChanged(IoEventHandler eventSource)
        {
            this.isMGood = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnDoneGoodChanged();

            return true;
        }

        private bool L2CDecelMarkFault_OnChanged(IoEventHandler eventSource)
        {
            this.isDecelMarkFault = eventSource.IsActivate;
            this.hanbitLaserExtender?.OnStateChanged();

            return true;
        }

        public void SetAlive(bool active)
        {
            if (active)
                this.c2lAliveTimer.Start();
            else
                this.c2lAliveTimer.Stop();
        }

        public void SetEmergency(bool active)
        {
            if (this.c2lEmg != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lEmg, active);
        }

        public void SetReset(bool active)
        {
            if (this.c2lRst != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lRst, active);
        }

        public void SetMark(bool active)
        {
            if (this.c2lMark != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lMark, active);
        }

        public void SetNotUse(bool active)
        {
            if (this.c2lNotUse != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lNotUse, active);
        }

        public void SetLotClear(bool active)
        {
            if (this.c2lLotClear != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lLotClear, active);
        }
    }

    public class HanbitLaserVirtual : HanbitLaser
    {
        Timer l2cAliveTimer = null;
        TimeOutTimer aliveTimeoutCheckerLaserSide = null;

        public override bool IsVirtual { get => true; }

        public HanbitLaserVirtual(DeviceController deviceController) : base(deviceController)
        {
            LogHelper.Debug(LoggerType.StartUp, "HanbitLaserVirtual Created");
        }

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);

            this.l2cAliveTimer = new Timer();
            this.l2cAliveTimer.Interval = HeartbeatIntervalMs;
            this.l2cAliveTimer.Tick += L2CAliveTimer_Tick;

            this.aliveTimeoutCheckerLaserSide = new TimeOutTimer(this);
            this.aliveTimeoutCheckerLaserSide.Start(HeartbeatIntervalTimeoutMs);

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lAlive, IoEventHandlerDirection.OutBound, false) { OnChanged = C2LAlive_OnChanged });

            deviceController.GetIoEventHandler(this.c2lLotClear).OnChanged += new IoEvent(f =>
             {
                 // LotChange 신호가 들오면 100ms 후에 ChangeDone 신호 보냄.
                 Task.Run(() =>
                 {
                     if (f.IsActivate)
                     {
                         System.Threading.Thread.Sleep(100);
                         this.hanbitLaserExtender.ClearDone();
                     }
                     SetLaserLotClearDode(this.IsSetLotClear);
                 });
                 return true;
             });

            deviceController.GetIoEventHandler(this.c2lRst).OnActivate += new IoEvent(f =>
            {
                // Rest 신호가 들오면 모든 Error상태 초기화
                this.SetLaserOutOfRange(false);
                this.SetLaserDecelMarkFault(false);

                this.SetLaserError(false);
                return true;
            });

            deviceController.GetIoEventHandler(this.c2lMark).OnActivate += new IoEvent(f =>
            {
                // NG신호가 들어오면 500ms 후에 Done 신호 보냄.
                // 신호가 켜져있으면 계속 보냄.
                Task.Run(() =>
                {
                    while (this.IsSetNG)
                    {
                        System.Threading.Thread.Sleep(500);
                        if (!this.IsSetNotUse)
                        {
                            this.SetLaserDone(true);
                            System.Threading.Thread.Sleep(100);
                            this.SetLaserDone(false);
                        }
                    }
                });
                return true;
            });

            SetLaserAlive(true);
            SetLaserReady(true);
        }

        private bool C2LAlive_OnChanged(IoEventHandler eventSource)
        {
            this.aliveTimeoutCheckerLaserSide.Restart(HeartbeatIntervalTimeoutMs);
            return true;
        }

        private void L2CAliveTimer_Tick(object sender, EventArgs e)
        {
            // Laser -> CM Alive
            this.l2cAliveTimer.Interval = HeartbeatIntervalMs;
            bool curState = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(this.l2cAlive);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cAlive, !curState);
        }

        public bool SetLaserAlive(bool active)
        {
            this.l2cAliveTimer.Interval = HeartbeatIntervalMs;

            if (active)
                this.l2cAliveTimer.Start();
            else
                this.l2cAliveTimer.Stop();

            return active;
        }

        public bool SetLaserReady(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cReady, active);
            return active;
        }

        public bool SetLaserDone(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cDone, active);
            return active;
        }

        public bool SetLaserError(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cError, active);
            return active;
        }

        public bool SetLaserOutOfRange(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cOutOfRange, active);
            return active;
        }

        public bool SetLaserLotClearDode(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cLotClearDone, active);
            return active;
        }

        public bool SetLaserDecelMarkFault(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cDecelMarkFault, active);
            return active;
        }

        public bool SetLaserMarkGood(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cNotMeanderDetect, active);
            return active;
        }
    }
}
