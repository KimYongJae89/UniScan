using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanS.Common.Data
{
    public class InspectorInfo
    {
        string address = "";
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        string path = "";
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        int camIndex;
        public int CamIndex
        {
            get { return camIndex; }
            set { camIndex = value; }
        }

        RectangleF fov = new RectangleF();
        public RectangleF Fov
        {
            get { return fov; }
            set { fov = value; }
        }

        public void Load(XmlElement xmlElement)
        {
            address = XmlHelper.GetValue(xmlElement, "Address", "");
            path = XmlHelper.GetValue(xmlElement, "Path", "");
            camIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CamIndex", "0"));

            float fovX = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovX", "0"));
            float fovY = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovY", "0"));
            float fovW = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovWidth", "0"));
            float fovH = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovHeight", "0"));
            fov = new RectangleF(fovX, fovY, fovW, fovH);
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Address", address);
            XmlHelper.SetValue(xmlElement, "Path", path);
            XmlHelper.SetValue(xmlElement, "CamIndex", camIndex.ToString());

            XmlHelper.SetValue(xmlElement, "FovX", fov.X.ToString());
            XmlHelper.SetValue(xmlElement, "FovY", fov.Y.ToString());
            XmlHelper.SetValue(xmlElement, "FovWidth", fov.Width.ToString());
            XmlHelper.SetValue(xmlElement, "FovHeight", fov.Height.ToString());
        }
    }
}
