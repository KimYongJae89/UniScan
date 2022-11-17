using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Gravure.Vision.RCI.Trainer;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;
using UniScanG.Vision;

namespace UniScanG.UI.Teach.Inspector
{
    public partial class ImageController : UserControl, IModellerControl, IMultiLanguageSupport, IModelListener, IUserHandlerListener
    {
        public const int RescaleFactor = 2;
        ModellerPageExtender modellerPageExtender;
        CanvasPanel canvasPanel;
        //ToolTip inspectTimeToolTip = null;

        DefectType[] selectedDefectTypes = new DefectType[0];// DefectType.get.Total;
        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
        InspectionResult inspectionResult = null;
        DebugContext debugContext = null;
        PictureBox progresingPictureBox = null;

        public ImageController()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.TabIndex = 0;
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.ReadOnly = true;
            this.canvasPanel.FastMode = true;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.FigureClicked = CanvasPanel_FigureClicked;
            this.canvasPanel.MouseMove += CanvasPanel_MouseMove;
            this.imageContanier.Controls.Add(canvasPanel);

            this.progresingPictureBox = new PictureBox();
            this.progresingPictureBox.Dock = DockStyle.Fill;
            this.progresingPictureBox.Image = UniScanG.Properties.Resources.InProgress;
            this.progresingPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.progresingPictureBox.Visible = false;
            this.imageContanier.Controls.Add(this.progresingPictureBox);

            InitDefectTypeFilter();

            //this.inspectTimeToolTip = new ToolTip()
            //{
            //    AutoPopDelay = 5000,
            //    InitialDelay = 1000,
            //    ReshowDelay = 500,
            //    ShowAlways = true
            //};

            UserHandler.Instance().AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void UpdateData() { }

        private void InitDefectTypeFilter()
        {
            DefectTypeFilterPanel defectTypeFilterPanel = new DefectTypeFilterPanel(2);
            defectTypeFilterPanel.SetDefectTpyeSelectChanged(OnDefectTpyeSelectChanged);
            defectTypeFilterPanel.Dock = DockStyle.Fill;

            this.panelDefectType.Controls.Add(defectTypeFilterPanel);

            this.selectedDefectTypes = defectTypeFilterPanel.GetSelected();
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.canvasPanel.Image == null)
                return;

            Point imagePoint = this.canvasPanel.PointToImage(e.Location);
            Rectangle imageRect = new Rectangle(Point.Empty, this.canvasPanel.Image.Size);
            if (DrawingHelper.IntersectsWith(imageRect, imagePoint))
            {
                Color color = this.canvasPanel.GetPixel(imagePoint);

                Point mulPoint = DrawingHelper.Mul(imagePoint, RescaleFactor);
                this.x.Text = mulPoint.X.ToString();
                this.y.Text = mulPoint.Y.ToString();
                this.v.Text = DrawingHelper.ToGreyByte(color).ToString();
            }
            else
            {
                this.x.Text = this.y.Text = this.v.Text = "";
            }
            //e.Location
        }

        private void CanvasPanel_FigureClicked(Figure figure, MouseEventArgs e)
        {
            this.onUpdate = true;

            if (figure.Tag is FoundedObjInPattern)
                CanvasPanel_FigureClicked((FoundedObjInPattern)figure.Tag);

            this.onUpdate = false;
        }

        private void CanvasPanel_FigureClicked(FoundedObjInPattern sheetSubResult)
        {
            this.defectList.ClearSelection();

            DataGridViewRow[] rows = this.defectList.Rows.Cast<DataGridViewRow>().ToArray();
            DataGridViewRow selectedRow = Array.Find(rows, f => f.Tag == sheetSubResult);
            selectedRow.Selected = true;

            //foreach (DataGridViewRow row in defectList.Rows)
            //    if (row.Tag == sheetSubResult)
            //        row.Selected = true;


            if (defectList.SelectedRows.Count > 0)
            {
                int totalCount = defectList.Rows.Count;
                int dispIndex = defectList.FirstDisplayedScrollingRowIndex;
                int dispCount = defectList.DisplayedRowCount(false);
                int selectIndex = defectList.SelectedRows[0].Index;
                if (selectIndex < dispIndex)
                    defectList.FirstDisplayedScrollingRowIndex = selectIndex;
                else if (dispIndex + dispCount < selectIndex)
                    defectList.FirstDisplayedScrollingRowIndex = Math.Max(0, selectIndex - dispCount) + 1;
            }
        }

        private void OnDefectTpyeSelectChanged(DefectType[] defectTypes)
        {
            this.selectedDefectTypes = defectTypes;
            ShowResult();
        }

        public void SetModellerExtender(ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender;

            this.canvasPanel.OnViewPortChanged += this.modellerPageExtender.ImageController_OnViewPortChanged;

            this.modellerPageExtender.OnProgressing += OnProgressing;
            this.modellerPageExtender.UpdateImage = UpdateImage;
            this.modellerPageExtender.UpdateFigure = UpdateFigure;
            this.modellerPageExtender.UpdateZoom = UpdateZoom;

            this.modellerPageExtender.UpdateSheetResult = UpdateSheetResult;
            this.modellerPageExtender.ExportData = ExportData;
        }

        private void OnProgressing(bool isProgressing)
        {
            if(this.InvokeRequired)
            {
                BeginInvoke(new OnProgressingDelegate(OnProgressing), isProgressing);
                return;
            }

            this.canvasPanel.Visible = !isProgressing;
            this.progresingPictureBox.Visible = isProgressing;
        }

        public void UpdateImage(ImageD grabImage, bool zoomFit)
        {
            Bitmap bitmap = null;
            Size imageSize = Size.Empty;

            if (grabImage != null)
            {
                imageSize = grabImage.Size;
                if (RescaleFactor != 1)
                {
                    AlgoImage grabAlgoImage = null;
                    AlgoImage rescaleAlgoImage = null;
                    try
                    {
                        grabAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabImage, ImageType.Grey);
                        ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(grabAlgoImage);

                        rescaleAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey,
                            new Size(grabImage.Width / RescaleFactor, grabImage.Height / RescaleFactor));
                        ip.Resize(grabAlgoImage, rescaleAlgoImage);
                        grabImage = rescaleAlgoImage.ToImageD();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Operation, string.Format("ImageController::UpdateImage - {0}", ex.Message));
                    }
                    finally
                    {
                        grabAlgoImage?.Dispose();
                        rescaleAlgoImage?.Dispose();
                    }
                }

                int size = grabImage.Width * grabImage.Height;
                if (size >= 536870912)// 2GB / 4
                {
                    int newHeight = (int)(2147483648 / 4 / grabImage.Width);
                    bitmap = grabImage.ClipImage(new Rectangle(0, 0, grabImage.Width, newHeight)).ToBitmap();
                }
                else
                {
                    bitmap = grabImage.ToBitmap();
                }
            }

            canvasPanel.WorkingFigures.Clear();
            canvasPanel.BackgroundFigures.Clear();
            canvasPanel.TempFigures.Clear();
            canvasPanel.HideFigure = false;

            zoomFit |= (canvasPanel.Image == null);
            canvasPanel.UpdateImage(bitmap);
            if (zoomFit)
                canvasPanel.ZoomFit();
            else if (canvasPanel.InvokeRequired)
                canvasPanel.Invoke(new MethodInvoker(canvasPanel.Invalidate), true);
            else
                canvasPanel.Invalidate(true);


            this.modellerPageExtender.ImageUpdated?.Invoke(imageSize);

            //bitmap?.Dispose();
        }

        private void buttonZoomIn_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomIn();
        }

        private void buttonZoomOut_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomOut();
        }

        private void buttonZoomFit_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomFit();
        }

        private void buttonDeleteFigure_Click(object sender, System.EventArgs e)
        {
            canvasPanel.HideFigure = !canvasPanel.HideFigure;
            canvasPanel.Invalidate(false);
            buttonDeleteFigure.Appearance.BackColor = canvasPanel.HideFigure ? Color.Red : SystemColors.ButtonFace;

        }

        private void UpdateFigure(FigureGroup fgFigureGroup, FigureGroup bgFigureGroup)
        {
            AlgorithmResult result = null;
            this.inspectionResult?.AlgorithmResultLDic.TryGetValue(Gravure.Vision.Calculator.CalculatorBase.TypeName, out result);
            float scaleFactor = 1.0f / ImageController.RescaleFactor;

            if (fgFigureGroup != null)
            {
                canvasPanel.WorkingFigures.Clear();

                if (result != null)
                    fgFigureGroup.Offset(result.OffsetFound);
                fgFigureGroup.Scale(scaleFactor, scaleFactor);
                canvasPanel.WorkingFigures.AddFigure(fgFigureGroup.FigureList);
            }

            if (bgFigureGroup != null)
            {
                canvasPanel.BackgroundFigures.Clear();

                //if (result != null)
                    //bgFigureGroup.Offset(result.OffsetFound);
                bgFigureGroup.Scale(scaleFactor, scaleFactor);
                canvasPanel.BackgroundFigures.AddFigure(bgFigureGroup.FigureList);
            }
            canvasPanel.Invalidate(false);
        }

        private void UpdateZoom(Rectangle viewPort)
        {
            float scaleFactor = 1f / RescaleFactor;
            //if (SystemManager.Instance().CurrentModel != null)
            //    scaleFactor *= SystemManager.Instance().CurrentModel.ScaleFactor;

            //scaleFactor = 1;

            float l = viewPort.Left * scaleFactor;
            float t = viewPort.Top * scaleFactor;
            float r = viewPort.Right * scaleFactor;
            float b = viewPort.Bottom * scaleFactor;
            RectangleF zoomRange = RectangleF.Inflate(RectangleF.FromLTRB(l, t, r, b), 100, 100);
            canvasPanel.ZoomRange(zoomRange);

            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }

        private void UpdateSheetResult(InspectionResult inspectionResult, DebugContext debugContext)
        {
            this.inspectionResult = inspectionResult;
            this.debugContext = debugContext;
            UpdateSheetResult();
        }

        private void UpdateSheetResult()
        {
            if (InvokeRequired)
            {
                //BeginInvoke(new UpdateSheetResultDelegate(UpdateSheetResult), this.inspectionResult, this.debugContext);
                BeginInvoke(new MethodInvoker(UpdateSheetResult));
                return;
            }

            dataGridViewRowList.Clear();
            this.inspectTime.Text = "";
            this.decision.Text = "";
            this.inspectTimeToolTip.RemoveAll();

            if (this.inspectionResult != null)
            {
                //StringBuilder spendTimeSb = new StringBuilder();
                foreach (AlgorithmResult algorithmResult in this.inspectionResult.AlgorithmResultLDic.Values)
                {
                    AlgorithmResultG algorithmResultG = algorithmResult as AlgorithmResultG;
                    if (algorithmResultG == null)
                        continue;

                    //spendTimeSb.AppendLine(string.Format("{0}: {1:F0}[ms]", algorithmResult.AlgorithmName, algorithmResult.SpandTime.TotalMilliseconds));
                    foreach (FoundedObjInPattern sheetSubResult in algorithmResultG.SheetSubResultList)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.Cells.Add(new DataGridViewTextBoxCell()
                        {
                            Value = sheetSubResult.GetDefectType().GetLocalString(),
                            Style = new DataGridViewCellStyle()
                            {
                                BackColor = sheetSubResult.GetColor(),
                                ForeColor = sheetSubResult.GetBgColor(),
                            },
                            ToolTipText = sheetSubResult.GetDefectTypeDiscription()
                        });

                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = sheetSubResult.ToString() });
                        row.Cells.Add(new DataGridViewImageCell()
                        {
                            Value = sheetSubResult.Image,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        });
                        row.Height = defectList.Height / 7;
                        row.Tag = sheetSubResult;

                        UpdateContextMenu(row);
                        
                        dataGridViewRowList.Add(row);
                    }
                }

                this.inspectTime.Text = this.inspectionResult.InspectionTime.TotalSeconds.ToString("F2");
                string toolTip = this.debugContext?.ToString();
                this.inspectTimeToolTip.SetToolTip(this.inspectTime, toolTip);
                
                this.decision.Text = StringManager.GetString(typeof(DynMvp.InspData.Judgment).FullName, this.inspectionResult.Judgment.ToString());
            }

            ShowResult();
        }

        bool onUpdate = false;
        private void ShowResult()
        {
            defectList.Rows.Clear();
            //if (dataGridViewRowList.Count == 0)
            //    return;

            SimpleProgressForm lodingForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Filtering"));

            List<DataGridViewRow> tempDataGridViewRowList = new List<DataGridViewRow>();
            if (this.canvasPanel != null)
            {
                canvasPanel.WorkingFigures.Clear();
                
                //Gravure.Vision.Calculator.CalculatorResult calcResult = this.inspectionResult.AlgorithmResultLDic[Gravure.Vision.Calculator.CalculatorBase.TypeName] as Gravure.Vision.Calculator.CalculatorResult;
                //FigureGroup figureGroup = new FigureGroup();
                //OffsetStructSet offsetStructSet = calcResult.OffsetSet;
                //// Pattern Offset
                //Point patterPos = DrawingHelper.Add(offsetStructSet.PatternOffset.Base, offsetStructSet.PatternOffset.Offset);
                //figureGroup.AddFigure(new LineFigure(new PointF(patterPos.X, 0), new PointF(patterPos.X, patterPos.Y), new Pen(Color.Red)));
                //figureGroup.AddFigure(new LineFigure(new PointF(0, patterPos.Y), new PointF(patterPos.X, patterPos.Y), new Pen(Color.Red)));

                //// Local Offset
                //for (int i = 0; i < offsetStructSet.LocalCount; i++)
                //{
                //    PointF pt0 = offsetStructSet.LocalOffsets[i].BaseF;
                //    PointF pt1 = DrawingHelper.Add(pt0, offsetStructSet.LocalOffsets[i].OffsetF);
                //    figureGroup.AddFigure(new LineFigure(pt0, pt1, new Pen(Color.Red)));
                //}

                //figureGroup.Scale(1.0f / ImageController.RescaleFactor);
                //canvasPanel.WorkingFigures.AddFigure(figureGroup);

                foreach (DataGridViewRow dataGridViewRow in dataGridViewRowList)
                {
                    FoundedObjInPattern sheetSubResult = (FoundedObjInPattern)dataGridViewRow.Tag;
                    DefectType dt = sheetSubResult.GetDefectType();
                    if (this.selectedDefectTypes.Contains(dt) || dt == DefectType.Transform)
                    {
                        if (useSize.Checked == true)
                        {
                            if (sheetSubResult.RealLength >= (float)sizeMin.Value && sheetSubResult.RealLength <= (float)sizeMax.Value)
                                tempDataGridViewRowList.Add(dataGridViewRow);
                        }
                        else
                        {
                            tempDataGridViewRowList.Add(dataGridViewRow);
                        }
                    }
                    //tempDataGridViewRowList.Add(dataGridViewRow);
                }

                foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
                {
                    FoundedObjInPattern sheetSubResult = (FoundedObjInPattern)dataGridViewRow.Tag;
                    canvasPanel.WorkingFigures.AddFigure(0, sheetSubResult.GetFigure(1f / RescaleFactor));
                }
            }
            //);

            totalDefectNum.Text = tempDataGridViewRowList.Count.ToString();
            onUpdate = true;
            defectList.Rows.Clear();
            defectList.Rows.AddRange(tempDataGridViewRowList.ToArray());
            defectList.ClearSelection();
            onUpdate = false;

            canvasPanel?.Invalidate(false);
            canvasPanel?.Update();
        }

        private void ExportData(string path)
        {
            if (defectList.Rows.Count <= 0)
                return;

            string folderPath = path;

            if (Directory.Exists(folderPath) == true)
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch (Exception e)
                {
                    Directory.Delete(folderPath, true);
                }
                //System.Threading.Thread.Sleep(1000);
            }

            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "Data.csv");
            StringBuilder sb = new StringBuilder();

            int index = 0;
            //foreach (DataGridViewRow row in defectList.Rows)
            //{
            //    SheetSubResult sheetSubResult =  (SheetSubResult)row.Tag;
            //    sb.AppendFormat("{1}\t{0}", index, sheetSubResult.ToExportString());
            //    sb.AppendLine();
            //    string imagePath = Path.Combine(folderPath, string.Format("{0}.bmp", index));
            //    index++;
            //    ImageHelper.SaveImage(sheetSubResult.Image, imagePath);
            //}

            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(folderPath);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            UpdateSheetResult();
            //StringManager.UpdateString(this.GetType().FullName, labelSheetAttack);
            //StringManager.UpdateString(this.GetType().FullName, labelPoleLine);
            //StringManager.UpdateString(this.GetType().FullName, labelPoleCircle);
            //StringManager.UpdateString(this.GetType().FullName, labelDielectric);
            //StringManager.UpdateString(this.GetType().FullName, labelPinHole);
            //StringManager.UpdateString(this.GetType().FullName, labelShape);
            //StringManager.UpdateString(this.GetType().FullName, labelInspectTime);
            //StringManager.UpdateString(this.GetType().FullName, labelTotalDefectNum);
            //StringManager.UpdateString(this.GetType().FullName, typeSheetAttack);

            //StringManager.UpdateString(this.GetType().FullName, labelType);
            //StringManager.UpdateString(this.GetType().FullName, typePoleCircle);
            //StringManager.UpdateString(this.GetType().FullName, typePoleLine);
            //StringManager.UpdateString(this.GetType().FullName, typeDielectric);
            //StringManager.UpdateString(this.GetType().FullName, typePinHole);
            //StringManager.UpdateString(this.GetType().FullName, typeShape);

            //StringManager.UpdateString(this.GetType().FullName, labelSize);
            //StringManager.UpdateString(this.GetType().FullName, labelMin);
            //StringManager.UpdateString(this.GetType().FullName, labelMax);

            //for (int i = 0; i < defectList.ColumnCount; i++)
            //    defectList.Columns[i].HeaderText = StringManager.GetString(this.GetType().FullName, defectList.Columns[i].HeaderText);
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

        public void ModelChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelChangedDelegate(ModelChanged));
                return;
            }

            canvasPanel.WorkingFigures.Clear();
            defectList.Rows.Clear();
        }

        public void ModelTeachDone(int camId) { }
        public void ModelRefreshed() { }

        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (defectList.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = defectList.SelectedRows[0];
            FoundedObjInPattern sheetSubResult = (FoundedObjInPattern)row.Tag;

            //canvasPanel.ZoomRange(Rectangle.Inflate(sheetSubResult.Region, 100, 100));
            float l = sheetSubResult.Region.Left / RescaleFactor;
            float t = sheetSubResult.Region.Top / RescaleFactor;
            float r = sheetSubResult.Region.Right / RescaleFactor;
            float b = sheetSubResult.Region.Bottom / RescaleFactor;
            RectangleF zoomRect = RectangleF.Inflate(RectangleF.FromLTRB(l, t, r, b), 100, 100);
            canvasPanel.ZoomRange(zoomRect);

            //canvasPanel.TempFigures.Clear();
            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }

        private void defectList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = defectList.Rows[e.RowIndex];
                FoundedObjInPattern ssr = row.Tag as FoundedObjInPattern;
                if (ssr == null)
                    return;

                DataGridViewImageCell cell = row.Cells[2] as DataGridViewImageCell;
                if (cell == null)
                    return;

                if (cell.Value == ssr.BufImage)
                    cell.Value = ssr.Image;
                else if (ssr.BufImage != null)
                    cell.Value = ssr.BufImage;
            }
        }

        private void defectList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = defectList.Rows[e.RowIndex];
                FoundedObjInPattern ssr = row.Tag as FoundedObjInPattern;
                if (ssr == null)
                    return;

                if (ssr is Gravure.Data.DefectObj)
                    return;

                DataGridViewImageCell cell = row.Cells[2] as DataGridViewImageCell;
                if (cell == null)
                    return;

                ssr.Image.Save(@"C:\temp\temp.bmp");
                System.Diagnostics.Process.Start(@"C:\temp\temp.bmp");
            }
        }

        private void ImageController_SizeChanged(object sender, EventArgs e)
        {
            this.layoutInfo.Width = this.Size.Width / 2;
        }


        public void UserChanged()
        {
            this.dataGridViewRowList.ForEach(f => UpdateContextMenu(f));
    }

        private delegate void UpdateContextMenuDelegate(DataGridViewRow row);
        private void UpdateContextMenu(DataGridViewRow row)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateContextMenuDelegate(UpdateContextMenu), row);
                return;
            }

            FoundedObjInPattern sheetSubResult = row?.Tag as FoundedObjInPattern;
            ContextMenuStrip cms = null;
            //if (UserHandler.Instance().CurrentUser.SuperAccount)
            {
                //ToolStripMenuItem tsmi = new ToolStripMenuItem("Add Ignore");
                //tsmi.Tag = sheetSubResult;
                //tsmi.Click += Tsmi_Click;

                //cms = new ContextMenuStrip();
                //cms.Items.Add(tsmi);

                cms = new ContextMenuStrip();
                cms.Items.Add(new ToolStripMenuItem("Add Ignore", null, ToolStripMenuItemAddDontcare_Click) { Tag = sheetSubResult });
                cms.Items.Add(new ToolStripMenuItem("Copy Clipboard", null, ToolStripMenuItemCopyClipboard_Click) { Tag = sheetSubResult });

                cms.Items.Add(new ToolStripMenuItem("Add Critical Point", null, ToolStripMenuItemAddCriticalPt_Click) { Tag = sheetSubResult });
                cms.Items.Add(new ToolStripMenuItem("Remove Critical Point", null, ToolStripMenuItemRemoveCriticalPt_Click) { Tag = sheetSubResult });
            }
            row.ContextMenuStrip = cms;
        }

        private void ToolStripMenuItemCopyClipboard_Click(object sender, EventArgs e)
        {
            if (this.defectList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = this.defectList.SelectedRows[0];
                Rectangle captureRect = this.defectList.GetRowDisplayRectangle(row.Index, true);
                captureRect = this.defectList.RectangleToScreen(captureRect);

                Bitmap bitmap = new Bitmap(captureRect.Width, captureRect.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                    g.CopyFromScreen(captureRect.X, captureRect.Y, 0, 0, captureRect.Size);
                Clipboard.SetImage((Image)bitmap);
            }            
        }

        private void ToolStripMenuItemAddDontcare_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            FoundedObjInPattern obj = (FoundedObjInPattern)tsmi.Tag;
            this.modellerPageExtender.AddDontcareRegion(Rectangle.Round(obj.Region));
        }

        private void ToolStripMenuItemAddCriticalPt_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            FoundedObjInPattern obj = (FoundedObjInPattern)tsmi.Tag;
            this.modellerPageExtender.AddCriticalPoint(Rectangle.Round(obj.Region));
        }

        private void ToolStripMenuItemRemoveCriticalPt_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            FoundedObjInPattern obj = (FoundedObjInPattern)tsmi.Tag;
            this.modellerPageExtender.RemoveCriticalPoint(Rectangle.Round(obj.Region));
        }

        private void inspectTime_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string toolTip = inspectTimeToolTip.GetToolTip(this.inspectTime);
            if (!string.IsNullOrEmpty(toolTip))
                System.Windows.MessageBox.Show(toolTip);
        }
    }
}