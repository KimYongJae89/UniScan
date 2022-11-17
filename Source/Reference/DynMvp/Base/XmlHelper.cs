using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.UI;
using System.IO;
using System.Globalization;

namespace DynMvp.Base
{
    public interface IStoring : ICloneable
    {
        void Save(XmlElement xmlElement, string key);
        void Load(XmlElement xmlElement, string key);
    }

    public class XmlHelper
    {
        public static XmlDocument Create(string root)
        {
            return new XmlDocument();
        }

        public static XmlDocument Load(string fileName)
        {
            XmlDocument xmlDocument;
            try
            {
                xmlDocument = _Load(fileName);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, string.Format("XmlHelper::Load - Exception: {0}", ex.Message));
                if (!File.Exists(fileName + ".bak"))
                    return null;
                xmlDocument = _Load(fileName + ".bak");
            }

            return xmlDocument;
        }

        private static XmlDocument _Load(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            return xmlDocument;
        }

        public static void Save(XmlDocument xmlDocument, string fileName)
        {
            string tempFileName = fileName + "~";
            string bakName = fileName + ".bak";

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            using (XmlWriter xmlWriter = XmlWriter.Create(tempFileName, xmlSettings))
            {
                xmlDocument.Save(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
                xmlWriter.Dispose();
            }

            FileHelper.SafeSave(tempFileName, bakName, fileName);
        }


        public static XmlElement RootElement(XmlDocument xmlDocument, string rootNodeName)
        {
            XmlElement xmlElement;
            if (!xmlDocument.HasChildNodes)
            {
                xmlElement = xmlDocument.CreateElement("", rootNodeName, "");
                xmlDocument.AppendChild(xmlElement);
            }
            else
            {
                xmlElement = (XmlElement)xmlDocument.FirstChild;
            }

            return xmlElement;
        }

        public static string GetAttributeValue(XmlElement xmlElement, string attributeName, string defaultValue)
        {
            string attributeValue = xmlElement.GetAttribute(attributeName);
            if (attributeValue == "")
                return defaultValue;

            return attributeValue;
        }

        public static XmlElement CreateAndAppendChild(XmlElement xmlElemnt, string name)
        {
            return (XmlElement)xmlElemnt.AppendChild(xmlElemnt.OwnerDocument.CreateElement(name));
        }

        public static void SetAttributeValue(XmlElement xmlElement, string attributeName, string value)
        {
            xmlElement.SetAttribute(attributeName, value);
        }

        public static bool Exist(XmlElement xmlElement, string keyName)
        {
            XmlElement subElement = xmlElement[keyName];
            return (subElement != null);
        }


        public static void SetValue(XmlElement xmlElement, string keyName, IStoring value)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            value?.Save(subElement, null);
        }

        public static void SetValues(XmlElement xmlElement, string keyName, IStoring[] values)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            foreach (IStoring value in values)
                value.Save(subElement, "Item");
        }

        public static void SetValue(XmlElement xmlElement, string keyName, string value)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = value;
        }

        public static string GetValue(XmlElement xmlElement, string keyName, string defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];

            if (subElement == null)
                return defaultValue;

            return subElement.InnerText;
        }

        public static ImageD GetValue(XmlElement xmlElement, string keyName, ImageD imageD)
        {
            ImageD loadedImage = imageD;
            string imageString = XmlHelper.GetValue(xmlElement, keyName, "");
            if (!string.IsNullOrEmpty(imageString))
            {
                using (Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString))
                    loadedImage = Image2D.FromBitmap(bitmap);
            }
            return loadedImage;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, ImageD imageD)
        {
            using (Bitmap bitmap = imageD.ToBitmap())
            {
                string imageString = ImageHelper.BitmapToBase64String(bitmap);
                XmlHelper.SetValue(xmlElement, "MasterImageD", imageString);
            }
        }

        // DateTime

        public static void SetValue(XmlElement xmlElement, string keyName, DateTime value)
        {
            SetValue(xmlElement, keyName, value, "yyyy-MM-dd HH:mm:ss");
        }

        public static void SetValue(XmlElement xmlElement, string keyName, DateTime value, string format)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = value.ToString(format);
        }

        public static DateTime GetValue(XmlElement xmlElement, string keyName, DateTime defaultValue)
        {
            DateTime dateTime = GetValue(xmlElement, keyName, "yyyy-MM-dd HH:mm:ss", DateTime.MinValue);
            if (dateTime == DateTime.MinValue)
            {
                CultureInfo culture = new CultureInfo("ko-KR");
                dateTime = GetValue(xmlElement, keyName, culture, defaultValue);
            }
            return dateTime;
        }

        public static DateTime GetValue(XmlElement xmlElement, string keyName, string format, DateTime defaultValue)
        {
            DateTime dateTIme;
            string parse = GetValue(xmlElement, keyName, defaultValue.ToString(format));
            bool good = DateTime.TryParseExact(parse, format, null, DateTimeStyles.None, out dateTIme);
            return good ? dateTIme : defaultValue;
        }

        public static DateTime GetValue(XmlElement xmlElement, string keyName, CultureInfo cultureInfo, DateTime defaultValue)
        {
            DateTime dateTIme;
            string parse = GetValue(xmlElement, keyName, defaultValue.ToString());
            bool good = DateTime.TryParse(parse, cultureInfo, DateTimeStyles.None, out dateTIme);
            return good ? dateTIme : defaultValue;
        }

        // int
        public static void SetValue(XmlElement xmlElement, string keyName, int value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static int GetValue(XmlElement xmlElement, string keyName, int @default)
        {
            string value = GetValue(xmlElement, keyName, @default.ToString());
            if (int.TryParse(value, out int @int))
                return @int;

            return @default;
        }

        public static byte GetValue(XmlElement xmlElement, string keyName, byte defaultValue)
        {
            string value = GetValue(xmlElement, keyName, defaultValue.ToString());
            return byte.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref int getValue)
        {
            getValue = int.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, uint defaultValue, ref uint getValue)
        {
            getValue = uint.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // float
        public static void SetValue(XmlElement xmlElement, string keyName, float value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, float[] values)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            Array.ForEach(values, f => SetValue(subElement, "Value", f));
        }

        public static float GetValue(XmlElement xmlElement, string keyName, float defaultValue)
        {
            return float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, float defaultValue, ref float getValue)
        {
            getValue = float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, ref float[] values)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
            {
                values = new float[0];
                return;
            }

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Value");
            values = new float[xmlNodeList.Count];
            for (int i = 0; i < xmlNodeList.Count; i++)
                values[i] = GetValue((XmlElement)xmlNodeList[i], "", 0.0f);
        }

        public static int GetItemCount(XmlElement xmlElement, string keyName)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return 0;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Item");
            return xmlNodeList.Count;
        }

        public static IStoring GetValue(XmlElement xmlElement, string keyName, IStoring defaultValue)
        {
            IStoring clone = (IStoring)defaultValue.Clone();

            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return clone;

            clone.Load(subElement, null);
            return clone;
        }

        public static int GetValues(XmlElement xmlElement, string keyName, IStoring[] values)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return 0;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Item");
            int loop = Math.Min(values.Length, xmlNodeList.Count);
            for (int i = 0; i < loop; i++)
                values[i].Load((XmlElement)xmlNodeList[i], null);
            return loop;
        }

        // double
        public static void SetValue(XmlElement xmlElement, string keyName, double value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, double[] values)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            Array.ForEach(values, f => SetValue(subElement, "Value", f));
        }

        public static double GetValue(XmlElement xmlElement, string keyName, double defaultValue)
        {
            return double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, ref double[] values)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
            {
                values = new double[0];
                return;
            }

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Value");
            values = new double[xmlNodeList.Count];
            for (int i = 0; i < xmlNodeList.Count; i++)
                values[i] = GetValue((XmlElement)xmlNodeList[i], "", 0.0);
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref double getValue)
        {
            getValue = double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // bool
        public static void SetValue(XmlElement xmlElement, string keyName, bool value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }
        
        public static bool GetValue(XmlElement xmlElement, string keyName, bool defaultValue)
        {
            bool parse;
            if (!bool.TryParse(GetValue(xmlElement, keyName, defaultValue.ToString()), out parse))
                return defaultValue;
            return parse;
        }

        public static void GetValue(XmlElement xmlElement, string keyName, bool defaultValue, ref bool getValue)
        {
            bool ok = bool.TryParse(GetValue(xmlElement, keyName, defaultValue.ToString()), out getValue);
            if (ok == false)
                getValue = defaultValue;
        }

        // Color
        //public static Color GetValue(XmlElement xmlElement, string keyName, Color defaultValue)
        //{
        //    int value = GetValue(xmlElement, keyName, defaultValue.ToArgb());
        //    return Color.FromArgb(value);
        //}

        //public static void SetValue(XmlElement xmlElement, string keyName, Color defaultValue)
        //{
        //    int value = GetValue(xmlElement, keyName, defaultValue.ToArgb());
        //    SetValue(xmlElement, keyName, value);
        //}

        // Rectangle
        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle[] rectangles)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            foreach (Rectangle rectangle in rectangles)
                SetValue(subElement, "Rectangle", rectangle);
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle[,] rectangles)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            int length0 = rectangles.GetLength(0);
            int length1 = rectangles.GetLength(1);
            XmlHelper.SetValue(subElement, "Length0", length0.ToString());
            XmlHelper.SetValue(subElement, "Length1", length1.ToString());

            for (int l0 = 0; l0 < length0; l0++)
            {
                XmlElement rowElement = subElement.OwnerDocument.CreateElement(string.Format("R{0}", l0));
                subElement.AppendChild(rowElement);
                for (int l1 = 0; l1 < length1; l1++)
                    SetValue(rowElement, string.Format("C{0}", l1), rectangles[l0, l1]);
            }
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = XmlHelper.GetValue(subElement, "X", 0);
            rectangle.Y = XmlHelper.GetValue(subElement, "Y", 0);
            rectangle.Width = XmlHelper.GetValue(subElement, "Width", 0);
            rectangle.Height = XmlHelper.GetValue(subElement, "Height", 0);

            return true;
        }

        public static Rectangle GetValue(XmlElement xmlElement, string keyName, Rectangle defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            Rectangle rectangle = new Rectangle();
            rectangle.X = XmlHelper.GetValue(subElement, "X", 0);
            rectangle.Y = XmlHelper.GetValue(subElement, "Y", 0);
            rectangle.Width = XmlHelper.GetValue(subElement, "Width", 0);
            rectangle.Height = XmlHelper.GetValue(subElement, "Height", 0);
            return rectangle;
        }

        public static RectangleF GetValue(XmlElement xmlElement, string keyName, RectangleF defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            RectangleF rectangle = new RectangleF();
            rectangle.X = XmlHelper.GetValue(subElement, "X", 0.0f);
            rectangle.Y = XmlHelper.GetValue(subElement, "Y", 0.0f);
            rectangle.Width = XmlHelper.GetValue(subElement, "Width", 0.0f);
            rectangle.Height = XmlHelper.GetValue(subElement, "Height", 0.0f);
            return rectangle;
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle[] rectangles)
        {
            rectangles = new Rectangle[0];
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Rectangle");
            Array.Resize(ref rectangles, xmlNodeList.Count);

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement subElement2 = (XmlElement)xmlNodeList[i];
                GetValue(subElement2, "", ref rectangles[i]);
            }

            return true;
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle[,] rectangles)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            int length0 = Convert.ToInt32(XmlHelper.GetValue(subElement, "Length0", "0"));
            int length1 = Convert.ToInt32(XmlHelper.GetValue(subElement, "Length1", "0"));
            rectangles = new Rectangle[length0, length1];

            for (int l0 = 0; l0 < length0; l0++)
            {
                XmlElement rowElement = subElement[string.Format("R{0}", l0)];
                if (rowElement == null)
                    continue;

                for (int l1 = 0; l1 < length1; l1++)
                {
                    GetValue(rowElement, string.Format("C{0}", l1), ref rectangles[l0, l1]);
                }
            }
            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RectangleF rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RectangleF rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RotatedRect rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
            XmlHelper.SetValue(subElement, "Angle", rectangle.Angle.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RotatedRect rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));
            rectangle.Angle = Convert.ToSingle(XmlHelper.GetValue(subElement, "Angle", "0"));

            return true;
        }

        // Pen
        public static void SetValue(XmlElement xmlElement, string keyName, Pen pen)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Color", pen.Color);
            XmlHelper.SetValue(subElement, "Width", pen.Width.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Pen pen)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            Color color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);
            int width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "1"));

            pen = new Pen(color, width);

            return true;
        }

        // Brush
        public static void SetValue(XmlElement xmlElement, string keyName, Brush brush)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            SolidBrush solidBrush = brush as SolidBrush;

            XmlHelper.SetValue(subElement, "Color", solidBrush.Color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Brush brush)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            Color color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);

            brush = new SolidBrush(color);

            return true;
        }

        // Point
        public static void SetValue(XmlElement xmlElement, string keyName, Point point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point.Y.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Point[] Points)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            foreach (Point rectangle in Points)
                SetValue(subElement, "Point", rectangle);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point point)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point.X = Convert.ToInt32(XmlHelper.GetValue(subElement, "X", "0"));
            point.Y = Convert.ToInt32(XmlHelper.GetValue(subElement, "Y", "0"));

            return true;
        }

        public static Point GetValue(XmlElement xmlElement, string keyName, Point point)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return point;

            point.X = Convert.ToInt32(XmlHelper.GetValue(subElement, "X", "0"));
            point.Y = Convert.ToInt32(XmlHelper.GetValue(subElement, "Y", "0"));

            return point;
        }


        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point[] points)
        {
            points = new Point[0];

            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Point");
            Array.Resize(ref points, xmlNodeList.Count);

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement subElement2 = (XmlElement)xmlNodeList[i];
                GetValue(subElement2, "", ref points[i]);
            }

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, PointF point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point.Y.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref PointF point)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            point.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Point3d point3d)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point3d.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point3d.Y.ToString());
            XmlHelper.SetValue(subElement, "Z", point3d.Z.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point3d point3d)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point3d.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            point3d.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            point3d.Z = Convert.ToSingle(XmlHelper.GetValue(subElement, "Z", "0"));

            return true;
        }

        // Size
        public static void SetValue(XmlElement xmlElement, string keyName, Size size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Width", size.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", size.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Size size)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            size.Width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "0"));
            size.Height = Convert.ToInt32(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static Size GetValue(XmlElement xmlElement, string keyName, Size defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            Size size = new Size();
            size.Width = XmlHelper.GetValue(subElement, "Width", defaultValue.Width);
            size.Height = XmlHelper.GetValue(subElement, "Height", defaultValue.Height);
            return size;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, SizeF size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Width", size.Width);
            XmlHelper.SetValue(subElement, "Height", size.Height);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref SizeF size)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            size.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            size.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static SizeF GetValue(XmlElement xmlElement, string keyName, SizeF defaultSize)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return defaultSize;

            SizeF sizeF = new SizeF();
            sizeF.Width = XmlHelper.GetValue(subElement, "Width", defaultSize.Width);
            sizeF.Height = XmlHelper.GetValue(subElement, "Height", defaultSize.Height);

            return sizeF;
        }

        // Font
        public static void SetValue(XmlElement xmlElement, string keyName, Font font)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Family", font.FontFamily.GetName(0));
            XmlHelper.SetValue(subElement, "Size", font.SizeInPoints.ToString());
            XmlHelper.SetValue(subElement, "Style", font.Style.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Font font)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            string family = XmlHelper.GetValue(subElement, "Family", "Arial");
            float size = Convert.ToSingle(XmlHelper.GetValue(subElement, "Size", "10"));
            FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle), XmlHelper.GetValue(subElement, "Style", "Regular"));

            font = new Font(family, size, style);

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Color color)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = System.Drawing.ColorTranslator.ToHtml(color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Color color)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            color = System.Drawing.ColorTranslator.FromHtml(subElement.InnerText);

            return true;
        }

        //Enum
        public static void SetValue(XmlElement xmlElement, string keyName, Enum @enum)
        {
            SetValue(xmlElement, keyName, @enum.ToString());
        }


        public static TValue GetValue<TValue>(XmlElement xmlElement, string keyName, TValue defaultValue)
             where TValue : struct
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            Type type = typeof(TValue);
            string stringValue = XmlHelper.GetValue(xmlElement, keyName, defaultValue.ToString());

            if (type.IsEnum)
            {
                TValue result;
                if (Enum.TryParse<TValue>(stringValue, out result))
                    return result;
                return defaultValue;
            }
            else
            {
                try
                {
                    return (TValue)Convert.ChangeType(stringValue, typeof(TValue));
                }
                catch (InvalidCastException ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("XmlHelper::GetValue<{0}> Exception - {1}", type, ex.Message));
                    return defaultValue;
                }
            }
        }
    }
}
