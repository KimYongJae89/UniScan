using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation
{
    public class Buffer : IDisposable
    {
        const int bufferHeight = 55000;

        AlgoImage fullImage;
        AlgoImage image;
        int needLine;

        public AlgoImage FullImage { get => fullImage; }
        public AlgoImage Image
        {
            get
            {
                Debug.Assert(this.image != null);
                return this.image;
            }
        }

        public bool IsFull { get => needLine == 0; }
        public bool IsEmpty { get => needLine == DeveloperSettings.Instance.BufferHeight; }

        public Buffer()
        {
            this.fullImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
        }

        public void Clear()
        {
            needLine = DeveloperSettings.Instance.BufferHeight;
            fullImage.Clear();
        }

        public void AddImage(AlgoImage grabbedImage, bool flipY)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grabbedImage);

            System.Drawing.Point srcPt = System.Drawing.Point.Empty;
            System.Drawing.Point dstPt = System.Drawing.Point.Empty;
            System.Drawing.Size size = System.Drawing.Size.Empty;
            if (flipY)
            {
                imageProcessing.Flip(grabbedImage, grabbedImage, DynMvp.Vision.Direction.Vertical);
                srcPt = new System.Drawing.Point(0, Math.Max(0, grabbedImage.Height - needLine));
                dstPt = new System.Drawing.Point(0, Math.Max(0, needLine - grabbedImage.Height));
                size = new System.Drawing.Size(Math.Min(grabbedImage.Width, DeveloperSettings.Instance.BufferWidth), Math.Min(grabbedImage.Height, needLine));
            }
            else
            {
                srcPt = new System.Drawing.Point();
                dstPt = new System.Drawing.Point(0, DeveloperSettings.Instance.BufferHeight - needLine);
                size = new System.Drawing.Size(Math.Min(grabbedImage.Width, DeveloperSettings.Instance.BufferWidth), Math.Min(grabbedImage.Height, needLine));
            }
            fullImage.Copy(grabbedImage, srcPt, dstPt, size);
            //grabbedImage.Save(@"C:\temp\grabbedImage.bmp");

            needLine = Math.Max(0, needLine - size.Height);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                this.image?.Dispose();
                this.image = null;
                if (disposing)
                {
                    fullImage.Dispose();
                }

                fullImage = null;

                disposedValue = true;
            }
        }

        ~Buffer()
        {
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion

        internal void SetClipRect(Rectangle subRect)
        {
            this.image?.Dispose();
            this.image = this.fullImage.GetSubImage(subRect);
        }
    }

    class ScanBuffer : IDisposable
    {
        public Buffer BackLightBuffer { get => backLightBuffer; }
        Buffer backLightBuffer;

        public Buffer TopLightBuffer { get => topLightBuffer; }
        Buffer topLightBuffer;

        public bool IsFull
        {
            get { return backLightBuffer.IsFull && topLightBuffer.IsFull; }
        }

        public ScanBuffer()
        {
            backLightBuffer = new Buffer();
            topLightBuffer = new Buffer();
        }

        public void Clear()
        {
            backLightBuffer.Clear();
            topLightBuffer.Clear();
        }

        public void AddImage(AlgoImage grabbedImage, ScanDirection scanDirection, bool isVirtualDevice)
        {
            switch (scanDirection)
            {
                case ScanDirection.Forward:
                    backLightBuffer.AddImage(grabbedImage, false);
                    break;
                case ScanDirection.Backward:
                    topLightBuffer.AddImage(grabbedImage, !isVirtualDevice);
                    break;
            }
        }

        public void Dispose()
        {
            backLightBuffer.Dispose();
            topLightBuffer.Dispose();
        }

        internal void SetClipRect(Rectangle subRect)
        {
            backLightBuffer.SetClipRect(subRect);
            topLightBuffer.SetClipRect(subRect);
        }
    }

    public class InspectBuffer : IDisposable
    {
        public string Tag { get; set; }
        AlgoImage algoImage;

        //public bool IsUsing
        //{
        //    get => isUsing;

        //    set { lock (this) isUsing = value; }
        //}

        public AlgoImage AlgoImage { get => algoImage; }
            
        public InspectBuffer()
        {
            this.algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
        }
        
        public void Dispose()
        {
            this.algoImage.Dispose();
            this.algoImage = null;
        }

        public void Clear()
        {
            this.algoImage.Clear();
        }
    }

    class BufferManager : IDisposable
    {
        const int bufferNum = 8;
        const int MaxBufferNum = 8;

        CancellationTokenSource cancellationTokenSource;
        
        ScanBuffer[] scanBufferArray;

        AlgoImage[] sheetBufferArray;
        AlgoImage[] maskBufferArray;

        List<InspectBuffer> inspectBufferList;
        ConcurrentQueue<InspectBuffer> inspectBufferQ;

        List<IDisposable> bufferList;
        ConcurrentQueue<IDisposable> disposableQueue;

        static BufferManager instance;
        public static BufferManager Instance()
        {
            if (instance == null)
            {
                instance = new BufferManager();
            }

            return instance;
        }

        private BufferManager()
        {
            bufferList = new List<IDisposable>();

            disposableQueue = new ConcurrentQueue<IDisposable>();

            scanBufferArray = new ScanBuffer[DeveloperSettings.Instance.ScanNum];
            sheetBufferArray = new AlgoImage[DeveloperSettings.Instance.ScanNum];
            maskBufferArray = new AlgoImage[DeveloperSettings.Instance.ScanNum];

            inspectBufferList = new List<InspectBuffer>();
            inspectBufferQ = new ConcurrentQueue<InspectBuffer>();

            cancellationTokenSource = new CancellationTokenSource();
            Thread disposeThread = new Thread(new ThreadStart(DisposeProc));
            disposeThread.IsBackground = true;
            disposeThread.Priority = ThreadPriority.Lowest;
            disposeThread.Start();
        }

        public void Init()
        {
            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
                scanBufferArray[i] = new ScanBuffer();
                sheetBufferArray[i] = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
                maskBufferArray[i] = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
            }

            bufferList.AddRange(scanBufferArray);
            bufferList.AddRange(sheetBufferArray);
            bufferList.AddRange(maskBufferArray);

            for (int i = 0; i < bufferNum; i++)
            {
                InspectBuffer inspectBuffer = new InspectBuffer();
                inspectBufferList.Add(inspectBuffer);
                this.inspectBufferQ.Enqueue(inspectBuffer);
            }

            bufferList.AddRange(inspectBufferList);
        }

        public void Dispose()
        {
            bufferList.ForEach(buffer => buffer.Dispose());
        }

        public void AddDispoableObj(IDisposable disposableObj)
        {
            if (disposableObj == null)
                return;

            this.disposableQueue.Enqueue(disposableObj);
        }

        public void DisposeProc()
        {
            while (cancellationTokenSource.IsCancellationRequested == false)
            {
                IDisposable disposableObj = null;
                if (this.disposableQueue.TryDequeue(out disposableObj))
                    disposableObj.Dispose();

                Thread.Sleep(100);
            }
        }

        public void Clear()
        {
            inspectBufferList.ForEach(buffer => buffer.Clear());

            foreach (ScanBuffer scanBuffer in scanBufferArray)
                scanBuffer.Clear();
        }

        public ScanBuffer GetScanBuffer(int flowPosition)
        {
            return scanBufferArray[flowPosition];
        }

        public AlgoImage GetSheetBuffer(int flowPosition)
        {
            return sheetBufferArray[flowPosition];
        }

        public AlgoImage GetMaskBuffer(int flowPosition)
        {
            return maskBufferArray[flowPosition];
        }

        public AlgoImage[] GetInspectBuffer(int count, string tag)
        {
            InspectBuffer[] inspectBuffer = null;

            while (inspectBuffer == null)
            {
                lock (this.inspectBufferQ)
                {
                    if (this.inspectBufferQ.Count >= count)
                    {
                        inspectBuffer = new InspectBuffer[count];
                        for (int i = 0; i < count; i++)
                        {
                            this.inspectBufferQ.TryDequeue(out inspectBuffer[i]);
                            //Debug.WriteLine($"BufferManager::GetInspectBuffer - {this.bufferList.IndexOf(inspectBuffer[i])}, {tag}");
                            Debug.Assert(string.IsNullOrEmpty(inspectBuffer[i].Tag));
                            inspectBuffer[i].Tag = tag;
                        }
                    }
                }

                if (inspectBuffer == null)
                    Thread.Sleep(1000);
            }
            Array.ForEach(inspectBuffer, f => f.AlgoImage.Clear());
            return inspectBuffer.Select(f => f.AlgoImage).ToArray();
        }

        public void PutInspectBuffer(AlgoImage algoImage)
        {
            InspectBuffer inspectBuffer = inspectBufferList.Find(buffer => buffer.AlgoImage == algoImage);
            if (inspectBuffer != null)
            {
                //Debug.WriteLine($"BufferManager::PutInspectBuffer - {this.bufferList.IndexOf(inspectBuffer)}, {inspectBuffer.Tag}");
                Debug.Assert(!this.inspectBufferQ.Contains(inspectBuffer));
                inspectBuffer.Tag = null;
                this.inspectBufferQ.Enqueue(inspectBuffer);
            }
        }
    }
}
