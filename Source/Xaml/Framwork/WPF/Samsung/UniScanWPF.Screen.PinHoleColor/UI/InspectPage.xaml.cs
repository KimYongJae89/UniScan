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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniEye.Base.Data;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : Page, IOpStateListener, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool startEnable;
        public bool StartEnable
        {
            get { return startEnable; }
            set
            {
                startEnable = value;
                OnPropertyChanged("StartEnable");
            }
        }

        bool stopEnable;
        public bool StopEnable
        {
            get { return stopEnable; }
            set
            {
                stopEnable = value;
                OnPropertyChanged("StopEnable");
            }
        }

        bool resetEnable;
        public bool ResetEnable
        {
            get { return resetEnable; }
            set
            {
                resetEnable = value;
                OnPropertyChanged("ResetEnable");
            }
        }

        public InspectPage()
        {
            InitializeComponent();

            this.DataContext = this;

            PinHoleFrame.Navigate(new PinHole.UI.InspectPage());
            ColorFrame.Navigate(new Color.UI.InspectPage());

            SystemState.Instance().AddOpListener(this);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().InspectRunner.EnterWaitInspection();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show(StringManager.GetString("Are you sure you want to stop?"), "UniEye", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                SystemManager.Instance().InspectRunner.ExitWaitInspection();
        }
        
        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            StartButton.IsEnabled = curOpState == OpState.Idle ? true : false;
            StopButton.IsEnabled = curOpState != OpState.Idle ? true : false;
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
    }
}
