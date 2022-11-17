using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using System.Drawing;

namespace UniScanX.MPAlignment.Data
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        SizeF patternSize = new SizeF();
        public SizeF PatternSize { get => patternSize; set => patternSize = value; }

        string imagePath;
        public string ImagePath { get => imagePath; set => imagePath = value; }

        public string ResultPath { get; set; }
        public string ModelPath { get; set; }
        bool isTrained;
        public bool IsTrained
        {
            get { return isTrained; }
            set { isTrained = value; }
        }

        public ModelDescription() : base()
        {

        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);
            XmlHelper.GetValue(modelDescElement, "patternSize", ref patternSize);
            imagePath = XmlHelper.GetValue(modelDescElement, "ImagePath", "");
            XmlHelper.SetValue(modelDescElement, "IsTrained", isTrained.ToString());
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);
            XmlHelper.SetValue(modelDescElement, "patternSize", patternSize);
            XmlHelper.SetValue(modelDescElement, "ImagePath", imagePath);
            isTrained = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "IsTrained", "false"));
        }
    }
}
