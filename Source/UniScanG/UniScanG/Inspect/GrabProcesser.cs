using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanG.Inspect
{
    public delegate void StartInspectionDelegate(ImageDevice imageDevice, IntPtr ptr);
    public abstract class GrabProcesser : IDisposable
    {
        //protected ManualResetEvent isFullImageGrabbed = new ManualResetEvent(false);

        public StartInspectionDelegate StartInspectionDelegate;

        public virtual int GetBufferCount() { return 0; }
        public abstract bool IsDisposable();
        public abstract void Dispose();

        public void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD imageD = (ImageD)imageDevice.GetGrabbedImage(ptr);
            ImageGrabbed(imageD);
        }

        public abstract void ImageGrabbed(ImageD imageD);
        public abstract void Start();
        public abstract void Stop();

        //public bool WaitFullImageGrabbed(int waitTimeMs = -1)
        //{
        //    return isFullImageGrabbed.WaitOne(waitTimeMs);
        //}

        public abstract void SetDebugMode(bool mode);
        public abstract void SetDebugMode(string testModePath);
    }
}