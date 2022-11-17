using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace UniScanWPF.Table.Data
{
    public interface IResultObject
    {
        Enum ResultObjectType { get; }
        SolidColorBrush GetBrush();
        //Rect GetRect(double scale);
        System.Windows.Point[] GetPoints(double scale);
        void Save(XmlElement xmlElement);
        BitmapSource GetBitmapSource();
    }
}
