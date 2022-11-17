using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Inspect;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;
using InvalidDataException = DynMvp.Base.InvalidDataException;
using UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase;
using UniScanG.Gravure.Vision.RCI.Trainer;
using UniScanG.Gravure.Vision.RCI;

namespace UniScanG.Gravure.UI.Teach
{
    public class ModellerPageExtenderG : UniScanG.UI.Teach.ModellerPageExtender
    {
        public Calibration Calibration => SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

        public new InspectionResult InspectionResult
        {
            get { return (InspectionResult)base.InspectionResult; }
            set { base.InspectionResult = value; }
        }

        private UniScanG.Data.Model.Model Model => SystemManager.Instance().CurrentModel;

        private SheetFinderBaseParam FiducialFinderParam
        {
            get { return AlgorithmSetting.Instance().SheetFinderBaseParam as SheetFinderBaseParam; }
        }

        private TrainerParam TrainerParam
        {
            get { return AlgorithmSetting.Instance().TrainerParam as TrainerParam; }
        }

        private CalculatorParam CalculatorParam
        {
            get { return AlgorithmSetting.Instance().CalculatorParam as CalculatorParam; }
        }

        private DetectorParam DetectorParam
        {
            get { return AlgorithmSetting.Instance().DetectorParam as DetectorParam; }
        }

        private WatcherParam WatcherParam
        {
            get { return AlgorithmSetting.Instance().WatcherParam as WatcherParam; }
        }



        GrabProcesserG grabProcesser = null;
        FigureType drawFigureType;
        int[] drawSelectedIndexes;

        public SizeF CurrentImageSizeUm { get => this.Calibration.PixelToWorld(this.CurrentImage.Size); }

        public ImageD DiffImageD => diffImageD;
        ImageD diffImageD;

        public ImageD BinalImageD => binalImageD;
        ImageD binalImageD;

        public ModellerPageExtenderG()
        {
            ModelImageLoadDone += ModellerPageExtender_ModelImageLoadDone;
        }

        #region Figure Control
        public enum FigureType { None, Chip, Bar, RCI, Extend }

        public void UpdateOverlayFigure(FigureType figureType, int[] selectedIndexes)
        {
            this.drawFigureType = figureType;
            this.drawSelectedIndexes = selectedIndexes;
            UpdateOverlayFigure();
        }

        public void UpdateOverlayFigure(FigureType figureType)
        {
            this.drawFigureType = figureType;
            this.drawSelectedIndexes = new int[0];
            UpdateOverlayFigure();
        }

        public void UpdateOverlayFigure()
        {
            if (UpdateFigure != null)
            {
                FigureGroup bgFigureGroup = new FigureGroup();
                FigureGroup fgFigureGroup = new FigureGroup();

                AppendFigureBase(bgFigureGroup);

                switch (this.drawFigureType)
                {
                    case FigureType.Chip:
                        // 패턴 그리기
                        AppendFigurePattern(bgFigureGroup, this.drawSelectedIndexes);
                        break;
                    case FigureType.Bar:
                        // 영역 그리기
                        AppendFigureRegion(bgFigureGroup, this.drawSelectedIndexes);
                        break;

                    case FigureType.RCI:
                        AppendFigureRCI(bgFigureGroup, this.drawSelectedIndexes);
                        break;

                    case FigureType.Extend:
                        // 모니터링 영역 그리기
                        AppendFigureWatch(bgFigureGroup, this.drawSelectedIndexes);
                        break;
                }

                UpdateFigure(null, bgFigureGroup);
            }
        }

        private void AppendFigureBase(FigureGroup figureGroup)
        {
            if (this.CurrentImage == null)
                return;

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            Size imageSize = this.CurrentImage.Size;

            SheetFinderBaseParam sheetFinderBaseParam = FiducialFinderParam;
            if (sheetFinderBaseParam != null)
            {
                Size skipFigureSize = Size.Empty;
                if (calibration != null)
                {
                    int w = (int)Math.Round(calibration.WorldToPixel(sheetFinderBaseParam.SearchSkipWidthMm * 1000));
                    skipFigureSize =  new Size(w, imageSize.Height);
                }

                Point skipFigureLoc = Point.Empty;
                if (sheetFinderBaseParam.GetBaseXSearchDir() == BaseXSearchDir.Right2Left)
                    skipFigureLoc = new Point(imageSize.Width - skipFigureSize.Width, 0);

                figureGroup.AddFigure(new RectangleFigure(new Rectangle(skipFigureLoc, skipFigureSize), Pens.Red, new SolidBrush(Color.FromArgb(128, Color.Red))));

                if (sheetFinderBaseParam is SheetFinderPJParam)
                {
                    SheetFinderPJParam sheetFinderPJParam = sheetFinderBaseParam as SheetFinderPJParam;
                    int fidSearchL = (int)(imageSize.Width * sheetFinderPJParam.FidSearchLBound);
                    int fidSearchR = (int)(imageSize.Width * sheetFinderPJParam.FidSearchRBound);
                    if (fidSearchR > fidSearchL)
                        figureGroup.AddFigure(new RectangleFigure(Rectangle.FromLTRB(fidSearchL, 0, fidSearchR, imageSize.Height), new Pen(Color.Red, 5)));
                }
            }

            // TODO [승근]: 패턴 개수가 적은 모델로 바뀌었을때 Exception 발생 가능성?
            Point offset = GetPatternOffset();
            CalculatorParam calculatorParam = CalculatorParam;
            if (calculatorParam != null)
            {
                Point pt = new Point(calculatorParam.ModelParam.BasePosition.X + offset.X, calculatorParam.ModelParam.BasePosition.Y);
                figureGroup.AddFigure(new LineFigure(new PointF(0, pt.Y), new PointF(imageSize.Width, pt.Y), new Pen(Color.Cyan)));
                figureGroup.AddFigure(new LineFigure(new PointF(pt.X, 0), new PointF(pt.X, imageSize.Height), new Pen(Color.Cyan)));
            }

            Vision.LengthVariation.LengthVariationParam lengthVariationParam = AlgorithmSetting.Instance().LengthVariationParam;
            if (lengthVariationParam != null && lengthVariationParam.Use)
            {
                int locX1 = (int)Math.Round(imageSize.Width * lengthVariationParam.Position) + offset.X;
                int locX2 = locX1 + (int)Math.Round(calibration.WorldToPixel(lengthVariationParam.WidthUm));

                figureGroup.AddFigure(new LineFigure(new PointF(locX1, 0), new PointF(locX1, imageSize.Height), new Pen(Color.Red)));
                figureGroup.AddFigure(new LineFigure(new PointF(locX2, 0), new PointF(locX2, imageSize.Height), new Pen(Color.Red)));
            }
        }

        private void AppendFigurePattern(FigureGroup figureGroup, int[] selectedIndexes)
        {
            CalculatorParam calculatorParam = CalculatorParam;

            try
            {
                // TODO [승근]: 패턴 개수가 적은 모델로 바뀌었을때 Exception 발생 가능성?
                SheetPatternGroup[] patternGroups = selectedIndexes.Select(f => calculatorParam.ModelParam.PatternGroupCollection[f]).ToArray();
                Point offset = GetPatternOffset();

                Array.ForEach(patternGroups, f =>
                {
                    Figure figure = f.CreateFigureGroup();
                    figure.Offset(offset);
                    figureGroup.AddFigure(figure);
                    int edgeWidth = calculatorParam.ModelParam.EdgeParam.Width;
                    if (calculatorParam.ModelParam.EdgeParam.EdgeFindMethod == EdgeParam.EEdgeFindMethod.Projection && edgeWidth > 0)
                    {
                        Figure figure2 = (Figure)figure.Clone();
                        figure2.Inflate(-edgeWidth * 2, -edgeWidth * 2);
                        figureGroup.AddFigure(figure2);

                        Figure figure3 = (Figure)figure.Clone();
                        figure3.Inflate(edgeWidth, edgeWidth);
                        figureGroup.AddFigure(figure3);
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
            }
        }

        private void AppendFigureRegion(FigureGroup figureGroup, int[] selectedIndexes)
        {
            CalculatorParam calculatorParam = CalculatorParam;
            RegionInfoG[] regionInfos = selectedIndexes.Select(f => calculatorParam.ModelParam.RegionInfoCollection[f]).ToArray();

            for (int i = 0; i < regionInfos.Length; i++)
            {
                RegionInfoG regionInfoG = regionInfos[i];
                if (regionInfoG == null)
                    continue;

                Figure figure = regionInfoG.GetFigure();
                figure.Offset(regionInfoG.Region.Location);
                figure.Offset(GetLocalOffset(i));
                figureGroup.AddFigure(figure);
            }
        }

        private void AppendFigureRCI(FigureGroup figureGroup, int[] selectedIndexes)
        {
            figureGroup.Movable = false;
            Model model = SystemManager.Instance().CurrentModel;
            bool isRight2Left = AlgorithmSetting.Instance().RCIGlobalOptions.RightToLeft;

            if (model != null)
            {
                figureGroup.AddFigure(new RectangleFigure(model.RCIOptions.ROISeedRect, new Pen(Color.Yellow, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot }) { Selectable = false });
                if (model.RCITrainResult.IsValid)
                {
                    figureGroup.AddFigure(new RectangleFigure(model.RCITrainResult.ROI, new Pen(Color.Yellow, 2)) { Selectable = false });

                    Pen yPen = new Pen(Color.Yellow);
                    Pen gPen = new Pen(Color.LightGreen);
                    Pen rPen = new Pen(Color.FromArgb(128, 255, 0, 0));
                    Brush rBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));

                    List<WorkPoint> wpList = new List<WorkPoint>();
                    wpList.AddRange(Array.FindAll(model.RCITrainResult.WorkPoints, f => !f.Use));
                    wpList.AddRange(Array.FindAll(model.RCITrainResult.WorkPoints, f => f.Column == 0));

                    WorkPoint firstfWorkPoint = model.RCITrainResult.WorkPoints.FirstOrDefault();
                    if (firstfWorkPoint != null)
                    {
                        if (!wpList.Contains(firstfWorkPoint))
                            wpList.Add(firstfWorkPoint); // Top-Base

                        if (selectedIndexes != null)
                            wpList.AddRange(selectedIndexes.Select(f => model.RCITrainResult.WorkPoints[f]));

                        wpList.ForEach(f =>
                        {
                            Rectangle rect = RCIHelper.Anchor(model.RCITrainResult.RightToLeft, model.RCITrainResult.ROI.Size, f.GetTeachRectangle());
                            rect.Offset(model.RCITrainResult.ROI.Location);

                            Figure figure = null;
                            if (f.Use)
                            {
                                figure = new RectangleFigure(rect, gPen);
                            }
                            else
                            {
                                figure = new RectangleFigure(rect, rPen, rBrush);
                            }

                            figureGroup.AddFigure(figure);

                            if (f == firstfWorkPoint || f.Use)
                                figureGroup.AddFigure(new TextFigure($"{f.Row}", rect.Location, new Font("맑은 고딕", 24), Color.Cyan, StringAlignment.Near, StringAlignment.Near));
                        });
                    }

                    //WorkPoint[] referencePoints = Array.FindAll(model.RCITrainResult.WorkPoints, f => f.IsReference);
                    Vision.RCI.Calculator.CalculatorResultV3 calculatorResultV3 = InspectionResult?.AlgorithmResultLDic[CalculatorBase.TypeName] as Vision.RCI.Calculator.CalculatorResultV3;
                    if (calculatorResultV3 != null && UserHandler.Instance().CurrentUser.IsSuperAccount)
                    {
                        Dictionary<WorkPoint, Rectangle> ptmResult = calculatorResultV3.PTMResults;
                        List<KeyValuePair<WorkPoint, Rectangle>> ptmList = ptmResult.ToList().FindAll(f => f.Key.IsReference);
                        ptmList.ForEach(f =>
                        {
                            WorkPoint key = f.Key;
                            Rectangle value = f.Value;

                            RectangleF rect1 = RCIHelper.Anchor(model.RCITrainResult.RightToLeft, model.RCITrainResult.ROI.Size, key.GetTeachRectangle());
                            rect1.Offset(model.RCITrainResult.ROI.Location);
                            //figureGroup.AddFigure(new RectangleFigure(rect1, gPen));

                            RectangleF rect2 = RCIHelper.Anchor(model.RCITrainResult.RightToLeft, calculatorResultV3.FoundRoi.Size, value);
                            rect2.Offset(calculatorResultV3.FoundRoi.Location);
                            //figureGroup.AddFigure(new RectangleFigure(rect2, yPen));

                            PointF drawOffset = DrawingHelper.Subtract(rect2.Location, rect1.Location);
                            SizeF sizeOffset = DrawingHelper.Subtract(rect2.Size, rect1.Size);

                            PointF textOffset = DrawingHelper.Subtract(key.GetTeachRectangle().Location, value.Location);
                            List<string> msgList = new List<string>();
                            if (key.IsReferenceX >= 0)
                                msgList.AddRange(new string[] { $"R{key.IsReferenceX}", $"dX{textOffset.X:+0;-0}" });
                            if (key.IsReferenceY)
                                msgList.AddRange(new string[] { $"C{key.IsReferenceY}", $"dY{textOffset.Y:+0;-0}" });

                            PointF drawLoc = DrawingHelper.Subtract(rect2.Location, drawOffset);
                            SizeF drawSize = DrawingHelper.Subtract(rect2.Size, sizeOffset);
                            RectangleF drawRect = new RectangleF(drawLoc, drawSize);
                            figureGroup.AddFigure(new RectangleFigure(drawRect, yPen));
                            figureGroup.AddFigure(new TextFigure(string.Join(Environment.NewLine, msgList), drawRect.Location, new Font("맑은 고딕", 12), Color.Cyan, StringAlignment.Near, StringAlignment.Near));
                        });
                    }
                }
            }
        }

        private void AppendFigureWatch(FigureGroup figureGroup, int[] selectedIndexes)
        {
            WatcherParam watcherParam = this.WatcherParam;
            if (watcherParam == null)
                return;

            OffsetStructSet offsetStructSet = this.InspectionResult?.OffsetSet;
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

            figureGroup.AddFigure(watcherParam.ModelParam.GetFigure(offsetStructSet, calibration));
        }

        public void ModellerPageExtender_ModelImageLoadDone()
        {
            UpdateOverlayFigure(FigureType.RCI);
        }
        #endregion

        private Point GetPatternOffset()
        {
            return GetLocalOffset(-1);
        }

        private Point GetLocalOffset(int i)
        {
            if (this.InspectionResult == null)
                return Point.Empty;

            return this.InspectionResult.GetOffset(i);
        }


        public override void Clear()
        {
            base.Clear();
            diffImageD?.Dispose();
            diffImageD = null;
            binalImageD?.Dispose();
            binalImageD = null;

            this.InspectionResult = null;
            UpdateOverlayFigure(FigureType.None);
        }

        public override void AddDontcareRegion(Rectangle rectangle)
        {
            RegionInfoG regionInfoG = CalculatorParam.ModelParam.RegionInfoCollection.Find(f => f.Region.IntersectsWith(rectangle));
            if (regionInfoG != null)
            {
                Point location = regionInfoG.Region.Location;
                rectangle.Offset(-location.X, -location.Y);
                rectangle.Inflate(rectangle.Width / 2, rectangle.Height / 2);
                regionInfoG.AddDontcareRect(rectangle);
                UpdateOverlayFigure();
                SystemManager.Instance().CurrentModel.Modified = true;
            }
        }

        public override void AddCriticalPoint(Rectangle rectangle)
        {
            RegionInfoG regionInfoG = CalculatorParam.ModelParam.RegionInfoCollection.Find(f => f.Region.IntersectsWith(rectangle));
            if (regionInfoG != null)
            {
                Point location = regionInfoG.Region.Location;
                rectangle.Offset(-location.X, -location.Y);
                rectangle.Inflate(10, 10);
                rectangle.Width = rectangle.Height = Math.Max(rectangle.Width, rectangle.Height);

                regionInfoG.AddCreticalPoint(rectangle);

                UpdateOverlayFigure();
                SystemManager.Instance().CurrentModel.Modified = true;
            }
        }
        public override void RemoveCriticalPoint(Rectangle rectangle)
        {
            RegionInfoG regionInfoG = CalculatorParam.ModelParam.RegionInfoCollection.Find(f => f.Region.IntersectsWith(rectangle));
            if (regionInfoG != null)
            {
                Point location = regionInfoG.Region.Location;
                rectangle.Offset(-location.X, -location.Y);
                rectangle.Inflate(rectangle.Width / 2, rectangle.Height / 2);
                rectangle.Width = rectangle.Height = Math.Max(rectangle.Width, rectangle.Height);

                regionInfoG.CreticalPointList.RemoveAll(f => f.IntersectsWith(rectangle));
                UpdateOverlayFigure();
                SystemManager.Instance().CurrentModel.Modified = true;
            }
        }

        public override string GetModelImageName()
        {
            int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            return SystemManager.Instance().CurrentModel.GetImageName(camIdx, 0, 0, "png");
        }

        public override void GrabSheet(int grabCount, CancellationTokenSource cts)
        {
            bool isTestMode = grabCount == -1;

            GrabProcesserG grabProcesserG = ((InspectRunner)SystemManager.Instance().InspectRunnerG).BuildSheetGrabProcesser(false);
            grabProcesserG.StartInspectionDelegate += ImageGrabbed;
            grabProcesserG.SetDebugMode(((UniScanG.Gravure.Settings.AdditionalSettings)AdditionalSettings.Instance()).DebugSheetGrabProcesser);
            grabProcesserG.Start();
            this.grabProcesser = grabProcesserG;

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(grabProcesserG.ImageGrabbed);
            SystemManager.Instance().DeviceControllerG.SetAsyncMode(this.lineSpeedMpm);

            //SimpleProgressForm simpleProgressForm =();
            Form mainForm = ConfigHelper.Instance().MainForm;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            //int grabTimeoutMs = Math.Max(30000, additionalSettings.AutoTeachTimeout * 3 / 4);
            int timeoutMs = additionalSettings.AutoTeachTimeout;
            bool isGrabbed = false;
            TimeOutTimer timeOutTimer = new TimeOutTimer();
            if (isTestMode == false)
            // 한장그랩
            {
                imageDeviceHandler.SetStepLight(0, 0); // 가상카메라 이미지인덱스 초기화
                imageDeviceHandler.GrabMulti();
                new SimpleProgressForm().Show(mainForm, () =>
                 {
                     int grabbedCnt = 0;
                     while (grabbedCnt < grabCount && !cts.IsCancellationRequested && !timeOutTimer.TimeOut)
                     {
                         timeOutTimer.Restart(timeoutMs);
                         if (!WaitGrabDone(grabProcesser, timeOutTimer, cts))
                             break;

                         isGrabbed = true;
                         grabbedCnt++;
                     }
                 }, cts);

                imageDeviceHandler.Stop();
            }
            else
            {
                // 무한그랩(테스트)
                //grabProcesserG.SetDebugMode(@"D:\GrabTest\");

                imageDeviceHandler.SetStepLight(0, 0); // 가상카메라 이미지인덱스 초기화
                imageDeviceHandler.GrabMulti();
                new SimpleProgressForm().Show(mainForm, () =>
                 {
                     while (cts.IsCancellationRequested == false)
                     {
                         System.Threading.Thread.Sleep(100);
                         if (grabProcesser.GrabbedSignal.WaitOne(0))
                             isGrabbed = true;
                     }
                 }, cts);
                imageDeviceHandler.Stop();
            }
            timeOutTimer.Stop();

            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            imageDeviceHandler.RemoveImageGrabbed(grabProcesserG.ImageGrabbed);

            grabProcesserG.StartInspectionDelegate -= ImageGrabbed;
            grabProcesserG.Stop();

            new SimpleProgressForm().Show(mainForm, () =>
            {
                grabProcesserG.Clear();
                while (grabProcesserG.IsDisposable() == false)
                    Thread.Sleep(100);
                grabProcesserG.Dispose();
                grabProcesserG = null;
            });
            this.grabProcesser = null;

            Model.ImageModified |= isGrabbed;

            if (timeOutTimer.TimeOut)
                throw new TimeoutException();
        }

        private bool WaitGrabDone(GrabProcesserG grabProcesser, TimeOutTimer timeOutTimer, CancellationTokenSource cts)
        {
            bool cancel, timeout, done;
            bool exit;
            do
            {
                cancel = cts.IsCancellationRequested;
                timeout = timeOutTimer.TimeOut;
                done = grabProcesser.GrabbedSignal.WaitOne(0);
                exit = cancel || timeout || done;
                if (!exit)
                    Thread.Sleep(20);
            } while (!exit);

            return done;
        }

        public override void GrabFrame(CancellationTokenSource cts)
        {
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.First();
            if (calibration == null)
                return;

            float lengthMm = (int)(calibration.ImageSize.Height * calibration.PelSize.Height / 1E3);
            InputForm inputForm = new InputForm(StringManager.GetString("Frame Length? [mm]"), lengthMm.ToString("F1"));
            if (inputForm.ShowDialog() == DialogResult.Cancel)
                return;

            int lengthPx = -1;
            bool ok = float.TryParse(inputForm.InputText, out lengthMm);
            if (ok)
                lengthPx = (int)Math.Round(lengthMm / calibration.PelSize.Height * 1E3);

            if (lengthPx < 0)
            {
                MessageForm.Show(null, StringManager.GetString("Wrong Input"));
                return;
            }

            Form mainForm = ConfigHelper.Instance().MainForm;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            int timeoutMs = additionalSettings.AutoTeachTimeout;

            FrameGrabProcesserG frameGrabProcesser = new FrameGrabProcesserG(lengthPx);
            frameGrabProcesser.SetDebugMode(additionalSettings.DebugSheetGrabProcesser);
            frameGrabProcesser.Start();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(frameGrabProcesser.ImageGrabbed);
            ((Device.DeviceControllerG)SystemManager.Instance().DeviceController).SetAsyncMode(this.lineSpeedMpm);

            imageDeviceHandler.SetStepLight(0, 0); // 가상카메라 이미지인덱스 초기화
            imageDeviceHandler.GrabMulti();

            TimeOutTimer timeOutTimer = new TimeOutTimer();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                timeOutTimer.Start(timeoutMs);
                WaitGrabDone(frameGrabProcesser, timeOutTimer, cts);
                timeOutTimer.Stop();
            }, cts);

            imageDeviceHandler.RemoveImageGrabbed(frameGrabProcesser.ImageGrabbed);
            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);

            frameGrabProcesser.Stop();

            using (ImageD grabbedImage = frameGrabProcesser.GetSheetImageSet()?.ToImageD())
                ShowImage(grabbedImage);

            new SimpleProgressForm().Show(mainForm, () =>
            {
                frameGrabProcesser.Clear();
                while (!frameGrabProcesser.IsDisposable())
                    Thread.Sleep(100);
                frameGrabProcesser.Dispose();
            });

            if (timeOutTimer.TimeOut)
                throw new TimeoutException();
        }

        AlgoImage tempAlgoImage = null;

        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Grab, "ModellerPageExtenderG::ImageGrabbed", true);

            //Task.Run(() =>
            //{
            //LogHelper.Debug(LoggerType.Grab, string.Format("ModellerPageExtenderG::ImageGrabbed - Task Start: ptr {0}", ptr.ToString()));
            try
            {
                int pattenrNo;
                ImageD grabbedImage = null;

                if (imageDevice != null)
                {
                    pattenrNo = 0;
                    LogHelper.Debug(LoggerType.Grab, string.Format("imageDevice: {0}, ptr: {1}", imageDevice.ToString(), ptr.ToString()));
                    grabbedImage = imageDevice.GetGrabbedImage(ptr).Clone();
                }
                else
                {
                    pattenrNo = (int)ptr;
                    using (SheetImageSet sheetImageSet = this.grabProcesser.GetSheetImageSet(pattenrNo))
                    {
                        this.grabProcesser.RemoveSheetImageSet(pattenrNo);

                        if (false)
                        {
                            LogHelper.Debug(LoggerType.Grab, string.Format("ModellerPageExtenderG.ImageGrabbed(...)---why null? fuck"), true);
                            return;
                        }
                        if (true)
                        {
                            LogHelper.Debug(LoggerType.Grab, string.Format("No: {0}, Start ToImage, Width: {1}, Height:{2}", pattenrNo, sheetImageSet.Width, sheetImageSet.Height), true);
                            grabbedImage = (Image2D)sheetImageSet.ToImageD();
                            LogHelper.Debug(LoggerType.Grab, string.Format("No: {0}, ToImage Done", pattenrNo, sheetImageSet.Width, sheetImageSet.Height), true);
                        }
                        else
                        {
                            // TODO: Test with M.S.Cho in China
                            if (tempAlgoImage == null)
                                tempAlgoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, 1782, 3500);

                            Size patternSize = sheetImageSet.GetFullImage(tempAlgoImage, 0.1f);
                            var bitmap = tempAlgoImage.ToBitmap();
                            var fileName = Path.Combine($"D:\\test\\WholePattern[{pattenrNo}].bmp");
                            ImageHelper.SaveImage(bitmap, fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            if (patternSize.IsEmpty)
                                return;

                            using (AlgoImage tempAlgoImage2 = tempAlgoImage.GetSubImage(new Rectangle(Point.Empty, patternSize)))
                            {
                                ImageD imageD = tempAlgoImage2.ToImageD();
                                LogHelper.Debug(LoggerType.Grab, $"ModellerPageExtenderG::ImageGrabbed - Pattern No: {pattenrNo}, Height: {patternSize.Height}", true);

                                if (pattenrNo % 5 == 0)
                                    ShowImage(imageD);

                                imageD.Dispose();
                            }
                            return;
                        }
                    }
                }

                // SaveImage
                //if (pattenrNo == 0)
                {
                    string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
                    int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                    string imageName = SystemManager.Instance().CurrentModel.GetImageName(camIdx, pattenrNo, 0, "png");

                    if (false)
                    {
                        ImageD saveImage = (ImageD)grabbedImage.Clone();
                        Task.Run(() =>
                        {
                            try
                            {
                                string imagePathC = @"C:\temp";
                                string pngFile = Path.Combine(imagePathC, imageName);

                                string tmpFile = Path.ChangeExtension(pngFile, "tmp");
                                saveImage.SaveImage(tmpFile, ImageFormat.Png);

                                FileHelper.Move(tmpFile, pngFile);

                                string bmpFile = Path.ChangeExtension(pngFile, "bmp");
                                if (File.Exists(bmpFile))
                                    File.Delete(bmpFile);

                                saveImage.Dispose();
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(LoggerType.Error, $"ModellerPageExtenderG::ImageGrabbed SaveTask - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                            }
                        });
                    }

                    //if (pattenrNo == 0)
                    ShowImage(grabbedImage);
                }
                grabbedImage.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"ModellerPageExtenderG::ImageGrabbed - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            //});
        }

        private void ShowImage(ImageD imageD)
        {
            //lock (lockObject)
            {
                LogHelper.Debug(LoggerType.Grab, $"ModellerPageExtenderG::ShowImage");
                this.IsModelImageLoading = false;
                this.CurrentImage = (Image2D)imageD.Clone();
                UpdateImage?.BeginInvoke(this.CurrentImage, false, null, null);
                LogHelper.Debug(LoggerType.Grab, $"ModellerPageExtenderG::ShowImage - Width: {imageD.Width}, Height: {imageD.Height}");
            }
        }

        protected override void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TrainerBase trainer = (TrainerBase)AlgorithmPool.Instance().GetAlgorithm(TrainerBase.TypeName);
            TrainerArgument trainerArgument = (TrainerArgument)e.Argument;

            if (trainer == null)
            {
                trainerArgument.Exception = new Exception("There is no Algorithm");
                e.Result = trainerArgument;
                return;
            }

            if (this.CurrentImage == null)
            {
                trainerArgument.Exception = new Exception("There is no Image");
                e.Result = trainerArgument;
                return;
            }
            //WatcherParam watcherParam = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName).Param as WatcherParam;
            //watcherParam.Clear();

            trainer.Teach((BackgroundWorker)sender, this.CurrentImage, e);
        }

        protected override void TeachRunWorkerCompleted(object result)
        {
            Form mainForm = ConfigHelper.Instance().MainForm;
            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new RunWorkerCompletedDelegate(TeachRunWorkerCompleted), result);
                return;
            }

            TrainerArgument trainerArgument = (TrainerArgument)result;

            if (trainerArgument.Exception != null)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ModellerPageExtenderG::TeachRunWorkerCompleted - {0}{1}{2}",
                    trainerArgument.Exception.Message, Environment.NewLine, trainerArgument.Exception.StackTrace));

                if (trainerArgument.IsAutoTeach)
                    SystemState.Instance().SetAlarm(trainerArgument.Exception.Message);
                else
                    MessageForm.Show(mainForm, trainerArgument.Exception.Message);

                return;
            }

            SystemManager.Instance().CurrentModel.IsTrained = trainerArgument.IsTeachDone;
            SystemManager.Instance().CurrentModel.Modified = true;

            int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            int clientIdx = SystemManager.Instance().ExchangeOperator.GetClientIndex();

            // 정상종료면 저장함.
            bool saveRequired = trainerArgument.IsTeachDone && !trainerArgument.DoNotSave;
#if DEBUG
            // 디버그에서는 정상종료+수동티칭시 물어봄
            if (saveRequired && !trainerArgument.IsAutoTeach)
                saveRequired = (MessageForm.Show(null, "Save?", MessageFormType.YesNo) == DialogResult.Yes);
#endif
            if (saveRequired)
            {
                SaveModel();
                SystemManager.Instance().ExchangeOperator.ModelTeachDone(camIdx);
            }
            else
            {
                SystemManager.Instance().ExchangeOperator.ModelTeachDone(camIdx);
            }

            //SystemManager.Instance().ExchangeOperator.ModelTeachDone(camIdx);
        }

        //        public override void Inspect(RegionInfo regionInfo)
        //        {
        //            if (currentImage == null)
        //                return;

        //            CalculatorBase calculator = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName) as CalculatorBase;
        //            Detector detector = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName) as Detector;
        //            Watcher watcher = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName) as Watcher;
        //            RegionInfoG regionInfoG = regionInfo as RegionInfoG;
        //            if (calculator == null || detector == null || regionInfoG == null)
        //                return;

        //            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp);

        //            AlgoImage algoImage = null;
        //            ProcessBufferSetG bufferSet = null;
        //            bool isMultiLayerBuffer = Settings.AdditionalSettings.Instance().MultiLayerBuffer;
        //            float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;

        //            try
        //            {
        //                SimpleProgressForm readyForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready"));
        //                readyForm.Show(() =>
        //                {
        //                    algoImage = ImageBuilder.Build(CalculatorBase.TypeName, currentImage, ImageType.Grey);
        //                    bufferSet = calculator.CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, currentImage.Width, currentImage.Height);
        //                    bufferSet.BuildBuffers(false);

        //                    calculator.PrepareInspection();
        //                    detector.PrepareInspection();
        //                    watcher.PrepareInspection();

        //                    CalculatorParam calculatorParam = calculator.Param as CalculatorParam;
        //                    DetectorParam detectorParam = detector.Param as DetectorParam;
        //                });

        //                //dynThresholdImage.Save(@"d:\temp\tt.bmp");
        //                InspectionResult inspectionResult = SystemManager.Instance().InspectRunner.InspectRunnerExtender.BuildInspectionResult() as InspectionResult;
        //                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //                Stopwatch sw = new Stopwatch();
        //                SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));
        //                inspectorForm.Show(new Action(() =>
        //                {
        //                    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext);
        //                    inspectParam.TestInspect = true;
        //                    inspectParam.AlgoImage = algoImage;
        //                    inspectParam.ProcessBufferSet = bufferSet;
        //                    inspectParam.RegionInfo = regionInfoG;
        //                    inspectParam.CancellationToken = cancellationTokenSource.Token;

        //                    //bufferSet.DynamicThreshold.Save(@"d:\temp\tt.bmp");
        //                    sw.Start();
        //                    bufferSet.Upload(algoImage);
        //                    inspectionResult.AlgorithmResultLDic.Add(calculator.GetAlgorithmType(), calculator.Inspect(inspectParam));
        //                    bufferSet.Download();
        //                    inspectionResult.AlgorithmResultLDic.Add(detector.GetAlgorithmType(), detector.Inspect(inspectParam));
        //                    inspectionResult.AlgorithmResultLDic.Add(detector.GetAlgorithmType(), watcher.Inspect(inspectParam));
        //                    bufferSet.Clear();
        //                    sw.Stop();
        //                    inspectionResult.InspectionEndTime = DateTime.Now;
        //                    inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;
        //                }), cancellationTokenSource);

        //                if (cancellationTokenSource.IsCancellationRequested)
        //                {
        //                    SimpleProgressForm waitForm = new SimpleProgressForm("Wait");
        //                    waitForm.Show(() => inspectorForm.Task.Wait());
        //                    return;
        //                }

        //                this.InspectionResult = inspectionResult;
        //                UpdateSheetResult(inspectionResult);

        //            }
        //#if DEBUG == false
        //            catch(Exception ex)
        //            {
        //                MessageForm.Show(null, ex.Message);
        //            }
        //#endif
        //            finally
        //            {
        //                algoImage.Dispose();
        //                bufferSet.Dispose();
        //            }

        //        }

        protected override Image2D GetMarkedPrevImage()
        {
            if (this.CurrentImage == null)
                return null;

            AlgoImage colorImage;
            using (Image2D prevImage = SheetCombiner.CreatePrevImage(this.CurrentImage))
            {
                using (AlgoImage fullImage = BuildAlgoImage(prevImage))
                    colorImage = fullImage.ConvertTo(ImageType.Color);
            }

            float resizeRatio = Common.Settings.SystemTypeSettings.Instance().ResizeRatio;
            Figure[] figures = CalculatorParam.ModelParam.RegionInfoCollection.Select(f =>
            {
                Figure figure = f.GetFigure(false);
                figure.Offset(f.Region.Location);
                figure.Scale(resizeRatio);
                return figure;
            }).ToArray();

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(colorImage);
            Array.ForEach(figures, f => DrawFigure(colorImage, ip, f));

            Image2D image2D = (Image2D)colorImage.ToImageD();
            //Size size = Size.Round(new SizeF(colorImage.Width * resizeRatio, colorImage.Height * resizeRatio));
            //Image2D image2D;
            //using (AlgoImage resized = ImageBuilder.Build(colorImage.LibraryType, colorImage.ImageType, size))
            //{
            //    ip.Resize(colorImage, resized);
            //    //resized.Save(@"C:\temp\resized.bmp");
            //    image2D = (Image2D)resized.ToImageD();
            //    //image2D.SaveImage(@"C:\temp\image2D.bmp");
            //}
            colorImage.Dispose();
            return image2D;
        }

        private void DrawFigure(AlgoImage fullImage, ImageProcessing ip, Figure f)
        {
            if (f is RectangleFigure)
            {
                RectangleFigure rectangleFigure = (RectangleFigure)f;
                bool fill = rectangleFigure.FigureProperty.Brush != null;
                ip.DrawRect(fullImage, Rectangle.Round(rectangleFigure.Rectangle.GetBoundRect()), rectangleFigure.FigureProperty.Pen.Color.ToArgb(), false);
            }
            else if (f is LineFigure)
            {
                LineFigure lineFigure = (LineFigure)f;
                float[] x = new float[] { lineFigure.StartPoint.X, lineFigure.EndPoint.X };
                float[] y = new float[] { lineFigure.StartPoint.Y, lineFigure.EndPoint.Y };
                ip.DrawPolygon(fullImage, x, y, lineFigure.FigureProperty.Pen.Color.ToArgb(), false);
            }
            else if (f is FigureGroup)
            {
                FigureGroup figureGroup = (FigureGroup)f;
                figureGroup.FigureList.ForEach(g => DrawFigure(fullImage, ip, g));
            }
        }

        public override void Inspect()
        {
            AlgoImage fullImage = null, scaleImage = null;
            ProcessBufferSetG bufferSet = null;
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;

            try
            {
                if (this.CurrentImage == null)
                    throw new InvalidSourceException(StringManager.GetString("There is no Image."));

                
                Algorithm[] algorithms = new Algorithm[3]
                {
                    AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName),
                    AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName),
                    AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName)
                };
                if (!Array.TrueForAll(algorithms, f => f != null))
                    throw new InvalidDataException(StringManager.GetString("Algorithm is Not Ready."));

                bool isMultiLayerBuffer = Settings.AdditionalSettings.Instance().MultiLayerBuffer;
                float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;

                DebugContextG debugContextG = new DebugContextG(new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp));
                debugContextG.LotNo = "ModellerPageTestInspect";

                SimpleProgressForm readyForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready"));
                Exception bufferInitEx = null;
                readyForm.Show(() =>
                {
                    try
                    {
                        fullImage = BuildAlgoImage(this.CurrentImage);
                        LogHelper.Debug(LoggerType.Inspection, string.Format("ModellerPageExtenderG::Inspect - fullImageSize: {0}", fullImage.Size));

                        //fullImage = ImageBuilder.Build(Calculator.TypeName, currentImage);

                        CalculatorParam calculatorParam = algorithms[0].Param as CalculatorParam;
                        DetectorParam detectorParam = algorithms[1].Param as DetectorParam;
                        WatcherParam watcherParam = algorithms[2].Param as WatcherParam;

                        UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
                        bufferSet = ((CalculatorBase)algorithms[0]).CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, fullImage.Width, fullImage.Height);
                        bufferSet.BuildBuffers(false);
                        if (!bufferSet.IsBuilded)
                            throw new Exception("Buffer Build Fail");
                        Array.ForEach(algorithms, f => f.PrepareInspection());
                    }
                    catch (Exception ex)
                    {
                        bufferInitEx = ex;
                        LogHelper.Error(LoggerType.Error, string.Format("Exception in ModellerPageExtenderG::Inspect - {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
                    }
                });

                if (bufferInitEx != null)
                {
                    bufferSet.Dispose();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(StringManager.GetString("Buffer Initialize Fail."));
                    sb.AppendLine(bufferInitEx.Message);
                    throw new InvalidDataException(sb.ToString());
                }
                //dynThresholdImage.Save(@"d:\temp\tt.bmp");

                InspectionResult inspectionResult = SystemManager.Instance().InspectRunner.InspectRunnerExtender.BuildInspectionResult("TEST") as InspectionResult;
                inspectionResult.InspectionNo = "-1";

                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));

                inspectorForm.Show(ConfigHelper.Instance().MainForm, new Action(() =>
                {
                    try
                    {
                        this.diffImageD?.Dispose();
                        this.diffImageD = null;

                        this.binalImageD?.Dispose();
                        this.binalImageD = null;

                        //Stopwatch sw = new Stopwatch();
                        //sw.Start();
                        inspectionResult.InspectionStartTime = DateTime.Now;

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Upload"));
                        //Stopwatch swUpload = Stopwatch.StartNew();
                        bufferSet.Upload(fullImage, debugContextG);
                        inspectionResult.SetOffsetStruct(bufferSet.OffsetStructSet);

                        //swUpload.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Upload: {0}[ms]", swUpload.ElapsedMilliseconds));

                        SheetInspectParam inspectParam = new SheetInspectParam(model, bufferSet, calibration, debugContextG)
                        {
                            TestInspect = true,
                            TargetRegionInfo = this.targetRegionInfo,
                            CancellationToken = cancellationTokenSource.Token,
                        };

                        string algorithmTypeCalc = algorithms[0].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeCalc));
                        //Stopwatch swCalc = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeCalc, algorithms[0].Inspect(inspectParam));
                        //swCalc.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeCalc, swCalc.ElapsedMilliseconds));

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Download"));
                        //Stopwatch swDownload = Stopwatch.StartNew();
                        bufferSet.Download();
                        //swDownload.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Download: {0}[ms]", swDownload.ElapsedMilliseconds));

                        string algorithmTypeDetect = algorithms[1].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeDetect));
                        //Stopwatch swDetect = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeDetect, algorithms[1].Inspect(inspectParam));
                        //swDetect.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeDetect, swDetect.ElapsedMilliseconds));

                        string algorithmTypeWatch = algorithms[2].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeWatch));
                        //Stopwatch swWatch = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeWatch, algorithms[2].Inspect(inspectParam));
                        //swWatch.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeWatch, swWatch.ElapsedMilliseconds));

                        inspectionResult.InspectionEndTime = DateTime.Now;
                        //inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;
                        inspectionResult.InspectionTime = new TimeSpan(0, 0, 0, 0, debugContextG.ProcessTimeLog.GetTotalTimeMs());
                        inspectionResult.UpdateJudgement();
                        //sw.Stop();
                        //DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Total: {0}[ms]", sw.ElapsedMilliseconds));

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Generate Preview"));
                        this.diffImageD = bufferSet.CalculatorResultGray.ToImageD();
                        this.binalImageD = bufferSet.CalculatorResultBinal.ToImageD();
                        if (false && this.targetRegionInfo != null)
                        {
                            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(bufferSet.AlgoImage);
                            Rectangle rect = this.targetRegionInfo.Region;

                            using (AlgoImage subAlgoImage = bufferSet.AlgoImage.Clip(rect))
                            {
                                //subAlgoImage.Save(@"C:\temp\AlgoImage.bmp");

                                if (bufferSet.EdgeMapImage != null)
                                {
                                    using (AlgoImage subImage = bufferSet.EdgeMapImage.Clip(rect))
                                    {
                                        //subImage.Save(@"C:\temp\EdgeMapImage.bmp");
                                        ip.Clipping(subImage, subImage, 0, 0, 1, 50);
                                        //subImage.Save(@"C:\temp\EdgeMapImage2.bmp");
                                        ip.Add(subAlgoImage, subImage, subAlgoImage);
                                    }
                                }

                                using (AlgoImage subImage = bufferSet.CalculatorResultGray.GetSubImage(rect))
                                {
                                    //subImage.Save(@"C:\temp\CalculatorResultGray.bmp");
                                    ip.Add(subAlgoImage, subImage, subAlgoImage);
                                }

                                using (AlgoImage subImage = bufferSet.CalculatorResultBinal.GetSubImage(rect))
                                {
                                    //subImage.Save(@"C:\temp\CalculatorResultBinal.bmp");
                                    ip.Add(subAlgoImage, subImage, subAlgoImage);
                                }

                                subAlgoImage.Save(@"C:\temp\AddImage.bmp");
                            }
                        }
                        bufferSet.Clear();

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Done"));
                        Thread.Sleep(1000);
                    }
                    catch (OperationCanceledException) { }
                }), cancellationTokenSource);

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    SimpleProgressForm waitForm = new SimpleProgressForm();
                    waitForm.Show(() => inspectorForm.Task.Wait());
                }

                if(inspectorForm.Exception != null)
                {
                    List<Exception> exceptionList = new List<Exception>();
                    exceptionList.Add(inspectorForm.Exception);
                    int aggregateExceptionCount = 0;
                    do
                    {
                        List<Exception> list = exceptionList.FindAll(f => f is AggregateException);
                        aggregateExceptionCount = list.Count;

                        list.ForEach(f =>
                        {
                            exceptionList.Remove(f);
                            exceptionList.AddRange(((AggregateException)f).InnerExceptions);
                        });
                    } while (aggregateExceptionCount > 0);

                    exceptionList.ForEach(f => LogHelper.Error(LoggerType.Inspection, f));
                    MessageForm.Show(ConfigHelper.Instance().MainForm, string.Join(Environment.NewLine, exceptionList.Select(f => $"{f.GetType().Name}: {f.Message}")));
                }

                Array.ForEach(algorithms, f => f.ClearInspection());
                LogHelper.Debug(LoggerType.Inspection, string.Format("InsepctionTime {0} {1}", debugContextG.PatternId, debugContextG.ProcessTimeLog.GetData()));

                this.InspectionResult = inspectionResult;
                UpdateSheetResult(inspectionResult, debugContextG);
                UpdateOverlayFigure();
            }
            //#if DEBUG == false
            catch (Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
            //#endif
            finally
            {
                scaleImage?.Dispose();
                fullImage?.Dispose();
                bufferSet?.Dispose();
            }

            //SizeF offset = new SizeF();
            //ProcessBufferSetS bufferSet = new ProcessBufferSetS(SheetInspector.TypeName, currentImage.Width, currentImage.Height);

            //FiducialFinderAlgorithmResult finderResult = new FiducialFinderAlgorithmResult();
            //if (AlgorithmSetting.Instance().IsFiducial == true)
            //{
            //    SimpleProgressForm fiducialForm = new SimpleProgressForm("Find Fiducial");
            //    fiducialForm.Show(new Action(() =>
            //    {
            //        FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);
            //        SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            //        inspectParam.ClipImage = currentImage;
            //        inspectParam.ProcessBufferSet = bufferSet;
            //        finderResult = (FiducialFinderAlgorithmResult)fiducialFinder.Inspect(inspectParam);
            //        offset = finderResult.OffsetFound;
            //    }));
            //}


            //SheetResult sheetResult = new SheetResult();

            //SimpleProgressForm inspectorForm = new SimpleProgressForm("Inspect");
            //inspectorForm.Show(new Action(() =>
            //{
            //    SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
            //    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            //    inspectParam.ClipImage = currentImage;
            //    inspectParam.ProcessBufferSet = bufferSet;
            //    inspectParam.FidOffset = finderResult.OffsetFound;
            //    stopwatch.Start();
            //    sheetResult = (SheetResult)sheetInspector.Inspect(inspectParam);
            //    stopwatch.Stop();
            //}));


            //bufferSet.Dispose();
        }

        private AlgoImage BuildAlgoImage(Image2D currentImage)
        {
            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(Detector.TypeName);
            AlgoImage algoImage = ImageBuilder.Build(algorithmStrategy.LibraryType, currentImage, ImageType.Grey);

            //Model model = SystemManager.Instance().CurrentModel;
            //Vision.Trainer.RCI.RCITrainResult result = model.RCITrainResult;
            //if (result != null)
            //{
            //    byte[] clibDatas = result.GetPRNUDatas(currentImage.Pitch, model.RCIOptions.UniformizeGv);
            //    byte[] datas = algoImage.GetByte();
            //    RCIHelperWithSIMD.IterateProduct(datas, clibDatas, datas, algoImage.Pitch, algoImage.Height);
            //    algoImage.SetByte(datas);
            //}
            return algoImage;
        }

        public void DataExport(BackgroundWorker worker, string path)
        {
            if (InspectionResult == null)
                return;

            //SimpleProgressForm form = new SimpleProgressForm();
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //form.Show(() =>
            //{
            //    InspectionResult.ResultPath = path;
            //    SystemManager.Instance().ExportData(InspectionResult, cancellationTokenSource);
            //    Process.Start(InspectionResult.ResultPath);
            //}, cancellationTokenSource);
            //form.Task.Wait();

            AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.CurrentImage, ImageType.Grey);
            AlgoImage diffImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.DiffImageD, ImageType.Grey);
            AlgoImage binalImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.BinalImageD, ImageType.Grey);
            if (!int.TryParse(this.InspectionResult.InspectionNo, out int tryNo))
                tryNo = -1;

            Directory.CreateDirectory(path);

            SaveTestInspection.Save(path, tryNo,
                algoImage, diffImage, binalImage,
                (UniScanG.Data.Inspect.InspectionResult)this.InspectionResult,
                worker);

            algoImage.Dispose();
            diffImage.Dispose();
            binalImage.Dispose();
        }

        public override void DataExport(string path)
        {
            DataExport(null, path);
        }

        public override float[] AutoLigthProcess(float rollDiaMm)
        {
            int lengthPx = (int)Math.Round(rollDiaMm * Math.PI);
            FrameGrabProcesserG frameGrabProcesser = new FrameGrabProcesserG(lengthPx);
            frameGrabProcesser.Start();
            return base.AutoLigthProcess(rollDiaMm);
        }

        public override void AutoTeachProcess(float lineSpeedMpm)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("ModellerPageExtenderG::AutoTeachProcess - lineSpeedMpm: {0}", lineSpeedMpm));
            if (SystemManager.Instance().ExchangeOperator.GetClientIndex() > 0)
                return;

            SystemState.Instance().SetTeach();

            try
            {
                float patternHeightMm = CalculatorBase.CalculatorParam.ModelParam.SheetSizeMm.Height;

                // 패턴 그랩
                this.lineSpeedMpm = lineSpeedMpm;
                float lowerMm = patternHeightMm * 0.9f;
                float upperMm = patternHeightMm * 1.1f;
                //int lower = (int)Math.Round(this.currentImage == null ? 0 : this.currentImage.Height * 0.9f);
                //int upper = (int)Math.Round(this.currentImage == null ? int.MaxValue : this.currentImage.Height * 1.1f);

                bool grabOk = false;
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        GrabSheet(1, new CancellationTokenSource());

                        float imageHeightMm = this.CurrentImageSizeUm.Height / 1000f;
                        bool isInRange = MathHelper.IsInRange(imageHeightMm, lowerMm, upperMm);
                        LogHelper.Debug(LoggerType.Grab, string.Format("ModellerPageExtenderG::AutoTeachProcess - isInRange: {0}, {1} < {2} < {3}", isInRange, lowerMm, imageHeightMm, upperMm));
                        if (isInRange)
                        {
                            grabOk = true;
                            break;
                        }
                    }
                    catch (TimeoutException) { grabOk = false; }
                }

                if (grabOk == false)
                    throw new Exception("Sheet Grab Fail");

                // 티칭
                //UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
                //this.Teach(additionalSettings.FastAutoTeach ? "Fast" : null);
                this.Teach(new TrainerArgument(true, true, true, true));

                LogHelper.Debug(LoggerType.Inspection, "ModellerPageExtenderG::AutoTeachProcess - Done.");
                SystemState.Instance().SetIdle();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ModellerPageExtenderG::AutoTeachProcess - {0}{1}{2}"
                    , ex.Message, Environment.NewLine, ex.StackTrace));

                SystemState.Instance().SetAlarm(ex.Message);
                //Thread.Sleep(3000);
                //MessageForm.Show(null, ex.Message);
                //SystemState.Instance().SetIdle();
            }
        }
    }
}