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
    public class UISettings
    {
        float fovWidth = 400;
        float cameraOffset = 200;
        float sheetLength = 5000;
        float sheetLeft = 10;
        float sheetWidth = 300;

        int leftMargin = 5;
        int rightMargin = 5;
        int topMargin = 5;
        int bottomMargin = 5 ;

        int numColumn = 5; // To Setting
        int numRow = 10; // To Setting

        int markerSize = 5;

        public float FovWidth { get => fovWidth; set => fovWidth = value; }
        public float CameraOffset { get => cameraOffset; set => cameraOffset = value; }
        public float SheetLength { get => sheetLength; set => sheetLength = value; }
        public float SheetLeft { get => sheetLeft; set => sheetLeft = value; }
        public float SheetWidth { get => sheetWidth; set => sheetWidth = value; }
        public int LeftMargin { get => leftMargin; set => leftMargin = value; }
        public int RightMargin { get => rightMargin; set => rightMargin = value; }
        public int TopMargin { get => topMargin; set => topMargin = value; }
        public int BottomMargin { get => bottomMargin; set => bottomMargin = value; }
        public int NumColumn { get => numColumn; set => numColumn = value; }
        public int NumRow { get => numRow; set => numRow = value; }
        public int MarkerSize { get => markerSize; set => markerSize = value; }

        static UISettings _instance;
        public static UISettings Instance()
        {
            if (_instance == null)
            {
                _instance = new UISettings();
            }

            return _instance;
        }

        protected UISettings()
        {
           
        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\UISettings.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement xmlElement = xmlDocument.CreateElement("", "UI", "");
            xmlDocument.AppendChild(xmlElement);

            XmlHelper.SetValue(xmlElement, "FovWidth", fovWidth.ToString());
            XmlHelper.SetValue(xmlElement, "CameraOffset", cameraOffset.ToString());
            XmlHelper.SetValue(xmlElement, "SheetLength", sheetLength.ToString());
            XmlHelper.SetValue(xmlElement, "SheetLeft", sheetLeft.ToString());
            XmlHelper.SetValue(xmlElement, "SheetWidth", sheetWidth.ToString());
            XmlHelper.SetValue(xmlElement, "LeftMargin", leftMargin.ToString());
            XmlHelper.SetValue(xmlElement, "RightMargin", rightMargin.ToString());
            XmlHelper.SetValue(xmlElement, "TopMargin", topMargin.ToString());
            XmlHelper.SetValue(xmlElement, "BottomMargin", bottomMargin.ToString());

            XmlHelper.SetValue(xmlElement, "NumColumn", numColumn.ToString());
            XmlHelper.SetValue(xmlElement, "NumRow", numRow.ToString());
            XmlHelper.SetValue(xmlElement, "MarkerSize", markerSize.ToString());


            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\UISettings.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement xmlElement = xmlDocument["UISettings"];
            if (xmlElement == null)
                return;

            fovWidth = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "FovWidth", "2048"));
            cameraOffset = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CameraOffset", "30"));
            sheetLength = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SheetLength", "5000"));
            sheetLeft = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SheetLeft", "5"));
            sheetWidth = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SheetWidth", "300"));
            leftMargin = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LeftMargin", "5"));
            rightMargin = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RightMargin", "5"));
            topMargin = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TopMargin", "5"));
            bottomMargin = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BottomMargin", "5"));

            numColumn = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "NumColumn", "5"));
            numRow = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "NumRow", "10"));
            markerSize = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "MarkerSize", "5"));
        }
    }
}
