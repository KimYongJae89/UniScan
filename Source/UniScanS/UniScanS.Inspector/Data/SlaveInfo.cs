using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanS.Inspector.Data
{
    public class SlaveInfo
    {
        string path = "";
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        int clientIndex;
        public int ClientIndex
        {
            get { return clientIndex; }
            set { clientIndex = value; }
        }
        
        public void Load(XmlElement xmlElement)
        {
            path = XmlHelper.GetValue(xmlElement, "Path", "");
            clientIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ClientIndex", "0"));
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Path", path);
            XmlHelper.SetValue(xmlElement, "ClientIndex", clientIndex.ToString());
        }
    }
}
