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
using UniScanG.UI.Etc;
using UniScanG.Data;
using DynMvp.Base;
using UniScanG.Common.Settings;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.UI.Touch;
using System.Diagnostics;
using DynMvp.Data.UI;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision;
using System.IO;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.UI.Report.TransformControl;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.UI.Report
{
    public partial class MultiSheetResultPanel : UserControl, IMultiLanguageSupport
    {
        private delegate void SingleSheetChartUpdateDelegate();
        private delegate void MultiSheetChartUpdateDelegate(RepeatedDefectItem repeatedDefectItem);
        private delegate void UpdateListDelegate();

        ContextInfoForm contextInfoForm;
        CanvasPanel imageCanvasPanel;
        //CanvasPanel transformCanvasPanel;
        RepeatedDefectItemCollection repeatedDefectItemList;
        List<MergeSheetResult> overallSheetResultList;
        List<MergeSheetResult> selectedSheetList;

        CheckBox[] defectCheckBoxs;

        Series[] defectSerieses;
        Series seriesNoprint;
        Series seriesPinhole;
        Series seriesSpread;
        Series seriesSheetAttack;
        Series seriesDielectric;
        Series seriesSticker;
        Series seriesSelection;

        Series seriesHeight;
        double heightSlope;

        List<Series> marginSeriesList;

        TransformImageControl transformImageControl;
        TransformChartControl transformChartControl;

        DefectType[] defectTypes = new DefectType[] { DefectType.Noprint, DefectType.PinHole, DefectType.Spread, DefectType.Attack, DefectType.Coating, DefectType.Sticker };
        bool onUpdate = false;
        bool showBufferDefect = false;

        public MultiSheetResultPanel()
        {
            InitializeComponent();

            onUpdate = true;

            chartDefect.ChartAreas[0].AxisX.Name = "chartDefectAxisX";
            chartDefect.ChartAreas[0].AxisY.Name = "chartDefectAxisY";

            chartLength.ChartAreas[0].AxisX.Name = "chartLengthAxisX";
            chartLength.ChartAreas[0].AxisY.Name = "chartLengthAxisY";

            chartMargin.ChartAreas[0].AxisX.Name = "chartMarginAxisX";
            chartMargin.ChartAreas[0].AxisY.Name = "chartMarginAxisY";

            this.seriesNoprint = new Series
            {
                Name = DefectType.Noprint.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Noprint),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Line
            };

            this.seriesPinhole = new Series
            {
                Name = DefectType.PinHole.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.PinHole),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Line
            };

            this.seriesSpread = new Series
            {
                Name = DefectType.Spread.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Spread),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Line
            };

            this.seriesSheetAttack = new Series
            {
                Name = DefectType.Attack.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Attack),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Line
            };

            this.seriesDielectric = new Series
            {
                Name = DefectType.Coating.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Coating),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Area
            };

            this.seriesSticker = new Series
            {
                Name = DefectType.Sticker.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Sticker),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                //ChartType = SeriesChartType.Area
            };

            this.seriesSelection = new Series
            {
                Name = "Selection",
                Color = Color.Black,
                BorderWidth = 3,
                BorderDashStyle = ChartDashStyle.Solid,
                //ChartType = SeriesChartType.Column
            };

            this.defectSerieses = new Series[] { seriesNoprint, seriesPinhole, seriesSpread, seriesSheetAttack, seriesDielectric, seriesSticker, seriesSelection };
            this.chartDefect.Series.Add(seriesNoprint);
            this.chartDefect.Series.Add(seriesPinhole);
            this.chartDefect.Series.Add(seriesSpread);
            this.chartDefect.Series.Add(seriesSheetAttack);
            this.chartDefect.Series.Add(seriesDielectric);
            this.chartDefect.Series.Add(seriesSticker);
            this.chartDefect.Series.Add(seriesSelection);
            this.chartDefect.ChartAreas[0].BackColor = Color.Gray;

            this.seriesHeight = new Series
            {
                Name = "Height",
                Color = Color.Red,
                BorderWidth = 3,
                BorderDashStyle = ChartDashStyle.Solid,
                //ChartType = SeriesChartType.Line
            };
            this.chartLength.Series.Add(seriesHeight);
            this.heightSlope = double.NaN;

            this.marginSeriesList = new List<Series>();
            this.gridMargin.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "columnMarginPatternNo",
                HeaderText = "PatternNo",
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "D" }
            });

            this.defectCheckBoxs = new CheckBox[] { checkNoprint, checkPinhole, checkSpread, checkSheetattack, checkDielectric, checkSticker };

            this.contextInfoForm = new ContextInfoForm();

            this.repeatedDefectItemList = new RepeatedDefectItemCollection();

            this.imageCanvasPanel = new CanvasPanel()
            {
                Dock = DockStyle.Fill,
                TabIndex = 0,
                ShowCenterGuide = false,
                ReadOnly = true
            };
            this.imageCanvasPanel.FigureMouseEnter += contextInfoForm.CanvasPanel_FigureFocused;
            this.imageCanvasPanel.FigureMouseLeave += contextInfoForm.CanvasPanel_FigureMouseLeaved;
            this.imageCanvasPanel.MouseLeaved += contextInfoForm.CanvasPanel_MouseLeaved;
            this.imageCanvasPanel.FigureClicked += ImageCanvasPanel_FigureClicked;
            this.imageCanvasPanel.MouseClick += CanvasPanel_MouseClick;
            this.imageCanvasPanel.MouseDoubleClick += CanvasPanel_MouseDoubleClick;
            this.imageCanvasPanel.SetPanMode();
            imagePanel.Controls.Add(imageCanvasPanel);


            this.transformImageControl = new TransformImageControl()
            {
                CanvasMouseClick = CanvasPanel_MouseClick,
                CanvasMouseDoubleClick = CanvasPanel_MouseDoubleClick,
                CanvasFigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused,
                CanvasFigureMouseLeave = contextInfoForm.CanvasPanel_FigureMouseLeaved,
                CanvasMouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved
            };
            this.tabPageTrandChartTransform.Controls.Add(this.transformImageControl);

            this.transformChartControl = new TransformChartControl();
            this.tabPageTrandChartTransform2.Controls.Add(this.transformChartControl);

            trandChartTabControl.TabPages.RemoveByKey("tabPageTrandChartTransform");

            if (!AlgorithmSetting.Instance().UseExtMargin)
                trandChartTabControl.TabPages.RemoveByKey("tabPageTrandChartMargin");

            if (!AlgorithmSetting.Instance().UseExtTransfrom)
            {
                trandChartTabControl.TabPages.RemoveByKey("tabPageTrandChartTransform2");
            }

            StringManager.AddListener(this);
            ColorTable.OnColorTableUpdated += UpdateControlsColor;
        }

        private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CanvasPanel canvasPanel = sender as CanvasPanel;
                if (canvasPanel == null)
                    return;

                this.defectList.ClearSelection();
            }
        }

        private void CanvasPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CanvasPanel canvasPanel = sender as CanvasPanel;
            if (canvasPanel == null)
                return;

            if (e.Button == MouseButtons.Left)
                canvasPanel.ZoomFit();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            string typeString = this.GetType().FullName;
            chartDefect.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().ToString(), chartDefect.ChartAreas[0].AxisX.Name, chartDefect.ChartAreas[0].AxisX.Title);
            chartDefect.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().ToString(), chartDefect.ChartAreas[0].AxisY.Name, chartDefect.ChartAreas[0].AxisY.Title);

            chartLength.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().ToString(), chartLength.ChartAreas[0].AxisX.Name, chartLength.ChartAreas[0].AxisX.Title);
            chartLength.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().ToString(), chartLength.ChartAreas[0].AxisY.Name, chartLength.ChartAreas[0].AxisY.Title);

            chartMargin.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().ToString(), chartMargin.ChartAreas[0].AxisX.Name, chartMargin.ChartAreas[0].AxisX.Title);
            chartMargin.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().ToString(), chartMargin.ChartAreas[0].AxisY.Name, chartMargin.ChartAreas[0].AxisY.Title);
        }

        private void ImageCanvasPanel_FigureClicked(Figure figure, MouseEventArgs e)
        {
            FoundedObjInPattern item = figure.Tag as FoundedObjInPattern;
            if (item == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                foreach (DataGridViewRow row in this.defectList.Rows)
                {
                    if (row.Tag is FoundedObjInPattern)
                    {
                        FoundedObjInPattern tag = (FoundedObjInPattern)row.Tag;
                        if (item == tag)
                        {
                            row.Selected = true;
                            this.defectList.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                    else if (row.Tag is RepeatedDefectItem)
                    {
                        RepeatedDefectItem tag = (RepeatedDefectItem)row.Tag;
                        if (tag.RepeatedDefectElementList.Contains(item))
                        {
                            row.Selected = true;
                            this.defectList.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
            }
            //else if (e.Button == MouseButtons.Right)
            //{
            //    SaveFileDialog dlg = new SaveFileDialog();
            //    dlg.Filter = "Bitmap(*.bmp)|*.bmp";
            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        //ImageHelper.SaveImage(bitmap, dlg.FileName);
            //        Bitmap bitmap2 = new Bitmap(item.RepeatedDefectElementList.FirstOrDefault()?.Image);
            //        ImageHelper.SaveImage(bitmap2, dlg.FileName);
            //        bitmap2.Dispose();
            //    }
            //}
        }

        public void Clear()
        {
            this.defectImage.Image = null;

            this.imageCanvasPanel.WorkingFigures.Clear();
            this.imageCanvasPanel.UpdateImage(null);

            this.transformImageControl.ClearAll();
            this.transformChartControl.ClearAll();

            ClearChartData();
        }

        public void SetFilterRect(RectangleF rect)
        {
            this.imageCanvasPanel.BackgroundFigures.Clear();
            if (!rect.IsEmpty)
            {
                float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
                Figure figure = new RectangleFigure(rect, new Pen(Color.Green, 10));
                figure.Scale(resizeRatio);
                this.imageCanvasPanel.BackgroundFigures.AddFigure(figure);
            }
            this.imageCanvasPanel.Invalidate();
        }

        public bool SelectSheet(Bitmap modelBitmap, List<MergeSheetResult> resultList)
        {
            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString("Update Defects Density..."));
            simpleProgressForm.Show(() =>
            {
                // Update Image
                MergeSheetResult first = resultList.FirstOrDefault(f => f.PrevImage != null);
                Bitmap bitmap = first == null ? modelBitmap : first.PrevImage;

                bool zoomFit = (imageCanvasPanel.Image == null);
                UpdateImageBitmap((Bitmap)bitmap?.Clone());
                if (zoomFit)
                    imageCanvasPanel.ZoomFit();

                // Figures
                imageCanvasPanel.WorkingFigures.Clear();

                this.selectedSheetList = resultList.OrderBy(f => f.Index).ToList();

                // Make RepeatedDefect
                this.repeatedDefectItemList.Clear();
                for (int i = 0; i < this.selectedSheetList.Count; i++)
                {
                    if (tokenSource.IsCancellationRequested == true)
                        break;

                    MergeSheetResult result = this.selectedSheetList[i];
                    this.repeatedDefectItemList.AddResult(result, false);
                }

                UpdateDefectDensityChart();

                UpdateDefectList();
                UpdateDefectLengthChart();
                UpdateTransformMap((Bitmap)bitmap?.Clone());
            }, tokenSource);

            //SelectDefect();


            return true;
        }

        private void UpdateDefectDensityChart()
        {
            if(this.chartDensity.InvokeRequired)
            {
                this.chartDensity.BeginInvoke(new MethodInvoker(UpdateDefectDensityChart));
                return;
            }

            this.chartDensity.Series.Clear();

            List<Series> seriesList = new List<Series>();
            this.selectedSheetList.ForEach(f =>
            {
                if (this.chartDensity.Series.Count >= 100)
                    return;

                if (f.DefectDensity != null && f.DefectDensity.Length > 0 && f.DefectDensity.Max() > 0)
                {
                    IEnumerable<PointF> points = f.DefectDensity.Select((g, i) => new PointF(i, g));
                    Series s = new Series(f.Index.ToString());
                    s.ChartType = SeriesChartType.StackedColumn;
                    s.Points.DataBind(points, "X", "Y", "");
                    seriesList.Add(s);
                }
            });

            if (seriesList.Count < 100)
                seriesList.ForEach(f => this.chartDensity.Series.Add(f));

            if (this.layoutDDensity.Visible = this.chartDensity.Series.Count > 0)
            {
                int xMax = this.chartDensity.Series.Max(f => f.Points.Count());
                float yMax = 0;
                for (int i = 0; i < xMax; i++)
                {
                    float y = (float)this.chartDensity.Series.Sum(f => f.Points[i].YValues.Max());
                    yMax = Math.Max(yMax, y);
                }

                this.chartDensity.ChartAreas[0].AxisX.Minimum =  - 0.5;
                this.chartDensity.ChartAreas[0].AxisX.Maximum = xMax - 0.5;
                this.chartDensity.ChartAreas[0].AxisX.Crossing =  - 0.5;

                this.chartDensity.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 0.5;
                this.chartDensity.ChartAreas[0].AxisX.LabelStyle.Interval = 10;
                this.chartDensity.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true;
                this.chartDensity.ChartAreas[0].AxisX.MajorGrid.Interval = 5;

                this.chartDensity.ChartAreas[0].AxisY.Minimum = 0;
                this.chartDensity.ChartAreas[0].AxisY.Maximum = Math.Max(0.1, Math.Round(yMax * 1.2, 3));
                this.chartDensity.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            }
        }

        private void UpdateDefectList()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateListDelegate(UpdateDefectList));
                return;
            }

            this.onUpdate = true;

            this.defectList.Rows.Clear();
            this.repeatedDefectItemList.Sort();
            List<DataGridViewRow> newRowList = new List<DataGridViewRow>();
            if (selectedSheetList.Count == 1)
            {
                MergeSheetResult mergeSheetResult = selectedSheetList[0];
                for (int i = 0; i < mergeSheetResult.SheetSubResultList.Count; i++)
                {
                    //Data.DefectInPattern subResult = mergeSheetResult.SheetSubResultList[i] as Data.DefectInPattern;
                    FoundedObjInPattern subResult = mergeSheetResult.SheetSubResultList[i] as FoundedObjInPattern;
                    if (subResult != null)
                    {
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(defectList);
                        newRow.Cells[0].Value = i + 1;
                        newRow.Cells[1].Value = subResult.GetDefectType().GetLocalString();
                        newRow.Cells[1].ToolTipText = subResult.GetDefectTypeDiscription();

                        newRow.Cells[2].Value = 1;
                        newRow.Tag = subResult;

                        newRowList.Add(newRow);
                    }
                }
            }
            else
            {
                for (int i = 0; i < repeatedDefectItemList.Count; i++)
                {
                    FoundedObjInPattern lastResult = repeatedDefectItemList[i].RepeatedDefectElementList.First(f => f != null);
                    if (lastResult == null)
                        continue;

                    DefectType defectType = lastResult.GetDefectType();

                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(defectList);
                    newRow.Cells[0].Value = i + 1;
                    newRow.Cells[1].Value = StringManager.GetString(typeof(DefectType).FullName, defectType.ToString());
                    newRow.Cells[2].Value = repeatedDefectItemList[i].ValidItemCount;
                    newRow.Tag = repeatedDefectItemList[i];
                    newRowList.Add(newRow);
                }

                //newRowList.Sort((f, g) => ((int)f.Cells[2].Value).CompareTo((int)g.Cells[2].Value));
            }

            defectList.Rows.AddRange(newRowList.ToArray());
            defectList.ClearSelection();
            SelectDefect();
            this.onUpdate = false;

            //defectList.Rows[0].Selected = true;
        }

        private void UpdateDefectLengthChart()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SingleSheetChartUpdateDelegate(UpdateDefectLengthChart));
                return;
            }

            ClearChartData();

            if (selectedSheetList.Count == 0)
                return;

            SeriesChartType chartType = this.selectedSheetList.Count == 1 ? SeriesChartType.Column : SeriesChartType.Line;
            int xMin = Math.Max(0, this.selectedSheetList.Min(f => f.Index)) + (this.selectedSheetList.Count == 1 ? 0 : 1);
            int xMax = this.selectedSheetList.Max(f => f.Index) + +(this.selectedSheetList.Count == 1 ? 2 : 1);
            SetChartAxisXRange(this.chartDefect.ChartAreas[0].AxisX, xMin, xMax);
            SetChartAxisXRange(this.chartLength.ChartAreas[0].AxisX, xMin, xMax);

            // Add data
            Array.ForEach(this.defectSerieses, f => f.ChartType = chartType);
            this.selectedSheetList.ForEach(f => AddChartData(f.Index + 1, f.SheetSubResultList, f.SheetSize.Height));
            UpdateHeightSlope();

            // Apply Axis Size
            int yMin = 0;
            int yMax = this.selectedSheetList.Max(f =>
            {
                var group = f.SheetSubResultList.GroupBy(g => g.GetDefectType()).ToArray();
                if (group.Length == 0)
                    return 0;
                return group.Max(g => g.Count());
            });

            if (yMax < 4)
                yMax = 4;
            else
                yMax = (int)Math.Ceiling(yMax / 25f) * 25;
            SetChartAxisYRange(chartDefect.ChartAreas[0].AxisY, yMin, yMax, false);

            // Set heigth length
            this.seriesHeight.ChartType = chartType;

            double y2Max = this.seriesHeight.Points.Max(f => Math.Ceiling(f.YValues.Max() * 100)) / 100;
            double y2Min = this.seriesHeight.Points.Min(f => Math.Floor(f.YValues.Min() * 100)) / 100;
            if (y2Max - y2Min < 0.5)
            {
                double average = Math.Round((y2Max + y2Min) / 2, 1);
                y2Max = average + 0.25;
                y2Min = average - 0.25;
            }
            SetChartAxisYRange(chartLength.ChartAreas[0].AxisY, y2Min, y2Max, true);
        }

        private void UpdateTransformMap(Bitmap bitmap)
        {
            this.transformImageControl.UpdateImage((Bitmap)bitmap?.Clone());
            this.transformChartControl.UpdateImage((Bitmap)bitmap?.Clone());

            List<Tuple<CalcResult, OffsetObj[]>> tupleList = new List<Tuple<CalcResult, OffsetObj[]>>();
            List<MergeSheetResult> mergeSheetResultList = this.overallSheetResultList.FindAll(f => this.selectedSheetList.Exists(g => f.Index == g.Index));
            if (this.selectedSheetList.Count > 0)
            {
                foreach (MergeSheetResult mergeSheetResult in mergeSheetResultList)
                {
                    List<OffsetObj> offsetObjList = mergeSheetResult.SheetSubResultList.FindAll(f => f is OffsetObj).Cast<OffsetObj>().ToList();
                    CalcResult calcResult = CalculateTransform(offsetObjList);
                    tupleList.Add(new Tuple<CalcResult, OffsetObj[]>(calcResult, offsetObjList.ToArray()));
                }
            }

            this.transformImageControl.Update(tupleList, 500);
            this.transformChartControl.Update(tupleList, 500);
            //this.transformImageControl.UpdateFigure(tupleList, 500);
            //this.transformChartControl.UpdateFigure(tupleList, 500);
            //this.transformImageControl.UpdateValue(tupleList.Select(f => f.Item1).ToArray());
            //this.transformChartControl.UpdateValue(tupleList.Select(f => f.Item1).ToArray());
        }

        private CalcResult CalculateTransform(List<OffsetObj> offsetObjList)
        {
            CalcResult calcResult = new CalcResult();
            MergeSheetResult mergeSheetResult = this.selectedSheetList[0];
            SizeF sheetSizeUm = mergeSheetResult.SheetSize;
            Size sheetSizePx = mergeSheetResult.SheetSizePx;
            List<OffsetObj> calcOffsetObjList = offsetObjList?.FindAll(f => !f.IsDefect).Cast<OffsetObj>().ToList();
            calcResult.pelSize = SizeF.Empty;

            if (!sheetSizePx.IsEmpty)
            {
                float w = mergeSheetResult.SheetSize.Width / mergeSheetResult.SheetSizePx.Width;
                float h = mergeSheetResult.SheetSize.Height / mergeSheetResult.SheetSizePx.Height;
                calcResult.pelSize = new SizeF(w * 1000, h * 1000);
            }
            else
            {
                if (calcOffsetObjList.Count == 0)
                    return calcResult;

                float w = calcOffsetObjList.Select(f => f.MatchingOffsetUm.Width / f.MatchingOffsetPx.Width).Where(f => !float.IsNaN(f)).Average();
                float h = calcOffsetObjList.Select(f => f.MatchingOffsetUm.Height / f.MatchingOffsetPx.Height).Where(f => !float.IsNaN(f)).Average();
                calcResult.pelSize = new SizeF(w, h);
                sheetSizePx = Size.Round(DrawingHelper.Div(DrawingHelper.Mul(mergeSheetResult.SheetSize, 1000), calcResult.pelSize));
            }

            PointF centerPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, sheetSizePx));

            // Translation
            calcResult.dX = float.NaN;
            calcResult.dY = float.NaN;
            if (calcOffsetObjList.Count > 0)
            {
                calcResult.dX = calcOffsetObjList.Average(f => f.MatchingOffsetUm.Width);
                calcResult.dY = calcOffsetObjList.Average(f => f.MatchingOffsetUm.Height);
            }
            //UiHelper.SetControlText(this.transformTranslationX, dX.ToString("F2"));
            //UiHelper.SetControlText(this.transformTranslationY, dY.ToString("F2"));

            // Rotation
            calcResult.dT = float.NaN;
            Tuple<float, float>[] dTs = calcOffsetObjList.Select(f =>
            {
                float deltaT = (float)MathHelper.GetAngle360(centerPt, f.AdjPointPx, PointF.Add(f.AdjPointPx, f.MatchingOffsetPx));
                if (deltaT > 180)
                    deltaT -= 360;

                float length = MathHelper.GetLength(centerPt, f.AdjPointPx);
                return new Tuple<float, float>(deltaT, length);
            }).ToArray();
            if (dTs.Length > 0)
                calcResult.dT = dTs.Average(f => f.Item1 * f.Item2) / dTs.Sum(f => f.Item2);
            //UiHelper.SetControlText(this.transformRotation, dT.ToString("F2"));

            PointF[] cornorPts = DrawingHelper.GetPoints(new Rectangle(Point.Empty, sheetSizePx), 0);
            OffsetObj[] cornors = new OffsetObj[]
            {
                calcOffsetObjList.OrderBy(f=> MathHelper.GetLength(cornorPts [0], f.AdjPointPx)).FirstOrDefault(),  // LT
                calcOffsetObjList.OrderBy(f=> MathHelper.GetLength(cornorPts [1], f.AdjPointPx)).FirstOrDefault(),  // RT
                calcOffsetObjList.OrderBy(f=> MathHelper.GetLength(cornorPts [2], f.AdjPointPx)).FirstOrDefault(),  // RB
                calcOffsetObjList.OrderBy(f=> MathHelper.GetLength(cornorPts [3], f.AdjPointPx)).FirstOrDefault(),  // LB
            };

            calcResult.dL1 = float.NaN;
            calcResult.dL2 = float.NaN;
            calcResult.dW = float.NaN;
            calcResult.dH = float.NaN;
            if (cornors.Count(f => f != null) > 2)
            {
                // Skewness
                PointF[] adjPoints = cornors.Select(f => f.AdjPointUm).ToArray();
                PointF[] matchingPoints = cornors.Select(f => PointF.Add(f.AdjPointUm, f.MatchingOffsetUm)).ToArray();
                calcResult.dL1 = MathHelper.GetLength(matchingPoints[0], matchingPoints[2]) - MathHelper.GetLength(adjPoints[0], adjPoints[2]);
                calcResult.dL2 = MathHelper.GetLength(matchingPoints[1], matchingPoints[3]) - MathHelper.GetLength(adjPoints[1], adjPoints[3]);

                // Erode/Dilate
                calcResult.dW = ((cornors[0].MatchingOffsetUm.Width + cornors[3].MatchingOffsetUm.Width) - (cornors[1].MatchingOffsetUm.Width + cornors[2].MatchingOffsetUm.Width)) / 2;
                calcResult.dH = ((cornors[0].MatchingOffsetUm.Height + cornors[3].MatchingOffsetUm.Height) - (cornors[1].MatchingOffsetUm.Height + cornors[2].MatchingOffsetUm.Height)) / 2;
            }
            calcResult.cornorObjs = cornors;

            //UiHelper.SetControlText(this.transformSkewnessLtrb, dLTRB.ToString("F2"));
            //UiHelper.SetControlText(this.transformSkewnessRtlb, dRTLB.ToString("F2"));
            //UiHelper.SetControlText(this.transformSizeW, dW.ToString("F2"));
            //UiHelper.SetControlText(this.transformSizeH, dH.ToString("F2"));
            return calcResult;
        }

        internal Rectangle[] GetHighlightPosition()
        {
            List<Rectangle> highlightList = new List<Rectangle>();
            foreach (DataGridViewRow row in this.defectList.SelectedRows)
            {
                if (row.Tag is RepeatedDefectItem)
                {
                    RepeatedDefectItem item = (RepeatedDefectItem)row.Tag;
                    highlightList.Add(Rectangle.Round(item.BoundingRect));
                }
            }

            return highlightList.ToArray();
        }

        internal void SetOverallResult(List<MergeSheetResult> overallSheetResultList)
        {
            this.overallSheetResultList = overallSheetResultList;
            UpdateMarginChart();
        }

        internal void UpdateMarginChart()
        {
            ClearChartDataMargin();
            chartMargin.Series.Clear();
            SortedList<int, MarginPanelItem._Item> itemList = new SortedList<int, MarginPanelItem._Item>();

            UiHelper.SuspendDrawing(gridMargin);
            for (int j = 0; j < this.overallSheetResultList.Count; j++)
            {
                int patternNo = this.overallSheetResultList[j].Index;
                List<FoundedObjInPattern> sheetSubResultList = this.overallSheetResultList[j].SheetSubResultList;
                List<MarginObj> marginObjList = sheetSubResultList.FindAll(f => f.GetDefectType() == DefectType.Margin).ConvertAll<MarginObj>(f => (MarginObj)f);
                marginObjList.Sort();

                if (marginObjList.Count > 0)
                {
                    MarginPanelItem._Item item = new MarginPanelItem._Item();
                    item.Update(patternNo, marginObjList);
                    itemList.Add(patternNo, item);

                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(gridMargin);
                    newRow.Cells[0].Value = (patternNo + 1).ToString();
                    for (int i = 0; i < marginObjList.Count; i++)
                    {
                        MarginObj f = marginObjList[i];
                        string name = f.GetDisplayName();
                        Series series = marginSeriesList.Find(g => g.Name == name);
                        if (series == null)
                        {
                            series = new Series
                            {
                                Name = name,
                                Color = Gravure.Data.ColorTable.GetColor(DefectType.Margin),
                                BorderWidth = 2,
                                BorderDashStyle = ChartDashStyle.Solid,
                                //ChartType = SeriesChartType.Area
                            };
                            marginSeriesList.Add(series);
                        }

                        if (!gridMargin.Columns.Contains(name))
                        {
                            gridMargin.Columns.Add(new DataGridViewTextBoxColumn()
                            {
                                Name = name,
                                HeaderText = string.Format("{0}{1}{2}", series.Name, Environment.NewLine, "W/L")
                            });
                            newRow.Cells.Add(new DataGridViewTextBoxCell());
                        }

                        int colIndex = gridMargin.Columns[name].Index;
                        newRow.Cells[colIndex].Value = string.Format("{0:F1} / {1:F1}", f.RealWidth, f.RealHeight);

                        double value = MathHelper.Min(f.MarginSizeUm.Width, f.MarginSizeUm.Height);
                        if (!double.IsNaN(value))
                        {
                            series.Points.AddXY(patternNo, value);;
                        }
                    };
                    gridMargin.Rows.Add(newRow);
                }
            }
            gridMargin.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            UiHelper.ResumeDrawing(gridMargin);

            if (itemList.Count > 0)
            // Update MarginTable
            {
                UpdateMarginTable(itemList.First().Value, itemList.Last().Value);
            }

            if (marginSeriesList.Count > 0)
            // Update MarginChart
            {
                marginSeriesList.ForEach(f =>
                {
                    f.ChartType = SeriesChartType.Line;
                    chartMargin.Series.Add(f);
                });
                double minChartYUm = 50;
                double marginMinY = marginSeriesList.FindAll(f => f.Points.Count > 0).Min(f => f.Points.Min(g => g.YValues.Min()));
                double marginMaxY = marginSeriesList.FindAll(f => f.Points.Count > 0).Max(f => f.Points.Max(g => g.YValues.Max()));
                if (marginMaxY - marginMinY < minChartYUm)
                {
                    double mean = (marginMaxY + marginMinY) / 2;
                    marginMinY = mean - (minChartYUm / 2);
                    marginMaxY = mean + (minChartYUm / 2);
                }
                SetChartAxisYRange(chartMargin.ChartAreas[0].AxisY, marginMinY, marginMaxY, true);
                SetChartAxisXRange(chartMargin.ChartAreas[0].AxisX, this.overallSheetResultList.Min(f => f.Index), this.overallSheetResultList.Max(f => f.Index));
            }
        }

        private void UpdateMarginTable(MarginPanelItem._Item value1, MarginPanelItem._Item value2)
        {
            MarginPanelItem._Item firstItem = value1;
            txtFirstPatternNo.Text = firstItem.PatternNo.ToString("D1");
            txtFirstW0.Text = firstItem.MarginSizeUm0.Width.ToString("F01");
            txtFirstH0.Text = firstItem.MarginSizeUm0.Height.ToString("F01");
            txtFirstW1.Text = firstItem.MarginSizeUm1.Width.ToString("F01");
            txtFirstH1.Text = firstItem.MarginSizeUm1.Height.ToString("F01");
            txtFirstW2.Text = firstItem.MarginSizeUm2.Width.ToString("F01");
            txtFirstH2.Text = firstItem.MarginSizeUm2.Height.ToString("F01");
            txtFirstW3.Text = firstItem.MarginSizeUm3.Width.ToString("F01");
            txtFirstH3.Text = firstItem.MarginSizeUm3.Height.ToString("F01");
            txtFirstW4.Text = firstItem.MarginSizeUm4.Width.ToString("F01");
            txtFirstH4.Text = firstItem.MarginSizeUm4.Height.ToString("F01");

            txtFirstWMin.Text = firstItem.Min.Width.ToString("F01");
            txtFirstHMin.Text = firstItem.Min.Height.ToString("F01");
            txtFirstWMax.Text = firstItem.Max.Width.ToString("F01");
            txtFirstHMax.Text = firstItem.Max.Height.ToString("F01");
            txtFirstWMean.Text = firstItem.Mean.Width.ToString("F02");
            txtFirstHMean.Text = firstItem.Mean.Height.ToString("F02");


            MarginPanelItem._Item lastItem = value2;
            txtLastPatternNo.Text = lastItem.PatternNo.ToString("D1");
            txtLastW0.Text = lastItem.MarginSizeUm0.Width.ToString("F01");
            txtLastH0.Text = lastItem.MarginSizeUm0.Height.ToString("F01");
            txtLastW1.Text = lastItem.MarginSizeUm1.Width.ToString("F01");
            txtLastH1.Text = lastItem.MarginSizeUm1.Height.ToString("F01");
            txtLastW2.Text = lastItem.MarginSizeUm2.Width.ToString("F01");
            txtLastH2.Text = lastItem.MarginSizeUm2.Height.ToString("F01");
            txtLastW3.Text = lastItem.MarginSizeUm3.Width.ToString("F01");
            txtLastH3.Text = lastItem.MarginSizeUm3.Height.ToString("F01");
            txtLastW4.Text = lastItem.MarginSizeUm4.Width.ToString("F01");
            txtLastH4.Text = lastItem.MarginSizeUm4.Height.ToString("F01");

            txtLastWMin.Text = lastItem.Min.Width.ToString("F01");
            txtLastHMin.Text = lastItem.Min.Height.ToString("F01");
            txtLastWMax.Text = lastItem.Max.Width.ToString("F01");
            txtLastHMax.Text = lastItem.Max.Height.ToString("F01");
            txtLastWMean.Text = lastItem.Mean.Width.ToString("F02");
            txtLastHMean.Text = lastItem.Mean.Height.ToString("F02");
        }

        private void SetChartAxisYRange(Axis axis, double min, double max, bool decimalPlace)
        {
            axis.Minimum = min;
            axis.Maximum = max;
            axis.IntervalAutoMode = IntervalAutoMode.FixedCount;
            int dec = 0;
            if (decimalPlace)
            {
                double diff = max - min;
                while (diff < 10)
                {
                    dec++;
                    diff *= 10;
                    if (diff == 0)
                        break;
                }
            }
            string dd = string.Format("{{F0{0}}}", dec);
            axis.LabelStyle.Format = dd;
        }

        private void UpdateChart(RepeatedDefectItem repeatedDefectItem)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MultiSheetChartUpdateDelegate(UpdateChart), repeatedDefectItem);
                return;
            }

            ClearChartDataDefects();

            // Selected Defect Trand
            AddChartData(repeatedDefectItem.RepeatedDefectElementList);

            for (int i = 0; i < this.defectSerieses.Length; i++)
                this.defectSerieses[i].ChartType = SeriesChartType.Line;

            this.chartDefect.ChartAreas[0].AxisX.MajorGrid.Interval = 10;
            this.chartDefect.ChartAreas[0].AxisX.MinorGrid.Interval = 5;
            this.chartDefect.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
        }

        private void ClearChartData()
        {
            ClearChartDataDefects();
            ClearChartDataLength();
        }

        private void ClearChartDataDefects()
        {
            Array.ForEach(defectSerieses, f => f.Points.Clear());
        }

        private void ClearChartDataLength()
        {
            seriesHeight.Points.Clear();
            heightSlope = double.NaN;

        }
        private void ClearChartDataMargin()
        {
            marginSeriesList.ForEach(f => f.Points.Clear());
            marginSeriesList.Clear();
            this.gridMargin.Rows.Clear();
            while (this.gridMargin.Columns.Count > 1)
                this.gridMargin.Columns.RemoveAt(1);
        }

        private void AddChartData(int sheetNo, List<FoundedObjInPattern> sheetSubResultList, float sheetHeight)
        {
            int noprint = 0, pinhole = 0, spread = 0, sheetAttack = 0, dielectric = 0, sticker = 0;
            sheetSubResultList.ForEach(f =>
            {
                DefectType defectType = f.GetDefectType();
                switch (defectType)
                {
                    case DefectType.Noprint:
                        noprint++;
                        break;
                    case DefectType.PinHole:
                        pinhole++;
                        break;
                    case DefectType.Spread:
                        spread++;
                        break;
                    case DefectType.Attack:
                        sheetAttack++;
                        break;
                    case DefectType.Coating:
                        dielectric++;
                        break;
                    case DefectType.Sticker:
                        sticker++;
                        break;
                }
            });
            this.seriesNoprint.Points.AddXY(sheetNo, noprint);
            this.seriesPinhole.Points.AddXY(sheetNo, pinhole);
            this.seriesSpread.Points.AddXY(sheetNo, spread);
            this.seriesSheetAttack.Points.AddXY(sheetNo, sheetAttack);
            this.seriesDielectric.Points.AddXY(sheetNo, dielectric);
            this.seriesSticker.Points.AddXY(sheetNo, sticker);

            this.seriesHeight.Points.AddXY(sheetNo, sheetHeight);
            //UpdateHeightSlope();
        }

        private void UpdateHeightSlope()
        {
            if (this.seriesHeight.Points.Count < 10)
            {
                this.heightSlope = double.NaN;
                return;
            }

            DataPoint[] dps = this.seriesHeight.Points.ToArray();
            IEnumerable<PointF> points = dps.Select(f => new PointF((float)f.XValue, (float)(f.YValues.First() * 1000)));
            MathHelper.Regression1(points, out double coff1, out double coff0);
            this.heightSlope = (float)coff1;
        }

        private void AddChartData(List<FoundedObjInPattern> sheetSubResultList)
        {
            int xMax = selectedSheetList.Count;
            int yMin = 0;
            int yMax = 0;
            for (int i = 0; i < sheetSubResultList.Count; i++)
            {
                int y = sheetSubResultList[i] != null ? 1 : 0;
                seriesSelection.Points.AddXY(xMax - i, y);
                //yMin = Math.Min(yMin, y);
                yMax = Math.Max(yMax, y);
            }

            int xMin = 1;
            if (yMax - yMin < 5)
                yMax = yMin + 5;
            SetChartAxisXRange(this.chartDefect.ChartAreas[0].AxisX, xMin, xMax);
            SetChartAxisXRange(this.chartLength.ChartAreas[0].AxisX, xMin, xMax);
            SetChartAxisYRange(this.chartDefect.ChartAreas[0].AxisY, yMin, yMax, false);
        }

        public List<FoundedObjInPattern> GetDefectList()
        {
            List<FoundedObjInPattern> imageList = new List<FoundedObjInPattern>();
            foreach (DataGridViewRow row in this.defectList.Rows)
            {
                if (row.Tag is FoundedObjInPattern)
                {
                    FoundedObjInPattern ssr = row.Tag as FoundedObjInPattern;
                    imageList.Add(ssr);
                }
                else if (row.Tag is RepeatedDefectItem)
                {
                    RepeatedDefectItem rdi = row.Tag as RepeatedDefectItem;
                    //SheetSubResult ssr = rdi.SheetSubResultList.Find(f => f.Image != null);
                    //imageList.Add(ssr);
                }
            }

            return imageList;
        }

        private void SetChartAxisXRange(Axis axis, int min, int max)
        {
            double curMax = axis.Maximum;
            double curMin = axis.Minimum;

            if (curMax < min)
            {
                axis.Maximum = max;
                axis.Minimum = min;
                axis.Interval = Math.Max(1, (int)(axis.Maximum - axis.Minimum) / 5);
            }
            else //if(curMin < max)
            {
                axis.Minimum = min;
                axis.Maximum = max;
                axis.Interval = Math.Max(1, (int)(axis.Maximum - axis.Minimum) / 5);
            }

            //chartDefect.ChartAreas[0].AxisX.Interval =
            //    Math.Max(1, (int)(chartDefect.ChartAreas[0].AxisX.Maximum - chartDefect.ChartAreas[0].AxisX.Minimum) / 5);
        }


        private void UpdateImageBitmap(Bitmap bitmap)
        {
            RectangleF rectangleF = imageCanvasPanel.ViewPort;
            imageCanvasPanel.UpdateImage(bitmap);
            //imageCanvasPanel.ZoomFit();
        }

        public void ZoomFit()
        {
            this.imageCanvasPanel.ZoomFit();
            this.transformImageControl.ZoomFit();
            this.transformChartControl.ZoomFit();
        }

        private void UpdateImageFigure(List<RepeatedDefectItem> itemList)
        {
            this.imageCanvasPanel.WorkingFigures.Clear();

            if (itemList == null || itemList.Count == 0)
                itemList = this.repeatedDefectItemList.List;

            itemList.ForEach(f => AddFigure(f));

            //if (itemList == null || itemList.Count == 0)
            //{
            //    for (int i = 0; i < repeatedDefectItemList.Count; i++)
            //        AddFigure(repeatedDefectItemList[i]);
            //}
            //else
            //{
            //    itemList.ForEach(f => AddFigure(f));
            //}
            this.imageCanvasPanel.Invalidate();
        }

        private void AddFigure(RepeatedDefectItem item)
        {
            //if (item.RepeatRatio > 01)
            {
                float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;

                Figure figure;
                if (item.DefectType == DefectType.Transform && item.ValidItemCount > 1)
                    figure = OffsetObj.GetFigure(resizeRatio, item.RepeatedDefectElementList.FindAll(f => f != null));
                else
                    figure = item.RepeatedDefectElementList.First(f => f != null).GetFigure(resizeRatio);

                if (figure != null)
                    this.imageCanvasPanel.WorkingFigures.AddFigure(figure);
            }
        }

        private void SelectDefect(List<FoundedObjInPattern> sheetSubResultList)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            imageCanvasPanel.WorkingFigures.Clear();
            foreach (FoundedObjInPattern sheetSubResult in sheetSubResultList)
            {
                if (sheetSubResult != null)
                    imageCanvasPanel.WorkingFigures.AddFigure(sheetSubResult.GetFigure(resizeRatio));
            }
            imageCanvasPanel.Invalidate();

            FoundedObjInPattern firstSheetSubResult = null;
            if (sheetSubResultList.Count == 1)
            {
                firstSheetSubResult = sheetSubResultList.FirstOrDefault(f => f.Image != null) as FoundedObjInPattern;
                if (firstSheetSubResult == null)
                    firstSheetSubResult = sheetSubResultList.FirstOrDefault();
            }
            UpdateDefectImage(firstSheetSubResult);
        }

        private void UpdateDefectImage(FoundedObjInPattern firstSheetSubResult)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            Bitmap bitmap = null;
            Bitmap bitmapB = null;
            string sizeW = "", sizeH = "", sizeV = "", sizeL = "";

            if (firstSheetSubResult != null)
            {
                bitmap = firstSheetSubResult?.Image;
                bitmapB = firstSheetSubResult?.BufImage;

                if (firstSheetSubResult is Data.MarginObj)
                {
                    Data.MarginObj marginObj = (Data.MarginObj)firstSheetSubResult;
                    sizeW = $"{marginObj.DiffMarginSizeUm.Width:F1}";
                    sizeH = $"{marginObj.DiffMarginSizeUm.Height:F1}";
                }
                else
                {
                    sizeW = string.Format("{0:F1}", firstSheetSubResult.RealWidth);
                    sizeH = string.Format("{0:F1}", firstSheetSubResult.RealHeight);
                    sizeL = string.Format("{0:F2}", firstSheetSubResult.RealLength);
                    if (firstSheetSubResult is Data.DefectObj)
                        sizeV = string.Format("{0}", ((Data.DefectObj)firstSheetSubResult).SubtractValueMax);
                    else if (firstSheetSubResult is Data.OffsetObj)
                        sizeV = string.Format("{0}", ((Data.OffsetObj)firstSheetSubResult).Score);
                }
                //if (firstSheetSubResult.GetDefectType() == DefectType.Transform)
                //{
                //    RectangleF zoomRect = DrawingHelper.Mul(firstSheetSubResult.Region, resizeRatio);
                //    zoomRect.Inflate(10, 10);
                //    this.imageCanvasPanel.ZoomRange(zoomRect);
                //}
            }

            showBufferDefect = false;
            defectImage.Image = bitmap;
            defectImage.Tag = firstSheetSubResult;
            defectSizeW.Text = sizeW;
            defectSizeH.Text = sizeH;
            defectSizeL.Text = sizeL;
            defectSizeV.Text = sizeV;

            if (firstSheetSubResult != null)
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(defectImage, Path.GetFileName(firstSheetSubResult.ImagePath));
                tt.ShowAlways = true;
            }
        }

        private void SelectDefect(List<RepeatedDefectItem> itemList)
        {
            onUpdate = true;

            //Bitmap bitmap = null, bitmapB = null;
            //string sizeW = "", sizeH = "", sizeL = "", sizeV = "";

            //ClearChartData();
            if (itemList.Count == 1)
            {
                DefectObj lastResult = itemList[0].RepeatedDefectElementList.First(f => f != null) as DefectObj;
                if (lastResult != null)
                {
                    UpdateDefectImage(lastResult);
                    //bitmap = lastResult.Image;
                    //bitmapB = lastResult.BufImage;

                    //sizeW = string.Format("{0:F1}", lastResult.RealRegion.Width);
                    //sizeH = string.Format("{0:F1}", lastResult.RealRegion.Height);
                    //sizeL = string.Format("{0:F2}", lastResult.RealLength);
                    //sizeV = string.Format("{0}", lastResult.SubtractValueMax);
                }
                UpdateChart(itemList[0]);
            }

            UpdateImageFigure(itemList);

            //defectImage.Image = bitmap;
            //defectImage.Tag = bitmapB;
            //defectSizeW.Text = sizeW;
            //defectSizeH.Text = sizeH;
            //defectSizeL.Text = sizeL;
            //defectSizeV.Text = sizeV;

            onUpdate = false;
        }

        private void SelectDefect()
        {
            onUpdate = true;

            //if (defectList.SelectedRows.Count == 0)
            //{
            //    UpdateDefectImage(null);
            //    return;
            //}

            List<FoundedObjInPattern> itemList = new List<FoundedObjInPattern>();
            List<RepeatedDefectItem> itemList2 = new List<RepeatedDefectItem>();
            for (int i = 0; i < defectList.SelectedRows.Count; i++)
            {
                if (defectList.SelectedRows[i].Tag is FoundedObjInPattern)
                    itemList.Add((FoundedObjInPattern)defectList.SelectedRows[i].Tag);
                else
                    itemList2.Add((RepeatedDefectItem)defectList.SelectedRows[i].Tag);
            }
            itemList.RemoveAll(f => f == null);
            itemList2.RemoveAll(f => f == null);
            if (itemList.Count == 0 || itemList2.Count == 0)
            {
                UpdateImageFigure(null);
            }
            else
            {
                if (itemList.Count >= 0)
                    SelectDefect(itemList);
                else
                    SelectDefect(itemList2);
            }

            //if (this.selectedSheetList.Count == 1)
            //{
            //    List<SheetSubResult> itemList = new List<SheetSubResult>();
            //    for (int i = 0; i < defectList.SelectedRows.Count; i++)
            //        itemList.Add(defectList.SelectedRows[i].Tag as SheetSubResult);
            //    itemList.RemoveAll(f => f == null);
            //    SelectDefect(itemList);
            //}
            //else
            //{
            //    List<RepeatedDefectItem> itemList = new List<RepeatedDefectItem>();
            //    for (int i = 0; i < defectList.SelectedRows.Count; i++)
            //        itemList.Add(defectList.SelectedRows[i].Tag as RepeatedDefectItem);
            //    itemList.RemoveAll(f => f == null);
            //    SelectDefect(itemList);
            //}

            onUpdate = false;
        }
        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (defectList.SelectedRows.Count == 0)
            {
                SelectDefect();
                return;
            }

            if (this.selectedSheetList.Count == 1)
            {
                List<FoundedObjInPattern> itemList = new List<FoundedObjInPattern>();
                for (int i = 0; i < defectList.SelectedRows.Count; i++)
                    itemList.Add(defectList.SelectedRows[i].Tag as FoundedObjInPattern);
                itemList.RemoveAll(f => f == null);
                SelectDefect(itemList);
            }
            else
            {
                List<RepeatedDefectItem> itemList = new List<RepeatedDefectItem>();
                for (int i = 0; i < defectList.SelectedRows.Count; i++)
                    itemList.Add(defectList.SelectedRows[i].Tag as RepeatedDefectItem);
                itemList.RemoveAll(f => f == null);
                SelectDefect(itemList);
            }
        }

        private void check_CheckedChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            for (int i = 0; i < defectCheckBoxs.Length; i++)
                EnableSeries(i, this.defectCheckBoxs[i].Checked);
        }

        private void EnableSeries(int i, bool visible)
        {
            Series series = this.defectSerieses[i];
            if (visible)
            {
                if (this.chartDefect.Series.Contains(series) == false)
                    this.chartDefect.Series.Add(series);
            }
            else
            {
                if (this.chartDefect.Series.Contains(series))
                    this.chartDefect.Series.Remove(series);
            }
        }

        private void MultiSheetResultPanel_Load(object sender, EventArgs e)
        {
            checkNoprint.Checked = seriesNoprint.Enabled;
            checkPinhole.Checked = seriesPinhole.Enabled;
            checkSpread.Checked = seriesSpread.Enabled;
            checkSheetattack.Checked = seriesSheetAttack.Enabled;
            checkDielectric.Checked = seriesDielectric.Enabled;
            checkSticker.Checked = seriesSticker.Enabled;

            UpdateControlsColor();
        }

        private void UpdateControlsColor()
        {
            Gravure.Data.ColorTable.UpdateControlColor(checkNoprint, DefectType.Noprint);
            Gravure.Data.ColorTable.UpdateControlColor(checkPinhole, DefectType.PinHole);
            Gravure.Data.ColorTable.UpdateControlColor(checkSpread, DefectType.Spread);
            Gravure.Data.ColorTable.UpdateControlColor(checkSheetattack, DefectType.Attack);
            Gravure.Data.ColorTable.UpdateControlColor(checkDielectric, DefectType.Coating);
            Gravure.Data.ColorTable.UpdateControlColor(checkSticker, DefectType.Sticker);
        }

        private void defectImage_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = defectImage.Image as Bitmap;
            MouseEventArgs mouseEventArgs = e as MouseEventArgs;

            FoundedObjInPattern tag = defectImage.Tag as FoundedObjInPattern;
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                if (tag != null)
                {
                    if (!this.showBufferDefect)
                    {
                        if (tag.BufImage != null)
                        {
                            defectImage.Image = tag.BufImage;
                            this.showBufferDefect = true;
                        }
                    }
                    else
                    {
                        defectImage.Image = tag.Image;
                        this.showBufferDefect = false;
                    }
                }
            }
            else if (mouseEventArgs.Button == MouseButtons.Right)
            {
                string path = !this.showBufferDefect ? tag.ImagePath : tag.BufImagePath;
                if (File.Exists(path))
                    Process.Start(path);
            }
        }

        private void imagePanel_Resize(object sender, EventArgs e)
        {
            this.imageCanvasPanel.ZoomFit();
        }

        private void MultiSheetResultPanel_VisibleChanged(object sender, EventArgs e)
        {
            //this.layoutDDensity.Visible = AdditionalSettings.Instance().StripeBlotAlarmSetting.Use;
        }

        private void chartDensity_MouseMove(object sender, MouseEventArgs e)
        {
            this.imageCanvasPanel.BackgroundFigures.Clear();

            var hitTest = chartDensity.HitTest(e.X, e.Y);
            Series series = hitTest.Series;
            if (series != null)
            {
                int pointIndex = hitTest.PointIndex;
                int dataPointLength = series.Points.Count();
                DataPoint dataPoint = series.Points[pointIndex];
                int xValue = (int)dataPoint.XValue;

                if (this.imageCanvasPanel.Image != null)
                {
                    Size imageSize = this.imageCanvasPanel.Image.Size;
                    float wStep = imageSize.Width * 1.0f / dataPointLength;
                    float wWidth = imageSize.Width * 1.0f / ((dataPointLength + 1) / 2);
                    RectangleF highlightRect = new RectangleF(xValue * wStep, 0, wWidth, imageSize.Height);
                    RectangleFigure highlightFigure = new RectangleFigure(highlightRect, new Pen(Color.Red, 2));

                    this.imageCanvasPanel.BackgroundFigures.AddFigure(highlightFigure);
                }
            }
            this.imageCanvasPanel.Refresh();
            
        }

        private void chartLength_Paint(object sender, PaintEventArgs e)
        {
            if (double.IsNaN(this.heightSlope) || double.IsInfinity(this.heightSlope))
                return;
            Font font = chartLength.ChartAreas[0].AxisX.TitleFont;
            int count = 1000;
            e.Graphics.DrawString($"{this.heightSlope * count:F0}[um] per {count} patterns", font, new SolidBrush(Color.Black), new Point(0, this.chartLength.Height - 20));
        }
    }
}
