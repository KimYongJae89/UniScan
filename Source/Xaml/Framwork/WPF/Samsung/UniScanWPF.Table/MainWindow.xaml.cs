using DynMvp.Authentication;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.UI;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, IUserHandlerListener,IMultiLanguageSupport
    {
        public delegate void OnNavigatedDelegate(PageType pageType);
        public OnNavigatedDelegate OnNavigated = null;
        //MainPage mainPage;
        InspectPage inspectPage;
        TeachPage teachPage;
        InfoPage infoPage;
        ModelPage modelPage;
        SettingPage settingPage;

        ReportPage reportPage;
        HistoryPage historyPage;

        PageType curPageType = PageType.NULL;

        public MainWindow()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().InitTableUnit();

            SystemManager.Instance().MainPage = this;
            //mainPage = new MainPage();
            //mainPage.OnNavigated = mainPage_OnNavigated;

            this.inspectPage = new InspectPage();
            this.teachPage = new TeachPage();
            this.infoPage = new InfoPage();
            this.modelPage = new ModelPage();
            this.settingPage = new SettingPage();
            this.reportPage = new ReportPage();
            this.historyPage = new HistoryPage();

            mainFrame.Navigated += MainFrame_Navigated;
            Navigate(PageType.Model);

            statusStripGrid.Children.Add(new StatusStrip());
            MenuPanel.DataContext = InfoBox.Instance;

            programTitle.DataContext = CustomizeSettings.Instance();

            AlarmWindow alarmWindow = new AlarmWindow();

            UserHandler.Instance().AddListener(this);
            UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op", "op");
        }
        
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show(LocalizeHelper.GetString("Are you sure you want to exit the program ?"), null, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            new SimpleProgressWindow(LocalizeHelper.GetString("Closing..")).Show(() => SystemManager.Instance().Close());
            SystemManager.Instance().Release();
            App.Current.MainWindow.Close();
            App.Current.Shutdown();
        }

        private void UserLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            if (logInWindow.ShowDialog() == true)
                UserHandler.Instance().CurrentUser = logInWindow.User;
        }

        public void UserChanged()
        {

        }

        private void InspectRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(mainPage);
            //mainPage.Navigate(PageType.Inspect);
            Navigate(PageType.Inspect);
        }

        private void TeachRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(mainPage);
            //mainPage.Navigate(PageType.Teach);
            Navigate(PageType.Teach);
        }

        private void ModelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(mainPage);
            //mainPage.Navigate(PageType.Model);
            Navigate(PageType.Model);
        }

        private void ReportRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(this.reportPage);
            Navigate(PageType.Report);
        }

        private void HistoryRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(this.reportPage);
            Navigate(PageType.History);
        }

        private void SettingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //mainFrame.Navigate(mainPage);
            //mainPage.Navigate(PageType.Setting);
            Navigate(PageType.Setting);

            //if (SystemManager.Instance().MachineObserver.IoBox.InOnDoor1 == false || SystemManager.Instance().MachineObserver.IoBox.InOnDoor2 == false)
            //{
            //    CustomMessageBox.Show("Door is open !!", "Cancle", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
            //    return;
            //}

            //MachineOperator.MoveHome(0, null, new CancellationTokenSource());
        }

        private void mainPage_OnNavigated(PageType oldPageType, PageType newPageType)
        {
            switch (newPageType)
            {
                case PageType.Inspect:
                    InspectRadioButton.IsChecked = true;
                    break;
                case PageType.Model:
                    ModelRadioButton.IsChecked = true;
                    break;
                case PageType.Teach:
                    TeachRadioButton.IsChecked = true;
                    break;
                case PageType.Setting:
                    SettingRadioButton.IsChecked = true;
                    break;
                case PageType.Report:
                    ReportRadioButton.IsChecked = true;
                    break;
            }

            if (oldPageType == PageType.Teach)
            {
                Model model = SystemManager.Instance().CurrentModel;
                bool saveOk = true;
                if (model != null && model.Modified)
                    new SimpleProgressWindow(LocalizeHelper.GetString("Save")).Show(() => saveOk = SystemManager.Instance().ModelManager.SaveModel(model));
                if(saveOk)
                    model.Modified = false;
                else
                    CustomMessageBox.Show("Model Save Fail", MessageBoxButton.OK);
            }
        }

        public void Navigate(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Inspect:
                    mainFrame.Navigate(this.inspectPage);
                    break;
                case PageType.Model:
                    mainFrame.Navigate(this.modelPage);
                    break;
                case PageType.Teach:
                    mainFrame.Navigate(this.teachPage);
                    break;
                case PageType.Setting:
                    mainFrame.Navigate(this.settingPage);
                    break;
                case PageType.Report:
                    mainFrame.Navigate(this.reportPage);
                    break;
                case PageType.History:
                    mainFrame.Navigate(this.historyPage);
                    break;
            }
            mainPage_OnNavigated(this.curPageType , pageType);
            this.curPageType = pageType;
        }
    }
}