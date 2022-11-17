using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.SheetFinder.SheetBase
{
    public class SheetFinderV2 : SheetFinderBase
    {
        public override SheetFinderBaseParam Param => this.param != null ? (SheetFinderV2Param)this.param : (SheetFinderV2Param)AlgorithmSetting.Instance().SheetFinderBaseParam;

        public SheetFinderV2() : base()
        {
            this.AlgorithmName = TypeName;
            this.param = null;
        }

        #region Abstract

        public override AlgorithmParam CreateParam()
        {
            return new SheetFinderV2Param();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetFinderV2 clone = new SheetFinderV2();
            clone.CopyFrom(this);

            return clone;
        }
        #endregion

        #region override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new SheetFinderResult(-1, -1, -1);
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SheetFinderInspectParam sheetFinderInspectParam = (SheetFinderInspectParam)algorithmInspectParam;
            DebugContextG debugContextG = algorithmInspectParam.DebugContext as DebugContextG;

            SheetFinderResult algorithmResult = FindFiducial(sheetFinderInspectParam, debugContextG);

            sw.Stop();
            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan();

            return algorithmResult;
        }

        public SheetFinderResult FindFiducial(SheetFinderInspectParam sheetFinderInspectParam, DebugContextG debugContextG)
        {
            float[] projDatas = sheetFinderInspectParam.ProjDatas;
            SheetFinderPrecisionBuffer precisionBuffer = sheetFinderInspectParam.PrecisionBuffer;
            float threshold = sheetFinderInspectParam.PatternFinderThreshold;
            Calibration calibration = sheetFinderInspectParam.CameraCalibration;

            SheetFinderV2Param sheetFinderV2Param = (SheetFinderV2Param)Param;
            //ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            ImageProcessing imageProcessing = new MilImageProcessing();

            Stopwatch sw = new Stopwatch();
            List<Point> foundPointList = new List<Point>();
            try
            {
                sw.Start();

                float intensityDiff = -1;
                float stdDev = MathHelper.StdDev(projDatas);
                float projThreshold = -1;
                if (2 * stdDev >= sheetFinderV2Param.MinBrightnessStdDev)
                {
                    //float projThreshold = projection.Average() * sheetFinderV2Param.ProjectionBinalizeMul;

                    float agv = projDatas.Average();// projection.Max();
                    float min = projDatas.Min();
                    float max = projDatas.Max();
                    intensityDiff = max - min;

                    float diff = agv - min;
                    projThreshold = (diff * sheetFinderV2Param.ProjectionBinalizeMul) + min;

                    // Binarize
                    if (threshold < 0)
                        threshold = projThreshold;

                    // 밝은 영역을 찾음.
                    // 첫 영역이 밝음이면 무시
                    // 마지막 영역이 밝음이면 무시
                    List<Point> whiteRangeList = new List<Point>();
                    Point whiteRange = new Point(-1, -1);
                    bool onStart = false; 
                    for (int i = 0; i < projDatas.Length; i++)
                    {
                        if (projDatas[i] > threshold)
                        {
                            if (onStart && whiteRange.X < 0)
                                //if (whiteRange.X < 0)
                                whiteRange.X = i;
                        }
                        else
                        {
                            onStart = true;
                            if (whiteRange.X >= 0)
                            {
                                whiteRange.Y = i;
                                whiteRangeList.Add(whiteRange);
                                whiteRange = new Point(-1, -1);
                            }
                        }
                    }
                    //if (whiteRange.X >= 0)
                    //    whiteRangeList.Add(new Point(whiteRange.X, projection.Length - 1));

                    if (whiteRangeList.Count > 0)
                    {
                        //whiteRangeList.Sort((f, g) => (f.Y - f.X).CompareTo(g.Y - g.X));

                        double thresholdDist = 0, thresholdDist2 = double.MaxValue;
                        if (sheetFinderV2Param.BoundaryHeightMm > 0)
                        {
                            double dist = calibration.WorldToPixel(sheetFinderV2Param.BoundaryHeightMm * 1000);
                            thresholdDist = dist * 0.5;
                            thresholdDist2 = dist * 1.5;
                        }

                        whiteRangeList.ForEach(f =>
                        {
                            double dist = f.Y - f.X;
                            bool add = (thresholdDist <= dist && dist <= thresholdDist2);
                            //LogHelper.Debug(LoggerType.Inspection, string.Format("SheetFinderV2::FindFiducial - whiteRange Dist {0}[px], {1}", dist, add ? "Add" : "Removed"));

                            if (add)
                                foundPointList.Add(f);
                        });
                        float[] dd = whiteRangeList.Select(f => (float)(f.Y - f.X)).ToArray();

                        // for Save Debug
                        if (debugContextG.SaveDebugImage)
                        {
                            Directory.CreateDirectory(debugContextG.FullPath);
                            string path = Path.Combine(debugContextG.FullPath, $"{debugContextG.FrameId}.txt");
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(string.Format("Projection,ProjectionThreshold,Binarize,DistThreshold,DistThreshold2,IsWhiteRange,"));
                            for (int i = 0; i < projDatas.Length; i++)
                            {
                                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
                                      projDatas[i],
                                      threshold,
                                      projDatas[i] > threshold ? 255 : 0,
                                      thresholdDist, thresholdDist2,
                                      foundPointList.Find(f => (f.X <= i && i <= f.Y)).IsEmpty ? 0 : 255));
                            }
                            File.WriteAllText(path, sb.ToString());
                        }
                    }
                    sw.Stop();
                    foundPointList.Sort((f, g) => f.X.CompareTo(g.X));
                }

                if (sheetFinderV2Param.PrecisionPatternSizeFInd)
                {
                    for (int i = 0; i < foundPointList.Count; i++)
                    {
                        Point pt = foundPointList[i];
                        pt.X = FindPrecisionPosition(pt.X, projDatas, precisionBuffer);
                        pt.Y = FindPrecisionPosition(pt.Y, projDatas, precisionBuffer);
                        foundPointList[i] = pt;
                    }
                }

                List<Rectangle> gapRectList = foundPointList.ConvertAll<Rectangle>(f => Rectangle.FromLTRB(0, f.X, 0, f.Y));
                List<SheetFinderResultRect> sheetFinderResultRectList = gapRectList.ConvertAll<SheetFinderResultRect>(f => new SheetFinderResultRect(f));

                SheetFinderResult sheetFinderResult = new SheetFinderResult(stdDev, projThreshold, intensityDiff);
                sheetFinderResult.SheetFinderResultRectList.AddRange(sheetFinderResultRectList);
                sheetFinderResult.Good = foundPointList.Count > 0;

                sheetFinderResult.SpandTime = sw.Elapsed;//new TimeSpan();

                return sheetFinderResult;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - SheetFinderV2::FindFiducial - {0}", ex.Message));
                return null;
            }
#endif
            finally
            {
            }

        }

        private int FindPrecisionPosition(int startPos, float[] projDatas, SheetFinderPrecisionBuffer buffer)
        {
            if (buffer.Length > projDatas.Length)
                return startPos;

            if (buffer.Length < 5)
                return startPos;

            int src = startPos - buffer.Length / 2;
            int dst = startPos + buffer.Length / 2;
            if (src < 0)
            {
                src = 0;
                dst = buffer.Length - 1;
            }
            else if (dst >= projDatas.Length)
            {
                dst = projDatas.Length - 1;
                src = projDatas.Length - buffer.Length;
            }
            int length = dst - src;

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(buffer.PrecisionBuffer);

            Array.Copy(projDatas, src, buffer.PrecisionSingles, 0, length);
            RCI.RCIHelper.GetBytes(buffer.PrecisionSingles, buffer.PrecisionBytes);
            buffer.PrecisionBuffer.SetByte(buffer.PrecisionBytes);
            ip.Sobel(buffer.PrecisionBuffer);
            buffer.PrecisionBuffer.GetByte(buffer.PrecisionBytes);
            RCI.RCIHelper.GetSingles(buffer.PrecisionBytes, buffer.PrecisionSingles);
            //Array.ForEach(PrecisionSingles, f => Console.WriteLine(f));
            float centerOfMass = RCI.RCIHelper.WeightedCenterIndex(buffer.PrecisionSingles, 0, length);

            return (int)Math.Round(centerOfMass) + src;
        }
    }
}
