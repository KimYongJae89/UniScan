using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;

namespace UniScanG.Gravure.Inspect
{
    public class SheetGrabProcesserSAG : FrameGrabProcesserG
    {
        public override bool IsStopAndGo => true;

        public bool UsetLengthFilter { get; set; }
        public bool UseStdDevFilter { get; set; } = false;

        public SheetFinderBase Algorithm { get; set; }

        List<int> patternLengthList = null;

        public SheetGrabProcesserSAG() : base(-1)
        {
            this.patternLengthList = new List<int>();
        }

        protected override bool Verify(SheetImageSet sheetImageSet)
        {
            LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserSAG::Verify - ");

            if (UseStdDevFilter)
            {
                // 인쇄여부 확인..
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(sheetImageSet);
                float[] project = ip.Projection(sheetImageSet, Direction.Vertical, ProjectionType.Mean);
                float stdDev = MathHelper.StdDev(project);
                if (2 * stdDev < SheetFinderBase.SheetFinderBaseParam.MinBrightnessStdDev)
                {
                    LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserSAG::Verify - No Printed");
                    return false;
                }
            }

            if (UsetLengthFilter)
            {
                this.patternLengthList.Add(sheetImageSet.PatternSizePx.Height);
                this.patternLengthList.Sort();
                if (this.patternLengthList.Count > 10)
                {
                    double average = this.patternLengthList.GetRange(1, this.patternLengthList.Count - 2).Average();

                    double firstDiff = Math.Abs(this.patternLengthList.First() - average);
                    double lastDiff = Math.Abs(average - this.patternLengthList.Last());
                    if (firstDiff > lastDiff)
                        this.patternLengthList.Remove(this.patternLengthList.First());
                    else
                        this.patternLengthList.Remove(this.patternLengthList.Last());

                    double min = average * 0.98;
                    double max = average * 1.02;
                    if (!MathHelper.IsInRange(sheetImageSet.Height, min, max))
                    {
                        LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserSAG::Verify - Notgood");
                        return false;
                    }
                }
            }

            LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserSAG::Verify - Good");
            return true;
        }
    }
}
