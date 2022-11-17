using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;
using UniScanG.Common;
using UniScanG.Common.Settings;
using UniScanG.Module.Inspector.Settings.Inspector;
using UniScanG.Module.Inspector.UI;
using System.Diagnostics;
using System.Collections.Generic;

namespace UniScanG.Module.Inspector
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

            bool bNew = true;
            Mutex mutex = new Mutex(true, "UniScanInsp", out bNew);
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
            InspectorConfigHelper.SetInstance();
            ConfigHelper.Instance().BuildSystemManager();

            //ApplicationHelper.LoadSettings();

         

            SystemTypeSettings.Instance().Load();
            InspectorSystemSettings.Instance().Load();


            UniScanG.Gravure.Settings.AdditionalSettings.CreateInstance();

            LogHelper.Info(LoggerType.Operation, $"Start Up");
            PathManager.DataPathType = DataPathType.Model_Day_Time;

            LogHelper.Debug(LoggerType.StartUp, "Start Setup.");
            bool setup = ConfigHelper.Instance().Setup();
            LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
            if (setup)
            {
                //SystemManager.Instance().ProductionManager.Load();
                //int resultStoringDays = OperationSettings.Instance().ResultStoringDays;
                //if (resultStoringDays > 0)
                //{
                //    SystemManager.Instance().DataRemover = new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Date, PathSettings.Instance().Result, resultStoringDays, "yyyy-MM-dd", true);
                //    SystemManager.Instance().DataRemover.Start();
                //}


                Form mainForm = ConfigHelper.Instance().GetMainForm();
                Application.Run(mainForm);
            }

            try
            {
                LogHelper.Debug(LoggerType.StartUp, "Terminating Program.");
                AlgorithmPool.Instance().Dispose();

                if (SystemManager.Instance() != null)
                    SystemManager.Instance().Release();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.StartUp, ex);
            }

            lockFile.Dispose();
            LogHelper.Debug(LoggerType.StartUp, "Program Terminated");

            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}