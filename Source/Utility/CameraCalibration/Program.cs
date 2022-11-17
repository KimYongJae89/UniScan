using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Device;
using UniEye.Base.Util;
using UniEye.Base.UI.CameraCalibration;
using DynMvp.Data;
using UniEye.Base;
using System.Threading;
using DynMvp.UI.Touch;
using DynMvp.Vision;

namespace CameraCalibration
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
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

                try
                {
                    BuildSystemManager();

                    ApplicationHelper.LoadSettings();

                    SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                    simpleProgressForm.Show(() => SystemManager.Instance().DeviceBox.InitializeCameraAndLight());

                    CameraCalibrationForm cameraCalibrationForm = new CameraCalibrationForm();
                    cameraCalibrationForm.Initialize();
                    Application.Run(cameraCalibrationForm);

                    SystemManager.Instance().DeviceBox.Release();
                }
                finally { }

            }
        }


        public static void BuildSystemManager()
        {
            SystemManager systemManager = new SystemManager();
            SystemManager.SetInstance(systemManager);

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
            systemManager.Init(null, null, null, new DeviceBox(new PortMap()), new DeviceController(), null, null);
        }

    }
}
