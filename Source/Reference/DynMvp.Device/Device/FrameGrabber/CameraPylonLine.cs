using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using PylonC.NETSupportLibrary;

namespace DynMvp.Devices.FrameGrabber
{
    //public enum TriggerSelector
    //{
    //    AcquisitionStart, FrameStart
    //}

    //public enum LineSelector
    //{
    //    Line1, Line2, Line3
    //}

    //public enum TriggerSource
    //{
    //    Line1, Line2, Line3
    //}

    //public enum LineMode
    //{
    //    Input, Output
    //}

    //public enum LineSource
    //{
    //    ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2
    //}

    /// <summary>
    /// Line Scan용으로 제작됨.
    /// raL2048-48gm 테스트 완료, Area는 테스트한적 없음, 
    /// ColorSensor, PinholeSensor, 파스용
    /// </summary>
    public class CameraPylonLine : Camera, IDigitalIo
    {
        CameraInfoPylon cameraInfoPylon;

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

        private List<Basler.Pylon.IGrabResult> GrabResultBuffer { get; set; } = new List<Basler.Pylon.IGrabResult>();

        public uint GrabResultBufferSize { get; set; } = 50; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!꼭봐라 !!!!

        private Basler.Pylon.IGrabResult LastGrabResult { get; set; } = null;

        bool IDigitalIo.IsVirtual => throw new NotImplementedException();

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

        public CameraPylonLine(CameraInfo cameraInfo) : base(cameraInfo) { base.Name = "CameraPylonLine"; }

        protected ManualResetEvent isWholeGrabDone = new ManualResetEvent(false); //N장의 이미지 모두 받음.

        public override void Initialize(bool calibrationMode)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize PylonLine Camera");

            cameraInfoPylon = (CameraInfoPylon)cameraInfo;

            LogHelper.Debug(LoggerType.StartUp, string.Format("Open PylonLine camera - Device Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3}",
                cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo));

            string ipAddr = cameraInfoPylon.IpAddress;
            string serial = cameraInfoPylon.SerialNo;
            if (string.IsNullOrEmpty(serial))
            {
                List<DeviceEnumerator.Device> deviceList = DeviceEnumerator.EnumerateDevices();
                foreach (DeviceEnumerator.Device device in deviceList)
                {
                    GrabberPylon.GetFeature(device.Tooltip, out string deviceUserId, out string ipAddress, out string serialNo, out string modelName);
                    if (cameraInfoPylon.IpAddress == ipAddress)
                        serial = serialNo;
                }
            }

            try
            {
                MainCamera = new Basler.Pylon.Camera(serial);

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

                    
                //open camera
                MainCamera.Open(5000, Basler.Pylon.TimeoutHandling.ThrowException);

                MainCamera.Parameters[PLCamera.AcquisitionMode].ParseAndSetValue("Continuous");


                SetupImageFormat(cameraInfoPylon.UpdateDeviceFeature);
                cameraInfo.Width = ImageSize.Width;
                cameraInfo.Height = ImageSize.Height;
                cameraInfo.SetNumBand(NumOfBand);

                MainCamera.Parameters["ExposureMode"].ParseAndSetValue("Timed");
                MainCamera.Parameters["ExposureAuto"].ParseAndSetValue("Off");

                //FlipX
                MainCamera.Parameters[PLCamera.ReverseX].SetValue(this.cameraInfo.RotateFlipType.IsFlipX());
                //set trigger
                SetTriggerExFunction(ref cameraInfoPylon);

                MainCamera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(GrabResultBufferSize);

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
            Debug.WriteLine("CameraPylonLine.OnImageGrabbed().....................");
            try
            {
                IGrabResult _grabResult;
                if (e.GrabResult.GrabSucceeded)
                {
                    lock (GrabResultBuffer)
                    {
                        _grabResult = e.GrabResult.Clone(); //얉은복사, 데이터는 그냥 참조되며, 복사객체는 디스포즈되야함.
                        
                        if (GrabResultBuffer.Count > GrabResultBufferSize)
                        {
                            var taken = GrabResultBuffer[0];
                            GrabResultBuffer.Remove(taken);
                            taken.Dispose();
                            Debug.WriteLine("CameraPylonLine.Camera Buffer has overrun !!!!!!!!!!!!!!!!");
                        }
                            
                        GrabResultBuffer.Add(_grabResult); //유효범위가 카메라에 세팅한 버퍼 사이즈임. 맞는지 확인필요.
                    }
                    if (_grabResult.PayloadTypeValue == PayloadType.ChunkData)
                    {

                    }
                    
                    IntPtr ptr = (IntPtr)_grabResult.BlockID; //GrabResult.ID;
                    //ID: 콜백 순서로 번호가 부여되며, 프로그램을 재시작해야 리셋됨
                    //BlockID: 카메라에서 그랩한 프래임 번호이며, 다시 그랩하면 리셋됨, 만일 번호가 연속적이지 않다면 버퍼 오버런, 언더런 난것임=> 즉 잘못된것임.
                    Debug.WriteLine(string.Format("CameraPylonLine.OnImageGrabbed.(ID:{0}, BlockID:{1})", _grabResult.ID, _grabResult.BlockID));
                    base.ImageGrabbedCallback(ptr);
                    if(base.remainGrabCount ==0) // 전체 그랩 (1~N장) 완료시.
                    {
                        isWholeGrabDone.Set();
                        Debug.WriteLine(string.Format("CameraPylonLine.OnImageGrabbed() >> isWholeGrabDone.Set() "));
                    }
                    //GrabResult.Dispose();
                }
                else
                {
                    GrabFailed?.Invoke(e.GrabResult.ErrorCode, e.GrabResult.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        private Image2D PopImageFromBuffer(long imageID) //ID는 1부터 시작 됨, 0이며 마지막 이미지 리턴
        {
            Image2D imageD = null;
            lock (GrabResultBuffer)
            {
                IGrabResult grabResult = null;
                if (imageID == 0)
                {
                    if (GrabResultBuffer.Count > 0)
                        grabResult = GrabResultBuffer[GrabResultBuffer.Count - 1];
                }
                else
                    grabResult = GrabResultBuffer.Find(a => a.BlockID == imageID);

                if (grabResult != null)
                {
                    switch (grabResult.PixelTypeValue)
                    {
                        case Basler.Pylon.PixelType.Mono8:
                            imageD = new Image2D();
                            //imageD.Initialize(grabResult.Width, grabResult.Height, 1);
                            imageD.Initialize(ImageSize.Width, ImageSize.Height, 1);
                            break;
                        default:
                            break;
                    }
                    imageD.SetData(grabResult.PixelData as byte[]);

                    if (grabResult.PayloadTypeValue == PayloadType.ChunkData)
                    {
                        if (imageD != null)
                        {
                            int w = 0, h = 0;
                            if (grabResult.ChunkData[PLChunkData.ChunkWidth].IsReadable)
                                w = (int)grabResult.ChunkData[PLChunkData.ChunkWidth].GetValue();

                            if (grabResult.ChunkData[PLChunkData.ChunkHeight].IsReadable)
                                h = (int)grabResult.ChunkData[PLChunkData.ChunkHeight].GetValue();

                            imageD.Tag = new CameraInfoPylonBufferTag((UInt64)grabResult.ID, new Size(w, h), (int)grabResult.BlockID);
                            Debug.WriteLine(string.Format("♠ ChunkWidth : {0}, ChunkHeight : {1})", w, h));
                        }
                    }
                    GrabResultBuffer.Remove(grabResult);
                    grabResult.Dispose(); //반드시 디스포즈 해줘야 카메라 내부에서 언더런이 안남...
                }
            }
            return imageD;
        }

        private void OnGrabStopping(object sender, GrabStopEventArgs e)
        {
            GrabStopping?.Invoke();
        }

        private void OnGrabStopped(object sender, GrabStopEventArgs e)
        {
            Debug.WriteLine("CameraPylonLine.OnGrabStopped()  >>>>>>>>>>>>>>>>>>>>>");
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
            return;
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
        private void SetTriggerExFunction(ref CameraInfoPylon cameraInfoPylon)
        {
            bool isAvail = false;
            try
            {
                //Line Trigger
                if (cameraInfoPylon.UseLineTrigger)
                {
                    if (isAvail=MainCamera.Parameters[PLCamera.TriggerSelector].TrySetValue("LineStart") )
                    {
                        MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");
                        MainCamera.Parameters["TriggerSource"].ParseAndSetValue(cameraInfoPylon.LineTriggerSourceType.ToString());
                        //TriggerActivation 은 무조건 Rising Edge
                        MainCamera.Parameters[PLCamera.TriggerActivation].SetValue(new PLCamera.TriggerActivationEnum().RisingEdge);
                    }
                }
                else
                {
                    if (isAvail = MainCamera.Parameters[PLCamera.TriggerSelector].TrySetValue("LineStart"))
                        MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
                }
                //Frame Trigger
                if (cameraInfoPylon.UseFrameTrigger)
                {
                    if (isAvail = MainCamera.Parameters[PLCamera.TriggerSelector].TrySetValue("FrameStart"))
                    {
                        MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");
                        MainCamera.Parameters["TriggerSource"].ParseAndSetValue(cameraInfoPylon.FrameTriggerSourceType.ToString());
                        MainCamera.Parameters[PLCamera.TriggerActivation].SetValue(cameraInfoPylon.FrameTriggerActivation.ToString());

                        switch (cameraInfoPylon.FrameTriggerActivation)
                        {
                            case TrigerActivation.LevelHigh:
                            case TrigerActivation.LevelLow:
                                //patialclosing은 레벨하이 또는 로우 일때만 설정 가능함
                                //PylonC.NET.Pylon.DeviceSetBooleanFeature(deviceHandle, "TriggerPartialClosingFrame", cameraInfoPylon.TriggerPartialClosingFrame);
                                MainCamera.Parameters[PLCamera.TriggerPartialClosingFrame].SetValue(cameraInfoPylon.TriggerPartialClosingFrame);
                                break;
                        }
                    }
                }
                else
                {
                    MainCamera.Parameters["TriggerSelector"].ParseAndSetValue("FrameStart");
                    MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
                }

                //if (PylonC.NET.Pylon.DeviceFeatureIsWritable(deviceHandle, "ChunkModeActive"))
                if (MainCamera.Parameters["ChunkModeActive"].IsWritable)
                {
                    /* Activate the chunk mode. */
                    MainCamera.Parameters[PLCamera.ChunkModeActive].SetValue(cameraInfoPylon.UseChunkMode);
                }
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "Pylon Exception - {0}", new object[] { ex.Message }, "");
            }
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            //MainCamera.Parameters["TriggerDelay"].ParseAndSetValue(triggerDelayUs.ToString());
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

        private void SetupImageFormat(bool updateToDeviceFeature)
        {
            Size imageSize = new Size(this.cameraInfo.Width, this.cameraInfo.Height);
            if (updateToDeviceFeature)
            {
                MainCamera.Parameters[PLCamera.Width].TrySetValue(imageSize.Width);
                MainCamera.Parameters[PLCamera.Height].TrySetValue(imageSize.Height);
            }
            else
            {
                imageSize.Width = (int)MainCamera.Parameters[PLCamera.Width].GetValue() ;
                imageSize.Height = (int)MainCamera.Parameters[PLCamera.Height].GetValue();
            }
            ImageSize = imageSize;

            string imageFormat = MainCamera.Parameters[PLCamera.PixelFormat].GetValue();
            if (imageFormat == "Mono8")
                NumOfBand = 1;
            else
                NumOfBand = 3;

            ImagePitch = (int)imageSize.Width * NumOfBand;

            LogHelper.Debug(LoggerType.Grab, String.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", imageSize.Width, imageSize.Height, ImagePitch, imageFormat));
        }

        /*
         * 한 번 그랩 (동기)
         */
        public void GrabOnceSync()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot (Sync)", Index));

            try
            {
                Basler.Pylon.IGrabResult grabResult = MainCamera.StreamGrabber.GrabOne(5000, Basler.Pylon.TimeoutHandling.ThrowException);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : {0}\n", e.Message));
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

        public override void GrabOnce() //overlap
        {
            Reset();
            base.remainGrabCount = 1;
            base.grabbedCount = 0;
            //GrabOnceSync();
            GrabOnceAsync();
        }

        private void ClearGrabBuffer()
        {
            GrabResultBuffer.ForEach(d => d.Dispose());
            GrabResultBuffer.Clear();
        }
        public override void GrabMulti(int grabCount = -1)
        {

            if (MainCamera.StreamGrabber.IsGrabbing)
                return;
            LogHelper.Debug(LoggerType.Grab, String.Format("Continuous Shot", Index));

            base.Reset();
            //lock (grabCountLockObj)
            base.remainGrabCount = grabCount;
            base.grabbedCount = 0;
            ClearGrabBuffer();
           
            try
            {
                if (grabCount == -1)
                    MainCamera.StreamGrabber.Start(Basler.Pylon.GrabStrategy.OneByOne, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
                else
                    MainCamera.StreamGrabber.Start(grabCount, Basler.Pylon.GrabStrategy.OneByOne, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("CameraPylonLine.GrabMulti()  Continuous Shot Error : {0}\n", e.Message));
            }
        }
        /*
         * 그랩 중지
         */
        public override void Stop()
        {
            if (MainCamera == null)
                return;

            base.Stop();
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
            ClearGrabBuffer();
            MainCamera?.Close();
        }

        public override void Reset()
        {
            base.Reset();
            isWholeGrabDone.Reset();
            base.remainGrabCount = 0;
            base.grabbedCount = 0;
        }
        public override bool WaitGrabDone(int timeoutMs = 0)
        {
            if (timeoutMs == 0)
                timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            LogHelper.Debug(LoggerType.Grab, "CameraPylonLine::WaitGrabDone");
            bool ok = false;

            while (timeoutMs > 10 && ok == false)
            {
                Thread.Sleep(10);
                bool isGrabbed = this.isWholeGrabDone.WaitOne(0); //todo
                bool isStopped = this.isStopped.WaitOne(0); //사용자 취소.
                if (isGrabbed || isStopped)
                    ok = true;
                timeoutMs -= 10;
            }
            return ok;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraPylonLine.Change Exposure {0} - {1}", Index, exposureTimeMs));
            float exposureTimeUs = exposureTimeMs * 1000f; //다시 us
            if (exposureTimeUs < 2)
                exposureTimeUs = 2;
            MainCamera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposureTimeUs);
            return true;
        }

        public override float GetDeviceExposureMs()
        {
            float expus=float.Parse(MainCamera.Parameters["ExposureTimeAbs"].ToString());
            return (float)(expus / 1000.0);
        }

        public override ImageD GetGrabbedImage(IntPtr ptr) //ptr = imageID
        {
            //IntPtr.Zero) //마지막 이미지 리턴
            return PopImageFromBuffer((long)ptr);
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
            if (hz <= 0) return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraPylon::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));
            try
            {
                MainCamera.Parameters[PLCamera.AcquisitionLineRateAbs].SetValue(hz);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("CameraPylon::SetDeviceExposure - {0}", ex.Message));
                return false;
            }
            return true;
        }

        public override float GetAcquisitionLineRate()
        {
            float hz = (float)MainCamera.Parameters[PLCamera.ResultingLineRateAbs].GetValue();//실제 카메라가 그랩할수 있는 성능
            return hz;
        }
        
        ////////////////////////////////////////////////////DIO////////////////////////////////////////////////////////////////
        string IDigitalIo.GetName()
        {
            return Name;
        }

        int IDigitalIo.GetNumInPortGroup()
        {
            return 1;
        }

        int IDigitalIo.GetNumOutPortGroup()
        {
            return 1;
        }

        int IDigitalIo.GetInPortStartGroupIndex()
        {
            return 0;
        }

        int IDigitalIo.GetOutPortStartGroupIndex()
        {
            return 0;
        }

        int IDigitalIo.GetNumInPort()
        {
            throw new NotImplementedException();
        }

        int IDigitalIo.GetNumOutPort()
        {
            throw new NotImplementedException();
        }

        bool IDigitalIo.Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        void IDigitalIo.Release()
        {
            throw new NotImplementedException();
        }

        bool IDigitalIo.IsReady()
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.UpdateState(DeviceState state, string stateMessage)
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            //MainCamera.Parameters["UserOutputValue"].ParseAndSetValue(outputPortStatus == 1 ? "True" : "False");
            throw new NotImplementedException();
        }
        void IDigitalIo.WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        uint IDigitalIo.ReadOutputGroup(int groupNo)
        {
            //bool userOutputValue = bool.Parse(MainCamera.Parameters["UserOutputValue"].ToString());
            //return userOutputValue ? (uint)1 : 0;
            throw new NotImplementedException();
        }

        uint IDigitalIo.ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }









    }
}
