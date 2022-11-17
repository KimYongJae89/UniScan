using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using UniEye.Base.Settings;
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// SplashWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashWindow : Window, IReportProgress, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public delegate bool SplashActionDelegate(SplashWindow window);
        
        private string title;
        private string version;
        private string buildDate;
        private string copyright;
        private ImageSource productLogo;
        private ImageSource companyLogo;
        string message;
        int progressPos;

        public string Version { get => version; set => version = value; }
        public string BuildDate { get => buildDate; set => buildDate = value; }
        public new string SplashTitle { get => title; set => title = value; }
        public string Copyright { get => copyright; set => copyright = value; }
        public ImageSource ProductLogo { get => productLogo; set => productLogo = value; }
        public ImageSource CompanyLogo { get => companyLogo; set => companyLogo = value; }
        public string Message
        {
            get => message;
            set
            {
                //message = LocalizeHelper.GetString(this.GetType().FullName, value);
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public int ProgressPos
        {
            get => progressPos;
            set
            {
                progressPos = value;
                OnPropertyChanged("ProgressPos");
            }
        }

        string lastError;
        bool doConfigAction = false;
        Thread workingThread = null;
        DispatcherTimer timer;
        
        public SplashActionDelegate ConfigAction = null;
        public SplashActionDelegate SetupAlgorithmStrategyAction = null;
        public SplashActionDelegate SetupAction = null;
        public SplashActionDelegate PostSetupAction = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public SplashWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
            title = CustomizeSettings.Instance().Title;
            version = string.Format("{0} {1}", LocalizeHelper.GetString(this.GetType().FullName,"Version"),VersionHelper.Instance().VersionString);
            buildDate = string.Format("{0} {1}", LocalizeHelper.GetString(this.GetType().FullName, "Build"), VersionHelper.Instance().BuildString);
            copyright = CustomizeSettings.Instance().Copyright;
            if (string.IsNullOrEmpty(PathSettings.Instance().ProductLogo) == false)
                productLogo = new BitmapImage(new Uri(PathSettings.Instance().ProductLogo));

            if (string.IsNullOrEmpty(PathSettings.Instance().CompanyLogo) == false)
                companyLogo = new BitmapImage(new Uri(PathSettings.Instance().CompanyLogo));

            LocalizeHelper.AddListener(this);
        }
        
        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        public void ReportProgress(int progressPos, string progressMessage)
        {
            this.ProgressPos = progressPos;
            this.Message = progressMessage;

            Thread.Sleep(100);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        bool setupSuccess = false;
        private void SpalashProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start SpalashProc.");

            setupSuccess = SetupAction(this);
            if (setupSuccess == false)
                //CustomMessageBox.Show("Some error is occurred. Please, check the configuration.\n\n" + lastError, "UniEye", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(string.Format("Some error is occurred. Please, check the configuration.{0}{1}", Environment.NewLine, lastError));
        }

        private void splashActionTimer_Tick(object sender, EventArgs e)
        {
            if (workingThread == null)
            {
                if (doConfigAction == false)
                {
                    doConfigAction = true;
                    LogHelper.Debug(LoggerType.StartUp, "Start Spalash Thread.");

                    message = StringManager.GetString("Start Setup...");

                    if (SetupAlgorithmStrategyAction != null)
                        SetupAlgorithmStrategyAction(this);

                    workingThread = new Thread(new ThreadStart(SpalashProc));
                    workingThread.IsBackground = true;
                    workingThread.Start();
                }
            }
            else
            {
                if (workingThread.IsAlive == false)
                {
                    timer.Stop();
                    this.DialogResult = this.setupSuccess;
                    this.Close();
                }
            }
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (workingThread != null)
                return;
            
            if (Keyboard.IsKeyDown(Key.F12))
            {
                if (ConfigAction != null && !doConfigAction)
                {
                    doConfigAction = true;
                    message = StringManager.GetString("Wait Configuration");

                    if (ConfigAction(this) == false)
                    {
                        this.DialogResult = false;
                        Close();
                    }

                    doConfigAction = false;
                }
            }
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(splashActionTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            timer.Start();
        }
    }
}
