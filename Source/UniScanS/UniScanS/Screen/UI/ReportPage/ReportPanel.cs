using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.UI.Report;

namespace UniScanG.Screen.UI.Report
{
    public partial class ReportPanel : UserControl, IReportPanel,IMultiLanguageSupport
    {
        ContextInfoForm contextInfoForm = new ContextInfoForm();

        CanvasPanel canvasPanel;
        List<DataGridViewRow> refSheetDataList = new List<DataGridViewRow>();
        bool onUpdateData = false;
        DefectType defectType = DefectType.Total;
        public ReportPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.DragMode = DragMode.Pan;
            imagePanel.Controls.Add(canvasPanel);

            canvasPanel.FigureFocused = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
        }


        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //total.Text = StringManager.GetString(total.Text);
            //labelSheetAttack.Text = StringManager.GetString(labelSheetAttack.Text);
            //labelPoleCircle.Text = StringManager.GetString(labelPoleCircle.Text);
            //labelPoleLine.Text = StringManager.GetString(labelPoleLine.Text);
            //labelDielectric.Text = StringManager.GetString(labelDielectric.Text);
            //labelShape.Text = StringManager.GetString(labelShape.Text);
            //labelPinHole.Text = StringManager.GetString(labelPinHole.Text);
            
            //sheetAttack.Text = StringManager.GetString(sheetAttack.Text);
            //poleCircle.Text = StringManager.GetString(poleCircle.Text);
            //poleLine.Text = StringManager.GetString(poleLine.Text);
            //dielectric.Text = StringManager.GetString(dielectric.Text);
            //pinHole.Text = StringManager.GetString(pinHole.Text);
            //shape.Text = StringManager.GetString(shape.Text);
        }

        public void Search(Production production)
        {
            onUpdateData = true;
            ProductionG productionG = (ProductionG)production;

            string resultPath = Path.Combine(
                PathSettings.Instance().Result,
                productionG.StartTime.ToString("yy-MM-dd"),
                productionG.Name,
                productionG.Thickness,
                productionG.Paste,
                productionG.LotNo);

            if (Directory.Exists(resultPath) == false)
                return;

            sheetList.Tag = production;

            refSheetDataList.Clear();

            ReportProgressForm loadingForm = new ReportProgressForm("Result Loading");
            loadingForm.Show(resultPath, ref refSheetDataList);
            
            onUpdateData = false;

            ShowResult();
        }

        private List<DataGridViewRow> Filtering()
        {
            List<DataGridViewRow> tempList = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in refSheetDataList)
            {
                MergeSheetResult sheetResult = (MergeSheetResult)row.Tag;
                
                MergeSheetResult tempResult = new MergeSheetResult(sheetResult.Index, sheetResult.resultPath, false);
                switch (defectType)
                {
                    case DefectType.Total:
                        tempResult.AddSheetSubResult(
                            sheetResult.SheetAttackList,
                            sheetResult.PoleLineList,
                            sheetResult.PoleCircleList,
                            sheetResult.DielectricList,
                            sheetResult.PinHoleList,
                            sheetResult.ShapeList);
                        break;
                    case DefectType.SheetAttack:
                        tempResult.SheetAttackList.AddRange(sheetResult.SheetAttackList);
                        break;
                    case DefectType.PoleLine:
                        tempResult.PoleLineList.AddRange(sheetResult.PoleLineList);
                        break;
                    case DefectType.PoleCircle:
                        tempResult.PoleCircleList.AddRange(sheetResult.PoleCircleList);
                        break;
                    case DefectType.Dielectric:
                        tempResult.DielectricList.AddRange(sheetResult.DielectricList);
                        break;
                    case DefectType.PinHole:
                        tempResult.PinHoleList.AddRange(sheetResult.PinHoleList);
                        break;
                    case DefectType.Shape:
                        tempResult.ShapeList.AddRange(sheetResult.ShapeList);
                        break;
                }
                
                if (useSize.Checked == true)
                    tempResult.AdjustSizeFilter((float)sizeMin.Value, (float)sizeMax.Value);

                DataGridViewRow dataGridViewRow = new DataGridViewRow();

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell() { Value = tempResult.Index + 1 };
                DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = tempResult.DefectNum };
                if (tempResult.DefectNum != 0)
                {
                    if (ngFilter.Checked == false)
                        continue;
                    qtyCell.Style.BackColor = Color.Red;
                }
                else
                {
                    if (okFilter.Checked == false)
                        continue;

                    qtyCell.Style.BackColor = Color.LightGreen;
                }
                    

                dataGridViewRow.Cells.Add(nameCell);
                dataGridViewRow.Cells.Add(qtyCell);
                
                dataGridViewRow.Tag = tempResult;
                
                tempList.Add(dataGridViewRow);
            }

            return tempList;
        }

        private void ShowResult()
        {
            onUpdateData = true;

            List<DataGridViewRow> tempResultList = null;
            float ngNum = 0;
            SimpleProgressForm loadingForm = new SimpleProgressForm("Filtering");
            loadingForm.Show(new Action(() =>
            {
                tempResultList = Filtering();
                foreach (DataGridViewRow row in tempResultList)
                {
                    MergeSheetResult result = (MergeSheetResult)row.Tag;
                    if (result.IsNG == true)
                        ngNum++;
                }
            }));

            if (tempResultList == null || tempResultList.Count == 0)
            {
                Clear();
                return;
            }

            sheetTotal.Text = tempResultList.Count.ToString();
            sheetNG.Text = ngNum.ToString();
            sheetRatio.Text = string.Format("{0} %", (ngNum / (float)tempResultList.Count * 100.0f));

            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(tempResultList.ToArray());
            sheetList.Sort(sheetList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            
            onUpdateData = false;

            SelectSheet();
        }

        public void Clear()
        {
            sheetTotal.Text = string.Empty;
            sheetNG.Text = string.Empty;
            sheetRatio.Text = string.Empty;

            defectImage.Image = null;
            sheetList.Rows.Clear();
            defectList.Rows.Clear();
            canvasPanel.UpdateImage(null);
        }
        
        private void UpdateImagePanel(MergeSheetResult result)
        {
            result.ImportPrevImage();

            if (result.PrevImage != null)
                canvasPanel.UpdateImage(result.PrevImage);

            List<SheetSubResult> subResultList = result.SheetSubResultList;

            canvasPanel.TempFigures.Clear();

            foreach (SheetSubResult subResult in subResultList)
                canvasPanel.TempFigures.AddFigure(subResult.GetFigure(1, SystemTypeSettings.Instance().ResizeRatio));
        }

        private void SelectSheet()
        {
            if (onUpdateData == true)
                return;

            if (sheetList.SelectedRows.Count == 0)
                return;

            onUpdateData = true;

            defectList.Rows.Clear();

            MergeSheetResult result = (MergeSheetResult)sheetList.SelectedRows[0].Tag;
            List<SheetSubResult> subResultList = result.SheetSubResultList;
            List<DataGridViewRow> defectDataList = new List<DataGridViewRow>();

            if (subResultList.Count == 0)
            {
                defectImage.Image = null;
                defectImage.Invalidate();
            }

            SimpleProgressForm loadingForm = new SimpleProgressForm("Loading");
            loadingForm.Show(new Action(() =>
            {
                ProductionG production = (ProductionG)sheetList.Tag;

                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

                string middlePath = Path.Combine("result",
                                        production.StartTime.ToString("yy-MM-dd"),
                                        production.Name,
                                        production.Thickness,
                                        production.Paste,
                                        production.LotNo,
                                        result.Index.ToString());

                int index = 0;
                foreach (SheetSubResult subResult in subResultList)
                {
                    foreach (InspectorObj inspector in server.GetInspectorList())
                    {
                        if (inspector.Info.CamIndex == subResult.CamIndex)
                        {
                            string imagePath = Path.Combine(
                                        inspector.Info.Path,
                                        middlePath,
                                        string.Format("{0}.bmp", subResult.Index));

                            if (File.Exists(imagePath) == false)
                                continue;

                            subResult.Image = (Bitmap)ImageHelper.LoadImage(imagePath);
                        }
                    }

                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = ++index };
                    DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = StringManager.GetString(this.GetType().FullName, subResult.DefectType.ToString()) };
                    dataGridViewRow.Cells.Add(indexCell);
                    dataGridViewRow.Cells.Add(typeCell);

                    dataGridViewRow.Tag = subResult;

                    defectDataList.Add(dataGridViewRow);
                }
            }));
            
            defectList.Rows.AddRange(defectDataList.ToArray());

            UpdateImagePanel(result);
            UpdateSheetDefectNum(result);
            
            onUpdateData = false;

            SelectDefect();
        }

        private void UpdateSheetDefectNum(MergeSheetResult result)
        {
            sheetAttackNum.Text = result.SheetAttackList.Count.ToString();
            poleLineNum.Text = result.PoleLineList.Count.ToString();
            poleCircleNum.Text = result.PoleCircleList.Count.ToString();
            dielectricNum.Text = result.DielectricList.Count.ToString();
            pinHoleNum.Text = result.PinHoleList.Count.ToString();
            shapeNum.Text = result.ShapeList.Count.ToString();
        }

        private void SelectDefect()
        {
            if (onUpdateData == true)
                return;

            if (defectList.SelectedRows.Count == 0)
                return;
            
            SheetSubResult sheetSubResult = (SheetSubResult)defectList.SelectedRows[0].Tag;
            
            if (sheetSubResult.Image != null)
                defectImage.Image = sheetSubResult.Image;
        }

        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            SelectDefect();
        }

        private void defectList_Click(object sender, EventArgs e)
        {
            SelectDefect();
        }

        private void buttonCam_Click(object sender, EventArgs e)
        {
            if (sheetList.SelectedRows.Count == 0)
                return;
        }

        public void Initialize() { }
        
        private void sheetList_Click(object sender, EventArgs e)
        {
            SelectSheet();
        }

        private void sheetList_SelectionChanged(object sender, EventArgs e)
        {
            SelectSheet();
        }

        private void total_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Total;

            ShowResult();
        }

        private void sheetAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.SheetAttack;
            ShowResult();
        }

        private void poleLine_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PoleLine;

            ShowResult();
        }

        private void poleCircle_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PoleCircle;

            ShowResult();
        }

        private void dielectric_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Dielectric;

            ShowResult();
        }

        private void pinHole_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PinHole;

            ShowResult();
        }

        private void shape_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Shape;

            ShowResult();
        }

        private void sizeMin_ValueChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void sizeMax_ValueChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void useSize_CheckedChanged(object sender, EventArgs e)
        {
            sizeMin.Enabled = useSize.Checked;
            sizeMax.Enabled = useSize.Checked;

            ShowResult();
        }

        private void okFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void ngFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult();
        }
    }
}
