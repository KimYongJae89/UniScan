using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class MultipleProduction : UniEye.Base.Data.Production
    {
        Production pinHoleProduction;
        Production colorProduction;
        string parentLotNo;

        public Production PinHoleProduction { get => pinHoleProduction; set => pinHoleProduction = value; }
        public Production ColorProduction { get => colorProduction; set => colorProduction = value; }

        string[] visibleStr;
        public string[] VisibleStr
        {
            get
            {
                return visibleStr;
            }
        }

        object[] brushes;
        public object[] Brushes
        {
            get
            {
                return brushes;
            }
        }

        public MultipleProduction(string parentLotNo, string name, DateTime dateTIme, string lotNo) : base(name, dateTIme, lotNo)
        {
            this.parentLotNo = parentLotNo;

            pinHoleProduction = new Production(name, dateTIme, lotNo);
            colorProduction = new Production(name, dateTIme, lotNo);

            brushes = new object[Name.Length];
            visibleStr = new string[Name.Length];

            for (int i = 0; i < Name.Length; i++)
            {
                if (Name[i] == '0')
                    brushes[i] = System.Windows.Media.Brushes.LightGray;//App.Current.Resources["RedBrush"];
                else
                {
                    visibleStr[i] = (i + 1).ToString();
                    brushes[i] = App.Current.Resources["GreenBrush"];
                }
            }
        }

        public MultipleProduction(XmlElement productionElement) : base(productionElement)
        {
            brushes = new object[Name.Length];
            visibleStr = new string[Name.Length];

            for (int i = 0; i < Name.Length; i++)
            {
                if (Name[i] == '0')
                    brushes[i] = System.Windows.Media.Brushes.LightGray;//App.Current.Resources["RedBrush"];
                else
                {
                    visibleStr[i] = (i + 1).ToString();
                    brushes[i] = App.Current.Resources["GreenBrush"];
                }
                   
            }
        }

        public override string GetResultPath()
        {
            return Path.Combine(PathSettings.Instance().Result, this.StartTime.ToString("yyyyMMdd"));
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            parentLotNo = XmlHelper.GetValue(productionElement, "Name", "");

            pinHoleProduction = new Production(Name, StartTime, LotNo);
            XmlElement pinHoleElement = productionElement["PinHole"];
            if (pinHoleElement != null)
                pinHoleProduction.Load(pinHoleElement);

            colorProduction = new Production(Name, StartTime, LotNo);
            XmlElement colorElement = productionElement["Color"];
            if (colorElement != null)
                colorProduction.Load(colorElement);
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            if (this.parentLotNo != null)
                XmlHelper.SetValue(productionElement, "ParentLotNo", parentLotNo);

            XmlElement pinHoleElement = productionElement.OwnerDocument.CreateElement("", "PinHole", "");
            productionElement.AppendChild(pinHoleElement);
            pinHoleProduction.Save(pinHoleElement);

            XmlElement colorElement = productionElement.OwnerDocument.CreateElement("", "Color", "");
            productionElement.AppendChild(colorElement);
            colorProduction.Save(colorElement);
        }

        public override void Reset()
        {
            base.Reset();
            colorProduction.Reset();
            pinHoleProduction.Reset();
        }
    }
}
