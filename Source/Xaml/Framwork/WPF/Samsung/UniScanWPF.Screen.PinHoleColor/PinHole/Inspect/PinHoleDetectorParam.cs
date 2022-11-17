using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    public class PinHoleDetectorParam : DetectorParam, INotifyPropertyChanged
    {
        int lowerThreshold;
        int upperThreshold;
        int edgeThreshold;
        int skipLength;
        SearchDireciton searchDireciton;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public int EdgeThreshold
        {
            get => edgeThreshold;
            set
            {
                edgeThreshold = value;
                OnPropertyChanged("EdgeThreshold");
            }
        }

        public int SkipLength
        {
            get => skipLength;
            set
            {
                skipLength = value;
                OnPropertyChanged("SkipLength");
            }
        }

        public SearchDireciton SearchDireciton
        {
            get => searchDireciton;
            set
            {
                searchDireciton = value;
                OnPropertyChanged("SearchDireciton");
            }
        }

        public PinHoleDetectorParam() : base()
        {

        }

        public override void Load(XmlElement xmlElement)
        {
            lowerThreshold = System.Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LowerThreshold", "20"));
            upperThreshold = System.Convert.ToInt32(XmlHelper.GetValue(xmlElement, "UpperThreshold", "20"));
            edgeThreshold = System.Convert.ToInt32(XmlHelper.GetValue(xmlElement, "EdgeThreshold", "10"));
            skipLength = System.Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SkipLength", "1000"));
            searchDireciton = (SearchDireciton)Enum.Parse(typeof(SearchDireciton), XmlHelper.GetValue(xmlElement, "SearchDireciton", SearchDireciton.LeftToRight.ToString()));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "LowerThreshold", lowerThreshold.ToString());
            XmlHelper.SetValue(xmlElement, "UpperThreshold", upperThreshold.ToString());
            XmlHelper.SetValue(xmlElement, "EdgeThreshold", edgeThreshold.ToString());
            XmlHelper.SetValue(xmlElement, "SkipLength", skipLength.ToString());
            XmlHelper.SetValue(xmlElement, "SearchDireciton", searchDireciton.ToString());
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