using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.CompositeColorMatch;
using System.Windows.Forms;

namespace DynMvp.Vision.Cognex
{
    public class CognexColorMatchChecker : ColorMatchChecker
    {
        CogCompositeColorMatchPattern colorMatchPattern = new CogCompositeColorMatchPattern();

        public CognexColorMatchChecker()
        {

        }

        public override Algorithm Clone()
        {
            CognexColorMatchChecker CognexColorMatchChecker = new CognexColorMatchChecker();
            CognexColorMatchChecker.CopyFrom(this);

            return CognexColorMatchChecker;
        }

        public override bool Trained
        {
            get { return colorMatchPattern.Trained; }
        }

        public override void PrepareInspection()
        {
             Train();
        }

        public override void Train()
        {
            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            colorMatchPattern.CompositeColorCollection.Clear();

            foreach (ColorPattern colorPattern in colorMatchCheckerParam.ColorPatternList)
            {
                CogRectangle cogRectangle = new CogRectangle();
                
                cogRectangle.SetXYWidthHeight(0, 0, colorPattern.Image.Width, colorPattern.Image.Height);

                CogCompositeColorItem item = new CogCompositeColorItem(CognexImageBuilder.ConvertColorImage(colorPattern.Image), cogRectangle);
                item.Name = colorPattern.Name;
                colorMatchPattern.GaussianSmoothing = colorPattern.Smoothing;
                colorMatchPattern.CompositeColorCollection.Add(item);
                try
                {
                    colorMatchPattern.Train();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public override ColorMatchCheckerResult Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext)
        {
            ColorMatchCheckerResult result = new ColorMatchCheckerResult();

            PointF centerPt = DrawingHelper.CenterPoint(probeRegion);

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, probeRegion.Width, probeRegion.Height, 0, 0);

            CognexColorImage colorImage = (CognexColorImage)algoImage;
            colorImage.Save("colorMatch.bmp", debugContext);

            CogCompositeColorMatchRunParams runParams = new CogCompositeColorMatchRunParams();
            runParams.SortResultSetByScores = true;

            CogCompositeColorMatchResultSet matchResults = colorMatchPattern.Execute(colorImage.Image, cogRectangle, runParams);

            foreach (CogCompositeColorMatchResult matchResult in matchResults)
            {
                result.AddColorMatchResult(new ColorMatchResult(matchResult.Color.Name, (float)matchResult.MatchScore));
            }

            return result;
        }
    }
}
