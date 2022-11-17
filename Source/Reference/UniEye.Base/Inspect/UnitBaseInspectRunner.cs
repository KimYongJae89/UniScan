using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;

namespace UniEye.Base.Inspect
{
    public delegate void InspectDoneDelegate(InspectionResult inspectionResult);

    /// <summary>
    /// 검사 객체를 Unit 단위로 관리. PipeLine 구조 적용
    /// </summary>
    public abstract class UnitBaseInspectRunner : InspectRunner
    {
        InspectDoneDelegate inspectDoneDelegate;
        
        public InspectUnitManager InspectUnitManager
        {
            get { return inspectUnitManager; }
        }

        protected InspectUnitManager inspectUnitManager = null;
        
        public UnitBaseInspectRunner()
        {
            this.inspectUnitManager = new InspectUnitManager();
            SetupUnitManager();
        }

        protected abstract void SetupUnitManager();

        ~UnitBaseInspectRunner()
        {
            this.inspectUnitManager.Dispose();
            this.inspectUnitManager = null;
        }

        public void AddInspectDoneDelegate(InspectDoneDelegate inspectDoneDelegate)
        {
            this.inspectDoneDelegate += inspectDoneDelegate;
        }

        public virtual void InspectDone(InspectionResult inspectionResult)
        {
            this.inspectDoneDelegate(inspectionResult);
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        { 
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            Debug.Assert(imageD != null);

            AlgorithmInspectParam algorithmInspectParam = new AlgorithmInspectParam(imageD, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, SystemManager.Instance().DeviceBox.CameraCalibrationList[0], inspectionOption.DebugContext);
            UnitInspectItem unitInspectItem = CreateInspectItem(algorithmInspectParam, inspectionResult, inspectionOption);
            if (unitInspectItem == null)
            {
                LogHelper.Error(LoggerType.Inspection, "UnitBaseInspectRunner::Inspect Fail");
                return;
            }

            inspectUnitManager.StartInspect(unitInspectItem);
        }

        protected virtual UnitInspectItem CreateInspectItem(AlgorithmInspectParam algorithmInspectParam, InspectionResult inspectionResult, InspectOption inspectionOption)
        {
            return new UnitInspectItem(algorithmInspectParam, inspectionResult, inspectionOption);
        }


        public void InspectDone(UnitInspectItem unitInspectItem)
        {
            SystemManager.Instance().ProductionManager.Save(PathSettings.Instance().Result);
        }
    }
}
