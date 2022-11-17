using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Vision.LengthVariation
{

    public class LengthVariationParam : UniScanG.Vision.AlgorithmParamG
    {
        public bool Use { get; set; } = false;
        public float Position { get; set; } = 0.2f;
        public float WidthUm { get; set; } = 1000;

        public override void SaveParam(XmlElement paramElement)
        {
            //if (!string.IsNullOrEmpty(key))
            //{
            //    XmlElement xmlElement = paramElement.OwnerDocument.CreateElement(key);
            //    paramElement.AppendChild(xmlElement);
            //    SaveParam(xmlElement, null);
            //    return;
            //}

            XmlHelper.SetValue(paramElement, "Use", this.Use);
            XmlHelper.SetValue(paramElement, "Position", this.Position);
            XmlHelper.SetValue(paramElement, "WidthUm", this.WidthUm);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            //if (!string.IsNullOrEmpty(key))
            //{
            //    LoadParam(paramElement[key], null);
            //    return;
            //}

            this.Use = XmlHelper.GetValue(paramElement, "Use", this.Use);
            this.Position = XmlHelper.GetValue(paramElement, "Position", this.Position);
            this.WidthUm = XmlHelper.GetValue(paramElement, "WidthUm", this.WidthUm);
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            LengthVariationParam srcParam = srcAlgorithmParam as LengthVariationParam;
            this.Use = srcParam.Use;
            this.Position = srcParam.Position;
            this.WidthUm = srcParam.WidthUm;
        }

        public override AlgorithmParam Clone()
        {
            LengthVariationParam clone = new LengthVariationParam();
            clone.CopyFrom(this);
            return clone;
        }
    }
}
