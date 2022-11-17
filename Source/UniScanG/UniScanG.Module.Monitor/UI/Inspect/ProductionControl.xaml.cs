using System;
using System.Collections.Generic;
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
using UniScanG.Module.Monitor.Data;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.UI.Inspect
{
    /// <summary>
    /// Production.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductionControl : UserControl, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int? Total => production?.Total;
        public int? Done => production?.Done;
        public int? Good => production?.Good;
        public int? Pass => production?.Pass;
        public int? Ng => production?.Ng;
        public int? NgMargin => production?.NgMargin;
        public int? NgBlot => production?.NgBlot;
        public int? NgPinhole => production?.NgPinhole;

        Production production;

        public ProductionControl()
        {
            InitializeComponent();
            this.DataContext = this;

            LocalizeHelper.AddListener(this);

            SystemManager.Instance().ProductionManager.OnLotChanged += OnLotChanged;
            SystemManager.Instance().InspectRunner.OnProductInspected += OnProductInspected;
        }


        private void OnLotChanged()
        {
            this.production = (Production)SystemManager.Instance().ProductionManager.CurProduction;
            UpdateData();
        }

        private void OnProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            UpdateData();
            //Dispatcher.Invoke(new Action(() =>
            //{
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Total"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Good"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pass"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ng"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgMargin"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgBlot"));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgPinhole"));
            //}));


        }

        private void UpdateData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Done"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Good"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pass"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ng"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgMargin"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgBlot"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NgPinhole"));
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }
    }
}
