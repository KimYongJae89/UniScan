using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using UniScanG.Gravure.Data;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Device;

namespace UniScanG.Module.Monitor.Inspect
{
    public class InspectOptionComparer : IComparer<InspectOption>
    {
        public int Compare(InspectOption x, InspectOption y)
        {
            //int comp = x.RollPos.CompareTo(y.RollPos);
            //if (comp != 0)
            //    return comp;

            return x.Ptr.ToInt64().CompareTo(y.Ptr.ToInt64());
        }
    }

    public class InspectOption : UniEye.Base.Inspect.InspectOption, IComparable
    {
        public int CameraNo => this.cameraNo;
        int cameraNo = -1;

        public int ImageIndex => this.imageIndex;
        int imageIndex = -1;

        public float RollPos => this.rollPos; 
        float rollPos = 0;

        public TeachData TeachData => this.teachData;
        TeachData teachData;

        public InspectOption(ImageDevice imageDevice, IntPtr ptr) : base(imageDevice, ptr)
        {
            this.cameraNo = imageDevice.Index;
            this.imageIndex = (int)ptr;
            this.rollPos = ((DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor.MachineIfData.GET_PRESENT_POSITION;
            this.teachData = ((Model)SystemManager.Instance().CurrentModel).TeachData.Clone();
        }

        public int CompareTo(object obj)
        {
            InspectOption y = obj as InspectOption;
            if (y == null)
                throw new NotImplementedException();

            return this.Ptr.ToInt64().CompareTo(y.Ptr.ToInt64());
        }

        public override int GetHashCode()
        {
            return this.Ptr.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            InspectOption y = obj as InspectOption;
            if (y == null)
                return false;

            return (this.cameraNo == y.cameraNo) && (this.Ptr == y.Ptr);
        }
    }
}
