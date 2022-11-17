using System;
using System.Collections.Generic;
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
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ModelPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();

            this.DataContext = InfoBox.Instance;
            this.InspectOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.InspectOperator;
            this.ExtractOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.StoringOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner.StoringOperator;
        }
        
        private void LotCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (SystemManager.Instance().CurrentModel == null)
            //{
            //    e.Handled = true;
            //    ((CheckBox)e.Source).IsChecked = false;
            //    return;
            //}

            //LotNoTextBox.Background = (Brush)Application.Current.Resources["LightBrush"];
            //LotNoTextBox.IsEnabled = true;
        }

        private void LotCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (SystemManager.Instance().CurrentModel == null)
            //{
            //    e.Handled = true;
            //    CustomMessageBox.Show("Need select model.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            //    return;
            //}

            
            
            //if (LotNoTextBox.Text.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter))
            //{
            //    e.Handled = true;
            //    ((CheckBox)sender).IsChecked = true;
            //    CustomMessageBox.Show("Lot no. need write english and number.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            //    return;
            //}

            //LotNoTextBox.Background = Brushes.White;
            //LotNoTextBox.IsEnabled = false;

            //if (string.IsNullOrEmpty(LotNoTextBox.Text))
            //{
            //    SystemManager.Instance().ProductionManager.LotChange(null, null);
            //    return;
            //}

            //SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, LotNoTextBox.Text);
        }
        
        public List<PatternGroup> GetSelectedPattern()
        {
            List<PatternGroup> pgList = new List<PatternGroup>();
            foreach (object obj in InspectPatternListView.SelectedItems)
            {
                if (obj is PatternGroup)
                    pgList.Add((PatternGroup)obj);
            }

            return pgList;
        }
    }
}
