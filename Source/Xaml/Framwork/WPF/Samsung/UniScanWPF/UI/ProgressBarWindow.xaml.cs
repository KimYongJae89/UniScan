using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace UniScanWPF.UI
{
    /// <summary>
    /// ProgressBarWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class ProgressBarWindow : Window, INotifyPropertyChanged
    {
        private string titleText;
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                OnPropertyChanged("TitleText");
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private int progressPos = 0;
        public int ProgressPos
        {
            get => progressPos;
            set
            {
                progressPos = value;
                OnPropertyChanged("ProgressPos");
            }
        }

        private object argument = null;

        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public BackgroundWorker BackgroundWorker
        {
            get { return backgroundWorker; }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ProgressBarWindow(string title, object argument)
        {
            InitializeComponent();
            this.DataContext = this;
            this.TitleText = title;
            this.argument = argument;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Message = e.UserState.ToString();
            this.ProgressPos = e.ProgressPercentage;
        }
        
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.RunWorkerAsync(argument);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            while (backgroundWorker.IsBusy)
                Thread.Sleep(100);

            Close();
        }
    }
}
