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
     // 
    public class CameraInfoPylon2 : CameraInfo
    {
        public CameraInfoPylon2()
        {
            base.GrabberType = GrabberType.Pylon2;
        }

        public CameraInfoPylon2(string deviceUserId, string ipAddress, string serialNo)
        {
            IpAddress = ipAddress;
            SerialNo = serialNo;
            DeviceUserId = deviceUserId;
            DeviceIndex = 0;
            ModelName = "";
            base.GrabberType = GrabberType.Pylon2;
        }


        public string DeviceUserId { get; set; } = "";

        public string IpAddress { get; set; } = "";

        public string SerialNo { get; set; } = "";

        public uint DeviceIndex { get; set; } = 0;

        public string ModelName { get; set; } = "";


        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            DeviceUserId = XmlHelper.GetValue(cameraElement, "DeviceUserId", "");
            IpAddress = XmlHelper.GetValue(cameraElement, "IpAddress", "");
            SerialNo = XmlHelper.GetValue(cameraElement, "SerialNo", "");
            ModelName = XmlHelper.GetValue(cameraElement, "ModelName", "");
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "DeviceUserId", DeviceUserId);
            XmlHelper.SetValue(cameraElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(cameraElement, "SerialNo", SerialNo);
            XmlHelper.SetValue(cameraElement, "ModelName", ModelName);
        }
    }

    public class GrabberPylon2 : Grabber
    {
        public GrabberPylon2(string name) : base(GrabberType.Pylon2, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public IList<Basler.Pylon.ICameraInfo> DeviceList { get; private set; }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraPylon2(cameraInfo);
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            PylonCameraListForm2 form = new PylonCameraListForm2();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        private Basler.Pylon.ICameraInfo GetDevice(CameraInfoPylon2 cameraInfo)
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

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "3000"); // ??
            DeviceList = Basler.Pylon.CameraFinder.Enumerate();
            return true;
        }

        public void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylon2) == false)
                return;

            CameraInfoPylon2 cameraInfoPylon2 = (CameraInfoPylon2)cameraInfo;
            Basler.Pylon.ICameraInfo pylonDevice = GetDevice(cameraInfoPylon2);
            if (pylonDevice == null)
            {
                string message = "Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}";
                string[] args = new string[] { cameraInfoPylon2.DeviceUserId, cameraInfoPylon2.IpAddress, cameraInfoPylon2.SerialNo };
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, message, args, "");
            }

            cameraInfoPylon2.DeviceIndex = (uint)cameraInfo.Index;//uint.Parse(pylonDevice[Basler.Pylon.CameraInfoKey.DeviceIdx]);
        }


    }
}
