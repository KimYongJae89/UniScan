using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Data.Vision
{
    public class PatternInfo : BlobRect
    {
        public float WaistLengthRatio => (float)(this.waistLength / this.GetValue(PatternFeature.Width));

        public float WaistLength { get => this.waistLength; set => this.waistLength = value; }
        float waistLength;

        public PatternInfo() : base()
        {

        }

        public PatternInfo(BlobRect f)
        {
            base.Copy(f);
            this.waistLength = 0;
        }

        public new PatternInfo Clone()
        {
            PatternInfo patternInfo = new PatternInfo();
            patternInfo.Copy(this);
            return patternInfo;
        }

        public void Copy(PatternInfo srcPatternInfo)
        {
            base.Copy(srcPatternInfo);
            this.waistLength = srcPatternInfo.waistLength;
        }

        public override void LoadXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement[key];
                if (sumElement != null)
                    LoadXml(sumElement);
                return;
            }

            base.LoadXml(xmlElement);
            this.waistLength = XmlHelper.GetValue(xmlElement, "waistLength", this.waistLength);
        }

        public override void SaveXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(sumElement);
                SaveXml(sumElement);
                return;
            }

            base.SaveXml(xmlElement);
            XmlHelper.SetValue(xmlElement, "WaistLength", this.waistLength);
        }

        internal float GetValue(PatternFeature patternFeature)
        {
            float value = 0;
            switch (patternFeature)
            {
                case PatternFeature.Area:
                    value = this.Area;
                    break;

                case PatternFeature.Width:
                    value = this.BoundingRect.Width;
                    //value = this.RotateWidth;
                    //value = this.RotateWidth * Math.Cos(this.RotateAngleRad) + this.RotateHeight * Math.Abs(Math.Sin(this.RotateAngleRad));
                    System.Diagnostics.Debug.Assert(value >= 0);
                    break;

                case PatternFeature.Height:
                    value = this.BoundingRect.Height;
                    //value = this.RotateHeight;
                    //value = this.RotateHeight * Math.Cos(this.RotateAngleRad) + this.RotateWidth * Math.Abs(Math.Sin(this.RotateAngleRad));
                    System.Diagnostics.Debug.Assert(value >= 0);
                    break;

                case PatternFeature.Waist:
                    value = this.waistLength;
                    break;

                case PatternFeature.WaistRatio:
                    value = this.WaistLengthRatio;
                    break;
                //case PatternFeature.CenterX:
                //    value = this.CenterOffset.X;
                //    break;

                //case PatternFeature.CenterY:
                //    value = this.CenterOffset.Y;
                //    break;

                //case PatternFeature.AreaRatio:
                //    value = this.AreaRatio;
                //    break;

                default:
                    throw new NotImplementedException();
            }

            return value;
        }
    }
}
