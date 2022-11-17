using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniScanWPF.Table.Inspect;

namespace UniScanWPF.Table.Algorithm
{
    public static class MarginMeasureAlgorhtm
    {
        public static Tuple<RectangleF, float[], BitmapSource> Measure(AlgoImage topLightImage, AlgoImage maskBuffer, MarginMeasurePos marginMeasurePos, DebugContext debugContext)
        {
            Tuple<RectangleF, float[], BitmapSource> result = null;
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            Size imageSize = topLightImage.Size;

            Operation.Operators.InspectOperatorSettings settings = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
            MarginMeasureParam marginMeasureParam = settings.MarginMeasureParam;
            if (marginMeasurePos.OverrideParam)
                marginMeasureParam = marginMeasurePos.MeasureParam;

            if (debugContext == null)
                debugContext = new DebugContext(true, @"C:\temp");

            topLightImage.Save(@"topLightImage.bmp", debugContext);

            if (marginMeasureParam.Threshold > 0)
                ip.Binarize(topLightImage, maskBuffer, marginMeasureParam.Threshold, true); // 전극을 하얗게. 성형을 검게.
            else
                ip.Binarize(topLightImage, maskBuffer, true); // 전극을 하얗게. 성형을 검게.

            maskBuffer.Save(@"maskBuffer0.bmp", debugContext);

            RectangleF measureCenterRect;
            float targetAngle = -1;
            float rotateAngle = 0;

            // 각도 찾기
            {
                //ip.Erode(maskBuffer, 1);
                ip.FillHoles(maskBuffer, maskBuffer);
                maskBuffer.Save(@"maskBuffer1.bmp", debugContext);
                RotatedRect rotatedRect = GetBlobRects(maskBuffer).First();
                measureCenterRect = rotatedRect.GetBoundRect();

                float angle = rotatedRect.Angle;

                if (Math.Min(Math.Abs(angle - 90), Math.Abs(angle +90)) < 10)
                    targetAngle = 90;
                else if (Math.Min(Math.Abs(angle - 0), Math.Abs(angle - 180)) < 10)
                    targetAngle = 0;

                if (targetAngle >= 0)
                {
                    if (rotatedRect.Angle > 0)
                        rotateAngle = targetAngle - angle;
                    else
                        rotateAngle = -(targetAngle + angle);
                }
            }

            // 탑 이미지 회전
            AlgoImage topLightRot = ImageBuilder.BuildSameTypeSize(topLightImage);
            topLightRot.Clear(0);
            ip.Rotate(topLightImage, topLightRot, rotateAngle);
            topLightRot.Save(@"topLightRot.bmp", debugContext);

            // 마스크 이미지 회전
            AlgoImage maskBufferRot = ImageBuilder.BuildSameTypeSize(topLightRot);
            if (marginMeasureParam.Threshold> 0)
                ip.Binarize(topLightRot, maskBufferRot, marginMeasureParam.Threshold, true);
            else
                ip.Binarize(topLightRot, maskBufferRot, true);
            ip.Close(maskBufferRot, 2);
            ip.FillHoles(maskBufferRot, maskBufferRot);
            ip.Erode(maskBufferRot, 2);
            maskBufferRot.Save(@"maskBufferRot.bmp", debugContext);

            AlgoImage drawImage = topLightRot.ConvertTo(ImageType.Color);

            if (marginMeasurePos.SubPosCollection.Count == 0)
            {
                //Rectangle inspRect = Rectangle.Inflate(new Rectangle(Point.Empty, topLightRot.Size), -topLightRot.Width / 20, -topLightRot.Height / 20);
                int inspRectInflate = Math.Min(topLightRot.Width / 20, topLightRot.Height / 20);
                Rectangle inspRect = Rectangle.Inflate(new Rectangle(Point.Empty, topLightRot.Size), -inspRectInflate, -inspRectInflate);

                AlgoImage topLightRotIn = topLightRot.Clip(inspRect);
                AlgoImage maskBufferRotIn = maskBufferRot.Clip(inspRect);
                AlgoImage drawImageIn = drawImage.GetSubImage(inspRect);
                ip.DrawRect(drawImage, inspRect, Color.Yellow.ToArgb(), false);

                Point imageCenter = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, imageSize));
                // 마스크 회전 이미지 블랍 -> 회전 후 블랍 좌표 구하기 위함.
                //BlobRectList blobRectList = Blob(maskBufferRotIn, SizeF.Empty);
                RectangleF[] blobRects = GetBlobRects(maskBufferRotIn).Select(f => f.GetBoundRect())
                    .OrderBy(f=>MathHelper.GetLength(imageCenter, DrawingHelper.CenterPoint(f))).ToArray();
                RectangleF blobRect = blobRects.First();

                float[] w = new float[2] { float.NaN, float.NaN };
                if (marginMeasurePos.UseW)
                {
                    // W방향
                    RectangleF maskRect = RectangleF.Intersect(inspRect, RectangleF.FromLTRB(0, blobRect.Top, float.MaxValue, blobRect.Bottom));
                    List<RectangleF> horRectList = blobRects.Select(f => RectangleF.Intersect(f, maskRect)).ToList();
                    horRectList.RemoveAll(f => f.IsEmpty);

                    RectangleF[] lRects = new RectangleF[] { horRectList.ToDictionary(f => f, f => blobRect.Left - f.Right).OrderBy(f => f.Value).FirstOrDefault(f => f.Value > 0).Key };
                    RectangleF[] rRects = new RectangleF[] { horRectList.ToDictionary(f => f, f => f.Left - blobRect.Right).OrderBy(f => f.Value).FirstOrDefault(f => f.Value > 0).Key };

                    DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "W"));
                    w = DoW(topLightRotIn, maskBufferRotIn, blobRect, lRects, rRects, drawImageIn, newDebugContext);  // left, right
                }

                float[] l = new float[2] { float.NaN , float.NaN };
                if (marginMeasurePos.UseL)
                {
                    // L방향
                    RectangleF maskRect = RectangleF.Intersect(inspRect, RectangleF.FromLTRB(blobRect.Left, 0, blobRect.Right, float.MaxValue));
                    List<RectangleF> verRectList = blobRects.Select(f => RectangleF.Intersect(f, maskRect)).ToList();
                    verRectList.RemoveAll(f => f.IsEmpty);

                    RectangleF[] tRects = new RectangleF[] { verRectList.ToDictionary(f => f, f => blobRect.Top - f.Bottom).OrderBy(f => f.Value).FirstOrDefault(f => f.Value > 0).Key };
                    RectangleF[] bRects = new RectangleF[] { verRectList.ToDictionary(f => f, f => f.Top - blobRect.Bottom).OrderBy(f => f.Value).FirstOrDefault(f => f.Value > 0).Key };

                    //RectangleF[] tRects = new RectangleF[] { verRectList.OrderBy(f => blobRect.Top - f.Bottom).FirstOrDefault(f => blobRect.Top - f.Bottom > 0) };
                    //RectangleF[] bRects = new RectangleF[] { verRectList.OrderBy(f => f.Top - blobRect.Bottom).FirstOrDefault(f => f.Top - blobRect.Bottom > 0) };

                    DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "L"));
                    l = DoL(topLightRotIn, maskBufferRotIn, blobRect, tRects, bRects, drawImageIn, newDebugContext);  // top, bottom
                }
                topLightRotIn?.Dispose();
                maskBufferRotIn?.Dispose();
                drawImageIn?.Dispose();
                result = new Tuple<RectangleF, float[], BitmapSource>(measureCenterRect, new float[] { w[0], l[0], w[1], l[1] }, drawImage.ToBitmapSource());
            }
            else
            {
                // 마스크 회전 이미지 블랍 -> 회전 후 블랍 좌표 구하기 위함.
                // 정상이라면 이 블랍의 센터는 이미지의 중심이어야 함.
                //Point imageCenter = new Point(imageSize.Width / 2, imageSize.Height / 2);
                //RectangleF[] rects = GetBlobRects(maskBufferRot).Select(f => f.GetBoundRect()).ToArray();
                //PointF centerPt = DrawingHelper.CenterPoint(rects.First());
                //PointF offset = DrawingHelper.Subtract(centerPt, imageCenter);

                List<MarginMeasureSubPos> subPosList = marginMeasurePos.SubPosCollection.ToList();
                float w = float.NaN, l = float.NaN;
                AlgoImage sobelImage = ImageBuilder.BuildSameTypeSize(topLightRot);
                for (int i = 0; i < subPosList.Count; i++)
                {
                    MarginMeasureSubPos g = subPosList[i];
                    RectangleF maskRect = g.Rectangle;
                    //maskRect.Offset(offset);
                    if (maskRect.Width > 15 && maskRect.Height > 15)
                    {
                        Rectangle clipRect = Rectangle.Round(maskRect);

                        List<RectangleF> validRectList;
                        BlobRectList blobRectList;
                        using (AlgoImage clipMask = maskBufferRot.GetSubImage(clipRect))
                        {
                            clipMask.Save(@"clipMask.bmp", debugContext);
                            SizeF minSize = new SizeF(clipRect.Width / 4, clipRect.Height / 4);
                            blobRectList = Blob(clipMask, minSize);

                            RectangleF[] blobRects = GetBlobRects(clipMask).Select(f => f.GetBoundRect()).ToArray();
                            validRectList = blobRects.Select(f => DrawingHelper.Offset(f, clipRect.Location)).ToList();
                            validRectList.RemoveAll(f => f.Width < clipRect.Width / 4 || f.Height < clipRect.Height / 4);
                        }

                        ip.DrawRect(drawImage, clipRect, Color.Yellow.ToArgb(), false);
                        if (g.UseL)
                        {
                            if (float.IsNaN(l))
                                l = 0;

                            ip.Sobel(topLightRot, sobelImage, Direction.Horizontal);
                            //sobelImage.Save(@"C:\temp\sobelImage.bmp");

                            float centerY = (maskRect.Top + maskRect.Bottom) / 2;
                            if (validRectList.Count > 0)
                                centerY = validRectList.Average(h => (h.Top + h.Bottom) / 2);
                            List<RectangleF> tRectList = validRectList.FindAll(f => f.Bottom < centerY + maskRect.Height / 4); // 1/4선보다 위에있으면 Top으로
                            List<RectangleF> bRectList = validRectList.FindAll(f => f.Top > centerY - maskRect.Height / 4); // 3/4선보다 아래에 있으면 Bottom으로
                            List<RectangleF> aRectList = validRectList.FindAll(f => tRectList.Contains(f) && bRectList.Contains(f)); // top과 bottom에 모두 속하면 무시
                            RectangleF[] tRects = tRectList.FindAll(f => !aRectList.Contains(f)).ToArray();
                            RectangleF[] bRects = bRectList.FindAll(f => !aRectList.Contains(f)).ToArray();                            
                            if (tRects.Length > 0 && tRects.Length > 0)
                            {
                                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "L"));

                                PointF[] ptTop = tRects.Select(h => GetPoint(maskBufferRot, sobelImage, Rectangle.Round(RectangleF.FromLTRB(h.Left, h.Bottom - 10, h.Right, h.Bottom + 10)), Direction.Vertical)).ToArray();
                                Array.ForEach(ptTop, d => ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(d, new SizeF(5, 5))), Color.Red.ToArgb(), false));
                                //drawImage.Save(@"C:\temp\darwImage.bmp");

                                PointF[] ptBot = bRects.Select(h => GetPoint(maskBufferRot, sobelImage, Rectangle.Round(RectangleF.FromLTRB(h.Left, h.Top - 10, h.Right, h.Top + 10)), Direction.Vertical)).ToArray();
                                Array.ForEach(ptBot, d => ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(d, new SizeF(5, 5))), Color.Red.ToArgb(), false));
                                //drawImage.Save(@"C:\temp\darwImage.bmp");

                                float diff = ptBot.Min(f => f.Y) - ptTop.Max(f => f.Y);
                                l = Math.Max(l, diff);
                            }
                        }

                        if (g.UseW)
                        {
                            if (float.IsNaN(w))
                                w = 0;

                            ip.Sobel(topLightRot, sobelImage, Direction.Vertical);
                            //sobelImage.Save(@"C:\temp\sobelImage.bmp");

                            float centerX = (maskRect.Left + maskRect.Right) / 2;
                            if (validRectList.Count > 1)
                            {
                                float lMax = validRectList.Max(h => h.Left);
                                float rMin = validRectList.Min(h => h.Right);
                                centerX = (lMax + rMin) / 2;
                            }
                            //centerX = validRectList.Average(h => (h.Left + h.Right) / 2);

                            List<RectangleF> lRectList = validRectList.FindAll(f => f.Right < centerX); // 1/4선보다 위에있으면 Top으로
                            List<RectangleF> rRectList = validRectList.FindAll(f => f.Left > centerX); // 3/4선보다 아래에 있으면 Bottom으로
                            List<RectangleF> aRectList = validRectList.FindAll(f => lRectList.Contains(f) && rRectList.Contains(f)); // top과 bottom에 모두 속하면 무시
                            RectangleF[] lRects = lRectList.FindAll(f => !aRectList.Contains(f)).ToArray();
                            RectangleF[] rRects = rRectList.FindAll(f => !aRectList.Contains(f)).ToArray();
                            if (lRects.Length > 0 && rRects.Length > 0)
                            {
                                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "W"));

                                PointF[] ptLeft = lRects.Select(h => GetPoint(maskBufferRot, sobelImage, Rectangle.Round(RectangleF.FromLTRB(h.Right - 10, h.Top, h.Right + 10, h.Bottom)), Direction.Horizontal)).ToArray();
                                Array.ForEach(ptLeft, d => ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(d, new SizeF(5, 5))), Color.Blue.ToArgb(), false));
                                //drawImage.Save(@"C:\temp\darwImage.bmp");

                                PointF[] ptRight = rRects.Select(h => GetPoint(maskBufferRot, sobelImage, Rectangle.Round(RectangleF.FromLTRB(h.Left - 10, h.Top, h.Left + 10, h.Bottom)), Direction.Horizontal)).ToArray();
                                Array.ForEach(ptRight, d => ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(d, new SizeF(5, 5))), Color.Blue.ToArgb(), false));
                                //drawImage.Save(@"C:\temp\darwImage.bmp");

                                float diff = ptRight.Min(f => f.X) - ptLeft.Max(f => f.X);
                                w = Math.Max(w, diff);
                            }
                        }
                        blobRectList?.Dispose();
                    }
                };
                sobelImage.Dispose();

                result = new Tuple<RectangleF, float[], BitmapSource>(measureCenterRect, new float[] { w, l, w, l }, drawImage.ToBitmapSource());
            }
            maskBufferRot?.Dispose();

            drawImage.Save(@"darwImage.bmp", debugContext);

            drawImage.Dispose();
            topLightRot.Dispose();

            return result;
        }


        private static float[] DoW(AlgoImage greyImage, AlgoImage binalImage, RectangleF rect, RectangleF[] leftRects, RectangleF[] rightRects, AlgoImage darwImage, DebugContext debugContext)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            AlgoImage sobelImage = ImageBuilder.BuildSameTypeSize(greyImage);
            ip.Sobel(greyImage, sobelImage, Direction.Vertical);
            sobelImage.Save(@"sobelImageW.bmp", debugContext);

            PointF centerR, centerL;
            PointF[] rightL, leftR;

            //AlgoImage binalImage2 = greyImage.Clone();
            //ip.Binarize(greyImage, binalImage2, true);
            AlgoImage binalImage2 = binalImage;
            {
                // 중심 블랍의 오른쪽 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Right - 10, boundRect.Top, boundRect.Right + 10, boundRect.Bottom);

                centerR = GetPoint(binalImage2, sobelImage, boundRectR, Direction.Horizontal);

                Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(centerR), Size.Empty), 4, 4);
                ip.DrawRect(darwImage, drawRect, Color.Red.ToArgb(), false);
                ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "R");
                darwImage.Save(@"darwImage.bmp", debugContext);
            }

            {
                // 오른쪽 블랍(들)의 왼쪽 면
                rightL = rightRects.Select(rightRect =>
                {
                    Rectangle boundRect = Rectangle.Round(rightRect);
                    Rectangle boundRectL = Rectangle.FromLTRB(boundRect.Left - 10, boundRect.Top, boundRect.Left + 10, boundRect.Bottom);

                    return GetPoint(binalImage2, sobelImage, boundRectL, Direction.Horizontal);
                }).ToList().FindAll(f => f.X >= 0 && f.Y >= 0).ToArray();

                Array.ForEach(rightL, f =>
                {
                    Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 4, 4);
                    ip.DrawRect(darwImage, drawRect, 255, false);
                    ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "L");
                    darwImage.Save(@"darwImage.bmp", debugContext);
                });
            }

            {
                // 중심 블랍의 왼쪽 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectL = Rectangle.FromLTRB(boundRect.Left - 10, boundRect.Top, boundRect.Left + 10, boundRect.Bottom);

                centerL = GetPoint(binalImage2, sobelImage, boundRectL, Direction.Horizontal);

                Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(centerL), Size.Empty), 4, 4);
                ip.DrawRect(darwImage, drawRect, Color.Red.ToArgb(), false);
                ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "L");
                darwImage.Save(@"darwImage.bmp", debugContext);
            }

            {
                // 왼쪽 블랍(들)의 오른쪽 면
                leftR = leftRects.Select(leftRect =>
                {
                    Rectangle boundRect = Rectangle.Round(leftRect);
                    Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Right - 10, boundRect.Top, boundRect.Right + 10, boundRect.Bottom);

                    return GetPoint(binalImage2, sobelImage, boundRectR, Direction.Horizontal);
                }).ToList().FindAll(f => f.X >= 0 && f.Y >= 0).ToArray();

                Array.ForEach(leftR, f =>
                {
                    Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 4, 4);
                    ip.DrawRect(darwImage, drawRect, 255, false);
                    ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "R");
                    darwImage.Save(@"darwImage.bmp", debugContext);
                });
            }

            sobelImage.Dispose();
            //binalImage2.Dispose();

            float wR = 0, wL = 0;
            if (rightL.Length > 0)
            {
                float min = rightL.Min(f => f.X);
                if (min >= 0)
                    wR = min - centerR.X;
            }

            if (leftR.Length > 0)
            {
                float max = leftR.Max(f => f.X);
                if (max >= 0)
                    wL = centerL.X - max;
            }

            return new float[] { wL, wR };
        }

        private static float[] DoL(AlgoImage greyImage, AlgoImage binalImage, RectangleF rect, RectangleF[] topRects, RectangleF[] bottomRects, AlgoImage darwImage, DebugContext debugContext)
        {
            ImageProcessing ip = new DynMvp.Vision.Matrox.MilImageProcessing();
            AlgoImage sobelImage = ImageBuilder.BuildSameTypeSize(greyImage);
            ip.Sobel(greyImage, sobelImage);
            sobelImage.Save(@"sobelImageL.bmp", debugContext);

            PointF centerT, centerB;
            PointF[] topBottom, bottomTop;

            //AlgoImage binalImage2 = greyImage.Clone();
            //ip.Binarize(greyImage, binalImage2, true);
            AlgoImage binalImage2 = binalImage;
            {
                // 중심 블랍의 윗 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectT = Rectangle.FromLTRB(boundRect.Left, boundRect.Top - 10, boundRect.Right, boundRect.Top + 10);

                centerT = GetPoint(binalImage2, sobelImage, boundRectT, Direction.Vertical);

                Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(centerT), Size.Empty), 4, 4);
                ip.DrawRect(darwImage, drawRect, Color.Red.ToArgb(), false);
                ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "T");
                darwImage.Save(@"darwImage.bmp", debugContext);
                //darwImage.Save(@"C:\temp\darwImage.bmp");
            }

            {
                // 윗 블랍(들)의 아래 면
                topBottom = topRects.Select(topRect =>
                {
                    Rectangle boundRect = Rectangle.Round(topRect);
                    Rectangle boundRectB = Rectangle.FromLTRB(boundRect.Left, boundRect.Bottom - 10, boundRect.Right, boundRect.Bottom + 10);

                    return GetPoint(binalImage2, sobelImage, boundRectB, Direction.Vertical);
                }).ToList().FindAll(f => f.X >= 0 && f.Y >= 0).ToArray();

                Array.ForEach(topBottom, f =>
                {
                    Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 4, 4);
                    ip.DrawRect(darwImage, drawRect, 255, false);
                    ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "B");
                    darwImage.Save(@"darwImage.bmp", debugContext);
                });
            }

            {
                // 중심 블랍의 아래 면
                Rectangle boundRect = Rectangle.Round(rect);
                Rectangle boundRectB = Rectangle.FromLTRB(boundRect.Left, boundRect.Bottom - 10, boundRect.Right, boundRect.Bottom + 10);

                centerB = GetPoint(binalImage2, sobelImage, boundRectB, Direction.Vertical);

                Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(centerB), Size.Empty), 4, 4);
                ip.DrawRect(darwImage, drawRect, Color.Red.ToArgb(), false);
                ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "B");
                darwImage.Save(@"darwImage.bmp", debugContext);
            }

            {
                // 아래 블랍(들)의 윗 면
                bottomTop = bottomRects.Select(bottomRect =>
                {
                    Rectangle boundRect = Rectangle.Round(bottomRect);
                    Rectangle boundRectR = Rectangle.FromLTRB(boundRect.Left, boundRect.Top - 10, boundRect.Right, boundRect.Top + 10);

                    return GetPoint(binalImage2, sobelImage, boundRectR, Direction.Vertical);
                }).ToList().FindAll(f => f.X >= 0 && f.Y >= 0).ToArray();


                Array.ForEach(bottomTop, f =>
                {
                    Rectangle drawRect = Rectangle.Inflate(new Rectangle(Point.Round(f), Size.Empty), 4, 4);
                    ip.DrawRect(darwImage, drawRect, 255, false);
                    ip.DrawText(darwImage, new Point(drawRect.Right, drawRect.Top), Color.Yellow.ToArgb(), "T");
                    darwImage.Save(@"darwImage.bmp", debugContext);
                });
            }

            sobelImage.Dispose();
            //binalImage2.Dispose();

            float lT = 0, lB = 0;
            if (bottomTop.Length > 0)
                lT = bottomTop.Min(f => f.Y) - centerB.Y;
            if (topBottom.Length > 0)
                lB = centerT.Y - topBottom.Max(f => f.Y);

            return new float[] { lT, lB };
        }

        private static PointF GetPoint(AlgoImage binalImage, AlgoImage sobleImage, Rectangle rectangle, Direction direction)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(binalImage);
            PointF foundPoint = new PointF(-1, -1);

            rectangle.Intersect(new Rectangle(Point.Empty, binalImage.Size));
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return foundPoint;

            AlgoImage binalSubImage = binalImage.GetSubImage(rectangle);
            AlgoImage sobelSubImage = sobleImage.GetSubImage(rectangle);
            //binalSubImage.Save(@"C:\temp\GetPoint_binalSubImage.bmp");
            //sobelSubImage.Save(@"C:\temp\GetPoint_sobelSubImage.bmp");

            try
            {
                // 흰색(전극)이 가장 많은 부분을 찾음
                float maxIndex;
                float[] proj = ip.Projection(binalSubImage, direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal, ProjectionType.Mean);
                float maxValue = proj.Max();
                List<int> maxIndexeList = proj.Select((f, i) => f == maxValue ? i : -1).OrderByDescending(f => f).ToList();
                maxIndexeList.RemoveAll(f => f < 0);
                maxIndex = Array.IndexOf(proj, maxValue);

                if (maxIndexeList.Count > 1)
                {
                    int[] keys = maxIndexeList.Select((f, i) => f + i).ToArray();
                    IGrouping<int, int>[] groups = keys.GroupBy(f => f, f => Array.IndexOf(keys, f)).ToArray();
                    Tuple<int, float>[] lengthValues = groups.Select(f =>
                    {
                        int[] idxs = f.Select((g, i) => f.Key - g - i).ToArray();
                        return new Tuple<int, float>(idxs.Count(), (float)idxs.Average());
                    }).ToArray();

                    int maxLength = lengthValues.Max(f => f.Item1);
                    maxIndex = Array.Find(lengthValues, f => f.Item1 == maxLength).Item2;
                }

                Point pt = new Point
                    (
                    direction == Direction.Horizontal ? 10 : (int)maxIndex,
                    direction == Direction.Vertical ? 10 : (int)maxIndex);

                Rectangle cogRect = DrawingHelper.FromCenterSize(pt, new Size(5, 5));
                cogRect.Intersect(new Rectangle(Point.Empty, rectangle.Size));
                if (cogRect.Width == 0 || cogRect.Height == 0)
                    return foundPoint;

                using (AlgoImage sobelCogImage = sobelSubImage.GetSubImage(cogRect))
                {
                    float[] pp = ip.Projection(sobelCogImage, direction == Direction.Horizontal ? Direction.Horizontal : Direction.Vertical, ProjectionType.Mean);
                    float cog = pp.Select((f, i) => f * i).Sum() / pp.Sum();
                    SizeF offset = direction == Direction.Horizontal ? new SizeF(cogRect.Left + cog, maxIndex) : new SizeF(maxIndex, cogRect.Top + cog);
                    foundPoint = PointF.Add(new PointF(rectangle.Left, rectangle.Top), offset);
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
            BlobParam blobParam = new BlobParam() { EraseBorderBlobs = false, SelectRotateRect = true, RotateWidthMin = 10, RotateHeightMin = 10 };

            using (BlobRectList blobRectList = ip.Blob(maskSubImage, blobParam))
            {
                BlobRect[] blobRects = blobRectList.GetArray();
                IGrouping<bool, BlobRect>[] groups = blobRects.GroupBy(f => f.IsTouchBorder).ToArray();

                Dictionary<bool, BlobRect[]> blobRectDic = new Dictionary<bool, BlobRect[]>();
                Array.ForEach(groups, f =>
                {
                    blobRectDic.Add(f.Key, f.OrderBy(g => MathHelper.GetLength(g.RotateCenterPt, imgCenterPt)).ToArray());
                });

                List<BlobRect> list = new List<BlobRect>();
                if (blobRectDic.ContainsKey(false))
                    list.AddRange(blobRectDic[false]);
                if (blobRectDic.ContainsKey(true))
                    list.AddRange(blobRectDic[true]);

                return list.Select(f => new RotatedRect(DrawingHelper.FromCenterSize(f.RotateCenterPt, new SizeF(f.RotateWidth, f.RotateHeight)), f.RotateAngle)).ToArray();
            }
        }

        private static BlobRectList Blob(AlgoImage algoImage, SizeF minSize)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            BlobParam blobParam = new BlobParam()
            {
                EraseBorderBlobs = false,
                SelectRotateRect = true,
                RotateWidthMin = Math.Max(minSize.Width, minSize.Height),
                RotateHeightMin = Math.Min(minSize.Width, minSize.Height),
            };

            return ip.Blob(algoImage, blobParam);
        }
    }
}
