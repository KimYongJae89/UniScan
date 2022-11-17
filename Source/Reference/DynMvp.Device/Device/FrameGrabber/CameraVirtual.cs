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
using DynMvp.UI;
using System.ComponentModel;
using System.Text.RegularExpressions;
using DynMvp.Device.Device.FrameGrabber;

using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public delegate bool OnGrabTimerDelegate();

    public class CameraVirtualBufferTag : CameraBufferTag
    {
        public string FileName => fileName;
        private string fileName = "";

        public CameraVirtualBufferTag(UInt64 frameId, Size frameSize, string fileName) : base(frameId, frameSize)
        {
            this.fileName = fileName;
        }
    }

    public class CameraVirtual : Camera
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        List<TimeSpan> grabTermList = new List<TimeSpan>();
        
        public override bool IsVirtual => true;
        /// <summary>
        /// Grab Timer 경과 후 가상 이미지가 준비될대까지 Grab 차단
        /// timer가 빠른 경우 이후 이미지가 이전 이미지를 앞서 ImageGrabbed 처리되는 경우 있음 (timer callback은 한번에 하나씩만 처리)
        /// </summary>
        protected bool virtualCameraReady = false;
        protected System.Timers.Timer callbackTimer = new System.Timers.Timer();
        protected bool requestStopGrab = false;

        protected Dictionary<string, ImageD> virtualSoruceImageDic = null;
        protected int virtualOutputImageIndex = -1;

        public CameraVirtual(CameraInfo cameraInfo) : base(cameraInfo) { }

        public override void Initialize(bool calibrationMode)
        {
            base.Initialize(calibrationMode);
            LogHelper.Debug(LoggerType.StartUp, "Initialize Virtual Camera");
            
            ImageSize = new Size(cameraInfo.Width, cameraInfo.Height);           
            NumOfBand = cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3;
            ImagePitch = cameraInfo.Width * NumOfBand;

            virtualSoruceImageDic = new Dictionary<string, ImageD>();

            string virtualImagePath = cameraInfo.VirtualImagePath;
            if (string.IsNullOrEmpty(virtualImagePath))
                virtualImagePath = Path.GetFullPath(@"..\VirtualImage");
            
            string[] vImagePaths = Directory.GetFiles(virtualImagePath, this.CameraInfo.VirtualImageNameFormat, SearchOption.TopDirectoryOnly);
            if (vImagePaths.Length == 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("There is no Exist Image Files");
                sb.AppendLine(cameraInfo.VirtualImagePath);
                //MessageForm.Show(null, sb.ToString());
            }
            else
            {
                // loadNow: 가상 이미지를 바로 로드한다. NO이면 Grab 할 때 로드한다.
                //bool loadNow = MessageForm.Show(null, StringManager.GetString("Load VirtualImage NOW?"), "UniScan", MessageFormType.YesNo, 3000, System.Windows.Forms.DialogResult.No) == System.Windows.Forms.DialogResult.Yes;
                bool loadNow = false;

                ProgressForm progressForm = new ProgressForm();
                progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                progressForm.TitleText = StringManager.GetString("Load Image");
                progressForm.MessageText = string.Format("0 / {0}", vImagePaths.Length);

                progressForm.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler((sender, arg) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    for (int i = 0; i < vImagePaths.Length; i++)
                    {
                        if (worker.CancellationPending)
                        {
                            arg.Cancel = true;
                            loadNow = false;
                        }

                        worker.ReportProgress((i + 1) * 100 / vImagePaths.Length);
                        progressForm.MessageText = string.Format("{0} / {1} ({2})", i + 1, vImagePaths.Length, Path.GetFileName(vImagePaths[i]));
                        string imageFilePath = vImagePaths[i];
                        LogHelper.Debug(LoggerType.StartUp, string.Format("CameraVirtual::Initialize - {0}", imageFilePath));
                        Image2D image2D = null;
                        if (loadNow)
                        {
                            image2D = new Image2D();
                            image2D.LoadImage(imageFilePath);

                           // image2D.SaveImage("D:\\loaded.bmp");
                            //image2D.RotateFlip(this.CameraInfo.RotateFlipType.ConvertTo());
                        }
                        virtualSoruceImageDic.Add(imageFilePath, image2D);
                    }
                });

                progressForm.ShowDialog();
            }

            virtualOutputImageIndex = -1;

            callbackTimer.Interval = 5000;
            callbackTimer.Elapsed += new ElapsedEventHandler(callbackTimer_Elapsed);
        }

        public void FilterVirtualSource(string searchPattern)
        {
            List<string> pathList = virtualSoruceImageDic.Keys.ToList();
            if (pathList.Count == 0)
                return;
            pathList.RemoveAll(f => !Regex.IsMatch(f, searchPattern));
            pathList.ForEach(f => virtualSoruceImageDic.Remove(f));
        }

        public override bool SetStepLight(int stepNo, int lightNo)
        {
            string searchPattern = String.Format("*S{1:000}_L{2:00}*", Index, stepNo, lightNo);
            //int idx = this.virtualSoruceImageDic.ToList().FindIndex(f => Path.GetFileNameWithoutExtension(f.Key) == searchPattern);

            string pat = "^" + Regex.Escape(searchPattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            int idx = this.virtualSoruceImageDic.ToList().FindIndex(f => Regex.IsMatch(Path.GetFileName(f.Key), pat));
            if (idx == -1)
            {
                Debug.WriteLine(string.Format("CameraVirtual::SetSetpLight({0}, {1}) Fail",stepNo,lightNo));
                virtualOutputImageIndex = -1;
                return false;
            }

            virtualOutputImageIndex = idx - 1;

#if DEBUG
            //string key = virtualSoruceImageDic.ElementAt(idx).Key;
            //virtualSoruceImageDic[key]?.Dispose();
            //virtualSoruceImageDic[key] = null;
#endif
            return true;
        }

        public override void Release()
        {
            base.Release();

            callbackTimer.Stop();

            foreach (KeyValuePair<string, ImageD> pair in virtualSoruceImageDic)
            {
                pair.Value?.Dispose();
            }
            virtualSoruceImageDic.Clear();
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);
        }

        public override void SetTriggerDelay(int triggerDelay)
        {

        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (virtualOutputImageIndex < 0)
                return null;

            int imageIdx = virtualOutputImageIndex;
            if (ptr != IntPtr.Zero)
                imageIdx = (int)ptr - 1;

            KeyValuePair<string, ImageD> pair = this.virtualSoruceImageDic.ElementAt(imageIdx);
            ImageD image = this.virtualSoruceImageDic[pair.Key];
            Debug.Assert(image != null);
            return image;
        }

        public override bool SetupGrab()
        {
            //this.virtualOutputImageIndex = -1;
            return base.SetupGrab();
        }

        public override void GrabOnce()
        {
            if (this.SetupGrab())
            {
                this.grabbedCount = 0;
                this.remainGrabCount = 1;
                //this.SetDeviceExposure(this.exposureTimeUs / 1000);

                requestStopGrab = false;
                virtualCameraReady = true;
                callbackTimer.Start();
            }
        }

        public override void GrabMulti(int grabCount = CONTINUOUS)
        {
            if (this.SetupGrab())
            {
                this.grabbedCount = 0;
                this.remainGrabCount = grabCount;
                //this.SetDeviceExposure(this.exposureTimeUs / 1000);

                requestStopGrab = false;
                virtualCameraReady = true;
                callbackTimer.Start();
            }
        }

        public override void Stop()
        {
            base.Stop();
            requestStopGrab = true;
            callbackTimer.Stop();
            remainGrabCount = 0;
        }

        //DateTime dt = DateTime.MinValue;
        void callbackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Thread.Sleep(1000);
            Debug.WriteLine(string.Format("[{0}] CameraVirtual::callbackTimer_Elapsed - {1}", DateTime.Now.ToLongTimeString(), this.Index));
            //callbackTimer.Stop();
            //DateTime now = DateTime.Now;
            //dt = now;
            if (this.virtualSoruceImageDic.Count < 1)
            {
                Stop();
                LogHelper.Debug(LoggerType.Error, "Virtual Source is Nothing");
                MessageForm.Show(null, "Virtual Source is Empty");
                return; 
            }

            //LogHelper.Debug(LoggerType.Function, string.Format("CameraVirtual::callbackTimer_Elapsed - CamIndex: {0}",this.Index));
            //Debug.WriteLine(string.Format("CameraVirtual::callbackTimer_Elapsed - {0}", this.Index));
            if (this.exposureTimeUs > 0)
            {
                //callbackTimer.Stop();
                //this.callbackTimer.Interval = this.exposureTimeUs / 1000;
                //callbackTimer.Start();
            }

            if (virtualCameraReady == false)
            {
                LogHelper.Error(LoggerType.Grab, string.Format("CameraVirtual::callbackTimer_Elapsed - virtualCameraReady is False. CamIndex: {0}", this.Index), true);
                return;
            }

            //ErrorManager.Instance().Report(new AlarmException(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Fatal, this.name, "Grabber not responding.", null, ""));
            //return;
            virtualCameraReady = false;

            int bufferId;
            lock (this)
            {
                bufferId = UpdateVirtualImage();
                LogHelper.Debug(LoggerType.Grab, $"CameraVirtual::callbackTimer_Elapsed - GrabbedCount: {this.grabbedCount}, BufferId: {bufferId}");
            }
            this.isGrabbed.Set();

            ImageGrabbedCallback((IntPtr)bufferId + 1);  // ptr == grabbed image index. 1-base

            virtualCameraReady = true;
            this.isGrabDone.Set();
        //    if(grabbedCount !=  (ulong)remainGrabCount)
          //      callbackTimer.Start();

        }

        protected virtual int UpdateVirtualImage()
        {
            virtualOutputImageIndex = (virtualOutputImageIndex + 1) % this.virtualSoruceImageDic.Count;
            GetVirtualSourceImage(virtualOutputImageIndex);
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraVirtual::UpdateVirtualImage - virtualOutputImageIndex: {0}", this.virtualOutputImageIndex));
            return virtualOutputImageIndex;
        }

        protected ImageD GetVirtualSourceImage(int index)
        {
            KeyValuePair<string, ImageD> pair = this.virtualSoruceImageDic.ElementAt(index);
            ImageD imageD = pair.Value;
            // pair.value가 null인 경우, dictionary value에 개채를 넣어도 pair.value값이 바뀌지 않음.
            if (imageD == null)
            {
                imageD = new Image2D(pair.Key);
                this.virtualSoruceImageDic[pair.Key] = imageD;
            }

            this.virtualSoruceImageDic[pair.Key].Tag = new CameraVirtualBufferTag(this.grabbedCount, imageD.Size, pair.Key);
            return this.virtualSoruceImageDic[pair.Key];
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs < 0)
                return false;

            this.exposureTimeUs = exposureTimeMs * 1000;
            //callbackTimer.Interval = Math.Max(1, (int)exposureTimeMs);

            return true;
        }

        public override List<ImageD> GetImageBufferList()
        {
            return virtualSoruceImageDic.Values.ToList();
            throw new NotImplementedException();
        }

        public override int GetImageBufferCount()
        {
            return virtualSoruceImageDic.Count;
        }

        public override float GetDeviceExposureMs()
        {
            return this.exposureTimeUs / 1000;
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            //this.SetExposureTime(1E6f / hz * this.ImageSize.Height);
            callbackTimer.Interval = 1000 / (hz / ImageSize.Height);
            return true;
        }

        public override float GetAcquisitionLineRate()
        {
            return (this.ImageSize.Height * 1E6f) / this.exposureTimeUs;
        }
    }
}
