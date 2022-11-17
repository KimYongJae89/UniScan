using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.FrameGrabber.UI;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoHIKLine : CameraInfoPylon2
    {
        public enum ETriggerSource
        {
            SW = MvCamCtrl.NET.MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE,
            Line0 = MvCamCtrl.NET.MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0,
            Line2 = MvCamCtrl.NET.MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE2
        }
        public CameraInfoHIKLine()
        {
            GrabberType = GrabberType.HIKLine;
            this.isLineScan = true;
        }

        public ETriggerSource TriggerSource { get; set; } = ETriggerSource.SW;

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);
            TriggerSource = (ETriggerSource)int.Parse(XmlHelper.GetValue(cameraElement, "TriggerSource", ((int)ETriggerSource.SW).ToString()));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);
            XmlHelper.SetValue(cameraElement, "TriggerSource", ((uint)TriggerSource).ToString());
        }
    }

    public class GrabberHIKLine : Grabber
    {
        private MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();

        public GrabberHIKLine(string name) : base(GrabberType.HIKLine, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "HIKRobot Device Handler Created");
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraHIKLine(cameraInfo);
        }


        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new HIKLineCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            if (MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_stDeviceList) != 0)
            {
                return false;
            }

            return true;
        }

        public override void Release()
        {
            foreach (Device device in DeviceManager.Instance().DeviceList)
            {
                if (device is CameraHIKLine)
                {
                    device.Release();
                }
            }

            base.Release();
        }
       /*
        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoHIKLine) == false)
            {
                return;
            }

            var cameraInfoHIK = (CameraInfoHIKLine)cameraInfo;
            try
            {
                var device =
                (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[cameraInfo.Index], typeof(MyCamera.MV_CC_DEVICE_INFO));

                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
                    {
                        var device2 =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));

                        var info1 = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device2.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                        if (gigeInfo.chSerialNumber == info1.chSerialNumber)
                        {
                            cameraInfoHIK.DeviceIndex = Convert.ToUInt32(i);
                            break;
                        }
                    }
                }
                else
                {
                    var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));

                    for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
                    {
                        var device2 =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));

                        var info2 = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device2.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                        if (usbInfo.chSerialNumber == info2.chSerialNumber)
                        {
                            cameraInfoHIK.DeviceIndex = Convert.ToUInt32(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                CameraInfoHIKLine cam = cameraInfoHIK;
                string message = $"Can't find camera. Device User Id : {cam.DeviceUserId} / Model Name : {cam.ModelName} / SerialNo : {cam.SerialNo}";
                LogHelper.Error(message);
                cameraInfoHIK.DeviceIndex = 0;
                cameraInfoHIK.Enabled = false;
                throw new Exception(message);
            }
        }
        */

    }
}
