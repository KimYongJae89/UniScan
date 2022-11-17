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
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
//using UniScanG.Gravure.Data;
using DynMvp.InspData;
using UniScanG.Gravure.Settings;
using DynMvp.Vision;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Data;
using System.Windows.Forms;
using UniScanG.Common.Data;

namespace UniScanG.Module.Inspector.Device
{
    public class DeviceController: DeviceControllerG
    {
        IoPort ioPort = null;

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutVisionNG);
            if (this.ioPort != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(this.ioPort);

            base.Initialize(deviceBox);
        }

        public override void OnProductInspected(InspectionResult inspectionResult)
        {
            base.OnProductInspected(inspectionResult);

            if (ioPort == null)
                return;

            UniScanG.Data.Inspect.InspectionResult inspectionResultG = inspectionResult as UniScanG.Data.Inspect.InspectionResult;
            List<FoundedObjInPattern> sheetSubResultList = inspectionResultG?.GetSubResultList();

            LaserSetting laserSetting = UniScanG.Gravure.Settings.AdditionalSettings.Instance().LaserSetting;
            {
                if (CheckCondition(laserSetting.ConditionSettingCollection, sheetSubResultList))
                {
                    LogHelper.Info(LoggerType.Operation, string.Format("DeviceController::OnProductInspected - Add List. InspectionNo: {0}, GrabTime: {1}, InspectionStartTime: {2}, InspectionEndTime: {3}.",
                        inspectionResultG.InspectionNo,
                        inspectionResultG.ImageGrabbedTime.ToString("HH:mm:ss.fff"),
                        inspectionResultG.InspectionStartTime.ToString("HH:mm:ss.fff"),
                        inspectionResultG.InspectionEndTime.ToString("HH:mm:ss.fff")));
                    
                    inspectionResultG.SetPostProcessed(true);

                    lock (this.tupleList)
                        this.tupleList.Add(new Tuple<DateTime, string>(inspectionResultG.ImageGrabbedTime, inspectionResultG.InspectionNo));
                }
            }
        }

        public bool CheckCondition(ConditionSettingCollection conditionSettingCollection, List<FoundedObjInPattern> sheetSubResultList)
        {
            if (sheetSubResultList == null)
                return false;

            foreach (ConditionSetting conditionSetting in conditionSettingCollection)
            {
                if (conditionSetting.Check(sheetSubResultList))
                    return true;
            }
            return false;
        }

        protected override double CalculateOutputDelayMs(float lineSpdMpm)
        {
            double lineSpdMps = Math.Max(0, lineSpdMpm / 60.0);
            if (lineSpdMps == 0)
                return -1;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            double laserDistanceM = additionalSettings.LaserSetting.DistanceM;
            double safeDistanceM = additionalSettings.LaserSetting.SafeDistanceM;
            double distanceM = Math.Max(laserDistanceM - safeDistanceM, 0);

            return distanceM / lineSpdMps * 1000;
        }

        protected override int GetOutputHoldTimeMs(float lineSpdMpm)
        {
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            return additionalSettings.LaserSetting.HoldTimeMs;
        }

        protected override bool SetIo(bool active)
        {
            if (active)
                return SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(this.ioPort);
            else
                return SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(this.ioPort);
        }

        public override void Shutdown(string imName, bool restart) { }

        public override void Startup(string imName) { }

        public override void Launch(string imName, string[] args) { }

        public override void InitializeInspectModule(IWin32Window parent, string imName) { }

    }
}
