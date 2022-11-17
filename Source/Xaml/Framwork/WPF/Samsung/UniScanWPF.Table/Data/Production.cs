using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Operation.Operators;

namespace UniScanWPF.Table.Data
{
    public class Production : DynMvp.Data.ProductionBase
    {
        public int Count { get => count; }
        int count = 0;

        public int PatternCount { get => this.patternCount; }
        int patternCount = 0;

        public int MarginCount { get => this.marginCount; }
        int marginCount = 0;

        public int ShapeCount { get => this.shapeCount; }
        int shapeCount = 0;

        public override string ResultPath => GetResultPath();

        public Production(XmlElement productionElement) : base(productionElement)
        {
            this.count = XmlHelper.GetValue(productionElement, "Count", this.count);
            this.patternCount = XmlHelper.GetValue(productionElement, "PatternCount", this.patternCount);
            this.marginCount = XmlHelper.GetValue(productionElement, "MarginCount", this.marginCount);
            this.shapeCount = XmlHelper.GetValue(productionElement, "ShapeCount", this.shapeCount);
        }

        public Production(string name, DateTime dateTime, string lotNo) : base(name, dateTime, lotNo)
        {

        }

        public override string GetModelPath()
        {
            throw new NotImplementedException();
        }

        public override string GetResultPath()
        {
            //string resultPath = Path.Combine(
            //         PathSettings.Instance().Result,
            //         this.LotNo);

            string resultPath = Path.Combine(
                    PathSettings.Instance().Result,
                    this.StartTime.ToString("yyyy-MM-dd"), 
                    this.Name,                    
                    this.LotNo);
            //Path.Combine(PathSettings.Instance().Result,
            //production.StartTime.ToString("yyyy-MM-dd"),
            //production.LotNo);

            return resultPath;
        }

        public void AddCount(List<CanvasDefect> defectList)
        {
            count++;

            this.patternCount += defectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Pattern));
            this.marginCount += defectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Margin));
            this.shapeCount += defectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Shape));
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "Count", count);
            XmlHelper.SetValue(productionElement, "PatternCount", patternCount);
            XmlHelper.SetValue(productionElement, "MarginCount", marginCount);
            XmlHelper.SetValue(productionElement, "ShapeCount", shapeCount);
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            this.count = XmlHelper.GetValue(productionElement, "Count", this.count);
            this.patternCount = XmlHelper.GetValue(productionElement, "PatternCount", this.patternCount);
            this.marginCount = XmlHelper.GetValue(productionElement, "MarginCount", this.marginCount);
            this.shapeCount = XmlHelper.GetValue(productionElement, "ShapeCount", this.shapeCount);
        }

        public override void Update(InspectionResult sheetResult)
        {
            throw new NotImplementedException();
        }

        internal void UpdateCount(List<LoadItem> curResultList)
        {
            this.patternCount = curResultList.Sum(f => f.CanvasDefectList.Count(g => g.Defect.ResultObjectType.Equals(DefectType.Pattern)));
            this.marginCount = curResultList.Sum(f => f.CanvasDefectList.Count(g => g.Defect.ResultObjectType.Equals(DefectType.Margin)));
            this.shapeCount = curResultList.Sum(f => f.CanvasDefectList.Count(g => g.Defect.ResultObjectType.Equals(DefectType.Shape)));
        }
    }
}
