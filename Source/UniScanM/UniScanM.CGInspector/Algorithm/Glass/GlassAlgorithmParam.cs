using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanM.CGInspector.Data;

namespace UniScanM.CGInspector.Algorithm.Glass
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    //[DefaultPropertyAttribute("Name")]
    class GlassAlgorithmParam : DynMvp.Vision.AlgorithmParam
    {
        //[CategoryAttribute("Threshold"), DisplayName("Threshold Value"), DescriptionAttribute("Threshold")]
        //public int SplitCount => splitCount;
        //int splitCount;

        [CategoryAttribute("Threshold"), DisplayName("Threshold Value"), DescriptionAttribute("Threshold")]
        public MinMaxPair<sbyte> BinalizeTh => binalizeTh;
        MinMaxPair<sbyte> binalizeTh;

        [CategoryAttribute("Blob"), DisplayName("MergeSize"), DescriptionAttribute("MergeSize")]
        public int CloseNum { get => this.closeTime; set => this.closeTime = value; }
        int closeTime;

        [CategoryAttribute("Blob"), DisplayName("Blob Size"), DescriptionAttribute("Blob")]
        public MinMaxPair<int> BlobArea => blobArea;
        MinMaxPair<int> blobArea;

        public GlassAlgorithmParam()
        {
            this.binalizeTh = MinMaxPair<sbyte>.Empty;
            this.blobArea = MinMaxPair<int>.Empty;
            this.closeTime = 0;
        }

        public GlassAlgorithmParam(MinMaxPair<sbyte> binalize, MinMaxPair<int> blobSize, int closeTime)
        {
            this.binalizeTh = binalize;
            this.blobArea = blobSize;
            this.closeTime = closeTime;
        }

        public override AlgorithmParam Clone()
        {
            GlassAlgorithmParam param = new GlassAlgorithmParam();
            //param.splitCount = this.splitCount;
            param.binalizeTh = this.binalizeTh.Clone();
            param.closeTime = this.closeTime;
            param.blobArea = this.blobArea.Clone();
            return param;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //XmlHelper.SetValue(algorithmElement, "SplitCount", this.splitCount);
            binalizeTh.Save(algorithmElement, "BinalizeTh");
            XmlHelper.SetValue(algorithmElement, "CloseNum", this.closeTime);
            blobArea.Save(algorithmElement, "BlobArea");
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            //this.splitCount = XmlHelper.GetValue(algorithmElement, "SplitCount", this.splitCount);
            binalizeTh.Load(algorithmElement, "BinalizeTh");
            this.closeTime = XmlHelper.GetValue(algorithmElement, "CloseTime", this.closeTime);
            blobArea.Load(algorithmElement, "BlobArea");
        }
    }

}
