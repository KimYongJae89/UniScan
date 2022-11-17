using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using Euresys.MultiCam;
using DynMvp.Devices.FrameGrabber.UI;
using System.ComponentModel;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoMultiCam : CameraInfo
    {
        [Category("CameraInfoMultiCam"), Description("BoardType")]
        public EuresysBoardType BoardType { get => this.boardType; set => this.boardType = value; }
        EuresysBoardType boardType;

        [Category("CameraInfoMultiCam"), Description("BoardId")]
        public uint BoardId { get => this.boardId; set => this.boardId = value; }
        uint boardId;

        [Category("CameraInfoMultiCam"), Description("ConnectorId")]
        public uint ConnectorId { get => this.connectorId; set => this.connectorId = value; }
        uint connectorId;

        [Category("CameraInfoMultiCam"), Description("CameraType")]
        public CameraType CameraType { get => this.cameraType; set => this.cameraType = value; }
        CameraType cameraType;

        [Category("CameraInfoMultiCam"), Description("SurfaceNum")]
        public uint SurfaceNum { get => this.surfaceNum; set => this.surfaceNum = value; }
        uint surfaceNum;

        [Category("CameraInfoMultiCam"), Description("TriggerChannel")]
        public int TriggerChannel { get; set; }

        public CameraInfoMultiCam()
        {
            GrabberType = GrabberType.MultiCam;
        }

        public CameraInfoMultiCam(EuresysBoardType boardType, uint boardId, uint connectorId, CameraType cameraType, uint surfaceNum, uint pageLength)
        {
            this.GrabberType = GrabberType.MultiCam;

            this.boardType = boardType;
            this.boardId = boardId;
            this.connectorId = connectorId;
            this.cameraType = cameraType;
            this.surfaceNum = surfaceNum;
            this.height = (int)pageLength;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            boardType = (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), XmlHelper.GetValue(cameraElement, "BoardType", "GrabLink_Base"));
            boardId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "BoardId", "0"));
            connectorId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ConnectorId", "0"));
            cameraType = (CameraType)Enum.Parse(typeof(CameraType), XmlHelper.GetValue(cameraElement, "CameraType", "Jai_GO_5000"));
            surfaceNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "SurfaceNum", "1"));
            this.TriggerChannel= Convert.ToInt32(XmlHelper.GetValue(cameraElement, "TriggerChannel", "0"));
            uint pageLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "PageLength", "0"));
            if (pageLength > 0)
                this.height = (int)pageLength;
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "BoardType", boardType.ToString());
            XmlHelper.SetValue(cameraElement, "BoardId", boardId.ToString());
            XmlHelper.SetValue(cameraElement, "ConnectorId", connectorId.ToString());
            XmlHelper.SetValue(cameraElement, "CameraType", cameraType.ToString());
            XmlHelper.SetValue(cameraElement, "SurfaceNum", surfaceNum.ToString());
            XmlHelper.SetValue(cameraElement, "TriggerChannel", TriggerChannel.ToString());
            //XmlHelper.SetValue(cameraElement, "PageLength", pageLength.ToString());
        }
    }

    public class GrabberMultiCam : Grabber
    {
        static int cntOpenDriver = 0;

        public GrabberMultiCam(string name) : base(GrabberType.MultiCam, name)
        {
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraMultiCam(cameraInfo);
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            EuresysBoardListForm form = new EuresysBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            // Open MultiCam driver
            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.StartUp, "Open MultiCam Driver");
                MC.OpenDriver();
            }

            cntOpenDriver++;

            // Enable error logging
            MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");

            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;

            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.Shutdown, "Release MultiCam Driver");
                MC.CloseDriver();
            }
        }
    }
}
