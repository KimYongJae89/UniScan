using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using System.Drawing.Imaging;

namespace DynMvp.UI
{
    public partial class AlarmMessageForm : Form
    {
        bool startup = true;
        int curIndex;
        ErrorItem curErrorItem;

        private Point mousePoint;

        public AlarmMessageForm()
        {
            InitializeComponent();
        }

        private void AlarmMessageForm_Load(object sender, EventArgs e)
        {
            errorCheckTimer.Start();
        }

        private void buttonPrevError_Click(object sender, EventArgs e)
        {
            ShowPrev();
        }

        private bool ShowPrev()
        {
            //curIndex = ErrorManager.Instance().ErrorItemList.FindLastIndex(curIndex - 1, f => f.IsAlarm);
            curIndex--;
            if (curIndex == -1)
                return false;

            ErrorItem newErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];
            newErrorItem.SetShowen(false);

            curErrorItem.SetShowen(true);
            curErrorItem = newErrorItem;

            UpdateData();
            return true;
        }

        private bool ShowFirst()
        {
            curIndex = ErrorManager.Instance().ErrorItemList.FindIndex(f => !f.IsShowen);
            if (curIndex == -1)
                return false;

            curErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];
            UpdateData();
            return true;
        }

        private void buttonNextError_Click(object sender, EventArgs e)
        {
            ShowNext();
        }

        private bool ShowNext()
        {
            //curIndex = ErrorManager.Instance().ErrorItemList.FindIndex(curIndex + 1, f => f.IsAlarm);
            curIndex++;
            if (curIndex == -1)
                return false;

            ErrorItem newErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];
            newErrorItem.SetShowen(false);

            curErrorItem.SetShowen(true);
            curErrorItem = newErrorItem;

            UpdateData();
            return true;
        }

        delegate void UpdateDataDelegate();
        void UpdateData()
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            buttonPrevError.Visible = false;
            buttonNextError.Visible = false;

            //int srcIdx = ErrorManager.Instance().ErrorItemList.FindIndex(f => f.IsAlarm);
            //int dstIdx = ErrorManager.Instance().ErrorItemList.FindLastIndex(f => f.IsAlarm);
            int srcIdx = 0;
            int dstIdx = ErrorManager.Instance().ErrorItemList.Count;
            if (curIndex > srcIdx)
                buttonPrevError.Visible = true;

            if (curIndex < dstIdx)
                buttonNextError.Visible = true;

            errorCode.Text = curErrorItem.ErrorCode.ToString("D4");
            errorLevel.Text = curErrorItem.ErrorLevel.ToString();
            errorTime.Text = curErrorItem.ErrorTime.ToString("yyyy. MM. dd. HH:mm:ss");
            errorSection.Text = curErrorItem.SectionStr;
            errorSubSection.Text = curErrorItem.ErrorStr;
            errorName.Text = curErrorItem.TargetName;
            messsage.Text = curErrorItem.LocaledMessage.ToString();

            if (curErrorItem.ErrorLevel == ErrorLevel.Warning)
                imageIcon.BackgroundImage = global::DynMvp.Properties.Resources.Warning;
            else
                imageIcon.BackgroundImage = global::DynMvp.Properties.Resources.Error;
        }

        private void errorCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!ErrorManager.Instance().IsShowen())
            {
                if (Visible == false || startup == true)
                {
                    CenterToScreen();
                    this.WindowState = FormWindowState.Normal;
                    Show();

                    ErrorManager.Instance().BuzzerOn = false;
                    ShowFirst();

                    Focus();

                    startup = false;
                }
            }
            else
            {
                if (Visible == true)
                {
                    Hide();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                ErrorManager.Instance().ResetAlarm(this.curErrorItem);
                ShowFirst();
            });
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X), this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void buttonAlarmOff_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().BuzzerOn = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().StopAllAlarm();
        }
    }
}
