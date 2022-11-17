using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;

namespace UniScanM.RVMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);
            if (bNew)
            {
                string paths = System.Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
                string[] pp = paths.Split(';');

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
                    //    return

                    ApplicationHelper.InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    //BuildSystemManager();

                    LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                    if (OperationSettings.Instance().UseHasp)
                        HaspHelper.CheckHaspLicsence();

                    if (SystemManager.Instance().Setup() == true)
                    {
                        SystemManager.Instance().ProductionManager.Load();
                        DynMvp.UI.Touch.SimpleProgressForm form = new DynMvp.UI.Touch.SimpleProgressForm("Wait");
                        //form.Show(() => LoadDefaultModel());

                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

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
                    MessageBox.Show(ex.Message, "RVMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private static void LoadDefaultModel()
        //{
        //    UniEye.Base.Data.ModelManager modelManager = SystemManager.Instance().ModelManager;
        //    if (modelManager == null)
        //        return;

        //    DynMvp.Data.ModelDescription modelDescription = modelManager.LoadModelDescription("DefaultModel");
        //    if (modelDescription == null)
        //    {
        //        modelDescription = modelManager.CreateModelDescription();
        //        modelDescription.Name = "DefaultModel";

        //        DynMvp.Data.Model model = modelManager.CreateModel();
        //        model.ModelDescription = modelDescription;
        //        model.ModelPath = Path.Combine(modelManager.GetModelPath(modelDescription));
        //        modelManager.SaveModel(model);
        //        //modelManager.SaveModelDescription(model);

        //        modelManager.Refresh();
        //    }

        //    SystemManager.Instance().LoadModel(modelDescription);

        //    GC.Collect();
        //    GC.WaitForFullGCComplete();
        //}

        public static void BuildSystemManager()
        {
            SystemManager systemManager = new SystemManager();
            SystemManager.SetInstance(systemManager);

            MachineIfProtocolList.Set(new UniScanM.RVMS.MachineIF.MachineIfProtocolList());
            systemManager.Init(
                new UniScanM.Data.ModelManager(),
                new RVMS.UI.UiChanger(),
                new AlgorithmArchiver(),
                new UniEye.Base.Device.DeviceBox(new PortMap()),
                new DeviceController(),
                new UniScanM.Data.ProductionManager(PathSettings.Instance().Result),
                new UniEye.Base.Data.PowerManager());
        }
    }
}
