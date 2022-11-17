using DynMvp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.UI.Graph
{
    internal class LocalizedCategoryAttributeChart : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeChart(string value) : base("LocalizedCategoryAttributeChart", value) { }
    }

    internal class LocalizedDisplayNameAttributeChart : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeChart(string value) : base("LocalizedDisplayNameAttributeChart", value) { }
    }

    internal class LocalizedDescriptionAttributeChart : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeChart(string value) : base("LocalizedDescriptionAttributeChart", value) { }
    }



    public abstract class LineProperty
    {
        [LocalizedCategoryAttributeChart("Line"),
            LocalizedDisplayNameAttributeChart("Name"),
            LocalizedDescriptionAttributeChart("Name")]
        public string Name { get => name; }
        protected string name = "";

        [LocalizedCategoryAttributeChart("Line"),
            LocalizedDisplayNameAttributeChart("Visiblity"),
            LocalizedDescriptionAttributeChart("Visiblity")]
        public bool Visiblity { get => visiblity; set => visiblity = value; }
        protected bool visiblity = true;

        [LocalizedCategoryAttributeChart("Line"),
            LocalizedDisplayNameAttributeChart("Color"),
            LocalizedDescriptionAttributeChart("Color")]
        public Color Color { get => color; set => color = value; }
        protected Color color = Color.Black;

        [LocalizedCategoryAttributeChart("Line"),
            LocalizedDisplayNameAttributeChart("Thickness"),
            LocalizedDescriptionAttributeChart("Thickness")]
        public double Thickness { get => thickness; set => thickness = value; }
        protected double thickness = 1;

        public LineProperty(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }


    public class ProfileOption
    {
        [LocalizedCategoryAttributeChart("Chart"),
        LocalizedDisplayNameAttributeChart("AxisX"),
        LocalizedDescriptionAttributeChart("AxisX")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AxisProperty AxisX { get => axisX; set => axisX = value; }
        AxisProperty axisX = null;


        [LocalizedCategoryAttributeChart("Chart"),
        LocalizedDisplayNameAttributeChart("AxisY"),
        LocalizedDescriptionAttributeChart("AxisY")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AxisProperty AxisY { get => axisY; set => axisY = value; }
        AxisProperty axisY = null;

        [LocalizedCategoryAttributeChart("Chart"),
        LocalizedDisplayNameAttributeChart("Background Color"),
        LocalizedDescriptionAttributeChart("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }
        private Color backColor;

        [LocalizedCategoryAttributeChart("Chart"),
        LocalizedDisplayNameAttributeChart("Strip Lines"),
        LocalizedDescriptionAttributeChart("Strip Lines")]
        [TypeConverter(typeof(StripPropertyConverter))]
        public StripPropertyCollection StripPropertyCollection { get => stripPropertyCollection; set => stripPropertyCollection = value; }
        private StripPropertyCollection stripPropertyCollection = new StripPropertyCollection();

        [LocalizedCategoryAttributeChart("Chart"),
            LocalizedDisplayNameAttributeChart("Series"),
            LocalizedDescriptionAttributeChart("Series")]
        [TypeConverter(typeof(SeriesPropertyConverter))]
        public SeriesPropertyCollection SeriesPropertyCollection { get => seriesPropertyCollection; set => seriesPropertyCollection = value; }
        private SeriesPropertyCollection seriesPropertyCollection = new SeriesPropertyCollection();

        public ProfileOption(int seriesCount)
        {
            this.axisX = new AxisProperty("X");
            this.axisY = new AxisProperty("Y");

            for (int i = 0; i < seriesCount; i++)
                this.seriesPropertyCollection.Add(new SeriesProperty(string.Format("Data{0}", i)));
        }

        public ProfileOption(int stripCount, int seriesCount)
        {
            this.axisX = new AxisProperty("X");
            this.axisY = new AxisProperty("Y");

            for (int i = 0; i < stripCount; i++)
                this.stripPropertyCollection.Add(new StripProperty(string.Format("Strip{0}", i)));

            for (int i = 0; i < seriesCount; i++)
                this.seriesPropertyCollection.Add(new SeriesProperty(string.Format("Data{0}", i)));
        }
    }
}