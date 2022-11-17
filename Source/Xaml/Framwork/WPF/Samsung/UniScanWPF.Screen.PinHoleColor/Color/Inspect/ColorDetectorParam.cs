using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Color.Inspect
{
    public class ColorDetectorParam : DetectorParam, INotifyPropertyChanged
    {
        int lowerThreshold;
        int upperThreshold;

        public int LowerThreshold
        {
            get => lowerThreshold;
            set
            {
                lowerThreshold = value;
                OnPropertyChanged("LowerThreshold");
            }
        }

        public int UpperThreshold
        {
            get => upperThreshold;
            set
            {
                upperThreshold = value;
                OnPropertyChanged("UpperThreshold");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override void Load(XmlElement xmlElement)
        {
            lowerThreshold = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LowerThreshold", "20"));
            upperThreshold = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "UpperThreshold", "20"));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "LowerThreshold", lowerThreshold.ToString());
            XmlHelper.SetValue(xmlElement, "UpperThreshold", upperThreshold.ToString());
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
