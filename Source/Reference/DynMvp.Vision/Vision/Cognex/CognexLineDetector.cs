using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.Base;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro;

namespace DynMvp.Vision.Cognex
{
    public class CognexLineDetector : LineDetector
    {
        public override LineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext)
        {
            CogFindLine cogFindLine = new CogFindLine();

            DebugHelper.SaveImage(algoImage, "Caliper.bmp", debugContext);

            CognexGreyImage greyImage = (CognexGreyImage)algoImage;

            cogFindLine.NumCalipers = Param.NumEdgeDetector;
            cogFindLine.ExpectedLineSegment.SetStartEnd(startPt.X, startPt.Y, endPt.X, endPt.Y);

            cogFindLine.CaliperProjectionLength = Param.ProjectionHeight;
            cogFindLine.CaliperSearchLength = Param.SearchLength;
            cogFindLine.CaliperRunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
            cogFindLine.CaliperRunParams.Edge0Polarity = GetCogPolarity();
            cogFindLine.CaliperSearchDirection = MathHelper.DegToRad(Param.SearchAngle);

            cogFindLine.CaliperRunParams.ContrastThreshold = Param.EdgeThreshold;
            cogFindLine.CaliperRunParams.FilterHalfSizeInPixels = Param.FilterSize;

            CogFindLineResults results = cogFindLine.Execute(greyImage.Image);

            return GetLineParameter(results);
        }

        public LineEq GetLineParameter(CogFindLineResults results)
        {
            LineEq lineEq = new LineEq();
            if (results != null && results.Count > 0)
            {
                CogLine line = results.GetLine();
                if (line != null)
                {
                    lineEq.Slope = new PointF((float)1, (float)Math.Tan(line.Rotation));
                    lineEq.PointOnLine = new PointF((float)line.X, (float)line.Y);
                }
            }

            return lineEq;
        }

        private CogCaliperPolarityConstants GetCogPolarity()
        {
            switch(Param.EdgeType)
            {
                case EdgeType.DarkToLight: return CogCaliperPolarityConstants.DarkToLight;
                case EdgeType.LightToDark: return CogCaliperPolarityConstants.LightToDark;
                case EdgeType.Any: return CogCaliperPolarityConstants.DontCare;
            }

            return CogCaliperPolarityConstants.DarkToLight;
        }
    }
}
