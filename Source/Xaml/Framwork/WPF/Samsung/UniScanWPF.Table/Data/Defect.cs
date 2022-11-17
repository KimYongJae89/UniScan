using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Data
{
    public enum DefectType { Lump, Pattern, CircularPattern, Margin, CircularMargin, Shape }

    public abstract class Defect: IResultObject
    {
        protected float drawingScale = 1;

        DefectType defectType;
        BitmapSource image;
        protected float diffValue;

        double[] rotateXArray;
        double[] rotateYArray;
        double rotateWidth;

        public string ImagePath { get; set; }

        public virtual double Length { get => this.rotateWidth * DeveloperSettings.Instance.Resolution; }
        public virtual float DiffValue { get => this.diffValue; }

        public BitmapSource Image
        {
            get
            {
                if (this.image == null && File.Exists(ImagePath))
                    this.image = WPFImageHelper.LoadBitmapSource(ImagePath);
                return this.image;
            }
        }

        public DefectType DefectType { get => defectType; }
        public float DrawingScale { get => drawingScale; }
        public string LocalizeDefectName => WpfControlLibrary.Helper.LocalizeHelper.GetString(typeof(DefectType).FullName, this.defectType.ToString());
        public virtual Enum ResultObjectType { get => this.defectType; }
        public System.Drawing.RectangleF BoundingRect => System.Drawing.RectangleF.FromLTRB(
                (float)this.rotateXArray.Min(), (float)this.rotateYArray.Min(), (float)this.rotateXArray.Max(), (float)this.rotateYArray.Max());

        protected Defect(DefectType defectType, BlobRect defectBlob, BitmapSource image, float diffValue)
        {
            this.defectType = defectType;
            this.image = image;
            this.drawingScale = 1;
            this.diffValue = diffValue;

            this.rotateXArray = Array.ConvertAll(defectBlob.RotateXArray, f => (double)f);
            this.rotateYArray = Array.ConvertAll(defectBlob.RotateYArray, f => (double)f);
            this.rotateWidth = defectBlob.RotateWidth;
        }

        protected Defect(DefectType defectType, BitmapSource image, XmlElement defectElement)
        {
            this.defectType = defectType;
            this.image = image;
            this.drawingScale = XmlHelper.GetValue(defectElement, "DrawingScale", this.drawingScale);
            this.diffValue = XmlHelper.GetValue(defectElement, "Diff", 0.0f);

            this.rotateXArray = new double[4];
            this.rotateYArray = new double[4];
            for (int i = 0; i < 4; i++)
            {
                this.rotateXArray[i] = XmlHelper.GetValue(defectElement, string.Format("DX{0}", i), 0.0);
                this.rotateYArray[i] = XmlHelper.GetValue(defectElement, string.Format("DY{0}", i), 0.0);
            }
            this.rotateWidth = XmlHelper.GetValue(defectElement, "RotateWidth", 0.0f);
        }

        public static IResultObject CreateDefect(string imagePath, XmlElement defectElement)
        {
            IResultObject obj = CreateDefect((BitmapSource)null, defectElement);
            if(obj is Defect)
            {
                Defect defect = (Defect)obj;
                defect.ImagePath = imagePath;
            }
            return obj;
        }

        public static IResultObject CreateDefect(BitmapSource image, XmlElement defectElement)
        {
            IResultObject defect = null;
            string typeString = XmlHelper.GetValue(defectElement, "Type", "");
            if (Enum.IsDefined(typeof(DefectType), typeString))
            {
                DefectType type = (DefectType)Enum.Parse(typeof(DefectType), typeString);

                switch (type)
                {
                    case DefectType.Lump:
                        defect = new LumpDefect(image, defectElement);
                        break;

                    case DefectType.CircularPattern:
                    case DefectType.Pattern:
                        defect = new PatternDefect(image, defectElement);
                        break;
                    case DefectType.CircularMargin:
                    case DefectType.Margin:
                        defect = new MarginDefect(image, defectElement);
                        break;
                    case DefectType.Shape:
                        defect = new ShapeDefect(image, defectElement);
                        break;
                    default:
                        return null;
                }
            }else if(Enum.IsDefined(typeof(MeasureType), typeString))
            {
                MeasureType type = (MeasureType)Enum.Parse(typeof(MeasureType), typeString);
                switch (type)
                {
                    case MeasureType.Length:
                        defect = new LengthMeasure(defectElement);
                        break;
                    case MeasureType.Extra:
                        defect = new ExtraMeasure(defectElement);
                        break;
                    case MeasureType.Meander:
                        defect = new MeanderMeasure(defectElement);
                        break;
                }
            }
            return defect;
        }

        public abstract System.Windows.Media.SolidColorBrush GetBrush();
        public static System.Windows.Media.SolidColorBrush GetBrush(DefectType defectType)
        {
            System.Windows.Media.SolidColorBrush brush = null;
            switch (defectType)
            {
                case DefectType.Pattern:
                    brush = System.Windows.Media.Brushes.Red;
                    break;
                case DefectType.Margin:
                    brush = System.Windows.Media.Brushes.Blue;
                    break;
                case DefectType.Shape:
                    brush = System.Windows.Media.Brushes.Yellow;
                    break;
                case DefectType.Lump:
                    brush = System.Windows.Media.Brushes.Pink;
                    break;
            }
            return brush;
        }

        public Point[] GetPoints(double scale)
        {
            scale /= this.drawingScale;
            return new Point[]                
            {
                new Point(this.rotateXArray[0] / scale, this.rotateYArray[0] / scale),
                new Point(this.rotateXArray[1] / scale, this.rotateYArray[1] / scale),
                new Point(this.rotateXArray[2] / scale, this.rotateYArray[2] / scale),
                new Point(this.rotateXArray[3] / scale, this.rotateYArray[3] / scale)
            };
        }

        public virtual void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DrawingScale", this.drawingScale);
            XmlHelper.SetValue(xmlElement, "Type", this.DefectType.ToString());
            XmlHelper.SetValue(xmlElement, "Diff", this.diffValue);

            for (int i = 0; i < 4; i++)
            {
                XmlHelper.SetValue(xmlElement, string.Format("DX{0}", i), this.rotateXArray[i]);
                XmlHelper.SetValue(xmlElement, string.Format("DY{0}", i), this.rotateYArray[i]);
            }
            XmlHelper.SetValue(xmlElement, "RotateWidth", this.rotateWidth);
            
        }

        public BitmapSource GetBitmapSource()
        {
            return this.image;
        }
    }

    public class LumpDefect : PatternDefect
    {
        public override Enum ResultObjectType => DefectType.Lump;

        public LumpDefect(BlobRect defectBlob, BitmapSource image, float diffValue, float compactness)
            : base(DefectType.Lump, defectBlob, image, diffValue, compactness)        {        }

        public LumpDefect(BitmapSource image, XmlElement defectElement)
            : base(DefectType.Lump, image, defectElement) { }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Lump);
        }
    }

    public class PatternDefect : Defect
    {
        bool isCircular = false;

        public override Enum ResultObjectType => (this.isCircular) ? DefectType.CircularPattern : base.ResultObjectType;

        protected PatternDefect(DefectType defectType, BlobRect defectBlob, BitmapSource image, float diffValue, float compactness)
            : base(defectType, defectBlob, image, diffValue)
        {
            double circularLimit = 1 + (4.0 / Math.PI * (SystemManager.Instance().OperatorManager.InspectOperator.Settings.CircularThreshold / 100));
            this.isCircular = (compactness < circularLimit);
        }

        protected PatternDefect(DefectType defectType, BitmapSource image, XmlElement defectElement)
            : base(defectType, image, defectElement)
        {
            this.isCircular = XmlHelper.GetValue(defectElement, "IsCircular", this.isCircular);
        }

        public PatternDefect(BlobRect defectBlob, BitmapSource image, float diffValue, float compactness)
            : this(DefectType.Pattern, defectBlob, image, diffValue, compactness) { }

        public PatternDefect(BitmapSource image, XmlElement defectElement)
            : this(DefectType.Pattern, image, defectElement) { }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Pattern);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "IsCircular", this.isCircular);
        }
    }
    
    public class MarginDefect : Defect
    {
        bool isCircular = false;

        public override Enum ResultObjectType => this.isCircular ? DefectType.CircularMargin : base.ResultObjectType;

        public MarginDefect(BlobRect defectBlob, BitmapSource image, float diffValue, float compactness) : base(DefectType.Margin, defectBlob, image, diffValue)
        {
            double circularLimit = 1 + (4.0 / Math.PI * (SystemManager.Instance().OperatorManager.InspectOperator.Settings.CircularThreshold / 100));
            this.isCircular = (compactness < circularLimit);
        }

        public MarginDefect(BitmapSource image, XmlElement defectElement) : base(DefectType.Margin, image, defectElement)
        {
            this.isCircular = XmlHelper.GetValue(defectElement, "IsCircular", this.isCircular);
        }
        
        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Margin);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "IsCircular", this.isCircular.ToString());
        }
    }

    public class ShapeDefect : Defect
    {
        public override double Length => this.diffValue * DeveloperSettings.Instance.Resolution;
        public override float DiffValue => float.NaN;

        public ShapeDefect(BlobRect defectBlob, BitmapSource image, float diffValue) : base(DefectType.Shape, defectBlob, image, diffValue)
        {
            this.drawingScale = 1 / SystemManager.Instance().OperatorManager.ExtractOperator.Settings.BlobScaleFactor;
        }

        public ShapeDefect(BitmapSource image, XmlElement defectElement) : base(DefectType.Shape, image, defectElement)
        {
            this.drawingScale = 1 / SystemManager.Instance().OperatorManager.ExtractOperator.Settings.BlobScaleFactor;
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Shape);
        }
    }

}
