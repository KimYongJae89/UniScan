using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;

namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class LogPage : UserControl, LoggingTarget
    {
        public LogPage()
        {
            InitializeComponent();
            LogHelper.LoggingTarget = this;
        }

        public void Log(string message)
        {
            try
            {
                if (lsbLogs.InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { Log(message); }));
                    return;
                }
                if (lsbLogs.Items.Count > 2000)
                    lsbLogs.Items.RemoveAt(0);
                if (message.Contains("Packet Sent :") || message.Contains("Data received :"))
                    return;

                lsbLogs.Items.Insert(0, message);
            }
            catch
            {
                return;
            }
        }

        public void Release()
        {
            LogHelper.LoggingTarget = null; ;
        }

        private void LogPage_VisibleChanged(object sender, EventArgs e)
        {
            var t =this.Visible;
        }

        private void LogPage_Enter(object sender, EventArgs e)
        {

        }
    }
}
