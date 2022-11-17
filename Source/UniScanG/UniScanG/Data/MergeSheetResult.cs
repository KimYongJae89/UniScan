using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace UniScanG.Data
{
    public class MergeSheetResult : AlgorithmResultG
    {
        static object offsetLogLockObject = new object();
        public static string PrevImageName = "Prev.jpg";

        int subCount = 1;

        public OffsetStructSet[] OffsetStructSet { get => this.offsetStructSet; }
        OffsetStructSet[] offsetStructSet = null;

        public float[][] PartialProjection { get; set; } = null;
        public float[] DefectDensity { get; set; } = null;
        public bool IsCritical { get; set; }
        public bool IsImported { get; private set; } = false;

        public Dictionary<string, TimeSpan> SpandTimes { get; private set; } = new Dictionary<string, TimeSpan>();
        public DateTime EndTime { get; set; }

        public int Index => this.index;
        protected int index;

        public bool IsNG => SheetSubResultList.Exists(f => f.IsDefect);

        public string ResultPath { get; set; }

        public override string PrevImagePath { get => Path.Combine(this.ResultPath, PrevImageName); }

        public MergeSheetResult(int index, int subCount, string resultPath, bool import = true) : base("MergeSheetResult")
        {
            this.index = index;
            this.subCount = subCount;
            this.offsetStructSet = new Data.OffsetStructSet[subCount];
            this.ResultPath = resultPath;

            if (import == true)
                this.Import(resultPath);
        }

        public MergeSheetResult(int index, AlgorithmResultG sheetResult) : base("MergeSheetResult")
        {
            this.index = index;
            this.Copy(sheetResult);
        }

        public override void Copy(AlgorithmResultG sheetResult)
        {
            base.Copy(sheetResult);

            MergeSheetResult mergeSheetResult = sheetResult as MergeSheetResult;
            this.EndTime = mergeSheetResult.EndTime;
            this.SpandTimes = new Dictionary<string, TimeSpan>(mergeSheetResult.SpandTimes);

            if (mergeSheetResult.subCount > 0)
            {
                this.subCount = mergeSheetResult.subCount;

                this.offsetStructSet = new Data.OffsetStructSet[this.subCount];
                Array.Copy(mergeSheetResult.offsetStructSet, this.offsetStructSet, this.subCount);
            }

            this.IsImported = mergeSheetResult.IsImported;
            this.DefectDensity = (float[])mergeSheetResult.DefectDensity?.Clone();
            if(mergeSheetResult.PartialProjection != null)
            {
                this.PartialProjection = new float[mergeSheetResult.PartialProjection.Length][];
                for (int i = 0; i < mergeSheetResult.PartialProjection.Length; i++)
                {
                    int length = mergeSheetResult.PartialProjection[i].Length;
                    this.PartialProjection[i] = new float[length];
                    Array.Copy(mergeSheetResult.PartialProjection[i], this.PartialProjection[i], length);
                }
            }
        }

        public MergeSheetResult Clone()
        {
            MergeSheetResult clone = new MergeSheetResult(this.index, this.subCount, this.ResultPath, false);
            clone.Copy(this);
            return clone;
        }

        public void Union(MergeSheetResult[] mergeSheetResults)
        {
            SizeF patternSize = SizeF.Empty;
            Size patternSizePx = Size.Empty;
            DateTime startTime = DateTime.MaxValue;
            DateTime endTime = DateTime.MinValue;
            TimeSpan spendTime = TimeSpan.Zero;

            this.subCount = mergeSheetResults.Length;
            this.offsetStructSet = new OffsetStructSet[this.subCount];
            this.PartialProjection = new float[this.subCount][];

            for (int i = 0; i < mergeSheetResults.Length; i++)
            {
                MergeSheetResult result = mergeSheetResults[i];

                if (result.offsetStructSet == null || result.offsetStructSet.Length == 0)
                {
                    this.offsetStructSet[i] = new OffsetStructSet(0);
                }
                else
                {
                    this.offsetStructSet[i] = result.offsetStructSet[0];
                }

                this.sheetSubResultList.AddRange(result.sheetSubResultList);

                SpandTimes.Add($"{i}_Whole", result.spandTime);
                foreach (var time in result.SpandTimes)
                    SpandTimes.Add($"{i}_{time.Key}", time.Value);

                patternSize.Width += result.sheetSize.Width;
                patternSize.Height = Math.Max(patternSize.Height, result.sheetSize.Height);

                patternSizePx.Width += result.sheetSizePx.Width;
                patternSizePx.Height = Math.Max(patternSizePx.Height, result.sheetSizePx.Height);

                this.postProcessed |= result.postProcessed;

                this.good &= result.good;

                startTime = DateTime.Compare(startTime, result.StartTime) < 0 ? startTime : result.StartTime;
                endTime = DateTime.Compare(endTime, result.EndTime) > 0 ? endTime : result.EndTime;
                spendTime = spendTime.CompareTo(result.SpandTime) > 0 ? spendTime : result.SpandTime;
                this.PartialProjection[i] = result.PartialProjection[0];
            }

            this.sheetSize = patternSize;
            this.sheetSizePx = patternSizePx;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.SpandTime = spendTime;
        }

        public void AdjustSizeFilter(float minSize, float maxSize)
        {
            sheetSubResultList = sheetSubResultList.FindAll
                (s => Math.Max(s.RealRegion.Width, s.RealRegion.Height) >= minSize
                && Math.Max(s.RealRegion.Width, s.RealRegion.Height) <= maxSize);
        }

        public override void UpdateSpandTime()
        {
            //this.spandTime = this.EndTime - this.StartTime;

            long tick = (this.SpandTimes.Count > 0) ? this.SpandTimes.Sum(f => f.Value.Ticks) : 0;
            this.spandTime = new TimeSpan(tick);
        }
        
        public string GetExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.AppendLine(ExportHeader());

            if (this.DefectDensity != null)
                stringBuilder.AppendLine($"DefectDensity,{string.Join(";", this.DefectDensity.Select(f => f.ToString()))}");

            if (this.PartialProjection != null)
            {
                for (int i = 0; i < this.PartialProjection.Length; i++)
                    stringBuilder.AppendLine($"PartialProjection[{i}],{string.Join(",", this.PartialProjection[i].Select(f => f.ToString()))}");
            }

            stringBuilder.AppendLine(FoundedObjInPattern.GetExportHeader());

            // text
            foreach (FoundedObjInPattern subResult in this.sheetSubResultList)
                stringBuilder.AppendLine(subResult.ToExportData());

            return stringBuilder.ToString();
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            // 불량 정보와 offset 정보를 함께 저장함.
            Directory.CreateDirectory(path);
            //string fileName = Path.Combine(path, string.Format("{0}.csv", SheetCombiner.TypeName));
            string fileName = Path.Combine(path, string.Format("{0}.csv", AlgorithmResultG.CSVFileName));

            // offset
            for (int i = 0; i < this.subCount; i++)
            {
                string offsetLogFile = Path.Combine(Path.GetDirectoryName(path), string.Format("CAM{0}.csv", i));

                string[] offsets = new string[this.offsetStructSet[i].LocalOffsets.Length + 3];
                offsets[offsets.Length - 1] = Environment.NewLine;

                offsets[0] = this.index.ToString("D04");

                //offsets[1] = string.Format("{0},{1}", this.offsetStructSet[i].PatternOffset.Offset.X, this.offsetStructSet[i].PatternOffset.Offset.Y);
                offsets[1] = this.offsetStructSet[i].PatternOffset.GetExportData();
                this.offsetStructSet[i].PatternOffset.ImageD?.SaveImage(Path.Combine(path, string.Format("PatternOffset_C{0:D}.Jpg", i)));

                for (int j = 0; j < this.offsetStructSet[i].LocalOffsets.Length; j++)
                {
                    OffsetStruct offsetStruct = this.offsetStructSet[i].LocalOffsets[j];
                    //offsets[j + 2] = string.Format("{0},{1},{2},{3}", offsetStruct.Good, offsetStruct.Offset.X, offsetStruct.Offset.Y, offsetStruct.Score);
                    offsets[j + 2] = offsetStruct.GetExportData();
                    this.offsetStructSet[i].LocalOffsets[j].ImageD?.SaveImage(Path.Combine(path, string.Format("LocalOffset_C{0:D}_{1:D02}.Jpg", i, j)));
                }
         
                lock (offsetLogLockObject)
                {
                    if (!File.Exists(offsetLogFile))
                        File.AppendAllText(offsetLogFile, "No, " + this.offsetStructSet[i].GetExportHeader());
                    File.AppendAllText(offsetLogFile, string.Join(", ", offsets));
                }
            }

            // Preview Image
            if (PrevImage != null)
            {
                if (this.IsNG || AdditionalSettings.Instance().SaveGoodPatternImage)
                {
                    //using (Bitmap newBitmap = (Bitmap)prevImage.Clone())
                    using (Bitmap newBitmap =new Bitmap(prevImage))
                    ImageHelper.SaveImage(newBitmap, Path.Combine(path, PrevImageName));
                }
            }


            string dataString = GetExportString();
            //StringBuilder stringBuilder = new StringBuilder();

            //// Header
            //stringBuilder.AppendLine(ExportHeader());

            //if (this.DefectDensity != null)
            //    stringBuilder.AppendLine($"DefectDensity,{string.Join(",", this.DefectDensity.Select(f => f.ToString()))}");

            //stringBuilder.AppendLine(FoundedObjInPattern.GetExportHeader());

            //// text
            //foreach (FoundedObjInPattern subResult in this.sheetSubResultList)
            //    stringBuilder.AppendLine(subResult.ToExportData());

            try
            {
                //File.WriteAllText(fileName, stringBuilder.ToString());
                File.WriteAllText(fileName, dataString);

                // 불량 image
                foreach (FoundedObjInPattern subResult in sheetSubResultList)
                {
                    if (subResult.Image != null)
                    {
                        string savePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}.jpg", this.index, subResult.CamIndex, subResult.Index));
                        Image2D image2D = Image2D.FromBitmap(subResult.Image);
                        image2D.SaveImage(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        image2D.Dispose();
                    }

                    if (subResult.BufImage != null)
                    {
                        string savePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}B.jpg", this.index, subResult.CamIndex, subResult.Index));
                        Image2D image2D = Image2D.FromBitmap(subResult.BufImage);
                        image2D.SaveImage(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        image2D.Dispose();
                    }
                }
            }
            catch (IOException)
            { }
        }

        private string ExportHeader()
        {
            //return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", 
            //    this.index, 
            //    this.postProcessed,
            //    this.StartTime.ToString("yyyyMMdd.HHmmss.fff"), 
            //    this.spandTime,
            //    this.sheetSize.Width, this.sheetSize.Height,
            //    this.sheetSizePx.Width, this.sheetSizePx.Height);

            List<string> list = new List<string>();
            list.Add($"*");
            list.Add($"No,{this.index}");
            list.Add($"PostProcessed,{this.PostProcessed}");
            list.Add($"Size,{this.SheetSize.Width},{this.SheetSize.Height}");
            list.Add($"SizePx,{this.SheetSizePx.Width},{this.SheetSizePx.Height}");
            list.Add($"StartTime,{this.StartTime.ToString(AlgorithmResultG.DateTimeFormat)}");
            list.Add($"EndTime,{this.EndTime.ToString(AlgorithmResultG.DateTimeFormat)}");
            list.AddRange(this.SpandTimes.Select(f => $"SpandTimes,{f.Key},{f.Value.Ticks}"));
            return string.Join("*", list);
        }

        public void ImportPrevImage()
        {
            prevImage = this.PrevImage;
            //string imageName = Path.Combine(resultPath, "Prev.jpg");
            //if (File.Exists(imageName) == true)
            //{
            //    //Bitmap image = (Bitmap)ImageHelper.LoadImage(imageName);
            //    prevImage = (Bitmap)ImageHelper.LoadImage(imageName);
            //}
        }

        public override bool Import(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = this.ResultPath;

            // 불량 정보만 로드함
            string fileName = Path.Combine(path, string.Format("{0}.csv", AlgorithmResultG.CSVFileName));
            if (File.Exists(fileName) == false)
            {
                fileName = Path.Combine(path, string.Format("{0}.csv", SheetCombiner.TypeName));
                if (File.Exists(fileName) == false)
                    return false;
            }

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                int lineCount = lines.Length;
                if(!ImportHeader(lines[0]))
                    return false;

                //this.PrevImagePath = Path.Combine(path, PrevImageName);
                //if (File.Exists(prevImagePath))
                //prevImage = ImageHelper.LoadImage(prevImagePath) as Bitmap;

                for (int i = 1; i < lineCount; i++)
                {
                    string line = lines[i];
                    if (ImportDefectDensity(line))
                        continue;

                    string token = line.Split(',').First();
                    FoundedObjInPattern subResult = FoundedObjInPattern.Create(token);
                    if (subResult == null)
                        continue;

                    if (subResult.FromExportData(line))
                    {
                        string imagePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}.jpg", this.index, subResult.CamIndex, subResult.Index));
                        if (File.Exists(imagePath) == false) // 하위 호환성
                        {
                            string imagePathOld = Path.Combine(path, string.Format("C{0}I{1}.jpg", subResult.CamIndex, subResult.Index));
                            if (File.Exists(imagePathOld))
                                imagePath = imagePathOld;
                        }

                        subResult.ImagePath = imagePath;

                        string bufImagePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}B.jpg", this.index, subResult.CamIndex, subResult.Index));
                        if (File.Exists(bufImagePath))
                            subResult.BufImagePath = bufImagePath;

                        int existIndex = sheetSubResultList.FindIndex(f => f.ImagePath == subResult.ImagePath);
                        if (existIndex < 0)
                            sheetSubResultList.Add(subResult);
                        else
                            sheetSubResultList[i] = subResult;
                    }
                }

                IsImported = true;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("MergeSheetResult::Import fail - path: {0}", path));
                //isImported = false;
                return false;
            }
        }

        public static bool IsHeader(string line)
        {
            MergeSheetResult temp = new MergeSheetResult(-1, 0, null, false);
            return temp.ImportHeader(line);
        }

        public bool ImportHeader(string headerLine)
        {
            if(ImportHeader2(headerLine))
                return true;

            string[] token = headerLine.Split(',');
            try
            {
                this.IsImported = true;

                if (token.Length == 1)
                {
                    TimeSpan spandTime = TimeSpan.Parse(token[0]);

                    this.spandTime = spandTime;

                    return true;
                }
                else if (token.Length == 4)
                {
                    int index = int.Parse(token[0]);
                    TimeSpan spandTime = TimeSpan.Parse(token[1]);
                    float sheetW = float.Parse(token[2]);
                    float sheetH = float.Parse(token[3]);

                    this.index = index;
                    this.spandTime = spandTime;
                    this.sheetSize.Width = sheetW;
                    this.sheetSize.Width = sheetH;

                    return true;
                }
                else if (token.Length == 5)
                {
                    int index = int.Parse(token[0]);
                    bool postProcessed = bool.Parse(token[1]);
                    TimeSpan spandTime = TimeSpan.Parse(token[2]);
                    float sheetSizeWidth = float.Parse(token[3]);
                    float sheetSizeHeight = float.Parse(token[4]);

                    this.index = index;
                    this.postProcessed = postProcessed;
                    this.spandTime = spandTime;
                    this.sheetSize.Width = sheetSizeWidth;
                    this.sheetSize.Height = sheetSizeHeight;
                    return true;
                }
                else if (token.Length == 6)
                {
                    int index = int.Parse(token[0]);
                    bool postProcessed = bool.Parse(token[1]);
                    DateTime startTime = DateTime.ParseExact(token[2], "yyyyMMdd.HHmmss.fff", null, System.Globalization.DateTimeStyles.None);
                    TimeSpan spandTime = TimeSpan.Parse(token[3]);
                    float sheetSizeWidth = float.Parse(token[4]);
                    float sheetSizeHeight = float.Parse(token[5]);

                    this.index = index;
                    this.postProcessed = postProcessed;
                    this.StartTime = startTime;
                    this.spandTime = spandTime;
                    this.sheetSize.Width = sheetSizeWidth;
                    this.sheetSize.Height = sheetSizeHeight;
                    this.sheetSizePx = Size.Round(DrawingHelper.Mul(this.sheetSize, 1000f / 14.0f));

                    return true;
                }
                else if (token.Length == 8)
                {
                    int index = int.Parse(token[0]);
                    bool postProcessed = bool.Parse(token[1]);
                    DateTime startTime = DateTime.ParseExact(token[2], "yyyyMMdd.HHmmss.fff", null, System.Globalization.DateTimeStyles.None);
                    TimeSpan spandTime = TimeSpan.Parse(token[3]);
                    float sheetSizeWidth = float.Parse(token[4]);
                    float sheetSizeHeight = float.Parse(token[5]);
                    int sheetSizePxWidth = int.Parse(token[6]);
                    int sheetSizePxHeight = int.Parse(token[7]);

                    this.index = index;
                    this.postProcessed = postProcessed;
                    this.StartTime = startTime;
                    this.SpandTime = spandTime;
                    this.sheetSize.Width = sheetSizeWidth;
                    this.sheetSize.Height = sheetSizeHeight;
                    this.sheetSizePx.Width = sheetSizePxWidth;
                    this.sheetSizePx.Height = sheetSizePxHeight;

                    return true;
                }

                this.IsImported = false;
                return false;
            }
            catch (FormatException)
            {
                this.IsImported = false;
                return false;
            }
        }

        private bool ImportHeader2(string line)
        {
            if (!line.StartsWith("*"))
                return false;

            string[] tokens = line.Trim('*').Split('*');

            foreach (string token in tokens)
            {
                string[] subTokens = token.Split(',');
                switch (subTokens[0])
                {
                    case "No":
                        this.index = int.Parse(subTokens[1]);
                        break;

                    case "PostProcessed":
                        this.PostProcessed = bool.Parse(subTokens[1]);
                        break;

                    case "Size":
                        this.SheetSize = new SizeF(float.Parse(subTokens[1]), float.Parse(subTokens[2]));
                        break;

                    case "SizePx":
                        this.SheetSizePx = new Size(int.Parse(subTokens[1]), int.Parse(subTokens[2]));
                        break;

                    case "StartTime":
                        this.StartTime = DateTime.ParseExact(subTokens[1], AlgorithmResultG.DateTimeFormat, null);
                        break;

                    case "EndTime":
                        this.EndTime = DateTime.ParseExact(subTokens[1], AlgorithmResultG.DateTimeFormat, null);
                        break;

                    case "SpandTimes":
                        {
                            if (!this.SpandTimes.ContainsKey(subTokens[1]))
                                this.SpandTimes.Add(subTokens[1], TimeSpan.Zero);
                            this.SpandTimes[subTokens[1]] = new TimeSpan(long.Parse(subTokens[2]));
                        }
                        break;
                }
            }
            return true;
        }

        public void ImportLine(string line)
        {
            if (ImportDefectDensity(line))
                return;

            string token = line.Split(',').First();
            FoundedObjInPattern subResult = FoundedObjInPattern.Create(token);
            if (subResult == null)
                return;

            if (subResult.FromExportData(line))
            {
                string imagePath = Path.Combine(this.ResultPath, string.Format("S{0}_C{1}_I{2}.jpg", this.index, subResult.CamIndex, subResult.Index));
                // 일년 이상 지났으니 이제 없어도 되겠지. 21.08.19
                //if (File.Exists(imagePath) == false) // 하위 호환성
                //{
                //    string imagePathOld = Path.Combine(this.ResultPath, string.Format("C{0}I{1}.jpg", subResult.CamIndex, subResult.Index));
                //    if (File.Exists(imagePathOld))
                //        imagePath = imagePathOld;
                //}
                subResult.ImagePath = imagePath;

                string bufImagePath = Path.Combine(this.ResultPath, string.Format("S{0}_C{1}_I{2}B.jpg", this.index, subResult.CamIndex, subResult.Index));
                //if (File.Exists(bufImagePath))
                    subResult.BufImagePath = bufImagePath;

                sheetSubResultList.Add(subResult);
            }
        }

        private bool ImportDefectDensity(string line)
        {
            if (this.DefectDensity != null)
                return false;

            string[] tokens = line.Split(',');
            if (tokens[0] != "DefectDensity")
                return false;

            this.DefectDensity = tokens.Skip(1).Select(f => float.Parse(f)).ToArray();
            return true;
        }

        public void ImportImage()
        {
            string prevImagePath = Path.Combine(this.ResultPath, string.Format("Prev.Jpg"));
            if (File.Exists(prevImagePath))
                prevImage = ImageHelper.LoadImage(prevImagePath) as Bitmap;

            foreach (FoundedObjInPattern subResult in sheetSubResultList)
            {
                if (string.IsNullOrEmpty(subResult.ImagePath))
                {
                    string imagePath = Path.Combine(this.ResultPath, string.Format("C{0}I{1}.jpg", subResult.CamIndex, subResult.Index));
                    if (File.Exists(imagePath))
                        subResult.ImagePath = imagePath;

                    string bufImagePath = Path.Combine(this.ResultPath, string.Format("C{0}I{1}B.jpg", subResult.CamIndex, subResult.Index));
                    if (File.Exists(bufImagePath))
                        subResult.BufImagePath = bufImagePath;
                }
            }
        }

        public void UpdateDefectDensity(StripeDefectAlarmSetting stripeBlotAlarmSetting)
        {
            if(!stripeBlotAlarmSetting.Use)
            {
                this.DefectDensity = null;
                return;
            }

            float w = stripeBlotAlarmSetting.WindowWidth * 1000;
            float w2 = w / 2;
            SizeF patternSizeUm = DrawingHelper.Mul(this.sheetSize, 1000f);
            SizeF windowSizeUm = new SizeF(w, patternSizeUm.Height);

            SortedList<int, float> list = new SortedList<int, float>();
            int step = (int)Math.Ceiling(patternSizeUm.Width / windowSizeUm.Width);
            for (int i = 0; i < 2 * step - 1; i++)
                list.Add(i, 0);

            //lock (this.StripeBlotAlarm)
            {
                RectangleF[] array = this.sheetSubResultList.FindAll(f => f is DefectObj).Select(f => f.RealRegion).ToArray();
                Array.ForEach(array, f =>
                {
                    int src = Math.Max((int)Math.Floor(f.Left / w2) - 1, 0);
                    int dst = Math.Min((int)Math.Ceiling(f.Right / w2) + 1, list.Count);

                    for (int i = src; i < dst; i++)
                    {
                        PointF windowLocUm = new PointF(i * w2, 0);
                        RectangleF windowUm = new RectangleF(windowLocUm, windowSizeUm);
                        if (windowUm.Right > patternSizeUm.Width)
                            windowUm.Width = patternSizeUm.Width - windowUm.X;
                        //float windowArea = windowUm.Width * windowUm.Height;

                        RectangleF intersect = RectangleF.Intersect(windowUm, f);
                        //float intersectArea = intersect.Width * intersect.Height;

                        //float ratio = intersectArea * 100 / windowArea;
                        float ratio = intersect.Height * 100 / windowUm.Height;

                        if (!list.ContainsKey(i))
                            list.Add(i, 0);
                        list[i] = list[i] + ratio;
                    }
                });
            }
            this.DefectDensity = list.Values.ToArray();
        }

        public void UpdateCritical(CriticalRollAlarmSetting rollDefectAlarmSetting)
        {
            int count = this.sheetSubResultList.Count(f => f.RealLength > rollDefectAlarmSetting.DefectLength);
            this.IsCritical = (count > rollDefectAlarmSetting.DefectCount);
        }
    }
}
