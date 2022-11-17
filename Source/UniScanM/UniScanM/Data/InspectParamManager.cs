using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public class InspectParamManager
    {
        InspectParam baseInspParam;
        private int maxIndex;

        List<InspectParam> inspectParamList = new List<InspectParam>();
        public List<InspectParam> InspectParamList { get => inspectParamList; }

        public InspectParamManager(InspectParam inspectParam, int maxIndex = 10)
        {
            baseInspParam = inspectParam;
            this.maxIndex = maxIndex;
            Load();
        }

        void Load()
        {
            string path = Path.Combine(PathSettings.Instance().Config, "InspectParam.xml");
            XmlDocument xmlDocument = XmlHelper.Load(path);
            for (int i = 0; i < maxIndex; i++)
            {
                inspectParamList.Add(baseInspParam);
            }

            if (xmlDocument == null)
            {
                Save();
                return;
            }
            XmlElement docElement = xmlDocument["InspectParamList"];
            XmlElement portListElement = xmlDocument.DocumentElement;
            int index = 0;
            foreach (XmlElement portElement in portListElement)
            {
                inspectParamList[index].Import(portElement);
                index++;
            }
        }
        void Save()
        {
            string path = Path.Combine(PathSettings.Instance().Config, "InspectParam.xml");
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement paramListElement = xmlDocument.CreateElement("", "InspectParamList", "");
            xmlDocument.AppendChild(paramListElement);

            for (int i = 0; i < maxIndex; i++)
            {
                InspectParam param = InspectParamList[i];
                XmlElement paramElement = xmlDocument.CreateElement("", string.Format("InspectParam{0}", i), "");
                param.Export(paramElement);
                paramListElement.AppendChild(paramElement);
            }

            xmlDocument.Save(path);
        }

        public InspectParam GetInspectParam(int index)
        {
            return InspectParamList[index];
        }

        public void SetInspectParam(int index, InspectParam insParam)
        {
            InspectParamList[index] = insParam;
        }


        public void Save2(string filePath)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement taskItemListElement = xmlDocument.CreateElement("", "TaskItemList", "");
            xmlDocument.AppendChild(taskItemListElement);

            //foreach (TaskItem taskItem in taskItemList)
            {
                XmlElement taskItemElement = xmlDocument.CreateElement("", "TaskItem", "");
                taskItemListElement.AppendChild(taskItemElement);

                //XmlHelper.SetValue(taskItemElement, "Code", taskItem.Code.ToString());
                //XmlHelper.SetValue(taskItemElement, "Name", taskItem.Name);
            }

            xmlDocument.Save(filePath);
        }

        //public void Load(string filePath)
        //{
        //    if (File.Exists(filePath) == false)
        //        return;

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.Load(filePath);

        //    XmlElement taskItemListElement = xmlDocument.DocumentElement;
        //    foreach (XmlElement taskItemElement in taskItemListElement)
        //    {
        //        if (taskItemElement.Name == "TaskItem")
        //        {
        //            int code = Convert.ToInt32(XmlHelper.GetValue(taskItemElement, "Code", "0"));
        //            string name = XmlHelper.GetValue(taskItemElement, "Name", "");

        //            taskItemList.Add(new TaskItem(code, name));
        //        }
        //    }
        //}
    }
}
