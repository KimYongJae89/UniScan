using DynMvp.Data;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.UI
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : Page, IInspectedListner, INotifyPropertyChanged, IOpStateListener
    {
        string totalNum;
        string ngNum;
        string skipNum;
        string ratio;

        ObservableCollection<Defect> defectList = new ObservableCollection<Defect>();
        WPFCanvasPanel imagePanel;

        public ObservableCollection<Defect> DefectList { get => defectList; }
        public string TotalNum
        {
            get => totalNum;
            set
            {
                totalNum = value;
                OnPropertyChanged("TotalNum");
            }
        }

        public string SkipNum
        {
            get => skipNum;
            set
            {
                skipNum = value;
                OnPropertyChanged("SkipNum");
            }
        }

        public string NGNum
        {
            get => ngNum;
            set
            {
                ngNum = value;
                OnPropertyChanged("NGNum");
            }
        }

        public string Ratio
        {
            get => ratio;
            set
            {
                ratio = value;
                OnPropertyChanged("Ratio");
            }
        }

        public InspectPage()
        {
            InitializeComponent();

            this.DataContext = this;

            imagePanel = new WPFCanvasPanel();

            ImageFrame.Navigate(imagePanel);

            ResultCombiner.Instance().AddInspectedIListner(this);
            SystemState.Instance().AddOpListener(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Inspected(InspectResult inspectResult)
        {
            imagePanel.TargetResult = inspectResult;

            Production production = SystemManager.Instance().ProductionManager.CurProduction.PinHoleProduction;

            TotalNum = production.Total.ToString();
            NGNum = production.Ng.ToString();
            SkipNum = production.Pass.ToString();
            Ratio = string.Format("{0:0.00}%", ((double)production.Ng  / (double)production.Total) * 100.0);
            
            PinHoleDetectorResult detectorResult = (PinHoleDetectorResult)inspectResult.DetectorResult;
            detectorResult.DefectList.ForEach(result => result.SectionIndex = inspectResult.Index);

            while (defectList.Count + detectorResult.DefectList.Count > 100 && defectList.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    defectList.RemoveAt(defectList.Count - 1);
                });
            }

            detectorResult.DefectList.ForEach(defect =>
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    defectList.Insert(0, defect);
                }))
            );
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (curOpState == OpState.Wait)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    defectList.Clear();
                }));
            }
        }
    }
}
