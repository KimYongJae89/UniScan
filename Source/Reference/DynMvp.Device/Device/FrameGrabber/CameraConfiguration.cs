using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using DynMvp.Base;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DynMvp.Devices.FrameGrabber
{
    public enum RotateFlipType
    {
        RotateNoneFlipNone,
        Rotate180FlipXY,
        Rotate90FlipNone,
        Rotate270FlipXY,
        Rotate180FlipNone,
        RotateNoneFlipXY,
        Rotate270FlipNone,
        Rotate90FlipXY,
        RotateNoneFlipX,
        Rotate180FlipY,
        Rotate90FlipX,
        Rotate270FlipY,
        Rotate180FlipX,
        RotateNoneFlipY,
        Rotate270FlipX,
        Rotate90FlipY
    }

    public static class RotateFlipTypeMethod
    {
        public static bool IsRotate0(this RotateFlipType r) => (int)r / 2 == 0;
        public static bool IsRotate90(this RotateFlipType r) => (int)r / 2 == 1;
        public static bool IsRotate180(this RotateFlipType r) => (int)r / 2 == 2;
        public static bool IsRotate270(this RotateFlipType r) => (int)r / 2 == 3;
        public static bool IsFlipX(this RotateFlipType r) => (int)r / 2 == 4;
        public static bool IsRotate90X(this RotateFlipType r) => (int)r / 2 == 5;
        public static bool IsFlipY(this RotateFlipType r) => (int)r / 2 == 6;
        public static bool IsRotate90Y(this RotateFlipType r) => (int)r / 2 == 7;
        public static System.Drawing.RotateFlipType ConvertTo(this RotateFlipType r) => (System.Drawing.RotateFlipType)(((int)r) / 2);
    }

    public class CameraInfo
    {
        public enum EFrameType { Continuous, Partial }

        [Category("CameraInfo"), Description("Name")]
        public string Name
        {
            get => string.IsNullOrEmpty(this.name) ? string.Format("Camera{0}", this.index) : this.name;
            set => this.name = value;
        }
        string name;

        [Category("CameraInfo"), Description("Index"), ReadOnly(false)]
        public int Index { get => this.index; set => this.index = value; }
        protected int index;

        [Category("CameraInfo"), Description("Grabber Type"), ReadOnly(true)]
        public GrabberType GrabberType { get => this.grabberType; set => this.grabberType = value; }
        GrabberType grabberType;

        [Category("CameraInfo"), Description("Use Camera")]
        public bool Enabled { get => this.enabled; set => this.enabled = value; }
        bool enabled = true;

        [Category("CameraInfo"), Description("Image Offset X")]
        public uint OffsetX { get => this.offsetX; set => this.offsetX = value; }
        uint offsetX = 0;

        [Category("CameraInfo"), Description("Image Width")]
        public int Width { get => this.width; set => this.width = value; }
        protected int width = 1000;

        [Category("CameraInfo"), Description("Image Height")]
        public int Height { get => this.height; set => this.height = value; }
        protected int height = 1000;

        [Category("CameraInfo"), Description("Pixel Format")]
        public PixelFormat PixelFormat { get => this.pixelFormat; set => this.pixelFormat = value; }
        protected PixelFormat pixelFormat = PixelFormat.Format8bppIndexed;

        [Category("CameraInfo"), Description("Bayer Camera")]
        public bool BayerCamera { get => this.bayerCamera; set => this.bayerCamera = value; }
        protected bool bayerCamera = false;

        [Category("CameraInfo"), Description("Bayer Type")]
        internal BayerType BayerType { get => this.bayerType; set => this.bayerType = value; }
        protected BayerType bayerType = BayerType.GB;

        [Category("CameraInfo"), Description("Manual PRNU")]
        public float[] WhiteBalanceCoefficient { get => this.whiteBalanceCoefficient; set => this.whiteBalanceCoefficient = value; }
        protected float[] whiteBalanceCoefficient = null;

        [Category("CameraInfo"), Description("Image Rotation")]
        public RotateFlipType RotateFlipType { get => this.rotateFlipType; set => this.rotateFlipType = value; }
        protected RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

        [Category("CameraInfo"), Description("Use image pointer instead of data")]
        public virtual bool UseNativeBuffering { get => this.useNativeBuffering; set => this.useNativeBuffering = value; }
        protected bool useNativeBuffering = true;

        [Category("CameraInfo"), Description("Scan Type")]
        public bool IsLineScan { get => this.isLineScan; set => this.isLineScan = value; }
        protected bool isLineScan = false;

        [Category("CameraInfo"), Description("Frame Type")]
        public EFrameType FrameType { get => this.frameType; set => this.frameType = value; }
        protected EFrameType frameType = EFrameType.Partial;

        [Category("CameraInfo"), Description("Virtual Image Path")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string VirtualImagePath
        {
            get => this.virtualImagePath;
            set => this.virtualImagePath = value;
        }
        protected string virtualImagePath = "";

        [Category("CameraInfo"), Description("Virtual Image Name Format")]
        //[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string VirtualImageNameFormat
        {
            get => this.virtualImageNameFormat;
            set => this.virtualImageNameFormat = value;
        }
        protected string virtualImageNameFormat = "Image_C??_S???_L??.bmp";

        public CameraInfo()
        {
            this.grabberType = GrabberType.Virtual;
            this.name = "Default";
        }

        public int GetNumBand()
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 1;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 2;
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 4;
            }

            return 0;
        }

        public void SetNumBand(int numBand)
        {
            switch (numBand)
            {
                case 1:
                    pixelFormat = PixelFormat.Format8bppIndexed;
                    break;
                case 2:
                    pixelFormat = PixelFormat.Format16bppRgb565;
                    break;
                case 3:
                    pixelFormat = PixelFormat.Format24bppRgb;
                    break;
                case 4:
                    pixelFormat = PixelFormat.Format32bppRgb;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
        }
        public static CameraInfo Create(GrabberType grabberType)
        {
            CameraInfo cameraInfo;
            switch (grabberType)
            {
                case GrabberType.MultiCam:
                    cameraInfo = new CameraInfoMultiCam(); break;
                case GrabberType.Pylon:
                case GrabberType.PylonLine:
                    cameraInfo = new CameraInfoPylon(); break;
                case GrabberType.Pylon2:
                    cameraInfo = new CameraInfoPylon2(); break;
                case GrabberType.MIL:
                    cameraInfo = new CameraInfoMil(); break;
                case GrabberType.GenTL:
                    cameraInfo = new CameraInfoGenTL(); break;
                case GrabberType.Sapera:
                    cameraInfo = new CameraInfoSapera(); break;

                case GrabberType.HIKLine:
                    cameraInfo = new CameraInfoHIKLine(); break;


                case GrabberType.Virtual:
                default:
                    cameraInfo = new CameraInfo(); break;
            }

            return cameraInfo;
        }

        public virtual void LoadXml(XmlElement cameraElement)
        {
            this.index = XmlHelper.GetValue(cameraElement, "Index", this.index);
            this.name = XmlHelper.GetValue(cameraElement, "Name", "");
            this.grabberType = XmlHelper.GetValue(cameraElement, "Type", this.grabberType);
            this.enabled = XmlHelper.GetValue(cameraElement, "Enabled", this.enabled);
            this.width = XmlHelper.GetValue(cameraElement, "Width", this.width);
            this.height = XmlHelper.GetValue(cameraElement, "Height", this.height);
            this.offsetX = XmlHelper.GetValue(cameraElement, "OffsetX", this.offsetX);

            this.bayerCamera = XmlHelper.GetValue(cameraElement, "BayerCamera", this.bayerCamera);
            this.pixelFormat = XmlHelper.GetValue(cameraElement, "PixelFormat", this.pixelFormat);
            this.rotateFlipType = XmlHelper.GetValue(cameraElement, "RotateFlipType", this.rotateFlipType);
            this.useNativeBuffering = XmlHelper.GetValue(cameraElement, "UseNativeBuffering", this.useNativeBuffering);
            this.isLineScan = XmlHelper.GetValue(cameraElement, "IsLineScan", this.isLineScan);
            this.frameType = XmlHelper.GetValue(cameraElement, "FrameType", this.frameType);
            this.virtualImagePath = XmlHelper.GetValue(cameraElement, "VirtualImagePath", this.virtualImagePath);
            this.virtualImageNameFormat = XmlHelper.GetValue(cameraElement, "VirtualImageNameFormat", this.virtualImageNameFormat);

            if (string.IsNullOrEmpty(this.virtualImageNameFormat) || this.virtualImageNameFormat.ToLower() == "*.bmp")
                this.virtualImageNameFormat = $"Image_C{this.index:00}_S???_L??.bmp";
        }

        public virtual void SaveXml(XmlElement cameraElement)
        {
            XmlHelper.SetValue(cameraElement, "Index", this.index);
            XmlHelper.SetValue(cameraElement, "Name", this.name);
            XmlHelper.SetValue(cameraElement, "Type", this.grabberType);
            XmlHelper.SetValue(cameraElement, "Enabled", this.enabled);
            XmlHelper.SetValue(cameraElement, "Width", this.width);
            XmlHelper.SetValue(cameraElement, "Height", this.height);
            XmlHelper.SetValue(cameraElement, "OffsetX", this.offsetX);

            XmlHelper.SetValue(cameraElement, "BayerCamera", this.bayerCamera);
            XmlHelper.SetValue(cameraElement, "PixelFormat", this.pixelFormat);
            XmlHelper.SetValue(cameraElement, "RotateFlipType", this.rotateFlipType);
            XmlHelper.SetValue(cameraElement, "UseNativeBuffering", this.useNativeBuffering);
            XmlHelper.SetValue(cameraElement, "IsLineScan", this.isLineScan);
            XmlHelper.SetValue(cameraElement, "FrameType", this.frameType);
            XmlHelper.SetValue(cameraElement, "VirtualImagePath", this.virtualImagePath?.ToString());
            XmlHelper.SetValue(cameraElement, "VirtualImageNameFormat", this.virtualImageNameFormat?.ToString());
        }
    }

    public class CameraConfiguration : System.Collections.IEnumerable
    {
        public static string ConfigFlag { get; set; } = "";

        public int RequiredCameras => this.cameraInfos.Length;

        public CameraInfo[] CameraInfos => this.cameraInfos;
        CameraInfo[] cameraInfos = null;

        public CameraConfiguration(int requiredCameras)
        {
            this.cameraInfos = new CameraInfo[requiredCameras];
        }

        public CameraConfiguration(string fileName)
        {
            try
            {
                LoadCameraConfiguration(fileName);
            }
            catch { }
        }

        //public void Clear()
        //{
        //    cameraInfos.Clear();
        //}

        public void SetCameraInfo(CameraInfo cameraInfo)
        {
            cameraInfos[cameraInfo.Index] = cameraInfo;
        }

        public void SetDefault(GrabberType grabberType)
        {
            List<CameraInfo> cameraInfoList = new List<CameraInfo>();
            for (int i = 0; i < RequiredCameras; i++)
                cameraInfoList.Add(CameraInfo.Create(grabberType));
            this.cameraInfos = cameraInfoList.ToArray();
        }

        public void LoadCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Camera Configuration");

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                throw new System.IO.FileNotFoundException();

            List<CameraInfo> cameraInfoList = new List<CameraInfo>();
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.GetElementsByTagName("Camera");
            foreach (XmlElement cameraElement in xmlNodeList)
            {
                GrabberType grabberType = XmlHelper.GetValue(cameraElement, "Type", GrabberType.Virtual);

                CameraInfo cameraInfo = CameraInfo.Create(grabberType);
                cameraInfo.LoadXml(cameraElement);

                cameraInfoList.Add(cameraInfo);
            }

            this.cameraInfos = cameraInfoList.ToArray();
        }

        public void SaveCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Camera Configuration");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement cameraListElement = xmlDocument.CreateElement("", "CameraList", "");
            xmlDocument.AppendChild(cameraListElement);

            foreach (CameraInfo cameraInfo in cameraInfos)
            {
                XmlElement cameraElement = xmlDocument.CreateElement("", "Camera", "");
                cameraListElement.AppendChild(cameraElement);

                cameraInfo.SaveXml(cameraElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }

        public IEnumerator GetEnumerator()
        {
            return cameraInfos.GetEnumerator();
        }
    }
}
