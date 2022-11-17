using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvCircleDetector : CircleDetector
    {
        //public override CircleEq Detect(AlgoImage algoImage, DebugContext debugContext)
        //{
        //    OpenCvGreyImage openCvImage = algoImage as OpenCvGreyImage;

        //    CircleF[][] Circles = openCvImage.Image.HoughCircles(new Gray(200), new Gray(10), 1, (int)Param.OutterRadius, (int)Param.InnerRadius, (int)Param.OutterRadius);

        //    foreach (CircleF circle in Circles[0])
        //    {
        //        CircleEq circleEq = new CircleEq();

        //        circleEq.Center = new PointF((float)circle.Center.X, (float)circle.Center.Y);
        //        circleEq.Radius = (float)circle.Radius;

        //        return circleEq;
        //    }

        //    return null;
        //}

        public override CircleEq Detect(AlgoImage algoImage, DebugContext debugContext)
        {
            //algoImage.Save("Input.bmp", debugContext);

            //ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            //imageProcessing.Binarize(algoImage);

            //imageProcessing.Dilate(algoImage, 1);
            //imageProcessing.Erode(algoImage, 1);

            //algoImage.Save("Processing.bmp", debugContext);

            //OpenCvGreyImage openCvImage = algoImage as OpenCvGreyImage;

            //Contour<Point> contours = openCvImage.Image.FindContours();

            //if (contours == null)
            //    return null;

            //PointF centerPoint;
            //float radius;

            //while (contours.HNext != null)
            //{
            //    Emgu.CV.CvInvoke.cvMinEnclosingCircle(contours, out centerPoint, out radius);

            //    contours = contours.HNext;

            //    if (radius > Param.OutterRadius || radius < Param.InnerRadius)
            //        continue;

            //    return new CircleEq(centerPoint, radius);
            //}

            return null;
        }
    }
}
