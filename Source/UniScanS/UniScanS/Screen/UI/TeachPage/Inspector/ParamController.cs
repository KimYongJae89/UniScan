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
using UniScanG.Screen.Vision.Detector;
using UniScanG.Vision;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.Vision.FiducialFinder;
using UniScanG.Screen.Vision;
using UniScan.Common;
using UniScanG.Screen.UI.Teach;
using UniScanG.UI;
using UniScanG.UI.TeachPage.Inspector;

namespace UniScanG.Screen.UI.TeachPage.Inspector
{
    public enum ParamTabKey
    {
        Spec, Shape, Region, Master, Developer
    }

    public partial class ParamController : UserControl, IModellerControl, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        private SheetInspectorParam InspectorParam
        {
            get { return (SheetInspectorParam)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName).Param; }
        }

        private SheetTrainerParam TrainerParam
        {
            get { return (SheetTrainerParam)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName).Param; }
        }

        private FiducialFinderParam FiducialFinderParam
        {
            get { return (FiducialFinderParam)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName).Param; }
        }

        bool updataData = false;

        ModellerPageExtender modellerPageExtender;

        public ParamController()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
            
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
        }
                     
        public void SetModellerExtender(UniEye.Base.UI.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = (ModellerPageExtender)modellerPageExtender;
        }
        
        delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }
            
            UpdateInspectorParamControl();
            UpdateTrainerParamControl();
            UpdateFiducialFinderParamControl();
            UpdateAlgorithmSetting();
            UpdateRegionInfoGridView();
            UpdateFiducialPatternSelector();
            UpdatePatternSelector();
        }
        
        private void UpdateInspectorParamControl()
        {
            updataData = true;

            usePoleLower.Checked = InspectorParam.PoleParam.UseLower;
            usePoleUpper.Checked = InspectorParam.PoleParam.UseUpper;
            poleLowerThreshold.Value = InspectorParam.PoleParam.LowerThreshold;
            poleUpperThreshold.Value = InspectorParam.PoleParam.UpperThreshold;

            useDielectricLower.Checked = InspectorParam.DielectricParam.UseLower;
            useDielectricUpper.Checked = InspectorParam.DielectricParam.UseUpper;
            dielectricLowerThreshold.Value = InspectorParam.DielectricParam.LowerThreshold;
            dielectricUpperThreshold.Value = InspectorParam.DielectricParam.UpperThreshold;

            useShape.Checked = InspectorParam.ShapeParam.UseInspect;
            diffTolerance.Value = (decimal)InspectorParam.ShapeParam.DiffTolerence;
            UseHeightDiffTolerance.Checked = InspectorParam.ShapeParam.UseHeightDiffTolerence;
            heightDiffTolerance.Value = (decimal)InspectorParam.ShapeParam.HeightDiffTolerence;
            minPatternArea.Value = InspectorParam.ShapeParam.MinPatternArea;

            heightDiffTolerance.Enabled = InspectorParam.ShapeParam.UseHeightDiffTolerence;

            updataData = false;
        }

        private void UpdateTrainerParamControl()
        {
            updataData = true;

            groupTolerance.Value = (decimal)TrainerParam.GroupThreshold;
            patternMaxGap.Value = TrainerParam.PatternMaxGap;

            dielectricRecommendLowerThreshold.Text = TrainerParam.DielectricRecommendLowerTh.ToString();
            dielectricRecommendUpperThreshold.Text = TrainerParam.DielectricRecommendUpperTh.ToString();
            poleRecommendLowerThreshold.Text = TrainerParam.PoleRecommendLowerTh.ToString();
            poleRecommendUpperThreshold.Text = TrainerParam.PoleRecommendUpperTh.ToString();

            updataData = false;
        }

        private void UpdateFiducialFinderParamControl()
        {
            updataData = true;

            searchRangeHalfWidth.Value = FiducialFinderParam.SearchRangeHalfWidth;
            searchRangeHalfHeight.Value = FiducialFinderParam.SearchRangeHalfHeight;
            minScore.Value = FiducialFinderParam.MinScore;

            updataData = false;
        }

        private void UpdateAlgorithmSetting()
        {
            updataData = true;

            layoutFiducialSelector.Visible = AlgorithmSetting.Instance().IsFiducial;

            sheetAttackMinSize.Value = AlgorithmSetting.Instance().SheetAttackMinSize;
            poleMinSize.Value = AlgorithmSetting.Instance().PoleMinSize;
            dielectricMinSize.Value = AlgorithmSetting.Instance().DielectricMinSize;
            pinHoleMinSize.Value = AlgorithmSetting.Instance().PinHoleMinSize;

            poleRecommendLowerThWeight.Value = AlgorithmSetting.Instance().PoleLowerWeight;
            poleRecommendUpperThWeight.Value = AlgorithmSetting.Instance().PoleUpperWeight;
            dielectricRecommendLowerThWeight.Value = AlgorithmSetting.Instance().DielectricLowerWeight;
            dielectricRecommendUpperThWeight.Value = AlgorithmSetting.Instance().DielectricUpperWeight;

            maxDefectNum.Value = AlgorithmSetting.Instance().MaxDefectNum;

            gridColNum.Value = AlgorithmSetting.Instance().GridColNum;
            gridRowNum.Value = AlgorithmSetting.Instance().GridRowNum;
            
            removalNum.Value = AlgorithmSetting.Instance().RemovalNum;

            xPixelCal.Value =  (decimal)AlgorithmSetting.Instance().XPixelCal;
            yPixelCal.Value = (decimal)AlgorithmSetting.Instance().YPixelCal;

            IsFiducial.Checked = AlgorithmSetting.Instance().IsFiducial;

            poleClassification.Value = (decimal)AlgorithmSetting.Instance().PoleCompactness;
            dielectricClassification.Value = (decimal)AlgorithmSetting.Instance().DielectricCompactness;

            updataData = false;
        }

        private void UpdateRegionInfoGridView()
        {
            updataData = true;

            regionInfoGridView.Rows.Clear();
            
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();

            int index = 0;
            foreach (RegionInfo regionInfo in InspectorParam.RegionInfoList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = index++ });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = regionInfo.Region.X });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = regionInfo.Region.Y });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = regionInfo.Region.Width });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = regionInfo.Region.Height });
                row.Tag = regionInfo;
                rowList.Add(row);
            }

            regionInfoGridView.Rows.AddRange(rowList.ToArray());

            updataData = false;
        }

        private void UpdateFiducialPatternSelector()
        {
            updataData = true;

            fiducialPatternSelector.Rows.Clear();
            
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            int index = 1;
            foreach (FiducialPattern fiducialPattern in FiducialFinderParam.FidPatternList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = index++ });
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = fiducialPattern.Pattern.PatternImage.ToBitmap() };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                row.Cells.Add(imageCell);
                row.Tag = fiducialPattern;
                row.Height = fiducialPatternSelector.Height / 2;
                rowList.Add(row);
            }

            fiducialPatternSelector.Rows.AddRange(rowList.ToArray());

            updataData = false;
        }

        private void UpdatePatternSelector()
        {                   
            updataData = true;

            total.Text = InspectorParam.ShapeParam.PatternList.Count.ToString();

            patternSelector.Rows.Clear();

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            foreach (SheetPattern pattern in InspectorParam.ShapeParam.PatternList)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = pattern.BitmapImage };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                row.Cells.Add(imageCell);
                row.Cells.Add(new DataGridViewCheckBoxCell() { Value = pattern.NeedInspect });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = pattern.PatternGroup.NumPattern });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = pattern.ToString() });
                row.Tag = pattern;
                row.Height = 300;
                rowList.Add(row);
            }

            patternSelector.Rows.AddRange(rowList.ToArray());

            updataData = false;
        }
        
        private void SetInspectorParam()
        {
            if (updataData == true)
                return;

            InspectorParam.PoleParam.UseLower = usePoleLower.Checked;
            InspectorParam.PoleParam.UseUpper = usePoleUpper.Checked;
            InspectorParam.PoleParam.LowerThreshold = (int)poleLowerThreshold.Value;
            InspectorParam.PoleParam.UpperThreshold = (int)poleUpperThreshold.Value;

            InspectorParam.DielectricParam.UseLower = useDielectricLower.Checked;
            InspectorParam.DielectricParam.UseUpper = useDielectricUpper.Checked;
            InspectorParam.DielectricParam.LowerThreshold = (int)dielectricLowerThreshold.Value;
            InspectorParam.DielectricParam.UpperThreshold = (int)dielectricUpperThreshold.Value;

            InspectorParam.ShapeParam.UseInspect = useShape.Checked;
            InspectorParam.ShapeParam.DiffTolerence = (float)diffTolerance.Value;
            InspectorParam.ShapeParam.UseHeightDiffTolerence = UseHeightDiffTolerance.Checked;
            InspectorParam.ShapeParam.HeightDiffTolerence = (float)heightDiffTolerance.Value;
            InspectorParam.ShapeParam.MinPatternArea = (int)minPatternArea.Value;

            heightDiffTolerance.Enabled = InspectorParam.ShapeParam.UseHeightDiffTolerence;
        }

        private void SetTrainerParam()
        {
            if (updataData == true)
                return;

            TrainerParam.GroupThreshold = (float)groupTolerance.Value;
            TrainerParam.PatternMaxGap = (int)patternMaxGap.Value;
        }

        private void SetFiducialFinderParam()
        {
            if (updataData == true)
                return;

            FiducialFinderParam.MinScore = (int)minScore.Value;
            FiducialFinderParam.SearchRangeHalfWidth = (int)searchRangeHalfWidth.Value;
            FiducialFinderParam.SearchRangeHalfHeight = (int)searchRangeHalfHeight.Value;
        }

        private void SetAlgorithmSetting()
        {
            if (updataData == true)
                return;

            AlgorithmSetting.Instance().SheetAttackMinSize = (int)sheetAttackMinSize.Value;
            AlgorithmSetting.Instance().PoleMinSize = (int)poleMinSize.Value;
            AlgorithmSetting.Instance().DielectricMinSize = (int)dielectricMinSize.Value;
            AlgorithmSetting.Instance().PinHoleMinSize = (int)pinHoleMinSize.Value;

            AlgorithmSetting.Instance().PoleLowerWeight = (int)poleRecommendLowerThWeight.Value;
            AlgorithmSetting.Instance().PoleUpperWeight = (int)poleRecommendUpperThWeight.Value;
            AlgorithmSetting.Instance().DielectricLowerWeight = (int)dielectricRecommendLowerThWeight.Value;
            AlgorithmSetting.Instance().DielectricUpperWeight = (int)dielectricRecommendUpperThWeight.Value;

            AlgorithmSetting.Instance().MaxDefectNum = (int)maxDefectNum.Value;

            AlgorithmSetting.Instance().GridColNum = (int)gridColNum.Value;
            AlgorithmSetting.Instance().GridRowNum = (int)gridRowNum.Value;
            
            AlgorithmSetting.Instance().RemovalNum = (int)removalNum.Value;

            AlgorithmSetting.Instance().XPixelCal = (float)xPixelCal.Value;
            AlgorithmSetting.Instance().YPixelCal = (float)yPixelCal.Value;

            AlgorithmSetting.Instance().IsFiducial = IsFiducial.Checked;

            AlgorithmSetting.Instance().DielectricCompactness = (float)dielectricClassification.Value;
            AlgorithmSetting.Instance().PoleCompactness = (float)poleClassification.Value;
        }

        private void SetRegionInfo()
        {
            if (updataData == true)
                return;

            foreach (DataGridViewRow row in regionInfoGridView.Rows)
            {
                RegionInfo regionInfo = (RegionInfo)row.Tag;
                regionInfo.Region = new Rectangle((int)row.Cells[1].Value, (int)row.Cells[2].Value, (int)row.Cells[3].Value, (int)row.Cells[4].Value);
            }
        }

        private void SetFiducialPattern()
        {
            if (updataData == true)
                return;

            List<FiducialPattern> tempList = new List<FiducialPattern>();

            foreach (DataGridViewRow row in fiducialPatternSelector.Rows)
            {
                FiducialPattern fiducialPattern = (FiducialPattern)row.Tag;
                tempList.Add(fiducialPattern);
            }

            FiducialFinderParam.FidPatternList.Clear();
            FiducialFinderParam.FidPatternList.AddRange(tempList);
        }

        private void SetPattern()
        {
            if (updataData == true)
                return;

            List<SheetPattern> tempList = new List<SheetPattern>();

            foreach (DataGridViewRow row in patternSelector.Rows)
            {
                SheetPattern pattern = (SheetPattern)row.Tag;
                tempList.Add(pattern);
            }

            InspectorParam.ShapeParam.PatternList.Clear();
            InspectorParam.ShapeParam.PatternList.AddRange(tempList);
        }

        private void poleLowerThreshold_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void poleUpperThreshold_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void dielectricLowerThreshold_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void dielectricUpperThreshold_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void sheetAttackMinSize_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void poleMinSize_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void dielectricMinSize_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void pinHoleMinSize_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void diffTolerance_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void heightDiffTolerance_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void minPatternArea_ValueChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }


        private void groupTolerance_ValueChanged(object sender, EventArgs e)
        {
            SetTrainerParam();
        }

        private void patternMaxGap_ValueChanged(object sender, EventArgs e)
        {
            SetTrainerParam();
        }

        private void usePoleLower_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void usePoleUpper_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void useDielectricLower_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void useDielectricUpper_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void useShape_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void poleRecommendLowerThWeight_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void poleRecommendUpperThWeight_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void dielectricRecommendLowerThWeight_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void dielectricRecommendUpperThWeight_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void minScore_ValueChanged(object sender, EventArgs e)
        {
            SetFiducialFinderParam();
        }

        private void searchRangeHalfWidth_ValueChanged(object sender, EventArgs e)
        {
            SetFiducialFinderParam();
        }

        private void searchRangeHalfHeight_ValueChanged(object sender, EventArgs e)
        {
            SetFiducialFinderParam();
        }

        private void maxDefectNum_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void gridColNum_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void gridRowNum_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void xPixelCal_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void yPixelCal_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void removalNum_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void resizeRatio_ValueChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
        }

        private void UseHeightDiffTolerance_CheckedChanged(object sender, EventArgs e)
        {
            SetInspectorParam();
        }

        private void IsFiducial_CheckedChanged(object sender, EventArgs e)
        {
            SetAlgorithmSetting();
            UpdateData();
        }

        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            tabControlParam.Tabs["Developer"].Visible = UserHandler.Instance().CurrentUser.Id == "developer";
            tabControlParam.Tabs["Master"].Visible = UserHandler.Instance().CurrentUser.SuperAccount;
        }

        private void regionInfoGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (regionInfoGridView.SelectedRows.Count > 0)
            {
                RegionInfo regionInfo = (RegionInfo)regionInfoGridView.SelectedRows[0].Tag;

                if (regionInfo != null)
                    modellerPageExtender.SelectedRegionInfo(regionInfo);
            }
        }

        private void ParamController_ClientSizeChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            UpdateFiducialPatternSelector();
            UpdatePatternSelector();
        }
        
        private void patternSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex != 1)
                return;
            
            if (patternSelector.SelectedRows.Count > 0)
            {
                SheetPattern pattern = (SheetPattern)patternSelector.SelectedRows[0].Tag;

                if (pattern != null)
                    pattern.NeedInspect = !pattern.NeedInspect;
            }
        }

        private void patternSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (patternSelector.SelectedRows.Count > 0)
            {
                SheetPattern pattern = (SheetPattern)patternSelector.SelectedRows[0].Tag;

                if (pattern != null)
                    modellerPageExtender.SelectedPattern(pattern);
            }
        }
        
        private void fiducialPatternSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (fiducialPatternSelector.SelectedRows.Count > 0)
            {
                FiducialPattern pattern = (FiducialPattern)fiducialPatternSelector.SelectedRows[0].Tag;

                if (pattern != null)
                    modellerPageExtender.SelectedFiducialPattern(pattern);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in patternSelector.SelectedRows)
            {
                SheetPattern pattern = (SheetPattern)row.Tag;
                InspectorParam.ShapeParam.PatternList.Remove(pattern);

                patternSelector.Rows.Remove(row);
            }
        }

        public void ModelChanged()
        {
            UpdateData();
        }

        public void ModelTeachDone()
        {
            UpdateData();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            // Infragistics Tab Controls
            foreach (string tabKey in Enum.GetNames(typeof(ParamTabKey)))
                tabControlParam.Tabs[tabKey].Text = StringManager.GetString(this.GetType().FullName, tabControlParam.Tabs[tabKey].Text);

        }
        
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            bool useExist = false;
            foreach (SheetPattern pattern in InspectorParam.ShapeParam.PatternList)
            {
                if (pattern.NeedInspect == true)
                {
                    useExist = true;
                    break;
                }
            }

            foreach (SheetPattern pattern in InspectorParam.ShapeParam.PatternList)
                pattern.NeedInspect = !useExist;

            UpdatePatternSelector();
        }

        private void deleteFiducialPatternButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in fiducialPatternSelector.SelectedRows)
            {
                FiducialPattern pattern = (FiducialPattern)row.Tag;
                FiducialFinderParam.FidPatternList.Remove(pattern);

                fiducialPatternSelector.Rows.Remove(row);
            }
        }

        private void poleClassification_ValueChanged(object sender, EventArgs e)
        {
            trackBarPoleClassification.Value = (int)((float)poleClassification.Value * 10.0f);
            SetAlgorithmSetting();
        }

        private void dielectricClassification_ValueChanged(object sender, EventArgs e)
        {
            trackBarDielectricClassification.Value = (int)((float)dielectricClassification.Value * 10.0f);
            SetAlgorithmSetting();
        }

        private void trackBarPoleClassification_Scroll(object sender, EventArgs e)
        {
            poleClassification.Value = (decimal)(trackBarPoleClassification.Value / 10.0f);
            SetAlgorithmSetting();
        }

        private void trackBarDielectricClassification_Scroll(object sender, EventArgs e)
        {
            dielectricClassification.Value = (decimal)(trackBarDielectricClassification.Value / 10.0f);
            SetAlgorithmSetting();
        }
    }
}
