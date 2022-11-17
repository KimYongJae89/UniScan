using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Util;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision;
using UniScanS.UI.Etc;
using UniScanS.UI.Teach;
using UniScanS.UI.Teach.Inspector;
using UniScanS.Vision;
using UniScanS.Vision.FiducialFinder;

namespace UniScanS.UI.Teach.Inspector
{
    public partial class ImageController : UserControl, IModellerControl, IMultiLanguageSupport, IModelListener
    {
        ModellerPageExtender modellerPageExtender;
        CanvasPanel canvasPanel;
        SheetResult curSheetResult = new SheetResult();

        ContextInfoForm contextInfoForm = new ContextInfoForm();

        DefectType selectedDefectType = DefectType.Total;
        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();

        public ImageController()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel();
            canvasPanel.TabIndex = 0;
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.SetPanMode();
            canvasPanel.FigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;

            imageContanier.Controls.Add(canvasPanel);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void SetModellerExtender(ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender;

            this.modellerPageExtender.UpdateImage = UpdateImage;
            this.modellerPageExtender.UpdateFigure = UpdateFigure;

            this.modellerPageExtender.UpdateSheetResult = UpdateSheetResult;
            this.modellerPageExtender.UpdateFiducialResult = UpdateFiducialResult;
            this.modellerPageExtender.ExportData = ExportData;
            this.modellerPageExtender.ParamChanged = ParamChanged;
        }

        public void ParamChanged()
        {
            this.layoutFiducialResult.Visible = InspectorSetting.Instance().IsFiducial;
        }

        public void UpdateImage(ImageD grabImage, bool zoomfit)
        {
            Bitmap bitmap = grabImage?.ToBitmap();
            canvasPanel.UpdateImage(bitmap);

            if (this.modellerPageExtender.ImageUpdated != null)
                this.modellerPageExtender.ImageUpdated(bitmap);

            bitmap?.Dispose();
            //ImageD rescale= grabImage.Resize(0.1f, 0.1f);
            //Bitmap bitmap = rescale.ToBitmap();
            //canvasPanel.UpdateImage(bitmap);
            //bitmap.Dispose();
            //rescale.Dispose();
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

            //canvasPanel.TempFigures.Clear();
            canvasPanel.Invalidate(false);
        }

        private void buttonDeleteFigure_Click(object sender, System.EventArgs e)
        {
            //canvasPanel.ClearFigure();

            //canvasPanel..Save("d:\\1.bmp");
        }

        private void UpdateFigure(FigureGroup figureGroup)
        {
            canvasPanel.TempFigures.Clear();
            canvasPanel.TempFigures.AddFigure(figureGroup);
            canvasPanel.Invalidate(false);
        }

        delegate void UpdateSheetResultDelegate(SheetResult sheetResult);
        private void UpdateSheetResult(SheetResult sheetResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateSheetResultDelegate(UpdateSheetResult), sheetResult);
                return;
            }

            dataGridViewRowList.Clear();
            inspectTime.Text = sheetResult.SpandTime.ToString("ss\\.fff");

            if (sheetResult.SheetErrorType == SheetErrorType.None)
            {
                if (sheetResult.SheetSubResultList.Count == 0)
                {
                    labelSheetResult.BackColor = Colors.Good;
                    labelSheetResult.Text = "Good";
                }
                else
                {
                    labelSheetResult.BackColor = Colors.NG;
                    labelSheetResult.Text = "NG";
                }
            }
            else if (sheetResult.SheetErrorType == SheetErrorType.FiducialNG)
            {
                labelSheetResult.BackColor = Colors.NG;
                labelSheetResult.Text = sheetResult.SheetErrorType.ToString();
            }
            else
            {
                labelSheetResult.BackColor = Colors.Idle;
                labelSheetResult.Text = sheetResult.SheetErrorType.ToString();
            }



            foreach (SheetSubResult sheetSubResult in sheetResult.SheetSubResultList)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = StringManager.GetString(this.GetType().FullName, sheetSubResult.DefectType.ToString()) };

                switch (sheetSubResult.DefectType)
                {
                    case DefectType.SheetAttack:
                        typeCell.Style.ForeColor = Color.Maroon;
                        break;
                    case DefectType.Pole:
                        typeCell.Style.ForeColor = Color.Red;
                        break;
                    case DefectType.Dielectric:
                        typeCell.Style.ForeColor = Color.Blue;
                        break;
                    case DefectType.PinHole:
                        typeCell.Style.ForeColor = Color.DarkMagenta;
                        break;
                    case DefectType.Shape:
                        typeCell.Style.ForeColor = Color.DarkGreen;
                        break;
                }

                row.Cells.Add(typeCell);

                row.Cells.Add(new DataGridViewTextBoxCell() { Value = sheetSubResult.ToString() });
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = sheetSubResult.Image };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                row.Cells.Add(imageCell);
                row.Height = defectList.Height / 5;
                row.Tag = sheetSubResult;

                dataGridViewRowList.Add(row);
            }

            ShowResult();
        }

        delegate void UpdateFiducialResultDelegate(FiducialFinderAlgorithmResult fiducialResult);
        private void UpdateFiducialResult(FiducialFinderAlgorithmResult fiducialResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateFiducialResultDelegate(UpdateFiducialResult), fiducialResult);
                return;
            }

            fiducialResultInspectTime.Text = fiducialResult.SpandTime.ToString("ss\\.fff");

            if (fiducialResultImage.Image != null)
            {
                fiducialResultImage.Image.Dispose();
                fiducialResultImage.Image = null;
            }

            canvasPanel.BackgroundFigures.Clear();
            canvasPanel.BackgroundFigures.AddFigure(fiducialResult.GetFigure());

            this.fiducialResult.BackColor = fiducialResult.Good == true ? Colors.Good : Colors.NG;
            this.fiducialResult.Text = (fiducialResult.Score * 100.0f).ToString("f2");
            fiducialResultOffsetX.Text = fiducialResult.OffsetFound.Width.ToString("f2");
            fiducialResultOffsetY.Text = fiducialResult.OffsetFound.Height.ToString("f2");
            fiducialResultImage.Image = fiducialResult.FoundImage;
        }

        private void ShowResult()
        {
            SimpleProgressForm lodingForm = new SimpleProgressForm("Filtering");

            defectList.Rows.Clear();

            List<DataGridViewRow> tempDataGridViewRowList = new List<DataGridViewRow>();
            lodingForm.Show(() =>
            {
                canvasPanel.WorkingFigures.Clear();

                foreach (DataGridViewRow dataGridViewRow in dataGridViewRowList)
                {
                    SheetSubResult sheetSubResult = (SheetSubResult)dataGridViewRow.Tag;
                    if (sheetSubResult.DefectType == selectedDefectType || selectedDefectType == DefectType.Total)
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
                }

                foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
                {
                    SheetSubResult sheetSubResult = (SheetSubResult)dataGridViewRow.Tag;
                    canvasPanel.WorkingFigures.AddFigure(sheetSubResult.GetFigure(100));
                }
            });

            totalDefectNum.Text = tempDataGridViewRowList.Count.ToString();
            defectList.Rows.Clear();
            defectList.Rows.AddRange(tempDataGridViewRowList.ToArray());

            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }

        private void defectList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (defectList.SelectedRows.Count > 0)
            {
                SheetSubResult sheetSubResult = (SheetSubResult)defectList.SelectedRows[0].Tag;

                canvasPanel.ZoomRange(Rectangle.Inflate(sheetSubResult.Region, 100, 100));

                canvasPanel.TempFigures.Clear();
                // canvasPanel.TempFigures.AddFigure(sheetSubResult.GetFigure(10));
                canvasPanel.Invalidate(false);
                canvasPanel.Update();
            }
        }

        private void ExportData()
        {
            if (defectList.Rows.Count <= 0)
                return;

            string folderPath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, DateTime.Now.ToString("yy-MM-dd HH-mm-ss"));

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
            sb.Append(SheetResult.AppendPostString());

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            int index = 1;
            foreach (DataGridViewRow row in defectList.Rows)
            {
                SheetSubResult sheetSubResult = (SheetSubResult)row.Tag;
                sheetSubResult.Index = index;
                sheetSubResult.CamIndex = client.GetCamIndex() + 1;
                sb.AppendFormat("{0}\t{1}", index, sheetSubResult.ToExportString());
                sb.AppendLine();
                string imagePath = Path.Combine(folderPath, string.Format("{0}.bmp", index));
                index++;
                ImageHelper.SaveImage(sheetSubResult.Image, imagePath);
            }

            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(folderPath);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

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

        private void typeTotal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Total;
            ShowResult();
        }

        private void typeSheetAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.SheetAttack;
            ShowResult();
        }

        private void typePoleLine_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void typeDielectric_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Dielectric;
            ShowResult();
        }

        private void typePinHole_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.PinHole;
            ShowResult();
        }

        private void typeShape_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Shape;
            ShowResult();
        }

        private void typePole_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Pole;
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

        delegate void ModelChangedDelegate();
        public void ModelChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelChangedDelegate(ModelChanged));
                return;
            }

            labelSheetResult.Text = "Result";
            labelSheetResult.BackColor = Colors.Idle;

            canvasPanel.WorkingFigures.Clear();

            defectList.Rows.Clear();
        }

        public void ModelTeachDone() { }
    }
}