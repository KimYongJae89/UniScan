using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Inspect
{
    public class FrameGrabProcesserG : GrabProcesserG
    {
        public override bool IsStopAndGo => false;

        protected SheetImageSet sheetImageSet = null;

        public int TargetLengthPx { get; private set; }

        public int RemainLengthPx { get; private set; }

        public FrameGrabProcesserG(int lengthPx)
        {
            this.TargetLengthPx = lengthPx;
        }

        protected override void OnStarted()
        {
            this.RemainLengthPx = this.TargetLengthPx;
        }

        protected override void OnStopped()
        {
            this.sheetImageSet?.Dispose();
            this.sheetImageSet = null;
        }

        protected override SheetImageSet[] GrabProcesserRunProc(ImageD imageD)
        {
            LogHelper.Debug(LoggerType.Grab, "FrameGrabProcesser::GrabProcesserRunProc");

            Size frameSize = ((CameraBufferTag)imageD.Tag).FrameSize;
            if (frameSize.IsEmpty)
                frameSize = imageD.Size;

            if (this.RemainLengthPx < 0)
                this.RemainLengthPx = frameSize.Height;

            List<SheetImageSet> list = new List<SheetImageSet>();

            using (AlgoImage algoImage = ImageBuilder.Build(Vision.SheetFinder.SheetFinderBase.TypeName, imageD, ImageType.Grey))
            {
                int srcY = 0;
                int dstY = frameSize.Height;
                do
                {
                    dstY = Math.Min(srcY + this.RemainLengthPx, frameSize.Height);

                    Rectangle rectangle = Rectangle.FromLTRB(0, srcY, frameSize.Width, dstY);
                    if (rectangle.Width > 0 && rectangle.Height > 0)
                    {
                        AlgoImage subImage = algoImage.Clip(rectangle);
                        if (this.sheetImageSet == null)
                            this.sheetImageSet = new SheetImageSet();
                        this.sheetImageSet.AddSubImage(subImage);

                        this.RemainLengthPx -= subImage.Height;

                        if (this.RemainLengthPx == 0)
                        {
                            this.sheetImageSet.PatternSizePx = this.sheetImageSet.Size;
                            list.Add(this.sheetImageSet);
                            this.sheetImageSet = null;
                            this.RemainLengthPx = TargetLengthPx;
                        }
                        srcY = dstY;
                    }
                } while (dstY < frameSize.Height);
            }

            return list.ToArray();
        }

        protected override bool Verify(SheetImageSet sheetImageSet)
        {
            return true;
        }
    }
}
