using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.Vision.Planbss;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Data.Forms;
using UniEye.Base;
using DynMvp.Authentication;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.Vision;
using UniScanG.Gravure.Vision.Trainer;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Gravure.Vision;
using DynMvp.Devices;
//using UniEye.Base.Settings;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using Infragistics.Win.UltraWinTabControl;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Settings;
using System.Linq;
using UniEye.Base.Settings;
using UniScanG.Data.Inspect;
using System.IO;
using UniScanG.Gravure.Vision.RCI.Calculator;
using UniScanG.Gravure.Vision.RCI;
using UniScanG.Gravure.Vision.RCI.Trainer;
using static UniScanG.Gravure.Vision.RCI.RCIReconOptions;

namespace UniScanG.Gravure.UI.Teach.Inspector
{

    public partial class ParamControllerRCI : UserControl, IModellerControl, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        public enum ParamTabKey { Fiducial, Train, Inspect, Misc }

        private Calibration calibration;
        private CanvasPanel canvasPanelReconImage;
        private CanvasPanel canvasPanelWeigthImage;

        private UniScanG.Data.Model.Model Model => SystemManager.Instance().CurrentModel;

        private SheetFinderBaseParam FiducialFinderParam => SheetFinderBase.SheetFinderBaseParam;

        private TrainerParam TrainerParam => TrainerBase.TrainerParam;

        private CalculatorParam CalculatorParam => CalculatorBase.CalculatorParam;

        private DetectorParam DetectorParam => Detector.DetectorParam;

        private WatcherParam WatcherParam => Watcher.WatcherParam;


        private bool OnUpdateData { get; set; }

        ModellerPageExtenderG modellerPageExtender;
        Size imageSize = Size.Empty;

        public ParamControllerRCI()
        {
            InitializeComponent();

            this.canvasPanelReconImage = new CanvasPanel(true) { Dock = DockStyle.Fill };
            this.canvasPanelReconImage.SetPanMode();
            this.layoutTeachReference.Controls.Add(this.canvasPanelReconImage, 0, 1);

            this.canvasPanelWeigthImage = new CanvasPanel(true) { Dock = DockStyle.Fill };
            this.canvasPanelWeigthImage.SetPanMode();
            this.layoutTeachReference.Controls.Add(this.canvasPanelWeigthImage, 1, 1);

            StringManager.AddListener(this);

            OnUpdateData = true;

            this.calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (this.calibration == null)
                this.calibration = new ScaledCalibration(new SizeF(14, 14));

            UiHelper.SetNumericMinMax(this.valueDielectricMinSize, 0, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.valueElectricMinSize, 0, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.progressSpeed, 0, decimal.MaxValue);

            UiHelper.SetNumericMinMax(this.valueSensitivityHigh, 0, byte.MaxValue);
            UiHelper.SetNumericMinMax(this.valueSensitivityLow, 0, byte.MaxValue);
            UiHelper.SetNumericMinMax(this.valueInnerCorrectionCount, 0, byte.MaxValue);


            UiHelper.SetNumericMinMax(this.valueDefectMaxCount, -1, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.valueDetectTimeout, -1, decimal.MaxValue);

            UiHelper.SetNumericMinMax(this.valueMarginMeasurement, 0, decimal.MaxValue);

            UiHelper.SetNumericMinMax(this.sizeVariationSizeW, (decimal)0.001, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.sizeVariationSizeH, (decimal)0.001, decimal.MaxValue);

            UiHelper.SetNumericMinMax(this.chipShare, 0, 100);

            UiHelper.SetNumericMinMax(this.valueGapLength, 0, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.valuePTMPeriodW, 0, decimal.MaxValue);
            UiHelper.SetNumericMinMax(this.valuePTMPeriodH, 0, decimal.MaxValue);


            string ver = VersionHelper.Instance().VersionString;
            string bld = VersionHelper.Instance().BuildString;
            string flg = DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag;
            if (string.IsNullOrEmpty(flg))
                this.versionBuild.Text = $"V{ver} / B{bld}";
            else
                this.versionBuild.Text = $"V{ver} / B{bld} / {flg}";

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
            OnUpdateData = false;
        }

        private void ParamControllerRCI_Load(object sender, EventArgs e)
        {
#if !DEBUG
            this.progressSpeed.Enabled = false;
#endif
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender as ModellerPageExtenderG;

            this.modellerPageExtender.ImageUpdated = modellerPageExtender_ImageUpdated;
            this.modellerPageExtender.OnLineSpeedUpdated += modellerPageExtender_LineSpeedUpdated;
            this.modellerPageExtender.ModelImageLoadDone += ModellerPageExtender_ModelImageLoadDone;
            this.modellerPageExtender.OnImageControllerViewPortChanged += ModellerPageExtender_OnImageControllerViewPortChanged;

        }

        private void ModellerPageExtender_OnImageControllerViewPortChanged(CanvasPanel canvasPanel)
        {
            if (this.Model == null)
                return;

            RectangleF viewPort = canvasPanel.ViewPort;
            PointF viewPortCenter = DrawingHelper.CenterPoint(viewPort);
            PointF offset = DrawingHelper.Div(this.Model.RCITrainResult.ROI.Location, -2);
            {
                float sX = this.canvasPanelReconImage.Width * 1f / canvasPanel.Width;
                float sY = this.canvasPanelReconImage.Height * 1f / canvasPanel.Height;
                SizeF viewPortSize = DrawingHelper.Mul(viewPort.Size, Math.Max(sX, sY));
                RectangleF rectF = DrawingHelper.FromCenterSize(viewPortCenter, viewPortSize);
                rectF.Offset(offset);
                this.canvasPanelReconImage.ZoomRange(rectF);
            }
            {
                float sX = this.canvasPanelReconImage.Width * 1f / canvasPanel.Width;
                float sY = this.canvasPanelReconImage.Height * 1f / canvasPanel.Height;
                SizeF viewPortSize = DrawingHelper.Mul(viewPort.Size, Math.Max(sX, sY));
                RectangleF rectF = DrawingHelper.FromCenterSize(viewPortCenter, viewPortSize);
                rectF.Offset(offset);
                this.canvasPanelWeigthImage.ZoomRange(rectF);
            }
        }

        private void modellerPageExtender_LineSpeedUpdated()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OnLineSpeedUpdatedDelegate(modellerPageExtender_LineSpeedUpdated));
                return;
            }

            if (this.modellerPageExtender != null)
            {
                this.progressSpeed.BackColor = this.modellerPageExtender.LineStartState ? Color.LightGreen : SystemColors.Window;
                UiHelper.SetNumericValue(this.progressSpeed, (decimal)this.modellerPageExtender.LineSpeedMpm);
            }
        }

        private void ModellerPageExtender_ModelImageLoadDone()
        {
            UpdateTeachReference();
        }

        private void modellerPageExtender_ImageUpdated(Size imageSize)
        {
            this.imageSize = imageSize;
        }


        delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            this.OnUpdateData = true;
            if (SystemManager.Instance().CurrentModel == null)
            {
                tabControlParam.Tabs[0].Visible = false;
                tabControlParam.Tabs[1].Visible = false;
            }
            else
            {
                tabControlParam.Tabs[0].Visible = true;
                tabControlParam.Tabs[1].Visible = true;

                UpdateTeachReference();
                UpdateTeachMonitoring();

                UpdateParamControl();
            }
            UpdateMiscParamControl();
            this.OnUpdateData = false;
        }

        private void UpdateOverlayFigure()
        {
            DataGridView dataGridView = null;
            UltraTab selectedTab = tabControlTeach.SelectedTab;
            if (selectedTab != null)
            {
                ModellerPageExtenderG.FigureType figureType = ModellerPageExtenderG.FigureType.Chip;
                switch (selectedTab.Key)
                {
                    case "Reference":
                        figureType = ModellerPageExtenderG.FigureType.RCI;
                        dataGridView = null;
                        break;

                    case "Monitoring":
                        // 모니터링 영역 그리기
                        figureType = ModellerPageExtenderG.FigureType.Extend;
                        dataGridView = this.monitoringSelector;
                        break;
                }

                int[] selectedIndexes = null;
                if (dataGridView != null)
                {
                    int selectedCount = dataGridView.SelectedRows.Count;
                    selectedIndexes = new int[selectedCount];
                    for (int i = 0; i < selectedCount; i++)
                        selectedIndexes[i] = dataGridView.SelectedRows[i].Index;
                }

                this.modellerPageExtender.UpdateOverlayFigure(figureType, selectedIndexes);
            }
        }

        #region UpdateUI
        private void UpdateParamControl()
        {
            this.OnUpdateData = true;

            RCIOptions options = Model.RCIOptions;
            RCIGlobalOptions gOptions = AlgorithmSetting.Instance().RCIGlobalOptions;

            // Train
            UiHelper.SetNumericValue(valueGapLength, options.PatternGapLengthMm);

            UiHelper.SetNumericValue(valuePTMPeriodW, options.PTM_InflateUm.Width);
            UiHelper.SetNumericValue(valuePTMPeriodH, options.PTM_InflateUm.Height);

            UiHelper.SetCheckBoxChecked(useSkipRowHead, options.SkipHeadRow);
            UiHelper.SetCheckBoxChecked(useSkipRowTail, options.SkipTailRow);

            UiHelper.SetComboBoxSelectedIndex(this.cmbReconstruct, (int)options.ReconstructOptions.Reconstruct);
            UiHelper.SetNumericValue(valueEdgeSmooth, options.ReconstructOptions.EdgeSmoothCount);
            UiHelper.SetNumericValue(valueEdgeValue, options.ReconstructOptions.EdgeValue);
            UiHelper.SetCheckBoxChecked(useRightToLeft, gOptions.RightToLeft);

            if (WatcherParam != null)
            {
                // SizeVariation
                TransformParam transformParam = WatcherParam.ModelParam.TransformCollection.Param;
                this.layoutSizeVariation.Visible = transformParam.Available;

                UiHelper.SetNumericValue(this.sizeVariationScore, (decimal)transformParam.MatchingScore);
                UiHelper.SetNumericValue(this.sizeVariationCountW, (decimal)transformParam.Count.Width);
                UiHelper.SetNumericValue(this.sizeVariationCountH, (decimal)transformParam.Count.Height);
                UiHelper.SetNumericValue(this.sizeVariationSizeW, (decimal)(transformParam.SizeUm.Width / 1000));
                UiHelper.SetNumericValue(this.sizeVariationSizeH, (decimal)(transformParam.SizeUm.Height / 1000));
            }

            // Inspect
            UiHelper.SetNumericValue(this.valueUniformalizeTarget, gOptions.UniformizeGv);

            UiHelper.SetNumericValue(this.valueSensitivityHigh, options.SensitiveOption.High);
            UiHelper.SetNumericValue(this.valueSensitivityLow, options.SensitiveOption.Low);
            UiHelper.SetNumericValue(this.valueInnerCorrectionCount, options.PTMCorrectionCount);
            UiHelper.SetCheckBoxChecked(this.useSplitX, options.SplitX);

            UiHelper.SetCheckBoxChecked(this.useMultiThread, gOptions.Parall);

            UiHelper.SetNumericValue(this.valueDefectMaxCount, DetectorParam.MaximumDefectCount);
            UiHelper.SetNumericValue(this.valueDetectTimeout, DetectorParam.TimeoutMs);

            UiHelper.SetCheckBoxChecked(this.useFineSizeMeasure, DetectorParam.FineSizeMeasure);
            UiHelper.SetNumericValue(this.valueFineSizeMeasureSizeMul, DetectorParam.FineSizeMeasureSizeMul);
            UiHelper.SetNumericValue(this.valueFineSizeMeasureThresholdMul, DetectorParam.FineSizeMeasureThresholdMul);

            UiHelper.SetCheckBoxChecked(this.useDetectorSpreadTracing, DetectorParam.UseSpreadTrace);
            UiHelper.SetCheckBoxChecked(this.useMergingDefects, DetectorParam.MergingDefects);

            UiHelper.SetControlVisible(this.layoutInspectSticker, AlgorithmSetting.Instance().UseExtSticker);
            UiHelper.SetCheckBoxChecked(this.useSticker, gOptions.StickerOption.Use);
            UiHelper.SetNumericValue(this.valueStickerDiffHigh, gOptions.StickerOption.High);
            UiHelper.SetNumericValue(this.valueStickerDiffLow, gOptions.StickerOption.Low);

            UiHelper.SetNumericValue(this.valueElectricMinSize, DetectorParam.MinBlackDefectLength);
            UiHelper.SetNumericValue(this.valueDielectricMinSize, DetectorParam.MinWhiteDefectLength);
            UiHelper.SetComboBoxSelectedIndex(this.cmbCriterionLength, (int)DetectorParam.CriterionLength);

            // Extention
            WatcherParam watcherParam = WatcherParam;
            if (watcherParam != null)
            {
                TransformParam transformParam = watcherParam.ModelParam.TransformCollection.Param;

                UiHelper.SetControlVisible(layoutSizeVariation, transformParam.Available);

                UiHelper.SetNumericValue(sizeVariationScore, transformParam.MatchingScore);
                UiHelper.SetNumericValue(sizeVariationCountW, transformParam.Count.Width);
                UiHelper.SetNumericValue(sizeVariationCountH, transformParam.Count.Height);
                UiHelper.SetNumericValue(sizeVariationSizeW, transformParam.SizeUm.Width / 1000);
                UiHelper.SetNumericValue(sizeVariationSizeH, transformParam.SizeUm.Height / 1000);
            }

            this.OnUpdateData = false;
        }

        private void UpdateTeachReference()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(UpdateTeachReference));
                return;
            }

            this.OnUpdateData = true;

            Action<CanvasPanel, ImageD> UpdateCanvasPanelAction = new Action<CanvasPanel, ImageD>((f, g) =>
            {
                if (g == null)
                {
                    f.UpdateImage(null);
                    return;
                }

                using (AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, g, ImageType.Grey))
                {
                    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                    using (AlgoImage scAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, DrawingHelper.Div(g.Size, 2)))
                    {
                        ip.Resize(algoImage, scAlgoImage);
                        f.UpdateImage(scAlgoImage.ToBitmap(), true);
                    }
                }
            });

            UpdateCanvasPanelAction(this.canvasPanelReconImage, this.Model.RCITrainResult.ReconImageD);
            UpdateCanvasPanelAction(this.canvasPanelWeigthImage, this.Model.RCITrainResult.WeightImageD);

            UiHelper.SuspendDrawing(this.chartProfile);
            this.chartProfile.Series[0].Points.Clear();
            this.chartProfile.Series[1].Points.Clear();

            double[] coffs = this.Model.RCITrainResult.UniformizeCoefficients;
            if (coffs != null && coffs.Length == 3)
            {
                int dataLength = this.Model.RCITrainResult.UniformizeLength;
                int darwStep = 512;
                for (int x = 0; x < dataLength; x += darwStep)
                {
                    double y = (coffs[2] * x * x + coffs[1] * x + coffs[0]);
                    this.chartProfile.Series[1].Points.AddXY(x, y);
                    //xs[x] = x;
                    //ys[x] = (coffs[2] * x * x + coffs[1] * x + coffs[0]);
                }
                //this.chartProfile.Series[1].Points.AddXY(xs, ys);

                this.chartProfile.ChartAreas[0].AxisX.Minimum = 0;
                this.chartProfile.ChartAreas[0].AxisX.Maximum = dataLength;
                this.chartProfile.ChartAreas[0].AxisX.Interval = dataLength / 8;
                this.chartProfile.ChartAreas[0].AxisX.MajorGrid.Interval = dataLength / 4;
                this.chartProfile.ChartAreas[0].AxisX.MinorGrid.Interval = dataLength / 8;
                this.chartProfile.ChartAreas[0].AxisX.LabelStyle.Format = "D";
                this.chartProfile.ChartAreas[0].AxisX.LabelStyle.Interval = dataLength / 4 + 1;
            }
            UiHelper.ResumeDrawing(this.chartProfile);

            UiHelper.SetNumericValue(this.chipShare, this.Model.ChipShare100p);

            this.OnUpdateData = false;
        }

        private void UpdateTeachMonitoring()
        {
            this.OnUpdateData = true;

            monitoringSelector.Rows.Clear();
            monitoringCount.Text = "0";

            WatcherParam watcherParam = WatcherParam;
            if (watcherParam != null)
            {
                List<ExtItem> watchItemList = watcherParam.ModelParam.GetItems();

                Rectangle imageRect = Rectangle.Empty;
                Image2D curImage = this.modellerPageExtender?.CurrentImage;
                if (curImage != null)
                    imageRect = new Rectangle(Point.Empty, this.modellerPageExtender.CurrentImage.Size);

                foreach (ExtItem watchItem in watchItemList)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(this.monitoringSelector);

                    if (!watchItem.IsLicenseExist)
                    {
                        newRow.DefaultCellStyle.BackColor = Color.LightPink;
                        newRow.Cells[1].ToolTipText = "Not Available";
                    }

                    newRow.Cells[0].Value = watchItem.Index;
                    newRow.Cells[1].Value = watchItem.Use;
                    newRow.Cells[2].Value = watchItem.MasterImageD?.ToBitmap();
                    newRow.Cells[3].Value = string.Format("{0}{1}{2}", watchItem.ExtType, Environment.NewLine, watchItem.Name);

                    newRow.Height = 150;
                    newRow.Tag = watchItem;

                    monitoringSelector.Rows.Add(newRow);
                }
            }
            monitoringCount.Text = monitoringSelector.Rows.Count.ToString();

            this.OnUpdateData = false;
        }

        private void monitoringSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 1)
            {
                ExtItem watchItem = this.monitoringSelector.Rows[e.RowIndex].Tag as ExtItem;
                if (watchItem.IsLicenseExist)
                {
                    ((DataGridViewCheckBoxCell)this.monitoringSelector.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = watchItem.Use = !watchItem.Use;
                    SystemManager.Instance().CurrentModel.Modified = true;
                }
            }
            else if (e.ColumnIndex == 2)
            {
                PointF viewPortLoc = PointF.Empty;
                SizeF viewPortSize = SizeF.Empty;
                ExtItem watchItem = this.monitoringSelector.Rows[e.RowIndex].Tag as ExtItem;

                viewPortLoc = watchItem.ClipRectangleUm.Location;
                viewPortSize = watchItem.ClipRectangleUm.Size;


                RectangleF viewPortR = new RectangleF(viewPortLoc, viewPortSize);
                Rectangle viewPort = Rectangle.Round(DrawingHelper.Mul(viewPortR, this.imageSize));
                if (!viewPort.IsEmpty)
                    modellerPageExtender.UpdateZoom(viewPort);
            }
        }

        private void UpdateMiscParamControl()
        {
            this.OnUpdateData = true;

            UiHelper.SetNumericValue(bufferSize, AlgorithmSetting.Instance().InspBufferCount);
            UiHelper.SetNumericValue(monitoringPeriod, WatcherParam.MonitoringPeriod);

            if (this.calibration != null)
            {
                UiHelper.SetNumericValue(xPixelCal, (decimal)this.calibration.PelSize.Width);
                UiHelper.SetNumericValue(yPixelCal, (decimal)this.calibration.PelSize.Height);
            }

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = Settings.AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            asyncMode.CheckState = (CheckState)additionalSettings.AsyncMode;
            UiHelper.SetNumericValue(grabSpeedMpM, (decimal)additionalSettings.AsyncGrabMpm);
            grabSpeedMpM.Enabled = (additionalSettings.AsyncMode != EAsyncMode.False);

            if (this.calibration != null)
            {
                float spdHz = additionalSettings.ConvertMpm2Hz(-1);
                grabSpeedkHz.Text = (spdHz / 1000).ToString("0.00");
                grabSpeedkHz.Enabled = (additionalSettings.AsyncMode != EAsyncMode.False);
            }

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            this.imageRescaler.Enabled = (model != null);
            if (model != null)
                this.imageRescaler.Value = model.ScaleFactor;

            this.debugMode.Checked = UniEye.Base.Settings.OperationSettings.Instance().SaveDebugImage;
            this.fallowingThreshold.Checked = this.FiducialFinderParam.FallowingThreshold;

            modellerPageExtender_LineSpeedUpdated();

            this.OnUpdateData = false;
        }
        #endregion

        #region Apply Value

        private void paramTrain_ValueChanged(object sender, EventArgs e)
        {
            SetParamControl();
        }

        private void SetParamControl()
        {
            if (this.OnUpdateData)
                return;

            this.Model.Modified = true;
            RCIOptions options = Model.RCIOptions;
            RCIGlobalOptions gOptions = AlgorithmSetting.Instance().RCIGlobalOptions;

            // Train
            options.PatternGapLengthMm = (float)UiHelper.GetNumericValue(valueGapLength);

            options.PTM_InflateUm = new SizeF()
            {
                Width = (float)UiHelper.GetNumericValue(valuePTMPeriodW),
                Height = (float)UiHelper.GetNumericValue(valuePTMPeriodH),
            };

            options.SkipHeadRow = UiHelper.GetCheckBoxChecked(useSkipRowHead);
            options.SkipTailRow = UiHelper.GetCheckBoxChecked(useSkipRowTail);


            options.ReconstructOptions.Reconstruct = (EReconstruct)UiHelper.GetComboBoxSelectedIndex(this.cmbReconstruct);
            options.ReconstructOptions.EdgeSmoothCount = (int)UiHelper.GetNumericValue(valueEdgeSmooth);
            options.ReconstructOptions.EdgeValue = (int)UiHelper.GetNumericValue(valueEdgeValue);
            gOptions.RightToLeft = UiHelper.GetCheckBoxChecked(useRightToLeft);

            // Inspect
            gOptions.UniformizeGv = (byte)UiHelper.GetNumericValue(valueUniformalizeTarget);

            options.SensitiveOption.High = (byte)UiHelper.GetNumericValue(valueSensitivityHigh);
            options.SensitiveOption.Low = (byte)UiHelper.GetNumericValue(valueSensitivityLow);
            options.PTMCorrectionCount = (int)UiHelper.GetNumericValue(valueInnerCorrectionCount);
            options.SplitX = UiHelper.GetCheckBoxChecked(useSplitX);

            gOptions.Parall = UiHelper.GetCheckBoxChecked(useMultiThread);

            DetectorParam.MaximumDefectCount = (int)UiHelper.GetNumericValue(valueDefectMaxCount);
            DetectorParam.TimeoutMs = (int)UiHelper.GetNumericValue(valueDetectTimeout);

            DetectorParam.FineSizeMeasure = UiHelper.GetCheckBoxChecked(useFineSizeMeasure);
            DetectorParam.FineSizeMeasureSizeMul = (int)UiHelper.GetNumericValue(valueFineSizeMeasureSizeMul);
            DetectorParam.FineSizeMeasureThresholdMul = (float)UiHelper.GetNumericValue(valueFineSizeMeasureThresholdMul);

            DetectorParam.UseSpreadTrace = UiHelper.GetCheckBoxChecked(useDetectorSpreadTracing);
            DetectorParam.MergingDefects = UiHelper.GetCheckBoxChecked(useMergingDefects);

            gOptions.StickerOption.Use = UiHelper.GetCheckBoxChecked(useSticker);
            gOptions.StickerOption.High = (byte)UiHelper.GetNumericValue(valueStickerDiffHigh);
            gOptions.StickerOption.Low = (byte)UiHelper.GetNumericValue(valueStickerDiffLow);

            DetectorParam.MinBlackDefectLength = (int)UiHelper.GetNumericValue(valueElectricMinSize);
            DetectorParam.MinWhiteDefectLength = (int)UiHelper.GetNumericValue(valueDielectricMinSize);
            DetectorParam.CriterionLength = (DetectorParam.ECriterionLength)UiHelper.GetComboBoxSelectedIndex(cmbCriterionLength);

            // Extention
            WatcherParam watcherParam = WatcherParam;
            if (watcherParam != null)
            {
                TransformParam transformParam = watcherParam.ModelParam.TransformCollection.Param;

                transformParam.MatchingScore = (float)UiHelper.GetNumericValue(sizeVariationScore);
                transformParam.Count = new Size()
                {
                    Width = (int)UiHelper.GetNumericValue(sizeVariationCountW),
                    Height = (int)UiHelper.GetNumericValue(sizeVariationCountH),
                };
                transformParam.SizeUm = new SizeF()
                {
                    Width = (float)UiHelper.GetNumericValue(sizeVariationSizeW) * 1000,
                    Height = (float)UiHelper.GetNumericValue(sizeVariationSizeH) * 1000,
                };
            }
        }

        private void MiscParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            SetMiscParamControl();
            if (sender is NumericUpDown)
            {
                NumericUpDown numericUpDown = (NumericUpDown)sender;
                if (numericUpDown.Name == this.imageRescaler.Name)
                    UpdateOverlayFigure();
            }
            UpdateMiscParamControl();
        }

        private void SetMiscParamControl()
        {
            if (this.OnUpdateData == true)
                return;

            AlgorithmSetting.Instance().InspBufferCount = (int)this.bufferSize.Value;
            WatcherParam.MonitoringPeriod = (int)monitoringPeriod.Value;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = Settings.AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            additionalSettings.AsyncMode = (EAsyncMode)this.asyncMode.CheckState;
            float AsyncGrabMpm;
            if (float.TryParse(grabSpeedMpM.Text, out AsyncGrabMpm))
                additionalSettings.AsyncGrabMpm = AsyncGrabMpm;

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            if (model != null)
            {
                model.ScaleFactor = (int)this.imageRescaler.Value;
                model.Modified = true;
            }

            UniEye.Base.Settings.OperationSettings.Instance().SaveDebugImage = debugMode.Checked;
            this.FiducialFinderParam.FallowingThreshold = this.fallowingThreshold.Checked;

            if (this.modellerPageExtender != null)
                this.modellerPageExtender.LineSpeedMpm = (float)this.progressSpeed.Value;
        }
        #endregion

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;

            //bool isSuperAccount = curUser.IsMasterAccount;
            //// Misc 파라메터 창
            //this.tabControlParam.Tabs["Misc"].Visible = isSuperAccount;

            //// 보정
            //this.labelThresholdMul.Visible = fiducialThresholdMul.Visible = this.labelThresholdMulUnit.Visible = isSuperAccount;
            //this.labelEdgeFinderVersion.Visible = this.edgeFinderVersion.Visible = isSuperAccount;
            //this.labelSearchDirXBase.Visible = this.searchDirXBase.Visible = isSuperAccount;
            //this.labelSearchHalfArea.Visible = this.searchHalfArea.Visible = isSuperAccount;
            //this.labelMinBrightnessDev.Visible = this.minBrightnessDev.Visible = this.labelMinBrightnessDevUnit.Visible = isSuperAccount;

            //// 트레인
            //this.labelMinPatternArea.Visible = this.minPatternArea.Visible = this.labelMinPatternAreaUnit.Visible = isSuperAccount;
            //this.labelGroupDirection.Visible = this.groupDirection.Visible = isSuperAccount;
            //this.labelMinLineIntens.Visible = this.minLineIntens.Visible = this.labelMinLineIntensityUnit.Visible = isSuperAccount;
            //this.labelKernalSize.Visible = this.kernalSize.Visible = this.labelKernalSizeUnit.Visible = isSuperAccount;
            //this.labelDiffThreshold.Visible = this.diffThreshold.Visible = this.labelDiffThresholdUnit.Visible = isSuperAccount;
            //this.labelSplitLargeBar.Visible = this.splitLargeBar.Visible = isSuperAccount;

            //// 검사
            //this.labelDetectMultiThread.Visible = this.detectMultiThread.Visible = isSuperAccount;
            //this.labelDetectTimeout.Visible = this.detectTimeout.Visible = this.labelDetectTimeoutUnit.Visible = isSuperAccount;
            //this.labelAdaptivePairing.Visible = this.adaptivePairing.Visible = isSuperAccount;
            //this.labelFineSizeMeasure.Visible = this.fineSizeMeasure.Visible =
            //    this.fineSizeMeasureSizeMul.Visible = this.fineSizeMeasureThresholdMul.Visible =
            //    this.labelFindMeasureUnit.Visible = isSuperAccount;

            //this.labelSplitLargeBar.Visible = this.splitLargeBar.Visible = isSuperAccount;
            UpdateData();
        }

        private void ParamController_ClientSizeChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            //UpdateRegionSelector(true);
        }

        public void ModelChanged()
        {
            if (this.InvokeRequired)
            {
                Invoke(new ModelChangedDelegate(ModelChanged));
                return;
            }

            this.tabControlParam.SelectedTab = this.tabControlParam.Tabs[0];
            UpdateData();
        }

        public void ModelTeachDone(int camId)
        {
            UpdateData();
            UpdateOverlayFigure();
        }

        public void ModelRefreshed() { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            this.OnUpdateData = true;
            // Infragistics Tab Controls
            //foreach (string tabKey in Enum.GetNames(typeof(ParamTabKey)))
            //    tabControlParam.Tabs[tabKey].Text = StringManager.GetString(this.GetType().FullName, tabControlParam.Tabs[tabKey].Text);
            foreach (UltraTab ultraTab in tabControlParam.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Key);

            foreach (UltraTab ultraTab in tabControlTeach.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Key);

            //UpdateComboBoxBox(this.edgeFinderVersion, typeof(EEdgeFinderVersion));
            //UpdateComboBoxBox(this.searchDirXBase, typeof(BaseXSearchDir));
            //UpdateComboBoxBox(this.groupDirection, typeof(GroupDirection));
            //UpdateComboBoxBox(this.ignoreMethod, typeof(CalculatorParam.EIgnoreMethod));
            UpdateComboBoxBox(this.cmbCriterionLength, typeof(DetectorParam.ECriterionLength));
            UpdateComboBoxBox(this.cmbReconstruct, typeof(EReconstruct));

            this.OnUpdateData = false;

            //UpdateData();
        }

        private void UpdateComboBoxBox(ComboBox comboBox, Type type)
        {
            Dictionary<Enum, string> dictionary = new Dictionary<Enum, string>();
            Array arr = Enum.GetValues(type);
            foreach (Enum e in arr)
                dictionary.Add(e, StringManager.GetString(this.GetType().FullName, e.ToString()));
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.DataSource = new BindingSource(dictionary, null);
        }


        private void regionSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            //if (regionSelector.Visible == false)
            //    return;

            UpdateOverlayFigure();
        }

        int rowContentIdx = -1;
        int cellContentCnt = -1;
        private void regionSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.RowIndex != rowContentIdx)
                cellContentCnt = -1;
            rowContentIdx = e.RowIndex;

            if (e.ColumnIndex != 2)
                cellContentCnt = -1;

            if (e.ColumnIndex == 1)
            {
                //RegionInfoG regionInfo = regionSelector.Rows[e.RowIndex].Tag as RegionInfoG;
                //((DataGridViewCheckBoxCell)regionSelector.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = regionInfo.Use = !regionInfo.Use;
                //UpdateOverlayFigure();

                //SystemManager.Instance().CurrentModel.Modified = true;
            }
            else if (e.ColumnIndex != 4)
            {
                //if (regionSelector.SelectedRows.Count == 0)
                //    return;

                //Point viewPortLoc = Point.Empty;
                //Size viewPortSize = Size.Empty;
                //RegionInfoG regionInfo = regionSelector.SelectedRows[0].Tag as RegionInfoG;

                //if (e.ColumnIndex == 2)
                //{
                //    Rectangle rectangle;
                //    cellContentCnt = (cellContentCnt + 1) % 3;
                //    switch (cellContentCnt)
                //    {
                //        default:
                //        case 0:
                //            viewPortLoc = regionInfo.Region.Location;
                //            viewPortSize = regionInfo.Region.Size;
                //            break;
                //        case 1:
                //            rectangle = DrawingHelper.FromCenterSize(regionInfo.Region.Location, new Size(1024, 1024));
                //            viewPortLoc = rectangle.Location;
                //            viewPortSize = rectangle.Size;
                //            break;
                //        case 2:
                //            rectangle = DrawingHelper.FromCenterSize(new Point(regionInfo.Region.Left, regionInfo.Region.Bottom), new Size(1024, 1024));
                //            viewPortLoc = rectangle.Location;
                //            viewPortSize = rectangle.Size;
                //            break;
                //    }
                //}
                //else if (e.ColumnIndex == 3)
                //{
                //    AlignInfo inBarAlignElement = regionInfo.AlignInfoCollection.FirstOrDefault();
                //    if (inBarAlignElement == null)
                //        return;

                //    viewPortLoc = DrawingHelper.Add(regionInfo.Region.Location, inBarAlignElement.SearchRect.Location);
                //    viewPortSize = inBarAlignElement.SearchRect.Size;
                //}

                //Rectangle viewPort = new Rectangle(viewPortLoc, viewPortSize);
                //if (!viewPort.IsEmpty)
                //    modellerPageExtender.UpdateZoom(viewPort);
            }
        }

        private void buttonRegionTest_Click(object sender, EventArgs e)
        {
            //if (regionSelector.SelectedRows.Count == 0)
            //    return;

            //RegionInfoG selRegionInfo = CalculatorParam.ModelParam.RegionInfoCollection[regionSelector.SelectedRows[0].Index];
            //modellerPageExtender.TargetRegionInfo = selRegionInfo;
            //modellerPageExtender.Inspect();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            //if (regionSelector.SelectedRows.Count == 0)
            //    return;

            //InspectionResult inspectionResult = this.modellerPageExtender.InspectionResult as InspectionResult;
            //Point offsetPt = Point.Empty;
            //int selIndex = regionSelector.SelectedRows[0].Index;
            //if (inspectionResult != null)
            //    offsetPt = inspectionResult.GetOffset(selIndex);

            //RegionInfoG selRegionInfo = CalculatorParam.ModelParam.RegionInfoCollection[selIndex];
            //Rectangle clipRect = DrawingHelper.Offset(selRegionInfo.Region, offsetPt);
            //Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

            //ImageD regionTrainImageD, regionPatternImageD;
            //using (AlgoImage trainImage = ImageBuilder.Build(TrainerBase.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey))
            //{
            //    AlgoImage regionTrainImage = trainImage.GetSubImage(clipRect);
            //    regionTrainImageD = regionTrainImage.ToImageD();
            //    regionTrainImage.Dispose();

            //    TrainerBase trainer = AlgorithmPool.Instance().GetAlgorithm(TrainerBase.TypeName) as TrainerBase;
            //    using (AlgoImage majorPatternImage = trainer.GetMajorPatternImage(trainImage))
            //    {
            //        AlgoImage regionPatternImage = majorPatternImage.GetSubImage(clipRect);
            //        regionPatternImageD = regionPatternImage.ToImageD();
            //        regionPatternImage.Dispose();
            //    }
            //}

            //RegionEditor regionEditor = new RegionEditor(regionTrainImageD, regionPatternImageD, calibration);
            //regionEditor.RegionInfo = selRegionInfo;
            //regionEditor.GroupDirection = TrainerParam.GroupDirection;
            //regionEditor.Dock = DockStyle.Fill;
            //regionEditor.ApplyAll = regionEditor_ApplyAll;

            //Form form = new Form();
            //form.WindowState = FormWindowState.Maximized;
            //form.Controls.Add(regionEditor);
            //form.ShowDialog();

            ////selRegionInfo.Dispose();
            //regionEditor.RegionInfo.BuildInspRegion(regionTrainImageD, regionPatternImageD, TrainerParam.ModelParam.IgnoreInnerChip, TrainerParam.GroupDirection);
            //CalculatorParam.ModelParam.RegionInfoCollection[selIndex] = regionEditor.RegionInfo;
            //regionSelector.SelectedRows[0].Tag = regionEditor.RegionInfo;
            //UpdateRegionSelector(false);
            //UpdateOverlayFigure();

            //regionTrainImageD?.Dispose();
            //regionPatternImageD?.Dispose();
        }

        private void regionEditor_ApplyAll(RegionInfoG regionInfo)
        {
            TrainerBase trainer = AlgorithmPool.Instance().GetAlgorithm(TrainerBase.TypeName) as TrainerBase;
            AlgoImage trainImage = ImageBuilder.Build(TrainerBase.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey);
            AlgoImage majorPatternImage = trainer.GetMajorPatternImage(trainImage);

            CalculatorParam.ModelParam.RegionInfoCollection.ForEach(f =>
            {
                //Point offset = Point.Empty;
                //{
                //    offset.X = f.PatRegionList[0, 0].X - regionInfo.PatRegionList[0, 0].X;
                //    offset.Y = f.PatRegionList[0, 0].Y - regionInfo.PatRegionList[0, 0].Y;
                //}
                //Rectangle srcRect = new Rectangle(Point.Empty, regionInfo.ContourImage.Size);
                //Rectangle dstRect = new Rectangle(Point.Empty, f.ContourImage.Size);
                //dstRect.Offset(offset);
                //dstRect.Intersect(srcRect);
                //srcRect.Offset(Math.Max(-offset.X, 0), Math.Max(-offset.Y, 0));
                //srcRect.Size = dstRect.Size;

                //ImageD contourImage = regionInfo.ContourImage;

                //f.ContourImage.Clear();`
                //f.ContourImage.CopyFrom(contourImage, srcRect, contourImage.Pitch, dstRect.Location);

                //f.DontcarePatLocationList.Clear();
                //f.DontcarePatLocationList.AddRange(regionInfo.DontcarePatLocationList);

                AlgoImage regionTrainImage = trainImage.GetSubImage(f.Region);
                AlgoImage regionPatternImage = majorPatternImage.GetSubImage(f.Region);
                f.IgnoreInnerChip = regionInfo.IgnoreInnerChip;
                f.GroupDirection = regionInfo.GroupDirection;
                f.OddEvenPair = regionInfo.OddEvenPair;
                f.BuildInspRegion(regionTrainImage.ToImageD(), regionPatternImage.ToImageD());
                regionTrainImage.Dispose();
                regionPatternImage.Dispose();
            });

            trainImage.Dispose();
            majorPatternImage.Dispose();
            //UpdateRegionSelector(true);

        }

        private void ultraButtonPatternUpdate_Click(object sender, EventArgs e)
        {
            Teach(new TrainerArgument(false, true, false, false));
        }

        private void ultraButtonMonitoringUpdate_Click(object sender, EventArgs e)
        {
            Teach(new TrainerArgument(false, false, false, true));
        }

        private void Teach(TrainerArgument trainerArgument)
        {
            try
            {
                modellerPageExtender.Teach(trainerArgument);
            }
            catch (Exception ex)
            {
                Form mainForm = ConfigHelper.Instance().MainForm;
                MessageForm.Show(mainForm, ex.Message);
            }
        }

        private void tabControlTarget_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            bool drawMode = (e.Tab.Index == 2);
            UpdateOverlayFigure();
        }

        private void buttonMonitoringTest_Click(object sender, EventArgs e)
        {
            Image2D curImage = this.modellerPageExtender.CurrentImage;
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;

            AlgoImage algoImage = ImageBuilder.Build(Watcher.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey);
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            DebugContextG debugContextG = new DebugContextG(new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp));
            debugContextG.LotNo = "ModellerPageTestInspect";

            SheetInspectParam sheetInspectParam = new SheetInspectParam(model, algoImage, calibration, debugContextG);
            Watcher watcher = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName) as Watcher;
            watcher.PrepareInspection();
            WatcherResult watcherResult = (WatcherResult)watcher.Inspect(sheetInspectParam);

            algoImage.Dispose();

            InspectionResult inspectionResult = SystemManager.Instance().InspectRunner.InspectRunnerExtender.BuildInspectionResult("TEST") as InspectionResult;
            inspectionResult.AlgorithmResultLDic.Add(Watcher.TypeName, watcherResult);
            modellerPageExtender.UpdateSheetResult?.Invoke(inspectionResult, debugContextG);
        }

        private void buttonMonitoringEdit_Click(object sender, EventArgs e)
        {
            Image2D curImage = this.modellerPageExtender.CurrentImage;
            WatcherParam watcherParam = WatcherParam;

            List<ExtItem> watchItemList = watcherParam.ModelParam.GetItems();
            if (curImage == null || watchItemList == null)
                return;

            WatchEditor watchEditor = new WatchEditor(1f);
            watchEditor.Dock = DockStyle.Fill;
            watchEditor.Initialize(curImage, watchItemList, this.modellerPageExtender.InspectionResult?.OffsetSet);

            Form form = new Form();
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Add(watchEditor);
            form.ShowDialog(this);

            if (watchEditor.WatchItemListDone != null)
            {
                watcherParam.ModelParam.SetItems(watchEditor.WatchItemListDone);
                SystemManager.Instance().CurrentModel.Modified = true;
                UpdateTeachMonitoring();
                UpdateOverlayFigure();
            }
        }

        private void grabSpeedkHz_ValueChanged(object sender, EventArgs e)
        {
            float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)grabSpeedkHz.Value * 1000;
            float spdMpm = grabHz * 60 / 1000 / 1000 * resUm;
            this.grabSpeedMpM.Text = spdMpm.ToString("F1");
        }

        private void grabSpeedMpM_ValueChanged(object sender, EventArgs e)
        {
            float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)grabSpeedMpM.Value / 60 * 1000 * 1000 / resUm;
            this.grabSpeedkHz.Text = (grabHz / 1000).ToString("F3");

            SetMiscParamControl();
            //AdditionalSettings.Instance().AsyncGrabMpm = (float)grabSpeedMpM.Value;
        }

        private void buttonProgressSpeed_Click(object sender, EventArgs e)
        {
            modellerPageExtender.RequestLineSpeed();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SimpleProgressForm().Show(() =>
            {
                string tempPath = PathSettings.Instance().Temp;
                CalculatorResultV3 calculatorResult = modellerPageExtender.InspectionResult?.AlgorithmResultLDic[CalculatorBase.TypeName] as CalculatorResultV3;
                calculatorResult?.SaveForDebug(tempPath);

                DetectorResult detectorResult = modellerPageExtender.InspectionResult?.AlgorithmResultLDic[Detector.TypeName] as DetectorResult;
                System.IO.Directory.CreateDirectory(Path.Combine(tempPath, "Detector"));
                detectorResult.SheetSubResultList.ForEach(f =>
                {
                    f.Image.Save(Path.Combine(tempPath, "Detector", $"{f.Index}.bmp"));
                    f.BufImage.Save(Path.Combine(tempPath, "Detector", $"{f.Index}B.bmp"));
                });
            });
        }

        private void buttonReferenceUpdate_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void buttonInvalidateHead_Click(object sender, EventArgs e)
        {
            WorkPoint[] wps = Array.FindAll(this.Model.RCITrainResult.WorkPoints, f => f.Row == 0);
            bool setValue = !Array.Exists(wps, f => f.Use);
            Array.ForEach(wps, f => f.Use = setValue);
            UpdateData();
            UpdateOverlayFigure();
        }

        private void buttonInvalidateTail_Click(object sender, EventArgs e)
        {
            WorkPoint[] wps = Array.FindAll(this.Model.RCITrainResult.WorkPoints, f => f.Row == this.Model.RCITrainResult.WorkPointRowCount - 1);
            bool setValue = !Array.Exists(wps, f => f.Use);
            Array.ForEach(wps, f => f.Use = setValue);
            UpdateData();
            UpdateOverlayFigure();
        }

        private void buttonReferenceEdit_Click(object sender, EventArgs e)
        {

        }

    }
}
