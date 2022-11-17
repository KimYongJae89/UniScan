using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Settings;
using UniScanG.UI;
using static UniScanG.UI.Inspect.InspectPage;

namespace UniScanG.Gravure.UI.Inspect
{
    public partial class DefectPanel : UserControl, IInspectDefectPanel, IMultiLanguageSupport
    {
        bool blockUpdate = true;

        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
        List<CheckBox> checkBoxCamTypeList = new List<CheckBox>();

        DefectType[] selectedDefectType = new DefectType[0];// DefectType.Total;

        UpdateResultDelegate1 UpdateResultDelegate1;
        UpdateResultDelegate2 UpdateResultDelegate2;
        List<DataGridViewRow> dataSource = new List<DataGridViewRow>();
        object lockObject = new object();
        List<DataGridViewRow> filteredDataSource = null;
    

        public bool BlockUpdate
        {
            get { return blockUpdate; }
            set { blockUpdate = value; }
        }

        public void AddDelegate(UpdateResultDelegate1 UpdateResultDelegate)
        {
            this.UpdateResultDelegate1 += UpdateResultDelegate;
        }

        public void AddDelegate(UpdateResultDelegate2 UpdateResultDelegate)
        {
            this.UpdateResultDelegate2 += UpdateResultDelegate;
        }

        public DefectPanel()
        {
            InitializeComponent();

            this.TabIndex = 0;
            this.Dock = DockStyle.Right;

            this.defectList.RowTemplate.Height = (this.defectList.Height - this.defectList.ColumnHeadersHeight) / 10;

            StringManager.AddListener(this);

            InitCheckBoxCam();
            InitDefectTypeSelector();

            SystemManager.Instance().InspectRunnerG.AddInspectDoneDelegate(InspectDone);
            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;
        }

        private void InitDefectTypeSelector()
        {
            DefectTypeFilterPanel defectTypeFilterPanel = new DefectTypeFilterPanel(1);
            defectTypeFilterPanel.Dock = DockStyle.Fill;
            this.panelFilterType.Controls.Add(defectTypeFilterPanel);

            defectTypeFilterPanel.OnDefectTpyeSelectChanged += DefectTypeFilterPanel_OnDefectTpyeSelectChanged;
            this.selectedDefectType = defectTypeFilterPanel.GetSelected();
        }

        private void DefectTypeFilterPanel_OnDefectTpyeSelectChanged(DefectType[] defectTypes)
        {
            this.selectedDefectType = defectTypes;
            ChangeFilter();
        }

        private void ProductionManager_OnLotChanged()
        {
            this.Reset();
        }

        void InitCheckBoxCam()
        {
            ExchangeOperator exchangeOperator = SystemManager.Instance().ExchangeOperator;
            if (exchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex == 0);
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

                    checkBoxNewCam.Tag = inspectorList[i];
                    checkBoxCamTypeList.Insert(0, checkBoxNewCam);
                    panelFilterCam.Controls.Add(checkBoxNewCam);
                }
            }
        }

        public void Reset()
        {
            if (InvokeRequired)
            {
                Invoke(new ResetDelegate(Reset));
                return;
            }

            ClearGridData();
            UiHelper.SetControlText(this.totalDefect, "0");

            UpdateResultDelegate1?.Invoke(null);
            UpdateResultDelegate2?.Invoke(null, null);
        }

        public void InspectDone(InspectionResult inspectionResult)
        {
            //UpdateGrid(0);
            if (inspectionResult.Judgment == Judgment.Skip)
                return;

            int curSheetNo = int.Parse(inspectionResult.InspectionNo);

            List<FoundedObjInPattern> sheetSubResultList = (inspectionResult as UniScanG.Data.Inspect.InspectionResult)?.GetSubResultList();
            UiHelper.SetControlText(this.totalDefect, sheetSubResultList.Count(f => f.IsDefect).ToString());

            if (this.blockUpdate)
                return;

            bool ok = System.Threading.Monitor.TryEnter(this.lockObject);
            if (ok)
            {
                Stopwatch sw = new Stopwatch();
                try
                {
                    sw.Start();
                    BuildGridData(sheetSubResultList, inspectionResult.InspectionNo);
                    UpdateGrid();

                    UpdateResultDelegate1?.Invoke(inspectionResult);
                    if (UpdateResultDelegate2 != null)
                    {
                        Bitmap prevImage = null;
                        if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName))
                            prevImage = ((AlgorithmResultG)inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName]).PrevImage;
                        else if (inspectionResult.AlgorithmResultLDic.ContainsKey(CalculatorBase.TypeName))
                            prevImage = ((AlgorithmResultG)inspectionResult.AlgorithmResultLDic[CalculatorBase.TypeName]).PrevImage;

                        if (prevImage == null)
                            prevImage = SystemManager.Instance().CurrentModel.GetPreviewImage("");

                        Bitmap prevImageClone = null;
                        lock (prevImage)
                            prevImageClone = (Bitmap)(prevImage.Clone());

                        UpdateResultDelegate2(prevImageClone, filteredDataSource);
                        //prevImageClone?.Dispose();
                    }

                }
                finally
                {
                    sw.Stop();
                    DynMvp.ConsoleEx.WriteLine(string.Format("DefectPanel::InspectDone - ID: {0}, Time: {1}[ms]", curSheetNo, sw.ElapsedMilliseconds));
                    System.Threading.Monitor.Exit(this.lockObject);
                }
            }
        }

        private void ClearGridData()
        {
            this.UpdateGrid(0);
            this.dataSource.Clear();
            this.filteredDataSource?.Clear();
        }

        private delegate void UpdateGridDelegate(int rowCount = -1);
        private void UpdateGrid(int rowCount = -1)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new UpdateGridDelegate(UpdateGrid), rowCount);
            //    return;
            //}

            if (filteredDataSource != null)
            {
                if (rowCount < 0)
                    rowCount = this.filteredDataSource.Count;
                ShowResult(new List<DataGridViewRow>(this.filteredDataSource.GetRange(0, rowCount)));
            }
            //    defectList.RowCount = rowCount;
            //    totalDefect.Text = rowCount.ToString();
        }

        private void BuildGridData(List<FoundedObjInPattern> sheetSubResultList, string inspectionNo)
        {
            if (sheetSubResultList == null)
                return;

            int maxViewCount = (int)this.maxViewCount.Value;
            lock (this.dataSource)
            {
                //if (maxViewCount < 0)
                //    this.dataSource.Clear();

                int insertRow = 0;
                if (this.dataSource.Count > 0)
                {
                    int patNo = int.Parse(inspectionNo);
                    insertRow = this.dataSource.FindIndex(f => int.Parse(f.Cells[0].Value.ToString()) < patNo);
                }

                for (int i = sheetSubResultList.Count - 1; i >= 0; i--)
                {
                    FoundedObjInPattern sheetSubResult = sheetSubResultList[i] as FoundedObjInPattern;
                    if (sheetSubResult == null || !sheetSubResult.ShowReport || !sheetSubResult.IsDefect)
                        continue;

                    DataGridViewRow row = new DataGridViewRow();
                    row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    row.CreateCells(this.defectList);

                    // Sheet No
                    row.Cells[0].Value = inspectionNo;

                    // Cam No
                    row.Cells[1].Value = sheetSubResult.CamIndex + 1;

                    // Defect Type
                    row.Cells[2].Value = sheetSubResult.GetDefectType().GetLocalString();
                    row.Cells[2].Style.ForeColor = sheetSubResult.GetBgColor();
                    row.Cells[2].Style.BackColor = sheetSubResult.GetColor();
                    row.Cells[2].ToolTipText = sheetSubResult.GetDefectTypeDiscription();

                    // Position
                    row.Cells[3].Value = sheetSubResult.GetPositionString();

                    // Size
                    row.Cells[4].Value = sheetSubResult.GetSizeString();

                    // Info
                    row.Cells[5].Value = sheetSubResult.GetInfoString();

                    // Image
                    Bitmap image = sheetSubResult.Image;
                    if (image != null)
                    {
                        lock (sheetSubResult.Image)
                            ((DataGridViewImageCell)row.Cells[6]).Value = sheetSubResult.Image;
                        ((DataGridViewImageCell)row.Cells[6]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    }

                    row.Tag = sheetSubResult;

                    this.dataSource.Insert(insertRow, row);
                }

                //defectList.RowTemplate.Height = defectList.Height / 10;
                this.filteredDataSource = GetFilteredList(this.dataSource);

                if (this.dataSource.Count > 1000)
                    this.dataSource = this.dataSource.GetRange(0, 1000);
            }
        }


        private void defectList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //return;
            //DataGridView dataGridView = sender as DataGridView;
            //if (dataGridView != null)
            //{
            //    dataGridView.Rows[e.RowIndex].Height = dataGridView.Height / 8;
            //}

            //lock (this.lockObject)
            e.Value = filteredDataSource[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private delegate void ShowResultDelegate(List<DataGridViewRow> filteredDataGridViewRowList);
        private void ShowResult(List<DataGridViewRow> filteredDataGridViewRowList)
        {
            if (InvokeRequired)
            {
                //Debug.WriteLine("BeginInvoke - DefectPaenl::ShowResult");
                BeginInvoke(new ShowResultDelegate(ShowResult), filteredDataGridViewRowList);
                return;
            }

            lock (defectList)
            {
                int rowIndex = defectList.FirstDisplayedScrollingRowIndex;

                defectList.Rows.Clear();
                //filteredDataGridViewRowList.Reverse();
                filteredDataGridViewRowList.ForEach(f => f.Height = 80);
                defectList.Rows.AddRange(filteredDataGridViewRowList.ToArray());
                defectList.ClearSelection();

                if (rowIndex >= 0 && defectList.Rows.Count > rowIndex)
                    defectList.FirstDisplayedScrollingRowIndex = rowIndex;
            }
        }

        private List<DataGridViewRow> GetFilteredList(List<DataGridViewRow> dataSource)
        {
            int maxViewCount = (int)this.maxViewCount.Value;
            List<DataGridViewRow> filteredDataGridViewRowList = new List<DataGridViewRow>();

            List<DataGridViewRow> tempDataGridViewRowList;
            lock (dataSource)
            {
                if (maxViewCount < 0)
                {
                    int lastInsex = dataSource.Max(f => ((FoundedObjInPattern)f.Tag).Index);
                    tempDataGridViewRowList = dataSource.FindAll(f => ((FoundedObjInPattern)f.Tag).Index == lastInsex);
                }
                else
                {
                    tempDataGridViewRowList = new List<DataGridViewRow>(dataSource);
                }
            }

            foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
            {
                if (maxViewCount >= 0 && filteredDataGridViewRowList.Count >= maxViewCount)
                    break;

                //Data.DefectInPattern sheetSubResult = (Data.DefectInPattern)dataGridViewRow.Tag;
                FoundedObjInPattern sheetSubResult = (FoundedObjInPattern)dataGridViewRow.Tag;

                if (checkBoxCamTypeList.Count > 0)
                {
                    CheckBox camCheckBox = checkBoxCamTypeList.Find(f => ((InspectorObj)f.Tag).Info.CamIndex == sheetSubResult.CamIndex);
                    if (camCheckBox == null || camCheckBox.Checked == false)
                        continue;
                }

                DefectType defectType = sheetSubResult.GetDefectType();
                if (defectType == DefectType.Unknown)
                    continue;

                if (this.selectedDefectType.Contains(defectType) && sheetSubResult.IsDefect)
                {
                    bool add = true;
                    if (useSize.Checked == true)
                        add = (sheetSubResult.RealLength >= (float)sizeMin.Value && sheetSubResult.RealLength <= (float)sizeMax.Value);

                    if (add)
                        filteredDataGridViewRowList.Add(dataGridViewRow);
                }
            }

            return filteredDataGridViewRowList;
        }

        private void ChangeFilter()
        {
            if (this.dataSource.Count == 0)
                return;

            //lock (defectList)
            //    defectList.Rows.Clear();

            new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Filtering")).Show(() =>
            {
                lock (this.dataSource)
                    this.filteredDataSource = GetFilteredList(this.dataSource);
                ShowResult(this.filteredDataSource);
            });
            UpdateResultDelegate2?.Invoke(null, GetFilteredList(this.dataSource));
        }

        void IMultiLanguageSupport.UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void useSize_CheckedChanged(object sender, System.EventArgs e)
        {
            sizeMin.Enabled = useSize.Checked;
            sizeMax.Enabled = useSize.Checked;

            ChangeFilter();
        }

        private void sizeMin_ValueChanged(object sender, System.EventArgs e)
        {
            ChangeFilter();
        }

        private void sizeMax_ValueChanged(object sender, System.EventArgs e)
        {
            ChangeFilter();
        }

        private void defectList_SelectionChanged(object sender, System.EventArgs e)
        {
            //((DataGridView)sender).ClearSelection();
        }

        private void DefectPanel_Load(object sender, EventArgs e)
        {
            this.defectList.RowTemplate.Height = (this.defectList.Height - this.defectList.ColumnHeadersHeight) / 10;
        }

        private void DefectPanel_VisibleChanged(object sender, EventArgs e)
        {

        }
    }
}
