using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniScanG.Data;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Vision;

namespace UniScanG.Gravure.Data
{
    class ColorSetPropertyDescriptor : PropertyDescriptor
    {
        Dictionary<DefectType, Color> source;

        public DefectType Key => this.key;
        DefectType key;

        internal ColorSetPropertyDescriptor(Dictionary<DefectType, Color> source, DefectType key)
            : base(StringManager.GetString(typeof(DefectType).FullName, key.ToString()), null)
        {
            this.source = source;
            this.key = key;
        }

        public override Type PropertyType
        {
            get { return source[key].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            source[key] = (Color)value;
        }

        public override object GetValue(object component)
        {
            return source[key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

    public class ColorSetConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return "";
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            ColorSet colorSet = (ColorSet)value;

            List<DefectType> defectTypeList =  colorSet.Colors.Select(f => f.Key).ToList();
            defectTypeList.Remove(DefectType.Total);
            defectTypeList.Remove(DefectType.Unknown);

            if(!DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin))
                defectTypeList.Remove(DefectType.Margin);

            if(!DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom))
                defectTypeList.Remove(DefectType.Transform);

            List<ColorSetPropertyDescriptor> descriptorList = defectTypeList.Select(f => new ColorSetPropertyDescriptor(colorSet.Colors, f)).ToList();
            return new PropertyDescriptorCollection(descriptorList.ToArray());
        }
    }

    [TypeConverter(typeof(ColorSetConverter))]
    public class ColorSet
    {
        public Dictionary<DefectType, Color> Colors => this.colors;

        Dictionary<DefectType, Color> colors = new Dictionary<DefectType, Color>();

        public ColorSet()
        {
            foreach (DefectType defectType in Enum.GetValues(typeof(DefectType)))
            {
                colors.Add(defectType, GetDefaultColor(defectType));
            }
        }

        public Color GetColor(DefectType defectType)
        {
            if (this.colors.ContainsKey(defectType))
                return this.colors[defectType];
            return SystemColors.Control;
        }

        public static Color GetDefaultColor(DefectType defectType)
        {
            Color color = Color.Empty;

            switch (defectType)
            {
                case DefectType.Unknown:
                case DefectType.Total:
                    color = SystemColors.Control;
                    break;

                case DefectType.Attack:
                    color = Color.Brown;
                    break;

                case DefectType.Noprint:
                    color = Color.Yellow;
                    break;

                case DefectType.Coating:
                    color = Color.Blue;
                    break;

                case DefectType.PinHole:
                case DefectType.Spread:
                    color = Color.Cyan;
                    break;

                case DefectType.Sticker:
                    color = Color.Red;
                    break;

                case DefectType.Margin:
                    color = Color.Orange;
                    break;

                case DefectType.Transform:
                    color = Color.DeepPink;
                    break;
            }

            return color;
        }

        internal void Save(XmlElement xmlElement, string v = "")
        {
            if (!string.IsNullOrEmpty(v))
            {
                Save((XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement(v)));
                return;
            }

            foreach (KeyValuePair<DefectType, Color> pair in this.colors)
                XmlHelper.SetValue(xmlElement, pair.Key.ToString(), pair.Value);
        }

        internal void Load(XmlElement xmlElement, string v = "")
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(v))
            {
                Load(xmlElement[v]);
                return;
            }

            Array array = Enum.GetValues(typeof(DefectType));
            foreach (DefectType defectType in array)
            {
                Color color = Color.Black;
                XmlHelper.GetValue(xmlElement, defectType.ToString(), ref color);
                if (this.colors.ContainsKey(defectType))
                    this.colors[defectType] = color;
                else
                    this.colors.Add(defectType, color);
            }

            this.colors[DefectType.Unknown] = SystemColors.Control;
            this.colors[DefectType.Total] = SystemColors.Control;
        }
    }

    public delegate void OnColorTableUpdatedDelegate();
    public static class ColorTable
    {
        static public event OnColorTableUpdatedDelegate OnColorTableUpdated;

        internal static ColorSet ColorSet = new ColorSet();

        internal static void Load(XmlElement xmlElement, string key)
        {
            ColorSet.Load(xmlElement, key);
            OnColorTableUpdated?.Invoke();
        }

        public static Color GetBgColor(DefectType defectType)
        {
            Color color = GetColor(defectType);
            return GetBgColor(color);
        }

        public static Color GetBgColor(Color color)
        {
            float value = color.GetBrightness();
            if (value < 0.3)
                return Color.White;

            // Sat가 낮으면 검은색.
            float sat = color.GetSaturation();
            if (sat < 0.5)
                return Color.Black;

            // Hue의 위치에 따라서.
            float hue = color.GetHue();
            float split = 30;
            if (hue < split || hue > 180 + split)
                return Color.White;
            else
                return Color.Black;
        }


        public static Color GetColor(DefectType defectType)
        {
            return ColorSet.GetColor(defectType);
        }

        public static void UpdateControlColor(Control control, DefectType defectType)
        {
            Color fColor = Gravure.Data.ColorTable.GetBgColor(defectType);
            Color bColor = Gravure.Data.ColorTable.GetColor(defectType);
            UpdateControlColor(control, fColor, bColor);
            //UpdateControlColor(control, bColor, fColor);
        }

        public delegate void UpdateControlColorDelegate(Control checkBox, Color fColor, Color bColor);
        public static void UpdateControlColor(Control control, Color fColor, Color bColor)
        {
            if(control.InvokeRequired)
            {
                control.Invoke(new UpdateControlColorDelegate(UpdateControlColor), control, fColor, bColor);
                return;
            }

            if (fColor.IsEmpty == false)
                control.ForeColor = fColor;

            if (bColor.IsEmpty == false)
                control.BackColor = bColor;
        }
    }

    public class DefectObj : FoundedObjInPattern
    {
        public enum EPositionType { None = 0, Pole = 1, Dielectric = 2, Edge = 4, Sheet = 8 }
        public enum EValueType { None = -1, Bright, Dark }
        public enum EShapeType { None = -1, Circular, Linear, Linear2, Complex }

        private new enum ExportOrder { Base, DefectTypePos, DefectTypeValue, DefectTypeShape, ValueDiff, SubtractValueMax, FillRate, IMPORT_MAX, DefectType, MAX_COUNT }

        public override bool IsDefect => GetDefectType() != DefectType.Unknown;

        public float FillRate { get => this.fillRate; set => this.fillRate = value; }
        protected float fillRate;

        public bool IsValid { get => this.isValid; set => this.isValid = value; }
        bool isValid = true;

        public RectangleF AvgIntensity { get => this.avgIntensity; set => this.avgIntensity = value; }
        RectangleF avgIntensity;

        public float ValueDiff { get => this.valueDiff; set => this.valueDiff = value; }
        float valueDiff = -1;

        public int SubtractValueMax { get => this.subtractValueMax; set => this.subtractValueMax = value; }
        int subtractValueMax = -1;

        public EPositionType PositionType { get => this.positionType; set => this.positionType = value; }
        EPositionType positionType = EPositionType.None;

        public EValueType ValueType { get => this.valueType; set => this.valueType = value; }
        EValueType valueType = EValueType.None;

        public EShapeType ShapeType { get => this.shapeType; set => this.shapeType = value; }
        EShapeType shapeType = EShapeType.None;

        public bool IsCoatingDark => this.positionType == EPositionType.Dielectric && this.valueType == EValueType.Dark;

        public DefectObj(bool isOldData = false) : base(ObjType.Defect, isOldData)
        {

        }

        public void Rescale(float scale)
        {
            this.region = DrawingHelper.Mul(this.region, scale);
            this.realRegion = DrawingHelper.Mul(this.realRegion, scale);
        }

        public static new string GetExportHeader()
        {
            List<string> headerItemList = new List<string>();

            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base:
                        headerItemList.Add(FoundedObjInPattern.GetExportHeader());
                        break;
                    case ExportOrder.IMPORT_MAX:
                        continue;
                    case ExportOrder.MAX_COUNT:
                        break;
                    default:
                        headerItemList.Add(e.ToString());
                        break;
                }
            }

            return string.Join(",", headerItemList.ToArray()).Trim(',');
        }

        public override string ToExportData()
        {
            string[] exportArray = new string[(int)ExportOrder.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base: exportArray[(int)ExportOrder.Base] = base.ToExportData(); break;
                    case ExportOrder.DefectTypePos: exportArray[(int)ExportOrder.DefectTypePos] = this.positionType.ToString(); break;
                    case ExportOrder.DefectTypeValue: exportArray[(int)ExportOrder.DefectTypeValue] = this.valueType.ToString(); break;
                    case ExportOrder.DefectTypeShape: exportArray[(int)ExportOrder.DefectTypeShape] = this.shapeType.ToString(); break;
                    case ExportOrder.ValueDiff: exportArray[(int)ExportOrder.ValueDiff] = this.valueDiff.ToString(); break;
                    case ExportOrder.SubtractValueMax: exportArray[(int)ExportOrder.SubtractValueMax] = this.subtractValueMax.ToString(); break;
                    case ExportOrder.FillRate: exportArray[(int)ExportOrder.FillRate] = this.fillRate.ToString(); break;
                    case ExportOrder.IMPORT_MAX: break;
                    case ExportOrder.DefectType: exportArray[(int)ExportOrder.DefectType] = this.GetDefectType().ToString(); break;
                }
            }

            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        public override void FromExportData(string[] tokens)
        {
            int length = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT + (int)ExportOrder.IMPORT_MAX - 1;

            if (tokens.Length < length)
                this.isOldData = true;

            base.FromExportData(tokens.ToArray());

            if (this.isOldData)
            {
                FromExportDataOld(tokens);
                return;
            }

            int baseCount = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT;
            string[] realTokens = tokens.Skip(baseCount - 1).ToArray();
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                if ((ExportOrder)e == ExportOrder.IMPORT_MAX)
                    break;

                switch (e)
                {
                    case ExportOrder.DefectTypePos:
                        this.positionType = (EPositionType)Enum.Parse(typeof(EPositionType), realTokens[(int)ExportOrder.DefectTypePos]);
                        break;
                    case ExportOrder.DefectTypeValue:
                        this.valueType = (EValueType)Enum.Parse(typeof(EValueType), realTokens[(int)ExportOrder.DefectTypeValue]);
                        break;
                    case ExportOrder.DefectTypeShape:
                        this.shapeType = (EShapeType)Enum.Parse(typeof(EShapeType), realTokens[(int)ExportOrder.DefectTypeShape]);
                        break;
                    case ExportOrder.ValueDiff:
                        this.valueDiff = Convert.ToSingle(realTokens[(int)ExportOrder.ValueDiff]);
                        break;
                    case ExportOrder.SubtractValueMax:
                        this.subtractValueMax = (int)Convert.ToSingle(realTokens[(int)ExportOrder.SubtractValueMax]);
                        break;
                    case ExportOrder.FillRate:
                        this.fillRate = (int)Convert.ToSingle(realTokens[(int)ExportOrder.FillRate]);
                        break;
                }
            }
        }

        private void FromExportDataOld(string[] tokens)
        {
            this.positionType = (EPositionType)Enum.Parse(typeof(EPositionType), tokens[2]);
            this.valueType = (EValueType)Enum.Parse(typeof(EValueType), tokens[3]);
            this.shapeType = (EShapeType)Enum.Parse(typeof(EShapeType), tokens[4]);

            this.valueDiff = Convert.ToSingle(tokens[16]);
            this.subtractValueMax = (int)Convert.ToSingle(tokens[17]);
            this.fillRate = (int)Convert.ToSingle(tokens[18]);
        }

        public override string ToString()
        {
            List<string> stringList = new List<string>();
            //sb.AppendLine(this.GetDefectTypeName());
            stringList.Add(string.Format("IDX: {0}", this.index));
            stringList.Add(string.Format("POS: X{0:F2} / Y{1:F2} [mm]", realRegion.X / 1000.0f, realRegion.Y / 1000.0f));
            stringList.Add(string.Format("SIZE: W{0:F1} / H{1:F1} / L{2:F2} [um]", realRegion.Width, realRegion.Height, this.RealLength));
            if (avgIntensity.IsEmpty == false)
            {
                stringList.Add(string.Format("IL: {0:0.00}, IR: {1:0.00} [Lv]", avgIntensity.Left, avgIntensity.Right));
                stringList.Add(string.Format("IT: {0:0.00}, IB: {1:0.00} [Lv]", avgIntensity.Top, avgIntensity.Bottom));
            }

            stringList.Add(string.Format("V: {0} [Lv], D: {1:F1} [Lv], R: {2:F1} [%]", subtractValueMax, valueDiff, fillRate));
            return string.Join(Environment.NewLine, stringList);
        }

        protected override Figure GetShape(float ratio)
        {
            RectangleF region = DrawingHelper.Mul(this.region, ratio);
            Color color = GetColor();

            Pen pen = new Pen(color, 10);
            RectangleFigure rectangleFigure = new RectangleFigure(RectangleF.Inflate(region, 15, 15), pen);
            return rectangleFigure;
        }

        public override DefectType GetDefectType()
        {
            //public enum PositionType { Pole, Dielectric }
            //public enum ValueType { Bright, Dark }
            //public enum ShapeType { Circular, Linear }

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(positionType.ToString());
            //sb.AppendLine(valueType.ToString());
            //return sb.ToString();

            float maxRect = Math.Max(this.region.Width, this.region.Height);
            float minRect = Math.Min(this.region.Width, this.region.Height);
            if (this.isValid == false)
                return DefectType.Unknown;

            if (positionType == EPositionType.Sheet)
                return DefectType.Sticker;

            if (shapeType == EShapeType.Complex)
                return DefectType.Attack;

            int idx = 0;
            idx |= (int)positionType << 8;
            idx |= (int)valueType << 4;
            idx |= (int)shapeType << 0;

            switch (idx)
            {
                case 0x100: // 전극-명-원
                case 0x101: // 전극-명-선
                case 0x500: // 전극엣지-명-원
                case 0x501: // 전극엣지-명-선
                    return DefectType.Noprint;
                case 0x110: // 전극-암-원
                case 0x111: // 전극-암-선
                    return DefectType.Attack;
                case 0x200: // 성형-명-원
                case 0x201: // 성형-명-선
                    return DefectType.Coating;
                case 0x210: // 성형-암-원
                case 0x211: // 성형-암-선
                case 0x610: // 성형엣지-암-원
                case 0x611: // 성형엣지-암-선
                    return DefectType.PinHole;
                case 0x212: // 성형-암-선2
                case 0x612: // 성형엣지-암-선2
                    return DefectType.Spread;
                default:
                    return DefectType.Unknown;
            }
        }

        public void Offset(Point point, SizeF pelSize)
        {
            region.Offset(point);

            PointF realOffset = new PointF(point.X * pelSize.Width, point.Y * pelSize.Height);
            realRegion.Offset(realOffset);
        }

        public override string GetInfoString()
        {
            return string.Format("{0}", this.SubtractValueMax); // sheetSubResult.ToString();
        }

        //public override Color GetColor()
        //{
        //    return ColorTable.GetColor(this.GetDefectType());
        //}

        //public override Color GetBgColor()
        //{
        //    return ColorTable.GetBgColor(this.GetDefectType());
        //}

        public override string GetDefectTypeDiscription()
        {
            List<string> tokenList = new List<string>();

            if (positionType != EPositionType.None)
            {
                if (positionType.HasFlag(EPositionType.Pole))
                    tokenList.Add(StringManager.GetString(this.GetType().FullName, EPositionType.Pole.ToString()));

                if (positionType.HasFlag(EPositionType.Dielectric))
                    tokenList.Add(StringManager.GetString(this.GetType().FullName, EPositionType.Dielectric.ToString()));

                if (positionType.HasFlag(EPositionType.Edge))
                    tokenList.Add(StringManager.GetString(this.GetType().FullName, EPositionType.Edge.ToString()));
            }

            if (shapeType != EShapeType.None)
                tokenList.Add(StringManager.GetString(this.GetType().FullName, shapeType.ToString()));

            if (valueType != EValueType.None)
                tokenList.Add(StringManager.GetString(this.GetType().FullName, valueType.ToString()));

            tokenList.RemoveAll(f => f == null);

            return string.Join(",", tokenList);
        }
    }

    public class OffsetObj : FoundedObjInPattern
    {
        private new enum ExportOrder { Base, Name, RefPosition, OffsetPx, MatchOffsetPx, MatchOffsetUm, Score, MAX_COUNT }

        public override bool ShowReport => !UniScanG.Common.Settings.SystemTypeSettings.Instance().ShowSimpleReportLotList;

        public string Name => string.Format("C{0}_{1}", this.camIndex, this.name);
        string name;

        public PointF RefPoint => this.refPoint;
        PointF refPoint;

        public PointF OffsetPx => this.offsetPx;
        PointF offsetPx;

        public PointF AdjPointPx => DrawingHelper.CenterPoint(this.region);

        public PointF AdjPointUm => DrawingHelper.CenterPoint(this.realRegion);

        public SizeF MatchingOffsetPx => this.matchingOffsetPx;
        SizeF matchingOffsetPx;

        public SizeF MatchingOffsetUm => this.matchingOffsetUm;
        SizeF matchingOffsetUm;

        public override bool IsDefect => this.isDefect;
        bool isDefect;

        public float Score => this.score;
        float score;

        public override float RealWidth => matchingOffsetUm.Width;
        public override float RealHeight => matchingOffsetUm.Height;
        public override float RealLength => MathHelper.GetLength(this.matchingOffsetUm);

        public static new string GetExportHeader()
        {
            List<string> headerItemList = new List<string>();

            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base:
                        headerItemList.Add(FoundedObjInPattern.GetExportHeader());
                        break;
                    case ExportOrder.MAX_COUNT:
                        break;
                    default:
                        headerItemList.Add(e.ToString());
                        break;
                }
            }

            return string.Join(",", headerItemList.ToArray()).Trim(',');
        }

        public static Figure GetFigure(float ratio, List<FoundedObjInPattern> list)
        {
            FigureGroup figureGroup = new FigureGroup();

            List<OffsetObj> list2 = list.Cast<OffsetObj>().ToList();
            if (list2.Count == 0)
                return figureGroup;

            List<float> distance = list2.ConvertAll(f => MathHelper.GetLength(f.matchingOffsetPx));
            float mean = distance.Average();
            //float stdVar = (float)Math.Sqrt(distance.Aggregate(0.0f, (f, g) => f + (float)Math.Pow(g - mean, 2)) / distance.Count);
            //if(stdVar==0)
            //{

            //}
            //List<float> zScore = distance.ConvertAll(f => (f - mean) / stdVar);

            //return figureGroup;

            // center
            PointF centerPoint = list2[0].refPoint;
            RectangleF centerRect = DrawingHelper.FromCenterSize(centerPoint, new SizeF(3 / ratio, 3 / ratio));
            figureGroup.AddFigure(new RectangleFigure(centerRect, new Pen(Color.LightGreen, 1)));

            // min
            float minDist = distance.Min() * 2;
            RectangleF minRect = DrawingHelper.FromCenterSize(centerPoint, new SizeF(minDist, minDist));
            figureGroup.AddFigure(new EllipseFigure(minRect, new Pen(Color.YellowGreen, 1)));

            // average
            float meanDist = mean * 2;
            RectangleF meanRect = DrawingHelper.FromCenterSize(centerPoint, new SizeF(meanDist, meanDist));
            figureGroup.AddFigure(new EllipseFigure(meanRect, new Pen(Color.YellowGreen, 1)));

            // max
            float maxDist = distance.Max() * 2;
            RectangleF maxRect = DrawingHelper.FromCenterSize(centerPoint, new SizeF(maxDist, maxDist));
            figureGroup.AddFigure(new EllipseFigure(maxRect, new Pen(Color.Red, 1)));

            // each
            SizeF matchSize = new SizeF(1 / ratio, 1 / ratio);
            list2.ForEach(f =>
            {
                PointF matchPoint = PointF.Add(centerPoint, f.matchingOffsetPx);
                RectangleF matchRect = DrawingHelper.FromCenterSize(matchPoint, matchSize);
                Color matchColor = f.GetColor();
                figureGroup.AddFigure(new RectangleFigure(matchRect, new Pen(matchColor, 1)));
            });

            figureGroup.Scale(ratio);
            return figureGroup;
        }

        public OffsetObj() : base(ObjType.Offset, false)
        {
            this.name = "";
            this.region = Rectangle.Empty;
            this.realRegion = RectangleF.Empty;
        }

        public void Set(string name, PointF refPoint, PointF offsetPx, SizeF matchingOffsetPx, SizeF matchingOffsetUm, float score, bool isDefect, Bitmap bitmap)
        {
            this.name = name;
            this.refPoint = refPoint;
            this.offsetPx = offsetPx;
            this.matchingOffsetPx = matchingOffsetPx;
            this.matchingOffsetUm = matchingOffsetUm;
            this.score = score;
            this.isDefect = isDefect;
            this.image = bitmap;
        }

        public void Clear()
        {
            this.refPoint = PointF.Empty;
            this.offsetPx = PointF.Empty;
            this.matchingOffsetPx = SizeF.Empty;
            this.matchingOffsetUm = SizeF.Empty;
            this.image?.Dispose();
            this.isDefect = false;
            this.image = null;
        }

        public void Dispose()
        {
            Clear();
        }

        public override void Offset(int x, int y, SizeF umPpx)
        {
            base.Offset(x, y, umPpx);
            this.refPoint = PointF.Add(this.refPoint, new Size(x, y));
        }

        public override string ToExportData()
        {
            string[] exportArray = new string[(int)ExportOrder.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base: exportArray[(int)ExportOrder.Base] = base.ToExportData(); break;
                    case ExportOrder.Name: exportArray[(int)ExportOrder.Name] = this.name; break;
                    case ExportOrder.RefPosition: exportArray[(int)ExportOrder.RefPosition] = string.Format("{0}/{1}", this.refPoint.X, this.refPoint.Y); break;
                    case ExportOrder.OffsetPx: exportArray[(int)ExportOrder.OffsetPx] = string.Format("{0}/{1}", this.offsetPx.X, this.offsetPx.Y); break;
                    case ExportOrder.MatchOffsetPx: exportArray[(int)ExportOrder.MatchOffsetPx] = string.Format("{0}/{1}", this.matchingOffsetPx.Width, this.matchingOffsetPx.Height); break;
                    case ExportOrder.MatchOffsetUm: exportArray[(int)ExportOrder.MatchOffsetUm] = string.Format("{0}/{1}", this.matchingOffsetUm.Width, this.matchingOffsetUm.Height); break;
                    case ExportOrder.Score: exportArray[(int)ExportOrder.Score] = this.score.ToString(); break;
                }
            }

            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        public override void FromExportData(string[] tokens)
        {
            int length = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT + (int)ExportOrder.MAX_COUNT - 1;
            if (tokens.Length < length)
                throw new Exception();

            Array array = Enum.GetValues(typeof(ExportOrder));

            int baseCount = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT;
            base.FromExportData(tokens.ToArray());

            string name = "";
            PointF refPoint = offsetPx = PointF.Empty;
            SizeF matchingOffsetPx = matchingOffsetUm = SizeF.Empty;
            float score = 0;

            string[] realTokens = tokens.Skip(baseCount - 1).ToArray();
            string[] subTokens;
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Name:
                        name = realTokens[(int)ExportOrder.Name];
                        break;
                    case ExportOrder.RefPosition:
                        subTokens = realTokens[(int)ExportOrder.RefPosition].Split('/');
                        refPoint = new PointF(float.Parse(subTokens[0]), float.Parse(subTokens[1]));
                        break;
                    case ExportOrder.OffsetPx:
                        subTokens = realTokens[(int)ExportOrder.OffsetPx].Split('/');
                        offsetPx = new PointF(float.Parse(subTokens[0]), float.Parse(subTokens[1]));
                        break;
                    case ExportOrder.MatchOffsetPx:
                        subTokens = realTokens[(int)ExportOrder.MatchOffsetPx].Split('/');
                        matchingOffsetPx = new SizeF(float.Parse(subTokens[0]), float.Parse(subTokens[1]));
                        break;
                    case ExportOrder.MatchOffsetUm:
                        subTokens = realTokens[(int)ExportOrder.MatchOffsetUm].Split('/');
                        matchingOffsetUm = new SizeF(float.Parse(subTokens[0]), float.Parse(subTokens[1]));
                        break;
                    case ExportOrder.Score:
                        score = (int)Convert.ToSingle(realTokens[(int)ExportOrder.Score]);
                        break;
                }
            }

            Set(name, refPoint, offsetPx, matchingOffsetPx, matchingOffsetUm, score, score == 0, null);
        }

        public override string ToString()
        {
            List<string> stringList = new List<string>();
            PointF centerPt = DrawingHelper.CenterPoint(this.realRegion);
            stringList.Add(string.Format("POSITION: {0:F2} / {1:F2} [mm]", centerPt.X / 1000.0f, centerPt.Y / 1000.0f));
            stringList.Add(string.Format("OFFSET: {0:F1} / {1:F1} [um]", this.matchingOffsetUm.Width, this.matchingOffsetUm.Height));
            stringList.Add(string.Format("SCORE: {0:F1} [%]", this.score));
            return string.Join(Environment.NewLine, stringList.ToArray());
        }

        protected override Figure GetShape(float scale)
        {
            FigureGroup figureGroup = new FigureGroup();

            // 탐색영역
            RectangleF region = DrawingHelper.FromCenterSize(this.AdjPointPx, this.region.Size);
            figureGroup.AddFigure(new RectangleFigure(region, new Pen(Color.LightGray, 1)));

            // 기준 위치
            RectangleF refRect = DrawingHelper.FromCenterSize(this.AdjPointPx, DrawingHelper.Div(this.region.Size, 3));
            figureGroup.AddFigure(new RectangleFigure(refRect, new Pen(Color.Yellow, 1)));

            // 검출 된 지정
            Color centerColor = this.isDefect ? Color.Red : Color.LightGreen;
            PointF foundPt = PointF.Add(this.AdjPointPx, this.matchingOffsetPx);
            SizeF foundSIze = DrawingHelper.Div(this.region, 3).Size;
            RectangleF foundRect = DrawingHelper.FromCenterSize(foundPt, foundSIze);
            figureGroup.AddFigure(new RectangleFigure(foundRect, new Pen(centerColor, 1)));

            //if (this.score > 0)
            {
                Color color = GetColor();

                // 검출 된 점. 선으로 그음
                PointF srcPt = DrawingHelper.CenterPoint(this.region);
                LineFigure lineFigure = new LineFigure(srcPt, PointF.Add(srcPt, this.matchingOffsetPx), new Pen(color, 1));
                figureGroup.AddFigure(lineFigure);

                // 이건 왜?
                //EllipseFigure ellipseFigure = new EllipseFigure(this.region, new Pen(Color.Yellow, 1));
                //figureGroup.AddFigure(ellipseFigure);
            }

            figureGroup.Scale(scale);

            return figureGroup;
        }

        public override DefectType GetDefectType()
        {
            return DefectType.Transform;
        }

        //public override string GetPositionString()
        //{
        //    PointF centerPt = DrawingHelper.CenterPoint(this.realRegion);
        //    return string.Format("X{0:0.0}{1}Y{2:0.0}",
        //        centerPt.X / 1000, Environment.NewLine, centerPt.Y / 1000);
        //}

        public override string GetSizeString()
        {
            string[] strings = new string[]
            {
                string.Format("dX{0:F01}",this.matchingOffsetUm.Width),
                string.Format("dY{0:F01}",this.matchingOffsetUm.Height)
            };
            return string.Join(Environment.NewLine, strings);
        }

        public override string GetInfoString()
        {
            return this.score.ToString("F01");
        }

        //public override Color GetColor()
        //{
        //    float minOffset = 100;
        //    float offset = Math.Min(minOffset, MathHelper.GetLength(this.matchingOffsetPx));
        //    byte r = (byte)(144 + ((255 - 144) * offset / minOffset));
        //    byte g = (byte)(238 + ((0 - 238) * offset / minOffset));
        //    byte b = (byte)(144 + ((0 - 144) * offset / minOffset));
        //    Color color = Color.FromArgb(r, g, b);
        //    return color;
        //}

        //public override Color GetBgColor()
        //{
        //    return Color.DeepPink;
        //}

        public override string GetDefectTypeDiscription()
        {
            return "PmAlign";
        }
    }

    public class MarginObj : FoundedObjInPattern
    {
        private new enum ExportOrder { Base, Name, Width, Height, WidthUm, HeightUm, PosType, DiffWidthUm, DiffHeigthUm, IsDefect, MAX_COUNT }


        public string Name { get => this.name; set => this.name = value; }
        string name;

        public SizeF MarginSize { get => this.marginSize; set => this.marginSize = value; }
        SizeF marginSize;

        public SizeF MarginSizeUm { get => this.marginSizeUm; set => this.marginSizeUm = value; }
        SizeF marginSizeUm;

        public Margin.EMarginPos MarginPos { get => this.marginPos; set => this.marginPos = value; }
        Margin.EMarginPos marginPos;

        public SizeF DiffMarginSizeUm { get => this.diffMarginSizeUm; set => this.diffMarginSizeUm = value; }
        SizeF diffMarginSizeUm;

        public override bool IsDefect => this.isDefect;
        bool isDefect;

        public override float RealWidth => MarginSizeUm.Width;
        public override float RealHeight => MarginSizeUm.Height;
        public override float RealLength => (Math.Abs(this.marginSizeUm.Width) > Math.Abs(this.marginSizeUm.Height) ? this.marginSizeUm.Width : this.marginSizeUm.Height);

        public static new string GetExportHeader()
        {
            List<string> headerItemList = new List<string>();

            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base:
                        headerItemList.Add(FoundedObjInPattern.GetExportHeader());
                        break;
                    case ExportOrder.MAX_COUNT:
                        break;
                    default:
                        headerItemList.Add(e.ToString());
                        break;
                }
            }

            return string.Join(",", headerItemList.ToArray()).Trim(',');
        }

        public MarginObj(bool isOldData = false) : base(ObjType.Margin, isOldData)
        {
            this.marginSize = Size.Empty;
            this.marginSizeUm = SizeF.Empty;
            this.marginPos = Margin.EMarginPos.CUSTOM;
            this.diffMarginSizeUm = SizeF.Empty;
            this.isDefect = false;
        }

        public override string GetPositionString()
        {
            return GetDisplayName();
        }

        //public override 
        public int GetDisplayIndex()
        {
            int index;
            switch (this.marginPos)
            {
                case Margin.EMarginPos.CM:
                    index = (this.camIndex == 0 ? 0 : 5);
                    break;
                case Margin.EMarginPos.ST:
                    index = (this.camIndex == 0 ? 1 : 2);
                    break;
                case Margin.EMarginPos.SB:
                    index = (this.camIndex == 0 ? 3 : 4);
                    break;
                default:
                    index = 6;
                    break;
            }
            return index;
        }

        public string GetDisplayName()
        {
            string displayName;
            switch (this.marginPos)
            {
                case Margin.EMarginPos.CM:
                    displayName = (this.camIndex == 0 ? "CM(1)" : "");
                    break;
                case Margin.EMarginPos.ST:
                    displayName = (this.camIndex == 0 ? "LT(2)" : "RT(3)");
                    break;
                case Margin.EMarginPos.SB:
                    displayName = (this.camIndex == 0 ? "LB(4)" : "RB(5)");
                    break;
                default:
                    displayName = null;
                    break;
            }

            if (string.IsNullOrEmpty(displayName))
                displayName = string.Format("C{0}_{1}", this.camIndex, this.name);

            return displayName;
        }

        public void SetDefect(bool isDefect)
        {
            LogHelper.Debug(LoggerType.Inspection, $"MarginObj::SetDefect - {isDefect}");
            this.isDefect = isDefect;
        }

        //public override Color GetBgColor()
        //{
        //    return Control.DefaultBackColor;
        //}

        //public override Color GetColor()
        //{
        //    return ColorTable.GetColor(this.GetDefectType());
        //}

        public override DefectType GetDefectType()
        {
            return DefectType.Margin;
        }

        public override string GetDefectTypeDiscription()
        {
            return "Margin";
        }

        public override string GetSizeString()
        {
            string[] strings = new string[]
            {
                string.Format("W{0:F01}",this.marginSizeUm.Width),
                string.Format("H{0:F01}",this.marginSizeUm.Height),
            };

            return string.Join(Environment.NewLine, strings);
        }

        public override string GetInfoString()
        {
            string[] strings = new string[]
            {
                string.Format("W{0:F01}",this.diffMarginSizeUm.Width),
                string.Format("H{0:F01}",this.diffMarginSizeUm.Height),
            };
            return string.Join(Environment.NewLine, strings);
        }

        protected override Figure GetShape(float ratio)
        {
            FigureGroup figureGroup = new FigureGroup();

            RectangleF rectangle1 = this.region;
            Color penColor1 = Color.Yellow;
            RectangleFigure rectangleFigure1 = new RectangleFigure(rectangle1, new Pen(penColor1, 1));

            RectangleF rectangle2 = RectangleF.Inflate(this.region, this.marginSize.Width, this.marginSize.Height);
            Color penColor2 = Color.Yellow;
            RectangleFigure rectangleFigure2 = new RectangleFigure(rectangle2, new Pen(penColor2, 1));

            figureGroup.AddFigure(rectangleFigure1);
            figureGroup.AddFigure(rectangleFigure2);

            if (this.marginPos != Margin.EMarginPos.CUSTOM)
            {
                string text = GetPositionString();
                PointF centerPosition = DrawingHelper.CenterPoint(this.region);
                PointF position = this.region.Location;
                StringAlignment stringAlignmentH = StringAlignment.Near;
                switch (this.marginPos)
                {
                    case Margin.EMarginPos.SB:
                    case Margin.EMarginPos.ST:
                        position = new PointF((this.camIndex > 0 ? this.region.Left : this.region.Right), centerPosition.Y);
                        stringAlignmentH = (this.camIndex > 0 ? StringAlignment.Far : StringAlignment.Near);
                        break;

                    case Margin.EMarginPos.CM:
                        position = new PointF((this.camIndex > 0 ? this.region.Right : this.region.Left), centerPosition.Y);
                        stringAlignmentH = (this.camIndex > 0 ? StringAlignment.Near : StringAlignment.Far);
                        break;
                }

                Font font = new Font("맑은 고딕", 1400);
                Color color = this.isDefect ? Color.Red : Color.LightGreen;
                TextFigure textFigure = new TextFigure(text, position, font, color, stringAlignmentH);
                //figureGroup.AddFigure(textFigure);
            }

            figureGroup.Scale(ratio);
            return figureGroup;
        }

        public override string ToString()
        {
            List<string> stringList = new List<string>();
            stringList.Add(string.Format("POS: {0:F2} / {1:F2} [mm]", realRegion.X / 1000.0f, realRegion.Y / 1000.0f));
            stringList.Add(string.Format("MARGIN: {0:F1} / {1:F1} [um]", this.marginSizeUm.Width, this.marginSizeUm.Height));
            return string.Join(Environment.NewLine, stringList);
        }

        public override string ToExportData()
        {
            string[] exportArray = new string[(int)ExportOrder.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base: exportArray[(int)ExportOrder.Base] = base.ToExportData(); break;
                    case ExportOrder.Name: exportArray[(int)ExportOrder.Name] = this.name; break;
                    case ExportOrder.Width: exportArray[(int)ExportOrder.Width] = this.marginSize.Width.ToString(); break;
                    case ExportOrder.Height: exportArray[(int)ExportOrder.Height] = this.marginSize.Height.ToString(); break;
                    case ExportOrder.WidthUm: exportArray[(int)ExportOrder.WidthUm] = this.marginSizeUm.Width.ToString(); break;
                    case ExportOrder.HeightUm: exportArray[(int)ExportOrder.HeightUm] = this.marginSizeUm.Height.ToString(); break;
                    case ExportOrder.PosType: exportArray[(int)ExportOrder.PosType] = this.marginPos.ToString(); break;
                    case ExportOrder.DiffWidthUm: exportArray[(int)ExportOrder.DiffWidthUm] = this.diffMarginSizeUm.Width.ToString(); break;
                    case ExportOrder.DiffHeigthUm: exportArray[(int)ExportOrder.DiffHeigthUm] = this.diffMarginSizeUm.Height.ToString(); break;
                    case ExportOrder.IsDefect: exportArray[(int)ExportOrder.IsDefect] = this.isDefect.ToString(); break;
                }
            }

            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        public override void FromExportData(string[] tokens)
        {
            int length = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT + (int)ExportOrder.MAX_COUNT - 1;

            base.FromExportData(tokens.ToArray());

            int baseCount = (int)FoundedObjInPattern.ExportOrder.MAX_COUNT;
            string[] realTokens = tokens.Skip(baseCount - 1).ToArray();
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                if (realTokens.Length <= (int)((ExportOrder)e))
                    continue;

                switch (e)
                {
                    case ExportOrder.Name:
                        this.name = realTokens[(int)ExportOrder.Name];
                        bool defined = Enum.IsDefined(typeof(Margin.EMarginPos), this.name);
                        if (defined)
                            this.marginPos = (Margin.EMarginPos)Enum.Parse(typeof(Margin.EMarginPos), this.name);
                        break;
                    case ExportOrder.Width:
                        this.marginSize.Width = float.Parse(realTokens[(int)ExportOrder.Width]);
                        break;
                    case ExportOrder.Height:
                        this.marginSize.Height = float.Parse(realTokens[(int)ExportOrder.Height]);
                        break;
                    case ExportOrder.WidthUm:
                        this.marginSizeUm.Width = float.Parse(realTokens[(int)ExportOrder.WidthUm]);
                        break;
                    case ExportOrder.HeightUm:
                        this.marginSizeUm.Height = float.Parse(realTokens[(int)ExportOrder.HeightUm]);
                        break;
                    case ExportOrder.PosType:
                        this.marginPos = (Margin.EMarginPos)Enum.Parse(typeof(Margin.EMarginPos), realTokens[(int)ExportOrder.PosType]);
                        break;
                    case ExportOrder.DiffWidthUm:
                        this.diffMarginSizeUm.Width = float.Parse(realTokens[(int)ExportOrder.DiffWidthUm]);
                        break;
                    case ExportOrder.DiffHeigthUm:
                        this.diffMarginSizeUm.Height = float.Parse(realTokens[(int)ExportOrder.DiffHeigthUm]);
                        break;
                    case ExportOrder.IsDefect:
                        this.isDefect = bool.Parse(realTokens[(int)ExportOrder.IsDefect]);
                        break;
                }
            }
        }

        public override int CompareTo(object obj)
        {
            MarginObj marginObj = obj as MarginObj;
            if (marginObj == null)
                return base.CompareTo(obj);

            return this.GetDisplayIndex().CompareTo(marginObj.GetDisplayIndex());
        }
    }
}
