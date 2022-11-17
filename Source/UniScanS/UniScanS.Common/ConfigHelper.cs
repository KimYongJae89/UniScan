using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.UI;

namespace UniScanS.Common
{
    public abstract class ConfigHelper
    {
        public abstract UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage();
        public abstract Form GetMainForm();
        public abstract void BuildSystemManager();
        public abstract void InitializeSystemManager();

        public bool Setup()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init SplashForm");
            
            if (OperationSettings.Instance().UseUserManager)
            {
                LogInForm loginForm = new LogInForm();
                loginForm.ShowDialog();
                if (loginForm.DialogResult == DialogResult.Cancel)
                    return false;

                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
            }

            SplashForm form = new SplashForm("");
            form.ConfigAction = SplashConfigAction;
            form.SetupAction = SplashSetupAction;
            form.title.Text = CustomizeSettings.Instance().ProgramTitle;
            if (File.Exists(PathSettings.Instance().CompanyLogo) == true)
                form.companyLogo.Image = new Bitmap(PathSettings.Instance().CompanyLogo);
            if (File.Exists(PathSettings.Instance().ProductLogo) == true)
                form.productLogo.Image = new Bitmap(PathSettings.Instance().ProductLogo);
            form.copyrightText.Text = CustomizeSettings.Instance().Copyright;
            form.title.Text = CustomizeSettings.Instance().Title;
            Configuration.Initialize(PathSettings.Instance().Config, PathSettings.Instance().Temp, 7, false, true, 1);
            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");
            
            form.ShowDialog();
            
            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");

            if (form.DialogResult == DialogResult.Abort)
            {
                return false;
            }

            if (AlgorithmBuilder.IsUseMatroxMil())
                MatroxHelper.InitApplication();

            try
            {
                SystemManager.Instance().DeviceBox?.Initialize(null);
                SystemManager.Instance().DeviceController?.Initialize(SystemManager.Instance().DeviceBox);
            }
#if !DEBUG
                catch (DllNotFoundException ex)
                {
                    string message = string.Format("DllNotFoundException\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                    MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (FileNotFoundException ex)
                {
                    string message = string.Format("FileNotFoundException\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                    MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
#endif
            finally { }

            SystemManager.Instance().ExchangeOperator.Initialize();
            SystemManager.Instance().InitalizeInspectRunner();

            bool ok = true;
            if (SystemManager.Instance().OnSetupDone != null)
                ok = SystemManager.Instance().OnSetupDone();
            return ok;
        }

        public virtual bool SplashConfigAction(IReportProgress reportProgress)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (loginForm.LogInUser.IsSuperAccount)
                {
                    BuildSystemManager();

                    ConfigForm form = new ConfigForm();
                    form.InitCustomConfigPage(GetCustomConfigPage());
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "Please, Restart the Program."));
                        return false;
                    }
                }
                else
                {
                    MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "You don't have proper permission."));
                }
            }
            return true;
        }
    
        bool SplashSetupAction(IReportProgress reportProgress)
        {
#if !DEBUG
            try
#endif
            {
                DoReportProgress(reportProgress, 10, "Initialize Model List");

                BuildSystemManager();
                InitializeSystemManager();

                SystemManager.Instance().BuildAlgorithmStrategy();
                SystemManager.Instance().SelectAlgorithmStrategy();

                SystemManager.Instance().ModelManager?.Refresh(PathSettings.Instance().Model);
                
                AlgorithmPool.Instance().Initialize(SystemManager.Instance().AlgorithmArchiver);
                //AlgorithmPool.Instance().BuildAlgorithmPool();

                DoReportProgress(reportProgress, 70, "Start Image Copy");

                SystemManager.Instance().InitializeResultManager();
                //if (OperationSettings.Instance().ResultStoringDays > 0)
                //{
                //    SystemManager.Instance().DataRemover = new DataRemover(DataStoringType.Date, PathSettings.Instance().Result,
                //                                        OperationSettings.Instance().ResultStoringDays, "yy-MM-dd", false);
                //    SystemManager.Instance().DataRemover.Start();
                //}

                DoReportProgress(reportProgress, 80, "Create Data Exporter");
                SystemManager.Instance().InitializeDataExporter();

                DoReportProgress(reportProgress, 90, "Init Additional Units");
                SystemManager.Instance().InitializeAdditionalUnits();
            }
#if !DEBUG
            catch (Exception ex)
            {
                DoReportProgress(reportProgress, 100, ex.Message);
                reportProgress.SetLastError(ex.Message);

                return false;
            }
#endif

            DoReportProgress(reportProgress, 90, "End of Initialize");

            return true;
        }

        private void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
                reportProgress.ReportProgress(percentage, StringManager.GetString(this.GetType().FullName, message));
        }

    }
}
