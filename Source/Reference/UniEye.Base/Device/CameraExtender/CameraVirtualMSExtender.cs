//using DynMvp.Base;
//using DynMvp.Device.Device.FrameGrabber;
//using DynMvp.Devices.FrameGrabber;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Timers;
//using UniEye.Base.Device;
//using UniEye.Base.Settings;

//namespace UniEye.Base.Device.CameraExtender
//{
//    public class CameraVirtualMSExtender : CameraVirtualMS, ICameraExtender
//    {
//        int imageCount = -1;
//        uint maxIndexImageBuffer = 10;
//        public int ImageBufferIndex
//        {
//            get { return imageCount % (int)maxIndexImageBuffer; }
//        }

//        Image2D[] arVirtualImage = null;
//        AlgoImage[] arVirtualAlgoImage = null;

//        public override void Initialize(CameraInfo cameraInfo)
//        {
//            base.Initialize(cameraInfo);
//            if (cameraInfo is CameraInfoGenTL)
//            {
//                CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;
//                maxIndexImageBuffer = cameraInfoGenTL.FrameNum;
//            }

//            this.onGrabTimer = OnGrabTimer;
//            UpdateBuffer(this.lineModeSize.Width, this.lineModeSize.Height, (uint)this.lineModeSize.Height, maxIndexImageBuffer, true);
//        }

//        private bool OnGrabTimer()
//        {
//            int newImageBufferIndex = (imageCount + 1) % (int)maxIndexImageBuffer;
//            bool ok = CreateGrabbedImage(ref arVirtualImage[newImageBufferIndex]);
//            if (ok)
//            {
//                imageCount++;
//                UpdateBufferTag(arVirtualImage[ImageBufferIndex]);
//            }

//            return ok;
//        }

//        public override void Release()
//        {
//            base.Release();
//            ReleaseBuffer();
//        }

//        public bool UpdateBuffer(int width, int height, uint scanLines, uint count, bool forceRealloc = false)
//        {
//            ReleaseBuffer();

//            List<Image2D> image2DList = new List<Image2D>();
//            List<AlgoImage> algoImageList = new List<AlgoImage>();
//            for (int i = 0; i < maxIndexImageBuffer; i++)
//            {
//                Image2D image2D = (Image2D)this.CreateCompatibleImage();
//                image2DList.Add(image2D);

//                GCHandle handle = GCHandle.Alloc(image2D.ImageData.Data, GCHandleType.Pinned);
//                IntPtr intPtr = handle.AddrOfPinnedObject();
//                Image2D ptrImage = new Image2D(image2D.Width, image2D.Height, image2D.NumBand, image2D.Pitch, intPtr);
//                ImageType imageType = OperationSettings.Instance().UseCuda ? ImageType.Gpu : ImageType.Grey;
//                AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ptrImage, imageType);
//                algoImageList.Add(algoImage);
//                algoImage.Name = i.ToString();
//                handle.Free();

//                image2D.Tag = algoImage.Tag = new CameraBufferTag(i);
//            }

//            arVirtualImage = image2DList.ToArray();
//            arVirtualAlgoImage = algoImageList.ToArray();
//            imageCount = 0;

//            return true;
//        }

//        public override bool IsCompatibleImage(ImageD image)
//        {
//            if (image == null)
//                return false;

//            return (image.NumBand == numOfBand && image.DataSize == 1 && lineModeSize == image.Size);
//            //IsCompatibleSize(new Size(image.Width, image.Height)));
//        }

//        public override ImageD CreateCompatibleImage()
//        {
//            //return base.CreateCompatibleImage();

//            LogHelper.Debug(LoggerType.Grab, "Camera - CreateCompatibleImage");

//            Image2D image2d = new Image2D();
//            if (UseNativeBuffering == false)
//                image2d.Initialize(lineModeSize.Width, lineModeSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), lineModePitch);

//            return image2d;
//        }

//        public void ReleaseBuffer()
//        {
//            if (arVirtualAlgoImage != null)
//            {
//                foreach (AlgoImage algoImage in arVirtualAlgoImage)
//                    algoImage.Dispose();
//            }
//            arVirtualAlgoImage = null;

//            if (arVirtualImage != null)
//            {
//                foreach (Image2D image2D in arVirtualImage)
//                    image2D.Dispose();
//            }
//            arVirtualImage = null;
//        }

//        public void UpdateBufferTag(object tagSource)
//        {
//            CameraBufferTag tag = (tagSource as Image2D).Tag as CameraBufferTag;
//            tag.TimeStamp = (ulong)DateTime.Now.Ticks;
//            tag.FrameId = (ulong)this.imageCount;
//        }

//        public override ImageD GetGrabbedImage(IntPtr ptr)
//        {
//            if (ImageBufferIndex < 0)
//                return null;

//            if (ptr == IntPtr.Zero)
//                return arVirtualImage[ImageBufferIndex];

//            int idx = (int)ptr;
//            return arVirtualImage[idx];
//        }

//        public virtual AlgoImage GetAlgoImage(IntPtr ptr)
//        {
//            if (ImageBufferIndex < 0)
//                return null;

//            if (ptr == IntPtr.Zero)
//                return arVirtualAlgoImage[ImageBufferIndex];

//            int idx = (int)ptr;
//            return arVirtualAlgoImage[idx];
//        }

//        public override void SetAreaMode()
//        {
//            if (ImageSize.Equals(areaModeSize) == false)
//            {
//                ImageSize = areaModeSize;
//                UpdateBuffer(CameraInfo.Width, 0, 0, (uint)maxIndexImageBuffer);
//            }
//        }

//        public override void SetLineScanMode()
//        {
//            if (ImageSize.Equals(LineModeSize) == false)
//            {
//                ImageSize = LineModeSize;
//                UpdateBuffer(CameraInfo.Width, CameraInfo.Height, (uint)CameraInfo.Height, (uint)maxIndexImageBuffer);
//            }
//        }
//    }
//}
