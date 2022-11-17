using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanG.Gravure.Data;
using UniScanG.Inspect;


namespace UniScanG.Gravure.Inspect
{
    public abstract class GrabProcesserG : GrabProcesser
    {
        public abstract bool IsStopAndGo { get; }

        public AutoResetEvent GrabbedSignal { get; } = new AutoResetEvent(false);
        public bool IsGrabbed => this.sheetImageSetList.Count > 0;
        public bool IsRunning => this.threadHandler == null ? false : this.threadHandler.IsRunning;
        public bool IsBusy { get; private set; }

        protected SortedList<int, SheetImageSet> sheetImageSetList = null;
        protected ConcurrentQueue<ImageD> grabbedImageQueue = null;
        protected int patternNo = 0;
        protected ThreadHandler threadHandler;

        //public DebugContext DebugContext { get; protected set; }
        public string DebugPath { get; private set; }
        public bool IsDebugMode => !string.IsNullOrEmpty(this.DebugPath);
        public int DebugWidth { get; private set; } = 512;

        protected abstract void OnStarted();
        protected abstract void OnStopped();
        protected abstract SheetImageSet[] GrabProcesserRunProc(ImageD imageD);
        protected abstract bool Verify(SheetImageSet sheetImageSet);

        public GrabProcesserG()
        {
            this.sheetImageSetList = new SortedList<int, SheetImageSet>();
            this.grabbedImageQueue = new ConcurrentQueue<ImageD>();

            this.patternNo = -1;
            this.threadHandler = null;
        }

        public override bool IsDisposable()
        {
            return (this.threadHandler == null || !this.threadHandler.IsRunning) && (sheetImageSetList.Count == 0);
        }

        public override void Dispose()
        {
            LogHelper.Debug(LoggerType.Grab, $"GrabProcesserG::Dispose");

            this.sheetImageSetList.ToList().ForEach(f => f.Value.Dispose());
            this.sheetImageSetList.Clear();
        }

        public override void SetDebugMode(bool mode)
        {
            if (mode)
                SetDebugMode(Path.Combine(PathSettings.Instance().Temp, this.GetType().Name));
            else
                SetDebugMode("");
        }

        public override void SetDebugMode(string path)
        {
            LogHelper.Debug(LoggerType.Inspection, $"GrabProcesserG::SetDebugMode - {path}");

            this.DebugPath = path;
            if (this.IsDebugMode)
                Directory.CreateDirectory(path);
        }

        static int _Count = -1;
        Task taskSaveImage = null;
        ConcurrentQueue<ImageD> imgQ = new ConcurrentQueue <ImageD>();
        void _SaveImage(ImageD imageD)
        {
            imgQ.Enqueue(imageD);
            
            
            if (taskSaveImage == null)
            {
                ImageD getImg = null;
                taskSaveImage = Task.Run(() =>
                {
                    while (true)
                    {
                        if (imgQ.TryDequeue(out getImg))
                        {
                            _Count++;

                            CameraBufferTag tag = (CameraBufferTag)getImg.Tag;
                            var bitmap = getImg.ToBitmap();//.Save(Path.Combine($"d:\\{_Count}.bmp"));
                            var fileName = Path.Combine($"d:\\Test\\{tag.FrameId}_({_Count}).bmp");
                            bitmap = ImageHelper.Resize(bitmap, 0.1f, 0.1f);
                            ImageHelper.SaveImage(bitmap, fileName, System.Drawing.Imaging.ImageFormat.Bmp);


                        }
                        else Thread.Sleep(10);
                        if (threadHandler == null && imgQ.Count == 0)
                        {
                            taskSaveImage = null;
                            _Count = -1;
                            return;
                        }
                    }
                });
            }
        }

        public override sealed void ImageGrabbed(ImageD imageD)
        {
            LogHelper.Debug(LoggerType.Grab, $"GrabProcesserG::ImageGrabbed - Queue Size Before Enqueue: {this.grabbedImageQueue.Count}");

            imageD.ConvertFromData();   // 이거로 DataPtr을 정해주면 Mil에서 Alloc하지 않고 Pointer로 참조해감. (가상카메라에서 더 빨라짐)
           
            this.grabbedImageQueue.Enqueue(imageD);
#if DEBUG
            //_SaveImage(imageD);
#endif
        }

        public override sealed void Start()
        {
            LogHelper.Debug(LoggerType.Grab, $"GrabProcesserG::Start");

            //string debugPath = Path.Combine(PathSettings.Instance().Temp, "GrabProcesser");
            //this.DebugContext = new DebugContext(this.IsDebugMode, this.DebugPath);

            OnStarted();
            this.patternNo = -1;

            this.threadHandler = new ThreadHandler("GrabProcesserG", new Thread(ThreadProc));
            this.threadHandler.Start();
        }

        public override sealed void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, $"GrabProcesserG::Stop");

            this.threadHandler?.Stop();
            this.threadHandler = null;

            OnStopped();
        }


        private void ThreadProc()
        {
            while (!this.threadHandler.RequestStop)
            {
                try
                {
                    if (!this.grabbedImageQueue.TryDequeue(out ImageD imageD))
                    {
                        IsBusy = false;
                        Thread.Sleep(50);
                        continue;
                    }

                    IsBusy = true;

                    CameraBufferTag tag = (CameraBufferTag)imageD.Tag;
                    if (this.IsDebugMode)
                        imageD.SaveImage(Path.Combine(this.DebugPath, "Frames", $"{tag.FrameId}.png"));

                    SheetImageSet[] sets = GrabProcesserRunProc(imageD);
                    Array.ForEach(sets, f =>
                    {
                        if (Verify(f))
                            GrabDone(f);
                        else
                            f.Dispose();
                    });
                }catch(Exception ex)
                {
                    LogHelper.Error(LoggerType.Grab, ex);
                }
            }

            while (this.grabbedImageQueue.TryDequeue(out ImageD image)) ;
            IsBusy = false;
        }

        private void GrabDone(SheetImageSet sheetImageSet)
        {
            LogHelper.Debug(LoggerType.Grab, $"GrabProcesserG::GrabDone");
            IntPtr patNo;
            lock (this.sheetImageSetList)
            {
                patternNo++;
                patNo = (IntPtr)patternNo;
                this.sheetImageSetList.Add(patternNo, sheetImageSet);

                if (this.IsDebugMode)
                {
                    string path = Path.Combine(this.DebugPath, "Patterns", $"{patternNo}.png");
                    Rectangle subRect = new Rectangle(Point.Empty, sheetImageSet.Size);
                    int inflate = sheetImageSet.Width - this.DebugWidth;
                    subRect.Inflate(-inflate / 2, 0);
                    using (AlgoImage subImage = sheetImageSet.GetSubImage(subRect))
                        subImage.Save(path);
                }
            }
            GrabbedSignal.Set();
            StartInspectionDelegate?.Invoke(null, patNo);
        }

        public SheetImageSet GetSheetImageSet(int patternNo)
        {
            lock (this.sheetImageSetList)
            {
                if (this.sheetImageSetList.Count == 0)
                    return null;

                if (patternNo < 0)
                    patternNo = this.sheetImageSetList.Last().Key;

                if (this.sheetImageSetList.ContainsKey(patternNo))
                    return this.sheetImageSetList[patternNo];

                return null;
            }
        }

        public virtual void Clear()
        {

            LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::Clear");
            lock (this.sheetImageSetList)
            {
                foreach (SheetImageSet imageSet in this.sheetImageSetList.Values)
                    imageSet.Dispose();
                this.sheetImageSetList.Clear();
            }
        }

        public SheetImageSet GetSheetImageSet()
        {
            return GetSheetImageSet(-1);
        }

        public bool ExistSheetImageSet(int patternNo)
        {
            lock (this.sheetImageSetList)
                return this.sheetImageSetList.ContainsKey(patternNo);
        }

        public void RemoveSheetImageSet(int patternNo)
        {
            lock (this.sheetImageSetList)
                this.sheetImageSetList.Remove(patternNo);
        }

        public override int GetBufferCount()
        {
            return this.grabbedImageQueue.Count() + (this.IsBusy ? 1 : 0);
        }
    }
}
