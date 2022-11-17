using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using DynMvp.Base;
using Matrox.MatroxImagingLibrary;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraMil : Camera
    {
        public MilSystem MilSystem { get; private set; }
        public MIL_ID DigitizerId { get; private set; }

        const int BufferPoolCount = 10;
        MIL_ID whiteKernel = MIL.M_NULL;

        GCHandle thisHandle;
        MIL_DIG_HOOK_FUNCTION_PTR frameTransferEndPtr = null;
        MIL_DIG_HOOK_FUNCTION_PTR processingFunctionPtr = null;

        MIL_ID[] grabImageBuffer = new MIL_ID[BufferPoolCount];
        MIL_ID sourceImage = MIL.M_NULL;
        MIL_ID grabbedImage = MIL.M_NULL;

        //internal CameraMil(CameraInfo cameraInfo) : base(cameraInfo) { }
        public CameraMil(CameraInfo cameraInfo) : base(cameraInfo) { }

        public override void Initialize(bool calibrationMode)
        {
            base.Initialize(calibrationMode);
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;

            this.MilSystem = GrabberMil.GetMilSystem(cameraInfoMil.SystemType, cameraInfoMil.SystemNum);

            if (this.MilSystem == null)
            {
                LogHelper.Error(LoggerType.Error, "MilSystem is empty. Skip create the digitizer.");
                return;
            }

            string dcfFileName = cameraInfoMil.GetDcfFile(calibrationMode);
            this.DigitizerId = MIL.MdigAlloc(this.MilSystem.SystemId, cameraInfoMil.DigitizerNum, dcfFileName, MIL.M_DEFAULT, MIL.M_NULL);
            if (this.DigitizerId == null)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Digitizer Allocation is Failed.{0}, {1}, {2}, {3}",
                    cameraInfoMil.SystemType.ToString(), this.MilSystem.SystemId, cameraInfoMil.DigitizerNum, cameraInfoMil.CameraType.ToString()));
                return;
            }

            MIL.MdigControl(this.DigitizerId, MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS);
            MIL.MdigControl(this.DigitizerId, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE);

            thisHandle = GCHandle.Alloc(this);

            //frameExposureEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameExposureEnd);
            //MIL.MdigHookFunction(this.DigitizerId , MIL.M_GRAB_FRAME_START, frameExposureEndPtr, GCHandle.ToIntPtr(thisHandle));

            frameTransferEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameTransferEnd);
            MIL.MdigHookFunction(this.DigitizerId, MIL.M_GRAB_END, frameTransferEndPtr, GCHandle.ToIntPtr(thisHandle));

            MIL_INT tempValue = 0;
            MIL_INT width = 0;
            MIL_INT height = 0;

            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_X, ref width);
            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_Y, ref height);

            ImageSize = new Size((int)width, (int)height);
            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_BAND, ref tempValue);
            NumOfBand = (int)tempValue;

            ImagePitch = (int)width * NumOfBand;

            if (NumOfBand == 1)
            {
                for (int i = 0; i < BufferPoolCount; i++)
                {
                    grabImageBuffer[i] = MIL.MbufAlloc2d(this.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                }
            }
            else
            {
                for (int i = 0; i < BufferPoolCount; i++)
                {
                    grabImageBuffer[i] = MIL.MbufAllocColor(this.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                }
            }

            IntPtr hostAddress = IntPtr.Zero;
            var ret = MIL.MbufInquire(grabImageBuffer[0], MIL.M_HOST_ADDRESS, hostAddress);
            var ret2 = MIL.MbufInquire(grabImageBuffer[1], MIL.M_HOST_ADDRESS, MIL.M_NULL);

            grabbedImage = grabImageBuffer[0];

            if (cameraInfoMil.BayerCamera == true)
            {
                sourceImage = MIL.MbufAllocColor(this.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                MIL.MbufPut(whiteKernel, cameraInfoMil.WhiteBalanceCoefficient);
                BayerType = cameraInfoMil.BayerType;
            }
            else
            {
                sourceImage = grabImageBuffer[0];
            }
        }

        public override void SetTriggerDelay(int exposureTimeUs)
        {
            MIL.MdigControl(this.DigitizerId, MIL.M_GRAB_TRIGGER_DELAY, exposureTimeUs * 1000);
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Grab, "CameraMil - Release Mil System");
            MIL.MdigFree(this.DigitizerId);

            MIL.MbufFree(grabImageBuffer[0]);
            MIL.MbufFree(grabImageBuffer[1]);

            if (whiteKernel != MIL.M_NULL)
            {
                MIL.MbufFree(whiteKernel);
                MIL.MbufFree(sourceImage);
            }
        }

        public MIL_INT FrameExposureEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - FrameExposureEnd");

            ExposureDoneCallback();

            return MIL.M_NULL;
        }

        public MIL_INT FrameTransferEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin FrameTransferEnd");

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            grabbedImage = currentImageId;

            ImageGrabbedCallback(grabbedImage);

            MIL.MbufSave("D:\\Data.bmp", grabbedImage);

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End FrameTransferEnd");

            return MIL.M_NULL;
        }

        // CameraInfoMil 로 이사감.
        //private string GetDcfFile(CameraType cameraType)
        //{
        //    switch (cameraType)
        //    {
        //        case CameraType.PrimeTech_PXCB120VTH:
        //            return "MIL90_PXCB120VTH1_1tap_HW.dcf";
        //        case CameraType.Crevis_MC_D500B:
        //            return "MIL10_SOL_5MCREVIS_2TAP_HWTRIG.dcf";
        //        case CameraType.PrimeTech_PXCB16QWTPM:
        //            return "HWTRIG.dcf";
        //        case CameraType.PrimeTech_PXCB16QWTPMCOMPACT:
        //            return "HWTRIG2.dcf";
        //        case CameraType.HV_B550CTRG1:
        //            return "HV_B550C_TRG1.dcf";
        //        case CameraType.HV_B550CTRG2:
        //            return "HV_B550C_TRG2.dcf";
        //        case CameraType.EliixaPlus16K:
        //            return "Eliixa16K.dcf";
        //        case CameraType.UNiiQA:
        //            return "UNiiQA.dcf";
        //    }

        //    return "";
        //}

        static MIL_INT ProcessingFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            if (IntPtr.Zero.Equals(HookDataPtr) == true)
                return MIL.M_NULL;

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

            // get a reference to the DigHookUserData object
            CameraMil cameraMil = hUserData.Target as CameraMil;
            cameraMil.grabbedImage = currentImageId;

            cameraMil.ImageGrabbedCallback(IntPtr.Zero);

            return MIL.M_NULL;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin SetTriggerMode");

            base.SetTriggerMode(triggerMode, triggerType);

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_SOFTWARE);
                MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_SOFTWARE + MIL.M_TIMER1, MIL.M_ACTIVATE);
            }
            else
            {
                //if (triggerChannel == 0)
                    MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_AUX_IO6);
                //else
                //    MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_AUX_IO5);

                if (triggerType == TriggerType.RisingEdge)
                    MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_ACTIVATION + MIL.M_TIMER1, MIL.M_EDGE_RISING);
                else
                    MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_TRIGGER_ACTIVATION + MIL.M_TIMER1, MIL.M_EDGE_FALLING);
            }

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End SetTriggerMode");
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (grabbedImage != null)
            {
                if (NumOfBand == 1)
                {
                    if (BayerCamera == true)
                    {
                        MIL.MbufBayer(grabbedImage, sourceImage, whiteKernel, (long)BayerType);
                        return CreateGrabbedImage(true);
                    }
                    else
                    {
                        sourceImage = grabbedImage;
                        return CreateGrabbedImage(false);
                    }
                }
                else if (NumOfBand == 3)
                {
                    sourceImage = grabbedImage;
                    return CreateGrabbedImage(true);
                }
            }

            return null;
        }

        public ImageD CreateGrabbedImage(bool colorImage)
        {
            if (sourceImage == MIL.M_NULL)
                return null;

            IntPtr hostAddress = IntPtr.Zero;
            var addr = MIL.MbufInquire(sourceImage, MIL.M_HOST_ADDRESS, hostAddress);

            var pitch = MIL.MbufInquire(sourceImage, MIL.M_PITCH, hostAddress);
            byte[] UserArrayPtr = new byte[ImageSize.Width * ImageSize.Height];
            MIL.MbufGet(sourceImage, UserArrayPtr);

            Image2D image2d = new Image2D();
            image2d.Initialize(ImageSize.Width, ImageSize.Height, colorImage ? 3 : 1, ImageSize.Width, UserArrayPtr);

            return image2d;
        }

        public bool CopyGrayImage(ImageD image)
        {
            if (sourceImage == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height];

            MIL.MbufGet(sourceImage, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public bool CopyColorImage(ImageD image)
        {
            if (sourceImage == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height * 3];

            MIL.MbufGetColor(sourceImage, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabOnce");

            if (SetupGrab() == false)
                return;

            this.isStopped.Reset();
            remainGrabCount = 1;
            MIL.MdigGrab(this.DigitizerId, grabbedImage);
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabContinuous");

            if (SetupGrab() == false)
                return;

            this.remainGrabCount = grabCount;

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigGrabContinuous(this.DigitizerId, grabbedImage);
            }
            else if (grabCount < 0)
            {
                if (processingFunctionPtr != null)
                    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, BufferPoolCount, MIL.M_START, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
            else if (grabCount > 0)
            {
                if (processingFunctionPtr != null)
                    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

                int wishCount = grabCount < BufferPoolCount ? grabCount : BufferPoolCount;

                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, BufferPoolCount, MIL.M_SEQUENCE + MIL.M_COUNT(wishCount), MIL.M_ASYNCHRONOUS, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigHalt(this.DigitizerId);
            }
            else
            {
                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, 2, MIL.M_STOP, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
            Thread.Sleep(50);
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            MIL.MdigControl(this.DigitizerId, MIL.M_TIMER_DURATION + MIL.M_TIMER1, exposureTimeMs * 1000);
            return true;
        }

        public override List<ImageD> GetImageBufferList()
        {
            throw new NotImplementedException();
        }

        public override int GetImageBufferCount()
        {
            throw new NotImplementedException();
        }

        public override float GetDeviceExposureMs()
        {
            throw new NotImplementedException();
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            throw new NotImplementedException();
        }

        public override float GetAcquisitionLineRate()
        {
            throw new NotImplementedException();
        }
    }
}
