using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Gravure.Vision.RCI;

namespace RCITest
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Options
    {
        public RCIOptions RCIOptions { get; set; }
        public RCIStandaloneOptions RCIStandaloneOptions { get; set; } = new RCIStandaloneOptions();
        public RCIGlobalOptions RCIGlobalOptions { get; set; } = new RCIGlobalOptions();

        public string PATH { get; set; } = @"D:\UniScan\Gravure_Inspector\VirtualImage\E21BNF01-NOR-WA01SX";

        public string SEARCH_PATTERN { get; set; } = "*.bmp";
        public bool SEARCH_INCLUDE_SUBFOLDER { get; set; } = true;

        public string PATH_DEBUG { get; set; } = @".\DEBUG";

        public string PATH_DEBUG_FULL => System.IO.Path.GetFullPath(System.IO.Path.Combine(this.PATH, this.PATH_DEBUG));

        public bool SaveDebugImage { get; set; } = true;

        public void Save(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElement = xmlDocument.CreateElement("ROOT");
            xmlDocument.AppendChild(xmlElement);

            this.RCIOptions.Save(xmlElement, "RCIOptions");
            this.RCIStandaloneOptions.Save(xmlElement, "RCIStandaloneOptions");
            this.RCIGlobalOptions.SaveParam(xmlElement, "RCIGlobalOptions");

            XmlHelper.SetValue(xmlElement, "PATH", this.PATH);
            XmlHelper.SetValue(xmlElement, "SEARCH_INCLUDE_SUBFOLDER", this.SEARCH_INCLUDE_SUBFOLDER);
            XmlHelper.SetValue(xmlElement, "SEARCH_PATTERN", this.SEARCH_PATTERN);

            XmlHelper.SetValue(xmlElement, "PATH_DEBUG", this.PATH_DEBUG);
            XmlHelper.SetValue(xmlElement, "SaveDebugImage", this.SaveDebugImage);

            XmlHelper.Save(xmlDocument, System.IO.Path.Combine(Environment.CurrentDirectory, "Options.xml"));
        }

        public void Load(string path)
        {
            XmlDocument xmlDocument = XmlHelper.Load("Options.xml");
            if (xmlDocument == null)
                return;

            XmlElement xmlElement = xmlDocument["ROOT"];

            this.RCIOptions.CopyFrom(RCIOptions.Load(xmlElement, "RCIOptions"));
            this.RCIStandaloneOptions.CopyFrom(RCIStandaloneOptions.Load(xmlElement, "RCIStandaloneOptions"));
            this.RCIGlobalOptions.LoadParam(xmlElement, "RCIGlobalOptions");

            this.PATH = XmlHelper.GetValue(xmlElement, "PATH", this.PATH);
            this.SEARCH_INCLUDE_SUBFOLDER = XmlHelper.GetValue(xmlElement, "SEARCH_INCLUDE_SUBFOLDER", this.SEARCH_INCLUDE_SUBFOLDER);
            this.SEARCH_PATTERN = XmlHelper.GetValue(xmlElement, "SEARCH_PATTERN", this.SEARCH_PATTERN);

            this.PATH_DEBUG = XmlHelper.GetValue(xmlElement, "PATH_DEBUG", this.PATH_DEBUG);
            this.SaveDebugImage = XmlHelper.GetValue(xmlElement, "SaveDebugImage", this.SaveDebugImage);
        }
    }
}
