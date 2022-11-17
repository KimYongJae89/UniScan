using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using UniScanM.StillImage.Algorithm;
using UniScanM.StillImage.Data;
using UniScanM.Data;

namespace UniScanM.StillImage.Data
{
    public class InspectionResult : UniScanM.Data.InspectionResult
    {

        private double printingPeriod; //인쇄물 주기(mm) : 롤러149mm 이면 149*PI
        public double PrintingPeriod
        {
            get { return printingPeriod; }
            set { printingPeriod = value; }
        }

        private double printingLength;  //인쇄영역 길이(mm): 보통 빈(지그재그)영역이 28mm 정도되는데 이걸뺀 나머지 인쇄영역
        public double PrintingLength
        {
            get { return printingLength; }
            set { printingLength = value; }
        }

        public int InspZone
        {
            get { return inspZone; }
            set { inspZone = value; }
        }
        public string InspectState
        {
            get { return inspectState; }
            set { inspectState = value; }
        }

        public Rectangle SheetRectInFrame
        {
            get { return sheetRectInFrame; }
            set { sheetRectInFrame = value; }
        }

        /// <summary>
        /// 시트 내 검사 부분. 검사 후 UI에서 보여줄 부분
        /// </summary>
        public Rectangle InspRectInSheet
        {
            get { return fovRectInSheet; }
            set { fovRectInSheet = value; }
        }

        public Rectangle BlotRectInInsp
        {
            get { return blotRectInInsp; }
            set { blotRectInInsp = value; }
        }

        public Rectangle MarginRectInInsp
        {
            get { return marginRectInInsp; }
            set { marginRectInInsp = value; }
        }
        
        public LightTuneResult LightTuneResult
        {
            get { return lightTuneResult; }
            set { lightTuneResult = value; }
        }

        public TeachData TeachData
        {
            get { return teachData; }
            set { teachData = value; }
        }

        public ProcessResultList ProcessResultList
        {
            get { return processResult; }
            set { processResult = value; }
        }

        public Rectangle DisplayImageRect
        {
            get { return displayImageRect; }
            set { displayImageRect = value; }
        }

        public int SheetNgCnt
        {
            get { return sheetNgCnt; }
            set { sheetNgCnt = value; }
        }

        int inspZone;
        string inspectState;


        Rectangle sheetRectInFrame;
        Rectangle fovRectInSheet;
        Rectangle blotRectInInsp;
        Rectangle marginRectInInsp;
        Rectangle displayImageRect;
        LightTuneResult lightTuneResult;
        TeachData teachData;
        ProcessResultList processResult;
        int sheetNgCnt = 0;

        //int rewinderCutCount;

        FigureGroup tempFigureGroup = new FigureGroup();
        public FigureGroup TempFigureGroup { get => tempFigureGroup; set => tempFigureGroup = value; }


        public string GetFullImageFileName()
        {
            return string.Format("{0}.jpg", this.InspectionNo);
        }

        public string GetDefectImageFileName( int defectIndex)
        {
            return string.Format("{0}_{1}.jpg", this.InspectionNo, defectIndex);
        }

        public override void Clear(bool clearImage = true)
        {
            base.Clear(clearImage);

            //teachResult?.Image.Dispose();
            //processResult?.Image.Dispose();
        }

        public override void UpdateJudgement()
        {
            ProcessResultList list =  this.ProcessResultList;
            if (list == null)
                return;

            if (list.InterestProcessResult == null)
                this.Judgment = DynMvp.InspData.Judgment.Skip;
            else if (list.InterestProcessResult.IsGood == false || list.DefectRectList.Count > 0)
                this.Judgment = DynMvp.InspData.Judgment.Reject;
            else
                this.Judgment = DynMvp.InspData.Judgment.Accept;
        }

        public override void AppendResultFigures(FigureGroup figureGroup, DynMvp.UI.FigureDrawOption option)
        {
            // useLocalCoord true: Image is Grabbed image
            // useLocalCoord false: Image is ROI image
            base.AppendResultFigures(figureGroup, option);

            UniScanM.Data.FigureDrawOption figureDrawOption = option as UniScanM.Data.FigureDrawOption;
            Point drawOffset = Point.Empty;

            // Base: Frame
            if (this.sheetRectInFrame.IsEmpty == false)
            // Draw Sheet Rect in Frame
            {
                if (figureDrawOption.useTargetCoord)
                {
                    Figure figure = new RectangleFigure(this.sheetRectInFrame, new Pen(Color.FromArgb(64, 0, 255, 255), 3), new SolidBrush(Color.FromArgb(32, 0, 255, 255)));
                    figure.Selectable = false;
                    figureGroup.AddFigure(figure);

                    drawOffset = this.sheetRectInFrame.Location;   // Frame->Sheet
                }
            }

            if (this.lightTuneResult != null)
            // Draw BrightnessChecker Result rect in sheet
            {
                //Rectangle drawRect = brightnessResultItem.rectangle;
                //drawRect.Offset(drawOffset); // Because Sheet Pos Need

                //bool good = brightnessResultItem.offsetValue == 0;
                //FigureDrawOptionProperty prop = good ? figureDrawOption.OkFigure : figureDrawOption.NgFigure;

                //FigureGroup fg = new FigureGroup();
                //fg.AddFigure(new RectangleFigure(drawRect, prop.Pen, prop.Brush));
                //fg.AddFigure(new TextFigure(brightnessResultItem.offsetValue.ToString(), Point.Round(DrawingHelper.CenterPoint(drawRect)), new Font("Gulim", 20), Color.FromArgb(255, 255, 0, 255)));
                //fg.Selectable = false;

                //figureGroup.AddFigure(fg);
            }

            if (this.fovRectInSheet.IsEmpty == false)
            // Draw FOV rect in sheet
            {
                if (figureDrawOption.useTargetCoord)
                {
                    Rectangle drawRect = this.fovRectInSheet;
                    drawRect.Offset(drawOffset); // Because Sheet Pos Need
                    Figure figure = new RectangleFigure(drawRect, new Pen(Color.FromArgb(64, 0, 0, 255), 3), new SolidBrush(Color.FromArgb(32, 0, 0, 255)));
                    figure.Selectable = false;
                    figureGroup.AddFigure(figure);

                    //drawOffset = Point.Add(drawOffset, new Size(this.fovRectInSheet.Location));  // Frame->Sheet->Roi
                }
                else
                {
                    //drawOffset = new Point(-this.fovRectInSheet.X, -this.fovRectInSheet.Y);   // ROI->Sheet
                }
            }

            if (this.teachData != null)
            // Draw PositionChecker Result rect in sheet
            {
                FigureGroup fg = new FigureGroup();
                foreach (PatternInfoGroup group in teachData.PatternInfoGroupList)
                {
                    FigureDrawOptionProperty prop = figureDrawOption.TeachResult;

                    foreach (ShapeInfo shapeInfo in group.ShapeInfoList)
                    {
                        if (prop.ShowFigure)
                        {
                            DrawSet drawSet = (group.TeachInfo.IsValid && group.TeachInfo.Use ? prop.Good : prop.Invalid);

                            FigureGroup fg2 = new FigureGroup();

                            Rectangle drawRect = shapeInfo.BaseRect;
                            drawRect.Offset(drawOffset); // Because Sheet Pos Need
                            fg2.AddFigure(new RectangleFigure(drawRect, drawSet.Pen, drawSet.Brush));
                            //fg.AddFigure(new RectangleFigure(drawRect, new Pen(Color.FromArgb(64, 255, 255, 0), 3), new SolidBrush(Color.FromArgb(32, 255, 255, 0))));

                            if (prop.ShowText)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine(string.Format("GID {0:D2}", group.Id));
                                sb.AppendLine(string.Format("SID {0:D2}", shapeInfo.Id));
                                //sb.AppendLine(string.Format("A {0:D2}", g.ShapeInfo.Area));
                                //sb.AppendLine(string.Format("MW {0:D2}", g.TeachingInfo.Margine.Width));
                                //sb.AppendLine(string.Format("ML {0:D2}", g.TeachingInfo.Margine.Height));
                                Point textPos = new Point((int)DrawingHelper.CenterPoint(drawRect).X, drawRect.Top);
                                //fg2.AddFigure(new TextFigure(sb.ToString(), textPos, prop.FontSet.Font, prop.FontSet.Color));
                            }

                            if (figureDrawOption.PatternConnection)
                            {
                                Color[] neighborColor = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Gray };
                                ShapeInfo[] neighbor = shapeInfo.Neighborhood;
                                Rectangle neighborRect;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (neighbor[i] != null)
                                    {
                                        neighborRect = neighbor[i].BaseRect;
                                        neighborRect.Offset(drawOffset);
                                        //fg.AddFigure(new LineFigure(DrawingHelper.CenterPoint(drawRect), neighborRect.Location, new Pen(neighborColor[i], 1)));
                                    }
                                }
                            }

                            fg2.Selectable = false;
                            fg.AddFigure(fg2);
                        }
                    }
                }
                fg.Selectable = false;
                figureGroup.AddFigure(fg);
            }

            // Base: ROI
            if (this.processResult != null)
            // Draw InspectProcesser Result rect in roi
            {
                FigureDrawOptionProperty prop = figureDrawOption.ProcessResult;

                foreach (ProcessResult item in processResult.ResultList)
                {
                    if (prop.ShowFigure)
                    {
                        FigureGroup fg = new FigureGroup();

                        DrawSet drawset = (item.IsInspected ? (item.IsGood ? prop.Good   : prop.Ng) : prop.Invalid);

                        Rectangle drawRect = item.InspPatternInfo.ShapeInfo.BaseRect;
                        drawRect.Offset(drawOffset); // Because ROI Pos Need
                        fg.AddFigure(new RectangleFigure(drawRect, drawset.Pen, drawset.Brush));

                        if (prop.ShowText)
                        {
                            Point centerMiddle = Point.Round(DrawingHelper.CenterPoint(drawRect));
                            Point centerBottom = new Point(centerMiddle.X, drawRect.Bottom);
                            StringBuilder sb = new StringBuilder();

                            if (prop.ShowText)
                            {
                                int sId = item.InspPatternInfo.ShapeInfo.Id;
                                int gId = item.TrainPatternInfo.TeachInfo.Id;
                                //int sId = g.TeachPatternInfo.ShapeInfo.Id;
                                sb.AppendLine((item.IsInspected ? (item.IsGood ? "OK" : "NG") : "INV"));
                                sb.AppendLine(string.Format("GID {0:D2}", gId));
                                sb.AppendLine(string.Format("SID {0:D2}", sId));
                                sb.AppendLine(string.Format("A {0:F0}", item.InspPatternInfo.TeachInfo.Feature.Area));
                                sb.AppendLine(string.Format("BW {0:F2}", item.InspPatternInfo.TeachInfo.Feature.Blot.Width));
                                sb.AppendLine(string.Format("BL {0:F2}", item.InspPatternInfo.TeachInfo.Feature.Blot.Height));
                                sb.AppendLine(string.Format("MW {0:F2}", item.InspPatternInfo.TeachInfo.Feature.Margin.Width));
                                sb.AppendLine(string.Format("ML {0:F2}", item.InspPatternInfo.TeachInfo.Feature.Margin.Height));
                            }
                            fg.AddFigure(new TextFigure(sb.ToString(), centerMiddle, prop.FontSet.Font, prop.FontSet.Color));
                        }
                        fg.Selectable = false;

                        figureGroup.AddFigure(fg);
                    }
                }
            }

            //if(this.blotInspRect.IsEmpty==false)
            //{
            //    Rectangle drawRect = this.blotInspRect;
            //    DrawSet drawset = .IsInspected ? (item.IsGood ? prop.Good : prop.Ng) : prop.Invalid);
            //    if(option.useTargetCoord)
            //        drawRect.Offset()
            //}
        }
    }
}

