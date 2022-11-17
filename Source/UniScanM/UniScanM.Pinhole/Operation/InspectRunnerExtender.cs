using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniScanM.Pinhole;

namespace UniScanM.Pinhole.Operation
{
    public class InspectRunnerExtender: UniScanM.Operation.InspectRunnerExtender
    {
        public DateTime? StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public DateTime? EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
             
        /// <summary>
        /// 검사 시작 시간 (수동, Rollor 구동 감지, PLC 시작 입력, ...)
        /// </summary>
        DateTime? startTime;

        /// <summary>
        /// 검사 종료 시간 (수동, Rollor 정지 감지, PLC 정지 입력, ...)
        /// </summary>
        DateTime? endTime;

        public InspectRunnerExtender() : base()
        {
           
        }

        protected override InspectionResult CreateInspectionResult()
        {
            return new Data.InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            return base.GetInspectionNo();
        }
    }
}
