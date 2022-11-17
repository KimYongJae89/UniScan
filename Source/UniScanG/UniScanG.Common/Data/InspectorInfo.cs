using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Common.Exchange;

namespace UniScanG.Common.Data
{
    public class InspectorInfo
    {
        public bool Use { get => this.use; set => this.use = value; }
        bool use;

        public string IpAddress { get => this.ipAddress; set => this.ipAddress = value; }
        string ipAddress;
        
        public string MacAddress { get => this.macAddress; set => this.macAddress = value; }
        string macAddress;

        public string Path { get => this.path; set => this.path = value; }
        string path;

        public int CamIndex { get => this.camIndex; set => this.camIndex = value; }
        int camIndex;

        public int ClientIndex { get => this.clientIndex; set => this.clientIndex = value; }
        int clientIndex;

        public string UserId { get => this.userId; set => this.userId = value; }
        string userId;

        public string UserPw { get => this.userPw; set => this.userPw = value; }
        string userPw;

        public RectangleF Fov { get => this.fov; set => this.fov = value; }
        RectangleF fov = new RectangleF();

        public InspectorInfo()
        {
            this.use = true;
            this.ipAddress = "";
            this.macAddress = "";
            this.path = "";
            this.camIndex = -1;
            this.clientIndex = -1;
            this.userId = "Administrator";
            this.userPw = "";
        }

        public void Load(XmlElement xmlElement)
        {
            this.use = XmlHelper.GetValue(xmlElement, "Use", this.use);
            this.ipAddress = XmlHelper.GetValue(xmlElement, "IpAddress", this.ipAddress);
            this.macAddress = XmlHelper.GetValue(xmlElement, "MacAddress", this.macAddress);
            this.path = XmlHelper.GetValue(xmlElement, "Path", this.path);
            this.camIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CamIndex", this.camIndex));
            this.clientIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ClientIndex", this.clientIndex));

            this.userId = XmlHelper.GetValue(xmlElement, "UserId", this.userId);
            this.userPw = XmlHelper.GetValue(xmlElement, "UserPw", this.userPw);

            this.fov = XmlHelper.GetValue(xmlElement, "Fov", this.fov);

            if (this.fov.IsEmpty)
            {
                float fovX = XmlHelper.GetValue(xmlElement, "FovX", 0);
                float fovY = XmlHelper.GetValue(xmlElement, "FovY", 0);
                float fovW = XmlHelper.GetValue(xmlElement, "FovWidth", 0);
                float fovH = XmlHelper.GetValue(xmlElement, "FovHeight", 0);
                this.fov = new RectangleF(fovX, fovY, fovW, fovH);
            }

            if (string.IsNullOrEmpty(ipAddress))
                this.ipAddress = AddressManager.Instance().GetInspectorAddress(this.camIndex, this.clientIndex);
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Use", this.use);
            XmlHelper.SetValue(xmlElement, "IpAddress", this.ipAddress);
            XmlHelper.SetValue(xmlElement, "MacAddress", this.macAddress);
            XmlHelper.SetValue(xmlElement, "Path", this.path);
            XmlHelper.SetValue(xmlElement, "CamIndex", this.camIndex);
            XmlHelper.SetValue(xmlElement, "ClientIndex", this.clientIndex);

            XmlHelper.SetValue(xmlElement, "UserId", this.userId);
            XmlHelper.SetValue(xmlElement, "UserPw", this.userPw);

            XmlHelper.SetValue(xmlElement, "Fov", this.fov);
        }

        public string GetName()
        {
            int camId = camIndex + 1;
            if (clientIndex >= 0)
            {
                char clientId = (char)(clientIndex + 0x41);
                if (char.IsLetter(clientId) && clientId != 'A')
                    return string.Format("IM{0}{1}", camId, clientId);
            }

            return string.Format("IM{0}", camId);
        }
    }
}
