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

namespace UniScanWPF.UI
{
    /// <summary>
    /// SplashWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashWindow : Window, IReportProgress, INotifyPropertyChanged
    {
        public delegate bool SplashActionDelegate(SplashWindow window);
        
        private string programTitle;
        private string version;
        private string buildDate;
        private string copyright;
        private ImageSource productLogo;
        private ImageSource companyLogo;
        string message;
        int progressPos;

        public string Version { get => version; set => version = value; }
        public string BuildDate { get => buildDate; set => buildDate = value; }
        public string ProgramTitle { get => programTitle; set => programTitle = value; }
        public string Copyright { get => copyright; set => copyright = value; }
        public ImageSource ProductLogo { get => productLogo; set => productLogo = value; }
        public ImageSource CompanyLogo { get => companyLogo; set => companyLogo = value; }
        public string Message
        {
            get => message;
            set
            {
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
            
            programTitle = CustomizeSettings.Instance().ProgramTitle;
            version = string.Format("Version {0}.{1}", VersionHelper.Instance().MajorVersion, VersionHelper.Instance().MinorVersion);
            buildDate = string.Format("Build {0}", VersionHelper.Instance().BuildDateTime.ToString("yyMMdd.HHmm"));
            copyright = CustomizeSettings.Instance().Copyright;
            if (string.IsNullOrEmpty(PathSettings.Instance().ProductLogo) == false)
                productLogo = new BitmapImage(new Uri(PathSettings.Instance().ProductLogo));

            if (string.IsNullOrEmpty(PathSettings.Instance().CompanyLogo) == false)
                companyLogo = new BitmapImage(new Uri(PathSettings.Instance().CompanyLogo));

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(splashActionTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            timer.Start();
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

        private void SpalashProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start SpalashProc.");

            if (SetupAction != null && SetupAction(this) == false)
                CustomMessageBox.Show("Some error is occurred. Please, check the configuration.\n\n" + lastError, "UniEye", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    Close();
            }
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (workingThread != null)
                return;
            
            if (Keyboard.IsKeyDown(Key.F12) && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
            {
                if (ConfigAction != null && !doConfigAction)
                {
                    doConfigAction = true;
                    message = StringManager.GetString("Wait Configuration");

                    if (ConfigAction(this) == false)
                    {
                        Close();
                    }

                    doConfigAction = false;
                }
            }
        }
    }
}
