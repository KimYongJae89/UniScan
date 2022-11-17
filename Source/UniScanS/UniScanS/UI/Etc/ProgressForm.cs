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

namespace UniScanS.UI.Etc
{
    public delegate void RunWorkerCompleted(bool result);

    public partial class ProgressForm : Form
    {
        public RunWorkerCompleted RunWorkerCompleted;

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

        string lastError;

        public ProgressForm()
        {
            InitializeComponent();
            
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelMessage.Text = StringManager.GetString(this.GetType().FullName, e.UserState.ToString());
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
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, messageText);

            if (titleText != null)
                labelTitle.Text = StringManager.GetString(this.GetType().FullName, titleText);

            startTimer.Start();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            startTimer.Stop();
            backgroundWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            Close();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                progressBar.Value = 0;
                labelMessage.Text = "Cancel";
            }
            else if (!(e.Error == null))
            {
                progressBar.Value = 0;
                labelMessage.Text = ("Error !!");
            }
            else if (e.Result != null)
            {
                progressBar.Value = 100;
                progressBar.ForeColor = Color.Red;
                labelMessage.Text = "Error : " + e.Result; 
            }
            else
            {
                labelMessage.Text = "Done";
                Close();
            }

            if (RunWorkerCompleted != null)
                RunWorkerCompleted(e.Result == null ? true : false);

            Invalidate();
        }

        private void layoutMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
