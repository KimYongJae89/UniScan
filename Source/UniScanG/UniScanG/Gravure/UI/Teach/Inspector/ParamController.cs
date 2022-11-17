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

namespace UniScanG.Gravure.UI.Teach.Inspector
{

    public partial class ParamController : UserControl, IModellerControl, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        public enum ParamTabKey { Fiducial, Train, Inspect, Misc }
        private float pelSize;

        private UniScanG.Data.Model.Model Model => SystemManager.Instance().CurrentModel;

        private SheetFinderBaseParam FiducialFinderParam => SheetFinderBase.SheetFinderBaseParam;

        private TrainerParam TrainerParam => TrainerBase.TrainerParam;

        private CalculatorParam CalculatorParam => CalculatorBase.CalculatorParam;

        private DetectorParam DetectorParam => Detector.DetectorParam;

        private WatcherParam WatcherParam => Watcher.WatcherParam;


        bool onUpdateData = false;
        bool OnUpdateData
        {
            get { return onUpdateData; }
            set
            {
                onUpdateData = value;
            }
        }

        ModellerPageExtenderG modellerPageExtender;
        Size imageSize = Size.Empty;

        public ParamController()
        {
            InitializeComponent();

            StringManager.AddListener(this);

            OnUpdateData = true;

            this.searchSkipLength.Minimum = this.patternGapLength.Minimum = this.dielectricMinSize.Minimum = this.electricMinSize.Minimum = this.progressSpeed.Minimum = 0;
            this.searchSkipLength.Maximum = this.patternGapLength.Maximum = this.dielectricMinSize.Maximum = this.electricMinSize.Maximum = this.progressSpeed.Maximum = int.MaxValue;

            this.sensitivityLow.Minimum = this.sensitivityHigh.Minimum = 0;
            this.sensitivityLow.Maximum = this.sensitivityHigh.Maximum = 255;

            this.defectMaxCount.Minimum = this.detectTimeout.Minimum = -1;
            this.defectMaxCount.Maximum = this.detectTimeout.Maximum = int.MaxValue;

            this.valueMarginMeasurement.Minimum = 0;
            this.valueMarginMeasurement.Maximum = int.MaxValue;

            this.sizeVariationSizeW.Minimum = this.sizeVariationSizeH.Minimum = (decimal)0.001;
            this.sizeVariationSizeW.Maximum = this.sizeVariationSizeH.Maximum = int.MaxValue;

            this.chipShare.Minimum = 0;
            this.chipShare.Maximum = 100;

            DynMvp.Vision.Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            this.pelSize = calibration == null ? 14.0f : calibration.PelSize.Height;

            string ver = VersionHelper.Instance().VersionString;
            string bld = VersionHelper.Instance().BuildString;
            string flg = DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag;
            if (string.IsNullOrEmpty(flg))
                this.versionBuild.Text = $"V{ver} / B{bld}";
            else
                this.versionBuild.Text = $"V{ver} / B{bld} / {flg}";

            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList.FirstOrDefault();
            int cameraImageHeight = (imageDevice == null ? 0 : imageDevice.ImageSize.Height);
            this.patternGapLength.Maximum = (decimal)(cameraImageHeight * this.pelSize / 1000);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
            OnUpdateData = false;

        }

        private void ParamController_Load(object sender, EventArgs e)
        {
#if !DEBUG
            this.progressSpeed.Enabled = false;
            this.calcMultiData.Enabled = false;
#endif
            UpdateData();
            this.OnUpdateData = false;
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender as ModellerPageExtenderG;

            this.modellerPageExtender.ImageUpdated = modellerPageExtender_ImageUpdated;
            this.modellerPageExtender.OnLineSpeedUpdated += modellerPageExtender_LineSpeedUpdated;
        }

        private void modellerPageExtender_LineSpeedUpdated()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OnLineSpeedUpdatedDelegate(modellerPageExtender_LineSpeedUpdated));
                return;
            }

            this.progressSpeed.BackColor = this.modellerPageExtender.LineStartState ? Color.LightGreen : SystemColors.Window;
            UiHelper.SetNumericValue(this.progressSpeed, (decimal)this.modellerPageExtender.LineSpeedMpm);
        }

        private void modellerPageExtender_LoadImage()
        {
            throw new NotImplementedException();
        }

        private void modellerPageExtender_ImageUpdated(Size imageSize)
        {
            this.imageSize = imageSize;
            //if (imageSize.Width > 0)
            this.searchSkipLength.Maximum = (decimal)(imageSize.Width * this.pelSize / 1000);
        }


        delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                tabControlParam.Tabs[0].Visible = false;
                tabControlParam.Tabs[1].Visible = false;
                tabControlParam.Tabs[2].Visible = false;
            }
            else
            {
                tabControlParam.Tabs[0].Visible = true;
                tabControlParam.Tabs[1].Visible = true;
                tabControlParam.Tabs[2].Visible = true;

                UpdatePatternSelector();
                UpdateRegionSelector(true);
                UpdateMonitoringSelector();

                UpdateFiducialFinderParamControl();
                UpdateTrainerParamControl();
                UpdateInspectorParamControl();

                UpdateOverlayFigure();
            }
            UpdateMiscParamControl();
            this.OnUpdateData = false;
        }

        private void UpdateOverlayFigure()
        {
            DataGridView dataGridView = null;
            UltraTab selectedTab = tabControlTarget.SelectedTab;
            if (selectedTab != null)
            {
                ModellerPageExtenderG.FigureType figureType = ModellerPageExtenderG.FigureType.Chip;
                switch (selectedTab.Key)
                {
                    case "Pattern":
                        // 패턴 그리기
                        figureType = ModellerPageExtenderG.FigureType.Chip;
                        dataGridView = this.patternSelector;
                        break;

                    case "Region":
                        // 영역 그리기
                        figureType = ModellerPageExtenderG.FigureType.Bar;
                        dataGridView = this.regionSelector;
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
        private void UpdatePatternSelector()
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            patternCount.Text = "0";
            patternSelector.Rows.Clear();

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();

            List<SheetPatternGroup> patternGroupList = CalculatorParam.ModelParam.PatternGroupCollection;
            patternCount.Text = patternGroupList.Count.ToString();

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            for (int i = 0; i < patternGroupList.Count; i++)
            {
                SheetPatternGroup patternGroup = patternGroupList[i] as SheetPatternGroup;
                if (patternGroup == null)
                    continue;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(patternSelector);

                row.Cells[0].Value = i + 1;
                row.Cells[1].Value = patternGroup.Use;
                row.Cells[2].Value = patternGroup.MasterImage?.ToBitmap();
                row.Cells[3].Value = patternGroup.GetInfoText(calibration);

                row.Tag = patternGroup;
                row.Height = 150;
                rowList.Add(row);
            }
            patternSelector.Rows.AddRange(rowList.ToArray());
            this.OnUpdateData = false;
        }

        private void UpdateRegionSelector(bool clearAndRedraw)
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            regionCount.Text = "0";

            CalculatorParam calculatorParam = this.CalculatorParam;
            if (calculatorParam != null)
            {
                List<RegionInfoG> regionInfoList = calculatorParam.ModelParam.RegionInfoCollection;
                regionCount.Text = regionInfoList.Count.ToString();
                basePos.Text = string.Format("{0},{1}", calculatorParam.ModelParam.BasePosition.X, calculatorParam.ModelParam.BasePosition.Y);

                if (clearAndRedraw)
                {
                    regionSelector.Rows.Clear();

                    for (int i = 0; i < regionInfoList.Count; i++)
                    {
                        RegionInfoG regionInfoG = regionInfoList[i] as RegionInfoG;
                        if (regionInfoG == null)
                            continue;

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(regionSelector);

                        FillRow(i, row, regionInfoG);
                        //row.Cells[0].Value = i + 1;
                        //((DataGridViewCheckBoxCell)row.Cells[1]).Value = regionInfoG.Use;
                        //row.Cells[2].Value = regionInfoG.PatternImage?.ToBitmap();
                        //row.Cells[3].Value = regionInfoG.GetInfoString();
                        //row.Height = 150;
                        //row.Tag = regionInfoG;
                        int idx = regionSelector.Rows.Add(row);
                    }
                }
                else
                {
                    for (int i = 0; i < regionSelector.Rows.Count; i++)
                    {
                        DataGridViewRow row = regionSelector.Rows[i];
                        RegionInfoG regionInfoG = row.Tag as RegionInfoG;
                        if (regionInfoG == null)
                            return;

                        FillRow(row.Index, row, regionInfoG);
                        //((DataGridViewCheckBoxCell)row.Cells[1]).Value = regionInfoG.Use;
                        //row.Cells[2].Value = regionInfoG.PatternImage?.ToBitmap();
                        //row.Cells[3].Value = regionInfoG.GetInfoString();
                        //row.Tag = regionInfoG;
                        //row.Height = 150;
                    }
                }

            }
            this.OnUpdateData = false;
        }

        private void FillRow(int idx, DataGridViewRow row, RegionInfoG regionInfoG)
        {
            row.Cells[0].Value = idx + 1;
            row.Cells[1].Value = regionInfoG.Use;
            row.Cells[2].Value = regionInfoG.ThumbnailImageD?.ToBitmap();
            row.Cells[3].Value = regionInfoG.AlignInfoCollection.FirstOrDefault()?.MasterImageD?.ToBitmap();
            row.Cells[4].Value = regionInfoG.GetInfoString();
            row.Height = 145;
            row.Tag = regionInfoG;
        }

        private void UpdateMonitoringSelector()
        {
            if (this.OnUpdateData)
                return;

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
                PointF viewPortLocUm = PointF.Empty;
                SizeF viewPortSizeUm = SizeF.Empty;
                ExtItem watchItem = this.monitoringSelector.Rows[e.RowIndex].Tag as ExtItem;

                viewPortLocUm = watchItem.ClipRectangleUm.Location;
                viewPortSizeUm = watchItem.ClipRectangleUm.Size;

                RectangleF viewPortUm = new RectangleF(viewPortLocUm, viewPortSizeUm);
                Rectangle viewPort = Rectangle.Round(this.modellerPageExtender.Calibration.WorldToPixel(viewPortUm));

                if (!viewPort.IsEmpty)
                    modellerPageExtender.UpdateZoom(viewPort);
            }
        }

        private void UpdateFiducialFinderParamControl()
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            SheetFinderBaseParam algorithmParam = FiducialFinderParam;
            if (algorithmParam != null)
            {
                this.edgeFinderVersion.SelectedValue = algorithmParam.EdgeFinderVersion;
                this.searchDirXBase.SelectedIndex = (int)algorithmParam.BaseXSearchDir;
                this.searchHalfArea.Checked = algorithmParam.BaseXSearchHalf;
                this.minBrightnessDev.Value = (decimal)algorithmParam.MinBrightnessStdDev;

                if (SystemManager.Instance().InspectRunnerG.GrabProcesser.IsStopAndGo)
                {
                    this.tableLayoutGrabProcesser.Visible = false;
                }
                else
                {
                    this.tableLayoutGrabProcesser.Visible = true;

                    if (algorithmParam is SheetFinderPJParam)
                    {
                        this.layoutFinderV1.Visible = true;
                        SheetFinderPJParam fiducialFinderPJParam = (SheetFinderPJParam)algorithmParam;
                        this.fiducialSearchRangeL.Value = (decimal)fiducialFinderPJParam.FidSearchLBound * 100;
                        this.fiducialSearchRangeR.Value = (decimal)fiducialFinderPJParam.FidSearchRBound * 100;
                        this.fiducialSizeW.Value = fiducialFinderPJParam.FidSize.Width;
                        this.fiducialSizeH.Value = fiducialFinderPJParam.FidSize.Height;
                    }
                    else
                    {
                        this.layoutFinderV1.Visible = false;
                    }

                    if (algorithmParam is SheetFinderV2Param)
                    {
                        this.layoutFinderV2.Visible = true;
                        SheetFinderV2Param sheetFinderV2Param = (SheetFinderV2Param)algorithmParam;
                        this.fiducialThresholdMul.Value = (decimal)sheetFinderV2Param.ProjectionBinalizeMul;
                    }
                    else
                    {
                        this.layoutFinderV2.Visible = false;
                    }
                }

                try
                {
                    SetNumericUpDownControl(patternGapLength, (decimal)algorithmParam.BoundaryHeightMm);
                    SetNumericUpDownControl(searchSkipLength, (decimal)algorithmParam.SearchSkipWidthMm);
                    SetNumericUpDownControl(alignGlobal, (decimal)algorithmParam.AlignGlobalMm);
                }
                catch (Exception e)
                {

                }
            }

            this.OnUpdateData = false;
        }

        private void SetNumericUpDownControl(NumericUpDown control, decimal value)
        {
            control.Value = Math.Min(Math.Max(value, control.Minimum), control.Maximum);
        }

        private void UpdateTrainerParamControl()
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            TrainerParam sheetTrainerParam = this.TrainerParam;
            if (sheetTrainerParam != null)
            {
                // pattern
                this.teachedBinalizeValue.Text = sheetTrainerParam.ModelParam.BinValue.ToString();
                this.binValueOffset.Value = (decimal)sheetTrainerParam.ModelParam.BinValueOffset;
                this.groupTolerance.Value = (decimal)sheetTrainerParam.ModelParam.SheetPatternGroupThreshold;
                this.advWaistMeas.Checked = sheetTrainerParam.ModelParam.AdvanceWaistMeasure;
                this.minPatternArea.Value = (decimal)sheetTrainerParam.MinPatternArea;
                this.chipShare.Value = (decimal)Model.ChipShare100p;

                // region
                this.groupDirection.SelectedIndex = (int)sheetTrainerParam.GroupDirection;
                this.isCrisscross.Checked = sheetTrainerParam.ModelParam.IsCrisscross;
                this.minLineIntens.Value = (int)sheetTrainerParam.MinLineIntensity;
                this.kernalSize.Value = (decimal)sheetTrainerParam.KernalSize;
                this.diffThreshold.Value = (decimal)sheetTrainerParam.DiffrentialThreshold;
                this.splitLargeBar.Checked = sheetTrainerParam.SplitLargeBar;
                this.ignoreInnerChip.Checked = sheetTrainerParam.ModelParam.IgnoreInnerChip;

                this.kernalSize.Enabled = this.diffThreshold.Enabled = sheetTrainerParam.ModelParam.IsCrisscross;

                this.alignLocalSearch.Value = Math.Max(alignLocalSearch.Minimum, (decimal)sheetTrainerParam.AlignLocalSearch);
                this.alignLocalMaster.Value = Math.Max(alignLocalMaster.Minimum, (decimal)sheetTrainerParam.AlignLocalMaster);

                if (sheetTrainerParam.ModelParam == null)
                {
                    this.fixMissingTooth.Checked = this.wholeRegionProjection.Checked = false;
                    this.fixMissingTooth.Enabled = this.wholeRegionProjection.Enabled = false;
                }
                else
                {
                    this.fixMissingTooth.Enabled = this.wholeRegionProjection.Enabled = true;
                    this.wholeRegionProjection.Checked = sheetTrainerParam.ModelParam.WholeRegionProjection;
                    this.fixMissingTooth.Checked = sheetTrainerParam.ModelParam.FixMissingTooth;
                }
            }

            WatcherParam watcherParam = WatcherParam;
            if (watcherParam != null)
            {
                // SizeVariation
                TransformParam transformParam = watcherParam.ModelParam.TransformCollection.Param;
                this.layoutSizeVariation.Visible = transformParam.Available;

                UiHelper.SetNumericValue(this.sizeVariationScore, (decimal)transformParam.MatchingScore);
                UiHelper.SetNumericValue(this.sizeVariationCountW, (decimal)transformParam.Count.Width);
                UiHelper.SetNumericValue(this.sizeVariationCountH, (decimal)transformParam.Count.Height);
                UiHelper.SetNumericValue(this.sizeVariationSizeW, (decimal)(transformParam.SizeUm.Width / 1000));
                UiHelper.SetNumericValue(this.sizeVariationSizeH, (decimal)(transformParam.SizeUm.Height / 1000));
            }

            this.OnUpdateData = false;
        }

        private void UpdateInspectorParamControl()
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            CalculatorParam calculatorParam = this.CalculatorParam as CalculatorParam;
            if (calculatorParam != null)
            {
                this.calculateEdgeWidth.Value = calculatorParam.ModelParam.EdgeParam.Width;
                this.calculateEdgeValue.Value = calculatorParam.ModelParam.EdgeParam.Value;
                this.calculateEdgeMulti.Checked = calculatorParam.ModelParam.EdgeParam.Multi;
                this.calculateEdgeErode.Checked = calculatorParam.ModelParam.EdgeParam.Erode;

                this.innerAlign.Checked = calculatorParam.InBarAlign;
                this.innnerAlignScore.Value = (decimal)calculatorParam.InBarAlignScore;
                this.innnerAlignScore.Enabled = calculatorParam.InBarAlign;

                this.sensitivity.Checked = calculatorParam.ModelParam.SensitiveParam.Multi;
                this.sensitivityLow.Value = calculatorParam.ModelParam.SensitiveParam.Min;
                this.sensitivityHigh.Value = calculatorParam.ModelParam.SensitiveParam.Max;
                this.sensitivityLow.Enabled = this.sensitivity.Checked;

                this.calcBoundaryAreaW.Value = (decimal)calculatorParam.ModelParam.BarBoundary.Width;
                this.calcBoundaryAreaH.Value = (decimal)calculatorParam.ModelParam.BarBoundary.Height;
                this.adaptivePairing.Checked = calculatorParam.AdaptivePairing;
                this.boundaryPairStep.Value = calculatorParam.BoundaryPairStep;
                this.ignoreMethod.SelectedIndex = (int)calculatorParam.ModelParam.IgnoreMethod;
                this.ignoreLastBoundary.Checked = calculatorParam.ModelParam.IgnoreSideLine;

                this.calcMultiThread.Checked = calculatorParam.UseMultiThread;
                this.calcMultiData.Checked = calculatorParam.ModelParam.UseMultiData;

                this.layoutInspectSticker.Visible = AlgorithmSetting.Instance().UseExtSticker;
                this.useSticker.Checked = calculatorParam.UseSticker;
                this.stickerDiffHigh.Value = calculatorParam.StickerDiffHigh;
                this.stickerDiffLow.Value = calculatorParam.StickerDiffLow;
            }

            DetectorParam detectorParam = this.DetectorParam as DetectorParam;
            if (detectorParam != null)
            {
                this.defectMaxCount.Value = detectorParam.MaximumDefectCount;
                this.detectMultiThread.Checked = detectorParam.UseMultiThread;
                this.detectTimeout.Value = detectorParam.TimeoutMs;

                this.fineSizeMeasure.Checked = detectorParam.FineSizeMeasure;
                this.fineSizeMeasureSizeMul.Value = (decimal)detectorParam.FineSizeMeasureSizeMul;
                this.fineSizeMeasureThresholdMul.Value = (decimal)detectorParam.FineSizeMeasureThresholdMul;
                this.fineSizeMeasureThresholdMul.Enabled = this.fineSizeMeasureSizeMul.Enabled = detectorParam.FineSizeMeasure;

                this.detectorSpreadTracing.Checked = detectorParam.UseSpreadTrace;

                this.electricMinSize.Value = detectorParam.MinBlackDefectLength;
                this.dielectricMinSize.Value = detectorParam.MinWhiteDefectLength;
                this.criterionLength.SelectedIndex = (int)detectorParam.CriterionLength;

                this.attackDiffUse.Checked = detectorParam.ModelParam.AttackDiffUse;
                this.attackMinValue.Value = (decimal)detectorParam.ModelParam.AttackMinValue;
                this.attackDiffValue.Value = (decimal)detectorParam.ModelParam.AttackDiffValue;
                this.ignoreLongDefect.Checked = detectorParam.IgnoreLongDefect;

                this.mergingDefects.Checked = detectorParam.MergingDefects;
            }

            WatcherModelParam watcherModelParam = this.WatcherParam.ModelParam;
            if (watcherModelParam != null)
            {
                this.labelStopImage.Visible = this.useStopImage.Visible = watcherModelParam.StopImgCollection.Param.Available;
                //this.useStopImage.Enabled = false;
                this.useStopImage.Checked = watcherModelParam.StopImgCollection.Param.Use;

                this.labelMargin.Visible = this.useMarginMeasurement.Visible =
                    this.valueMarginMeasurement.Visible = this.labelValueMarginMeasurementUnit.Visible = watcherModelParam.MarginCollection.Param.Available;
                //this.useMarginMeasurement.Enabled = false;
                this.useMarginMeasurement.Checked = watcherModelParam.MarginCollection.Param.Use;
                this.valueMarginMeasurement.Value = (decimal)watcherModelParam.MarginCollection.Param.AbsDefectSize;

                this.labelTransform.Visible = this.useTransformMeasurement.Visible =
                    this.sizeVariationScore.Visible = this.labelSizeVariationScoreUnit.Visible = watcherModelParam.TransformCollection.Param.Available;
                //this.useTransformMeasurement.Enabled = false;
                this.useTransformMeasurement.Checked = watcherModelParam.TransformCollection.Param.Use;
            }

            this.OnUpdateData = false;
        }

        private void UpdateMiscParamControl()
        {
            if (this.OnUpdateData)
                return;

            this.OnUpdateData = true;

            bufferSize.Value = AlgorithmSetting.Instance().InspBufferCount;
            monitoringPeriod.Value = WatcherParam.MonitoringPeriod;

            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count > 0)
            {
                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                xPixelCal.Value = (decimal)pelSize.Width;
                yPixelCal.Value = (decimal)pelSize.Height;
            }

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = Settings.AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            asyncMode.CheckState = (CheckState)additionalSettings.AsyncMode;
            UiHelper.SetNumericValue(grabSpeedMpM, (decimal)additionalSettings.AsyncGrabMpm);
            //grabSpeedMpM.Text = additionalSettings.AsyncGrabMpm.ToString("F0");
            grabSpeedMpM.Enabled = (additionalSettings.AsyncMode != EAsyncMode.False);

            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count > 0)
            {
                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
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

            Vision.LengthVariation.LengthVariationParam lengthVariationParam = AlgorithmSetting.Instance().LengthVariationParam;
            if (lengthVariationParam == null)
            {
                lengthVariation.Enabled = lengthVariationPosition.Enabled = lengthVariationWidth.Enabled = false;
            }
            else
            {
                lengthVariation.Enabled = lengthVariationPosition.Enabled = lengthVariationWidth.Enabled = true;
                lengthVariation.Checked = lengthVariationParam.Use;
                SetNumericUpDownControl(lengthVariationPosition, (decimal)lengthVariationParam.Position * 100);
                SetNumericUpDownControl(lengthVariationWidth, (decimal)lengthVariationParam.WidthUm / 1000);

            }

            modellerPageExtender_LineSpeedUpdated();

            this.OnUpdateData = false;
        }
        #endregion

        #region Apply Value
        private void SetFiducialFinderParam(bool updateFigure)
        {
            if (this.OnUpdateData == true)
                return;

            SystemManager.Instance().CurrentModel.Modified = true;
            SheetFinderBaseParam sheetFinderBaseParam = FiducialFinderParam;

            sheetFinderBaseParam.EdgeFinderVersion = (EEdgeFinderVersion)this.edgeFinderVersion.SelectedValue;
            sheetFinderBaseParam.BaseXSearchDir = (BaseXSearchDir)this.searchDirXBase.SelectedValue;
            sheetFinderBaseParam.BaseXSearchHalf = searchHalfArea.Checked;
            sheetFinderBaseParam.MinBrightnessStdDev = (float)minBrightnessDev.Value;

            if (!SystemManager.Instance().InspectRunnerG.GrabProcesser.IsStopAndGo)
            {
                if (sheetFinderBaseParam is SheetFinderPJParam)
                {
                    this.layoutFinderV1.Visible = true;
                    SheetFinderPJParam fiducialFinderPJParam = (SheetFinderPJParam)sheetFinderBaseParam;
                    fiducialFinderPJParam.FidSearchLBound = ((float)this.fiducialSearchRangeL.Value) / 100.0f;
                    fiducialFinderPJParam.FidSearchRBound = ((float)this.fiducialSearchRangeR.Value) / 100.0f;
                    int fidSizeW = (int)this.fiducialSizeW.Value;
                    int fidSizeH = (int)this.fiducialSizeH.Value;
                    fiducialFinderPJParam.FidSize = new Size(fidSizeW, fidSizeH);
                }

                if (sheetFinderBaseParam is SheetFinderV2Param)
                {
                    this.layoutFinderV2.Visible = true;
                    SheetFinderV2Param sheetFinderV2Param = (SheetFinderV2Param)sheetFinderBaseParam;
                    sheetFinderV2Param.ProjectionBinalizeMul = (float)fiducialThresholdMul.Value;
                }
            }

            sheetFinderBaseParam.BoundaryHeightMm = (float)patternGapLength.Value;
            sheetFinderBaseParam.SearchSkipWidthMm = (float)searchSkipLength.Value;
            sheetFinderBaseParam.AlignGlobalMm = (float)alignGlobal.Value;

            if (updateFigure)
                UpdateOverlayFigure();
            //UpdateRegionSelector();
            //UpdateData();
            UpdateFiducialFinderParamControl();
        }

        private void SetTrainerParam()
        {
            if (this.OnUpdateData == true)
                return;

            SystemManager.Instance().CurrentModel.Modified = true;
            TrainerParam sheetTrainerParam = this.TrainerParam;

            // pattern
            sheetTrainerParam.ModelParam.BinValueOffset = (int)binValueOffset.Value;
            sheetTrainerParam.ModelParam.SheetPatternGroupThreshold = (float)groupTolerance.Value;
            sheetTrainerParam.ModelParam.AdvanceWaistMeasure = advWaistMeas.Checked;
            sheetTrainerParam.MinPatternArea = (int)minPatternArea.Value;
            Model.ChipShare100p = (float)this.chipShare.Value;

            // region
            sheetTrainerParam.GroupDirection = (GroupDirection)this.groupDirection.SelectedIndex;
            sheetTrainerParam.ModelParam.IsCrisscross = this.isCrisscross.Checked;
            sheetTrainerParam.MinLineIntensity = (int)this.minLineIntens.Value;
            sheetTrainerParam.KernalSize = (int)this.kernalSize.Value;
            sheetTrainerParam.DiffrentialThreshold = (int)this.diffThreshold.Value;
            sheetTrainerParam.SplitLargeBar = this.splitLargeBar.Checked;
            sheetTrainerParam.ModelParam.IgnoreInnerChip = this.ignoreInnerChip.Checked;

            sheetTrainerParam.AlignLocalSearch = (float)this.alignLocalSearch.Value;
            sheetTrainerParam.AlignLocalMaster = (float)this.alignLocalMaster.Value;

            // size variation
            TransformParam transformParam = WatcherParam.ModelParam.TransformCollection.Param;
            if (transformParam != null)
            {
                //transformParam.Use = this.sizeVariationUse.Checked;
                transformParam.Count = new Size((int)this.sizeVariationCountW.Value, (int)this.sizeVariationCountH.Value);
                transformParam.SizeUm = new SizeF((float)this.sizeVariationSizeW.Value * 1000, (float)this.sizeVariationSizeH.Value * 1000);
            }

            sheetTrainerParam.ModelParam.WholeRegionProjection = this.wholeRegionProjection.Checked;
            sheetTrainerParam.ModelParam.FixMissingTooth = this.fixMissingTooth.Checked;

            UpdateTrainerParamControl();
        }

        private void SetInspectorParam(bool updateFigure)
        {
            if (this.OnUpdateData)
                return;

            SystemManager.Instance().CurrentModel.Modified = true;
            CalculatorParam calculatorParam = this.CalculatorParam as CalculatorParam;
            if (calculatorParam != null)
            {
                calculatorParam.ModelParam.EdgeParam = new EdgeParam(
                    this.calculateEdgeMulti.Checked,
                    (int)this.calculateEdgeValue.Value,
                    (int)this.calculateEdgeWidth.Value,
                    EdgeParam.EEdgeFindMethod.Soble,
                    this.calculateEdgeErode.Checked
                    );

                calculatorParam.InBarAlign = this.innerAlign.Checked;
                calculatorParam.InBarAlignScore = (float)this.innnerAlignScore.Value;

                calculatorParam.ModelParam.SensitiveParam.Multi = this.sensitivity.Checked;
                calculatorParam.ModelParam.SensitiveParam.Min = (byte)this.sensitivityLow.Value;
                calculatorParam.ModelParam.SensitiveParam.Max = (byte)this.sensitivityHigh.Value;

                calculatorParam.ModelParam.BarBoundary = new SizeF((float)this.calcBoundaryAreaW.Value, (float)this.calcBoundaryAreaH.Value);
                calculatorParam.AdaptivePairing = this.adaptivePairing.Checked;
                calculatorParam.BoundaryPairStep = (int)this.boundaryPairStep.Value;
                calculatorParam.ModelParam.IgnoreMethod = (CalculatorParam.EIgnoreMethod)this.ignoreMethod.SelectedIndex;
                calculatorParam.ModelParam.IgnoreSideLine = this.ignoreLastBoundary.Checked;

                calculatorParam.UseMultiThread = this.calcMultiThread.Checked;
                calculatorParam.ModelParam.UseMultiData = this.calcMultiData.Checked;

                calculatorParam.UseSticker = this.useSticker.Checked;
                calculatorParam.StickerDiffHigh = (int)Math.Max(this.stickerDiffHigh.Value, this.stickerDiffLow.Value);
                calculatorParam.StickerDiffLow = (int)Math.Min(this.stickerDiffHigh.Value, this.stickerDiffLow.Value);

                if (updateFigure)
                    UpdateOverlayFigure();
            }

            DetectorParam detectorParam = this.DetectorParam as DetectorParam;
            if (detectorParam != null)
            {
                detectorParam.MaximumDefectCount = (int)this.defectMaxCount.Value;
                detectorParam.UseMultiThread = this.detectMultiThread.Checked;
                detectorParam.TimeoutMs = (int)this.detectTimeout.Value;

                detectorParam.FineSizeMeasure = this.fineSizeMeasure.Checked;
                detectorParam.FineSizeMeasureThresholdMul = (float)this.fineSizeMeasureThresholdMul.Value;
                detectorParam.FineSizeMeasureSizeMul = (int)this.fineSizeMeasureSizeMul.Value;

                detectorParam.UseSpreadTrace = this.detectorSpreadTracing.Checked;

                detectorParam.CriterionLength = (DetectorParam.ECriterionLength)this.criterionLength.SelectedIndex;

                detectorParam.MinBlackDefectLength = (int)this.electricMinSize.Value;
                detectorParam.MinWhiteDefectLength = (int)this.dielectricMinSize.Value;

                detectorParam.ModelParam.AttackDiffUse = this.attackDiffUse.Checked;
                detectorParam.ModelParam.AttackMinValue = (float)this.attackMinValue.Value;
                detectorParam.ModelParam.AttackDiffValue = (float)this.attackDiffValue.Value;
                detectorParam.IgnoreLongDefect = this.ignoreLongDefect.Checked;

                detectorParam.MergingDefects = this.mergingDefects.Checked;
            }

            WatcherParam watcherParam = this.WatcherParam;
            if (watcherParam != null)
            {
                //watcherParam.ModelParam.StopImgCollection.Param.Use = this.useStopImage.Checked;

                Settings.AdditionalSettings.Instance().MarginUse = this.useMarginMeasurement.Checked;
                //watcherParam.ModelParam.MarginCollection.Param.Use = this.useMarginMeasurement.Checked;
                watcherParam.ModelParam.MarginCollection.Param.AbsDefectSize = (float)this.valueMarginMeasurement.Value;

                Settings.AdditionalSettings.Instance().TransformUse = this.useTransformMeasurement.Checked;
                //watcherParam.ModelParam.TransformCollection.Param.Use = this.useTransformMeasurement.Checked;
                WatcherParam.ModelParam.TransformCollection.Param.MatchingScore = (float)this.sizeVariationScore.Value;
            }
            //AlgorithmSetting.Instance().Save();
        }

        private void SetMiscParam()
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

            Vision.LengthVariation.LengthVariationParam lengthVariationParam = AlgorithmSetting.Instance().LengthVariationParam;
            if (lengthVariationParam != null)
            {
                lengthVariationParam.Use = lengthVariation.Checked;
                lengthVariationParam.Position = (float)lengthVariationPosition.Value / 100;
                lengthVariationParam.WidthUm = (float)lengthVariationWidth.Value * 1000;
            }
        }

        private void SetPattern()
        {
            if (this.OnUpdateData == true)
                return;

            List<SheetPattern> tempList = new List<SheetPattern>();

            foreach (DataGridViewRow row in regionSelector.Rows)
            {
                SheetPattern pattern = (SheetPattern)row.Tag;
                tempList.Add(pattern);
            }

            //InspectorParam.ShapeParam.PatternList.Clear();
            //InspectorParam.ShapeParam.PatternList.AddRange(tempList);
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

            bool isSuperAccount = curUser.IsMasterAccount;
            // Misc 파라메터 창
            this.tabControlParam.Tabs["Misc"].Visible = isSuperAccount;

            // 보정
            this.labelThresholdMul.Visible = fiducialThresholdMul.Visible = this.labelThresholdMulUnit.Visible = isSuperAccount;
            this.labelEdgeFinderVersion.Visible = this.edgeFinderVersion.Visible = isSuperAccount;
            this.labelSearchDirXBase.Visible = this.searchDirXBase.Visible = isSuperAccount;
            this.labelSearchHalfArea.Visible = this.searchHalfArea.Visible = isSuperAccount;
            this.labelMinBrightnessDev.Visible = this.minBrightnessDev.Visible = this.labelMinBrightnessDevUnit.Visible = isSuperAccount;

            // 트레인
            this.labelMinPatternArea.Visible = this.minPatternArea.Visible = this.labelMinPatternAreaUnit.Visible = isSuperAccount;
            this.labelGroupDirection.Visible = this.groupDirection.Visible = isSuperAccount;
            this.labelMinLineIntens.Visible = this.minLineIntens.Visible = this.labelMinLineIntensityUnit.Visible = isSuperAccount;
            this.labelKernalSize.Visible = this.kernalSize.Visible = this.labelKernalSizeUnit.Visible = isSuperAccount;
            this.labelDiffThreshold.Visible = this.diffThreshold.Visible = this.labelDiffThresholdUnit.Visible = isSuperAccount;
            this.labelSplitLargeBar.Visible = this.splitLargeBar.Visible = isSuperAccount;

            // 검사
            this.labelDetectMultiThread.Visible = this.detectMultiThread.Visible = isSuperAccount;
            this.labelDetectTimeout.Visible = this.detectTimeout.Visible = this.labelDetectTimeoutUnit.Visible = isSuperAccount;
            this.labelAdaptivePairing.Visible = this.adaptivePairing.Visible = isSuperAccount;
            this.labelFineSizeMeasure.Visible = this.fineSizeMeasure.Visible =
                this.fineSizeMeasureSizeMul.Visible = this.fineSizeMeasureThresholdMul.Visible =
                this.labelFindMeasureUnit.Visible = isSuperAccount;
            this.calcMultiData.Enabled = isSuperAccount;

            this.labelSplitLargeBar.Visible = this.splitLargeBar.Visible = isSuperAccount;
            UpdateData();
        }

        private void ParamController_ClientSizeChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            UpdateRegionSelector(true);
        }

        public void ModelChanged()
        {
            if (this.InvokeRequired)
            {
                Invoke(new ModelChangedDelegate(ModelChanged));
                return;
            }
            //DetectorParam.UpdateDefectLength();
            this.tabControlParam.SelectedTab = this.tabControlParam.Tabs[0];
            UpdateData();
        }

        public void ModelTeachDone(int camId)
        {
            UpdateData();
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

            foreach (UltraTab ultraTab in tabControlTarget.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Key);

            UpdateComboBoxBox(this.edgeFinderVersion, typeof(EEdgeFinderVersion));
            UpdateComboBoxBox(this.searchDirXBase, typeof(BaseXSearchDir));
            UpdateComboBoxBox(this.groupDirection, typeof(GroupDirection));
            UpdateComboBoxBox(this.ignoreMethod, typeof(CalculatorParam.EIgnoreMethod));
            UpdateComboBoxBox(this.criterionLength, typeof(DetectorParam.ECriterionLength));

            //this.searchDirXBase.Items.Clear();
            //Array.ForEach(Enum.GetNames(typeof(BaseXSearchDir)), f => this.searchDirXBase.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            //this.groupDirection.Items.Clear();
            //Array.ForEach(Enum.GetNames(typeof(GroupDirection)), f => this.groupDirection.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            //this.calculateEdgeMethod.Items.Clear();
            //Array.ForEach(Enum.GetNames(typeof(CalculatorParam.EEdgeFindMethod)), f => this.calculateEdgeMethod.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            //this.ignoreMethod.Items.Clear();
            //Array.ForEach(Enum.GetNames(typeof(CalculatorParam.EIgnoreMethod)), f => this.ignoreMethod.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            //this.criterionLength.Items.Clear();
            //Array.ForEach(Enum.GetNames(typeof(DetectorParam.ECriterionLength)), f => this.criterionLength.Items.Add(StringManager.GetString(this.GetType().FullName, f)));
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

        private void fiducialSearch_ValueChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            Control control = sender as Control;
            bool update = false;

            if (control != null)
                update = control.Name == fiducialSearchRangeL.Name
                    || control.Name == fiducialSearchRangeR.Name
                    || control.Name == this.searchSkipLength.Name
                    || control.Name == this.searchDirXBase.Name;

            SetFiducialFinderParam(update);
        }

        private void Train_ValueChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            SetTrainerParam();
        }

        private void Inspect_ValueChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            Control control = sender as Control;
            bool updateFigure = false;
            if (control != null)
                updateFigure = (control.Name == calcBoundaryAreaW.Name)
                    || (control.Name == calcBoundaryAreaH.Name)
                    || (control.Name == calculateEdgeWidth.Name && CalculatorParam.ModelParam.EdgeParam.EdgeFindMethod == EdgeParam.EEdgeFindMethod.Projection);

            SetInspectorParam(updateFigure);
            UpdateInspectorParamControl();
        }

        private void MiscParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMiscParam();

            if (sender is Control)
            {
                Control control = (Control)sender;
                if ((control.Name == this.lengthVariation.Name) ||
                    (control.Name == this.lengthVariationPosition.Name) ||
                    (control.Name == this.lengthVariationWidth.Name))
                    UpdateOverlayFigure();
            }
            UpdateMiscParamControl();
        }

        private void regionSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            if (regionSelector.Visible == false)
                return;

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
                RegionInfoG regionInfo = regionSelector.Rows[e.RowIndex].Tag as RegionInfoG;
                ((DataGridViewCheckBoxCell)regionSelector.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = regionInfo.Use = !regionInfo.Use;
                UpdateOverlayFigure();

                SystemManager.Instance().CurrentModel.Modified = true;
            }
            else if (e.ColumnIndex != 4)
            {
                if (regionSelector.SelectedRows.Count == 0)
                    return;

                Point viewPortLoc = Point.Empty;
                Size viewPortSize = Size.Empty;
                RegionInfoG regionInfo = regionSelector.SelectedRows[0].Tag as RegionInfoG;

                if (e.ColumnIndex == 2)
                {
                    Rectangle rectangle;
                    cellContentCnt = (cellContentCnt + 1) % 3;
                    switch (cellContentCnt)
                    {
                        default:
                        case 0:
                            viewPortLoc = regionInfo.Region.Location;
                            viewPortSize = regionInfo.Region.Size;
                            break;
                        case 1:
                            rectangle = DrawingHelper.FromCenterSize(regionInfo.Region.Location, new Size(1024, 1024));
                            viewPortLoc = rectangle.Location;
                            viewPortSize = rectangle.Size;
                            break;
                        case 2:
                            rectangle = DrawingHelper.FromCenterSize(new Point(regionInfo.Region.Left, regionInfo.Region.Bottom), new Size(1024, 1024));
                            viewPortLoc = rectangle.Location;
                            viewPortSize = rectangle.Size;
                            break;
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    AlignInfo inBarAlignElement = regionInfo.AlignInfoCollection.FirstOrDefault();
                    if (inBarAlignElement == null)
                        return;

                    viewPortLoc = DrawingHelper.Add(regionInfo.Region.Location, inBarAlignElement.SearchRect.Location);
                    viewPortSize = inBarAlignElement.SearchRect.Size;
                }

                Rectangle viewPort = new Rectangle(viewPortLoc, viewPortSize);
                if (!viewPort.IsEmpty)
                    modellerPageExtender.UpdateZoom(viewPort);
                //UpdateOverlayFigure();
            }
        }

        private void buttonRegionTest_Click(object sender, EventArgs e)
        {
            if (regionSelector.SelectedRows.Count == 0)
                return;

            RegionInfoG selRegionInfo = CalculatorParam.ModelParam.RegionInfoCollection[regionSelector.SelectedRows[0].Index];
            modellerPageExtender.TargetRegionInfo = selRegionInfo;
            modellerPageExtender.Inspect();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (regionSelector.SelectedRows.Count == 0)
                return;

            InspectionResult inspectionResult = this.modellerPageExtender.InspectionResult as InspectionResult;
            Point offsetPt = Point.Empty;
            int selIndex = regionSelector.SelectedRows[0].Index;
            if (inspectionResult != null)
                offsetPt = inspectionResult.GetOffset(selIndex);

            RegionInfoG selRegionInfo = CalculatorParam.ModelParam.RegionInfoCollection[selIndex];
            Rectangle clipRect = DrawingHelper.Offset(selRegionInfo.Region, offsetPt);
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

            ImageD regionTrainImageD, regionPatternImageD;
            using (AlgoImage trainImage = ImageBuilder.Build(TrainerBase.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey))
            {
                AlgoImage regionTrainImage = trainImage.GetSubImage(clipRect);
                regionTrainImageD = regionTrainImage.ToImageD();
                regionTrainImage.Dispose();

                TrainerBase trainer = AlgorithmPool.Instance().GetAlgorithm(TrainerBase.TypeName) as TrainerBase;
                using (AlgoImage majorPatternImage = trainer.GetMajorPatternImage(trainImage))
                {
                    AlgoImage regionPatternImage = majorPatternImage.GetSubImage(clipRect);
                    regionPatternImageD = regionPatternImage.ToImageD();
                    regionPatternImage.Dispose();
                }
            }

            RegionEditor regionEditor = new RegionEditor(regionTrainImageD, regionPatternImageD, calibration);
            regionEditor.RegionInfo = selRegionInfo;
            regionEditor.GroupDirection = TrainerParam.GroupDirection;
            regionEditor.Dock = DockStyle.Fill;
            regionEditor.ApplyAll = regionEditor_ApplyAll;

            Form form = new Form();
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Add(regionEditor);
            form.ShowDialog();

            //selRegionInfo.Dispose();
            regionEditor.RegionInfo.IgnoreInnerChip = TrainerParam.ModelParam.IgnoreInnerChip;
            regionEditor.RegionInfo.GroupDirection = TrainerParam.GroupDirection;
            regionEditor.RegionInfo.BuildInspRegion(regionTrainImageD, regionPatternImageD);
            CalculatorParam.ModelParam.RegionInfoCollection[selIndex] = regionEditor.RegionInfo;
            regionSelector.SelectedRows[0].Tag = regionEditor.RegionInfo;
            UpdateRegionSelector(false);
            UpdateOverlayFigure();

            regionTrainImageD?.Dispose();
            regionPatternImageD?.Dispose();
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

        private void patternSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (this.OnUpdateData)
                return;

            if (patternSelector.SelectedRows.Count == 0)
                return;

            SheetPatternGroup patternGroup = patternSelector.SelectedRows[0].Tag as SheetPatternGroup;
            UpdateOverlayFigure();
        }

        private void ultraButtonPatternUpdate_Click(object sender, EventArgs e)
        {
            Teach(new TrainerArgument(false, true, false, false));
        }

        private void ultraButtonRegionUpdate_Click(object sender, EventArgs e)
        {
            Teach(new TrainerArgument(false, false, true, false));
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

        private void patternSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                SheetPatternGroup patternGroup = patternSelector.Rows[e.RowIndex].Tag as SheetPatternGroup;
                patternGroup.Use = !patternGroup.Use;
                ((DataGridViewCheckBoxCell)patternSelector.Rows[e.RowIndex].Cells[1]).Value = patternGroup.Use;
            }
            else if (e.ColumnIndex == 3)
            {
                SheetPatternGroup patternGroup = patternSelector.Rows[e.RowIndex].Tag as SheetPatternGroup;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Count: {0}EA,", patternGroup.PatternList.Count));
                sb.AppendLine(string.Format("Area: {0:F2}px,", patternGroup.AverageArea));
                sb.AppendLine(string.Format("AreaRatio: {0:F2}%,", patternGroup.AverageAreaRatio));
                sb.AppendLine(string.Format("Width: {0:F2}px,", patternGroup.AverageWidth));
                sb.AppendLine(string.Format("Height: {0:F2}px,", patternGroup.AverageHeight));
                sb.AppendLine(string.Format("Waist: {0:F2}px,", patternGroup.AverageWaist));
                sb.AppendLine(string.Format("WaistRatio: {0:F2} ~ {1:F2},", patternGroup.PatternList.Min(f => f.WaistLengthRatio), patternGroup.PatternList.Max(f => f.WaistLengthRatio)));
                MessageBox.Show(sb.ToString());
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
                UpdateMonitoringSelector();
                UpdateOverlayFigure();
            }
        }

        bool onSpeedUpdate = false;
        private void grabSpeedkHz_ValueChanged(object sender, EventArgs e)
        {
            //if (onSpeedUpdate)
            //    return;

            //this.onSpeedUpdate = true;
            float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)grabSpeedkHz.Value * 1000;
            float spdMpm = grabHz * 60 / 1000 / 1000 * resUm;
            this.grabSpeedMpM.Text = spdMpm.ToString("F1");


            //this.onSpeedUpdate = false;
        }

        private void grabSpeedMpM_ValueChanged(object sender, EventArgs e)
        {
            if (onSpeedUpdate)
                return;

            this.onSpeedUpdate = true;
            float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)grabSpeedMpM.Value / 60 * 1000 * 1000 / resUm;
            this.grabSpeedkHz.Text = (grabHz / 1000).ToString("F3");

            SetMiscParam();
            //AdditionalSettings.Instance().AsyncGrabMpm = (float)grabSpeedMpM.Value;

            this.onSpeedUpdate = false;
        }

        private void buttonProgressSpeed_Click(object sender, EventArgs e)
        {
            modellerPageExtender.RequestLineSpeed();
        }
    }
}
