using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Xml;
using DynMvp.Base;
using System.IO;
using DynMvp.Devices.MotionController;

namespace DynMvp.Device
{
    public class RobotAligner
    {
        int width = 0;
        int height = 0;

        PointF[,] refPoints = null;
        public PointF[,] RefPoints
        { get { return refPoints; } }

        PointF[,] realPoints = null;
        public PointF[,] RealPoints
        { get { return realPoints; } }

        PointF[,] offset = null;
        public PointF[,] Offset
        { get { return offset; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPoint"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Calculate(PointF[,] refPoint, PointF[,] offset)
        {
            width = refPoint.GetLength(0);
            height = refPoint.GetLength(1);

            this.refPoints = refPoint;
            this.realPoints = new PointF[width, height];
            this.offset = offset;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    realPoints[x, y] = PointF.Add(refPoint[x, y], new SizeF(offset[x, y]));
                }
            }

            return true;
        }

        public PointF InverseAlign(PointF point)
        {
            //width = refPoints.GetLength(0);
            //height = refPoints.GetLength(1);

            if (width == 0 || height == 0)
                return point;

            // 입력받은 점은 Real 좌표계이다.
            try
            {
                int x, y;
                // 입력받은 점을 둘러싸는 Real 좌표계 4개 점 인덱스 확인
                PointF realLT = GetLeftTopRealPoint(point, out x, out y);
                PointF realLB = realPoints[x, y + 1];
                PointF realRB = realPoints[x + 1, y + 1];
                PointF realRT = realPoints[x + 1, y];

                // Real 좌표계 4개 점과 매칭되는 점 확인
                PointF refLT = refPoints[x, y];
                PointF refLB = refPoints[x, y + 1];
                PointF refRB = refPoints[x + 1, y + 1];
                PointF refRT = refPoints[x + 1, y];

                // OpenCV로 Affine 변환
                float[,] sourcePoints = { { refLT.X, refLT.Y }, { refLB.X, refLB.Y }, { refRB.X, refRB.Y }, { refRT.X, refRT.Y } };
                float[,] destPoints = { { realLT.X, realLT.Y }, { realLB.X, realLB.Y }, { realRB.X, realRB.Y }, { realRT.X, realRT.Y } };

                Matrix<float> sourceMat = new Matrix<float>(sourcePoints);
                Matrix<float> destMat = new Matrix<float>(destPoints);
                Matrix<float> homogMat = new Matrix<float>(3, 3);
                Matrix<float> invertHomogMat = new Matrix<float>(3, 3);
                MCvScalar s = new MCvScalar(0, 0, 0);

                Mat tempHomogMat = CvInvoke.FindHomography(sourceMat, destMat, Emgu.CV.CvEnum.HomographyMethod.Default, 3.0);
                CvInvoke.Invert(tempHomogMat, invertHomogMat, Emgu.CV.CvEnum.DecompMethod.LU);

                float[,] sourcePoints2 = { { point.X, point.Y, 1 } };
                Emgu.CV.Matrix<float> sourceMat2 = new Matrix<float>(sourcePoints2);
                Emgu.CV.Matrix<float> resultMat = invertHomogMat.Mul(sourceMat2.Transpose()).Transpose();

                float scale = resultMat[0, 2];
                PointF alignedPoint = new PointF(resultMat[0, 0] / scale, resultMat[0, 1] / scale);
                return alignedPoint;
            }
            catch
            {
                return point;
            }
        }

        public PointF Align(PointF point)
        {
            //width = refPoints.GetLength(0);
            //height = refPoints.GetLength(1);

            if (width == 0 || height == 0)
                return point;

            // 입력받은 점은 Ref 좌표계이다.
            try
            {
                int x, y;
                // 입력받은 점을 둘러싸는 Ref 좌표계 4개 점 인덱스 확인
                PointF refLT = GetLeftTopRefPoint(point, out x, out y);
                PointF refLB = refPoints[x, y + 1];
                PointF refRB = refPoints[x + 1, y + 1];
                PointF refRT = refPoints[x + 1, y];

                // Ref 좌표계 4개 점과 매칭되는 실제 점 확인
                PointF realLT = realPoints[x, y];
                PointF realLB = realPoints[x, y + 1];
                PointF realRB = realPoints[x + 1, y + 1];
                PointF realRT = realPoints[x + 1, y];

                // OpenCV로 Affine 변환
                float[,] sourcePoints = { { refLT.X, refLT.Y }, { refLB.X, refLB.Y }, { refRB.X, refRB.Y }, { refRT.X, refRT.Y } };
                float[,] destPoints = { { realLT.X, realLT.Y }, { realLB.X, realLB.Y }, { realRB.X, realRB.Y }, { realRT.X, realRT.Y } };

                Matrix<float> sourceMat = new Matrix<float>(sourcePoints);
                Matrix<float> destMat = new Matrix<float>(destPoints);
                // Matrix<float> homogMat = new Matrix<float>(3, 3);
                Matrix<float> invertHomogMat = new Matrix<float>(3, 3);
                IntPtr maskMat = new IntPtr();
                MCvScalar s = new MCvScalar(0, 0, 0);

                Mat tempHomogMat = CvInvoke.FindHomography(sourceMat, destMat);
                Matrix<float> homogMat = new Matrix<float>(tempHomogMat.Rows, tempHomogMat.Cols);
                tempHomogMat.CopyTo(homogMat);

                float[,] sourcePoints2 = { { point.X, point.Y, 1 } };
                Emgu.CV.Matrix<float> sourceMat2 = new Matrix<float>(sourcePoints2);
                Emgu.CV.Matrix<float> resultMat = homogMat.Mul(sourceMat2.Transpose()).Transpose();

                float scale = resultMat[0, 2];
                PointF alignedPoint = new PointF(resultMat[0, 0] / scale, resultMat[0, 1] / scale);
                return alignedPoint;
            }
            catch
            {
                return point;
            }
        }

        private PointF GetLeftTopRefPoint(PointF point, out int x, out int y)
        {
            return GetLeftTopPoint(point, true, out x, out y);
        }

        private PointF GetLeftTopRealPoint(PointF point, out int x, out int y)
        {
            return GetLeftTopPoint(point, false, out x, out y);
        }

        private PointF GetLeftTopPoint(PointF point, bool isReference, out int x, out int y)
        {
            PointF[,] points = isReference ? refPoints : realPoints;

            PointF foundPoint = Point.Empty;
            float minLen = float.MaxValue;
            x = -1;
            y = -1;
            for (int w=0; w< points.GetLength(0);w++)
            {
                for (int h = 0; h < points.GetLength(1); h++)
                {
                    PointF refPoint = points[w, h];

                    if (refPoint.X <= point.X && refPoint.Y <= point.Y)
                    {
                        float dx = refPoint.X - point.X;
                        float dy = refPoint.Y - point.Y;
                        float len = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (minLen >= len)
                        {
                            foundPoint = refPoint;
                            minLen = len;
                            x = w;
                            y = h;
                        }
                    }
                }
            }

            if (x == -1)
            {
                x = 0;
            }

            if (y == -1)
            {
                y = 0;
            }

            if (x >= refPoints.GetLength(0) - 1)
            {
                x = refPoints.GetLength(0) - 2;
            }

            if (y >= refPoints.GetLength(1) - 1)
            {
                y = refPoints.GetLength(1) - 2;
            }

            return points[x,y];
        }

        public void Save(string configPath)
        {
            if (Directory.Exists(configPath) == false)
                return;

            string configFile = string.Format(@"{0}\RobotAligner.xml", configPath);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement robotAlignerElement = xmlDocument.CreateElement("", "RobotAligner", "");
            xmlDocument.AppendChild(robotAlignerElement);

            Save(robotAlignerElement);

            xmlDocument.Save(configFile);
        }

        public void Clear()
        {
            width = 0;
            height = 0;
            refPoints = null;
            this.realPoints = null;
        }

        private void Save(XmlElement robotAlignerElement)
        {
            XmlElement referencePosElement = robotAlignerElement.OwnerDocument.CreateElement("", "ReferencePosition", "");
            robotAlignerElement.AppendChild(referencePosElement);
            Save(referencePosElement, refPoints);

            XmlElement realPositionElement = robotAlignerElement.OwnerDocument.CreateElement("", "RealPosition", "");
            robotAlignerElement.AppendChild(realPositionElement);
            Save(realPositionElement, this.realPoints);

            XmlElement offsetElement = robotAlignerElement.OwnerDocument.CreateElement("", "Offset", "");
            robotAlignerElement.AppendChild(offsetElement);
            Save(offsetElement, this.Offset);
        }

        private void Save(XmlElement element, PointF[,] points)
        {
            int width = points.GetLength(0);
            int heigth = points.GetLength(1);
            XmlHelper.SetValue(element, "Width", width.ToString());
            XmlHelper.SetValue(element, "Height", height.ToString());

            for (int y = 0; y < points.GetLength(1); y++)
            {
                for (int x = 0; x < points.GetLength(0); x++)
                {
                    XmlElement subElement = element.OwnerDocument.CreateElement("", "Point", "");
                    element.AppendChild(subElement);

                    XmlHelper.SetValue(subElement, "X", x.ToString());
                    XmlHelper.SetValue(subElement, "Y", y.ToString());
                    XmlHelper.SetValue(subElement, "PosX", points[x, y].X.ToString());
                    XmlHelper.SetValue(subElement, "PosY", points[x, y].Y.ToString());
                }
            }
        }

        public bool Load(string configPath)
        {
            string configFile = string.Format(@"{0}\RobotAligner.xml", configPath);
            if (File.Exists(configFile) == false)
                return false;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(configFile);

            Load(xmlDocument.DocumentElement);

            width = refPoints.GetLength(0);
            height = refPoints.GetLength(1);

            return true;
        }

        private void Load(XmlElement documentElement)
        {
            XmlElement referencePosElement = documentElement["ReferencePosition"];
            if (referencePosElement != null)
            {
                Load(referencePosElement,ref refPoints);
            }

            XmlElement realPositionElement = documentElement["RealPosition"];
            if (realPositionElement != null)
            {
                Load(realPositionElement, ref realPoints);
            }

            XmlElement offsetElement = documentElement["Offset"];
            if (offsetElement != null)
            {
                Load(offsetElement,ref offset);
            }
        }

        private void Load(XmlElement element,ref PointF[,] points)
        {
            int width = int.Parse(XmlHelper.GetValue(element, "Width", "0"));
            int height = int.Parse(XmlHelper.GetValue(element, "Height", "0"));

            points = new PointF[width, height];

            foreach(XmlElement subElement in element)
            {
                if(subElement.Name == "Point")
                {
                    int x = int.Parse(XmlHelper.GetValue(subElement, "X", "0"));
                    int y = int.Parse(XmlHelper.GetValue(subElement, "Y", "0"));
                    float posX = float.Parse(XmlHelper.GetValue(subElement, "PosX", "0"));
                    float posY = float.Parse(XmlHelper.GetValue(subElement, "PosY", "0"));

                    points[x, y] = new PointF(posX, posY);
                }
            }

        }

        public AxisPosition Align(AxisPosition position)
        {
            if (position.NumAxis != 2)
                return position;

            PointF alignedPos = Align(new PointF(position[0], position[1]));
            return new AxisPosition(alignedPos.X, alignedPos.Y);
        }

        public AxisPosition InverseAlign(AxisPosition position)
        {
            if (position.NumAxis != 2)
                return position;

            PointF refPos = InverseAlign(new PointF(position[0], position[1]));
            return new AxisPosition(refPos.X, refPos.Y);
        }
    }
}
