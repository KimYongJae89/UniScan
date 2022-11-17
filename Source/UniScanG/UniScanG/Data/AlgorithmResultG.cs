using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;

namespace UniScanG.Data
{
    public enum DefectType { Unknown = -1, Noprint, PinHole, Spread, Attack, Coating, Sticker, Margin, Transform, Total = 99 }
    static class DefectTypeExtender
    {
        public static bool IsMeasureType(DefectType defectType) { return (defectType == DefectType.Margin || defectType == DefectType.Transform); }

        public static string GetLocalString(this DefectType defectType)
        {
            return StringManager.GetString(typeof(DefectType).FullName, defectType.ToString());
        }

        public static DefectType[] GetDefaultSelectState()
        {
            return new DefectType[] { DefectType.Noprint, DefectType.PinHole, DefectType.Spread, DefectType.Attack, DefectType.Coating, DefectType.Sticker };
        }
    }

    public class OffsetStruct : IDisposable
    {
        public static Size ImageSize => new Size(256, 256);

        enum ExportHeader { Judgment, Position, BX, BY, OX, OY, VW, VH, MinScore }

        public bool IsGood { get; set; }

        public string Position { get; set; }

        public Point Base { get => Point.Round(this.BaseF); }
        public PointF BaseF { get; set; }

        public Point Offset { get => Point.Round(this.OffsetF); }
        public PointF OffsetF { get; set; }

        public Size Variation { get => Size.Round(VariationF); }
        public SizeF VariationF { get; set; }

        public float Score { get; set; }

        public ImageD ImageD { get; set; }

        public OffsetStruct()
        {
            Set(false, "", Point.Empty, PointF.Empty, Size.Empty, 0, null);
        }

        public OffsetStruct(bool good, string position, PointF baseF, PointF offsetF, SizeF size, float score, ImageD imageD)
        {
            Set(good, position, baseF, offsetF, size, score, imageD);
        }

        public void Set(bool good, string position,PointF baseF, PointF offsetF, SizeF variation, float score, ImageD imageD)
        {
            this.IsGood = good;
            this.Position = position;
            this.BaseF = baseF;
            this.OffsetF = offsetF;
            this.Score = score;
            this.VariationF = variation;
            this.ImageD = imageD;
        }

        public void Clear()
        {
            this.IsGood = false;
            this.Position = "";
            this.BaseF = PointF.Empty;
            this.OffsetF = PointF.Empty;
            this.VariationF = SizeF.Empty;
            this.Score = 0;
            this.ImageD = null;
        }

        public void Dispose()
        {
            this.ImageD?.Dispose();
        }

        public OffsetStruct Clone()
        {
            return new OffsetStruct(this.IsGood, this.Position,this.BaseF, this.OffsetF, this.VariationF, this.Score, this.ImageD?.Clone());
        }

        public void CopyFrom(OffsetStruct src)
        {
            this.IsGood = src.IsGood;
            this.Position = src.Position;
            this.BaseF = src.BaseF;
            this.OffsetF = src.OffsetF;
            this.VariationF = src.VariationF;
            this.Score = src.Score;
            this.ImageD = src.ImageD?.Clone();
        }

        public static string GetExportHeader()
        {
            string[] headers = Enum.GetNames(typeof(ExportHeader));
            return string.Join(",", headers);
        }

        public static OffsetStruct FromExportData(string exportData)
        {
            string[] tokens = exportData.Split(',');
            try
            {
                bool isGood = bool.Parse(tokens[0]);
                string s = tokens[1];
                float bx = 0, by = 0;
                float ox = 0, oy = 0;
                float vw = 0, vh = 0;
                float score = 0;
                if (tokens.Length == 7)
                {
                    ox = float.Parse(tokens[2]);
                    oy = float.Parse(tokens[3]);
                    vw = float.Parse(tokens[4]);
                    vh = float.Parse(tokens[5]);
                    score = float.Parse(tokens[6]);
                }
                else if (tokens.Length == 9)
                {
                    bx = float.Parse(tokens[2]);
                    by = float.Parse(tokens[3]);
                    ox = float.Parse(tokens[4]);
                    oy = float.Parse(tokens[5]);
                    vw = float.Parse(tokens[6]);
                    vh = float.Parse(tokens[7]);            
                    score = float.Parse(tokens[7]);
                }

                return new OffsetStruct(isGood, s, new PointF(bx, by), new PointF(ox, oy), new SizeF(vw, vh), score, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("OffsetStruct::FromExportData - Message: {0}, Data: {1}", ex.Message, exportData));
                return new OffsetStruct();
            }
        }

        public string GetExportData()
        {
            Array values = Enum.GetValues(typeof(ExportHeader));

            string[] str = new string[values.Length];
            foreach (object value in values)
            {
                string valueString;
                switch (value)
                {
                    case ExportHeader.Judgment:
                        valueString = this.IsGood.ToString();
                        break;
                    case ExportHeader.Position:
                        valueString = this.Position;
                        break;
                    case ExportHeader.BX:
                        valueString = this.BaseF.X.ToString();
                        break;
                    case ExportHeader.BY:
                        valueString = this.BaseF.Y.ToString();
                        break;
                    case ExportHeader.OX:
                        valueString = this.OffsetF.X.ToString();
                        break;
                    case ExportHeader.OY:
                        valueString = this.OffsetF.Y.ToString();
                        break;
                    case ExportHeader.VW:
                        valueString = this.VariationF.Width.ToString();
                        break;
                    case ExportHeader.VH:
                        valueString = this.VariationF.Height.ToString();
                        break;
                    case ExportHeader.MinScore:
                        valueString = this.Score.ToString();
                        break;
                    default:
                        valueString = "";
                        break;
                }
                str[(int)value] = valueString;
            }

            return string.Join(",", str);
        }
    }

    public class OffsetStructSet
    {
        public OffsetStruct PatternOffset => this.patternOffset;
        OffsetStruct patternOffset;

        public OffsetStruct[] LocalOffsets => this.localOffsets;
        OffsetStruct[] localOffsets;

        public int LocalCount => this.localOffsets.Length;

        public OffsetStructSet(int localCount)
        {
            this.patternOffset = new OffsetStruct();
            this.localOffsets = new OffsetStruct[localCount];
            for (int i = 0; i < localCount; i++)
                this.localOffsets[i] = new OffsetStruct();
        }

        public void CopyFrom(OffsetStructSet src)
        {
            this.patternOffset = src.PatternOffset.Clone();
            this.localOffsets = new OffsetStruct[src.LocalCount];
            for (int i = 0; i < src.LocalCount; i++)
                this.localOffsets[i] = src.localOffsets[i].Clone();
        }

        public OffsetStructSet Clone(OffsetStructSet src)
        {
            OffsetStructSet set = new OffsetStructSet(this.LocalCount);
            set.CopyFrom(src);
            return set;
        }

        public void Resize(int localCount)
        {
            Array.Resize(ref this.localOffsets, localCount);
            for (int i = 0; i < localCount; i++)
                this.localOffsets[i] = new OffsetStruct();
        }

        public PointF GetLocalOffset(int i)
        {
            if (0 <= i && i < this.localOffsets.Length)
                return this.localOffsets[i].OffsetF;
            return PointF.Empty;
        }

        public Point GetOffset(int i)
        {
            return Point.Round(GetOffsetF(i));
        }

        public PointF GetOffsetF(int i)
        {
            if (i < 0)
                return this.patternOffset.OffsetF;

            OffsetStruct offsetStruct = this.localOffsets.ElementAtOrDefault(i);
            PointF localOffset = offsetStruct == null ? PointF.Empty : offsetStruct.OffsetF;
            return DrawingHelper.Add(this.patternOffset.OffsetF, localOffset);
        }

        public string GetExportHeader()
        {
            List<string> stringList = new List<string>();
            stringList.Add(OffsetStruct.GetExportHeader());
            for (int i = 0; i < this.LocalOffsets.Length; i++)
                stringList.Add(OffsetStruct.GetExportHeader());
            stringList.Add(Environment.NewLine);

            return string.Join(",", stringList.ToArray());
        }

        public string GetExportData()
        {
            string global = this.PatternOffset.GetExportData();

            string[] locals = new string[this.LocalOffsets.Length];
            for (int i = 0; i < this.LocalOffsets.Length; i++)
            {
                OffsetStruct offsetStruct = this.LocalOffsets[i];
                locals[i] = offsetStruct.GetExportData();
            }

            return string.Join(",", global, locals);
        }
    }

    public class PartialProjection
    {
        public float[] Datas => this.datas;
        float[] datas;

        public int Length { get; private set; }

        public void Initialize(int size)
        {
            this.datas = new float[size];
            this.Length = 0;
        }

        public void Dispose()
        {
            Array.Resize(ref this.datas, 0);
            this.Length = 0;
        }

        public void Clear()
        {
            Array.Clear(this.datas, 0, this.datas.Length);
            this.Length = 0;
        }

        public void Set(float[] datas, int length)
        {
            System.Diagnostics.Debug.Assert(datas.Length >= length);
            Array.Copy(datas, this.datas, length);
            this.Length = length;
        }
    }

    public abstract class AlgorithmResultG : AlgorithmResult, IExportable
    {
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public static string CSVFileName = "Result.csv";

        public static ResultCollector resultCollector = null;

        public DateTime StartTime { get; set; } = DateTime.MinValue;

        public virtual string PrevImagePath { get; private set; }

        public Bitmap PrevImage
        {
            get
            {
                if (prevImage == null && File.Exists(this.PrevImagePath))
                    prevImage = ImageHelper.LoadImage(this.PrevImagePath) as Bitmap;
                return prevImage;
            }
            set { prevImage = value; }
        }
        protected Bitmap prevImage = null;

        public bool PostProcessed { get => this.postProcessed; set => this.postProcessed = value; }
        protected bool postProcessed = false;

        public List<FoundedObjInPattern> SheetSubResultList { get => sheetSubResultList; set => this.sheetSubResultList = value; }
        protected List<FoundedObjInPattern> sheetSubResultList = new List<FoundedObjInPattern>();
        
        public SizeF SheetSize { get => this.sheetSize; set => this.sheetSize = value; }
        protected SizeF sheetSize = SizeF.Empty;

        public Size SheetSizePx { get => this.sheetSizePx; set => this.sheetSizePx = value; }
        protected Size sheetSizePx = Size.Empty;

        public AlgorithmResultG(string algorithmName) : base(algorithmName)
        {
            this.StartTime = DateTime.Now;
        }

        public virtual void Copy(AlgorithmResultG sheetResult)
        {
            this.StartTime = sheetResult.StartTime;
            this.spandTime = sheetResult.spandTime;
            this.prevImage = sheetResult.prevImage;
            this.good = sheetResult.good;
            this.sheetSize = sheetResult.sheetSize;
            this.sheetSizePx = sheetResult.sheetSizePx;
            this.postProcessed = sheetResult.postProcessed;

            this.prevImage = (Bitmap)sheetResult.prevImage?.Clone();
            this.PrevImagePath = sheetResult.PrevImagePath;
            this.SheetSubResultList.AddRange(sheetResult.sheetSubResultList.ToArray());
        }

        public virtual void UpdateSpandTime()
        {
            this.spandTime = DateTime.Now - this.StartTime;
        }

        public void Offset(int x, int y, SizeF pelSize)
        {
            if (x == 0 && y == 0)
                return;

            float resizeReatio = SystemTypeSettings.Instance().ResizeRatio;
            //resizeReatio = 1;
            sheetSubResultList.ForEach(f =>
            {
                FoundedObjInPattern ssr = f as FoundedObjInPattern;
                if (ssr != null)
                    ssr.Offset(x, y, pelSize);
                //else
                //    f.Offset(x * resizeReatio, y * resizeReatio);
            });

        }

        internal void UpdateSubResultImage()
        {
            this.sheetSubResultList.ForEach(f => f.ImportImage());
        }
        //public bool Import(string path)
        //{
        //    string fileName = Path.Combine(path, string.Format("{0}.csv", "Result"));
        //    if (File.Exists(fileName) == false)
        //        return false;

        //    List<string> lines = File.ReadAllLines(fileName).ToList();
        //    return Import(lines);
        //}

        //public abstract bool Import(List<string> lines);

        public abstract void Export(string path, CancellationToken cancellationToken);
        public abstract bool Import(string path);

    }
}
