using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Vision.RCI.Trainer
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RCITrainResult
    {
        [Category("General"), DisplayName("IsValid"), ReadOnly(true)]
        public bool IsValid => IsValidd();

        [Category("Uniformize"), DisplayName("Coefficients"), ReadOnly(true)]
        public double[] UniformizeCoefficients { get; set; }
        [Category("Uniformize"), DisplayName("Length"), ReadOnly(true)]
        public int UniformizeLength{ get; set; }

        [Category("Images"), DisplayName("ImageD"), ReadOnly(true)]
        public ImageD ImageD { get; set; }
        [Category("Images"), DisplayName("ReconImageD"), ReadOnly(true)]
        public ImageD ReconImageD { get; set; }
        [Category("Images"), DisplayName("BgImageD"), ReadOnly(true)]
        public ImageD BgImageD { get; set; }
        [Category("Images"), DisplayName("WeightImageD"), ReadOnly(true)]
        public ImageD WeightImageD { get; set; }

        [Category("Pattern"), DisplayName("Rectangle"), ReadOnly(true)]
        public Rectangle ROI { get; set; }
        [Category("Pattern"), DisplayName("Slope [dy/dx]"), ReadOnly(true)]
        public float Slope { get; set; }
        [Category("Pattern"), DisplayName("Slope [Deg]"), ReadOnly(true)]
        public float SlopeDeg => (float)MathHelper.RadToDeg(Math.Atan(Slope));

        [Category("Pattern"), DisplayName("Histogram Mid GV"), ReadOnly(true)]
        public float Otsu { get; set; }
        [Category("Pattern"), DisplayName("Dark Mean"), ReadOnly(true)]
        public float DarkMean { get; set; }

        [Category("Pattern"), DisplayName("White Mean"), ReadOnly(true)]
        public float WhiteMean { get; set; }

        [Category("Features"), DisplayName("X"), ReadOnly(true)]
        public SpikeCollection FeatureX { get; private set; }
        [Category("Features"), DisplayName("Y"), ReadOnly(true)]
        public SpikeCollection FeatureY { get; private set; }
        [Category("Features"), DisplayName("RightToLeft"), ReadOnly(true)]
        public bool RightToLeft { get; private set; }

        [Category("Inspects"), DisplayName("WorkRects"), ReadOnly(true)]
        public WorkPoint[] WorkPoints { get; private set; }
        [Category("Inspects"), DisplayName("Rows"), ReadOnly(true)]
        public int WorkPointRowCount { get; set; }
        [Category("Inspects"), DisplayName("Columns"), ReadOnly(true)]
        public int WorkPointColumnCount { get; set; }

        [Category("Debug"), DisplayName("Elapsed Time [ms]"), ReadOnly(true)]
        public float ElapsedMs { get; set; }
        [Category("Debug"), DisplayName("DebugImageD"), ReadOnly(true)]
        public ImageD DebugImageD { get; set; }

        public bool IsValidd()
        {
            if (this.ReconImageD == null)
                return false;

            if (this.BgImageD == null)
                return false;

            if (this.WeightImageD == null)
                return false;

            if (this.ROI.Width <= 0 || this.ROI.Height <= 0)
                return false;

            if (this.WorkPoints == null)
                return false;
            return true;
        }

        public void ThrowIfInvalid()
        {
            if (this.ReconImageD == null)
                throw new Exception("Invalid Masterimage");

            if (this.BgImageD == null)
                throw new Exception("Invalid Backgroudimage");

            if (this.WeightImageD == null)
                throw new Exception("Invalid WeightImage");

            if (this.ROI.Width <= 0 || this.ROI.Height <= 0)
                throw new Exception("Invalid Pattern Area");

            if (this.WorkPoints == null)
                throw new Exception("Invalid Unit Area");
        }

        public RCITrainResult() { }

        public void Clear()
        {
            this.UniformizeCoefficients = null;
            this.UniformizeLength = 0;
            
            this.ROI = Rectangle.Empty;
            this.Slope = 0;
            this.Otsu = 0;
            this.DarkMean = 0;
            this.WhiteMean = 0;

            FeatureX = null;
            FeatureY = null;
            this.RightToLeft = false;

            this.WorkPointRowCount = 0;
            this.WorkPointColumnCount = 0;

            this.WorkPoints = null;
            
            this.ReconImageD?.Dispose();
            this.ReconImageD = null;
            this.BgImageD?.Dispose();
            this.BgImageD = null;
            this.WeightImageD?.Dispose();
            this.WeightImageD = null;
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, null);
                return;
            }

            if (this.UniformizeCoefficients != null)
                XmlHelper.SetValue(xmlElement, "UniformizeCoefficient", this.UniformizeCoefficients);
            XmlHelper.SetValue(xmlElement, "UniformizeLength", this.UniformizeLength);

            //this.ImageD?.SaveImage(@"C:\temp\ImageD.png");
            //this.ReconImageD?.SaveImage(@"C:\temp\ReconImageD.png");
            //this.BgImageD?.SaveImage(@"C:\temp\BgImageD.png");
            //this.WeightImageD?.SaveImage(@"C:\temp\WeightImageD.png");

            XmlHelper.SetValue(xmlElement, "ROI", this.ROI);
            XmlHelper.SetValue(xmlElement, "Slope", this.Slope);
            XmlHelper.SetValue(xmlElement, "Otsu", this.Otsu);
            XmlHelper.SetValue(xmlElement, "DarkMean", this.DarkMean);
            XmlHelper.SetValue(xmlElement, "WhiteMean", this.WhiteMean);

            FeatureX?.Save(xmlElement, "X");
            FeatureY?.Save(xmlElement, "Y");
            XmlHelper.SetValue(xmlElement, "RightToLeft", this.RightToLeft);

            XmlHelper.SetValue(xmlElement, "WorkRectRowCount", this.WorkPointRowCount);
            XmlHelper.SetValue(xmlElement, "WorkRectColumnCount", this.WorkPointColumnCount);
            if (this.WorkPoints != null)
                XmlHelper.SetValues(xmlElement, "WorkPoints", this.WorkPoints);

            //this.DebugImageD?.SaveImage(@"C:\temp\DebugImageD.png");
        }

        public void SaveImages(string path)
        {
            try
            {
                Task[] tasks = new Task[]
                {
                    Task.Run(() => this.ImageD?.SaveImage(Path.Combine(path, "ImageD.png"))),
                    Task.Run(() => this.ReconImageD?.SaveImage(Path.Combine(path, "ReconImageD.png"))),
                    Task.Run(() => this.BgImageD?.SaveImage(Path.Combine(path, "BgImageD.png"))),
                    Task.Run(() => this.WeightImageD?.SaveImage(Path.Combine(path, "WeightImageD.png"))),
                    Task.Run(() => this.DebugImageD?.SaveImage(Path.Combine(path, "DebugImageD.png"))),
                };
                Array.ForEach(tasks, f => f.Wait());
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
            }
        }

        public static RCITrainResult Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return Load(xmlElement[key], null);
            }

            RCITrainResult result = new RCITrainResult();
            if (xmlElement == null)
                return result;

            double[] uniformizeCoefficients = null;
            XmlHelper.GetValue(xmlElement, "UniformizeCoefficient", ref uniformizeCoefficients);
            result.UniformizeCoefficients = uniformizeCoefficients;
            result.UniformizeLength = XmlHelper.GetValue(xmlElement, "UniformizeLength", result.UniformizeLength);

            //result.ImageD = new Image2D(@"C:\temp\ImageD.png");
            //result.ReconImageD = new Image2D(@"C:\temp\ReconImageD.png");
            //result.BgImageD = new Image2D(@"C:\temp\BgImageD.png");
            //result.WeightImageD = new Image2D(@"C:\temp\WeightImageD.png");

            result.ROI = XmlHelper.GetValue(xmlElement, "ROI", result.ROI);
            result.Slope = XmlHelper.GetValue(xmlElement, "Slope", result.Slope);
            result.Otsu = XmlHelper.GetValue(xmlElement, "Otsu", result.Otsu);
            result.DarkMean = XmlHelper.GetValue(xmlElement, "DarkMean", result.DarkMean);
            result.WhiteMean = XmlHelper.GetValue(xmlElement, "WhiteMean", result.WhiteMean);

            result.FeatureX = SpikeCollection.Load(xmlElement, "X");
            result.FeatureY = SpikeCollection.Load(xmlElement, "Y");
            result.RightToLeft = XmlHelper.GetValue(xmlElement, "RightToLeft", result.RightToLeft);

            result.WorkPointRowCount = XmlHelper.GetValue(xmlElement, "WorkRectRowCount", result.WorkPointRowCount);
            result.WorkPointColumnCount = XmlHelper.GetValue(xmlElement, "WorkRectColumnCount", result.WorkPointColumnCount);

            int workRectItemCount = XmlHelper.GetItemCount(xmlElement, "WorkPoints");
            Debug.Assert(result.WorkPointRowCount * result.WorkPointColumnCount == workRectItemCount);
            result.WorkPoints = new WorkPoint[workRectItemCount];
            for (int i = 0; i < workRectItemCount; i++)
                result.WorkPoints[i] = new WorkPoint(-1, 0, 0, Point.Empty, Size.Empty);
            XmlHelper.GetValues(xmlElement, "WorkPoints", result.WorkPoints);

            return result;
        }

        public void LoadImages(string path)
        {
            try
            {
                string[] files = new string[]
                {
                    Path.Combine(path, "ImageD.png"),
                    Path.Combine(path, "ReconImageD.png"),
                    Path.Combine(path, "BgImageD.png"),
                    Path.Combine(path, "WeightImageD.png")
                };

                Task[] tasks = new Task[]
                {
                    Task.Run(() => this.ImageD = File.Exists(files[0])? new Image2D(files[0]) : null),
                    Task.Run(() => this.ReconImageD = File.Exists(files[1]) ? new Image2D(files[1]) : null),
                    Task.Run(() => this.BgImageD = File.Exists(files[2]) ? new Image2D(files[2]) : null),
                    Task.Run(() => this.WeightImageD = File.Exists(files[3]) ? new Image2D(files[3]):null)
                };

                Array.ForEach(tasks, f => f.Wait());
                //this.DebugImageD = new Image2D(Path.Combine(path, "DebugImageD.png"));
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                this.ImageD = null;
                this.ReconImageD = null;
                this.BgImageD = null;
                this.WeightImageD = null;
                this.DebugImageD = null;
            }
        }

        public byte[] GetPRNUDatas(int length, byte targetGv)
        {
            byte[] datas = new byte[length];
            for (int i = 0; i < length; i++)
            {
                double p = i * 1.0 / length;
                int l = (int)Math.Round(p * this.UniformizeLength);
                double f = (UniformizeCoefficients[2] * l * l + UniformizeCoefficients[1] * l + UniformizeCoefficients[0]);
                datas[i] = (byte)MathHelper.Clipping(targetGv / f * 100, byte.MinValue, byte.MaxValue);
            }
            return datas;
        }

        public void Update(SpikeCollection featureX, SpikeCollection featureY, AlgoImage algoImage, Size prjInflate, bool rightToLeft, BackgroundWorker backgroundWorker)
        {
            this.FeatureX = featureX;
            this.FeatureY = featureY;
            this.RightToLeft = rightToLeft;
            UpdateWorkPoints(algoImage, prjInflate, rightToLeft, backgroundWorker);
        }

        private void UpdateWorkPoints(AlgoImage algoImage, Size inflate, bool rightToLeft, BackgroundWorker backgroundWorker)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            Rectangle fullRect = new Rectangle(Point.Empty, algoImage.Size);

            WorkPoint[] workPoints = GetWorkPoints(out int xPitch, out Size prjInflate);
            //Tuple<WorkRect[], int> workRects = Program.TimeCheck("AnalyzeResult::GetWorkRects", new Func<Tuple<WorkRect[], int>>(() => GetWorkRects()));

            this.WorkPoints = workPoints;
            this.WorkPointRowCount = workPoints.Length / xPitch;
            this.WorkPointColumnCount = xPitch;
            Size workRectCnt = new Size(this.WorkPointColumnCount, this.WorkPointRowCount);

            using (AlgoImage bgImage = ImageBuilder.Build(algoImage.LibraryType, this.BgImageD, ImageType.Grey))
            {
                for (int i = 0; i < this.WorkPoints.Length; i++)
                {
                    if (backgroundWorker.CancellationPending)
                        throw new OperationCanceledException();

                    WorkPoint f = this.WorkPoints[i];
                    Point point = f.Point;
                    if (rightToLeft)
                        point.X = bgImage.Width - point.X;
                    Rectangle rect = DrawingHelper.FromCenterSize(point, Size.Empty);
                    rect.Inflate(prjInflate);
                    int inflateW = Math.Min(inflate.Width, Math.Min(rect.Left - fullRect.Left, fullRect.Right - rect.Right));
                    int inflateH = Math.Min(inflate.Height, Math.Min(fullRect.Bottom - rect.Bottom, rect.Top - fullRect.Top));
                    Size adjInflate = new Size(inflateW, inflateH);
                    rect.Inflate(adjInflate);
                    //Debug.Assert(inflateW > 0 && inflateH > 0);
                    //Debug.Assert(Rectangle.Intersect(rect, fullRect) == rect);
                    //Debug.Assert(f.Row == 93);

                    f.Use = (Rectangle.Intersect(rect, fullRect) == rect) && inflateW > 0 && inflateH > 0;
                    if (f.Use)
                    {
                        using (AlgoImage subImage = algoImage.GetSubImage(rect))
                        {
                            f.UpdateProjection(subImage, adjInflate);
                            //subImage.Save($@"D:\temp\RCITrainProjections\{i:00000}.bmp");
                            using (AlgoImage subMaskImage = bgImage.GetSubImage(rect))
                            {
                                //subImage.Save(@"C:\temp\temp\subImage.png");
                                //subMaskImage.Save(@"C:\temp\temp\subMaskImage.png");
                                f.MeanBgGv = ip.GetGreyAverage(subImage, subMaskImage);
                            }
                        }
                    }
                    else
                    {
                        f.UpdateProjection(null, Size.Empty);
                    }
                }
            }
        }

        private WorkPoint[] GetWorkPoints(out int xPitch, out Size prjInflate)
        {
            Tuple<int, int>[] xRanges = this.FeatureX.GetRanges(false, out int marginX);
            Tuple<int, int>[] yRanges = this.FeatureY.GetRanges(false, out int marginY);

            Tuple<int, int> centerTuple = xRanges[xRanges.Length / 2];
            double xCenter = centerTuple.Item1;
            int[] yOffsets = xRanges.Select(f => (int)Math.Round((f.Item1 - xCenter) * Slope)).ToArray();

            int pitch = xRanges.Length;
            WorkPoint[] workPoints = yRanges.SelectMany((g, y) =>
            {
                return xRanges.Select((f, x) =>
                {
                    Rectangle modelRect = Rectangle.FromLTRB(f.Item1, g.Item1, f.Item2, g.Item2);
                    modelRect.Offset(0, yOffsets[x]);
                    modelRect.Offset(0, -Math.Min(0, modelRect.Y));
                    Debug.Assert(modelRect.Width > 0 && modelRect.Height > 0);
                    Point loc = modelRect.Location;
                    Size size = modelRect.Size;

                    return new WorkPoint(y * pitch + x, x, y, loc, size);
                });
            }).ToArray();

            xPitch = pitch;
            prjInflate = new Size(marginX, marginY);
            return workPoints;
        }

        //private WorkPoint[] FindSimilarity(AlgoImage algoImage, WorkPoint workRect, WorkPoint[] workRects, float v)
        //{
        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    //using (AlgoImage workImage = algoImage.GetSubImage(workRect.Rectangle))
        //    //    workImage.Save(@"C:\temp\workImage.png");

        //    Rectangle refRect = workRect.Rectangle;
        //    AlgoImage bufImage = algoImage.Clip(refRect);
        //    AlgoImage refImage = algoImage.Clip(refRect);
        //    ip.Sobel(refImage, refImage);


        //    var va = workRects.GroupBy(f =>
        //    {
        //        int index = Array.IndexOf(this.WorkRects, f);
        //        int col = index / this.WorkRectRowCount;
        //        int row = index % this.WorkRectRowCount;

        //        return col;
        //    }).ToArray();

        //    Tuple<WorkPoint, float>[] tuples = va.SelectMany(f =>
        //    {
        //        Tuple<WorkPoint, float>[] scoreTuples = f.Select(g =>
        //        {
        //            Rectangle rect = g.Rectangle;

        //            float diffW = Math.Abs(rect.Width - refRect.Width) * 1f / (rect.Width + refRect.Width) * 100;
        //            float diffH = Math.Abs(rect.Height - refRect.Height) * 1f / (rect.Height + refRect.Height) * 100;
        //            if (diffW > 5 || diffH > 5)
        //                return new Tuple<WorkPoint, float>(g, float.NaN);

        //            using (AlgoImage image = algoImage.GetSubImage(rect))
        //                ip.Resize(image, bufImage);
        //            ip.Sobel(bufImage, bufImage);
        //            ip.Subtract(refImage, bufImage, bufImage, true);

        //            StatResult statResult = ip.GetStatValue(bufImage);
        //            float score = (float)statResult.average;
        //            return new Tuple<WorkPoint, float>(g, score);
        //        }).ToArray();

        //        IEnumerable<Tuple<WorkPoint, float>> ee = Array.FindAll(scoreTuples, g => !float.IsNaN(g.Item2));
        //        if (ee.Count() == 0)
        //            return scoreTuples;

        //        float average = ee.Average(g => g.Item2);
        //        float stdDev = MathHelper.StdDev(ee.Select(g => g.Item2));
        //        return scoreTuples.Select(g => new Tuple<WorkPoint, float>(g.Item1, g.Item2 - average + (float.IsNaN(stdDev) ? 0 : stdDev)));
        //        //return Array.FindAll(scoreTuples, g => g.Item2 > average).Select(g => g.Item1);
        //    }).ToArray();

        //    var vv = tuples.GroupBy(f =>
        //    {
        //        int idx = Array.IndexOf(this.WorkRects, f.Item1);
        //        return idx % this.WorkRectRowCount;
        //    }).OrderBy(f => f.Key);

        //    //int iii = Array.IndexOf(this.WorkRects, workRect);
        //    //System.IO.File.WriteAllText(@"C:\temp\temp\temp.txt", string.Join(Environment.NewLine, vv.Select((f, i) => string.Join(",", f.Select(g => g.Item2.ToString("F2"))))));
        //    //System.IO.File.WriteAllText(@"C:\temp\temp\temp2.txt", string.Join(Environment.NewLine, Array.FindAll(tuples, f => f.Item2 < 1).Select(f => Array.IndexOf(this.WorkRects, f.Item1).ToString())));

        //    refImage.Dispose();
        //    bufImage.Dispose();

        //    return Array.FindAll(tuples, f => f.Item2 < 1).Select(f => f.Item1).ToArray();
        //}

        //private ImageD BuildMasterImage(AlgoImage algoImage, WorkPoint[] workRects)
        //{
        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    int length = workRects.Length;
        //    int widthMin = workRects.Min(f => f.Rectangle.Width);
        //    int heightMin = workRects.Min(f => f.Rectangle.Height);
        //    Size masterSize = new Size(widthMin, heightMin);

        //    using (AlgoImage accum = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, masterSize))
        //    {
        //        AlgoImage work = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, masterSize);

        //        Array.ForEach(workRects, f =>
        //        {
        //            Rectangle rect = f.Rectangle;
        //            Size sizeDiff = new Size(rect.Width - masterSize.Width, rect.Height - masterSize.Height);

        //            Rectangle rectM = new Rectangle(f.Rectangle.Location, masterSize);
        //            rectM.Offset(sizeDiff.Width / 2, sizeDiff.Height / 2);

        //            using (AlgoImage subAlgoImage = algoImage.GetSubImage(rectM))
        //                ip.Add(subAlgoImage, accum, accum);
        //        });

        //        work.Dispose();

        //        ip.Div(accum, accum, length);
        //        //accum.Save(@"C:\temp\accum.png");
        //        return accum.ToImageD();
        //    }
        //}

        public Figure GetPatternFigure()
        {
            FigureGroup figureGroup = new FigureGroup();
            if (this.IsValid)
                figureGroup.AddFigure(new RectangleFigure(this.ROI, new Pen(Color.Yellow, 2)));
            return figureGroup;
        }

        public Figure GetRegionFigure()
        {
            FigureGroup figureGroup = new FigureGroup();
            if (this.IsValid)
            {
                Pen pen = new Pen(Color.Cyan);
                Brush notUseBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));

                Array.ForEach(this.WorkPoints, f =>
                {
                    if (f.Column > 0 && f.Row > 0)
                        return;

                    Rectangle rect = RCIHelper.Anchor(this.RightToLeft, this.ROI.Size, f.GetTeachRectangle());
                    rect.Offset(this.ROI.Location);

                    figureGroup.AddFigure(new RectangleFigure(rect, pen));
                    if (!f.Use)
                        figureGroup.AddFigure(new RectangleFigure(rect, pen, notUseBrush));
                    //if (model.RCIOptions.ShowTextFigure)
                    //{
                    //    figureGroup.AddFigure(new TextFigure($"{f.Index}", rect.Location, new Font("맑은 고딕", 12), Color.Cyan, StringAlignment.Near, StringAlignment.Near));
                    //    figureGroup.AddFigure(new TextFigure($"H: {f.Projection.ScoreH}", DrawingHelper.Add(rect.Location, 0, 12), new Font("맑은 고딕", 12), Color.Cyan, StringAlignment.Near, StringAlignment.Near));
                    //    figureGroup.AddFigure(new TextFigure($"V: {f.Projection.ScoreV}", DrawingHelper.Add(rect.Location, 0, 24), new Font("맑은 고딕", 12), Color.Cyan, StringAlignment.Near, StringAlignment.Near));
                    //}
                });
            }
            return figureGroup;
        }
    }
}
