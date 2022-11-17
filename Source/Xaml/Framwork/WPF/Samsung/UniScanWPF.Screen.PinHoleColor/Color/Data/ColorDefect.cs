using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Color.Data
{
    public class ColorDefect : Defect
    {
        ColorDefectType type;
        float diffValue;
        
        public float DiffValue { get => diffValue; }
        public ColorDefectType Type { get => type; }
        public string Diff { get => string.Format("{0:0.00}", diffValue); }
        public string TypeName { get => type.ToString(); }

        public ColorDefect(int index, ColorDefectType type, Rectangle rectangle, BitmapSource image, float diffValue)
        {
            this.index = index;
            this.type = type;
            this.rectangle = rectangle;
            this.image = image;
            this.diffValue = diffValue;
        }

        public ColorDefect(string resultPath, int index, Rectangle rectangle, XmlElement defectElement)
        {
            this.resultPath = resultPath;
            this.index = index;
            this.rectangle = rectangle;

            this.type = (ColorDefectType)Enum.Parse(typeof(ColorDefectType), XmlHelper.GetValue(defectElement, "Type", type.ToString()));
            this.diffValue = Convert.ToSingle(XmlHelper.GetValue(defectElement, "DiffValue", "0"));
        }

        public override void ExportResult(string resultPath, XmlElement defectElement)
        {
            base.ExportResult(resultPath, defectElement);

            XmlHelper.SetValue(defectElement, "Type", type.ToString());
            XmlHelper.SetValue(defectElement, "DiffValue", diffValue.ToString());
        }
    }
}
