using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System.ComponentModel;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class GrabberVirtual : Grabber
    {
        public GrabberVirtual(string name) : base(GrabberType.Virtual, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "Virtual Camera Manager Created");
        }

        public GrabberVirtual(GrabberType grabberType, string name) : base(grabberType, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "Virtual Camera Manager Created");
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            if (cameraInfo.IsLineScan)
                return new CameraVirtualMS(cameraInfo);
            else
                return new CameraVirtual(cameraInfo);
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            VirtualCameraListForm form = new VirtualCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            return true;
        }

        public override void Release()
        {
            base.Release();
        }
    }
}
