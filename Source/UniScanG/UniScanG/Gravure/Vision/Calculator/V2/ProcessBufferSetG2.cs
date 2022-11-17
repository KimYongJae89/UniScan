using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Gravure.Vision.Calculator.V2
{
    public class ProcessBufferSetG2 : ProcessBufferSetG
    {
        public Calibration Calibration => SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

        float previewScale = 0.1f;
        bool halfScale = false;

        public Task ImageSaveTask { get; private set; }

        public static OffsetStructSet GlobalOffsetStructSet => globalOffsetStructSet;
        static OffsetStructSet globalOffsetStructSet;

        internal InspectRegion[] InspectRegions => this.inspectRegions;
        InspectRegion[] inspectRegions = new InspectRegion[0];

        internal MeasureShrinkAndExtend MeasureShrinkAndExtend => this.measureShrinkAndExtend;
        MeasureShrinkAndExtend measureShrinkAndExtend = new MeasureShrinkAndExtend();

        public AlgoImage PreviewBuffer { get => previewBuffer; }
        protected AlgoImage previewBuffer = null;

        ManualResetEvent isWorkDone = new ManualResetEvent(true);
        Task bitmapBuildTask = null;
        

        public override bool IsDone => isWorkDone.WaitOne(0);

        public ProcessBufferSetG2(float previewScale, float scaleFactor, bool isMultiLayer, int width, int height) : base(scaleFactor, isMultiLayer, width, height)
        {
            this.previewScale = previewScale;
        }

        public override void BuildBuffers(bool halfScale)
        {
            base.BuildBuffers(halfScale);

            this.halfScale = halfScale;
            if (halfScale)
                this.scaleFactor /= 2;

            ImagingLibrary calculatorLibType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).LibraryType;
            ImageType calculatorImgType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).ImageType;
            ImagingLibrary detectorLibType = AlgorithmBuilder.GetStrategy(Detector.Detector.TypeName).LibraryType;
            ImageType detectorImgType = AlgorithmBuilder.GetStrategy(Detector.Detector.TypeName).ImageType;
            bool isHeterogenous = (calculatorLibType != detectorLibType) || (calculatorImgType != detectorImgType);

            int bufferDepth = this.isMultiLayer ? 4 : 1;

            bufferList.Add(this.previewBuffer = ImageBuilder.Build(Detector.Detector.TypeName, (int)(width * previewScale), (int)(height * previewScale)));

            width = (int)(width * scaleFactor);
            height = (int)(height * scaleFactor);

            //if (scaleFactor < 1 || isHeterogenous)
            bufferList.Add(this.scaledImage = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            bufferList.Add(this.calculatorResultGray = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            bufferList.Add(this.calculatorResultBinal = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            bufferList.Add(this.maskImage = ImageBuilder.Build(Detector.Detector.TypeName, width, height));

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            CalculatorParam calculatorParam = AlgorithmSetting.Instance().CalculatorParam;
            this.inspectRegions = CalculatorV2Extender.Train(model, calculatorParam, halfScale, scaleFactor);
            this.offsetStructSet = new OffsetStructSet(this.inspectRegions.Length);
            if (globalOffsetStructSet == null || globalOffsetStructSet.LocalCount != this.inspectRegions.Length)
                globalOffsetStructSet = new OffsetStructSet(this.inspectRegions.Length);

            DetectorParam detectorParam = AlgorithmSetting.Instance().DetectorParam;
            if (detectorParam.Reconstruction)
                bufferList.Add(this.edgeMapImage = ImageBuilder.Build(Detector.Detector.TypeName, width, height));

            WatcherParam watcherParam = AlgorithmSetting.Instance().WatcherParam;
            List<ExtItem> watchItemList = model.WatcherModelParam.TransformCollection.Items;
            this.measureShrinkAndExtend.Initialize(watchItemList.ToArray(), Calibration);
        }

        public override void Dispose()
        {
            Array.ForEach(inspectRegions, f => f?.Dispose());
            this.measureShrinkAndExtend.Dispose();

            //this.EdgeFinderBuffer?.Dispose();
            base.Dispose();
        }

        public override void Upload(DebugContext debugContext)
        {
            DebugContextG debugContextG = debugContext as DebugContextG;
            //Clear();
            
            Vision.AlgorithmCommon.ScaleImage(this.algoImage, this.previewBuffer, this.previewScale);

            if (this.scaledImage != null)
                Vision.AlgorithmCommon.ScaleImage(this.algoImage, this.scaledImage, this.scaleFactor);

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            Stopwatch sw = Stopwatch.StartNew();

            Point patternOffset = AlgorithmCommon.FindOffsetPosition(this.algoImage, this.EdgeFinderBuffer, model.CalculatorModelParam.BasePosition, calibration, debugContextG);
            ImageD patternOffsetImageD = null;

            if (AdditionalSettings.Instance().DebugOffsetLog)
            {
                Rectangle basePointRect = DrawingHelper.FromCenterSize(DrawingHelper.Add(model.CalculatorModelParam.BasePosition, patternOffset), OffsetStruct.ImageSize);
                basePointRect.Intersect(new Rectangle(Point.Empty, this.algoImage.Size));
                if (basePointRect.Width * basePointRect.Height > 0)
                {
                    using (AlgoImage basePointImage = this.algoImage.GetSubImage(basePointRect))
                        patternOffsetImageD = basePointImage.ToImageD();
                }
            }
            SetPatternOffset(true, model.CalculatorModelParam.BasePosition, patternOffset, patternOffsetImageD);

            sw.Stop();
            debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Align, 0);
            debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Align, sw.ElapsedMilliseconds);

            StartPreviewBitmapBuild(this.algoImage.Size);

            AlgoImage inspImage = (this.scaledImage == null ? this.algoImage : this.scaledImage);

            if (AlgorithmSetting.Instance().LengthVariationParam.Use)
            {
                int locXPx = (int)Math.Round(inspImage.Width * AlgorithmSetting.Instance().LengthVariationParam.Position);
                locXPx += patternOffset.X;
                int szW = (int)Math.Round(calibration.WorldToPixel(AlgorithmSetting.Instance().LengthVariationParam.WidthUm));
                Rectangle rectangle = new Rectangle(locXPx, 0, szW, inspImage.Height);
                UpdateLengthVariateion(inspImage, rectangle);
            }

            sw.Restart();
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = calculatorParam.UseMultiThread ? -1 : 1 };
            //parallelOptions.MaxDegreeOfParallelism = this.inspectRegions.Length / 1;
            debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Align2, parallelOptions.MaxDegreeOfParallelism);
            debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Build, parallelOptions.MaxDegreeOfParallelism);
            Parallel.For(0, this.inspectRegions.Length, parallelOptions, i =>
            {
                InspectRegion inspectRegion = this.inspectRegions[i];
                if (!inspectRegion.Use)
                    return;

                DebugContextG regionDebugContextG = debugContextG.Clone();
                regionDebugContextG.RegionId = i;

                if (inspectRegion != null)
                {
                    OffsetStruct offsetStruct = inspectRegion.SetImage(this.algoImage, inspImage, this.calculatorResultGray, this.calculatorResultBinal, this.edgeMapImage, this.maskImage, patternOffset, regionDebugContextG);
                    SetLocalOffset(i, offsetStruct.IsGood, offsetStruct.Position, offsetStruct.BaseF, offsetStruct.OffsetF, offsetStruct.VariationF, offsetStruct.Score, offsetStruct.ImageD);
                }
            });
        }

        private void SetPatternOffset(bool result, PointF basePt, PointF offset, ImageD imageD)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("ProcessBufferSetG2::SetPatternOffset - Result: {0}, Offset: ({1}, {2})", result, offset.X, offset.Y));
            this.offsetStructSet.PatternOffset.Set(result, "", basePt, offset, Size.Empty, 0, imageD);
            if (result)
                globalOffsetStructSet.PatternOffset.Set(result, "", basePt, offset, Size.Empty, 0, imageD);
        }

        private void SetLocalOffset(int index, bool result, string position, PointF basePt, PointF offset, SizeF size, float score, ImageD imageD)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("ProcessBufferSetG2::SetLocalOffset - Index: {0}, Result: {1}, Offset: ({2}, {3}), Scale: ({4}, {5}), Score: {6}",
                index, result, offset.X, offset.Y, size.Width, size.Height, score));
            this.offsetStructSet.LocalOffsets[index].Set(result, position, basePt, offset, size, score, imageD);
            if (result)
                globalOffsetStructSet.LocalOffsets[index].Set(result, "", basePt, offset, size, score, imageD);
        }

        public void StartPreviewBitmapBuild(Size fullImageSize)
        {
            this.isWorkDone.Reset();
            LogHelper.Info(LoggerType.Algorithm, "ProcessBufferSetG2::StartPreviewBitmapBuild");

            this.bitmapBuildTask = Task.Run(() =>
             {
                 Rectangle previewBufferRect = new Rectangle(Point.Empty, this.previewBuffer.Size);
                 Size roiSize = Size.Round(new SizeF(fullImageSize.Width * previewScale, fullImageSize.Height * previewScale));
                 Rectangle roiRect = new Rectangle(Point.Empty, roiSize);
                 roiRect.Intersect(previewBufferRect);

                 LogHelper.Info(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - RoiRect W{0}, H{1}", roiRect.Width, roiRect.Height));
                 if (roiRect.Width > 0 && roiRect.Height > 0)
                 {
                     AlgoImage previewChildBuffer = this.previewBuffer.GetSubImage(roiRect);
                     //calculatorResult.PreviewImageD = BuildPreviewImageD(previewChildBuffer, null);
                     try
                     {
                         this.prevBitmap = previewChildBuffer.ToBitmap();
                         //this.prevBitmap = BuildPreviewBitmap(previewChildBuffer, null);
                         LogHelper.Info(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - Ok"));
                     }
                     catch (Exception ex)
                     {
                         LogHelper.Error(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - Exception {0}. {1}", ex.Message, ex.StackTrace));
                     }
                     previewChildBuffer.Dispose();
                 }
                 //calculatorResult.PrevImage = BuildPreviewBitmap(previewBuffer, null);

                 this.isWorkDone.Set();
             });
            //this.bitmapBuildTask.Start();
        }

        private ImageD BuildPreviewImageD(AlgoImage previewBuffer, DebugContext debugContext)
        {
            //return previewBuffer.ToImageD();

            int width = previewBuffer.Width;
            int height = previewBuffer.Height;
            int pitch = previewBuffer.Pitch;
            PixelFormat pixelFormat = previewBuffer.ImageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            int numBand = previewBuffer.ImageType == ImageType.Color ? 3 : 1;
            byte[] bytes = previewBuffer.GetByte();

            //previewBuffer.Save(@"C:\temp\previewBuffer.bmp");
            Image2D image2D = new Image2D(width, height, numBand, pitch, bytes);
            //image2D.SaveImage(@"C:\temp\image2D.bmp");
            //image2D.ConvertFromDataPtr();
            return image2D;
        }

        private Bitmap BuildPreviewBitmap(AlgoImage previewBuffer, DebugContext debugContext)
        {
            int width = previewBuffer.Width;
            int height = previewBuffer.Height;
            int pitch = previewBuffer.Pitch;
            PixelFormat pixelFormat = previewBuffer.ImageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            IntPtr imagePtr = previewBuffer.Ptr;
            Bitmap bitmap = new Bitmap(width, height, pitch, pixelFormat, imagePtr);

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            Bitmap previewBitmap = new Bitmap(stream);
            ColorPalette cp = previewBitmap.Palette;
            for (int i = 0; i < cp.Entries.Length; i++)
                cp.Entries[i] = Color.FromArgb(255, i, i, i);
            previewBitmap.Palette = cp;
            return previewBitmap;

            //{
            //    previewBitmap = previewBuffer.ToBitmap();
            //    Debug.WriteLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //}

            //{
            //    System.Windows.Media.Imaging.BitmapSource bitmapSource = resizeSheetImage.ToBitmapSource();
            //    sb.AppendLine(string.Format("ToBitmapSource: {0}", sw.Elapsed.TotalMilliseconds));
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
            //        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //        bmpBitmapEncoder.Save(stream);

            //        previewBitmap = new Bitmap(stream);
            //    }
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //    previewBitmap?.Save(@"C:\temp\previewBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //}

            //{
            //    ImageD previewImageD = resizeSheetImage.ToImageD();
            //    sb.AppendLine(string.Format("ToImageD: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewBitmap = previewImageD.ToBitmap();
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewImageD.Dispose();
            //}
        }

        public override void Download()
        {
            //if (this.detectorInsp != null)
            //    DynMvp.Vision.ImageConverter.Convert(this.calculatorResult, this.detectorInsp);
        }

        public override void Clear()
        {
            this.previewBuffer?.Clear();
            this.scaledImage?.Clear();
            //this.EdgeFinderBuffer?.Clear();
            this.edgeMapImage?.Clear();
            this.maskImage.Clear();
            Array.ForEach(this.inspectRegions, f => f?.ClearImage());

            base.Clear();
        }

        public override void WaitDone()
        {
            //Stopwatch sw = Stopwatch.StartNew();
            this.isWorkDone.WaitOne();
            //Debug.WriteLine(string.Format("ProcessBufferSetG2::WaitDone {0}", sw.Elapsed.TotalMilliseconds));
        }
    }
}
