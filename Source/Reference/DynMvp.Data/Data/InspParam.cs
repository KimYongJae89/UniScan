using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.Devices;
using DynMvp.Vision;
using System.Drawing.Imaging;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.InspData;
using DynMvp.UI;

namespace DynMvp.Data
{
    public class InspParam
    {
        private DeviceImageSet deviceImageSet;
        public DeviceImageSet DeviceImageSet
        {
            get { return deviceImageSet; }
        }

        private PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            get { return positionAligner; }
            set { positionAligner = value; }
        }

        private SizeF fiducialProbeOffset;
        public SizeF FiducialProbeOffset
        {
            get { return fiducialProbeOffset; }
            set { fiducialProbeOffset = value; }
        }

        private float fiducialProbeAngle;
        public float FiducialProbeAngle
        {
            get { return fiducialProbeAngle; }
            set { fiducialProbeAngle = value; }
        }

        private RotatedRect fiducialProbeRect;
        public RotatedRect FiducialProbeRect
        {
            get { return fiducialProbeRect; }
            set { fiducialProbeRect = value; }
        }

        private bool saveDebugImage;
        public bool SaveDebugImage
        {
            get { return saveDebugImage; }
        }

        private bool saveProbeImage;
        public bool SaveProbeImage
        {
            get { return saveProbeImage; }
        }

        private bool saveTargetImage;
        public bool SaveTargetImage
        {
            get { return saveTargetImage; }
        }

        private bool saveTargetGroupImage;
        public bool SaveTargetGroupImage
        {
            get { return saveTargetGroupImage; }
        }

        private ImageFormat targetGroupImageFormat;
        public ImageFormat TargetGroupImageFormat
        {
            get { return targetGroupImageFormat; }
        }

        private InspectionResult inspectionResult;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        private List<Probe> selectedProbeList = new List<Probe>();
        public List<Probe> SelectedProbeList
        {
            get { return selectedProbeList; }
            set { selectedProbeList = value; }
        }

        private float sensorDistance;
        public float SensorDistance
        {
            get { return sensorDistance; }
        }

        private Calibration cameraCalibration;
        public Calibration CameraCalibration
        {
            get { return cameraCalibration; }
            set { cameraCalibration = value; }
        }

        DigitalIoHandler digitalIoHandler;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
            set { digitalIoHandler = value; }
        }

        float pixelRes3d;
        public float PixelRes3d
        {
            get { return pixelRes3d; }
            set { pixelRes3d = value; }
        }

        bool teachMode;
        public bool TeachMode
        {
            get { return teachMode; }
            set { teachMode = value; }
        }

        int clipExtendSize;
        public int ClipExtendSize
        {
            get { return clipExtendSize; }
        }

        public TargetInspectedDelegate TargetInspected = null;

        public InspParam(InspectionResult inspectionResult)
        {
            teachMode = false;
            this.inspectionResult = inspectionResult;
        }

        public InspParam(int sequenceNo, DeviceImageSet deviceImageSet, bool saveDebugImage, 
            bool saveProbeImage, bool saveTargetImage, bool saveTargetGroupImage, 
            ImageFormat targetGroupImageFormat, int clipExtendSize)
        {
            teachMode = false;
            this.deviceImageSet = deviceImageSet;
//#if DEBUG
            //this.saveDebugImage = true;
//#else
            this.saveDebugImage = saveDebugImage;
//#endif
//            this.sequenceNo = sequenceNo;
            this.saveProbeImage = saveProbeImage;
            this.saveTargetImage = saveTargetImage;
            this.saveTargetGroupImage = saveTargetGroupImage;
            this.targetGroupImageFormat = targetGroupImageFormat;
            this.clipExtendSize = clipExtendSize;
        }

        public void SetDeviceParam(DigitalIoHandler digitalIoHandler, float sensorDistance)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.sensorDistance = sensorDistance;
        }
    }
}
