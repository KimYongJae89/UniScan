using DynMvp.Vision.OpenCv;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision.OpenCv.Object
{
    class OpenCvBlobRectList : BlobRectList
    {
        public CvBlobDetector BlobDetector => this.blobDetector;
        CvBlobDetector blobDetector;

        public CvBlobs CvBlobs => this.cvBlobs;
        CvBlobs cvBlobs;

        public OpenCvBlobRectList()
        {
            this.blobDetector = new CvBlobDetector();
            this.cvBlobs = new CvBlobs();
        }

        public OpenCvBlobRectList(CvBlobDetector blobDetector, CvBlobs cvBlobs)
        {
            this.blobDetector = blobDetector;
            this.cvBlobs = cvBlobs;
        }

        ~OpenCvBlobRectList()
        {
            Dispose();
        }

        public override void Dispose()
        {
            this.blobDetector?.Dispose();
            this.blobDetector = null;

            this.cvBlobs?.Dispose();
            this.cvBlobs = null;
        }

        internal void UpdateBlobRects(BlobParam blobParam, AlgoImage greyMask)
        {
            OpenCvGreyImage openCvGreyImage = greyMask as OpenCvGreyImage;
            Image<Gray, byte> blobMask = null;
            if (openCvGreyImage != null)
            {
                //blobMask = new Image<Gray, byte>(openCvGreyImage.Width, openCvGreyImage.Height);
                //blobMask.SetZero();
            }

            if (blobParam.SelectArea)
            {
                int min = (int)blobParam.AreaMin;
                int max = (int)blobParam.AreaMax;
                if (max == 0)
                    max = int.MaxValue;
                this.cvBlobs.FilterByArea(min, max);
            }

            CvBlob[] cvBlobs = this.cvBlobs.Values.ToArray();
            foreach (CvBlob cvBlob in cvBlobs)
            {
                BlobRect blobRect = new BlobRect();

                blobRect.LabelNumber = (int)cvBlob.Label;
                blobRect.Area = cvBlob.Area;
                blobRect.BoundingRect = cvBlob.BoundingBox;
                blobRect.CenterPt = cvBlob.Centroid;
                blobRect.CalcCenterOffset();

                Point[] vertexs = cvBlob.GetContour();
                blobRect.RotateXArray = vertexs.Select(f => (float)f.X).ToArray();
                blobRect.RotateYArray = vertexs.Select(f => (float)f.Y).ToArray();

                RotatedRect rRect = CvInvoke.MinAreaRect(vertexs.Select(f => new PointF(f.X, f.Y)).ToArray());
                if (rRect.Size.Width > rRect.Size.Height)
                {
                    blobRect.RotateAngle = rRect.Angle;
                    blobRect.RotateWidth = rRect.Size.Width;
                    blobRect.RotateHeight = rRect.Size.Height;
                }
                else
                {
                    blobRect.RotateAngle = (rRect.Angle + 90) % 360;
                    blobRect.RotateWidth = rRect.Size.Height;
                    blobRect.RotateHeight = rRect.Size.Width;
                }
                //blobRect.RotateWidth = 

                if (blobMask != null)
                {
                    //double minValue = 0, maxValue = 0, menaValue = 0;
                    //blobMask.SetZero();
                    //CvInvoke.DrawContours(blobMask, vertexs.ToList(), 0, new MCvScalar(255));
                    ////blobMask.DrawPolyline(vertexs, false, new Gray(255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    ////blobMask.DrawPolyline(vertexs.Reverse().ToArray(), false, new Gray(255), -1, Emgu.CV.CvEnum.LineType.Filled);
                    //blobMask.Save(@"C:\temp\blobMask.bmp");

                    //Point minLoc = Point.Empty, maxLoc = Point.Empty;
                    //CvInvoke.MinMaxLoc(openCvGreyImage.Image, ref minValue, ref maxValue, ref minLoc, ref maxLoc, blobMask);
                    //menaValue = CvInvoke.Mean(openCvGreyImage.Image, blobMask).V0;
                    
                    //blobRect.MeanValue = (float)menaValue;
                    //blobRect.MaxValue = (float)maxValue;
                    //blobRect.MinValue = (float)minValue;

                    //blobMask.SetZero();
                }
                this.Append(blobRect);
            }

            blobMask?.Dispose();
        }
    }
}
