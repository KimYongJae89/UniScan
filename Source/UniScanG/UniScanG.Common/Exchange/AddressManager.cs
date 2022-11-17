using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanG.Common.Exchange
{
    public abstract class AddressManager
    {
        static AddressManager instance = null;
        
        public static AddressManager Instance()
        {
            return instance;
        }

        public static void SetInstance(AddressManager addressManager)
        {
            instance = addressManager;
        }

        public abstract string GetMonitorAddress();
        public virtual int GetMonitorLinteningPort() { return 6000; }
        public abstract string GetInspectorAddress(int camIndex, int clientIndex);
    }

    public class AddressManagerS : AddressManager
    {
        public override string GetMonitorAddress()
        {
            return string.Format("192.168.0.100");
        }

        public override string GetInspectorAddress(int camIndex, int clientIndex)
        {
            return string.Format("192.168.0.{0}", ((camIndex + 1) * 10) + clientIndex + 100);
        }
    }

    public class AddressManagerG : AddressManager
    {
        string[] monitorIpRange = new string[] { "192", "168", "50", "({0}+1)*10+{1}+1" };
        string[] inspectorIpRange = new string[] { "192", "168", "50", "({0}+1)*10+{1}" };
        public AddressManagerG()
        {
            string path = Path.Combine(PathSettings.Instance().Config, "AddressManager.xml");
            bool ok = Load();
            if (ok == false)
                Save();
        }

        public override string GetMonitorAddress()
        {
            //return string.Format("192.168.50.1");
            return GetAddress(monitorIpRange, -1, 0);
        }

        public override string GetInspectorAddress(int camIndex, int clientIndex)
        {
            //return string.Format("192.168.50.{0}", ((camIndex + 1) * 10) + Math.Max(0, clientIndex));
            return GetAddress(inspectorIpRange, camIndex, clientIndex);
        }

        public string GetAddress(string[] ipRange, int camIndex, int cliendIndex)
        {
            string[] ipAddress = new string[4];
            for (int i = 0; i < 4; i++)
                ipAddress[i] = new DataTable().Compute(string.Format(ipRange[i], camIndex, Math.Max(0, cliendIndex)), null).ToString();
            return string.Join(".", ipAddress);
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement("AddressManager");
            xmlDoc.AppendChild(xmlElement);

            XmlElement monitorIpRangeElement =xmlDoc.CreateElement("MonitorIpRange");
            xmlElement.AppendChild(monitorIpRangeElement);
            for (int i = 0; i < this.monitorIpRange.Length; i++)
                XmlHelper.SetValue(monitorIpRangeElement, string.Format("Range{0}", i), this.monitorIpRange[i]);

            XmlElement inspectorIpRangeElement = xmlDoc.CreateElement("InspectorIpRange");
            xmlElement.AppendChild(inspectorIpRangeElement);
            for (int i = 0; i < this.inspectorIpRange.Length; i++)
                XmlHelper.SetValue(inspectorIpRangeElement, string.Format("Range{0}", i), this.inspectorIpRange[i]);

            string path = Path.Combine(PathSettings.Instance().Config, "AddressManager.xml");
            xmlDoc.Save(path);
        }

        public bool Load()
        {
            try
            {
                string path = Path.Combine(PathSettings.Instance().Config, "AddressManager.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlElement xmlElement = xmlDoc["AddressManager"];
                if (xmlElement == null)
                    return false;

                XmlElement monitorIpRangeElement = xmlElement["MonitorIpRange"];
                for (int i = 0; i < this.monitorIpRange.Length; i++)
                    this.monitorIpRange[i] = XmlHelper.GetValue(monitorIpRangeElement, string.Format("Range{0}", i), this.monitorIpRange[i]);

                XmlElement inspectorIpRangeElement = xmlElement["InspectorIpRange"];
                for (int i = 0; i < this.inspectorIpRange.Length; i++)
                    this.inspectorIpRange[i] = XmlHelper.GetValue(inspectorIpRangeElement, string.Format("Range{0}", i), this.inspectorIpRange[i]);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
