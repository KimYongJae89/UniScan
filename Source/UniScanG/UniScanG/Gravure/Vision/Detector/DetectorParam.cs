using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Detector
{
    public class DetectorModelParam : AlgorithmModelParam
    {
        public bool AttackDiffUse { get => this.attackDiffUse; set => this.attackDiffUse = value; }
        bool attackDiffUse;

        public float AttackMinValue { get => this.attackMinValue; set => this.attackMinValue = value; }
        float attackMinValue;

        public float AttackDiffValue { get => this.attackDiffValue; set => this.attackDiffValue = value; }
        float attackDiffValue;


        public DetectorModelParam() : base() { }

        public DetectorModelParam(AlgorithmModelParam src) : base(src) { }

        public override void Clear()
        {
            this.attackDiffUse = false;
            this.attackDiffValue = 75;
            this.attackMinValue = 50;
        }

        public override AlgorithmModelParam Clone()
        {
            return new DetectorModelParam(this);
        }

        public override void CopyFrom(AlgorithmModelParam algorithmModelParam)
        {
            DetectorModelParam detectorModelParam = (DetectorModelParam)algorithmModelParam;
            this.attackDiffUse = detectorModelParam.attackDiffUse;
            this.attackDiffValue = detectorModelParam.attackDiffValue;
            this.attackMinValue = detectorModelParam.attackMinValue;
        }

        public override void Load(XmlElement xmlElement)
        {
            this.attackDiffUse = XmlHelper.GetValue(xmlElement, "AttackDiffUse", this.attackDiffUse);
            this.attackDiffValue = XmlHelper.GetValue(xmlElement, "AttackDiffValue", this.attackDiffValue);
            this.attackMinValue = XmlHelper.GetValue(xmlElement, "AttackMinValue", this.attackMinValue);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "AttackDiffUse", this.attackDiffUse);
            XmlHelper.SetValue(xmlElement, "AttackDiffValue", this.attackDiffValue);
            XmlHelper.SetValue(xmlElement, "AttackMinValue", this.attackMinValue);
        }
    }

    public class DetectorParam : UniScanG.Vision.AlgorithmParamG
    {
        public enum ECriterionLength { Min, Max, Diagonal }

        public DetectorModelParam ModelParam => this.modelParam == null ? SystemManager.Instance().CurrentModel.DetectorModelParam : this.modelParam;
        DetectorModelParam modelParam;

        public int MaximumDefectCount { get => maximumDefectCount; set => maximumDefectCount = value; }
        int maximumDefectCount;

        public bool UseMultiThread { get => useMultiThread; set => useMultiThread = value; }
        bool useMultiThread;

        public int TimeoutMs { get => timeoutMs; set => timeoutMs = value; }
        int timeoutMs;

        public bool Reconstruction { get => reconstruction; set => reconstruction = value; }
        bool reconstruction;

        public bool FineSizeMeasure { get => fineSizeMeasure; set => fineSizeMeasure = value; }
        bool fineSizeMeasure;

        public float FineSizeMeasureThresholdMul { get => fineSizeMeasureThresholdMul; set => fineSizeMeasureThresholdMul = value; }
        float fineSizeMeasureThresholdMul;

        public int FineSizeMeasureSizeMul { get => fineSizeMeasureSizeMul; set => fineSizeMeasureSizeMul = value; }
        int fineSizeMeasureSizeMul;

        //public _SpreadTracerParam SpreadTracerParam { get => this.spreadTracerParam; }
        //_SpreadTracerParam spreadTracerParam;

        public bool UseSpreadTrace { get => this.useSpreadTrace; set => this.useSpreadTrace = value; }
        bool useSpreadTrace;

        public bool IgnoreLongDefect { get => this.ignoreLongDefect; set => this.ignoreLongDefect = value; }
        bool ignoreLongDefect;

        public bool MergingDefects { get => this.mergingDefects; set => this.mergingDefects = value; }
        bool mergingDefects;

        public ECriterionLength CriterionLength { get => criterionLength; set => criterionLength = value; }
        ECriterionLength criterionLength;

        public int MinBlackDefectLength { get => minBlackDefectLength; set => minBlackDefectLength = value; }
        int minBlackDefectLength;

        public int MinWhiteDefectLength { get => minWhiteDefectLength; set => minWhiteDefectLength = value; }
        int minWhiteDefectLength;

        public DetectorParam(bool iscludeModelParam) : base()
        {
            if (iscludeModelParam)
                this.modelParam = new DetectorModelParam();

            this.maximumDefectCount = 100;
            this.useMultiThread = false;
            this.timeoutMs = -1;

            this.reconstruction = false;
            this.fineSizeMeasure = true;
            this.fineSizeMeasureThresholdMul = 0.5f;
            this.FineSizeMeasureSizeMul = 10;
            this.useSpreadTrace = true;
            this.ignoreLongDefect = true;
            this.mergingDefects = false;

            this.minWhiteDefectLength = 150;
            this.minBlackDefectLength = 150;
            this.criterionLength = ECriterionLength.Max;
        }

        #region override
        public override AlgorithmParam Clone()
        {
            DetectorParam detectorParam = new DetectorParam(true);

            detectorParam.modelParam.CopyFrom(this.ModelParam);
            detectorParam.reconstruction = this.reconstruction;
            detectorParam.fineSizeMeasure = this.fineSizeMeasure;
            detectorParam.fineSizeMeasureThresholdMul = this.fineSizeMeasureThresholdMul;
            detectorParam.FineSizeMeasureSizeMul = this.FineSizeMeasureSizeMul;
            detectorParam.useSpreadTrace = this.useSpreadTrace;
            detectorParam.ignoreLongDefect = this.ignoreLongDefect;
            detectorParam.mergingDefects = this.mergingDefects;
            detectorParam.maximumDefectCount = this.maximumDefectCount;

            return detectorParam;
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
        #endregion

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MaximumDefectCount", maximumDefectCount);
            XmlHelper.SetValue(algorithmElement, "UseMultiThread", this.useMultiThread);
            XmlHelper.SetValue(algorithmElement, "TimeoutMs", this.timeoutMs);

            XmlHelper.SetValue(algorithmElement, "Reconstruction", this.reconstruction);
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasure", this.fineSizeMeasure);
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasureThresholdMul", this.fineSizeMeasureThresholdMul);
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasureSizeMul", this.fineSizeMeasureSizeMul);

            //string str = SerializeHelper.Serialize(this.spreadTracerParam);
            //XmlHelper.SetValue(algorithmElement, "SpreadTracerParam", str);
            XmlHelper.SetValue(algorithmElement, "UseSpreadTrace", this.useSpreadTrace);

            XmlHelper.SetValue(algorithmElement, "IgnoreLongDefect", this.ignoreLongDefect);
            XmlHelper.SetValue(algorithmElement, "MergingDefects", this.mergingDefects);

            XmlHelper.SetValue(algorithmElement, "MinBlackDefectLength", this.minBlackDefectLength);
            XmlHelper.SetValue(algorithmElement, "MinWhiteDefectLength", this.minWhiteDefectLength);
            XmlHelper.SetValue(algorithmElement, "CriterionLength", this.criterionLength);

            //AlgorithmSetting.Instance().MinWhiteDefectLength = this.minWhiteDefectLength;
            //AlgorithmSetting.Instance().MinBlackDefectLength = this.minBlackDefectLength;
            //AlgorithmSetting.Instance().MaxDefectNum = this.maximumDefectCount;
            //AlgorithmSetting.Instance().CriterionLength = this.criterionLength;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            this.maximumDefectCount = XmlHelper.GetValue(algorithmElement, "MaximumDefectCount", maximumDefectCount);
            this.useMultiThread = XmlHelper.GetValue(algorithmElement, "UseMultiThread", this.useMultiThread);
            this.timeoutMs = XmlHelper.GetValue(algorithmElement, "TimeoutMs", this.timeoutMs);

            this.reconstruction = XmlHelper.GetValue(algorithmElement, "Reconstruction", this.reconstruction);
            this.fineSizeMeasure = XmlHelper.GetValue(algorithmElement, "FineSizeMeasure", this.fineSizeMeasure);
            this.fineSizeMeasureThresholdMul = XmlHelper.GetValue(algorithmElement, "FineSizeMeasureThresholdMul", this.fineSizeMeasureThresholdMul);
            this.fineSizeMeasureSizeMul = XmlHelper.GetValue(algorithmElement, "FineSizeMeasureSizeMul", this.fineSizeMeasureSizeMul);

            //string str = XmlHelper.GetValue(algorithmElement, "SpreadTracerParam", "");
            //this.spreadTracerParam = SerializeHelper.Deserialize<_SpreadTracerParam>(str);
            this.useSpreadTrace = XmlHelper.GetValue(algorithmElement, "UseSpreadTrace", this.useSpreadTrace);

            this.ignoreLongDefect = XmlHelper.GetValue(algorithmElement, "IgnoreLongDefect", this.ignoreLongDefect);
            this.mergingDefects = XmlHelper.GetValue(algorithmElement, "MergingDefects", this.mergingDefects);

            this.minBlackDefectLength = XmlHelper.GetValue(algorithmElement, "MinBlackDefectLength", this.minBlackDefectLength);
            this.minWhiteDefectLength = XmlHelper.GetValue(algorithmElement, "MinWhiteDefectLength", this.minWhiteDefectLength);
            this.criterionLength = XmlHelper.GetValue(algorithmElement, "CriterionLength", this.criterionLength);

            this.reconstruction = false;
            this.useMultiThread = false;
#if DEBUG
            //this.reconstruction = true;
#endif
        }
    }
}
