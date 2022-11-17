using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Detector
{

    public abstract class SpreadTracer
    {
        public static SpreadTracer Create()
        {
            return new SpreadTracerBlob();
        }

        public abstract void Inspect(AlgoImage subInspImage, AlgoImage subBinalImage, SizeF pelSize, DefectObj subResult, DebugContextG debugContext);
    }

    class SpreadTracerProj : SpreadTracer
    {
        public class _SpreadTracerParam
        {
            public enum EMethod { PRJ, BLB }

            public bool Use { get; set; } = true;
            public EMethod Method { get; set; } = EMethod.BLB;
            public int Step { get; set; } = 4;
            public int Diff { get; set; } = 2;

            public _SpreadTracerParam Clone()
            {
                return new _SpreadTracerParam()
                {
                    Use = this.Use,
                    Method = this.Method,
                    Step = this.Step,
                    Diff = this.Diff,
                };
            }
        }

        public override void Inspect(AlgoImage subInspImage, AlgoImage subBinalImage, SizeF pelSize, DefectObj subResult, DebugContextG debugContext)
        {
            _SpreadTracerParam spreadTracerParam = new _SpreadTracerParam();

            const int searchPxU = 16;
            const int searchPxL = 0;
            RectangleF searchRectF = subResult.Region;

            // 위로 16픽셀 늘림. 폭 5~6픽셀 고정.
            searchRectF.Y -= searchPxU;
            searchRectF.Height += (searchPxU + searchPxL);
            float inflateX = (searchRectF.Width - 5) / 2;
            searchRectF.Inflate(-inflateX, 0);
            searchRectF.Intersect(new Rectangle(Point.Empty, subInspImage.Size));
            Rectangle searchRect = Rectangle.Round(searchRectF);

            // 늘린 영역의 원본영상
            using (AlgoImage inflatedImage = subInspImage.GetSubImage(searchRect))
            {
                inflatedImage.Save("inflatedImage.bmp", debugContext);

                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(inflatedImage);
                float[] projection = ip.Projection(inflatedImage, Direction.Vertical, ProjectionType.Mean);

                int filterSize = Math.Min(spreadTracerParam.Step, searchRect.Height - 1);
                int dataLen = projection.Length - filterSize + 1;
                float[] mean = new float[dataLen];
                bool[] rising = new bool[dataLen];
                int firstTrue = -1, lastFalse = 0;
                for (int i = 0; i < dataLen; i++)
                {
                    mean[i] = projection.Skip(i).Take(filterSize).Average();
                    rising[i] = false;
                    if (i > 0)
                    {
                        rising[i] = (mean[i] - mean[i - 1]) > spreadTracerParam.Diff;
                        if (rising[i] && firstTrue < 0)
                            firstTrue = i;
                        if (!rising[i])
                            lastFalse = i;
                    }
                }

                float threshold = -1;
                int newHeigth = -1;
                if (lastFalse + 1 == firstTrue)
                {
                    float average = projection.Average();
                    float minimum = projection.Min();
                    threshold = (average * 3 + minimum) / 4;
                    int foundIdx = Array.FindLastIndex(projection, f => f < threshold) + 1;

                    newHeigth = searchRect.Height - foundIdx;
                    int offsetY = foundIdx - searchPxU;
                    if (offsetY <= 0)
                    {
                        RectangleF defectRect = subResult.Region;
                        defectRect.Offset(0, offsetY);
                        defectRect.Height = newHeigth;
                        subResult.Region = defectRect;
                        subResult.RealRegion = DrawingHelper.Mul(defectRect, pelSize);
                        subResult.ShapeType = DefectObj.EShapeType.Linear2;
                    }
                }

                if (AdditionalSettings.Instance().DebugSpreadTrace)
                {
                    SaveDebugData(inflatedImage, projection, dataLen,filterSize, mean, rising, firstTrue, lastFalse, threshold, newHeigth, debugContext);
                }
            }
        }

        private void SaveDebugData(AlgoImage inflatedImage, float[] projection, int length, int filterSize, float[] mean, bool[] rising, int firstTrue, int lastFalse, float threshold, int newHeigth, DebugContextG debugContext)
        {
            Dictionary<int, string> dics = new Dictionary<int, string>();
            for (int i = 0; i < length; i++)
            {
                string str = string.Format("{0},{1}", mean[i], rising[i]);
                if (i == lastFalse)
                    str += ",LastFalse";
                if (i == firstTrue)
                    str += ",FirstTrue";
                dics.Add(i, str);
            }

            int offset = filterSize / 2;
            StringBuilder sb = new StringBuilder();
            using (Dictionary<int, string>.Enumerator enumerator = dics.GetEnumerator())
            {
                bool existNext = enumerator.MoveNext();
                for (int i = 0; i < projection.Length; i++)
                {
                    string aLine;

                    if (i >= offset && existNext)
                    {
                        aLine = string.Format("{0},{1}", projection[i].ToString(), enumerator.Current.Value);
                        existNext = enumerator.MoveNext();
                    }
                    else
                    {
                        aLine = projection[i].ToString();
                    }

                    sb.AppendLine(aLine);
                }
            }
            sb.AppendLine();
            sb.AppendLine(string.Format("{0},Threshold", threshold));
            sb.AppendLine(string.Format("{0},NewHeigth", newHeigth));
            string ss = sb.ToString();

            Directory.CreateDirectory(debugContext.FullPath);
            File.WriteAllText(Path.Combine(debugContext.FullPath, string.Format("{0}.txt", debugContext.BlobId)), ss);
            inflatedImage.Save(Path.Combine(debugContext.FullPath, string.Format("{0}.bmp", debugContext.BlobId)));
        }
    }

    class SpreadTracerBlob : SpreadTracer
    {
        public override void Inspect(AlgoImage subInspImage, AlgoImage subBinalImage, SizeF pelSize, DefectObj subResult, DebugContextG debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subInspImage);
            Rectangle region = Rectangle.Round(subResult.Region);
            Rectangle inflated = Rectangle.Inflate(region, region.Width, region.Height);
            inflated.Intersect(new Rectangle(Point.Empty, subInspImage.Size));

            if (inflated.Width <= 0 || inflated.Height <= 0)
                return;

            AlgoImage inspImage = null;
            AlgoImage binalImage = null;
            AlgoImage tempImage = null;
            AlgoImage tempImage2 = null;

            debugContext.SaveDebugImage |= AdditionalSettings.Instance().DebugSpreadTrace;
            try
            {
                inspImage = subInspImage.Clip(inflated);
                binalImage = subBinalImage.Clip(inflated);
                tempImage = ImageBuilder.BuildSameTypeSize(inspImage);
                tempImage2 = ImageBuilder.BuildSameTypeSize(inspImage);

                inspImage.Save(@"inspImage.0.bmp", debugContext);
                binalImage.Save(@"binalImage.0.bmp", debugContext);

                PointF centerPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, inspImage.Size));
                StatResult statResult = ip.GetStatValue(inspImage, binalImage);

                // 어두운 부분
                //ip.Binarize(inspImage, inspImage, (int)statResult.min, (int)statResult.max);
                //ip.Binarize(inspImage, inspImage, true);
                ip.Binarize(inspImage, inspImage, (int)Math.Round((statResult.average + statResult.max) / 2), true);
                inspImage.Save(@"inspImage.1.bmp", debugContext);

                // 불량1
                using (BlobRectList blobRectList = ip.Blob(binalImage, new BlobParam() { SelectLabelValue = true, Connectivity4 = false }))
                {
                    BlobRect[] blobRects = blobRectList.GetArray();
                    BlobRect centerBlob = Array.Find(blobRects, f => f.BoundingRect.Contains(centerPt));

                    binalImage.Clear();
                    ip.DrawBlob(binalImage, blobRectList, new BlobRect[] { centerBlob }, new DrawBlobOption() { SelectBlob = true });
                    binalImage.Save(@"binalImage.1.bmp", debugContext);
                }

                // 불량2
                ip.And(inspImage, binalImage, tempImage);
                tempImage.Save(@"1.AND.bmp", debugContext);

                // 불량+붙은전극
                ip.ReconstructIncludeBlob(inspImage, tempImage2, tempImage, new ResconstructParam() { AllIncluded = true });
                tempImage2.Save(@"2.REC.bmp", debugContext);

                // (불량+붙은전극) - 불량
                ip.Subtract(tempImage2, tempImage, tempImage2);
                ip.Erode(tempImage2, 1);
                tempImage2.Save(@"3.SUB.bmp", debugContext);

                using (BlobRectList blobRectList = ip.Blob(tempImage2, new BlobParam() { AreaMin = 2, EraseBorderBlobs = false }))
                {
                    BlobRect[] blobRects = blobRectList.GetArray();

                    switch (blobRects.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            subResult.ShapeType = DefectObj.EShapeType.Linear2;
                            break;
                        default:
                            subResult.ShapeType = DefectObj.EShapeType.Linear2;
                            break;
                    }
                    inspImage.Save(string.Format(@"4.{0}.bmp", subResult.ShapeType.ToString()), debugContext);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("SpreadTracerV2::Inspect - {0}", ex.Message));
            }
            finally
            {
                inspImage?.Dispose();
                binalImage?.Dispose();
                tempImage?.Dispose();
                tempImage2?.Dispose();
            }
        }
    }
}
