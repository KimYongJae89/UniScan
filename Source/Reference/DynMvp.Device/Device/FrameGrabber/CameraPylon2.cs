using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber
{
    public enum TriggerSelector
    {
        AcquisitionStart, FrameStart
    }

    public enum LineSelector
    {
        Line1, Line2, Line3
    }

    public enum TriggerSource
    {
        Line1, Line2, Line3
    }

    public enum LineMode
    {
        Input, Output
    }

    public enum LineSource
    {
        ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2
    }

    public class CameraPylon2 : Camera, IDigitalIo
    {
        CameraInfoPylon2 cameraInfoPylon;

        public CameraInfo CamInfo
        {
            get { return cameraInfoPylon; }
        }

        public delegate void CameraOpenedEventDelegate();
        public delegate void CameraOpeningEventDelegate();
        public delegate void CameraClosedEventDelegate();
        public delegate void CameraClosingEventDelegate();
        public delegate void CameraConnectionLostEventDelegate();
        public delegate void GrabStartedEventDelegate();
        public delegate void GrabStartingEventDelegate();
        public delegate void GrabStoppedEventDelegate();
        public delegate void GrabStoppingEventDelegate();
        public delegate void GrabSucceededEventDelegate(Image2D image);
        public delegate void GrabFailedEventDelegate(int errorCode, string errorDescription);



        public Basler.Pylon.Camera MainCamera { get; private set; }

        private Queue<Basler.Pylon.IGrabResult> GrabResultBuffer { get; set; } = new Queue<Basler.Pylon.IGrabResult>();

        public uint GrabResultBufferSize { get; set; } = 1;

        private Basler.Pylon.IGrabResult LastGrabResult { get; set; } = null;

        public event CameraOpenedEventDelegate CameraOpened;

        public event CameraOpeningEventDelegate CameraOpening;

        public event CameraClosedEventDelegate CameraClosed;

        public event CameraClosingEventDelegate CameraClosing;

        public event CameraConnectionLostEventDelegate CameraConnectionLost;

        public event GrabStartedEventDelegate GrabStarted; // 그랩 세션 시작 후

        public event GrabStartingEventDelegate GrabStarting; // 그랩 세션 시작 전

        public event GrabStoppedEventDelegate GrabStopped; // 그랩 세션 중단 후

        public event GrabStoppingEventDelegate GrabStopping; // 그랩 세션 중단 전

        public event GrabSucceededEventDelegate GrabSucceeded; // 그랩 성공

        public new event GrabFailedEventDelegate GrabFailed; // 실패하면 GrabFailed 가 호출됨.

        public CameraPylon2(CameraInfo cameraInfo) : base(cameraInfo) { }

        public override void Initialize(bool calibrationMode)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Pylon Camera");

            cameraInfoPylon = (CameraInfoPylon2)cameraInfo;

            LogHelper.Debug(LoggerType.StartUp, string.Format("Open pylon camera - Device Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3}",
                cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo));

            try
            {

                IDictionary<string, string> cameraInfoMap = new Dictionary<string, string>();
                if (cameraInfoPylon.DeviceUserId != "")
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.UserDefinedName] = cameraInfoPylon.DeviceUserId;
                if (cameraInfoPylon.SerialNo != "")
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.SerialNumber] = cameraInfoPylon.SerialNo;
                if (cameraInfoPylon.IpAddress != "")
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.DeviceIpAddress] = cameraInfoPylon.IpAddress;

                //MainCamera = new Basler.Pylon.Camera(cameraInfoMap, Basler.Pylon.CameraSelectionStrategy.FirstFound);
                MainCamera = new Basler.Pylon.Camera(cameraInfoPylon.SerialNo);

                MainCamera.CameraOpened += OnCameraOpened;
                MainCamera.CameraOpening += OnCameraOpening;
                MainCamera.CameraClosed += OnCameraClosed;
                MainCamera.CameraClosing += OnCameraClosing;
                MainCamera.ConnectionLost += OnCameraConnectionLost;
                MainCamera.StreamGrabber.GrabStarted += OnGrabStarted;
                MainCamera.StreamGrabber.GrabStarting += OnGrabStarting;
                MainCamera.StreamGrabber.GrabStopped += OnGrabStopped;
                MainCamera.StreamGrabber.GrabStopping += OnGrabStopping;
                MainCamera.StreamGrabber.ImageGrabbed += OnImageGrabbed;

                MainCamera.Open(2000, Basler.Pylon.TimeoutHandling.ThrowException);

                SetupImageFormat();
                cameraInfo.Width = ImageSize.Width;
                cameraInfo.Height = ImageSize.Height;
                cameraInfo.SetNumBand(NumOfBand);

                MainCamera.Parameters["ExposureMode"].ParseAndSetValue("Timed");
                MainCamera.Parameters["ExposureAuto"].ParseAndSetValue("Off");
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, "Pylon Exception - {0}", new object[] { ex.Message }, "");
                //string message = "Can't open camera. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} / Message : {4} ";
                //string[] args = new string[] { cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo };
                //throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, message, args, "");
            }

        }

        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            if (e.GrabResult.GrabSucceeded)
            {
                Image2D image = new Image2D();
                lock (GrabResultBuffer)
                {
                    LastGrabResult = e.GrabResult.Clone();
                    if (GrabResultBuffer.Count > GrabResultBufferSize)
                        GrabResultBuffer.Dequeue();
                    GrabResultBuffer.Enqueue(e.GrabResult);
                    GrabResultToImage(LastGrabResult, ref image);
                }
                GrabSucceeded?.Invoke(image);
                ImageGrabbedCallback(IntPtr.Zero); 
            }
            else
            {
                GrabFailed?.Invoke(e.GrabResult.ErrorCode, e.GrabResult.ErrorDescription);
            }
        }

        private void GrabResultToImage(Basler.Pylon.IGrabResult grabResult, ref Image2D image)
        {
            switch (grabResult.PixelTypeValue)
            {
                case Basler.Pylon.PixelType.Mono8:
                    image.Initialize(grabResult.Width, grabResult.Height, 1);
                    break;
                default:
                    break;
            }
            image.SetData(grabResult.PixelData as byte[]);
        }

        private void OnGrabStopping(object sender, GrabStopEventArgs e)
        {
            GrabStopping?.Invoke();
        }

        private void OnGrabStopped(object sender, GrabStopEventArgs e)
        {
            GrabStopped?.Invoke();
 //           Started = false;
        }

        private void OnGrabStarting(object sender, EventArgs e)
        {
            GrabStarting?.Invoke();
        }

        private void OnGrabStarted(object sender, EventArgs e)
        {
            GrabStarted?.Invoke();
        }

        private void OnCameraConnectionLost(object sender, EventArgs e)
        {
            string message = string.Format("Pylon camera connection lost. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} ",
                    cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);
   //         LogHelper.Error(message);
            CameraConnectionLost?.Invoke();
  //          ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.InvalidState,
  //                  ErrorLevel.Warning, ErrorSection.Grabber.ToString(), CommonError.InvalidState.ToString(), message);
        }

        private void OnCameraClosing(object sender, EventArgs e)
        {
            CameraClosing?.Invoke();
        }

        private void OnCameraClosed(object sender, EventArgs e)
        {
            CameraClosed?.Invoke();
        }

        private void OnCameraOpening(object sender, EventArgs e)
        {
            CameraOpening?.Invoke();
        }

        private void OnCameraOpened(object sender, EventArgs e)
        {
            CameraOpened?.Invoke();
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            MainCamera.Parameters["TriggerSelector"].ParseAndSetValue("FrameStart");
            if (triggerMode == TriggerMode.Software)
            {
                MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
                MainCamera.Parameters["TriggerSource"].ParseAndSetValue("Software");
            }
            else //TriggerMode.Hardware
            {
                MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");
                MainCamera.Parameters["TriggerSource"].ParseAndSetValue("Line1");
                MainCamera.Parameters["TriggerActivation"].ParseAndSetValue(triggerType == TriggerType.RisingEdge ? "RisingEdge" : "FallingEdge");
            }
        }

        public void SetDigitalIOControl(LineSelector lineSelector, LineMode lineMode, LineSource lineSource = LineSource.ExposureActive, bool lineInverter = false)
        {
            MainCamera.Parameters["LineSelector"].ParseAndSetValue(lineSelector.ToString());
            MainCamera.Parameters["LineMode"].ParseAndSetValue(lineMode.ToString());
            if (lineMode == LineMode.Output)
                MainCamera.Parameters["LineSource"].ParseAndSetValue(lineSource.ToString());
            MainCamera.Parameters["LineInverter"].ParseAndSetValue(lineInverter ? "1" : "0");
        }

        public void SetAcquisitionFrameRate(float acquisitionFrameRate, bool enable)
        {
            MainCamera.Parameters["AcquisitionFrameRateAbs"].ParseAndSetValue(acquisitionFrameRate.ToString());
            MainCamera.Parameters["AcquisitionFrameRateEnable"].ParseAndSetValue(enable ? "1" : "0");
        }

        public void SetLineDebouncerTimeAbs(float lineDebouncerTimeAbs)
        {
            MainCamera.Parameters["LineDebouncerTimeAbs"].ParseAndSetValue(lineDebouncerTimeAbs.ToString());
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            //MainCamera.Parameters["TriggerDelay"].ParseAndSetValue(triggerDelayUs.ToString());
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Change Exposure {0} - {1}", Index, exposureTimeMs));
            float exposureTimeUs = exposureTimeMs * 1000f; //다시 us
            if (exposureTimeUs < 100)
                exposureTimeUs = 100;
            MainCamera.Parameters["ExposureTimeAbs"].ParseAndSetValue(exposureTimeUs.ToString());
            return true;
        }

        public void SetGain(float gain)
        {
            MainCamera.Parameters["ExposureTime"].ParseAndSetValue(((int)gain).ToString());
        }

        public void SetImageSize(uint width, uint height, uint offsetX, uint offsetY)
        {
            uint widthMax = uint.Parse(MainCamera.Parameters["WidthMax"].ToString());
            uint heightMax = uint.Parse(MainCamera.Parameters["HeightMax"].ToString());
            if (width > widthMax)
                width = widthMax;
            if (height > heightMax)
                height = heightMax;
            if (offsetX > widthMax - width)
                offsetX = widthMax - width;
            if (offsetY > heightMax - height)
                offsetY = heightMax - height;
            MainCamera.Parameters["Width"].ParseAndSetValue(width.ToString());
            MainCamera.Parameters["Height"].ParseAndSetValue(height.ToString());
            MainCamera.Parameters["OffsetX"].ParseAndSetValue(offsetX.ToString());
            MainCamera.Parameters["OffsetY"].ParseAndSetValue(offsetY.ToString());
        }

        public void SetImagecenter(bool centerX, bool centerY)
        {
            MainCamera.Parameters["CenterX"].ParseAndSetValue(centerX.ToString());
            MainCamera.Parameters["CenterY"].ParseAndSetValue(centerY.ToString());
        }

        private void SetupImageFormat()
        {
            int width = int.Parse(MainCamera.Parameters["Width"].ToString());
            int height = int.Parse(MainCamera.Parameters["Height"].ToString());
            string pixelFormat = MainCamera.Parameters["PixelFormat"].ToString();
            ImageSize = new Size(width, height);
            NumOfBand = pixelFormat == "Mono8" ? 1 : 3;
            ImagePitch = width * NumOfBand;
            LogHelper.Debug(LoggerType.Grab, String.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", width, height, ImagePitch, pixelFormat));
        }

        /*
         * 버퍼에서 가장 최근 이미지를 반환함.
         */
        public  ImageD GetGrabbedImage()
        {
            Image2D image = new Image2D();
            lock (GrabResultBuffer)
            {
                if (LastGrabResult == null)
                {
                    LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : 버퍼에 이미지가 들어온 적이 없음.\n"));
                }
                else
                {
                    var copiedResult = LastGrabResult;
                    switch (copiedResult.PixelTypeValue)
                    {
                        case Basler.Pylon.PixelType.Mono8:
                            image.Initialize(copiedResult.Width, copiedResult.Height, 1);
                            break;
                        default:
                            break;
                    }
                    image.SetData(copiedResult.PixelData as byte[]);
                }
            }
            return image;
        }

        /*
         * 한 번 그랩 (동기)
         */
        public void GrabOnceSync()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot (Sync)", Index));

            try
            {
                //if (MainCamera.StreamGrabber.IsGrabbing)
                //    throw new Exception("비동기 그랩이 실행중임.");
   //             Started = true;
                Basler.Pylon.IGrabResult grabResult = MainCamera.StreamGrabber.GrabOne(5000, Basler.Pylon.TimeoutHandling.ThrowException);
                //Basler.Pylon.IGrabResult grabResult = MainCamera.StreamGrabber.GrabOne(500);
                //Image2D image = new Image2D();
                //switch (grabResult.PixelTypeValue)
                //{
                //    case Basler.Pylon.PixelType.Mono8:
                //        image.Initialize(grabResult.Width, grabResult.Height, 1);
                //        break;
                //    default:
                //        break;
                //}
                //byte[] rawImage = grabResult.PixelData as byte[];
                //image.SetData(rawImage);
                //Stop();
 //               Started = false;
                //return image;
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : {0}\n", e.Message));
 //               Started = false;
                //return new Image2D();
            }
        }

        /*
         * 한 번 그랩 (비동기)
         * 그랩 완료시 
         */
        public void GrabOnceAsync()
        {
            if (MainCamera.StreamGrabber.IsGrabbing)
                return;
            LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot (Async)", Index));

            try
            {
                MainCamera.StreamGrabber.Start(1, Basler.Pylon.GrabStrategy.LatestImages, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : {0}\n", e.Message));
            }
        }

        /*
         * 프레임 지정 그랩 (비동기)
         * 그랩 완료시 
         */
        public void GrabOnceAsync(uint grabCount)
        {
            if (grabCount == 0)
                return;
            if (MainCamera.StreamGrabber.IsGrabbing)
                return;
            LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot (Async)", Index));

            try
            {
                MainCamera.StreamGrabber.Start(grabCount, Basler.Pylon.GrabStrategy.LatestImages, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : {0}\n", e.Message));
            }
        }

        /*
         * 그랩 중지
         */
        public override void Stop()
        {
            if (MainCamera == null)
                return;

            LogHelper.Debug(LoggerType.Grab, String.Format("Stop Continuous {0}", Index));
            MainCamera.StreamGrabber.Stop();
            remainGrabCount = 0;
        }

        /*
         *  해제 
         */
        public override void Release()
        {
            base.Release();
            Stop();
            MainCamera?.Close();
        }

        //private void OnGrabDone(IAsyncResult result)
        //{
        //    ((CameraEventDelegate)result.AsyncState).EndInvoke(result);
        //}

        public string GetName()
        {
            return Name;
        }

        public int GetNumInPortGroup()
        {
            return 1;
        }

        public int GetNumOutPortGroup()
        {
            return 1;
        }

        public int GetInPortStartGroupIndex()
        {
            return 0;
        }

        public int GetOutPortStartGroupIndex()
        {
            return 0;
        }

        public int GetNumInPort()
        {
            return 1;
        }

        public int GetNumOutPort()
        {
            return 1;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public bool IsInitialized()
        {
            return true;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            MainCamera.Parameters["UserOutputValue"].ParseAndSetValue(outputPortStatus == 1 ? "True" : "False");
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public uint ReadOutputGroup(int groupNo)
        {
            bool userOutputValue = bool.Parse(MainCamera.Parameters["UserOutputValue"].ToString());
            return userOutputValue ? (uint)1 : 0;
        }

        public uint ReadInputGroup(int groupNo)
        {
            return 0;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }

        //public override void SetStopFlag()
        //{
        //    throw new NotImplementedException();
        //}

        public override void GrabOnce()
        {
            GrabOnceSync();
        }

        //public override void GrabAsync()
        //{
        //    GrabOnceAsync();
        //}

        public override void GrabMulti(int grabCount = -1)
        {

            if (MainCamera.StreamGrabber.IsGrabbing)
                return;
            LogHelper.Debug(LoggerType.Grab, String.Format("Continuous Shot", Index));

            try
            {
                MainCamera.StreamGrabber.Start(Basler.Pylon.GrabStrategy.OneByOne, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
                remainGrabCount = CONTINUOUS;
  //              Started = true;
            }
            catch (Exception e)
            {
   //             Started = false;
                LogHelper.Debug(LoggerType.Grab, String.Format("Continuous Shot Error : {0}\n", e.Message));
            }
        }

        //public override bool SetGain(float gain)
        //{
        //    throw new NotImplementedException();
        //}

        public override void Reset()
        {
            base.Reset();
            Stop();
        }

        //public override GrabberType GetGrabberType()
        //{
        //    return GrabberType.Pylon2;
        //}

        public override float GetDeviceExposureMs()
        {
            float expus=float.Parse(MainCamera.Parameters["ExposureTimeAbs"].ToString());
            return (float)(expus / 1000.0);
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            return GetGrabbedImage();
        }

        public override List<ImageD> GetImageBufferList()
        {
            throw new NotImplementedException();
        }

        public override int GetImageBufferCount()
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
