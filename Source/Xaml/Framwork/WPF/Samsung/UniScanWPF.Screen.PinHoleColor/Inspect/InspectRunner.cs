using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanWPF.Screen.PinHoleColor.Color.Settings;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public class InspectRunner : UniEye.Base.Inspect.DirectTriggerInspectRunner
    {
        int colorNgCount = 0;
        bool isRunning;

        List<InspectSet> inspectSetList = new List<InspectSet>();
        CancellationTokenSource source;

        public bool IsRunning { get => isRunning; }

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().OpState != OpState.Idle)
                return false;
            
            if (SystemManager.Instance().CurrentModel == null)
            {
                CustomMessageBox.Show("Model is null !!");
                return false;
            }

            try
            {
                string productionName = DateTime.Now.ToString("yyyyMMddHHmmss");

                SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, productionName);
                SystemManager.Instance().ProductionManager.Save();

                inspectSetList.Clear();

                ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
                foreach (ImageDevice imageDevice in imageDeviceHandler)
                {
                    DetectorParam param = SystemManager.Instance().CurrentModel.DeviceDictionary[imageDevice];

                    InspectSet inspectSet = new InspectSet(imageDevice, Detector.Create(param), param, Inspected);
                    inspectSetList.Add(inspectSet);
                    BufferManager.Instance().AddInspectSet(inspectSet);

                    imageDevice.ImageGrabbed = ImageGrabbed;
                }

                BufferManager.Instance().Connect();

                source = new CancellationTokenSource();

                inspectSetList.ForEach(set => set.Start());
                ResultExportManager.Instance().Start();

                InspectResult.Reset();
                ResultCombiner.Instance().Reset();

                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRun), true);

                colorNgCount = 0;
                imageDeviceHandler.GrabMulti();

                if (SystemState.Instance().OpState != OpState.Teach)
                    SystemState.Instance().SetInspect();

                isRunning = true;

                return true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.Message);
            }

            return false;
        }
        
        public override void ExitWaitInspection()
        {
            try
            {
                source.Cancel();

                ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
                imageDeviceHandler.Stop();

                foreach (ImageDevice imageDevice in imageDeviceHandler)
                    imageDevice.ImageGrabbed = null;

                inspectSetList.ForEach(set => set.Stop());
                inspectSetList.Clear();
                BufferManager.Instance().Clear();

                ResultCombiner.Instance().Reset();

                ResultExportManager.Instance().Stop();

                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRun), false);

                SystemManager.Instance().ProductionManager.Save();

                if (SystemState.Instance().OpState != OpState.Teach)
                    SystemState.Instance().SetIdle();

                isRunning = false;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.Message);
            }

        }

        private void Inspected(InspectResult inspectResult)
        {
            inspectResult.EndTime = DateTime.Now;

            if (inspectResult.DetectorResult is Color.Inspect.ColorDetectorResult)
            {
                if (SystemState.Instance().OpState != OpState.Teach)
                {
                    if (inspectResult.Judgment == DynMvp.InspData.Judgment.Reject)
                    {
                        colorNgCount++;

                        if (colorNgCount >= ColorSettings.Instance().NgCount)
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutColor), true);
                            Thread.Sleep(ColorSettings.Instance().SignalTime);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutColor), false);
                        }
                    }
                    else
                    {
                        colorNgCount = 0;
                    }

                    if (inspectResult.Judgment != DynMvp.InspData.Judgment.Accept)
                        ResultExportManager.Instance().ExportResult(inspectResult);
                }
            }

            SystemManager.Instance().Inspected(inspectResult);
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            InspectSet inspectSet = inspectSetList.Find(set => set.TargetDevice == imageDevice);

            if (inspectSet == null)
            {
                LogHelper.Debug(LoggerType.Operation, "Not found inspect set.");
                return;
            }

            InspectResult inspectResult = new InspectResult(imageDevice);
            inspectSet.Enqueue(new Tuple<InspectResult, IntPtr>(inspectResult, ptr));
        }
    }
}