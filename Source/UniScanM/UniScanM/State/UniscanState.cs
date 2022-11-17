using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
//using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;

namespace UniScanM.State
{
    public abstract class UniScanState : IProcesser
    {
        protected bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }

        public abstract bool IsTeachState { get; }

        protected int imageSequnece = -1;
        public int ImageSequnece
        {
            get { return imageSequnece; }
        }

        protected InspectState inspectState;
        public InspectState InspectState
        {
            get { return inspectState; }
        }

        public UniScanState()
        {
            initialized = false;
        }

        public void Initialize()
        {
            if (initialized == false)
                Init();
            initialized = true;
        }

        public ProcessTask Process(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            PreProcess();
            OnProcess(imageD, inspectionResult, inspectionOption);
            PostProcess(inspectionResult);
            return null;
        }

        /// <summary>
        /// 상태 변경시
        /// </summary>
        protected abstract void Init();

        /// <summary>
        ///  알고리즘 시작 전
        /// </summary>
        public abstract void PreProcess();

        /// <summary>
        /// 알고리즘
        /// </summary>
        /// <param name="algoImage"></param>
        /// <param name="inspectionResult"></param>
        public abstract void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null);

        /// <summary>
        /// 알고리즘 종료 후
        /// </summary>
        /// <param name="inspectionResult"></param>
        public abstract void PostProcess(DynMvp.InspData.InspectionResult inspectionResult);

        /// <summary>
        /// 다음 스탭 가져오기
        /// </summary>
        /// <param name="inspectionResult">현재 검사 결과</param>
        /// <returns></returns>
        public abstract UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult);

        public bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1)
        {
            throw new NotImplementedException();
        }

        public void CancelProcess(ProcessTask inspectionTask = null)
        {
            throw new NotImplementedException();
        }

        public IProcesser GetNextProcesser()
        {
            throw new NotImplementedException();
        }
         
    }
}
