using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;

namespace UniScanG.UI.Etc
{
    public delegate void RunWorkerCompletedDelegate(object result);

    public partial class ProgressForm : Form
    {
        public RunWorkerCompletedDelegate RunWorkerCompleted;

        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public BackgroundWorker BackgroundWorker
        {
          get { return backgroundWorker; }
        }

        private string titleText;
        public string TitleText
        {
            set { titleText = value; }
        }

        private string messageText;
        public string MessageText
        {
          set { messageText = value; }
        }

        private object argument = null;
        public object Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        public Exception LastException => this.lastException;
        Exception lastException;

        public ProgressForm()
        {
            InitializeComponent();

#if DEBUG
            this.ControlBox = true;
#endif
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, "Cancel");

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }
        
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string userMessage = e.UserState?.ToString();
            if (!string.IsNullOrEmpty(userMessage))
                labelMessage.Text = userMessage;
            progressBar.Value = e.ProgressPercentage;
            Invalidate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (messageText != null)
                labelMessage.Text = messageText;

            if (titleText != null)
                labelTitle.Text = titleText;

            startTimer.Start();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            startTimer.Stop();

            if(!backgroundWorker.CancellationPending)
                backgroundWorker.RunWorkerAsync(argument);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, "Canceling");
            backgroundWorker.CancelAsync();

            if (!backgroundWorker.IsBusy)
                Close();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool good = false;
            DialogResult dialogResult = DialogResult.No;

            // 사용자 취소
            if (e.Cancelled == true)
            {
                this.lastException = new OperationCanceledException();
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Canceled");
            }

            // 핸들링 되지 않은 예외
            else if (!(e.Error == null))
            {
                this.lastException = e.Error;
                labelMessage.Text = string.Format("{0} !! {1}", StringManager.GetString(this.GetType().FullName, "Error"), e.Error.Message);
            }

            // 핸들링 된 예외
            else if (e.Result is Exception)
            {
                Exception exception = e.Result as Exception;
                this.lastException = exception;
                labelMessage.Text = string.Format("{0} !! {1}", StringManager.GetString(this.GetType().FullName, "Error"), exception.Message);
            }

            // 완료 또는 실패
            else if (e.Result is Tuple<bool, string>)
            {
                Tuple<bool, string> resultTuple = (Tuple<bool, string>)e.Result;
                if (resultTuple.Item1 == false)
                {
                    this.lastException = new Exception(resultTuple.Item2);
                    labelMessage.Text = string.Format("{0} !! {1}", StringManager.GetString(this.GetType().FullName, "Error"), resultTuple.Item2);
                }
                else
                {
                    progressBar.Value = 100;
                    labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Done");
                    //Hide();
                    dialogResult = DialogResult.OK;
                    good = true;
                }
            }
            // 완료
            else
            {
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Done");
                //Hide();
                dialogResult = DialogResult.OK;
                good = true;
            }

            if (good)
            {
                RunWorkerCompleted?.BeginInvoke(e.Result, null, null);
            }

            this.DialogResult = dialogResult;
            Invalidate();
        }
    }
}
