using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;

namespace DynMvp.Devices.UI
{
    public partial class CameraView : PictureBox
    {
        private ImageDevice linkedImageDevice = null;
        public ImageDevice LinkedImageDevice
        {
            get { return linkedImageDevice; }
            set { linkedImageDevice = value; }
        }

        private bool lockImageUpdate = false;
        public bool LockImageUpdate
        {
            get { return lockImageUpdate; }
            set { lockImageUpdate = value; }
        }

        public CameraView()
        {
            InitializeComponent();
        }

        public void SetImageDevice(ImageDevice imageDevice)
        {
            if (imageDevice != null)
            {
                linkedImageDevice = imageDevice;

                if (linkedImageDevice != null)
                    linkedImageDevice.ImageGrabbed += ImageGrabbed;
            }
            else
            {
                linkedImageDevice.ImageGrabbed -= ImageGrabbed;
                linkedImageDevice = null;
            }
        }

        public void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (lockImageUpdate == true)
                return;

            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Start UpdateImage Invoke");
                Invoke(new ImageDeviceEventDelegate(ImageGrabbed), imageDevice, ptr);
                return;
            }

            LogHelper.Debug(LoggerType.Grab, "Start UpdateImage");

            Image2D image2d = (Image2D)imageDevice.GetGrabbedImage(IntPtr.Zero);

            LogHelper.Debug(LoggerType.Grab, "Set Bitmp");
            Image = image2d.ToBitmap();
        }

        public void EnableMeasureMode(float scaleX, float scaleY)
        {

        }
    }
}
