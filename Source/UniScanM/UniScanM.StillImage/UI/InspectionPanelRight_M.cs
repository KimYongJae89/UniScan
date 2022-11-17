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
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using UniScanM.StillImage.Operation;
using DynMvp.UI.Touch;
using System.Threading;
using Infragistics.Win.Misc;
using Infragistics.Win;
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniScanM.Operation;


namespace UniScanM.StillImage.UI
{
    public partial class InspectionPanelRight_M : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        

        List<double>[] ZonePrintingLength = new List<double>[6];

        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelRight_M()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            if (SystemManager.Instance().DeviceController.RobotStage != null)
            {
                AxisPosition[] limitPosition = SystemManager.Instance().DeviceController.RobotStage?.GetLimitPos();
                
                trackBarPos.Minimum = (int)limitPosition[0].Position[0];
                trackBarPos.Maximum = (int)limitPosition[1].Position[0];

                robotPosUpdateTimer.Start();
            }
            InitPrintingLengthGrid();
            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
        }
        private void InitPrintingLengthGrid()
        {
            for (int i = 0; i < 6; i++)
                ZonePrintingLength[i] = new List<double>();

            for (int i = 0; i < 7; i++)
            {
                double val = 0;//(i + 1) + (i + 1) / 10.0;

                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                //dataGridViewRow.Cells[0] = val * 1;
                if (i == 0)
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Gray;
                dataGridViewRow.Height = 40;
                dataGridViewRow.CreateCells(dgv_PatternLength, val * 1, val * 2, val * 3, val * 4, val * 5, val * 6);
                dgv_PatternLength.Rows.Add(dataGridViewRow);
                //dgv_PatternLength.Rows[i].HeaderCell.Value = titles[i];
            }
            dgv_PatternLength.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dgv_PatternLength.ClearSelection();
        }
        private void ResetPrintingLengthGridView()
        {
            lock (ZonePrintingLength)
            {
                for (int i = 0; i < 6; i++)
                    ZonePrintingLength[i]?.Clear();
            }
            for (int y=0; y< 7; y++)
            {
                for(int x=0; x< 6; x++)
                {
                    dgv_PatternLength[x, y].Value = 0;
                    if(y != 0 )  //0번째는 기준값임..
                        dgv_PatternLength[x, y].Style.BackColor = Color.White;
                    //데이터셀만 0 base {열, 행}
                }
            }
        }


        private void UpdatePrintingLengthGridView(InspectionResult inspectionResult)
        {
            //for (int i = 0; i < 6; i++)
            //    ZonePrintingLength[i] = new List<double>();
            UniScanM.StillImage.Data.Model model = SystemManager.Instance().CurrentModel as UniScanM.StillImage.Data.Model;
            //Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
            StillImage.Data.InspectParam inspectParam = (StillImage.Data.InspectParam)model.InspectParam;
            //double threshold = inspectParam.PrintingLengthWarnLevelum ;
            double threshold = 99999;
            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;
            
            if (myInspectionResult.InspectState == "Inspection")
            {
                int zone = myInspectionResult.InspZone;
                if (zone < 0 || zone > 5)
                    return;

                var listZone = ZonePrintingLength[zone];
                //****************************************************************//
                double value = 0;
                switch (Settings.StillImageSettings.Instance().PrintingLengthMeasurMode)
                {
                    case Settings.PrintingLengthMode.PrintingPeriod:
                        value = myInspectionResult.PrintingPeriod;
                        break;

                    case Settings.PrintingLengthMode.PrintingLength:
                        value = myInspectionResult.PrintingLength;
                        break;

                    case Settings.PrintingLengthMode.UnPrintLength:
                        value = myInspectionResult.PrintingPeriod - myInspectionResult.PrintingLength;
                        break;
                }
                //****************************************************************//
                lock (ZonePrintingLength)
                {
                    if (listZone.Count > 0) //선행데이터(0th) 있으면.. value를 차이값으로 변환  //0번째는 기준값임..
                    {
                        double firstValue = listZone.First<double>();
                        value = (value - firstValue) * 1000; //차이값 mm 에서 um 값으로 변환
                    }
                    listZone.Add(value); //데이터가 없으면 초기값 그대로 들어감mm

                    if (listZone.Count > 7) listZone.RemoveAt(1);

                    //double lastval=  listZone.Last<double>();
                    //int row = listZone.Count - 1;
                    //dgv_PatternLength[zone, row].Value = value;  //해당 데이터셀만 갱신 //하면 안됨..해당 column 다해야됨

                    for (int i = 0; i < listZone.Count; i++)
                    {
                        dgv_PatternLength[zone, i].Value = listZone[i]; //데이터셀만 0 base {열, 행}
                        if (i == 0) continue;
                        if (Math.Abs(listZone[i]) > threshold)
                            dgv_PatternLength[zone, i].Style.BackColor = Color.Red;
                        else
                            dgv_PatternLength[zone, i].Style.BackColor = Color.White;
                    }
                }
            }

        }

        private void ErrorManager_OnStartAlarm()
        {

        }

        delegate void ProductInspectedDelegate(InspectionResult inspectionResult);
        public void ProductInspected(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            int inspectZone = -1;
            if (inspectionResult.ExtraResult.ContainsKey("InspectSequence"))
                inspectZone = (int)inspectionResult.ExtraResult["InspectSequence"];
            else
            {
                if (inspectionResult.ExtraResult.ContainsKey("Sequnece"))
                    inspectZone = (int)inspectionResult.ExtraResult["Sequnece"];
            }

            if (SystemState.Instance().OpState == OpState.Idle)
                return;

            string inspectState = myInspectionResult.InspectState;

            //UiHelper.SuspendDrawing(labelState);
            //UiHelper.SuspendDrawing(state);
            labelState.SuspendLayout();
            state.SuspendLayout();

            if (inspectState == "Monitoring")
            // In Monitoring State
            {
                string text = StringManager.GetString(this.GetType().FullName, inspectState);
                UpdateContol(labelState, text, Color.Green, Color.White);
                UpdateContol(state, text, Color.Green, Color.White);
                UpdateResultValue(Color.Green);
            }
            else if (inspectState == "Inspection")//myInspectionResult.ProcessResultList != null)
            // In Inspection State
            {
                UpdatePrintingLengthGridView(inspectionResult);
                //dgv_PatternLength[0, 0].Value = myInspectionResult.PrintingPeriod ;
                //dgv_PatternLength[0, 1].Value = myInspectionResult.PrintingLength  ;

                if (inspectionResult.Judgment == Judgment.Skip)
                // sheet not founded 
                {
                    string text = StringManager.GetString(this.GetType().FullName, "Skip");
                    UpdateContol(labelState, null, Color.Yellow, Color.White);
                    UpdateContol(state, text, Color.Yellow, Color.White);
                    UpdateResultValue(Color.Yellow);
                    UiHelper.ResumeDrawing(state);
                    return;
                }
                
                Data.ProcessResult interestProcessResult = myInspectionResult.ProcessResultList.InterestProcessResult;
                int defectCount = myInspectionResult.ProcessResultList.DefectRectList.Count;
                Rectangle defectRect = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect();

                bool isGood = (interestProcessResult == null ? false : interestProcessResult.IsGood) && defectRect.IsEmpty;
                Color color = isGood ? Color.Green : Color.Red;

                if (interestProcessResult == null)
                // Inspection Fail
                {
                    string text = StringManager.GetString(this.GetType().FullName, "Fail");
                    UpdateContol(labelState, null, Color.Red, Color.White);
                    UpdateContol(state, text, Color.Red, Color.White);
                    UpdateResultValue(Color.Red);
                }
                else
                {
                    string text = StringManager.GetString(this.GetType().FullName, inspectState);
                    UpdateContol(labelState, null, Color.Green, Color.White);
                    UpdateContol(state, text, Color.Green, Color.White);

                    SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

                    // 검출된 크기
                    Data.Feature feature = interestProcessResult.InspPatternInfo.TeachInfo.Feature.Mul(pelSize);

                    // 오프셋 크기
                    Data.Feature offset = interestProcessResult.OffsetValue.Mul(pelSize);

                    // Defect 크기
                    SizeF defectSizeF = new SizeF((float)(defectRect.Width * pelSize.Width), (float)(defectRect.Height * pelSize.Height));

                    Color colorMargin = interestProcessResult.IsMarginGood ? Color.Green : Color.Red;
                    Color colorBlot = interestProcessResult.IsBlotGood ? Color.Green : Color.Red;
                    Color colorDef = defectCount == 0 ? Color.Green : Color.Red;
                    UpdateResultValue(feature, offset, defectCount, defectSizeF, colorMargin, colorBlot, colorDef);
                }
            }
            else if (inspectState == "Teaching")
            {
                string text = StringManager.GetString(this.GetType().FullName, inspectState);
                UpdateContol(labelState, null, Color.CornflowerBlue, Color.Black);
                UpdateContol(state, text, Color.CornflowerBlue, Color.Black);
                UpdateResultValue(Color.CornflowerBlue);
            }
            else if (inspectState == "LightTune")
            {
                string text = StringManager.GetString(this.GetType().FullName, inspectState);
                if (myInspectionResult.LightTuneResult.currentBright >= 0)
                    text += string.Format("({0})", myInspectionResult.LightTuneResult.currentBright);
                UpdateContol(labelState, null, Color.Gold, Color.Black);
                UpdateContol(state, text, Color.Gold, Color.Black);
                UpdateResultValue(Color.Gold);
                //ResetPrintingLengthGridView();//시점이 애매하네.
            }

            //UiHelper.ResumeDrawing(state);
            //UiHelper.ResumeDrawing(labelState);
            state.ResumeLayout();
            labelState.ResumeLayout();
        }

        private void UpdateSummary()
        {
            UniScanM.Data.Production Production = SystemManager.Instance().ProductionManager.CurProduction;
            Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as Data.Production;
            if (production != null)
            {
                this.summaryProduction.Text = production.Total.ToString();
                this.summaryGood.Text = production.Good.ToString();
                this.summaryNG.Text = production.Ng.ToString();
                this.summarySkip.Text = production.Pass.ToString();
                this.summaryMargin.Text = production.MarginDefectCnt.ToString();
                this.summaryBlot.Text = production.BlotDefectCnt.ToString();
                this.summaryPinhole.Text = production.PinholeDefectCnt.ToString();
            }
            else
            {
                this.summaryProduction.Text = "";
                this.summaryGood.Text = "";
                this.summaryNG.Text = "";
                this.summarySkip.Text = "";
                this.summaryMargin.Text = "";
                this.summaryBlot.Text = "";
                this.summaryPinhole.Text = "";
            }
        }

        delegate void UpdateResultValueDelgate1(Color color);
        private void UpdateResultValue(Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateResultValueDelgate1(UpdateResultValue), color);
                return;
            }

            this.marginW.Text = "---.-";
            this.marginW.ForeColor = color;

            this.marginL.Text = "---.-";
            this.marginL.ForeColor = color;

            this.blotW.Text = "--.-";
            this.blotW.ForeColor = color;

            this.blotL.Text = "--.-";
            this.blotL.ForeColor = color;

            this.defectW.Text = "---";
            this.defectW.ForeColor = color;

            this.defectH.Text = "---";
            this.defectH.ForeColor = color;

            this.defectC.Text = "-";
            this.defectC.ForeColor = color;
        }
        
        delegate void UpdateResultValueDelgate3(Data.Feature measure, Data.Feature offset, int defectCount, SizeF defectSize, Color colorMargin, Color colorBlot, Color colorDef);
        private void UpdateResultValue(Data.Feature measure, Data.Feature offset, int defectCount, SizeF defectSize, Color colorMargin, Color colorBlot, Color colorDef)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateResultValueDelgate3(UpdateResultValue), measure, offset, defectCount, defectSize, colorMargin, colorBlot, colorDef);
                return;
            }
            
            string format1 = "{0:000.0}";
            string format2 = "{0:00.0}";
            string format3 = "{0:000}";
            string format4 = "{0:0}";
            // DefectSize
            this.marginW.Text = string.Format(format1, measure.Margin.Width);
            this.marginW.ForeColor = colorMargin;

            this.marginL.Text = string.Format(format1, measure.Margin.Height);
            this.marginL.ForeColor = colorMargin;

            this.blotW.Text = string.Format(format2, offset.Blot.Width);
            this.blotW.ForeColor = colorBlot;

            this.blotL.Text = string.Format(format2, offset.Blot.Height);
            this.blotL.ForeColor = colorBlot;

            this.defectW.Text = string.Format(format3, defectSize.Width);
            this.defectW.ForeColor = colorDef;

            this.defectH.Text = string.Format(format3, defectSize.Height);
            this.defectH.ForeColor = colorDef;

            this.defectC.Text = string.Format(format4, defectCount);
            this.defectC.ForeColor = colorDef;
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (SystemManager.Instance().DeviceController.RobotStage != null)
                trackBarPos.Value = Math.Max(trackBarPos.Minimum, Math.Min(trackBarPos.Maximum , (int)SystemManager.Instance().DeviceController.RobotStage.GetActualPos().Position[0]));
        }

        public void Initialize() { }
        public void ClearPanel()
        {
            ResetPrintingLengthGridView();
        }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
        
        private void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

            ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (curOpState == OpState.Idle)
            {
                UpdateContol(labelState, null, Color.Black, Color.White);
                UpdateContol(state, "", Color.Black, Color.White);
                return;
            }
        }

        private delegate void UpdateContolDelegate(Control control, string text, Color backColor, Color foreColor);
        private void UpdateContol(Control control, string text, Color backColor, Color foreColor)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateContolDelegate(UpdateContol), control, text, backColor, foreColor);
                return;
            }

            //UiHelper.SuspendDrawing(control);
            control.SuspendLayout();
            if(text!=null)
                control.Text = text;
            control.BackColor = backColor;
            control.ForeColor = foreColor;
            //UiHelper.ResumeDrawing(control);
            control.ResumeLayout();
        }

        private void summaryUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateSummary();
        }
    }
}
