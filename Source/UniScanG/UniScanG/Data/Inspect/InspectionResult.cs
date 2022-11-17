using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Data.Inspect
{
    public class InspectionResult : DynMvp.InspData.InspectionResult
    {
        public OffsetStructSet OffsetSet { get => offsetSet; set => offsetSet = value; }
        protected OffsetStructSet offsetSet;

        public DateTime ImageGrabbedTime { get => imageGrabbedTime; set => imageGrabbedTime = value; }
        DateTime imageGrabbedTime;

        public InspectionResult()
        {
            this.imageGrabbedTime = DateTime.Now;
        }

        public override void UpdateJudgement()
        {
            if (this.Judgment == DynMvp.InspData.Judgment.Skip)
                return;

            try
            {
                this.Judgment = DynMvp.InspData.Judgment.Accept;
                foreach (AlgorithmResult algorithmResult in this.algorithmResultDic.Values)
                {
                    AlgorithmResultG algorithmResultG = algorithmResult as AlgorithmResultG;
                    if (algorithmResultG != null)
                    {
                        if (algorithmResultG.SheetSubResultList.Exists(f => f.IsDefect && f.ShowReport))
                        {
                            this.Judgment = DynMvp.InspData.Judgment.Reject;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                this.judgment = DynMvp.InspData.Judgment.Reject;
            }
        }

        public List<FoundedObjInPattern> GetSubResultList()
        {
            List<FoundedObjInPattern> sheetSubResultList = new List<FoundedObjInPattern>();
            foreach (AlgorithmResult algorithmResult in this.AlgorithmResultLDic.Values)
            {
                AlgorithmResultG algorithmResultG = algorithmResult as AlgorithmResultG;
                if (algorithmResultG == null)
                    continue;

                sheetSubResultList.AddRange(algorithmResultG.SheetSubResultList);
            }
            return sheetSubResultList;
        }

        public void SetPostProcessed(bool set)
        {
            foreach (AlgorithmResult algorithmResult in this.AlgorithmResultLDic.Values)
            {
                AlgorithmResultG algorithmResultG = algorithmResult as AlgorithmResultG;
                if (algorithmResultG == null)
                    continue;

                algorithmResultG.PostProcessed = set;
            }
        }

        public void Add(string key, AlgorithmResult algorithmResult)
        {
            this.algorithmResultDic.Add(key, algorithmResult);
        }

        public System.Drawing.Point GetOffset(int localIndex)
        {
            if (this.offsetSet == null)
                return System.Drawing.Point.Empty;

            return this.offsetSet.GetOffset(localIndex);
            //if (localIndex < 0)
            //    return this.OffsetSet.PatternOffset.Offset;
            //else
            //    return System.Drawing.Point.Add(this.OffsetSet.PatternOffset.Offset, new System.Drawing.Size(this.OffsetSet.LocalOffsets[localIndex].Offset));
        }

        public void SetOffsetStruct(OffsetStructSet offsetStructSet)
        {
            this.offsetSet = new OffsetStructSet(0);
            this.offsetSet.CopyFrom(offsetStructSet);
        }

        public FigureGroup GetDefectFigure()
        {
            FigureGroup figureGroup = new FigureGroup();

            foreach (AlgorithmResult algorithmResult in this.AlgorithmResultLDic.Values)
            {
                AlgorithmResultG algorithmResultG = algorithmResult as AlgorithmResultG;
                if (algorithmResultG == null)
                    continue;

                Figure[] figures = algorithmResultG.SheetSubResultList.Select(f => f.GetFigure()).ToArray();
                figureGroup.AddFigure(figures);

                //foreach (FoundedObjInPattern sheetSubResult in algorithmResultG.SheetSubResultList)
                //{
                //    Figure figure = sheetSubResult.GetFigure();
                //    figureGroup.AddFigure(figure);
                //}
            }

            return figureGroup;
        }

        public Image2D DrawDefects(AlgoImage algoImage, float resizeRatio)
        {
            List<AlgorithmResultG> list = this.AlgorithmResultLDic.Values.Select(f => f as AlgorithmResultG).ToList();
            list.RemoveAll(f => f == null);

            using (AlgoImage colorAlgoImage = algoImage.ConvertTo(ImageType.Color))
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(colorAlgoImage);

                list.ForEach(f => f.SheetSubResultList.ForEach(g =>
                {
                    Rectangle drawRect = Rectangle.Round(DrawingHelper.Mul(g.Region, resizeRatio));
                    drawRect.Inflate(3, 3);
                    ip.DrawRect(colorAlgoImage, drawRect, g.GetColor().ToArgb(), false);
                    ip.DrawText(colorAlgoImage, new Point(drawRect.Right + 2, drawRect.Top), g.GetColor().ToArgb(), g.Index.ToString("000"));
                }));

                return (Image2D)colorAlgoImage.ToImageD();
            }
        }
    }
}
