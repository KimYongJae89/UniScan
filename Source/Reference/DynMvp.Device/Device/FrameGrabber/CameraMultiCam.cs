using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using Euresys.MultiCam;

using DynMvp.Base;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DynMvp.Devices.FrameGrabber
{
    public enum EuresysBoardType
    {
        GrabLink_Value, GrabLink_Base, GrabLink_DualBase, GrabLink_Full, Picolo
    }

    public enum EuresysImagingType
    {
        AREA, LINE //LINE 확인 O, AREA는 확인 X
    }

    class McSurface
    {
        uint handle;
        byte[] surfaceData;
        GCHandle pinnedArray;
        IntPtr dataPtr;

        public void Create(int width, int height, int pitch)
        {
            MC.Create(MC.DEFAULT_SURFACE_HANDLE, out handle);
            MC.SetParam(handle, "SurfaceSize", width * height);
            MC.SetParam(handle, "SurfacePitch", pitch);

            surfaceData = new byte[pitch * height];
            GCHandle pinnedArray = GCHandle.Alloc(surfaceData, GCHandleType.Pinned);
            dataPtr = pinnedArray.AddrOfPinnedObject();

            MC.SetParam(handle, "SurfaceAddr", dataPtr);
        }

        public void Release()
        {
            pinnedArray.Free();
        }
    }

    public class CameraMultiCam : Camera
    {
        //EuresysBoardType boardType;

        // The MultiCam object that controls the acquisition
        UInt32 channel;
        // The MultiCam object that contains the acquired buffer
        private UInt32 currentSurface;

        MC.CALLBACK multiCamCallback;

        int grabIndex = 0;
        // The object that will contain the acquired image
        private Image2D[] grabbedImage = null;

        // The Mutex object that will protect image objects during processing
        private Mutex imageMutex = new Mutex();

        private object grabLock = new object();

        public EuresysImagingType imagingType;
        public EuresysImagingType ImagingType
        {
            get { return imagingType; }
        }

        internal CameraMultiCam(CameraInfo cameraInfo) : base(cameraInfo) { }

        public override void Initialize(bool calibrationMode)
        {
            base.Initialize(calibrationMode);
            CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)cameraInfo;

            try
            {
                SetBoardTopology();

                // Create a channel and associate it with the first connector on the first board
                MC.Create("CHANNEL", out channel);
                MC.SetParam(channel, "DriverIndex", cameraInfoMultiCam.BoardId);

                // For all GrabLink boards exect Grablink Expert 2 and Dualbase
                string connectorString = GetConnectorString(cameraInfoMultiCam.ConnectorId);
                if (connectorString == "")
                {
                    throw new CameraInitializeFailException(String.Format("Connector String is not specified. BoardType = {0} / ConnectorId = {1}", cameraInfoMultiCam.BoardType.ToString(), cameraInfoMultiCam.ConnectorId));
                }

                MC.SetParam(channel, "Connector", connectorString);

                string camFile = GetCamFile(cameraInfoMultiCam.CameraType);
                if (camFile == "")
                {
                    throw new CameraInitializeFailException("Cam File is not specified");
                }
                MC.SetParam(channel, "CamFile", camFile);

                string imagingType = null;

                MC.GetParam(channel, "Imaging", out imagingType);
                this.imagingType = (EuresysImagingType)Enum.Parse(typeof(EuresysImagingType), imagingType);

                InitializeDevice(cameraInfoMultiCam);

                // Register the callback function
                multiCamCallback = new MC.CALLBACK(MultiCamCallback);
                MC.RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_END_EXPOSURE, "ON");
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_FILLED, "ON");
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");

                Int32 width, height, bufferPitch;
                MC.GetParam(channel, "ImageSizeX", out width);
                MC.GetParam(channel, "ImageSizeY", out height);
                MC.GetParam(channel, "BufferPitch", out bufferPitch);

                Int32 planeCount = 1;
                MC.GetParam(channel, "ImagePlaneCount", out planeCount);

                ImageSize = new Size(Math.Min(cameraInfo.Width, width), Math.Min(cameraInfo.Height, height));
                NumOfBand = planeCount;
                ImagePitch = bufferPitch;

                MC.SetParam(channel, "SurfaceCount", cameraInfoMultiCam.SurfaceNum);

                grabbedImage = new Image2D[cameraInfoMultiCam.SurfaceNum];

                for (int i = 0; i < cameraInfoMultiCam.SurfaceNum; i++)
                {
                    grabbedImage[i] = new Image2D();
                    if (this.cameraInfo.UseNativeBuffering)
                    {
                        grabbedImage[i].Initialize(width, height, planeCount, bufferPitch, IntPtr.Zero);
                    }
                    else
                    {
                        grabbedImage[i].Initialize(width, height, planeCount, bufferPitch);
                    }
                }

                MC.SetParam(channel, "ChannelState", "ACTIVE");
                MC.SetParam(channel, "ChannelState", "IDLE");
            }

            catch (Euresys.MultiCamException exc)
            {
                throw new CameraInitializeFailException("MultiCam Exception : " + exc.Message);
            }
        }

        private void InitializeDevice(CameraInfoMultiCam cameraInfoMultiCam)
        {
            SetColorFormat(cameraInfoMultiCam.PixelFormat != PixelFormat.Format8bppIndexed);

            switch (this.imagingType)
            {
                case EuresysImagingType.AREA:
                    // Choose the camera expose duration
                    MC.SetParam(channel, "Expose_us", 20);
                    // Choose the pixel color format


                    //Set the acquisition mode to Snapshot
                    MC.SetParam(channel, "AcquisitionMode", "SNAPSHOT");
                    // Choose the way the first acquisition is triggered
                    MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                    // Choose the triggering mode for subsequent acquisitions
                    MC.SetParam(channel, "NextTrigMode", "REPEAT");
                    // Choose the number of images to acquire
                    MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                    break;
                case EuresysImagingType.LINE:
                    MC.SetParam(channel, "AcquisitionMode", "LONGPAGE");
                    MC.SetParam(channel, "TrigMode", "HARD");

                    MC.SetParam(channel, "LineCaptureMode", "ALL");
                    MC.SetParam(channel, "LineTrigCtl", "DIFF");
                    MC.SetParam(channel, "LineTrigEdge", "RISING_A");
                    MC.SetParam(channel, "LineTrigFilter", "OFF");
                    MC.SetParam(channel, "LineTrigLine", "DIN1");

                    MC.SetParam(channel, "RateDivisionFactor", "1");
                    MC.SetParam(channel, "LineRateMode", "PULSE");
                    MC.SetParam(channel, "PageLength_Ln", cameraInfoMultiCam.Height);
                    MC.SetParam(channel, "SeqLength_Ln", MC.INDETERMINATE);

                    MC.SetParam(channel, "EndTrigMode", "HARD");
                    MC.SetParam(channel, "EndTrigCtl", "ISO");
                    MC.SetParam(channel, "EndTrigEdge", "GOLOW");
                    MC.SetParam(channel, "EndTrigEffect", "PRECEDINGLINE");
                    MC.SetParam(channel, "EndTrigFilter", "OFF");
                    MC.SetParam(channel, "EndTrigLine", "IIN1");
                    MC.SetParam(channel, "EndTrigFollowingLinesCount", "ONELINE");

                    MC.SetParam(channel, "TrigCtl", "ISO");
                    MC.SetParam(channel, "TrigDelay_Pls", "0");
                    MC.SetParam(channel, "TrigEdge", "GOHIGH");
                    MC.SetParam(channel, "TrigFilter", "OFF");
                    MC.SetParam(channel, "TrigLine", "IIN1");

                    MC.SetParam(channel, "EndPageDelay_Ln", "0");
                    MC.SetParam(channel, "PageDelay_Ln", "0");

                    break;
            }
        }

        private void SetBoardTopology()
        {
            CameraInfoMultiCam cameraInfoMultiCam = this.cameraInfo as CameraInfoMultiCam;
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.Picolo)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "1_01_2");
            }
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.GrabLink_Full)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "MONO_DECA");
            }
        }

        private void SetColorFormat(bool color)
        {
            CameraInfoMultiCam cameraInfoMultiCam = this.cameraInfo as CameraInfoMultiCam;
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.Picolo)
            {
                if (color)
                    MC.SetParam(channel, "ColorFormat", "RGB24");
                else
                    MC.SetParam(channel, "ColorFormat", "Y8");
            }
            else
            {
                if (color)
                    MC.SetParam(channel, "ColorFormat", "RGB24");
                else
                    MC.SetParam(channel, "ColorFormat", "Y8");
            }
        }

        private string GetCamFile(CameraType cameraType)
        {
            CameraInfoMultiCam cameraInfoMultiCam = this.cameraInfo as CameraInfoMultiCam;
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.Picolo)
            {
                return "NTSC";
            }
            else
            {
                switch (cameraType)
                {
                    case CameraType.Jai_GO_5000:
                        return "GO-5000M-PMCL_3T8_RG(2560, 2048)_TRG_DN";
                    case CameraType.RaL12288_66km:
                        return "raL12288-66km_L12288RG";
                }
            }

            return "";
        }

        private string GetConnectorString(uint connectorId)
        {
            CameraInfoMultiCam cameraInfoMultiCam = this.cameraInfo as CameraInfoMultiCam;
            switch (cameraInfoMultiCam.BoardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_Full:
                    return "M";
                case EuresysBoardType.GrabLink_DualBase:
                    if (connectorId == 0)
                        return "A";
                    else
                        return "B";
                case EuresysBoardType.Picolo:
                    return String.Format("VID{0}", connectorId + 1);
            }

            return "";
        }

        private string GetTriggerLineName()
        {
            CameraInfoMultiCam cameraInfoMultiCam = this.cameraInfo as CameraInfoMultiCam;
            switch (cameraInfoMultiCam.BoardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_DualBase:
                case EuresysBoardType.GrabLink_Full:
                    string[] grabLinkTriggerNameList = new string[] { "NOM", "DIN1", "DIN2", "IIN1", "IIN2", "IIN3", "IIN4" };
                    return grabLinkTriggerNameList[cameraInfoMultiCam.TriggerChannel];
            }

            return "NOM";
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            remainGrabCount = 1;

            base.SetTriggerMode(triggerMode, triggerType);
            try
            {
                if (triggerMode == TriggerMode.Software)
                {
                    // Choose the way the first acquisition is triggered
                    switch (this.imagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                            MC.SetParam(channel, "ChannelState", "IDLE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                            break;
                    }
                }
                else
                {
                    switch (this.imagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "HARD");

                            MC.SetParam(channel, "TrigLine", GetTriggerLineName());        // Norminal

                            if (triggerType == TriggerType.FallingEdge)
                                MC.SetParam(channel, "TrigEdge", "GOLOW");
                            else
                                MC.SetParam(channel, "TrigEdge", "GOHIGH");

                            MC.SetParam(channel, "TrigFilter", "ON");

                            MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);

                            // Parameter valid only for Grablink Full, DualBase, Base
                            MC.SetParam(channel, "TrigCtl", "ISO");
                            //MC.SetParam(channel, "ChannelState", "ACTIVE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "TrigMode", "HARD");
                            break;
                    }

                }
            }
            catch (Euresys.MultiCamException exc)
            {
                //LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            MC.SetParam(channel, "TrigDelay_us", triggerDelayUs);
        }

        public override void Release()
        {
            base.Release();

            MC.Delete(channel);
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            Debug.Assert(grabbedImage != null);

            lock (grabLock)
            {
                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - UpdateImage");
                return Array.Find(grabbedImage, f => f.DataPtr == ptr);
                foreach (Image2D image in grabbedImage)
                {
                    //if (image.ImageData == null)
                    //    continue;
                    if (image.DataPtr == ptr)
                        return image;
                }

                return null;
            }
        }

        public override List<ImageD> GetImageBufferList()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GetImageBufferList");
            return new List<ImageD>(grabbedImage);
        }

        public override int GetImageBufferCount()
        {
            return grabbedImage.Length;
        }

        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabSingle");

            if (SetupGrab() == false)
                return;

            try
            {
                //grabDoneEvent.Reset();
                remainGrabCount = 1;

                //MC.SetParam(channel, "SeqLength_Fr", 1);

                MC.SetParam(channel, "ChannelState", "ACTIVE");

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + exc.Message);
            }
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabContinuous");

            try
            {
                this.remainGrabCount = grabCount;

                if (grabCount == CONTINUOUS)
                    MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                else
                    MC.SetParam(channel, "SeqLength_Fr", grabCount);

                MC.SetParam(channel, "ChannelState", "ACTIVE");

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + exc.Message);
            }
        }

        private void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case MC.SIG_END_EXPOSURE:
                    ProcessingEndExposureCallback();
                    break;
                case MC.SIG_SURFACE_FILLED:
                    ProcessingSurfaceFilledCallback(signalInfo);
                    break;

                case MC.SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new Euresys.MultiCamException("Unknown signal");
            }
        }

        private void ProcessingStartExposureCallback()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingStartExposureCallback");

            //grabbedImage[grabIndex].Clear();
        }

        private void ProcessingEndExposureCallback()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingEndExposureCallback");

            ExposureDoneCallback();
        }

        private void ProcessingSurfaceFilledCallback(MC.SIGNALINFO signalInfo)
        {
            if (imageMutex.WaitOne(0) == false)
                return;

            //grabDoneEvent.Set();

            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback");

            CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)CameraInfo;

            UInt32 currentChannel = (UInt32)signalInfo.Context;

            currentSurface = signalInfo.SignalInfo;

            try
            {
                lock (grabLock)
                {
                    // Update the image with the acquired image buffer data 
                    IntPtr bufferAddress;
                    MC.GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
                    MC.GetParam(currentSurface, "SurfaceSize", out int sizeBytes);
                    MC.GetParam(currentSurface, "SurfacePitch", out int pitchBytes);
                    int h = sizeBytes / pitchBytes;

                    LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback - SetData");

                    if (bufferAddress != null)
                    {
                        int nextGrabIndex = (int)((grabIndex + 1) % cameraInfoMultiCam.SurfaceNum);

                        //if (grabbedImage[nextGrabIndex] != null)
                        //grabbedImage[nextGrabIndex].Clear();

                        grabbedImage[grabIndex].SetData(bufferAddress);

                        LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback - Call ImageGrabbedCallback");

                        ImageGrabbedCallback(bufferAddress);

                        grabIndex = nextGrabIndex;
                    }
                }

                imageMutex.ReleaseMutex();
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + exc.Message);
            }
            catch (System.Exception exc)
            {
                LogHelper.Error(LoggerType.Error, "System Exception : " + exc.Message);
            }
        }

        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            LogHelper.Error(LoggerType.Error, "Acquisition Failure, Channel State: IDLE");
        }

        public override void Stop()
        {
            base.Stop();

            MC.GetParam(channel, "ChannelState", out string state);
            if (state != "IDLE")
                MC.SetParam(channel, "ChannelState", "IDLE");
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            MC.SetParam(channel, "Expose_us", exposureTimeMs * 1000);
            return true;
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
