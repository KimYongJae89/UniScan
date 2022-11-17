using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Trainer
{
    public class SheetTrainerParam : DynMvp.Vision.AlgorithmParam
    {
        bool isThinFilm;
        public bool IsThinFilm
        {
            get { return isThinFilm; }
            set { isThinFilm = value; }
        }

        float groupThreshold;
        public float GroupThreshold
        {
            get { return groupThreshold; }
            set { groupThreshold = value; }
        }

        int patternMaxGap;
        public int PatternMaxGap
        {
            get { return patternMaxGap; }
            set { patternMaxGap = value; }
        }
        
        int dielectricRecommendLowerTh;
        public int DielectricRecommendLowerTh
        {
            get { return dielectricRecommendLowerTh; }
            set { dielectricRecommendLowerTh = value; }
        }

        int dielectricRecommendUpperTh;
        public int DielectricRecommendUpperTh
        {
            get { return dielectricRecommendUpperTh; }
            set { dielectricRecommendUpperTh = value; }
        }

        int poleRecommendLowerTh;
        public int PoleRecommendLowerTh
        {
            get { return poleRecommendLowerTh; }
            set { poleRecommendLowerTh = value; }
        }

        int poleRecommendUpperTh;
        public int PoleRecommendUpperTh
        {
            get { return poleRecommendUpperTh; }
            set { poleRecommendUpperTh = value; }
        }

        public SheetTrainerParam()
        {
            groupThreshold = 15;
            patternMaxGap = 200;
            dielectricRecommendLowerTh = 0;
            dielectricRecommendUpperTh = 0;
            poleRecommendLowerTh = 0;
            poleRecommendUpperTh = 0;
        }

        public override AlgorithmParam Clone()
        {
            SheetTrainerParam clone = new SheetTrainerParam();

            clone.CopyFrom(this);

            return clone;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetTrainerParam srcParam = (SheetTrainerParam)srcAlgorithmParam;
            
            this.groupThreshold = srcParam.groupThreshold;
            this.patternMaxGap = srcParam.patternMaxGap;
            this.dielectricRecommendLowerTh = srcParam.dielectricRecommendLowerTh;
            this.dielectricRecommendUpperTh = srcParam.dielectricRecommendUpperTh;
            this.poleRecommendLowerTh= srcParam.poleRecommendLowerTh;
            this.poleRecommendUpperTh = srcParam.poleRecommendUpperTh;
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
            
            XmlHelper.SetValue(algorithmElement, "GroupThreshold", groupThreshold.ToString());
            XmlHelper.SetValue(algorithmElement, "PatternMaxGap", patternMaxGap.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricRecommendLowerTh", dielectricRecommendLowerTh.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricRecommendUpperTh", dielectricRecommendUpperTh.ToString());
            XmlHelper.SetValue(algorithmElement, "PoleRecommendLowerTh", poleRecommendLowerTh.ToString());
            XmlHelper.SetValue(algorithmElement, "PoleRecommendUpperTh", poleRecommendUpperTh.ToString());
            XmlHelper.SetValue(algorithmElement, "IsThinFilm", isThinFilm.ToString());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);
            
            groupThreshold = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "GroupThreshold", "15"));
            patternMaxGap = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "PatternMaxGap", "200"));
            dielectricRecommendLowerTh = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DielectricRecommendLowerTh", "0"));
            dielectricRecommendUpperTh = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DielectricRecommendUpperTh", "0"));
            poleRecommendLowerTh = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "PoleRecommendLowerTh", "0"));
            poleRecommendUpperTh = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "PoleRecommendUpperTh", "0"));

            isThinFilm = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "IsThinFilm", "false"));
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
