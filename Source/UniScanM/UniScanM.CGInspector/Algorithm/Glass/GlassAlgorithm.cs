using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanM.CGInspector.Data;

namespace UniScanM.CGInspector.Algorithm.Glass
{
    class GlassAlgorithm : DynMvp.Vision.Algorithm
    {
        public static string TypeName => "CoverGlass";

        public GlassAlgorithm()
        {
            this.param = new GlassAlgorithmParam();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            return;
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
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            return new List<AlgorithmResultValue>();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            GlassAlgorithmParam param = this.param as GlassAlgorithmParam;
            DebugContext debugContext = algorithmInspectParam.DebugContext;
            ImageD imageD = algorithmInspectParam.ClipImage;

            GlassAlgorithmResult algorithmResult = new GlassAlgorithmResult();
            using (AlgoImage algoImage = ImageBuilder.Build(this.GetAlgorithmType(), imageD))
            {
                algoImage.Save(@"C:\temp\algoImage.bmp");
                Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
                using (AlgoImage binalImage = ImageBuilder.BuildSameTypeSize(algoImage))
                {
                    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

                    // 영역 나누기
                    float[] proj = ip.Projection(algoImage, Direction.Horizontal, ProjectionType.Mean);
                    int[] splitPoint = GetSplitPoint(proj, 3);

                    if (splitPoint.Length == 1)
                    {
                        Binalize(algoImage, binalImage, ip, param.BinalizeTh);                        
                    }
                    else
                    {
                        for (int i = 1; i < splitPoint.Length; i++)
                        {
                            int srcX = splitPoint[i - 1];
                            int dstX = splitPoint[i];
                            Rectangle rectangle = Rectangle.FromLTRB(srcX, 0, dstX, algoImage.Height);

                            AlgoImage algoSubImage = algoImage.GetSubImage(rectangle);
                            AlgoImage resultSubImage = binalImage.GetSubImage(rectangle);

                            Binalize(algoSubImage, resultSubImage, ip, param.BinalizeTh);
                            resultSubImage.Dispose();
                            algoSubImage.Dispose();
                        }
                    }
                    binalImage.Save(@"0.Binarize.bmp", debugContext);
                    ip.Close(binalImage, param.CloseNum);
                    binalImage.Save(@"1.Close.bmp", debugContext);

                    BlobParam blobParam = new BlobParam()
                    {
                        AreaMin = param.BlobArea.Min,
                        AreaMax = param.BlobArea.Max
                    };
                    List<DefectOnScreen> defectOnScreenList;
                    using (BlobRectList blobRectList = ip.Blob(binalImage, blobParam, algoImage))
                    {
                        BlobRect[] blobParams = blobRectList.GetList().ToArray();
                        ip.DrawBlob(binalImage, blobRectList, null, null);
                        binalImage.Save(@"2.DrawBlob.bmp", debugContext);

                        defectOnScreenList = blobRectList.GetList().ConvertAll(f =>
                        {
                            Rectangle rectangle = Rectangle.Round(f.BoundingRect);
                            rectangle.Inflate(30, 30);
                            rectangle.Intersect(imageRect);
                            AlgoImage clipAlgoImage = algoImage.Clip(rectangle);
                            AlgoImage clipResultImage = binalImage.Clip(rectangle);
                            DefectOnScreen defectOnScreen = new DefectOnScreen(rectangle, clipAlgoImage.ToBitmap(), clipResultImage.ToBitmap(), (int)f.Area, f.RotateXArray, f.RotateYArray);
                            clipResultImage.Dispose();
                            clipAlgoImage.Dispose();
                            return defectOnScreen;
                        });
                    }
                    defectOnScreenList.Sort((f, g) => g.Area.CompareTo(f.Area));

                    algorithmResult.AlgorithmParam = param;
                    algorithmResult.ResultImageD = binalImage.ToImageD();
                    algorithmResult.ClipRect = algorithmInspectParam.ClipRegionInFov;
                    algorithmResult.DefectOnScreens = defectOnScreenList.ToArray();
                    algorithmResult.AppendResultFigures(algorithmResult.ResultFigureGroup, PointF.Empty);
                }
            }

            return algorithmResult;
        }

        private void Binalize(AlgoImage algoImage, AlgoImage binalImage, ImageProcessing ip, MinMaxPair<sbyte> binalizeTh)
        {
            float grayAverage = ip.GetGreyAverage(algoImage);
            byte thMin = (byte)MathHelper.Clipping<double>(grayAverage + binalizeTh.Min, byte.MinValue, byte.MaxValue);
            byte thMax = (byte)MathHelper.Clipping<double>(grayAverage + binalizeTh.Max, byte.MinValue, byte.MaxValue);

            ip.Binarize(algoImage, binalImage, thMin, thMax, true);
        }

        private int[] GetSplitPoint(float[] proj, int diff)
        {
            float average = 0;
            float localMin = 0;
            float localMax = 0;
            int count = 0;
            List<int> splitPointList = new List<int>();
            splitPointList.Add(0);
            splitPointList.Add(proj.Length - 1);

            for (int x = 0; x < proj.Length; x++)
            {
                float value = proj[x];
                count++;
                if (count == 1)
                {
                    average = localMax = localMin = value;
                }
                else
                {
                    average = ((average * (count - 1)) + proj[x]) / count;
                    localMax = Math.Max(localMax, value);
                    localMin = Math.Max(localMin, value);

                    if (localMax - average > diff || average - localMin > diff)
                    {
                        splitPointList.Add(x);
                        count = 0;
                    }
                }
            }
            splitPointList.Sort();
            return splitPointList.ToArray();
        }
    }
}
