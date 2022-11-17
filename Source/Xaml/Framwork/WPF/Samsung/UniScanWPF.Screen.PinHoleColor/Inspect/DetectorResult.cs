using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Screen.PinHoleColor.Color.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public abstract class Defect
    {
        private int sectionIndex;
        protected string resultPath;

        protected int index;
        protected Rectangle rectangle;
        protected BitmapSource image;

        public int Index { get => index; }
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public BitmapSource Image
        {
            get
            {
                if (image == null && resultPath != null)
                {
                    string imagePath = Path.Combine(resultPath, string.Format("Defect{0}.png", index));
                    if (File.Exists(imagePath) == true)
                        image = WPFImageHelper.LoadBitmapSource(imagePath);
                }

                return image;
            }
        }
        public int SectionIndex { get => sectionIndex; set => sectionIndex = value; }

        public static Defect ImportResult(DetectorType type, string resultPath, XmlElement defectElement)
        {
            int index = Convert.ToInt32(XmlHelper.GetValue(defectElement, "Index", "0"));
            int x = Convert.ToInt32(XmlHelper.GetValue(defectElement, "X", "0"));
            int y = Convert.ToInt32(XmlHelper.GetValue(defectElement, "Y", "0"));
            int width = Convert.ToInt32(XmlHelper.GetValue(defectElement, "Width", "0"));
            int height = Convert.ToInt32(XmlHelper.GetValue(defectElement, "Height", "0"));

            string imagePath = Path.Combine(resultPath, string.Format("Defect{0}.png", index));
            //if (File.Exists(imagePath) == true)
                //image = WPFImageHelper.LoadBitmapSource(imagePath);

            Rectangle rectangle = new Rectangle(x, y, width, height);

            switch (type)
            {
                case DetectorType.PinHole:
                    return new PinHoleDefect(resultPath, index, rectangle, defectElement);
                case DetectorType.Color:
                    return new ColorDefect(resultPath, index, rectangle, defectElement);
            }

            return null;
        }

        public virtual void ExportResult(string resultPath, XmlElement defectElement)
        {
            XmlHelper.SetValue(defectElement, "Index", index.ToString());
            XmlHelper.SetValue(defectElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(defectElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(defectElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(defectElement, "Height", rectangle.Height.ToString());

            if (image != null)
            {
                string imagePath = Path.Combine(resultPath, string.Format("Defect{0}.png", index));
                WPFImageHelper.SaveBitmapSource(imagePath, image);
            }
        }
    }

    public abstract class DetectorResult
    {
        protected List<Defect> defectList = new List<Defect>();
        protected Judgment judgment = Judgment.Reject;

        public abstract DetectorType GetDetectorType();

        public Judgment Judgment { get => judgment; set => judgment = value; }
        public List<Defect> DefectList { get => defectList; set => defectList = value; }
        public abstract List<UIElement> GetFigures();
        public virtual void ImportResult(string resultPath, XmlElement detectorElement)
        {
            judgment = (Judgment)Enum.Parse(typeof(Judgment), XmlHelper.GetValue(detectorElement, "Judgment", Judgment.Reject.ToString()));
            foreach (XmlElement defectElement in detectorElement.ChildNodes)
            {
                if (defectElement.Name.Contains("Defect") == false)
                    continue;

                defectList.Add(Defect.ImportResult(GetDetectorType(), resultPath, defectElement));
            }
        }

        public virtual void ExportResult(string resultPath, XmlElement detectorElement)
        {
            XmlHelper.SetValue(detectorElement, "Judgment", judgment.ToString());

            foreach (Defect defect in defectList)
            {
                XmlElement defectElement = detectorElement.OwnerDocument.CreateElement("", string.Format("Defect{0}", defect.Index), "");
                detectorElement.AppendChild(defectElement);

                defect.ExportResult(resultPath, defectElement);
            }
        }
    }
}
