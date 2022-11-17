using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarginMaesure
{
    public static class MarginMeasureProcess
    {
        public static void Measure(AlgoImage backLightImage, AlgoImage topLightImage, AlgoImage maskBuffer, AlgoImage sheetBuffer)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();

            float rotateAngle = 0;
            AlgoImage maskBufferRot = ImageBuilder.BuildSameTypeSize(maskBuffer);
            AlgoImage topLightRot = ImageBuilder.BuildSameTypeSize(topLightImage);

            // 각도 찾기
            {
                ip.FillHoles(maskBuffer, maskBufferRot);
                RotatedRect rotatedRect = GetBlobRects(maskBufferRot).First();
                if (rotatedRect.Angle > 0)
                    rotateAngle = 90 - rotatedRect.Angle;
                else
                    rotateAngle = -(90 + rotatedRect.Angle);
            }

            // 마스크 이미지 회전
            maskBufferRot.Clear(0);
            ip.Rotate(maskBuffer, maskBufferRot, rotateAngle);

            // 탑 이미지 회전
            topLightRot.Clear(0);
            ip.Rotate(topLightImage, topLightRot, rotateAngle);

            // 마스크 회전 이미지 블랍 -> 회전 후 블랍 좌표 구하기 위함.
            RectangleF[] blobRects = GetBlobRects(maskBufferRot).Select(f => f.GetBoundRect()).ToArray();
            RectangleF blobRect = blobRects.First();

            // W방향
            RectangleF[] lRects = Array.FindAll(blobRects, f => f.Right < blobRect.Left);
            RectangleF[] rRects = Array.FindAll(blobRects, f => f.Left > blobRect.Right);
            float w = DoW(topLightRot, maskBufferRot, blobRect, lRects, rRects);

            // L방향
            RectangleF[] tRects = Array.FindAll(blobRects, f => f.Top > blobRect.Bottom);
            RectangleF[] bRects = Array.FindAll(blobRects, f => f.Bottom < blobRect.Top);
            float h = DoL(topLightRot, maskBufferRot, blobRect, tRects, bRects);

            maskBufferRot.Dispose();
            topLightRot.Dispose();
        }


        private static float DoW(AlgoImage greyImage, AlgoImage binalImage, RectangleF rect, RectangleF[] leftRects, RectangleF[] rightRects)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            AlgoImage sobelImage = ImageBuilder.BuildSameTypeSize(greyImage);
            ip.Sobel(greyImage, sobelImage, Direction.Horizontal);

            greyImage.Save(@"D:\temp\greyImage.bmp");
            binalImage.Save(@"D:\temp\binalImage.bmp");
            sobelImage.Save(@"D:\temp\sobelImage.bmp");

            PointF centerR, centerL;
            PointF[] rightL, leftR;

            {
                // 중심 블랍의 오른쪽 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Right - 10, boundRect.Top, boundRect.Right + 10, boundRect.Bottom);

                centerR = GetPoint(binalImage, sobelImage, boundRectR, Direction.Horizontal);

                ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(centerR), Size.Empty), 3, 3), 255, true);
                greyImage.Save(@"D:\temp\greyImage1.bmp");
            }

            {
                // 오른쪽 블랍(들)의 왼쪽 면
                rightL = rightRects.Select(rightRect =>
                {
                    Rectangle boundRect = Rectangle.Round(rightRect);
                    Rectangle boundRectL = Rectangle.FromLTRB(boundRect.Left - 10, boundRect.Top, boundRect.Left + 10, boundRect.Bottom);

                    return GetPoint(binalImage, sobelImage, boundRectL, Direction.Horizontal);
                }).ToArray();

                Array.ForEach(rightL, f =>
                {
                    ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 3, 3), 255, false);
                    greyImage.Save(@"D:\temp\greyImage1.bmp");
                });
            }

            {
                // 중심 블랍의 왼쪽 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectL = Rectangle.FromLTRB(boundRect.Left - 10, boundRect.Top, boundRect.Left + 10, boundRect.Bottom);

                centerL = GetPoint(binalImage, sobelImage, boundRectL, Direction.Horizontal);

                ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(centerL), Size.Empty), 3, 3), 255, true);
                greyImage.Save(@"D:\temp\greyImage1.bmp");
            }

            {
                // 왼쪽 블랍(들)의 오른쪽 면
                leftR = leftRects.Select(leftRect =>
                {
                    Rectangle boundRect = Rectangle.Round(leftRect);
                    Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Right - 10, boundRect.Top, boundRect.Right + 10, boundRect.Bottom);

                    return GetPoint(binalImage, sobelImage, boundRectR, Direction.Horizontal);
                }).ToArray();

                Array.ForEach(leftR, f =>
                {
                    ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 3, 3), 255, false);
                    greyImage.Save(@"D:\temp\greyImage1.bmp");
                });
            }

            sobelImage.Dispose();

            float wR = float.MaxValue, wL = float.MaxValue;
            if (rightL.Length > 0)
                wR = rightL.Min(f => f.X) - centerR.X;
            if (leftR.Length > 0)
                wL = centerL.X - leftR.Min(f => f.X);

            return Math.Min(wR, wL);
        }

        private static float DoL(AlgoImage greyImage, AlgoImage binalImage, RectangleF rect, RectangleF[] topRects, RectangleF[] bottomRects)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            AlgoImage sobelImage = ImageBuilder.BuildSameTypeSize(greyImage);
            ip.Sobel(greyImage, sobelImage, Direction.Horizontal);

            greyImage.Save(@"D:\temp\greyImage.bmp");
            binalImage.Save(@"D:\temp\binalImage.bmp");
            sobelImage.Save(@"D:\temp\sobelImage.bmp");

            PointF centerT, centerB;
            PointF[] topBottom, bottomTop;
            {
                // 중심 블랍의 윗 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectT = Rectangle.FromLTRB(boundRect.Left, boundRect.Top - 10, boundRect.Right, boundRect.Top + 10);

                centerT = GetPoint(binalImage, sobelImage, boundRectT, Direction.Vertical);

                ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(centerT), Size.Empty), 3, 3), 255, true);
                greyImage.Save(@"D:\temp\greyImage1.bmp");
            }

            {
                // 윗 블랍(들)의 아래 면
                topBottom = topRects.Select(topRect =>
                {
                    Rectangle boundRect = Rectangle.Round(topRect);
                    Rectangle boundRectB = Rectangle.FromLTRB(boundRect.Left, boundRect.Bottom - 10, boundRect.Right, boundRect.Bottom + 10);

                    return GetPoint(binalImage, sobelImage, boundRectB, Direction.Vertical);
                }).ToArray();

                Array.ForEach(topBottom, f =>
                {
                    ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 3, 3), 255, false);
                    greyImage.Save(@"D:\temp\greyImage1.bmp");
                });
            }

            {
                // 중심 블랍의 아래 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectB = Rectangle.FromLTRB(boundRect.Left, boundRect.Bottom - 10, boundRect.Right, boundRect.Bottom + 10);

                centerB = GetPoint(binalImage, sobelImage, boundRectB, Direction.Vertical);

                ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(centerB), Size.Empty), 3, 3), 255, true);
                greyImage.Save(@"D:\temp\greyImage1.bmp");
            }

            {
                // 아래 블랍(들)의 윗 면
                bottomTop = bottomRects.Select(bottomRect =>
                {
                    Rectangle boundRect = Rectangle.Round(bottomRect);
                    Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Left, boundRect.Top - 10, boundRect.Right, boundRect.Top + 10);

                    return GetPoint(binalImage, sobelImage, boundRectR, Direction.Vertical);
                }).ToArray();


                Array.ForEach(bottomTop, f =>
                {
                    ip.DrawRect(greyImage, Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 3, 3), 255, false);
                    greyImage.Save(@"D:\temp\greyImage1.bmp");
                });
            }

            sobelImage.Dispose();

            float lT = float.MaxValue, lB = float.MaxValue;
            if (bottomTop.Length > 0)
                lT = bottomTop.Min(f => f.Y) - centerB.Y;
            if (topBottom.Length > 0)
                lB = centerT.Y - topBottom.Min(f => f.Y);

            return Math.Min(lT, lB);
        }

        private static PointF GetPoint(AlgoImage binalImage, AlgoImage sobleImage, Rectangle rectangle, Direction direction)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            PointF foundPoint = new PointF(-1, -1);

            rectangle.Intersect(new Rectangle(Point.Empty, binalImage.Size));
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return foundPoint;

            AlgoImage binalSubImage = binalImage.GetSubImage(rectangle);
            AlgoImage sobelSubImage = sobleImage.GetSubImage(rectangle);
            try
            {
                // 흰색(전극)이 가장 많은 부분을 찾음
                float[] proj = ip.Projection(binalSubImage, direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal, ProjectionType.Mean);
                float maxValue = proj.Max();
                int maxIndex = Array.IndexOf(proj, maxValue);
                Point pt = new Point
                    (
                    direction == Direction.Horizontal ? 10 : maxIndex,
                    direction == Direction.Vertical ? 10 : maxIndex);

                Rectangle cogRect = DrawingHelper.FromCenterSize(pt, new Size(10, 10));
                cogRect.Intersect(new Rectangle(Point.Empty, rectangle.Size));
                if (cogRect.Width == 0 || cogRect.Height == 0)
                    return foundPoint;

                using (AlgoImage sobelCogImage = sobelSubImage.GetSubImage(cogRect))
                {
                    sobelCogImage.Save(@"D:\temp\sobelCogImage.bmp");
                    float[] pp = ip.Projection(sobelCogImage, direction == Direction.Horizontal ? Direction.Horizontal : Direction.Vertical, ProjectionType.Mean);
                    float cog = pp.Select((f, i) => f * i).Sum() / pp.Sum();
                    foundPoint = new PointF(rectangle.Left + cogRect.Left + cog, rectangle.Top + pt.Y);
                }
            }
            finally
            {
                binalSubImage.Dispose();
                sobelSubImage.Dispose();
            }

            return foundPoint;
        }

        private static RotatedRect[] GetBlobRects(AlgoImage maskSubImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(maskSubImage);
            PointF imgCenterPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, maskSubImage.Size));
            maskSubImage.Save(@"D:\temp\maskSubImage.bmp");

            using (BlobRectList blobRectList = ip.Blob(maskSubImage, new BlobParam() { EraseBorderBlobs = false, SelectRotateRect = true }))
                return blobRectList.GetArray()
                    .Select(f => new RotatedRect(DrawingHelper.FromCenterSize(f.RotateCenterPt, new SizeF(f.RotateWidth, f.RotateHeight)), f.RotateAngle))
                    .OrderBy(f => MathHelper.GetLength(DrawingHelper.CenterPoint(f), imgCenterPt))
                    .ToArray();
        }
    }
}
