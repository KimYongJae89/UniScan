using System;
using System.Collections.Generic;
using System.Threading;
using DynMvp.Base;
using System.Diagnostics;
using DALSA.SaperaLT.SapClassBasic;
using Euresys;
using System.Windows.Forms;
using DynMvp.Device.Device.FrameGrabber;
using System.Drawing;
using DynMvp.Device.Device;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraSaperaBufferTag : CameraBufferTag
    {
        public int BufferId => bufferId;
        private int bufferId = -1;

        public CameraSaperaBufferTag(UInt64 frameId, Size frameSize, int bufferId) : base(frameId, frameSize)
        {
            this.bufferId = bufferId;
        }

        public override string ToString()
        {
            return string.Format("Buffer {0} / Frame {1}", this.bufferId, this.FrameId);
        }
    }

    delegate void ProgrammaryStartEndDelegate();
    public class CameraSapera : Camera
    {
        private ProgrammaryStartEndDelegate OnProgrammaryStart;
        private ProgrammaryStartEndDelegate OnProgrammaryEnd;

        private SapLocation m_ServerLocation;

        private SapAcquisition m_Acquisition; //Frame-Grabber
        private SapAcqDevice m_AcqDevice;  //Camera-Device  for GeniCam communication

        private SapBuffer m_Buffers;
        private SapTransfer m_Xfer;

        private SapGio[] sapGio;

        private System.Timers.Timer eventPollingTimer;
        private System.Timers.Timer ProgrammaryEndTimer;
        private int programmaryAbortLines = -1;

        public bool IsMasterDevice => ((CameraInfoSapera)this.cameraInfo).ClientType == CameraInfoSapera.EClientType.Master;

        // for Stop and Go
        private bool isGrabValid = false;

        public bool IsGrabProgrammary { get; private set; }
        private bool onProgrammaticGrab = false;

        //TimeOutTimer timeOutTimer = null;
        int eventNotifyCounter = 0;

        IntPtr lastGrabbedImgaePtr = IntPtr.Zero;

        protected List<Image2D> grabbedImageList = new List<Image2D>();

        private bool SetGrabberParam(SapAcquisition.Prm param, string value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<string> - {param}, {value}"); return this.m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, int value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<int> - {param}, {value}"); return this.m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, int[] value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<int[]> - {param}, {value}"); return this.m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, SapAcquisition.Val value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<Val> - {param}, {value}"); return this.m_Acquisition.SetParameter(param, value, true); }

        private bool SetDeviceFeature(string feature, Enum value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<Enum> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value.ToString()); }
        private bool SetDeviceFeature(string feature, bool value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<bool> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, double value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<double> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, float value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<float> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, int value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<int> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, long value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<long> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, SapBuffer value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<SapBuffer> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, SapLut value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<SapLut> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, string value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<string> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, uint value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<uint> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }
        private bool SetDeviceFeature(string feature, ulong value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeature<ulong> - {feature}, {value}"); return this.m_AcqDevice.SetFeatureValue(feature, value); }


        public CameraSapera(CameraInfo cameraInfo) : base(cameraInfo)
        {
            //this.timeOutTimer = new TimeOutTimer();
            //this.timeOutTimer.OnTimeout += timeOutTimer_OnTimeout;
        }

        //private void timeOutTimer_OnTimeout(object sender, EventArgs e)
        //{
        //    LogHelper.Error(LoggerType.Grab, "CameraSapera::timeOutTimer_OnTimeout");
        //    ErrorManager.Instance().Report(new AlarmException(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Fatal, this.name, "Grabber not responding.", null, ""));
        //}

        public int GetBufferIndex(IntPtr ptr)
        {
            return grabbedImageList.FindIndex(f => f.DataPtr == ptr);
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            CameraInfoSapera cameraInfoSapera = (CameraInfoSapera)this.CameraInfo;
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                try
                {
                    SapAcquisition.Val trigger = (triggerType == TriggerType.RisingEdge ? SapAcquisition.Val.RISING_EDGE : SapAcquisition.Val.FALLING_EDGE);
                    SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_DETECTION, trigger);

                    if (triggerMode == TriggerMode.Hardware)
                    {
                        SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0);
                        SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 1);

                        SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_SOURCE, 1);
                    }
                    else
                    {
                        SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 0);
                        SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 1);
                    }
                }
                catch (Exception ex)
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
            CameraInfoSapera cameraInfoSapera = ((CameraInfoSapera)this.cameraInfo);

            SetGrabberParam(SapAcquisition.Prm.STROBE_ENABLE, 0);

            SetFrameTriggerMode(new CameraInfoSapera.__EXT_FRAME_TRIGGER()); // Frame Tirgger None

            SetGrabberParam(SapAcquisition.Prm.SCAN, (int)SapAcquisition.Val.SCAN_AREA);

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
                this.m_AcqDevice.SetFeatureValue("sensorTDIModeSelection", "TdiArea");

            SetTriggerMode(TriggerMode.Software);

            UpdateBuffer(cameraInfoSapera.TdiArea.Width, cameraInfoSapera.TdiArea.Height);
        }

        private void SetLineScanMode()
        {
            CameraInfoSapera cameraInfoSapera = ((CameraInfoSapera)this.cameraInfo);

            SetGrabberParam(SapAcquisition.Prm.SCAN, (int)SapAcquisition.Val.SCAN_LINE);

            SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 0);

            // Trigger Signal and Action
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_METHOD, 0x2); //CORACQ_VAL_LINE_TRIGGER_METHOD_2
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_DELAY, 0);
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_ENABLE, 1);

            // Grabber Strobe
            SetGrabberParam(SapAcquisition.Prm.STROBE_METHOD, SapAcquisition.Val.STROBE_METHOD_3);
            SetGrabberParam(SapAcquisition.Prm.STROBE_POLARITY, SapAcquisition.Val.ACTIVE_HIGH);
            SetGrabberParam(SapAcquisition.Prm.STROBE_DURATION, 1000); //매뉴얼은 us, 실제CamExpert에는 ns ??????
            SetGrabberParam(SapAcquisition.Prm.STROBE_DELAY, 0);  //매뉴얼은 us, 실제CamExpert에는 ns ??????
            SetGrabberParam(SapAcquisition.Prm.STROBE_ENABLE, 0x00000001);

            //External Line Trigger Source(Encoder)
            CameraInfoSapera.__SHAFT_ENCODER extLineTringger = cameraInfoSapera.ExtLineTringger;
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_DIRECTION, (SapAcquisition.Val)extLineTringger.DIRECTION);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_DROP, extLineTringger.DROP);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_MULTIPLY, (int)extLineTringger.MULTIPLY);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ORDER, (SapAcquisition.Val)extLineTringger.ORDER);

            //Line Sync Source
            //SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_DETECTION, SapAcquisition.Val.RISING_EDGE);
            //SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_SOURCE, 1);
            SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);


            // Frame Trigger
            CameraInfoSapera.__EXT_FRAME_TRIGGER extFrameTringger = cameraInfoSapera.ExtFrameTringger;
            SetFrameTriggerMode(cameraInfoSapera.ExtFrameTringger);

            //SetGrabberParam(SapAcquisition.Prm.FRAME_LENGTH, SapAcquisition.Val.FRAME_LENGTH_VARIABLE);  //CORACQ_VAL_FRAME_LENGTH_FIX (0x00000001)//CORACQ_VAL_FRAME_LENGTH_VARIABLE(0x00000002)

            // Camera
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                SetDeviceFeature("sensorTDIModeSelection", cameraInfoSapera.TdiMode);
                if (cameraInfoSapera.TdiMode == CameraInfoSapera.ETDIMode.TdiMTF)
                    SetDeviceFeature("superResolutionMode", "srMapped");  //srDetailRestored 
                SetDeviceFeature("sensorScanDirection", ((CameraInfoSapera)this.cameraInfo).DirectionType.ToString());

                SetDeviceFeature("GainSelector", "All");

                // 카메라 트리거는 HW로 설정. 모드 변환은 그래버에서...
                SetDeviceFeature("AcquisitionLineRate", "100000");
                SetDeviceFeature("TriggerMode", "External");
                SetDeviceFeature("TriggerSource", "CLHS");

                // Trigger Mirroring Output
                CameraInfoSapera.__Gpio cameraStrobIo = cameraInfoSapera.CameraStrobIo;
                if (cameraStrobIo.LineSelector != CameraInfoSapera.__Gpio.ELineSelector.NONE)
                {
                    SetDeviceFeature("LineSelector", cameraStrobIo.LineSelector.ToString()); // "GPIO4");
                    SetDeviceFeature("outputLineSource", cameraStrobIo.outputLineSource ? "On" : "Off"); // "On");
                    SetDeviceFeature("outputLinePulseDelay", cameraStrobIo.outputLinePulseDelay.ToString());// "0"); //us
                    SetDeviceFeature("outputLinePulseDuration", cameraStrobIo.outputLinePulseDuration.ToString());// "1"); //us 
                    SetDeviceFeature("LineInverter", cameraStrobIo.LineInverter ? "On" : "Off");// "Off"); //us
                }
            }
            UpdateBuffer(this.cameraInfo.Width, this.cameraInfo.Height);
        }

        private void SetFrameTriggerMode(CameraInfoSapera.__EXT_FRAME_TRIGGER extFrameTringger)
        {
            bool isFrameTrigger = (extFrameTringger.MODE == CameraInfoSapera.__EXT_FRAME_TRIGGER.EMode.FrameTrigger);
            SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, isFrameTrigger ? 1 : 0);
            if (isFrameTrigger)
                //External Frame Trigger
            {
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_DETECTION, (SapAcquisition.Val)extFrameTringger.DETECTION);
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_LEVEL, (SapAcquisition.Val)extFrameTringger.LEVEL);
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_SOURCE, extFrameTringger.SOURCE);
                SetGrabberParam(SapAcquisition.Prm.EXT_TRIGGER_DURATION, extFrameTringger.DURATION);
            }

            this.IsGrabProgrammary = true;
            this.OnProgrammaryStart = null;
            this.OnProgrammaryEnd = null;
            switch (extFrameTringger.MODE)
            {
                case CameraInfoSapera.__EXT_FRAME_TRIGGER.EMode.RisingSnap:
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryStart"));
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => SetDeviceFeature("TriggerMode", "External"));
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => this.m_Xfer.Snap());

                    this.OnProgrammaryEnd = null;
                    break;

                case CameraInfoSapera.__EXT_FRAME_TRIGGER.EMode.RisingGrabFallingStop:
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryStart"));
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => SetDeviceFeature("TriggerMode", "External"));
                    this.OnProgrammaryStart += new ProgrammaryStartEndDelegate(() => this.m_Xfer.Grab());

                    if (false)
                    {
                        this.OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() => LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryEnd"));
                        this.OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() => this.m_Xfer.Freeze());
                        this.OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() => this.m_Xfer.Abort());
                    }
                    else
                    {
                        this.OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() => this.ProgrammaryEndTimer.Start());
                    }
                    break;

                default:
                    this.IsGrabProgrammary = false;
                    break;
            }
        }

        private void ProgrammaryAbort()
        {
            if (!this.m_Xfer.Grabbing)
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::ProgrammaryAbort - Transfre is not Grabbing.");
                return;
            }

            int spaceUsed = this.m_Buffers.SpaceUsed;
            int pitch = this.m_Buffers.Pitch;
            this.programmaryAbortLines = spaceUsed / pitch;
            LogHelper.Debug(LoggerType.Grab, $"CameraSapera::ProgrammaryAbort - validLines: {this.programmaryAbortLines}");

            this.m_Xfer.Freeze();

            SetDeviceFeature("TriggerMode", "Internal");
        }

        public override bool SetupGrab()
        {
            this.isGrabValid = (this.cameraInfo.FrameType == CameraInfo.EFrameType.Continuous) || this.isCalibrationMode;
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::SetupGrab - isGrabValid: {0}", this.isGrabValid));
            this.programmaryAbortLines = -1;
            SetDeviceFeature("TriggerMode", "External");
            this.ProgrammaryEndTimer?.Stop();

            this.eventPollingTimer?.Start();
            return base.SetupGrab();
        }

        void xfer_XferNotify(object sender, SapXferNotifyEventArgs argsNotify)
        {
            if (argsNotify.EventType != SapXferPair.XferEventType.EndOfFrame)
                return;

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::xfer_XferNotify - Start. isGrabValid: {0}", this.isGrabValid));
            this.eventNotifyCounter = 0;

            if (!this.isGrabValid)
            {
                // Partial Frame이면 첫번째 프레임은 버림.
                this.isGrabValid = true;
                return;
            }

            // Sanp으로 찍은 경우 여기서 막힘.
            //if (this.remainGrabCount == 0 && !this.m_Xfer.Grabbing)
            //    return;

            CameraSapera camera = argsNotify.Context as CameraSapera;
            SapTransfer transfer = sender as SapTransfer;

            IntPtr ptr;
            m_Buffers.GetAddress(m_Buffers.Index, out ptr);

            int bufferId = m_Buffers.Index;
            Debug.Assert(bufferId == grabbedImageList.FindIndex(f => f.DataPtr == ptr));

            int width = m_Buffers.Width;
            int sizeBytes = m_Buffers.get_SpaceUsed(m_Buffers.Index);
            int height = sizeBytes / m_Buffers.Pitch;
            if (this.programmaryAbortLines >= 0)
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::xfer_XferNotify - programmaryAbortLines: {this.programmaryAbortLines}");
                height = this.programmaryAbortLines;
                this.programmaryAbortLines = -1;
                SetDeviceFeature("TriggerMode", "External");
            }

            Size frameSize = new Size(width, height);
            grabbedImageList[bufferId].Tag = new CameraSaperaBufferTag((ulong)argsNotify.EventCount, frameSize, bufferId);
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::xfer_XferNotify - W: {0}, H: {1}", width, height));

            lastGrabbedImgaePtr = ptr;
            isGrabbed.Set();
            ImageGrabbedCallback(ptr);
        }

        private void GioNotify(object sender, SapGioNotifyEventArgs e)
        {
            if (!this.onProgrammaticGrab)
                return;

            LogHelper.Debug(LoggerType.Grab, $"Grabber IO PinNo: {e.PinNumber}, Type: {e.EventType}, Count: {e.EventCount}");

            if (e.PinNumber == 0)
            {
                switch (e.EventType)
                {
                    case SapGio.EventType.RisingEdge:
                        if (this.ProgrammaryEndTimer.Enabled)
                            this.ProgrammaryEndTimer.Stop();
                        else
                            this.OnProgrammaryStart?.Invoke();
                        break;

                    case SapGio.EventType.FallingEdge:
                        if (this.m_Xfer.Grabbing)
                            this.OnProgrammaryEnd?.Invoke();
                        break;
                }
            }
        }

        private void OnSignalNotify(object sender, SapSignalNotifyEventArgs arg)
        {
            if (arg == null)
                return;

            // Signal 이벤트가 먼저 들어올까? Frame Done 이벤트가 먼저 들어올까?
            OnSignalNotify(arg.SignalStatus);
        }

        private void OnSignalNotify(SapAcquisition.AcqSignalStatus acqSignalStatus)
        {
            try
            {
                // Signal 이벤트에서는 신호 끄기만 함. FrameDone 이벤트에서는 신호 켜기만 함.
                if (!this.m_Xfer.Grabbing)
                    return;

                bool isFrameValid = acqSignalStatus.HasFlag(SapAcquisition.AcqSignalStatus.FrameValidPresent);
                bool isLineValid = acqSignalStatus.HasFlag(SapAcquisition.AcqSignalStatus.LineValidPresent);
                this.eventNotifyCounter++;
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::UpdateSignalStatus - FrameValid: {isFrameValid}, LineValid: {isLineValid}, EventCount: {eventNotifyCounter}");

                if (this.eventNotifyCounter > ((CameraInfoSapera)this.cameraInfo).EventNotifyCounter)
                {
                    // SignalNotify n회동안 xfer_XferNotify 이벤트 없으면 오류.
                    ErrorManager.Instance().Report(new AlarmException(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Fatal, this.name, "Grabber not responding.", null, ""));
                }
            }
            catch (Exception)
            { }
        }

        public override void Initialize(bool calibrationMode)
        {
            try
            {
                base.Initialize(calibrationMode);

                ScanMode scanMode = (calibrationMode || !this.cameraInfo.IsLineScan) ? ScanMode.Area : ScanMode.Line;
                InitializeDevice(scanMode);
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "Sapera Exception - {0}", new object[] { ex.Message }, "");
                //string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                //System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void InitializeDevice(ScanMode scanMode)
        {
            CameraInfoSapera cameraInfoSapera = this.cameraInfo as CameraInfoSapera;

            this.m_ServerLocation = new SapLocation(cameraInfoSapera.ServerName, cameraInfoSapera.ServerResourceIndex);

            if (System.IO.File.Exists(cameraInfoSapera.CcfFilePath) && scanMode == ScanMode.Line)
                InitDeviceWithCcf(scanMode, cameraInfoSapera);
            else
                InitDeviceWithoutCcf(scanMode, cameraInfoSapera);

            if (true)
            {
                // event 
                this.m_Acquisition.SignalNotifyEnable = true;
                this.m_Acquisition.SignalNotify += new SapSignalNotifyHandler(OnSignalNotify);
                this.m_Acquisition.SignalNotifyContext = this;
            }
            else
            {
                // polling
                SapAcquisition.AcqSignalStatus acqSignalStatusMaskedOld;
                SapAcquisition.AcqSignalStatus mask = (SapAcquisition.AcqSignalStatus.LineValidPresent | SapAcquisition.AcqSignalStatus.FrameValidPresent);
                this.eventPollingTimer = new System.Timers.Timer();
                this.eventPollingTimer.Interval = 50;
                this.eventPollingTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) =>
                {
                    SapAcquisition.AcqSignalStatus acqSignalStatusMasked = this.m_Acquisition.SignalStatus & mask;
                    if (acqSignalStatusMaskedOld != acqSignalStatusMasked)
                        OnSignalNotify(this.m_Acquisition.SignalStatus);
                    acqSignalStatusMaskedOld = acqSignalStatusMasked;
                });
            }

            InitGio();

            this.ProgrammaryEndTimer = new System.Timers.Timer() { Interval = 100 };
            this.ProgrammaryEndTimer.AutoReset = false;
            this.ProgrammaryEndTimer.Elapsed += ProgrammaryEndTimer_Elapsed;
        }

        private void ProgrammaryEndTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ProgrammaryAbort();
        }

        private void InitGio()
        {
            int gioCount = SapManager.GetResourceCount(this.m_ServerLocation, SapManager.ResourceType.Gio);
            this.sapGio = new SapGio[gioCount];
            for (int i = 0; i < gioCount; i++)
            {
                SapLocation sapLocation = new SapLocation(this.m_ServerLocation.ServerIndex, i);
                SapGio sapGio = new SapGio(sapLocation);
                if (!sapGio.Create())
                    throw new Exception($"CameraSapera GIO Manager Initialize Fail");

                sapGio.GetCapability(SapGio.Cap.DIR_INPUT, out int inputCapValue);
                sapGio.GetCapability(SapGio.Cap.DIR_OUTPUT, out int outputCapValue);

                if (outputCapValue == 0 && inputCapValue > 0)
                {
                    sapGio.GioNotify += new SapGioNotifyHandler(GioNotify);
                    sapGio.EnableEvent(0, SapGio.EventType.RisingEdge | SapGio.EventType.FallingEdge);
                }
                this.sapGio[i] = sapGio;
            }
        }

        private void InitDeviceWithCcf(ScanMode scanMode, CameraInfoSapera cameraInfoSapera)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::InitDeviceWithCcf");
            this.m_Acquisition = new SapAcquisition(this.m_ServerLocation, cameraInfoSapera.CcfFilePath);
            CreateObject(this.m_Acquisition);

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                this.m_AcqDevice = new SapAcqDevice(m_ServerLocation, false);
                CreateObject(this.m_AcqDevice);
            }

            UpdateBuffer(this.cameraInfo.Width, this.cameraInfo.Height);
        }

        private void InitDeviceWithoutCcf(ScanMode scanMode, CameraInfoSapera cameraInfoSapera)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::InitDeviceWithoutCcf");
            //Frame Grabber: SapAcquisition
            this.m_Acquisition = new SapAcquisition(m_ServerLocation);
            CreateObject(this.m_Acquisition);
            ConfigAcquisition();

            //Camera Device: SapAcqDevice
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                this.m_AcqDevice = new SapAcqDevice(m_ServerLocation, false);
                CreateObject(this.m_AcqDevice);
            }
            SetScanMode(scanMode);
        }

        private bool SettingFrameGrabber_Test(SapAcquisition framegrabber)
        {
            bool success;
            int outvalue;
            int[] capValue;
            bool result = framegrabber.GetCapability(SapAcquisition.Cap.SCAN, out capValue);
            success = framegrabber.GetParameter(SapAcquisition.Prm.SCAN, out outvalue);
            var vvval = SapAcquisition.GetParameterType(SapAcquisition.Prm.SCAN);

            //Basic
            success = framegrabber.SetParameter(SapAcquisition.Prm.SCAN, SapAcquisition.Val.SCAN_LINE, true);
            //success = framegrabber.SetParameter(SapAcquisition.Prm.VIDEO, SapAcquisition.Val.VIDEO_MONO, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.PIXEL_DEPTH, 8, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.DATA_LANES, 5, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.CROP_WIDTH, 32768, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.HACTIVE, 32768, true); //가로 Pixel 사이즈
            success = framegrabber.SetParameter(SapAcquisition.Prm.DATA_VALID_ENABLE, 0, true); //diable
            //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CLHS_CONFIGURATION, 0, true); //none
            success = framegrabber.SetParameter(SapAcquisition.Prm.POCL_ENABLE, SapAcquisition.Val.SIGNAL_POCL_ACTIVE, true); //enalble

            CameraInfoSapera cameraInfoSapera = (CameraInfoSapera)CameraInfo;
            //grabber.setStringRemoteModule("PixelFormat", "Mono8");
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                //Advanced Control
                success = framegrabber.SetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0, true);        //Line Sync Source
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.INT_FRAME_TRIGGER_FREQ, 34567, true);         //Internal Frame Trigger Frequency (in Hz)
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MIN, 1, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MAX, 100000, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.INT_FRAME_TRIGGER_FREQ, 34567, true);

                success = framegrabber.SetParameter(SapAcquisition.Prm.LINE_INTEGRATE_METHOD, SapAcquisition.Val.LINE_INTEGRATE_METHOD_7, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.LINE_TRIGGER_METHOD, SapAcquisition.Val.LINE_TRIGGER_METHOD_1, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.LINE_INTEGRATE_ENABLE, SapAcquisition.Val.LINE_INTEGRATE_METHOD_1, true);

                success = framegrabber.SetParameter(SapAcquisition.Prm.STROBE_ENABLE, 0, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.STROBE_METHOD, SapAcquisition.Val.STROBE_METHOD_1, true);



                //External Trigger
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_LEVEL, SapAcquisition.Val.LEVEL_TTL, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_ENABLE, SapAcquisition.Val.EXT_TRIGGER_OFF, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_DETECTION, SapAcquisition.Val.RISING_EDGE, true);
                //External Trigger



            }
            else if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Slave)
            {

            }

            int width;
            int height;
            //framegrabber.SetParameter(SapAcquisition.Prm.CROP_HEIGHT, imageHeight, true);
            framegrabber.GetParameter(SapAcquisition.Prm.CROP_WIDTH, out width);   // 버퍼 width 가져옴 -> SizeX 
            framegrabber.GetParameter(SapAcquisition.Prm.CROP_HEIGHT, out height);  // 버퍼 height 가져옴 -> SizeY

            this.ImageSize = new System.Drawing.Size(width, height);
            this.ImagePitch = height;
            return true;
        }

        private bool SettingCamera_Test(SapAcqDevice cameraDev)
        {
            bool success = false;
            CameraInfoSapera cameraInfoSapera = (CameraInfoSapera)CameraInfo;

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                ///Camera Control
                success = cameraDev.SetFeatureValue("AcquisitionLineRate", 87719.298); //Acquisition line Rate
                success = cameraDev.SetFeatureValue("sensorTDIModeSelection", "TdiMTF"); //TDI Super Resolution
                                                                                         //success = cameraDev.SetFeatureValue("sensorTDIStagesSelection", "Lines128");
                                                                                         //success = cameraDev.SetFeatureValue("sensorScanDirectionSource", "Internal"); //GPIO2  //Encoder //worng
                success = cameraDev.SetFeatureValue("sensorScanDirection", "Forward"); //Reverse
                success = cameraDev.SetFeatureValue("GainSelector", "All");   //System
                success = cameraDev.SetFeatureValue("superResolutionMode", "srMapped");  //srDetailRestored
                                                                                         //success = cameraDev.SetFeatureValue("srStrength", 0.1);
                                                                                         //Digital IO Control
                success = cameraDev.SetFeatureValue("TriggerMode", "Internal"); //External
                                                                                //success = cameraDev.SetFeatureValue("TriggerSource", "CLHS"); //Encoder GPIO1
            }
            return success;
        }

        private bool ConfigAcquisition()
        {
            CameraInfoSapera cameraInfoSapera = (CameraInfoSapera)CameraInfo;
            //int outvalue;
            //int[] capValue;
            //bool result = framegrabber.GetCapability(SapAcquisition.Cap.SCAN, out capValue);
            //success = framegrabber.GetParameter(SapAcquisition.Prm.SCAN, out outvalue);
            //var vvval = SapAcquisition.GetParameterType(SapAcquisition.Prm.SCAN);

            //Basic
            SetGrabberParam(SapAcquisition.Prm.PIXEL_DEPTH, 8);
            SetGrabberParam(SapAcquisition.Prm.DATA_LANES, 5);
            SetGrabberParam(SapAcquisition.Prm.DATA_VALID_ENABLE, 0);
            SetGrabberParam(SapAcquisition.Prm.POCL_ENABLE, SapAcquisition.Val.SIGNAL_POCL_ACTIVE); //enalble

            SetGrabberParam(SapAcquisition.Prm.FRAME_LENGTH, SapAcquisition.Val.FRAME_LENGTH_VARIABLE);

            return true;
        }

        private void CreateObject(SapXferNode node)
        {
            // Create acquisition object
            if (node != null && !node.Initialized)
            {
                if (!node.Create())
                {
                    ReleaseObject(node);
                    throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                        this.name, "Exception in CameraSapera::CreateObject({0})", new object[] { node.GetType().Name }, "");
                }
            }
        }

        private void ReleaseObject(SapXferNode node)
        {
            if (node != null)
            {
                if (node.Initialized)
                    node.Destroy();
                node.Dispose();
            }
        }

        public bool UpdateBuffer(int width, int height)
        {
            CameraInfoSapera cameraInfoSapera = (CameraInfoSapera)CameraInfo;

            try
            {
                int hActive;
                this.m_Acquisition.GetParameter(SapAcquisition.Prm.HACTIVE, out hActive);
                // HACTIVE 값은 CROP_WIDTH 값보다 크거나 같아야 한다.
                if (hActive > width)
                {
                    SetGrabberParam(SapAcquisition.Prm.CROP_WIDTH, width);
                    SetGrabberParam(SapAcquisition.Prm.HACTIVE, width);
                }
                else
                {
                    SetGrabberParam(SapAcquisition.Prm.HACTIVE, width);
                    SetGrabberParam(SapAcquisition.Prm.CROP_WIDTH, width);
                }
                //SetGrabberParam(SapAcquisition.Prm.CROP_LEFT, (int)cameraInfoSapera.OffsetX);
                SetGrabberParam(SapAcquisition.Prm.CROP_HEIGHT, height);

                {
                    int frameNum = cameraInfoSapera.FrameNum;
                    if (SapBuffer.IsBufferTypeSupported(m_ServerLocation, SapBuffer.MemoryType.ScatterGather))
                        this.m_Buffers = new SapBufferWithTrash(frameNum, m_Acquisition, SapBuffer.MemoryType.ScatterGather);
                    else
                        this.m_Buffers = new SapBufferWithTrash(frameNum, m_Acquisition, SapBuffer.MemoryType.ScatterGatherPhysical);
                    CreateObject(m_Buffers);

                    //Transfer
                    this.m_Xfer = new SapAcqToBuf(m_Acquisition, m_Buffers);
                    this.m_Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
                    this.m_Xfer.XferNotify += new SapXferNotifyHandler(xfer_XferNotify);
                    this.m_Xfer.XferNotifyContext = this;
                    this.m_Xfer.Create();

                    this.ImageSize = new System.Drawing.Size(width, height);
                    this.ImagePitch = width;
                    grabbedImageList.ForEach(f => f.Dispose());
                    grabbedImageList.Clear();

                    IntPtr intPtr;
                    for (int i = 0; i < frameNum; i++)
                    {
                        this.m_Buffers.GetAddress(i, out intPtr);
                        var imageBuffer = new Image2D();
                        imageBuffer.Initialize(width, height, 1, width, intPtr);
                        grabbedImageList.Add(imageBuffer);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "Exception in CameraSapera::UpdateBuffer() - {0}", new object[] { ex.Message }, "");
            }

            return true;
        }

        public virtual void ReleaseBuffer()
        {
            foreach (Image2D grabbedImage in grabbedImageList)
                grabbedImage.Dispose();
            grabbedImageList.Clear();
        }

        private void Start()
        {
            //grabber.resetBufferQueue(); //off for buffer Operate round-robin
            //grabber.flushBuffers();

        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_DELAY, triggerDelayUs);
        }

        public override void Release()
        {
            if (this.m_Xfer != null && this.m_Xfer.Grabbing)
            {
                m_Xfer.Freeze();
                bool waitDone = m_Xfer.Wait(1000);
                if (!waitDone)
                {
                    LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                    m_Xfer.Abort();
                }
            }

            ReleaseBuffer();

            //ReleaseObject(this.m_Xfer);
            if (this.m_Xfer != null)
                this.m_Xfer.XferNotify -= xfer_XferNotify;
            this.m_Xfer?.Destroy();
            this.m_Xfer?.Dispose();
            this.m_Xfer = null;

            //ReleaseObject(this.m_Buffers);
            this.m_Buffers?.Destroy();
            this.m_Buffers?.Dispose();
            this.m_Buffers = null;

            // Dispose 하면 다음 Create 이후 오류 발생함..
            // Destory 하면 오류는 안나지만 Frame Grabbed Event 안들어옴...
            //ReleaseObject(this.m_AcqDevice);
            this.m_AcqDevice?.Destroy();
            this.m_AcqDevice?.Dispose();
            this.m_AcqDevice = null;

            //ReleaseObject(this.m_Acquisition);
            this.m_Acquisition.SignalNotifyEnable = false;
            this.m_Acquisition.SignalNotify -= OnSignalNotify;
            this.m_Acquisition?.Destroy();
            this.m_Acquisition?.Dispose();
            this.m_Acquisition = null;

            this.m_ServerLocation?.Dispose();
            this.m_ServerLocation = null;

            base.Release();
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            //return m_LastGrabbedImage;
            if (ptr == IntPtr.Zero)
                ptr = this.lastGrabbedImgaePtr;

            int bufferIndex = GetBufferIndex(ptr);

            //Debug.Assert(bufferIndex >= 0);
            if (bufferIndex < 0)
                return null;

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::GetGrabbedImage - {0}", bufferIndex));
            return grabbedImageList[bufferIndex].Clone();
        }

        public override List<ImageD> GetImageBufferList()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::GetImageBufferList");
            return new List<ImageD>(grabbedImageList);
        }

        public override int GetImageBufferCount()
        {
            //LogHelper.Debug(LoggerType.Grab, "CameraSapera::GetImageBufferCount");
            return grabbedImageList.Count;
        }

        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabOnce");

            try
            {
                if (SetupGrab() == false)
                    return;

                this.remainGrabCount = 1;
                if (IsGrabProgrammary)
                {
                    this.onProgrammaticGrab = true;
                }
                else
                {
                    m_Xfer.Snap();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::GrabOnce Exception - " + e.Message);
            }
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabMulti");

            try
            {
                if (SetupGrab() == false)
                    return;

                if (!m_Xfer.Grabbing)
                {
                    m_Buffers.ResetIndex();
                    m_Buffers.Clear();

                    this.remainGrabCount = grabCount;
                    this.eventNotifyCounter = 0;

                    if (IsGrabProgrammary)
                    {
                        this.onProgrammaticGrab = true;
                    }
                    else
                    {
                        m_Xfer.Grab();
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::GrabMulti Exception - " + e.Message);
            }
        }

        public override void Stop()
        {
            if (this.IsStopped())
                return;

            base.Stop();

            this.eventNotifyCounter = 0;
            this.isGrabValid = false;
            try
            {
                this.eventPollingTimer?.Stop();

                if (true)
                {
                    this.remainGrabCount = 0;
                    this.onProgrammaticGrab = false;

                    m_Xfer.Freeze();
                    bool waitDone = m_Xfer.Wait(5000);
                    if (!waitDone)
                    {
                        LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                        m_Xfer.Abort();
                    }
                }
                else
                {
                    // Stop 하지 않음.
                    Thread.Sleep(100);

                    //m_Xfer.Freeze();
                    //bool waitDone = m_Xfer.Wait(1000);
                    //if (!waitDone)
                    //{
                    //    LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                    //    DebugWriteLine("CameraSapera::Stop - Wait Timeout");
                    //    m_Xfer.Abort();
                    //}
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::Stop Exception - " + e.Message);
            }
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs <= 0)
                return false;

            if (!IsMasterDevice)
                return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraSapera::SetDeviceExposure"));
            try
            {
                if (this.isCalibrationMode || !this.cameraInfo.IsLineScan)
                // Area Mode
                {
                    double framePerSec = 1000 / exposureTimeMs * this.ImageSize.Height;
                    SetDeviceFeature("AcquisitionFrameRate", framePerSec);
                }
                else
                // Line Mode
                {
                    int hz = (int)Math.Round(1000 / exposureTimeMs);
                    SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, hz);
                }


                return true;
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetDeviceExposureMs()
        {
            double framePerSec = double.NaN; ;
            if (((CameraInfoSapera)this.cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
                this.m_AcqDevice.GetFeatureValue("AcquisitionFrameRate", out framePerSec);

            return (float)(1000 / framePerSec);
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            if (!IsMasterDevice)
                return false;

            int iHz = (int)Math.Floor(hz);
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraSapera::SetAcquisitionLineRate {0:F3}kHz", iHz / 1000f));
            try
            {
                //int cur, max, min, max2, min2;
                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, out cur);
                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ_MAX, out max);
                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ_MIN, out min);

                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MAX, out max2);
                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MIN, out min2);

                //SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, iHz);
                //this.m_Acquisition.GetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, out cur);

                if (((CameraInfoSapera)this.cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
                    SetDeviceFeature("AcquisitionLineRate", hz);

                return true;
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetAcquisitionLineRate()
        {
            float hz = float.NaN;
            if (((CameraInfoSapera)this.cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
                this.m_AcqDevice.GetFeatureValue("AcquisitionLineRate", out hz);
            return hz;
        }

        protected override void UpdateGrabSpeed()
        {
            if (this.m_Xfer.UpdateFrameRateStatistics())
            {
                var stamp = this.m_Xfer.CounterStampInfo;
                int leng = stamp.Length;

                SapXferFrameRateInfo stats = this.m_Xfer.FrameRateStatistics;
                if (stats.IsLiveFrameRateAvailable)
                    this.grabPerSec = stats.LiveFrameRate;
            }
        }
    }
}
