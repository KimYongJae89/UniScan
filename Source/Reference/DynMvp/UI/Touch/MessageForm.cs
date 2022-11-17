using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.UI.Touch
{
    public enum MessageFormType
    {
        OK, YesNo, RetryCancel, Buzzer
    }

    public partial class MessageForm : Form, IMultiLanguageSupport
    {
        static MessageForm curMessageForm = null;
        public static MessageForm CurMessageForm
        {
            get { return curMessageForm; }
        }

        CancellationToken? cancellationToken;
        public CancellationToken? CancellationToken
        {
            get { return cancellationToken; }
            set { cancellationToken = value; }
        }

        private MessageFormType type;
        public MessageFormType Type
        {
            get { return type; }
            set { type = value; }
        }

        private DialogResult defaultDialogResult;
        public DialogResult DefaultDialogResult
        {
            get { return defaultDialogResult; }
            set { defaultDialogResult = value; }
        }

        private string titleText;
        public string TitleText
        {
            set { titleText = value; }
        }

        private static string defaultTitleText;
        public static string DefaultTitleText
        {
            set { defaultTitleText = value; }
        }

        private string messageText;
        public string MessageText
        {
            set { messageText = value; }
        }

        private int displayTimeMs = -1;
        public int DisplayTimeMs
        {
            get { return displayTimeMs; }
            set { displayTimeMs = value; }
        }

        private System.Windows.Forms.Timer displayTimer = null;

        private SoundPlayer buzzerPlayer = new SoundPlayer(DynMvp.Properties.Resources.BUZZER_1);

        private MessageForm()
        {
            InitializeComponent();

            // 제발 TopMost 쓰지 마세요...
            // ShowDialog 할때 전달인자로 부모 컨트롤 넣어서 할것!
            //TopMost = true;

            //languge change
            //btnYes.Text = StringManager.GetString(this.GetType().FullName, btnYes.Text);
            //btnNo.Text = StringManager.GetString(this.GetType().FullName, btnNo.Text);
            //btnClose.Text = StringManager.GetString(this.GetType().FullName, btnClose.Text);

            StringManager.AddListener(this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.UserNotify, "btnClose_Click");
            buzzerStart = false;

            Close();
        }

        private delegate DialogResult ShowShowDelegate(Control parentForm);
        private DialogResult ShowShow(Control parentControl)
        {
            //bool invokeRequired = parentControl == null ? this.InvokeRequired : parentControl.InvokeRequired;
            if (parentControl == null)
            {
                if (this.InvokeRequired)
                {
                    return (DialogResult)this.Invoke(new ShowShowDelegate(ShowShow), parentControl);
                }
                return this.ShowDialog();
            }
            else
            {
                if (parentControl.InvokeRequired)
                {
                    return (DialogResult)parentControl.Invoke(new ShowShowDelegate(ShowShow), parentControl);
                }
                return this.ShowDialog(parentControl);
            }
        }

        public static DialogResult Show(Control parentControl, string msg, MessageFormType messageFormType = MessageFormType.OK, CancellationToken? cancellationToken = null)
        {
            LogHelper.Info(LoggerType.UserNotify, string.Format("Show MessageForm - {0}", msg));

            //curMessageForm.Invoke(() => Close());
            curMessageForm = new MessageForm();
            curMessageForm.Type = messageFormType;
            curMessageForm.TitleText = defaultTitleText;
            curMessageForm.MessageText = msg;
            curMessageForm.StartPosition = FormStartPosition.CenterParent;
            //curMessageForm.TopMost = true;
            curMessageForm.cancellationToken = cancellationToken;

            return curMessageForm.ShowShow(parentControl);
        }

        public static DialogResult Show(Control parentControl, string msg, MessageFormType messageFormType, DialogResult defaultDialogResult, CancellationToken? cancellationToken = null)
        {
            LogHelper.Info(LoggerType.UserNotify, string.Format("Show MessageForm - {0}", msg));

            //curMessageForm.Invoke(() => Close());
            curMessageForm = new MessageForm();
            curMessageForm.Type = messageFormType;
            curMessageForm.TitleText = defaultTitleText;
            curMessageForm.MessageText = msg;
            curMessageForm.defaultDialogResult = defaultDialogResult;
            curMessageForm.StartPosition = FormStartPosition.CenterParent;
            //curMessageForm.TopMost = true;
            curMessageForm.cancellationToken = cancellationToken;

            return curMessageForm.ShowShow(parentControl);
        }

        public static DialogResult Show(Control parentControl, string msg, string title, MessageFormType messageFormType = MessageFormType.OK, int displayTimeMs = -1, DialogResult defaultDialogResult = DialogResult.Yes)
        {
            LogHelper.Info(LoggerType.UserNotify, string.Format("Show MessageForm - {0}", msg));

            curMessageForm = new MessageForm();
            curMessageForm.Type = messageFormType;
            curMessageForm.TitleText = title;
            curMessageForm.MessageText = msg;
            curMessageForm.DisplayTimeMs = displayTimeMs;
            curMessageForm.defaultDialogResult = defaultDialogResult;
            curMessageForm.StartPosition = FormStartPosition.CenterParent;

//#if DEBUG == false
//            curMessageForm.TopMost = true;
//#endif

            return curMessageForm.ShowShow(parentControl);
        }

        public static DialogResult Show(Control parentControl, string msg, string title, Color backColor, MessageFormType messageFormType = MessageFormType.OK, int displayTimeMs = -1, DialogResult defaultDialogResult = DialogResult.Yes)
        {
            LogHelper.Info(LoggerType.UserNotify, string.Format("Show MessageForm - {0}", msg));

            curMessageForm = new MessageForm();
            curMessageForm.Type = messageFormType;
            curMessageForm.TitleText = title;
            curMessageForm.MessageText = msg;
            curMessageForm.DisplayTimeMs = displayTimeMs;
            curMessageForm.defaultDialogResult = defaultDialogResult;
            curMessageForm.StartPosition = FormStartPosition.CenterParent;
            curMessageForm.labelTitle.BackColor = backColor;
//#if DEBUG == false
//            curMessageForm.TopMost = true;
//#endif

            return curMessageForm.ShowShow(parentControl);
        }
        private void MessageForm_Load(object sender, EventArgs e)
        {
            message.Text = messageText;
            labelTitle.Text = titleText;
            //StartBuzzerThread();
            switch (type)
            {
                case MessageFormType.OK:
                    btnYes.Hide();
                    btnNo.Hide();
                    break;
                case MessageFormType.YesNo:
                    btnYes.DialogResult = DialogResult.Yes;
                    btnNo.DialogResult = DialogResult.No;
                    btnClose.Hide();
                    break;
                case MessageFormType.RetryCancel:
                    btnYes.DialogResult = DialogResult.Retry;
                    btnYes.Text = StringManager.GetString("Retry");

                    btnNo.DialogResult = DialogResult.Cancel;
                    btnNo.Text = StringManager.GetString("Cancel");

                    btnClose.Hide();
                    break;
                case MessageFormType.Buzzer:
                    btnYes.Hide();
                    btnNo.Hide();
                    StartBuzzerThread();
                    break;
                default:
                    throw new InvalidTypeException();
            }

            switch (this.defaultDialogResult)
            {
                case DialogResult.OK:
                    btnClose.ForeColor = Color.Green;
                    break;
                case DialogResult.Cancel:
                    btnYes.ForeColor = Color.Red;
                    btnNo.ForeColor = Color.Green;
                    break;
                case DialogResult.Retry:
                    btnYes.ForeColor = Color.Green;
                    btnNo.ForeColor = Color.Red;
                    break;
                case DialogResult.Yes:
                    btnYes.ForeColor = Color.Green;
                    btnNo.ForeColor = Color.Red;
                    break;
                case DialogResult.No:
                    btnYes.ForeColor = Color.Red;
                    btnNo.ForeColor = Color.Green;
                    break;
            }

            displayTimer = new System.Windows.Forms.Timer();
            displayTimer.Interval = 100;
            displayTimer.Tick += displayTimer_Tick;
            displayTimer.Start();
            //alarmCheckTimer.Start();
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            if (this.displayTimeMs >= 0)
            {
                this.displayTimeMs = Math.Max(0, this.displayTimeMs - 100);

                if (this.displayTimeMs > 0)
                {
                    labelTitle.Text = string.Format("{0} ... ({1})", titleText, displayTimeMs / 1000);
                }
                else
                {
                    this.displayTimer.Stop();
                    this.DialogResult = this.defaultDialogResult;
                    Close();
                }
            }

            if (cancellationToken.HasValue)
            {
                if (cancellationToken.Value.IsCancellationRequested)
                {
                    this.DialogResult = this.defaultDialogResult;
                    Close();
                }
            }
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private Thread buzzerThread;
        private void StartBuzzerThread()
        {
            if (buzzerThread == null || buzzerThread.IsAlive == false)
            {
                buzzerThread = new Thread(new ThreadStart(BuzzerThreadProc));
                buzzerThread.IsBackground = true;
                buzzerThread.Start();
            }
        }

        private bool buzzerStart = true;

        private void BuzzerThreadProc()
        {
            while (buzzerStart == true)
            {
                buzzerPlayer.Play();
                Thread.Sleep(1000);
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.Debug(LoggerType.UserNotify, "btnYes_Click");
                buzzerStart = false;
                this.DialogResult = DialogResult.Yes;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.UserNotify, "btnNo_Click");
            buzzerStart = false;
            this.DialogResult = DialogResult.No;
            Close();
        }

        private void MessageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogHelper.Debug(LoggerType.UserNotify, "MessageForm_FormClosed");
        }

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHelper.Info(LoggerType.UserNotify, string.Format("DialogResult = {0}", this.DialogResult.ToString()));
            StringManager.RemoveListener(this);
        }

        private void alarmCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!ErrorManager.Instance().IsCleared())
                Close();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void MessageForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool esc = (e.KeyCode == Keys.Escape);
            bool enter = (e.KeyCode == Keys.Enter);

            switch (curMessageForm.Type)
            {
                case MessageFormType.OK:
                case MessageFormType.Buzzer:
                    if (esc || enter) btnClose_Click(sender, e);
                    break;
                case MessageFormType.YesNo:
                case MessageFormType.RetryCancel:
                    if (enter) btnYes_Click(sender, e);
                    else if (esc) btnNo_Click(sender, e);
                    break;
            }
        }

        private void MessageForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
