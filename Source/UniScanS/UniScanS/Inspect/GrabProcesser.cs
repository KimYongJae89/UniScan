using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Inspect
{
    public delegate void StartInspectionDelegate(ImageDevice imageDevice, IntPtr ptr);
    public abstract class GrabProcesser
    {
        public virtual int GetBufferCount() { return 0; }
        public abstract IntPtr GetGrabbedImagePtr();
        public StartInspectionDelegate StartInspectionDelegate;
        public abstract void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr);
        public abstract void Start();
        public abstract void Stop();
        public abstract void EnterPause();
        public abstract void ExitPause();
    }
}