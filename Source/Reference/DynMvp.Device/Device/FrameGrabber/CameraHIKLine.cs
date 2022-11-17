using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraHIKLine : Camera
    {

        public uint GrabResultBufferSize { get; set; } = 50; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!꼭봐라 !!!!
        private List<MyCamera.MV_FRAME_OUT?> GrabResultBuffer { get; set; } = new List<MyCamera.MV_FRAME_OUT?>();
        private MyCamera.MV_FRAME_OUT? LastGrabResult { get; set; } =null;

        private MyCamera.MV_CC_DEVICE_INFO deviceInfo = new MyCamera.MV_CC_DEVICE_INFO();
        private MyCamera.MVCC_INTVALUE intValueParam = new MyCamera.MVCC_INTVALUE();
        private MyCamera.MV_FRAME_OUT_INFO_EX frameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();
        private MyCamera.cbOutputExdelegate ImageCallback;

        private MyCamera MyCamera { get; set; } = new MyCamera();
        //private object BufForDriverLock { get; set; } = new object();
        private uint PayLoadSize { get; set; }
        private IntPtr BufForDriver { get; set; }
        private bool IsGrabDoneState { get; set; } = true;
        private bool IsGrabbing { get; set; } = false;
        private bool IsReleaseBuf { get; set; } = true;
        private Image2D LastGrabbedImage { get; set; }


        public CameraHIKLine(CameraInfo cameraInfo) : base(cameraInfo) { base.Name = "CameraHIKLine"; }

        CameraInfoHIKLine cameraInfoHIKLine;
        public CameraInfo CamInfo
        {
            get { return cameraInfoHIKLine; }
        }

        public override void Initialize(bool calibrationMode)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize HIK Camera");

            base.Initialize(calibrationMode);

            if (cameraInfo is CameraInfoHIKLine cameraInfoHIK)
            {
                int result = MyCamera.MV_OK;
                var m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();

                result = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_stDeviceList);
                if (result != MyCamera.MV_OK)
                {
                    return;
                }

                for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
                {
                    deviceInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

                    if (deviceInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(deviceInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                        if (gigeInfo.chSerialNumber == cameraInfoHIK.SerialNo)
                        {
                            break;
                        }
                    }
                    else if (deviceInfo.nTLayerType == MyCamera.MV_USB_DEVICE)
                    {
                        var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(deviceInfo.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                        if (usbInfo.chSerialNumber == cameraInfoHIK.SerialNo)
                        {
                            break;
                        }
                    }
                }

                result = MyCamera.MV_CC_CreateDevice_NET(ref deviceInfo);
                if (MyCamera.MV_OK != result)
                {
                    throw new Exception(ErrorMessage("Device Create Fail!", result));
                }

                //카메라 오픈이 안됬을 경우 재시도한다.
                result = MyCamera.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != result)
                {
                    ReConnectTry(tryCount: 30, sleepTime: 2000);
                }

#if DEBUG

#else
                MyCamera.MV_CC_SetHeartBeatTimeout_NET(1000);
#endif

                if (deviceInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    int nPacketSize = MyCamera.MV_CC_GetOptimalPacketSize_NET();
                    if (nPacketSize > 0)
                    {
                        result = MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                        if (result != MyCamera.MV_OK)
                        {
                            throw new Exception(ErrorMessage("Set Packet Size failed!", result));
                        }
                    }
                    else
                    {
                        throw new Exception(ErrorMessage("Get Packet Size failed!", nPacketSize));
                    }
                }

                result = MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                if (result != 0)
                {
                    throw new Exception(ErrorMessage("Acquisition Mode set failed!", result));
                }

                result = MyCamera.MV_CC_GetIntValue_NET("PayloadSize", ref intValueParam);
                if (MyCamera.MV_OK != result)
                {
                    throw new Exception(ErrorMessage("Get PayloadSize failed", result));
                }

                PayLoadSize = intValueParam.nCurValue;


                var heightValue = new MyCamera.MVCC_INTVALUE();
                result = MyCamera.MV_CC_GetHeight_NET(ref heightValue);
                if (result != 0)
                {
                    throw new Exception(ErrorMessage("Getting height is failed!", result));
                }

                var widthValue = new MyCamera.MVCC_INTVALUE();
                result = MyCamera.MV_CC_GetWidth_NET(ref widthValue);
                if (result != 0)
                {
                    throw new Exception(ErrorMessage("Getting width is failed!", result));
                }

                base.ImageSize = new System.Drawing.Size((int)widthValue.nCurValue, (int)heightValue.nCurValue);


                result = MyCamera.MV_CC_SetImageNodeNum_NET(GrabResultBufferSize);
                if (result != 0)
                {
                    throw new Exception(ErrorMessage("MV_CC_SetImageNodeNum_NET", result));
                }

                
                ///
                //result = MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                //if (result != 0)
                //{
                //    throw new Exception(ErrorMessage("Trigger Mode set failed!", result));
                //}

                //result = MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)cameraInfoHIK.TriggerSource);
                //if (result != 0)
                //{
                //    throw new Exception(ErrorMessage("TriggerSource set failed!", result));
                //}

                // Register image callback
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackFunc);
                result = MyCamera.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, (IntPtr)cameraInfoHIK.Index);
                if (MyCamera.MV_OK != result)
                {
                    throw new Exception(ErrorMessage("Register image callback failed!", result));
                }
            }
        }

        private void ImageCallbackFunc(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            MyCamera.MV_FRAME_OUT imageinfo = new MyCamera.MV_FRAME_OUT();

            imageinfo.pBufAddr = pData;
            imageinfo.stFrameInfo = pFrameInfo;

            PushImageToBuffer(imageinfo);
        }

        private void PushImageToBuffer(MyCamera.MV_FRAME_OUT image)
        {
            Debug.WriteLine("CameraPylonLine.OnImageGrabbed().....................");
            MyCamera.MV_FRAME_OUT? grabResult= image;
            try
            {
                if (image.stFrameInfo.nLostPacket ==0)
                {
                    lock (GrabResultBuffer)
                    {
                        if (GrabResultBuffer.Count > GrabResultBufferSize)
                        {
                            var taken = GrabResultBuffer[0];
                            GrabResultBuffer.Remove(taken);
                            //taken.Dispose();
                            Debug.WriteLine("CameraPylonLine.Camera Buffer has overrun !!!!!!!!!!!!!!!!");
                        }
                        GrabResultBuffer.Add(grabResult); //유효범위가 카메라에 세팅한 버퍼 사이즈임. 맞는지 확인필요.
                    }
                    //if (_grabResult.PayloadTypeValue == PayloadType.ChunkData)
                    //{
                    //}

                    IntPtr ptr = (IntPtr)image.stFrameInfo.nFrameNum; 
                    Debug.WriteLine(string.Format("CameraHIKLine.PushImageToBuffer.(nFrameNum:{0}, nFrameCounter:{1})", image.stFrameInfo.nFrameNum, image.stFrameInfo.nFrameCounter));
                    base.ImageGrabbedCallback(ptr);
                    if (base.remainGrabCount == 0) // 전체 그랩 (1~N장) 완료시.
                    {
                     //   isWholeGrabDone.Set();
                        Debug.WriteLine(string.Format("CameraPylonLine.OnImageGrabbed() >> isWholeGrabDone.Set() "));
                    }
                    //GrabResult.Dispose();
                }
                else
                {
                   // GrabFailed?.Invoke(e.GrabResult.ErrorCode, e.GrabResult.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }//*/

        private Image2D PopImageFromBuffer(long imageID) //ID는 1부터 시작 됨, 0이며 마지막 이미지 리턴
        {
            Image2D imageD = null;
            lock (GrabResultBuffer)
            {
                MyCamera.MV_FRAME_OUT? grabResult = null;

                if (imageID == 0)
                {
                    if (GrabResultBuffer.Count > 0)
                        grabResult = GrabResultBuffer[GrabResultBuffer.Count - 1];
                }
                else
                    grabResult = GrabResultBuffer.Find(a => a.Value.stFrameInfo.nFrameNum == imageID);

                if (grabResult != null)
                {
                    switch (grabResult.Value.stFrameInfo.enPixelType)
                    {
                        case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                            byte[] temp = new byte[grabResult.Value.stFrameInfo.nFrameLen];
                            Marshal.Copy(grabResult.Value.pBufAddr, temp, 0, (int)grabResult.Value.stFrameInfo.nFrameLen);
                            imageD = new Image2D();
                            imageD.Initialize(
                                grabResult.Value.stFrameInfo.nWidth,
                                grabResult.Value.stFrameInfo.nHeight,
                                  CameraInfo.GetNumBand(),
                                0, temp);
                            break;
                        default:
                            break;
                    }
                    //switch (grabResult.PixelTypeValue)
                    //{
                    //    case Basler.Pylon.PixelType.Mono8:
                    //        imageD = new Image2D();
                    //        //imageD.Initialize(grabResult.Width, grabResult.Height, 1);
                    //        imageD.Initialize(ImageSize.Width, ImageSize.Height, 1);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //imageD.SetData(grabResult.PixelData as byte[]);

                    //if (grabResult.PayloadTypeValue == PayloadType.ChunkData)
                    //{
                    //    if (imageD != null)
                    //    {
                    //        int w = 0, h = 0;
                    //        if (grabResult.ChunkData[PLChunkData.ChunkWidth].IsReadable)
                    //            w = (int)grabResult.ChunkData[PLChunkData.ChunkWidth].GetValue();

                    //        if (grabResult.ChunkData[PLChunkData.ChunkHeight].IsReadable)
                    //            h = (int)grabResult.ChunkData[PLChunkData.ChunkHeight].GetValue();

                    //        imageD.Tag = new CameraInfoPylonBufferTag((UInt64)grabResult.ID, new Size(w, h), (int)grabResult.BlockID);
                    //        Debug.WriteLine(string.Format("♠ ChunkWidth : {0}, ChunkHeight : {1})", w, h));
                    //    }
                    //}
                    GrabResultBuffer.Remove(grabResult);
     
                }
            }
            return imageD;
        }

        public override void Release()
        {
            if (IsReleaseBuf == false)
            {
                Marshal.FreeHGlobal(BufForDriver);
            }

            MyCamera.MV_CC_StopGrabbing_NET();
            MyCamera.MV_CC_CloseDevice_NET();
            MyCamera.MV_CC_DestroyDevice_NET();
        }

        private string ErrorMessage(string csMessage, int nErrorNum)
        {
            string errorMsg = nErrorNum == 0 ? csMessage : csMessage + ": Error =" + string.Format("{0:X}", nErrorNum);
            switch (nErrorNum)
            {
                case MyCamera.MV_E_HANDLE: errorMsg += " Error or invalid handle "; break;
                case MyCamera.MV_E_SUPPORT: errorMsg += " Not supported function "; break;
                case MyCamera.MV_E_BUFOVER: errorMsg += " Cache is full "; break;
                case MyCamera.MV_E_CALLORDER: errorMsg += " Function calling order error "; break;
                case MyCamera.MV_E_PARAMETER: errorMsg += " Incorrect parameter "; break;
                case MyCamera.MV_E_RESOURCE: errorMsg += " Applying resource failed "; break;
                case MyCamera.MV_E_NODATA: errorMsg += " No data "; break;
                case MyCamera.MV_E_PRECONDITION: errorMsg += " Precondition error, or running environment changed "; break;
                case MyCamera.MV_E_VERSION: errorMsg += " Version mismatches "; break;
                case MyCamera.MV_E_NOENOUGH_BUF: errorMsg += " Insufficient memory "; break;
                case MyCamera.MV_E_UNKNOW: errorMsg += " Unknown error "; break;
                case MyCamera.MV_E_GC_GENERIC: errorMsg += " General error "; break;
                case MyCamera.MV_E_GC_ACCESS: errorMsg += " Node accessing condition error "; break;
                case MyCamera.MV_E_ACCESS_DENIED: errorMsg += " No permission "; break;
                case MyCamera.MV_E_BUSY: errorMsg += " Device is busy, or network disconnected "; break;
                case MyCamera.MV_E_NETER: errorMsg += " Network error "; break;
            }

            LogHelper.Debug(LoggerType.Error, $"Error : {errorMsg}");
            return errorMsg;
        }

        public override void GrabOnce()
        {
            throw new NotImplementedException();
        }

        //public override void GrabOnceAsync()
        //{
        //    IsGrabDoneState = false;

        //    IntPtr m_pBufForDriver = Marshal.AllocHGlobal((int)PayLoadSize);

        //    IsReleaseBuf = false;

        //    int nRet = MyCamera.MV_CC_StartGrabbing_NET();
        //    if (MyCamera.MV_OK != nRet)
        //    {
        //        ErrorMessage("Start grabbing failed", nRet);
        //        return;
        //    }

        //    nRet = MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");

        //    lock (BufForDriverLock)
        //    {
        //        nRet = MyCamera.MV_CC_GetOneFrameTimeout_NET(m_pBufForDriver, PayLoadSize, ref frameInfo, 1000);
        //        if (nRet != 0)
        //        {
        //            ErrorMessage("Error", nRet);
        //            MyCamera.MV_CC_StopGrabbing_NET();
        //            return;
        //        }
        //    }

        //    byte[] temp = new byte[PayLoadSize];
        //    Marshal.Copy(m_pBufForDriver, temp, 0, (int)PayLoadSize);
        //    Marshal.FreeHGlobal(m_pBufForDriver);

        //    LastGrabbedImage = new Image2D();
        //    LastGrabbedImage.Initialize(frameInfo.nWidth, frameInfo.nHeight, CameraInfo.GetNumBand(), 0, temp);
        //    //Stop();
        //}

        //public override void GrabOnceAsyncBySoftwareTrigger()
        //{
        //    if (IsGrabbing == true)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        int nRet = MyCamera.MV_CC_StartGrabbing_NET();
        //        if (MyCamera.MV_OK != nRet)
        //        {
        //            ErrorMessage("Start grabbing failed", nRet);
        //            return;
        //        }
        //        IsGrabbing = true;
        //    }
        //}

        public override void GrabMulti(int grabCount)
        {
            IsGrabDoneState = false;

            base.Reset();
            //lock (grabCountLockObj)
            base.remainGrabCount = grabCount;
            base.grabbedCount = 0;
          //  ClearGrabBuffer();



            int nRet = MyCamera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("Start grabbing failed", nRet);
                return;
            }
            IsGrabbing = true;
        }

        public override void Stop()
        {
            int nRet = MyCamera.MV_CC_StopGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("Start grabbing failed", nRet);
                return;
            }
            IsGrabDoneState = true;
            IsGrabbing = false;
        }

        public bool TriggerOn()
        {
            BufForDriver = Marshal.AllocHGlobal((int)PayLoadSize);
            IsReleaseBuf = false;

            int nRet = MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("Trigger Software Fail!", nRet);
                return false;
            }

            //lock (BufForDriverLock)
            {
                nRet = MyCamera.MV_CC_GetOneFrameTimeout_NET(BufForDriver, PayLoadSize, ref frameInfo, 1000);
                if (nRet != 0)
                {
                    ErrorMessage("Error", nRet);
                    return false;
                }
            }

            byte[] temp = new byte[PayLoadSize];
            Marshal.Copy(BufForDriver, temp, 0, (int)PayLoadSize);
            Marshal.FreeHGlobal(BufForDriver);

            LastGrabbedImage = new Image2D();
            LastGrabbedImage.Initialize(frameInfo.nWidth, frameInfo.nHeight, CameraInfo.GetNumBand(), 0, temp);

            //Task.Run(() => ImageGrabbedCallback()); ///////////////////////////////////////////////////////////////뭐냐..
            return true;
        }

        public override bool IsGrabDone()
        {
            return IsGrabDoneState;
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            //IntPtr.Zero) //마지막 이미지 리턴
            return PopImageFromBuffer((long)ptr);
        }

        public int SetNumBand(MyCamera.MvGvspPixelType type)
        {
            switch (type)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return 1;
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return 3;
                default:
                    return 1;
            }
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            int nRet = MyCamera.MV_OK;

            nRet = MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            if (nRet != 0)
            {
                ErrorMessage("Acquisition Mode set failed!", nRet);
            }

            switch (triggerMode)
            {
                case TriggerMode.Software:
                    nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    if (nRet != 0)
                    {
                        ErrorMessage("Trigger Mode set failed!", nRet);
                    }

                    //nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                    //if (nRet != 0)
                    //{
                    //    ErrorMessage("Software trigger set failed!", nRet);
                    //    return;
                    //}
                    break;

                case TriggerMode.Hardware:
                    nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                    if (nRet != 0)
                    {
                        ErrorMessage("Trigger Mode set failed!", nRet);
                    }

                    nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)((CameraInfoHIKLine)CameraInfo).TriggerSource);
                    if (nRet != 0)
                    {
                        ErrorMessage("Software trigger set failed!", nRet);
                        return;
                    }
                    break;

                case TriggerMode.Off:
                    nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    if (nRet != 0)
                    {
                        ErrorMessage("Trigger Mode set failed!", nRet);
                    }

                    //nRet = MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                    //if (nRet != 0)
                    //{
                    //    ErrorMessage("Software trigger set failed!", nRet);
                    //    return;
                    //}
                    break;
                default: break;
            }
        }
        
      

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            int nRet = MyCamera.MV_CC_SetExposureTime_NET(exposureTimeMs*1000);
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("Set exposure time is Failed", nRet);
            }
            return true;
        }

        public void SetGain(float gain = 10.0f)
        {
            int nRet = MyCamera.MV_CC_SetGain_NET(gain);
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("Set Gain is Failed", nRet);
            }
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            int nRet = MyCamera.MV_CC_SetTriggerDelay_NET(triggerDelayUs);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Setting trigger delay is failed");
                return;
            }
            return;
        }

        private void ReConnectTry(int tryCount = 30, int sleepTime = 2000)
        {
            int nRet = MyCamera.MV_OK;
            MyCamera.MV_CC_CloseDevice_NET();
            MyCamera.MV_CC_DestroyDevice_NET();

            nRet = MyCamera.MV_CC_CreateDevice_NET(ref deviceInfo);
            if (MyCamera.MV_OK != nRet)
            {
                throw new Exception(ErrorMessage("Device Create Fail!", nRet));
            }

            int reTry = 0;
            while (true)
            {
                Thread.Sleep(sleepTime);
                nRet = MyCamera.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    if (tryCount < reTry++)
                    {
                        throw new Exception(ErrorMessage("Device Create Fail!", nRet));
                    }
                    else
                    {
                        LogHelper.Error(LoggerType.Device, $"HIK Camera Open Error. ReConnectTry...{reTry}");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public override float GetDeviceExposureMs()
        {
            var pstValue = new MyCamera.MVCC_FLOATVALUE();
            MyCamera.MV_CC_GetExposureTime_NET(ref pstValue); //us 단위임.
            return pstValue.fCurValue *1000;
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
            //Acquisition Line Rate Control Enable
            //Enables manual control of the camera Line rate.
            //Node Name: AcquisitionLineRateEnable
            //Type: Boolean
            int nRet=0;
            nRet = MyCamera.MV_CC_SetBoolValue_NET("AcquisitionLineRateEnable", true);
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("MV_CC_SetBoolValue_NET", nRet);
                return false;
            }

            //Acquisition Line Rate(Hz)
            //Controls the acquisition rate at which the Lines are captured.
            //Node Name: AcquisitionLineRate
            //Type: Integer
            uint nHz = (uint)Math.Abs(hz);
            nRet = MyCamera.MV_CC_SetAcquisitionLineRate_NET(nHz);
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("MV_CC_SetAcquisitionLineRate_NET", nRet);
                return false;
            }

            return true;
        }

        public override float GetAcquisitionLineRate()
        {
            int nRet = 0;
            MyCamera.MVCC_INTVALUE pstValue = new MyCamera.MVCC_INTVALUE();
            nRet = MyCamera.MV_CC_GetAcquisitionLineRate_NET(ref pstValue);
            if (MyCamera.MV_OK != nRet)
            {
                ErrorMessage("MV_CC_GetAcquisitionLineRate_NET", nRet);
                return -1;
            }
            return  pstValue.nCurValue;            
        }

    }
}
