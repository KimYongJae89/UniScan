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
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    public class WorkerItem : INotifyPropertyChanged, IMultiLanguageSupport
    {
        string name;
        string text;
        int progressPos;
        object argument;

        public object Argument { get => argument; set => argument = value; }
        public string Name { get => name; }

        public int ProgressPos
        {
            get => progressPos;
            set
            {
                progressPos = value;
                OnPropertyChanged("ProgressPos");
            }
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WorkerItem(string name, object argument)
        {
            this.name = name;
            this.argument = argument;

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    /// <summary>
    /// MultipleProgressBarWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MultipleProgressBarWindow : Window, IMultiLanguageSupport
    {
        private string titleText;
        public string TitleText
        {
            get { return titleText; }
        }

        Dictionary<BackgroundWorker, WorkerItem> workerSetDictionary = new Dictionary<BackgroundWorker, WorkerItem>();
        
        public MultipleProgressBarWindow(string title)
        {
            InitializeComponent();
            this.DataContext = this;
            this.titleText = title;

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.AddListener(this);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            workerSetDictionary[worker].Text = e.UserState.ToString();
            workerSetDictionary[worker].ProgressPos = e.ProgressPercentage;
        }

        public void SetArgument(string name, BackgroundWorker worker, object argument)
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            worker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            workerSetDictionary.Add(worker, new WorkerItem(name, argument));
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workerSetDictionary.Keys.Any(worker => worker.IsBusy))
                return;

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkerList.ItemsSource = workerSetDictionary;

            foreach (BackgroundWorker worker in workerSetDictionary.Keys)
                worker.RunWorkerAsync(workerSetDictionary[worker].Argument);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (BackgroundWorker worker in workerSetDictionary.Keys)
                worker.CancelAsync();

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            while (workerSetDictionary.Keys.Any(worker => worker.IsBusy))
            {
                if (stopWatch.ElapsedMilliseconds >= 5000)
                    break;

                Thread.Sleep(100);
            }
                

            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }       
    }
}
