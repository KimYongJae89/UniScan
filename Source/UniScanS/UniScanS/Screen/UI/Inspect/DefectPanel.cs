using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Util;
using UniScanS.Data;
using UniScanS.Data.UI;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Settings;
using UniScanS.UI;
using static UniScanS.UI.Inspect.InspectPage;

namespace UniScanS.Screen.UI.Inspect
{
    public partial class DefectPanel : UserControl, IInspectDefectPanel, IMultiLanguageSupport, IModelListener, IProductionListener
    {
        AlarmForm alarmForm = new AlarmForm();
        ErrorForm errorForm = new ErrorForm();
        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
        List<CheckBox> checkBoxCamList = new List<CheckBox>();

        DefectType selectedDefectType = DefectType.Total;

        UpdateResultDelegate UpdateResultDelegate;
        List<DataGridViewRow> currentDataGridViewRowList = new List<DataGridViewRow>();

        public void AddDelegate(UpdateResultDelegate UpdateResultDelegate)
        {
            this.UpdateResultDelegate = UpdateResultDelegate;
        }

        public DefectPanel()
        {
            InitializeComponent();

            this.TabIndex = 0;
            this.Dock = DockStyle.Right;
            
            InitCheckBoxCam();
            
            StringManager.AddListener(this);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            ((ProductionManagerS)SystemManager.Instance().ProductionManager).AddListener(this);
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
        
        public void InspectDone(InspectionResult inspectionResult)
        {
            AlgorithmResult algorithmResult = inspectionResult.AlgorithmResultLDic[SheetInspector.TypeName];
            if (algorithmResult == null)
                return;
            
            if (InvokeRequired)
            {
                BeginInvoke(new InspectDoneDelegate(InspectDone), inspectionResult);
                return;
            }
            
            SheetResult sheetResult = (SheetResult)algorithmResult;
            currentDataGridViewRowList.Clear();
            foreach (SheetSubResult sheetSubResult in sheetResult.SheetSubResultList)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewRow row = new DataGridViewRow();
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = inspectionResult.InspectionNo};
                row.Cells.Add(indexCell);

                DataGridViewTextBoxCell camCell = new DataGridViewTextBoxCell() { Value = sheetSubResult.CamIndex};
                row.Cells.Add(camCell);

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
                row.Height = defectList.Height / 6;
                row.Tag = sheetSubResult;

                currentDataGridViewRowList.Add(row);
                dataGridViewRowList.Add(row);
            }

            if (DataSetting.Instance().MaxShowResultNum < dataGridViewRowList.Count)
            {
                lock (dataGridViewRowList)
                    dataGridViewRowList.RemoveRange(0, dataGridViewRowList.Count - DataSetting.Instance().MaxShowResultNum);
            }
            
            List<DataGridViewRow> filteredDataGridViewRowList = GetFilteredList(dataGridViewRowList);
            ShowResult(filteredDataGridViewRowList);

            if (UpdateResultDelegate != null)
                UpdateResultDelegate(sheetResult, GetFilteredList(currentDataGridViewRowList));
            
            if (sheetResult is MergeSheetResult)
            {
                ErrorCheck((MergeSheetResult)sheetResult);
                AlarmCheck((MergeSheetResult)sheetResult);
            }
                
        }

        private void ShowResult(List<DataGridViewRow> filteredDataGridViewRowList)
        {
            lock (defectList)
            {
                int rowIndex = defectList.FirstDisplayedScrollingRowIndex;

                defectList.Rows.Clear();
                filteredDataGridViewRowList.Reverse();

                defectList.Rows.AddRange(filteredDataGridViewRowList.ToArray());
                defectList.ClearSelection();

                rowIndex = (int)System.Math.Min(rowIndex, defectList.Rows.Count - 1);

                if (rowIndex != -1)
                    defectList.FirstDisplayedScrollingRowIndex = rowIndex;
            }

            //lock (totalDefect)
                //totalDefect.Text = defectList.Rows.Count.ToString();
        }

        private List<DataGridViewRow> GetFilteredList(List<DataGridViewRow> tempDataGridViewRowList)
        {
            List<DataGridViewRow> filteredDataGridViewRowList = new List<DataGridViewRow>();

            lock (tempDataGridViewRowList)
            {
                foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
                {
                    SheetSubResult sheetSubResult = (SheetSubResult)dataGridViewRow.Tag;

                    CheckBox checkBox = checkBoxCamList.Find(cam => ((InspectorObj)cam.Tag).Info.CamIndex == sheetSubResult.CamIndex - 1);
                    if (checkBox == null || checkBox.Checked == false)
                        continue;

                    if (sheetSubResult.DefectType == selectedDefectType || selectedDefectType == DefectType.Total)
                    {
                        if (useSize.Checked == true)
                        {
                            if (sheetSubResult.RealLength >= (float)sizeMin.Value && sheetSubResult.RealLength <= (float)sizeMax.Value)
                                filteredDataGridViewRowList.Add(dataGridViewRow);
                        }
                        else
                        {
                            filteredDataGridViewRowList.Add(dataGridViewRow);
                        }
                    }
                }
            }

            return filteredDataGridViewRowList;
        }

        private void ChangeFilter()
        {
            if (dataGridViewRowList.Count == 0)
                return;

            lock (defectList)
                defectList.Rows.Clear();

            SimpleProgressForm lodingForm = new SimpleProgressForm("Filtering");
            
            List<DataGridViewRow> filteredDataGridViewRowList = new List<DataGridViewRow>();
            lodingForm.Show(() =>
            {
                lock (dataGridViewRowList)
                    filteredDataGridViewRowList = GetFilteredList(dataGridViewRowList);
            });

            ShowResult(filteredDataGridViewRowList);
            if (UpdateResultDelegate != null)
                UpdateResultDelegate(null, GetFilteredList(currentDataGridViewRowList));
        }

        void IMultiLanguageSupport.UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void ClearDefectView()
        {
            currentDataGridViewRowList.Clear();
            dataGridViewRowList.Clear();
        }

        public void ModelChanged()
        {
            ClearDefectView();
        }

        public void ModelTeachDone() { }

        private void CamButton_CheckedChanged(object sender, System.EventArgs e)
        {
            ChangeFilter();
        }

        private void useSize_CheckedChanged(object sender, System.EventArgs e)
        {
            sizeMin.Enabled = useSize.Checked;
            sizeMax.Enabled = useSize.Checked;

            ChangeFilter();
        }

        private void total_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Total;

            ChangeFilter();
        }

        private void pole_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Pole;

            ChangeFilter();
        }

        private void sheetAttack_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.SheetAttack;

            ChangeFilter();
        }

        private void dielectric_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Dielectric;

            ChangeFilter();
        }

        private void pinHole_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.PinHole;

            ChangeFilter();
        }

        private void shape_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Shape;

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
            ((DataGridView)sender).ClearSelection();
        }

        public void ProductionChanged()
        {
            ClearDefectView();
        }

        private void ErrorCheck(MergeSheetResult mergeSheetResult)
        {
            SheetErrorType sheetErrorType = MonitorSetting.Instance().ErrorChecker.CheckError(mergeSheetResult);
            if (sheetErrorType != SheetErrorType.None)
            {
                errorForm.UpdateData(MonitorSetting.Instance().ErrorChecker);
                errorForm.Popup();
            }
        }

        private void AlarmCheck(MergeSheetResult mergeSheetResult)
        {
            foreach (AlarmChecker alarmChecker in MonitorSetting.Instance().AlarmCheckerList)
            {
                if (alarmChecker.CalcResult(mergeSheetResult) == false)
                {
                    if (alarmForm.Visible == true)
                        break;
                        
                    alarmForm.UpdateData(alarmChecker);
                    alarmForm.Popup();
                    break;
                }
            }
        }
    }
}
