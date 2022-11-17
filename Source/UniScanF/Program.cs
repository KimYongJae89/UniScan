using DynMvp.Base;
using System;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Util;
using UniEye.Base.Settings;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base;
using DynMvp.Data;
using DynMvp.Devices.FrameGrabber;
using UniScan.UI;
using DynMvp.Authentication;

namespace UniScan
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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ApplicationHelper.LoadStyleLibrary();
                ApplicationHelper.LoadSettings();

                SystemSettings.Instance().Load();

                ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

                ApplicationHelper.InitLogSystem();

                LogHelper.Info(LoggerType.Operation, "Start Up");
                //if (ApplicationHelper.CheckLicense() == false)
                //    return;

                ApplicationHelper.InitStringTable();
                ApplicationHelper.InitAuthentication();

                BuildSystemManager();
                LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                if (SystemManager.Instance().Setup() == true)
                {
                    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

                    LinkDevice();

                    UniEye.Base.UI.IMainForm mainForm = SystemManager.Instance().UiChanger.CreateMainForm();
                    SystemManager.Instance().MainForm = mainForm;
                    Application.Run((Form)mainForm);
                }

                SystemManager.Instance().Release();                

                Application.ExitThread();
                Environment.Exit(0);
            }
        }

        static void BuildSystemManager()
        {
            //CameraConfiguration cameraConfiguration = new CameraConfiguration();
            //CameraInfoGenTL cameraInfoGenTL = new CameraInfoGenTL(18000, 20000, 20000, 1, CameraInfoGenTL.ClientTypes.Master, CameraInfoGenTL.ScanDirectionTypes.Forward);
            //cameraConfiguration.AddCameraInfo(cameraInfoGenTL);

            //GrabberInfo grabberInfo = new GrabberInfo("TDILine", GrabberType.GenTL, 1);
            //grabberInfo.CameraConfiguration = cameraConfiguration;

            //GrabberInfoList grabberInfoList = MachineSettings.Instance().GrabberInfoList;
            //grabberInfoList.Add(grabberInfo);                        
                       
            SystemManager systemManager = new SystemManager();
            SystemManager.SetInstance(systemManager);

            systemManager.Init(new Data.ModelManager(), new UiChanger(), new DynMvp.Vision.AlgorithmArchiver(),
                new DeviceBox(new PortMap()), new DeviceController(), null);

        }

        static void LinkDevice()
        {
            PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;

            IoMachineIf machineIf = (IoMachineIf)SystemManager.Instance().DeviceBox.MachineIf;
            
            if(machineIf == null)
            {
                LogHelper.Debug(LoggerType.Device, "LinkDevice() : machineIf null");
                return;
            }
            machineIf.Initialize();

            //(IoMachineIf)machineIf.Init()
        }
    }
}
