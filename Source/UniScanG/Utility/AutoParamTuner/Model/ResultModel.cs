using AutoParamTuner.Base;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoParamTuner.Model
{
    internal class ResultModel : Base.Model
    {
        public Image2D ModelImage { get; private set; }
        public Dictionary<ParamName, Dictionary<object, TunerResult>> ResultDictionary { get; private set; }

        public ResultModel(Image2D image)
        {
            this.ModelImage = image;
            this.ResultDictionary = new Dictionary<ParamName, Dictionary<object, TunerResult>>();
        }

        public ResultModel(Image2D image, Dictionary<ISwapParamItem, TunerResult> resultDictionary):this(image)
        {
            var groupByName = resultDictionary.GroupBy(f => f.Key.Name);
            foreach(var a in groupByName)
            {
                if (!this.ResultDictionary.ContainsKey(a.Key))
                    this.ResultDictionary.Add(a.Key, new Dictionary<object, TunerResult>());

                foreach (var b in a)
                    this.ResultDictionary[a.Key].Add(b.Key.Value, b.Value);
            }
        }

        public override void Save(string xmlFile)
        {
            // Image
            string imageFileName = Path.ChangeExtension(xmlFile, ".bmp");
            ModelImage.SaveImage(imageFileName);

            // Dictionary
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement rootElement = (XmlElement)xmlDocument.AppendChild(xmlDocument.CreateElement("ROOT"));
            foreach (KeyValuePair<ParamName, Dictionary<object, TunerResult>> pair in ResultDictionary)
            {
                Type type = null;
                XmlElement xmlPairElement = XmlHelper.CreateAndAppendChild(rootElement, "Pair");
                XmlHelper.SetValue(xmlPairElement, "Key", pair.Key);

                foreach (KeyValuePair<object, TunerResult> pair2 in pair.Value)
                {
                    type = pair2.Key.GetType();

                    XmlElement xmlValueElement = XmlHelper.CreateAndAppendChild(xmlPairElement, "Value");
                    XmlHelper.SetValue(xmlValueElement, "Swap", pair2.Key.ToString());
                    pair2.Value.SaveXml(xmlValueElement, "Result");
                }
                XmlHelper.SetValue(xmlPairElement, "AssemblyName", type.Assembly.FullName);
                XmlHelper.SetValue(xmlPairElement, "TypeName", type.Name);
                XmlHelper.SetValue(xmlPairElement, "TypeFullName", type.FullName);
            }

            xmlDocument.Save(xmlFile);
        }

        public override void Load(string xmlFile)
        {
            // Image
            string imageFileName = Path.ChangeExtension(xmlFile, ".bmp");
            Image2D image2D = new Image2D(imageFileName);
            Set<Image2D>("ModelImage", image2D);

            // Dictionary
            Dictionary<ParamName, Dictionary<object, TunerResult>> dic = new Dictionary<ParamName, Dictionary<object, TunerResult>>();
            XmlDocument xmlDocument = XmlHelper.Load(xmlFile);
            XmlElement rootElement = xmlDocument["ROOT"];
            XmlNodeList xmlPairNodeList = rootElement.GetElementsByTagName("Pair");
            foreach(XmlElement xmlPairElement in xmlPairNodeList)
            {
                string key = XmlHelper.GetValue(xmlPairElement, "Key", "");
                Enum.TryParse(key, out ParamName name);
                if (!dic.ContainsKey(name))
                    dic.Add(name, new Dictionary<object, TunerResult>());

                string assemblyName = XmlHelper.GetValue(xmlPairElement, "AssemblyName", "");
                string typeName = XmlHelper.GetValue(xmlPairElement, "TypeName", "");
                string typeFullName = XmlHelper.GetValue(xmlPairElement, "TypeFullName", "");

                XmlNodeList xmlValueNodeList = xmlPairElement.GetElementsByTagName("Value");
                foreach (XmlElement xmlValueElement in xmlValueNodeList)
                {
                    TunerResult tunerResult = TunerResult.Load(xmlValueElement, "Result");

                    string swapValueStr = XmlHelper.GetValue(xmlValueElement, "Swap", "");
                    if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeFullName))
                    {
                        Type type = Type.GetType(typeFullName, false, false);
                        object swapValue = Convert.ChangeType(swapValueStr, type);
                        dic[name].Add(swapValue, tunerResult);
                    }
                    else
                    {
                        dic[name].Add(swapValueStr, tunerResult);
                    }
                }
            }
            Set("ResultDictionary", dic);
        }
    }
}
