using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.RCI.Trainer.Reconstrct
{
    internal class BlobReconstrctor : Reconstrctor
    {

        public override void Reconstruct(AlgoImage scAlgoImage, AlgoImage scModelImage, AlgoImage scWeightImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(scAlgoImage);
            Rectangle scaleRect = new Rectangle(Point.Empty, scAlgoImage.Size);

            // Sobel
            AlgoImage scSobelImage = ImageBuilder.BuildSameTypeSize(scAlgoImage);
            ip.Sobel(scAlgoImage, scSobelImage);
            //scSobelImage.Save(@"C:\temp\bmp\scSobelImage.bmp");

            // binalize
            AlgoImage scBinalImage = ImageBuilder.BuildSameTypeSize(scAlgoImage);
            ip.Binarize(scAlgoImage, scBinalImage, true); // 전극을 하얗게.
            //scBinalImage.Save(@"C:\temp\bmp\scBinalImage.bmp");

            GetStatResult(scAlgoImage, out StatResult highStat, out StatResult lowStat);

            // blob
            BlobParam blobParam = new BlobParam()
            {
                SelectLabelValue = true,
                SelectArea = true,
                SelectBoundingRect = true,
                SelectRotateRect = true,
                SelectBorderBlobs = false,
                SelectCenterPt = true,
                EraseBorderBlobs = true
            };

            using (BlobRectList bList = ip.Blob(scBinalImage, blobParam))
            {
                Dictionary<int, BlobRect[]> blobRectGroups = GetBlobRectGroup(scAlgoImage, bList);

                AlgoImage scDrawBlobImage = ImageBuilder.Build(scAlgoImage.LibraryType, ImageType.Grey, scaleRect.Size);
                AlgoImage scModelMaskedImage = ImageBuilder.Build(scAlgoImage.LibraryType, ImageType.Grey, scaleRect.Size);
                AlgoImage scWeightMaskedImage = ImageBuilder.Build(scAlgoImage.LibraryType, ImageType.Grey, scaleRect.Size);

                for (int i = 0; i < blobRectGroups.Count; i++)
                {
                    KeyValuePair<int, BlobRect[]> f = blobRectGroups.ElementAt(i);
                    {
                        scDrawBlobImage.Clear();
                        ip.DrawBlob(scDrawBlobImage, bList, f.Value, new DrawBlobOption { SelectBlob = true });
                        ip.Dilate(scDrawBlobImage, 4);

                        Rectangle[] rects = f.Value.Select(g => Rectangle.Inflate(Rectangle.Round(g.BoundingRect), 4, 4)).ToArray();
                        rects = Array.FindAll(rects, g => Rectangle.Intersect(scaleRect, g) == g);
                        {
                            scModelMaskedImage.Clear();
                            ip.And(scBinalImage, scDrawBlobImage, scModelMaskedImage);
                            Reconstruct(scModelMaskedImage, scModelImage, rects);

                            scWeightMaskedImage.Clear();
                            ip.And(scSobelImage, scDrawBlobImage, scWeightMaskedImage);
                            Reconstruct(scWeightMaskedImage, scWeightImage, rects);
                        }
                    }
                    scWeightMaskedImage.Dispose();
                    scModelMaskedImage.Dispose();
                    scDrawBlobImage.Dispose();
                }
            }

            // 모델 이미지
            using (AlgoImage scModelTempImage = scModelImage.Clone())
            {
                float scale = (float)((highStat.average - lowStat.average) / 255);
                byte add = (byte)lowStat.average;

                ip.Not(scModelTempImage);
                ip.Mul(scModelTempImage, scModelTempImage, scale);
                ip.Add(scModelTempImage, scModelTempImage, add);
                for (int i = 0; i < 3; i++)
                    ip.Average(scModelTempImage);

                scModelImage.Copy(scModelTempImage);
            }

            scBinalImage.Dispose();
            scSobelImage.Dispose();
        }

        private static Dictionary<int, BlobRect[]> GetBlobRectGroup(AlgoImage algoImage, BlobRectList blobRectList)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            RCIBlobRect[] blobRects = blobRectList.GetList().FindAll(f => f.Area > 100).Select(f =>
            {
                Rectangle rect = Rectangle.Round(f.BoundingRect);
                Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2);
                float greyAverage = -1;
                if (r.Width > 0 && r.Height > 0)
                    greyAverage = ip.GetGreyAverage(algoImage, r);

                return new RCIBlobRect(f) { GreyMeanLT = greyAverage };
            }).ToArray();

            Func<RCIBlobRect, float>[] funcs = new Func<RCIBlobRect, float>[]
            {
                new Func<RCIBlobRect, float>(f=>f.BlobRect.BoundingRect.Width),
                new Func<RCIBlobRect, float>(f=>f.BlobRect.BoundingRect.Height),
                new Func<RCIBlobRect, float>(f=>f.BlobRect.Area),
                new Func<RCIBlobRect, float>(f=>f.GreyMeanLT),
            };

            float[] thresholds = new float[] { 20, 20, 800, 4 };

            List<IEnumerable<RCIBlobRect>> foundList = GroupBlobRect(blobRects, funcs, thresholds, 0);
            int fullSize = algoImage.Width * algoImage.Height;
            List<Tuple<IEnumerable<RCIBlobRect>, float>> l = foundList.Select(f => new Tuple<IEnumerable<RCIBlobRect>, float>(f, f.Sum(g => g.BlobRect.Area) * 100f / fullSize)).OrderByDescending(f => f.Item2).ToList();
            //vv.RemoveAll(f => f.Item2 < 0.1f);

            System.IO.Directory.CreateDirectory(@"C:\temp\GroupBlobRect\");
            FileHelper.ClearFolder(@"C:\temp\GroupBlobRect\");
            for (int i = 0; i < l.Count; i++)
            {
                Tuple<IEnumerable<RCIBlobRect>, float> tuple = l[i];
                IEnumerable<RCIBlobRect> orderd = tuple.Item1.OrderBy(f => f.BlobRect.Area);
                string[] strs = orderd.Select(f => $"{ f.BlobRect.LabelNumber:00000}: {f.BlobRect.BoundingRect}, {f.BlobRect.Area}, {f.GreyMeanLT}").ToArray();
                System.IO.File.WriteAllText($@"C:\temp\GroupBlobRect\scTempImage_{i}.txt", string.Join(Environment.NewLine, strs));

                using (AlgoImage scTempImage = algoImage.Clone())
                {
                    ip.Div(scTempImage, scTempImage, 4);
                    Array.ForEach(tuple.Item1.ToArray(), f =>
                    {
                        Rectangle rect = Rectangle.Round(f.BlobRect.BoundingRect);
                        ip.DrawRect(scTempImage, rect, 255, true);
                        ip.DrawText(scTempImage, rect.Location, 255, i.ToString());
                    });
                    using (AlgoImage scTempResizeImage = ImageBuilder.Build(scTempImage.LibraryType, scTempImage.ImageType, scTempImage.Width / 4, scTempImage.Height / 4))
                    {
                        ip.Resize(scTempImage, scTempResizeImage);
                        scTempResizeImage.Save($@"C:\temp\GroupBlobRect\scTempResizeImage{i}.bmp");
                    }
                }
            }
            return l.ToDictionary(f => l.IndexOf(f), f => f.Item1.Select(g => g.BlobRect).ToArray());
        }


        private static void Reconstruct(AlgoImage maskedSource, AlgoImage target, Rectangle[] rects)
        {
            //source.Save(@"C:\temp\bmp\source.bmp");
            //mask.Save(@"C:\temp\bmp\mask.bmp");
            //target.Save(@"C:\temp\bmp\target.bmp");
            Rectangle fullRect = new Rectangle(Point.Empty, maskedSource.Size);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(maskedSource);
            int avgW = (int)Math.Round(rects.Average(g => g.Width));
            int avgH = (int)Math.Round(rects.Average(g => g.Height));
            Size accImageSize = new Size(avgW, avgH);

            using (AlgoImage accImage = ImageBuilder.Build(maskedSource.LibraryType, ImageType.Depth, accImageSize))
            {
                accImage.Clear();

                int N = 256; // N개마다 다른 버퍼 사용..
                int bufCnt = (rects.Length - 1) / N + 1;
                AlgoImage[] bufImages = new AlgoImage[bufCnt];
                int[] bufLen = new int[bufCnt];
                for (int i = 0; i < bufCnt; i++)
                {
                    bufImages[i] = ImageBuilder.Build(maskedSource.LibraryType, ImageType.Depth, accImageSize);
                    bufImages[i].Clear();
                    bufLen[i] = 0;
                }

                Parallel.For(0, bufCnt, new Action<int>(bufIndx =>
                {
                    for (int i = 0; i < N; i++)
                    {
                        int rectIndex = bufIndx * N + i;
                        if (rectIndex >= rects.Length)
                            break;

                        Rectangle g = rects[rectIndex];
                        AlgoImage bufImage = bufImages[bufIndx];
                        //if (Rectangle.Intersect(fullRece, g) != g)
                        //    continue;

                        using (AlgoImage img = maskedSource.Clip(g))
                        {

                            // Center-Align
                            Size minSize = new Size(Math.Min(accImageSize.Width, g.Width), Math.Min(accImageSize.Height, g.Height));
                            Rectangle depthRect = Rectangle.Round(DrawingHelper.FromCenterSize(DrawingHelper.CenterPoint(new SizeF(g.Size)), minSize));
                            Rectangle bufRect = Rectangle.Round(DrawingHelper.FromCenterSize(DrawingHelper.CenterPoint(new SizeF(accImageSize)), minSize));
                            Debug.Assert(bufRect.Size == depthRect.Size);

                            //img.Save(@"C:\temp\bmp\sourceClip1.bmp");
                            using (AlgoImage depth = img.ConvertTo(ImageType.Depth))
                            {
                                //depth.Save(@"C:\temp\bmp\sourceClip2.bmp");
                                using (AlgoImage a = depth.GetSubImage(depthRect))
                                {
                                    ip.Div(a, a, 255);
                                    using (AlgoImage b = bufImage.GetSubImage(bufRect))
                                        ip.Add(a, b, b);
                                }
                                //bufImage.Save(@"C:\temp\bmp\sourceClip3.bmp");
                            }
                        }

                        bufLen[bufIndx]++;
                    }
                }));

                for (int i = 0; i < bufCnt; i++)
                {
                    AlgoImage bufImage = bufImages[i];

                    ip.Div(bufImage, bufImage, bufLen[i]);
                    ip.Add(bufImage, accImage, accImage);

                    bufImage.Dispose();
                }

                ip.Div(accImage, accImage, bufLen.Length);
                ip.Mul(accImage, accImage, 255);

                using (AlgoImage accImageG = accImage.ConvertTo(ImageType.Grey))
                {
                    Parallel.ForEach(rects, g =>
                    {
                        RectangleF rect = g;
                        PointF centerPt = DrawingHelper.CenterPoint(rect);
                        Rectangle addRec = Rectangle.Round(DrawingHelper.FromCenterSize(centerPt, accImageSize));

                        //if (Rectangle.Intersect(fullRece, addRec) == addRec)
                        {
                            using (AlgoImage targetSubImage = target.GetSubImage(addRec))
                                ip.Add(targetSubImage, accImageG, targetSubImage);
                        }
                    });
                }
            }
        }


        private static List<IEnumerable<RCIBlobRect>> GroupBlobRect(IEnumerable<RCIBlobRect> blobRects, Func<RCIBlobRect, float>[] funcs, float[] thresholds, int level)
        {
            List<IEnumerable<RCIBlobRect>> foundList = new List<IEnumerable<RCIBlobRect>>();
            if (level == funcs.Length)
            {
                foundList.Add(blobRects);
                return foundList;
            }

            Tuple<RCIBlobRect, float>[] orders = blobRects.Select(f => new Tuple<RCIBlobRect, float>(f, funcs[level](f))).OrderBy(f => f.Item2).ToArray();

            if (false)
            {
                List<RCIBlobRect> list = new List<RCIBlobRect>();
                float agvValue = orders.First().Item2;
                //RCIHelper.SaveSingles(@"C:\temp\Singles.txt", orders.Select(f => f.Item2).ToArray());
                Array.ForEach(orders, f =>
                {
                    RCIBlobRect item = f.Item1;
                    float value = f.Item2;

                    float d = value - agvValue;
                    if (d > value / 10f)
                    {
                        if (list.Count > 0)
                            foundList.AddRange(GroupBlobRect(list.ToArray(), funcs, thresholds, level + 1));

                        list.Clear();
                        agvValue = value;
                    }

                    if (value > 15)
                        list.Add(item);

                    if (list.Count == 0)
                        agvValue = value;
                    else
                        agvValue = (agvValue * (list.Count - 1) + value) / list.Count;
                });

                if (list.Count > 0)
                    foundList.AddRange(GroupBlobRect(list.ToArray(), funcs, thresholds, level + 1));

                return foundList;
            }
            else
            {
                List<RCIBlobRect> rectList = orders.Select(f => f.Item1).ToList();
                float[] diffs = RCIHelper.Difference(orders.Select(f => f.Item2));
                int[] spikePos = RCITrainer.FindSpikes(diffs, 0, thresholds[level]).Select(f => (int)f.Item1).ToArray();
                //int[] spikePos = RCIAnalyzer.FindSpikes(diffs, out float threshold).Select(f=>(int)f.Item1).ToArray();
                //var v2 = RCIAnalyzer.FindSpikes(diffs.ToList().GetRange(0, spikePos[0] + 1).ToArray(), out float th2);

                if (spikePos.Length == 0)
                    return GroupBlobRect(rectList, funcs, thresholds, level + 1);

                int src = 0;
                for (int i = 0; i < spikePos.Length; i++)
                {
                    int dst = spikePos[i];
                    int cnt = dst - src + 1;
                    foundList.AddRange(GroupBlobRect(rectList.GetRange(src, cnt), funcs, thresholds, level + 1));
                    src += cnt;
                }

                if (src != rectList.Count)
                {
                    int cnt = rectList.Count - src;
                    foundList.AddRange(GroupBlobRect(rectList.GetRange(src, cnt), funcs, thresholds, level + 1));
                }
                return foundList;
            }
        }

        public static void ReconstructRemove(AlgoImage scAlgoImage, AlgoImage scBinalImage, StatResult highStat, StatResult lowStat,
            AlgoImage scModelImage, AlgoImage scWeightImage)
        {

        }
    }
}
