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
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public delegate void SetArgumentDelegate(string name, BackgroundWorker worker, object argument);

    public partial class ReportPage : Page
    {
        object highlightedItem;
        public object HighlightedItem
        {
            set
            {
                highlightedItem = value;
                if (highlightedItem != null)
                    SelectionChanged(highlightedItem);
            }
        }

        MultipleProgressBarWindow multipleProgressBarWindow;
        
        PinHole.UI.ReportPage pinHoleReportPage;
        Color.UI.ReportPage colorReportPage;
        public ReportPage()
        {
            InitializeComponent();
            this.DataContext = this;
            
            pinHoleReportPage = new PinHole.UI.ReportPage(SetArgument);
            colorReportPage = new Color.UI.ReportPage(SetArgument);

            PinHoleButton.Tag = pinHoleReportPage;
            ColorButton.Tag = colorReportPage;
            
            ReportFrame.Content = PinHoleButton.Tag;
            ProductionList.Items.SortDescriptions.Add(
                new SortDescription("LotNo", ListSortDirection.Descending));

        }

        private void SetArgument(string name, BackgroundWorker worker, object argument)
        {
            multipleProgressBarWindow.SetArgument(name, worker, argument);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchProduction();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            pinHoleReportPage.DataContext = colorReportPage.DataContext = null;

            SearchProduction();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                pinHoleReportPage.DataContext = colorReportPage.DataContext = null;
                SearchProduction();
            }
        }

        private void SearchProduction()
        {
            if (ProductionList == null)
                return;

            ProductionList.ItemsSource = null;
            ProductionList.ItemsSource = SystemManager.Instance().ProductionManager.List.FindAll(production => production.StartTime >= StartDate.SelectedDate && production.StartTime <= EndDate.SelectedDate.Value.AddDays(1)); ;
        }

        private void SelectionChanged(object multipleProduction)
        {
            MultipleProduction mp = (MultipleProduction)multipleProduction;

            multipleProgressBarWindow = new MultipleProgressBarWindow("Load");

            pinHoleReportPage.DataContext = mp.PinHoleProduction;
            colorReportPage.DataContext = mp.ColorProduction;

            multipleProgressBarWindow.Topmost = true;
            multipleProgressBarWindow.ShowDialog();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportFrame.Content = ((Button)sender).Tag;
        }
    }
}
