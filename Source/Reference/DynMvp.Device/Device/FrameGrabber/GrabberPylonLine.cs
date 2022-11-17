using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Basler.Pylon;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class GrabberPylonLine : Grabber
    {
        public GrabberPylonLine(string name) : base(GrabberType.PylonLine, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public IList<Basler.Pylon.ICameraInfo> DeviceList { get; private set; }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraPylonLine(cameraInfo);
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            PylonCameraListForm form = new PylonCameraListForm();
            // HIKLineCameraListForm form = new HIKLineCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        private Basler.Pylon.ICameraInfo GetDevice(CameraInfoPylon cameraInfo)
        {
            if (DeviceList == null)
                return null;

            foreach (Basler.Pylon.ICameraInfo device in DeviceList)
            {
                string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];
                string ipAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];
                string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                if (!string.IsNullOrEmpty(deviceUserId) && cameraInfo.DeviceUserId == deviceUserId)
                    return device;
                else if (!string.IsNullOrEmpty(ipAddress) && cameraInfo.IpAddress == ipAddress)
                    return device;
                else if (!string.IsNullOrEmpty(serialNo) && cameraInfo.SerialNo == serialNo)
                    return device;
            }

            return null;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000"); // ??
            DeviceList = Basler.Pylon.CameraFinder.Enumerate();
            return true;
        }

        public void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylon) == false)
                return;

            CameraInfoPylon cameraInfoPylonLine = (CameraInfoPylon)cameraInfo;
            Basler.Pylon.ICameraInfo pylonDevice = GetDevice(cameraInfoPylonLine);
            if (pylonDevice == null)
            {
                string message = "Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}";
                string[] args = new string[] { cameraInfoPylonLine.DeviceUserId, cameraInfoPylonLine.IpAddress, cameraInfoPylonLine.SerialNo };
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, message, args, "");
            }

            cameraInfoPylonLine.DeviceIndex = (uint)cameraInfo.Index;//uint.Parse(pylonDevice[Basler.Pylon.CameraInfoKey.DeviceIdx]);
        }
    }
}
