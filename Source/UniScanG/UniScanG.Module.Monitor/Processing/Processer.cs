using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.Inspect;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Processing
{
    abstract class Processer : IProcesser
    {
        public abstract InspectState InspectState { get; }

        public void CancelProcess(ProcessTask inspectionTask = null)
        {
            throw new NotImplementedException();
        }
        public bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1)
        {
            throw new NotImplementedException();
        }

        public abstract IProcesser GetNextProcesser();

        public ProcessTask Process(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, UniEye.Base.Inspect.InspectOption inspectionOption = null)
        {
            Inspect.InspectOption myinspectionOption = inspectionOption as Inspect.InspectOption;
            if (myinspectionOption == null)
                return null;

            //if (myinspectionOption.CameraNo < 0 || myinspectionOption.Index < 0 || myinspectionOption.WatchType != Gravure.Data.WatchType.CHIP)
            //    return null;

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[myinspectionOption.CameraNo];

            InspectionResult myInspectionResult = inspectionResult as InspectionResult;
            myInspectionResult.ZoneIndex = myinspectionOption.ImageIndex;
            myInspectionResult.RollPos = myinspectionOption.RollPos;
            myInspectionResult.InspImageSize = imageD.Size;
            myInspectionResult.InspPelSize = calibration.PelSize;
            myInspectionResult.TeachData = myinspectionOption.TeachData;

            Algorithm algorithm = AlgorithmPool.Instance().GetAlgorithm(Vision.MyAlgorithm.TypeName);
            AlgorithmInspectParam algorithmInspectParam = algorithm.CreateAlgorithmInspectParam(imageD, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, calibration, myinspectionOption.DebugContext);
            AlgorithmResult algorithmResult = algorithm.Inspect(algorithmInspectParam);

            PostProcess(inspectionResult, algorithmResult);
            return null;
        }
        public abstract AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, Calibration calibration, DebugContext debugContext);

        public virtual void PostProcess(DynMvp.InspData.InspectionResult inspectionResult, AlgorithmResult algorithmResult)
        {
            InspectionResult myInspectionResult = inspectionResult as InspectionResult;
            MyAlgorithmResult myAlgorithmResult = algorithmResult as MyAlgorithmResult;
            TeachData teachData = myInspectionResult.TeachData;

            myInspectionResult.MarginSize = myAlgorithmResult.MarginUm;
            myInspectionResult.MarginRect = myAlgorithmResult.MarginRect;

            myInspectionResult.BlotSize = myAlgorithmResult.BlotUm;
            myInspectionResult.BlotRect = myAlgorithmResult.BlotRect;

            myInspectionResult.Defects = myAlgorithmResult.Defects.FindAll(f => f.SizeUm.Width > teachData.MinDefectLimitW && f.SizeUm.Height > teachData.MinDefectLimitL).ToArray();
        }
    }
}
