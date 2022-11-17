using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.Settings;
using UniEye.Base.Util;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Device;
using DynMvp.Vision;
using UniScanG.Module.Monitor.Config;
using UniEye.Base.MachineInterface;

namespace UniScanG.Module.Monitor
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        LockFile lockFile;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);

            if (bNew)
            {
                lockFile = ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);

                try
                {
                    MainWindow mainWindow = new MainWindow();
                    ConfigHelper configHelper = new ConfigHelper();
                    ApplicationHelper.LoadSettings();
                    ApplicationHelper.InitLicense();
                    PathManager.DataPathType = DataPathType.Day_Model;

                    string logConfigFile = String.Format("{0}\\log4net.xml", PathSettings.Instance().Config);
                    if (LogHelper.InitializeLogSystem(logConfigFile))
                    {
                        LogLevel logLevel = OperationSettings.Instance().LogLevel;
                        logLevel = LogLevel.Debug;
                        LogHelper.ChangeLevel(OperationSettings.Instance().LogLevel.ToString());
                    }
                    else
                    {
                        CustomMessageBox.Show("UniScan", LocalizeHelper.GetString("Can't initialize log configuration."), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    LogHelper.Info(LoggerType.StartUp, "Start Up");

                    InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    if (configHelper.Setup() == true)
                    {
                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                        mainWindow.Initialize();
                        App.Current.MainWindow = mainWindow;
                        App.Current.MainWindow.Show();
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                catch (DllNotFoundException ex)
                {
                    CustomMessageBox.Show("", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }

            }
            else
            {
                CustomMessageBox.Show("", LocalizeHelper.GetString("Application already started."), MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public void InitStringTable()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init StringTable.");
            string localeCode = OperationSettings.Instance().GetLocaleCode();
            string configPath = PathSettings.Instance().Config;

            string fileName;
            if (localeCode == "")
                fileName = String.Format("LocalizeHelper.xml", configPath);
            else
                fileName = String.Format("LocalizeHelper_{1}.xml", configPath, localeCode);

            LocalizeHelper.Clear();
            LocalizeHelper.Load(Path.Combine(configPath, fileName));
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            lockFile?.Dispose();
            SystemManager.Instance()?.Release();
        }
    }
}
