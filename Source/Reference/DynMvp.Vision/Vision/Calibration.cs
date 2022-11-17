using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class CalibrationResult
    {
        public bool IsGood
        {
            get { return (this.pelSize.Width > 0 && this.pelSize.Height > 0); }
        }

        public Rectangle calibRect = Rectangle.Empty;
        public Image2D clipImage = null;
        public Image2D binalImage = null;
        public float avgBrightness = -1;
        public float minBrightness = -1;
        public float maxBrightness = -1;
        public float focusValue = -1;
        public SizeF pelSize = SizeF.Empty;
        public float[] projectionData = null;
        public List<PointF> cellDataList = new List<PointF>();
        public List<CalibrationResult> subCalibrationResult = new List<CalibrationResult>();

        public void AppendFigure(FigureGroup figureGroup)
        {
            if (this.calibRect.IsEmpty == false)
                figureGroup.AddFigure(new RectangleFigure(this.calibRect, new Pen(Color.Yellow, 3)));

            bool rbrb = false;
            foreach (PointF cellData in cellDataList)
            {
                Color lineColor = (rbrb ? Color.Red : Color.Blue);
                PointF srcPt = new PointF((float)cellData.X + calibRect.Left, this.calibRect.Top);
                PointF dstPt = new PointF((float)cellData.Y + calibRect.Left, this.calibRect.Top);
                figureGroup.AddFigure(new LineFigure(srcPt, dstPt, new Pen(lineColor, 5)));
                rbrb = !rbrb;
            }

            //foreach (CalibrationResult calibrationResult in subCalibrationResult.Values)
            //    calibrationResult.AppendFigure(figureGroup);
        }
    }


    public enum CalibrationType
    {
        SingleScale, Grid, Ruler
    }

    public enum CalibrationGridType
    {
        Dots, Chessboard
    }

    public abstract class Calibration
    {
        CalibrationType calibrationType = CalibrationType.SingleScale;
        public CalibrationType CalibrationType
        {
            get { return calibrationType; }
            set { calibrationType = value; }
        }

        protected int cameraIndex;
        public int CameraIndex
        {
            get { return cameraIndex; }
            set { cameraIndex = value; }
        }

        protected string datFileName;
        public string DatFileName
        {
            get { return datFileName; }
            set { datFileName = value; }
        }

        protected string gridFileName;
        public string GridFileName
        {
            get { return gridFileName; }
            set { gridFileName = value; }
        }

        public static string TypeName
        {
            get { return "Calibration"; }
        }

        protected SizeF pelSize;
        public SizeF PelSize
        {
            get { return pelSize; }
            set { pelSize = value; }
        }

        Size imageSize;
        public Size ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }

        //Size patternCount = Size.Empty;
        //public Size PatternCount
        //{
        //    get { return patternCount; }
        //    set { patternCount = value; }
        //}

        //SizeF patternSize = new SizeF();
        //public SizeF PatternSize
        //{
        //    get { return patternSize; }
        //    set { patternSize = value; }
        //}

        bool fileLoadSuccessed = true;
        public bool FileLoadSuccessed
        {
            get { return fileLoadSuccessed; }
            set { fileLoadSuccessed = value; }
        }

        /// <summary>
        /// Single Scale
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public CalibrationResult Calibrate(float scaleX, float scaleY)
        {
            this.calibrationType = CalibrationType.SingleScale;
            LogHelper.Debug(LoggerType.Function, String.Format("CameraCalibrationForm::constantCalibrate"));

            CalibrationResult calibrationResult = new CalibrationResult();
            bool ok = (scaleX > 0 && scaleY > 0);
            if (ok)
            {
                this.pelSize = new SizeF(scaleX, scaleY);
                calibrationResult.pelSize = this.pelSize;
            }
            return calibrationResult;
        }

        /// <summary>
        /// Grid or Chessboard
        /// </summary>
        /// <param name="image"></param>
        /// <param name="calibrationType">Grid or Chessboard ONLY</param>
        /// <param name="numRow"></param>
        /// <param name="numCol"></param>
        /// <param name="rowSpace"></param>
        /// <param name="colSpace"></param>
        /// <returns></returns>
        public CalibrationResult Calibrate(ImageD image, CalibrationGridType calibrationGridType, int numRow, int numCol, float rowSpace, float colSpace)
        {
            this.calibrationType = CalibrationType.Grid;
            CalibrationResult calibrationResult = null;
            switch (calibrationGridType)
            {
                case CalibrationGridType.Dots:
                    calibrationResult = CalibrateGrid(image, numRow, numCol, rowSpace, colSpace);
                    break;
                case CalibrationGridType.Chessboard:
                    calibrationResult = CalibrateChessboard(image, numRow, numCol, rowSpace, colSpace);
                    break;
            }
            return calibrationResult;
        }

        public abstract CalibrationResult CalibrateGrid(ImageD image, int numRow, int numCol, float rowSpace, float colSpace);
        public abstract CalibrationResult CalibrateChessboard(ImageD image, int numRow, int numCol, float rowSpace, float colSpace);

        /// <summary>
        /// Ruler
        /// </summary>
        /// <param name="image"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public CalibrationResult Calibrate(ImageD image, Rectangle calibRect, float rulerScale, int threshold)
        {
            this.calibrationType = CalibrationType.Ruler;
            LogHelper.Debug(LoggerType.Function, String.Format("CameraCalibrationForm::rulerCalibrate"));

            Image2D image2D = image as Image2D;
            Rectangle measureReagion = calibRect;

            AlgoImage fullImage = ImageBuilder.Build(Calibration.TypeName, image2D, ImageType.Grey);

            AlgoImage algoImage = fullImage.Clip(measureReagion);
            //algoImage.Save(@"D:\temp\algoImage.bmp");

            if (threshold < 0)
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                threshold = (int)Math.Round(ip.GetGreyAverage(algoImage));
            }

            AlgoImage maskImage = ImageBuilder.Build(Calibration.TypeName, ImageType.Grey, algoImage.Width, algoImage.Height);
            maskImage.Clear(255);

            CalibrationResult calibrationResult = GetResult(algoImage, maskImage, rulerScale, threshold);
            calibrationResult.calibRect = calibRect;

            //float partialRegionWidth = measureReagion.Width / 3.0f;
            int partialRegionSize = (int)Math.Round((double)(Math.Min(measureReagion.Width, measureReagion.Height) * 2));
            Size partialSize = new Size(partialRegionSize, partialRegionSize);

            Point[] patialPoint = new Point[3]
            {
                Point.Empty,
                new Point((measureReagion.Width - partialRegionSize) / 2, 0),
                new Point((measureReagion.Width - partialRegionSize), 0)
            };

            Array.ForEach(patialPoint, f =>
            {
                Rectangle partionReagion = new Rectangle(f, partialSize);
                partionReagion.Intersect(new Rectangle(Point.Empty, algoImage.Size));
                AlgoImage algoImage2 = algoImage.Clip(partionReagion);
                AlgoImage maskImage2 = maskImage.Clip(partionReagion);
                CalibrationResult subResult = GetResult(algoImage2, maskImage2, rulerScale, threshold);
                subResult.calibRect = partionReagion;
                calibrationResult.subCalibrationResult.Add(subResult);
                algoImage2.Dispose();
                maskImage2.Dispose();
            });

            maskImage.Dispose();
            algoImage.Dispose();
            fullImage.Dispose();

            return calibrationResult;
        }

        private CalibrationResult GetResult(AlgoImage algoImage, AlgoImage maskImage, float rulerScale, int threshold)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            AlgoImage binalImage = algoImage.Clone();
            if (threshold > 0)
                imageProcessing.Binarize(binalImage, BinarizationType.SingleThreshold, threshold, 0);
            else
                imageProcessing.Binarize(binalImage, BinarizationType.AdaptiveThreshold, 0, 0);
            //binalImage.Save(@"d:\temp\binalize.bmp");
            //imageProcessing.Open(binalImage, 3);

            imageProcessing.And(algoImage, maskImage, algoImage);
            //binalImage.Save(@"d:\temp\binalize2.bmp");

            StatResult statResult = imageProcessing.GetStatValue(algoImage, maskImage);

            CalibrationResult result = new CalibrationResult();
            result.clipImage = (Image2D)algoImage.ToImageD();
            result.binalImage = (Image2D)binalImage.ToImageD();
            result.avgBrightness = (float)statResult.average;
            result.minBrightness = (float)statResult.min;
            result.maxBrightness = (float)statResult.max;

            result.focusValue = GetFocusValue(algoImage, maskImage);

            result.projectionData = imageProcessing.Projection(algoImage, Direction.Horizontal, ProjectionType.Mean);

            result.cellDataList = GetCellData(binalImage, threshold);

            result.pelSize = GetResolution(result.cellDataList, rulerScale);

            return result;
        }

        private float GetFocusValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            Size rectSize = new Size(algoImage.Width - 1, algoImage.Height);
            System.Drawing.Rectangle rect1 = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), rectSize);
            System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(new System.Drawing.Point(1, 0), rectSize);
            AlgoImage image0 = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, rectSize.Width, rectSize.Height);
            AlgoImage image1 = algoImage.GetSubImage(rect1);
            AlgoImage image2 = algoImage.GetSubImage(rect2);
            imageProcessing.Subtract(image1, image2, image0, true);
            StatResult statResult = imageProcessing.GetStatValue(algoImage, maskImage);
            image0.Dispose();
            image1.Dispose();
            image2.Dispose();

            return (float)(statResult.stdDev);
            return (float)(statResult.squareSum / statResult.count);

            float[] projData = imageProcessing.Projection(image0, Direction.Vertical, ProjectionType.Mean);
            float fv = 0;
            foreach (float data in projData)
                fv += data;
            fv /= projData.Length;


            return fv;
        }

        private List<PointF> GetCellData(AlgoImage binalImage, int threshold)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(binalImage);

            List<PointF> cellList = new List<PointF>();
            BlobRectList blobRectList = null;
            AlgoImage blobImage = null;
            try
            {
                Rectangle subRect = Rectangle.Inflate(new Rectangle(Point.Empty, binalImage.Size), -1, -1);
                if (subRect.Width > 0 && subRect.Height > 0)
                {
                    blobImage = binalImage.GetSubImage(Rectangle.FromLTRB(0, 1, binalImage.Width, binalImage.Height - 1));
                    //blobImage.Save("d:\\temp\\AdBin2.bmp", null);
                    BlobParam blobParam = new BlobParam();
                    blobParam.AreaMin = 10;

                    blobRectList = imageProcessing.Blob(blobImage, blobParam);

                    List<BlobRect> blobRects = blobRectList.GetList();

                    // Sort and Filter
                    blobRects.Sort((f, g) => f.BoundingRect.Left.CompareTo(g.BoundingRect.Left));
                    for (int i = 1; i < blobRects.Count - 2; i++)
                    {
                        BlobRect src = blobRects[i];
                        BlobRect dst = blobRects[i + 1];

                        System.Drawing.Rectangle regionRect = System.Drawing.Rectangle.FromLTRB((int)Math.Round(src.CenterPt.X), 0, (int)Math.Round(dst.CenterPt.X), blobImage.Height);
                        if (regionRect.Width <= 0 || regionRect.Height <= 0)
                            continue;

                        PointF cell = new PointF(src.CenterPt.X, dst.CenterPt.X);
                        cellList.Add(cell);
                    }

                    cellList.RemoveAll(f => (f.Y - f.X <= 0));
                    if (cellList.Count > 0)
                    {
                        //float meanWidth = cellList.Average(f => (f.Y - f.X));
                        //float max = cellList.Max(f => f.Y - f.X);
                        //int maxIdx = cellList.FindIndex(f => (f.Y - f.X) == max);
                        //cellList.RemoveAt(maxIdx);

                        //float min = cellList.Min(f => f.Y - f.X);
                        //int minIdx = cellList.FindIndex(f => (f.Y - f.X) == min);
                        //cellList.RemoveAt(minIdx);

                        float meanWidth = cellList.Average(f => (f.Y - f.X));
                        cellList.RemoveAll(f =>
                        {
                            double width = (f.Y - f.X);
                            return width > (meanWidth * 1.3) || width < ((meanWidth * 0.8));
                        });
                    }
                }
            }
            finally
            {
                blobRectList?.Dispose();
                blobImage?.Dispose();
            }
            return cellList;
        }

        private SizeF GetResolution(List<PointF> cellData, float rulerScale)
        {
            float unitPerUm = rulerScale * 1000.0f;
            float widthSum = 0;
            int cellCount = 0;
            for (int i = 0; i < cellData.Count; i++)
            {
                PointF cell = cellData[i];
                float cellWidth = (float)(cell.Y - cell.X);
                widthSum += cellWidth;
                cellCount++;
            }

            // Average
            float averPxPerUnit = float.NaN;
            if (cellCount > 0)
                averPxPerUnit = widthSum / cellCount;
            float averResolution = 1 / (averPxPerUnit) * unitPerUm;

            // End 2 End
            float e2ePxPerUnit = float.NaN;
            if (cellCount > 0)
                e2ePxPerUnit = (float)((cellData[cellData.Count - 1].Y - cellData[0].X) / cellCount);
            float e2eResolution = 1 / (averPxPerUnit) * unitPerUm;

            //return new SizeF(averResolution, e2eResolution);
            return new SizeF(averResolution, averResolution);
        }

        public virtual void Initialize(int cameraIndex, string datFileName, string gridFileName)
        {
            this.DatFileName = datFileName;
            this.gridFileName = gridFileName;
            this.cameraIndex = cameraIndex;

            Load();
        }

        public virtual void Load()
        {
            if (File.Exists(datFileName) == true)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(datFileName);

                XmlElement calibrationElement = xmlDocument.DocumentElement;

                calibrationType = (CalibrationType)Enum.Parse(typeof(CalibrationType), XmlHelper.GetValue(calibrationElement, "CalibrationType", "SingleScale"));

                pelSize.Width = Convert.ToSingle(XmlHelper.GetValue(calibrationElement, "ScaleX", "1.0"));
                pelSize.Height = Convert.ToSingle(XmlHelper.GetValue(calibrationElement, "ScaleY", "1.0"));

                LoadGrid();

            }
            else if (File.Exists(gridFileName) == true)
            {
                LoadGrid();
            }
            else
            {
                fileLoadSuccessed = false;
                pelSize.Width = 10;
                pelSize.Height = 10;
            }
        }

        public void UpdatePelSize(int imageWidth, int imageHeight)
        {
            imageSize = new Size(imageWidth, imageHeight);

            if (IsCalibrated() == true)
            {
                Point centerPt = new Point(imageWidth / 2, imageHeight / 2);
                Rectangle checkRect = new Rectangle(centerPt.X - 1, centerPt.Y - 1, centerPt.X + 1, centerPt.Y + 1);

                PointF leftTop = PixelToWorld(new PointF(checkRect.Left, checkRect.Top));
                PointF rightBottom = PixelToWorld(new PointF(checkRect.Right, checkRect.Bottom));

                pelSize.Width = Math.Abs(rightBottom.X - leftTop.X) / checkRect.Width;
                pelSize.Height = Math.Abs(rightBottom.Y - leftTop.Y) / checkRect.Height;
            }
        }

        public SizeF GetFovSize()
        {
            return new SizeF(imageSize.Width * pelSize.Width, imageSize.Height * pelSize.Height);
        }

        public virtual void Save()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement calibrationElement = xmlDocument.CreateElement("", "Calibration", "");
            xmlDocument.AppendChild(calibrationElement);

            XmlHelper.SetValue(calibrationElement, "CalibrationType", calibrationType.ToString());

            XmlHelper.SetValue(calibrationElement, "ScaleX", pelSize.Width.ToString());
            XmlHelper.SetValue(calibrationElement, "ScaleY", pelSize.Height.ToString());

            SaveGrid();

            xmlDocument.Save(datFileName);
            fileLoadSuccessed = true;
        }

        public bool IsCalibrated()
        {
            return (pelSize.Width > 0 && pelSize.Height > 0);
        }

        public abstract bool IsGridCalibrated();

        public float WorldToPixel(float fWorld)
        {
            return WorldToPixel(new PointF(fWorld, 0)).X;
        }

        public virtual SizeF WorldToPixel(SizeF szWorld)
        {
            SizeF szPixel = SizeF.Empty;
            if (IsCalibrated() == true)
            {
                switch (calibrationType)
                {
                    case CalibrationType.SingleScale:
                    case CalibrationType.Ruler:
                        szPixel = new SizeF((float)(szWorld.Width / pelSize.Width), (float)(szWorld.Height / pelSize.Height));
                        break;

                    case CalibrationType.Grid:
                        szPixel = WorldToPixelGrid(szWorld);
                        break;
                }
            }

            return szPixel;
        }

        public virtual PointF WorldToPixel(PointF ptWorld)
        {
            PointF ptPixel = PointF.Empty;

            if (IsCalibrated() == true)
            {
                switch (calibrationType)
                {
                    case CalibrationType.SingleScale:
                    case CalibrationType.Ruler:
                        ptPixel = new PointF((float)(ptWorld.X / pelSize.Width), (float)(ptWorld.Y / pelSize.Height));
                        break;

                    case CalibrationType.Grid:
                        ptPixel = WorldToPixelGrid(ptWorld);
                        break;
                }
            }

            return ptPixel;
        }

        public RectangleF WorldToPixel(RectangleF rectWorld)
        {
            PointF ptPointLeftTop = WorldToPixel(new PointF(rectWorld.Left, rectWorld.Top));
            PointF ptPointRightBottom = WorldToPixel(new PointF(rectWorld.Right, rectWorld.Bottom));

            return DrawingHelper.FromPoints(ptPointLeftTop, ptPointRightBottom);
        }

        public float PixelToWorld(float pixel)
        {
            return PixelToWorld(new PointF(pixel, 0)).X;
        }

        public virtual PointF PixelToWorld(PointF ptPixel)
        {
            if (IsCalibrated() == true)
            {
                if (calibrationType == CalibrationType.SingleScale || calibrationType == CalibrationType.Ruler)
                {
                    float ptWorldX = ptPixel.X * pelSize.Width;
                    float ptWorldY = ptPixel.Y * pelSize.Height;

                    return new PointF(ptWorldX, ptWorldY);
                }
                else
                {
                    return PixelToWorldGrid(ptPixel);
                }
            }

            return ptPixel;
        }

        public virtual SizeF PixelToWorld(SizeF szPixel)
        {
            if (IsCalibrated() == true)
            {
                if (calibrationType == CalibrationType.SingleScale || calibrationType == CalibrationType.Ruler)
                {
                    float ptWorldX = szPixel.Width * pelSize.Width;
                    float ptWorldY = szPixel.Height * pelSize.Height;

                    return new SizeF(ptWorldX, ptWorldY);
                }
                else
                {
                    return PixelToWorldGrid(szPixel);
                }
            }

            return szPixel;
        }

        public virtual RectangleF PixelToWorld(RectangleF pixel)
        {
            if (IsCalibrated() == true)
            {
                if (calibrationType == CalibrationType.SingleScale || calibrationType == CalibrationType.Ruler)
                {
                    RectangleF world = new RectangleF()
                    {
                        X = pixel.X * pelSize.Width,
                        Y = pixel.Y * pelSize.Height,
                        Width = pixel.Width * pelSize.Width,
                        Height = pixel.Height * pelSize.Height,
                    };
                    return world;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return pixel;
        }

        public abstract void Dispose();

        public abstract void TransformImage(ImageD image);

        public abstract PointF WorldToPixelGrid(PointF ptWorld);
        public abstract SizeF WorldToPixelGrid(SizeF ptWorld);

        public abstract PointF PixelToWorldGrid(PointF ptPixel);
        public abstract SizeF PixelToWorldGrid(SizeF szPixel);

        public abstract void LoadGrid();
        public abstract void SaveGrid();

        public PointF GetFovCenterPos()
        {
            return PixelToWorld(new PointF(imageSize.Width / 2, imageSize.Height / 2));
        }
    }

    public class ScaledCalibration : Calibration
    {
        private ScaledCalibration() { this.CalibrationType = CalibrationType.SingleScale; }

        public ScaledCalibration(SizeF pelSize) : this() { this.pelSize = pelSize; }
        public ScaledCalibration(float pelSize) : this() { this.pelSize = new SizeF(pelSize, pelSize); }
        public ScaledCalibration(float pelSizeW, float pelSizeH) : this() { this.pelSize = new SizeF(pelSizeW, pelSizeH); }

        public override CalibrationResult CalibrateChessboard(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            throw new NotImplementedException();
        }

        public override CalibrationResult CalibrateGrid(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override bool IsGridCalibrated()
        {
            throw new NotImplementedException();
        }

        public override void LoadGrid()
        {
            throw new NotImplementedException();
        }

        public override PointF PixelToWorldGrid(PointF ptPixel)
        {
            throw new NotImplementedException();
        }

        public override SizeF PixelToWorldGrid(SizeF szPixel)
        {
            throw new NotImplementedException();
        }

        public override void SaveGrid()
        {
            throw new NotImplementedException();
        }

        public override void TransformImage(ImageD image)
        {
            throw new NotImplementedException();
        }

        public override PointF WorldToPixelGrid(PointF ptWorld)
        {
            throw new NotImplementedException();
        }

        public override SizeF WorldToPixelGrid(SizeF szWorld)
        {
            throw new NotImplementedException();
        }
    }
}
