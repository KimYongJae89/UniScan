using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public enum ScanDirection
    {
        Forward, Backward
    }

    public class ScanOperator : GrabOperator
    {
        Task scanTask = null;

        ScanDirection scanDirection;
        int flowPosition;
        float fovWidthUm;
        float moveLengthUm;

        List<Task> saveTaskList = new List<Task>();

        public ScanOperatorSettings Settings { get => this.settings; }
        ScanOperatorSettings settings;

        public ScanOperator() : base()
        {
            settings = new ScanOperatorSettings();

            InfoBox.Instance.UpdateRegion(settings);
        }

        public override void Release()
        {
            base.Release();

            this.axisHandler.StopMove();
            this.axisHandler.StartMultipleMove(new AxisPosition(new float[] { 0, 0}), settings.Velocity);
            this.axisHandler.WaitMoveDone();

            //this.saveTaskList.ForEach(f => f.Wait());
        }

        public int GetTotalSteps()
        {
            float resolution = DeveloperSettings.Instance.Resolution;
            float fovWidthUm = SystemManager.Instance().DeviceBox.ImageDeviceHandler[0].ImageSize.Width * resolution;
            float overlap = settings.OverlapUm;
            int moveLengthUm = (int)Math.Floor(fovWidthUm - overlap);
            if (moveLengthUm <= 0)
                return -1;

            int totalProgressSteps = (int)Math.Ceiling((settings.DstX - settings.SrcX) / moveLengthUm);
            return totalProgressSteps;
        }

        public override bool Initialize(ResultKey resultKey, int totalProgressSteps, CancellationTokenSource cancellationTokenSource)
        {
            BufferManager.Instance().Clear();
            
            this.flowPosition = 0;
            this.scanDirection = ScanDirection.Forward;

            float resolution = DeveloperSettings.Instance.Resolution;

            this.fovWidthUm = SystemManager.Instance().DeviceBox.ImageDeviceHandler[0].ImageSize.Width * resolution;
            float overlap = settings.OverlapUm;
            this.moveLengthUm = (int)Math.Floor(this.fovWidthUm - overlap);

            this.saveTaskList.Clear();

            return base.Initialize(resultKey, totalProgressSteps, cancellationTokenSource);
        }
        
        protected override bool StartGrab()
        {
            //DebugWriteLine("ScanOperator::PrepereGrab");
            WaitMoveDone();

            if (cancellationTokenSource.IsCancellationRequested)
                return false;

            scanTask = Task.Factory.StartNew(() =>
            {
                //DebugWriteLine("ScanOperator::PrepereGrabTask Start");
                LightValue lightValue = new LightValue(2);

                float targetX = settings.Dst.X - (moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float targetY = 0;
                float sourceY = 0;
                bool increase = true;
                switch (scanDirection)
                {
                    case ScanDirection.Forward:
                        sourceY = settings.Src.Y;
                        increase = true;
                        lightValue.Value[0] = 0;
                        lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;
                        targetY = settings.Dst.Y + DeveloperSettings.Instance.MoveOffset;
                        break;
                    case ScanDirection.Backward:
                        sourceY = settings.Dst.Y + DeveloperSettings.Instance.MoveOffset;
                        increase = false;
                        lightValue.Value[0] = SystemManager.Instance().CurrentModel.LightValueTop;
                        lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;
                        targetY = settings.Src.Y;
                        break;
                }

                if (lightValue.Value[1] <= 0)
                    lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;

                axisHandler.StartCmp("Y", (int)sourceY, (int)Math.Round(DeveloperSettings.Instance.Resolution), increase);
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
                imageDeviceHandler.SetStepLight(flowPosition, (int)scanDirection);
                imageDeviceHandler.GrabMulti();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, true);

                axisHandler.StartMultipleMove(new AxisPosition(new float[] { targetX, targetY }), settings.Velocity);
                WaitMoveDone();

                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, false);

                // 가상모드일때는 그랩 타이머가 1바퀴 돌때까지 기다려야 함??
                if (SystemManager.Instance().DeviceBox.ImageDeviceHandler.IsVirtual)
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.WaitGrabbed();

                imageDeviceHandler.Stop();
                //SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                axisHandler.EndCmp("Y");

                //DebugWriteLine("ScanOperator::PrepereGrabTask End");
            }, cancellationTokenSource.Token);

            return true;
        }

        protected override bool PrepareGrab()
        {
            //DebugWriteLine("ScanOperator::PostGrab");
            WaitMoveDone();

            scanTask?.Wait();

            if (scanDirection == ScanDirection.Forward)
            {
                float targetX = settings.Dst.X - (moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float targetY = settings.Src.Y;
                axisHandler.StartMultipleMove(new AxisPosition(new float[] { targetX, targetY }), settings.Velocity);
            }
            
            return !cancellationTokenSource.IsCancellationRequested;
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            DebugWriteLine("ScanOperator::ImageGrabbed");
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            if (imageDevice.IsVirtual)
                imageDevice.Stop();

            ImageD grabbedImage = imageDevice.GetGrabbedImage(ptr);
            
            ScanBuffer scanBuffer = BufferManager.Instance().GetScanBuffer(flowPosition);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);
            AlgoImage sourceImage;
            using (AlgoImage sourceImageFull = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabbedImage, ImageType.Grey))
            {
                //sourceImageFull.Save(@"C:\temp\sourceImageFull.bmp");
                sourceImage = sourceImageFull.Clip(new Rectangle(Point.Empty, imageDevice.ImageSize));
                sourceImageFull.Clear();
                //sourceImage.Save(@"C:\temp\sourceImage.bmp");
            }

            scanBuffer.AddImage(sourceImage, scanDirection, imageDevice.IsVirtual);
            sourceImage.Dispose();

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            if (scanBuffer.IsFull == true)
            {
                scanTask?.Wait();

                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

                scanDirection = ScanDirection.Forward;


                float res = DeveloperSettings.Instance.Resolution;
                float realPosX = settings.Dst.X - (this.moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float realPosY = settings.Src.Y;
                float dispPosX = DispResizeRatio * (InfoBox.Instance.DispScanRegion.X + (this.moveLengthUm * flowPosition) / res);
                float dispPosY = DispResizeRatio * (InfoBox.Instance.DispScanRegion.Y);

                int w = scanBuffer.TopLightBuffer.FullImage.Width;
                int h = (int)((settings.Height) / res);
                int top = scanBuffer.TopLightBuffer.FullImage.Height - h;
                Rectangle clipRect = new Rectangle(0, top, w, h);
                scanBuffer.SetClipRect(clipRect);

                if (this.resultKey.Production == null && false)
                {
                    this.saveTaskList.Add(Task.Run(() =>
                    {
                        string toplightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 1);
                        using (AlgoImage topClone = scanBuffer.TopLightBuffer.Image.Clone())
                            topClone?.Save(toplightImagePath);
                    }));

                    this.saveTaskList.Add(Task.Run(() =>
                    {
                        string backlightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 0);
                        using (AlgoImage backClone = scanBuffer.BackLightBuffer.Image.Clone())
                            backClone?.Save(backlightImagePath);
                    }));
                }
                else
                {
                    //string toplightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 1);
                    //string backlightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 0);
                    //topLightImage.Save(toplightImagePath);
                    //backLightImage.Save(backlightImagePath);
                }

                Size bitmapResize = DrawingHelper.Mul(clipRect.Size, DispResizeRatio);
                BitmapSource backBitmapSource = null;
                //using (AlgoImage subAlgoImage = scanBuffer.BackLightBuffer.Image.GetSubImage(subRect))
                {
                    using (AlgoImage backLightImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, bitmapResize))
                    {
                        imageProcessing.Resize(scanBuffer.BackLightBuffer.Image, backLightImage);
                        backBitmapSource = backLightImage.ToBitmapSource();
                    }
                }

                BitmapSource topBitmapSource = null;
                //using (AlgoImage subAlgoImage = scanBuffer.TopLightBuffer.Image.GetSubImage(subRect))
                {
                    using (AlgoImage topLightImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, bitmapResize))
                    {
                        imageProcessing.Resize(scanBuffer.TopLightBuffer.Image, topLightImage);
                        topBitmapSource = topLightImage.ToBitmapSource();
                    }
                }

                //int pixelPosX = (int)((settings.FovUm - settings.OverlapUm) * flowPosition / res);
                int pixelPosX = (int)(this.moveLengthUm * flowPosition / res);
                AxisPosition[] limitPos = axisHandler.GetLimitPos();

                ScanOperatorResult scanOperatorResult = new ScanOperatorResult(resultKey,
                    backBitmapSource, topBitmapSource,
                    flowPosition, scanBuffer.BackLightBuffer.Image, scanBuffer.TopLightBuffer.Image,
                    new AxisPosition(realPosX, realPosY),
                    new AxisPosition(dispPosX, dispPosY),
                    pixelPosX);

                SystemManager.Instance().OperatorProcessed(scanOperatorResult);
                if (cancellationTokenSource.IsCancellationRequested)
                    return;

                flowPosition++;
                this.CurProgressSteps++;

                float nextFovStartX = settings.Dst.X - (moveLengthUm * flowPosition);

                if (flowPosition == DeveloperSettings.Instance.ScanNum || nextFovStartX < settings.Src.X)
                {
                    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                    SystemManager.Instance().OperatorCompleted(new ScanOperatorResult(resultKey, null));
                    return;
                }

                if (PrepareGrab() == false)
                    return;

                if (StartGrab() == false)
                    return;
            }
            else if (scanBuffer.BackLightBuffer.IsFull == true && scanBuffer.TopLightBuffer.IsEmpty == true)
            {
                scanTask?.Wait();
                scanDirection = ScanDirection.Backward;

                if (PrepareGrab() == false)
                    return;

                if (StartGrab() == false)
                    return;
            }
        }

        public bool IsSaving()
        {
            return this.saveTaskList.Exists(f => !f.IsCompleted);
        }
    }

    public class ScanOperatorResult : OperatorResult
    {
        int flowPosition;
        AlgoImage backLightImage;
        AlgoImage topLightImage;

        AxisPosition axisPosition;
        AxisPosition canvasAxisPosition;
        int pixelPositionX;

        BitmapSource backLightBitmap;
        BitmapSource topLightBitmap;

        public int FlowPosition { get => flowPosition; }
        public AxisPosition AxisPosition { get => axisPosition; }
        public AlgoImage BackLightImage { get => backLightImage; }
        public AlgoImage TopLightImage { get => topLightImage; }
        public BitmapSource TopLightBitmap { get => topLightBitmap; }
        public BitmapSource BackLightBitmap { get => backLightBitmap; }
        public AxisPosition CanvasAxisPosition { get => canvasAxisPosition; }
        public int PixelPositionX => this.pixelPositionX;

        public ScanOperatorResult(ResultKey resultKey, 
            BitmapSource backLightBitmap, BitmapSource topLightBitmap, int flowPosition, AlgoImage backLightImage, AlgoImage topLightImage, AxisPosition axisPosition, AxisPosition canvasAxisPosition, int pixelPositionX)
            : base(ResultType.Scan, resultKey, DateTime.Now)
        {
            this.backLightBitmap = backLightBitmap;
            this.topLightBitmap = topLightBitmap;
            this.flowPosition = flowPosition;
            this.backLightImage = backLightImage;
            this.topLightImage = topLightImage;
            this.axisPosition = axisPosition;
            this.canvasAxisPosition = canvasAxisPosition;
            this.pixelPositionX = pixelPositionX;
        }

        public ScanOperatorResult(ResultKey resultKey, Exception exception)
            : base(ResultType.Scan, resultKey, DateTime.Now, exception)
        {

        }

        protected override string GetLogMessage()
        {
            return string.Format("FlowPosition,{0}", this.flowPosition);
        }
    }

    public class ScanOperatorSettings : GrabOperatorSettings
    {

        PointF src;
        PointF dst;
        
        float overlapUm;


        public int ScanCount
        {
            get
            {
                float resolution = DeveloperSettings.Instance.Resolution;
                float fovWidthUm = SystemManager.Instance().DeviceBox.ImageDeviceHandler[0].ImageSize.Width * resolution;
                float overlap = this.overlapUm;
                int moveLengthUm = (int)Math.Floor(fovWidthUm - overlap);
                if (moveLengthUm <= 0)
                    return -1;

                int totalProgressSteps = (int)Math.Ceiling((dst.X - src.X) / moveLengthUm);
                return Math.Min(totalProgressSteps, DeveloperSettings.Instance.ScanNum);
                return totalProgressSteps;
            }
        }

        public PointF Src { get => src; }
        public PointF Dst { get => dst; }
        public float OverlapUm
        {
            get => overlapUm;
            set
            {
                overlapUm = value;
                OnPropertyChanged("Width");
            }
        }

        public float SrcX
        {
            get => src.X;
            set
            {
                src.X = value;
                OnPropertyChanged("Width");
                OnPropertyChanged("ScanCount");                
            }
        }
        public float SrcY
        {
            get => src.Y;
            set
            {
                src.Y = value;
                OnPropertyChanged("Height");
            }
        }
        public float DstX
        {
            get => dst.X;
            set
            {
                dst.X = value;
                OnPropertyChanged("Width");
                OnPropertyChanged("ScanCount");
            }
        }
        public float DstY
        {
            get => dst.Y;
            set
            {
                dst.Y = value;
                OnPropertyChanged("Height");
            }
        }

        public float Width { get => (dst.X - src.X) - (overlapUm * (DeveloperSettings.Instance.ScanNum - 1)); }
        public float Height { get => dst.Y - src.Y; }

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Scan");

            this.src = PointF.Empty;
            this.dst= PointF.Empty;
            overlapUm = 0; 
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            XmlHelper.GetValue(xmlElement, "Src", ref this.src);
            XmlHelper.GetValue(xmlElement, "Dst", ref this.dst);
            overlapUm = XmlHelper.GetValue(xmlElement, "OverlapUm", this.overlapUm);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Src", this.src);
            XmlHelper.SetValue(xmlElement, "Dst", this.dst);
            XmlHelper.SetValue(xmlElement, "OverlapUm", this.overlapUm);
        }

    }
}
