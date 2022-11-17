using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
//using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.Inspect;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Processing
{
    class InspectProcesser : Processer
    {
        public override InspectState InspectState => InspectState.Run;

        public SizeF MarginTarget { get => this.marginTarget; set => this.marginTarget = value; }
        SizeF marginTarget = SizeF.Empty;

        public SizeF BlotTarget { get => this.blotTarget; set => this.blotTarget = value; }
        SizeF blotTarget = SizeF.Empty;


        public InspectProcesser(SizeF marginTarget, SizeF blotTarget)
        {
            this.marginTarget = marginTarget;
            this.blotTarget = blotTarget;
        }

        public override IProcesser GetNextProcesser()
        {
            return this;
        }

        public override AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, Calibration calibration, DebugContext debugContext)
        {
            return new MyAlgorithmInspectionParam(clipImage, calibration, debugContext);
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult, AlgorithmResult algorithmResult)
        {
            base.PostProcess(inspectionResult, algorithmResult);

            InspectionResult myInspectionResult = inspectionResult as InspectionResult;
            MyAlgorithmResult myAlgorithmResult = algorithmResult as MyAlgorithmResult;

            TeachData teachData = myInspectionResult.TeachData;
            myInspectionResult.MarginTarget = this.marginTarget;
            myInspectionResult.MarginResult = Calculate(myAlgorithmResult.MarginUm, this.marginTarget, new SizeF(teachData.MarginLimitW, teachData.MarginLimitL) ,false);

            myInspectionResult.BlotTarget = this.blotTarget;
            myInspectionResult.BlotResult = Calculate(myAlgorithmResult.BlotUm, this.blotTarget, new SizeF(teachData.BlotLimitW, teachData.BlotLimitL), false);

            SizeF maxSizeDefect = myInspectionResult.MaxDefectSize;
            myInspectionResult.DefectResult = (maxSizeDefect.Width < teachData.MinDefectLimitW && maxSizeDefect.Height < teachData.MinDefectLimitL);

            myInspectionResult.UpdateJudgement();
        }

        private bool Calculate(SizeF size, SizeF target, SizeF limit, bool isLimitP)
        {
            float diffW, diffH;
            if (isLimitP)
            {
                diffW = Math.Abs(1 - size.Width / target.Width) * 100;
                diffH = Math.Abs(1 - size.Height / target.Height) * 100;
            }
            else
            {
                diffW = Math.Abs(size.Width - target.Width);
                diffH = Math.Abs(size.Height - target.Height);
            }

            return diffW < limit.Width && diffH < limit.Height;
        }
    }
}
