using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Module.Controller.Inspect;
using UniEye.Base.Inspect;
using DynMvp.InspData;
using UniScanG.UI;
using UniScanG.Gravure.Settings;
using UniScanG.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanG.Module.Controller.UI.Inspect
{
    public partial class ExtraImagePanel : UserControl, IInspectExtraPanel
    {
        AlarmManager alarmManager = (AlarmManager)SystemManager.Instance().InspectRunnerG.AlarmManager;
        InspectionResult inspectionResult;

        public ExtraImagePanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Bottom;
            //((UnitBaseInspectRunner)SystemManager.Instance().InspectRunnerG).AddInspectDoneDelegate(InspectDone);
        }

        public void UpdateResult(InspectionResult inspectionResult)
        {
            this.inspectionResult = inspectionResult;
            UpdateData();
        }

        private void Reset()
        {
            if (this.chart1.InvokeRequired)
            {
                this.chart1.Invoke(new MethodInvoker(Reset));
                return;
            }
            this.chart1.Series[0].Points.Clear();
        }

        private void UpdateData()
        {
            if (this.chart1.InvokeRequired)
            {
                this.chart1.Invoke(new MethodInvoker(UpdateData));
                return;
            }

            this.chart1.Series[0].Points.Clear();

            if (this.inspectionResult == null)
                return;
            
                MergeSheetResult mergeSheetResult = this.inspectionResult.AlgorithmResultLDic[UniScanG.Data.SheetCombiner.TypeName] as MergeSheetResult;
            if (mergeSheetResult == null)
                return;

            PointF[] points = mergeSheetResult.DefectDensity.Select((f, i) => new PointF(i, f)).ToArray();
            this.chart1.Series[0].Points.DataBind(points, "X", "Y", "");
            this.chart1.ChartAreas[0].AxisX.Minimum = -0.5;
            this.chart1.ChartAreas[0].AxisX.Maximum = points.Length - 0.5;
            this.chart1.ChartAreas[0].AxisX.Crossing = -0.5;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 0.5;
            this.chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
            this.chart1.ChartAreas[0].AxisX.MajorTickMark.IntervalOffset = 1;
            this.chart1.ChartAreas[0].AxisX.IsLabelAutoFit = false;

            //this.chart1.ChartAreas[0].AxisX.CustomLabels.Clear();
            //for (int i = 0; i < this.chart1.Series[0].Points.Count; i++)
            //{
            //    DataPoint dp = this.chart1.Series[0].Points[i];
            //    this.chart1.ChartAreas[0].AxisX.CustomLabels.Add((0.5d + i) * 1, (1.5d + i) * 1, dp.XValue.ToString());
            //}

            //this.chart1.ChartAreas[0].AxisY.Maximum = 0.0005;
        }

        public void UpdateVisible()
        {
            if (this.chart1.InvokeRequired)
            {
                this.chart1.Invoke(new MethodInvoker(UpdateVisible));
                return;
            }

            this.Visible = AdditionalSettings.Instance().StripeBlotAlarmSetting.Use;

            double yMax = Math.Min(100, AdditionalSettings.Instance().StripeBlotAlarmSetting.Percent * 1.2);
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = yMax;
            this.chart1.ChartAreas[0].AxisY.Interval = yMax / 6; ;
            this.chart1.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            this.chart1.ChartAreas[0].AxisY.StripLines.Clear();
            StripLine stripLine = new StripLine();
            stripLine.BackColor = Color.Red;
            stripLine.IntervalOffset = AdditionalSettings.Instance().StripeBlotAlarmSetting.Percent;
            stripLine.Interval = 0;
            stripLine.StripWidth = yMax / 100;
            this.chart1.ChartAreas[0].AxisY.StripLines.Add(stripLine);
        }

        private void ExtraImagePanel_Load(object sender, EventArgs e)
        {
            this.Height = 200;
        }
    }
}
