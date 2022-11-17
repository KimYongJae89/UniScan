using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniScanWPF.UI;

namespace UniScanWPF.Settings
{
    public abstract class ConfigHelper
    {
        protected abstract void BuildSystemManager();
        
        public bool Setup()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init SplashForm");

            SplashWindow window = new SplashWindow();
            window.ConfigAction = SplashConfigAction;
            window.SetupAction = SplashSetupAction;
            
            Configuration.Initialize(PathSettings.Instance().Config, PathSettings.Instance().Temp, 7, false, true, 1);
            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");
            
            window.ShowDialog();
            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");

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
            
            SystemManager.Instance().InitalizeInspectRunner();

            bool ok = true;
            if (SystemManager.Instance().OnSetupDone != null)
                ok = SystemManager.Instance().OnSetupDone();

            return ok;
        }

        public virtual bool SplashConfigAction(IReportProgress reportProgress)
        {
            LogInWindow logInWindow = new LogInWindow();
            if (logInWindow.ShowDialog() == true)
            {
                if (logInWindow.User.IsSuperAccount)
                {
                    ConfigForm form = new ConfigForm();
                    form.TopMost = true;
                    //form.InitCustomConfigPage();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        CustomMessageBox.Show(StringManager.GetString("Please, Restart the Program."), "UniEye", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    CustomMessageBox.Show(StringManager.GetString("You don't have proper permission."), "UniEye", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            return true;
        }

        bool SplashSetupAction(IReportProgress reportProgress)
        {
            try
            {
                DoReportProgress(reportProgress, 10, "Initialize System");
                BuildSystemManager();

                DoReportProgress(reportProgress, 50, "Setup Algorithm");
                SystemManager.Instance().BuildAlgorithmStrategy();
                SystemManager.Instance().SelectAlgorithmStrategy();
                if (AlgorithmBuilder.LicenseErrorCount > 0)
                {
                    throw new Exception("License Authorize Fail");
                }

                DoReportProgress(reportProgress, 60, "Load Algorithm Pool");
                AlgorithmPool.Instance().Initialize(SystemManager.Instance().AlgorithmArchiver);

                DoReportProgress(reportProgress, 70, "Setup Result Manager");
                SystemManager.Instance().InitializeResultManager();

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
            finally { }

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
