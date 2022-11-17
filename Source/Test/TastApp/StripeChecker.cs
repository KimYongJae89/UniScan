using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastApp
{
    public class StripeCheckerResult
    {
        float meanWidth;
        PointF point1;
        PointF point2;

        public PointF Point1 { get => point1; set => point1 = value; }
        public PointF Point2 { get => point2; set => point2 = value; }
        public float MeanWidth { get => meanWidth; set => meanWidth = value; }
    }

    public static class StripeChecker
    {
        public static StripeCheckerResult Check(AlgoImage algoImage, PointF pos, float width, float height, float angle)
        {
            MIL_ID detectorId = MIL.M_NULL;
            MIL.MmeasAllocMarker(MIL.M_DEFAULT_HOST, MIL.M_STRIPE, MIL.M_DEFAULT, ref detectorId);
            
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_CENTER, pos.X, pos.Y);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_SIZE, width, height);

            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_ANGLE, angle, MIL.M_NULL);

            MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, MIL.M_POSITIVE, MIL.M_NEGATIVE);
            MIL.MmeasSetMarker(detectorId, MIL.M_ORIENTATION, MIL.M_VERTICAL, MIL.M_NULL);

            MilGreyImage milImage = (MilGreyImage)algoImage;

            MIL.MmeasFindMarker(MIL.M_DEFAULT, milImage.Image, detectorId, MIL.M_DEFAULT);

            int numberOccurrencesFound = 0;
            MIL.MmeasGetResult(detectorId, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound);

            double[] meanWidth = new double[numberOccurrencesFound];
            double[] positionX1 = new double[numberOccurrencesFound];
            double[] positionY1 = new double[numberOccurrencesFound];
            double[] positionX2 = new double[numberOccurrencesFound];
            double[] positionY2 = new double[numberOccurrencesFound];

            MIL.MmeasGetResult(detectorId, MIL.M_STRIPE_WIDTH, meanWidth, MIL.M_NULL);
            MIL.MmeasGetResult(detectorId, MIL.M_POSITION + MIL.M_EDGE_FIRST, positionX1, positionY1);
            MIL.MmeasGetResult(detectorId, MIL.M_POSITION + MIL.M_EDGE_SECOND, positionX2, positionY2);
            
            MIL.MmeasFree(detectorId);

            StripeCheckerResult result = new StripeCheckerResult();
            for (int i = 0; i < numberOccurrencesFound; i++)
            {
                result.MeanWidth = width;
                result.Point1 = new PointF((float)positionX1[i], (float)positionY1[i]);
                result.Point2 = new PointF((float)positionX2[i], (float)positionY2[i]);
            }

            return result;
        }
    }
}