using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using UniScanG.Gravure.Data;
using DynMvp.Base;
using UniScanG.Common.Settings;

namespace UniScanG.Gravure.UI.Report.TransformControl
{
    public partial class TransformImageControl : UserControl, IMultiLanguageSupport, ITransformControl
    {
        CanvasPanel canvasPanel;

        public MouseEventHandler CanvasMouseClick { set => canvasPanel.MouseClick += value; }
        public MouseEventHandler CanvasMouseDoubleClick { set => canvasPanel.MouseDoubleClick += value; }
        public MouseLeavedDelegate CanvasMouseLeaved { set => canvasPanel.MouseLeaved += value; }
        public FigureMouseOverDelegate CanvasFigureMouseEnter { set => canvasPanel.FigureMouseEnter += value; }
        public FigureMouseOverDelegate CanvasFigureMouseLeave { set => canvasPanel.FigureMouseLeave += value; }

        public TransformImageControl()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.Dock = DockStyle.Fill;
            this.canvasPanel = new CanvasPanel()
            {
                Dock = DockStyle.Fill,
                TabIndex = 0,
                ShowCenterGuide = false,
                ReadOnly = true
            };

            this.canvasPanel.SetPanMode();
            panelImage.Controls.Add(this.canvasPanel);
        }

        private void TransformImageControl_Load(object sender, EventArgs e)
        {
            this.canvasPanel.ZoomFit();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void ClearAll()
        {
            this.canvasPanel.WorkingFigures.Clear();
            this.canvasPanel.UpdateImage(null);
        }

        private void AddFigure(Figure figure)
        {
            this.canvasPanel.WorkingFigures.AddFigure(figure);
        }

        private void ClearFigure()
        {
            this.canvasPanel.WorkingFigures.Clear();
        }

        public void UpdateImage(Bitmap bitmap)
        {
            this.canvasPanel.UpdateImage(bitmap);
        }

        public void ZoomFit()
        {
            this.canvasPanel.ZoomFit();
        }

        public void Update(List<Tuple<CalcResult, OffsetObj[]>> tupleList, float baseCircleRadUm)
        {
            SizeF pelSize = UpdateValue(tupleList.Select(f => f.Item1).ToArray());
            UpdateFigure(tupleList.Select(f => f.Item2).ToList(), baseCircleRadUm, pelSize);
        }

        public SizeF UpdateValue(CalcResult[] calcResults)
        {
            CalcResult calcResult = new CalcResult();
            if (calcResults != null && calcResults.Length > 0)
                calcResult = CalcResult.GetAverage(calcResults);

            UiHelper.SetControlText(this.translationX, calcResult.dX.ToString("F2"));
            UiHelper.SetControlText(this.translationY, calcResult.dY.ToString("F2"));
            UiHelper.SetControlText(this.rotation, calcResult.dT.ToString("F2"));
            UiHelper.SetControlText(this.skewnessLtrb, calcResult.dL1.ToString("F2"));
            UiHelper.SetControlText(this.skewnessRtlb, calcResult.dL2.ToString("F2"));
            UiHelper.SetControlText(this.sizeW, calcResult.dW.ToString("F2"));
            UiHelper.SetControlText(this.sizeH, calcResult.dH.ToString("F2"));
            return calcResult.pelSize;
        }

        public void UpdateFigure(List<OffsetObj[]> offsetObjsList, float baseCircleRadUm, SizeF pelSize)
        {
            ClearFigure();

            if (offsetObjsList.Count == 0)
                return;

            // UpdateFigure
            OffsetObj[] baseOffsetObjs = offsetObjsList.FirstOrDefault(f => f.Length > 0);
            if (baseOffsetObjs == null)
                return;

            PointF[] basePoints = baseOffsetObjs.Select(f => DrawingHelper.CenterPoint(f.Region)).ToArray();

            for (int i = 0; i < offsetObjsList.Count; i++)
            {
                OffsetObj[] offsetObjs = offsetObjsList[i];
                PointF[] matchingPointList = offsetObjs.Select(f => PointF.Add(f.RefPoint, f.MatchingOffsetPx)).ToArray();

                RectangleF fullRectPx = matchingPointList.Select(f => new RectangleF(f, SizeF.Empty)).Aggregate((f, g) => RectangleF.Union(f, g));
                PointF[] cornor4Points = new PointF[]
                {
                    matchingPointList.OrderBy(f=>MathHelper.GetLength(new PointF(fullRectPx.Left,fullRectPx.Top), f)).FirstOrDefault(),
                    matchingPointList.OrderBy(f=>MathHelper.GetLength(new PointF(fullRectPx.Right,fullRectPx.Top), f)).FirstOrDefault(),
                    matchingPointList.OrderBy(f=>MathHelper.GetLength(new PointF(fullRectPx.Right,fullRectPx.Bottom), f)).FirstOrDefault(),
                    matchingPointList.OrderBy(f=>MathHelper.GetLength(new PointF(fullRectPx.Left,fullRectPx.Bottom), f)).FirstOrDefault()
                };

                //Array.ForEach(offsetObjs, f =>
                //{
                //    PointF adjCenter = DrawingHelper.CenterPoint(f.Region);
                //    float[] dist = basePoints.Select(g => MathHelper.GetLength(adjCenter, g)).ToArray();
                //    float minDist = dist.Min();
                //    int minIdx = Array.FindIndex(dist, g => g == minDist);
                //    PointF basePoint = basePoints[minIdx];

                //    PointF foundPt = PointF.Add(basePoint, f.MatchingOffsetPx);
                //    matchingPointList.Add(foundPt);
                //    //RectangleF foundCircle = DrawingHelper.FromCenterSize(foundPt, new SizeF(6, 6));
                //    //fg.AddFigure(new EllipseFigure(foundCircle, new Pen(Color.Red, 1)) { Tag = f });

                //    int cornorIdx = Array.IndexOf(tuple.Item1.cornorObjs, f);
                //    if (cornorIdx >= 0 && cornorIdx < cornor4Points.Length)
                //        cornor4Points[cornorIdx] = foundPt;

                //});

                if (Array.TrueForAll(cornor4Points, f => !f.IsEmpty))
                {
                    AddFigure(new LineFigure(cornor4Points[0], cornor4Points[1], new Pen(Color.Red, 1)));
                    AddFigure(new LineFigure(cornor4Points[1], cornor4Points[2], new Pen(Color.Red, 1)));
                    AddFigure(new LineFigure(cornor4Points[2], cornor4Points[3], new Pen(Color.Red, 1)));
                    AddFigure(new LineFigure(cornor4Points[3], cornor4Points[0], new Pen(Color.Red, 1)));
                }

                Array.ForEach(matchingPointList, f => AddFigure(new EllipseFigure(DrawingHelper.FromCenterSize(f, new SizeF(6, 6)), new Pen(Color.LightGreen, 1))));
            }

            foreach (OffsetObj offsetObj in baseOffsetObjs)
            {
                PointF centerPt = DrawingHelper.CenterPoint(offsetObj.Region);
                // Center
                //fg.AddFigure(new EllipseFigure(DrawingHelper.FromCenterSize(basePoint, new SizeF(10, 10)), new Pen(Color.Yellow, 1)));

                // Base Circle
                float baseCircleW = (baseCircleRadUm / pelSize.Width) * 2;
                float baseCircleH = (baseCircleRadUm / pelSize.Height) * 2;
                AddFigure(new EllipseFigure(DrawingHelper.FromCenterSize(centerPt, new SizeF(baseCircleW, baseCircleH)), new Pen(Color.Yellow, 1)) { Tag = offsetObj });
            }

            this.canvasPanel.WorkingFigures.Scale(SystemTypeSettings.Instance().ResizeRatio);
        }
    }
}
