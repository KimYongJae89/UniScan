using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Screen.Vision;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Vision;

namespace UniScanS.Screen.Data
{
    public class SheetSubResult : AlgorithmResult
    {
        int lowerTh;
        int upperTh;

        int camIndex;
        public int CamIndex
        {
            get { return camIndex; }
            set { camIndex = value; }
        }

        int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        float lowerDiffValue;
        public float LowerDiffValue
        {
            get { return lowerDiffValue; }
            set { lowerDiffValue = value; }
        }

        float upperDiffValue;
        public float UpperDiffValue
        {
            get { return upperDiffValue; }
            set { upperDiffValue = value; }
        }

        float compactness;
        public float Compactness
        {
            get { return compactness; }
            set { compactness = value; }
        }

        float elongation;
        public float Elongation
        {
            get { return elongation; }
            set { elongation = value; }
        }

        Bitmap binaryImage;
        public Bitmap BinaryImage
        {
            get { return binaryImage; }
            set { binaryImage = value; }
        }

        DefectType defectType;
        public DefectType DefectType
        {
            get { return defectType; }
            set { defectType = value; }
        }

        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        RotatedRect rotatedRect;
        public RotatedRect RotatedRect
        {
            get { return rotatedRect; }
            set { rotatedRect = value; }
        }

        Rectangle srcRegion;
        public Rectangle SrcRegion
        {
            get { return srcRegion; }
            set { srcRegion = value; }
        }

        Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        PointF realPos;
        public PointF RealPos
        {
            get { return realPos; }
            set { realPos = value; }
        }

        float length;
        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        int area;
        public int Area
        {
            get { return area; }
            set { area = value; }
        }

        float realLength;
        public float RealLength
        {
            get { return realLength; }
            set { realLength = value; }
        }

        string imagePath;

        public SheetSubResult(string algorithmName = "") : base(algorithmName)
        {
        }

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        public SheetSubResult() : base(SheetInspector.TypeName)
        {

        }

        public void SetThreshold(int lowerTh, int upperTh)
        {
            this.lowerTh = lowerTh;
            this.upperTh = upperTh;
        }

        public void Offset(int x, int y, float ratio)
        {
            region.Offset(x, y);
        }

        protected string ToBaseExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t", camIndex.ToString(), index.ToString(), defectType.ToString());
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t", region.X, region.Y, region.Width, region.Height, area);
            stringBuilder.AppendFormat("{0}\t{1}\t", realPos.X, realPos.Y);

            return stringBuilder.ToString();
        }

        public static DefectType GetDefectType(string line)
        {
            DefectType type;
            bool result = Enum.TryParse(line, out type);
            if (result == false)
                type = DefectType.Pole;

            return type;
        }

        protected void FromBaseExportData(string[] splitLine)
        {
            camIndex = Convert.ToInt32(splitLine[0]);
            index = Convert.ToInt32(splitLine[1]);
            defectType = GetDefectType(splitLine[2]);
            region.X = Convert.ToInt32(splitLine[3]);
            region.Y = Convert.ToInt32(splitLine[4]);
            region.Width = Convert.ToInt32(splitLine[5]);
            region.Height = Convert.ToInt32(splitLine[6]);
            area = Convert.ToInt32(splitLine[7]);

            realPos.X = Convert.ToSingle(splitLine[8]);
            realPos.Y = Convert.ToSingle(splitLine[9]);
        }
        
        public virtual string ToExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}", ToBaseExportString());
            stringBuilder.AppendFormat("{0:0.00}\t", realLength);
            stringBuilder.AppendFormat("{0:0.00}\t", lowerDiffValue);
            stringBuilder.AppendFormat("{0:0.00}\t", upperDiffValue);
            stringBuilder.AppendFormat("{0:0.00}\t", compactness);
            stringBuilder.AppendFormat("{0:0.00}\t", elongation);


            return stringBuilder.ToString();
        }

        public virtual void FromExportData(string[] splitLine)
        {
            if (splitLine.Length != 16)
                return;

            FromBaseExportData(splitLine);
            
            realLength = Convert.ToSingle(splitLine[10]);
            lowerDiffValue = Convert.ToSingle(splitLine[11]);
            upperDiffValue = Convert.ToSingle(splitLine[12]);
            compactness = Convert.ToSingle(splitLine[13]);
            elongation = Convert.ToSingle(splitLine[14]);

            switch (defectType)
            {
                case DefectType.SheetAttack:
                case DefectType.Pole:
                    if (realLength >= AlgorithmSetting.Instance().SheetAttackMinSize
                        && lowerDiffValue > 0
                        && (compactness > AlgorithmSetting.Instance().PoleCompactness || elongation > AlgorithmSetting.Instance().PoleElongation))
                    {
                        defectType = DefectType.SheetAttack;
                    }
                    else
                    {
                        defectType = DefectType.Pole;
                    }
                    break;
                case DefectType.Dielectric:
                case DefectType.PinHole:
                    if ((compactness <= AlgorithmSetting.Instance().DielectricCompactness || elongation <= AlgorithmSetting.Instance().DielectricElongation) 
                        && lowerDiffValue > 0)
                    {
                        defectType = DefectType.PinHole;
                    }
                    else
                    {
                        defectType = DefectType.Dielectric;
                    }
                    break;
            }
        }

        public override string ToString()
        {
            string message = "";
            message += string.Format("X : {0}mm\nY : {1}mm\nLength : {2}um\n", realPos.X / 1000.0f, realPos.Y / 1000.0f, realLength);

            if (Math.Abs(lowerDiffValue) > lowerTh)
                message += string.Format("L : {0:0.00} ", lowerDiffValue);

            if (Math.Abs(upperDiffValue) > upperTh)
                message += string.Format("U : {0:0.00}", upperDiffValue);

            message += "\n";
            message += string.Format("C : {0:0.00} ", compactness);
            message += string.Format("E : {0:0.00}", elongation);

            return message;
        }

        public Figure GetFigure(int width, float ratio = 1.0f, bool review = false, float inflate = 10)
        {
            Color defectColor = Color.Maroon;

            if (review == true)
                defectColor = Color.White;
            else
            {
                switch (defectType)
                {
                    case DefectType.SheetAttack:
                        defectColor = Color.Maroon;
                        break;
                    case DefectType.Pole:
                        defectColor = Color.Red;
                        break;
                    case DefectType.Dielectric:
                        defectColor = Color.Blue;
                        break;
                    case DefectType.PinHole:
                        defectColor = Color.DarkMagenta;
                        break;
                    case DefectType.Shape:
                        defectColor = Color.DarkGreen;
                        width /= 5;
                        width = Math.Max(width, 1);
                        break;
                }
            }

            Rectangle inflateRegion = region;
            inflateRegion.Inflate((int)inflate, (int)inflate);
            inflateRegion.X = (int)Math.Round(inflateRegion.X * ratio);
            inflateRegion.Y = (int)Math.Round(inflateRegion.Y * ratio);
            inflateRegion.Width = (int)Math.Round(inflateRegion.Width * ratio);
            inflateRegion.Height = (int)Math.Round(inflateRegion.Height * ratio);
            RectangleFigure rectangleFigure = new RectangleFigure(inflateRegion, new Pen(defectColor, width));
            rectangleFigure.Tag = this;

            return rectangleFigure;
        }
    }

    public class ShapeDiffValue
    {
        float diffTol;
        float heightDiffTol;

        public float SumDiff
        {
            get { return Math.Abs(areaDiff) + Math.Abs(widthDiff) + Math.Abs(heightDiff) + Math.Abs(centerXDiff) + Math.Abs(centerYDiff); }
        }
        
        SheetPattern similarPattern;
        public SheetPattern SimilarPattern
        {
            get { return similarPattern; }
            set { similarPattern = value; }
        }
        
        float areaDiff;
        public float AreaDiff
        {
            get { return areaDiff; }
            set { areaDiff = value; }
        }

        float widthDiff;
        public float WidthDiff
        {
            get { return widthDiff; }
            set { widthDiff = value; }
        }

        float heightDiff;
        public float HeightDiff
        {
            get { return heightDiff; }
            set { heightDiff = value; }
        }

        float centerXDiff;
        public float CenterXDiff
        {
            get { return centerXDiff; }
            set { centerXDiff = value; }
        }

        float centerYDiff;
        public float CenterYDiff
        {
            get { return centerYDiff; }
            set { centerYDiff = value; }
        }
        
        public ShapeDiffValue(bool init)
        {
            if (init == true)
            {
                areaDiff = float.MaxValue;
                widthDiff = float.MaxValue;
                heightDiff = float.MaxValue;
                centerXDiff = float.MaxValue;
                centerYDiff = float.MaxValue;
            }
        }

        public void SetTolerance(float diffTol, float heightDiffTol = -1)
        {
            this.diffTol = diffTol;
            this.heightDiffTol = heightDiffTol;
        }

        public string ToExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0:0.00}\t", areaDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", widthDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", heightDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", centerXDiff);
            stringBuilder.AppendFormat("{0:0.00}", centerYDiff);

            return stringBuilder.ToString();
        }

        public void FromExportData(string[] splitLine)
        {
            areaDiff = Convert.ToSingle(splitLine[10]);
            widthDiff = Convert.ToSingle(splitLine[11]);
            heightDiff = Convert.ToSingle(splitLine[12]);
            centerXDiff = Convert.ToSingle(splitLine[13]);
            centerYDiff = Convert.ToSingle(splitLine[14]);
        }

        public override string ToString()
        {
            string message = "";

            if (Math.Abs(areaDiff) > diffTol)
                message += string.Format("A : {0:0.00} ", areaDiff);

            if (Math.Abs(widthDiff) > diffTol)
                message += string.Format("W : {0:0.00} ", widthDiff);


            if (heightDiffTol == -1)
            {
                if (Math.Abs(heightDiff) > diffTol)
                    message += string.Format("H : {0:0.00} ", heightDiff);
            }
            else
            {
                if (Math.Abs(heightDiff) > heightDiffTol)
                    message += string.Format("H : {0:0.00} ", heightDiff);
            }

            if (Math.Abs(centerXDiff) > diffTol)
                message += string.Format("Cx : {0:0.00} ", centerXDiff);

            if (Math.Abs(centerYDiff) > diffTol)
                message += string.Format("Cy : {0:0.00} ", centerYDiff);

            return message;
        }

        public bool IsDefect()
        {
            if (Math.Abs(areaDiff) > diffTol)
                return true;

            if (Math.Abs(widthDiff) > diffTol)
                return true;
                

            if (heightDiffTol == -1)
            {
                if (Math.Abs(heightDiff) > diffTol)
                    return true;
            }
            else
            {
                if (Math.Abs(heightDiff) > heightDiffTol)
                    return true;
            }

            if (Math.Abs(centerXDiff) > diffTol)
                return true;

            if (Math.Abs(centerYDiff) > diffTol)
                return true;

            return false;
        }
    }

    public class ShapeResult : SheetSubResult
    {
        public ShapeResult(string algorithmName = "") : base(algorithmName)
        {
            DefectType = DefectType.Shape;
        }

        ShapeDiffValue shapeDiffValue;
        public ShapeDiffValue ShapeDiffValue
        {
            get { return shapeDiffValue; }
            set { shapeDiffValue = value; }
        }

        public override string ToString()
        {
            string message = "";
            message += string.Format("X : {0}mm  Y : {1}mm\n W : {2}um, H : {3}um\n", Region.X * AlgorithmSetting.Instance().XPixelCal / 1000.0f, Region.Y * AlgorithmSetting.Instance().YPixelCal / 1000.0f, Region.Width * AlgorithmSetting.Instance().XPixelCal, Region.Height * AlgorithmSetting.Instance().YPixelCal);
            message += shapeDiffValue.ToString();

            return message;
        }

        public override string ToExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0}", ToBaseExportString());
            stringBuilder.AppendFormat("{0}", shapeDiffValue.ToExportString());
            
            return stringBuilder.ToString();
        }

        public override void FromExportData(string[] splitLine)
        {
            if (splitLine.Length != 15)
                return;

            FromBaseExportData(splitLine);
            
            shapeDiffValue = new ShapeDiffValue(false);
            shapeDiffValue.FromExportData(splitLine);
        }
    }
}
