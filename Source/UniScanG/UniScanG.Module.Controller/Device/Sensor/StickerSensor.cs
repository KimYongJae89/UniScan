using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;
using UniScanG.Module.Controller.MachineIF;

namespace UniScanG.Module.Controller.Device.Sensor
{
    public delegate void OnStickerSensed(DateTime onSenseStarted);

    public class StickerSensor : DeviceControllerExtender
    {
        public OnStickerSensed OnStickerSensed = null;

        public bool UseFromLocal { get => useFromLocal; set => useFromLocal = value; }
        bool useFromLocal;

        public IoPort IoPort { get => this.IoPort; }
        IoPort ioPort = null;

        public bool IsSensed { get => this.isSensed; }
        bool isSensed = false;

        DateTime senseStarted = DateTime.MinValue;
        bool startRequest = false;

        public StickerSensor(DeviceController deviceController) : base(deviceController) { }

        public override void Initialize(DeviceBox deviceBox)
        {
            InitializeIoEventHandler(deviceBox);
        }

        private void InitializeIoEventHandler(DeviceBox deviceBox)
        {
            this.ioPort = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InStickerSensor);

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;
            deviceController.AddIoEventHandler(new IoEventHandler("StickerSensor", digitalIoHandler, this.ioPort, IoEventHandlerDirection.InBound) { OnChanged = IoPort_OnChanged });
        }

        private bool IoPort_OnChanged(IoEventHandler eventSource)
        {
            this.isSensed = eventSource.IsActivate;

            if (!this.startRequest || !this.useFromLocal)
                return true;

            if (this.isSensed)
            {
                this.senseStarted = DateTime.Now;
            }
            else
            {
                double limitTimeMs = CalculateIoTimeMs();
                if (limitTimeMs < 0)
                    return true;

                TimeSpan sensedTime = DateTime.Now - senseStarted;
                if (sensedTime.TotalMilliseconds > limitTimeMs)
                    OnStickerSensed?.Invoke(senseStarted);
            }

            return true;
        }

        private double CalculateIoTimeMs()
        {
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            float stickerLengthMm = additionalSettings.LaserSetting.StickerSensorSetting.StickerLengthMm;

            ProductionG productionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (productionG == null || productionG.LineSpeedMpm <= 0)
                return -1;

            double lineSpdMps = Math.Max(0, productionG.LineSpeedMpm / 60.0);
            return stickerLengthMm / lineSpdMps;
        }

        public bool Start()
        {
            this.startRequest = true;
            return true;
        }

        public bool Stop()
        {
            this.startRequest = false;
            return true;
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
