using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Inspect
{
    public class InspectionResult : DynMvp.InspData.InspectionResult
    {
        public float RollPos { get => this.rollPos; set => this.rollPos = value; }
        float rollPos = 0;

        public int ZoneIndex { get => this.zoneIndex; set => this.zoneIndex = value; }
        int zoneIndex = -1;

        public Size InspImageSize { get => this.inspImageSize; set => this.inspImageSize = value; }
        Size inspImageSize = Size.Empty;

        public SizeF InspPelSize { get => this.inspPelSize; set => this.inspPelSize = value; }
        SizeF inspPelSize = SizeF.Empty;

        public TeachData TeachData { get => this.teachData; set => this.teachData = value; }
        TeachData teachData;

        public SizeF MarginTarget { get => this.marginTarget; set => this.marginTarget = value; }
        SizeF marginTarget = SizeF.Empty;

        public SizeF MarginSize { get => this.marginSize; set => this.marginSize = value; }
        SizeF marginSize = SizeF.Empty;

        public SizeF MarginDiff => SizeF.Subtract(this.marginTarget, this.marginSize);

        public bool MarginResult { get => this.marginResult; set => this.marginResult = value; }
        bool marginResult = false;

        public Rectangle MarginRect { get => this.marginRect; set => this.marginRect = value; }
        Rectangle marginRect = Rectangle.Empty;


        public SizeF BlotTarget { get => this.blotTarget; set => this.blotTarget = value; }
        SizeF blotTarget = SizeF.Empty;

        public SizeF BlotSize { get => this.blotSize; set => this.blotSize = value; }
        SizeF blotSize = SizeF.Empty;

        public SizeF BlotDiff => SizeF.Subtract(this.blotTarget, this.blotSize);

        public bool BlotResult { get => this.blotResult; set => this.blotResult = value; }
        bool blotResult = false;

        public Rectangle BlotRect { get => this.blotRect; set => this.blotRect = value; }
        Rectangle blotRect = Rectangle.Empty;

        public Defect[] Defects { get => this.defects; set => this.defects = value; }
        Defect[] defects = null;

        public SizeF MaxDefectSize { get => this.defects.Length==0?SizeF.Empty: this.defects.First().SizeUm;}

        public Rectangle MaxDefectRect { get => this.defects.Length == 0 ? Rectangle.Empty : this.defects.FirstOrDefault().Rectangle; }

        public bool DefectResult { get => defectResult; set => this.defectResult = value; }
        bool defectResult;

        public int DefectCount => defects.Length;


        public override void AppendResultFigures(FigureGroup figureGroup, FigureDrawOption option = null)
        {
            base.AppendResultFigures(figureGroup, option);
        }

        public override void UpdateJudgement()
        {
            bool good = this.marginResult && this.blotResult && this.defectResult;
            this.judgment = good ? DynMvp.InspData.Judgment.Accept : DynMvp.InspData.Judgment.Reject;
        }
    }
}
