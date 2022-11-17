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
using System.ComponentModel;
using UniScanM.Data;

namespace UniScanM.EDMSW.Data
{
    public class MobileParam
    {
        public string model;
        public string lotno;
        public string worker;
        public double lineSpeed=45; // m/min
        public int maxDistance=5000;


        public void Export(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "model", model);
            XmlHelper.SetValue(element, "lotno", lotno);
            XmlHelper.SetValue(element, "worker", worker);
            XmlHelper.SetValue(element, "lineSpeed", lineSpeed.ToString());
            XmlHelper.SetValue(element, "maxDistance", maxDistance.ToString());
        }

        public void Import(XmlElement element, string subKey = null)
        {
            model =XmlHelper.GetValue(element, "model", "Default Model");
            lotno = XmlHelper.GetValue(element, "lotno", "");
            worker = XmlHelper.GetValue(element, "worker", "Default Worker");
            lineSpeed = Convert.ToSingle(XmlHelper.GetValue(element, "lineSpeed", "45.0"));
            maxDistance = Convert.ToInt32(XmlHelper.GetValue(element, "maxDistance", "5000"));
        }
    }
    
    public class EDMSParam : InspectParam
    {
        public EDMSParam() { }
        public EDMSParam(EDMSParam param)
        {
            if (param != null)
            {
                this.filmThreshold = param.filmThreshold;
                this.coatingThreshold = param.coatingThreshold;
                this.printingThreshold = param.printingThreshold;
            }
        }

        double filmThreshold = 10;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Film Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Film Threshold [D.N.]")]
        public double FilmThreshold
        {
            get { return filmThreshold; }
            set { filmThreshold = value; }
        }

        double coatingThreshold = 50;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Coating Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Coating Threshold [D.N.]")]
        public double CoatingThreshold
        {
            get { return coatingThreshold; }
            set { coatingThreshold = value; }
        }

        double printingThreshold = 10;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Printing Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Printing Threshold [D.N.]")]
        public double PrintingThreshold
        {
            get { return printingThreshold; }
            set { printingThreshold = value; }
        }
        
        public override void Export(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "FilmThreshold", filmThreshold.ToString());
            XmlHelper.SetValue(element, "CoatingThreshold", coatingThreshold.ToString());
            XmlHelper.SetValue(element, "PrintingThreshold", printingThreshold.ToString());
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            filmThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "FilmThreshold", "0"));
            coatingThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "CoatingThreshold", "0"));
            printingThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "PrintingThreshold", "0"));
        }
    }

    public class Model : UniScanM.Data.Model
    {
        public MobileParam Mobileparam = new MobileParam();
        public Model() : base()
        {
            inspectParam = new EDMSParam();
            for(int i=0; i< PresetArray.Length; i++)
            {
                PresetArray[i] = new EDMSParam();
                PresetLightParam[i] = new LightParamSet();
            }
        }

        public new EDMSParam InspectParam
        {
            get { return (EDMSParam)inspectParam; }
        }

        int selectedPresetNum = 0;
        public int SelectedPresetNum
        {
            get => selectedPresetNum;
            set
            {
                selectedPresetNum = value;
                inspectParam = PresetArray[selectedPresetNum];
                LightParamSet = PresetLightParam[selectedPresetNum];  //정석적으론 락걸어야되나...
            }
        }

        public EDMSParam[] PresetArray = new EDMSParam[5];
        public LightParamSet[] PresetLightParam = new LightParamSet[5];

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);
            XmlHelper.SetValue(xmlElement, "SelectedPresetNum", SelectedPresetNum.ToString());
            for (int i =0; i<PresetArray.Length; i++)
            {
                string keyname = string.Format("Preset{0}", i);
                XmlElement newElement = xmlElement.OwnerDocument.CreateElement(keyname);
                PresetArray[i].Export(newElement);
                //PresetLightParam[i].Save(newElement);
                //모델의 LightParamSet을 저장 한다.
                XmlElement lightParamSetElement = xmlElement.OwnerDocument.CreateElement("", "LightParamSet", "");
                PresetLightParam[i].Save(lightParamSetElement);
                newElement.AppendChild(lightParamSetElement);

                xmlElement.AppendChild(newElement);
            }
            Mobileparam.Export(xmlElement);
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);
            selectedPresetNum = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SelectedPresetNum", "0"));

            bool loaded = false;
            for (int i = 0; i < PresetArray.Length; i++)
            {
                loaded = false;
                string keyname = string.Format("Preset{0}", i);
                //XmlNodeList list0 = xmlElement.SelectNodes(keyname);
                //XmlNodeList list1 = xmlElement.GetElementsByTagName(keyname);
                //XmlNodeList list2 =  xmlElement.OwnerDocument.GetElementsByTagName(keyname);
                //XmlNode node0 = xmlElement.SelectSingleNode(keyname);

                foreach(XmlElement element in xmlElement)
                {
                    if (element.Name == keyname) //최초 업그레이드 배포시에 없을수도 있음..
                    {
                        PresetArray[i].Import(xmlElement[keyname]);
                        //PresetLightParam[i].Load(xmlElement[keyname]);
                        // 모델의 LightParamSet을 불러온다.
                        XmlElement lightParamSetElement = xmlElement["LightParamSet"];
                        if (lightParamSetElement != null)
                        {
                            PresetLightParam[i].Initialize(LightParamSet.NumLight, LightParamSet.NumLightType);
                            PresetLightParam[i].Load(lightParamSetElement);
                        }

                        loaded = true;
                    }
                }
                if(loaded ==false)//기존 버전에 데이터가 없으면?? 배포 최초의 경우 대비loaded
                {
                    PresetArray[i] = new EDMSParam((EDMSParam)inspectParam);
                    PresetLightParam[i] = LightParamSet.Clone();
                }
            }
            Mobileparam.Import(xmlElement);
            //
        }
    }
}
