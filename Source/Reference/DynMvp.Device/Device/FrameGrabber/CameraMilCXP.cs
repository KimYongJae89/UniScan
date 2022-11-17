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
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraMilCXPBufferTag : CameraBufferTag
    {
        public int BufferId { get; private set; }

        public CameraMilCXPBufferTag(UInt64 frameId, Size frameSize, int bufferId) : base(frameId, frameSize)
        {
            this.BufferId = bufferId;
        }

        public override string ToString()
        {
            return $"Buffer {this.BufferId} / Frame {this.FrameId} / Width {this.FrameSize.Width} / Height {this.FrameSize.Height}";
        }
    }


    public class CameraMilCXP : Camera
    {
        public MilSystem MilSystem { get; private set; }
        public MIL_ID DigitizerId { get; private set; }

        public int Digitizer => (int)DigitizerId;

        uint BufferPoolCount = 15;
        MIL_ID whiteKernel = MIL.M_NULL;

        GCHandle thisHandle;
        MIL_DIG_HOOK_FUNCTION_PTR frameTransferEndPtr = null;
        MIL_DIG_HOOK_FUNCTION_PTR processingFunctionPtr = null;

        MIL_ID HugeGrabImageBuffer = MIL.M_NULL;
        MIL_ID[] grabImageBuffer = null;
        Dictionary<MIL_ID, CameraMilCXPBufferTag> tagDic = new Dictionary<MIL_ID, CameraMilCXPBufferTag>();

        MIL_ID bayerSourceImage = MIL.M_NULL;
        MIL_ID lastGrabbedBufferId = MIL.M_NULL;

        public CameraMilCXP(CameraInfo cameraInfo) : base(cameraInfo) { }

        public override void Initialize(bool calibrationMode)
        {
            base.Initialize(calibrationMode);
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;

            BufferPoolCount = cameraInfoMil.GrabBufferCount;
            grabImageBuffer = new MIL_ID[BufferPoolCount];

            this.MilSystem = GrabberMil.GetMilSystem(cameraInfoMil.SystemType, cameraInfoMil.SystemNum);
            if (this.MilSystem == null)
            {
                LogHelper.Error(LoggerType.Error, "MilSystem is empty. Skip create the digitizer.");

                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "CameraMilCXP Exception - MilSystem Allocation Fail", null, "");
                return;
            }

            string dcfFileName = cameraInfoMil.GetDcfFile(calibrationMode);
            if (!System.IO.File.Exists(dcfFileName))
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "CameraMilCXP Exception - DCF is not exist", null, "");

            this.DigitizerId = MIL.MdigAlloc(this.MilSystem.SystemId, cameraInfoMil.DigitizerNum, dcfFileName, MIL.M_DEFAULT, MIL.M_NULL);
            if (this.DigitizerId == MIL.M_NULL)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Digitizer Allocation is Failed.{0}, {1}, {2}, {3}",
                    cameraInfoMil.SystemType.ToString(), this.MilSystem.SystemId, cameraInfoMil.DigitizerNum, cameraInfoMil.CameraType.ToString()));
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "CameraMilCXP Exception - MilDigitizer Allocation Fail", null, "");
                return;
            }

            MIL.MdigControl(this.DigitizerId, MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS);
            MIL.MdigControl(this.DigitizerId, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE);
            
            this.thisHandle = GCHandle.Alloc(this);
            this.frameTransferEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameTransferEnd);
            MIL.MdigHookFunction(this.DigitizerId, MIL.M_GRAB_END, this.frameTransferEndPtr, GCHandle.ToIntPtr(thisHandle));

            MIL_INT width = 0;
            MIL_INT height = 0;
            MIL_INT band = 0;

            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_X, ref width);
            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_Y, ref height);

            ImageSize = new Size((int)width, (int)height);

            MIL.MdigInquire(this.DigitizerId, MIL.M_SIZE_BAND, ref band);
            NumOfBand = (int)band;

            ImagePitch = (int)width * NumOfBand;

            if (NumOfBand == 1)
            {
                if(cameraInfoMil.UseChunkMemory == true) //specialy grabonce() to huge memory 
                {
                    HugeGrabImageBuffer = MIL.MbufAlloc2d(this.MilSystem.SystemId, width, height* grabImageBuffer.Length, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_GRAB, MIL.M_NULL);
                    MIL.MbufClear(HugeGrabImageBuffer, 0);

                    MIL_INT w = 0;
                    MIL_INT h = 0;
                    MIL_INT p = 0;

                    MIL.MbufInquire(this.HugeGrabImageBuffer, MIL.M_SIZE_X, ref w);
                    MIL.MbufInquire(this.HugeGrabImageBuffer, MIL.M_SIZE_Y, ref h);
                    MIL.MbufInquire(this.HugeGrabImageBuffer, MIL.M_PITCH, ref p);

                    int childHeight = (int)height; //DCF에서 설정한 Frame Size of Height
                    uint childCount = BufferPoolCount;
                    int offsetY = 0;

                    for (int n = 0; n < childCount; n++)
                    {
                        offsetY = n * childHeight;
                        MIL_ID childid = MIL.MbufChild2d(HugeGrabImageBuffer, (int)0, (int)offsetY, (int)width, (int)childHeight, MIL.M_NULL);
                        MIL.MbufClear(childid, 0);
                        grabImageBuffer[n] = childid;
                    }
                    tagDic.Add(HugeGrabImageBuffer, null);
                }
                else //normal
                {
                    for (int i = 0; i < grabImageBuffer.Length; i++)
                    {
                        MIL_ID id = MIL.MbufAlloc2d(this.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_GRAB, MIL.M_NULL);
                        MIL.MbufClear(id, 0);

                        //this.bytess[i] = new byte[width * height];
                        //GCHandle handle = GCHandle.Alloc(this.bytess[i]);
                        //IntPtr ptr = GCHandle.ToIntPtr(handle);
                        //MIL_ID id = MIL.MbufCreate2d(this.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_PHYSICAL_ADDRESS, MIL.M_DEFAULT, (ulong)ptr, MIL.M_NULL);
                        grabImageBuffer[i] = id;
                        tagDic.Add(id, null);

                        //image buffer cannot be allocated.
                        //you can increase the total of non-paged memory using minconfig.
                    }
                }
            }
            else
            {
                for (int i = 0; i < grabImageBuffer.Length; i++)
                {
                    MIL_ID id = MIL.MbufAllocColor(this.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                    grabImageBuffer[i] = id;
                    tagDic.Add(id, null);
                }
            }

            IntPtr hostAddress = IntPtr.Zero;
            var ret = MIL.MbufInquire(grabImageBuffer[0], MIL.M_HOST_ADDRESS, hostAddress);
            var ret2 = MIL.MbufInquire(grabImageBuffer[1], MIL.M_HOST_ADDRESS, MIL.M_NULL);

            if (cameraInfoMil.BayerCamera == true)
            {
                this.bayerSourceImage = MIL.MbufAllocColor(this.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                MIL.MbufPut(whiteKernel, cameraInfoMil.WhiteBalanceCoefficient);
                BayerType = cameraInfoMil.BayerType;
            }
        }

        public void SetOffsetX(int offsetX)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            MIL_INT i = offsetX;
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "OffsetX", MIL.M_TYPE_INT64, ref i);
        }

        public int GetOffsetX()
        {
            ThrowIfSlaveClient();

            MIL_INT i = 0;
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "OffsetX", MIL.M_TYPE_INT64, ref i);
            return (int)i;
        }

        public void SetReverseX(bool reverseX)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "ReverseX", MIL.M_TYPE_BOOLEAN, ref reverseX);
        }

        public bool GetReverseX()
        {
            ThrowIfSlaveClient();

            bool b = false;
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "ReverseX", MIL.M_TYPE_BOOLEAN, ref b);
            return b;
        }

        public void SetTriggerRescalerRate(float triggerRescalerRate)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            double d = triggerRescalerRate;
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerRescalerRate", MIL.M_TYPE_DOUBLE, ref d);
        }

        public float GetTriggerRescalerRate()
        {
            ThrowIfSlaveClient();

            double d = 0;
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerRescalerRate", MIL.M_TYPE_DOUBLE, ref d);
            return (float)d;
        }

        public void SetTriggerRescalerMode(bool triggerRescalerMode)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            string str = triggerRescalerMode ? "On" : "Off";
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerRescalerMode", MIL.M_TYPE_STRING, str);
        }

        public bool GetTriggerRescalerMode()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerRescalerMode", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();

            if (str == "On")
                return true;
            return false;
        }

        public void SetScanDirectionType(EScanDirectionType scanDirectionType)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "ScanDirection", MIL.M_TYPE_STRING, scanDirectionType.ToString());
        }

        public EScanDirectionType GetScanDirectionType()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "ScanDirection", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();

            return (EScanDirectionType)Enum.Parse(typeof(EScanDirectionType), str);
        }

        public void SetAnalogGain(EAnalogGain analogGain)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "AnalogGain", MIL.M_TYPE_STRING, analogGain.ToString());            
        }

        public EAnalogGain GetAnalogGain()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "AnalogGain", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();

            return (EAnalogGain)Enum.Parse(typeof(EAnalogGain), str);
        }

        public override void SetTriggerDelay(int exposureTimeUs)
        {
           // MIL.MdigControl(digitizerId, MIL.M_GRAB_TRIGGER_DELAY, exposureTimeUs * 1000);
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Grab, "CameraMil - Release Mil System");

            Array.ForEach(grabImageBuffer, f => MIL.MbufFree(f));
            Array.Clear(grabImageBuffer, 0, grabImageBuffer.Length);

            if (HugeGrabImageBuffer != MIL.M_NULL)
            {
                MIL.MbufFree(HugeGrabImageBuffer);
                HugeGrabImageBuffer = MIL.M_NULL;
            }

            if (whiteKernel != MIL.M_NULL)
            {
                MIL.MbufFree(whiteKernel);
                whiteKernel = MIL.M_NULL;
            }

            if (bayerSourceImage != MIL.M_NULL)
            {
                MIL.MbufFree(bayerSourceImage);
                bayerSourceImage = MIL.M_NULL;
            }

            if (this.DigitizerId != MIL.M_NULL)
            {
                MIL.MdigFree(this.DigitizerId);
                this.DigitizerId = MIL.M_NULL;
            }

            if (this.MilSystem != null)
            {
                this.MilSystem.Free();
                this.MilSystem = null;
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
            //LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin FrameTransferEnd");

            // Frame Count를 얻을 수 없다??
            //MIL_ID frameBlockId = MIL.M_NULL;
            //MIL.MdigGetHookInfo(EventId, MIL.M_GC_FRAME_BLOCK_ID, ref frameBlockId);

            MIL_ID bufferId = MIL.M_NULL;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref bufferId);

            MIL_INT sizeX = 0;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_REGION_SIZE_X, ref sizeX);

            MIL_INT sizeY = 0;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_REGION_SIZE_Y, ref sizeY);

            this.tagDic[bufferId] = new CameraMilCXPBufferTag(this.grabbedCount + 1, new Size((int)sizeX, (int)sizeY), Array.IndexOf(this.grabImageBuffer, bufferId));

            lastGrabbedBufferId = bufferId;
            ImageGrabbedCallback(bufferId);

            //MIL.MbufSave("D:\\Data.bmp", grabbedImage);

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End FrameTransferEnd");

            return MIL.M_NULL;
        }

        //static MIL_INT ProcessingFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        //{
        //    if (IntPtr.Zero.Equals(HookDataPtr) == true)
        //        return MIL.M_NULL;

        //    MIL_ID currentImageId = MIL.M_NULL;
        //    MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

        //    GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

        //    // get a reference to the DigHookUserData object
        //    CameraMilCXP cameraMil = hUserData.Target as CameraMilCXP;
        //    cameraMil.ImageGrabbedCallback(currentImageId);

        //    return MIL.M_NULL;
        //}

        public TriggerMode GetTriggerMode()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerMode", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();

            if (str == "On")
                return TriggerMode.Hardware;
            return TriggerMode.Software;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin SetTriggerMode");

            base.SetTriggerMode(triggerMode, triggerType);

            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerSelector", MIL.M_TYPE_STRING, "LineStart");

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerMode", MIL.M_TYPE_STRING, "Off");
            }
            else
            {
                MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerMode", MIL.M_TYPE_STRING, "On");
                MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TriggerActivation", MIL.M_TYPE_STRING, triggerType.ToString());
            }

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End SetTriggerMode");
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                ptr = lastGrabbedBufferId;

            MIL_ID bufferId = (MIL_ID)ptr;
            if (ptr != null)
            {
                if (NumOfBand == 1)
                {
                    if (BayerCamera == true)
                    {
                        MIL.MbufBayer(this.bayerSourceImage, bufferId, whiteKernel, (long)BayerType);
                        return CreateGrabbedImage(bufferId, true);
                    }
                    else
                    {
                        return CreateGrabbedImage(bufferId, false);
                    }
                }
                else if (NumOfBand == 3)
                {
                    return CreateGrabbedImage(bufferId, true);
                }
            }

            return null;
        }

        public ImageD CreateGrabbedImage(MIL_ID milId, bool colorImage)
        {
            if (milId == MIL.M_NULL)
                return null;


            Image2D image2d;
            if (false)
            {
                byte[] bytes = new byte[ImageSize.Width * ImageSize.Height];
                MIL.MbufGet(milId, bytes);

                image2d = new Image2D();
                image2d.Initialize(ImageSize.Width, ImageSize.Height, colorImage ? 3 : 1, ImageSize.Width, bytes);
                image2d.Tag = this.tagDic[milId];
            }
            else
            {
                int grabbedImageWidth =(int) MIL.MbufInquire(milId, MIL.M_SIZE_X);
                int grabbedImageHeight =(int) MIL.MbufInquire(milId, MIL.M_SIZE_Y);
                int grabbedPitch = (int)MIL.MbufInquire(milId, MIL.M_PITCH, MIL.M_NULL);
                IntPtr addr = (IntPtr)MIL.MbufInquire(milId, MIL.M_HOST_ADDRESS, MIL.M_NULL);

                image2d = new Image2D();
                image2d.Initialize((int)grabbedImageWidth, (int)grabbedImageHeight, colorImage ? 3 : 1, grabbedPitch, addr);
                image2d.Tag = this.tagDic[milId];
            }
            return image2d;
        }

        public bool CopyGrayImage(MIL_ID milId, ImageD image)
        {
            if (milId == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height];

            MIL.MbufGet(milId, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public bool CopyColorImage(MIL_ID milId, ImageD image)
        {
            if (milId == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height * 3];

            MIL.MbufGetColor(milId, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, milBuf);

            image2d.SetData(milBuf);

            return true;
        }
        //정지화상용으로의 특별히 동작하고 있음,
        //Matrox 보드가 특이해서 15만라인 한방 획득에 제한이있음.
        //큰 메모리 할당후 차일드 버퍼ID를 할당하여 그랩함.
        //매 프레임마다 콜백 작동하면, 기존 정지화상과 호환되지 않아, 동기적으로 모두 받은후 콜백 시킴
        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabOnce");

            if (SetupGrab() == false)
                return;

            this.isStopped.Reset();
            remainGrabCount = 1;
            //if (processingFunctionPtr != null)
            //    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

            
            int childHeight = 10000;
            int childCount = (int)(ImageSize.Height / childHeight);
            int offsetY = 0;

            //Stop Callback hookfunction
            MIL.MdigHookFunction(this.DigitizerId, MIL.M_GRAB_END+ MIL.M_UNHOOK, this.frameTransferEndPtr, GCHandle.ToIntPtr(thisHandle));
          
            MIL.MdigProcess(this.DigitizerId, grabImageBuffer, grabImageBuffer.Length, 
                MIL.M_SEQUENCE +MIL.M_COUNT(grabImageBuffer.Length),
                MIL.M_SYNCHRONOUS, MIL.M_NULL, GCHandle.ToIntPtr(thisHandle));

            Thread.Sleep(0);
            lastGrabbedBufferId = HugeGrabImageBuffer;
            ImageGrabbedCallback(HugeGrabImageBuffer);
            //revival Callback hookfunction
            MIL.MdigHookFunction(this.DigitizerId, MIL.M_GRAB_END, this.frameTransferEndPtr, GCHandle.ToIntPtr(thisHandle));
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabContinuous");

            if (SetupGrab() == false)
                return;

            this.remainGrabCount = grabCount;

            if (grabCount < 0)
            {
                //if (processingFunctionPtr != null)
                //    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, grabImageBuffer.Length, MIL.M_START, MIL.M_DEFAULT, MIL.M_NULL, GCHandle.ToIntPtr(thisHandle));
            }
            else if (grabCount > 0)
            {
                //if (processingFunctionPtr != null)
                //    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

                int wishCount = grabCount < (int)BufferPoolCount ? grabCount : (int)BufferPoolCount;

                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, grabImageBuffer.Length, MIL.M_SEQUENCE + MIL.M_COUNT(wishCount), MIL.M_ASYNCHRONOUS, MIL.M_NULL, GCHandle.ToIntPtr(thisHandle));
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (triggerMode == TriggerMode.Software)
            {
                //MIL.MdigHalt(digitizerId);
                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, 2, MIL.M_STOP, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
            else
            {
                MIL.MdigProcess(this.DigitizerId, grabImageBuffer, 2, MIL.M_STOP, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
            Thread.Sleep(50);
        }



        public override List<ImageD> GetImageBufferList()
        {
            throw new NotImplementedException();
        }

        public override int GetImageBufferCount()
        {
            return this.grabImageBuffer.Length;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return false;

            // Operation Mode가 Area 일때만 가능함.
            if (GetOperationMode() != ScanMode.Area)
                return false;

            double ExposureTime = (double)exposureTimeMs * 1000;
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE,$"ExposureTime", MIL.M_TYPE_DOUBLE, ref ExposureTime);

            return true;
        }

        public override float GetDeviceExposureMs()
        {
            ThrowIfSlaveClient();

            double exposureTime = -9.9999999999;  //milisecond
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, $"ExposureTime", MIL.M_TYPE_DOUBLE, ref exposureTime);
            return (float)(exposureTime / 1000);
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return false;

            // Operation Mode가 TDI 일때만 가능함.
            if (GetOperationMode() != ScanMode.Line)
                return false;

            double AcquisitionLineRate = hz;
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, $"AcquisitionLineRate", MIL.M_TYPE_DOUBLE, ref AcquisitionLineRate);

            return true;
        }

        public override float GetAcquisitionLineRate()
        {
            ThrowIfSlaveClient();

            double acquisitionLineRate = 0;
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, $"AcquisitionLineRate", MIL.M_TYPE_DOUBLE, ref acquisitionLineRate);
            return (float)acquisitionLineRate;
        }

        public ScanMode GetOperationMode()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "OperationMode", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();
            switch (str)
            {
                case "Area":
                    return ScanMode.Area;
                case "TDI":
                    return ScanMode.Line;
                default:
                    throw new Exception();
            }
        }

        public void SetOperationMode(ScanMode scanMode)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return ;

            string mode = "";
            switch (scanMode)
            {
                case ScanMode.Area:
                    mode = "Area";
                    break;
                case ScanMode.Line:
                    mode = "TDI";
                    break;
            }
            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "OperationMode", MIL.M_TYPE_STRING, mode);
        }

        public ETDIStages GetTdiStage()
        {
            ThrowIfSlaveClient();

            StringBuilder sb = new StringBuilder();
            MIL.MdigInquireFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TDIStages", MIL.M_TYPE_STRING, sb);
            string str = sb.ToString();

            return (ETDIStages)Enum.Parse(typeof(ETDIStages), str);
        }

        public bool SetTdiStage(ETDIStages tdiStages)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return false;

            MIL.MdigControlFeature(this.DigitizerId, MIL.M_FEATURE_VALUE, "TDIStages", MIL.M_TYPE_STRING, tdiStages.ToString());
            return true;
        }

        private void ThrowIfSlaveClient()
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                throw new InvalidOperationException("ClientType == Slave");
        }
    }
}
