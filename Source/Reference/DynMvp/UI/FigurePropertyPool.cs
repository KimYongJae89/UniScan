using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.UI
{
    public class FigureProperty
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        Font font;
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        Color textColor;
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        StringAlignment alignment;
        public StringAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        Pen pen;
        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }

        Brush brush;
        public Brush Brush
        {
            get { return brush; }
            set { brush = value; }
        }

        public FigureProperty(string name = "")
        {
            this.name = name;
            font = new Font("Arial", 12, FontStyle.Regular);
            pen = new Pen(Color.Yellow, 0);
            brush = null;
            alignment = StringAlignment.Near;
            textColor = Color.Black;
        }

        public FigureProperty Clone()
        {
            FigureProperty cloneFigureProperty = new FigureProperty();

            cloneFigureProperty.font = (Font)font.Clone();
            cloneFigureProperty.pen = (Pen)pen.Clone();
            if (brush != null)
                cloneFigureProperty.brush = (Brush)brush.Clone();
            cloneFigureProperty.alignment = alignment;
            cloneFigureProperty.textColor = textColor;

            return cloneFigureProperty;
        }

        public void Load(XmlElement xmlElement)
        {
            Font font = new Font("Arial", 12, FontStyle.Regular);
            if (XmlHelper.GetValue(xmlElement, "Font", ref font))
                this.Font = font;

            Brush brush = new SolidBrush(Color.Ivory);
            if (XmlHelper.GetValue(xmlElement, "Brush", ref brush))
                this.Brush = brush;

            Pen pen = new Pen(Color.Black);
            if (XmlHelper.GetValue(xmlElement, "Pen", ref pen))
                this.Pen = pen;

            Color textColor = Color.Black;
            if (XmlHelper.GetValue(xmlElement, "TextColor", ref textColor))
                this.TextColor = textColor;

            this.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), XmlHelper.GetValue(xmlElement, "Alignment", StringAlignment.Near.ToString()));
        }

        public void Save(XmlElement xmlElement)
        {
            if (font != null)
                XmlHelper.SetValue(xmlElement, "Font", font);
            if (brush != null)
                XmlHelper.SetValue(xmlElement, "Brush", brush);
            if (pen != null)
                XmlHelper.SetValue(xmlElement, "Pen", pen);
            if (textColor != null)
                XmlHelper.SetValue(xmlElement, "TextColor", textColor);

            XmlHelper.SetValue(xmlElement, "Alignment", alignment.ToString());
        }
    }

    /// <summary>
    /// Figure 객체의 공용 Drawing Object 관리 객체
    /// 이 클래스를 상속 받아 프로그램에서 사용할 FigureProperty 목록을 관리할 수 있다.
    /// </summary>
    public class FigurePropertyPool
    {
        static FigurePropertyPool _instance;
        public static void SetInstance(FigurePropertyPool instance)
        {
            _instance = instance;
        }
        public static FigurePropertyPool Instance()
        {
            if (_instance == null)
                _instance = new FigurePropertyPool();

            return _instance;
        }

        List<FigureProperty> figurePropertyList = new List<FigureProperty>();
        public List<FigureProperty> FigurePropertyList
        {
            get { return figurePropertyList; }
            set { figurePropertyList = value; }
        }

        /// <summary>
        /// FigurePropertyPool을 파일로 부터 읽어 온다.
        /// </summary>
        /// <param name="fileName">FigurePropertyPool을 읽어 올 파일명</param>
        public void Load(string fileName)
        {
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement docElement = xmlDocument["FigurePropertyPool"];

            figurePropertyList.Clear();

            foreach (XmlElement figurePropertyElement in docElement)
            {
                string figurePropertyName = XmlHelper.GetValue(figurePropertyElement, "Name", "");
                GetFigureProperty(figurePropertyName).Load(figurePropertyElement);
            }
        }

        /// <summary>
        /// FigurePropertyPool을 저장한다.
        /// </summary>
        /// <param name="fileName">FigurePropertyPool을 저장할 파일명</param>
        public void Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement figurePropertyPoolElement = xmlDocument.CreateElement("", "FigurePropertyPool", "");
            xmlDocument.AppendChild(figurePropertyPoolElement);

            foreach (FigureProperty figureProperty in figurePropertyList)
            {
                XmlElement figurePropertyElement = xmlDocument.CreateElement("", "FigureProperty", "");
                figurePropertyPoolElement.AppendChild(figurePropertyElement);

                figureProperty.Save(figurePropertyElement);
            }

            xmlDocument.Save(fileName);
        }


        /// <summary>
        /// Figure Property Pool에 저장된 FigureProperty를 가져 온다.
        /// 만약, 전달된 이름을 가진 Figure Property가 없다면 새로운 객체를 생성하여 반환한다.
        /// </summary>
        /// <param name="figurePropertyName">Figure Property 이름</param>
        /// <returns></returns>
        public virtual FigureProperty GetFigureProperty(string figurePropertyName)
        {
            FigureProperty figureProperty = figurePropertyList.Find(x => figurePropertyName == x.Name);

            if (figureProperty == null)
            {
                figureProperty = new FigureProperty(figurePropertyName);
                figurePropertyList.Add(figureProperty);
            }

            return figureProperty;
        }

        /// <summary>
        /// 새로운 FIgureProperty를 추가한다.
        /// </summary>
        /// <param name="figureProperty">추가할 FIgure Property</param>
        public void AddFigure(FigureProperty figureProperty)
        {
            figurePropertyList.Add(figureProperty);
        }
    }
}
