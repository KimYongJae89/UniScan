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
using DynMvp.UI;
using System.Threading.Tasks;

namespace DynMvp.UI.Touch
{
    public partial class SimpleProgressForm : Form
    {         
        public Exception Exception { get; private set; }

        private string messageText;
        public string MessageText
        {
            set {
                messageText = value;
                SetLabelMessage(messageText);
            }
        }

        CancellationTokenSource cancellationTokenSource;

        Task task;
        public Task Task
        {
            get {return  task; }
        }

        public SimpleProgressForm(string message = null)
        {
            InitializeComponent();

#if DEBUG
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
#endif

            messageText = message;
            SetLabelMessage(messageText);

            this.ShowInTaskbar = false;
        }

        public delegate void SetLabelMessageDelegate(string message);
        public void SetLabelMessage(string message)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new SetLabelMessageDelegate(SetLabelMessage), message);
                return;
            }

            messageText = message;
            if (string.IsNullOrEmpty(messageText))
                this.labelMessage.Text = StringManager.GetString("Wait");
            else
                this.labelMessage.Text = message;
        }

        delegate void ShowDelegate(IWin32Window parent, Action action, CancellationTokenSource cancellationTokenSource = null);
        public void Show(IWin32Window parent, Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            if (parent is Control)
            {
                Control control = (Control)parent;
                if (control.InvokeRequired)
                {
                    control.Invoke(new ShowDelegate(Show), parent, action, cancellationTokenSource);
                    return;
                }
            }

            this.cancellationTokenSource = cancellationTokenSource;

            if (cancellationTokenSource != null)
            {
                task = new Task(action, cancellationTokenSource.Token);
            }
            else
            {
                task = new Task(action);
            }

            if (parent == null)
            {
                //this.TopLevel = true;
                base.ShowDialog();
            }
            else
            {
                //this.TopLevel = false;
                base.ShowDialog(parent);
            }

            if (this.messageText == null)
                this.Hide();
        }

        public void Show(Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            Show(null, action, cancellationTokenSource);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            CheckProgress();
        }

        private void CheckProgress()
        {
            if (task == null || task.IsCompleted)
            {
                Exception = task.Exception;
                Close();
            }

            if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
            {
                Close();
                //throw new OperationCanceledException();
            }
        }

        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            if (cancellationTokenSource == null)
                buttonCancel.Visible = false;

            if (this.Parent == null)
                CenterToScreen();

            if (this.task.IsCompleted)
            {
                CheckProgress();
            }
            else
            {
                this.task.Start();
                this.taskCheckTimer.Start();
            }
        }
    }
}
