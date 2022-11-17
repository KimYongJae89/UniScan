using DynMvp.Base;
using DynMvp.Data;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using UniEye.Base.Settings;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Inspect;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.UI.Report
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReportPage : Page, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public event PropertyChangedEventHandler PropertyChanged;

        enum SeriesOrder { MarginW, MarginL, BlotW, BlotL, DefectC };

        public Func<double, string> YFormatter => this.yFormatter;
        Func<double, string> yFormatter = new Func<double, string>(f => f.ToString("F2"));

        public string[] Labels => this.labels;
        string[] labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May" };

        public SeriesCollection SeriesCollection => seriesCollection;
        SeriesCollection seriesCollection;

        public int XStep => this.xStep;
        int xStep=1;

        public int XMin => this.xMin;
        int xMin = 1;

        public int XMax => this.xMax;
        int xMax = 1;

        public InspectionResult InspectionResult => this.inspectionResult;
        InspectionResult inspectionResult = null;

        public ReportPage()
        {
            InitializeComponent();

            var mappers = Mappers.Xy<PointF>();
            mappers.X(f => f.X);
            mappers.Y(f => f.Y);

            this.seriesCollection = new SeriesCollection(mappers);
            this.seriesCollection.Add(new LineSeries() { Title = "Margin W", Values = new ChartValues<PointF>() });
            this.seriesCollection.Add(new LineSeries() { Title = "Margin L", Values = new ChartValues<PointF>() });
            this.seriesCollection.Add(new LineSeries() { Title = "Blot W", Values = new ChartValues<PointF>() });
            this.seriesCollection.Add(new LineSeries() { Title = "Blot L", Values = new ChartValues<PointF>() });
            this.seriesCollection.Add(new LineSeries() { Title = "Defect C", Values = new ChartValues<PointF>(), });

            Random random = new Random();
            foreach (Series series in  this.seriesCollection)
            {
                for (int i = 0; i < 50; i++)
                {
                    series.Values.Add(new PointF(i * 2, (float)random.NextDouble() * 10));
                }
            }
            this.xMin = 0;
            this.xMax = 100;
            this.xStep = 10;

            this.chart.DataContext = this;
            this.DataContext = this;

            LocalizeHelper.AddListener(this);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.dateTo.SelectedDate = DateTime.Today;
            this.dateFrom.SelectedDate = DateTime.Today.AddDays(-1);

            this.filterName.Text = "";
            this.filterLot.Text = "";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateFrom = this.dateFrom.SelectedDate == null ? DateTime.MinValue : this.dateFrom.SelectedDate.Value;
            DateTime dateTo = this.dateTo.SelectedDate == null ? DateTime.MaxValue : this.dateTo.SelectedDate.Value;

            List<ProductionBase> productionBaseList = SystemManager.Instance().ProductionManager.FindAll(this.filterName.Text, this.filterLot.Text, dateFrom, dateTo);
            productionList.ItemsSource = productionBaseList;

        }

        private void ProductionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Production production = this.productionList.SelectedItem as Production;
            if (production == null)
                return;

            string resultPath = production.GetResultPath();
            DataArchiver dataArchiver = new DataArchiver();
            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            for(int i=0; i<production.Total; i++)
            {
                string path = System.IO.Path.Combine(resultPath, i.ToString());
                InspectionResult inspectionResult = dataArchiver.Load(path) as InspectionResult;
                if (inspectionResult != null)
                {
                    inspectionResult.ResultPath = production.GetResultPath();
                    inspectionResultList.Add(inspectionResult);
                }
            }
            inspectionList.ItemsSource = inspectionResultList;
            if (inspectionResultList.Count > 0)
            {
                int xMin = (int)Math.Floor(inspectionResultList.Min(f => f.RollPos));
                int xMax = (int)Math.Ceiling(inspectionResultList.Max(f => f.RollPos));
                int xLen = xMax - xMin;
                if (xLen % 2 == 1)
                    xLen++;

                int xStep = xLen / 10;
                if (xStep < 1)
                    xStep = 1;


                this.xMin = xMin;
                this.xMax = xMin + xLen;
                this.xStep = xStep;
            }

            foreach (Series series in this.seriesCollection)
                series.Values.Clear();

            inspectionResultList.ForEach(f =>
            {
                this.seriesCollection[(int)SeriesOrder.MarginW].Values.Add(new PointF(f.RollPos, f.MarginSize.Width));
                this.seriesCollection[(int)SeriesOrder.MarginL].Values.Add(new PointF(f.RollPos, f.MarginSize.Height));
            });

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XMin"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XMax"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XStep"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesCollection"));
            
        }

        private void InspectionList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            InspectionResult inspectionResult = this.inspectionList.SelectedItem as InspectionResult;
            if (inspectionResult == null)
                return;

            inspectionResult.GrabImageList.Clear();
            string imagePath = System.IO.Path.Combine(inspectionResult.ResultPath, inspectionResult.InspectionNo, "GrabImage.jpg");
            if (System.IO.File.Exists(imagePath))
                inspectionResult.GrabImageList.Add(new Image2D(imagePath));

            this.inspectionResult = inspectionResult;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InspectionResult"));
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }
    }
}
