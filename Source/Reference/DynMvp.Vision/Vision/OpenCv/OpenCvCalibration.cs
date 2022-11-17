using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DynMvp.UI;
using DynMvp.Base;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;
using Emgu.CV.Util;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvCalibration : Calibration
    {
        Matrix<double> worldToImageHomography = null;
        Matrix<double> imageToWorldHomography = null;

        Matrix<double> camera;
        IntrinsicCameraParameters icp = null;


        bool isGridCalibrated = false;

        public OpenCvCalibration()
        {
            try
            {
                this.worldToImageHomography = new Matrix<double>(0, 0);
                this.imageToWorldHomography = new Matrix<double>(0, 0);

                this.icp = new IntrinsicCameraParameters();
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                do
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(exception.GetType().ToString());
                    sb.AppendLine(exception.Message);
                    sb.AppendLine(exception.HelpLink);
                    DynMvp.UI.Touch.MessageForm.Show(null, sb.ToString());
                    exception = exception.InnerException;
                } while (exception != null);
                throw ex;
            }
        }

        public override void Dispose()
        {
            if (imageToWorldHomography != null)
            {
                imageToWorldHomography.Dispose();
            }
            if (worldToImageHomography != null)
            {
                worldToImageHomography.Dispose();
            }
            if (icp != null)
            {
                icp.IntrinsicMatrix.Dispose();
                icp.DistortionCoeffs.Dispose();
            }
        }

        public override bool IsGridCalibrated()
        {
            return isGridCalibrated;
        }

        public override CalibrationResult CalibrateGrid(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            //Dispose();
            isGridCalibrated = false;

            AlgoImage greyImage = ImageBuilder.Build(DynMvp.Vision.Calibration.TypeName, image, ImageType.Grey);

            Size patternCount = new Size(numCol, numRow);
            SizeF patternSize = new SizeF(colSpace, rowSpace);

            isGridCalibrated = CalibrateGrid(greyImage, patternCount, patternSize);

            UpdatePelSize(image.Width, image.Height);

            return new CalibrationResult() { pelSize = this.PelSize };
        }

        public override CalibrationResult CalibrateChessboard(ImageD image, int numRow, int numCol, float rowSpace, float colSpace)
        {
            //Dispose();
            isGridCalibrated = false;

            OpenCvGreyImage greyImage = (OpenCvGreyImage)ImageBuilder.OpenCvImageBuilder.Build(image, ImageType.Grey);

            Size patternCount = new Size(numCol, numRow);
            SizeF patternSize = new SizeF(colSpace, rowSpace);

            isGridCalibrated = CalibrateChess(greyImage, patternCount, patternSize);

            UpdatePelSize(image.Width, image.Height);

            return new CalibrationResult() { pelSize = this.PelSize };
        }

        public bool CalibrateChess(AlgoImage algoImage, Size patternCount, SizeF patternSize)
        {
            Image<Gray, byte> grayImage = ((OpenCvGreyImage)algoImage).Image;

            List<PointF> checkerboardPoint = FindChessboard(grayImage, patternCount, patternSize);
            if (checkerboardPoint == null)
            {
                return false;
            }

            {
                Image<Gray, byte> drawImage = grayImage.Clone();
                foreach (PointF point in checkerboardPoint)
                {
                    drawImage.Draw(new CircleF(point, 2), new Gray(127), 20);
                }

                DebugHelper.SaveImage(Image2D.FromBitmap(drawImage.ToBitmap()), @"DetectedPoints.bmp", new DebugContext(true, @"d:\"));
            }

            bool ok = Calibration(grayImage, checkerboardPoint, patternCount, patternSize);
            return ok;
        }

        private List<PointF> FindChessboard(Image<Gray, byte> grayImage, Size patternCount, SizeF patternSize)
        {
            Size patternCount0 = new Size(patternCount.Width - 1, patternCount.Height - 1);
            Matrix<float> corners = new Matrix<float>(0, 0);
            bool ok = CvInvoke.FindChessboardCorners(grayImage, patternCount0, corners);
            if (ok == false)
            {
                return null;
            }

            PointF[] pointCorners = new PointF[corners.Cols];
            for (int i = 0; i < corners.Cols; i++)
                pointCorners[i] = new PointF(corners[0, i], corners[1, i]);

            Size patternSize4 = new Size((int)(patternSize.Width / 2 - 0.5), (int)(patternSize.Height / 2 - 0.5));
            grayImage.FindCornerSubPix(new PointF[1][] { pointCorners }, patternSize4, new Size(-1, -1), new MCvTermCriteria(30, 0.1));
            List<PointF> listPointCorner = pointCorners.ToList();
            return listPointCorner;
        }

        private bool Calibration(Image<Gray, byte> image, List<PointF> point, Size patternCount, SizeF patternSize)
        {
            Size patternCount0 = new Size(patternCount.Width - 1, patternCount.Height - 1);

            MCvPoint3D32f[][] refPoints = new MCvPoint3D32f[1][];//[patternSize.Width * patternSize.Height];
            MCvPoint3D32f[] refPoints2 = new MCvPoint3D32f[patternCount0.Width * patternCount0.Height];

            int c = 0;
            for (int i = 0; i < patternCount0.Height; i++)
            {
                for (int j = 0; j < patternCount0.Width; j++)
                {
                    refPoints2[c] = (new MCvPoint3D32f(j * patternCount0.Width, i * patternCount0.Height, 0.0f));
                    c++;
                }
            }
            refPoints[0] = refPoints2;

            PointF[][] corners = new PointF[1][];
            corners[0] = point.ToArray();

            IntrinsicCameraParameters icp = new IntrinsicCameraParameters();
            ExtrinsicCameraParameters[] ecp = null;
            double resultValue = CameraCalibration.CalibrateCamera(refPoints, corners, image.Size, icp, CalibType.RationalModel, new MCvTermCriteria(30, 0.1), out ecp);
            this.icp = icp;

            return true;
        }

        bool CalibrateGrid(AlgoImage algoImage, Size patternCount, SizeF patternSize)
        {
            int numRow = patternCount.Height;
            int numCol = patternCount.Width;
            float rowSpace = patternSize.Height;
            float colSpace = patternSize.Width;

            CalibrationType = CalibrationType.Grid;

            float[,] imagePosition = Segmentation(algoImage, numRow, numCol, rowSpace, colSpace);

            if (imagePosition == null)
                return false;

            int width = algoImage.Width;
            int height = algoImage.Height;

            float[,] worldPosition = new float[numCol * numRow, 2];

            int stXPos = numCol / 2;
            int stYPos = numRow / 2;
            int centerPos = (numCol * stYPos) + stXPos;

            float stX = imagePosition[centerPos, 0];
            float stY = imagePosition[centerPos, 1];

            for (int rowIndex = 0; rowIndex < numRow; rowIndex++)
            {
                for (int colIndex = 0; colIndex < numCol; colIndex++)
                {
                    double xGap = Math.Abs(stX - imagePosition[rowIndex * numCol + colIndex, 0]);
                    double yGap = Math.Abs(stY - imagePosition[rowIndex * numCol + colIndex, 1]);

                    double imageR = Math.Sqrt(Math.Pow(xGap, 2) + Math.Pow(yGap, 2));

                    double xRatio = xGap / imageR;
                    double yRatio = yGap / imageR;

                    double worldR = Math.Sqrt(Math.Pow(Math.Abs(stXPos - colIndex), 2) + Math.Pow(Math.Abs(stYPos - rowIndex), 2));

                    if (rowIndex * numCol + colIndex == centerPos)
                        worldPosition[rowIndex * numCol + colIndex, 0] = 0;
                    else if (imagePosition[centerPos, 0] > imagePosition[rowIndex * numCol + colIndex, 0])
                        worldPosition[rowIndex * numCol + colIndex, 0] = -(float)((xRatio * worldR) * colSpace);
                    else
                        worldPosition[rowIndex * numCol + colIndex, 0] = (float)((xRatio * worldR) * colSpace);

                    if (rowIndex * numCol + colIndex == centerPos)
                        worldPosition[rowIndex * numCol + colIndex, 1] = 0;
                    else if (imagePosition[centerPos, 1] > imagePosition[rowIndex * numCol + colIndex, 1])
                        worldPosition[rowIndex * numCol + colIndex, 1] = -(float)((yRatio * worldR) * rowSpace);
                    else
                        worldPosition[rowIndex * numCol + colIndex, 1] = (float)((yRatio * worldR) * rowSpace);

                    //worldPosition[rowIndex * numCol + colIndex, 0] = colIndex * rowSpace;
                    //worldPosition[rowIndex * numCol + colIndex, 1] = rowIndex * colSpace;
                }
            }

            Matrix<float> imagePositionMat = new Matrix<float>(imagePosition);
            Matrix<float> worldPositionMat = new Matrix<float>(worldPosition);

            Mat imageToWorldMat = CvInvoke.FindHomography(imagePositionMat, worldPositionMat);
            Mat worldToImageMat = CvInvoke.FindHomography(worldPositionMat, imagePositionMat);

            imageToWorldHomography = new Matrix<double>(imageToWorldMat.Rows, imageToWorldMat.Cols);
            worldToImageHomography = new Matrix<double>(worldToImageMat.Rows, worldToImageMat.Cols);

            imageToWorldMat.CopyTo(imageToWorldHomography);
            worldToImageMat.CopyTo(worldToImageHomography);

            double doubleScaleX = 0;

            for (int rowIndex = 0; rowIndex < numRow; rowIndex++)
            {
                for (int colIndex = 0; colIndex < numCol - 1; colIndex++)
                {
                    doubleScaleX += (double)(worldPosition[rowIndex * numCol + colIndex + 1, 0] - worldPosition[rowIndex * numCol + colIndex, 0])
                                    / (double)(imagePosition[rowIndex * numCol + colIndex + 1, 0] - imagePosition[rowIndex * numCol + colIndex, 0]);
                }
            }

            doubleScaleX /= (((double)numCol - 1.0) * (double)numRow);

            double doubleScaleY = 0;

            for (int colIndex = 0; colIndex < numCol; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < numRow - 1; rowIndex++)
                {
                    doubleScaleY += (double)(worldPosition[(rowIndex + 1) * numCol + colIndex, 1] - worldPosition[rowIndex * numCol + colIndex, 1])
                                    / (double)(imagePosition[(rowIndex + 1) * numCol + colIndex, 1] - imagePosition[rowIndex * numCol + colIndex, 1]);
                }
            }

            doubleScaleY /= (((double)numRow - 1) * (double)numCol);
            PelSize = new SizeF((float)doubleScaleX, (float)doubleScaleY);

            imagePositionMat.Dispose();
            worldPositionMat.Dispose();

            return true;
        }

        public float[,] Segmentation(AlgoImage algoImage, int numRow, int numCol, float rowSpace, float colSpace)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage cloneImage = algoImage.Clone();

            if (algoImage.CheckFilterd(ImageFilterType.Binarization) == false)
                imageProcessing.Binarize(cloneImage, false);

            cloneImage.Save("source2.jpg", new DebugContext(true, "d:\\"));

            imageProcessing.Not(cloneImage, cloneImage);

            //cloneImage.Save("source2.jpg", new DebugContext(true, "d:\\"));

            BlobRectList allBlobRectList = imageProcessing.Blob(cloneImage, new BlobParam());
            BlobRectList filteredBlobRectList;
            /////////////////////
            /*
            List<BlobRect> alllist = allBlobRectList.GetList();
            DrawBlobOption option = new DrawBlobOption();
            foreach (BlobRect blobRect in alllist)
            {
                imageProcessing.DrawBlob(cloneImage, allBlobRectList, blobRect, option);
                cloneImage.Save("source2.jpg", new DebugContext(true, "d:\\"));
            }
            */
            //cloneImage.Save("source2.jpg", new DebugContext(true, "d:\\"));

            ////////////////////
            cloneImage.Dispose();

            int blobRectNum = allBlobRectList.GetList().Count;

            if (numRow * numCol == blobRectNum)
            {
                filteredBlobRectList = allBlobRectList;
            }
            else
            {
                List<BlobRect> sortedBlobRectList = allBlobRectList.GetList().OrderByDescending(x => x.Area).ToList();

                filteredBlobRectList = new BlobRectList();

                //double averageArea, sumArea = 0;

                //foreach (BlobRect blobRect in allBlobRectList)
                //    sumArea += blobRect.Area;

                //averageArea = sumArea / allBlobRectList.GetList().Count;

                foreach (BlobRect blobRect in sortedBlobRectList)
                {
                    filteredBlobRectList.Append(blobRect);
                    if (filteredBlobRectList.GetList().Count >= (numRow * numCol))
                        break;
                }
            }

            //algoImage.Save("SourceImage.jpg", new DebugContext(true, "D:\\"));

            List<BlobRect> blobRectList = filteredBlobRectList.GetList();
            blobRectNum = blobRectList.Count;

            if (numRow * numCol != blobRectNum)
                return null;

            List<PointF>[] rowPoints = new List<PointF>[numRow];

            for (int index = 0; index < numRow; index++)
            {
                rowPoints[index] = new List<PointF>();
            }

            blobRectList = blobRectList.OrderByDescending(x => x.CenterPt.Y).ToList();

            for (int rowIndex = 0; rowIndex < numRow; rowIndex++)
            {
                for (int colIndex = 0; colIndex < numCol; colIndex++)
                {
                    if (rowPoints[rowIndex] == null || blobRectList.Count == 0)
                        return null;

                    //rowPoints[rowIndex].Add(new PointF(blobRectList.First().BoundingRect.X + (blobRectList.First().BoundingRect.Width / 2), blobRectList.First().BoundingRect.Y + (blobRectList.First().BoundingRect.Height / 2)));
                    rowPoints[rowIndex].Add(blobRectList.First().CenterPt);
                    blobRectList.RemoveAt(0);
                }

                if (rowPoints[rowIndex].Count != numCol)
                    return null;
            }

            if (blobRectList.Count != 0)
                return null;


            float[,] points = new float[numRow * numCol, 2];

            int pointsIndex = 0;
            for (int rowIndex = numRow - 1; rowIndex >= 0; rowIndex--)
            {
                PointF[] array = rowPoints[rowIndex].OrderByDescending(x => x.X).ToArray();

                for (int colIndex = numCol - 1; colIndex >= 0; colIndex--)
                {
                    points[pointsIndex, 0] = array[colIndex].X;
                    points[pointsIndex, 1] = array[colIndex].Y;
                    pointsIndex++;
                }
            }

            return points;
        }

        public override void TransformImage(ImageD image)
        {
            //if (CalibrationType == CalibrationType.ChessBoard)
            {
                OpenCvGreyImage greyImage = (OpenCvGreyImage)ImageBuilder.OpenCvImageBuilder.Build(image, ImageType.Grey);

                Matrix<float> map_x, map_y;
                icp.InitUndistortMap(greyImage.Width, greyImage.Height, out map_x, out map_y);

                CvInvoke.Remap(greyImage.Image, greyImage.Image, map_x, map_y, Inter.Nearest, BorderType.Default);
            }
        }

        public override PointF PixelToWorld(PointF ptPixel)
        {
            float ptWorldX = ptPixel.X * PelSize.Width;
            float ptWorldY = ptPixel.Y * PelSize.Height;

            return new PointF((float)ptWorldX, (float)ptWorldY);
        }

        public override PointF WorldToPixelGrid(PointF ptWorld)
        {
            if (IsGridCalibrated() == true)
            {
                double ptPixelX = ptWorld.X;
                double ptPixelY = ptWorld.Y;

                ptPixelX = worldToImageHomography.Data[0, 0] * ptWorld.X + worldToImageHomography.Data[0, 1] * ptWorld.Y + worldToImageHomography.Data[0, 2];
                ptPixelY = worldToImageHomography.Data[1, 0] * ptWorld.X + worldToImageHomography.Data[1, 1] * ptWorld.Y + worldToImageHomography.Data[1, 2];

                return new PointF((float)ptPixelX, (float)ptPixelY);
            }

            return ptWorld;
        }

        public override SizeF WorldToPixelGrid(SizeF ptWorld)
        {
            return new SizeF(WorldToPixelGrid(new PointF(ptWorld.Width, ptWorld.Height)));
        }

        public override PointF PixelToWorldGrid(PointF ptPixel)
        {
            if (IsGridCalibrated() == true)
            {
                double ptWorldX = ptPixel.X;
                double ptWorldY = ptPixel.Y;

                ptWorldX = imageToWorldHomography.Data[0, 0] * ptPixel.X + imageToWorldHomography.Data[0, 1] * ptPixel.Y + imageToWorldHomography.Data[0, 2];
                ptWorldY = imageToWorldHomography.Data[1, 0] * ptPixel.X + imageToWorldHomography.Data[1, 1] * ptPixel.Y + imageToWorldHomography.Data[1, 2];

                return new PointF((float)ptWorldX, (float)ptWorldY);
            }

            return ptPixel;
        }

        public override SizeF PixelToWorldGrid(SizeF szPixel)
        {
            float ptWorldX = szPixel.Width * PelSize.Width;
            float ptWorldY = szPixel.Height * PelSize.Height;

            return new SizeF((float)ptWorldX, (float)ptWorldY);
        }


        public override void LoadGrid()
        {
            if (File.Exists(gridFileName) == false)
                return;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(gridFileName);

                XmlElement calibrationElement = xmlDocument.DocumentElement;

                // 기존버전 호환성 유지 필요
                worldToImageHomography = new Matrix<double>(3, 3);
                imageToWorldHomography = new Matrix<double>(3, 3);

                worldToImageHomography.Data[0, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_0x0", "1.0"));
                worldToImageHomography.Data[0, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_0x1", "1.0"));
                worldToImageHomography.Data[0, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_0x2", "1.0"));
                worldToImageHomography.Data[1, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_1x0", "1.0"));
                worldToImageHomography.Data[1, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_1x1", "1.0"));
                worldToImageHomography.Data[1, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_1x2", "1.0"));
                worldToImageHomography.Data[2, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_2x0", "1.0"));
                worldToImageHomography.Data[2, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_2x1", "1.0"));
                worldToImageHomography.Data[2, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "WolrdToImage_2x2", "1.0"));

                imageToWorldHomography.Data[0, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_0x0", "1.0"));
                imageToWorldHomography.Data[0, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_0x1", "1.0"));
                imageToWorldHomography.Data[0, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_0x2", "1.0"));
                imageToWorldHomography.Data[1, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_1x0", "1.0"));
                imageToWorldHomography.Data[1, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_1x1", "1.0"));
                imageToWorldHomography.Data[1, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_1x2", "1.0"));
                imageToWorldHomography.Data[2, 0] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_2x0", "1.0"));
                imageToWorldHomography.Data[2, 1] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_2x1", "1.0"));
                imageToWorldHomography.Data[2, 2] = Convert.ToDouble(XmlHelper.GetValue(calibrationElement, "ImageToWorld_2x2", "1.0"));

                foreach (XmlElement xmlElement in calibrationElement)
                {
                    if (xmlElement.Name == "IntrinsicMatrix")
                    {
                        icp.IntrinsicMatrix = LoadMatrix(xmlElement);
                    }
                    else if (xmlElement.Name == "DistortionCoeffs")
                    {
                        icp.DistortionCoeffs = LoadMatrix(xmlElement);
                    }
                    else if (xmlElement.Name == "Parameter")
                    {
                        int patternCountW = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "PatternCountX", "1"));
                        int patternCountH = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "PatternCountY", "1"));
                        //PatternCount = new Size(patternCountW, patternCountH);
                        float patternSizeW = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternSizeW", "1.0"));
                        float patternSizeH = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternSizeH", "1.0"));
                        //PatternSize = new SizeF(patternSizeW, patternSizeH);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public override void SaveGrid()
        {
            if (IsGridCalibrated())
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement calibrationElement = xmlDocument.CreateElement("", "Homography", "");
                xmlDocument.AppendChild(calibrationElement);

                // 기존버전 호환성 유지 필요
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_0x0", worldToImageHomography.Data[0, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_0x1", worldToImageHomography.Data[0, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_0x2", worldToImageHomography.Data[0, 2].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_1x0", worldToImageHomography.Data[1, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_1x1", worldToImageHomography.Data[1, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_1x2", worldToImageHomography.Data[1, 2].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_2x0", worldToImageHomography.Data[2, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_2x1", worldToImageHomography.Data[2, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "WolrdToImage_2x2", worldToImageHomography.Data[2, 2].ToString());

                XmlHelper.SetValue(calibrationElement, "ImageToWorld_0x0", imageToWorldHomography.Data[0, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_0x1", imageToWorldHomography.Data[0, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_0x2", imageToWorldHomography.Data[0, 2].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_1x0", imageToWorldHomography.Data[1, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_1x1", imageToWorldHomography.Data[1, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_1x2", imageToWorldHomography.Data[1, 2].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_2x0", imageToWorldHomography.Data[2, 0].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_2x1", imageToWorldHomography.Data[2, 1].ToString());
                XmlHelper.SetValue(calibrationElement, "ImageToWorld_2x2", imageToWorldHomography.Data[2, 2].ToString());

                XmlElement xmlElement = xmlDocument.CreateElement("", "Parameter", "");
                calibrationElement.AppendChild(xmlElement);
                //XmlHelper.SetValue(xmlElement, "PatternCountX", PatternCount.Width.ToString());
                //XmlHelper.SetValue(xmlElement, "PatternCountY", PatternCount.Height.ToString());
                //XmlHelper.SetValue(xmlElement, "PatternSizeW", PatternSize.Width.ToString());
                //XmlHelper.SetValue(xmlElement, "PatternSizeH", PatternSize.Height.ToString());

                SaveMatrix(xmlElement, icp.IntrinsicMatrix);

                // Save intrinsicMatrix
                xmlElement = xmlDocument.CreateElement("", "IntrinsicMatrix", "");
                calibrationElement.AppendChild(xmlElement);
                SaveMatrix(xmlElement, icp.IntrinsicMatrix);

                // Save DistortionCoeffs
                xmlElement = xmlDocument.CreateElement("", "DistortionCoeffs", "");
                calibrationElement.AppendChild(xmlElement);
                SaveMatrix(xmlElement, icp.DistortionCoeffs);

                xmlDocument.Save(gridFileName);
            }
        }

        void SaveMatrix(XmlElement xmlElement, Matrix<double> matrix)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (double data in matrix.Data)
            {
                stringBuilder.AppendFormat("{0},", data.ToString());
            }
            string strData = stringBuilder.ToString();
            strData.Trim(new char[] { ',' });
            XmlHelper.SetValue(xmlElement, "Rows", matrix.Rows.ToString());
            XmlHelper.SetValue(xmlElement, "Cols", matrix.Cols.ToString());
            XmlHelper.SetValue(xmlElement, "Data", strData);
        }

        Matrix<double> LoadMatrix(XmlElement xmlElement)
        {
            try
            {
                int rows = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Rows", "-1"));
                int cols = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Cols", "-1"));
                if (rows <= 0 || cols <= 0)
                {
                    return new Matrix<double>(0, 0);
                }

                Matrix<double> matrix = new Matrix<double>(rows, cols);

                string data = XmlHelper.GetValue(xmlElement, "Data", "");
                string[] elements = data.Split(',');
                int c = 0;
                foreach (string element in elements)
                {
                    if (element == "")
                    {
                        continue;
                    }
                    matrix[c / rows, c % rows] = Convert.ToDouble(element);
                }
                c++;
                return matrix;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
