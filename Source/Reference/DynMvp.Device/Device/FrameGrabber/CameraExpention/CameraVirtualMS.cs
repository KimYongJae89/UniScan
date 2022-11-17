using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Runtime.InteropServices;

using DynMvp.Base;

using System.Diagnostics;
using DynMvp.UI.Touch;
using System.Windows.Forms;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices;
using System.Threading;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class DialogState
    {
        public DialogResult result;
        public FileDialog dialog;

        public void ThreadProcShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }

    public class CameraVirtualMS : CameraVirtual
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        protected object onCreating = new object();

        List<ImageD> virtualImageList = null;

        int copySrcImageStartIdx = -1;
        int copySrcImageStartLineNo = 0;

        protected int lineModePitch;
        protected Size lineModeSize;
        protected Size areaModeSize;
        int bufferOutputImageIndex = -1;

        int remainLines = 0;
        int frameNo = 0;

        public CameraVirtualMS(CameraInfo cameraInfo) : base(cameraInfo)
        {
            this.virtualImageList = new List<ImageD>();
        }

        protected override void SetScanMode(ScanMode scanMode)
        {
            switch (scanMode)
            {
                case ScanMode.Area:
                    ImageSize = areaModeSize;
                    exposureTimeUs *= areaModeSize.Height * 1.0f / lineModeSize.Height;
                    break;
                case ScanMode.Line:
                    ImageSize = lineModeSize;
                    exposureTimeUs *= lineModeSize.Height * 1.0f / areaModeSize.Height;
                    break;
            }
        }

        [STAThreadAttribute]
        public override void Initialize(bool calibrationMode)
        {
            base.Initialize(calibrationMode);
            LogHelper.Debug(LoggerType.StartUp, "Initialize Virtual CameraMS");

            int frameBufferSize = 10;
            
            if (cameraInfo is CameraInfoGenTL)
                frameBufferSize = (int)((CameraInfoGenTL)cameraInfo).FrameNum;

            for (int i = 0; i < frameBufferSize; i++)
                this.virtualImageList.Add(null);

            lineModeSize = ImageSize;
            lineModePitch = lineModeSize.Width * NumOfBand;

            areaModeSize = new Size(ImageSize.Width, 128);

            SetScanMode(ScanMode.Line);
        }

        public override void Release()
        {
            callbackTimer.Stop();
            base.Release();
        }

        public override bool SetupGrab()
        {
            this.frameNo = -1;
            return base.SetupGrab();
        }

        protected override int UpdateVirtualImage()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraVirtualMS::UpdateVirtualImage");
            virtualOutputImageIndex = (virtualOutputImageIndex + 1) % this.virtualSoruceImageDic.Count;
            bufferOutputImageIndex = (bufferOutputImageIndex + 1) % this.virtualImageList.Count;

            ImageD imageD = this.virtualImageList[bufferOutputImageIndex];
            if (imageD == null)
                imageD = (ImageD)this.CreateCompatibleImage();
            if (this.remainLines == 0)
            {
                this.frameNo++;
                this.remainLines = imageD.Height;
            }

            Size makedSize;
            if (this.remainLines<5000)
            // GenTL 카메라 Valid Lines 이상 현상 시뮬레이션
            {
                int skipLines = 0;
                if (false)
                {
                    Random random = new Random((int)DateTime.Now.Ticks);

                    // 0.5의 확률로 전체 이미지. 
                    // 0.5 ~ 1.0 까지 0 ~ makeLines 라인씩 줄임.
                    double rand = random.NextDouble();
                    rand = 0;
                    skipLines = (int)((Math.Max(0.5, rand) - 0.5) * 2 * this.remainLines);
                    Console.WriteLine($"CameraVirtualMS::MackNextVirtualImage - SkipLines: {skipLines}");
                }

                makedSize = MakeNextVirtualImage(ref imageD, this.remainLines - skipLines);
            }
            else
            {
                makedSize = MakeNextVirtualImage(ref imageD, this.remainLines);
            }
            this.remainLines -= makedSize.Height;

            Size frameSize = new Size(makedSize.Width, imageD.Height - this.remainLines);
            imageD.Tag = new CameraVirtualBufferTag((ulong)this.frameNo, frameSize, null);
            this.virtualImageList[bufferOutputImageIndex] = imageD;

#if Debug
            string fileName = $@"C:\temp\UpdateVirtualImage\{this.frameNo}_{this.grabbedCount:000}_H{frameSize.Height}.bmp";
            using (Image2D resizeImageD = (Image2D)imageD.Resize(0.1f))
                resizeImageD.SaveImage(fileName);
#endif

            return bufferOutputImageIndex;
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (this.bufferOutputImageIndex < 0)
                return null;

            int imageIdx = this.bufferOutputImageIndex;
            if (ptr != IntPtr.Zero)
                imageIdx = (int)ptr - 1;

            ImageD imageD = this.virtualImageList[imageIdx];
            LogHelper.Debug(LoggerType.Grab, $"CameraVirtualMS::GetGrabbedImage - ptr: {ptr}, imageIdx: {imageIdx}, Tag: {imageD.Tag?.ToString()}");
            Debug.Assert(imageD != null);
            return imageD;
        }

        /// <summary>
        /// 높이가 다른이미지를 src-> dst로 카피, src가 더 작으면 다시 처음 영상으로 이어 붙여줌
        /// 다음 호출시 이전 offset 영상 위치를 기억하여 거기부터 만들어줌.
        /// </summary>
        /// <param name="dstImage"></param>
        private Size MakeNextVirtualImage(ref ImageD imageD, int makeLines)     //184~186, 372,
        {
            Debug.Assert(makeLines > 0);
            int LastYOffset = 0;
            if (virtualOutputImageIndex <0) virtualOutputImageIndex=0;
            if (copySrcImageStartIdx < 0)
            {
                copySrcImageStartIdx = 0;
                copySrcImageStartLineNo = 0;
            }

            if (Monitor.TryEnter(onCreating) == false)
                return Size.Empty;

            imageD.Clear();

            int srcImageIdx = copySrcImageStartIdx;
            int YoffsetSrc = copySrcImageStartLineNo;
            int YoffsetDst = 0; //0 base
            int copiedLines = 0;
            int skipLines = 0;

            int totalLine = makeLines - skipLines;
            while (YoffsetDst < totalLine)
            {
                ImageD srcImage = base.GetVirtualSourceImage(srcImageIdx);

                int rectWidth = Math.Min(srcImage.Width, imageD.Width);

                int srcHeight = srcImage.Height - YoffsetSrc;
                int dstHeight = totalLine - YoffsetDst;
                int rectHeight = Math.Min(srcHeight, dstHeight);

                Rectangle srcRect = new Rectangle(0, YoffsetSrc, rectWidth, rectHeight);
                Rectangle dstRect = new Rectangle(0, YoffsetDst, rectWidth, rectHeight);

                imageD.CopyFrom(srcImage, srcRect, srcImage.Pitch, dstRect.Location);

                int befr = YoffsetSrc;
                YoffsetSrc = YoffsetSrc + rectHeight;
                YoffsetDst = YoffsetDst + rectHeight;

                if (YoffsetSrc == srcImage.Height)  //다음 이미지 파일로 작업
                {
                    srcImageIdx = (++srcImageIdx % this.virtualSoruceImageDic.Count);
                    YoffsetSrc = 0;
                }
                LastYOffset = YoffsetSrc;

                copiedLines += rectHeight;
            }
            copySrcImageStartIdx = srcImageIdx;
            copySrcImageStartLineNo = LastYOffset;

            Monitor.Exit(onCreating);
            return new Size(imageD.Width, copiedLines);
        }

        public override List<ImageD> GetImageBufferList()
        {
            return new List<ImageD>(this.virtualImageList);
        }

        public override int GetImageBufferCount()
        {
            return this.virtualImageList.Count;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            base.SetDeviceExposure(exposureTimeMs);
            return true;
            //float exp = exposureTimeMs * this.ImageSize.Height;
            //return base.SetDeviceExposure(exp);
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            //base.SetDeviceExposure(1E3f / hz * this.ImageSize.Height);
            base.SetAcquisitionLineRate(hz);
            return true;
        }
    }
}