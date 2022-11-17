//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.InspData;
//using DynMvp.UI;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UniEye.Base;
//using UniEye.Base.Inspect;
//using UniEye.Base.Settings;

//namespace UniScanG.Operation.Inspect
//{
//    internal class UniScanGInspectUnitManager : InspectUnitManager
//    {
//        private static object lockObject = new byte();
//        ProcessBufferManager processBufferManager = new ProcessBufferManager();

//        public ProcessBufferManager ProcessBufferManager
//        {
//            get { return processBufferManager; }
//        }

//        public override void Dispose()
//        {
//            base.Dispose();
//            processBufferManager.DisposeBuffer();
//        }

//        public override bool StartInspect(UnitInspectParam unitInspectParam)
//        {
//            ProcessBuffer processBuffer = processBufferManager.RequestBuffer();
//            if (processBuffer == null)
//            {
//                LogHelper.Warn(LoggerType.Inspection, "Inspect Skip. Buffer is full");
//                return false;
//            }

//           ((SheetCheckerInspectParam)unitInspectParam.AlgorithmInspectParam).ProcessBuffer = processBuffer;
//            processBuffer.IsUsing = 0;
//            return StartInspect(unitInspectParam, 0);
//        }

//        public void CalculateStep_PipelineInspected(UnitInspectParam unitInspectParam)
//        {
//            SheetCheckerInspectParam sheetCheckerInspectParam = unitInspectParam.AlgorithmInspectParam as SheetCheckerInspectParam;
//            SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult = sheetCheckerInspectParam.SheetCheckerAlgorithmResult;
//            Data.InspectionResult inspectionResult = sheetCheckerInspectParam.InspectionResult;

//            AlgorithmResult algorithmResult = unitInspectParam.AlgorithmResult;

//            if ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Text) > 0)
//            {
//                int seqNo = int.Parse(inspectionResult.InspectionNo);
//                (SystemManager.Instance().MainForm as UniScanG.Operation.UI.MainForm).WriteTimeLog("ProcessStepTime", seqNo, (long)algorithmResult.SpandTime.TotalMilliseconds);
//            }

//            if (algorithmResult.Good == false)
//            {
//                processBufferManager.ReturnBuffer(((SheetCheckerInspectParam)unitInspectParam.AlgorithmInspectParam).ProcessBuffer);
//                if (this.ProductInspected != null)
//                    ProductInspected(null, inspectionResult, null);
//                return;
//            }

//            sheetCheckerInspectParam.SheetCheckerAlgorithmResult.CalculateResult = algorithmResult;
//            inspectionResult.InspectionTime = inspectionResult.InspectionTime.Add(algorithmResult.SpandTime);

//            StartInspect(unitInspectParam, 1);//-----------1-------------//
//        }

//        public void DetectStep_PipelineInspected(UnitInspectParam unitInspectParam)
//        {
//            SheetCheckerInspectParam sheetCheckerInspectParam = unitInspectParam.AlgorithmInspectParam as SheetCheckerInspectParam;
//            SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult = sheetCheckerInspectParam.SheetCheckerAlgorithmResult;
//            Data.InspectionResult inspectionResult = sheetCheckerInspectParam.InspectionResult;

//            processBufferManager.ReturnBuffer(((SheetCheckerInspectParam)unitInspectParam.AlgorithmInspectParam).ProcessBuffer);

//            AlgorithmResult algorithmResult = unitInspectParam.AlgorithmResult;

//            if ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Text) > 0)
//            {
//                int seqNo = int.Parse(inspectionResult.InspectionNo);
//                (SystemManager.Instance().MainForm as UniScanG.Operation.UI.MainForm).WriteTimeLog("DetectStepTime", seqNo, (long)algorithmResult.SpandTime.TotalMilliseconds);
//            }

//            if (algorithmResult.Good == false)
//            {
//                if (this.ProductInspected != null)
//                    ProductInspected(null, inspectionResult, null);
//                return;
//            }

//            // 알고리즘 성공
//            sheetCheckerInspectParam.SheetCheckerAlgorithmResult.DetectResult = algorithmResult;
//            ((SheetCheckerInspectParam)unitInspectParam.AlgorithmInspectParam).ProcessBuffer = null;

//            sheetCheckerAlgorithmResult.SubResultList.AddRange(algorithmResult.SubResultList);
//            sheetCheckerAlgorithmResult.Message = algorithmResult.Message;
//            inspectionResult.InspectionTime = inspectionResult.InspectionTime.Add(algorithmResult.SpandTime);
//            ProbeResult probeResult = new VisionProbeResult(null, sheetCheckerAlgorithmResult, null);
//            inspectionResult.AddProbeResult(probeResult);

//            StartInspect(unitInspectParam, 2);//-------------2-----------//
//            //SaveStep_PipelineInspected(inspParam, inspResult, productInspectionResult);
//        }

//        public void SaveStep_PipelineInspected(UnitInspectParam unitInspectParam)
//        {
//            SheetCheckerInspectParam sheetCheckerInspectParam = unitInspectParam.AlgorithmInspectParam as SheetCheckerInspectParam;
//            SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult = sheetCheckerInspectParam.SheetCheckerAlgorithmResult;
//            Data.InspectionResult inspectionResult = sheetCheckerInspectParam.InspectionResult;

//            AlgorithmResult algorithmResult = unitInspectParam.AlgorithmResult;

//            if ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Text) > 0)
//            {
//                int seqNo = int.Parse(inspectionResult.InspectionNo);
//                (SystemManager.Instance().MainForm as UniScanG.Operation.UI.MainForm).WriteTimeLog("SaveStepTime", seqNo, (long)algorithmResult.SpandTime.TotalMilliseconds);
//            }

//            sheetCheckerInspectParam.SheetCheckerAlgorithmResult.SaveResult = unitInspectParam.AlgorithmResult;
//            inspectionResult.ExportTime = algorithmResult.SpandTime;

//            if (this.ProductInspected != null)
//            {
//                ProductInspected(null, inspectionResult, null);
//            }
//        }
//    }
//}
