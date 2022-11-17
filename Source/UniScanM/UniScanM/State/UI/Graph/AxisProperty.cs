using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.UI.Graph
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AxisProperty : LineProperty
    {
        public bool Invert { get => invert; set => invert = value; }
        bool invert = false;

        public bool AutoScale { get => autoScale; set => autoScale = value; }
        bool autoScale = true;
        
        public float Length { get => length; set => length = value; }
        float length = 10;

        public float Interval { get => interval; set => interval = value; }
        float interval = 0f;

        public int Decimals { get => decimals; set => decimals = value; }
        int decimals = 2;

        public AxisProperty(string name) : base(name) { }
    }

    //public class AxisPropertyCollection : PropertyCollection
    //{
    //    public new AxisProperty this[int index] { get => (AxisProperty)this.InnerList[index]; }
    //    public new AxisProperty this[string idx] { get => (AxisProperty)this.InnerList.[index]; }

    //    public override PropertyDescriptor BuildPropertyCollectionPropertyDescriptor(PropertyCollection propertyCollection, int index)
    //    {
    //        return new AxisPropertyCollectionPropertyDescriptor(this, index);
    //    }
    //}

    //public class AxisPropertyCollectionPropertyDescriptor : PropertyCollectionPropertyDescriptor
    //{
    //    public AxisPropertyCollectionPropertyDescriptor(AxisPropertyCollection collection, int index)
    //    : base(collection, index)
    //    {

    //    }
    //}

    public class AxisPropertyConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is AxisProperty)
            {
                AxisProperty axisProperty = (AxisProperty)value;
                return axisProperty.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
