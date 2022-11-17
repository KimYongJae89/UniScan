using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Settings;
using UniScanS.Common.Util;
using UniScanS.Data;
using UniScanS.Screen.Data;
using UniScanS.UI.Etc;
using UniScanS.UI.Report;

namespace UniScanS.Screen.UI.Report
{
    public partial class ReportPanel : UserControl, IReportPanel, IMultiLanguageSupport
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        ContextInfoForm contextInfoForm = new ContextInfoForm();
        List<CheckBox> checkBoxCamList = new List<CheckBox>();

        CanvasPanel canvasPanel;
        List<DataGridViewRow> refSheetDataList = new List<DataGridViewRow>();
        bool onUpdateData = false;
        bool onMouseDown = false;

        DefectType defectType = DefectType.Total;
        public ReportPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel(true, System.Drawing.Drawing2D.InterpolationMode.Bilinear);
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.SetPanMode();
            imagePanel.Controls.Add(canvasPanel);

            canvasPanel.FigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;

            InitCheckBoxCam();

            saveFileDialog.DefaultExt = "png";
            saveFileDialog.Filter = "png files(*.png) | *.png";
            saveFileDialog.RestoreDirectory = true;
        }

        void InitCheckBoxCam()
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList();
                for (int i = inspectorList.Count - 1; i >= 0; i--)
                {
                    CheckBox checkBoxNewCam = new CheckBox();
                    checkBoxNewCam.Appearance = checkBoxCam.Appearance;
                    checkBoxNewCam.AutoSize = checkBoxCam.AutoSize;
                    checkBoxNewCam.Checked = checkBoxCam.Checked;
                    checkBoxNewCam.Dock = checkBoxCam.Dock;
                    checkBoxNewCam.FlatAppearance.BorderSize = checkBoxCam.FlatAppearance.BorderSize;
                    checkBoxNewCam.FlatAppearance.CheckedBackColor = checkBoxCam.FlatAppearance.CheckedBackColor;
                    checkBoxNewCam.FlatAppearance.MouseDownBackColor = checkBoxCam.FlatAppearance.MouseDownBackColor;
                    checkBoxNewCam.FlatAppearance.MouseOverBackColor = checkBoxCam.FlatAppearance.MouseOverBackColor;
                    checkBoxNewCam.FlatStyle = checkBoxCam.FlatStyle;
                    checkBoxNewCam.Margin = checkBoxCam.Margin;
                    checkBoxNewCam.Name = "checkBoxCam" + (inspectorList[i].Info.CamIndex + 1).ToString();
                    checkBoxNewCam.Size = checkBoxCam.Size;
                    checkBoxNewCam.TabIndex = 0;
                    checkBoxNewCam.Text = (inspectorList[i].Info.CamIndex + 1).ToString();
                    checkBoxNewCam.TextAlign = checkBoxCam.TextAlign;
                    checkBoxNewCam.UseVisualStyleBackColor = checkBoxCam.UseVisualStyleBackColor;
                    checkBoxNewCam.CheckedChanged += CamButton_CheckedChanged;
                    checkBoxNewCam.Tag = inspectorList[i];
                    checkBoxCamList.Add(checkBoxNewCam);
                    panelSelectCam.Controls.Add(checkBoxNewCam);
                }
            }
        }

        private void CamButton_CheckedChanged(object sender, System.EventArgs e)
        {
            ShowResult();
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

        public void Search(ProductionBase production)
        {
            onUpdateData = true;
            ProductionS productionG = (ProductionS)production;

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
            loadingForm.Show(productionG.Total, resultPath, ref refSheetDataList);

            onUpdateData = false;

            ShowResult();
        }

        private void ShowProductionInfo()
        {
            if (sheetList.Tag == null)
                return;

            ProductionS productionG = (ProductionS)sheetList.Tag;

            modelName.Text = productionG.Name;
            modelThickness.Text = productionG.Thickness.ToString();
            modelPaste.Text = productionG.Paste.ToString();

            productionLotName.Text = productionG.LotNo;
            productionStartTime.Text = productionG.StartTime.ToString("yy-MM-dd HH:mm");
            productionEndTime.Text = productionG.LastUpdateTime.ToString("yy-MM-dd HH:mm");

            gradeDefectTotal.Text = productionG.TotalDefectNum.ToString();
            gradeDefectSheet.Text = productionG.TotalDefectPatternNum.ToString();

            gradeSheetAttackTotal.Text = productionG.SheetAttackNum.ToString();
            gradeSheetAttackSheet.Text = productionG.SheetAttackPatternNum.ToString();

            gradePoleTotal.Text = productionG.PoleNum.ToString();
            gradePoleSheet.Text = productionG.PolePatternNum.ToString();

            gradeDielectricTotal.Text = productionG.DielectricNum.ToString();
            gradeDielectricSheet.Text = productionG.DielectricPatternNum.ToString();

            gradePinHoleTotal.Text = productionG.PinHoleNum.ToString();
            gradePinHoleSheet.Text = productionG.PinHolePatternNum.ToString();

            gradeShapeTotal.Text = productionG.ShapeNum.ToString();
            gradeShapeSheet.Text = productionG.ShapePatternNum.ToString();

            sheetTotal.Text = productionG.SheetIndex.ToString();
        }

        private List<DataGridViewRow> Filtering()
        {
            List<DataGridViewRow> tempList = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in refSheetDataList)
            {
                MergeSheetResult sheetResult = (MergeSheetResult)row.Tag;

                MergeSheetResult tempResult = new MergeSheetResult(sheetResult.Index, sheetResult.ResultPath, false);
                switch (defectType)
                {
                    case DefectType.Total:
                        tempResult.AddSheetSubResult(
                            sheetResult.SheetAttackList,
                            sheetResult.PoleList,
                            sheetResult.DielectricList,
                            sheetResult.PinHoleList,
                            sheetResult.ShapeList);
                        break;
                    case DefectType.SheetAttack:
                        tempResult.SheetAttackList.AddRange(sheetResult.SheetAttackList);
                        break;
                    case DefectType.Pole:
                        tempResult.PoleList.AddRange(sheetResult.PoleList);
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

                foreach (CheckBox checkBox in checkBoxCamList)
                {
                    if (checkBox.Checked == false)
                    {
                        InspectorObj obj = (InspectorObj)checkBox.Tag;

                        tempResult.SheetAttackList.RemoveAll(subResult => subResult.CamIndex == obj.Info.CamIndex + 1);
                        tempResult.PoleList.RemoveAll(subResult => subResult.CamIndex == obj.Info.CamIndex + 1);
                        tempResult.DielectricList.RemoveAll(subResult => subResult.CamIndex == obj.Info.CamIndex + 1);
                        tempResult.PinHoleList.RemoveAll(subResult => subResult.CamIndex == obj.Info.CamIndex + 1);
                        tempResult.ShapeList.RemoveAll(subResult => subResult.CamIndex == obj.Info.CamIndex + 1);
                    }
                }

                DataGridViewRow dataGridViewRow = new DataGridViewRow();

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell() { Value = tempResult.Index };
                dataGridViewRow.Cells.Add(nameCell);

                if (sheetResult.SheetErrorType != SheetErrorType.None)
                {
                    if (errorFilter.Checked == false)
                        continue; ;

                    DataGridViewTextBoxCell errorTypeCell = new DataGridViewTextBoxCell() { Value = sheetResult.SheetErrorType.ToString() };
                    errorTypeCell.Style.BackColor = Colors.Idle;

                    dataGridViewRow.Cells.Add(errorTypeCell);
                }
                else
                {
                    DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = tempResult.DefectNum };
                    if (tempResult.IsNG == true)
                    {
                        if (ngFilter.Checked == false)
                            continue;

                        qtyCell.Style.BackColor = Colors.NG;
                    }
                    else
                    {
                        if (okFilter.Checked == false)
                            continue;

                        qtyCell.Style.BackColor = Colors.Good;
                    }

                    dataGridViewRow.Cells.Add(qtyCell);
                }

                dataGridViewRow.Tag = tempResult;

                tempList.Add(dataGridViewRow);
            }

            return tempList;
        }

        private void ShowResult()
        {
            onUpdateData = true;

            Clear();

            ShowProductionInfo();

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

            if (tempResultList == null)
                return;

            sheetNG.Text = ngNum.ToString();
            sheetRatio.Text = tempResultList.Count == 0 ? "0 %" : string.Format("{0:0.00} %", ((float)ngNum / (float)refSheetDataList.Count * 100.0f));

            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(tempResultList.ToArray());
            sheetList.Sort(sheetList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            onUpdateData = false;

            SelectSheet();
        }

        public void Clear()
        {
            productionLotName.Text = string.Empty;
            productionStartTime.Text = string.Empty;
            productionEndTime.Text = string.Empty;

            sheetTotal.Text = string.Empty;
            sheetNG.Text = string.Empty;
            sheetRatio.Text = string.Empty;

            defectImage.Image = null;
            sheetList.Rows.Clear();
            defectList.Rows.Clear();
            canvasPanel.Clear();
            canvasPanel.UpdateImage(null);

            lenght.Text = string.Empty;
            lower.Text = string.Empty;
            upper.Text = string.Empty;
            elongation.Text = string.Empty;
            compactness.Text = string.Empty;

            area.Text = string.Empty;
            width.Text = string.Empty;
            height.Text = string.Empty;
            centerX.Text = string.Empty;
            centerY.Text = string.Empty;
        }

        private void UpdateImagePanel(MergeSheetResult result)
        {
            if (result.PrevImage == null)
                result.ImportPrevImage();

            if (result.PrevImage != null)
                canvasPanel.UpdateImage(result.PrevImage);

            List<SheetSubResult> subResultList = result.SheetSubResultList;
        }

        private void LoadSheetResult(List<MergeSheetResult> sheetResultList)
        {
            List<DataGridViewRow> defectDataList = new List<DataGridViewRow>();

            SimpleProgressForm loadingForm = new SimpleProgressForm("Loading");
            loadingForm.Show(new Action(() =>
            {
                ProductionS production = (ProductionS)sheetList.Tag;
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

                foreach (MergeSheetResult result in sheetResultList)
                {
                    string middlePath = Path.Combine("result",
                                            production.StartTime.ToString("yy-MM-dd"),
                                            production.Name,
                                            production.Thickness,
                                            production.Paste,
                                            production.LotNo,
                                            result.Index.ToString());

                    foreach (SheetSubResult subResult in result.SheetSubResultList)
                    {
                        foreach (InspectorObj inspector in server.GetInspectorList())
                        {
                            if (inspector.Info.CamIndex == subResult.CamIndex - 1)
                            {
                                string imagePath = Path.Combine(
                                            inspector.Info.Path,
                                            middlePath,
                                            string.Format("{0}.bmp", subResult.Index));

                                if (File.Exists(imagePath) == false)
                                    continue;

                                subResult.ImagePath = imagePath;

                                //subResult.Image = (Bitmap)ImageHelper.LoadImage(imagePath);
                            }
                        }

                        DataGridViewRow dataGridViewRow = new DataGridViewRow();
                        DataGridViewTextBoxCell camIndexCell = new DataGridViewTextBoxCell() { Value = subResult.CamIndex };
                        DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell();// { Value = subResult.Index };
                        if (sheetResultList.Count == 0)
                            indexCell.Value = subResult.Index;
                        else
                            indexCell.Value = result.Index;

                        DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = StringManager.GetString(this.GetType().FullName, subResult.DefectType.ToString()) };
                        dataGridViewRow.Cells.Add(camIndexCell);
                        dataGridViewRow.Cells.Add(indexCell);
                        dataGridViewRow.Cells.Add(typeCell);

                        dataGridViewRow.Tag = subResult;

                        defectDataList.Add(dataGridViewRow);
                    }
                }
            }));

            defectList.Rows.AddRange(defectDataList.ToArray());
            defectList.Sort(defectList.Columns[1], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void SelectSheet()
        {
            if (onUpdateData == true)
                return;

            if (sheetList.SelectedRows.Count == 0)
                return;

            onUpdateData = true;

            defectList.Rows.Clear();

            List<MergeSheetResult> sheetResultList = new List<MergeSheetResult>();
            foreach (DataGridViewRow row in sheetList.SelectedRows)
                sheetResultList.Add((MergeSheetResult)row.Tag);

            LoadSheetResult(sheetResultList);

            if (sheetResultList.Count > 0)
                UpdateImagePanel(sheetResultList[0]);

            canvasPanel.WorkingFigures.Clear();

            foreach (SheetResult sheetResult in sheetResultList)
                foreach (SheetSubResult subResult in sheetResult.SheetSubResultList)
                    canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(25, SystemTypeSettings.Instance().ResizeRatio));

            onUpdateData = false;

            SelectDefect();
        }


        private void SelectDefect()
        {
            if (onUpdateData == true)
                return;

            if (defectList.SelectedRows.Count == 0)
            {
                return;
            }


            SheetSubResult sheetSubResult = (SheetSubResult)defectList.SelectedRows[0].Tag;

            if (sheetSubResult.Image == null)
            {
                if (sheetSubResult.ImagePath != null)
                {
                    if (File.Exists(sheetSubResult.ImagePath))
                        sheetSubResult.Image = (Bitmap)ImageHelper.LoadImage(sheetSubResult.ImagePath);
                }
            }

            defectImage.Image = sheetSubResult.Image;

            if (sheetSubResult.DefectType == DefectType.Shape)
            {
                panelCommon.Visible = false;
                panelShape.Visible = true;

                ShapeResult shapeResult = (ShapeResult)sheetSubResult;
                area.Text = shapeResult.ShapeDiffValue.AreaDiff.ToString();
                width.Text = shapeResult.ShapeDiffValue.WidthDiff.ToString();
                height.Text = shapeResult.ShapeDiffValue.HeightDiff.ToString();
                centerX.Text = shapeResult.ShapeDiffValue.CenterXDiff.ToString();
                centerY.Text = shapeResult.ShapeDiffValue.CenterYDiff.ToString();
            }
            else
            {
                panelCommon.Visible = true;
                panelShape.Visible = false;

                lenght.Text = sheetSubResult.RealLength.ToString();
                lower.Text = sheetSubResult.LowerDiffValue == 0 ? string.Empty : sheetSubResult.LowerDiffValue.ToString();
                upper.Text = sheetSubResult.UpperDiffValue == 0 ? string.Empty : sheetSubResult.UpperDiffValue.ToString();
                elongation.Text = sheetSubResult.Elongation.ToString();
                compactness.Text = sheetSubResult.Compactness.ToString();
            }
            //defectWidth.Text = (sheetSubResult.RealRegion.Width * 1000).ToString();
            //defectHeight.Text = (sheetSubResult.RealRegion.Height * 1000).ToString();

            canvasPanel.TempFigures.Clear();

            canvasPanel.TempFigures.AddFigure(sheetSubResult.GetFigure(75, SystemTypeSettings.Instance().ResizeRatio, true));

            canvasPanel.Invalidate();
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

            MergeSheetResult mergeSheetResult = (MergeSheetResult)sheetList.SelectedRows[0].Tag;

            if (Directory.Exists(mergeSheetResult.ResultPath) == true)
            {
                SimpleProgressForm loadingForm = new SimpleProgressForm("Open");
                loadingForm.Show(new Action(() =>
                {
                    foreach (DataGridViewRow row in defectList.Rows)
                    {
                        SheetSubResult sheetSubResult = (SheetSubResult)row.Tag;

                        ImageHelper.SaveImage(sheetSubResult.Image, Path.Combine(mergeSheetResult.ResultPath, string.Format("{0}-{1}.bmp", sheetSubResult.CamIndex, sheetSubResult.Index)));
                    }

                    System.Diagnostics.Process.Start(mergeSheetResult.ResultPath);
                }));
            }
        }

        public void Initialize() { }

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

        private void pole_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Pole;

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

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            onUpdateData = true;
            sheetList.SelectAll();
            onUpdateData = false;

            SelectSheet();
        }

        private void errorFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void sheetList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            onMouseDown = true;
        }

        private void sheetList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (onMouseDown == false)
                return;

            SelectSheet();

            onMouseDown = false;
        }

        private void sheetList_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void sheetList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down || e.KeyData == Keys.Up)
                SelectSheet();
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "buttonCapture_Click_1");

            int bitsPerPixel = System.Windows.Forms.Screen.PrimaryScreen.BitsPerPixel;

            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            if (bitsPerPixel <= 16)
                pixelFormat = PixelFormat.Format16bppRgb565;
            else if (bitsPerPixel == 24)
                pixelFormat = PixelFormat.Format24bppRgb;

            LogHelper.Debug(LoggerType.Operation, "buttonCapture_Click_3");

            Rectangle screenRect = this.RectangleToScreen(this.DisplayRectangle);

            Bitmap bitmap = new Bitmap(screenRect.Width, screenRect.Height, pixelFormat);

            using (Graphics gr = Graphics.FromImage(bitmap))
                gr.CopyFromScreen(screenRect.X, screenRect.Y, 0, 0, screenRect.Size);

            LogHelper.Info(LoggerType.Operation, "buttonCapture_Click_4");

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                LogHelper.Debug(LoggerType.Operation, "buttonCapture_Click_5");

                bitmap.Dispose();
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "buttonCapture_Click_6");

            ImageHelper.SaveImage(bitmap, saveFileDialog.FileName);
            bitmap.Dispose();
        }
    }
}
