using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniScanG.Data;
using DynMvp.UI;
using UniScanG.Common;
using DynMvp.Base;
using UniScanG.Gravure.Settings;
//using UniScanG.MachineIF;
using UniScanG.UI.Inspect;
using UniScanG.Module.Controller.MachineIF;
using UniEye.Base.Inspect;
using UniScanG.Common.Data;
using UniScanG.Gravure.Device;
using DynMvp.Data.UI;

namespace UniScanG.Module.Controller.UI
{
    public partial class RepeatedDefectAlarmForm : Form, IRepeatedDefectAlarmForm, IModelListener, IMultiLanguageSupport
    {
        MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
        CanvasPanel canvasPanel = new CanvasPanel();
        RepeatedDefectItemCollection repeatedDefectItemList = new RepeatedDefectItemCollection();
        List<RepeatedDefectItem> alarmedData = new List<RepeatedDefectItem>();

        Control parent;

        public bool IsAlarmState
        {
            get
            {
                lock (alarmedData)
                    return alarmedData.Exists(f => f.IsAlarmState);
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public RepeatedDefectAlarmForm(Control parent)
        {
            InitializeComponent();

            this.parent = parent;

            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.MouseDoubleClick += new MouseEventHandler((s, e) =>
            {
                canvasPanel.ZoomFit();
                this.dataGridView1.ClearSelection();
            });

            canvasPanel.SetPanMode();
            canvasPanel.ReadOnly = true;
            panelMain.Controls.Add(canvasPanel);

            //((DataGridViewImageCell)row.Cells[6]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.dataGridView1.RowTemplate.Height = this.dataGridView1.Height / 12;
            //this.dataGridView1.RowTemplate.DefaultCellStyle.

            ((UnitBaseInspectRunner)SystemManager.Instance().InspectRunnerG).AddInspectDoneDelegate(InspectDone);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            this.updateTimer.Start();

            StringManager.AddListener(this);

            //this.Show();
            //this.Hide();
        }

        public new void Show()
        {
            base.Show(this.parent);
        }

        public void InspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName) == false)
                return;

            AlgorithmResultG sheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as AlgorithmResultG;
            if (sheetResult == null)
                return;

            repeatedDefectItemList.AddResult(sheetResult, true);
            List<RepeatedDefectItem> alarmItemList = repeatedDefectItemList.GetAlarmData();
            List<RepeatedDefectItem> newItemList = alarmItemList.FindAll(f => f.IsNotified == false);
            newItemList.ForEach(f => f.IsNotified = true);
            AddAlarm(newItemList);
        }

        public void Clear()
        {
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P = false;

            this.canvasPanel.WorkingFigures.Clear();
            this.dataGridView1.RowCount = 0;

            lock (this.alarmedData)
                this.alarmedData.Clear();

            lock (this.repeatedDefectItemList)
                repeatedDefectItemList.Clear();

            Hidee();
        }

        private delegate void HideDelegate();
        private void Hidee()
        {
            if (InvokeRequired)
            {
                Invoke(new HideDelegate(Hide));
                return;
            }

            this.Hide();
        }

        private delegate void ShowDataDelegate(List<RepeatedDefectItem> newData);
        private void AddAlarm(List<RepeatedDefectItem> newData)
        {
            if (newData.Count > 0)
            {
                lock (this.alarmedData)
                {
                    this.alarmedData.AddRange(newData);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Silent();
            this.Hide();
        }

        public void Silent()
        {
            if (alarmedData != null)
            {
                lock (alarmedData)
                {
                    foreach (RepeatedDefectItem repeatedDefectItem in alarmedData)
                    {
                        repeatedDefectItem.IsAlarmCleared = true;
                    }
                }
            }
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N = false;
            machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P = false;
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (alarmedData == null)
                return;

            lock (alarmedData)
            {
                if (e.RowIndex >= alarmedData.Count)
                    return;

                List<FoundedObjInPattern> sheetSubResult = alarmedData[e.RowIndex].RepeatedDefectElementList.FindAll(f => f != null);
                if (sheetSubResult.Count == 0)
                {
                    e.Value = null;
                    return;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = e.RowIndex.ToString();
                        break;
                    case 1:
                        e.Value = sheetSubResult[0].Image;
                        break;
                    case 2:
                        e.Value = sheetSubResult[0].GetDefectType().ToString();
                        break;
                    case 3:
                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    sb.AppendLine(string.Format("W: {0:0.00}", sheetSubResult.Average(f => f.RealRegion.Width)));
                        //    sb.AppendLine(string.Format("H: {0:0.00}", sheetSubResult.Average(f => f.RealRegion.Height)));
                        //    e.Value = sb.ToString();
                        //}
                        e.Value = string.Format("W: {0:0.0}\r\nH: {1:0.0}", sheetSubResult.Average(f => f.RealRegion.Width), sheetSubResult.Average(f => f.RealRegion.Height));
                        break;
                    case 4:
                        e.Value = alarmedData[e.RowIndex].RepeatRatio.ToString("0.00");
                        break;
                    case 5:
                        break;
                }
            }
        }

        private void UpdateImage()
        {
            UniScanG.Common.Data.Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return;

            Bitmap prevImg = (SystemManager.Instance().ModelManager as ModelManager)?.GetPreviewImage(curModel.ModelDescription, "");
            canvasPanel.UpdateImage(prevImg);
            canvasPanel.ZoomFit();
        }

        public void ModelChanged()
        {
            UpdateImage();
        }

        public void ModelTeachDone(int camId)
        {
            if (camId < 0)
                UpdateImage();
        }
        public void ModelRefreshed() { }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            canvasPanel.WorkingFigures.Clear();
            List<DataGridViewRow> rowList = dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList();

            if (rowList.Count == 0)
            {
                rowList = dataGridView1.Rows.Cast<DataGridViewRow>().ToList();
            }

            foreach (DataGridViewRow row in rowList)
            {
                RepeatedDefectItem item = this.alarmedData[row.Index];
                FoundedObjInPattern president = item.RepeatedDefectElementList.Find(f => f != null);
                Figure figure = president?.GetFigure(0.1f);
                canvasPanel.WorkingFigures.AddFigure(figure);
            }
            canvasPanel.ZoomFit();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (!UniEye.Base.Data.SystemState.Instance().IsInspection)
                return;

            lock (this.alarmedData)
            {
                this.alarmedData.RemoveAll(f => f.RepeatedDefectElementList.FindAll(g => g != null).Count == 0);
                try
                {
                    dataGridView1.RowCount = this.alarmedData.Count;
                    List<RepeatedDefectItem> notid = alarmedData.FindAll(f => f.IsAlarmCleared == false);
                    if (notid.Count > 0)
                    {
                        if (this.Visible == false)
                            this.Show();

                        IEnumerable<DefectType> defectTypes = notid.Select(f => f.RepeatedDefectElementList.First(g => g != null).GetDefectType());

                        CalculateRepeatedAlarm(defectTypes, DefectType.Noprint, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N);
                        CalculateRepeatedAlarm(defectTypes, DefectType.PinHole, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P);
                        CalculateRepeatedAlarm(defectTypes, DefectType.Spread, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B);
                        CalculateRepeatedAlarm(defectTypes, DefectType.Total, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF);

                        if (this.Visible)
                            this.dataGridView1.Invalidate();
                    }
                }
                catch (Exception) { }
                finally { }
            }
        }

        private void CalculateRepeatedAlarm(IEnumerable<DefectType> defectTypes, DefectType defectType, ref bool machineIfDataItem)
        {
            bool state = defectTypes.Contains(defectType);
            if (defectType == DefectType.Total)
            {
                DefectType[] exclusives = new DefectType[] { DefectType.PinHole, DefectType.Spread, DefectType.Noprint };
                int exclusive = defectTypes.Count(f => exclusives.Contains(f));
                state = (defectTypes.Count() - exclusive) > 0;
            }

            if (state && !machineIfDataItem)
            {
                string tail = StringManager.GetString(typeof(DefectType).FullName, defectType.ToString());
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                    "Defect Detected", "Repeated Defect Warning. ({0})", new object[] { tail });
            }

            // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
            machineIfDataItem = state;
        }
    }
}
