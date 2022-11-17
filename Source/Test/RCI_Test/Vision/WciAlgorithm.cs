using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCI_Test.Vision
{
    class WciAlgorithm
    {
        public WciAlgorithmParam Param => this.param;
        WciAlgorithmParam param;

        public WciAlgorithm()
        {
            this.param = new WciAlgorithmParam();
        }

        public void Initialize()
        {
            MatroxHelper.InitApplication();
        }

        public void Dispose()
        {
            MatroxHelper.FreeApplication();
        }

        public void BackgroundInspect(object sender, DoWorkEventArgs e)
        {
            WciInspectionParam wciInspectionParam = e.Argument as WciInspectionParam;
            e.Result = Inspect(wciInspectionParam, sender as BackgroundWorker);
        }

        //public WciInspectionResult Inspect(WciInspectionParam inspectionParam, BackgroundWorker backgroundWorker = null)
        //{
        //    bool license = MatroxHelper.LicenseExist();
        //    if (!license)
        //        throw new Exception("License");

        //    backgroundWorker?.ReportProgress(0, "Build Buffer");
        //    AlgoImage masterAlgoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectionParam.MasterImageD, ImageType.Grey);
        //    AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectionParam.ImageD, ImageType.Grey);
        //    AlgoImage resultAlgoImage = ImageBuilder.Build(algoImage);
        //    resultAlgoImage.Clear(255);
        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(masterAlgoImage);

        //    Rectangle imageRect = new Rectangle(Point.Empty, masterAlgoImage.Size);
        //    Rectangle patternRect = Rectangle.FromLTRB(550, 1030, 17740, 32650);
        //    Size refSize = new Size(70, 280);
        //    //Size searchSize = DrawingHelper.Mul(refSize, 3f);
        //    int countW = (int)Math.Ceiling(patternRect.Width * 1.0f / refSize.Width);
        //    int countH = (int)Math.Ceiling(patternRect.Height * 1.0f / refSize.Height);
        //    int count = countW * countH;
        //    MatchPos[,] bastPos = new MatchPos[countH, countW];

        //    float step = 80 * 1f / count;
        //    int curStep = 0;

        //    backgroundWorker?.ReportProgress(10, "Split");
        //    StringBuilder sb = new StringBuilder();
        //    for (int y = 0; y < countH; y++)
        //    {
        //        int cenY = patternRect.Top + patternRect.Height / (countH - 1) * y;
        //        for (int x = 0; x < countW; x++)
        //        {
        //            int cenX = patternRect.Left + patternRect.Width / (countW - 1) * x;

        //            Rectangle refRect = DrawingHelper.FromCenterSize(new Point(cenX, cenY), refSize);
        //            Rectangle searchRect = Rectangle.Intersect(imageRect, Rectangle.Inflate(refRect, refRect.Width, refRect.Height));
        //            PointF searchCenter = DrawingHelper.Subtract(DrawingHelper.CenterPoint(searchRect), searchRect.Location);

        //            AlgoImage patternAlgoImage = masterAlgoImage.Clip(refRect);
        //            //patternAlgoImage.Save(string.Format(@"C:\temp\[Y{0:D02},X{1:D02}]refAlgoImage.bmp", y, x));
        //            ip.Sobel(patternAlgoImage);

        //            AlgoImage searchAlgoImage = masterAlgoImage.Clip(searchRect);
        //            //searchAlgoImage.Save(string.Format(@"C:\temp\[Y{0:D02},X{1:D02}]searchAlgoImage.bmp", y, x));
        //            ip.Sobel(searchAlgoImage);

        //            Pattern pattern = AlgorithmBuilder.CreatePattern(ImagingLibrary.MatroxMIL);
        //            pattern.Train(patternAlgoImage, new PatternMatchingParam());
        //            PatternResult patternResult = pattern.Inspect(searchAlgoImage, new PatternMatchingParam() { NumToFind = -1, MatchScore = (int)this.param.MatchingScore }, null);

        //            string header = string.Format("[Y{0:D02},X{1:D02}]", y, x);
        //            SortedList<float, Tuple<MatchPos,PointF>> sortedList = new SortedList<float, Tuple<MatchPos, PointF>>();
        //            patternResult.MatchPosList.ForEach(f =>
        //            {
        //                PointF offset = DrawingHelper.Subtract(f.Pos, searchCenter);
        //                float length = MathHelper.GetLength(Point.Empty, offset);
        //                sortedList.Add(length, new Tuple<MatchPos, PointF>(f, offset));
        //            });

        //            Tuple<MatchPos, PointF>[] matchPos = sortedList.Values.ToArray();
        //            string[] datas = Array.ConvertAll(matchPos, f => string.Format("X{0:F02}/Y{1:F02}/S{2:F02}", f.Item2.X, f.Item2.Y, f.Item1.Score));
        //            bastPos[y, x] = patternResult.MaxMatchPos;

        //            string data = string.Join(", ", datas);
        //            sb.AppendLine(string.Format("{0} : {1}", header, data));
        //            if (bastPos[y, x] != null)
        //            {
        //                PointF offset = DrawingHelper.Subtract(patternResult.MaxMatchPos.Pos, searchCenter);
        //                Rectangle adjRect = DrawingHelper.Offset(refRect, Point.Round(offset));

        //                AlgoImage subRefAlgoImage = masterAlgoImage.GetSubImage(adjRect);
        //                AlgoImage subResultAlgoImage = resultAlgoImage.GetSubImage(adjRect);
        //                AlgoImage subAlgoImage = algoImage.GetSubImage(adjRect);

        //                ip.Subtract(subRefAlgoImage, subAlgoImage, subResultAlgoImage, true);

        //                //subRefAlgoImage.Save(@"C:\temp\subRefAlgoImage.bmp");
        //                //subAlgoImage.Save(@"C:\temp\subAlgoImage.bmp");
        //                //subResultAlgoImage.Save(@"C:\temp\subResultAlgoImage.bmp");

        //                subRefAlgoImage.Dispose();
        //                subAlgoImage.Dispose();
        //                subResultAlgoImage.Dispose();
        //            }
        //            patternResult.Dispose();
        //            pattern.Dispose();
        //            searchAlgoImage.Dispose();
        //            patternAlgoImage.Dispose();

        //            curStep++;
        //            backgroundWorker?.ReportProgress(10 + (int)Math.Round(curStep * step), string.Format("{0} / {1}", curStep, count));
        //            if (backgroundWorker.CancellationPending)
        //                throw new OperationCanceledException();
        //        }
        //    }
        //    File.WriteAllText(@"C:\temp\result.txt", sb.ToString());

        //    backgroundWorker?.ReportProgress(90, "Release Buffer");
        //    ImageD resultImageD = resultAlgoImage.ToImageD();
        //    masterAlgoImage?.Dispose();
        //    algoImage?.Dispose();
        //    resultAlgoImage?.Dispose();

        //    backgroundWorker?.ReportProgress(100, "Done");
        //    WciInspectionResult wciInspectionResult = new WciInspectionResult();
        //    wciInspectionResult.ResultImage = resultImageD;

        //    return wciInspectionResult;
        //}

        public WciInspectionResult Inspect(WciInspectionParam inspectionParam, BackgroundWorker backgroundWorker = null)
        {
            bool license = MatroxHelper.LicenseExist();
            if (!license)
                throw new Exception("License");

            backgroundWorker?.ReportProgress(0, "Build Buffer");
            AlgoImage masterAlgoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectionParam.MasterImageD, ImageType.Grey);
            AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectionParam.ImageD, ImageType.Grey);
            AlgoImage resultAlgoImage = ImageBuilder.BuildSameTypeSize(algoImage);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(masterAlgoImage);

            resultAlgoImage.Clear(255);
            backgroundWorker?.ReportProgress(10, "Split");
            Rectangle fullRect = new Rectangle(Point.Empty, algoImage.Size);

            int w = (masterAlgoImage.Width + this.param.BlockSize.Width - 1) / this.param.BlockSize.Width;
            int h = (masterAlgoImage.Height + this.param.BlockSize.Height - 1) / this.param.BlockSize.Height;
            int total = w * h;
            MatchPos[,] bastPos = new MatchPos[h, w];
            int count = 0;
            float step = 80 * 1f / total;
            Parallel.For(1, h, y =>
            //for (int y = 1; y < h; y++)
            {
                int bottom = Math.Min(masterAlgoImage.Height, y * this.param.BlockSize.Height);
                int top = bottom - this.param.BlockSize.Height;
                for (int x = 1; x < w; x++)
                {
                    int right = Math.Min(masterAlgoImage.Width, x * this.param.BlockSize.Width);
                    int left = right - this.param.BlockSize.Width;

                    Rectangle refRect = Rectangle.FromLTRB(left, top, right, bottom);
                    Rectangle inflateRect = Rectangle.Inflate(refRect, this.param.InflateSize.Width, this.param.InflateSize.Height);
                    PointF inflateCenter = DrawingHelper.CenterPoint(inflateRect);
                    Rectangle searchRect = Rectangle.Intersect(fullRect, inflateRect);
                    PointF searchLoc = searchRect.Location;
                    PointF searchCenter = DrawingHelper.Subtract(inflateCenter, searchLoc);

                    AlgoImage refAlgoImage = masterAlgoImage.Clip(refRect);
                    //refAlgoImage.Save(@"C:\temp\refAlgoImage.bmp");
                    ip.Sobel(refAlgoImage);

                    AlgoImage searchAlgoImage = algoImage.Clip(searchRect);
                    //searchAlgoImage.Save(@"C:\temp\searchAlgoImage.bmp");
                    ip.Sobel(searchAlgoImage);

                    Pattern pattern = AlgorithmBuilder.CreatePattern(ImagingLibrary.MatroxMIL);
                    pattern.Train(refAlgoImage, new PatternMatchingParam());
                    PatternResult patternResult = pattern.Inspect(searchAlgoImage, new PatternMatchingParam() { NumToFind = -1, MatchScore = (int)this.param.MatchingScore }, null);
                    bastPos[y - 1, x - 1] = patternResult.MaxMatchPos;
                    if (bastPos[y - 1, x - 1] != null)
                    {
                        PointF offset = DrawingHelper.Subtract(patternResult.MaxMatchPos.Pos, searchCenter);
                        Rectangle adjRect = DrawingHelper.Offset(refRect, Point.Round(offset));

                        AlgoImage subRefAlgoImage = masterAlgoImage.GetSubImage(adjRect);
                        AlgoImage subResultAlgoImage = resultAlgoImage.GetSubImage(adjRect);
                        AlgoImage subAlgoImage = algoImage.GetSubImage(adjRect);

                        ip.Subtract(subRefAlgoImage, subAlgoImage, subResultAlgoImage, true);

                        //subRefAlgoImage.Save(@"C:\temp\subRefAlgoImage.bmp");
                        //subAlgoImage.Save(@"C:\temp\subAlgoImage.bmp");
                        //subResultAlgoImage.Save(@"C:\temp\subResultAlgoImage.bmp");

                        subRefAlgoImage.Dispose();
                        subAlgoImage.Dispose();
                        subResultAlgoImage.Dispose();
                    }
                    patternResult.Dispose();
                    pattern.Dispose();
                    searchAlgoImage.Dispose();
                    refAlgoImage.Dispose();

                    count++;
                    backgroundWorker?.ReportProgress(10 + (int)Math.Round(count * step), string.Format("{0} / {1}", count, total));
                    if (backgroundWorker.CancellationPending)
                        throw new OperationCanceledException();
                }
            });

            backgroundWorker?.ReportProgress(90, "Release Buffer");
            ImageD resultImageD = resultAlgoImage.ToImageD();
            masterAlgoImage?.Dispose();
            algoImage?.Dispose();
            resultAlgoImage?.Dispose();

            backgroundWorker?.ReportProgress(100, "Done");
            WciInspectionResult wciInspectionResult = new WciInspectionResult();
            wciInspectionResult.ResultImage = resultImageD;

            return wciInspectionResult;
        }

    }
}
