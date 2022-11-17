using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Xml;
using System.Diagnostics;

using System.ComponentModel;
using System.Globalization;

namespace UniScanX.MPAlignment.Data
{
        [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
        public class MinMaxPair<T> where T : struct
        {
            [DisplayName("Min"), DescriptionAttribute("Min Value")]
            public T Min { get => this.min; set => this.min = value; }
            T min;

            [DisplayName("Max"), DescriptionAttribute("Max Value")]
            public T Max { get => this.max; set => this.max = value; }
            T max;

            public static MinMaxPair<T> Empty => new MinMaxPair<T>();

            public override string ToString()
            {
                return string.Format("{0}, {1}", this.min, this.max);
            }

            public MinMaxPair()
            {

            }

            public MinMaxPair(T min, T max)
            {
                this.min = min;
                this.max = max;
            }

            public MinMaxPair<T> Clone()
            {
                return new MinMaxPair<T>(this.min, this.min);
            }

            public void Save(XmlElement xmlElement, string key)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    XmlElement subXmlElement = xmlElement.OwnerDocument.CreateElement(key);
                    xmlElement.AppendChild(subXmlElement);
                    Save(subXmlElement, null);
                    return;
                }

                XmlHelper.SetValue(xmlElement, "Min", this.min.ToString());
                XmlHelper.SetValue(xmlElement, "Max", this.max.ToString());
            }

            public void Load(XmlElement xmlElement, string key)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    XmlElement subXmlElement = xmlElement[key];
                    Load(subXmlElement, null);
                    return;
                }

                if (xmlElement == null)
                    return;

                string min = XmlHelper.GetValue(xmlElement, "Min", "0");
                this.min = (T)Convert.ChangeType(min, typeof(T));

                string max = XmlHelper.GetValue(xmlElement, "Max", "0");
                this.max = (T)Convert.ChangeType(max, typeof(T));

                //this.min = XmlHelper.GetValue(xmlElement, "Min", this.min);
                //this.max = XmlHelper.GetValue(xmlElement, "Max", this.max);
            }

        }

        public struct DefectOnScreen
        {
            public Rectangle Rectangle { get; }

            public int Area { get; }

            public Point[] Points { get; }

            public Bitmap SrcBitmap { get; }
            public Bitmap ResultBitmap { get; }

            public DefectOnScreen(Rectangle rectangle, Bitmap srcBitmap, Bitmap resultBitmap, int area, Point[] points)
            {
                Rectangle = rectangle;
                SrcBitmap = srcBitmap;
                ResultBitmap = resultBitmap;
                Area = area;
                Points = points;
            }
            public DefectOnScreen(Rectangle rectangle, Bitmap srcBitmap, Bitmap resultBitmap, int area, float[] pointXs, float[] pointYs)
            {
                Rectangle = rectangle;
                SrcBitmap = srcBitmap;
                ResultBitmap = resultBitmap;
                Area = area;

                int length = Math.Min(pointXs.Length, pointYs.Length);
                Points = new Point[length];
                for (int i = 0; i < length; i++)
                    Points[i] = Point.Round(new PointF(pointXs[i], pointYs[i]));
            }
        }
    
}
