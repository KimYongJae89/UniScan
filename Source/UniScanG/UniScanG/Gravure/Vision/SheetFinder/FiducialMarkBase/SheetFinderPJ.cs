using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase
{
    public class SheetFinderPJ : SheetFinderBase
    {
        public SheetFinderPJ() : base()
        {
            this.AlgorithmName = TypeName;
            this.param = null;
        }

        #region Abstract
        public override AlgorithmParam CreateParam()
        {
            return new SheetFinderPJParam();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetFinderPJ clone = new SheetFinderPJ();
            clone.CopyFrom(this);

            return clone;
        }
        #endregion

        #region override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new SheetFinderResult(-1, -1, -1);
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SheetFinderPJParam fiducialFinderPJParam = (SheetFinderPJParam)this.param;

            Size fidSize = fiducialFinderPJParam.FidSize;
            SizeF FidSearchBound = new SizeF(fiducialFinderPJParam.FidSearchLBound, fiducialFinderPJParam.FidSearchRBound);

            AlgoImage algoImage = null;
            bool disposeNeed;
            if (algorithmInspectParam is SheetInspectParam)
            {
                algoImage = ((SheetInspectParam)algorithmInspectParam).AlgoImage;
                disposeNeed = false;
            }
            else
            {
                algoImage = ImageBuilder.Build(this.GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey);
                disposeNeed = true;
            }
            DebugContext debugContext = algorithmInspectParam.DebugContext;

            SheetFinderResult algorithmResult = FindFiducial(algoImage, fidSize, FidSearchBound, debugContext);

            if (disposeNeed)
                algoImage.Dispose();

            sw.Stop();
            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan();

            //if ((SamsungElectroSettings.Instance().SaveFiducialDebugData & SaveDebugData.Text) > 0)
            //    (MpisInspectorSystemManager.Instance().MainForm as Operation.UI.MainForm).WriteTimeLog("FiducialFinder", baseImage.Height, sw.ElapsedMilliseconds);

            return algorithmResult;
        }
               
        public SheetFinderResult FindFiducial(AlgoImage algoImage, Size fidSize, SizeF fidSearchBound, DebugContext debugContext)
        {
            List<Rectangle> fidRectList = FindFiducial2(algoImage, fidSize, fidSearchBound, debugContext);
            SheetFinderResult sheetFinderPJResult = (SheetFinderResult)CreateAlgorithmResult(); 
            sheetFinderPJResult.Good = false;

            if (fidRectList != null)
            {
                Rectangle algoImgaeRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);
                for (int i = 0; i < fidRectList.Count; i++)
                {
                    fidRectList[i] = Rectangle.Intersect(algoImgaeRect, fidRectList[i]);
                }

                fidRectList.RemoveAll(f => f.Width == 0 || f.Height == 0);

                sheetFinderPJResult.Good = (fidRectList.Count > 0);

                fidRectList.ForEach(f => sheetFinderPJResult.SheetFinderResultRectList.Add(new SheetFinderResultRect(f)));
            }
            return sheetFinderPJResult;
        }

        private List<Rectangle> FindFiducial2(AlgoImage algoImage, Size fidSize, SizeF fidSearchBound, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            bool disposeNeed = false;
            algoImage.Save("FindFiducial_Image.bmp", debugContext);

            int boundL = (int)Math.Round(algoImage.Width * fidSearchBound.Width);
            int boundR = (int)Math.Round(algoImage.Width * fidSearchBound.Height);
            int boundT = (int)0;
            int boundB = algoImage.Height;
            Rectangle clipRect = Rectangle.FromLTRB(boundL, boundT, boundR, boundB);
            if (clipRect.Width == 0 || clipRect.Height == 0 || Rectangle.Intersect(new Rectangle(0, 0, algoImage.Width, algoImage.Height), clipRect) != clipRect)
                return null;

            List<Rectangle> fidRectList = null;
            AlgoImage clipImage = null;

            try
            {
                clipImage = algoImage.Clip(clipRect);
                clipImage.Save("FindFiducial_SearchRegion.bmp", debugContext);

                fidRectList = FindFiducial3(clipImage, fidSize, debugContext);
                for (int i = 0; i < fidRectList.Count; i++)
                {
                    Rectangle fidRect = fidRectList[i];
                    fidRect.Offset(boundL, boundT);
                    fidRectList[i] = fidRect;
                };
            }
            finally
            {
                clipImage.Dispose();
                if (disposeNeed)
                    algoImage.Dispose();
            }

            return fidRectList;
        }

        private List<Rectangle> FindFiducial3(AlgoImage algoImage, Size fidSize, DebugContext debugContext)
        {
            List<Rectangle> fidRect = new List<Rectangle>();

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            StatResult statResult = imageProcessing.GetStatValue(algoImage, null);
            int threshold = (int)Math.Round((statResult.min + statResult.average) / 3);
            AlgoImage binImage = algoImage.Clone();
            imageProcessing.Binarize(binImage, threshold, true);
            binImage.Save("FindFiducial_SearchRegion_Binalize.bmp", debugContext);

            List<Point> fidPosYList = FindFiducialYPos(binImage, fidSize.Height);
            foreach (Point fidPosY in fidPosYList)
            {
                Rectangle clipRect = Rectangle.FromLTRB(0, fidPosY.X, algoImage.Width, fidPosY.Y);
                clipRect.Intersect(new Rectangle(0, 0, algoImage.Width, algoImage.Height));

                Point fidPosX = Point.Empty;
                AlgoImage subImage = binImage.GetSubImage(clipRect);
                fidPosX = FindFiducialX(subImage, fidSize.Width, debugContext);
                subImage.Dispose();

                if (fidPosX.IsEmpty == false)
                    fidRect.Add(Rectangle.FromLTRB(fidPosX.X, fidPosY.X, fidPosX.Y, fidPosY.Y));
            }
            binImage.Dispose();

            return fidRect;
        }

        private List<Point> FindFiducialYPos(AlgoImage algoImage, int height)
        {
            float minLength = height * 0.8f;
            float maxLength = height * 1.2f;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float[] projVData = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Sum);
            List<Point> posList = FindFiducialPos(projVData);
            posList.RemoveAll(f =>
            {
                if (height <= 0)
                    return false;
                int len = (f.Y - f.X);
                if (len > maxLength || len < minLength)
                    return true;
                return false;
            });

            //for (int i = 0; i < posList.Count; i++)
            //{
            //    Point pos = posList[i];
            //    pos.Offset(-fiducialMargine, fiducialMargine);
            //    posList[i] = pos;
            //}

            return posList;
        }

        private List<Point> FindFiducialPos(float[] data)
        {
            List<Point> posList = new List<Point>();

            float max = data.Max();
            float avg = data.Average();
            float threshold = (max + avg) / 2;
            int src = -1;
            bool onWhite = false;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= threshold)
                // 흰색이다.
                {
                    // 이전에 검은색이였으면 시작점 등록
                    if (onWhite == false)
                    {
                        src = i;
                        onWhite = true;
                    }
                }
                else
                // 검은색이다
                {
                    if (onWhite == true)
                    // 이전에 흰색이였으면 등록
                    {
                        posList.Add(new Point(src, i));
                        src = -1;
                        onWhite = false;
                    }
                }
            }

            if (onWhite)
            // 탐색이 끝났는데 흰색인 경우
            {
                posList.Add(new Point(src, data.Length - 1));
                src = -1;
                onWhite = false;
            }

            return posList;
        }

        private Point FindFiducialX(AlgoImage algoImage, int width, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            List<Rectangle> fidRectList = new List<Rectangle>();

            algoImage.Save("FindFiducial_FindFiducialX.bmp", debugContext);

            Point foundXPos = FindFiducialXPos(algoImage, width);
            return foundXPos;
        }

        private Point FindFiducialXPos(AlgoImage algoImage, int length)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float[] data = imageProcessing.Projection(algoImage, Direction.Horizontal, ProjectionType.Sum);
            List<Point> foundXPosList = FindFiducialPos(data);
            List<Point> foundXPosList2 = new List<Point>();
            foundXPosList.RemoveAll(f => f.Y - f.X <= 2);
            int min = (int)Math.Round(length * 0.8f);
            int max = (int)Math.Round(length * 1.2f);

            float threshold = 0.9f;
            for (int i = 0; i < foundXPosList.Count; i++)
            {
                int src = foundXPosList[i].X;
                int wLen = 0;
                for (int j = i; j < foundXPosList.Count; j++)
                {
                    int dst = foundXPosList[j].Y;
                    int len = dst - src;
                    if (length > 0 && (max < len || min > len))
                        continue;

                    wLen += foundXPosList[j].Y - foundXPosList[j].X;
                    float dataMean = wLen * 1.0f / len;
                    //for (int k = src; k < dst; k++)
                    //    dataMean += data[k];

                    //dataMean /= (255 * len* algoImage.Height);

                    if (dataMean > threshold)
                        foundXPosList2.Add(new Point(src, dst));
                }
            }
            if (foundXPosList2.Count == 0)
                return Point.Empty;

            foundXPosList2.Sort((f, g) => (g.Y - g.X).CompareTo(f.Y - f.X));
            return foundXPosList2[0];
        }
    }
}
