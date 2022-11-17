using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoGenTL : CameraInfo
    {
        
        public enum ELineInputToolSource { DIN11, TTLIO11 }
        //public enum ETDIStages { TDI64, TDI128, TDI192, TDI256 }
        public enum ECxpLinkConfiguration { CXP3, CXP5, CXP6, CXP10, CXP12 }
        public enum EPRNUMode { On, Off }

        public override bool UseNativeBuffering
        {
            get => true;
            set => throw new NotSupportedException();
        }

        [CategoryAttribute("CameraInfoGenTL.Stream"), DescriptionAttribute("Frame Buffer Count")]
        public int FrameNum { get; set; } = 20;

        [Category("CameraInfoGenTL"), Description("Alloc in MIL Non-paged Area")]
        public bool UseMilBuffer { get; set; } = false;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("DF Client Type")]
        public EClientType ClientType { get; set; } = EClientType.Master;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Camera Scan Direction")]
        public EScanDirectionType DirectionType { get; set; } = EScanDirectionType.Forward;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("TDI Stages")]
        public ETDIStages TDIStages { get; set; } = ETDIStages.TDI128;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Degital Gain")]
        public float DigitalGain { get; set; } = 1.0f;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Analog Gain")]
        public EAnalogGain AnalogGain { get; set; } = EAnalogGain.X1;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Binning Vertical")]
        public bool BinningHorizontal { get; set; } = false;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Binning Vertical")]
        public bool BinningVertical { get; set; } = false;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Trigger Rescaler Mode")]
        public bool TriggerRescalerMode { get; set; } = true;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("Trigger Rescaler Rate")]
        public float TriggerRescalerRate { get; set; } = 4.0f;

        [Category("CameraInfoGenTL.Interface"), Description("LineInput Tool Source")]
        public ELineInputToolSource LineInputToolSource { get; set; } = ELineInputToolSource.DIN11;

        [Category("CameraInfoGenTL.Interface"), Description("CXP Connection Version")]
        public ECxpLinkConfiguration CxpLinkConfiguration { get; set; } = ECxpLinkConfiguration.CXP5;

        [Category("CameraInfoGenTL.RemoteDevice"), Description("PRNU Mode")]
        public EPRNUMode PRNUMode { get; set; } = EPRNUMode.Off;

        public CameraInfoGenTL()
        {
            GrabberType = GrabberType.GenTL;
            this.isLineScan = true;
            this.width = 17824;
            this.height = 15000;
            this.useNativeBuffering = true;
            this.frameType = EFrameType.Continuous;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            this.FrameNum = XmlHelper.GetValue(cameraElement, "FrameNum", this.FrameNum);
            this.UseMilBuffer = XmlHelper.GetValue(cameraElement, "UseMilBuffer", this.UseMilBuffer);
            this.ClientType = XmlHelper.GetValue(cameraElement, "ClientType", this.ClientType);
            this.DirectionType = XmlHelper.GetValue(cameraElement, "DirectionType", this.DirectionType);
            this.TDIStages = XmlHelper.GetValue(cameraElement, "TDIStages", this.TDIStages);
            this.DigitalGain = XmlHelper.GetValue(cameraElement, "DigitalGain", this.DigitalGain);
            this.AnalogGain = XmlHelper.GetValue(cameraElement, "AnalogGain", this.AnalogGain);
            this.BinningVertical = XmlHelper.GetValue(cameraElement, "BinningVertical", this.BinningVertical);
            this.BinningHorizontal = XmlHelper.GetValue(cameraElement, "BinningHorizontal", this.BinningHorizontal);
            this.TriggerRescalerMode = XmlHelper.GetValue(cameraElement, "TriggerRescalerMode", this.TriggerRescalerMode);
            this.TriggerRescalerRate = XmlHelper.GetValue(cameraElement, "TriggerRescalerRate", this.TriggerRescalerRate);            
            this.LineInputToolSource = XmlHelper.GetValue(cameraElement, "LineInputToolSource", this.LineInputToolSource);
            this.CxpLinkConfiguration = XmlHelper.GetValue(cameraElement, "CxpLinkConfiguration", this.CxpLinkConfiguration);
            this.PRNUMode = XmlHelper.GetValue(cameraElement, "PRNUMode", this.PRNUMode);

            //this.isLineScan = true;
            //this.frameType = EFrameType.Continuous;
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "FrameNum", this.FrameNum);
            XmlHelper.SetValue(cameraElement, "UseMilBuffer", this.UseMilBuffer);
            XmlHelper.SetValue(cameraElement, "ClientType", this.ClientType);
            XmlHelper.SetValue(cameraElement, "DirectionType", this.DirectionType);
            XmlHelper.SetValue(cameraElement, "TDIStages", this.TDIStages);
            XmlHelper.SetValue(cameraElement, "DigitalGain", this.DigitalGain);
            XmlHelper.SetValue(cameraElement, "AnalogGain", this.AnalogGain);
            XmlHelper.SetValue(cameraElement, "BinningVertical", this.BinningVertical);
            XmlHelper.SetValue(cameraElement, "BinningHorizontal", this.BinningHorizontal);
            XmlHelper.SetValue(cameraElement, "TriggerRescalerMode", this.TriggerRescalerMode);
            XmlHelper.SetValue(cameraElement, "TriggerRescalerRate", this.TriggerRescalerRate);
            XmlHelper.SetValue(cameraElement, "LineInputToolSource", this.LineInputToolSource);
            XmlHelper.SetValue(cameraElement, "CxpLinkConfiguration", this.CxpLinkConfiguration);
            XmlHelper.SetValue(cameraElement, "PRNUMode", this.PRNUMode);
        }
    }

    public class GrabberGenTL : Grabber
    {
        static int cntOpenDriver = 0;

        public GrabberGenTL(string name) : base(GrabberType.GenTL, name)
        {
      
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraGenTL(cameraInfo);           
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            GenTLCameraListForm form = new GenTLCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");
            
            cntOpenDriver++;
            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;
        }
    }
}
