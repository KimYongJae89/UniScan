using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.SheetFinder
{
    public struct SheetFinderResultRect
    {
        public Rectangle Rectangle { get => rectangle; }
        Rectangle rectangle;

        public SheetFinderResultRect(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }

        public void Offset(int x, int y)
        {
            rectangle.Offset(x, y);
        }
    }

    public class SheetFinderResult : AlgorithmResult
    {
        public List<SheetFinderResultRect> SheetFinderResultRectList { get; private set; }

        public float BrightnessDev { get; private set; }
        public float Threshold { get; private set; }
        public float IntensityDiff { get; private set; }

        public SheetFinderResult(float brightnessDev, float threshold, float intensityDiff) : base("SheetFinderResult")
        {
            this.BrightnessDev = brightnessDev;
            this.Threshold = threshold;
            this.IntensityDiff = intensityDiff;
            this.SheetFinderResultRectList = new List<SheetFinderResultRect>();
        }
    }
}
