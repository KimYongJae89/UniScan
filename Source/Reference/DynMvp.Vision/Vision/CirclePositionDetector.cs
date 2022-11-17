using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.UI;
using DynMvp.Base;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;

using DynMvp.Vision.OpenCv;

namespace DynMvp.Vision
{
    public class CirclePositionDetectorParam
    {
        private float innerRadius;
        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        private float outterRadius;
        public float OutterRadius
        {
            get { return outterRadius; }
            set { outterRadius = value; }
        }
        
        private float accumulate;
        public float Accumulate
        {
            get { return accumulate; }
            set { accumulate = value; }
        }

        private int closing;
        public int Closing
        {
            get { return closing; }
            set { closing = value; }
        }

        private float centerDistance;
        public float CenterDistance
        {
            get { return centerDistance; }
            set { centerDistance = value; }
        }
        
        public CirclePositionDetectorParam()
        {
            innerRadius = 0;
            outterRadius = 0;
            accumulate = 10;
            closing = 1;
            centerDistance = 30;
        }

        public void Copy(CirclePositionDetectorParam param)
        {
            innerRadius = param.innerRadius;
            outterRadius = param.outterRadius;
            accumulate = param.accumulate;
            closing = param.closing;
            centerDistance = param.centerDistance;
        }

        public void LoadParam(XmlElement paramElement)
        {
            innerRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "InnerRadius", "0"));
            outterRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "OutterRadius", "0"));
            accumulate = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Accumulate", "10"));
            closing = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Closing", "3"));
            centerDistance = Convert.ToSingle(XmlHelper.GetValue(paramElement, "centerDistance", "30"));
            
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "InnerRadius", innerRadius.ToString());
            XmlHelper.SetValue(paramElement, "OutterRadius", outterRadius.ToString());
            XmlHelper.SetValue(paramElement, "Accumulate", accumulate.ToString());
            XmlHelper.SetValue(paramElement, "Closing", closing.ToString());
            XmlHelper.SetValue(paramElement, "centerDistance", centerDistance.ToString());
        }
    }

    public class CirclePositionDetector
    {
        CirclePositionDetectorParam param = new CirclePositionDetectorParam();
        public CirclePositionDetectorParam Param
        {
            get { return param; }
            set { param = value; }
        }

        public CirclePositionDetector()
        {
        }

        public static string TypeName
        {
            get { return "CirclePositionDetector"; }
        }

        public CircleEq Detect(AlgoImage probeClipImage, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(probeClipImage);

            imageProcessing.Binarize(probeClipImage, false);

            imageProcessing.Dilate(probeClipImage, Param.Closing);
            imageProcessing.Erode(probeClipImage, Param.Closing);

            AlgoImage roiImage = probeClipImage.Clone();

            CircleEq resultCircle = null;
            List<CircleEq> circles = new List<CircleEq>();

            circles = getMinEnclosingCircle(roiImage, debugContext);

            Point circleOffset = new Point();
            bool finded = false;

            if (circles != null && circles.Count != 0)
            {
                resultCircle = circles.OrderByDescending(x => x.Radius).First();
                circles.Remove(resultCircle);

                foreach (CircleEq circle in circles)
                {
                    if (Math.Sqrt(Math.Pow(resultCircle.Center.X - circle.Center.X, 2) + Math.Pow(resultCircle.Center.Y - circle.Center.Y, 2)) < Param.CenterDistance)
                    {
                        resultCircle = new CircleEq(new PointF((resultCircle.Center.X + circle.Center.X) / 2, (resultCircle.Center.Y + circle.Center.Y) / 2), (resultCircle.Radius + circle.Radius) / 2);
                        finded = true;
                    }
                }
            }

            if (finded == false)
            {
                if (resultCircle != null)
                {
                    int margin = 10;

                    circleOffset = new Point((int)(resultCircle.Center.X - resultCircle.Radius - margin), (int)(resultCircle.Center.Y - resultCircle.Radius - margin));
                    Size size = new Size((int)((resultCircle.Radius + margin) * 2), (int)((resultCircle.Radius + margin) * 2));

                    if (circleOffset.X < 0)
                        circleOffset.X = 0;
                    if (circleOffset.Y < 0)
                        circleOffset.Y = 0;
                    if (circleOffset.X + size.Width > probeClipImage.Width)
                        size.Width -= ((circleOffset.X + size.Width) - probeClipImage.Width);
                    if (circleOffset.Y + size.Height > probeClipImage.Height)
                        size.Height -= ((circleOffset.Y + size.Height) - probeClipImage.Height);

                    roiImage = probeClipImage.Clip(new Rectangle(circleOffset, size));
                }
                else
                    roiImage = probeClipImage.Clone();

                resultCircle = getHoughCircle(roiImage, circleOffset, resultCircle, debugContext);
            }

            return resultCircle;
        }

        List<CircleEq> getMinEnclosingCircle(AlgoImage roiImage, DebugContext debugContext)
        {
            return null;
            //OpenCvGreyImage openCvImage = roiImage as OpenCvGreyImage;

            //Contour<Point> contours = openCvImage.Image.FindContours();

            //if (contours == null)
            //    return null;

            //PointF centerPoint;
            //float radius;

            //CircleEq circle;
            //List<CircleEq> circles = new List<CircleEq>();

            //while (contours.HNext != null)
            //{
            //    Emgu.CV.CvInvoke.cvMinEnclosingCircle(contours, out centerPoint, out radius);

            //    contours = contours.HNext;

            //    if (radius > param.OutterRadius || radius < param.InnerRadius)
            //        continue;

            //    circle = new CircleEq(centerPoint, radius);
            //    circles.Add(circle);
            //}

            //roiImage.Save("Circle.bmp", debugContext);

            //return circles;
        }

        CircleEq getHoughCircle(AlgoImage roiImage, Point offset, CircleEq resultCircle, DebugContext debugContext)
        {
            OpenCvGreyImage openCvImage = roiImage as OpenCvGreyImage;

            CircleF[][] Circles = openCvImage.Image.HoughCircles(new Gray(200), new Gray(Param.Accumulate), 1, (int)Param.CenterDistance, (int)param.InnerRadius, (int)param.OutterRadius);

            List<CircleF> findedCircles = new List<CircleF>();
            
            List<PointF> subCenters = new List<PointF>();
            List<float> subRadius = new List<float>();
            List<int> subCircleNum = new List<int>();

            if (resultCircle != null)
            {
                subCircleNum.Add(1);

                subCenters.Add(resultCircle.Center);
                subRadius.Add(resultCircle.Radius);
            }

            foreach (CircleF circle in Circles[0])
            {
                bool selected = false;
                PointF selectCenter = new PointF();
                float prevDistance = Param.CenterDistance;

                foreach (PointF center in subCenters)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(circle.Center.X - center.X, 2) + Math.Pow(circle.Center.Y - center.Y, 2));

                    if (distance < prevDistance)
                    {
                        prevDistance = distance;
                        selectCenter = center;
                        selected = true;
                    }
                }

                if (selected == false)
                {
                    subCircleNum.Add(1);

                    subCenters.Add(circle.Center);
                    subRadius.Add(circle.Radius);
                }
                else
                {
                    int index = subCenters.IndexOf(selectCenter);

                    subCenters[index] = new PointF(((subCenters[index].X * subCircleNum[index]) + circle.Center.X) / (subCircleNum[index] + 1),
                                                    ((subCenters[index].Y * subCircleNum[index]) + circle.Center.Y) / (subCircleNum[index] + 1));
                    subRadius[index] = ((subRadius[index] * subCircleNum[index]) + circle.Radius) / (subCircleNum[index] + 1);

                    subCircleNum[index]++;
                }
            }

            if (subCircleNum.Count == 0)
                return null;

            int resultIndex = 0;
            int prevCircleNum = 0;

            for (int index = 0; index < subCircleNum.Count; index++)
            {
                if (subCircleNum[index] > prevCircleNum)
                {
                    prevCircleNum = subCircleNum[index];
                    resultIndex = index;
                }
            }

            if (prevCircleNum < 5)
                return null;

            PointF resultCenter = new PointF(subCenters[resultIndex].X + offset.X, subCenters[resultIndex].Y + offset.Y);

            return new CircleEq(resultCenter, subRadius[resultIndex]);
        }
    }
}