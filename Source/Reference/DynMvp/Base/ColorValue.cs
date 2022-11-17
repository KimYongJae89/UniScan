using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// Dummy
namespace DynMvp.Base
{
    public enum ColorSpace
    {
        RGB, HSI
    }

    public struct ColorValue
    {
        ColorSpace colorSpace;
        public ColorSpace ColorSpace
        {
            get { return colorSpace; }
            set { colorSpace = value; }
        }

        float value1;
        public float Value1
        {
            get { return value1; }
            set { value1 = value; }
        }

        float value2;
        public float Value2
        {
            get { return value2; }
            set { value2 = value; }
        }

        float value3;
        public float Value3
        {
            get { return value3; }
            set { value3 = value; }
        }

        public ColorValue(ColorSpace colorSpace)
        {
            this.colorSpace = colorSpace;
            value1 = 0;
            value2 = 0;
            value3 = 0;
        }

        public ColorValue(float value1, float value2, float value3, ColorSpace colorSpace = ColorSpace.RGB)
        {
            this.colorSpace = colorSpace;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        public ColorValue(Color color)
        {
            colorSpace = ColorSpace.RGB;
            value1 = color.R;
            value2 = color.G;
            value3 = color.B;
        }

        public ColorValue GetColor(ColorSpace colorSpace)
        {
            if (this.ColorSpace == ColorSpace.RGB && colorSpace == ColorSpace.HSI)
                return RgbToHsi();
            else if (this.ColorSpace == ColorSpace.HSI && colorSpace == ColorSpace.RGB)
                return HsiToRgb();

            return this;
        }

        public ColorValue HsiToRgb()
        {
            ColorValue rgbColor = new ColorValue(ColorSpace.RGB);

            float hue = value1;
            float saturation = value2;
            float luminance = value3;

            float red, green, blue;
            if (saturation == 0)
            {
                red = green = blue = luminance * 255;
            }
            else
            {
                double t1, t2;
                double th = hue / 6.0d;

                if (luminance < 0.5d)
                {
                    t2 = luminance * (1d + saturation);
                }
                else
                {
                    t2 = (luminance + saturation) - (luminance * saturation);
                }
                t1 = 2d * luminance - t2;

                double tr, tg, tb;
                tr = th + (1.0d / 3.0d);
                tg = th;
                tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);

                red = (float)(tr * 255f);
                green = (float)(tg * 255f);
                blue = (float)(tb * 255f);
            }

            rgbColor.value1 = red;
            rgbColor.value2 = green;
            rgbColor.value3 = blue;

            return rgbColor;
        }

        private static double ColorCalc(double c, double t1, double t2)
        {
            if (c < 0)
                c += 1d;
            if (c > 1)
                c -= 1d;
            if (6.0d * c < 1.0d)
                return t1 + (t2 - t1) * 6.0d * c;
            if (2.0d * c < 1.0d)
                return t2;
            if (3.0d * c < 2.0d)
                return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            return t1;
        }

        public ColorValue RgbToHsi()
        {
            ColorValue hlsColor = new ColorValue(ColorSpace.HSI);

            float red = (value1 / 255f);
            float green = (value2 / 255f);
            float blue = (value3 / 255f);

            float min = Math.Min(Math.Min(red, green), blue);
            float max = Math.Max(Math.Max(red, green), blue);
            float delta = max - min;

            float hue = 0;
            float saturation = 0;
            float luminance = (float)((max + min) / 2.0f);

            if (delta != 0)
            {
                if (luminance < 0.5f)
                {
                    saturation = (float)(delta / (max + min));
                }
                else
                {
                    saturation = (float)(delta / (2.0f - max - min));
                }

                float deltaRed = (float)(((max - red) / 6.0f + (delta / 2.0f)) / delta);
                float deltaGreen = (float)(((max - green) / 6.0f + (delta / 2.0f)) / delta);
                float deltaBlue = (float)(((max - blue) / 6.0f + (delta / 2.0f)) / delta);

                if (red == max)
                {
                    hue = deltaBlue - deltaGreen;
                }
                else if (green == max)
                {
                    hue = (1.0f / 3.0f) + deltaRed - deltaBlue;
                }
                else if (blue == max)
                {
                    hue = (2.0f / 3.0f) + deltaGreen - deltaRed;
                }

                if (hue < 0)
                    hue += 1.0f;
                if (hue > 1)
                    hue -= 1.0f;
            }

            hlsColor.value1 = hue;
            hlsColor.value2 = saturation;
            hlsColor.value3 = luminance;

            return hlsColor;
        }

        public Color ToColor()
        {
            ColorValue rgbColor = GetColor(ColorSpace.RGB);
            return Color.FromArgb((int)rgbColor.value1, (int)rgbColor.value2, (int)rgbColor.value3);
        }

        public void Load(XmlElement colorElement)
        {
            colorSpace = (ColorSpace)Enum.Parse(typeof(ColorSpace), XmlHelper.GetValue(colorElement, "ColorSpace", "RGB"));
            value1 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value1", "0"));
            value2 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value2", "0"));
            value3 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value3", "0"));
        }

        public void Save(XmlElement colorElement)
        {
            XmlHelper.SetValue(colorElement, "ColorSpace", colorSpace.ToString());
            XmlHelper.SetValue(colorElement, "Value1", value1.ToString());
            XmlHelper.SetValue(colorElement, "Value2", value2.ToString());
            XmlHelper.SetValue(colorElement, "Value3", value3.ToString());
        }

        public void SetValue(int valueNo, float value)
        {
            switch (valueNo)
            {
                default:
                case 0: value1 = value; break;
                case 1: value2 = value; break;
                case 2: value3 = value; break;
            }
        }
    }
}
