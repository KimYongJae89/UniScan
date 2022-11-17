using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.RCI.Calculator;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace RCITest.Processer
{
    public class SimpleBlobResult
    {
        public ImageD SimpleImage { get; }
        public Rectangle ROI { get; }
        public DefectBlob[] DefectBlobs { get; }

        public SimpleBlobResult(ImageD simpleImage, Rectangle roi, DefectBlob[] defectBlobs)
        {
            this.SimpleImage = simpleImage;
            this.ROI = roi;
            this.DefectBlobs = defectBlobs;
        }

        internal FigureGroup GetFigure()
        {
            FigureGroup figureGroup = new FigureGroup();
            Pen pen = new Pen(Color.Yellow);
            Figure[] figures = this.DefectBlobs.Select((f,i) =>
            {
                Rectangle rect = DrawingHelper.FromCenterSize(f.CenterPosPx, new Size(10, 10));
                rect.Offset(this.ROI.Location);
                return new RectangleFigure(rect, pen) { Movable = false, Tag = $"Index: {i}" };
            }).ToArray();

            figureGroup.AddFigure(figures);
            return figureGroup;
        }

        internal void SaveForDebug(string path)
        {
            Task task = Task.Run(() => this.SimpleImage.SaveImage(Path.Combine(path, $"Defect.png")));

            string defectPath = Path.Combine(path, "Defects");
            Directory.CreateDirectory(defectPath);
            FileHelper.ClearFolder(defectPath, $"*.*", false);
            Array.ForEach(this.DefectBlobs, f => f.Save(defectPath, ""));

            task.Wait();
        }
    }

    public class DefectBlob
    {
        public int DIndex { get; set; }
        public int GIndex { get; set; }
        public Point GridPos { get; set; }
        public Point CenterPosPx { get; set; }
        public Size SizePx { get; set; }
        public byte GvMax { get; set; }
        public ImageD BinalImage { get; set; }
        public ImageD GrayImage { get; set; }

        public DefectBlob() { }

        public void Save(string path, string key)
        {
            string message = $"{key}, Idx, {this.DIndex}, GridIdx, {this.GIndex}, GridX, {this.GridPos.X}, GridY, {this.GridPos.Y}, CenterX, {this.CenterPosPx.X}, CenterY, {this.CenterPosPx.Y}, SizeWPx, {this.SizePx.Width}, SizeHPx, {this.SizePx.Height}, GvMax: {this.GvMax}{Environment.NewLine}";
            File.AppendAllText(Path.Combine(path, "Result.txt"), message);

            this.GrayImage.SaveImage(Path.Combine(path, $"{key}_{this.DIndex}_{this.GIndex}.bmp"));
            this.BinalImage.SaveImage(Path.Combine(path, $"{key}_{this.DIndex}B_{this.GIndex}.png"));
        }
    }

    public static class SimpleBlob
    {
        internal static SimpleBlobResult Blob(RCITrainResult analyzeResult, ProcessBufferSetG3 bufferSetG3, CalculatorResultV3 compareResult, byte threshold, Size size)
        {
            ImageProcessing ip = Program.ImageProcessing;
            Rectangle fullRect = new Rectangle(Point.Empty, compareResult.FoundRoi.Size);
            List<DefectBlob> defectBlobList = new List<DefectBlob>();
            ImageD simpleImageD = null;

            AlgoImage simpleRoiImage = null;
            AlgoImage targetRoiImage = null;
            AlgoImage resultRoiImage = null;
            AlgoImage binalRoiImage = null;
            try
            {
                targetRoiImage = bufferSetG3.AlgoImage.GetSubImage(compareResult.FoundRoi);
                resultRoiImage = bufferSetG3.ResultGrayFullImage.GetSubImage(compareResult.FoundRoi);
                binalRoiImage = bufferSetG3.ResultBinFullImage.GetSubImage(compareResult.FoundRoi);

                ip.Binarize(resultRoiImage, binalRoiImage, threshold);

                simpleRoiImage = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Grey, DrawingHelper.Div(compareResult.FoundRoi.Size, 2));
                ip.Resize(targetRoiImage, simpleRoiImage);
                ip.Div(simpleRoiImage, simpleRoiImage, 2);

                BlobParam blobParam = new BlobParam()
                {
                    SelectLabelValue = true,
                    SelectArea = true,
                    SelectBoundingRect = true,
                    SelectRotateRect = true,
                    SelectBorderBlobs = false,
                    SelectCenterPt = false,
                    EraseBorderBlobs = true,
                    SelectGrayMaxValue = true,
                    RotateWidthMin = Math.Max(size.Width, size.Height),
                    RotateHeightMin = Math.Min(size.Width, size.Height),
                };

                using (BlobRectList bList = ip.Blob(binalRoiImage, blobParam, resultRoiImage))
                {
                    BlobRect[] blobRects = bList.GetArray(f => f.BoundingRect.Width >= size.Width && f.BoundingRect.Height >= size.Height);
                    for (int i = 0; i < blobRects.Length; i++)
                    {
                        BlobRect blobRect = blobRects[i];
                        Rectangle rectangle = Rectangle.Round(blobRect.BoundingRect);

                        int index = i;
                        Size sizePx = rectangle.Size;
                        Point centerPt = Point.Round(blobRect.CenterPt);


                        BlockResult blockResult = Array.Find(compareResult.BlockResults, f => f.TargetRect.IntersectsWith(rectangle));

                        DefectBlob defectBlob= new DefectBlob()
                        {
                            DIndex = index,
                            GIndex = blockResult.Index,
                            GridPos = blockResult == null ? new Point(-1, -1) : new Point(blockResult.Column, blockResult.Row),
                            CenterPosPx = centerPt,
                            SizePx = sizePx,
                            GvMax = (byte)blobRect.MaxValue
                        };

                        Rectangle clipRect = Rectangle.Inflate(rectangle, 20, 20);
                        clipRect.Intersect(fullRect);

                        using (AlgoImage subImage = targetRoiImage.GetSubImage(clipRect))
                            defectBlob.GrayImage = subImage.ToImageD();

                        using (AlgoImage subImage = binalRoiImage.GetSubImage(clipRect))
                            defectBlob.BinalImage = subImage.ToImageD();

                        defectBlobList.Add(defectBlob);

                        Rectangle drawRect = DrawingHelper.Div(rectangle, 2);
                        drawRect.Inflate(3, 3);
                        ip.DrawRect(simpleRoiImage, drawRect, 255, true);
                        ip.DrawText(simpleRoiImage, new Point(drawRect.Right, drawRect.Bottom), 255, i.ToString());
                    }
                }
                //simpleRoiImage.Save(@"C:\temp\SimpleBlob\simpleImageD.bmp");
                simpleImageD = simpleRoiImage.ToImageD();
            }
            finally
            {
                binalRoiImage?.Dispose();
                resultRoiImage?.Dispose();
                targetRoiImage?.Dispose();
                simpleRoiImage?.Dispose();
            }

            return new SimpleBlobResult(simpleImageD, compareResult.FoundRoi, defectBlobList.ToArray());
        }
    }
}