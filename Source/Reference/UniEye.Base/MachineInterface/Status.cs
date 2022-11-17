using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UmxService
{
    public class StatusItem
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        int sizeBit;
        public int SizeBit
        {
            get { return sizeBit; }
            set { sizeBit = value; }
        }

        bool isCharactor = false;
        public bool IsCharactor
        {
            get { return isCharactor; }
            set { isCharactor = value; }
        }

        string address;
        public string Address
        {
            get { return address; }
            set { this.address = value; }
        }

        string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public StatusItem()
        {
            this.name = "";
            this.sizeBit = 0;
            this.value = "";
            this.isCharactor = false;
        }

        public StatusItem(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public StatusItem(string name,string address,int sizeBit, string value, bool isCharactor= false)
        {
            this.name = name;
            this.address = address;
            this.sizeBit = sizeBit;
            this.value = value;
            this.isCharactor = isCharactor;
        }

        public ushort GetBitMask16()
        {
           ushort mask = 0x0000;
            for (int i = 0; i < this.sizeBit; i++)
            {
                mask = (ushort)((mask << 1) | 0x01);
            }
            return mask;
        }

        public void Save(XmlElement xmlElement)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
            xmlElement.AppendChild(subElement);
            XmlHelper.SetValue(subElement, "Name", name);
            XmlHelper.SetValue(subElement, "Address", address);
            XmlHelper.SetValue(subElement, "SizeBit", sizeBit.ToString());
            XmlHelper.SetValue(subElement, "Value", value);
            XmlHelper.SetValue(subElement, "IsCharactor", isCharactor.ToString());
        }

        public void Load(XmlElement xmlElement)
        {
            name = XmlHelper.GetValue(xmlElement, "Name", "");
            address = XmlHelper.GetValue(xmlElement, "Address", "");
            sizeBit = int.Parse(XmlHelper.GetValue(xmlElement, "SizeBit", "0"));
            value = XmlHelper.GetValue(xmlElement, "Value", "");
            isCharactor = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "IsCharactor", "false"));
        }
    }

    public class StatusCollection : List<StatusItem>
    {
        public void Save(XmlElement xmlElement)
        {
            foreach(StatusItem item in this)
            {
                item.Save(xmlElement);
            }
        }

        public void Load(XmlElement xmlElement)
        {
            bool clear = false;
            foreach (XmlElement subElement in xmlElement.ChildNodes)
            {
                if (subElement.Name == "Item")
                {
                    if (!clear)
                    {
                        this.Clear();
                        clear = true;
                    }

                    StatusItem item = new StatusItem();
                    item.Load(subElement);
                    this.Add(item);
                }
            }
        }
    }
}
