using DynMvp.Base;
using DynMvp.Device.Device.Dio;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Device.Device.MotionController;
using DynMvp.Device.Device.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device
{
    public class ErrorSectionDevice : ErrorSubSections
    {
        public enum ESectionDevice
        {
            Grabber = 0,
            Serial = 100,
            DigitalIo = 200,
            DAQ = 300,
            Motion = 400,
            Comms = 500,

            Light = 1000,
            AxisHandler = 1100,
            Utility = 1200,
            //Air = 1200,
            //Vacuum = 1250,
            Cylinder = 1300,
            NEXT = 1400
        }

        public static ErrorSectionDevice Instance { get; } = new ErrorSectionDevice();

        public ErrorCodeGrabber Grabber { get; private set; }
        public ErrorCodeSerial Serial { get; private set; }
        public ErrorCodeDigitalIo DigitalIo { get; private set; }
        public ErrorCodes DAQ { get; private set; }
        public ErrorCodeMotion Motion { get; private set; }
        public ErrorCodes Comms { get; private set; }

        public ErrorCodes Light { get; private set; }
        public ErrorCodes AxisHandler { get; private set; }
        public ErrorCodeUtility Utility { get; private set; }
        //public ErrorCodes Air { get; private set; }
        //public ErrorCodes Vacuum { get; private set; }
        public ErrorCodes Cylinder { get; private set; }


        public ErrorSectionDevice() : base(ErrorSections.ESection.Device)
        {
            Grabber = new ErrorCodeGrabber(this);
            Serial = new ErrorCodeSerial(this);
            DigitalIo = new ErrorCodeDigitalIo(this);
            DAQ = new ErrorCodes(this, ESectionDevice.DAQ);
            Motion = new ErrorCodeMotion(this);
            Comms = new ErrorCodes(this, ESectionDevice.Comms);

            Light = new ErrorCodes(this, ESectionDevice.Light);
            AxisHandler = new ErrorCodes(this, ESectionDevice.AxisHandler);
            Utility = new ErrorCodeUtility(this);
            //Air = new ErrorCodes(this, ESectionDevice.Air);
            //Vacuum = new ErrorCodes(this, ESectionDevice.Vacuum);
            Cylinder = new ErrorCodes(this, ESectionDevice.Cylinder);
        }
    }

    //public class ErrorCodeMotion : ErrorCodes
    //{
    //    public enum EErrorCodeComms
    //    {
    //        CantFindMasterMotion = ECodeCommon.NEXT,
    //        InvalidMasterMotion,
    //    }
        
    //    public ErrorCodeMotion(ErrorSection errorSection)
    //        : base(errorSection, ErrorSectionDevice.ESectionDevice.Motion) { }
    //}

    public class ErrorCodeUtility : ErrorCodes
    {
        public enum EErrorCodeUtility
        {
            Air = 0,
            Vacuum = 50
        }

        public static ErrorCodeUtility Instance => ErrorSectionDevice.Instance.Utility;

        public ErrorCodes Air { get; private set; }
        public ErrorCodes Vacuum { get; private set; }

        public ErrorCodeUtility(ErrorSection errorSection)
            : base(errorSection, ErrorSectionDevice.ESectionDevice.Utility)
        {
            Air = new ErrorCodes(errorSection, (int)ErrorSectionDevice.ESectionDevice.Utility + (int)EErrorCodeUtility.Air, "Air");
            Vacuum = new ErrorCodes(errorSection, (int)ErrorSectionDevice.ESectionDevice.Utility + (int)EErrorCodeUtility.Vacuum, "Vacuum");
        }
    }
}
