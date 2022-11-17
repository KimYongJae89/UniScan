using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.UI.Graph
{
    public enum ChartType { Point = SeriesChartType.Point, Line = SeriesChartType.Line, Column = SeriesChartType.Column }

    [TypeConverter(typeof(SeriesPropertyConverter))]
    public class SeriesProperty : LineProperty
    {
        [LocalizedCategoryAttributeChart("Series"),
LocalizedDisplayNameAttributeChart("Type"),
LocalizedDescriptionAttributeChart("Type")]
        public ChartType ChartType { get => chartType; set => chartType = value; }
        ChartType chartType = ChartType.Line;

        public SeriesProperty(string name) : base(name) { }
    }

    public class SeriesPropertyCollection : PropertyCollection
    {
        public new SeriesProperty this[int index] { get => (SeriesProperty)this.InnerList[index]; }

        public override PropertyDescriptor BuildPropertyCollectionPropertyDescriptor(PropertyCollection propertyCollection, int index)
        {
            return new SeriesPropertyCollectionPropertyDescriptor(this, index);
        }
    }

    public class SeriesPropertyCollectionPropertyDescriptor : PropertyCollectionPropertyDescriptor
    {
        public SeriesPropertyCollectionPropertyDescriptor(SeriesPropertyCollection collection, int index)
            : base(collection, index)
        {

        }
    }

    public class SeriesPropertyConverter: ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SeriesProperty)
            {
                SeriesProperty seriesProperty = (SeriesProperty)value;
                return seriesProperty.Name;
            }
            else if (destinationType == typeof(string) && value is SeriesPropertyCollection)
            {
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
