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

namespace UniScanG.Gravure.Vision.Trainer
{

    public class TrainerModelParam : AlgorithmModelParam
    {
        public bool IsCrisscross { get => this.isCrisscross; set => this.isCrisscross = value; }
        private bool isCrisscross;

        public int BinValue { get => this.binValue; set => this.binValue = value; }
        private int binValue;

        public int BinValueOffset { get => this.binValueOffset; set => this.binValueOffset = value; }
        private int binValueOffset;

        public float SheetPatternGroupThreshold { get => this.sheetPatternGroupThreshold; set => this.sheetPatternGroupThreshold = value; }
        private float sheetPatternGroupThreshold;

        public bool AdvanceWaistMeasure { get => this.advanceWaistMeasure; set => this.advanceWaistMeasure = value; }
        private bool advanceWaistMeasure;

        public bool WholeRegionProjection { get => this.wholeRegionProjection; set => this.wholeRegionProjection = value; }
        bool wholeRegionProjection;

        public bool FixMissingTooth { get => this.fixMissingTooth; set => this.fixMissingTooth = value; }
        bool fixMissingTooth;

        public bool IgnoreInnerChip { get => this.ignoreInnerChip; set => this.ignoreInnerChip = value; }
        private bool ignoreInnerChip;

        public TrainerModelParam() : base() { }

        public TrainerModelParam(AlgorithmModelParam src) : base(src) { }

        public override void Clear()
        {
            this.binValue = 20;
            this.binValueOffset = 0;
            this.isCrisscross = true;
            this.sheetPatternGroupThreshold = 20f;
            this.advanceWaistMeasure = false;
            this.wholeRegionProjection = true;
            this.fixMissingTooth = false;
        }

        public override AlgorithmModelParam Clone()
        {
            return new TrainerModelParam(this);
        }

        public override void CopyFrom(AlgorithmModelParam algorithmModelParam)
        {
            TrainerModelParam trainerModelParam = (TrainerModelParam)algorithmModelParam;

            this.binValue = trainerModelParam.binValue;
            this.binValueOffset = trainerModelParam.binValueOffset;
            this.isCrisscross = trainerModelParam.isCrisscross;
            this.sheetPatternGroupThreshold = trainerModelParam.sheetPatternGroupThreshold;
            this.advanceWaistMeasure = trainerModelParam.advanceWaistMeasure;
            this.wholeRegionProjection = trainerModelParam.wholeRegionProjection;
            this.fixMissingTooth = trainerModelParam.fixMissingTooth;
        }

        public override void Save(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "BinValue", this.binValue);
            XmlHelper.SetValue(algorithmElement, "BinValueOffset", this.binValueOffset);
            XmlHelper.SetValue(algorithmElement, "IsCrisscross", this.isCrisscross);
            XmlHelper.SetValue(algorithmElement, "SheetPatternGroupThreshold", this.sheetPatternGroupThreshold);
            XmlHelper.SetValue(algorithmElement, "AdvanceWaistMeasure", this.advanceWaistMeasure);
            XmlHelper.SetValue(algorithmElement, "WholeRegionProjection", this.wholeRegionProjection);
            XmlHelper.SetValue(algorithmElement, "FixMissingTooth", this.fixMissingTooth);
            XmlHelper.SetValue(algorithmElement, "IgnoreInnerChip", this.ignoreInnerChip);
        }

        public override void Load(XmlElement xmlElement)
        {
            this.binValue = XmlHelper.GetValue(xmlElement, "BinValue", this.binValue);
            this.binValueOffset = XmlHelper.GetValue(xmlElement, "BinValueOffset", this.binValueOffset);
            this.isCrisscross = XmlHelper.GetValue(xmlElement, "IsCrisscross", this.isCrisscross);
            this.sheetPatternGroupThreshold = XmlHelper.GetValue(xmlElement, "SheetPatternGroupThreshold", this.sheetPatternGroupThreshold);
            this.advanceWaistMeasure = XmlHelper.GetValue(xmlElement, "AdvanceWaistMeasure", this.advanceWaistMeasure);
            this.wholeRegionProjection = XmlHelper.GetValue(xmlElement, "WholeRegionProjection", this.wholeRegionProjection);
            this.fixMissingTooth = XmlHelper.GetValue(xmlElement, "FixMissingTooth", this.fixMissingTooth);
            this.ignoreInnerChip = XmlHelper.GetValue(xmlElement, "IgnoreInnerChip", this.ignoreInnerChip);
        }
    }

    public class TrainerParam : UniScanG.Vision.AlgorithmParamG
    {
        public TrainerModelParam ModelParam => this.modelParam == null ? SystemManager.Instance().CurrentModel.TrainerModelParam : this.modelParam;
        TrainerModelParam modelParam;

        // Find pattern
        public int MinPatternArea { get => this.minPatternArea; set => this.minPatternArea = value; }
        private int minPatternArea;

        public GroupDirection GroupDirection { get => this.groupDirection; set => this.groupDirection = value; }
        private GroupDirection groupDirection;

        // Align
        public float AlignLocalSearch { get => this.alignLocalSearch; set => this.alignLocalSearch = value; }
        private float alignLocalSearch;

        public float AlignLocalMaster { get => this.alignLocalMaster; set => this.alignLocalMaster = value; }
        private float alignLocalMaster;


        public int MinLineIntensity { get => this.minLineIntensity; set => this.minLineIntensity = value; }
        private int minLineIntensity;

        public int KernalSize { get => this.kernalSize; set => this.kernalSize = value; }
        private int kernalSize;

        public int DiffrentialThreshold { get => this.diffrentialThreshold; set => this.diffrentialThreshold = value; }
        private int diffrentialThreshold;

        public bool SplitLargeBar { get => this.splitLargeBar; set => this.splitLargeBar = value; }
        private bool splitLargeBar;

        public TrainerParam(bool includeModelParam) : base()
        {
            if(includeModelParam)
                this.modelParam = new TrainerModelParam();
      
            this.minPatternArea = 20;
            this.groupDirection = GroupDirection.Vertical;

            this.alignLocalSearch = 3.0f;
            this.alignLocalMaster = 2.0f;

            this.minLineIntensity = 10;
            this.kernalSize = 50;
            this.diffrentialThreshold = 5;
            this.splitLargeBar = false;
        }

        public override AlgorithmParam Clone()
        {
            TrainerParam clone = new TrainerParam(true);
            clone.CopyFrom(this);

            return clone;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            TrainerParam srcParam = (TrainerParam)srcAlgorithmParam;

            this.modelParam?.CopyFrom(srcParam.ModelParam);
            this.minPatternArea = srcParam.minPatternArea;
            this.groupDirection = srcParam.groupDirection;
            this.alignLocalSearch = srcParam.alignLocalSearch;
            this.alignLocalMaster = srcParam.alignLocalMaster;

            this.minLineIntensity = srcParam.minLineIntensity;
            this.kernalSize = srcParam.kernalSize;
            this.diffrentialThreshold = srcParam.diffrentialThreshold;
            this.splitLargeBar = srcParam.splitLargeBar;
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //this.trainerModelParam?.Save(algorithmElement);

            //XmlHelper.SetValue(algorithmElement, "BinValue", binValue.ToString());
            //XmlHelper.SetValue(algorithmElement, "BinValueOffset", binValueOffset.ToString());
            XmlHelper.SetValue(algorithmElement, "MinPatternArea", minPatternArea.ToString());
            //XmlHelper.SetValue(algorithmElement, "SheetPatternGroupThreshold", sheetPatternGroupThreshold.ToString());
            XmlHelper.SetValue(algorithmElement, "GroupDirection", this.groupDirection);

            XmlHelper.SetValue(algorithmElement, "AlignLocalSearch", this.alignLocalSearch);
            XmlHelper.SetValue(algorithmElement, "AlignLocalMaster", this.alignLocalMaster);

            //XmlHelper.SetValue(algorithmElement, "IsCrisscross", isCrisscross);
            XmlHelper.SetValue(algorithmElement, "MinLineIntensity", minLineIntensity);
            XmlHelper.SetValue(algorithmElement, "KernalSize", this.kernalSize);
            XmlHelper.SetValue(algorithmElement, "DiffrentialThreshold", this.diffrentialThreshold);
            XmlHelper.SetValue(algorithmElement, "SplitLargeBar", this.splitLargeBar);
        }
        

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            if (this.modelParam != null)
                this.modelParam.Load(algorithmElement);

            //this.binValue = XmlHelper.GetValue(algorithmElement, "BinValue", this.binValue);
            //this.binValueOffset = XmlHelper.GetValue(algorithmElement, "BinValueOffset", this.binValueOffset);
            this.minPatternArea = XmlHelper.GetValue(algorithmElement, "MinPatternArea", this.minPatternArea);
            //this.sheetPatternGroupThreshold = XmlHelper.GetValue(algorithmElement, "SheetPatternGroupThreshold", this.sheetPatternGroupThreshold);
            this.groupDirection = XmlHelper.GetValue(algorithmElement, "GroupDirection", GroupDirection.Vertical);

            this.alignLocalSearch = XmlHelper.GetValue(algorithmElement, "AlignLocalSearch", this.alignLocalSearch);
            this.alignLocalMaster = XmlHelper.GetValue(algorithmElement, "AlignLocalMaster", this.alignLocalMaster);
            if (!MathHelper.IsInRange(this.alignLocalSearch, 1f, 3.5f)) this.alignLocalSearch = 3.0f;
            if (!MathHelper.IsInRange(this.alignLocalMaster, 1f, 3f)) this.alignLocalMaster = 2.0f;

            //this.isCrisscross = XmlHelper.GetValue(algorithmElement, "IsCrisscross", this.isCrisscross);
            this.minLineIntensity = XmlHelper.GetValue(algorithmElement, "MinLineIntensity", this.minLineIntensity);
            this.kernalSize = XmlHelper.GetValue(algorithmElement, "KernalSize", kernalSize);
            this.diffrentialThreshold = XmlHelper.GetValue(algorithmElement, "DiffrentialThreshold", this.diffrentialThreshold);
            this.splitLargeBar = XmlHelper.GetValue(algorithmElement, "SplitLargeBar", this.splitLargeBar);
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }

    public class TrainerArgument
    {
        public UniScanG.Data.Model.Model Model { get; set; }

        public bool IsAutoTeach { get; set; } = true;

        public bool DoPattern { get; set; } = false;
        public bool DoRegion { get; set; } = false;
        public bool DoMonitoring { get; set; } = false;

        public bool PatternGood { get; set; } = true;
        public bool RegionGood { get; set; } = true;
        public bool SizeVariationGood { get; set; } = true;

        public bool IsTeachDone { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public Exception Exception { get; set; } = null;

        public bool DoNotSave { get; set; }

        public TrainerArgument(bool isAutoTeach, bool pattern, bool region, bool mon)
        {
            IsAutoTeach = isAutoTeach;
            DoPattern = pattern;
            DoRegion = region;
            DoMonitoring = mon;
        }
    }
}
