using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using System.ComponentModel;

namespace UniScanG.Gravure.UI.Teach
{
    class SaveTestInspection
    {
        public static void Save(string path, int tryId, AlgoImage algoImage, AlgoImage diffImage, AlgoImage binnImage, InspectionResult inspectionResult, BackgroundWorker worker = null)
        {
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);

            worker?.ReportProgress(0, "Initialize..");

            CalculatorResult calculatorResult = inspectionResult?.AlgorithmResultLDic[CalculatorBase.TypeName] as CalculatorResult;
            Point patternOffset = calculatorResult == null ? Point.Empty : calculatorResult.OffsetSet.PatternOffset.Offset;

            if (calculatorResult.PartialProjections != null)
            {
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(path, "PartialProejctionData.csv"), false))
                {
                    foreach (float f in calculatorResult.PartialProjections)
                        streamWriter.WriteLine($"{f}");
                    streamWriter.Flush();
                }
            }

            DetectorResult detectorResult = inspectionResult?.AlgorithmResultLDic[Detector.TypeName] as DetectorResult;
            int totalDefects = detectorResult.SheetSubResultList.Count;
            int expordedDefects = 0;


            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(path, "Summary.csv"), true))
            {
                if (streamWriter.BaseStream.Length == 0)
                    streamWriter.WriteLine($"Start Time, Try No, TotalDefects [EA], TotalMilliseconds [ms]");
                streamWriter.WriteLine($"{inspectionResult.InspectionStartTime.ToString("yyyy-MM-dd HH:mm:ss")}, {tryId}, {totalDefects}, {inspectionResult.InspectionTime.TotalMilliseconds}");
                //summaryStreamWriter.Flush();
            }

            string fileNameFormatGrabImage = tryId < 0 ? "GrabImage.png" : "T{0}_GrabImage.png";
            string fileNameFormatDiffImage = tryId < 0 ? "DiffImage.png" : "T{0}_DiffImage.png";
            string fileNameFormatBinnImage = tryId < 0 ? "BinnImage.png" : "T{0}_BinnImage.png";
            string fileNameFormatDefGImage = tryId < 0 ? "D{1}_{2}.png" : "T{0}_D{1}_{2}.png";
            string fileNameFormatDefBImage = tryId < 0 ? "B{1}_{2}.png" : "T{0}_B{1}_{2}.png";
            string fileNameFormatMarkImage = tryId < 0 ? "MarkImage.png" : "T{0}_MarkImage.png";
            string fileNameFormatDebugImage = tryId < 0 ? "DebugImage.png" : "T{0}_DebugImage.png";
            string markImagePath = Path.Combine(path, "MarkImage");
            string debugImagePath = Path.Combine(path, "DebugImage");
            //algoImage?.Save(Path.Combine(path, string.Format(fileNameFormatGrabImage, tryId)));
            //diffImage?.Save(Path.Combine(path, string.Format(fileNameFormatDiffImage, tryId)));
            //binnImage?.Save(Path.Combine(path, string.Format(fileNameFormatBinnImage, tryId)));

            StreamWriter overallStreamWriter = new StreamWriter(Path.Combine(path, "Defects.csv"), true);
            if (overallStreamWriter.BaseStream.Length == 0)
                overallStreamWriter.WriteLine("Bar, cpName, Image, X[mm], Y[mm], W[um], H[um]");

            if (Vision.AlgorithmSetting.Instance().CalculatorVersion == Vision.ECalculatorVersion.V3_RCI)
            {
                AlgoImage colorImage = null;
                try
                {
                    if (!Directory.Exists(debugImagePath))
                        Directory.CreateDirectory(debugImagePath);
                    calculatorResult.DebugImageD?.SaveImage(Path.Combine(debugImagePath, string.Format(fileNameFormatDebugImage, tryId)));

                    if (detectorResult.SheetSubResultList.Count > 0)
                        colorImage = algoImage?.ConvertTo(ImageType.Color);

                    detectorResult.SheetSubResultList.ForEach(f =>
                    {
                        if (worker != null && worker.CancellationPending)
                            throw new OperationCanceledException();

                        Rectangle defectRect = Rectangle.Round(DrawingHelper.Offset(f.Region, new Point(0, 0)));
                        string defectGImageFileName = string.Format(fileNameFormatDefGImage, tryId, f.Index, f.GetDefectType());
                        f.Image.Save(Path.Combine(path, defectGImageFileName));

                        string defectBImageFileName = string.Format(fileNameFormatDefBImage, tryId, f.Index, f.GetDefectType());
                        f.BufImage?.Save(Path.Combine(path, defectBImageFileName));

                        if (colorImage != null)
                        {
                            int argb = f.GetColor().ToArgb();

                            Rectangle drawRect = Rectangle.Inflate(defectRect, 20, 20);  //DrawingHelper.Offset(Rectangle.Inflate(g, 10, 10), new Point(0, 0));
                            for (int i = 0; i < 4; i++)
                            {
                                drawRect.Inflate(1, 1);
                                ip.DrawRotateRact(colorImage, new RotatedRect(drawRect, 0), argb, false);
                            }

                            Point textPoint = new Point(drawRect.Right, drawRect.Bottom);
                            ip.DrawText(colorImage, textPoint, argb, string.Format("{000}", f.Index), 4);
                        }

                        //if (colorImage != null)
                        //{
                        //    Rectangle drawRect = DrawingHelper.Offset(defectRect, new Point(-passRect.X, -passRect.Y));
                        //    drawRect.Inflate(5, 5);
                        //    ip.DrawRect(colorImage, drawRect, argb, false);
                        //    ip.DrawText(colorImage, Point.Add(new Point(drawRect.Right, drawRect.Top), new Size(5, 0)), argb, f.Index.ToString("000"));
                        //}

                        string[] writeDatas = new string[]
                        {
                            defectGImageFileName,
                            (f.RealRegion.X/1000).ToString(),
                            (f.RealRegion.Y/1000).ToString(),
                            (f.RealRegion.Width).ToString(),
                            (f.RealRegion.Height).ToString()
                        };

                        string writeData = string.Join(", ", writeDatas);
                        overallStreamWriter.WriteLine($", , {writeData}");
                        expordedDefects++;
                        worker?.ReportProgress((int)(expordedDefects * 100f / totalDefects), $"{expordedDefects}/{totalDefects}");
                    });
                }
                finally
                {
                    if (colorImage != null)
                    {
                        using (AlgoImage resize = ImageBuilder.Build(colorImage.LibraryType, colorImage.ImageType, DrawingHelper.Div(colorImage.Size, 2)))
                        {
                            ip.Resize(colorImage, resize);

                            if (!Directory.Exists(markImagePath))
                                Directory.CreateDirectory(markImagePath);
                            resize.ToImageD().SaveImage(Path.Combine(markImagePath, string.Format(fileNameFormatMarkImage, tryId)));
                        }
                        //colorImage.ToImageD().SaveImage(Path.Combine(path, string.Format(fileNameFormatMarkImage, tryId)));
                        colorImage.Dispose();
                    }
                    colorImage = null;
                }
            }
            else
            {
                List<RegionInfoG> regionInfoList = model.CalculatorModelParam.RegionInfoCollection;
                for (int i = 0; i < regionInfoList.Count; i++)
                {
                    if (worker != null && worker.CancellationPending)
                        break;

                    RegionInfoG regionInfoG = regionInfoList[i];
                    if (!regionInfoG.Use)
                        continue;

                    Rectangle barRegion = regionInfoG.Region;
                    Rectangle[] passRegion;
                    if (regionInfoG.PassRectList.Count > 0)
                        passRegion = regionInfoG.PassRectList.ToArray();
                    else
                    {
                        Rectangle fullRect = new Rectangle(Point.Empty, barRegion.Size);
                        //if (barRegion.Width * barRegion.Height > 50000000)
                        if (barRegion.Width * barRegion.Height * 3 < 0)
                        // RGB image Size overflow
                        {
                            Point centerPt = new Point(barRegion.Width / 2, barRegion.Height / 2);
                            passRegion = new Rectangle[]
                            {
                            Rectangle.FromLTRB(0, 0, centerPt.X,centerPt.Y),
                            Rectangle.FromLTRB(centerPt.X, 0, fullRect.Right, centerPt.Y),
                            Rectangle.FromLTRB(0,centerPt.Y, centerPt.X, fullRect.Bottom),
                            Rectangle.FromLTRB(centerPt.X,centerPt.Y, fullRect.Right, fullRect.Bottom),
                            };
                        }
                        else
                        {
                            passRegion = new Rectangle[] { fullRect };
                        }
                    }

                    for (int j = 0; j < passRegion.Length; j++)
                    {
                        if (worker != null && worker.CancellationPending)
                            break;

                        string subPath = string.Format("B{0}_S{1}", i, j);
                        string savePath = Path.Combine(path, subPath);
                        Rectangle passRect = DrawingHelper.Offset(passRegion[j], patternOffset);
                        float resizeRatio = diffImage.Width * 1.0f / algoImage.Width;
                        Rectangle saveRect = DrawingHelper.Offset(passRect, barRegion.Location);
                        Rectangle scaledRect = Rectangle.Round(DrawingHelper.Mul(saveRect, resizeRatio));

                        // 이미지
                        {
                            AlgoImage algoImg = null;
                            if (algoImage.IsInnerRect(saveRect))
                                algoImg = algoImage.Clip(saveRect);
                            //algoImg?.Save(Path.Combine(savePath, string.Format(fileNameFormatGrabImage, tryId)));
                            algoImg?.ToImageD().SaveImage(Path.Combine(savePath, string.Format(fileNameFormatGrabImage, tryId)));

                            using (AlgoImage clipDiffImage = diffImage?.Clip(scaledRect))
                            {
                                clipDiffImage.ToImageD().SaveImage(Path.Combine(savePath, string.Format(fileNameFormatDiffImage, tryId)));
                                //clipDiffImage?.Save(Path.Combine(savePath, string.Format(fileNameFormatDiffImage, tryId)));
                            }

                            using (AlgoImage clipBinnImage = binnImage?.Clip(scaledRect))
                            {
                                //clipBinnImage?.Save(Path.Combine(savePath, string.Format(fileNameFormatBinnImage, tryId)));
                                clipBinnImage?.ToImageD().SaveImage(Path.Combine(savePath, string.Format(fileNameFormatBinnImage, tryId)));
                            }

                            {
                                AlgoImage colorImage = algoImg?.ConvertTo(ImageType.Color);
                                int red = Color.Red.ToArgb();
                                if (colorImage != null)
                                {
                                    //colorImage.Save(Path.Combine(savePath, string.Format("T{0}_MarkImage.bmp", tryId)));
                                    List<Rectangle> intersectRectList = regionInfoG.CreticalPointList.FindAll(g => passRect.IntersectsWith(g));
                                    ip.DrawText(colorImage, Point.Empty, red, string.Format("CP: {000}", intersectRectList.Count));
                                    intersectRectList.ForEach(g =>
                                    {
                                        Rectangle drawRect = DrawingHelper.Offset(Rectangle.Inflate(g, 10, 10), new Point(-passRect.X, -passRect.Y));
                                        ip.DrawRotateRact(colorImage, new RotatedRect(drawRect, 45), red, false);
                                        ip.DrawText(colorImage, Point.Subtract(drawRect.Location, new Size(10, 10)), red, regionInfoG.CreticalPointList.IndexOf(g).ToString("000"));
                                    });
                                }

                                int cntInSec = 0;
                                try
                                {
                                    if (detectorResult != null)
                                    {
                                        detectorResult.SheetSubResultList.ForEach(f =>
                                        {
                                            if (worker != null && worker.CancellationPending)
                                                throw new OperationCanceledException();

                                            Rectangle defectRect = Rectangle.Round(DrawingHelper.Offset(f.Region, new Point(-barRegion.X, -barRegion.Y)));
                                            if (defectRect.IntersectsWith(passRect))
                                            {
                                                if (colorImage != null)
                                                {
                                                    Rectangle drawRect = DrawingHelper.Offset(defectRect, new Point(-passRect.X, -passRect.Y));
                                                    drawRect.Inflate(5, 5);
                                                    int argb = f.GetColor().ToArgb();
                                                    ip.DrawRect(colorImage, drawRect, argb, false);
                                                    ip.DrawText(colorImage, Point.Add(new Point(drawRect.Right, drawRect.Top), new Size(5, 0)), argb, f.Index.ToString("000"));
                                                }

                                                int[] cpIdxs = new int[] { -1 };
                                                Rectangle[] rects = regionInfoG.CreticalPointList.FindAll(g => g.IntersectsWith(defectRect)).ToArray();
                                                if (rects.Length > 0)
                                                    cpIdxs = rects.Select(g => regionInfoG.CreticalPointList.IndexOf(g)).ToArray();
                                                //{
                                                //    float[] dists = rects.Select(g => MathHelper.GetLength(DrawingHelper.CenterPoint(drawRect), DrawingHelper.CenterPoint(g))).ToArray();
                                                //    int minDistIdx = Array.FindIndex(dists, g => g == dists.Min());
                                                //    cpIdxs = new int[] { regionInfoG.CreticalPointList.IndexOf(rects[minDistIdx]) };
                                                //}
                                                Array.ForEach(cpIdxs, g =>
                                                {
                                                    string cpFolderName;
                                                    if (g < 0)
                                                        cpFolderName = "None";
                                                    else
                                                        cpFolderName = $"CP{g:000}";

                                                    string cpPath = Path.Combine(savePath, cpFolderName);
                                                    if (!Directory.Exists(cpPath))
                                                        Directory.CreateDirectory(cpPath);

                                                    string defectImageFileName = string.Format(fileNameFormatDefGImage, tryId, f.Index, f.GetDefectType());
                                                    int cnt = Directory.GetFiles(cpPath, string.Format("{0}*", defectImageFileName)).Length;
                                                    if (cnt > 0)
                                                        defectImageFileName += string.Format("_{0}", cnt);

                                                    f.Image.Save(Path.Combine(cpPath, string.Format("{0}.jpg", defectImageFileName)));

                                                    string[] writeDatas = new string[]
                                                    {
                                                        defectImageFileName,
                                                        (f.RealRegion.X/1000).ToString(),
                                                        (f.RealRegion.Y/1000).ToString(),
                                                        (f.RealRegion.Width).ToString(),
                                                        (f.RealRegion.Height).ToString()
                                                    };
                                                    //File.WriteAllText(Path.Combine(cpPath, string.Format("{0}.txt", defectImageFileName)), string.Join(Environment.NewLine, writeDatas));

                                                    string writeData = string.Join(", ", writeDatas);
                                                    overallStreamWriter.WriteLine($"{subPath}, {cpFolderName}, {writeData}");

                                                    string summaryFile = Path.Combine(cpPath, "Summary.txt");
                                                    using (StreamWriter sw = new StreamWriter(summaryFile, true))
                                                    {
                                                        if (sw.BaseStream.Length == 0)
                                                            sw.WriteLine("N, X[mm], Y[mm], W[um], H[um]");
                                                        sw.WriteLine(writeData);
                                                    }

                                                    expordedDefects++;
                                                    worker?.ReportProgress((int)(expordedDefects * 100f / totalDefects), $"{expordedDefects}/{totalDefects}");
                                                });
                                                cntInSec++;
                                            }
                                        });
                                    }
                                }
                                catch { }
                                finally
                                {
                                    if (colorImage != null)
                                    {
                                        ip.DrawText(colorImage, new Point(0, 15), red, string.Format("DEF: {000}", cntInSec));
                                        //colorImage.Save(Path.Combine(savePath, string.Format(fileNameFormatMarkImage, tryId)));
                                        colorImage.ToImageD().SaveImage(Path.Combine(savePath, string.Format(fileNameFormatMarkImage, tryId)));
                                    }
                                }
                                colorImage?.Dispose();
                            }
                            algoImg?.Dispose();
                        }
                    }
                }
            }

            if (tryId >= 0)
                File.WriteAllText(Path.Combine(path, tryId.ToString()), "");

            overallStreamWriter.Flush();
            overallStreamWriter.Close();
        }
    }
}
