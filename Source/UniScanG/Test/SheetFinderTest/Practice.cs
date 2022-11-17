using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Gravure.Inspect;
using System.Drawing;
using System.Threading;
using UniScanG.Gravure.Data;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices.FrameGrabber;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder.ReversOffset;
using UniScanG.Gravure.Vision.SheetFinder;
using UniEye.Base.Settings;

namespace SheetFinderTest
{
    class Practice
    {
        public PracticeGrabbedDelegate FrameGrabbed = null;
        public PracticeGrabbedDelegate SheetFounded = null;
        public GapFoundedDelegate GapFounded = null;

        ImageD baseFrame = null;
        int frameHeight;
        string debugPath;

        SheetGrabProcesserG grabProcesser = null;
        ThreadHandler workingThreadHandler = null;
        SheetFinderBaseParam sheetFinderBaseParam = null;
        StreamWriter streamWriter = null;

        public Practice()
        {
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetFinderV2.TypeName, ImagingLibrary.OpenCv, ""));
            AlgorithmBuilder.SetAlgorithmEnabled(SheetFinderV2.TypeName, true);
        }

        public void Start(Image2D templateImageD, SheetFinderBaseParam param, int frameHeight, float pelSize, string debugPath)
        {
            this.baseFrame = templateImageD.GetGrayImage();
            this.frameHeight = frameHeight;
            this.debugPath = debugPath;

            if (!string.IsNullOrEmpty(this.debugPath))
            {
                string txtFile = Path.Combine(this.debugPath, "Patterns", "Patterns.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(txtFile));
                this.streamWriter = File.AppendText(txtFile);
                this.streamWriter.WriteLine();
                this.streamWriter.WriteLine(DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss"));
                this.streamWriter.WriteLine("DateTime, No, Length, Base.X, Parants");
            }

            this.sheetFinderBaseParam = param;
            this.grabProcesser = new SheetGrabProcesserG();
            this.grabProcesser.Calibration = new ScaledCalibration(pelSize);
            this.grabProcesser.UseLengthFilter = true;
            this.grabProcesser.Initialize(param);

            SheetFinderBase finder = param.CreateFinder();
            finder.Param = param;
            grabProcesser.Algorithm = finder;
            grabProcesser.StartInspectionDelegate = grabProcesser_StartInspectionDelegate;
            grabProcesser.SetDebugMode(this.debugPath);
            grabProcesser.Start();

            workingThreadHandler = new ThreadHandler("WorkingThreadHandler", new System.Threading.Thread(ThreadProc), false);
            workingThreadHandler.Start();
        }

        private void grabProcesser_StartInspectionDelegate(DynMvp.Devices.ImageDevice imageDevice, IntPtr ptr)
        {
            SheetImageSet sheetImageSet = grabProcesser.GetSheetImageSet();
            Point patternOffset = UniScanG.Gravure.Vision.AlgorithmCommon.FindOffsetPosition(sheetImageSet, null, Point.Empty, this.sheetFinderBaseParam, null, null);

            ImageD imageD = sheetImageSet.ToImageD(0.1f);
            List<int> frameIds = new List<int>(sheetImageSet.PartImageList.ConvertAll<int>(f => (int)(f.ParentImage.Tag as CameraBufferTag)?.FrameId));
            SheetFounded?.Invoke(imageD, (int)ptr, sheetImageSet.PatternSizePx, patternOffset, frameIds);

            this.streamWriter?.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")}, {(int)ptr}, {sheetImageSet.Height}, {patternOffset.X}, {string.Join("/", frameIds)}");
            this.streamWriter?.Flush();

            sheetImageSet.Dispose();
        }

        public void Stop()
        {
            if (workingThreadHandler == null)
                return;

            workingThreadHandler.Stop();
            grabProcesser.Stop();
            grabProcesser.Dispose();

            this.streamWriter.Close();
            this.streamWriter.Dispose();
            this.streamWriter = null;

            baseFrame.Dispose();
            baseFrame = null;
            grabProcesser = null;
        }

        private void ThreadProc()
        {
            List<Fiducial> fiducialList = new List<Fiducial>();
            Size frameSize = new Size(baseFrame.Width, frameHeight);

            Image2D[] imageDs = new Image2D[5];
            for (int i = 0; i < imageDs.Length; i++)
                imageDs[i] = new Image2D(frameSize.Width, frameSize.Height, 1);

            int count = 0;

            while (workingThreadHandler.RequestStop == false)
            {
                Thread.Sleep(100);
                if (grabProcesser.IsBusy)
                    continue;

                List<Fiducial> exceptList = grabProcesser.FiducialList.Except(fiducialList).ToList();
                exceptList.ForEach(f =>
                {
                    if(f.IsValid)
                    {
                        CameraVirtualBufferTag tag = (CameraVirtualBufferTag)f.AlgoImage.Tag;
                        GapFounded?.Invoke((int)tag.FrameId, f.Rectnagle);
                    }
                });

                fiducialList = grabProcesser.FiducialList;

                int bufferId = count % imageDs.Length;
                Image2D frame = imageDs[bufferId];
                GetNextFrame(frameSize, frame);
                frame.Tag = new CameraVirtualBufferTag((ulong)count, frame.Size, "");

                FrameGrabbed?.Invoke(frame, count, frame.Size, Point.Empty, null);
                AddGrabProcesser(frame);
                count++;
                Thread.Sleep(100);
            }

            while (grabProcesser.IsBusy)
                Thread.Sleep(50);

            Array.ForEach(imageDs, f => f.Dispose());
        }

        private void AddGrabProcesser(ImageD frame)
        {
            //if (((CameraVirtualBufferTag)frame.Tag).FrameId > 10)
            grabProcesser.ImageGrabbed(frame);
        }

        int src = 0;
        int dst = 0;
        private void GetNextFrame(Size frameSize, Image2D newFrame)
        {
            Rectangle baseFrameRect = new Rectangle(Point.Empty, this.baseFrame.Size);

            Rectangle srcRect = new Rectangle(Point.Empty, frameSize);
            srcRect.Offset(0, src);

            Point dstPoint = Point.Empty;

            newFrame.Clear();
            while (srcRect.Height > 0)
            {
                Rectangle srcIntersectRect = Rectangle.Intersect(srcRect, baseFrameRect);
                newFrame.CopyFrom(this.baseFrame, srcIntersectRect, this.baseFrame.Pitch, dstPoint);

                dstPoint.Y += srcIntersectRect.Height;
                srcRect.Height -= srcIntersectRect.Height;
                srcRect.Y = 0;
            }
            src = (src + frameSize.Height) % this.baseFrame.Height;
        }
    }
}
