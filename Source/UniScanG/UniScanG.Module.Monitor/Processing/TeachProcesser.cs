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
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.Inspect;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Processing
{
    class TeachProcesser : Processer
    {
        public override InspectState InspectState => InspectState.Scan;

        List<MyAlgorithmResult> myAlgorithmResultList = new List<MyAlgorithmResult>();

        public TeachProcesser() { }

        public override IProcesser GetNextProcesser()
        {
            Settings.AdditionalSettings additionalSettings = (Settings.AdditionalSettings)AdditionalSettings.Instance();
            
            if (myAlgorithmResultList.Count >= additionalSettings.TeachInterval*6)
            {
                int src = myAlgorithmResultList.Count / 4;
                int cnt = myAlgorithmResultList.Count / 2;

                float marginW = GetMedianAverage(new Func<MyAlgorithmResult, float>(f => f.MarginUm.Width), src, cnt);
                float marginH = GetMedianAverage(new Func<MyAlgorithmResult, float>(f => f.MarginUm.Height), src, cnt);
                float blotW = GetMedianAverage(new Func<MyAlgorithmResult, float>(f => f.BlotUm.Width), src, cnt);
                float blotH = GetMedianAverage(new Func<MyAlgorithmResult, float>(f => f.BlotUm.Height), src, cnt);
                //float marginW = myAlgorithmResultList.OrderBy(f=>f.MarginUm.Width).Skip(src).Take(cnt).Average(f=>f.MarginUm.Width);
                //float marginH = myAlgorithmResultList.Average(f => f.MarginUm.Height);
                //float blotW = myAlgorithmResultList.Average(f => f.BlotUm.Width);
                //float blotH = myAlgorithmResultList.Average(f => f.BlotUm.Height);
                InspectProcesser inspectProcesser = new InspectProcesser(new SizeF(marginW,marginH), new SizeF(blotW,blotH));
                return inspectProcesser;
            }
            return this;
        }

        private float GetMedianAverage(Func<MyAlgorithmResult,float> func, int src, int cnt)
        {
            return myAlgorithmResultList.OrderBy(func).Skip(src).Take(cnt).Average(func);
        }

        public override AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, Calibration calibration, DebugContext debugContext)
        {
            return new MyAlgorithmInspectionParam(clipImage, calibration, debugContext);
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult, AlgorithmResult algorithmResult)
        {
            base.PostProcess(inspectionResult, algorithmResult);

            InspectionResult myInspectionResult = inspectionResult as InspectionResult;
            myInspectionResult.MarginResult = true;
            myInspectionResult.BlotResult= true;
            myInspectionResult.DefectResult = true;

            if (algorithmResult.Good)
                myAlgorithmResultList.Add((MyAlgorithmResult)algorithmResult);


        }
    }
}
