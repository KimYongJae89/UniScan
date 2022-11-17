using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DynMvp.Base;
using UniEye.Base.UI;
using UniScanG.Data;
using UniScanG.Data.Model;
using System.Reflection;
using DynMvp.Authentication;
using UniScanG.Gravure.Data;
using System.Diagnostics;
using DynMvp.UI.Touch;
using UniScanG.Common.Settings;

namespace UniScanG.UI.Report
{
    public partial class ReportPage : UserControl, IMainTabPage, IMultiLanguageSupport, IUserHandlerListener
    {
        bool showLaserData = false;

        private enum LotOrder
        {
            // 날짜, 설비명, 모델, 로트, 검사수, 미인쇄, 미인쇄(비율), 미인쇄(등급), 핀홀, 핀홀, 핀홀, 번짐, 시트어택, 성형, 스티커, 마진, 변형, 총불량, 불량(비율), 불량(등급), 삭제개수, 성공개수, 전극사양, 성형사양, 설비속도, 메모
            Date, Machine, Model, Lot, TotalCnt, NP, NPR, NPG, PH, PHR, PHG, SP, SA, CO, ST, MG, TF, NG, NGR, NGG, KillCnt, KillGoodCnt, SpecChip, SpecCoat, SPD, Note, END
        }

        bool onUpdateData = false;
        IReportPanel reportPanel;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ReportPage(bool useLaser)
        {
            onUpdateData = true;

            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            reportPanel = SystemManager.Instance().UiChanger.CreateReportPanel();
            reportContainer.Controls.Add((Control)reportPanel);

            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";

            InitializeColumnStyle();
            
            UserHandler.Instance().AddListener(this);
            ColorTable.OnColorTableUpdated += UpdateLotColumnColor;

            onUpdateData = false;
        }

        private List<ModelDescription> FindModel(DateTime start, DateTime end, string search)
        {
            List<ModelDescription> modelDescriptionList = new List<ModelDescription>();

            foreach (DynMvp.Data.ProductionBase production in SystemManager.Instance().ProductionManager.List)
            {
                if (production.StartTime < startDate.Value.Date || production.StartTime > endDate.Value.Date.AddDays(1))
                    continue;

                if (production.Name.Contains(findModelName.Text) == false)
                    continue;

                if (modelDescriptionList.Exists(f => f.Name == production.Name))
                    continue;

                Production productionG = (Production)production;
                UniScanG.Data.Model.ModelDescription modelDescription = new UniScanG.Data.Model.ModelDescription();
                modelDescription.Name = productionG.Name;
                modelDescription.Thickness = productionG.Thickness;
                modelDescription.Paste = productionG.Paste;

                modelDescriptionList.Add(modelDescription);
            }

            return modelDescriptionList;
        }

        private void InitializeColumnStyle()
        {
            this.columnLotDate.DefaultCellStyle.Format = "yy/MM/dd HH:mm:ss";

            this.columnLotNgNPRatio.DefaultCellStyle.Format = "F01";
            this.columnLotNgPHRatio.DefaultCellStyle.Format = "F01";
            this.columnLotNgRatio.DefaultCellStyle.Format = "F01";


            // 컬럼 가리기
            bool simpleReport = SystemTypeSettings.Instance().ShowSimpleReportLotList;
            if (simpleReport)
            {
                this.columnLotNg.Visible = false;
                this.columnLotNgRatio.Visible = false;

                this.columnLotSpecChip.Visible = false;
                this.columnLotSpecCoat.Visible = false;
                this.columnLotSpdMpm.Visible = false;
            }

            if (simpleReport || !this.showLaserData)
            {
                // 레이저 삭제기..??
                this.columnLotKill.Visible = false;
                this.columnLotKillGood.Visible = false;
            }

            // Extended Function
            this.columnLotNgST.Visible = !simpleReport && Gravure.Vision.AlgorithmSetting.Instance().UseExtSticker;
            this.columnLotNgMG.Visible = !simpleReport && DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin);
            this.columnLotNgTF.Visible = !simpleReport && DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom);
            if (simpleReport || !DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.DecGrade))
            {
                this.columnLotNgNPRatio.Visible = false;
                this.columnLotNgNPGrade.Visible = false;

                this.columnLotNgPHRatio.Visible = false;
                this.columnLotNgPHGrade.Visible = false;

                this.columnLotNgGrade.Visible = false;
            }

            //this.columnLotDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.columnLotMachine.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotModel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotLotNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotTotalCnt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgNP.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgPH.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgSA.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgCO.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgST.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgCnt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotNgRto.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotSpecChip.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.columnLotSpecCoat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            UpdateLotColumnColor();

        }

        private void UpdateLotColumnColor()
        {
            this.columnLotNgNP.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Noprint);
            this.columnLotNgNP.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Noprint);
            this.columnLotNgPH.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.PinHole);
            this.columnLotNgPH.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.PinHole);
            this.columnLotNgSP.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Spread);
            this.columnLotNgSP.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Spread);
            this.columnLotNgSA.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Attack);
            this.columnLotNgSA.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Attack);
            this.columnLotNgCO.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Coating);
            this.columnLotNgCO.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Coating);
            this.columnLotNgST.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Sticker);
            this.columnLotNgST.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Sticker);
            this.columnLotNgMG.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Margin);
            this.columnLotNgMG.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Margin);
            this.columnLotNgTF.DefaultCellStyle.BackColor = ColorTable.GetColor(DefectType.Transform);
            this.columnLotNgTF.DefaultCellStyle.ForeColor = ColorTable.GetBgColor(DefectType.Transform);
        }

        private List<ProductionG> FindLot(ModelDescription modelDescription, DateTime start, DateTime end, string search)
        {
            DynMvp.Data.ProductionBase[] modelProductionLists;
            if (modelDescription == null)
                modelProductionLists = SystemManager.Instance().ProductionManager.List.ToArray();
            else
                modelProductionLists = SystemManager.Instance().ProductionManager.GetProductions(modelDescription);

            List<ProductionG> productionList = new List<ProductionG>();
            foreach (ProductionG production in modelProductionLists)
            {
                if (production.StartTime < startDate.Value.Date || production.StartTime > endDate.Value.Date.AddDays(1))
                    continue;

                if (production.LotNo.Contains(search) == false)
                    continue;

                productionList.Add(production);
            }
            return productionList;
        }

        private void UpdateModelList()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            ModelDescription selectedMd = null;
            if (this.modelList.SelectedRows.Count > 0)
                selectedMd = this.modelList.SelectedRows[0].Tag as ModelDescription;

            modelList.Rows.Clear();
            modelList.Rows.Add(0, "__ALL__", 0, "");

            int selectIndex = 0;
            List<ModelDescription> modelDescriptionList = FindModel(startDate.Value.Date, endDate.Value.Date.AddDays(1), findModelName.Text);
            for (int i = 0; i < modelDescriptionList.Count; i++)
            {
                UniScanG.Data.Model.ModelDescription md = modelDescriptionList[i];
                int rowIndex = modelList.Rows.Add(i + 1, md.Name, md.Thickness, md.Paste?.ToString());
                modelList.Rows[rowIndex].Tag = md;

                if (md.Equals(selectedMd))
                    selectIndex = i + 1;
            }

            modelList.Sort(modelList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            totalModel.Text = modelDescriptionList.Count.ToString();
            modelList.ClearSelection();

            onUpdateData = false;

            modelList.Rows[selectIndex].Selected = true;
        }

        public void UpdateLotList()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            ProductionG curSelectProduction = null;
            if (lotList.SelectedRows.Count > 0)
                curSelectProduction = lotList.SelectedRows[0].Tag as ProductionG;

            lotList.Rows.Clear();

            ModelDescription selectedMd = null;
            if (this.modelList.SelectedRows.Count > 0)
            {
                selectedMd = this.modelList.SelectedRows[0].Tag as ModelDescription;
            }

            List<ProductionG> productionList = FindLot(selectedMd, startDate.Value.Date, endDate.Value.Date.AddDays(1), findLotName.Text);
            List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
            DataGridViewRow selectedRow= null;
            foreach (ProductionG p in productionList)
            {
                if ((p.Total > 0 || p.UpdateRequire) && !p.UserRemoveFlag)
                {
                    if (!Directory.Exists(p.GetResultPath()))
                        continue;

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(lotList);
                    if (FillRowData(row, p as ProductionG))
                    {
                        dataGridViewRowList.Add(row);
                        if (row.Tag == curSelectProduction)
                            selectedRow = row;
                    }
                }
            }
            dataGridViewRowList.Sort((f,g)=>((ProductionG)g.Tag).StartTime.CompareTo(((ProductionG)f.Tag).StartTime));
            lotList.Rows.AddRange(dataGridViewRowList.ToArray());

            lotList.ClearSelection();
            int indexOf = dataGridViewRowList.IndexOf(selectedRow);
            if (indexOf >= 0)
                lotList.Rows[indexOf].Selected = true;

            //lotList.Sort(lotList.Columns[0], System.ComponentModel.ListSortDirection.Descending);

            totalLot.Text = lotList.Rows.Count.ToString();

            //lotList.AutoResizeColumns();
            //lotList.Rows.Add(DateTime.Now, "GRV-00", "440-32BMJE502-GL08SC", "0000000000", 10000, 1000, 1000, 1000, 1000, 1000, 5000, 50, 80, 80, 40);

            //for (int i = 0; i < lotList.Columns.Count; i++)
            //    System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", lotList.Columns[i].HeaderText, lotList.Columns[i].Width));

            //lotList.ClearSelection();
            onUpdateData = false;
        }

        private bool FillRowData(DataGridViewRow row, ProductionG p)
        {
            if (p == null)
                return false;

            row.Cells[(int)LotOrder.Date].Value = p.StartTime;
            row.Cells[(int)LotOrder.Machine].Value = SystemInformation.ComputerName;
            row.Cells[(int)LotOrder.Model].Value = p.Name;
            row.Cells[(int)LotOrder.Lot].Value = p.LotNo;
            if (p.UpdateRequire)
            {
                row.Cells[(int)LotOrder.TotalCnt].Value =
                row.Cells[(int)LotOrder.NP].Value =
                row.Cells[(int)LotOrder.NPR].Value =
                row.Cells[(int)LotOrder.NPG].Value =
                row.Cells[(int)LotOrder.PH].Value =
                row.Cells[(int)LotOrder.PHR].Value =
                row.Cells[(int)LotOrder.PHG].Value =
                row.Cells[(int)LotOrder.SP].Value =
                row.Cells[(int)LotOrder.SA].Value =
                row.Cells[(int)LotOrder.CO].Value =
                row.Cells[(int)LotOrder.ST].Value =
                row.Cells[(int)LotOrder.MG].Value =
                row.Cells[(int)LotOrder.TF].Value =
                row.Cells[(int)LotOrder.NG].Value =
                row.Cells[(int)LotOrder.NGR].Value =
                row.Cells[(int)LotOrder.NGG].Value =
                row.Cells[(int)LotOrder.KillCnt].Value =
                row.Cells[(int)LotOrder.KillGoodCnt].Value =

                row.Cells[(int)LotOrder.SpecChip].Value =
                row.Cells[(int)LotOrder.SpecCoat].Value =

                row.Cells[(int)LotOrder.SPD].Value = "?";
                row.Cells[(int)LotOrder.Note].Value = "";
            }
            else
            {
                row.Cells[(int)LotOrder.TotalCnt].Value = p.Done;
                row.Cells[(int)LotOrder.NP].Value = p.Patterns.NoPrint;
                row.Cells[(int)LotOrder.NPR].Value = p.NoPrintPatternRatio;
                row.Cells[(int)LotOrder.NPG].Value = p.NoPrintGrade;
                row.Cells[(int)LotOrder.NPG].Style.ForeColor = Gravure.Settings.AdditionalSettings.Instance().Grade.GetColor(p.NoPrintGrade);
                row.Cells[(int)LotOrder.PH].Value = p.Patterns.PinHole;
                row.Cells[(int)LotOrder.PHR].Value = p.PinholePatternRatio;
                row.Cells[(int)LotOrder.PHG].Value = p.PinHoleGrade;
                row.Cells[(int)LotOrder.PHG].Style.ForeColor = Gravure.Settings.AdditionalSettings.Instance().Grade.GetColor(p.PinHoleGrade);
                row.Cells[(int)LotOrder.SP].Value = p.Patterns.Spread;
                row.Cells[(int)LotOrder.SA].Value = p.Patterns.SheetAttack;
                row.Cells[(int)LotOrder.CO].Value = p.Patterns.Dielectric;
                row.Cells[(int)LotOrder.ST].Value = p.Patterns.Sticker;
                row.Cells[(int)LotOrder.MG].Value = p.Patterns.Margin;
                row.Cells[(int)LotOrder.TF].Value = p.Patterns.Transform;
                row.Cells[(int)LotOrder.NG].Value = p.Ng;
                row.Cells[(int)LotOrder.NGR].Value = p.NgRatio;
                row.Cells[(int)LotOrder.NGG].Value = p.NgGrade;
                row.Cells[(int)LotOrder.NGG].Style.ForeColor = Gravure.Settings.AdditionalSettings.Instance().Grade.GetColor(p.NgGrade);
                row.Cells[(int)LotOrder.KillCnt].Value = p.EraseNum;
                row.Cells[(int)LotOrder.KillGoodCnt].Value = p.EraseGood;

                row.Cells[(int)LotOrder.SpecChip].Value = p.SpecBlackUm;
                row.Cells[(int)LotOrder.SpecCoat].Value = p.SpecWhiteUm;

                row.Cells[(int)LotOrder.SPD].Value = p.LineSpeedMpm;
                row.Cells[(int)LotOrder.Note].Value = p.Note;
            }
            row.Tag = p;

            //string[] deb = new string[(int)LotOrder.END];
            //for (int i = 0; i < row.Cells.Count; i++)
            //    deb[i] = row.Cells[i].Value == null ? "" : row.Cells[i].Value.ToString();
            //Debug.WriteLine(string.Format("ReportPage::CreateRowData - {0}", string.Join(";", deb)));

            return true;
        }

        private void Search()
        {
            if (lotList.SelectedRows.Count != 1)
                return;

            ProductionG production = (ProductionG)lotList.SelectedRows[0].Tag;
            reportPanel.Search(production);
            //UpdateLotList();

            ShowReportContainer(true);
        }

        private void ShowReportContainer(bool v)
        {
            ultraSplitter1.Collapsed = !v;
            if (v)
                this.reportContainer.Width = this.Size.Width - 670;
        }

        private void findLotName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                findLotName.SelectAll();
                UpdateLotList();
            }
        }

        private void findLotName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(findLotName.Text))
                UpdateLotList();
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }
        public void EnableControls(UserType userType) { }

        public void UpdateLanguage()
        {
            onUpdateData = true;

            StringManager.UpdateString(this);

            onUpdateData = false;
        }

        private void SelectModelList()
        {
            if (onUpdateData == true)
                return;

            if (modelList.SelectedRows.Count == 0)
                return;

            Data.Model.ModelDescription md = (Data.Model.ModelDescription)modelList.SelectedRows[0].Tag;
            UpdateLotList();
            //findModelName.Text = md.Name;
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            if (startDate.Value > endDate.Value)
            {
                endDate.Value = startDate.Value;
                return;
            }
            UpdateModelList();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            if (startDate.Value > endDate.Value)
            {
                startDate.Value = endDate.Value;
                return;
            }
            UpdateModelList();
        }

        private void findModelName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(findModelName.Text))
                UpdateModelList();
        }

        private void findModelName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                findModelName.SelectAll();
                UpdateModelList();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void lotList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)LotOrder.Note)
            {
                lotList.BeginEdit(true);
                return;
            }

            Search();
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLotList();
        }

        private void modelList_Click(object sender, EventArgs e)
        {
            UpdateLotList();
        }

        private void ReportPage_VisibleChanged(object sender, EventArgs e)
        {
            InitializeColumnStyle();
            if (this.Visible == true)
                UpdateModelList();
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            this.onUpdateData = true;
            Production lastProduction = SystemManager.Instance().ProductionManager.GetLastProduction(f => f.Total > 0) as Production;
            //startDate.Value = lastProduction == null ? DateTime.Now : lastProduction.StartTime;
            //endDate.Value = DateTime.Now;
            endDate.Value = lastProduction == null ? DateTime.Now : lastProduction.StartTime;
            startDate.Value = endDate.Value.AddDays(-7);
            this.onUpdateData = false;

            UpdateModelList();
        }

        private void lotList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == (int)LotOrder.Note)
            {
                ProductionG productionG = lotList.Rows[e.RowIndex].Tag as ProductionG;
                productionG.Note = lotList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                productionG.Save();
                SystemManager.Instance().ProductionManager.Save();
            }
        }

        private void lotList_ColumnHeadersHeightChanged(object sender, EventArgs e)
        {
        }

        private void exportxlsxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lotList.SelectedRows.Count != 1)
                return;

            List<ProductionG> selected = new List<ProductionG>();
            foreach (DataGridViewRow row in lotList.SelectedRows)
                selected.Add(row.Tag as ProductionG);

            selected.RemoveAll(f => f == null);
            //this.reportPanel.Export(selected.ToArray());
        }

        public void UserChanged()
        {
            this.lotList.ContextMenuStrip = UserHandler.Instance().CurrentUser.IsMasterAccount ? this.lotDataGridContextMenuStrip : null;
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lotList.SelectedRows.Count != 1)
                return;

            ProductionG selProductionG = lotList.SelectedRows[0].Tag as ProductionG;
            if (selProductionG == null)
                return;

            DirectoryInfo directoryInfo = new DirectoryInfo(selProductionG.ResultPath);
            if (!directoryInfo.Exists)
                return;

            FileInfo[] fileInfo = directoryInfo.GetFiles("Copied", SearchOption.TopDirectoryOnly);
            if (fileInfo.Length > 0)
            {
                DirectoryInfo copiedDirectoryInfo = new DirectoryInfo(File.ReadAllText(fileInfo[0].FullName));
                if (copiedDirectoryInfo.Exists)
                    directoryInfo = copiedDirectoryInfo;
            }
            Process.Start(directoryInfo.FullName);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ProductionG> targetProductionList = new List<ProductionG>();
            foreach (DataGridViewRow row in lotList.SelectedRows)
                targetProductionList.Add(row.Tag as ProductionG);
            targetProductionList.RemoveAll(f => f == null);

            // 검사중인 로트는 삭제 못 함.
            ProductionG curProduction = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (targetProductionList.Exists(f => f == curProduction) && !UniEye.Base.Data.SystemState.Instance().IsIdle)
            {
                MessageForm.Show(this, StringManager.GetString("Stop Inspection to remove current production."));
                return;
            }

            int count = targetProductionList.Count;
            string message = null;
            if (count == 1)
            {
                ProductionG productionG = targetProductionList[0];
                string[] messages = new string[]
                {
                        string.Format(StringManager.GetString("Start Time [{0}]"), productionG.StartTime.ToString("yyyy/MM/dd HH:mm")),
                        string.Format(StringManager.GetString("Model [{0}]"), productionG.Name),
                        string.Format(StringManager.GetString("Lot [{0}]"), productionG.LotNo),
                        StringManager.GetString("Will be Deleted!")
                };
                message = string.Join(Environment.NewLine, messages);
            }
            else if (count > 1)
            {
                string[] messages = new string[]
                {
                        string.Format(StringManager.GetString("Selected [{0}] Items"), count),
                        StringManager.GetString("Will be Deleted!")
                };
                message = string.Join(Environment.NewLine, messages);
            }

            if (string.IsNullOrEmpty(message))
                return;

            if (MessageForm.Show(this, message, MessageFormType.YesNo) == DialogResult.Yes)
            {
                targetProductionList.ForEach(f =>
                {
                    ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.DataRemove, ErrorLevel.Info, "Report [{0}] / [{1}] / [{2}] Deleted Manually.",
                        new object[] { f.StartTime, f.Name, f.LotNo }, ""));
                    f.UserRemoveFlag = true;
                });
                SystemManager.Instance().ProductionManager.Save();
                UpdateLotList();
            }
        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ProductionG> targetProductionList = new List<ProductionG>();
            foreach (DataGridViewRow row in lotList.SelectedRows)
                targetProductionList.Add(row.Tag as ProductionG);
            targetProductionList.RemoveAll(f => f == null);

            targetProductionList.ForEach(f =>
            {
                string path = DynMvp.Data.DataCopier.GetCopiedPath(f.GetResultPath());
                FileInfo fInfo = new FileInfo(Path.Combine(path, Data.Inspect.DataExporterG.CacheFileName));
                fInfo.Delete();
            });
        }
    }
}
