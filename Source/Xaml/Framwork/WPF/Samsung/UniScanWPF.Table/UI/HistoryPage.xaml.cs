using DynMvp.Base;
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

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// HistoryPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ErrorItem> ItemCollection
        {
            get => itemCollection;
            set
            {
                itemCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs("itemCollection"));
            }
        }
        ObservableCollection<ErrorItem> itemCollection;

        public HistoryPage()
        {
            this.DataContext = this;
            InitializeComponent();

            ErrorManager.Instance().CollectionChanged += HistoryPage_CollectionChanged;
        }

        private void HistoryPage_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemList"));
            ItemCollection = new ObservableCollection<ErrorItem>(ErrorManager.Instance().ErrorItemList);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorManager.Instance().ResetAllAlarm();
        }
    }
}
