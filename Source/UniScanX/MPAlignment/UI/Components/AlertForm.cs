using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanX.MPAlignment.UI.Components
{
    public partial class AlertForm : Form
    {
        public AlertForm()
        {
            InitializeComponent();
        }

        public enum ActionType
        {
            wait,
            start,
            close
        }

        public enum FormType
        {
            Success,
            Warning,
            Error,
            Info
        }
        private AlertForm.ActionType action;

        private FormType formType;

        private int x, y;

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch(this.action)
            {
                case ActionType.wait:
                    timer1.Interval = 60000; // 딱히 끄고 있지 않는다면 1분동안 알람이 켜져 있는다.
                    action = ActionType.close;
                    break;
                case AlertForm.ActionType.start:
                    this.timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = AlertForm.ActionType.wait;
                        }
                    }
                    break;
                case ActionType.close:
                    timer1.Interval = 10;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(formType == FormType.Error)
            {
     //           DeviceManager.Instance?.IoHandler?.TowerLampWaiting();
            }
            timer1.Interval = 1;
            action = ActionType.close;
        }

        public void ShowAlert(string msg, FormType type)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            if (type == AlertForm.FormType.Error)
            {
  //              DeviceManager.Instance.IoHandler?.TowerLampAlarm();
            }
            int i;
            for ( i = 1; i < 15; i++)
            {
                fname = msg;
                AlertForm frm = (AlertForm)Application.OpenForms[fname]; // 켜저 있는 폼을 가지고 온다.

                // 빈곳에 짚어 넣기 용 
                List<AlertForm> alertForms = new List<AlertForm>();
                foreach (var form in Application.OpenForms)
                {
                    if (form is AlertForm)
                    {
                        alertForms.Add((AlertForm)form);
                    }
                }

                if (frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.Location = new Point(this.x, this.y);
                    break;

                }
                else
                {
                    return;
                }

            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            formType = type;

            switch (type)
            {
                case FormType.Success:
                    this.picIcon.Image = Properties.Resources.success;
                    this.BackColor = Color.SeaGreen;
                    break;
                case FormType.Error:
                    this.picIcon.Image = Properties.Resources.error;
                    this.BackColor = Color.DarkRed;
                    break;
                case FormType.Info:
                    this.picIcon.Image = Properties.Resources.info;
                    this.BackColor = Color.RoyalBlue;
                    break;
                case FormType.Warning:
                    this.picIcon.Image = Properties.Resources.warning;
                    this.BackColor = Color.DarkOrange;
                    break;
            }


            this.lblMsg.Text = msg;


            this.Show();
            this.action = ActionType.start;
            this.timer1.Interval = 1;
            this.timer1.Start();
        }
    }
}
