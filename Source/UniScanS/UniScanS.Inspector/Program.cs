using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;
using UniScanS.Common;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.Inspector;
using UniScanS.Inspector.UI;

namespace UniScanS.Inspector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            KillProcesses();

            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);
            if (bNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SystemTypeSettings.Instance().Load();
                InspectorSystemSettings.Instance().Load();
                
                ApplicationHelper.LoadStyleLibrary();
                ApplicationHelper.LoadSettings();
                
                PathManager.DataPathType = DataPathType.Model_Day_Time;

                ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

                ApplicationHelper.InitLogSystem();

                LockFile lockFile = CreateLockFile(PathSettings.Instance().Temp, PathSettings.Instance().Log);

                LogHelper.Info(LoggerType.Operation, "Start Up");
                //if (ApplicationHelper.CheckLicense() == false)
                //    return;

                ApplicationHelper.InitStringTable();
                ApplicationHelper.InitAuthentication();

                //BuildSystemManager();

                LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                if (InspectorConfigHelper.Instance().Setup() == true)
                {
                    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

                    Form mainForm = InspectorConfigHelper.Instance().GetMainForm();
                    Application.Run(mainForm);
                }

                AlgorithmPool.Instance().Dispose();

                if (SystemManager.Instance() != null)
                    SystemManager.Instance().Release();

                lockFile.Dispose();

                Application.ExitThread();
                Environment.Exit(0);
            }
        }
        

        public static LockFile CreateLockFile(string tempFIlder, string logFolder)
        {
            // 다운이 발생했을 때, 이전 디버그 로그를 저장
            string lockFilePath = Path.Combine(tempFIlder, "~UniEye.lock");
            string debugLogPath = Path.Combine(logFolder, "Debug.log");

            return new LockFile(lockFilePath);
        }

        private static void KillProcesses()
        {
            List<Process> uniScanList = new List<Process>();
            Process[] processList = Process.GetProcesses();

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Contains("UniScan"))
                    uniScanList.Add(process);
                //process.
            }

            if (uniScanList.Count > 1)
            {
                uniScanList = uniScanList.OrderByDescending(p => p.StartTime).ToList();
                uniScanList.Remove(uniScanList[0]);

                uniScanList.ForEach(u => u.Kill());
            }
        }
    }
}
