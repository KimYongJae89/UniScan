using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;
using UniEye.Base.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.UI;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.Color.UI
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReportPage : Page
    {
        BackgroundWorker worker;
        SetArgumentDelegate SetArgumentDelegate;

        WPFCanvasPanel canvasPanel = new WPFCanvasPanel(false);

        public BackgroundWorker Worker { get => worker; }

        public ReportPage(SetArgumentDelegate SetArgumentDelegate)
        {
            InitializeComponent();
            this.SetArgumentDelegate = SetArgumentDelegate;

            InspectResultList.Items.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));
            worker = new BackgroundWorker();

            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            ImageFrame.Navigate(canvasPanel);
        }

        private void Page_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext == null)
            {
                InspectResultList.ItemsSource = null;
                return;
            }

            if (SetArgumentDelegate != null)
                SetArgumentDelegate("Color", worker, this.DataContext);
        }
        
        private void InspectResultList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canvasPanel.TargetResult = InspectResultList.SelectedItem;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DefectList.SelectedItem == null)
                return;

            Defect defect = (Defect)DefectList.SelectedItem;

            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.White);
            rect.StrokeThickness = 10;

            System.Drawing.Rectangle rectangle = defect.Rectangle;

            rectangle.Inflate(25, 25);

            rect.Width = rectangle.Width;
            rect.Height = rectangle.Height;
            Canvas.SetLeft(rect, rectangle.X);
            Canvas.SetTop(rect, rectangle.Y);


            List<UIElement> figureList = new List<UIElement>();
            figureList.Add(rect);

            canvasPanel.Overlay(figureList);
        }

        protected void DoWork(object sender, DoWorkEventArgs e)
        {
            Production production = e.Argument as Production;
            string resultPath = System.IO.Path.Combine(SystemManager.Instance().ProductionManager.DefaultPath, production.StartTime.ToString("yyyyMMdd"), production.LotNo, "Color");

            if (Directory.Exists(resultPath) == false)
                return;

            List<InspectResult> inspectResultList = new List<InspectResult>();

            BackgroundWorker worker = (BackgroundWorker)sender;

            DirectoryInfo resultDirectory = new DirectoryInfo(resultPath);

            DirectoryInfo[] directorys = resultDirectory.GetDirectories();

            int reportIndex = directorys.Length / 100;

            foreach (DirectoryInfo directory in directorys)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                InspectResult inspectResult = new InspectResult(DetectorType.Color);
                inspectResult.ResultPath = directory.FullName;
                inspectResult.ImportResult();

                inspectResultList.Add(inspectResult);

                if (reportIndex == 0)
                    continue;

                if (inspectResultList.Count % reportIndex == 0)
                {
                    worker.ReportProgress((int)(100.0f * inspectResultList.Count / directorys.Length), string.Format("{0} / {1}", inspectResultList.Count, directorys.Length));
                    System.Threading.Thread.Sleep(100);
                }
            }

            e.Result = inspectResultList;
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
                return;

            InspectResultList.ItemsSource = (IEnumerable<InspectResult>)e.Result;
        }
    }
}
