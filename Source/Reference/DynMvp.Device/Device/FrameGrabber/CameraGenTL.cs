using System;
using System.Collections.Generic;
using System.Threading;

using DynMvp.Base;
using System.Diagnostics;

using Euresys;
using System.Windows.Forms;
using DynMvp.Device.Device.FrameGrabber;
using System.Drawing;
using System.Linq;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraGenTLBufferTag : CameraBufferTag
    {
        public int BufferId { get; private set; }

        public CameraGenTLBufferTag(UInt64 frameId, Size frameSize, int bufferId) : base(frameId, frameSize)
        {
            this.BufferId = bufferId;
        }

        public override string ToString()
        {
            return $"Buffer {this.BufferId} / Frame {this.FrameId} / Width {this.FrameSize.Width} / Height {this.FrameSize.Height}";
        }
    }

    public class _GenTL
    {
        static Euresys.GenTL genTL = null;
        public static Euresys.GenTL Instance()
        {
            if (genTL == null)
                genTL = new Euresys.GenTL();
            return genTL;
        }
    }

    public class CameraGenTL : Camera
    {
        class GenTLWrapper
        {
            bool writeLog = false;
            Euresys.RGBConverter.RGBConverter converter = null;
            Euresys.GenTL genTL = null;
            public Euresys.EGrabberCallbackSingleThread Grabber => this.grabber;
            Euresys.EGrabberCallbackSingleThread grabber = null;

            public GenTLWrapper(bool writeLog, int interfaceID)
            {
                this.writeLog = writeLog;
                this.genTL = _GenTL.Instance();
                this.converter = new Euresys.RGBConverter.RGBConverter(genTL);
                //this.grabber = new Euresys.EGrabberCallbackSingleThread(genTL);
                this.grabber = new Euresys.EGrabberCallbackSingleThread(genTL, interfaceID);
            }

            internal void Dispose()
            {
                this.grabber?.stop();
                this.grabber?.Dispose();
                this.grabber = null;

                this.converter?.Dispose();
                converter = null;

                this.genTL?.Dispose();
                genTL = null;
            }

            // Script
            internal void runScript(string script)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::runScript - script: {0}", script));
                grabber.runScript(script);
            }

            // Interface
            internal void setStringInterfaceModule(string key, string value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setStringInterfaceModule - Key: {0}, Value: {1}", key, value));
                grabber.setStringInterfaceModule(key, value);
            }
            internal string getStringInterfaceModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringInterfaceModule - Key: {key}");
                var value = grabber.getStringInterfaceModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringInterfaceModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }

            internal void setIntegerInterfaceModule(string key, int value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setIntegerInterfaceModule - Key: {0}, Value: {1}", key, value));
                grabber.setIntegerInterfaceModule(key, value);
            }
            internal long getIntegerInterfaceModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerInterfaceModule - Key: {key}");
                var value = grabber.getIntegerInterfaceModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::getIntegerInterfaceModule - Key: {0}, Value: {1}", key, value));
                return value;
            }

            //Devices
            internal void setStringDeviceModule(string key, string value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setStringDeviceModule - Key: {0}, Value: {1}", key, value));
                grabber.setStringDeviceModule(key, value);
            }

            internal void setFloatDeviceModule(string key, double value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setFloatDeviceModule - Key: {0}, Value: {1}", key, value));
                grabber.setFloatDeviceModule(key, value);
            }

            //Remove Devices
            internal string[] getStringListRemoteModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringListRemoteModule - Key: {key}");
                var value = grabber.getStringListRemoteModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringListRemoteModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {string.Join("/", value)}");
                return value;
            }

            internal void setStringRemoteModule(string key, string value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setStringRemoteModule - Key: {0}, Value: {1}", key, value));
                grabber.setStringRemoteModule(key, value);
            }
            internal string getStringRemoteModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringRemoteModule - Key: {key}");
                var value = grabber.getStringRemoteModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringRemoteModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }

            internal void setIntegerRemoteModule(string key, long value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setIntegerRemoteModule - Key: {0}, Value: {1}", key, value));
                grabber.setIntegerRemoteModule(key, value);
            }
            internal long getIntegerRemoteModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerRemoteModule - Key: {key}");
                var value = grabber.getIntegerRemoteModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerRemoteModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }

            internal void setFloatRemoteModule(string key, double value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setFloatRemoteModule - Key: {0}, Value: {1}", key, value));
                grabber.setFloatRemoteModule(key, value);
            }
            internal double getFloatRemoteModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getFloatRemoteModule - Key: {key}");
                var value = grabber.getFloatRemoteModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getFloatRemoteModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }

            // Stream
            internal void setStringStreamModule(string key, string value)
            {
                LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setStringStreamModule - Key: {0}, Value: {1}", key, value));
                grabber.setStringStreamModule(key, value);
            }
            internal string getStringStreamModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerStreamModule - Key: {key}");
                var value = grabber.getStringStreamModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getStringStreamModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }

            internal void setIntegerStreamModule(string key, int value)
            {
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, string.Format("GenTLWrapper::setIntegerStreamModule - Key: {0}, Value: {1}", key, value));
                grabber.setIntegerStreamModule(key, value);
            }
            internal long getIntegerStreamModule(string key)
            {
                LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerStreamModule - Key: {key}");
                var value = grabber.getIntegerStreamModule(key);
                if (writeLog)
                    LogHelper.Debug(LoggerType.Device, $"GenTLWrapper::getIntegerStreamModule - Key: {key}, ValueType: {value.GetType().Name}, Value: {value}");
                return value;
            }
        }

        GenTLWrapper grabber = null;
        Euresys.Buffer currentBuffer = null;

        MatroxBuffer mBuffer = null;
        IntPtr lastGrabbedImgaePtr = IntPtr.Zero;

        protected List<Image2D> grabbedImageList = new List<Image2D>();

        private Mutex imageMutex = new Mutex();

        public CameraGenTL(CameraInfo cameraInfo) : base(cameraInfo) { }

        public int GetBufferIndex(IntPtr ptr)
        {
            return grabbedImageList.FindIndex(f => f.DataPtr == ptr);
        }

        public override bool IsBinningVirtical()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;
            return cameraInfoGenTL.BinningVertical;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType == EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "TDI")
                    return;
                try
                {
                    if (triggerMode == TriggerMode.Hardware)
                    {
                        grabber.setStringRemoteModule("TriggerMode", "On");
                    }
                    else
                    {
                        grabber.setStringRemoteModule("TriggerMode", "Off");
                    }
                }
                catch (Euresys.gentl_error ex)
                {
                    LogHelper.Error(LoggerType.Error, ex.Message);
                    return;
                }
            }
        }

        protected override void SetScanMode(ScanMode scanMode)
        {
            switch (scanMode)
            {
                case ScanMode.Area:
                    SetAreaMode();
                    break;
                case ScanMode.Line:
                    SetLineScanMode();
                    break;
            }
        }

        private void SetAreaMode()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType != EClientType.Master)
                throw new Exception("Slave Camera is not supported");

            string mode = grabber.getStringRemoteModule("OperationMode");
            if (mode != "Area")
                grabber.setStringRemoteModule("OperationMode", "Area");

            string quary = GenApiQueryBuilder.EnumEntries("TDIStages", true);
            int[] entries = Array.ConvertAll(grabber.getStringListRemoteModule(quary), f =>
            {
                int tdi;
                if (!int.TryParse(f.Substring(3), out tdi))
                    return -1;
                return tdi;
            });
            int maxTdiStage = entries.Max();

            grabber.setStringRemoteModule("TDIStages", string.Format("TDI{0}", maxTdiStage));
            UpdateBuffer(cameraInfoGenTL.Width, maxTdiStage, 10, true);
        }

        private void SetLineScanMode()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType == EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "TDI")
                    grabber.setStringRemoteModule("OperationMode", "TDI");

                grabber.setStringRemoteModule("TDIStages", cameraInfoGenTL.TDIStages.ToString());
                grabber.setStringRemoteModule("ScanDirection", cameraInfoGenTL.DirectionType.ToString());
                grabber.setStringRemoteModule("PRNUMode", cameraInfoGenTL.PRNUMode.ToString());
            }
            UpdateBuffer(cameraInfoGenTL.Width, cameraInfoGenTL.Height, cameraInfoGenTL.FrameNum, true);
        }

        public override void Initialize(bool calibrationMode)
        {
            try
            {
                base.Initialize(calibrationMode);

                CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.cameraInfo;

                //genTL = new Euresys.GenTL();
                //converter = new Euresys.RGBConverter.RGBConverter(genTL);
                //grabber = new Euresys.EGrabberCallbackSingleThread(genTL);
                grabber = new GenTLWrapper(true, cameraInfoGenTL.Index);

                //CxpConnectionCheck();

                SetGrabber();

                if (calibrationMode)
                {
                    if (cameraInfoGenTL.ClientType == EClientType.Master)
                        SetScanMode(ScanMode.Area);
                }
                else
                {
                    ScanMode sm = this.cameraInfo.IsLineScan ? ScanMode.Line : ScanMode.Area;
                    SetScanMode(sm);
                }

                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', true);}");
                grabber.Grabber.onNewBufferEvent = GenTLCamCallback;
                grabber.Grabber.enableAllEvent();
            }
            catch (Euresys.gentl_error ex)
            {
                this.mBuffer?.Dispose();
                this.mBuffer = null;

                LogHelper.Error(LoggerType.Device, string.Format("CameraGenTL::Initialize - {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));

                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "GenTL Exception - {0}", new object[] { ex.Message }, "");
                //throw new CameraInitializeFailException("GenTL Exception : " + ex.Message);
            }
            catch (Exception ex)
            {
                this.mBuffer?.Dispose();
                this.mBuffer = null;

                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "GenTL Exception - {0}", new object[] { ex.Message }, "");
                //string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                //System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void CxpConnectionCheck()
        {
            int cxpHostConnectionCount = (int)grabber.getIntegerInterfaceModule("CxpHostConnectionCount");
            int[] hostConnections = new int[cxpHostConnectionCount];
            for (int i = 0; i < cxpHostConnectionCount; i++)
            {
                int deviceId = -1;

                grabber.setIntegerInterfaceModule("CxpHostConnectionSelector", i);
                string cxpConnectionState = grabber.getStringInterfaceModule("CxpConnectionState");
                if (cxpConnectionState == "Detected")
                {
                    string cxpDeviceConnectionId = grabber.getStringInterfaceModule("CxpDeviceConnectionID");
                    if (cxpDeviceConnectionId.Contains("Master"))
                        deviceId = 0;
                    else if (cxpDeviceConnectionId.Contains("Extension"))
                    {
                        int start = cxpDeviceConnectionId.IndexOf("Extension");
                        string str = cxpDeviceConnectionId.Substring(start + 9);
                        int.TryParse(str, out deviceId);
                    }
                }
                hostConnections[i] = deviceId;
            }
            hostConnections[2] = 0;
            hostConnections[3] = 1;

            int lastConnectedId = Array.FindLastIndex(hostConnections, f => f >= 0);
            if (lastConnectedId < 0)
            {
                // connected Nothing
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                        this.name, "CxpHostConnections are Empty", new object[] { }, "");
            }

            int firstDisconnectedId = Array.FindIndex(hostConnections, f => f < 0);
            if (firstDisconnectedId >= 0 && !(lastConnectedId < firstDisconnectedId))
            {
                // Something disconnected
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                        this.name, "CxpHostConnection \'{0}\' is Disconnected", new object[] { (char)(firstDisconnectedId + 65) }, "");
            }

            int next = 0;
            for (int i = 0; i < hostConnections.Length; i++)
            {
                if (hostConnections[i] == 0)
                    next = 0;

                if (hostConnections[i] != next)
                    throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                        this.name, "CxpHostConnection \'{0}\' is Missmatch - Device {1}", new object[] { (char)(i + 65), hostConnections[i] + 1 }, "");
                next++;
            }

            //int foundIndex = Array.FindIndex(tuples, f => f.Item1 && Array.IndexOf(tuples, f) != f.Item2);
            //if (foundIndex >= 0)
            //    throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
            //        this.name, "CxpHostConnection \'{0}\' is Missmatch - Device {1}", new object[] { (char)(foundIndex + 65), tuples[foundIndex].Item2 + 1 }, "");
        }

        private void SetGrabber()
        {
            SetCxpLinkConfiguration();

            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
            grabber.setStringRemoteModule("PixelFormat", "Mono8");
            switch (cameraInfoGenTL.ClientType)
            {
                case EClientType.Master:
                    // Interface
                    grabber.setStringInterfaceModule("LineInputToolSelector", "LIN1");
                    grabber.setStringInterfaceModule("LineInputToolSource", cameraInfoGenTL.LineInputToolSource.ToString());
                    grabber.setStringInterfaceModule("LineInputToolActivation", "RisingEdge");

                    // Device
                    grabber.setStringDeviceModule("CameraControlMethod", "RC");
                    grabber.setFloatDeviceModule("CycleMinimumPeriod", 3.36);

                    grabber.setStringDeviceModule("CycleTriggerSource", "Immediate");

                    grabber.setStringDeviceModule("StartOfSequenceTriggerSource", "LIN1");
                    grabber.setStringDeviceModule("EndOfSequenceTriggerSource", "SequenceLength");
                    grabber.setStringDeviceModule("SequenceLength", "1");

                    grabber.setStringDeviceModule("CxpTriggerMessageFormat", "Pulse");

                    // Romote Module
                    grabber.setStringRemoteModule("TDIStages", cameraInfoGenTL.TDIStages.ToString());

                    grabber.setStringRemoteModule("TriggerSource", "CXPin");
                    grabber.setStringRemoteModule("TriggerActivation", "RisingEdge");

                    grabber.setStringRemoteModule("TriggerRescalerMode", cameraInfoGenTL.TriggerRescalerMode ? "On" : "Off");
                    grabber.setStringRemoteModule("TriggerRescalerRate", cameraInfoGenTL.TriggerRescalerRate.ToString());
                    grabber.setStringRemoteModule("BinningVertical", cameraInfoGenTL.BinningVertical ? "X2" : "X1");
                   grabber.setStringRemoteModule("BinningHorizontal", cameraInfoGenTL.BinningHorizontal ? "X2" : "X1");
                    grabber.setStringRemoteModule("ReverseX", cameraInfoGenTL.RotateFlipType.IsFlipX().ToString());
                    grabber.setStringRemoteModule("AnalogGain", cameraInfoGenTL.AnalogGain.ToString());
                    grabber.setFloatRemoteModule("DigitalGain", cameraInfoGenTL.DigitalGain);
                    break;

                case EClientType.Slave:
                    grabber.setStringInterfaceModule("DelayToolSelector", "DEL1");
                    grabber.setStringInterfaceModule("DelayToolSource1", "NONE");

                    grabber.setStringInterfaceModule("EventInputToolSelector", "EIN1");
                    grabber.setStringInterfaceModule("EventInputToolSource", "A");
                    grabber.setStringInterfaceModule("EventInputToolActivation", "StartOfScan");

                    grabber.setStringInterfaceModule("EventInputToolSelector", "EIN2");
                    grabber.setStringInterfaceModule("EventInputToolSource", "A");
                    grabber.setStringInterfaceModule("EventInputToolActivation", "EndOfScan");

                    grabber.setStringStreamModule("StartOfScanTriggerSource", "EIN1");
                    grabber.setStringStreamModule("EndOfScanTriggerSource", "ScanLength");
                    break;
            }
        }

        private void SetCxpLinkConfiguration()
        {
            return;
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;

            string cxpSupportedQuary = string.Format("{0}Supported", cameraInfoGenTL.CxpLinkConfiguration);
            int cxpSupported = (int)grabber.getIntegerInterfaceModule(cxpSupportedQuary);
            if (cxpSupported == 0)
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, string.Format("{0} is not supported", cameraInfoGenTL.CxpLinkConfiguration), null, "");

            //CxpLinkConfiguration
            int hostConnectionCount = (int)grabber.getIntegerInterfaceModule("CxpHostConnectionCount");
            bool[] cxpConnectionStates = new bool[hostConnectionCount];
            for (int i = 0; i < hostConnectionCount; i++)
            {
                //string selector = new string((char)('A' + i), 1);
                //grabber.setStringInterfaceModule("CxpHostConnectionSelector", selector);
                grabber.setIntegerInterfaceModule("CxpHostConnectionSelector", i);
                string cxpConnectionState = grabber.getStringInterfaceModule("CxpConnectionState");
                cxpConnectionStates[i] = (cxpConnectionState == "Detected");
            }

            string cxpLinkConfiguration = string.Format("{0}_X{1}", cameraInfoGenTL.CxpLinkConfiguration, cxpConnectionStates.Count(f => f));
            grabber.setStringRemoteModule("CxpLinkConfiguration", cxpLinkConfiguration);
        }

        public virtual bool UpdateBuffer(int width, int height, int count, bool forceRealloc = false)
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;

            if (width <= 0)
                width = cameraInfoGenTL.Width;//17824;

            if (height <= 0)
                height = cameraInfoGenTL.Height;

            if (count <= 0)
                count = cameraInfoGenTL.FrameNum;

            try
            {
                int curWidth = (int)grabber.getIntegerRemoteModule("Width");
                int curBufferHeight = (int)grabber.getIntegerStreamModule("BufferHeight");
                if (forceRealloc == false && curWidth == width && curBufferHeight == height)
                    return false;

                grabber.setIntegerRemoteModule("Width", width);
                grabber.setIntegerStreamModule("BufferHeight", height);
                grabber.setIntegerStreamModule("ScanLength", height);

                if (cameraInfoGenTL.ClientType == EClientType.Master)
                    grabber.setIntegerRemoteModule("OffsetX", cameraInfoGenTL.OffsetX);

                Thread.Sleep(200);

                ReleaseBuffer();
                grabber.Grabber.flushBuffers(Euresys.gc.ACQ_QUEUE_TYPE.ACQ_QUEUE_ALL_DISCARD);
                grabber.Grabber.resetBufferQueue();

                this.imageSize = new System.Drawing.Size(width, height);
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "GenTL - UpdateBuffer() - Exception - {0}: {1}", new object[] { ex.GetType().Name, ex.Message }, "");
            }

            BufferIndexRange bufferIndexRange;
            if (cameraInfoGenTL.UseMilBuffer)
            {
                long bufWidth = width;
                long bufHeight = height * count;
                this.mBuffer?.Dispose();
                this.mBuffer = new MatroxBuffer(bufWidth, bufHeight);

                UserMemoryArray userMemoryArray = new UserMemoryArray();
                userMemoryArray.memory.@base = mBuffer.Ptr;
                userMemoryArray.memory.size = (ulong)(width * height * count);
                userMemoryArray.bufferSize = (ulong)(width * height);

                bufferIndexRange = grabber.Grabber.announceAndQueue(userMemoryArray);
            }
            else
            {
                bufferIndexRange = grabber.Grabber.reallocBuffers((uint)count);
            }

            Euresys.SizeT bufferIndex = bufferIndexRange.begin;
            while (bufferIndex != bufferIndexRange.end)
            {
                IntPtr imgPtr;
                grabber.Grabber.getBufferInfo(bufferIndex, Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

                Image2D grabbedImage = new Image2D();
                if (this.cameraInfo.UseNativeBuffering)
                {
                    grabbedImage.Initialize(width, height, 1, width, imgPtr);
                }
                else
                {
                    grabbedImage.Initialize(width, height, 1, width);
                }

                grabbedImage.Tag = new CameraGenTLBufferTag(0, Size.Empty, 0);
                grabbedImageList.Add(grabbedImage);

                bufferIndex++;
            }

            ImageSize = new System.Drawing.Size(width, height);

            return true;
        }

        public virtual void ReleaseBuffer()
        {
            foreach (Image2D grabbedImage in grabbedImageList)
                grabbedImage.Dispose();
            grabbedImageList.Clear();
        }

        public void GenTLCamCallback(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GenTLCamCallback Start");
            isGrabbed.Set();

            if (remainGrabCount != CONTINUOUS)
            {
                if (remainGrabCount != 0)
                    remainGrabCount--;
                if (remainGrabCount == 0)
                    Stop();
            }

            IntPtr ptr;

            using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer((Euresys.EGrabberCallbackSingleThread)g, data))
            {
                ulong frameId;
                long size, width, height;
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out ptr);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_FRAMEID, out frameId);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_WIDTH, out width);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_HEIGHT, out height);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_SIZE, out size);
                height = size / width;

                int bufferId = grabbedImageList.FindIndex(f => f.DataPtr == ptr);
                Size frameSize = new Size((int)width, (int)height);
                grabbedImageList[bufferId].Tag = new CameraGenTLBufferTag((ulong)frameId, frameSize, bufferId);

                //Debug.WriteLine(string.Format("[{0}] CameraGenTL::GenTLCamCallback, Id{1}, W{2}, H{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), frameId, width, height));

                //UpdateBufferTag(buffer);
                lastGrabbedImgaePtr = ptr;
            }


            ImageGrabbedCallback(ptr);
        }

        static Int64 fpsToMicroseconds(Int64 fps)
        {
            if (fps == 0)
            {
                return 0;
            }
            else
            {
                return (1000000 + fps - 1) / fps;
            }
        }

        private void Start()
        {
            //grabber.resetBufferQueue(); //off for buffer Operate round-robin
            //grabber.flushBuffers();
            grabber.Grabber.start();
        }


        public override void SetTriggerDelay(int triggerDelayUs)
        {

        }

        public override void Release()
        {
            base.Release();

            if (mBuffer != null)
                mBuffer.Dispose();
            mBuffer = null;

            if (grabber != null)
                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', false);}");

            //if (grabber != null)
            //{
            //    grabber.stop();
            //    grabber.Dispose();
            //}
            //grabber = null;

            //if (converter != null)
            //{
            //    converter.Dispose();
            //}
            //converter = null;

            //if (genTL != null)
            //{
            //    genTL.Dispose();
            //}
            //genTL = null;

            this.grabber?.Dispose();
            this.grabber = null;

        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                ptr = this.lastGrabbedImgaePtr;

            int bufferIndex = GetBufferIndex(ptr);

            //Debug.Assert(bufferIndex >= 0);
            if (bufferIndex < 0)
                return null;

            ImageD imageD = grabbedImageList[bufferIndex];
            LogHelper.Debug(LoggerType.Grab, $"CameraGenTL::GetGrabbedImage - Tag: {imageD.Tag}");
            return imageD.Clone();
        }

        public override List<ImageD> GetImageBufferList()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GetImageBufferList");
            return new List<ImageD>(grabbedImageList);
        }

        public override int GetImageBufferCount()
        {
            //LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GetImageBufferCount");
            return grabbedImageList.Count;
        }

        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GrabOnce");

            try
            {
                if (SetupGrab() == false)
                    return;

                this.remainGrabCount = 1;

                Start();

                //LogHelper.Debug(LoggerType.Grab, "CameraGenTL - Channel Activated");
            }
            catch (Euresys.gentl_error e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::GrabOnce Exception - " + e.Message);
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::GrabOnce GenTlException - " + e.Message);
            }
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GrabMulti");

            try
            {
                if (SetupGrab() == false)
                    return;

                this.remainGrabCount = grabCount;

                //grabber.setStringDeviceModule("EndOfSequenceTriggerSource", "StopSequence");
                Start();

                //LogHelper.Debug(LoggerType.Grab, "CameraGenTL - Channel Activated");
            }
            catch (Euresys.gentl_error e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::GrabOnce GenTlException - " + e.Message);
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::GrabOnce Exception - " + e.Message);

            }
        }

        public override void Stop()
        {
            base.Stop();
            grabber?.Grabber.stop();
            remainGrabCount = 0;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraGenTL::SetDeviceExposure"));
            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "Area")
                {
                    grabber.setFloatRemoteModule("ExposureTime", exposureTimeMs * 1000);
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetDeviceExposureMs()
        {
            CameraInfoGenTL cameraInfoGenTL = this.cameraInfo as CameraInfoGenTL;
            if (cameraInfoGenTL.ClientType == EClientType.Slave)
                return -1;

            if (grabber.getStringRemoteModule("OperationMode") == "Area")
            {
                return (float)grabber.getFloatRemoteModule("ExposureTime") / 1000f;
            }
            return -1;
            //else
            //{
            //    float grabHz = (float)grabber.getFloatRemoteModule("AcquisitionLineRate"); // [1/s]
            //    return (float)((1 / grabHz) * 1000f);
            //}
        }

        public string GetPropertyData(string itemName)
        {
            switch (itemName)
            {
                case "DeviceModelName":
                case "DeviceSerialNumber":
                case "OperationMode":
                case "TDIStages":
                case "ScanDirection":
                    return grabber.getStringRemoteModule(itemName);

                case "AcquisitionLineRate":
                    return grabber.getFloatRemoteModule(itemName).ToString();

                case "Width":
                    return grabber.getIntegerRemoteModule(itemName).ToString();

                case "ScanLength":
                    return grabber.getIntegerStreamModule(itemName).ToString();

                default:
                    return null;
            }
        }

        public void SetPropertyData(string itemName, string value)
        {
            switch (itemName)
            {
                case "OperationMode":
                case "TDIStages":
                case "ScanDirection":
                    grabber.setStringRemoteModule(itemName, value);
                    break;

                case "AcquisitionLineRate":
                    grabber.setFloatRemoteModule(itemName, Convert.ToSingle(value));
                    break;

                case "Width":
                case "ScanLength":
                    grabber.setIntegerRemoteModule(itemName, Convert.ToInt32(value));
                    break;

                default:
                    break;
            }
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraGenTL::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));
            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "TDI")
                {
                    //if (((CameraInfoGenTL)this.cameraInfo).BinningVertical)
                    //    hz *= 2;
                    grabber.setFloatRemoteModule("AcquisitionLineRate", hz);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetAcquisitionLineRate()
        {
            double hz = grabber.getFloatRemoteModule("AcquisitionLineRate");
            return (float)hz;
        }
    }
}
