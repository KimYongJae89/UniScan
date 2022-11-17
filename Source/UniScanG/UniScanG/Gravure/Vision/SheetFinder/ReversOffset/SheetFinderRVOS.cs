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

namespace UniScanG.Gravure.Vision.SheetFinder.ReversOffset
{
    public class SheetFinderRVOS : SheetFinderBase
    {
//        public new SheetFinderRVOSParam Param => this.param != null ? (SheetFinderRVOSParam)this.param : (SheetFinderRVOSParam)AlgorithmSetting.Instance().SheetFinderBaseParam;
        public override SheetFinderBaseParam Param => this.param != null ? (SheetFinderRVOSParam)this.param : (SheetFinderRVOSParam)AlgorithmSetting.Instance().SheetFinderBaseParam;

        public SheetFinderRVOS() : base()
        {
            this.AlgorithmName = TypeName;
            this.param = null;
        }

        #region Abstract

        public override AlgorithmParam CreateParam()
        {
            return new SheetFinderRVOSParam();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetFinderRVOS clone = new SheetFinderRVOS();
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
            DebugContext debugContext = algorithmInspectParam.DebugContext;

            SheetFinderResult algorithmResult = FindFiducial(sheetFinderInspectParam, debugContext);

            sw.Stop();
            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan();

            //if ((SamsungElectroSettings.Instance().SaveFiducialDebugData & SaveDebugData.Text) > 0)
            //    (MpisInspectorSystemManager.Instance().MainForm as Operation.UI.MainForm).WriteTimeLog("FiducialFinder", baseImage.Height, sw.ElapsedMilliseconds);

            return algorithmResult;
        }

        public SheetFinderResult FindFiducial(SheetFinderInspectParam sheetFinderInspectParam, DebugContext debugContext)
        {
            SheetFinderRVOSParam sheetFinderParam = (SheetFinderRVOSParam)this.Param;

            float[] projDatas = sheetFinderInspectParam.ProjDatas;
            float threshold = sheetFinderInspectParam.PatternFinderThreshold;
            bool skipOutter = sheetFinderInspectParam.SkipOutter;
            Calibration calibration = sheetFinderInspectParam.CameraCalibration;

            Stopwatch sw = new Stopwatch();
            List<Point> foundPointList = new List<Point>();
            try
            {
                sw.Start();

                float intensityDiff = -1;
                float calcThreshold = -1;
                float stdDev = MathHelper.StdDev(projDatas);
                Debug.Assert(3 * stdDev >= sheetFinderParam.MinBrightnessStdDev);
                if (2 * stdDev >= sheetFinderParam.MinBrightnessStdDev)
                {
                    // calculate
                    float agv = projDatas.Average();// projection.Max();
                    float min = projDatas.Min();
                    float max = projDatas.Max();
                    intensityDiff = projDatas.Max() - projDatas.Min();

                    float diff = max - agv;
                    calcThreshold = diff + min;
                    calcThreshold = (agv*1 + max*2) / 3;

                    // Binarize
                    if (threshold < 0)
                    {
                        //float threshold = projection.Average() * sheetFinderV2Param.ProjectionBinalizeMul;
                        threshold = calcThreshold;
                    }
                    
                    // 밝은 영역을 찾음.
                    // 첫 영역이 밝음이면 무시
                    // 마지막 영역이 밝음이면 무시
                    List<Point> whiteRangeList = new List<Point>();
                    int whiteRangeStart = -1;
                    bool onStart = false; 
                    for (int i = 0; i < projDatas.Length; i++)
                    {
                        if (projDatas[i] > threshold)
                        {
                            if (onStart && whiteRangeStart < 0)
                                whiteRangeStart = i;
                        }
                        else
                        {
                            onStart = true;
                            if (whiteRangeStart >= 0)
                            {
                                whiteRangeList.Add(new Point(whiteRangeStart, i));
                                whiteRangeStart = -1;
                            }
                        }
                    }

                    // 밝은 영역간의 거리를 구함.
                    double thresholdDist = 0, thresholdDist2 = double.MaxValue;
                    if (sheetFinderParam.BoundaryHeightMm > 0)
                    {
                        double dist = calibration.WorldToPixel(sheetFinderParam.BoundaryHeightMm * 1000);
                        thresholdDist = dist * 0.5;
                        thresholdDist2 = dist * 1.5;
                    }

                    foundPointList.AddRange(whiteRangeList);

                    // Merge
                    for (int i = 0; i < foundPointList.Count; i++)
                    {
                        Point working = foundPointList[i];
                        int src = working.X;

                        List<Point> founded = foundPointList.FindAll(g => MathHelper.IsInRange(g.Y - working.X, 0, thresholdDist2)).ToList();
                        if (founded.Count > 0)
                        {
                            foundPointList.RemoveAll(g => founded.Contains(g));
                            foundPointList.Remove(working);
                            working.X = Math.Min(working.X, founded.Min(g => g.X));
                            working.Y = Math.Max(working.Y, founded.Max(g => g.Y));
                            foundPointList.Add(working);
                        }
                    }

                    foundPointList.RemoveAll(f => !MathHelper.IsInRange(f.Y - f.X, thresholdDist, thresholdDist2));
                    if (skipOutter)
                    {
                        foundPointList.RemoveAll(f => f.X > projDatas.Length - thresholdDist2);
                        foundPointList.RemoveAll(f => f.Y < thresholdDist2);
                    }

                    //whiteRangeCenterList.Aggregate((f,g) =>
                    //{
                    //    double dist = g - f;
                    //    bool add = (thresholdDist <= dist && dist <= thresholdDist2);
                    //    //LogHelper.Debug(LoggerType.Inspection, string.Format("SheetFinderV2::FindFiducial - whiteRange Dist {0}[px], {1}", dist, add ? "Add" : "Removed"));

                    //    if (add)
                    //        foundPointList.Add(new Point(f,g));
                    //    return g;
                    //});

                    // for Save Debug
                    if (debugContext.SaveDebugImage)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("Projection,Threshold,Binarize,MinDistance,MaxDistance,IsFounded,"));
                        for (int i = 0; i < projDatas.Length; i++)
                        {
                            sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
                                  projDatas[i],
                                  threshold,
                                  projDatas[i] > threshold ? "255" : "",
                                  thresholdDist, thresholdDist2,
                                  foundPointList.Find(f => (f.X <= i && i <= f.Y)).IsEmpty ? 0 : 255));
                        }
                        string fileName = "Projection.txt";
                        if (debugContext is DebugContextG)
                            fileName = $"{((DebugContextG)debugContext).FrameId}.txt";

                        DebugHelper.SaveText(sb.ToString(), fileName, debugContext);
                    }
                    sw.Stop();
                    foundPointList.Sort((f, g) => f.X.CompareTo(g.X));
                }

                List<Rectangle> gapRectList = foundPointList.ConvertAll<Rectangle>(f => Rectangle.FromLTRB(0, f.X, 0, f.Y));
                List<SheetFinderResultRect> sheetFinderResultRectList = gapRectList.ConvertAll<SheetFinderResultRect>(f => new SheetFinderResultRect(f));
                
                SheetFinderResult sheetFinderResult = new SheetFinderResult(stdDev, calcThreshold, intensityDiff);
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
    }
}
