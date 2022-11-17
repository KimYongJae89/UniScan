using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraFile : Camera
    {
        int triggerDelayMs = 0;
        ThreadHandler threadHandler = null;
        ImageD grabbedImage = null;

        public CameraFile(CameraInfo cameraInfo) :base(cameraInfo)
        {

        }

        public override void Initialize(bool calibrationMode)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize File Camera");
            base.Initialize(calibrationMode);
        }

        public override float GetAcquisitionLineRate()
        {
            return 0;
        }

        public override float GetDeviceExposureMs()
        {
            return 0;
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            return grabbedImage;
        }

        public override int GetImageBufferCount()
        {
            return 1;
        }

        public override List<ImageD> GetImageBufferList()
        {
            return new ImageD[] { grabbedImage }.ToList();
        }

        public override void GrabMulti(int grabCount = -1)
        {
            if (this.SetupGrab())
            {
                this.grabbedCount = 0;
                this.remainGrabCount = grabCount;

                if (this.threadHandler == null || !this.threadHandler.WorkingThread.IsAlive)
                {
                    this.threadHandler = new ThreadHandler("CameraFile", new System.Threading.Thread(ThreadProc));
                    this.threadHandler.Start();
                }
            }
        }
        public override bool SetupGrab()
        {
            FileInfo[] imageFileInfos = new DirectoryInfo(this.cameraInfo.VirtualImagePath).GetFiles(this.CameraInfo.VirtualImageNameFormat);
            FileInfo[] lockFileInfos = imageFileInfos.Select(f => new FileInfo(f.FullName.Substring(0, f.FullName.Length - f.Extension.Length))).ToArray();
            Array.ForEach(lockFileInfos, f =>
            {
                if (f.Exists)
                    f.Delete();
            });

            return base.SetupGrab();
        }

        private void ThreadProc()
        {
            while (!threadHandler.RequestStop)
            {
                DirectoryInfo info = new DirectoryInfo(this.cameraInfo.VirtualImagePath);
                if (info.Exists)
                {
                    FileInfo[] imageFileInfos = new DirectoryInfo(this.cameraInfo.VirtualImagePath).GetFiles(this.CameraInfo.VirtualImageNameFormat);
                    FileInfo[] lockFileInfos = imageFileInfos.Select(f => new FileInfo(f.FullName.Substring(0, f.FullName.Length - f.Extension.Length))).ToArray();

                    int firstIndex = Array.FindIndex(lockFileInfos, f => f.Exists);
                    if (firstIndex >= 0)
                    {
                        FileInfo imageFileInfo = imageFileInfos[firstIndex];
                        FileInfo lockFileInfo = lockFileInfos[firstIndex];

                        try
                        {
                            ImageD grabbedImage = new Image2D(imageFileInfo.FullName);
                            grabbedImage.Tag = new CameraVirtualBufferTag(this.grabbedCount, grabbedImage.Size, imageFileInfo.Name);
                            //imageFileInfo.Delete();
                            lockFileInfo.Delete();

                            Thread.Sleep(this.triggerDelayMs);

                            this.grabbedImage = grabbedImage;
                            ImageGrabbedCallback(IntPtr.Zero);

                            int sleepTime = Math.Max((int)(this.exposureTimeUs / 1000), 100);
                            Thread.Sleep(sleepTime);
                        }
                        catch (Exception) { }
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }

                //if (fileInfos.Length > 0)
                //{
                //    FileInfo fileInfo = fileInfos[0];
                //    ImageD grabbedImage = new Image2D(fileInfo.FullName);
                //    grabbedImage.Tag = new CameraBufferTag(fileInfo.Name);
                //    fileInfo.Delete();

                //    Thread.Sleep(this.triggerDelayMs);

                //    this.grabbedImage = grabbedImage;
                //    ImageGrabbedCallback(IntPtr.Zero);
                //}
                //int sleepTime = Math.Max((int)(this.exposureTimeUs / 1000), 100);
                //Thread.Sleep(sleepTime);
            }
        }

        public override void GrabOnce()
        {
            GrabMulti(1);
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            return true;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            return true;
        }

        public override void Stop()
        {
            base.Stop();

            if (this.threadHandler != null)
                this.threadHandler.RequestStop = true;
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            this.triggerDelayMs = (int)Math.Round(triggerDelayUs / 1000f);
        }
    }
}
