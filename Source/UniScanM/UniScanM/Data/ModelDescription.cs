using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;

namespace UniScanM.Data
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        private string paste;
        public string Paste
        {
            get { return paste; }
            set { paste = value; }
        }

        LightParamSet lightParamSet = new LightParamSet();
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        public ModelDescription() : base()
        {
            if (SystemManager.Instance().CurrentModel == null)
                lightParamSet = new LightParamSet();
            else
                lightParamSet = SystemManager.Instance().CurrentModel?.LightParamSet;
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);
        
            paste = XmlHelper.GetValue(modelDescElement, "Paste", "");
            
            XmlElement lightParamSetElement = modelDescElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                List<LightParam> lightParamList = new List<LightParam>();
                lightParamList.Add(new LightParam((int)1));

                lightParamSet.LightParamList = lightParamList;

                LightParamSetLoad(lightParamSetElement);
            }
        }

        private void LightParamSetLoad(XmlElement lightParamSetElement)
        {
            XmlNodeList list = lightParamSetElement.GetElementsByTagName("LightParam");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement lightParamElement = list[i] as XmlElement;
                LightParam lightParam = new LightParam(2);
                lightParam.Load(lightParamElement);

                if (String.IsNullOrEmpty(lightParam.Name))
                {
                    lightParam.Name = String.Format("LightType {0}", i);
                }
                lightParamSet.LightParamList[i] = lightParam;
            }
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);
            XmlHelper.SetValue(modelDescElement, "Paste", paste);

            XmlElement lightParamSetElement = modelDescElement.OwnerDocument.CreateElement("", "LightParamSet", "");
            modelDescElement.AppendChild(lightParamSetElement);
            lightParamSet.Save(lightParamSetElement);
        }


        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();

            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
            ModelDescription md = (ModelDescription)srcDesc;
            paste = md.paste;
        }
    }
}
