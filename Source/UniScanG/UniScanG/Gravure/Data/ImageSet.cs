using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices.FrameGrabber;

namespace UniScanG.Gravure.Data
{
    public delegate void OnSheetImageSetDisposeDelegate(SheetImageSet sheetImageSet);
    public delegate void ImageGrabCompleteDelegate();
    public class SheetImageSet : AlgoImage, IDisposable
    {
        public List<AlgoImage> PartImageList { get => partImageList; }
        private List<AlgoImage> partImageList;

        //public event OnSheetImageSetDisposeDelegate OnSheetImageSetDispose;

        public override int Width
        {
            get { return partImageList.Count == 0 ? 0 : partImageList.Max(f => f.Width); }
        }

        public override int Height
        {
            get { return partImageList.Count == 0 ? 0 : partImageList.Sum(f => f.Height); }
        }

        public override int Pitch => throw new NotImplementedException();
        public override int BitPerPixel => throw new NotImplementedException();
        public override bool IsAllocated => throw new NotImplementedException();

        public int Count => this.partImageList.Count;
        public Size PatternSizePx { get; set; }

        public SheetImageSet()
        {
            this.partImageList = new List<AlgoImage>();
        }

        public SheetImageSet(ICollection<AlgoImage> subImageList)
        {
            this.partImageList = new List<AlgoImage>();
            AlgoImage[] arr = subImageList.ToArray();
            Array.ForEach(arr, f => AddSubImage(f));            
        }

        ~SheetImageSet()
        {
            this.Dispose();
        }

        protected override void Disposing()
        {
            foreach (AlgoImage subImage in this.partImageList)
                subImage.Dispose();

            partImageList.Clear();

            //OnSheetImageSetDispose?.Invoke(this);
        }

        public void AddSubImage(AlgoImage algoImage)
        {
            partImageList.Add(algoImage);
            if (partImageList.Count == 1)
            {
                this.LibraryType = partImageList[0].LibraryType;
                this.ImageType = partImageList[0].ImageType;
            }
        }

        //public ImagingLibrary LibraryType
        //{
        //    get { return subImageList[0].LibraryType; }
        //}


        public override AlgoImage Clone()
        {
            List<AlgoImage> subImageList = new List<AlgoImage>();
            foreach (AlgoImage subImage in this.partImageList)
            {
                subImageList.Add(subImage.Clone());
            }
            SheetImageSet sheetImageSet = new SheetImageSet(subImageList);
            sheetImageSet.Tag = this.Tag;
            return sheetImageSet;
        }

        public AlgoImage GetChildImage(int i)
        {
            return this.partImageList[i];
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            Point rectOffset = rectangle.Location;
            Size rectSize = rectangle.Size;
            int accHeigth = 0;
            List<AlgoImage> partialImageList = new List<AlgoImage>();
            foreach (AlgoImage subImage in this.partImageList)
            {
                Point offset = new Point(0, accHeigth);
                accHeigth += subImage.Height;
                Rectangle globalImageRect = new Rectangle(offset.X, offset.Y, subImage.Width, subImage.Height);

                Rectangle globalIntersectRect = Rectangle.Intersect(globalImageRect, rectangle);

                if (globalIntersectRect.Width == 0 || globalIntersectRect.Height == 0)
                    continue;

                Rectangle imageIntersectRect = globalIntersectRect;
                imageIntersectRect.Offset(-offset.X, -offset.Y);

                //if (imageIntersectRect.Size == rectangle.Size)
                //{
                //    dstImage = subImage.GetSubImage(imageIntersectRect, syncDispose);
                //}

                partialImageList.Add(subImage.GetSubImage(imageIntersectRect));
            }

            if (partialImageList.Count == 0)
            {
                dstImage = null;
            }
            else if (partialImageList.Count == 1)
            {
                dstImage = partialImageList[0];
            }
            else
            {
                AlgoImage baseAlgoImage = this.partImageList[0];
                AlgoImage subAlgoImage = ImageBuilder.Build(baseAlgoImage.LibraryType, baseAlgoImage.ImageType, rectSize.Width, rectSize.Height);
                Point dstPoint = new Point();
                foreach (AlgoImage partialAlgoImage in partialImageList)
                {
                    subAlgoImage.Copy(partialAlgoImage, Point.Empty, dstPoint, partialAlgoImage.Size);
                    dstPoint.Y += partialAlgoImage.Height;
                    partialAlgoImage.Dispose();
                }
                //partialImageList.ForEach(f => f.Dispose());

                //subAlgoImage.Save(@"D:\temp\ttt.bmp");
                dstImage = subAlgoImage;
            }
        }

        public ImageD ToImageD(float scale = 1)
        {
            ImageD imageD;
            using (AlgoImage algoImage = this.GetFullImage(scale))
                imageD = (Image2D)algoImage.ToImageD();
            
            return imageD;
        }

        public Size GetFullImage(AlgoImage algoImage, float scale = 1)
        {
            return GetFullImage(algoImage, new SizeF(scale, scale));
        }

        public Size GetFullImage(AlgoImage algoImage, SizeF scale)
        {
            //LogHelper.Debug(LoggerType.Function, "SheetImageSet::GenerateWholeImage Start");

            Rectangle resizeRect = new Rectangle();
            resizeRect.Width = (int)Math.Truncate(this.Width * scale.Width);
            resizeRect.Height = (int)Math.Truncate(this.Height * scale.Height);

            if (algoImage.Width < resizeRect.Width || algoImage.Height < resizeRect.Height)
            {
                LogHelper.Error(LoggerType.Error, string.Format("SheetImageSet::GetFullImage - Size Over! MaxSize: {0}, CurSize: {1}", this.Size, resizeRect.Size));
                return Size.Empty;
            }
            //LogHelper.Debug(LoggerType.Function, string.Format("resizeFull: {0}", resizeFull));

            int offsetY = 0;
            foreach (AlgoImage subImage in this.partImageList)
            {
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subImage);

                int resizeSubWidth = (int)Math.Round(subImage.Width * scale.Width, MidpointRounding.AwayFromZero);
                int resizeSubHeight = (int)Math.Round(subImage.Height * scale.Height, MidpointRounding.AwayFromZero);
                Rectangle resizeSubRect = new Rectangle(0, offsetY, resizeSubWidth, resizeSubHeight);
                resizeSubRect.Intersect(resizeRect);

                //CameraBufferTag tag = (CameraBufferTag)subImage.Tag;

                LogHelper.Debug(LoggerType.ImageProcessing, string.Format("SheetImageSet::GetFullImage - resizeSubRect: L{0} T{1} R{2} B{3} - W{4} H{5}",
                     resizeSubRect.Left, resizeSubRect.Top, resizeSubRect.Right, resizeSubRect.Bottom, resizeSubRect.Width, resizeSubRect.Height));

                if (resizeSubRect.Width == 0 || resizeSubRect.Height == 0)
                    continue;

                Debug.Assert(resizeSubRect.Bottom <= resizeRect.Height);

                using (AlgoImage partResizeAlgoImage = algoImage.GetSubImage(resizeSubRect))
                {
                    if (subImage.Size == partResizeAlgoImage.Size)
                        partResizeAlgoImage.Copy(subImage, new Rectangle(Point.Empty, subImage.Size));
                    else
                        imageProcessing.Resize(subImage, partResizeAlgoImage);
                }

                offsetY = resizeSubRect.Bottom;
            }
            Debug.Assert(offsetY == resizeRect.Height);
            LogHelper.Debug(LoggerType.ImageProcessing, $"SheetImageSet::GetFullImage - Total Size is {resizeRect.Size}");

            algoImage.DateTime = this.DateTime;
            return resizeRect.Size;
        }

        public AlgoImage GetFullImage(float scale = 1)
        {
            return GetFullImage(new SizeF(scale, scale));
        }

        public AlgoImage GetFullImage(SizeF scale)
        {
            Size resizeFull = new Size();
            resizeFull.Width = (int)Math.Truncate(this.Width * scale.Width);
            resizeFull.Height = (int)Math.Truncate(this.Height * scale.Height);

            AlgoImage algoImage = ImageBuilder.Build(this.LibraryType, this.ImageType, resizeFull);
            GetFullImage(algoImage, scale);
            return algoImage;
        }

        public override void GetByte(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            return ToImageD(1);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            AlgoImage subImage = GetSubImage(rectangle);
            AlgoImage clipImage = subImage.Clone();
            subImage.Dispose();
            return clipImage;
        }

        public override void Save(string fileName)
        {
            AlgoImage fullImage = this.GetFullImage();
            fullImage.Save(fileName);
            fullImage.Dispose();
        }

        public override void Clear(byte initVal = 0)
        {
            lock (this.partImageList)
            {
                this.partImageList.ForEach(f => f.Dispose());
                this.partImageList.Clear();
            }
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            throw new NotImplementedException();
        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
