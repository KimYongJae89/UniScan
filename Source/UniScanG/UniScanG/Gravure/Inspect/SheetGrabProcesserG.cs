using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanG.Data.Model;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Inspect;
using UniScanG.Vision;
using System.Runtime.InteropServices;


namespace UniScanG.Gravure.Inspect
{
    public class Fiducial
    {
        private AlgoImage algoImage;
        private Rectangle rectangle;

        public AlgoImage AlgoImage
        {
            get { return algoImage; }
        }

        public Rectangle Rectnagle
        {
            get { return this.rectangle; }
        }

        public DateTime GrabbedDateTime
        {
            get { return ((CameraBufferTag)this.algoImage.Tag).DateTime; }
        }

        public bool IsValid
        {
            get { return this.rectangle.IsEmpty == false; }
        }

        public Fiducial(AlgoImage algoImage, Rectangle rectangle)
        {
            this.algoImage = algoImage;
            this.rectangle = rectangle;
        }

        public Fiducial(AlgoImage algoImage, SheetFinderResultRect sheetFinderResultRect)
        {
            this.algoImage = algoImage;
            this.rectangle = sheetFinderResultRect.Rectangle;
            this.rectangle.Width = algoImage.Width;
        }

        public bool IntersectWith(Fiducial fiducial, int marginX = 0, int marginY = 0)
        {
            if (this.algoImage != fiducial.algoImage)
                return false;

            if (this.rectangle.IsEmpty || fiducial.rectangle.IsEmpty)
                return true;

            Rectangle rectA = this.rectangle;
            Rectangle rectB = fiducial.rectangle;
            rectA.Inflate(marginX, marginY);
            rectB.Inflate(marginX, marginY);
            bool isIntersectsWith = rectA.IntersectsWith(rectB);
            return isIntersectsWith;
        }

        public int GetMidLine()
        {
            return (this.rectangle.Top + this.rectangle.Bottom) / 2;
        }

        public override string ToString()
        {
            CameraBufferTag cameraBufferTag = (CameraBufferTag)this.algoImage.Tag;
            if (this.IsValid)
                return string.Format("Valid({0}). H{1} ({2}~{3})", this.algoImage.Tag.ToString(), this.rectangle.Height, this.rectangle.Top, this.rectangle.Bottom);
            else
                return string.Format("Not Valid({0}). {1}", this.algoImage.Tag.ToString(), cameraBufferTag?.FrameSize.Height);
        }
    }

    public class SheetGrabProcesserG : GrabProcesserG
    {
        public class DataSet
        {
            public AlgoImage AlgoImage { get; set; }
            public CameraBufferTag Tag { get; set; }
            public float[] ProjDatas { get; set; }
        }

        public override bool IsStopAndGo => false;

        StreamWriter fiducialLogFileStream = null;
        StreamWriter patternLogFileStream = null;

        private List<int> foundFidHeightList = new List<int>();
        List<float> grabbedSheetHeightMm = new List<float>();


        public List<Fiducial> FiducialList => new List<Fiducial>(fiducialList);
        List<Fiducial> fiducialList = new List<Fiducial>();

        private double averageFoundFidHeight = 0;
        private int boundSearchHeight2 = 0;   // 이미지 접합 부분에서 검사할 높이
        private int startSearchLine = 0;// 첫 이미지의 위 500라인은 SKIP
        private float findThreshold = -1;
        private int updateCount = 0;
        SheetFinderPrecisionBuffer SheetFinderPrecisionBuffer = null;

        DataSet prevDataSet;

        float[] boundProjDatas = null;
        List<Task> saveTaskList = new List<Task>();
        Dictionary<IntPtr, AlgoImage> algoDic = new Dictionary<IntPtr, AlgoImage>();

        public Calibration Calibration { get; set; }
        public bool UseLengthFilter { get; set; }
        public bool SplitExactPattern { get; set; } = false;
        public bool PrecisionTimeTrace { get; set; }

        public SheetFinderBase Algorithm
        {
            get { return algorithm; }
            set
            {
                this.algorithm = value;
                this.foundFidHeightList?.Clear();
            }
        }
        private SheetFinderBase algorithm = null;

        //public SheetFinderBaseParam AlgorithmParam { get => (SheetFinderBaseParam)SheetFinderBase.SheetFinderBaseParam; }
        public SheetFinderBaseParam AlgorithmParam { get => (SheetFinderBaseParam)algorithm.Param; }

        private DateTime lastGrabDoneTime = DateTime.Now;

        public SheetGrabProcesserG()
        {
            this.foundFidHeightList = new List<int>();
            this.Calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (this.Calibration == null)
                this.Calibration = new ScaledCalibration(new SizeF(14, 14));
        }

        ~SheetGrabProcesserG()
        {
            Dispose();
        }

        public void Initialize(AlgorithmParam param)
        {
            this.algorithm = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
        }

        protected override void OnStarted()
        {
            LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::OnStarted");

            Debug.Assert(this.algorithm != null);

            {
                if (this.algorithm.Param == null)
                    this.boundSearchHeight2 = (int)(this.Calibration.WorldToPixel(SheetFinderBase.SheetFinderBaseParam.BoundaryHeightMm * 1000) * 1.5);
                else
                    this.boundSearchHeight2 = (int)(this.Calibration.WorldToPixel(((SheetFinderBaseParam)algorithm.Param).BoundaryHeightMm * 1000) * 1.5);
            }
            if (this.boundSearchHeight2 < 0)
                this.boundSearchHeight2 = 5000;
            this.averageFoundFidHeight = 0;

            StartLog();

            this.grabbedSheetHeightMm.Clear();

            Model model = SystemManager.Instance()?.CurrentModel;
            if (model != null && model.CalculatorModelParam.SheetSizeMm.Height > 0)
            {
                for (int i = 0; i < 10; i++)
                    this.grabbedSheetHeightMm.Add(model.CalculatorModelParam.SheetSizeMm.Height);
            }

            this.SheetFinderPrecisionBuffer = new SheetFinderPrecisionBuffer(50);
            //this.isFullImageGrabbed.Reset();
            this.findThreshold = -1;
            this.updateCount = 0;

            this.Clear();
        }

        protected override void OnStopped()
        {
            LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::OnStopped");
            this.SheetFinderPrecisionBuffer?.Dispose();
            EndLog();
        }

        public override void Clear()
        {
            LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::Clear");
            base.Clear();

            this._prevPatialImageD = null;
            this.prevDataSet = null;

            this.foundFidHeightList?.Clear();
            this.sheetImageSetList?.Clear();
            this.fiducialList.Clear();

            this.algoDic.ToList().ForEach(f =>
            {
                f.Value.Dispose();
            });
            this.algoDic.Clear();

            this.SheetFinderPrecisionBuffer?.Clear();
        }


        Image2D _prevPatialImageD = null;
        protected ImageD _MakeWholeFrameImage(ImageD imageD)
        {
            CameraBufferTag tag = (CameraBufferTag)imageD.Tag;
            CameraBufferTag _prevtag = (CameraBufferTag)_prevPatialImageD?.Tag;

            Image2D nowImage = imageD as Image2D;

            if (tag.FrameSize.Height == imageD.Size.Height && _prevPatialImageD == null)
            // 정상 이미지
            {
                return imageD;
            }
            // 비정상 이미지
            else
            {
                if (_prevPatialImageD == null)
                // 일단 보관
                {
                    _prevPatialImageD = nowImage;
                    LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::_MakeWholeFrameImage() - Frame: {tag.FrameId}, Size: {tag.FrameSize}, Reserved □");
                }
                else if (_prevtag != null && _prevtag.FrameId == tag.FrameId)//todo 여기 폭을 >> 피치로...
                                                                             // 보관된 이미지 뒤에 이어붙이기
                {
                    // 잘려진 영상 합성
                    int residual = _prevPatialImageD.Height - _prevtag.FrameSize.Height;
                    //var srcRC = new Rectangle(0, 0, _prevPatialImageD.Width, residual);
                    //var dstPT = new Point(0, _prevtag.FrameSize.Height);

                    unsafe
                    {
                        IntPtr dst = _prevPatialImageD.DataPtr + _prevtag.FrameSize.Width * _prevtag.FrameSize.Height;
                        Buffer.MemoryCopy(nowImage.DataPtr.ToPointer(), dst.ToPointer(),
                            _prevtag.FrameSize.Width * residual,
                            _prevtag.FrameSize.Width * residual);
                    }
                    _prevPatialImageD.Tag = tag;

                    if (_prevPatialImageD.Height != tag.FrameSize.Height)
                        // 또 쪼개짐..
                        return null;

                    var wholeFrame = _prevPatialImageD;
#if DEBUG
                    var bitmap = wholeFrame.ToBitmap();
                    var fileName = Path.Combine($"D:\\test\\{tag.FrameId}_Merge.bmp");
                    bitmap = ImageHelper.Resize(bitmap, 0.1f, 0.1f);
                    ImageHelper.SaveImage(bitmap, fileName, System.Drawing.Imaging.ImageFormat.Bmp);
#endif
                    // whole Frame 용 tag 작성
                    wholeFrame.Tag = tag;
                    var prevTgagg = _prevtag as CameraGenTLBufferTag;
                    if (prevTgagg != null)
                        wholeFrame.Tag = new CameraGenTLBufferTag((ulong)_prevtag.FrameId, _prevPatialImageD.Size, prevTgagg.BufferId);
                    LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::_MakeWholeFrameImage() - Frame: {_prevtag.FrameId}, Size: {_prevPatialImageD.Size}, Made whole-frame image ■");
                    _prevPatialImageD = null;
                    return wholeFrame;

                }
                else //절대 나올수 없음.
                {
                    LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::_MakeWholeFrameImage() - Frame: {tag.FrameId}, Size: {tag.FrameSize}, creep  ★");
                }
            }
            return null;
        }

        protected override SheetImageSet[] GrabProcesserRunProc(ImageD imageD)
        {
            CameraBufferTag tag = (CameraBufferTag)imageD.Tag;
            LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::GrabProcesserRunProc() - Tag: {tag}");

            // 영상이 짤려서 들어온경우 에러처리  -> 짤린이미지를 합성하여 정상적인 프레임으로 만들어 리턴, 아니면 null.
            imageD = _MakeWholeFrameImage(imageD);
            if (imageD == null)
                return null;

            Stopwatch sw = Stopwatch.StartNew();

            IntPtr intPtr = ((Image2D)imageD).DataPtr;

            AlgoImage algoImage;
            if (!this.algoDic.ContainsKey(intPtr))
                this.algoDic.Add(intPtr, ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey));
            algoImage = this.algoDic[intPtr];
            algoImage.Tag = imageD.Tag;

            Size frameSize = tag.FrameSize;
            if (frameSize.IsEmpty)
                frameSize = imageD.Size;

            if (this.IsDebugMode)
            {
                string path = Path.Combine(this.DebugPath, "Frames", $"{tag.FrameId}.png");
                Rectangle subRect = new Rectangle(Point.Empty, frameSize);
                int inflate = frameSize.Width - this.DebugWidth;
                subRect.Inflate(-inflate / 2, 0);
                using (AlgoImage subImage = algoImage.GetSubImage(subRect))
                    subImage.Save(path);
            }

            Rectangle projRect = new Rectangle(Point.Empty, frameSize);
            LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::AlgorithmParam is null: {AlgorithmParam==null}");
            if (AlgorithmParam.BaseXSearchHalf)
            {
                Size size = new Size(frameSize.Width / 6, frameSize.Height);
                Point loc = Point.Empty;
                //projRect.Inflate(-projRect.Width / 4, 0);
                switch (AlgorithmParam.GetBaseXSearchDir())
                {
                    case BaseXSearchDir.Left2Right:
                        loc = new Point((frameSize.Width / 4 * 3) - (size.Width / 2), 0);
                        break;
                    case BaseXSearchDir.Right2Left:
                        loc = new Point((frameSize.Width / 4 * 1) - (size.Width / 2), 0);
                        break;
                }
                projRect = new Rectangle(loc, size);
            }

            float[] curProjDatas;
            using (AlgoImage subI = algoImage.GetSubImage(projRect))
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                curProjDatas = ip.Projection(subI, Direction.Vertical, ProjectionType.Mean);
            }

            // 이전 프레임과 현재 프레임의 경계부분에서 검색.
            if (this.prevDataSet != null && this.findThreshold >= 0)
            {
                //Debug.Assert(this.prevDataSet.Tag.FrameSize == tag.FrameSize);
                LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::ThreadWorkProc #### - {this.prevDataSet.Tag.FrameSize} - {tag.FrameSize}");

                if (prevDataSet.Tag.FrameSize != tag.FrameSize)
                    System.Diagnostics.Trace.WriteLine($"SheetGrabProcesserG::ThreadWorkProc What's up !!!!");
                LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::ThreadWorkProc - Search in Bound Frame Between {this.prevDataSet.Tag.FrameId} - {tag.FrameId}");

                if (this.prevDataSet.ProjDatas.Length > boundSearchHeight2 && curProjDatas.Length > boundSearchHeight2)
                {
                    if (boundProjDatas == null || boundProjDatas.Length != boundSearchHeight2 * 2)
                        boundProjDatas = new float[boundSearchHeight2 * 2];
                    int srcIdx = this.prevDataSet.ProjDatas.Length - boundSearchHeight2;

                    Array.Copy(this.prevDataSet.ProjDatas, this.prevDataSet.ProjDatas.Length - boundSearchHeight2, boundProjDatas, 0, boundSearchHeight2);
                    Array.Copy(curProjDatas, 0, boundProjDatas, boundSearchHeight2, boundSearchHeight2);

                    //if (this.IsDebugMode)
                    //{
                    //    // 경계부 디버그 이미지... 중앙 {this.DebugWidth} x H [px] 이미지 저장
                    //    Rectangle lowerBoundRect = new Rectangle(0, this.prevDataSet.AlgoImage.Height - boundSearchHeight2, this.prevDataSet.AlgoImage.Width, boundSearchHeight2);
                    //    AlgoImage prevBoundAlgoImage = this.prevDataSet.AlgoImage.GetSubImage(lowerBoundRect);
                    //    Rectangle upperBoundRect = new Rectangle(0, 0, algoImage.Width, boundSearchHeight2);
                    //    AlgoImage curBoundAlgoImage = algoImage.GetSubImage(upperBoundRect);

                    //    Size boundAlgoImageSize = new Size(Math.Max(lowerBoundRect.Width, upperBoundRect.Width), lowerBoundRect.Height + upperBoundRect.Height);
                    //    using (AlgoImage boundAlgoImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, boundAlgoImageSize.Width, boundAlgoImageSize.Height))
                    //    {
                    //        ip.Stitch(prevBoundAlgoImage, curBoundAlgoImage, boundAlgoImage, Direction.Vertical);

                    //        Rectangle debugRect = new Rectangle(Point.Empty, boundAlgoImage.Size);
                    //        debugRect.Inflate(-(boundAlgoImage.Width - this.DebugWidth) / 2, 0);
                    //        using (AlgoImage debugImage = boundAlgoImage.GetSubImage(debugRect))
                    //        {
                    //            string pp = Path.Combine(DebugContext.FullPath, "Frames", $"{prevDataSet.Tag.FrameId}-{tag.FrameId}.png");
                    //            debugImage.Save(pp);
                    //        }
                    //    }

                    //    curBoundAlgoImage.Dispose();
                    //    prevBoundAlgoImage.Dispose();
                    //}

                    // 원래 이미지 및 좌표 찾아가기. Fiducial List 등록
                    DebugContextG boundDebugContextG = new DebugContextG(this.IsDebugMode, this.DebugPath) { FrameId = $"{prevDataSet.Tag.FrameId}-{tag.FrameId}" };
                    List<SheetFinderResultRect> boundFiducialRectList = FindFiducial(boundProjDatas, 0, false, boundDebugContextG);
                    UpdateFiducial(boundFiducialRectList, algoImage, this.prevDataSet.AlgoImage);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::ThreadWorkProc - Done in Bound Frame (Fiducials: {0}, ValidCount: {1})", this.fiducialList.Count, this.fiducialList.Count(f => f.IsValid)));
                }
            }

            // 현재 프레임에서 검색
            LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::ThreadWorkProc - Search in Current Frame {tag.FrameId}");
            DebugContextG rangeDebugContextG = new DebugContextG(this.IsDebugMode, this.DebugPath) { FrameId = tag.FrameId.ToString() };
            bool updateThreshold = (this.findThreshold < 0) || AlgorithmParam.FallowingThreshold;
            List<SheetFinderResultRect> curFiducialList = FindFiducial(curProjDatas, startSearchLine, updateThreshold, rangeDebugContextG);
            UpdateFiducial(curFiducialList, algoImage);

            LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::ThreadWorkProc - Done in Current Frame (Fiducials: {0}, ValidCount: {1})", this.fiducialList.Count, this.fiducialList.Count(f => f.IsValid)));

            WriteFiducialLog(this.fiducialList);

            //prevAlgoImage?.Dispose();

            this.prevDataSet = new DataSet()
            {
                AlgoImage = algoImage,
                ProjDatas = curProjDatas,
                Tag = tag,
            };
            sw.Stop();
            LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::End - Fiducial Exist Count: {this.fiducialList.Count} (Fisrt Valid ID is {this.fiducialList.FindIndex(f => f.IsValid)}), {sw.ElapsedMilliseconds:F0}[ms]");

            SheetImageSet[] sheetImageSets = CalcFiducial();

            startSearchLine = 0;
            while (saveTaskList.Exists(f => f.IsCompleted == false))
                Thread.Sleep(50);
            saveTaskList.Clear();

            return sheetImageSets;
        }

        private void UpdateFiducial(List<SheetFinderResultRect> curFiducialList, AlgoImage algoImage)
        {
            if (curFiducialList.Count == 0)
            {
                Fiducial newFiducial = new Fiducial(algoImage, Rectangle.Empty);
                bool exist = this.fiducialList.Exists(g => newFiducial.IntersectWith(g));
                if (exist == false)
                {
                    this.fiducialList.Add(newFiducial);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("Add fiducialList - {0}", newFiducial.ToString()));
                }
            }
            else
            {
                curFiducialList.ForEach(f =>
                {
                    Fiducial newFiducial = new Fiducial(algoImage, f);
                    Fiducial oldFiducial = this.fiducialList.Find(g => newFiducial.IntersectWith(g));

                    if (oldFiducial != null)
                    {
                        if (oldFiducial.Rectnagle.Height > newFiducial.Rectnagle.Height)
                            return;

                        this.fiducialList.Remove(oldFiducial);
                        LogHelper.Debug(LoggerType.Inspection, string.Format("Remove fiducialList - {0}", oldFiducial.ToString()));
                    }

                    this.fiducialList.Add(newFiducial);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("Add fiducialList - {0}", newFiducial.ToString()));
                    UpdateBoundSearchHeight(newFiducial.Rectnagle.Height);
                });
            }
        }

        private void UpdateFiducial(List<SheetFinderResultRect> boundFiducialRectList, AlgoImage algoImage, AlgoImage prevAlgoImage)
        {
            boundFiducialRectList.ForEach(f =>
            {
                Fiducial newFiducial = null;
                int midY = (f.Rectangle.Top + f.Rectangle.Bottom) / 2;
                if (midY < boundSearchHeight2)
                {
                    // 이전 이미지에서 검출됨.
                    f.Offset(0, this.prevDataSet.ProjDatas.Length - boundSearchHeight2);
                    newFiducial = new Fiducial(prevAlgoImage, f);
                }
                else
                {
                    // 현재 이미지에서 검출됨
                    f.Offset(0, -boundSearchHeight2);
                    newFiducial = new Fiducial(algoImage, f);
                }

                Fiducial oldFiducial = this.fiducialList.Find(g => newFiducial.IntersectWith(g));
                if (oldFiducial != null)
                {
                    if (oldFiducial.Rectnagle.Height > newFiducial.Rectnagle.Height)
                        return;

                    this.fiducialList.Remove(oldFiducial);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("Remove fiducialList - {0}", oldFiducial.ToString()));
                }

                this.fiducialList.Add(newFiducial);
                LogHelper.Debug(LoggerType.Inspection, string.Format("Add fiducialList - {0}", newFiducial.ToString()));

                UpdateBoundSearchHeight(newFiducial.Rectnagle.Height);
            });
        }

        private void UpdateBoundSearchHeight(int newHeight)
        {
            return;

            foundFidHeightList.Add(newHeight);
            foundFidHeightList.Sort();
            if (foundFidHeightList.Count > 10)
            {
                // 평균과 가장 멀리 떨어진 값을 제거.
                double avg = foundFidHeightList.Average();
                int min = foundFidHeightList.Min();
                int max = foundFidHeightList.Max();

                double diffMin = avg - min;
                double diffMax = max - avg;
                if (diffMin > diffMax)
                    foundFidHeightList.Remove(min);
                else
                    foundFidHeightList.Remove(max);

                float newAverageFoundFidHeight = (float)foundFidHeightList.Average();
                if (Math.Abs(newAverageFoundFidHeight - this.averageFoundFidHeight) > this.averageFoundFidHeight * 0.1f)
                    this.boundSearchHeight2 = (int)Math.Round(newAverageFoundFidHeight * 1.5);
                this.averageFoundFidHeight = newAverageFoundFidHeight;
            }
        }

        private void MergeFiducial(List<SheetFinderResultRect> curFiducialList, int marginX, int marginY)
        {
            List<SheetFinderResultRect> resultList = new List<SheetFinderResultRect>();
            List<SheetFinderResultRect> tempList = new List<SheetFinderResultRect>(curFiducialList);
            while (tempList.Count > 0)
            {
                Rectangle rectangle = tempList[0].Rectangle;

                Rectangle inflated = Rectangle.Inflate(rectangle, marginX, marginY);
                List<SheetFinderResultRect> intersectList = tempList.FindAll(f => inflated.IntersectsWith(f.Rectangle));
                tempList.RemoveAll(f => intersectList.Contains(f));

                intersectList.ForEach(f => rectangle = Rectangle.Union(rectangle, f.Rectangle));

                resultList.Add(new SheetFinderResultRect(rectangle));
            }
            curFiducialList.Clear();
            curFiducialList.AddRange(resultList);
        }

        private void FilterFiducial(List<SheetFinderResultRect> fiducialList)
        {
            double lowLimit = this.averageFoundFidHeight * 0.8;
            fiducialList.RemoveAll(f => f.Rectangle.Height < lowLimit);
        }

        private SheetImageSet[] CalcFiducial()
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::CalcFiducial Start - Valid fiducial Count: {0}", fiducialList.Count(f => f.IsValid)));
            List<SheetImageSet> sheetImageSetList = new List<SheetImageSet>();
            do
            {
                int src = this.fiducialList.FindIndex(0, f => f.IsValid);
                int dst = this.fiducialList.FindIndex(src + 1, f => f.IsValid);
                if (dst < 0)
                {
                    LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::CalcFiducial - dst is negative");
                    if (src < 0)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::CalcFiducial - src is negative");
                        this.fiducialList.Clear();
                    }
                    if (src == 0 && this.fiducialList.Count > 5)
                    {
                        // 시트가 인쇄 중단됨 -> 시작GAP은 있으나 종료GAP이 없음.
                        LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::CalcFiducial - too long gap");
                        this.fiducialList.Clear();
                    }
                    else if (src > 0)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::CalcFiducial - src is positive");
                        this.fiducialList.RemoveRange(0, src);
                    }
                    break;
                }

                //LogHelper.Debug(LoggerType.Inspection, string.Format("GrabProcesserG::CalcFiducial - Fiducial Pair Founded. Frame {0}(Height {1}) ~ Frame {2}(Height {3}), {4}Frames",
                //    ((CameraBufferTag)fiducialList[src].AlgoImage.Tag).FrameId, fiducialList[src].Rectnagle.Height,
                //    ((CameraBufferTag)fiducialList[dst].AlgoImage.Tag).FrameId, fiducialList[dst].Rectnagle.Height,
                //    ((CameraBufferTag)fiducialList[dst].AlgoImage.Tag).FrameId - ((CameraBufferTag)fiducialList[src].AlgoImage.Tag).FrameId + 1));

                int count = dst - src + 1;
                List<Fiducial> calcFiducialList = fiducialList.GetRange(src, count);
                WritePatternLog(calcFiducialList);
                SheetImageSet sheetImageSet = new SheetImageSet()
                {
                    DateTime = this.fiducialList[src].GrabbedDateTime,
                    PatternSizePx = Size.Empty
                };

                if (this.fiducialList[src].AlgoImage == this.fiducialList[dst].AlgoImage) //한장이미지에 시작과 끝 피듀셜이 모두 있으면,..
                {
                    AlgoImage algoImage = this.fiducialList[src].AlgoImage;
                    int srcY = this.fiducialList[src].GetMidLine();
                    int dstY = this.fiducialList[dst].GetMidLine();

                    LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::CalcFiducial - Frame {0}, SrcMidLine {1}, DstMidLine {2}", ((CameraBufferTag)algoImage.Tag).FrameId, srcY, dstY));

                    Rectangle clipRect = Rectangle.FromLTRB(0, srcY, algoImage.Width, dstY);
                    if (clipRect.Width > 0 && clipRect.Height > 0)
                    {
                        AlgoImage subImage = algoImage.GetSubImage(clipRect);
                        sheetImageSet.AddSubImage(subImage);
                    }
                }
                else //------------------------------------------------------------------- 다수의 이미지에 있으면 시작과 끝 피듀셜이 각각 있으며..
                {
                    int firatY = ((CameraBufferTag)fiducialList[src].AlgoImage.Tag).FrameSize.Height - fiducialList[src].Rectnagle.Bottom;
                    int lastY = fiducialList[dst].Rectnagle.Top;
                    int patternLength = 0;
                    for (int i = src; i <= dst; i++)
                    {
                        Fiducial fiducial = fiducialList[i];
                        CameraBufferTag tag = (CameraBufferTag)fiducial.AlgoImage.Tag;
                        Rectangle clipRect;
                        UInt64 frameId = ((CameraBufferTag)fiducial.AlgoImage.Tag).FrameId;
                        if (fiducial.IsValid)
                        {
                            if (!SplitExactPattern)
                            // 중심라인 ~ 중심라인까지
                            {
                                int midLine = fiducial.GetMidLine();
                                LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::CalcFiducial - Frame {frameId}, MidLine {midLine}");

                                if (i == src)
                                {
                                    clipRect = Rectangle.FromLTRB(0, midLine, tag.FrameSize.Width, tag.FrameSize.Height);
                                    patternLength += (tag.FrameSize.Height - fiducial.Rectnagle.Bottom);
                                }
                                else if (i == dst)
                                {
                                    clipRect = Rectangle.FromLTRB(0, 0, tag.FrameSize.Width, midLine);
                                    patternLength += (fiducial.Rectnagle.Top - 0);
                                }
                                else
                                {
                                    clipRect = Rectangle.FromLTRB(0, 0, tag.FrameSize.Width, tag.FrameSize.Height);
                                    patternLength += (tag.FrameSize.Height - 0);
                                }
                            }
                            else
                            // 상부 피듀설의 아래 ~ 하부 피듀셜의 위 까지
                            {
                                if (i == src)
                                    clipRect = Rectangle.FromLTRB(0, fiducial.Rectnagle.Bottom, tag.FrameSize.Width, tag.FrameSize.Height);
                                else if (i == dst)
                                    clipRect = Rectangle.FromLTRB(0, 0, tag.FrameSize.Width, fiducial.Rectnagle.Top);
                                else
                                    clipRect = Rectangle.FromLTRB(0, 0, tag.FrameSize.Width, tag.FrameSize.Height);

                                patternLength += Math.Max(0, clipRect.Height);
                            }
                        }
                        else
                        {
                            if (!SplitExactPattern)
                            {
                                LogHelper.Debug(LoggerType.Inspection, $"SheetGrabProcesserG::CalcFiducial - Frame {frameId}");
                                clipRect = new Rectangle(Point.Empty, tag.FrameSize);
                                patternLength += clipRect.Height;
                            }
                            else
                            {
                                // 두 경우를 모두 만족하는 때가 있을까?
                                Debug.Assert((i == src + 1 && firatY < 0) && (i == dst - 1 && lastY < 0));

                                if (i == src + 1 && firatY < 0)
                                    clipRect = Rectangle.FromLTRB(0, -firatY, tag.FrameSize.Width, tag.FrameSize.Height);
                                else if (i == dst - 1 && lastY < 0)
                                    clipRect = Rectangle.FromLTRB(0, 0, tag.FrameSize.Width, tag.FrameSize.Height + lastY);
                                else
                                    clipRect = new Rectangle(Point.Empty, tag.FrameSize);

                                patternLength += Math.Max(0, clipRect.Height);
                            }
                        }

                        //Debug.Assert(clipRect.Width > 0 && clipRect.Height > 0);
                        if (clipRect.Width > 0 && clipRect.Height > 0)
                        {
                            AlgoImage subAlgoImage = fiducial.AlgoImage.GetSubImage(clipRect);
                            sheetImageSet.AddSubImage(subAlgoImage);
                        }
                    }
                    sheetImageSet.PatternSizePx = new Size(sheetImageSet.Width, patternLength);

                    // 점프패턴 위치에 따라 그랩 시각을 미룸.
                    if (this.PrecisionTimeTrace)
                    {
                        double grabTimeOffset = (fiducialList[src + 1].GrabbedDateTime - fiducialList[src].GrabbedDateTime).TotalMilliseconds;
                        grabTimeOffset /= fiducialList[src].AlgoImage.Height;
                        grabTimeOffset *= (fiducialList[src].AlgoImage.Height - fiducialList[src].GetMidLine());
                        LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::CalcFiducial - GrabTime Reduction: {0}[ms]", grabTimeOffset));
                        sheetImageSet.DateTime.AddMilliseconds(-grabTimeOffset);
                    }
                }

                if (sheetImageSet.Count > 0)
                    sheetImageSetList.Add(sheetImageSet);

                sheetImageSet = null;
                fiducialList.RemoveRange(src, count - 1);
            } while (fiducialList.Count > 0);
            LogHelper.Debug(LoggerType.Inspection, "SheetGrabProcesserG::CalcFiducial - End");
            return sheetImageSetList.ToArray();
        }

        protected override bool Verify(SheetImageSet sheetImageSet)
        {
            LogHelper.Info(LoggerType.Inspection, $"SheetGrabProcesserG::Verify");

            float val = this.Calibration.PixelToWorld(sheetImageSet.Height) / 1000f;

            if (this.UseLengthFilter)
            {
                lock (this.grabbedSheetHeightMm)
                {
                    this.grabbedSheetHeightMm.Add(val);
                    this.grabbedSheetHeightMm.Sort();

                    if (this.grabbedSheetHeightMm.Count > 10)
                    {
                        float average = (float)this.grabbedSheetHeightMm.Average();
                        float diff1 = average - this.grabbedSheetHeightMm.First();
                        float diff2 = this.grabbedSheetHeightMm.Last() - average;
                        if (diff1 > diff2)
                            this.grabbedSheetHeightMm.Remove(this.grabbedSheetHeightMm.First());
                        else
                            this.grabbedSheetHeightMm.Remove(this.grabbedSheetHeightMm.Last());

                        float min = average * 0.8f;
                        float max = average * 1.2f;
                        if (!MathHelper.IsInRange(val, min, max))
                        {
                            LogHelper.Info(LoggerType.Inspection, $"SheetGrabProcesserG::Verify - false");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void StartLog()
        {
            if (!this.IsDebugMode)
                return;

            string debugPath = this.DebugPath;
            if (string.IsNullOrEmpty(debugPath))
                return;

            string path = Path.GetFullPath(debugPath);
            Directory.CreateDirectory(path);
            DateTime now = DateTime.Now;

            string fiducialLogFile = Path.Combine(path, string.Format("Fiducial_{0}.txt", now.ToString("yyyyMMdd_HHmmss")));
            this.fiducialLogFileStream = new StreamWriter(fiducialLogFile, true) { AutoFlush = true };
            this.fiducialLogFileStream.WriteLine("DateTime, MemoryUsage, FiducialInfo");

            string patternLogFile = Path.Combine(path, string.Format("Pattern_{0}.txt", now.ToString("yyyyMMdd_HHmmss")));
            this.patternLogFileStream = new StreamWriter(patternLogFile) { AutoFlush = true };
            this.patternLogFileStream.WriteLine("DateTime, Height, Frames");
        }

        private void WriteFiducialLog(List<Fiducial> fiducialList)
        {
            if (this.fiducialLogFileStream == null)
                return;

            string[] tokens = fiducialList.Select(f => $"{f.AlgoImage.Tag.ToString()}, {f.Rectnagle.ToString()}").ToArray();
            string fidInfo = string.Join("\t", tokens);

            string log = $"{DateTime.Now.ToString("HH:mm:ss")}\t{GC.GetTotalMemory(false)}\t{fidInfo}";

            try
            {
                this.fiducialLogFileStream.WriteLine(log);
                this.fiducialLogFileStream.Flush();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("SheetGrabProcesserG::WriteLog - {0} {1}", ex.ToString(), ex.Message));
            }
        }

        private void WritePatternLog(List<Fiducial> fiducialList)
        {
            if (this.patternLogFileStream == null)
                return;

            Fiducial fidHead = fiducialList.First();
            Fiducial[] fidBodies = fiducialList.GetRange(1, fiducialList.Count - 2).ToArray();
            Fiducial fidTail = fiducialList.Last();

            int head = fidHead.AlgoImage.Height - fidHead.GetMidLine();
            int body = fidBodies.Sum(f => f.AlgoImage.Height);
            int tail = fidTail.GetMidLine();

            string dateTime = DateTime.Now.ToString("HH:mm:ss.fff");
            int height = head + body + tail;
            string frames = string.Join("/", fiducialList.Select(f =>
            {
                CameraBufferTag tag = ((CameraBufferTag)f.AlgoImage.Tag);
                if (!f.IsValid)
                    return $"{tag.FrameId}";
                else
                    return $"{tag.FrameId}({f.GetMidLine()})";
            }).ToArray());
            this.patternLogFileStream.WriteLine($"{dateTime}, {height}, {frames}");
            this.patternLogFileStream.Flush();
        }

        private void EndLog()
        {
            this.fiducialLogFileStream?.Close();
            this.fiducialLogFileStream?.Dispose();
            this.fiducialLogFileStream = null;

            this.patternLogFileStream?.Close();
            this.patternLogFileStream?.Dispose();
            this.patternLogFileStream = null;
        }

        private void AppendLog(SheetImageSet tempSheetImageSet)
        {
            if (!this.IsDebugMode)
                return;

            Directory.CreateDirectory(this.DebugPath);

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} ", tempSheetImageSet.Name));
            //tempSheetImageSet.subImageList.ForEach(f => sb.Append(string.Format("\t{0}", f.Tag)));

            //StreamWriter streamWriter = new StreamWriter(this.logSheetFile, true);
            //streamWriter.WriteLine(sb.ToString());
            //streamWriter.Close();
            //streamWriter.Dispose();
        }

        private void AppendLog(ulong frameId, int bufferId, List<Point> fidPointList)
        {
            if (!this.IsDebugMode)
                return;

            Directory.CreateDirectory(this.DebugPath);

            StringBuilder sb = new StringBuilder();
            fidPointList.ForEach(f => sb.AppendLine(string.Format("Frame{0} Buffer{1} Position{2}", frameId, bufferId, f.ToString())));

            //StreamWriter streamWriter = new StreamWriter(this.logFiducialFile, true);
            //streamWriter.Write(sb.ToString());
            //streamWriter?.Close();
        }

        private List<SheetFinderResultRect> FindFiducial(float[] projDatas, int startSearch, bool updateThreshold, DebugContextG debugContextG)
        {
            List<SheetFinderResultRect> sheetFinderResultRectList = new List<SheetFinderResultRect>();

            if (startSearch > 0)
                projDatas = projDatas.Skip(startSearch).ToArray();

            SheetFinderInspectParam sheetInspectParam = new SheetFinderInspectParam(projDatas, this.SheetFinderPrecisionBuffer, this.Calibration, debugContextG)
            {
                PatternFinderThreshold = this.findThreshold,
                SkipOutter = updateThreshold
            };

            SheetFinderResult sheetFinderResult = (SheetFinderResult)algorithm.Inspect(sheetInspectParam);

            int foundCount = sheetFinderResult.SheetFinderResultRectList.Count;
            float threshold = sheetFinderResult.Threshold;
            float brightnessDev = sheetFinderResult.BrightnessDev;
            float intensityDiff = sheetFinderResult.IntensityDiff;
            LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::FindFiducial - Size {0}[px], StdDev {1}, threshold {2}, Count {3}, Time {4}ms. ",
                projDatas.Length, brightnessDev, threshold, foundCount, sheetFinderResult.SpandTime.TotalMilliseconds));

            foreach (SheetFinderResultRect sheetFinderResultRect in sheetFinderResult.SheetFinderResultRectList)
            {
                sheetFinderResultRect.Offset(0, startSearch);
                LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::FindFiducial - Founded Y{0}, H{1}", sheetFinderResultRect.Rectangle.Y, sheetFinderResultRect.Rectangle.Height));
                sheetFinderResultRectList.Add(sheetFinderResultRect);
            }

            if (updateThreshold && threshold >= 0 && foundCount > 0)
            {
                this.findThreshold = ((this.findThreshold * this.updateCount) + threshold) / (this.updateCount + 1);
                //if (this.findThreshold < 0)
                //    this.findThreshold = threshold;
                //else
                //    this.findThreshold = (this.findThreshold * 9 + threshold) / 10;
                this.updateCount++;
                LogHelper.Debug(LoggerType.Inspection, string.Format("SheetGrabProcesserG::FindFiducial - Threshold Updated. Threshold: {0}, Count {1}", this.findThreshold, this.updateCount));
            }

            if (this.IsDebugMode)
            {
                string debugFile = Path.Combine(this.DebugPath, "Frames", "Frames.txt");
                if (!File.Exists(debugFile))
                    File.WriteAllText(debugFile, "Date, FrameNo, Length, ProjThreshold, FinalThreshold, BrightnessDev");
                File.AppendAllText(debugFile, $"{Environment.NewLine}{DateTime.Now}, {debugContextG.FrameId}, {projDatas.Length}, {sheetFinderResult.Threshold}, {this.findThreshold}, {sheetFinderResult.BrightnessDev}");
            }

            MergeFiducial(sheetFinderResultRectList, 500, 500);
            FilterFiducial(sheetFinderResultRectList);
            return sheetFinderResultRectList;
        }

        public override bool IsDisposable()
        {
            return base.IsDisposable();
        }

        public override void Dispose()
        {
            LogHelper.Debug(LoggerType.Grab, $"SheetGrabProcesserG::Dispose");
            //Stop();
            Clear();
            base.Dispose();
        }
    }
}