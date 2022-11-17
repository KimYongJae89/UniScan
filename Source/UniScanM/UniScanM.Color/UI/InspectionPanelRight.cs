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


namespace UniScanM.ColorSens.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IOpStateListener, IMultiLanguageSupport
    {
        List<float> listY_SheetBrightness = new List<float>();
        List<float> listX_Distance = new List<float>();
        
        public InspectionPanelRight()//생성자
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            initGraph();

            //chart_CertainTime.ChartAreas[0].AxisY.StripLines.Add(new StripLine());
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].BackColor = Color.FromArgb(80, 252, 180, 65);
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].StripWidth = 20;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].Interval = 10000;
            //chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 100;

            SystemState.Instance().AddOpListener(this);
            StringManager.AddListener(this);
        }
        
        void initGraph()
        {
            Random rand = new Random();
            chart_CertainTime.Series["graphdata"].Points.Clear();
            chart_FullTime.Series["graphdata"].Points.Clear();

            double Max = 90;
            int Distance = 100;
            for (int i = 0; i < Distance; i++)
            {
                double value = Math.Sin((double)i / 100 * Math.PI * 2) * Max / 2 + Max / 2 + 10;
                chart_CertainTime.Series["graphdata"].Points.Add(value);
                //chart_CertainTime.Series["graphdata"].Points.Add(rand.Next(100));
            }
            Distance = 5000;
            for (int i = 0; i < Distance; i++)
            {
                double value = Math.Sin((double)i / 5000 * Math.PI * 2) * Max / 2 + Max / 2 + 10;
                chart_FullTime.Series["graphdata"].Points.Add(value);
                // chart_FullTime.Series["graphdata"].Points.Add(rand.Next(100));
            }
            
        }
        private void InspectPage_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < this.positionIndicator.Length; i++)
            //{
            //    this.positionIndicator[i].Font = UiHelper.AutoFontSize(this.positionIndicator[i], this.positionIndicator[i].Text);
            //    this.positionIndicator[i].BackColor = Color.LightSteelBlue;
            //}
            timer1.Interval = 200;
            timer2.Interval = 1000;
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
        public void ClearPanel()
        {
            //throw new NotImplementedException();
        }
        //IInspectionPanel
        public void EnterWaitInspection()
        {



        }
        //IInspectionPanel
        public void ExitWaitInspection() //inspectrunner 에서 종료시 호출해줌
        {



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
            ColorSens.Data.InspectionResult myInspectionResult = (ColorSens.Data.InspectionResult)inspectionResult;

            {
                bool isGood =  myInspectionResult.IsGood();
                Color forcolor = isGood ? Color.White : Color.Red;
                Color backcolor = isGood ? Color.Green : Color.Black;
                string message = string.Format("{0}", isGood ? "OK" : "NG");

                //label_chartCertainTime.Text = string.Format("Certain [{0}]", myInspectionResult.SheetBrightness.ToString("F2"));
            }
            //Add data
            lock (listY_SheetBrightness)
            {
                listY_SheetBrightness.Add(myInspectionResult.SheetBrightness);
                listX_Distance.Add((float)myInspectionResult.RollDistance);
            }
        }

        void RevisionGraph1()
        {
            int count = 0;
            int XInterval = (int)ColorSensorSettings.Instance().GraphCertain_XDistance;
            int YInterval = (int)ColorSensorSettings.Instance().GraphCertain_YLength;

            if (listY_SheetBrightness.Count < 1) return;

            lock (listY_SheetBrightness)
            {
                float max = listX_Distance[listX_Distance.Count - 1];
                float min = max - XInterval;
                min = min < 0 ? 0 : min;

                int endIndex = listX_Distance.Count - 1;

                List<float> lx = listX_Distance.FindAll(data => data > min && data < max);

                if (lx.Count < 1) return;

                int beginIndex = endIndex - lx.Count;
                if (beginIndex < 0) beginIndex = 0;

                List<float> ly = listY_SheetBrightness.GetRange(beginIndex, endIndex - beginIndex);

                chart_CertainTime.ChartAreas[0].AxisY.Minimum = ly.Min() - YInterval/2;
                chart_CertainTime.ChartAreas[0].AxisY.Maximum = ly.Max() + YInterval/2;

                chart_CertainTime.ChartAreas[0].AxisX.Minimum = lx.Min();
                chart_CertainTime.ChartAreas[0].AxisX.Maximum = lx.Min() + XInterval;

                chart_CertainTime.Series["graphdata"].Points.DataBindXY(lx, ly);
            }

            chart_CertainTime.ChartAreas[0].AxisY.StripLines.Add(new StripLine());
            chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].BackColor = Color.FromArgb(80, 252, 180, 65);
            chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].StripWidth = 20;
            chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].Interval = 10000;
            chart_CertainTime.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 100;
        }

        void RevisionGraph2()
        {
            int XInterval = (int)ColorSensorSettings.Instance().GraphWhole_XDistance;
            int YInterval = (int)ColorSensorSettings.Instance().GraphWhole_YLength;
    
            if (listY_SheetBrightness.Count < 1) return;

            lock (listY_SheetBrightness)
            {
                int denominator = 20;// list_DataX.Count/100;
                int i = 0;
                List<float> Fx = listX_Distance.FindAll(f =>
                {
                    i++;
                    return (i % denominator == 0);
                }); // x축 1/20 압축

                if (Fx.Count < 1) return;

                i = 0;
                List<float> Fy = listY_SheetBrightness.FindAll(f =>
                {
                    i++;
                    return (i % denominator == 0);
                });

                chart_FullTime.ChartAreas[0].AxisY.Minimum = Fy.Min() - YInterval / 2;
                chart_FullTime.ChartAreas[0].AxisY.Maximum = Fy.Max() + YInterval / 2;

                chart_FullTime.ChartAreas[0].AxisX.Minimum = Fx.Min();
                chart_FullTime.ChartAreas[0].AxisX.Maximum = Fx.Min() + XInterval;
                chart_FullTime.Series["graphdata"].Points.DataBindXY(Fx, Fy);
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
            throw new NotImplementedException();
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
            //label_Model.Text = item;
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

            timer1.Start(); //Revision Graph
            timer2.Start(); //Revision Graph

            //initGraph();
        }

        void StopRevisionGraph()
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                timer1.Stop();//Revision Graph
                timer2.Stop();//Revision Graph
            });
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
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
    }
}
