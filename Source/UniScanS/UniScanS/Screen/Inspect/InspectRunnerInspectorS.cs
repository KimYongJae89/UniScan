using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI; 
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanS.Inspect;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Vision.FiducialFinder;
using UniScanS.Screen.Vision;
using System.Collections.Generic;
using UniScanS.Vision;
using UniScanS.Common.Exchange;
using UniScanS.Screen.Data;
using DynMvp.Authentication;
using UniScanS.Data;
using System.Diagnostics;
using UniScanS.Common;
using DynMvp.Devices.FrameGrabber;
using System.Xml;

namespace UniScanS.Screen.Inspect
{
    public  class InspectRunnerInspectorS : UniScanS.Inspect.InspectRunner
    {
        public InspectRunnerInspectorS() : base()
        {
            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                for (int i = 0; i < 5; i++)
                {
                    ProcessBufferSetS processBufferSetS = new ProcessBufferSetS(SheetInspector.TypeName, imageDevice.ImageSize.Width, imageDevice.ImageSize.Height);
                    this.processBufferManager.AddProcessBufferSet(imageDevice, processBufferSetS);
                }

                this.grabProcesser = new GrabProcesserS(imageDevice);
                this.grabProcesser.StartInspectionDelegate = StartFiducialInspect;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            processBufferManager.Dispose();
        }

        protected override void SetupUnitManager() { }

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().OpState != OpState.Idle)
                return false;

            if (SystemManager.Instance().CurrentModel == null)
                return false;
            
            if (SystemManager.Instance().CurrentModel.ModelDescription.IsTrained == false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            if (SystemManager.Instance().ProductionManager.CurProduction == null)
                return false;

            FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);
            SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);

            if (InspectorSetting.Instance().IsFiducial == true)
            {
                InspectUnit fiducialUnit = new InspectUnit(FiducialFinder.TypeName, fiducialFinder);
                fiducialUnit.UnitInspected = FiducialInspected;
                inspectUnitManager.Add(fiducialUnit);
            }

            InspectUnit inspectUnit = new InspectUnit(sheetInspector.GetAlgorithmType(), sheetInspector);
            inspectUnit.UnitInspected = DetectorInspected;
            inspectUnitManager.Add(inspectUnit);

            inspectUnitManager.AllUnitInspected = InspectDone;

            inspectUnitManager.Run();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice device in imageDeviceHandler)
            {
                if (device is Camera)
                    ((Camera)device).SetupGrab();
            }

            grabProcesser.Start();

            //imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            //imageDeviceHandler.SetExposureTime(2000);
            imageDeviceHandler.GrabMulti();
            SystemState.Instance().SetWait();

            //LogHelper.Info(LoggerType.Inspection, string.Format("Start - Model : {0}, Lot : {1}", SystemManager.Instance().CurrentModel.Name, SystemManager.Instance().ProductionManager.CurProduction.LotNo));

            return true;
        }

        public override void ExitWaitInspection()
        {
            grabProcesser.Stop();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.Stop();

            //imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed = null;
            
            inspectUnitManager.Stop();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (inspectUnitManager.IsRunning() == true || stopwatch.ElapsedMilliseconds >= 30000)
                Thread.Sleep(100);

            stopwatch.Stop();

            inspectUnitManager.Dispose();

            processBufferManager.Return(null);

            SystemState.Instance().SetInspectState(InspectState.Done);
            SystemState.Instance().SetIdle();

            //LogHelper.Info(LoggerType.Inspection, string.Format("Stop - Total {0}", SystemManager.Instance().ProductionManager.CurProduction.Total));
        }

        public override void EnterPauseInspection()
        {
            if (SystemState.Instance().InspectState == InspectState.Pause)
            {
                grabProcesser.ExitPause();
                SystemState.Instance().SetInspectState(InspectState.Wait);
            }
            else
            {
                grabProcesser.EnterPause();
                SystemState.Instance().SetInspectState(InspectState.Pause);
            }
        }

        public void FiducialInspected(UnitInspectItem unitInspectParam)
        {
            if (unitInspectParam.InspectionResult.AlgorithmResultLDic.ContainsKey(FiducialFinder.TypeName) == false)
            {
                Debug.WriteLine("FiducialInspected not contain");
                return;
            }

            FiducialFinderAlgorithmResult algorithmResult = (FiducialFinderAlgorithmResult)unitInspectParam.InspectionResult.AlgorithmResultLDic[FiducialFinder.TypeName];
            
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.F_FOUNDED, algorithmResult.Good.ToString(), algorithmResult.OffsetFound.Width.ToString(), algorithmResult.OffsetFound.Height.ToString());
            StartSheetInspector(unitInspectParam.InspectionResult, unitInspectParam, algorithmResult.Good, algorithmResult.OffsetFound);
        }

        public void DetectorInspected(UnitInspectItem unitInspectParam)
        {
            Debug.WriteLine("DetectorInspected");
            
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectParam.AlgorithmInspectParam;
            ProcessBufferSet processBufferSet = sheetAlgorithmInspectParam.ProcessBufferSet;
            processBufferManager.Return(processBufferSet);
        }

        public void StartSheetInspector(InspectionResult inspectionResult, UnitInspectItem unitInspectParam, bool fidResult, SizeF fidOffset)
        {
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectParam.AlgorithmInspectParam;
            sheetAlgorithmInspectParam.FidResult = fidResult;
            sheetAlgorithmInspectParam.FidOffset = fidOffset;

            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, unitInspectParam.InspectionOption);

            inspectUnitManager.StartInspect(SheetInspector.TypeName, unitInspectItem);
        }

        private void SaveProduction(Production production)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement productionElement = xmlDocument.CreateElement("", "Production", "");
            xmlDocument.AppendChild(productionElement);
            production.Save(productionElement);
            
            try
            {
                string fileName = Path.Combine(PathSettings.Instance().Result, "CurProduction.src");
                string readFileName = Path.Combine(PathSettings.Instance().Result, "CurProduction.csv");
                string bakupFileName = Path.Combine(PathSettings.Instance().Result, "CurProduction.src");

                xmlDocument.Save(fileName);
                File.Replace(fileName, readFileName, bakupFileName, false);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, e.Message);
            }
        }

        public override void InspectDone(InspectionResult inspectionResult)
        {
            Debug.WriteLine("InspectDone");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.InspectDone(inspectionResult);
            SystemManager.Instance().ExportData(inspectionResult);
            
            ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
            //SaveProduction(productionG);
            
            stopwatch.Stop();
            inspectionResult.ExportTime = stopwatch.Elapsed;
            
            //LogHelper.Info(LoggerType.Inspection, string.Format("Sheet No : {0}, Spend Time : {1} ms, Export Time : {2} ms", 
            //    unitInspectItem.InspectionResult.InspectionNo,
            //    unitInspectItem.InspectionResult.AlgorithmResultLDic[SheetInspector.TypeName].SpandTime.ToString("ss\\.fff"),
            //    unitInspectItem.InspectionResult.ExportTime.ToString("ss\\.fff")));

            if (inspectUnitManager.IsAlgorithmComplete() == true)
                SystemState.Instance().SetInspectState(InspectState.Wait);
            else
                SystemState.Instance().SetInspectState(InspectState.Run);

            IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            clientExchangeOperator.SendInspectDone(inspectionResult.InspectionNo, productionG.StartTime.ToString("yy-MM-dd"));
        }

        public void StartSheetInspector(bool fidResult, SizeF fidOffset)
        {
            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);


            IntPtr intPtr = grabProcesser.GetGrabbedImagePtr();
            if (intPtr == IntPtr.Zero)
            {
                Debug.WriteLine("StartSheetInspector intPtr null");
                return;
            }
                
            ImageD image = imageDevice.GetGrabbedImage(intPtr);
            if (image == null)
            {
                Debug.WriteLine("StartSheetInspector image null");
                return;
            }   

            ProcessBufferSet processBufferSet = processBufferManager.Request(imageDevice);
            if (processBufferSet == null)
            {
                Debug.WriteLine("StartSheetInspector processBufferSet null");
                return;
            }   

            SheetInspectParam sheetAlgorithmInspectParam = new SheetInspectParam(image, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            sheetAlgorithmInspectParam.ClipImage = image;
            sheetAlgorithmInspectParam.ProcessBufferSet = processBufferSet;
            sheetAlgorithmInspectParam.FidResult = fidResult;
            sheetAlgorithmInspectParam.FidOffset = fidOffset;

            InspectionResult inspectionResult = BuildInspectionResult();

            InspectOption inspectionOption = new InspectOption(imageDevice, intPtr);
            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, inspectionOption);

            inspectUnitManager.StartInspect(SheetInspector.TypeName, unitInspectItem);
        }

        public void StartFiducialInspect(ImageDevice imageDevice, IntPtr ptr)
        {
            SystemState.Instance().SetInspectState(InspectState.Run);

            InspectOption inspectionOption = new InspectOption(imageDevice, ptr);

            ProcessBufferSet processBufferSet = processBufferManager.Request(imageDevice);

            if (processBufferSet == null)
            {
                return;
            }

            ImageD image = imageDevice.GetGrabbedImage(ptr);
            SheetInspectParam sheetAlgorithmInspectParam = new SheetInspectParam(image, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            sheetAlgorithmInspectParam.ClipImage = image;
            sheetAlgorithmInspectParam.ProcessBufferSet = processBufferSet;
            InspectionResult inspectionResult = BuildInspectionResult();
            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, inspectionOption);

            inspectUnitManager.StartInspect(FiducialFinder.TypeName, unitInspectItem);
        }

        public void AdjustFidOffset(bool result, float width, float height)
        {
            SystemState.Instance().SetInspectState(InspectState.Run);

            if (InspectorSetting.Instance().IsFiducial == true)
                return;
            
            StartSheetInspector(result, new SizeF(width, height));
        }
    }
}
