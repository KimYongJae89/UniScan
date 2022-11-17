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
    [TypeConverter(typeof(StripPropertyConverter))]
    public class StripProperty : LineProperty
    {
        [LocalizedCategoryAttributeChart("Line"),
    LocalizedDisplayNameAttributeChart("Offset"),
    LocalizedDescriptionAttributeChart("Offset")]
        public float Offset { get => offset; set => offset = value; }
        protected float offset = 0;

        public StripProperty(string name) : base(name)
        {
            this.color = System.Drawing.Color.Red;
            this.thickness = 3;
        }
    }

    public class StripPropertyCollection : PropertyCollection
    {
        public new StripProperty this[int index] { get => (StripProperty)this.InnerList[index]; }

        public override PropertyDescriptor BuildPropertyCollectionPropertyDescriptor(PropertyCollection propertyCollection, int index)
        {
            return new StripPropertyCollectionPropertyDescriptor(this, index);
        }
    }

    public class StripPropertyCollectionPropertyDescriptor : PropertyCollectionPropertyDescriptor
    {
        public StripPropertyCollectionPropertyDescriptor(StripPropertyCollection collection, int index)
            : base(collection, index)
        {

        }
    }

    public class StripPropertyConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is StripProperty)
            {
                StripProperty stripProperty = (StripProperty)value;
                return stripProperty.Name;
            }
            else if (destinationType == typeof(string) && value is StripPropertyCollection)
            {
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}