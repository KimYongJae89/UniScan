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
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// ProgressBarWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class SimpleProgressWindow : Window, INotifyPropertyChanged
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

        public Visibility ButtonVisibility
        {
            get => cancellationTokenSource != null ? Visibility.Visible : Visibility.Hidden;
        }

        Task task;
        CancellationTokenSource cancellationTokenSource;
        DispatcherTimer timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public SimpleProgressWindow(string message, string title = "")
        {
            InitializeComponent();
            this.DataContext = this;
            if (string.IsNullOrEmpty(title))
                title = UniEye.Base.Settings.CustomizeSettings.Instance().ProgramTitle;

            this.TitleText = title;
            this.Message = message;

            //this.Topmost = true;
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
            if (task.Status == TaskStatus.Created)
                task.Start();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Tick;
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            rotation.Angle += 15;
            
            if (DynMvp.Base.ErrorManager.Instance().IsAlarmed())
            {
                cancellationTokenSource?.Cancel();
                timer?.Stop();
                this.Close();
            }

            if (task.IsCompleted || task.Status == TaskStatus.RanToCompletion)
            {
                timer?.Stop();
                this.Close();
            }
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();

            task?.Wait();

            timer?.Stop();
            Close();
        }

        public void Show(Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            this.cancellationTokenSource = cancellationTokenSource;

            if (cancellationTokenSource != null)
                task = new Task(action, cancellationTokenSource.Token);
            else
                task = new Task(action);
            
            base.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            
            Close();
        }
    }
}
