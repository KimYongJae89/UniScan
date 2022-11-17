using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnieyeLauncher.UI
{
    public partial class ProgressForm : Form
    {
        BackgroundWorker backgroundWorker;
        DoWorkEventHandler DoWorkEventHandler;
        object argument = null;
        bool silent;
        bool cancelable;

        public ProgressForm(string title, bool cancelable, DoWorkEventHandler DoWorkEventHandler, bool silent, object argument=null)
        {
            InitializeComponent();
            this.Text = title;
            this.DoWorkEventHandler = DoWorkEventHandler;
            this.silent = silent;
            this.argument = argument;
            this.cancelable = cancelable;
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            this.progressLabel.Text = "";
            this.buttonCancel.Visible = this.cancelable;

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += DoWorkEventHandler;
            this.backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            this.backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.RunWorkerAsync(this.argument);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show("Cancelled");
            else if (e.Error != null)
                MessageBox.Show(e.Error.Message, e.Error.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (e.Result != null)
                MessageBox.Show(e.Result.ToString());
            else if (silent == false)
                MessageBox.Show("Done");
            this.Close();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressLabel.Text = (e.ProgressPercentage.ToString() + " %");
            progressBar.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 0)
            {
                string message = e.UserState.ToString();
                if (!string.IsNullOrEmpty(message))
                    this.Text = string.Format("{0} : {1}", this.Text, message);
            }
            else
            {
                this.labelWorking.Text = e.UserState.ToString();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Cancel?", "UnieyeLauncher", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                this.backgroundWorker.CancelAsync();
        }
    }
}
