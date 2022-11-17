using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.Inspection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanS.Inspect;
using UniScanS.Screen.Vision;
using UniScanS.Vision;

namespace UniScanS.Screen.Inspect
{
    public class GrabProcesserS : GrabProcesser
    {
        bool onStopGrab;
        
        ImageDevice imageDevice = null;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
        }

        List<IntPtr> ptrList = new List<IntPtr>();

        public GrabProcesserS(ImageDevice imageDevice)
        {
            this.imageDevice = imageDevice;
        }

        private void Dispose()
        {
            onStopGrab = true;
            this.ptrList.Clear();
        }

        public override void Start()
        {
            lock (this)
            {
                Dispose();
                onStopGrab = false;
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(ImageGrabbed);
            }
        }

        public override void Stop()
        {
            lock (this)
            {
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.RemoveImageGrabbed(ImageGrabbed);
                Dispose();
            }
        }

        public void AddGrabbedImagePtr(IntPtr ptr)
        {
            lock (ptrList)
            {
                if (ptrList.Find(p => p == ptr) != null)
                    ptrList.Remove(ptr);

                ptrList.Add(ptr);
            }
        }

        public override IntPtr GetGrabbedImagePtr()
        {
            lock (ptrList)
            {
                if (ptrList.Count != 0)
                {
                    IntPtr ptr = ptrList.First();
                    ptrList.Remove(ptr);

                    return ptr;
                }
            }

            return IntPtr.Zero;
        }
        
        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (this.imageDevice != imageDevice)
                return;

            if (onStopGrab == true)
                return;

            Image2D image = (Image2D)imageDevice.GetGrabbedImage(ptr);

            if (ptr == IntPtr.Zero)
            {
                image.ConvertFromData();
                image.SetData(null);
            }   

            AddGrabbedImagePtr(image.DataPtr);
            if (InspectorSetting.Instance().IsFiducial == true)
                Task.Run(() => StartInspectionDelegate(imageDevice, ptr));

            SystemState.Instance().SetInspectState(InspectState.Run);
        }

        public override void EnterPause()
        {
            lock (this)
                onStopGrab = true;
        }

        public override void ExitPause()
        {
            lock (this)
                onStopGrab = false;
        }
    }
}
