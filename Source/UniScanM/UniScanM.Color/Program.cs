using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;
using UniScanM.MachineIF;

namespace UniScanM.ColorSens
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);
            if (bNew)
            {                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    ApplicationHelper.KillProcesses();

                    BuildSystemManager();

                    ApplicationHelper.LoadStyleLibrary();
                    ApplicationHelper.LoadSettings();

                    PathManager.DataPathType = DataPathType.Model_Day_Time;

                    ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

                    ApplicationHelper.InitLogSystem();

                    LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
                    if (lockFile.IsLocked == false)
                        return;

                    LogHelper.Info(LoggerType.Operation, "Start Up");
                    //if (ApplicationHelper.CheckLicense() == false)
                    //    return;

                    ApplicationHelper.InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                    if (SystemManager.Instance().Setup() == true)
                    {
                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                        SystemManager.Instance().ProductionManager.Load();

                        IMainForm mainForm = SystemManager.Instance().UiChanger.CreateMainForm();
                        SystemManager.Instance().MainForm = mainForm;
                        SystemManager.Instance().LoadDefaultModel();

                        Application.Run((Form)mainForm);
                    }

                    SystemManager.Instance().Release();

                    lockFile.Dispose();

                    Application.ExitThread();
                    Environment.Exit(0);
                }
                catch (DllNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void BuildSystemManager()
        {
            SystemManager systemManager = new UniScanM.ColorSens.SystemManager();
            SystemManager.SetInstance(systemManager);

            UniEye.Base.MachineInterface.MachineIfProtocolList.Set(new UniScanM.ColorSens.MachineIF.MachineIfProtocolList());
            
            systemManager.Init(
                new UniScanM.ColorSens.Data.ModelManager(), 
                new UniScanM.ColorSens.UI.UiChanger(), 
                new AlgorithmArchiver(),
                new UniScanM.ColorSens.Device.DeviceBox(new PortMap()), 
                new DeviceController(),
                new UniScanM.Data.ProductionManager(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());
            //new UniScanM.ColorSens.Device.DeviceBox(new PortMap())
        }
    }
}
