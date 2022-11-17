using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using DALSA.SaperaLT.SapClassBasic;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices.FrameGrabber.UI;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoSapera : CameraInfo
    {
        public enum EClientType { Master, Slave }
        public enum EScanDirectionType { Forward, Reverse }
        public enum ETDIMode { TdiMTF, Tdi }

        [Serializable]
        public class __EXT_FRAME_TRIGGER
        {
            public enum EMode { None, FrameTrigger, RisingSnap, RisingGrabFallingStop }

            public enum EDETECTION
            {
                HIGH = SapAcquisition.Val.ACTIVE_HIGH,
                LOW = SapAcquisition.Val.ACTIVE_LOW
            }

            public enum ELEVEL
            {
                TTL = SapAcquisition.Val.LEVEL_TTL,
                V12_24 = SapAcquisition.Val.LEVEL_24VOLTS,
                //V24 = SapAcquisition.Val.LEVEL_24VOLTS
            }

            [CategoryAttribute("CameraInfoSapera_EXT_FRAME_TRIGGER"), DescriptionAttribute("ENABLE")]
            public EMode MODE { get; set; } =  EMode.None;

            [CategoryAttribute("CameraInfoSapera_EXT_FRAME_TRIGGER"), DescriptionAttribute("DETECTION")]
            public EDETECTION DETECTION { get; set; } = EDETECTION.HIGH;

            [CategoryAttribute("CameraInfoSapera_EXT_FRAME_TRIGGER"), DescriptionAttribute("LEVEL")]
            public ELEVEL LEVEL { get; set; } = ELEVEL.V12_24;

            [CategoryAttribute("CameraInfoSapera_EXT_FRAME_TRIGGER"), DescriptionAttribute("SOURCE")]
            public int SOURCE { get; set; } = 1;

            [CategoryAttribute("CameraInfoSapera_EXT_FRAME_TRIGGER"), DescriptionAttribute("DURATION. 0~255")]
            public byte DURATION{ get; set; } = 1;

            public override string ToString()
            {
                return this.MODE.ToString();
            }
        }

        [Serializable]
        public class __SHAFT_ENCODER
        {
            public enum EDiRECTION
            {
                IGNORE = SapAcquisition.Val.SHAFT_ENCODER_DIRECTION_IGNORE,
                FORWARD = SapAcquisition.Val.SHAFT_ENCODER_DIRECTION_FORWARD,
                REVERSE = SapAcquisition.Val.SHAFT_ENCODER_DIRECTION_REVERSE
            }

            public enum EMULTIPLY
            {
                M01 = 1, M02 = 2, M04 = 4, M08 = 8, M16 = 16, M32 = 32,
            }

            public enum EORDER
            {
                Auto = SapAcquisition.Val.SHAFT_ENCODER_ORDER_AUTO,
                DROP_MULTIPLY = SapAcquisition.Val.SHAFT_ENCODER_ORDER_DROP_MULTIPLY,
                MULTIPLY_DROP = SapAcquisition.Val.SHAFT_ENCODER_ORDER_MULTIPLY_DROP
            }
            [CategoryAttribute("CameraInfoSapera_SHAFT_ENCODER"), DescriptionAttribute("DIRECTION")]
            public EDiRECTION DIRECTION { get; set; } = EDiRECTION.IGNORE;

            [CategoryAttribute("CameraInfoSapera_SHAFT_ENCODER"), DescriptionAttribute("DROP")]
            public int DROP { get; set; } = 0;

            [CategoryAttribute("CameraInfoSapera_SHAFT_ENCODER"), DescriptionAttribute("MULTIPLY")]
            public EMULTIPLY MULTIPLY { get; set; } = EMULTIPLY.M01; // 1,2,4,8,16,32

            [CategoryAttribute("CameraInfoSapera_SHAFT_ENCODER"), DescriptionAttribute("ORDER")]
            public EORDER ORDER { get; set; } = EORDER.Auto;

            public override string ToString()
            {
                return this.DIRECTION.ToString();
            }
        }

        [Serializable]
        public class __Gpio
        {
            public enum ELineSelector
            {
                NONE = -1, GPIO0, GPIO1, GPIO2, GPIO3, GPIO4, GPIO5, GPIO6, GPIO7
            }

            [CategoryAttribute("CameraInfoSapera_Gpio"), DescriptionAttribute("LineSelector")]
            public ELineSelector LineSelector { get; set; } = ELineSelector.NONE;

            [CategoryAttribute("CameraInfoSapera_Gpio"), DescriptionAttribute("outputLineSource")]
            public bool outputLineSource { get; set; } = false;

            [CategoryAttribute("CameraInfoSapera_Gpio"), DescriptionAttribute("outputLinePulseDelay")]
            public int outputLinePulseDelay { get; set; } = 0;

            [CategoryAttribute("CameraInfoSapera_Gpio"), DescriptionAttribute("outputLinePulseDuration")]
            public int outputLinePulseDuration { get; set; } = 0;

            [CategoryAttribute("CameraInfoSapera_Gpio"), DescriptionAttribute("LineInverter")]
            public bool LineInverter { get; set; } = false;

            public override string ToString()
            {
                string s = outputLineSource ? "Use" : "Unuse";
                return $"{LineSelector}: {s}";
            }
        }


        [Category("CameraInfoSapera"), Description("Server Name")]
        public string ServerName { get => this.serverName; set => this.serverName = value; }
        string serverName = "Xtium2-CLHS_PX8_1";

        [Category("CameraInfoSapera"), Description("Server Resource Index")]
        public int ServerResourceIndex { get => this.serverResourceIndex; set => this.serverResourceIndex = value; }
        int serverResourceIndex = 0;

        [Category("CameraInfoSapera"), Description("Event Notify Counter")]
        public int EventNotifyCounter { get => this.eventNotifyCounter; set => this.eventNotifyCounter = value; }
        int eventNotifyCounter;

        [Category("CameraInfoSapera"), Description("CCF File Path")]
        public string CcfFilePath { get => this.ccfFilePath; set => this.ccfFilePath = value; }
        string ccfFilePath = "";

        [Category("CameraInfoSapera"), Description("Frame Buffer Count")]
        public int FrameNum { get => this.frameNum; set => this.frameNum = value; }
        int frameNum = 20;

        [Category("CameraInfoSapera"), Description("DF Client Type")]
        public EClientType ClientType { get => this.clientType; set => this.clientType = value; }
        EClientType clientType = EClientType.Master;

        [Category("CameraInfoSapera"), Description("Camera Scan Direction")]
        public EScanDirectionType DirectionType { get => this.directionType; set => this.directionType = value; }
        EScanDirectionType directionType = EScanDirectionType.Forward;

        [Category("CameraInfoSapera"), Description("Area Mode Frame Size")]
        public Size TdiArea { get; set; }

        [Category("CameraInfoSapera"), Description("TDI Mode")]
        public ETDIMode TdiMode { get; set; }

        // External Frame Trigger
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CameraInfoSapera"), Description("External Frame Tringger")]
        public __EXT_FRAME_TRIGGER ExtFrameTringger => this.extFrameTringger;
        __EXT_FRAME_TRIGGER extFrameTringger = new __EXT_FRAME_TRIGGER();

        // External Line Trigger
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CameraInfoSapera"), Description("External Line Tringger")]
        public __SHAFT_ENCODER ExtLineTringger => this.extLineTringger;
        __SHAFT_ENCODER extLineTringger = new __SHAFT_ENCODER();

        // Camera Strobe Channel
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CameraInfoSapera"), Description("Trigger Output")]
        public __Gpio CameraStrobIo => this.cameraStrobIo;
        __Gpio cameraStrobIo = new __Gpio();

        public CameraInfoSapera()
        {
            GrabberType = GrabberType.Sapera;

            this.useNativeBuffering = true;
            this.width = 32768;
            this.height = 65535;

            this.eventNotifyCounter = 20;
            
            this.frameNum = 5;
            this.TdiArea = new Size(16384, 128);
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            this.serverName = XmlHelper.GetValue(cameraElement, "ServerName", this.serverName);
            this.serverResourceIndex = XmlHelper.GetValue(cameraElement, "ServerResourceIndex", this.serverResourceIndex);
            this.eventNotifyCounter = XmlHelper.GetValue(cameraElement, "EventNotifyCounter", this.eventNotifyCounter);
            this.ccfFilePath = XmlHelper.GetValue(cameraElement, "CcfFilePath", this.ccfFilePath);

            this.frameNum = XmlHelper.GetValue(cameraElement, "FrameNum", this.frameNum);
            this.clientType = XmlHelper.GetValue(cameraElement, "ClientType", this.clientType);
            this.directionType = XmlHelper.GetValue(cameraElement, "DirectionType", this.directionType);
            this.TdiArea = XmlHelper.GetValue(cameraElement, "TdiArea", this.TdiArea);
            this.TdiMode = XmlHelper.GetValue(cameraElement, "TdiMode", this.TdiMode);

            string extFrameTringgerValue = XmlHelper.GetValue(cameraElement, "ExtFrameTringgerValue", "");
            this.extFrameTringger = SerializeHelper.Deserialize<__EXT_FRAME_TRIGGER>(extFrameTringgerValue);

            string extLineTringgerValue = XmlHelper.GetValue(cameraElement, "ExtLineTringger", "");
            this.extLineTringger = SerializeHelper.Deserialize<__SHAFT_ENCODER>(extLineTringgerValue);

            string cameraStrobIoValue = XmlHelper.GetValue(cameraElement, "CameraStrobIo", "");
            this.cameraStrobIo = SerializeHelper.Deserialize<__Gpio>(cameraStrobIoValue);


            // 호환성을 위해 임시로...
            if (XmlHelper.Exist(cameraElement, "IsStopAndGo"))
            {
                bool isStopAndGo = XmlHelper.GetValue(cameraElement, "IsStopAndGo", true);
                this.frameType = isStopAndGo ? EFrameType.Partial : EFrameType.Continuous;
            }
        }

        private T Deserialize<T>(string value)
            where T : class, new()
        {
            T obj = new T();
            byte[] bytes = Convert.FromBase64String(value);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    obj = (T)binaryFormatter.Deserialize(ms);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("CameraInfoSapera::Deserialize<{0}>  - {1}", typeof(T).Name, ex.Message));
                }
            }
            return obj;
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "ServerName", this.serverName);
            XmlHelper.SetValue(cameraElement, "ServerResourceIndex", this.serverResourceIndex);
            XmlHelper.SetValue(cameraElement, "EventNotifyCounter", this.eventNotifyCounter);
            XmlHelper.SetValue(cameraElement, "CcfFilePath", this.ccfFilePath);

            XmlHelper.SetValue(cameraElement, "FrameNum", this.frameNum);
            XmlHelper.SetValue(cameraElement, "ClientType", this.clientType);
            XmlHelper.SetValue(cameraElement, "DirectionType", this.directionType);
            XmlHelper.SetValue(cameraElement, "TdiArea", this.TdiArea);
            XmlHelper.SetValue(cameraElement, "TdiMode", this.TdiMode);

            string extFrameTringgerValue = SerializeHelper.Serialize(this.extFrameTringger);
            XmlHelper.SetValue(cameraElement, "ExtFrameTringgerValue", extFrameTringgerValue);

            string extLineTringgerValue = SerializeHelper.Serialize(this.extLineTringger);
            XmlHelper.SetValue(cameraElement, "ExtLineTringger", extLineTringgerValue);

            string cameraStrobIoValue = SerializeHelper.Serialize(this.cameraStrobIo);
            XmlHelper.SetValue(cameraElement, "CameraStrobIo", cameraStrobIoValue);

        }

        private string Serialize(object obj)
        {
            string str = "";
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, obj);
                //ms.Position = 0;
                str = Convert.ToBase64String(ms.GetBuffer());
            }
            return str;
        }
    }

    public class GrabberSapera : Grabber
    {
        static int cntOpenDriver = 0;


        public GrabberSapera(string name) : base(GrabberType.Sapera, name)
        {

        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraSapera(cameraInfo);
        }

        //public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        //{
        //    GenTLCameraListForm form = new GenTLCameraListForm();
        //    form.CameraConfiguration = cameraConfiguration;
        //    return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        //}

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");
#if DEBUG
                        SapManager.DisplayStatusMode = SapManager.StatusMode.Popup;
#else
            SapManager.DisplayStatusMode = SapManager.StatusMode.Exception;
#endif

            if (cntOpenDriver == 0)
            {
                ResetGrabber();
                SapManager.Open();
            }

            cntOpenDriver++;
            return true;
        }

        private void SapManager_Error(object sender, SapErrorEventArgs e)
        {
            LogHelper.Error(LoggerType.Error, string.Format("GrabberSapera::SapManager_Error - Code: {0}, Message: {1}, Context: {2}", e.Code.ToString(), e.Message, e.Context));
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;
            if (cntOpenDriver == 0)
            {
                SapManager.Close();
                //ResetGrabber();
            }
        }

        private void ResetGrabber()
        {
            // Reset Server
            Dictionary<int, bool> isResetDone = new Dictionary<int, bool>();
            System.Threading.ManualResetEvent mse = new System.Threading.ManualResetEvent(false);

            SapManager.EndReset += new SapResetHandler((s, e) =>
            {
                isResetDone[e.ServerIndex] = true;
                if (isResetDone.Values.All(f => f))
                    mse.Set();
            });

            //SapAcqDevice sapAcqDevice = new SapAcqDevice(new SapLocation(0, 0), @"C:\Program Files\Teledyne DALSA\Sapera\CamFiles\User\3_32K_MASTER007_32K_MASTER007_32K_MASTER007.ccf");
            //sapAcqDevice.Create();
            //int c = sapAcqDevice.FeatureCount;
            try
            {
                SapManager.DetectAllServers(SapManager.DetectServerType.All);
                int totalServerCnt = SapManager.GetServerCount(SapManager.ResourceType.Acq);
                for (int i = 0; i < totalServerCnt; i++)
                {
                    string serverName = SapManager.GetServerName(i, SapManager.ResourceType.Acq);
                    int serverIndex = SapManager.GetServerIndex(serverName);
                    isResetDone.Add(serverIndex, false);
                    //isResetDone.Add(i, false);

                    int totalCameraCnt = SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.AcqDevice);
                    for (int j = 0; j < totalCameraCnt; j++)
                    {
                        string resName = SapManager.GetResourceName(serverIndex, SapManager.ResourceType.AcqDevice, j);
                        int resIndex = SapManager.GetResourceIndex(serverIndex, SapManager.ResourceType.AcqDevice, resName);
                        SapLocation location = new SapLocation(serverIndex, resIndex);

                        SapAcqDevice sapAcqDevice = new SapAcqDevice(location);
                        sapAcqDevice.Create();

                        //sapAcqDevice.UpdateFeaturesFromDevice();
                        //string[] fns = sapAcqDevice.FeatureNames;
                        //Array.ForEach(fns, f =>
                        //{
                        //    SapFeature feature = new SapFeature(location);
                        //    feature.Create();
                        //    sapAcqDevice.GetFeatureInfo(f, feature);

                        //    object obj = "NULL";

                        //    switch (feature.DataType)
                        //    {
                        //        case SapFeature.Type.Bool:
                        //            sapAcqDevice.GetFeatureValue(f, out bool bValue);
                        //            obj = bValue;
                        //            break;                               
                        //        case SapFeature.Type.Double:
                        //            sapAcqDevice.GetFeatureValue(f, out double dValue);
                        //            obj = dValue;
                        //            break;
                        //        case SapFeature.Type.Float:
                        //            sapAcqDevice.GetFeatureValue(f, out float fValue);
                        //            obj = fValue;
                        //            break;
                        //        case SapFeature.Type.Int32:
                        //            sapAcqDevice.GetFeatureValue(f, out Int32 iValue);
                        //            obj = iValue;
                        //            break;
                        //        case SapFeature.Type.Int64:
                        //            sapAcqDevice.GetFeatureValue(f, out long lValue);
                        //            obj = lValue;
                        //            break;
                        //        case SapFeature.Type.String:
                        //            sapAcqDevice.GetFeatureValue(f, out string strValue);
                        //            obj = strValue;
                        //            break;
                        //    }
                        //    System.Diagnostics.Debug.WriteLine($"{f}\t{feature.DisplayName}\t{obj}");
                        //});

                        //sapAcqDevice.SetFeatureValue("DeviceReset", true);

                        //SapBuffer buffer = new SapBufferWithTrash(1, sapAcqDevice, SapBuffer.MemoryType.ScatterGather);
                        //buffer.Create();
                        //SapTransfer t = new SapAcqDeviceToBuf(sapAcqDevice, buffer);
                        //t.Create();
                        //t.AutoConnect = true;
                        //while (!t.Connected)
                        //{
                        //    t.Connect();
                        //    System.Threading.Thread.Sleep(10000);
                        //}
                        sapAcqDevice.Dispose();
                    }
                }

                //System.Threading.Thread.Sleep(10000);
                foreach (int index in isResetDone.Keys)
                    SapManager.ResetServer(index, false);

                if (!mse.WaitOne(5000))
                    throw new AlarmException(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Fatal, this.name, "Reset Wait Timeout", null, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("GrabberSapera::ResetGrabber - Exception : {0}", ex.Message));
                throw ex;
            }
        }
    }
}
