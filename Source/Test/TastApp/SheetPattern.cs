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

namespace TastApp
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
        
        public SheetPattern(Bitmap image, SheetPatternGroup patternGroup)
        {
            this.bitmapImage = image;
            this.patternGroup = patternGroup;
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
