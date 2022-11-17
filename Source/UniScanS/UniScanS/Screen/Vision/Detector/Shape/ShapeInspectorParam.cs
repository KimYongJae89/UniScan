using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanS.Screen.Data;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Detector.Shape
{
    public class ShapeInspectorParam
    {
        string name = "Shape";

        bool useInspect;
        public bool UseInspect
        {
            get { return useInspect; }
            set { useInspect = value; }
        }

        List<SheetPattern> patternList = new List<SheetPattern>();
        public List<SheetPattern> PatternList
        {
            get { return patternList; }
            set { patternList = value; }
        }
        
        float diffTolerence;
        public float DiffTolerence
        {
            get { return diffTolerence; }
            set { diffTolerence = value; }
        }

        bool useHeightDiffTolerence = false;
        public bool UseHeightDiffTolerence
        {
            get { return useHeightDiffTolerence; }
            set { useHeightDiffTolerence = value; }
        }

        float heightDiffTolerence;
        public float HeightDiffTolerence
        {
            get { return heightDiffTolerence; }
            set { heightDiffTolerence = value; }
        }

        protected int minPatternArea;
        public int MinPatternArea
        {
            get { return minPatternArea; }
            set { minPatternArea = value; }
        }

        public ShapeInspectorParam Clone()
        {
            ShapeInspectorParam clone = new ShapeInspectorParam();
            clone.Copy(this);

            return clone;
        }

        public ShapeInspectorParam()
        {
            useInspect = true;
            diffTolerence = 15;
            useHeightDiffTolerence = false;
            heightDiffTolerence = 20;
            MinPatternArea = 100;
        }

        public void Copy(ShapeInspectorParam srcParam)
        {
            this.patternList.AddRange(srcParam.patternList.ToArray());
            this.diffTolerence = srcParam.diffTolerence;
            this.useHeightDiffTolerence = srcParam.useHeightDiffTolerence;
            this.heightDiffTolerence = srcParam.heightDiffTolerence;
            this.minPatternArea = srcParam.minPatternArea;
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlElement shapeElement = algorithmElement.OwnerDocument.CreateElement(name);
            algorithmElement.AppendChild(shapeElement);

            XmlHelper.SetValue(shapeElement, "UseInspect", useInspect.ToString());
            XmlHelper.SetValue(shapeElement, "DiffTolerence", diffTolerence.ToString());
            XmlHelper.SetValue(shapeElement, "UseHeightDiffTolerence", useHeightDiffTolerence.ToString());
            XmlHelper.SetValue(shapeElement, "HeightDiffTolerence", heightDiffTolerence.ToString());
            XmlHelper.SetValue(shapeElement, "minPatternArea", MinPatternArea.ToString());

            foreach (SheetPattern pattern in patternList)
            {
                XmlElement patternElement = algorithmElement.OwnerDocument.CreateElement("Pattern");
                shapeElement.AppendChild(patternElement);

                pattern.SaveParam(patternElement);
            }
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            XmlElement shapeElement = algorithmElement[name];

            if (shapeElement == null)
                return;

            useInspect = Convert.ToBoolean(XmlHelper.GetValue(shapeElement, "UseInspect", "true"));
            diffTolerence = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "DiffTolerence", "15"));
            useHeightDiffTolerence = Convert.ToBoolean(XmlHelper.GetValue(shapeElement, "UseHeightDiffTolerence", "false"));
            heightDiffTolerence = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "HeightDiffTolerence", "20"));
            MinPatternArea = Convert.ToInt32(XmlHelper.GetValue(shapeElement, "minPatternArea", "100"));

            foreach (XmlElement patternElement in shapeElement)
            {
                if (patternElement.Name == "Pattern")
                {
                    SheetPattern pattern = new SheetPattern(patternElement);
                    patternList.Add(pattern);
                }
            }
        }
    }
}
