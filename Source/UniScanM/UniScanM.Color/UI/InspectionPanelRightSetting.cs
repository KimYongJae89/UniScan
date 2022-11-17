using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Data.UI;

using UniEye.Base;
using UniEye.Base.UI;
using UniEye.Base.Data;
using System.IO;

using UniScanM.ColorSens.Data;
using UniScanM.ColorSens.Operation;

using UniScanM.ColorSens.Settings;


using System.Windows.Forms.DataVisualization.Charting;

using UniScanM.ColorSens.Algorithm;
using DynMvp.UI.Touch;
using System.Threading;
using UniScanM.Operation;

namespace UniScanM.ColorSens.UI
{
    public partial class InspectionPanelRightSetting : UserControl, IInspectionPanel, IOpStateListener, IMultiLanguageSupport
    {
        bool onUpdateData = false;

        List<float> listY_DnLimit = new List<float>();
        List<float> listY_SheetBrightness = new List<float>();
        List<float> listX_Distance = new List<float>();

        public InspectionPanelRightSetting()//생성자
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            initGraph();

            SystemState.Instance().AddOpListener(this);
            StringManager.AddListener(this);
        }

        private delegate void initGraphDelegate();
        void initGraph()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new initGraphDelegate(initGraph));
                return;
            }
            Random rand = new Random();
            chart_CertainTime.Series["graphdata"].Points.Clear();
            chart_FullTime.Series["graphdata"].Points.Clear();

            //double Max = 90;
            //int Distance = 100;
            //for (int i = 0; i < Distance; i++)
            //{
            //    double value = Math.Sin((double)i / 100 * Math.PI * 2) * Max / 2 + Max / 2 + 10;
            //    chart_CertainTime.Series["graphdata"].Points.Add(value);
            //    //chart_CertainTime.Series["graphdata"].Points.Add(rand.Next(100));
            //}


            ////chart_CertainTime.ChartAreas[0].AxisY.Minimum = 0;
            ////chart_CertainTime.ChartAreas[0].AxisY.Maximum = 100;

            ////chart_CertainTime.ChartAreas[0].AxisX.Minimum = 0;
            ////chart_CertainTime.ChartAreas[0].AxisX.Maximum = Distance;


            //Distance = 5000;
            //for (int i = 0; i < Distance; i++)
            //{
            //    double value = Math.Sin((double)i / 5000 * Math.PI * 2) * Max / 2 + Max / 2 + 10;
            //    chart_FullTime.Series["graphdata"].Points.Add(value);
            //    // chart_FullTime.Series["graphdata"].Points.Add(rand.Next(100));
            //}

            //chart_FullTime.ChartAreas[0].AxisY.Minimum = 0;
            //chart_FullTime.ChartAreas[0].AxisY.Maximum = 100;

            //chart_FullTime.ChartAreas[0].AxisX.Minimum = 0;
            //chart_FullTime.ChartAreas[0].AxisX.Maximum = Distance;

        }
        private void InspectPage_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < this.positionIndicator.Length; i++)
            //{
            //    this.positionIndicator[i].Font = UiHelper.AutoFontSize(this.positionIndicator[i], this.positionIndicator[i].Text);
            //    this.positionIndicator[i].BackColor = Color.LightSteelBlue;
            //}
            timer1.Interval = 200;
            timer2.Interval = 2000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RevisionGraph1();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            RevisionGraph2();
        }

        //IMainTabPage
        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        public List<IInspectionPanel> InspectionPanelList => throw new NotImplementedException();

        //IInspectionPanel
        public void Initialize()
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if(InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            listY_DnLimit.Clear();
            listY_SheetBrightness.Clear();
            listX_Distance.Clear();

            chart_CertainTime.Series["graphdata"].Points.Clear();
            //chart_CertainTime.Series["limit"].Points.Clear();
            chart_FullTime.Series["graphdata"].Points.Clear();
        }
        //IInspectionPanel
        public void EnterWaitInspection()
        {
            //Diable Parameter
            Enable_ParameterControlRun(false);
           // initGraph();


            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;

            SetYStrip(colorSensorParam.SpecLimit);
        }
        //IInspectionPanel
        public void ExitWaitInspection() //inspectrunner 에서 종료시 호출해줌
        {
            //Enalble Parameter
            Enable_ParameterControlRun(true);
        }
        //IInspectionPanel
        public void OnPreInspection()
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult)
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult)
        {
            throw new NotImplementedException();
        }

        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            updatedata(false);

            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            bool isGood = myInspectionResult.IsGood();
            Color forcolor = isGood ? Color.White : Color.Red;
            Color backcolor = isGood ? Color.Green : Color.Black;
            string message = string.Format("{0}", isGood ? "OK" : "NG");
            string certainString = StringManager.GetString(this.GetType().FullName, "Certain Time");
            //label_chartCertainTime.Text = string.Format("{0}{1}", certainString, myInspectionResult.DeltaBrightness.ToString("F2"));

            if (myInspectionResult.Judgment != Judgment.Skip)
            {
                //Add data
                lock (listY_SheetBrightness)
                {
                    // listY_SheetBrightness.Add(myInspectionResult.SheetBrightness);
                    listY_SheetBrightness.Add(myInspectionResult.DeltaBrightness);
                    listY_DnLimit.Add((float)this.ngRange.Value);
                    listX_Distance.Add((float)myInspectionResult.RollDistance);
                }
            }
            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel != null)
            {
                ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
                if (colorSensorParam == null)
                {
                    SetYStrip(colorSensorParam.SpecLimit);
                }
            }
        }

        void RevisionGraph1()
        {
            int count = 0;
            int XInterval = (int)ColorSensorSettings.Instance().GraphCertain_XDistance;
            int YInterval = (int)ColorSensorSettings.Instance().GraphCertain_YLength;
            int thickness = (int)ColorSensorSettings.Instance().GraphCertain_Thick;

            if (listY_SheetBrightness.Count < 1)
            {
                return;
            }
            try
            {
                lock (listY_SheetBrightness)
                {
                    float max = listX_Distance.LastOrDefault();
                    if (max <= 0)
                        return;

                    float min = Math.Max(0, max - XInterval);

                    int endIndex = listX_Distance.Count;

                    List<float> lx = listX_Distance.FindAll(data => data >= min && data <= max);
                    if (lx.Count < 1) return;

                    for (int i = 0; i < lx.Count; i++)
                    {
                        int J = lx.Count(f => f == lx[i]);
                        if (J > 0)
                        {
                            float step = 1f / J;
                            for (int j = 0; j < J; j++)
                                lx[i + j] += step * j;
                        }
                    }

                    int beginIndex = endIndex - lx.Count;
                    if (beginIndex < 0) beginIndex = 0;

                    if (endIndex - beginIndex < 0)
                    {
                        return;
                    }


                    //System.Diagnostics.Debug.WriteLine(beginIndex.ToString());
                    List<float> ly = listY_SheetBrightness.GetRange(beginIndex, endIndex - beginIndex);

                    chart_CertainTime.ChartAreas[0].AxisY.Minimum = Math.Min(ly.Min(), 0);
                    chart_CertainTime.ChartAreas[0].AxisY.Maximum = Math.Max(ly.Max(), (float)YInterval);

                    double yInterval = (chart_CertainTime.ChartAreas[0].AxisY.Maximum - chart_CertainTime.ChartAreas[0].AxisY.Minimum) / 4.0f;
                    chart_CertainTime.ChartAreas[0].AxisY.MajorGrid.Interval = yInterval;
                    chart_CertainTime.ChartAreas[0].AxisY.Interval = yInterval;
                    chart_CertainTime.ChartAreas[0].AxisY.LabelStyle.Interval = yInterval;
                    chart_CertainTime.ChartAreas[0].AxisY.LabelStyle.Format = "F1";

                    chart_CertainTime.ChartAreas[0].AxisX.Minimum = lx.Min();
                    chart_CertainTime.ChartAreas[0].AxisX.Maximum = lx.Min() + (float)XInterval;

                    double xInterval = (chart_CertainTime.ChartAreas[0].AxisX.Maximum - chart_CertainTime.ChartAreas[0].AxisX.Minimum) / 4.0f;
                    chart_CertainTime.ChartAreas[0].AxisX.Interval = xInterval;
                    chart_CertainTime.ChartAreas[0].AxisX.MajorGrid.Interval = xInterval;
                    chart_CertainTime.ChartAreas[0].AxisX.Interval = xInterval;
                    chart_CertainTime.ChartAreas[0].AxisX.LabelStyle.Interval = xInterval;
                    chart_CertainTime.ChartAreas[0].AxisX.LabelStyle.Format = "F0";

                    chart_CertainTime.Series["graphdata"].Points.DataBindXY(lx, ly);
                    chart_CertainTime.Series["graphdata"].BorderWidth = thickness;

                    List<float> limitSeriesData = listY_DnLimit.GetRange(beginIndex, endIndex - beginIndex);
                    //chart_CertainTime.Series["limit"].Points.DataBindXY(lx, limitSeriesData);
                }
            }
            catch (Exception ex)
            {

            }

            //chart_CertainTime.ChartAreas[0].AxisY.StripLines.Add(new StripLine());
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].BackColor = Color.FromArgb(80, 252, 180, 65);
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].StripWidth = 20;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].Interval = 10000;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 100;
        }
        /*
        void RevisionGraph1()
        {
            int count = 0;
            int XInterval = (int)ColorSensorSettings.Instance().GraphCertain_XDistance;
            int YInterval = (int)ColorSensorSettings.Instance().GraphCertain_YLength;
            int thickness = (int)ColorSensorSettings.Instance().GraphCertain_Thick;

            if (listY_SheetBrightness.Count < 1)
            {
                return;
            }

            lock (listY_SheetBrightness)
            {
                float max = listX_Distance.LastOrDefault();
                if (max <= 0)
                    return;

                float min = Math.Max(0, max - XInterval);

                int endIndex = listX_Distance.Count;

                List<float> lx = listX_Distance.FindAll(data => data >= min && data <= max);
                if (lx.Count < 1) return;

                for (int i = 0; i < lx.Count; i++)
                {
                    int J = lx.Count(f => f == lx[i]);
                    if(J>0)
                    {
                        float step = 1f / J;
                        for (int j = 0; j < J; j++)
                            lx[i + j] += step * j;
                    }
                }

                int beginIndex = endIndex - lx.Count;
                if (beginIndex < 0) beginIndex = 0;

                //System.Diagnostics.Debug.WriteLine(beginIndex.ToString());
                List<float> ly = listY_SheetBrightness.GetRange(beginIndex, endIndex - beginIndex);

                chart_CertainTime.ChartAreas[0].AxisY.Minimum = Math.Min(ly.Min(), 0);
                chart_CertainTime.ChartAreas[0].AxisY.Maximum = Math.Max(ly.Max(), (float)YInterval);

                double yInterval = (chart_CertainTime.ChartAreas[0].AxisY.Maximum - chart_CertainTime.ChartAreas[0].AxisY.Minimum) / 4.0f;
                chart_CertainTime.ChartAreas[0].AxisY.MajorGrid.Interval = yInterval;
                chart_CertainTime.ChartAreas[0].AxisY.Interval = yInterval;
                chart_CertainTime.ChartAreas[0].AxisY.LabelStyle.Interval = yInterval;
                chart_CertainTime.ChartAreas[0].AxisY.LabelStyle.Format = "F1";

                chart_CertainTime.ChartAreas[0].AxisX.Minimum = lx.Min();
                chart_CertainTime.ChartAreas[0].AxisX.Maximum = lx.Min() + (float)XInterval;

                double xInterval = (chart_CertainTime.ChartAreas[0].AxisX.Maximum - chart_CertainTime.ChartAreas[0].AxisX.Minimum) / 4.0f;
                chart_CertainTime.ChartAreas[0].AxisX.Interval = xInterval;
                chart_CertainTime.ChartAreas[0].AxisX.MajorGrid.Interval = xInterval;
                chart_CertainTime.ChartAreas[0].AxisX.Interval = xInterval;
                chart_CertainTime.ChartAreas[0].AxisX.LabelStyle.Interval = xInterval;
                chart_CertainTime.ChartAreas[0].AxisX.LabelStyle.Format = "F0";

                chart_CertainTime.Series["graphdata"].Points.DataBindXY(lx, ly);
                chart_CertainTime.Series["graphdata"].BorderWidth = thickness;

                List<float> limitSeriesData = listY_DnLimit.GetRange(beginIndex, endIndex - beginIndex);
                //chart_CertainTime.Series["limit"].Points.DataBindXY(lx, limitSeriesData);
            }

            //chart_CertainTime.ChartAreas[0].AxisY.StripLines.Add(new StripLine());
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].BackColor = Color.FromArgb(80, 252, 180, 65);
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].StripWidth = 20;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].Interval = 10000;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 100;
        }
        */
        void RevisionGraph2()
        {
            int XInterval = (int)ColorSensorSettings.Instance().GraphWhole_XDistance;
            int YInterval = (int)ColorSensorSettings.Instance().GraphWhole_YLength;
            int thickness = (int)ColorSensorSettings.Instance().GraphWhole_Thick;

            if (listY_SheetBrightness.Count < 1)
            {
                return;
            }

            lock (listY_SheetBrightness)
            {
                int denominator = ColorSensorSettings.Instance().GraphWhole_denominator;// list_DataX.Count/100;

                List<float> Fx = new List<float>();
                List<float> Fy = new List<float>();
                int dataCount = Math.Min(listX_Distance.Count, listY_SheetBrightness.Count);
                int loop = (dataCount / denominator) + 1;

                for (int i = 0; i < loop; i++)
                {
                    int samplingIndexFrom = i * denominator;
                    int samplingIndexTo = Math.Min(dataCount, (i + 1) * denominator);
                    int sampleCount = samplingIndexTo - samplingIndexFrom;
                    if (sampleCount > 0)
                    {
                        Fx.Add(listX_Distance[samplingIndexFrom]);
                        Fy.Add(listY_SheetBrightness.GetRange(samplingIndexFrom, sampleCount).Average());
                    }
                }

                chart_FullTime.ChartAreas[0].AxisY.Minimum = Math.Min(Fy.Min(), 0);
                chart_FullTime.ChartAreas[0].AxisY.Maximum = Math.Max(Fy.Max(), (float)YInterval);

                chart_FullTime.ChartAreas[0].AxisX.Maximum = Fx.Max();
                chart_FullTime.ChartAreas[0].AxisX.Minimum = Math.Max(0, Fx.Max() - (float)XInterval);
                chart_FullTime.Series["graphdata"].Points.DataBindXY(Fx, Fy);
                chart_FullTime.Series["graphdata"].BorderWidth = thickness;
            }
        }

        //IInspectionPanel
        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, DynMvp.InspData.InspectionResult inspectionResult)
        {

        }
        //IInspectionPanel
        public void OnPostInspection()
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public void ModelChanged(DynMvp.Data.Model model)
        {
            throw new NotImplementedException();
        }
        //IInspectionPanel
        public void InfomationChanged(object obj)
        {
            //throw new NotImplementedException(); 
            if (obj == null) return;
            else
            {
                bool update = (bool)obj;
                updatedata(update);
            }

        }

        private delegate void UpdateControlTextDelegate(Label labelResult, string text, Color? foreColor);
        private void UpdateControlText(Control control, string text, Color? foreColor = null)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateControlTextDelegate(UpdateControlText), control, text, foreColor);
                return;
            }
            //if (control is Label)
            //    control.Font = DynMvp.UI.UiHelper.AutoFontSize((Label)control, text);
            if (foreColor != null)
                control.ForeColor = foreColor.Value;

            control.Text = StringManager.GetString(this.GetType().FullName, text);
        }

        private void UpdateResultValue(Color color)
        {
            //UpdateControlText(this.labelResult, "-----", color);
        }

        public void UpdateControl(string item, object value)
        {
            if(item == "updatedata(false)" && value == null)
            {
                updatedata(false);
            }
        }

        public void EnableControls()
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }
        public Production GetCurrentProduction() //IInspectionPage
        {
            throw new NotImplementedException();
        }

        void StratRevisonGraph()
        {
            //chart_CertainTime.Series[0].Points.Clear();
            //chart_FullTime.Series[0].Points.Clear();

            listY_SheetBrightness.Clear();
            listX_Distance.Clear();

            chart_CertainTime.Series["graphdata"].Points.Clear();
            chart_FullTime.Series["graphdata"].Points.Clear();
            
            timer1.Start(); //Revision Graph
            timer2.Start(); //Revision Graph

            //initGraph();
        }

        void StopRevisionGraph()
        {
            Task task =Task.Run(() =>
            {
                Thread.Sleep(1000);
                timer1.Stop();
                timer2.Stop();
            });
            //task.Wait();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }


        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            if (curOpState == OpState.Idle) //stop
            {
                StopRevisionGraph();
            }
            //else if (curOpState == OpState.Wait) //검사대기-> 러닝상태
            //{
            //    if (prevOpState == OpState.Idle)
            //    {
            //        //// 정지->대기. 검사결과창 제거
            //    }
            //    else if (prevOpState == OpState.Inspect)
            //    {
            //        // 검사->대기.
            //    }
            //}
            else if (curOpState == OpState.Inspect) //running inspect
            {
                StratRevisonGraph();
            }
        }

        delegate void Enable_ParameterControlDelegate(bool en);
        private void Enable_ParameterControlRun(bool en)
        {
            if(InvokeRequired)
            {
                Invoke(new Enable_ParameterControlDelegate(Enable_ParameterControlRun), en);
                return;
            }

            button_GetRollerDiaFromPLC.Enabled = en;
            rollerDia.Enabled = en;
            button_AutoLight.Enabled = en;
        }

        private void Enable_ParameterControlComm(bool en)
        {
            if (InvokeRequired)
            {
                Invoke(new Enable_ParameterControlDelegate(Enable_ParameterControlComm), en);
                return;
            }

            lightValue.Enabled = en;
            ngRange.Enabled = en;
        }

        delegate void UpdateDataDelegate(bool update);
        private void updatedata(bool update)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(updatedata), update);
                return;
            }


            if (onUpdateData == true)
                return;

            onUpdateData = true;

            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;

            if (update) //UI -> Model
            {
                colorSensorParam.SpecLimit = Convert.ToDouble(ngRange.Value);
                colorSensorParam.LightValue = (uint)Math.Round((Convert.ToDouble(lightValue.Value) * 255.0 / 100.0 ));
                colorSensorParam.PatternPeriod = Convert.ToDouble(rollerDia.Value) * Math.PI;

                SystemManager.Instance().ModelManager.SaveModel(curmodel);
            }
            else  //Model -> UI
            {
                ngRange.Value = Math.Min(100, (decimal)colorSensorParam.SpecLimit);
                lightValue.Value = (decimal)(colorSensorParam.LightValue / 255.0 * 100.0);
                double RollerDia = Math.Max(1, colorSensorParam.PatternPeriod / Math.PI);
                rollerDia.Value = Math.Max(rollerDia.Minimum, Math.Min(rollerDia.Maximum, (decimal)RollerDia));

                SetYStrip(colorSensorParam.SpecLimit);
            }

            onUpdateData = false;
        }

        private void panel_Recipe_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_GetRollerDiaFromPLC_Click(object sender, EventArgs e)//button_GetRollerDiaFromPLC
        {
            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;

            double rollerDia = SystemManager.Instance().InspectStarter.GetRollerDia();;
            rollerDia = rollerDia < 50 ? 50 : rollerDia;
            rollerDia = rollerDia > 200 ? 200 : rollerDia;
            colorSensorParam.PatternPeriod = rollerDia * Math.PI;

            SystemManager.Instance().ModelManager.SaveModel(curmodel);

            updatedata(false);
        }

        private void button_AutoLight_Click(object sender, EventArgs e)
        {
            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;

            progressForm = new SimpleProgressForm();
            progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            progressForm.TopMost = true;
            progressForm.Text = "Tune";
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            progressForm.Show(() =>
            {
                lighttune = new autoLightTune(0.03);

                foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                    imageDevice.ImageGrabbed = TuneImageGrabbed;

                uint lightval = (uint)lighttune.tune();
                lightval = lightval < 5 ? 5 : lightval;
                lightval = lightval > 100 ? 100 : lightval;
                colorSensorParam.LightValue = lightval;
                SystemManager.Instance().ModelManager.SaveModel(curmodel);

            }, cancellationTokenSource);

            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                imageDevice.ImageGrabbed = null;

            updatedata(false);
        }

        FinderSheetPeriod brightCtrl;
        SimpleProgressForm progressForm;
        autoLightTune lighttune;
        private void button_GetBrightness_Click(object sender, EventArgs e)
        {
            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;
            
            progressForm = new SimpleProgressForm();
            progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            progressForm.TopMost = true;
            progressForm.Text = "Calc";
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            progressForm.Show(() =>
            {
                brightCtrl = new FinderSheetPeriod();

                foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                    imageDevice.ImageGrabbed = BrigtnessImageGrabbed;

                colorSensorParam.AverageBrightnessSheet = brightCtrl.CalculateOnePatternBright(colorSensorParam.PatternPeriod);
                SystemManager.Instance().ModelManager.SaveModel(curmodel);

            }, cancellationTokenSource);

            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                imageDevice.ImageGrabbed = null;

            updatedata(false);
        }

        protected void BrigtnessImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            progressForm.MessageText = string.Format("Calc Value : {0:0.0} %", (float)brightCtrl.GrabbedCount / (float)brightCtrl.TargetGrabCount * 100.0f);
        }

        protected void TuneImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
             progressForm.MessageText = string.Format("Light Tune : {0:0.0} %", (float)lighttune.CurTryCount / (float)lighttune.TryCount * 100.0f);
        }

        private void lsl_ValueChanged(object sender, EventArgs e)
        {
            updatedata(true);
        }

        private void Recipe_ValueChanged(object sender, EventArgs e)
        {
            updatedata(true);
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

            ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();
        }

        private void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");

            panelParam.Enabled = !flag;
            Enable_ParameterControlComm(panelParam.Enabled);
        }
        
        delegate void Chart1_DataBoundDelegate(double limit);

        protected void SetYStrip(double limit)
        {
            if (InvokeRequired)
            {
                Invoke(new Chart1_DataBoundDelegate(SetYStrip), limit);
                return;
            }
            chart_CertainTime.ChartAreas[0].AxisY.StripLines.Clear();
            chart_FullTime.ChartAreas[0].AxisY.StripLines.Clear();
            // Find point with maximum Y value 
            //DataPoint maxValuePoint = chart_CertainTime.Series[0].Points.FindMaxByValue();
            DataPoint maxValuePoint = new DataPoint();
            maxValuePoint.SetValueY(limit);
            chart_CertainTime.ChartAreas[0].AxisY.StripLines.Add(
                new StripLine()
                {
                    BorderColor = Color.Red,
                    IntervalOffset = maxValuePoint.YValues[0],
                    BorderWidth = ColorSensorSettings.Instance().GraphCertain_Thick
                    //Text = "Max Value"
                });

            chart_FullTime.ChartAreas[0].AxisY.StripLines.Add(
            new StripLine()
            {
                BorderColor = Color.Red,
                IntervalOffset = maxValuePoint.YValues[0],
                BorderWidth = ColorSensorSettings.Instance().GraphCertain_Thick
                //Text = "Max Value"
            });
        }
    }
}
