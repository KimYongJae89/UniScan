using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanX.MPAlignment.Data;


namespace UniScanX.MPAlignment.Algo
{
    public class MPAlgorithmResult : DynMvp.Vision.AlgorithmResult
    {

        public float[] EdgeProfileX;
        public float[] EdgeProfileY;
        MPAlgorithmParam algorithmParam;
        public MPAlgorithmParam AlgorithmParam
        {
            get => this.algorithmParam;
            set => this.algorithmParam = value;
        }

        ImageD resultImageD;
        public ImageD ResultImageD
        {
            get => this.resultImageD;
            set => this.resultImageD = value;
        }

        //RotatedRect clipRect;
        //public RotatedRect ClipRect
        //{
        //    get => this.clipRect;
        //    set => this.clipRect = value;
        //}
        Rectangle clipRect;
        public Rectangle ClipRect
        {
            get => this.clipRect;
            set => this.clipRect = value;
        }
        private List<MarginEdgePair> xLinePairList;
        public List<MarginEdgePair> XLinePairList
        {
            get => this.xLinePairList;
            set => this.xLinePairList = value;
        }

        private List<MarginEdgePair> yLinePairList;
        public List<MarginEdgePair> YLinePairList
        {
            get => this.yLinePairList;
            set => this.yLinePairList = value;
        }

        DefectOnScreen[] defectOnScreens;
        public DefectOnScreen[] DefectOnScreens
        {
            get => this.defectOnScreens;
            set => this.defectOnScreens = value;
        }
        
        public MPAlgorithmResult() : base(MPAlgorithm.TypeName)
        {

        }

        public PointF GetMargin()
        {
            float marginX = 0;
            XLinePairList.ForEach(f => marginX += f.GetMagin());
            marginX /= XLinePairList.Count;

            float marginY = 0;
            YLinePairList.ForEach(f => marginY += f.GetMagin());
            marginY /= YLinePairList.Count;

            return new PointF(marginX,marginY);
        }

        public PointF GetOffset2nd()
        {
            float offsetX = GetAverageOffet(xLinePairList);
            float offsetY = GetAverageOffet(yLinePairList);

            float GetAverageOffet(List<MarginEdgePair> list)
            {
                float sum = 0;
                int count = 0;
                foreach (var pair in xLinePairList)
                {
                    var offset = pair.GetOffset2nd();
                    if (offset != float.NaN)
                    {
                        sum += offset;
                        count++;
                    }
                }
                return sum / count;
            }
            return new PointF(offsetX, offsetY);
        }

        public override void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            FigureGroup resultFigureGroup = new FigureGroup();

            foreach(var pair in xLinePairList)
            {
                AddfigureX(pair.Left1stRising, new Pen(Brushes.Red, 1));
                AddfigureX(pair.Right1stFalling, new Pen(Brushes.Blue, 1));
                AddfigureX(pair.Left2ndFalling, new Pen(Brushes.Green, 1));
                AddfigureX(pair.Right2ndRising, new Pen(Brushes.Yellow, 1));

                void AddfigureX(Peak peak, Pen pen)
                {
                    if (peak == null) return;
                    PointF startPoint = new PointF(peak.peakPos, 0);
                    PointF endPoint = new PointF(peak.peakPos, ClipRect.Height);
                    LineFigure lineFigure = new LineFigure(startPoint, endPoint, pen);
                    lineFigure.Selectable = false;
                    lineFigure.Tag = pair;
                    resultFigureGroup.AddFigure(lineFigure);
                }
            }
            foreach (var pair in yLinePairList)
            {
                AddfigureY(pair.Left1stRising, new Pen(Brushes.Red, 1));
                AddfigureY(pair.Right1stFalling, new Pen(Brushes.Blue, 1));
                AddfigureY(pair.Left2ndFalling, new Pen(Brushes.Green, 1));
                AddfigureY(pair.Right2ndRising, new Pen(Brushes.Yellow, 1));

                void AddfigureY(Peak peak, Pen pen)
                {
                    if (peak == null) return;
                    PointF startPoint = new PointF(ClipRect.Left, peak.peakPos);
                    PointF endPoint = new PointF(ClipRect.Right, peak.peakPos);
                    LineFigure lineFigure = new LineFigure(startPoint, endPoint, pen);
                    lineFigure.Selectable = false;
                    lineFigure.Tag = pair;
                    resultFigureGroup.AddFigure(lineFigure);
                }
            }
            resultFigureGroup.Offset(offset.X,offset.Y);
            figureGroup.AddFigure(resultFigureGroup.FigureList.ToArray());
        }

        public override string ToString()
        {
            return String.Format("Margin =  {0}  ,  Offset ={1}", GetMargin().ToString(), GetOffset2nd().ToString());
        }
        //public override List<AlgorithmResultValue> GetResultValues()
        //{
        //    List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
        //    resultValues.Add(new AlgorithmResultValue("Result", true));
        //    resultValues.Add(new AlgorithmResultValue("Brightness", UpperValue, LowerValue, 0));

        //    return resultValues;
        //}
    }
}
