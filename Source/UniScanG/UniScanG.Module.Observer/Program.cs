using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.Util;

namespace UniScanG.Module.Observer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
            if (lockFile.IsLocked == false)
                return;

            ApplicationHelper.LoadStyleLibrary();
            Application.EnableVisualStyles();

            ApplicationHelper.InitLogSystem();
            ApplicationHelper.InitStringTable();
            ApplicationHelper.InitAuthentication();
            ApplicationHelper.LoadSettings();

            //PathManager.DataPathType = DataPathType.Model_Day_Time;

            ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

            LogHelper.Info(LoggerType.Operation, "Start Up");

            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            lockFile.Dispose();
        }
    }
}
