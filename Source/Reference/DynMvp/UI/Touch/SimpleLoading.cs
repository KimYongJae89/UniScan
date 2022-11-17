using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Base;

namespace DynMvp.UI.Touch
{
    public class SimpleLoading
    {
        private Thread NowLoadingThread = null;

        private string message = "Loading";
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public SimpleLoading(string message)
        {
            this.message = message;
            SetThread();
        }

        public void SetThread()
        {
            NowLoadingThread = new Thread(new ThreadStart(LoadingProc));
            NowLoadingThread.IsBackground = true;
        }

        private void LoadingProc()
        {
            SimpleProgressForm form = new SimpleProgressForm(message);
            form.TopLevel = true;
            form.TopMost = true;

            try
            {
                form.ShowDialog();
                form.Dispose();
            }
            catch (Exception)
            {
                form.Close();
                LogHelper.Debug(LoggerType.Operation, "Simple Loading is safety closed.");
            }
        }

        public void Show()
        {
            LogHelper.Debug(LoggerType.Operation, "Simple Loading is show.");
            NowLoadingThread.Start();
        }

        public void Close()
        {
            if (NowLoadingThread.IsAlive == false)
                return;
            NowLoadingThread.Abort();
            //SetThread();
        }
    }
}
