using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.RCI
{

    public class Px2MmConverter : TypeConverter
    {
        public static decimal PelSize { get; set; } = (decimal)14.0;

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return new decimal((float)value) * PelSize / 1000;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new decimal((float)value) / 1000 * PelSize;
        }
    }

}
