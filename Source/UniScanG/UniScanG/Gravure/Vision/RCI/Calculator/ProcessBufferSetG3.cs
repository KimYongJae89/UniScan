using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;
using UniEye.Base.Settings;
using UniScanG.Data.Model;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace UniScanG.Gravure.Vision.RCI.Calculator
{
    public class ProcessBufferSetG3 : ProcessBufferSetG
    {
        public Model Model { get; private set; }

        public byte[] UniformalizeFactor { get; private set; }

        public AlgoImage FindRoiLineBuffer { get; private set; }

        public AlgoImage ModelRoiImage { get; private set; }
        public AlgoImage ModelBgRoiImage { get; private set; }
        public AlgoImage WeightRoiImage { get; private set; }

        public AlgoImage TargetFullImage { get; private set; }
        public AlgoImage ResultGrayFullImage { get; private set; }
        public AlgoImage ResultBinFullImage { get; private set; }

        public AlgoImage PreviewBuffer { get; private set; }


        public WorkPoint[] WorkPoints { get; private set; }
        public int WorkPointColumnCount { get; private set; }
        public int WorkPointRowCount { get; private set; }

        public WorkPoint[][] WorkRectsByColumn { get; private set; }
        public WorkPoint[][] WorkRectsByRow { get; private set; }
        public WorkPoint[][] AlignRectsByVertical { get; private set; }
        public WorkPoint[][] AlignRectsByHorizontal { get; private set; }
        public WorkPoint[] AlignRects { get; private set; }

        public List<int> BlockKeys { get; private set; }
        public List<BlockProjection> BlockPrjImg { get; private set; }
        public List<BlockProjection> BlockBufImg { get; private set; }
        public List<BlockProjection> BlockResImg { get; private set; }
        public List<PTMLogger> PTMLogger { get; private set; }

        public BlockResult[] BlockResults { get; set; }
        public Rectangle TargetRoi { get; set; }
        
        public Task PrevBitmapBuildTask { get; private set; }

        public ProcessBufferSetG3(Model model, float scaleFactor, bool isMultiLayer, int width, int height) : base(scaleFactor, isMultiLayer, width, height)
        {
            this.Model = model;

            this.BlockKeys = new List<int>();
            this.BlockPrjImg = new List<BlockProjection>();
            this.BlockBufImg = new List<BlockProjection>();
            this.BlockResImg = new List<BlockProjection>();
            this.PTMLogger = new List<PTMLogger>();

            this.BlockResults = null;
            this.TargetRoi = Rectangle.Empty;
        }

        public override void BuildBuffers(bool halfScale)
        {
            base.BuildBuffers(halfScale);

            ImagingLibrary imagingLibrary = this.fullAlgoImage.LibraryType;

            RCITrainResult trainResult = this.Model.RCITrainResult;
            RCIOptions options = this.Model.RCIOptions;

            int pitchLib = this.fullAlgoImage.PitchLib;
            IntPtr intPtr = this.fullAlgoImage.GetImagePtr();

            this.UniformalizeFactor = trainResult.GetPRNUDatas(pitchLib, AlgorithmSetting.Instance().RCIGlobalOptions.UniformizeGv);

            this.WorkPoints = trainResult.WorkPoints.Select(f => (WorkPoint)f.Clone()).ToArray();
            this.WorkPointRowCount = trainResult.WorkPointRowCount;
            this.WorkPointColumnCount = trainResult.WorkPointColumnCount;
            UpdateAlighRects(options.PTMCorrectionCount);

            this.FindRoiLineBuffer = ImageBuilder.Build(imagingLibrary, ImageType.Depth, (int)(Math.Max(this.width, this.height)), 1);

            this.ModelRoiImage = ImageBuilder.Build(imagingLibrary, trainResult.ReconImageD, ImageType.Grey);
            this.ModelBgRoiImage = ImageBuilder.Build(imagingLibrary, trainResult.BgImageD, ImageType.Grey);
            this.WeightRoiImage = ImageBuilder.Build(imagingLibrary, trainResult.WeightImageD, ImageType.Grey);

            //this.TargetFullImage = ImageBuilder.Build(imagingLibrary, ImageType.Grey, this.width, this.height);
            this.ResultGrayFullImage = ImageBuilder.Build(imagingLibrary, ImageType.Grey, this.width, this.height);
            this.ResultBinFullImage = ImageBuilder.Build(imagingLibrary, ImageType.Grey, this.width, this.height);

            float previewScale = Common.Settings.SystemTypeSettings.Instance().ResizeRatio;
            Size previewSize = DrawingHelper.Mul(new Size(this.width, this.height), previewScale);
            this.PreviewBuffer = ImageBuilder.Build(imagingLibrary, ImageType.Grey, previewSize);

            //this.algoImage = TargetFullImage; // original
            this.calculatorResultGray = ResultGrayFullImage; // calc result
            this.calculatorResultBinal = ResultBinFullImage; // binal - defects
        }

        private void UpdateAlighRects(int correctionCount)
        {
            Action<WorkPoint> BuildPrjAction = new Action<WorkPoint>(f =>
            {
                if (f == null || !f.IsReference)
                    return;

                if (f.Projection.PrjH.Length == 0 || f.Projection.PrjV.Length == 0)
                    return;

                if (!this.BlockKeys.Contains(f.Index))
                {
                    this.BlockKeys.Add(f.Index);
                    int id = this.BlockKeys.IndexOf(f.Index);

                    Tuple<BlockProjection, BlockProjection, BlockProjection> tuple = BuildProjection(f);
                    this.BlockPrjImg.Add(tuple.Item1);
                    this.BlockBufImg.Add(tuple.Item2);
                    this.BlockResImg.Add(tuple.Item3);
                    this.PTMLogger.Add(new PTMLogger()
                    {
                        WorkPoint = f,
                    });
                }
            });

            this.WorkRectsByColumn = this.WorkPoints.GroupBy(f => f.Column).Select(f => f.OrderBy(g => g.Row).ToArray()).ToArray();
            this.WorkRectsByRow = this.WorkPoints.GroupBy(f => f.Row).Select(f => f.OrderBy(g => g.Column).ToArray()).ToArray();

            // Vertical Group for Correcting Y-Axis
            {
                this.AlignRectsByVertical = new WorkPoint[2][];
                this.AlignRectsByVertical[0] = this.WorkRectsByRow.Select(f =>
                {
                    WorkPoint[] first5 = f.OrderBy(g => g.Column).Take(5).OrderByDescending(g => g.Projection.ScoreV).ToArray();
                    return first5.First();
                }).ToArray();

                this.AlignRectsByVertical[1] = this.WorkRectsByRow.Select(f =>
                {
                    WorkPoint[] last5 = f.OrderByDescending(g => g.Column).Take(5).OrderByDescending(g => g.Projection.ScoreV).ToArray();
                    return last5.First();
                }).ToArray();

                int[] colsL = this.AlignRectsByVertical[0].Select(f => f == null ? -1 : f.Column).ToArray();
                int[] colsR = this.AlignRectsByVertical[1].Select(f => f == null ? -1 : f.Column).ToArray();

                for (int i = 0; i < this.AlignRectsByVertical.Length; i++)
                {
                    for (int j = 0; j < this.AlignRectsByVertical[i].Length; j++)
                    {
                        if (this.AlignRectsByVertical[i][j] != null &&
                            this.AlignRectsByVertical[i][j].Projection.PrjH.Length > 0 &&
                            this.AlignRectsByVertical[i][j].Projection.PrjV.Length > 0)
                        {
                            this.AlignRectsByVertical[i][j].IsReferenceY = true;
                            BuildPrjAction(this.AlignRectsByVertical[i][j]);
                        }
                    }
                }
            }

            // Horizontal Group for Correcting X-Axis
            {
                if (true)
                {
                    List<int> rowList = new List<int>();
                    rowList.Add(0);
                    rowList.Add(this.WorkRectsByRow.Length - 1);

                    if (correctionCount > 0)
                    {
                        float step = this.WorkRectsByRow.Length * 1f / (correctionCount + 1);
                        for (int i = 0; i < correctionCount; i++)
                        {
                            int value = (int)Math.Round(step * (i + 1));
                            if (!rowList.Contains(value))
                                rowList.Add(value);
                        }
                    }
                    rowList.Sort();

                    List<WorkPoint[]> list = new List<WorkPoint[]>();
                    foreach(int row in rowList)
                    {
                        //WorkPoint[] rects = this.WorkRectsByColumn.Select(f => f.OrderBy(g => Math.Abs(g.Row - row)).First(g => g.Projection.CanPTMReferenceH)).ToArray();
                        WorkPoint[] rects = this.WorkRectsByColumn.Select(f =>
                         {
                             IEnumerable<WorkPoint> @enums = f.OrderBy(g => Math.Abs(g.Row - row)).Take(5).OrderByDescending(g => g.Projection.ScoreH);
                             return @enums.First();
                         }).ToArray();

                        Array.ForEach(rects, f =>
                        {
                            f.IsReferenceX = rowList.IndexOf(row);
                            BuildPrjAction(f);
                        });

                        list.Add(rects);
                    }
                    this.AlignRectsByHorizontal = list.ToArray();
                }
                else
                {
                    if (correctionCount > 0)
                    {
                        List<WorkPoint[]> list = new List<WorkPoint[]>();
                        for (int row = 0; row < this.WorkRectsByRow.Length; row++)
                        {
                            bool onPeriod = row % correctionCount == 0;
                            if (!onPeriod)
                                continue;

                            WorkPoint[] rects = this.WorkRectsByColumn.Select(f => f.OrderBy(g => Math.Abs(g.Row - row)).Take(3).OrderByDescending(g => g.Projection.ScoreH).First(g => g.Projection.CanPTMReferenceH)).ToArray();
                            Array.ForEach(rects, f =>
                            {
                                f.IsReferenceX = row / correctionCount;
                                if (onPeriod)
                                    BuildPrjAction(f);
                            });

                            if (onPeriod)
                                list.Add(rects);
                        }
                        this.AlignRectsByHorizontal = list.ToArray();
                    }
                    else
                    {
                        this.AlignRectsByHorizontal = new WorkPoint[2][];
                        this.AlignRectsByHorizontal[0] = this.WorkRectsByColumn.Select(f => f.First(g => g.Projection.CanPTMReferenceH)).ToArray();
                        this.AlignRectsByHorizontal[1] = this.WorkRectsByColumn.Select(f => f.Last(g => g.Projection.CanPTMReferenceH)).ToArray();
                        for (int i = 0; i < this.AlignRectsByHorizontal.Length; i++)
                        {
                            for (int j = 0; j < this.AlignRectsByHorizontal[i].Length; j++)
                            {
                                if (this.AlignRectsByHorizontal[i][j] != null)
                                {
                                    this.AlignRectsByHorizontal[i][j].IsReferenceX = i;
                                    BuildPrjAction(this.AlignRectsByHorizontal[i][j]);
                                }
                            }
                        }
                    }
                }
            }

            List<WorkPoint> overallList = new List<WorkPoint>();
            overallList.AddRange(this.AlignRectsByHorizontal.SelectMany(f => f.Select(g => g)));
            overallList.AddRange(this.AlignRectsByVertical.SelectMany(f => f.Select(g => g)));
            this.AlignRects = overallList.ToArray();
        }

        private static Tuple<BlockProjection, BlockProjection, BlockProjection> BuildProjection(WorkPoint f)
        {
            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(Vision.Calculator.CalculatorBase.TypeName);
            ImagingLibrary imagingLibrary = algorithmStrategy.LibraryType;

            ProjectionData projection = f.Projection;

            AlgoImage blockPrjImgW = ImageBuilder.Build(imagingLibrary, ImageType.Grey, projection.PrjH.Length, 1);
            AlgoImage blockPrjImgH = ImageBuilder.Build(imagingLibrary, ImageType.Grey, projection.PrjV.Length, 1);
            BlockProjection blockPrjImg = new BlockProjection(blockPrjImgW, blockPrjImgH, projection.Inflate);

            blockPrjImgW.PutByte(RCIHelper.AbsRound(f.Projection.PrjH));
            blockPrjImgH.PutByte(RCIHelper.AbsRound(f.Projection.PrjV));

            AlgoImage blockPrjBufW = ImageBuilder.Build(imagingLibrary, ImageType.Grey, projection.PrjH.Length - projection.Inflate.Width * 2, 1);
            AlgoImage blockPrjBufH = ImageBuilder.Build(imagingLibrary, ImageType.Grey, projection.PrjV.Length - projection.Inflate.Height * 2, 1);
            BlockProjection blockBufImg = new BlockProjection(blockPrjBufW, blockPrjBufH, Size.Empty);

            AlgoImage resultWImg = ImageBuilder.Build(imagingLibrary, ImageType.Depth, blockPrjImgW.Width - blockPrjBufW.Width + 1, 1);
            AlgoImage resultHImg = ImageBuilder.Build(imagingLibrary, ImageType.Depth, blockPrjImgH.Width - blockPrjBufH.Width + 1, 1);
            BlockProjection blockResImg = new BlockProjection(resultWImg, resultHImg, Size.Empty);

            resultWImg.Clear();
            resultHImg.Clear();

            return new Tuple<BlockProjection, BlockProjection, BlockProjection>(blockPrjImg, blockBufImg, blockResImg);
        }


        public override void Dispose()
        {
            base.Dispose();

            this.FindRoiLineBuffer?.Dispose();

            this.BlockKeys.Clear();

            foreach (var v in this.BlockBufImg)
                v.Dispose();
            this.BlockBufImg.Clear();

            foreach (var v in this.BlockPrjImg)
                v.Dispose();
            this.BlockPrjImg.Clear();

            foreach (var v in this.BlockResImg)
                v.Dispose();
            this.BlockResImg.Clear();

            this.PreviewBuffer?.Dispose();
            this.ResultBinFullImage?.Dispose();
            this.ResultGrayFullImage?.Dispose();
            this.TargetFullImage = null;
            //this.TargetFullImage?.Dispose();
            this.WeightRoiImage?.Dispose();
            this.ModelBgRoiImage?.Dispose();
            this.ModelRoiImage?.Dispose();

            this.WorkRectsByColumn = null;
            this.WorkRectsByRow = null;
            this.AlignRectsByVertical = null;
            this.AlignRectsByHorizontal = null;
            this.AlignRects = null;

            this.calculatorResultGray = null;
            this.calculatorResultBinal = null;
        }

        public override void Download()
        {
        }

        public override void Upload(DebugContext debugContext)
        {
            if (this.UniformalizeFactor.Length == this.fullAlgoImage.PitchLib)
            {
                //byte[] bytes = this.fullAlgoImage.GetByte();
                //RCIHelperWithSIMD.IterateProduct(bytes, this.UniformalizeFactor, bytes, this.fullAlgoImage.Size);
                //this.fullAlgoImage.SetByte(bytes);

                IntPtr ptr = this.fullAlgoImage.GetImagePtr();
                int pitch = this.fullAlgoImage.PitchLib;
                int height = this.fullAlgoImage.Height;
                byte[] datas = this.UniformalizeFactor;

                SIMD.IterateProduct(ptr, datas, ptr, pitch, algoImage.Height);
            }

            this.TargetFullImage = this.algoImage;

            this.PrevBitmapBuildTask = StartBitmapBuildTask();
        }

        public override void WaitDone()
        {
            if (this.PrevBitmapBuildTask == null)
                return;

            while (!this.PrevBitmapBuildTask.IsCompleted)
                System.Threading.Thread.Sleep(10);
        }

        private Task StartBitmapBuildTask()
        {
            return Task.Run(() =>
            {
                float previewScale = Common.Settings.SystemTypeSettings.Instance().ResizeRatio;
                Vision.AlgorithmCommon.ScaleImage(this.algoImage, this.PreviewBuffer, previewScale);

                Rectangle fullRect = new Rectangle(Point.Empty, this.algoImage.Size);
                Rectangle previewRect = new Rectangle(Point.Empty, this.PreviewBuffer.Size);
                Rectangle resizeRect = DrawingHelper.Mul(fullRect, previewScale);
                resizeRect.Intersect(previewRect);
                LogHelper.Info(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - ResizeRect W{0}, H{1}", resizeRect.Width, resizeRect.Height));

                if (resizeRect.Width > 0 && resizeRect.Height > 0)
                {
                    AlgoImage previewChildBuffer = this.PreviewBuffer.GetSubImage(resizeRect);
                    try
                    {
                        this.prevBitmap = previewChildBuffer.ToBitmap();
                        LogHelper.Info(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - Ok"));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Algorithm, string.Format("ProcessBufferSetG2::StartPreviewBitmapBuild - Exception {0}. {1}", ex.Message, ex.StackTrace));
                    }
                    previewChildBuffer.Dispose();
                }
            });
        }

        public Tuple<BlockProjection, BlockProjection, BlockProjection, PTMLogger> GetPrjBufImg(int key)
        {
            int id = this.BlockKeys.IndexOf(key);
            if (id < 0)
                return null;

            return new Tuple<BlockProjection, BlockProjection, BlockProjection, PTMLogger>(BlockPrjImg[id], BlockBufImg[id], BlockResImg[id], PTMLogger[id]);
        }
    }
}
