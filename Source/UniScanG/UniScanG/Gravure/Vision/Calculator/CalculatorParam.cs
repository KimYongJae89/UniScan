using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Calculator
{
    public class CalculatorModelParam : AlgorithmModelParam
    {
        public Point BasePosition { get => basePosition; set => basePosition = value; }
        Point basePosition = Point.Empty;

        public SizeF SheetSizeMm { get => sheetSizeMm; set => sheetSizeMm = value; }
        SizeF sheetSizeMm = SizeF.Empty;

        public EdgeParam EdgeParam { get => this.edgeParam; set => this.edgeParam = value; }
        EdgeParam edgeParam;

        public SensitiveParam SensitiveParam { get => this.sensitiveParam; set => this.sensitiveParam = value; }
        SensitiveParam sensitiveParam;

        public SizeF BarBoundary { get => barBoundary; set => barBoundary = value; }
        SizeF barBoundary;

        public bool UseMultiData { get => useMultiData; set => useMultiData = value; }
        bool useMultiData;

        public CalculatorParam.EIgnoreMethod IgnoreMethod { get => ignoreMethod; set => ignoreMethod = value; }
        public bool IgnoreSideLine { get => ignoreSideLine; set => ignoreSideLine = value; }

        CalculatorParam.EIgnoreMethod ignoreMethod;
        bool ignoreSideLine;


        public SheetPatternGroupCollection PatternGroupCollection { get => patternGroupCollection; set => patternGroupCollection = value; }
        SheetPatternGroupCollection patternGroupCollection = null;

        public RegionInfoGCollection RegionInfoCollection { get => regionInfoCollection; set => regionInfoCollection = value; }
        RegionInfoGCollection regionInfoCollection = null;

        public CalculatorModelParam() : base()
        {
            this.patternGroupCollection = new SheetPatternGroupCollection();
            this.regionInfoCollection = new RegionInfoGCollection();
        }

        public CalculatorModelParam(AlgorithmModelParam src) : base(src) { }

        public override void Clear()
        {
            this.basePosition = Point.Empty;
            this.sheetSizeMm = Size.Empty;
            this.edgeParam = new EdgeParam(false, 150, 5, EdgeParam.EEdgeFindMethod.Soble, false);
            this.sensitiveParam = new SensitiveParam(false, 25, 30);
            this.barBoundary = new SizeF(2, 1);
            this.useMultiData = false;
            this.ignoreMethod = CalculatorParam.EIgnoreMethod.Basic;
            this.ignoreSideLine = false;

            this.patternGroupCollection?.Clear();
            this.regionInfoCollection?.Clear();
        }

        public override AlgorithmModelParam Clone()
        {
            return new CalculatorModelParam(this);
        }

        public override void CopyFrom(AlgorithmModelParam algorithmModelParam)
        {
            CalculatorModelParam calculatorModelParam = (CalculatorModelParam)algorithmModelParam;

            this.basePosition = calculatorModelParam.basePosition;
            this.sheetSizeMm = calculatorModelParam.sheetSizeMm;
            this.edgeParam.CopyFrom(calculatorModelParam.edgeParam);
            this.sensitiveParam.CopyFrom(calculatorModelParam.sensitiveParam);
            this.barBoundary = calculatorModelParam.barBoundary;
            this.useMultiData = calculatorModelParam.useMultiData;

            this.patternGroupCollection = new SheetPatternGroupCollection(calculatorModelParam.patternGroupCollection);
            this.regionInfoCollection = new RegionInfoGCollection(calculatorModelParam.regionInfoCollection);
        }

        public override void Load(XmlElement xmlElement)
        {
            this.basePosition = XmlHelper.GetValue(xmlElement, "BasePosition", this.basePosition);
            this.sheetSizeMm = XmlHelper.GetValue(xmlElement, "SheetSizeMm", this.sheetSizeMm);
            this.edgeParam.LoadParam(xmlElement, "EdgeParam");
            this.sensitiveParam.LoadParam(xmlElement, "SensitiveParam");
            this.barBoundary = XmlHelper.GetValue(xmlElement, "BarBoundary", this.barBoundary);
            this.useMultiData = XmlHelper.GetValue(xmlElement, "UseSIMD", this.useMultiData);
            this.ignoreMethod = XmlHelper.GetValue(xmlElement, "IgnoreMethod", this.ignoreMethod);
            this.ignoreSideLine = XmlHelper.GetValue(xmlElement, "IgnoreSideLine", this.ignoreSideLine);

            this.patternGroupCollection.Load(xmlElement);
            this.regionInfoCollection.Load(xmlElement);

            LogHelper.Debug(LoggerType.Operation, $"CalculatorModelParam::Load - sheetSizeMm: {sheetSizeMm}");

            Size sheetSizePx = XmlHelper.GetValue(xmlElement, "SheetSizePx", Size.Empty);
            LogHelper.Debug(LoggerType.Operation, $"CalculatorModelParam::Load - sheetSizePx: {sheetSizePx}");

            if (this.sheetSizeMm.IsEmpty && !sheetSizePx.IsEmpty)
                this.sheetSizeMm = new SizeF(sheetSizePx.Width * 14 / 1000f, sheetSizePx.Height * 14 / 1000f);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "BasePosition", this.basePosition);
            XmlHelper.SetValue(xmlElement, "SheetSizeMm", this.sheetSizeMm);
            this.edgeParam.SaveParam(xmlElement, "EdgeParam");
            this.sensitiveParam.SaveParam(xmlElement, "SensitiveParam");
            XmlHelper.SetValue(xmlElement, "BarBoundary", this.barBoundary);
            XmlHelper.SetValue(xmlElement, "UseSIMD", this.useMultiData);
            XmlHelper.SetValue(xmlElement, "IgnoreMethod", this.ignoreMethod);
            XmlHelper.SetValue(xmlElement, "IgnoreSideLine", this.ignoreSideLine);

            this.patternGroupCollection.Save(xmlElement);
            this.regionInfoCollection.Save(xmlElement);
        }
    }

    public class SensitiveParam
    {
        public bool Multi { get => this.multi; set => this.multi = value; }
        public byte Min { get => this.min; set => this.min = value; }
        public byte Max { get => this.max; set => this.max = value; }

        bool multi;
        byte min;
        byte max;

        public SensitiveParam() { }

        public SensitiveParam(bool multi, byte min, byte max)
        {
            this.multi = multi;
            this.min = min;
            this.max = max;
        }

        public void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);

                SaveParam(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Multi", this.multi);
            XmlHelper.SetValue(xmlElement, "Min", this.min);
            XmlHelper.SetValue(xmlElement, "Max", this.max);
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                LoadParam(subElement, "");
                return;
            }

            this.multi = XmlHelper.GetValue(xmlElement, "Multi", this.multi);
            this.min = XmlHelper.GetValue(xmlElement, "Min", this.min);
            this.max = XmlHelper.GetValue(xmlElement, "Max", this.max);
        }

        public void CopyFrom(SensitiveParam sensitiveParam)
        {
            this.multi = sensitiveParam.multi;
            this.min = sensitiveParam.min;
            this.max = sensitiveParam.max;
        }

        public SensitiveParam Clone()
        {
            SensitiveParam clone = new SensitiveParam();
            clone.CopyFrom(this);
            return clone;
        }
    }

    public class EdgeParam
    {
        public enum EEdgeFindMethod { Projection, Soble, /*Include*/ }
        public bool Multi { get => this.multi; }
        bool multi;

        public int Value { get => this.value; }
        int value;

        public int Width { get => this.width; }
        int width;

        public bool Erode { get => this.erode; }
        bool erode;

        public EEdgeFindMethod EdgeFindMethod { get => findMethod; }
        EEdgeFindMethod findMethod;

        public EdgeParam() { }
        public EdgeParam(bool multi, int value, int width, EEdgeFindMethod findMethod, bool erode)
        {
            this.multi = multi;
            this.value = value;
            this.width = width;
            this.findMethod = findMethod;
            this.erode = erode;
        }

        public void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);

                SaveParam(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Multi", this.multi);
            XmlHelper.SetValue(xmlElement, "Value", this.value);
            XmlHelper.SetValue(xmlElement, "Width", this.width);
            XmlHelper.SetValue(xmlElement, "Erode", this.erode);
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                LoadParam(subElement, "");
                return;
            }

            this.multi = XmlHelper.GetValue(xmlElement, "Multi", this.multi);
            this.value = XmlHelper.GetValue(xmlElement, "Value", this.value);
            this.width = XmlHelper.GetValue(xmlElement, "Width", this.width);
            this.erode = XmlHelper.GetValue(xmlElement, "Erode", this.erode);
        }

        public void CopyFrom(EdgeParam edgeParam)
        {
            this.multi = edgeParam.multi;
            this.value = edgeParam.value;
            this.width = edgeParam.width;
            this.findMethod = edgeParam.findMethod;
            this.erode = edgeParam.erode;
        }

        public EdgeParam Clone()
        {
            EdgeParam clone = new EdgeParam();
            clone.CopyFrom(this);
            return clone;
        }
    }

    public class LineSetParam
    {
        public bool AdaptivePairing { get => this.adaptivePairing; }
        bool adaptivePairing;

        public int BoundaryPairStep { get => this.boundaryPairStep; }
        int boundaryPairStep;

        public CalculatorParam.EIgnoreMethod IgnoreMethod { get => this.ignoreMethod; }
        CalculatorParam.EIgnoreMethod ignoreMethod;

        public bool IgnoreSideLine { get => this.ignoreSideLine; }
        bool ignoreSideLine;

        public LineSetParam(bool adaptivePairing, int boundaryPairStep, CalculatorParam.EIgnoreMethod ignoreMethod, bool ignoreSideLine)
        {
            this.adaptivePairing = adaptivePairing;
            this.boundaryPairStep = boundaryPairStep;
            this.ignoreMethod = ignoreMethod;
            this.ignoreSideLine = ignoreSideLine;
        }

        public void CopyFrom(LineSetParam lineSetParam)
        {
            this.adaptivePairing = lineSetParam.adaptivePairing;
            this.boundaryPairStep = lineSetParam.boundaryPairStep;
            this.ignoreMethod = lineSetParam.ignoreMethod;
            this.ignoreSideLine = lineSetParam.ignoreSideLine;
        }
    }

    public class CalculatorParam : UniScanG.Vision.AlgorithmParamG
    {
        public enum EIgnoreMethod { Basic, Neighborhood }

        public CalculatorModelParam ModelParam => this.modelParam == null ? SystemManager.Instance().CurrentModel.CalculatorModelParam : this.modelParam;
        CalculatorModelParam modelParam;

        //public Point BasePosition { get => basePosition; }
        //public Size SheetSize { get => sheetSize; }
        //public List<SheetPatternGroup> PatternGroupList { get => patternGroupList; }
        //public List<RegionInfoG> RegionInfoList { get => regionInfoList; }

        //Point basePosition = Point.Empty;
        //Size sheetSize = Size.Empty;
        //List<SheetPatternGroup> patternGroupList = null;
        //List<RegionInfoG> regionInfoList = null;

        //public EdgeParam EdgeParam => new EdgeParam(this.edgeMultiple, this.edgeValue, this.edgeWidth, this.edgeFindMethod, this.edgeErode);

        //public EdgeParam.EEdgeFindMethod EdgeFindMethod { get => edgeFindMethod; set => edgeFindMethod = value; }
        //public int EdgeValue { get => edgeValue; set => edgeValue = value; }
        //public int EdgeWidth { get => edgeWidth; set => edgeWidth = value; }
        //public bool EdgeMultiple { get => edgeMultiple; set => edgeMultiple = value; }
        //public bool EdgeErode { get => edgeErode; set => edgeErode = value; }

        //EdgeParam.EEdgeFindMethod edgeFindMethod;
        //int edgeValue;
        //int edgeWidth;
        //bool edgeMultiple;
        //bool edgeErode;

        public bool InBarAlign { get => inBarAlign; set => inBarAlign = value; }
        public float InBarAlignScore { get => inBarAlignScore; set => inBarAlignScore = value; }

        bool inBarAlign;
        float inBarAlignScore;

        //public SensitiveParam SensitiveParam => new SensitiveParam(this.multiSensitivity, this.sensitivityMin, this.sensitivityMax);

        //public bool MultiSensitivity { get => multiSensitivity; set => multiSensitivity = value; }
        //public byte SensitivityMin { get => sensitivityMin; set => sensitivityMin = value; }
        //public byte SensitivityMax { get => sensitivityMax; set => sensitivityMax = value; }

        //bool multiSensitivity;
        //byte sensitivityMin;
        //byte sensitivityMax;


        public int BoundaryPairStep { get => boundaryPairStep; set => boundaryPairStep = value; }
        int boundaryPairStep;

        public bool AdaptivePairing { get => adaptivePairing; set => adaptivePairing = value; }
        bool adaptivePairing;

        public bool UseMultiThread { get => useMultiThread; set => useMultiThread = value; }
        bool useMultiThread;


        public bool UseSticker { get => this.useSticker; set => this.useSticker = value; }
        public bool StickerBrightOnly { get => this.stickerBrightOnly; set => this.stickerBrightOnly = value; }
        public int StickerDiffHigh { get => this.stickerDiffHigh; set => this.stickerDiffHigh = value; }
        public int StickerDiffLow { get => this.stickerDiffLow; set => this.stickerDiffLow = value; }

        bool useSticker;
        bool stickerBrightOnly;
        int stickerDiffHigh;
        int stickerDiffLow;

        public CalculatorParam(bool iscludeModelParam) : base()
        {
            if (iscludeModelParam)
                this.modelParam = new CalculatorModelParam();

            this.inBarAlign = true;
            this.inBarAlignScore = 55.0f;

            //this.multiSensitivity = false;
            //this.sensitivityMin = 25;
            //this.sensitivityMax = 30;

            this.adaptivePairing = true;
            this.boundaryPairStep = 2;

            this.useMultiThread = true;

            this.useSticker = true;
            this.stickerBrightOnly = true;
            this.stickerDiffHigh = 50;
            this.stickerDiffLow = 30;
        }

        #region override
        public override AlgorithmParam Clone()
        {
            CalculatorParam calculatorParam = new CalculatorParam(true);

            calculatorParam.modelParam?.CopyFrom(this.modelParam);

            calculatorParam.inBarAlign = this.inBarAlign;
            calculatorParam.inBarAlignScore = this.inBarAlignScore;

            //calculatorParam.multiSensitivity = this.multiSensitivity;
            //calculatorParam.sensitivityMin = this.sensitivityMin;
            //calculatorParam.sensitivityMax = this.sensitivityMax;

            calculatorParam.adaptivePairing = this.adaptivePairing;
            calculatorParam.boundaryPairStep = this.boundaryPairStep;

            calculatorParam.useMultiThread = this.useMultiThread;

            return calculatorParam;
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
        #endregion

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //XmlHelper.SetValue(algorithmElement, "EdgeFindMethod", this.edgeFindMethod);
            //XmlHelper.SetValue(algorithmElement, "EdgeWidth", this.edgeWidth);
            //XmlHelper.SetValue(algorithmElement, "EdgeValue", this.edgeValue);
            //XmlHelper.SetValue(algorithmElement, "EdgeMultiple", this.edgeMultiple);
            //XmlHelper.SetValue(algorithmElement, "EdgeErode", this.edgeErode);

            XmlHelper.SetValue(algorithmElement, "InBarAlign", this.inBarAlign);
            XmlHelper.SetValue(algorithmElement, "InBarAlignScore", this.inBarAlignScore);

            //XmlHelper.SetValue(algorithmElement, "Sensitivity", this.multiSensitivity);
            //XmlHelper.SetValue(algorithmElement, "SensitivityMin", Math.Min(this.sensitivityMin, this.sensitivityMax));
            //XmlHelper.SetValue(algorithmElement, "SensitivityMax", Math.Max(this.sensitivityMin, this.sensitivityMax));

            //XmlHelper.SetValue(algorithmElement, "BarBoundary", this.barBoundary);

            XmlHelper.SetValue(algorithmElement, "AdaptivePairing", this.adaptivePairing);
            XmlHelper.SetValue(algorithmElement, "BoundaryPairStep", this.boundaryPairStep);

            XmlHelper.SetValue(algorithmElement, "UseMultiThread", this.useMultiThread);

            XmlHelper.SetValue(algorithmElement, "UseSticker", this.useSticker);
            XmlHelper.SetValue(algorithmElement, "StickerDiffHigh", this.stickerDiffHigh);
            XmlHelper.SetValue(algorithmElement, "StickerDiffLow", this.stickerDiffLow);

            //AlgorithmSetting.Instance().UseSticker = this.useSticker;
            //AlgorithmSetting.Instance().StickerDiffHigh = this.stickerDiffHigh;
            //AlgorithmSetting.Instance().StickerDiffLow = this.stickerDiffLow;

            //AlgorithmSetting.Instance().UseMultiThread = this.useMultiThread;
            //AlgorithmSetting.Instance().UseMultiData = this.useMultiData;
        }


        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            this.modelParam?.Load(algorithmElement);
            //this.basePosition = XmlHelper.GetValue(algorithmElement, "BasePosition", this.basePosition);
            //this.sheetSize = XmlHelper.GetValue(algorithmElement, "SheetSize", this.sheetSize);

            //XmlNodeList sheetPatternGroupNodeList = algorithmElement.GetElementsByTagName("SheetPatternGroup");
            //if (sheetPatternGroupNodeList.Count > 0)
            //{
            //    this.patternGroupList = new List<SheetPatternGroup>();
            //    foreach (XmlElement subElement in sheetPatternGroupNodeList)
            //    {
            //        SheetPatternGroup sheetPatternGroup = new SheetPatternGroup();
            //        sheetPatternGroup.LoadParam(subElement);
            //        patternGroupList.Add(sheetPatternGroup);
            //    }
            //}

            //XmlNodeList regionInfoNodeList = algorithmElement.GetElementsByTagName("RegionInfo");
            //if (regionInfoNodeList.Count > 0)
            //{
            //    this.regionInfoList = new List<RegionInfoG>();
            //    foreach (XmlElement subElement in regionInfoNodeList)
            //    {
            //        RegionInfoG regionInfoG = RegionInfoG.Load(subElement);
            //        regionInfoList.Add(regionInfoG);
            //    }
            //}

            //this.edgeFindMethod = EdgeParam.EEdgeFindMethod.Soble;
            //this.edgeWidth = XmlHelper.GetValue(algorithmElement, "EdgeWidth", this.edgeWidth);
            //this.edgeValue = XmlHelper.GetValue(algorithmElement, "EdgeValue", this.edgeValue);
            //this.edgeMultiple = XmlHelper.GetValue(algorithmElement, "EdgeMultiple", this.edgeMultiple);
            //this.edgeErode = XmlHelper.GetValue(algorithmElement, "EdgeErode", this.edgeErode);

            this.inBarAlign = XmlHelper.GetValue(algorithmElement, "InBarAlign", this.inBarAlign);
            this.inBarAlignScore = XmlHelper.GetValue(algorithmElement, "InBarAlignScore", this.inBarAlignScore);

            //this.multiSensitivity = XmlHelper.GetValue(algorithmElement, "Sensitivity", this.multiSensitivity);
            //this.sensitivityMin = XmlHelper.GetValue(algorithmElement, "SensitivityMin", this.sensitivityMin);
            //this.sensitivityMax = XmlHelper.GetValue(algorithmElement, "SensitivityMax", this.sensitivityMax);

            //this.barBoundary = XmlHelper.GetValue(algorithmElement, "BarBoundary", this.barBoundary);
            this.adaptivePairing = XmlHelper.GetValue(algorithmElement, "AdaptivePairing", this.adaptivePairing);
            this.boundaryPairStep = XmlHelper.GetValue(algorithmElement, "BoundaryPairStep", this.boundaryPairStep);

            this.useMultiThread = XmlHelper.GetValue(algorithmElement, "UseMultiThread", this.useMultiThread);

            this.useSticker = XmlHelper.GetValue(algorithmElement, "UseSticker", this.useSticker);
            this.stickerDiffLow = XmlHelper.GetValue(algorithmElement, "StickerDiffLow", this.stickerDiffLow);
            this.stickerDiffHigh = XmlHelper.GetValue(algorithmElement, "StickerDiffHigh", this.stickerDiffHigh);
        }
    }
}