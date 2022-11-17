using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Gravure.Vision;
using UniScanG.UI.Etc;
using UniScanG.UI.Report;

namespace UniScanG.Gravure.UI.Report
{
    public partial class ReportPanel : UserControl, IReportPanel, IUserHandlerListener, IMultiLanguageSupport
    {
        List<CheckBox> checkBoxCamList = new List<CheckBox>();
        List<MergeSheetResult> overallSheetResultList = new List<MergeSheetResult>();
        List<DataGridViewRow> filterdSheetResultList = new List<DataGridViewRow>();
        RectangleF filterRect = Rectangle.Empty;

        bool onUpdateData = false;
        DefectType[] defectTypes = new DefectType[0];

        SingleSheetResultPanel singleSheetResultPanel;
        MultiSheetResultPanel multiSheetResultPanel;

        Data.ProductionG selProduction = null;
        Bitmap modelBitmap = null;

        public ReportPanel(bool showBurnerState)
        {
            InitializeComponent();
            this.columnErased.Visible = false;
            this.columnTime.Visible = false;

            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            InitCheckBoxCam();
            InitDefectTypeSelector();
            InitDefectInfo(showBurnerState);

            this.singleSheetResultPanel = new SingleSheetResultPanel();
            this.singleSheetResultPanel.Dock = DockStyle.Fill;
            this.singleSheetResultPanel.Hide();
            this.panelResult.Controls.Add(singleSheetResultPanel);

            this.multiSheetResultPanel = new MultiSheetResultPanel();
            this.multiSheetResultPanel.Dock = DockStyle.Fill;
            this.multiSheetResultPanel.Hide();
            this.panelResult.Controls.Add(multiSheetResultPanel);

            this.marginToolStripMenuItem.Visible = AlgorithmSetting.Instance().UseExtMargin;
            this.offsetToolStripMenuItem.Visible = AlgorithmSetting.Instance().UseExtTransfrom;
            UserHandler.Instance().AddListener(this);

            ColorTable.OnColorTableUpdated += UpdateControlsColor;
        }

        private void InitDefectTypeSelector()
        {
            DefectTypeFilterPanel defectTypeFilterPanel = new DefectTypeFilterPanel(1);
            defectTypeFilterPanel.Dock = DockStyle.Fill;
            this.panelSelectType.Controls.Add(defectTypeFilterPanel);

            this.defectTypes = defectTypeFilterPanel.GetSelected();
            defectTypeFilterPanel.OnDefectTpyeSelectChanged += DefectTypeFilterPanel_OnDefectTpyeSelectChanged;
        }


        private void InitDefectInfo(bool showBurnerState)
        {
            UpdateControlsColor();

            this.labelSticker.Visible = this.stickerNum.Visible = Vision.AlgorithmSetting.Instance().UseExtSticker;
            this.labelMargin.Visible = this.marginNum.Visible = Vision.AlgorithmSetting.Instance().UseExtMargin;

            this.labelSheetEraser.Visible = this.sheetEraser.Visible = showBurnerState;
        }

        private void UpdateControlsColor()
        {
            Data.ColorTable.UpdateControlColor(this.labelSheetAttack, DefectType.Attack);
            Data.ColorTable.UpdateControlColor(this.labelNoPrint, DefectType.Noprint);
            Data.ColorTable.UpdateControlColor(this.labelDielectric, DefectType.Coating);
            Data.ColorTable.UpdateControlColor(this.labelPinHole, DefectType.PinHole);
            Data.ColorTable.UpdateControlColor(this.labelSpread, DefectType.Spread);
            Data.ColorTable.UpdateControlColor(this.labelSticker, DefectType.Sticker);
            Data.ColorTable.UpdateControlColor(this.labelMargin, DefectType.Margin);
        }

        private void DefectTypeFilterPanel_OnDefectTpyeSelectChanged(DefectType[] defectTypes)
        {
            this.defectTypes = defectTypes;
            ShowResult(false);
        }

        public void UserChanged()
        {
            UpdateColumnTime();
        }

        private void UpdateColumnTime()
        {
            this.columnTime.Visible = false;

            //bool showTimes = UserHandler.Instance().CurrentUser.IsMasterAccount && (this.overallSheetResultList.TrueForAll(f => f.StartTime != DateTime.MinValue));
            //this.columnTime.Visible = showTimes;
        }

        void InitCheckBoxCam()
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0);
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
                checkBoxCamList.Reverse();
            }
        }

        private void CamButton_CheckedChanged(object sender, System.EventArgs e)
        {
            ShowResult(false);
        }


        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Search(ProductionBase production)
        {
            onUpdateData = true;

            this.selProduction = production as Data.ProductionG;
            if (this.selProduction == null)
                return;

            string resultPath = selProduction.GetResultPath();
            if (!Directory.Exists(resultPath))
            {
                MessageForm.Show(this, StringManager.GetString("Can not Find Result File."));
                return;
            }

            this.overallSheetResultList.Clear();
            bool ok = LoadResult(ref resultPath, this.overallSheetResultList);

            if (ok)
            {
                ProductionBase curProduction = SystemManager.Instance().ProductionManager.CurProduction;
                if (production != curProduction && this.overallSheetResultList.Count != this.selProduction.Total)
                {
                    this.selProduction.Reset();
                    this.selProduction.UpdateFrom(this.overallSheetResultList);
                    this.selProduction.Save();
                    SystemManager.Instance().ProductionManager.Save();
                }
            }

            this.modelBitmap = null;
            string modelPath = this.selProduction.GetModelPath();
            if (!string.IsNullOrEmpty(modelPath))
            {
                string modelImagePath = Path.Combine(modelPath, "Image", "Prev.bmp");
                if (File.Exists(modelImagePath))
                    this.modelBitmap = (Bitmap)Image.FromFile(modelImagePath);
            }

            if (this.selProduction.UpdateRequire)
            {
                this.selProduction.UpdateFrom(this.overallSheetResultList);
                SystemManager.Instance().ProductionManager.Save();
                this.selProduction.Save();
            }

            UpdateInfo();
            UpdateDefectInfo();

            onUpdateData = false;

            UpdateColumnTime();
            ShowResult(true);
        }

        private bool LoadResult(ref string resultPath, List<MergeSheetResult> list)
        {
            ReportProgressForm loadingForm = new ReportProgressForm("", true) { MaxCount = selProduction.Total };
            return loadingForm.Show(this, ref resultPath, list);
        }

        private void UpdateInfo()
        {
            productionModelName.Text = selProduction.Name;
            productionLotName.Text = selProduction.LotNo;
            productionTime.Text = $"{selProduction.StartTime.ToString("yy-MM-dd HH:mm")} ~ {selProduction.LastUpdateTime.ToString("HH:mm")}";
            productionTargetSpd.Text = selProduction.LineSpeedMpm.ToString("0.0");

            // Update Sheet Length Data
            Label[] labels = new Label[] { infoHeight1, infoHeight2, infoHeight3 };
            Array.ForEach(labels, f => f.Text = "");

            List<float> sheetHeightList = this.overallSheetResultList.ConvertAll(f => f.SheetSize.Height);
            sheetHeightList.Sort();
            int count = sheetHeightList.Count;
            int count10 = sheetHeightList.Count / 10;
            if (count10 > 0)
            {
                int[] indexs = new int[] { 0, count10, count - count10, count };
                for (int i = 0; i < 3; i++)
                {
                    int src = indexs[i];
                    int dst = indexs[i + 1];
                    int len = dst - src;
                    List<float> list = sheetHeightList.GetRange(src, len);
                    if (list.Count > 0)
                        labels[i].Text = list.Average().ToString("F3");
                }
            }

            MathHelper.StdDev(sheetHeightList.ToArray(), out double mean, out double stdDev);
            float min = sheetHeightList.Min();
            float max = sheetHeightList.Max();
            float range = max - min;
            UiHelper.SetControlText(this.infoHeightMean, mean.ToString("F3"));
            UiHelper.SetControlText(this.infoHeightStdDev, stdDev.ToString("F3"));
            UiHelper.SetControlText(this.infoHeightMin, min.ToString("F2"));
            UiHelper.SetControlText(this.infoHeightMax, max.ToString("F2"));
            UiHelper.SetControlText(this.infoHeightRange, range.ToString("F2"));
        }

        private void UpdateDefectInfo()
        {
            ProductionG productionG = this.selProduction;

            if (productionG != null)
            {
                if (patternRadioButton.Checked == true)
                {
                    int sheetAttackNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Attack));
                    sheetAttackNum.Text = string.Format("{0} / {1}", sheetAttackNums, productionG.Patterns.SheetAttack);

                    int noPrintNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Noprint));
                    noPrintNum.Text = string.Format("{0} / {1}", noPrintNums, productionG.Patterns.NoPrint);

                    int dielectricNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Coating));
                    dielectricNum.Text = string.Format("{0} / {1}", dielectricNums, productionG.Patterns.Dielectric);

                    int pinHoleNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.PinHole));
                    pinHoleNum.Text = string.Format("{0} / {1}", pinHoleNums, productionG.Patterns.PinHole);

                    int spreadNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Spread));
                    spreadNum.Text = string.Format("{0} / {1}", spreadNums, productionG.Patterns.Spread);

                    int stickerNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Sticker));
                    stickerNum.Text = string.Format("{0} / {1}", stickerNums, productionG.Patterns.Sticker);

                    int marginNums = this.filterdSheetResultList.Count(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Exists(g => g.GetDefectType() == DefectType.Margin));
                    marginNum.Text = string.Format("{0} / {1}", marginNums, productionG.Patterns.Margin);
                }
                else if (defectRadioButton.Checked == true)
                {
                    int sheetAttackNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Attack));
                    sheetAttackNum.Text = string.Format("{0} / {1}", sheetAttackNums, productionG.Defects.SheetAttack);

                    int noPrintNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Noprint));
                    noPrintNum.Text = string.Format("{0} / {1}", noPrintNums, productionG.Defects.NoPrint);

                    int dielectricNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Coating));
                    dielectricNum.Text = string.Format("{0} / {1}", dielectricNums, productionG.Defects.Dielectric);

                    int pinHoleNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.PinHole));
                    pinHoleNum.Text = string.Format("{0} / {1}", pinHoleNums, productionG.Defects.PinHole);

                    int spreadNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Spread));
                    spreadNum.Text = string.Format("{0} / {1}", spreadNums, productionG.Defects.Spread);

                    int stickerNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Sticker));
                    stickerNum.Text = string.Format("{0} / {1}", stickerNums, productionG.Defects.Sticker);

                    int marginNums = this.filterdSheetResultList.Sum(f => ((MergeSheetResult)f.Tag).SheetSubResultList.Count(g => g.GetDefectType() == DefectType.Margin));
                    marginNum.Text = string.Format("{0} / {1}", marginNums, productionG.Defects.Margin);
                }
            }
        }

        private void Filtering()
        {
            this.filterdSheetResultList.Clear();

            foreach (MergeSheetResult mergeSheetResult in overallSheetResultList)
            {
                MergeSheetResult filterdMergeSheetResult = mergeSheetResult.Clone();
                //List<FoundedObjInPattern> sheetSubResultList = new List<FoundedObjInPattern>(mergeSheetResult.SheetSubResultList);

                filterdMergeSheetResult.SheetSubResultList.RemoveAll(f => !f.ShowReport);

                // 카메라 번호 필터
                int[] selectedCams = checkBoxCamList.FindAll(f => f.Checked).Select(f => ((InspectorObj)f.Tag).Info.CamIndex).ToArray();
                filterdMergeSheetResult.SheetSubResultList.RemoveAll(f => !selectedCams.Contains(f.CamIndex));

                // 위치 필터
                this.filterRect = Rectangle.Empty;
                if (this.useRect.Checked)
                {
                    int cx = (int)(rectCX.Value / 100 * mergeSheetResult.SheetSizePx.Width);
                    int cy = (int)(rectCY.Value / 100 * mergeSheetResult.SheetSizePx.Height);
                    int w = (int)(rectW.Value / 100 * mergeSheetResult.SheetSizePx.Width);
                    int h = (int)(rectH.Value / 100 * mergeSheetResult.SheetSizePx.Height);
                    this.filterRect = DrawingHelper.FromCenterSize(new Point(cx, cy), new Size(w, h));

                    filterdMergeSheetResult.SheetSubResultList.RemoveAll(f => !f.Region.IntersectsWith(this.filterRect));
                }

                // 타입 필터
                filterdMergeSheetResult.SheetSubResultList.RemoveAll(f => !defectTypes.Contains(f.GetDefectType()));

                // 크기 필터
                if (useSize.Checked == true)
                {
                    float sizeMin = (float)this.sizeMin.Value;
                    float sizeMax = (float)this.sizeMax.Value;
                    if (sizeMax == 0)
                        sizeMax = float.MaxValue;

                    filterdMergeSheetResult.SheetSubResultList.RemoveAll(f =>
                    {
                        float length = Math.Max(f.RealRegion.Width, f.RealRegion.Height);
                        return length > sizeMax || length < sizeMin;
                    });
                }

                // 불량 필터
                filterdMergeSheetResult.SheetSubResultList.RemoveAll(f =>
                {
                    return !((ngFilter.Checked && f.IsDefect) || (okFilter.Checked && !f.IsDefect));
                });

                //filterdMergeSheetResult.SheetSubResultList.Sort((f, g) => f.Region.X.CompareTo(g.Region.X));

                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(this.sheetList);
                dataGridViewRow.Cells[0].Value = filterdMergeSheetResult.Index + 1;
                dataGridViewRow.Cells[1].Value = (filterdMergeSheetResult.StartTime - this.selProduction.StartTime).TotalSeconds;
                dataGridViewRow.Cells[2].Value = filterdMergeSheetResult.SheetSubResultList.Count;
                dataGridViewRow.Cells[3].Value = filterdMergeSheetResult.PostProcessed ? "O" : "";

                if (filterdMergeSheetResult.IsNG)
                {
                    if (ngFilter.Checked == false)
                        continue;

                    dataGridViewRow.Cells[2].Style.BackColor = Color.Red;
                }
                else
                {
                    if (okFilter.Checked == false)
                        continue;

                    dataGridViewRow.Cells[2].Style.BackColor = Color.LightGreen;
                }

                if (filterdMergeSheetResult.PostProcessed)
                    dataGridViewRow.Cells[3].Style.BackColor = Color.Red;

                dataGridViewRow.Tag = filterdMergeSheetResult;

                this.filterdSheetResultList.Add(dataGridViewRow);
            }
        }

        private void ShowResult(bool updateOverallChart)
        {
            if (selProduction == null)
                return;

            onUpdateData = true;

            SimpleProgressForm loadingForm = new SimpleProgressForm();
            loadingForm.Show(new Action(() =>
            {
                this.Invoke(new MethodInvoker(Filtering));
                //Filtering();
            }));

            int totalcount = this.overallSheetResultList.Count;
            int ngCount = this.filterdSheetResultList.Count(f => (f.Tag as MergeSheetResult).IsNG);
            //int eraseCount = tempResultList.Count(f => (f.Tag as MergeSheetResult).PostProcessed);

            UiHelper.SetControlText(this.sheetInspector, string.Format("{0} / {1}", totalcount, ngCount));

            if (selProduction.EraseGood <= 0)
                UiHelper.SetControlText(sheetEraser, string.Format("{0}", selProduction.EraseNum));
            else
                UiHelper.SetControlText(sheetEraser, string.Format("{0} / {1} ({2})", selProduction.EraseNum, selProduction.EraseGood, selProduction.EraseDuplicated));

            UiHelper.SetControlText(this.sheetRatio, string.Format("{0:F2} %", totalcount == 0 ? 0 : ngCount * 100.0f / totalcount));

            UpdateDefectInfo();

            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(this.filterdSheetResultList.ToArray());
            sheetList.Sort(sheetList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            sheetList.AutoResizeColumn(0);

            onUpdateData = false;

            if (updateOverallChart)
                this.multiSheetResultPanel.SetOverallResult(this.overallSheetResultList);

            SelectSheet(updateOverallChart);
        }

        public void Clear()
        {
            sheetInspector.Text = string.Empty;
            sheetEraser.Text = string.Empty;
            sheetRatio.Text = string.Empty;
            sheetList.Rows.Clear();

            this.singleSheetResultPanel.Clear();
            this.singleSheetResultPanel.Hide();

            this.multiSheetResultPanel.Clear();
            multiSheetResultPanel.Hide();
        }

        private void SelectSheet(bool zoomFit)
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            List<MergeSheetResult> mergeSheetResultList = new List<MergeSheetResult>();
            foreach (DataGridViewRow row in sheetList.SelectedRows)
            {
                MergeSheetResult mergeSheetResult = (MergeSheetResult)row.Tag;
                mergeSheetResult.ImportImage();
                //if (!mergeSheetResult.IsImported)
                //{
                //    mergeSheetResult.SheetSubResultList.Clear();
                //    mergeSheetResult.Import(null);
                //}
                mergeSheetResultList.Add(mergeSheetResult);
            }

            if (mergeSheetResultList.Count == 0)
            {
                this.singleSheetResultPanel.Hide();
                //this.multiSheetResultPanel.Hide();
                this.multiSheetResultPanel.SelectSheet(this.modelBitmap, mergeSheetResultList);
            }
            //else if (mergeSheetResultList.Count == 1)
            //{
            //    this.multiSheetResultPanel.Hide();
            //    this.singleSheetResultPanel.Show();
            //    this.singleSheetResultPanel.SelectSheet(mergeSheetResultList[0]);
            //}
            else
            {
                this.singleSheetResultPanel.Hide();
                this.multiSheetResultPanel.Show();
                this.multiSheetResultPanel.SelectSheet(this.modelBitmap, mergeSheetResultList);
                if(zoomFit)
                    this.multiSheetResultPanel.ZoomFit();
                this.multiSheetResultPanel.SetFilterRect(this.filterRect);
            }

            onUpdateData = false;
        }

        private void openFolder_Click(object sender, EventArgs e)
        {

        }

        public void Initialize() { }

        private void sheetList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdateData)
                return;

            SelectSheet(false);
        }

        private void useSize_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult(false);
        }

        private void size_ValueChanged(object sender, EventArgs e)
        {
            if (!useSize.Checked)
                return;
            ShowResult(false);
        }

        private void okFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult(false);
        }

        private void ngFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult(false);
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            sheetList.SelectAll();
        }

        private void buttonDeselect_Click(object sender, EventArgs e)
        {
            sheetList.ClearSelection();
        }

        private void collect_Click(object sender, EventArgs e)
        {
            //IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            //List<InspectorObj> inspectorObjList = server.GetInspectorList();
            //int inspectorCnt = inspectorObjList.Count;
            //int camCnt = inspectorObjList.FindAll(f => f.Info.ClientIndex <= 0).Count;
            //string[] selecetedPath = new string[inspectorCnt];

            //for (int i=0; i<4; i++)
            //{
            //    FolderBrowserDialog dlg = new FolderBrowserDialog();
            //    if (i > 0)
            //    {
            //        string selectedPath2 = selecetedPath[i - 1].Replace(inspectorObjList[i - 1].Info.Path, inspectorObjList[i].Info.Path);
            //        dlg.SelectedPath = selectedPath2;
            //        if (Directory.Exists(selectedPath2))
            //            selecetedPath[i] = dlg.SelectedPath;
            //    }

            //    if (string.IsNullOrEmpty(selecetedPath[i]))
            //    {
            //        dlg.SelectedPath = Path.Combine(inspectorObjList[i].Info.Path, "Result");
            //        if (dlg.ShowDialog() == DialogResult.Cancel)
            //            return;
            //        selecetedPath[i] = dlg.SelectedPath;
            //    }
            //}

            //List<Tuple<int,string>>[] sheetList = new List<Tuple<int, string>>[camCnt];
            //for (int i = 0; i < camCnt; i++)
            //    sheetList[i] = new List<Tuple<int, string>>();

            //SimpleProgressForm loadingForm = new SimpleProgressForm("Loading");
            //loadingForm.Show(() =>
            //{
            //    Parallel.For(0, inspectorCnt, i =>
            //     {
            //         int camIdx = inspectorObjList[i].Info.CamIndex;
            //         string[] subResultPaths = Directory.GetDirectories(selecetedPath[i]);
            //         Array.ForEach(subResultPaths, f =>
            //         {
            //             string sheetName = Path.GetFileName(f);
            //             int sheetNo;
            //             bool ok = int.TryParse(sheetName, out sheetNo);
            //             if (ok)
            //             {
            //                 lock (sheetList[camIdx])
            //                     sheetList[camIdx].Add(new Tuple<int, string>(sheetNo, f));
            //             }
            //         });
            //     });
            //});

            //Array.ForEach(sheetList, f => f.Sort());
            //int[] idxPointer = new int[camCnt];

            //while (true)
            //{
            //    int sheetNo = sheetList[0][idxPointer[0]].Item1;
            //    int minSheetIdx = -1;
            //    for (int camIdx = 1; camIdx < camCnt; camIdx++)
            //    {
            //        int idx = idxPointer[camIdx];
            //        int sheetNo2 =  sheetList[camIdx][idx].Item1;
            //        if(sheetNo == sheetNo2)
            //        {
            //            GOGOGOGOGO();
            //        }
            //        else
            //        {
            //            int incIdx = sheetNo < sheetNo2 ? 0 : 1;
            //            idxPointer[incIdx]++;
            //        }
            //    }

            //    if(Array)
            //}

        }

        private void buttonExportSheet_Click(object sender, EventArgs e)
        {
            if (sheetList.SelectedRows.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("No Sheet Selected"));
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string path = Path.Combine(dlg.SelectedPath, string.Format("Export_{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            Directory.CreateDirectory(path);

            List<MergeSheetResult> mergeSheetResultList = new List<MergeSheetResult>();
            foreach (DataGridViewRow row in sheetList.SelectedRows)
                mergeSheetResultList.Add(row.Tag as MergeSheetResult);
            mergeSheetResultList.RemoveAll(f => f == null);

            Rectangle[] highlights = this.multiSheetResultPanel.GetHighlightPosition();

            ExportSheetDefectData(path, mergeSheetResultList, highlights);

            Process.Start(path);
        }

        private void ExportSheetDefectData(string path, List<MergeSheetResult> exportTargetList, Rectangle[] highlights)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() =>
            {
                foreach (MergeSheetResult mergeSheetResult in exportTargetList)
                {
                    // CSV 저장
                    string sheetPath = Path.Combine(path, mergeSheetResult.Index.ToString());
                    mergeSheetResult.Export(sheetPath, cts.Token);

                    // 불량 이미지 저장
                    if (false)
                    {
                        string report = Path.Combine(sheetPath, "Report");
                        Directory.CreateDirectory(report);
                        List<FoundedObjInPattern> list = mergeSheetResult.SheetSubResultList;
                        for (int i = 0; i < list.Count; i++)
                        {
                            Bitmap newBitmap = new Bitmap(list[i].Image);
                            ImageHelper.SaveImage(newBitmap, Path.Combine(report, string.Format("I{0}_W{1}_H{2}.jpg", i, list[i].RealRegion.Width, list[i].RealRegion.Height)));
                            newBitmap.Dispose();
                        }
                    }

                    
                    // 전체 Marking 이미지 저장
                    if (false && mergeSheetResult.PrevImage != null)
                    {
                        float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
                        //Bitmap bitmap32 = new Bitmap(mergeSheetResult.PrevImage);
                        Bitmap bitmap24 = ImageHelper.MakeColor24(mergeSheetResult.PrevImage);
                        ImageD imageD = Image2D.FromBitmap(bitmap24);
                        using (AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Color))
                        {
                            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                            Rectangle allHighlights = highlights.FirstOrDefault();
                            if(highlights.Length>1)
                                allHighlights = highlights.Aggregate((f, g) => Rectangle.Union(f, g));
                            Array.ForEach(highlights, f =>
                            {
                                Rectangle drawRect = Rectangle.Round(DrawingHelper.Mul(f, resizeRatio));
                                drawRect.Inflate(5, 5);
                                ip.DrawRotateRact(algoImage, new RotatedRect(drawRect, 45), Color.Red.ToArgb(), false);
                            });

                            mergeSheetResult.SheetSubResultList.ForEach(f =>
                            {
                                Rectangle drawRect = Rectangle.Round(DrawingHelper.Mul(f.Region, resizeRatio));
                                drawRect.Inflate(3, 3);
                                ip.DrawRect(algoImage, drawRect, f.GetColor().ToArgb(), false);
                                ip.DrawText(algoImage, new Point(drawRect.Right + 2, drawRect.Top), f.GetColor().ToArgb(), f.Index.ToString("000"));
                            });
                            
                            using (ImageD saveImageD = algoImage.ToImageD())
                                saveImageD.SaveImage(Path.Combine(sheetPath, "Prev_Mark.jpg"));

                            algoImage.Save(Path.Combine(path, string.Format("Prev_{0:00}_Mark.png", mergeSheetResult.Index)));
                            if (!allHighlights.IsEmpty)
                            {
                                Rectangle subRect = Rectangle.Round(DrawingHelper.Mul(allHighlights, resizeRatio));
                                subRect.Inflate(50, 50);
                                subRect.Intersect(new Rectangle(Point.Empty, algoImage.Size));
                                AlgoImage subAlgoImage = algoImage.GetSubImage(subRect);
                                subAlgoImage.Save(Path.Combine(path, string.Format("Summary_{0:00}_Mark.png", mergeSheetResult.Index)));
                                subAlgoImage.Dispose();
                            }

                        }
                        imageD.Dispose();
                        bitmap24.Dispose();
                        //bitmap32.Dispose();
                    }
                }
            }, cts);
        }


        private void buttonExportPartialProjection_Click(object sender, EventArgs e)
        {
            if (this.overallSheetResultList.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("There is no Data to Export"));
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = string.Format("Projection_{0}_{1}_{2}.csv", selProduction.Name, selProduction.LotNo, selProduction.StartTime.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "CSV file (*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;

            Dictionary<int, float[][]> partialProjectionDic = this.overallSheetResultList.ToDictionary(f => f.Index, f => f.PartialProjection);
            ExportPartialProjection(filePath, partialProjectionDic);

            System.Diagnostics.Process.Start("Explorer.exe", $"/select, \"{filePath}\"");
        }

        private void ExportPartialProjection(string filePath, Dictionary<int, float[][]> partialProjectionDic)
        {
            int max = partialProjectionDic.Max(f => f.Value.Max(g => g.Length));
            StringBuilder[] stringBuilders = new StringBuilder[max + 2];
            for (int i = 0; i < stringBuilders.Length; i++)
            {
                stringBuilders[i] = new StringBuilder();
                switch (i)
                {
                    case 0:
                        stringBuilders[i].AppendFormat("{0}, ", "Pattern");
                        break;
                    case 1:
                        stringBuilders[i].AppendFormat("{0}, ", "IM");
                        break;
                    case 2:
                        stringBuilders[i].AppendFormat("{0}, ", "Datas");
                        break;
                    default:
                        stringBuilders[i].AppendFormat("{0}, ", "");
                        break;
                }
            }

            foreach (KeyValuePair<int, float[][]> pair in partialProjectionDic)
            {
                stringBuilders[0].AppendFormat("{0}, ", pair.Key);

                for (int i = 0; i < pair.Value.Length; i++)
                {
                    stringBuilders[1].AppendFormat("{0}, ", i);
                    for (int j = 0; j < pair.Value[i].Length; j++)
                        stringBuilders[j + 2].AppendFormat("{0}, ", pair.Value[i][j]);
                }
            }

            StringBuilder sb = new StringBuilder();
            Array.ForEach(stringBuilders, f => sb.AppendLine(f.ToString()));
            File.WriteAllText(filePath, sb.ToString());
        }

        private void buttonExportLength_Click(object sender, EventArgs e)
        {
            if (this.overallSheetResultList.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("There is no Data to Export"));
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = string.Format("LEGNTH_{0}_{1}_{2}.csv", selProduction.Name, selProduction.LotNo, selProduction.StartTime.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "CSV file (*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;

            Dictionary<int, float> sheetLengthDic = this.overallSheetResultList.ToDictionary(f => f.Index, f => f.SheetSize.Height);
            ExportSheetLengthData(filePath, sheetLengthDic);
            System.Diagnostics.Process.Start("Explorer.exe", $"/select, \"{filePath}\"");
        }

        private void ExportSheetLengthData(string filePath, Dictionary<int, float> sheetLengthDic)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("PatternNo,Length");
            foreach (KeyValuePair<int, float> pair in sheetLengthDic)
                sb.AppendLine(string.Format("{0},{1}", pair.Key, pair.Value));

            File.WriteAllText(filePath, sb.ToString());
        }

        private void buttonExportMargin_Click(object sender, EventArgs e)
        {
            if (this.overallSheetResultList.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("There is no Data to Export"));
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = string.Format("MARGIN_{0}_{1}_{2}.csv", selProduction.Name, selProduction.LotNo, selProduction.StartTime.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "CSV file (*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;

            Dictionary<int, List<MarginObj>> marginDic = this.overallSheetResultList.ToDictionary(f => f.Index, f => f.SheetSubResultList.FindAll(g => g is MarginObj).ConvertAll(g => (MarginObj)g));
            ExportMarginData(filePath, marginDic);
            System.Diagnostics.Process.Start("Explorer.exe", $"/select, \"{filePath}\"");
        }

        private void ExportMarginData(string filePath, Dictionary<int, List<MarginObj>> marginDic)
        {
            // Column 만들기
            List<string> columnList = new List<string>();
            foreach (KeyValuePair<int, List<MarginObj>> pair in marginDic)
            {
                int patternNo = pair.Key;

                List<MarginObj> objList = pair.Value;
                objList.ForEach(f =>
                {
                    string displayName = f.GetDisplayName();
                    int columnIdx = columnList.IndexOf(displayName);
                    if (columnIdx < 0)
                        columnList.Add(displayName);
                });
            }

            string header = string.Join(",,", columnList);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("[um],{0}", header));
            sb.Append("Pattern");
            columnList.ForEach(f => sb.Append(",Width,Length"));
            sb.AppendLine();

            foreach (KeyValuePair<int, List<MarginObj>> pair in marginDic)
            {
                string[] rows = new string[columnList.Count * 2 + 1];
                rows[0] = pair.Key.ToString();

                List<MarginObj> objList = pair.Value;
                if (objList.Count == 0)
                    continue;

                objList.ForEach(f =>
                {
                    string displayName = f.GetDisplayName();
                    int columnIdx = columnList.IndexOf(displayName);
                    if (columnIdx >= 0)
                    {
                        rows[2 * columnIdx + 1] = f.RealWidth.ToString("F01");
                        rows[2 * columnIdx + 2] = f.RealHeight.ToString("F01");
                    }
                });
                string row = string.Join(",", rows);
                sb.AppendLine(row);
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        private void buttonExportOffset_Click(object sender, EventArgs e)
        {
            if (this.overallSheetResultList.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("There is no Data to Export"));
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = string.Format("TRANSFORM_{0}_{1}_{2}.csv", selProduction.Name, selProduction.LotNo, selProduction.StartTime.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "CSV file (*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;

            Dictionary<int, List<OffsetObj>> marginDic = this.overallSheetResultList.ToDictionary(f => f.Index, f => f.SheetSubResultList.FindAll(g => g is OffsetObj).ConvertAll(g => (OffsetObj)g));
            ExportTransform(filePath, marginDic);
            System.Diagnostics.Process.Start("Explorer.exe", $"/select, \"{filePath}\"");
        }

        //private void ExportTransform(string filePath, Dictionary<int, List<OffsetObj>> offsetDic)
        //{
        //    float scale = 3f / 4f;
        //    // Name으로 Column 만들기
        //    List<Tuple<string, float, float>> offsetList = new List<Tuple<string, float, float>>();
        //    List<string> columnStringTemp = new List<string>();
        //    foreach (List<OffsetObj> value in offsetDic.Values)
        //    {
        //        columnStringTemp.AddRange(value.Select(f => f.Name));
        //        offsetList.AddRange(value.Select(f => new Tuple<string, float, float>(f.Name, f.MatchingOffsetUm.Width * scale, f.MatchingOffsetUm.Height * scale)));
        //    }
        //    columnStringTemp.Sort();

        //    // 중복된 이름 삭제
        //    List<string> columnStringList = new List<string>();
        //    while (columnStringTemp.Count > 0)
        //    {
        //        string columnString = columnStringTemp[0];
        //        columnStringList.Add(columnString);
        //        columnStringTemp.RemoveAll(f => f == columnString);
        //    }

        //    // offset값 생성
        //    IGrouping<string, Tuple<string, float, float>>[] groups = offsetList.GroupBy(f => f.Item1).ToArray();
        //    Dictionary<string, Tuple<float, float>> meanDic = new Dictionary<string, Tuple<float, float>>();
        //    Array.ForEach(groups, f => meanDic.Add(f.Key, new Tuple<float, float>(f.Average(g => g.Item2), f.Average(g => g.Item3))));


        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(string.Format("[um],{0}", string.Join(",,,", columnStringList)));

        //    sb.Append("");
        //    columnStringList.ForEach(f => sb.Append(",X,Y,"));
        //    sb.AppendLine();

        //    // Position
        //    sb.Append("Position");
        //    columnStringList.ForEach(f =>
        //    {
        //        List<OffsetObj> offsetObjList = offsetDic.Values.FirstOrDefault(g => g.Exists(h => h.Name == f));
        //        RectangleF realRegion = offsetObjList.Find(g => g.Name == f).RealRegion;
        //        PointF centerPoint = DrawingHelper.CenterPoint(realRegion);
        //        sb.Append(string.Format(",{0},{1},", centerPoint.X, centerPoint.Y));
        //    });
        //    sb.AppendLine();

        //    // Offset
        //    sb.Append("Offset");
        //    columnStringList.ForEach(f =>
        //    {
        //        sb.Append(string.Format(",{0:F03},{1:F03},", meanDic[f].Item1, meanDic[f].Item2));
        //    });
        //    sb.AppendLine();
        //    sb.AppendLine();

        //    // dx-dy
        //    sb.Append("Pattern");
        //    columnStringList.ForEach(f => sb.Append(",dx,dy,Score"));
        //    sb.AppendLine();

        //    // data
        //    foreach (KeyValuePair<int, List<OffsetObj>> pair in offsetDic)
        //    {
        //        int patNo = pair.Key;
        //        sb.Append(patNo);

        //        columnStringList.ForEach(f =>
        //        {
        //            OffsetObj offsetObj = pair.Value.Find(g => g.Name == f);
        //            if (offsetObj == null)
        //            {
        //                sb.Append(",,,");
        //            }
        //            else
        //            {
        //                float dx = offsetObj.MatchingOffsetUm.Width * scale - meanDic[f].Item1;
        //                float dy = offsetObj.MatchingOffsetUm.Height * scale - meanDic[f].Item2;
        //                sb.Append(string.Format(",{0},{1},{2}", dx, dy, offsetObj.Score));
        //            }
        //        });
        //        sb.AppendLine();
        //    }

        //    File.WriteAllText(filePath, sb.ToString());
        //    Process.Start(filePath);
        //}

        private void ExportTransform(string filePath, Dictionary<int, List<OffsetObj>> offsetDic)
        {
            Dictionary<int, SortedList<string, Tuple<float, float, float>>> exportItemDic = new Dictionary<int, SortedList<string, Tuple<float, float, float>>>();
            List<string> columnList = new List<string>();
            foreach (KeyValuePair<int, List<OffsetObj>> pair in offsetDic)
            {
                int patNo = pair.Key;
                if (!exportItemDic.ContainsKey(patNo))
                    exportItemDic.Add(patNo, new SortedList<string, Tuple<float, float, float>>());

                SortedList<string, Tuple<float, float, float>> list = exportItemDic[patNo];
                foreach (OffsetObj offsetObj in pair.Value)
                {
                    string objName = offsetObj.Name;
                    if (list.ContainsKey(objName))
                        continue;

                    float dx = offsetObj.MatchingOffsetUm.Width;
                    float dy = offsetObj.MatchingOffsetUm.Height;
                    float score = offsetObj.Score;

                    Tuple<float, float, float> tuple = new Tuple<float, float, float>(dx, dy, score);
                    list.Add(objName, tuple);

                    if (!columnList.Contains(objName))
                    {
                        columnList.Add(objName);
                        columnList.Sort();
                    }
                }
            }

            int columnItemCnt = columnList.Count;
            foreach (KeyValuePair<int, SortedList<string, Tuple<float, float, float>>> pair in exportItemDic)
            {
                int patNo = pair.Key;
                SortedList<string, Tuple<float, float, float>> dataList = pair.Value;
                foreach (string columnItem in columnList)
                {
                    if (!dataList.ContainsKey(columnItem))
                        dataList.Add(columnItem, new Tuple<float, float, float>(float.NaN, float.NaN, float.NaN));
                }
                Debug.Assert(dataList.Count == columnItemCnt);
            }

            Dictionary<string, List<Tuple<float, float, float>>> newDic = new Dictionary<string, List<Tuple<float, float, float>>>();
            foreach (KeyValuePair<int, SortedList<string, Tuple<float, float, float>>> pair in exportItemDic)
            {
                SortedList<string, Tuple<float, float, float>> list = pair.Value;
                foreach (KeyValuePair<string, Tuple<float, float, float>> pair2 in list)
                {
                    if (!newDic.ContainsKey(pair2.Key))
                        newDic.Add(pair2.Key, new List<Tuple<float, float, float>>());
                    //if (newDic.ContainsKey(pair2.Key))
                    newDic[pair2.Key].Add(pair2.Value);
                }
            }

            Dictionary<string, Tuple<float, float, float>> meanDic = new Dictionary<string, Tuple<float, float, float>>();
            foreach (KeyValuePair<string, List<Tuple<float, float, float>>> pair in newDic)
            {
                float mX = Array.Find(pair.Value.Select(f => f.Item1).GroupBy(f => float.IsNaN(f)).ToArray(), f => f.Key == false).Average();
                float mY = Array.Find(pair.Value.Select(f => f.Item2).GroupBy(f => float.IsNaN(f)).ToArray(), f => f.Key == false).Average();
                float mS = Array.Find(pair.Value.Select(f => f.Item3).GroupBy(f => float.IsNaN(f)).ToArray(), f => f.Key == false).Average();
                meanDic.Add(pair.Key, new Tuple<float, float, float>(mX, mY, mS));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("[um],{0}", string.Join(",,,", columnList)));

            sb.Append("Pattern");
            columnList.ForEach(f => sb.Append(",Width,Heigth,Score"));
            sb.AppendLine();

            sb.Append("Offset");
            meanDic.ToList().ForEach(f => sb.Append(string.Format(",{0:F03},{1:F03},", f.Value.Item1, f.Value.Item2)));
            sb.AppendLine();
            sb.AppendLine();

            foreach (KeyValuePair<int, SortedList<string, Tuple<float, float, float>>> pair in exportItemDic)
            {
                string[] arr = new string[columnItemCnt * 3 + 1];
                arr[0] = pair.Key.ToString();
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    KeyValuePair<string, Tuple<float, float, float>> v = pair.Value.ElementAt(i);
                    Tuple<float, float, float> mean = meanDic[v.Key];

                    arr[i * 3 + 1] = float.IsNaN(v.Value.Item1) ? "" : (v.Value.Item1 - mean.Item1).ToString("F02");
                    arr[i * 3 + 2] = float.IsNaN(v.Value.Item2) ? "" : (v.Value.Item2 - mean.Item2).ToString("F02");
                    arr[i * 3 + 3] = float.IsNaN(v.Value.Item3) ? "" : v.Value.Item3.ToString("F02");
                }
                sb.AppendLine(string.Join(",", arr));
            }

            try
            {
                File.WriteAllText(filePath, sb.ToString());
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                MessageForm.Show(this, ex.Message);
            }
        }

        private void sheetList_Click(object sender, EventArgs e)
        {
            SelectSheet(false);
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPEG File(*.jpg)|*.jpg";
            dlg.FileName = (selProduction == null) ?
                string.Format("Capture_{0}.jpg", DateTime.Now.ToString("yyyyMMdd_HHmmss")) :
                string.Format("M{0}_L{1}.jpg", this.selProduction.Name, this.selProduction.LotNo);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            this.Update();
            Rectangle rect = RectangleToScreen(this.DisplayRectangle);
            Task.Factory.StartNew(new Action<object>(Capture), new object[] { rect, dlg.FileName });
        }

        private void Capture(object args)
        {
            Thread.Sleep(500);

            object[] array = (object[])args;
            Bitmap bmp = ImageHelper.Capture((Rectangle)array[0]);
            ImageHelper.SaveImage(bmp, (string)array[1]);
            //Process.Start((string)array[2]);
        }

        private void patternRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (patternRadioButton.Checked == true)
                UpdateDefectInfo();
        }

        private void totalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (defectRadioButton.Checked == true)
                UpdateDefectInfo();
        }

        private void ReportPanel_Load(object sender, EventArgs e)
        {
            //if (!SystemTypeSettings.Instance().ShowDetailPatternLength)
            //    tableLayoutPanel1.Visible = false;
        }

        public void Export(ProductionBase[] productions)
        {
            ExcelExporterG excelExporterG = new ExcelExporterG();
            excelExporterG.Initialize();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Data.ProductionG[] productionGs = Array.ConvertAll(productions, f => f as Data.ProductionG);
            foreach (Data.ProductionG productionG in productionGs)
            {
                if (productionG == null)
                    continue;

                List<MergeSheetResult> list = new List<MergeSheetResult>();
                string resultPath = productionG.GetResultPath();
                bool done = LoadResult(ref resultPath, list);
                if (!done)
                    break;

                InspectionResult ir = new InspectionResult();
                list.ForEach(f => ir.AlgorithmResultLDic.Add(f.Index.ToString(), f));
                excelExporterG.Export(ir);
            }

            excelExporterG.Save(@"C:\temp\Excel.xlsx");
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            Point pt = buttonExport.PointToClient(MousePosition);
            buttonExport.ContextMenuStrip.Show((Control)sender, pt);
        }

        private void useRect_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult(false);
        }

        private void rect_ValueChanged(object sender, EventArgs e)
        {
            if (!this.useRect.Checked)
                return;
            ShowResult(false);
        }
    }
}
