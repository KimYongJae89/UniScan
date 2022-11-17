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
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;

namespace UniScanG.Common
{
    public abstract class ConfigHelper
    {
        protected static ConfigHelper instance = null;

        public Form MainForm { get => mainForm; }
        protected Form mainForm;

        public abstract UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage();
        public abstract Form GetMainForm();
        public abstract void BuildSystemManager();
        public abstract void InitializeSystemManager();

        public static ConfigHelper Instance()
        {
            return instance;
        }

        public static void SetInstance(ConfigHelper instance)
        {
            ConfigHelper.instance = instance;
        }

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
            
            SplashForm form = new SplashForm(DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag);
            form.ConfigAction = SplashConfigAction;
            form.SetupAlgorithmStrategyAction = SetupAlgorithmStrategyAction;
            form.SetupAction = SplashSetupAction;
            form.title.Text = CustomizeSettings.Instance().ProgramTitle;
            if (File.Exists(PathSettings.Instance().CompanyLogo) == true)
                form.companyLogo.Image = new Bitmap(PathSettings.Instance().CompanyLogo);
            if (File.Exists(PathSettings.Instance().ProductLogo) == true)
                form.productLogo.Image = new Bitmap(PathSettings.Instance().ProductLogo);
            string copyright = CustomizeSettings.Instance().Copyright;
            if (string.IsNullOrEmpty(copyright))
                form.copyrightText.Text = string.Format("@2019 UniEye, All right reserved.");
            else
                form.copyrightText.Text = string.Format("@{0}, All right reserved.", copyright);
            form.title.Text = CustomizeSettings.Instance().Title;

            Configuration.Initialize(PathSettings.Instance().Config, PathSettings.Instance().Temp, 7, false, true, 1);

            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");
            DialogResult dialogResult = form.ShowDialog();

            if (dialogResult == DialogResult.Abort)
                MessageForm.Show(null, string.Format("Some error is occurred. Please, check the configuration.{0}{1}", Environment.NewLine, form.GetLastError()));

            form.Dispose();
            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");
            return dialogResult == DialogResult.OK;
        }

        public virtual bool SplashConfigAction(IReportProgress reportProgress)
        {
            LogInForm loginForm = new LogInForm(false);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (loginForm.LogInUser.IsSuperAccount)
                {
                    //BuildSystemManager();

                    ConfigForm form = new ConfigForm();
                    form.InitSystemType(SystemManager.Instance().GetSystemTypeNames(), OperationSettings.Instance().SystemType);
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

        public bool SetupAlgorithmStrategyAction(IReportProgress reportProgress)
        {
            DoReportProgress(reportProgress, 0, "Initialize Algorithm");
            SystemManager.Instance().BuildAlgorithmStrategy();
            SystemManager.Instance().SelectAlgorithmStrategy();
            if (AlgorithmBuilder.LicenseErrorCount > 0)
                throw new AlarmException(ErrorSectionSystem.Instance.Initialize.InvalidState, ErrorLevel.Fatal, "License Authorize Fail", null, "");

            if (AlgorithmBuilder.IsUseMatroxMil())
                MatroxHelper.InitApplication(OperationSettings.Instance().UseNonPagedMem, OperationSettings.Instance().UseCuda);

            AlgorithmPool.Instance().Initialize(SystemManager.Instance().AlgorithmArchiver);
            return true;
        }

        bool SplashSetupAction(IReportProgress reportProgress)
        {
            float step = 100f / 11f;
            int stepi = 0;
            DoReportProgress(reportProgress, 0, "Start of Initialize");
            InitializeSystemManager();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Model List");
            SystemManager.Instance().ModelManager?.Refresh(PathSettings.Instance().Model);

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Result Manager");
            SystemManager.Instance().InitializeResultManager();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Data Exporter");
            SystemManager.Instance().InitializeDataExporter();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Device");
            SystemManager.Instance().DeviceBox?.Initialize(null);

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Controller");
            SystemManager.Instance().DeviceController?.Initialize(SystemManager.Instance().DeviceBox);

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Additional Units");
            SystemManager.Instance().InitializeAdditionalUnits();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Exchange Operator");
            SystemManager.Instance().ExchangeOperator.Initialize();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize InspectRunner");
            SystemManager.Instance().InitalizeInspectRunner();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Initialize Power Manager");
            SystemManager.Instance().InitializePowerManager();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "End of Initialize");
            SystemManager.Instance().OnSetupDone?.Invoke();

            DoReportProgress(reportProgress, (int)Math.Round(step * stepi++), "Finish");
            return true;
        }

        private void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
                reportProgress.ReportProgress(percentage, StringManager.GetString(this.GetType().FullName, message));
        }

        public void Dispose()
        {
            AlgorithmPool.Instance().Dispose();
            SystemManager.Instance()?.Release();

            if (AlgorithmBuilder.IsUseMatroxMil())
                MatroxHelper.FreeApplication();
        }
    }
}
