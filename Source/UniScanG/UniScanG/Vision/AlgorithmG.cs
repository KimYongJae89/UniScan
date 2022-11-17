using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Vision
{
    public abstract class AlgorithmG : Algorithm
    {
        public abstract AlgorithmParam CreateParam();

        #region Abstract
        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }
        public override string GetAlgorithmTypeShort()
        {
            throw new NotImplementedException();
        }
        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }
        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override void CopyFrom(DynMvp.Vision.Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            if (this.param == null)
                this.param = CreateParam();
            this.param.CopyFrom(algorithm.Param);
        }
        public override void Dispose()
        {
            this.param?.Dispose();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            this.AlgorithmName = XmlHelper.GetValue(algorithmElement, "AlgorithmName", this.AlgorithmName);

            if (this.param == null)
                this.param = CreateParam();

            this.param.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "AlgorithmName", this.AlgorithmName);
            XmlHelper.SetValue(algorithmElement, "AlgorithmType", GetAlgorithmType().ToString());

            if (param == null)
                XmlHelper.SetValue(algorithmElement, "IsEmpty", true);
            else
                param.SaveParam(algorithmElement);
        }
        #endregion  
    }

    public abstract class AlgorithmParamG: AlgorithmParam
    {
        public bool IsEmpty => this.isEmpty;
        protected bool isEmpty;

        public AlgorithmParamG()
        {
            this.isEmpty = false;
        }

        public override void Dispose() { }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            if (algorithmElement == null)
                throw new NullReferenceException();

            this.isEmpty = XmlHelper.GetValue(algorithmElement, "IsEmpty", this.isEmpty);
            base.LoadParam(algorithmElement);
        }
    }

    public abstract class AlgorithmModelParam
    {
        public AlgorithmModelParam() { Clear(); }
        public AlgorithmModelParam(AlgorithmModelParam src) { this.CopyFrom(src); }

        public abstract void Save(XmlElement xmlElement);
        public abstract void Load(XmlElement xmlElement);
        public abstract AlgorithmModelParam Clone();
        public abstract void CopyFrom(AlgorithmModelParam algorithmModelParam);

        public abstract void Clear();

        public void Save(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
            xmlElement.AppendChild(subElement);
            Save(subElement);
        }

        public void Load(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement[key];
            if (subElement == null)
                return;
            Load(subElement);
        }


    }
}
