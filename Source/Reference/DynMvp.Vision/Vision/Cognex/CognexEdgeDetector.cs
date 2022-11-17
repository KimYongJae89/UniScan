using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Cognex.VisionPro.Caliper;
using Cognex.VisionPro;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision.Cognex
{
    public class CognexEdgeDetector : EdgeDetector
    {
        public override EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext)
        {
            EdgeDetectionResult result = new EdgeDetectionResult();

            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, rotatedRect.Width, rotatedRect.Height, MathHelper.DegToRad(rotatedRect.Angle), 0);

            CogCaliper cogCaliper = new CogCaliper();

            CognexGreyImage greyImage = (CognexGreyImage)algoImage;
            greyImage.Save("Caliper.bmp", debugContext);

            cogCaliper.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
            cogCaliper.Edge0Polarity = GetCogPolarity();
            cogCaliper.ContrastThreshold = Param.EdgeThreshold;
            cogCaliper.FilterHalfSizeInPixels = Param.FilterSize;

            CogCaliperResults cogResults = cogCaliper.Execute(greyImage.Image, cogRectangle);

            if (cogResults.Edges.Count > 0)
            {
                result.Result = true;

                CogCaliperEdge maxEdge = cogResults.Edges[0];
                foreach (CogCaliperEdge edge in  cogResults.Edges)
                {
                    if (cogCaliper.Edge0Polarity == CogCaliperPolarityConstants.LightToDark)
                    {
                        if (maxEdge.Contrast > edge.Contrast)
                        {
                            maxEdge = edge;
                        }
                    }
                    else if (cogCaliper.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)
                    {
                        if (maxEdge.Contrast < edge.Contrast)
                        {
                            maxEdge = edge;
                        }
                    }
                    else
                    {
                        if (Math.Abs(maxEdge.Contrast) < Math.Abs(edge.Contrast))
                        {
                            maxEdge = edge;
                        }
                    }
                }
                result.EdgePosition = new PointF((float)maxEdge.PositionX, (float)maxEdge.PositionY);
            }

            return result;
        }

        private CogCaliperPolarityConstants GetCogPolarity()
        {
            switch (Param.EdgeType)
            {
                case EdgeType.DarkToLight: return CogCaliperPolarityConstants.DarkToLight;
                case EdgeType.LightToDark: return CogCaliperPolarityConstants.LightToDark;
                case EdgeType.Any: return CogCaliperPolarityConstants.DontCare;
            }

            return CogCaliperPolarityConstants.DarkToLight;
        }
    }
}
