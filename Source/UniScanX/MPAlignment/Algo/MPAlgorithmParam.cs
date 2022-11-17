using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanX.MPAlignment.Data;


namespace UniScanX.MPAlignment.Algo
{
    public class MPAlgorithmInspectParam : DynMvp.Vision.AlgorithmInspectParam
    {
        public MPAlgorithmInspectParam(
            ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov,
            Size wholeImageSize, Calibration calibration, DebugContext debugContext) 
            : base(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext)
        {
        }

        public MPAlgorithmInspectParam(AlgorithmInspectParam param) : base(param)
        {
            
        }
    }

    public class MPAlgorithmParam : DynMvp.Vision.AlgorithmParam
    {
        public bool Force; //강제로 2도 offset을 찾음 (문턱값에 도달하지 못할경우, 이옵션을 켜면 근처 제일 높은 엣지값을 갖는것을 취함)
        public float ThresholdX;//-255~+255   전국 엣지
        public float ThresholdY;//-255~+255   전국 엣지


        public float ThresholdX_2nd;//-255~+255   2도 엣지
        public float ThresholdY_2nd;//-255~+255   2도 엣지
        public float HysterisisX;//-255~+255   전국 엣지
        public float HysterisisY;//-255~+255   전국 엣지

        public int Estimated_Margin = 250; //pixel
               
        public MPAlgorithmParam()
        {

        }

        public override AlgorithmParam Clone()
        {
            MPAlgorithmParam param = new MPAlgorithmParam();
            param.Force = this.Force;
            param.ThresholdX = this.ThresholdX;
            param.ThresholdY = this.ThresholdY;
            param.ThresholdX_2nd = this.ThresholdX_2nd;
            param.ThresholdY_2nd = this.ThresholdY_2nd;

            param.HysterisisX = this.HysterisisX;
            param.HysterisisY = this.HysterisisY;
            return param;
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
            XmlHelper.SetValue(algorithmElement, "Force", this.Force);
            XmlHelper.SetValue(algorithmElement, "ThresholdX", this.ThresholdX);
            XmlHelper.SetValue(algorithmElement, "ThresholdY", this.ThresholdY);
            XmlHelper.SetValue(algorithmElement, "ThresholdX_2nd", this.ThresholdX_2nd);
            XmlHelper.SetValue(algorithmElement, "ThresholdY_2nd", this.ThresholdY_2nd);

            XmlHelper.SetValue(algorithmElement, "HysterisisX", this.HysterisisX);
            XmlHelper.SetValue(algorithmElement, "HysterisisY", this.HysterisisY);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);
            this.Force = XmlHelper.GetValue(algorithmElement, "Force", Force);
            this.ThresholdX = XmlHelper.GetValue(algorithmElement, "ThresholdX", ThresholdX);
            this.ThresholdY = XmlHelper.GetValue(algorithmElement, "ThresholdY", ThresholdY);
            this.ThresholdX_2nd = XmlHelper.GetValue(algorithmElement, "ThresholdX_2nd", ThresholdX_2nd);
            this.ThresholdY_2nd = XmlHelper.GetValue(algorithmElement, "ThresholdY_2nd", ThresholdY_2nd);

            this.HysterisisX = XmlHelper.GetValue(algorithmElement, "HysterisisX", HysterisisX);
            this.HysterisisY = XmlHelper.GetValue(algorithmElement, "HysterisisY", HysterisisY);
        }

    }
}
