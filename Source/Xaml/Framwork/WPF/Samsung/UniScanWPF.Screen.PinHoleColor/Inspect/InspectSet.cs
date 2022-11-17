using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public class InspectSet : IDisposable, INotifyPropertyChanged
    {
        CancellationTokenSource source;

        Thread inspectThread;
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        ImageDevice targetDevice;

        Detector detector;
        DetectorParam detectorParam;
        Tuple<int, ConcurrentStack<AlgoImage>> bufferStack;

        Queue<Tuple<InspectResult, IntPtr>> tupleQueue = new Queue<Tuple<InspectResult, IntPtr>>();

        InspectedDelegate inspectedDelegate;

        public ImageDevice TargetDevice { get => targetDevice; }
        public bool ThreadRun { get => manualResetEvent.WaitOne(0); }
        public int QueueCount { get => tupleQueue.Count; }

        public event PropertyChangedEventHandler PropertyChanged;

        public InspectSet(ImageDevice targetDevice, Detector detector, DetectorParam detectorParam, InspectedDelegate inspectedDelegate)
        {
            this.targetDevice = targetDevice;
            this.detector = detector;
            this.detectorParam = detectorParam;
            this.bufferStack = detector.GetBufferStack(targetDevice);
            this.inspectedDelegate = inspectedDelegate;
        }

        public void ChangeParam(DetectorParam detectorParam)
        {
            lock (this.detectorParam)
                this.detectorParam = detectorParam;
        }

        public void Enqueue(Tuple<InspectResult, IntPtr> tuple)
        {
            lock (tupleQueue)
                tupleQueue.Enqueue(tuple);

            OnPropertyChanged("QueueCount");

            manualResetEvent.Set();
        }

        public void Start()
        {
            manualResetEvent.Reset();

            source = new CancellationTokenSource();

            inspectThread = new Thread(ThreadProc);
            inspectThread.IsBackground = true;
            inspectThread.Priority = ThreadPriority.Highest;
            inspectThread.Start();
        }

        public void Stop(int milliSecond = 5000)
        {
            source.Cancel();
            manualResetEvent.Set();

            if (!inspectThread.Join(TimeSpan.FromMilliseconds(milliSecond)))
            {
                inspectThread.Abort();
                LogHelper.Debug(LoggerType.Operation, "Thread abort");
            }

            while (bufferStack.Item2.IsEmpty == false)
            {
                AlgoImage buffer = null;
                bufferStack.Item2.TryPop(out buffer);
                buffer.Dispose();
            }

            tupleQueue.Clear();
        }

        public void ThreadProc()
        {
            try
            {
                while (source.IsCancellationRequested == false || tupleQueue.Count != 0)
                {
                    if (tupleQueue.Count == 0)
                        manualResetEvent.Reset();

                    manualResetEvent.WaitOne();

                    if (tupleQueue.Count == 0)
                        continue;

                    Tuple<InspectResult, IntPtr> tuple = null;
                    lock (tupleQueue)
                    {
                        if (tupleQueue.Count > 0)
                            tuple = tupleQueue.Dequeue();
                    }

                    if (tuple == null)
                        continue;

                    OnPropertyChanged("QueueCount");

                    ImageD targetImage = targetDevice.GetGrabbedImage(tuple.Item2);
                    if (targetImage == null)
                    {
                        LogHelper.Debug(LoggerType.Operation, "Image is null.");
                        continue;
                    }

                    Inspect(targetImage, tuple.Item1);
                    targetImage.Clear();

                    //UniScanWPF.Helper.WPFImageHelper.SaveBitmapSource("d:\\image.jpg",tuple.Item1.TargetImage);

                    if (inspectedDelegate != null)
                        inspectedDelegate(tuple.Item1);
                }
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Inspection, e.Message);
            }
        }

        private int GetEndFramePos(AlgoImage sourceImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sourceImage);
            float[] vProjData = imageProcessing.Projection(sourceImage, Direction.Vertical, ProjectionType.Mean);

            int endPos = -1;
            for (int i = vProjData.Length - 1; i > 0; i--)
            {
                if (vProjData[i - 1] != 0 && vProjData[i] == 0)
                    endPos = i;
            }

            return endPos;
        }
        
        public void Inspect(ImageD targetImage, InspectResult inspectResult)
        {
            AlgoImage[] buffers = new AlgoImage[bufferStack.Item1];
            lock (bufferStack)
                bufferStack.Item2.TryPopRange(buffers);

            try
            {
                if (buffers.Count() == 0)
                {
                    LogHelper.Debug(LoggerType.Operation, "Buffer is busy.");
                    return;
                }

                AlgoImage sourceImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, targetImage, ImageType.Grey);

                int endPos = GetEndFramePos(sourceImage);
                float minPos = 0;
                if (detector is PinHole.Inspect.PinHoleDetector)
                {
                    minPos = sourceImage.Width * 1.8f;
                }
                else
                {
                    minPos = sourceImage.Width * 0.9f;
                }

                if (endPos < minPos)
                {
                    sourceImage.Dispose();
                    lock (bufferStack)
                        bufferStack.Item2.PushRange(buffers);
                    return;
                }

                AlgoImage subImage = sourceImage.GetSubImage(new System.Drawing.Rectangle(0, 0, sourceImage.Width, endPos));

                DetectorResult result = detector.Detect(subImage, buffers, detectorParam);

                inspectResult.DetectorResult = result;

                if (result is Color.Inspect.ColorDetectorResult)
                {
                    inspectResult.TargetImage = ((Color.Inspect.ColorDetectorResult)result).SheetImage;

                    DynMvp.Data.ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction.ColorProduction;

                    production.AddTotal();

                    if (inspectResult.Judgment == DynMvp.InspData.Judgment.Skip)
                        production.AddPass();
                    else if (inspectResult.Judgment == DynMvp.InspData.Judgment.Accept)
                        production.AddGood();
                    else
                        production.AddNG();

                    inspectResult.ResultPath = Path.Combine(SystemManager.Instance().ProductionManager.DefaultPath, production.StartTime.ToString("yyyyMMdd"), production.LotNo, "Color", inspectResult.Index.ToString());
                }

                if (inspectResult.TargetImage == null)
                {
                    inspectResult.TargetImage = subImage.ToBitmapSource();
                }

                subImage.Dispose();
                sourceImage.Dispose();

                lock (bufferStack)
                    bufferStack.Item2.PushRange(buffers);
            }
            catch (Exception e)
            {
                lock (bufferStack)
                    bufferStack.Item2.PushRange(buffers);

                LogHelper.Debug(LoggerType.Inspection, e.Message);
            }
        }

        public void Dispose()
        {
            while (bufferStack.Item2.IsEmpty == false)
            {
                AlgoImage buffer = null;
                bufferStack.Item2.TryPop(out buffer);
                buffer.Dispose();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
