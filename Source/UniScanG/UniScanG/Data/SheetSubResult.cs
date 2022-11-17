using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Vision;
using UniScanG.Vision;

namespace UniScanG.Data
{
    public enum ObjType { Defect, Offset, Margin}

    interface IFoundedObj
    {

    }

    public abstract class FoundedObjInPattern : IFoundedObj, IComparable
    {
        protected enum ExportOrder { Type, CamIndex, ImageIndex, X, RealX, Y, RealY, W, RealW, H, RealH, MAX_COUNT } //13

        protected bool isOldData = false;

        public abstract bool IsDefect { get; }

        public virtual bool ShowReport => true;

        public ObjType ObjType => this.objType;
        private ObjType objType;

        public int CamIndex { get => this.camIndex; set => this.camIndex = value; }
        protected int camIndex;

        public int Index { get => this.index; set => this.index = value; }
        protected int index;

        public RectangleF Region { get => this.region; set => this.region = value; }
        protected RectangleF region;

        public RectangleF RealRegion { get => this.realRegion; set => this.realRegion = value; }
        protected RectangleF realRegion;

        public float Length => MathHelper.GetLength(this.region.Size);

        public float PostProcessSize
        {
            get
            {
                switch (Gravure.Vision.AlgorithmSetting.Instance().DetectorParam.CriterionLength)
                {
                    case Gravure.Vision.Detector.DetectorParam.ECriterionLength.Min:
                        return Math.Min(RealWidth, RealHeight);
                    case Gravure.Vision.Detector.DetectorParam.ECriterionLength.Max:
                        return Math.Max(RealWidth, RealHeight);
                    default:
                    case Gravure.Vision.Detector.DetectorParam.ECriterionLength.Diagonal:
                        return RealLength;
                }
            }
        }

        public virtual float RealWidth => this.realRegion.Width;
        public virtual float RealHeight => this.realRegion.Height;
        public virtual float RealLength => MathHelper.GetLength(this.realRegion.Size);

        protected string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        protected Bitmap image;
        public Bitmap Image
        {
            get
            {
                if (image == null)
                {
                    if (File.Exists(this.imagePath))
                        image = (Bitmap)ImageHelper.LoadImage(this.imagePath);
                }
                return image;
            }
            set { image = value; }
        }

        private string bufImagePath;
        public string BufImagePath
        {
            get { return bufImagePath; }
            set { bufImagePath = value; }
        }

        public Bitmap BufImage
        {
            get
            {
                if (bufImage == null)
                {
                    if (File.Exists(this.bufImagePath))
                        bufImage = (Bitmap)ImageHelper.LoadImage(this.bufImagePath);
                }
                return bufImage;
            }
            set { bufImage = value; }
        }
        protected Bitmap bufImage;

        public string ImageFileName => string.Format("{0}_{1}.jpg", this.objType, this.index);

        public static string GetExportHeader()
        {
            List<string> stringList = new List<string>();
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.MAX_COUNT:
                        break;
                    default:
                        stringList.Add(e.ToString());
                        break;
                }
            }
            return string.Join(",", stringList);
        }

        public static FoundedObjInPattern Create(string type)
        {
            if (type == "Type")
                return null;

            bool isDefined = Enum.IsDefined(typeof(ObjType), type);
            if (isDefined)
            {
                ObjType objType = (ObjType)Enum.Parse(typeof(ObjType), type);
                switch (objType)
                {
                    case ObjType.Defect:
                        return new Gravure.Data.DefectObj();
                    case ObjType.Offset:
                        return new Gravure.Data.OffsetObj();
                    case ObjType.Margin:
                        return new Gravure.Data.MarginObj();
                }
            }

            return new Gravure.Data.DefectObj(true);
        }

        public FoundedObjInPattern(ObjType objType, bool isOldData)
        {
            this.objType = objType;
            this.isOldData = isOldData;
        }

        public bool FromExportData(string line)
        {
            string[] tokens = line.Split(',');
            try
            {
                FromExportData(tokens);
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("FoundedObjInPattern::FromExportData - {0}", ex.Message));
                return false;
            }
        }

        public virtual string ToExportData()
        {
            string[] exportArray = new string[(int)ExportOrder.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Type: exportArray[(int)ExportOrder.Type] = this.objType.ToString(); break;
                    case ExportOrder.CamIndex: exportArray[(int)ExportOrder.CamIndex] = string.Format("{0}", camIndex.ToString()); break;
                    case ExportOrder.ImageIndex: exportArray[(int)ExportOrder.ImageIndex] = string.Format("{0}", index.ToString()); break;
                    case ExportOrder.X: exportArray[(int)ExportOrder.X] = string.Format("{0}", region.X); break;
                    case ExportOrder.RealX: exportArray[(int)ExportOrder.RealX] = string.Format("{0}", realRegion.X / 1000f); break;
                    case ExportOrder.Y: exportArray[(int)ExportOrder.Y] = string.Format("{0}", region.Y); break;
                    case ExportOrder.RealY: exportArray[(int)ExportOrder.RealY] = string.Format("{0}", realRegion.Y / 1000f); break;
                    case ExportOrder.W: exportArray[(int)ExportOrder.W] = string.Format("{0}", region.Width); break;
                    case ExportOrder.RealW: exportArray[(int)ExportOrder.RealW] = string.Format("{0}", realRegion.Width); break;
                    case ExportOrder.H: exportArray[(int)ExportOrder.H] = string.Format("{0}", region.Height); break;
                    case ExportOrder.RealH: exportArray[(int)ExportOrder.RealH] = string.Format("{0}", realRegion.Height); break;
                }
            }
            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        public virtual void FromExportData(string[] tokens)
        {
            if (tokens.Length < (int)ExportOrder.MAX_COUNT)
                throw new Exception();

            if(this.isOldData)
            {
                FromExportDataOld(tokens);
                return;
            }

            this.objType = (ObjType)Enum.Parse(typeof(ObjType), tokens[(int)ExportOrder.Type]);
            this.camIndex = Convert.ToInt32(tokens[(int)ExportOrder.CamIndex]);
            this.index = Convert.ToInt32(tokens[(int)ExportOrder.ImageIndex]);
            this.region.X = Convert.ToSingle(tokens[(int)ExportOrder.X]);
            this.realRegion.X = Convert.ToSingle(tokens[(int)ExportOrder.RealX]) * 1000.0f;
            this.region.Y = Convert.ToSingle(tokens[(int)ExportOrder.Y]);
            this.realRegion.Y = Convert.ToSingle(tokens[(int)ExportOrder.RealY]) * 1000.0f;
            this.region.Width = Convert.ToSingle(tokens[(int)ExportOrder.W]);
            this.realRegion.Width = Convert.ToSingle(tokens[(int)ExportOrder.RealW]);
            this.region.Height = Convert.ToSingle(tokens[(int)ExportOrder.H]);
            this.realRegion.Height = Convert.ToSingle(tokens[(int)ExportOrder.RealH]);

            if (this.region.Width < 0)
            {
                this.region = RectangleF.FromLTRB(this.region.Right, this.region.Top, this.region.Left, this.region.Bottom);
                this.realRegion = RectangleF.FromLTRB(this.realRegion.Right, this.realRegion.Top, this.realRegion.Left, this.realRegion.Bottom);
            }

            if (this.region.Height < 0)
            {
                this.region = RectangleF.FromLTRB(this.region.Left, this.region.Bottom, this.region.Right, this.region.Top);
                this.realRegion = RectangleF.FromLTRB(this.realRegion.Left, this.realRegion.Bottom, this.realRegion.Right, this.realRegion.Top);
            }
        }

        private void FromExportDataOld(string[] tokens)
        {
            this.objType = ObjType.Defect;
            this.camIndex = Convert.ToInt32(tokens[0]);
            this.index = Convert.ToInt32(tokens[1]);
            this.region.X = Convert.ToInt32(tokens[5]);
            this.realRegion.X = Convert.ToSingle(tokens[6]) * 1000.0f;
            this.region.Y = Convert.ToInt32(tokens[7]);
            this.realRegion.Y = Convert.ToSingle(tokens[8]) * 1000.0f;
            this.region.Width = Convert.ToInt32(tokens[9]);
            this.realRegion.Width = Convert.ToSingle(tokens[10]);
            this.region.Height = Convert.ToInt32(tokens[11]);
            this.realRegion.Height = Convert.ToSingle(tokens[12]);
        }

        public virtual void Offset(int x, int y, SizeF umPpx)
        {
            //resultRect.Offset(x, y);
            region.Offset(x, y);
            realRegion.Offset((float)x * umPpx.Width, (float)y * umPpx.Height);
        }

        public Figure GetFigure(float ratio = 1.0f)
        {
            Figure figure = GetShape(ratio);// region, new Pen(defectColor, width));
            figure.Tag = this;

            return figure;
        }

        public void ImportImage()
        {
            this.image = Image;
            this.bufImage = BufImage;
        }

        public virtual string GetPositionString()
        {
            PointF centerPt = DrawingHelper.CenterPoint(this.realRegion);
            return string.Format("X{0:0.0}{1}Y{2:0.0}",
                centerPt.X / 1000, Environment.NewLine, centerPt.Y / 1000);
        }

        public virtual string GetSizeString()
        {
            string[] strings = new string[]
            {
                string.Format("W{0:F01}",this.RealRegion.Width),
                string.Format("H{0:F01}",this.RealRegion.Height),
                string.Format("L{0:F01}",this.RealLength)
            };

            return string.Join(Environment.NewLine, strings);
        }

        public virtual Color GetColor() { return Gravure.Data.ColorTable.GetColor(this.GetDefectType()); }
        public virtual Color GetBgColor() { return Gravure.Data.ColorTable.GetBgColor(this.GetDefectType()); }

        protected abstract Figure GetShape(float ratio);
        public abstract DefectType GetDefectType();

        public abstract string GetInfoString();
        public abstract string GetDefectTypeDiscription();

        public virtual int CompareTo(object obj)
        {
            return index.CompareTo(obj);
        }
    }
}
