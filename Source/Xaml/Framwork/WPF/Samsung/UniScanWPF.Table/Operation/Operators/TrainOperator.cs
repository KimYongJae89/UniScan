using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class TeachOperator : Operator
    {
        TeachOperatorSettings settings;

        public TeachOperatorSettings Settings { get => settings; }

        public TeachOperator()
        {
            settings = new TeachOperatorSettings();
        }

        public void Train(List<ExtractOperatorResult> extractOperatorResultList)
        {
            lock (this)
            {
                OperatorState = OperatorState.Run;
                PatternGroup sumPatternGroup = new PatternGroup();

                foreach (ExtractOperatorResult extractOperatorResult in extractOperatorResultList)
                {
                    if (extractOperatorResult.BlobRectList != null)
                        sumPatternGroup.AddPattern(extractOperatorResult.BlobRectList);
                }

                //using (FileStream fs = new FileStream(@"C:\temp\DevideSubGroup.txt", FileMode.Create))
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendLine("Area, RotateWidth, RotateHeight,RotateAngle");
                //    sumPatternGroup.PatternList.ForEach(f =>
                //    {
                //        sb.AppendLine(string.Format("{0},{1},{2},{3}", f.Area, f.RotateWidth, f.RotateHeight, f.RotateAngle));

                //    });

                //    byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
                //    fs.Write(bytes, 0, bytes.Length);
                //}

                List<PatternGroup> patternGroupList = sumPatternGroup.DivideSubGroup(settings.DiffGroupThreshold / DeveloperSettings.Instance.Resolution);
                patternGroupList = patternGroupList.OrderByDescending(patternGroup => patternGroup.Count).ToList();

                PatternGroup refPatternGroup = new PatternGroup();
                if (patternGroupList.Count > 0)
                {
                    float maxArea = patternGroupList.Max(patternGroup => patternGroup.SumArea);
                    refPatternGroup = patternGroupList.Find(patternGroup => patternGroup.SumArea == maxArea);

                    float refAngle = refPatternGroup.PatternList.Average(blob => blob.RotateAngle);

                    int inflateLength = 50;

                    foreach (PatternGroup patternGroup in patternGroupList)
                    {
                        BlobRect blobRect = patternGroup.GetAverageBlobRect();
                        AlgoImage patternImage = null;

                        ExtractOperatorResult extractOperatorResult = extractOperatorResultList.Find(f => f.BlobRectList.Contains(blobRect));
                        if (extractOperatorResult != null)
                        {
                            Rectangle sourceRect = new Rectangle(0, 0, extractOperatorResult.ScanOperatorResult.TopLightImage.Width, extractOperatorResult.ScanOperatorResult.TopLightImage.Height);
                            Rectangle rect = Rectangle.Truncate(blobRect.BoundingRect);
                            rect = DrawingHelper.Mul(rect, 1 / extractOperatorResult.BlobScaleFactor);
                            rect.Inflate(inflateLength, inflateLength);
                            rect.Intersect(sourceRect);
                            System.Diagnostics.Debug.Assert(rect.Width > 0 && rect.Height > 0);
                            patternImage = extractOperatorResult.ScanOperatorResult.TopLightImage.GetSubImage(rect);
                        }

                        System.Diagnostics.Debug.Assert(patternImage != null);
                        if (patternImage != null)
                        {
                            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(patternImage);
                            //int width = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateWidth + inflateLength) : (int)Math.Ceiling(blobRect.RotateHeight + inflateLength);
                            //int height = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateHeight + inflateLength) : (int)Math.Ceiling(blobRect.RotateWidth + inflateLength);
                            //Size size = DrawingHelper.Mul(new Size(width, height), 1 / resizeRatio);
                            int width = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateWidth) : (int)Math.Ceiling(blobRect.RotateHeight);
                            int height = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateHeight) : (int)Math.Ceiling(blobRect.RotateWidth);
                            Size size = DrawingHelper.Mul(new Size(width, height), 1 / extractOperatorResult.BlobScaleFactor);
                            size = DrawingHelper.Add(size, inflateLength);

                            string imagePath = string.Format(@"C:\temp\patternImage{0}.bmp", patternGroupList.IndexOf(patternGroup));
                            patternGroup.RefImagePath = imagePath;

                            using (AlgoImage rotateImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, size))
                            {
                                imageProcessing.Rotate(patternImage, rotateImage, blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? -blobRect.RotateAngle : 90 - blobRect.RotateAngle);
                                patternGroup.RefImage = rotateImage.ToBitmapSource();
                                //rotateImage.Save(imagePath);
                            }
                        }
                        patternImage?.Dispose();
                    }

                    patternGroupList.Remove(refPatternGroup);
                }

                // 이미지 정합
                BitmapSource fullBitmapSource = null;
                if (false)
                {
                    int overlapPx = (int)Math.Round(SystemManager.Instance().OperatorManager.ScanOperator.Settings.OverlapUm / DeveloperSettings.Instance.Resolution);
                    int flowCount = extractOperatorResultList.Count();
                    Size fullImageSize = Size.Empty;
                    fullImageSize.Width = (int)extractOperatorResultList.Sum(f => f.ScanOperatorResult.TopLightImage.Width) - (overlapPx * (flowCount - 1));
                    fullImageSize.Height = (int)extractOperatorResultList.Max(f => f.ScanOperatorResult.TopLightImage.Height);
                    using (AlgoImage fullImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, fullImageSize.Width / 10, fullImageSize.Height / 10))
                    {
                        int dstX = 0;
                        foreach (ExtractOperatorResult extractOperatorResult in extractOperatorResultList)
                        {
                            AlgoImage srcImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
                            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(srcImage);
                            Size copySize = DrawingHelper.Div(srcImage.Size, 10);
                            Rectangle dstRect = new Rectangle(new Point(dstX, 0), copySize);
                            dstRect.Intersect(new Rectangle(Point.Empty, fullImage.Size));
                            using (AlgoImage dstImage = fullImage.GetSubImage(dstRect))
                                ip.Resize(srcImage, dstImage);
                            dstX += (copySize.Width - overlapPx / 10);
                        }
                        fullBitmapSource = fullImage.ToBitmapSource();
                    }
                }
                TeachOperatorResult teachOperatorResult = new TeachOperatorResult(resultKey, fullBitmapSource, new List<PatternGroup>() { refPatternGroup }, patternGroupList);
                SystemManager.Instance().OperatorCompleted(teachOperatorResult);

                OperatorState = OperatorState.Idle;
            }
        }
    }

    public class TeachOperatorResult : OperatorResult
    {
        //BitmapSource bitmapSource;
        List<PatternGroup> inspectPatternList = new List<PatternGroup>();
        List<PatternGroup> candidatePatternList = new List<PatternGroup>();

        //public BitmapSource BitmapSource { get => bitmapSource; }
        public List<PatternGroup> InspectPatternList { get => inspectPatternList; }
        public List<PatternGroup> CandidatePatternList { get => candidatePatternList; }

        public TeachOperatorResult(ResultKey resultKey, 
          BitmapSource bitmapSource,  List<PatternGroup> inspectPatternList, List<PatternGroup> candidatePatternList) 
            : base(ResultType.Train, resultKey, DateTime.Now)
        {
            //this.bitmapSource = bitmapSource;
            this.inspectPatternList = inspectPatternList;
            this.candidatePatternList = candidatePatternList;
        }

        public TeachOperatorResult(ResultKey resultKey, Exception exception) 
            : base(ResultType.Train, resultKey, DateTime.Now, exception)
        {

        }

        protected override string GetLogMessage()
        {
            return "";
        }
    }

    public class TeachOperatorSettings : OperatorSettings
    {
        [CatecoryAttribute("Teach"), NameAttribute("Grouping Threshold")]
        public float DiffGroupThreshold
        {
            get => diffGroupThreshold;
            set => diffGroupThreshold = Math.Max(5, value);
        }
        float diffGroupThreshold = 100;

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Trainner");
        }

        public override void Load(XmlElement xmlElement)
        {
            diffGroupThreshold = XmlHelper.GetValue(xmlElement, "DiffGroupThreshold", diffGroupThreshold);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DiffGroupThreshold", diffGroupThreshold.ToString());
        }
    }
}
