using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Base
{
    public class StringTable
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Count
        {
            get { return stringTable.Count; }
        }

        private Dictionary<string, string> stringTable = new Dictionary<string, string>();

        public StringTable()
        {
        }

        public StringTable(string name)
        {
            this.Name = name;
        }

        public Dictionary<string, string>.Enumerator GetEnumerator()
        {
            return stringTable.GetEnumerator();
        }

        public void Load(string fileName)
        {
            if (System.IO.File.Exists(fileName) == false)
                return;

            stringTable = new Dictionary<string, string>();

            if (Path.GetExtension(fileName) == ".xml")
                LoadXml(fileName);
            else if (Path.GetExtension(fileName) == ".st")
                LoadSt(fileName);
        }

        public void LoadXml(string fileName)
        { 
            XmlDocument xmldocument = new XmlDocument();
            xmldocument.Load(fileName);

            XmlElement rootElement = xmldocument.DocumentElement;

            foreach (XmlElement stringElment in rootElement)
            {
                if (stringElment.Name == "String")
                {
                    string key = XmlHelper.GetValue(stringElment, "Key", "");
                    string value = XmlHelper.GetValue(stringElment, "Value", "");
                    stringTable.Add(key, value);
                }
            }
        }

        public void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("String");
            foreach (XmlElement stringElment in xmlNodeList)
            {
                string key = XmlHelper.GetValue(stringElment, "Key", "").Replace("\r\n", "").Trim();
                string value = XmlHelper.GetValue(stringElment, "Value", "").Replace("\r\n", "").Trim();
                if (stringTable.ContainsKey(key) == false && string.IsNullOrEmpty(value) == false)
                    stringTable.Add(key, value);
            }
        }

        private void LoadSt(string fileName)
        {
            string[] stringLines = File.ReadAllLines(fileName);

            foreach (string line in stringLines)
            {
                string[] tokens = line.Split('=');

                if (tokens.Count() == 2)
                {
                    if (String.IsNullOrEmpty(tokens[0]) == false && String.IsNullOrEmpty(tokens[1]) == false)
                        stringTable.Add(tokens[0], tokens[1]);
                }
            }
        }

        public void Save(string fileName)
        {
            if (Path.GetExtension(fileName) == ".xml")
                SaveXml(fileName);
            else if (Path.GetExtension(fileName) == ".st")
                SaveSt(fileName);
        }

        public void SaveSt(string fileName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                stringBuilder.AppendLine(String.Format("{0}={1}", stringPair.Key, stringPair.Value));
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public void SaveXml(string fileName)
        {
            XmlDocument xmldocument = new XmlDocument();
            XmlElement rootElement = xmldocument.CreateElement("", "StringTable", "");
            xmldocument.AppendChild(rootElement);

            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                XmlElement stringElement = rootElement.OwnerDocument.CreateElement("", "String", "");
                rootElement.AppendChild(stringElement);

                if (string.IsNullOrEmpty(stringPair.Value) == false)
                {
                    XmlHelper.SetValue(stringElement, "Key", stringPair.Key);
                    XmlHelper.SetValue(stringElement, "Value", stringPair.Value);
                }
            }

            xmldocument.Save(fileName);
        }

        public void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                if (string.IsNullOrEmpty(stringPair.Key))
                    return;

                XmlElement stringElement = xmlElement.OwnerDocument.CreateElement("", "String", "");
                xmlElement.AppendChild(stringElement);
                               
                XmlHelper.SetValue(stringElement, "Key", stringPair.Key);

                if (string.IsNullOrEmpty(stringPair.Value) == false)
                    XmlHelper.SetValue(stringElement, "Value", stringPair.Value);

            }
        }

        public bool IsExist(string key)
        {
            return stringTable.ContainsKey(key);
        }

        public void AddString(string key, string value)
        {
            stringTable.Add(key, value);
        }

        public void Merge(StringTable stringTable)
        {
            this.stringTable.Concat(stringTable.stringTable);
        }

        public string GetString(string searchKey)
        {
            if(stringTable.ContainsKey(searchKey))
            {
                return stringTable[searchKey];
            }
            return null;
        }

        public bool SetString(string searchKey, string value)
        {
            string tempValue;
            if (stringTable.TryGetValue(searchKey, out tempValue) == true)
            {
                stringTable[searchKey] = value;
                return true;
            }
            return false;
        }

        public bool DeleteString(string key)
        {
            return stringTable.Remove(key);
        }
    }
}
