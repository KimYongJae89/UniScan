using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Util;
using UniEye.Base.Settings;
using DynMvp.Base;
using System.IO;
using UniScanG.Common.Settings;
using UniEye.Base.Data;
using DynMvp.Data;
using UniEye.Base.MachineInterface;
using UniScanG.Module.Controller.UI;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Common;
using DynMvp.Vision;
using DynMvp.Devices;
using UniScanG.Module.Controller.Settings.Monitor;
using System.Runtime.InteropServices;

namespace UniScanG.Module.Controller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationHelper.InitStringTable();
            ApplicationHelper.InitLogSystem();
            ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);
            ApplicationHelper.InitAuthentication();
            ApplicationHelper.InitLicense();

            if (!UniScanG.Program.ParseArgs(Environment.GetCommandLineArgs()))
                return;

            bool bNew;
            Mutex mutex = new Mutex(true, "UniScanMon", out bNew);
            if (!bNew)
            {
                MessageBox.Show("Program is Already Running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationHelper.LoadStyleLibrary();

            LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
            if (lockFile.IsLocked == false)
                return;
#if DEBUG
            //DynMvp.ConsoleEx.Alloc();
#endif

            MonitorConfigHelper.SetInstance();
            ConfigHelper.Instance().BuildSystemManager();

            //ApplicationHelper.LoadSettings();

            SystemTypeSettings.Instance().Load();
            MonitorSystemSettings.Instance().Load();


            UniScanG.Gravure.Settings.AdditionalSettings.CreateInstance();
            UniScanG.Gravure.Settings.AdditionalSettings.Instance().SetBrawserable("LaserSetting", MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None);

            LogHelper.Info(LoggerType.Operation, "Start Up");
            PathManager.DataPathType = DataPathType.Model_Day_Time;

            LogHelper.Debug(LoggerType.StartUp, "Start Setup.");
            bool ok = ConfigHelper.Instance().Setup();
            LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

            if (ok)
                Application.Run(ConfigHelper.Instance().GetMainForm());

            LogHelper.Debug(LoggerType.StartUp, "Terminating Program.");

            ConfigHelper.Instance().Dispose();
            lockFile.Dispose();

            LogHelper.Debug(LoggerType.StartUp, "Program Terminated");
            mutex.Close();
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}