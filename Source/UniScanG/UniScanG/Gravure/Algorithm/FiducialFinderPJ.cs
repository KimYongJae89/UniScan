using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using DynMvp.Vision;

namespace UniScanG.Gravure.Algorithm
{
    public class FiducialFinderPJParam : DynMvp.Vision.AlgorithmParam
    {
        private Size targetFidSize = Size.Empty;
        private Rectangle searchFidRect = Rectangle.Empty;

        public Size TargetFidSize
        {
            get { return targetFidSize; }
            set { targetFidSize = value; }
        }

        public Rectangle SearchFidRect
        {
            get { return searchFidRect; }
            set { searchFidRect = value; }
        }

        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class FiducialFindePJrResult : DynMvp.Vision.AlgorithmResult
    {

    }

    public class FiducialFinderPJ : DynMvp.Vision.Algorithm
    {
        public static string TypeName
        {
            get { return "FiducialFinder"; }
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            throw new NotImplementedException();
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Gravure" + TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //if (pattern != null)
            //    pattern.Dispose();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            throw new NotImplementedException();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //FiducialFinderParam fiducialFinderParam = (FiducialFinderParam)this.param;

            //Size fidSize = fiducialFinderParam.TargetFidSize;
            //Rectangle searchFidRect = fiducialFinderParam.SearchFidRect;

            //AlgoImage baseImage = fiducialParam.FrameImage;

            //DebugContext debugContext = algorithmInspectParam.DebugContext;

            //AlgorithmResult algorithmResult = FindFiducial(baseImage, updatePatternImage, fidSize, FidSearchBound, debugContext);
            //sw.Stop();
            //algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan();

            //if ((SamsungElectroSettings.Instance().SaveFiducialDebugData & SaveDebugData.Text) > 0)
            //    (MpisInspectorSystemManager.Instance().MainForm as Operation.UI.MainForm).WriteTimeLog("FiducialFinder", baseImage.Height, sw.ElapsedMilliseconds);

            //return algorithmResult;
        }


        //public AlgorithmResult FindFiducial(AlgoImage algoImage, bool updatePatternImage, Size fidSize, SizeF fidSearchBound, DebugContext debugContext)
        //{
        //    List<Rectangle> fidRectList = FindFiducial2(algoImage, fidSize, fidSearchBound, debugContext);
        //    AlgorithmResult algoResult = CreateAlgorithmResult();
        //    algoResult.Good = false;

        //    if (fidRectList != null)
        //    {
        //        Rectangle algoImgaeRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);
        //        for (int i = 0; i < fidRectList.Count; i++)
        //        {
        //            fidRectList[i] = Rectangle.Intersect(algoImgaeRect, fidRectList[i]);
        //        }

        //        fidRectList.RemoveAll(f => f.Width == 0 || f.Height == 0);

        //        algoResult.Good = (fidRectList.Count > 0);

        //        if (algoResult.Good && updatePatternImage)
        //        {
        //            SheetCheckerParam param = this.param as SheetCheckerParam;
        //            FiducialFinderParam fiducialFinderParam = param.FiducialFinderParam;

        //            Rectangle fidRect = fidRectList[0];
        //            AlgoImage clipImage = algoImage.Clip(fidRect);
        //            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);
        //            imageProcessing.Average(clipImage);
        //            imageProcessing.Sobel(clipImage, fiducialSobelSize);
        //            fiducialFinderParam.FiducialPatternEdgeImage = (Image2D)clipImage.ToImageD();
        //            clipImage.Save("NewPattern.bmp", debugContext);
        //            clipImage.Dispose();
        //        }

        //        fidRectList.ForEach(fidRect =>
        //        {
        //            string name = (fidRect.Height > 0 ? "FidRect" : "IncompleteFidRect");
        //            algoResult.ResultValueList.Add(new AlgorithmResultValue(name, fidRect));
        //        });
        //    }
        //    return algoResult;
        //}


        //private List<Rectangle> FindFiducial2(AlgoImage algoImage, Size fidSize, SizeF fidSearchBound, DebugContext debugContext)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    bool disposeNeed = false;
        //    algoImage.Save("FindFiducial_Image.bmp", debugContext);

        //    int boundL = (int)Math.Round(algoImage.Width * fidSearchBound.Width);
        //    int boundR = (int)Math.Round(algoImage.Width * fidSearchBound.Height);
        //    int boundT = (int)0;
        //    int boundB = algoImage.Height;
        //    Rectangle clipRect = Rectangle.FromLTRB(boundL, boundT, boundR, boundB);
        //    if (clipRect.Width == 0 || clipRect.Height == 0 || Rectangle.Intersect(new Rectangle(0, 0, algoImage.Width, algoImage.Height), clipRect) != clipRect)
        //        return null;

        //    List<Rectangle> fidRectList = null;
        //    AlgoImage clipImage = null;

        //    try
        //    {
        //        clipImage = algoImage.Clip(clipRect);
        //        clipImage.Save("FindFiducial_SearchRegion.bmp", debugContext);

        //        fidRectList = FindFiducial3(clipImage, fidSize, debugContext);
        //        for (int i = 0; i < fidRectList.Count; i++)
        //        {
        //            Rectangle fidRect = fidRectList[i];
        //            fidRect.Offset(boundL, boundT);
        //            fidRectList[i] = fidRect;
        //        };
        //    }
        //    finally
        //    {
        //        clipImage.Dispose();
        //        if (disposeNeed)
        //            algoImage.Dispose();
        //    }

        //    return fidRectList;
        //}

        //private List<Rectangle> FindFiducial3(AlgoImage algoImage, Size fidSize, DebugContext debugContext)
        //{
        //    List<Rectangle> fidRect = new List<Rectangle>();
        //    SheetCheckerParam param = this.param as SheetCheckerParam;
        //    FiducialFinderParam fiducialFinderParam = param.FiducialFinderParam;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    StatResult statResult = imageProcessing.GetStatValue(algoImage, null);
        //    int threshold = (int)Math.Round((statResult.min + statResult.average) / 3);
        //    AlgoImage binImage = algoImage.Clone();
        //    imageProcessing.Binarize(binImage, threshold, true);
        //    binImage.Save("FindFiducial_SearchRegion_Binalize.bmp", debugContext);

        //    List<Point> fidPosYList = FindFiducialVPos(binImage, fidSize.Height);
        //    foreach (Point fidPosY in fidPosYList)
        //    {
        //        fidPosY.Offset(-fiducialMargine, fiducialMargine);
        //        Rectangle clipRect = Rectangle.FromLTRB(0, fidPosY.X, algoImage.Width, fidPosY.Y);
        //        clipRect.Intersect(new Rectangle(0, 0, algoImage.Width, algoImage.Height));

        //        Point fidPosX = Point.Empty;
        //        AlgoImage subImage;

        //        if (param.AdaptiveFidSearchRange == true)
        //        //if (this.pattern != null)
        //        // Find Fiducial with Pattern Matching
        //        {
        //            subImage = algoImage.GetSubImage(clipRect);
        //            fidPosX = FindFiducialXPM(subImage, debugContext);
        //        }
        //        else
        //        // Find Fiducial with Projection
        //        {
        //            subImage = binImage.GetSubImage(clipRect);
        //            fidPosX = FindFiducialXPJ(subImage, fidSize.Width, debugContext);
        //        }
        //        subImage.Dispose();

        //        if (fidPosX.IsEmpty == false)
        //            fidRect.Add(Rectangle.FromLTRB(fidPosX.X, fidPosY.X, fidPosX.Y, fidPosY.Y));
        //    }
        //    binImage.Dispose();

        //    return fidRect;
        //}


        //private Point FindFiducialXPM(AlgoImage algoImage, DebugContext debugContext)
        //{
        //    Point fidPosX = Point.Empty;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    imageProcessing.Average(algoImage);
        //    imageProcessing.Sobel(algoImage, fiducialSobelSize);
        //    //imageProcessing.Binarize(algoImage, fiducialBin);
        //    algoImage.Save("FindFiducial_SearchRegion_SobelBin.bmp", debugContext);

        //    PatternResult patternResult = pattern.Inspect(algoImage, patternMatchingParam, debugContext);
        //    if (patternResult.Good)
        //    {
        //        MatchPos foundFidPos = patternResult.MaxMatchPos;
        //        fidPosX = new Point(foundFidPos.Rect.Left, foundFidPos.Rect.Right);
        //    }
        //    //pattern.Dispose();

        //    return fidPosX;
        //}

        //private Point FindFiducialXPJ(AlgoImage algoImage, int width, DebugContext debugContext)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    List<Rectangle> fidRectList = new List<Rectangle>();

        //    algoImage.Save("FindFiducial_FindFiducialXPJ.bmp", debugContext);

        //    Point foundXPos = FindFiducialXPos(algoImage, width);

        //    if (foundXPos.IsEmpty == false)
        //    {
        //        foundXPos.Offset(-fiducialMargine, fiducialMargine);
        //    }
        //    return foundXPos;
        //}

        //internal void Train(Image2D patternImageD)
        //{
        //    LogHelper.Debug(LoggerType.Grab, "FiducialFinder::Train");

        //    if (pattern != null)
        //        pattern.Dispose();

        //    pattern = AlgorithmBuilder.CreatePattern();
        //    pattern.Train(patternImageD, patternMatchingParam);
        //}

        //private List<Point> FindFiducialVPos(AlgoImage algoImage, int height)
        //{
        //    float minLength = height * 0.8f;
        //    float maxLength = height * 1.2f;
        //    Point validPos = new Point(fiducialMargine, algoImage.Height - fiducialMargine);

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    float[] projVData = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Sum);
        //    List<Point> posList = FindFiducialPos(projVData);
        //    posList.RemoveAll(f =>
        //    {
        //        if (f.X < validPos.X || f.Y > validPos.Y)
        //            return true;

        //        int len = (f.Y - f.X);
        //        if (len > maxLength || len < minLength)
        //            return true;
        //        return false;
        //    });

        //    //for (int i = 0; i < posList.Count; i++)
        //    //{
        //    //    Point pos = posList[i];
        //    //    pos.Offset(-fiducialMargine, fiducialMargine);
        //    //    posList[i] = pos;
        //    //}

        //    return posList;
        //}

        //private Point FindFiducialXPos(AlgoImage algoImage, int length)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    float[] data = imageProcessing.Projection(algoImage, Direction.Horizontal, ProjectionType.Sum);
        //    List<Point> foundXPosList = FindFiducialPos(data);
        //    List<Point> foundXPosList2 = new List<Point>();
        //    foundXPosList.RemoveAll(f => f.Y - f.X <= 2);
        //    int min = (int)Math.Round(length * 0.8f);
        //    int max = (int)Math.Round(length * 1.2f);

        //    float threshold = 0.9f;
        //    for (int i = 0; i < foundXPosList.Count; i++)
        //    {
        //        int src = foundXPosList[i].X;
        //        int wLen = 0;
        //        for (int j = i; j < foundXPosList.Count; j++)
        //        {
        //            int dst = foundXPosList[j].Y;
        //            int len = dst - src;
        //            if (max < len || min > len)
        //                continue;

        //            wLen += foundXPosList[j].Y - foundXPosList[j].X;
        //            float dataMean = wLen * 1.0f / len;
        //            //for (int k = src; k < dst; k++)
        //            //    dataMean += data[k];

        //            //dataMean /= (255 * len* algoImage.Height);

        //            if (dataMean > threshold)
        //                foundXPosList2.Add(new Point(src, dst));
        //        }
        //    }
        //    if (foundXPosList2.Count == 0)
        //        return Point.Empty;

        //    foundXPosList2.Sort((f, g) => (g.Y - g.X).CompareTo(f.Y - f.X));
        //    return foundXPosList2[0];
        //}

        //private List<Point> FindFiducialPos(float[] data)
        //{
        //    List<Point> posList = new List<Point>();

        //    float max = data.Max();
        //    float avg = data.Average();
        //    float threshold = (max + avg) / 2;
        //    int src = -1;
        //    bool onWhite = false;

        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        if (data[i] >= threshold)
        //        // 흰색이다.
        //        {
        //            // 이전에 검은색이였으면 시작점 등록
        //            if (onWhite == false)
        //            {
        //                src = i;
        //                onWhite = true;
        //            }
        //        }
        //        else
        //        // 검은색이다
        //        {
        //            if (onWhite == true)
        //            // 이전에 흰색이였으면 등록
        //            {
        //                posList.Add(new Point(src, i));
        //                src = -1;
        //                onWhite = false;
        //            }
        //        }
        //    }

        //    if (onWhite)
        //    // 탐색이 끝났는데 흰색인 경우
        //    {
        //        posList.Add(new Point(src, data.Length - 1));
        //        src = -1;
        //        onWhite = false;
        //    }

        //    return posList;
        //}

    }
}
