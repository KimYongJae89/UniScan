using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DynMvp.UI;
using System.Xml;
using DynMvp.Base;

namespace DynMvp.Cad.Gerber
{
    public enum Coornidate
    {
        LL, LR, UL, UR
    }

    public enum Unit
    {
        MM, Inch
    }

    public class Module
    {
        int no;
        public int No
        {
            get { return no; }
        }

        PointF pos;
        public PointF Pos
        {
            get { return pos; }
        }

        int angle;
        public int Angle
        {
            get { return angle; }
        }

        public Module(int no, float posX, float posY, int angle)
        {
            this.no = no;
            this.pos = new PointF(posX, posY);
            this.angle = angle;
        }

        public void Offset(float offsetX, float offsetY)
        {
            SizeF offset = new SizeF(offsetX, offsetY);
            pos = PointF.Add(pos, offset);
        }
    }

    public enum FiducialType {  Global, Module, Local }
    public enum FigureShape {  Rectangle, Circle, Undifined, Oblong, Sloped }

    public class Fiducial
    {
        int no;
        public int No
        {
            get { return no; }
        }

        FiducialType type;
        public FiducialType Type
        {
            get { return type; }
        }

        FigureShape shape;
        public FigureShape Shape
        {
            get { return shape; }
        }

        PointF pos;
        public PointF Pos
        {
            get { return pos; }
        }

        SizeF size;
        public SizeF Size
        {
            get { return size; }
        }

        SizeF offset;
        public SizeF Offset
        {
            get { return offset; }
        }

        string refCode;
        public string RefCode
        {
            get { return refCode; }
        }

        int moduleNo;
        public int ModuleNo
        {
            get { return moduleNo; }
        }

        public Fiducial(int no, FiducialType fiducialType, FigureShape fiducialShape, float posX, float posY, float width, float height, float offsetX, float offsetY, string refCode, int moduleNo)
        {
            this.no = no;
            this.type = fiducialType;
            this.shape = fiducialShape;
            this.pos = new PointF(posX, posY);
            this.size = new SizeF(width, height);
            this.offset = new SizeF(offsetX, offsetY);
            this.refCode = refCode;
            this.moduleNo = moduleNo;
        }

        public Figure CreateFigure()
        {
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), size);

            Figure figure = null;
            switch (shape)
            {
                case FigureShape.Rectangle:
                case FigureShape.Undifined:
                case FigureShape.Oblong:
                case FigureShape.Sloped:
                    figure = new RectangleFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
                case FigureShape.Circle:
                    figure = new EllipseFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
            }

            figure.Offset(pos.X, pos.Y);
            return figure;
        }

        public void OffsetPos(float offsetX, float offsetY)
        {
            SizeF offset = new SizeF(offsetX, offsetY);
            pos = PointF.Add(pos, offset);
        }
    }

    public class Pattern
    {
        int no;
        public int No
        {
            get { return no; }
        }

        FigureShape shape;
        public FigureShape Shape
        {
            get { return shape; }
        }

        SizeF size;
        public SizeF Size
        {
            get { return size; }
        }

        PointF centroid;
        public PointF Centroid
        {
            get { return centroid; }
        }

        float area;
        public float Area
        {
            get { return area; }
        }

        float angle;
        public float Angle
        {
            get { return angle; }
        }

        float boundary;
        public float Boundary
        {
            get { return boundary; }
            set { boundary = value; }
        }

        public Pattern(int patternNo, FigureShape patternShape, float width, float height, float centroidX, float centroidY, float area, float angle)
        {
            this.no = patternNo;
            this.shape = patternShape;
            this.size = new SizeF(width, height);
            this.centroid = new PointF(centroidX, centroidY);
            this.area = area;
            this.angle = angle;
            this.boundary = CalculateBoundary();
        }

        private float CalculateBoundary()
        {
            float calculationBoundary = 0.0f;

            switch (shape)
            {
                case FigureShape.Rectangle:
                    calculationBoundary = size.Width * 2 + size.Height * 2;
                    break;
                case FigureShape.Circle:
                    calculationBoundary = Convert.ToSingle(Math.PI) * size.Width;
                    break;
                case FigureShape.Oblong:
                case FigureShape.Sloped:
                    calculationBoundary = size.Width * 2 + size.Height * 2;
                    break;
            }

            return calculationBoundary;
        }

        public Figure CreateFigure(bool good = true)
        {
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), size);
            RotatedRect figureRotatedRect = new RotatedRect(figureRect, angle);

            Color color = Color.Yellow;
            int width = 1;
            if (good == false)
            {
                color = Color.Red;
                width = 2;
            }

            Figure figure = null;
            switch (shape)
            {
                case FigureShape.Rectangle:
                    figure = new RectangleFigure(figureRect, new Pen(color, width));
                    break;
                case FigureShape.Circle:
                    figure = new EllipseFigure(figureRect, new Pen(color, width));
                    break;
                case FigureShape.Oblong:
                case FigureShape.Sloped:
                    figure = new RectangleFigure(figureRotatedRect, new Pen(color, width));
                    break;
            }

            return figure;
        }

        public bool InspectAreaRatio(int maskThicknessUm)
        {
            bool result = true;
            if (area / (boundary * maskThicknessUm) < 0.66)
                result = false;

            return result;
        }
    }

    public class Pad
    {
        int no;
        public int No
        {
            get { return no; }
        }

        int patternNo;
        public int PatternNo
        {
            get { return patternNo; }
        }

        PointF pos;
        public PointF Pos
        {
            get { return pos; }
        }

        RectangleF rectangle;
        public RectangleF Rectangle
        {
            get { return rectangle; }
        }

        string refCode;
        public string RefCode
        {
            get { return refCode; }
        }

        int moduleNo;
        public int ModuleNo
        {
            get { return moduleNo; }
        }

        int pinNo;
        public int PinNo
        {
            get { return pinNo; }
        }

        int fovNo;
        public int FovNo
        {
            get { return fovNo; }
        }

        public Pad(int padNo, int patternNo, float posX, float posY, float left, float top, float right, float bottom, string refCode, int pinNo, int moduleNo, int fovNo)
        {
            this.no = padNo;
            this.patternNo = patternNo;
            this.pos = new PointF(posX, posY);
            this.rectangle = RectangleF.FromLTRB(left, top, right, bottom);
            this.refCode = refCode;
            this.moduleNo = moduleNo;
            this.fovNo = fovNo;
            this.pinNo = pinNo;
        }

        public void Offset(float offsetX, float offsetY)
        {
            SizeF offset = new SizeF(offsetX, offsetY);
            pos = PointF.Add(pos, offset);
            rectangle = DrawingHelper.Offset(rectangle, offset);
        }
    }

    public class Fov
    {
        int no;
        public int No
        {
            get { return no; }
        }

        PointF pos;
        public PointF Pos
        {
            get { return pos; }
        }

        public Fov(int padNo, float posX, float posY)
        {
            this.no = padNo;
            this.pos = new PointF(posX, posY);
        }

        public void Offset(float offsetX, float offsetY)
        {
            SizeF offset = new SizeF(offsetX, offsetY);
            pos = PointF.Add(pos, offset);
        }
    }

    public class PatternEdge
    {
        int patternNo;
        public int PatternNo
        {
            get { return patternNo; }
        }

        List<List<PointF>> pointChainList = new List<List<PointF>>();

        public PatternEdge(int patternNo)
        {
            this.patternNo = patternNo;
        }

        public void AddNewList(PointF point)
        {
            List<PointF> pointList = new List<PointF>();
            pointChainList.Add(pointList);

            pointList.Add(point);
        }

        public void AddPoint(PointF point)
        {
            pointChainList.Last().Add(point);
        }

        public Figure CreateFigure()
        {
            FigureGroup figureGroup = new FigureGroup(); 
            foreach(List<PointF> pointList in pointChainList)
            {
                figureGroup.AddFigure(new PolygonFigure(pointList, true, new Pen(Color.Yellow, 1)));
            }

            return figureGroup;
        }
    }

    public class PadInfo
    {
        Figure figure;
        public Figure Figure
        {
            get { return figure; }
        }

        int patternNo;
        public int PatternNo
        {
            get { return patternNo; }
            set { patternNo = value; }
        }

        float width;
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        float height;
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        float area;
        public float Area
        {
            get { return area; }
            set { area = value; }
        }

        PointF centroid;
        public PointF Centroid
        {
            get { return centroid; }
            set { centroid = value; }
        }

        public PadInfo()
        {
        }

        public PadInfo(int patternNo, Figure figure, float area, float width, float height, PointF centroid)
        {
            this.patternNo = patternNo;
            this.figure = figure;
            this.area = area;
            this.width = width;
            this.height = height;
            this.centroid = centroid;
        }

        public void Copy(PadInfo srcPadInfo)
        {
            patternNo = srcPadInfo.patternNo;
            area = srcPadInfo.area;
            width = srcPadInfo.width;
            height = srcPadInfo.height;
            centroid = srcPadInfo.centroid;
            figure = (Figure)srcPadInfo.figure?.Clone();
        }

        public void LoadParam(XmlElement paramElement)
        {
            patternNo = Convert.ToInt32(XmlHelper.GetValue(paramElement, "PatternNo", "0"));
            area = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Area", "0"));
            width = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Width", "0"));
            height = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Height", "0"));

            XmlHelper.GetValue(paramElement, "Centroid", ref centroid);

            XmlElement figureElement = paramElement["Figure"];
            if (figureElement != null)
            {
                string typeStr = XmlHelper.GetValue(figureElement, "Type", "");
                figure = FigureFactory.Create(typeStr);

                figure.Load(figureElement);
            }
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "PatternNo", patternNo.ToString());
            XmlHelper.SetValue(paramElement, "Area", area.ToString());
            XmlHelper.SetValue(paramElement, "Width", width.ToString());
            XmlHelper.SetValue(paramElement, "Height", height.ToString());
            XmlHelper.SetValue(paramElement, "Centroid", centroid);

            XmlElement figureElement = paramElement.OwnerDocument.CreateElement("Figure");
            paramElement.AppendChild(figureElement);

            if (figure.Type != FigureType.Group && figure.Type != FigureType.Polygon)
            {
                figure.Save(figureElement);
            }
            else
            {
                RectangleFigure rectFigure = new RectangleFigure(figure.GetRectangle(), new Pen(Color.Yellow, 1));
                rectFigure.Save(figureElement);
            }
        }
    }

    public class GerberData
    {
        Unit unit;
        public Unit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        Coornidate coornidate;
        public Coornidate Coornidate
        {
            get { return coornidate;  }
            set { coornidate = value; }
        }

        int version;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        float offsetX;
        public float OffsetX
        {
            get { return offsetX; }
            set { offsetX = value; }
        }

        float offsetY;
        public float OffsetY
        {
            get { return offsetY; }
            set { offsetY = value; }
        }

        float offsetXFromRightBottom;
        public float OffsetXFromRightBottom
        {
            get { return offsetXFromRightBottom; }
            set { offsetXFromRightBottom = value; }
        }

        float offsetYFromRightBottom;
        public float OffsetYFromRightBottom
        {
            get { return offsetYFromRightBottom; }
            set { offsetYFromRightBottom = value; }
        }

        bool combineArray;
        public bool CombineArray
        {
            get { return combineArray; }
            set { combineArray = value; }
        }

        int numModule;
        public int NumModule
        {
            get { return numModule; }
            set { numModule = value; }
        }

        int numFiducial;
        public int NumFiducial
        {
            get { return numFiducial; }
            set { numFiducial = value; }
        }

        int numPattern;
        public int NumPattern
        {
            get { return numPattern; }
            set { numPattern = value; }
        }

        int numPad;
        public int NumPad
        {
            get { return numPad; }
            set { numPad = value; }
        }

        int numFov;
        public int NumFov
        {
            get { return numFov; }
            set { numFov = value; }
        }

        SizeF boardSize;
        public SizeF BoardSize
        {
            get { return boardSize; }
            set { boardSize = value; }
        }

        List<Module> moduleList = new List<Module>();
        public List<Module> ModuleList
        {
            get { return moduleList; }
        }

        List<Fiducial> fiducialList = new List<Fiducial>();
        public List<Fiducial> FiducialList
        {
            get { return fiducialList; }
        }

        List<Pattern> patternList = new List<Pattern>();
        public List<Pattern> PatternList
        {
            get { return patternList; }
        }

        List<Pad> padList = new List<Pad>();
        public List<Pad> PadList
        {
            get { return padList; }
        }

        List<Fov> fovList = new List<Fov>();
        public List<Fov> FovList
        {
            get { return fovList; }
        }

        List<PatternEdge> patternEdgeList = new List<PatternEdge>();

        public void AddModule(Module module)
        {
            moduleList.Add(module);
        }

        public void AddFiducial(Fiducial fiducial)
        {
            fiducialList.Add(fiducial);
        }

        public void AddPattern(Pattern pattern)
        {
            patternList.Add(pattern);
        }

        public void AddPad(Pad pad)
        {
            padList.Add(pad);
        }

        public void AddFov(Fov fov)
        {
            fovList.Add(fov);
        }

        public void AddPatternEdge(PatternEdge patternEdge)
        {
            patternEdgeList.Add(patternEdge);
        }

        public PatternEdge GetLastPatternEdge()
        {
            if (patternEdgeList.Count > 0)
                return patternEdgeList.Last();

            return null;
        }

        public Pattern GetPattern(int patternNo)
        {
            return patternList.Find(x => x.No == patternNo);
        }

        public PatternEdge GetPatternEdge(int patternNo)
        {
            return patternEdgeList.Find(x => x.PatternNo == patternNo);
        }

        public PadInfo CreatePadInfo(Pad pad)
        {
            Pattern pattern = GetPattern(pad.PatternNo);
            Figure figure = CreatePadFigure(pad);

            if (figure != null)
            {
                PointF centroid = DrawingHelper.Add(pad.Pos, pattern.Centroid);
                figure.Offset(pad.Pos.X, pad.Pos.Y);
                return new PadInfo(pad.PatternNo, figure, pattern.Area, pattern.Size.Width, pattern.Size.Height, centroid);
            }

            return null;
        }

        public Figure CreatePadFigure(Pad pad, bool good = true)
        {
            Pattern pattern = GetPattern(pad.PatternNo);

            Figure figure = null;
            if (pattern.Shape == FigureShape.Undifined)
            {
                PatternEdge patternEdge = GetPatternEdge(pad.PatternNo);
                figure = patternEdge.CreateFigure();
            }
            else
            {
                figure = pattern.CreateFigure(good);
            }

            return figure;
        }

        public void Offset(float offsetX, float offsetY)
        {
            foreach(Pad pad in padList)
            {
                pad.Offset(offsetX, offsetY);
            }

            foreach (Fov fov in fovList)
            {
                fov.Offset(offsetX, offsetY);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.OffsetPos(offsetX, offsetY);
            }

            foreach (Module module in moduleList)
            {
                module.Offset(offsetX, offsetY);
            }
        }

        public void FlipX(float centerY)
        {
            foreach (Pad pad in padList)
            {
                pad.Offset(offsetX, offsetY);
            }

            foreach (Fov fov in fovList)
            {
                fov.Offset(offsetX, offsetY);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.OffsetPos(offsetX, offsetY);
            }

            foreach (Module module in moduleList)
            {
                module.Offset(offsetX, offsetY);
            }
        }

        public void FlipY(float centerX)
        {
            foreach (Pad pad in padList)
            {
                pad.Offset(offsetX, offsetY);
            }

            foreach (Fov fov in fovList)
            {
                fov.Offset(offsetX, offsetY);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.OffsetPos(offsetX, offsetY);
            }

            foreach (Module module in moduleList)
            {
                module.Offset(offsetX, offsetY);
            }
        }
    }
}
