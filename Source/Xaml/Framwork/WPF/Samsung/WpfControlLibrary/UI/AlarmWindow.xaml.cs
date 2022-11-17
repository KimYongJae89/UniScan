using DynMvp.Base;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// AlarmWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmWindow : Window, INotifyPropertyChanged,IMultiLanguageSupport
    {
        ErrorItem curErrorItem;
        
        public ErrorItem CurErrorItem
        {
            set
            {
                curErrorItem = value;
                OnPropertyChanged("CurErrorItem");
            }
            get => curErrorItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AlarmWindow()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);

            errorInfoGrid.DataContext = this;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(AlarmTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
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

        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                if (this.Visibility != Visibility.Visible)
                {
                    CurErrorItem = ErrorManager.Instance().ErrorItemList.Find(item => item.IsCleared == false);

                    Show();
                    Focus();
                }
            }
            else
            {
                if (this.Visibility == Visibility.Visible)
                {
                    Hide();
                }
            }
        }
        
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorManager.Instance().ResetAllAlarm();
        }

        private void BuzzerButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorManager.Instance().BuzzerOn = false;
        }
    }
}
