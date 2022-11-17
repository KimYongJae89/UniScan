using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;

using DynMvp.Base;
using System.Collections;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public enum BayerType    {        GB, BG, RG, GR    }
    public enum ScanMode { Area, Line }
    public enum ETDIStages { TDI64, TDI128, TDI192, TDI256 }
    public enum EClientType { Master, Slave }
    public enum EScanDirectionType { Forward, Reverse }
    public enum EAnalogGain { X1, X2, X3, X4 }

    public class CameraSpec
    {
        private int imageWidth;
        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }

        private int imageHeight;
        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }

        private int imageDepth;
        public int ImageDepth
        {
            get { return imageDepth; }
            set { imageDepth = value; }
        }
    }

    public enum CameraType
    {
        Jai_GO_5000, PrimeTech_PXCB120VTH, Crevis_MC_D500B, PrimeTech_PXCB16QWTPM,
        PrimeTech_PXCB16QWTPMCOMPACT, HV_B550CTRG1, HV_B550CTRG2, RaL12288_66km,
        EliixaPlus16K, UNiiQA, CXP
    }

    public class CameraInitializeFailedException : ApplicationException
    {
    }

    public delegate IntPtr BufferAllocatorDelegate(int size);
    public abstract class Camera : ImageDevice
    {
        public BufferAllocatorDelegate BufferAllocator = null;

        protected CameraInfo cameraInfo;
        public CameraInfo CameraInfo
        {
            get { return cameraInfo; }
            //set { cameraInfo = value; }
        }

        private SizeF fovSize;
        public SizeF FovSize
        {
            get { return fovSize; }
            set { fovSize = value; }
        }

        protected int numOfBand;
        public int NumOfBand
        {
            get { return numOfBand; }
            set { numOfBand = value; }
        }

        private bool bayerCamera;
        public bool BayerCamera
        {
            get { return bayerCamera; }
            set { bayerCamera = value; }
        }

        BayerType bayerType;
        public BayerType BayerType
        {
            get { return bayerType; }
            set { bayerType = value; }
        }

        public ulong GrabbedCount => grabbedCount;
        protected ulong grabbedCount = 0;
        protected int remainGrabCount = 0;
        private object grabCountLockObj = new object();

        public bool IsCalibrationMode => this.isCalibrationMode;
        protected bool isCalibrationMode = false;

        protected Stopwatch grabTimer = new Stopwatch();

        /// <summary>
        /// What is it??
        /// </summary>
        protected ManualResetEvent exposureDoneEvent = new ManualResetEvent(true);

        /// <summary>
        /// for GrabDone() check
        /// </summary>
        protected ManualResetEvent isGrabbed = new ManualResetEvent(false); //이미지를 받은 직후, 콜백 시작 지점 (매번 세트됨)
        protected ManualResetEvent isGrabDone = new ManualResetEvent(false); //이미지를 박은 후 콜백 끝지점(매번 세트됨)
        protected ManualResetEvent isStopped = new ManualResetEvent(true);  

        /// <summary>
        /// for ImageGrabbed() done check
        /// </summary>
        //protected ManualResetEvent imageGrabbedDoneEvent = new ManualResetEvent(true);

        protected float exposureTimeUs = 0;

        public override bool IsVirtual => false;
        Mutex grabCallBackMutex = new Mutex();

        public Camera(CameraInfo cameraInfo) : base()
        {
            this.cameraInfo = cameraInfo;
            this.name = cameraInfo.Name;
            this.Index = cameraInfo.Index;
            this.Enabled = cameraInfo.Enabled;

            DeviceType = DeviceType.Camera;

            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
        }

        public override bool SetStepLight(int stepNo, int lightNo) { return true; }

        //public virtual bool SetFreeMode(float grabHz) { return true; }
        //public virtual float GetGrabHz() { return float.NaN; }
        protected virtual void SetScanMode(ScanMode scanMode) { }

        public virtual void Initialize(bool calibrationMode)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Camera");
            this.isCalibrationMode = calibrationMode;
        }

        public void UpdateFovSize(SizeF pelSize)
        {
            fovSize.Width = ImageSize.Width * pelSize.Width;
            fovSize.Height = ImageSize.Height * pelSize.Height;
        }

        public override bool IsOnLive()
        {
            return remainGrabCount == CONTINUOUS;
        }

        public override bool IsCompatibleImage(ImageD image)
        {
            //LogHelper.Debug(LoggerType.Grab, "Camera - IsCompatibleBitmap");

            if (image == null)
                return false;

            return (image.NumBand == numOfBand && image.DataSize == 1 && IsCompatibleSize(new Size(image.Width, image.Height)));
        }

        protected bool IsCompatibleSize(Size bitMapSize)
        {
            if (Is90Or270Rotated() == false)
                return bitMapSize.Width == ImageSize.Width && bitMapSize.Height == ImageSize.Height;
            else
                return bitMapSize.Width == ImageSize.Height && bitMapSize.Height == ImageSize.Width;
        }

        public bool Is90Or270Rotated()
        {
            return this.cameraInfo.RotateFlipType.IsRotate90()
                || this.cameraInfo.RotateFlipType.IsRotate90X()
                || this.cameraInfo.RotateFlipType.IsRotate90Y()
                || this.cameraInfo.RotateFlipType.IsRotate270();
        }

        public override ImageD CreateCompatibleImage()
        {
            LogHelper.Debug(LoggerType.Grab, "Camera - CreateCompatibleImage");

            Image2D image2d = new Image2D();
            if (this.cameraInfo.UseNativeBuffering == false)
                image2d.Initialize(ImageSize.Width, ImageSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), ImagePitch);
            else
                image2d.Initialize(ImageSize.Width, ImageSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), ImagePitch, null);


            return image2d;
        }

        public override bool IsGrabbed()
        {
            return isGrabbed.WaitOne(0, false);
            //return isGrabbed.WaitOne(0);
        }

        public override bool IsGrabDone()
        {
            return isGrabDone.WaitOne(0, false);
            //return isGrabDone.WaitOne(0);
        }

        public override bool IsStopped()
        {
            return isStopped.WaitOne(0, false);
            //return isStopped.WaitOne(0);
        }

        public override bool WaitGrabDone(int timeoutMs = 0)
        {
            if (timeoutMs == 0)
                timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            LogHelper.Debug(LoggerType.Grab, "Camera::WaitGrabDone");
            bool ok = false;

            while (timeoutMs > 10 && ok == false)
            {
                Thread.Sleep(10);
                bool isGrabbed = this.isGrabbed.WaitOne(0); //todo
                bool isGrabDone = this.isGrabDone.WaitOne(0);
                bool isStopped = this.isStopped.WaitOne(0);
                if (isGrabbed)
                {
                    ok = isGrabDone;
                }
                else
                {
                    ok = isStopped;
                }
                timeoutMs -= 10;
            }

            return ok;
        }

        public override void Reset()
        {
            exposureDoneEvent.Set();
            isGrabbed.Reset();
            isStopped.Reset();
            grabTimer.Reset();
            grabFailed = false;
        }

        public override bool SetExposureTime(float exposureTimeUs)
        {
            if (exposureTimeUs <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, string.Format("Camera::SetExposureTime - {0} um", exposureTimeUs));

            bool result = true;
            if (this.exposureTimeUs != exposureTimeUs)
            {
                result = SetDeviceExposure(exposureTimeUs / 1000);
                if (result == true)
                    this.exposureTimeUs = exposureTimeUs;
            }
            return result;
        }

        public virtual bool SetupGrab()
        {
            if (Enabled == false)
                return false;

            if (grabFailed)
            {
                ErrorManager.Instance().Report(ErrorCodeGrabber.Instance.InvalidState, ErrorLevel.Warning,
                    this.name, "Grab Fail State", null);
                return false;
            }

            this.grabPerSec = 0;
            this.grabbedCount = 0;

            exposureDoneEvent.Reset();
            isGrabDone.Reset();
            isGrabbed.Reset();
            isStopped.Reset();
            grabTimer.Reset();

            grabTimer.Start();

            LogHelper.Debug(LoggerType.Grab, String.Format("Camera[{0}]::SetupGrab", Index));

            return true;
        }

        public override void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Camera[{0}]::Stop", Index));
            isStopped.Set();
        }

        protected void ExposureDoneCallback()
        {
            if (remainGrabCount != CONTINUOUS && remainGrabCount == 1)
            {
                exposureDoneEvent.Set();
            }
        }

        protected void ImageGrabbedCallback(IntPtr ptr)
        {
       //    LogHelper.Debug(LoggerType.Grab, "Camera::ImageGrabbedCallback - Begin");
            isGrabbed.Set();
            exposureDoneEvent.Set();

            lock (grabCountLockObj)
            {
                this.grabbedCount++;
                if (remainGrabCount != CONTINUOUS)
                {
                    remainGrabCount--;
                    if (remainGrabCount < 0)
                        remainGrabCount = 0;

                    if (remainGrabCount == 0)
                        Stop();
                }
            }

            UpdateGrabSpeed();

            if (ImageGrabbed != null)
            {
                //LogHelper.Debug(LoggerType.Function, String.Format("ImageGrabbed - [{0}] Start. {1}ms", Index, grabTime));
                LogHelper.Debug(LoggerType.Grab, String.Format("Camera::ImageGrabbed - Index: {0} Start", Index));

                ImageGrabbed(this, ptr);

                LogHelper.Debug(LoggerType.Grab, String.Format("Camera::ImageGrabbed - Index: {0} End.", Index));
            }

            ////GC.Collect();
            ////GC.WaitForFullGCComplete(); // 여기서 간헐적으로 무한루프에 빠지는 경우 있음

            //ImageProvider
            isGrabDone.Set();
        }

        protected virtual void UpdateGrabSpeed()
        {
            double totalSec = this.grabTimer.Elapsed.TotalSeconds;
            this.grabPerSec = (float)(this.grabbedCount / totalSec);
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            if (this.grabFailed)
                this.grabFailed = false;
        }

        public abstract bool SetDeviceExposure(float exposureTimeMs);
        public abstract float GetDeviceExposureMs();

        public virtual void SetImageSize(int width, int height)
        {

        }
    }
}
