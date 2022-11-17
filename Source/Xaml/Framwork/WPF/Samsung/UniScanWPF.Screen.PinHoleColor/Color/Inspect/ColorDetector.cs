using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Color.Data;
using UniScanWPF.Screen.PinHoleColor.Color.Settings;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Color.Inspect
{
    internal class ColorDetector : Detector
    {
        public override Tuple<int, ConcurrentStack<AlgoImage>> GetBufferStack(ImageDevice targetDevice)
        {
            ConcurrentStack<AlgoImage> stack = new ConcurrentStack<AlgoImage>();

            for (int i = 0; i < ColorSettings.Instance().BufferNum; i++)
            {
                AlgoImage buffer = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height));
                stack.Push(buffer);
            }

            return new Tuple<int, ConcurrentStack<AlgoImage>>(1, stack);
        }

        public override DetectorResult Detect(AlgoImage targetImage, AlgoImage[] buffers, DetectorParam detectorParam)
        {
            ColorDetectorResult colorDetectorResult = new ColorDetectorResult();

            ColorDetectorParam colorDetectorParam = (ColorDetectorParam)detectorParam;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(targetImage);
            float average = imageProcessing.GetGreyAverage(targetImage);

            FindBlank(targetImage, average, Direction.Horizontal, out List<Tuple<int, int, int>> blankListH);
            FindBlank(targetImage, average, Direction.Vertical, out List<Tuple<int, int, int>> blankListV);

            if (blankListH.Count < 2 || blankListV.Count == 0)
            {
                colorDetectorResult.Judgment = DynMvp.InspData.Judgment.Reject;
                return colorDetectorResult;
            }

            List<Rectangle> interestRectList = GetInterestRegionList(blankListH, blankListV, targetImage.Height, out int width, out int height);
            float minWidthRatio = ColorSettings.Instance().MinWidthRatio;
            float minHeightRatio = ColorSettings.Instance().MinHeightRatio;
            colorDetectorResult.WidthRatio = (float)width / (float)targetImage.Width;
            colorDetectorResult.HeightRatio = (float)height / (float)targetImage.Height;

            if (width < targetImage.Width * minWidthRatio / 100.0f || height < targetImage.Height * minHeightRatio / 100.0f)
            {
                colorDetectorResult.Judgment = DynMvp.InspData.Judgment.Reject;
                return colorDetectorResult;
            }

            AlgoImage sheetImage = null;
            List<Tuple<Rectangle, float>> avgList = null;
            if (interestRectList.Count == 1)
            {
                sheetImage = targetImage.GetSubImage(interestRectList[0]);
                avgList = GetAvgList(sheetImage, interestRectList[0].Location);
            }
            else
            {
                sheetImage = CreateSheetImage(interestRectList, targetImage, buffers[0], width, height);
                avgList = GetAvgList(sheetImage, new Point(0, 0));
            }

            colorDetectorResult.Average = avgList.Average(result => result.Item2);
            List<Tuple<ColorDefectType, Rectangle, float>> resultList = new List<Tuple<ColorDefectType, Rectangle, float>>();

            float lowerValue = colorDetectorResult.Average - colorDetectorParam.LowerThreshold;
            float upperValue = colorDetectorResult.Average + colorDetectorParam.UpperThreshold;

            int index = 0;
            foreach (Tuple<Rectangle, float> avgTuple in avgList)
            {
                if (avgTuple.Item2 >= lowerValue && avgTuple.Item2 <= upperValue)
                    continue;

                ColorDefectType type = ColorDefectType.Blot;

                if (avgTuple.Item2 > upperValue)
                    type = ColorDefectType.NoPrint;

                float diffValue = 0;
                switch (type)
                {
                    case ColorDefectType.Blot:
                        diffValue = avgTuple.Item2 - colorDetectorResult.Average;
                        break;
                    case ColorDefectType.NoPrint:
                        diffValue = avgTuple.Item2 - colorDetectorResult.Average;
                        break;
                }

                Rectangle rectangle = avgTuple.Item1;
                if (interestRectList.Count == 1)
                    rectangle = new Rectangle(avgTuple.Item1.X - interestRectList[0].X, avgTuple.Item1.Y - interestRectList[0].Y, avgTuple.Item1.Width, avgTuple.Item1.Height);

                AlgoImage subImage = sheetImage.GetSubImage(rectangle);

                ColorDefect defect = new ColorDefect(index++, type, avgTuple.Item1, subImage.ToBitmapSource(), diffValue);
                subImage.Dispose();
                colorDetectorResult.DefectList.Add(defect);
            }

            colorDetectorResult.SheetImage = sheetImage.ToBitmapSource();

            sheetImage.Dispose();
            
            if (colorDetectorResult.DefectList.Count == 0)
                colorDetectorResult.Judgment = DynMvp.InspData.Judgment.Accept;

            return colorDetectorResult;
        }

    private List<Tuple<Rectangle, float>> GetAvgList(AlgoImage sheetImage, Point offset)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sheetImage);

            List<Tuple<Rectangle, float>> resultList = new List<Tuple<Rectangle, float>>();

            int rown = ColorSettings.Instance().Row;
            int column = ColorSettings.Instance().Column;

            for (int row = 0; row < rown; row++)
            {
                for (int col = 0; col < column; col++)
                {
                    int subWidth = sheetImage.Width / column;
                    int subHeight = sheetImage.Height / rown;

                    int x = col* subWidth;
                    int y = row * subHeight;

                    if (col == ColorSettings.Instance().Column - 1)
                        subWidth = sheetImage.Width - (col * subWidth);
                    if (row == ColorSettings.Instance().Row - 1)
                        subHeight = sheetImage.Height - (row * subHeight);

                    Rectangle rect = new Rectangle(x, y, subWidth, subHeight);
                    AlgoImage interestImage = sheetImage.GetSubImage(rect);
                    float average = imageProcessing.GetGreyAverage(interestImage);
                    rect.Offset(offset);
                    resultList.Add(new Tuple<Rectangle, float>(rect, average));
                    interestImage.Dispose();
                }
            }
            
            return resultList;
        }

        private AlgoImage CreateSheetImage(List<Rectangle> interestRectList, AlgoImage targetImage, AlgoImage buffer, int width, int height)
        {
            AlgoImage sheetImage = buffer.GetSubImage(new Rectangle(0, 0, width, height));

            interestRectList.ForEach(rect =>
            {
                AlgoImage srcSubImage = targetImage.GetSubImage(rect);
                int y = 0;
                if (rect != interestRectList.First())
                    y = interestRectList.TakeWhile(takeRect => takeRect != rect).Sum(takeRect => takeRect.Height);

                AlgoImage destSubImage = sheetImage.GetSubImage(new Rectangle(0, y, width, rect.Height));

                destSubImage.Copy(srcSubImage);

                srcSubImage.Dispose();
                destSubImage.Dispose();
            });

            return sheetImage;
        }

        private List<Rectangle> GetInterestRegionList(List<Tuple<int, int, int>> blankListH, List<Tuple<int, int, int>> blankListV, int height, out int sheetWidth, out int sheetHeight)
        {
            List<Rectangle> interestRectList = new List<Rectangle>();
            blankListH = blankListH.OrderByDescending(blank => blank.Item3).Take(2).OrderBy(blank => blank.Item1).ToList();

            int x = blankListH[0].Item2 + 1;
            int width = sheetWidth =  blankListH[1].Item1 - 1 - x;

            blankListV.Reverse();
            Rectangle last = new Rectangle(x, blankListV.First().Item2 + 1, width, height - (blankListV.First().Item2 + 1));
            if (last.Height > 0 && last.Width > 0)
                interestRectList.Add(last);

            if (blankListV.Count >= 2)
            {
                blankListV.Aggregate((f, g) =>
                {
                    int startY = g.Item2 + 1;
                    interestRectList.Add(new Rectangle(x, startY, width, (f.Item1 - 1) - startY));
                    return g;
                });
            }

            Rectangle first = new Rectangle(x, 0, width, blankListV.Last().Item1 - 1);

            if (first.Width > 0 && first.Height > 0)
                interestRectList.Add(first);

            sheetHeight = interestRectList.Sum(rect => rect.Height);

            return interestRectList;
        }

        private void FindBlank(AlgoImage targetImage, float average, Direction direction, out List<Tuple<int, int, int>> blankList)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(targetImage);

            float[] sheetProfile = imageProcessing.Projection(targetImage, direction, ProjectionType.Mean);

            //start, end, gap
            blankList = new List<Tuple<int, int, int>>();

            int startPos = -1;
            for (int i = 0; i < sheetProfile.Length; i++)
            {
                if (startPos != -1)
                {
                    if (sheetProfile[i] < average)
                    {
                        blankList.Add(new Tuple<int, int, int>(startPos, i - 1, i - startPos - 1));
                        startPos = -1;
                    }
                    else if (i == sheetProfile.Length - 1)
                    {
                        blankList.Add(new Tuple<int, int, int>(startPos, i, i - startPos));
                        startPos = -1;
                    }

                    continue;
                }

                if (sheetProfile[i] > average)
                    startPos = i;
            }

            if (blankList.Count != 0)
            {
                double minLength = blankList.ConvertAll<int>(blank => blank.Item3).Average() * 2.0;
                blankList.RemoveAll(blank => blank.Item3 < minLength);
            }
        }
}
}
