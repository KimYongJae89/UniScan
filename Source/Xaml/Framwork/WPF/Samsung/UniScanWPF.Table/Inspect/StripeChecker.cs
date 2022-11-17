//using DynMvp.Vision;
//using DynMvp.Vision.Matrox;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Shapes;

//namespace UniScanWPF.Table.Inspect
//{
//    public class Stripe
//    {
//        float length;
//        PointF point1;
//        PointF point2;

//        public PointF Point1 { get => point1; set => point1 = value; }
//        public PointF Point2 { get => point2; set => point2 = value; }
//        public float Length { get => length; set => length = value; }
//    }
    
//    public class StripeCheckerResult
//    {
//        List<Stripe> stripeList;

//        public List<Stripe> StripeList { get => stripeList; }

//        public StripeCheckerResult()
//        {
//            stripeList = new List<Stripe>();
//        }

//        public List<Line> GetStripeLineList()
//        {
//            List<Line> lineList = new List<Line>();
//            foreach (var stripe in stripeList)
//            {
//                Line line = new Line();
//                line.X1 = stripe.Point1.X;
//                line.X2 = stripe.Point2.X;

//                line.Y1 = stripe.Point1.Y;
//                line.Y2 = stripe.Point2.Y;

//                line.Stroke = System.Windows.Media.Brushes.Red;
//                line.StrokeThickness = 1000;

//                lineList.Add(line);
//            }

//            return lineList;
//        }
//    }

//    public static class StripeChecker
//    {
//        public static StripeCheckerResult Check(AlgoImage algoImage, PointF pos, float rotateWidth, float rotateHeight, float angle)
//        {
//            MilGreyImage milImage = (MilGreyImage)algoImage;
            
//            MIL_ID detectorId = MIL.M_NULL;
//            MIL.MmeasAllocMarker(MIL.M_DEFAULT_HOST, MIL.M_STRIPE, MIL.M_DEFAULT, ref detectorId);

//            MIL.MmeasSetMarker(detectorId, MIL.M_NUMBER, MIL.M_ALL, MIL.M_NULL);

//            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_ORIGIN, pos.X, pos.Y);
//            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_SIZE, rotateWidth, rotateHeight);
//            MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, MIL.M_POSITIVE, MIL.M_NEGATIVE);

//            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_ANGLE, angle, MIL.M_NULL);

//            if (algoImage.Width > 1000)
//                algoImage.Save("asdafcv.bmp", new DebugContext(true, "d:\\"));

//            //MIL.MmeasSetMarker(detectorId, MIL.M_SUB_REGIONS_NUMBER, 10, MIL.M_NULL);
//            //MIL.MmeasSetMarker(detectorId, MIL.M_SUB_REGIONS_SIZE, 1, MIL.M_NULL);
//            //MIL.MmeasSetMarker(detectorId, MIL.M_SUB_REGIONS_OFFSET, 50, MIL.M_NULL);

//            StripeCheckerResult stripeCheckerResult = new StripeCheckerResult();
//            stripeCheckerResult.StripeList.AddRange(FindStripe(milImage, detectorId, Direction.Horizontal));
            
//            MIL.MmeasFree(detectorId);
            
//            return stripeCheckerResult;
//        }

//        private static List<Stripe> FindStripe(MilGreyImage milImage, MIL_ID detectorId, Direction direction)
//        {
//            MIL.MmeasSetMarker(detectorId, MIL.M_ORIENTATION, direction == Direction.Horizontal ? MIL.M_HORIZONTAL : MIL.M_VERTICAL, MIL.M_NULL);

//            MIL.MmeasFindMarker(MIL.M_DEFAULT, milImage.Image, detectorId, MIL.M_DEFAULT);

//            int numberOccurrencesFound = 0;
//            MIL.MmeasGetResult(detectorId, MIL.M_NUMBER+ MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound);

//            List<Stripe> stripeList = new List<Stripe>();
//            if (numberOccurrencesFound > 0)
//            {
//                double[] length = new double[numberOccurrencesFound];
//                double[] positionX1 = new double[numberOccurrencesFound];
//                double[] positionY1 = new double[numberOccurrencesFound];
//                double[] positionX2 = new double[numberOccurrencesFound];
//                double[] positionY2 = new double[numberOccurrencesFound];

//                MIL.MmeasGetResult(detectorId, MIL.M_LENGTH, length, MIL.M_NULL);
//                MIL.MmeasGetResult(detectorId, MIL.M_EDGEVALUE_PEAK_POS_MAX + MIL.M_EDGE_FIRST, positionX1, positionY1);
//                MIL.MmeasGetResult(detectorId, MIL.M_EDGEVALUE_PEAK_POS_MIN + MIL.M_EDGE_SECOND, positionX2, positionY2);

//                for (int i = 0; i < numberOccurrencesFound; i++)
//                {
//                    Stripe stripe = new Stripe();
//                    stripe.Length = (float)length[i];
//                    stripe.Point1 = new PointF((float)positionX1[i], (float)positionY1[i]);
//                    stripe.Point2 = new PointF((float)positionX2[i], (float)positionY2[i]);

//                    stripeList.Add(stripe);
//                }
//            }

//            return stripeList;
//        }
//    }
//}