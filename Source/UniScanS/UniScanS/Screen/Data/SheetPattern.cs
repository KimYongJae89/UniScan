using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanS.Screen.Data
{
    public class SheetPattern
    {
        bool needInspect;
        public bool NeedInspect
        {
            get { return needInspect; }
            set { needInspect = value; }
        }

        Bitmap bitmapImage;
        public Bitmap BitmapImage
        {
            get { return bitmapImage; }
            set { bitmapImage = value; }
        }
        
        SheetPatternGroup patternGroup;
        public SheetPatternGroup PatternGroup
        {
            get { return patternGroup; }
            set { patternGroup = value; }
        }

        public void Dispose()
        {
            bitmapImage.Dispose();
        }

        public SheetPattern(XmlElement paramElement)
        {
            LoadParam(paramElement);
        }

        public SheetPattern(Bitmap image, SheetPatternGroup patternGroup)
        {
            this.bitmapImage = image;
            this.patternGroup = patternGroup;
        }
        
        public void LoadParam(XmlElement paramElement)
        {
            needInspect = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "NeedInspect", "false"));

            XmlElement patternGroupElement = paramElement["PatternGroup"];
            patternGroup = new SheetPatternGroup();
            patternGroup.LoadParam(patternGroupElement);

            string patternImageStr = XmlHelper.GetValue(paramElement, "Image", "");
            if (patternImageStr != null)
                bitmapImage = ImageHelper.Base64StringToBitmap(patternImageStr);
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "NeedInspect", needInspect.ToString());

            XmlElement patternGroupElement = paramElement.OwnerDocument.CreateElement("PatternGroup");
            paramElement.AppendChild(patternGroupElement);
            patternGroup.SaveParam(patternGroupElement);

            string patternImageStr = ImageHelper.BitmapToBase64String(bitmapImage);
            if (patternImageStr != null)
                XmlHelper.SetValue(paramElement, "Image", patternImageStr);
        }

        public FigureGroup GetFigureGroup()
        {
            FigureGroup figureGroup = new FigureGroup();

            foreach (BlobRect blobRect in patternGroup.PatternList)
                figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Yellow)));

            return figureGroup;
        }

        public override string ToString()
        {
            return string.Format("Area : {0:0.00}\nWidth : {1:0.00}\nHeight : {2:0.00}\nCenterX : {3:0.00}\nCenterY : {4:0.00}",
                    patternGroup.AverageArea, patternGroup.AverageWidth, patternGroup.AverageHeight, patternGroup.AverageCenterOffsetX, patternGroup.AverageCenterOffsetY);
        }
    }
}
