using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public abstract class DetectorParam
    {
        public abstract void Save(XmlElement xmlElement);
        public abstract void Load(XmlElement xmlElement);
    }
}
