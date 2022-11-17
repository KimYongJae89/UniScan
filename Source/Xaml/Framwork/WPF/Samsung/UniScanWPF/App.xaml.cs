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
using UniScanWPF.UI;

namespace UniScanWPF
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);

            if (bNew)
            {
                try
                {
                    ApplicationHelper.LoadSettings();

                    string logConfigFile = String.Format("{0}\\log4net.xml", PathSettings.Instance().Config);
                    if (LogHelper.InitializeLogSystem(logConfigFile))
                    {
                        LogLevel logLevel = OperationSettings.Instance().LogLevel;
                        logLevel = LogLevel.Debug;
                        LogHelper.ChangeLevel(OperationSettings.Instance().LogLevel.ToString());
                    }
                    else
                    {
                        //CustomMessageBox.Show("Can't initialize log configuration.", "UniScan", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    //ErrorManager.Instance().LoadErrorList();

                    //ApplicationHelper.InitLogSystem();

                    LogHelper.Info(LoggerType.Operation, "Start Up");

                    ApplicationHelper.InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    //BuildSystemManager();

                    new MainWindow().Show();

                    //LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                    //if (SystemManager.Instance().Setup() == true)
                    //{
                    //    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

                    //    IMainForm mainForm = SystemManager.Instance().UiChanger.CreateMainForm();
                    //    SystemManager.Instance().MainForm = mainForm;

                    //    Application.Run((Form)mainForm);
                    //}

                    //SystemManager.Instance().Release();

                    //lockFile.Dispose();
                }
                catch (DllNotFoundException ex)
                {
                    CustomMessageBox.Show(ex.Message, "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                CustomMessageBox.Show("Application already started.", "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
