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
using DynMvp.UI.Touch;
using System.IO;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public delegate bool SplashActionDelegate(SplashForm form);

    public partial class SplashForm : Form, IReportProgress
    {
        string lastError;
        string startupArgs = "";
        bool doConfigAction = false;
        public SplashActionDelegate ConfigAction = null;
        public SplashActionDelegate SetupAlgorithmStrategyAction = null;
        public SplashActionDelegate SetupAction = null;
        public SplashActionDelegate PostSetupAction = null;

        public SplashForm(string startupArgs)
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Loading...");
            this.startupArgs = startupArgs;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(title.Text) == false)
                title.Font = UiHelper.AutoFontSize(title, title.Text);

            this.label1.Text = startupArgs;
            versionText.Text = string.Format("Version {0}", VersionHelper.Instance().VersionString);
            buildText.Text = string.Format("Build {0}", VersionHelper.Instance().BuildString);

            splashActionTimer.Start();
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        public void ReportProgress(int progressPos, string progressMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReportProgressDelegate(ReportProgress), progressPos, progressMessage);
                return;
            }

            this.progressBar.Value = progressPos;
            this.progressMessage.Text = progressMessage;
        }

        private void SpalashProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start SpalashProc.");
            SetupAction(this);
        }

        private void SplashForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12 /*&& e.Alt == true*/)
            {
                if (false)
                {
                    splashActionTimer.Stop();
                    UiHelper.SetControlText(progressMessage, StringManager.GetString(this.GetType().FullName, "Wait Configuration"));

                    Task task = new Task(() =>
                    {
                        if (ConfigAction != null && !doConfigAction)
                        {
                            doConfigAction = true;
                            bool result = ConfigAction(this);
                            doConfigAction = false;

                            if (!result)
                            {
                                this.DialogResult = DialogResult.Cancel;
                                //Close();
                                return;
                            }

                            splashActionTimer.Start();
                            UiHelper.SetControlText(progressMessage, StringManager.GetString(this.GetType().FullName, "Loading..."));
                        }
                    });
                    task.Start();
                }
                else
                {
                    splashActionTimer.Stop();
                    progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Wait Configuration");
                    if (ConfigAction != null && !doConfigAction)
                    {
                        doConfigAction = true;
                        bool result = ConfigAction(this);
                        doConfigAction = false;

                        if (!result)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            //Close();
                            return;
                        }
                    }

                    splashActionTimer.Start();
                    progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Loading...");
                }
            }
        }

        private void splashActionTimer_Tick(object sender, EventArgs e)
        {
            doConfigAction = true;
            splashActionTimer.Stop();

            LogHelper.Debug(LoggerType.StartUp, "Start Spalash Thread.");
            progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Start Setup...");
            DialogResult dialogResult = DialogResult.Abort;
            try
            {
                SetupAlgorithmStrategyAction?.Invoke(this);
                SpalashProc();
                dialogResult = DialogResult.OK;
            }
#if DEBUG == false
            catch (AlarmException ex)
            {
                ex.ShowMessageBox();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("!! ERROR !!");
                sb.AppendLine(ex.GetType().ToString());
                sb.AppendLine();
                sb.AppendLine(ex.Message);

                MessageBox.Show(sb.ToString(), "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
            finally { }
            this.DialogResult = dialogResult;
        }
    }
}
