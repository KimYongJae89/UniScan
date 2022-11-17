using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using DynMvp.Device.Device.MotionController;
//using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using UniScanG.Common.Settings;
using UniScanG.Gravure.Settings;
using UniEye.Base.MachineInterface;
using UniEye.Base.Data;
using UniScanG.MachineIF;

namespace UniScanG.Gravure.Device
{
    public abstract class DeviceControllerG : UniScanG.Device.DeviceController
    {
        public MachineIfMonitor MachineIfMonitor => machineIfMonitor;
        protected MachineIfMonitor machineIfMonitor;

        ThreadHandler ioThreadHandler = null;
        TimeOutTimer ioOutputHolder = null;
        protected List<Tuple<DateTime, string>> tupleList = new List<Tuple<DateTime, string>>();

        public override void Release()
        {
            this.ioThreadHandler?.Stop();
            this.machineIfMonitor?.Stop();

            base.Release();
        }

        public override bool OnEnterWaitInspection(params object[] args)
        {
            tupleList.Clear();

            LogHelper.Debug(LoggerType.Operation, "DeviceControllerG::OnEnterWaitInspection");
            this.ioOutputHolder = new TimeOutTimer() { OnTimeout = IoOutputHolder_OnTimeout };

            this.ioThreadHandler?.Stop();
            this.ioThreadHandler = new ThreadHandler("DeviceControllerGIoThreadProc", new Thread(IoThreadProc), false);
            this.ioThreadHandler.Start();

            return true;
        }

        public override bool OnExitWaitInspection()
        {
            LogHelper.Debug(LoggerType.Operation, "DeviceControllerG::OnExitWaitInspection");
            if (!base.OnExitWaitInspection())
                return false;

            this.ioThreadHandler?.Stop();

            tupleList.Clear();

            return true;
        }

        public void SetAsyncMode(float lineSpeedMpm)
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;

            EAsyncMode asyncMode = additionalSettings.AsyncMode;
            LogHelper.Debug(LoggerType.Operation, $"DeviceControllerG::SetAsyncMode - asyncMode: {asyncMode}, lineSpeedMpm: {lineSpeedMpm}");

            TriggerMode triggerMode = TriggerMode.Software;
            float asyncGrabHz = additionalSettings.ConvertMpm2Hz(-1);

            if (lineSpeedMpm <= additionalSettings.MinimumLineSpeed)
                lineSpeedMpm = additionalSettings.MinimumLineSpeed;

            switch (additionalSettings.AsyncMode)
            {
                case Settings.EAsyncMode.True:
                    triggerMode = TriggerMode.Software;
                    if (additionalSettings.AsyncGrabMpm < 0)
                        asyncGrabHz = additionalSettings.ConvertMpm2Hz(lineSpeedMpm);
                    break;

                case Settings.EAsyncMode.False:
                    triggerMode = TriggerMode.Hardware;
                    break;

                case Settings.EAsyncMode.Auto:
                    triggerMode = TriggerMode.Hardware;

                    if (lineSpeedMpm > additionalSettings.AsyncGrabMpm)
                    {
                        triggerMode = TriggerMode.Software;
                        asyncGrabHz = additionalSettings.ConvertMpm2Hz(lineSpeedMpm);
                    }
                    break;
            }
            LogHelper.Debug(LoggerType.Operation, $"DeviceControllerG::SetAsyncMode - triggerMode: {triggerMode}, asyncGrabHz: {asyncGrabHz}");

            imageDeviceHandler.SetTriggerMode(triggerMode);
            if (triggerMode == TriggerMode.Software)
                imageDeviceHandler.SetAquationLineRate(asyncGrabHz);
        }

        private void IoThreadProc()
        {
            bool sleep = false;
            while (!this.ioThreadHandler.RequestStop)
            {
                if (sleep)
                    Thread.Sleep(50);

                sleep = true;
                ProductionG curProductionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                if (curProductionG == null)
                    continue;

                double ioOutputDelayMs = CalculateOutputDelayMs(curProductionG.LineSpeedMpm);
                if (ioOutputDelayMs < 0)
                    continue;

                DateTime now = DateTime.Now;
                DateTime delayedTime = now.AddMilliseconds(-ioOutputDelayMs);

                Tuple<DateTime, string> found = null;
                lock (this.tupleList)
                {
                    found = this.tupleList.Find(f => f.Item1.CompareTo(delayedTime) < 0);
                    if (found == null)
                        continue;

                    this.tupleList.Remove(found);
                }

                LogHelper.Info(LoggerType.IO, string.Format("DeviceController::OnProductInspected - Set Io Active. InspectionNo: {0}, GrabTime: {1}, Delay: {2}[ms], Now: {3}.",
                    found.Item2,
                    found.Item1.ToString("HH:mm:ss.fff"),
                    ioOutputDelayMs,
                    now.ToString("HH:mm:ss.fff")));

                sleep = false;
                if(SetIo(true))
                    this.ioOutputHolder.Restart(GetOutputHoldTimeMs(curProductionG.LineSpeedMpm));
            }
        }

        private void IoOutputHolder_OnTimeout(object sender, EventArgs e)
        {
            LogHelper.Info(LoggerType.IO, "DeviceController::OnProductInspected - Set Io Deactive");
            SetIo(false);
        }

        protected abstract double CalculateOutputDelayMs(float lineSpdMpm);
        protected abstract int GetOutputHoldTimeMs(float lineSpdMpm);
        protected abstract bool SetIo(bool active);
    }
}
