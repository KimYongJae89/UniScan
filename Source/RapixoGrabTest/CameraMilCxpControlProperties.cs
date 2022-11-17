using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapixoGrabTest
{
    public class CameraMilCxpControlProperties
    {
        public float ExposureMs { get; set; }
        public float AcquisitionLineRate { get; set; }
        public TriggerMode TriggerMode { get; set; }
        public bool TriggerRescalerMode { get; set; }
        public float TriggerRescalerRate { get; set; }
        public EScanDirectionType ScanDirectionType { get; set; }
        public ScanMode ScanMode { get; set; } 
        public EAnalogGain AnalogGain { get; set; }
        public ETDIStages TDIStages { get; set; }
        public bool ReverseX { get; set; }
        public int OffsetX { get; set; }

        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }

        internal void SetImageSize(int width, int height)
        {
            this.ImageWidth = width;
            this.ImageHeight = height;
        }
    }
}