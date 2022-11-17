using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Serial;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.MachineIF;
using UniScanG.UI;

namespace UniScanG.Gravure.UI.Inspect
{
    public partial class InfoPanel : UserControl, IInspectStateListener, IOpStateListener, IMultiLanguageSupport, IModelListener
    {
        //List<float> sheetHeightList = new List<float>();
        MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
        System.Windows.Forms.Timer updateTimer = null;

        //float sheetLengthLow, sheetLengthMid, sheetLengthHigh;
        double inspectTimeMs;

        IInfoPanelBufferState infoPanelBufferState;


        public InfoPanel(IInfoPanelBufferState infoPanelBufferState, bool showBurnerState)
        {
            InitializeComponent();

            StringManager.AddListener(this);
            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;
            //UpdateLanguage();

            bool simpleReport = UniScanG.Common.Settings.SystemTypeSettings.Instance().ShowSimpleReportLotList;

            this.labelProductionErase.Visible = this.tableLayoutPanelEraseOk.Visible
                = this.labelProductionEraseDuplicate.Visible = this.tableLayoutPanelEraseDuplicate.Visible
                = !simpleReport && showBurnerState;

            // 항상 가리기
            this.labelProductionEraseDuplicate.Visible = this.tableLayoutPanelEraseDuplicate.Visible = false;

            this.labelSticker.Visible = this.sticker.Visible = !simpleReport && AlgorithmSetting.Instance().UseExtSticker;
            this.labelMargin.Visible = this.margin.Visible = !simpleReport && AlgorithmSetting.Instance().UseExtMargin;
            this.labelTransform.Visible = this.transform.Visible = !simpleReport && AlgorithmSetting.Instance().UseExtTransfrom;

            //this.productionCiricalRoll

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            ((UnitBaseInspectRunner)SystemManager.Instance().InspectRunnerG).AddInspectDoneDelegate(InspectDone);
            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);

            this.infoPanelBufferState = infoPanelBufferState;
            panelBufferUsage.Controls.Add((Control)infoPanelBufferState);
            Clear();

            labelChipShare.Visible = tableLayoutPanelChipShare.Visible = false;

            ColorTable.OnColorTableUpdated += UpdateLabelsColor;
        }

        private void ProductionManager_OnLotChanged()
        {
            this.inspectTimeMs = 0;
            UpdateData();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void InspectDoneDelegate(InspectionResult inspectionResult);
        public void InspectDone(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InspectDoneDelegate(InspectDone), inspectionResult);
                return;
            }

            if (inspectionResult.Judgment == Judgment.Skip)
                return;

            this.inspectTimeMs = inspectionResult.AlgorithmResultLDic.Values.Sum(f => f.SpandTime.TotalMilliseconds);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            this.updateTimer.Stop();
            try
            {
                UpdateData();
            }
            finally
            {
                this.updateTimer.Start();
            }
        }

        delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            UpdateLineSpeed();

            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG != null)
            {
                // Product Info
                UiHelper.SetControlText(this.lotNo, productionG.LotNo);
                UiHelper.SetControlText(this.startTime, productionG.StartTime.ToString("MM/dd HH:mm"));
                UiHelper.SetControlText(this.endTime, productionG.LastUpdateTime.ToString("MM/dd HH:mm"));

                // Inspect Info
                UiHelper.SetControlText(this.productionInspect, productionG.Done.ToString());
                UiHelper.SetControlText(this.productionInspectNg, productionG.Ng.ToString());
                UiHelper.SetControlText(this.productionErase, productionG.EraseNum.ToString());
                UiHelper.SetControlText(this.productionEraseOk, productionG.EraseGood.ToString());
                UiHelper.SetControlText(this.productionEraseDuplicate, productionG.EraseDuplicated.ToString());
                UiHelper.SetControlText(this.productionInspectRatio, productionG.NgRatio.ToString("F1"));
                UiHelper.SetControlText(this.productionCiricalRoll, $"{productionG.CriticalPatternNum} / {AdditionalSettings.Instance().CriticalRollAlarm.Count}");
            }

            UiHelper.SetControlText(processTime, this.inspectTimeMs.ToString("F0"));
            
            AlarmManager alarmManager = SystemManager.Instance().InspectRunnerG.AlarmManager;
            UiHelper.SetControlText(this.productionHeightMid, alarmManager.Middle80.ToString("F2"));
            UiHelper.SetControlText(this.productionHeightDiff, alarmManager.Diff10.ToString("F2"));

            // Defect Info
            UpdateDefectInfo();

            // Buffer Info
            this.infoPanelBufferState.UpdateBufferState();
            //UpdateBufferState();
        }

        private void UpdateLineSpeed()
        {
            UiHelper.SetControlText(progressSpdSv, this.infoPanelBufferState?.GetLineSpdSv());
            UiHelper.SetControlText(progressSpdPv, this.infoPanelBufferState?.GetLineSpdPv());
        }

        private void UpdateDefectInfo()
        {
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG == null)
                return;

            if (patternRadioButton.Checked == true)
            {
                this.sheetAttack.Text = productionG.Patterns.SheetAttack.ToString();
                this.noPrint.Text = productionG.Patterns.NoPrint.ToString();
                this.dielectric.Text = productionG.Patterns.Dielectric.ToString();
                this.pinHole.Text = productionG.Patterns.PinHole.ToString();
                this.spread.Text = productionG.Patterns.Spread.ToString();
                this.sticker.Text = productionG.Patterns.Sticker.ToString();
                this.margin.Text = productionG.Patterns.Margin.ToString();
                this.transform.Text = productionG.Patterns.Transform.ToString();
            }
            else if (defectRadioButton.Checked == true)
            {
                this.sheetAttack.Text = productionG.Defects.SheetAttack.ToString();
                this.noPrint.Text = productionG.Defects.NoPrint.ToString();
                this.dielectric.Text = productionG.Defects.Dielectric.ToString();
                this.pinHole.Text = productionG.Defects.PinHole.ToString();
                this.spread.Text = productionG.Defects.Spread.ToString();
                this.sticker.Text = productionG.Defects.Sticker.ToString();
                this.margin.Text = productionG.Defects.Margin.ToString();
                this.transform.Text = productionG.Defects.Transform.ToString();
            }
        }

        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InspectStateChangedDelegate(InspectStateChanged), curInspectState);
                return;
            }

            switch (curInspectState)
            {
                case UniEye.Base.Data.InspectState.Run:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Run;
                    break;
                case UniEye.Base.Data.InspectState.Wait:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
                case UniEye.Base.Data.InspectState.Done:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
            }
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
            switch (curOpState)
            {
                case OpState.Idle:
                    status.Appearance.BackColor = Colors.Idle;
                    break;
                case OpState.Wait:
                    status.Appearance.BackColor = Colors.Wait;
                    break;
                case OpState.Alarm:
                    status.Appearance.BackColor = Colors.Alarm;
                    break;
                case OpState.Inspect:
                    InspectStateChanged(SystemState.Instance().InspectState);
                    //UpdateData();
                    break;
            }
            //UpdateData();
        }

        private void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Clear));
                return;
            }

            lotNo.Text = string.Empty;
            startTime.Text = string.Empty;

            processTime.Text = string.Empty;

            UiHelper.SetControlText(productionInspect, "");
            UiHelper.SetControlText(productionErase, "");

            UiHelper.SetControlText(productionInspectNg, "");
            UiHelper.SetControlText(productionEraseOk, "");

            UiHelper.SetControlText(productionInspectRatio, "");
            UiHelper.SetControlText(chipShare, "");
            
            sheetAttack.Text = string.Empty;
            noPrint.Text = string.Empty;
            dielectric.Text = string.Empty;
            pinHole.Text = string.Empty;
            sticker.Text = string.Empty;

            UiHelper.SetControlText(productionHeightMid, string.Empty);
        }

        public void ModelChanged()
        {
            Clear();
            if (SystemManager.Instance().CurrentModel != null)
                UiHelper.SetControlText(chipShare, SystemManager.Instance().CurrentModel.ChipShare100p.ToString("F2"));
        }

        public void ModelTeachDone(int camId)
        {
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            if (model != null)
                UiHelper.SetControlText(chipShare, model.ChipShare100p.ToString("F2"));
        }

        public void ModelRefreshed() { }
        
        private delegate void UpdateUltraProgreessBarDelegate(Infragistics.Win.UltraWinProgressBar.UltraProgressBar progressBar, int maxValue, int curValue);
        private void UpdateUltraProgreessBar(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int maxValue, int curValue)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateUltraProgreessBarDelegate(UpdateUltraProgreessBar), ultraProgressBar, maxValue, curValue);
                return;
            }

            ultraProgressBar.Maximum = maxValue;
            ultraProgressBar.Value = Math.Min(curValue, maxValue);

            float load = (maxValue == 0) ? 0 : curValue * 100.0f / maxValue;
            ultraProgressBar.Text = string.Format("{0:0.0}% ({1}/{2})", load, curValue, maxValue);
        }

        private void InfoPanel_Load(object sender, EventArgs e)
        {
            UpdateLabelsColor();

            this.infoPanelBufferState.Clear();

            this.updateTimer = new System.Windows.Forms.Timer();
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += updateTimer_Tick;
            this.updateTimer.Start();
        }

        private void UpdateLabelsColor()
        {
            Data.ColorTable.UpdateControlColor(this.labelSheetAttack, DefectType.Attack);
            Data.ColorTable.UpdateControlColor(this.labelNoPrint, DefectType.Noprint);
            Data.ColorTable.UpdateControlColor(this.labelCoating, DefectType.Coating);
            Data.ColorTable.UpdateControlColor(this.labelPinHole, DefectType.PinHole);
            Data.ColorTable.UpdateControlColor(this.labelSpread, DefectType.Spread);
            Data.ColorTable.UpdateControlColor(this.labelSticker, DefectType.Sticker);
            Data.ColorTable.UpdateControlColor(this.labelMargin, DefectType.Margin);
            Data.ColorTable.UpdateControlColor(this.labelTransform, DefectType.Transform);
        }

        private void DefectInfoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                UpdateDefectInfo();
        }

        private void progressSpdPv_DoubleClick(object sender, EventArgs e)
        {
            this.infoPanelBufferState.TogCurLineSpdSpdType();
        }

        private void InfoPanel_VisibleChanged(object sender, EventArgs e)
        {
            bool use = AdditionalSettings.Instance().CriticalRollAlarm.Use;
            UiHelper.SetControlVisible(this.tableLayoutProductionCiricalRoll, use);
            UiHelper.SetControlVisible(this.labelProductionCiricalRoll, use);
        }

        private void buttonProductionCiricalRollReset_Click(object sender, EventArgs e)
        {
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG != null)
            {
                productionG.ResetCriticalPatternNum();
                productionG.Save();
                SystemManager.Instance().ProductionManager.Save();
            }
        }
    }
}