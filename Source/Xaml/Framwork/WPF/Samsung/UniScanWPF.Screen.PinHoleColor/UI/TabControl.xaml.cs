using DynMvp.Base;
using DynMvp.Data;
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
using UniEye.Base;
using UniEye.Base.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// MainTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TabControl : UserControl, IOpStateListener, INotifyPropertyChanged, IModelChangedListerner, IProductionListener
    {
        ModelWindow mw;
        SettingWindow sw;
        InspectPage inspectPage;
        ReportPage reportPage;
        OpState curOpState;
        Brush curOpStateBrush;
        Data.Model curModel;
        ProductionBase curProduction;

        public Brush CurOpStateBrush
        {
            get { return curOpStateBrush; }
            set
            {
                curOpStateBrush = value;
                OnPropertyChanged("CurOpStateBrush");
            }
        }
        public OpState CurOpState
        {
            get { return curOpState; }
            set
            {
                curOpState = value;
                OnPropertyChanged("CurOpState");
            }
        }

        public Data.Model CurModel
        {
            get { return curModel; }
            set
            {
                curModel = value;
                OnPropertyChanged("CurModel");
            }
        }

        public ProductionBase CurProduction
        {
            get { return curProduction; }
            set
            {
                curProduction = value;
                OnPropertyChanged("CurProduction");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TabControl()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            this.DataContext = this;

            mw = new ModelWindow(SystemManager.Instance().ModelManager);
            mw.Topmost = true;

            sw = new SettingWindow();
            sw.Topmost = true;

            inspectPage = new InspectPage();
            reportPage = new ReportPage();

            Frame.Navigate(inspectPage);
            InspectButton.Background = App.Current.Resources["MainBrush"] as Brush;
            InspectButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;

            SystemState.Instance().AddOpListener(this);
            SystemManager.Instance().AddModelChangedListner(this);
            SystemManager.Instance().ProductionManager.AddListener(this);
        }
        
        private void InspectButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(inspectPage);
            InspectButton.Background = App.Current.Resources["MainBrush"] as Brush;
            InspectButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;
            ReportButton.Background = Brushes.White;
            ReportButton.Foreground = App.Current.Resources["FontBrush"] as Brush;
        }

        private void ModelButton_Click(object sender, RoutedEventArgs e)
        {
            mw.Show();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(reportPage);
            InspectButton.Background = Brushes.White; 
            InspectButton.Foreground = App.Current.Resources["FontBrush"] as Brush; 
            ReportButton.Background = App.Current.Resources["MainBrush"] as Brush;
            ReportButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            if (logInWindow.ShowDialog() == true)
            {
                if (logInWindow.User.UserType != DynMvp.Authentication.UserType.Operator)
                {
                    sw.Show();
                }
                else
                {
                    CustomMessageBox.Show(StringManager.GetString("You don't have proper permission."), "UniEye", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show(StringManager.GetString("Message", "Do you want to exit program?"), "UniEye", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SystemState.Instance().OpState != OpState.Idle)
                    SystemManager.Instance().InspectRunner.ExitWaitInspection();

                SystemManager.Instance().Close();
            }
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            this.CurOpState = curOpState;
            switch (curOpState)
            {
                case OpState.Teach:
                    CurOpStateBrush = App.Current.Resources["LightYellowBrush"] as Brush;
                    break;
                case OpState.Wait:
                    
                    CurOpStateBrush = App.Current.Resources["LightGreenBrush"] as Brush;
                    break;
                case OpState.Inspect:
                    CurOpStateBrush = App.Current.Resources["LightGreenBrush"] as Brush;
                    break;
                default:
                    CurOpStateBrush = System.Windows.Media.Brushes.LightGray;
                    break;
            }
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

        public void ModelChanged()
        {
            CurModel = SystemManager.Instance().CurrentModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().ModelManager.WindowLoaded();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Tuple<MessageBoxResult, string> tuple = CustomInputForm.Show("What is lot no ?", "Lot No", MessageBoxImage.Question);
            if (tuple.Item1 == MessageBoxResult.OK && string.IsNullOrEmpty(tuple.Item2) == false && SystemManager.Instance().CurrentModel != null)
                SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, tuple.Item2);
        }

        public void ProductionChanged()
        {
            CurProduction = SystemManager.Instance().ProductionManager.CurProduction;
        }
    }
}