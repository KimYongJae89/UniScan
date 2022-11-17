using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanS.Vision.FiducialFinder
{
    public class FiducialPattern
    {
        Pattern pattern;
        public Pattern Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        PointF centerPt;
        public PointF CenterPt
        {
            get { return centerPt; }
            set { centerPt = value; }
        }

        Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }
        
        public FiducialPattern(XmlElement algorithmElement)
        {
            LoadParam(algorithmElement);
        }

        public FiducialPattern(Image2D patternImage, Rectangle region, PointF centerPt)
        {
            pattern = new MilPattern();
            pattern.PatternImage = patternImage;
            this.region = region;
            this.centerPt = centerPt;
        }

        public void Dispose()
        {
            pattern?.Dispose();
        }

        public void Train(PatternMatchingParam patternMatchingParam)
        {
            AlgoImage patternImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, pattern.PatternImage, ImageType.Grey);
            
            pattern.Train(patternImage, patternMatchingParam);
            
            patternImage.Dispose();
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            pattern.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CenterX", centerPt.X.ToString());
            XmlHelper.SetValue(algorithmElement, "CenterY", centerPt.Y.ToString());

            XmlHelper.SetValue(algorithmElement, "RegionX", region.X.ToString());
            XmlHelper.SetValue(algorithmElement, "RegionY", region.Y.ToString());
            XmlHelper.SetValue(algorithmElement, "RegionWidth", region.Width.ToString());
            XmlHelper.SetValue(algorithmElement, "RegionHeight", region.Height.ToString());
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            if (pattern != null)
                pattern.Dispose();

            pattern = new MilPattern();//AlgorithmBuilder.CreatePattern();
            pattern.LoadParam(algorithmElement);
            region = new Rectangle(
                Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RegionX", "0")),
                Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RegionY", "0")),
                Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RegionWidth", "0")),
                Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RegionHeight", "0")));

            centerPt = new PointF(
                Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "CenterX", "0")),
                Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "CenterY", "0")));
        }

        public FigureGroup GetFigureGroup(int searchRangeHalfWidth, int searchRangeHalfHeight)
        {
            FigureGroup figureGroup = new FigureGroup();
            
            figureGroup.AddFigure(new RectangleFigure(region, new Pen(Color.Blue, 2)));
            Rectangle inflateRegion = region;
            inflateRegion.Inflate(searchRangeHalfWidth, searchRangeHalfHeight);
            figureGroup.AddFigure(new RectangleFigure(inflateRegion, new Pen(Color.Green, 5)));

            return figureGroup;
        }
    }
}
