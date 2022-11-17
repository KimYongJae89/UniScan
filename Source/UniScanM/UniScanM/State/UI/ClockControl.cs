using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanM.UI
{
    public partial class ClockControl : UserControl
    {
        private DateTime _StartUpTime = DateTime.Now;
        int _State = 0;
        public ClockControl()
        {
            InitializeComponent();
        }

        void UpdateClock(int state=0)
        {
            string date, time;
            switch (state)
            {
                case 0: //현재 시간
                    date = DateTime.Now.ToString("yyyy. MM. dd");
                    if (labelDate.Text != date)
                        labelDate.Text = date;

                    time = DateTime.Now.ToString("HH:mm:ss");
                    if (labelTime.Text != time)
                        labelTime.Text = time;
                    break;

                case 1: //스타트업 시간
                    date = string.Format("StartUp-Time");
                    if (labelDate.Text != date)
                        labelDate.Text = date;

                    time = _StartUpTime.ToString("yy.MM.dd HH:mm");
                    if (labelTime.Text != time)
                        labelTime.Text = time;
                    break;


                case 2: //스타트업 시간
                    TimeSpan timespan = DateTime.Now.Subtract(_StartUpTime); // - new TimeSpan(365,22,33,44,55));

                    date = string.Format("UpTime");
                    if (labelDate.Text != date)
                        labelDate.Text = date;

                    time = timespan.ToString("dd\\.hh\\:mm\\:ss");
                    if (labelTime.Text != time)
                        labelTime.Text = time;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateClock(_State);
        }

        private void ClockControl_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void labelDate_DoubleClick(object sender, EventArgs e)
        {
            _State++;
            _State = _State < 3 ? _State : 0;
        }
    }
}
