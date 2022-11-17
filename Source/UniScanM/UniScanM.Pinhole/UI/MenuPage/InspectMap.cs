using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.DataVisualization;
using UniEye.Base;
using UniScanM.Pinhole.Data;

namespace UniScanM.Pinhole.UI.MenuPage
{
    public partial class InspectMap : UserControl
    {
        BubbleSeries pinholeSeries;
        BubbleSeries dustSeries;

        List<SeriesDefectInfo> pinholeList = new List<SeriesDefectInfo>();

        Data.InspectionResult rollInspectResult;
        public Data.InspectionResult RollInspectResult { get => rollInspectResult; set => rollInspectResult = value; }

        Data.InspectionResult mapInspResult;

        bool zoom = true;
        public bool Zoom { get => zoom; set => zoom = value; }

        public InspectMap()
        {
            InitializeComponent();
            InitSeries();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            mapInspResult = new InspectionResult();
        }

        protected override void OnNotifyMessage(Message m)
        {
            if(m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void NewInitSeries()
        {
            mapInspResult.Clear();
            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
            //Data.ModelDescription desc = (Data.ModelDescription)model.ModelDescription;
            NumericXAxis numericXAxis = new NumericXAxis();
            numericXAxis.MaximumValue = 500;
            numericXAxis.MinimumValue = 0;
            numericXAxis.LabelLocation = AxisLabelsLocation.OutsideTop;

            NumericYAxis numericYAxis = new NumericYAxis();
            //numericYAxis.MaximumValue = desc.SheetLength;
            numericYAxis.MinimumValue = 0;
            numericYAxis.IsInverted = true;
            pinholeSeries = new BubbleSeries
            {
                IsHighlightingEnabled = true,
                XMemberPath = "X",
                YMemberPath = "Y",
                RadiusMemberPath = "Area",
                LabelMemberPath = "Label",
                MarkerType = MarkerType.Circle,
                XAxis = numericXAxis,
                YAxis = numericYAxis,
                MarkerBrush = new SolidBrush(Color.Green),
            };

            dustSeries = new BubbleSeries
            {
                IsHighlightingEnabled = true,
                XMemberPath = "X",
                YMemberPath = "Y",
                RadiusMemberPath = "Area",
                LabelMemberPath = "Label",
                MarkerType = MarkerType.Circle,
                XAxis = numericXAxis,
                YAxis = numericYAxis,
                MarkerBrush = new SolidBrush(Color.Blue),
            };
            //chartInspMap.HorizontalZoomable = true;
            chartInspMap.VerticalZoomable = true;

            chartInspMap.Axes.Add(numericXAxis);
            chartInspMap.Axes.Add(numericYAxis);

        }

        public void InitSeries()
        {
            

            NumericXAxis numericXAxis = new NumericXAxis();
            numericXAxis.MaximumValue = 500;
            numericXAxis.MinimumValue = 0;
            numericXAxis.LabelLocation = AxisLabelsLocation.OutsideTop;

            NumericYAxis numericYAxis = new NumericYAxis();
            numericYAxis.MaximumValue = 5000;
            numericYAxis.MinimumValue = 0;
            numericYAxis.IsInverted = true;
            pinholeSeries = new BubbleSeries
            {
                IsHighlightingEnabled = true,
                XMemberPath = "X",
                YMemberPath = "Y",
                RadiusMemberPath = "Area",
                LabelMemberPath = "Label",
                MarkerType = MarkerType.Circle,
                XAxis = numericXAxis,
                YAxis = numericYAxis,
                MarkerBrush = new SolidBrush(Color.Green),
            };

            dustSeries = new BubbleSeries
            {
                IsHighlightingEnabled = true,
                XMemberPath = "X",
                YMemberPath = "Y",
                RadiusMemberPath = "Area",
                LabelMemberPath = "Label",
                MarkerType = MarkerType.Circle,
                XAxis = numericXAxis,
                YAxis = numericYAxis,                
                MarkerBrush = new SolidBrush(Color.Blue),
            };
            //chartInspMap.HorizontalZoomable = true;
            chartInspMap.VerticalZoomable = true;

            chartInspMap.Axes.Add(numericXAxis);
            chartInspMap.Axes.Add(numericYAxis);            

            //ultraDataChart.Series.Add(series1);
        }
        public void AddResult(Data.InspectionResult inspectResult)
        {
            pinholeList.Clear();
            mapInspResult.AddDefectInfo(inspectResult);
            Parallel.For(0, mapInspResult.NumDefect, (i) =>
            {
                DefectInfo defectInfo = mapInspResult[i];
                if (defectInfo != null)
                {
                    int x = (int)defectInfo.RealPosition.X;
                    int y = (int)defectInfo.RealPosition.Y;
                    int area = 10;
                    string label = defectInfo.GetImageFileName();
                    SeriesDefectInfo seriesDefcetInfo = new SeriesDefectInfo(x, (int)(y / 100), area, label);
                    lock(pinholeList)
                        pinholeList.Add(seriesDefcetInfo);
                }
            });
            UpdateDataSource();
        }
        delegate void UpdateDataSourceDelegate();
        void UpdateDataSource()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataSourceDelegate(UpdateDataSource));
                return;
            }
            lock(pinholeList)
            {
                try
                {
                    pinholeSeries.DataSource = pinholeList;
                    chartInspMap.Series.Add(pinholeSeries);                    
                }
                catch(Exception e)
                {
                    //업데이트 실패

                }
                
            }
            
        }

        private void chartInspMap_SeriesMouseLeftButtonDown(object sender, ChartMouseButtonEventArgs e)
        {            
            SeriesDefectInfo info = (SeriesDefectInfo)e.Item;
        }
    }

    public class SeriesDefectInfo
    {
        private int x;
        private int y;
        private int area;
        private string label;

        public SeriesDefectInfo(int x, int y, int area, string label)
        {
            this.X = x;
            this.Y = y;
            this.Area = area;
            this.Label = label;
        }

        public int Area { get => area; set => area = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public string Label { get => label; set => label = value; }
    }
}
