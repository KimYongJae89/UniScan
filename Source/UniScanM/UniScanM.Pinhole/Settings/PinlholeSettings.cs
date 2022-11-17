using DynMvp.Barcode;
using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniScanM.Pinhole.Settings
{
    public class PinholeSettings : UniScanM.Settings.UniScanMSettings
    {
        private int totalRollLength = 5000;
        public int TotalRollLength
        {
            get { return totalRollLength; }
            set { totalRollLength = value; }
        }

        private int maxDefect = 5;
        public int MaxDefect { get => maxDefect; set => maxDefect = value; }

        private int defectPenSize = 5;
        public int DefectPenSize { get => defectPenSize; set => defectPenSize = value; }

        int skipLength = 54;
        public int SkipLength { get => skipLength; set => skipLength = value; }

        int mergeCheckTop = 30;
        public int MergeCheckTop { get => mergeCheckTop; set => mergeCheckTop = value; }

        int mergeCheckBottom = 30;
        public int MergeCheckBottom { get => mergeCheckBottom; set => mergeCheckBottom = value; }

        SizeF cam1PelSize = new SizeF(122, 122);
        public SizeF Cam1PelSize { get => cam1PelSize; set => cam1PelSize = value; }

        SizeF cam2PelSize = new SizeF(122, 122);
        public SizeF Cam2PelSize { get => cam2PelSize; set => cam2PelSize = value; }

        int pixelResolution = 129;
        public int PixelResolution { get => pixelResolution; set => pixelResolution = value; }
        
        SizeF smallSize = new SizeF(2.5f,2.5f);
        public SizeF SmallSize { get => smallSize; set => smallSize = value; }

        SizeF midSize = new SizeF(5f,5f);
        public SizeF MidSize { get => midSize; set => midSize = value; }

        SizeF bigSize = new SizeF(5f,5f);
        public SizeF BigSize { get => bigSize; set => bigSize = value; }
        
        float resizeRatio = 0.2f;
        public float ResizeRatio { get => resizeRatio; set => resizeRatio = value; }
        
        bool useReject = true;
        public bool UseReject { get => useReject; set => useReject = value; }

        int defalutLightValue = 31;
        public int DefalutLightValue { get => defalutLightValue; set => defalutLightValue = value; }


        public static PinholeSettings Instance()
        {
            return instance as PinholeSettings;
        }

        protected PinholeSettings()
        {
            this.skipLength = 54;
            this.mergeCheckTop = 30;
            this.mergeCheckBottom = 30;
            this.cam1PelSize = new SizeF(122, 122);
            this.cam2PelSize = new SizeF(122, 122);
            this.totalRollLength = 5000;
            this.maxDefect = 5;
            this.pixelResolution = 129;
            this.smallSize = new SizeF(2.5f, 2.5f);
            this.midSize = new SizeF(5, 5);
            this.bigSize = new SizeF(5, 5);
            this.useReject = true;
            this.defalutLightValue = 31;
            this.defectPenSize = 5;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new PinholeSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "SkipLength", skipLength);
            XmlHelper.SetValue(xmlElement, "MergeCheckTop", mergeCheckTop);
            XmlHelper.SetValue(xmlElement, "MergeCheckBottom", mergeCheckBottom);
            XmlHelper.SetValue(xmlElement, "Cam1PelSize", cam1PelSize);
            XmlHelper.SetValue(xmlElement, "Cam2PelSize", cam2PelSize);
            XmlHelper.SetValue(xmlElement, "TotalRollLength", totalRollLength.ToString());
            XmlHelper.SetValue(xmlElement, "MaxDefectCount", maxDefect.ToString());
            XmlHelper.SetValue(xmlElement, "PixelResolution", pixelResolution.ToString());
            XmlHelper.SetValue(xmlElement, "SmallSize", smallSize);
            XmlHelper.SetValue(xmlElement, "MidSize", midSize);
            XmlHelper.SetValue(xmlElement, "BigSize", bigSize);
            XmlHelper.SetValue(xmlElement, "UseReject", useReject.ToString());
            XmlHelper.SetValue(xmlElement, "DefaultLightValue", defalutLightValue.ToString());
            XmlHelper.SetValue(xmlElement, "DefectPenSize", defectPenSize.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            skipLength = XmlHelper.GetValue(xmlElement, "SkipLength", skipLength);
            mergeCheckTop = XmlHelper.GetValue(xmlElement, "MergeCheckTop", mergeCheckTop);
            mergeCheckBottom = XmlHelper.GetValue(xmlElement, "MergeCheckBottom", mergeCheckBottom);
            XmlHelper.GetValue(xmlElement, "Cam1PelSize", ref cam1PelSize);
            XmlHelper.GetValue(xmlElement, "Cam2PelSize", ref cam2PelSize);
            totalRollLength = XmlHelper.GetValue(xmlElement, "TotalRollLength", totalRollLength);
            maxDefect = XmlHelper.GetValue(xmlElement, "MaxDefectCount", maxDefect);
            pixelResolution = XmlHelper.GetValue(xmlElement, "PixelResolution", pixelResolution);
            useReject = XmlHelper.GetValue(xmlElement, "UseReject", useReject);
            defalutLightValue= XmlHelper.GetValue(xmlElement, "DefaultLightValue", defalutLightValue);
            defectPenSize= XmlHelper.GetValue(xmlElement, "DefectPenSize", defectPenSize);

            XmlHelper.GetValue(xmlElement, "SmallSize", ref smallSize);
            XmlHelper.GetValue(xmlElement, "MidSize", ref midSize);
            XmlHelper.GetValue(xmlElement, "BigSize", ref bigSize);
        }
    }
}
