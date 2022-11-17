using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Xml;
using System.Diagnostics;
using UniScanM.Data;
using DynMvp.Vision.OpenCv;

namespace UniScanM.StillImage.Data
{
    public class ShapeOfInterest : UniScanM.Data.IExportable, UniScanM.Data.IImportable
    {
        Rectangle aroundRectangle;
        ShapeInfo shapeInfo;

        public Rectangle AroundRectangle
        {
            get { return aroundRectangle; }
        }

        public ShapeInfo ShapeInfo
        {
            get { return shapeInfo; }
        }

        public bool IsEmpty
        {
            get { return shapeInfo == null; }
        }

        private ShapeOfInterest() { }
        public ShapeOfInterest(Rectangle aroundRectangle, ShapeInfo shapeInfo)
        {
            this.aroundRectangle = aroundRectangle;
            this.shapeInfo = shapeInfo;
        }

        public void Offset(int v1, int v2)
        {
            aroundRectangle.Offset(v1, v2);
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "Rectangle", aroundRectangle);
            shapeInfo?.Export(element, "ShapeInfo");
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            XmlHelper.GetValue(element, "Rectangle", ref aroundRectangle);
            shapeInfo = ShapeInfo.Load(element, "ShapeInfo");
        }

        static public ShapeOfInterest Load(XmlElement element, string subKey = null)
        {
            ShapeOfInterest shapeOfInterest = new ShapeOfInterest();
            shapeOfInterest.Import(element, subKey);
            return shapeOfInterest;
        }

    }

    // 패턴 모양 정보
    public class ShapeInfo : IExportable, IImportable
    {
        int id;
        private Rectangle baseRect;
        private int area;    // blob area
        private int waist;  // waist Length
        private double length;  // bound rect diagonal length
        private ShapeInfo[] neighborhood;
        private Point centerPT;

        public int Id
        {
            get { return id; }
        }

        public Rectangle BaseRect
        {
            get { return baseRect; }
            set { baseRect = value; }
        }

        public int Area
        {
            get { return area; }
            set { area = value; }
        }

        public int Waist
        {
            get { return waist; }
            set { waist = value; }
        }
        
        public double Length
        {
            get { return length; }
            set { length = value; }
        }
                
        public ShapeInfo[] Neighborhood
        {
            get { return neighborhood; }
            set { neighborhood = value; }
        }
        
        public int Width
        {
            get { return this.baseRect.Width; }
        }

        public int Height
        {
            get { return this.baseRect.Height; }
        }
        public Point CenterPT
        {
            get { return centerPT; }
        }
        
        private ShapeInfo()
        { }

        public ShapeInfo(int id, Rectangle baseRect, int area, int waist, double length, Point centerpt)
        {
            this.id = id;
            this.baseRect = baseRect;
            this.area = area;
            this.waist = waist;
            this.length = length;
            this.centerPT = centerpt;
        }

        public void Offset(Point pt)
        {
            this.baseRect.Offset(pt);
        }

        public void Offset(int x, int y)
        {
            this.baseRect.Offset(x, y);
        }

        public void ChangeId(int id)
        {
            this.id = id;
        }

        internal ShapeInfo Clone()
        {
            return new ShapeInfo(this.id, this.BaseRect, this.area,this.waist, this.length, this.centerPT);
        }

        //width, height, area, 대각 length, waist 비교. 유사도 안으로 들어오면...리턴.
        public static bool IsSimilar(ShapeInfo shapeInfo1, ShapeInfo shapeInfo2, float rate)
        {
            if (shapeInfo1 == null || shapeInfo2 == null)
                return false;

            double val;
            float min = 1 - (1-rate);
            float max = 1 + (1-rate);

            val = (double)shapeInfo1.baseRect.Width / (double)shapeInfo2.baseRect.Width;
            if (val < min || val > max)
                return false;

            val = (double)shapeInfo1.baseRect.Height / (double)shapeInfo2.baseRect.Height;
            if (val < min || val > max)
                return false;

            val = (double)shapeInfo1.area / (double)shapeInfo2.area;
            if (val < min || val > max)
                return false;

            val = (double)shapeInfo1.length / (double)shapeInfo2.length;
            if (val < min || val > max)
                return false;

            val = (double)shapeInfo1.waist / (double)shapeInfo2.waist;
            if (val < min || val > max)
                return false;

            //Rectangle temp = Rectangle.Intersect(shapeInfo1.baseRect, shapeInfo2.baseRect);
            //float temp1 = (temp.Width * temp.Height) * 2;
            //float temp2 = (shapeInfo1.baseRect.Width * shapeInfo1.baseRect.Height) + (shapeInfo2.baseRect.Width * shapeInfo2.baseRect.Height);
            //val = (double)temp1 / (double)temp2;
            //if (val < min || val > max)
            //    return false;

            return true;
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if(string.IsNullOrEmpty(subKey)==false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "ID", this.id.ToString());

            XmlHelper.SetValue(element, "BaseRect", this.baseRect);

            XmlHelper.SetValue(element, "Area", this.area.ToString());
            XmlHelper.SetValue(element, "Waist", this.waist.ToString());
            XmlHelper.SetValue(element, "Length", this.length.ToString());
        }

        public static ShapeInfo Load(XmlElement element, string key=null)
        {
            ShapeInfo shapeInfo = new ShapeInfo();
            shapeInfo.Import(element, key);
            return shapeInfo;
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            this.id = Convert.ToInt32(XmlHelper.GetValue(element, "ID", "-1"));

            Rectangle baseRect = Rectangle.Empty;
            XmlHelper.GetValue(element, "BaseRect", ref baseRect);
            this.baseRect = baseRect;

            this.area = Convert.ToInt32(XmlHelper.GetValue(element, "Area", "0"));
            this.waist = Convert.ToInt32(XmlHelper.GetValue(element, "Waist", "0"));
            this.length = Convert.ToSingle(XmlHelper.GetValue(element, "Length", "0"));
        }
    }

    public struct Feature : IExportable, IImportable
    {
        private float area;
        private SizeF margin;  //pixel
        private SizeF blot;  //pixel

        public float Area
        {
            get { return area; }
            set { area = value; }
        }

        public SizeF Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        public SizeF Blot
        {
            get { return blot; }
            set { blot = value; }
        }

        public Feature(float area, SizeF margin, SizeF blot)
        {
            this.area = area;
            this.margin = margin;
            this.blot = blot;
        }

        public void Update(Feature feature)
        {
            Update(feature.area, feature.margin, feature.blot);
        }
        public void Update(float area, SizeF margin, SizeF blot)
        {
            this.area = area;
            this.margin = margin;
            this.blot = blot;
        }

        public Feature Mul(SizeF pelSize)
        {
            Feature result = new Feature();
            result.Area = area * pelSize .Width* pelSize.Height;
            result.Margin = new SizeF(margin.Width * pelSize.Width, margin.Height * pelSize.Height);
            result.Blot = new SizeF(blot.Width * pelSize.Width, blot.Height * pelSize.Height);
            return result;
        }

        public static Feature Sub(Feature a, Feature b)
        {
            Feature result = new Feature();
            result.Area = (a.Area - b.Area);
            result.Margin = SizeF.Subtract(a.Margin, b.Margin);
            result.Blot =new SizeF(Math.Max(0, a.blot.Width - b.blot.Width), Math.Max(0, a.blot.Height- b.blot.Height));
            return result;
        }

        public void Export(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "Area", this.area.ToString());

            XmlHelper.SetValue(element, "Margin", this.Margin);
            
            XmlHelper.SetValue(element, "Blot", this.blot);
        }
        
        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            this.area = Convert.ToSingle(XmlHelper.GetValue(element, "Area", "0"));

            SizeF margine = new SizeF();
            XmlHelper.GetValue(element, "Margin", ref margine);
            this.margin = margine;

            SizeF blot = new SizeF();
            XmlHelper.GetValue(element, "Blot", ref blot);
            this.blot = blot;
        }

        public Feature Load(XmlElement element, string v=null)
        {
            Feature feature = new Feature();
            feature.Import(element, v);
            return feature;
        }
    }

    public class TeachInfo : IExportable, IImportable
    {
        private int id;
        private Feature feature;
        private bool use;

        public int Id
        {
            get { return id; }
        }

        public Feature Feature
        {
            get { return feature; }
        }
        
        public bool IsValid
        {
            get { return feature.Margin.IsEmpty == false && feature.Blot.IsEmpty == false; }
        }

        public bool Use
        {
            get { return use; }
            set { this.use = value; }
        }

        public bool Inspectable
        {
            get { return Use && IsValid; }
        }

        private TeachInfo()
        { }

        public TeachInfo(int id, float area, SizeF margine, SizeF blot)
        {
            this.id = id;
            this.feature = new Feature();
            this.feature.Area = area;
            this.feature.Margin = margine;
            this.feature.Blot = blot;
            this.use = false;
        }

        public TeachInfo(int id, Feature feature)
        {
            this.id = id;
            this.feature = feature;
            this.use = false;
        }

        public void ChangeId(int id)
        {
            this.id = id;
        }

        public TeachInfo Clone()
        {
            TeachInfo clone = new TeachInfo(this.id, this.feature);
            clone.use = this.use;
            return clone;
        }

        internal static Feature Sub(TeachInfo inspTeachInfo, TeachInfo refTeachInfo)
        {
            //float ar = Math.Abs(inspTeachInfo.Area - refTeachInfo.Area);
            //float mW = Math.Abs(inspTeachInfo.Margin.Width - refTeachInfo.Margin.Width);
            //float mL = Math.Abs(inspTeachInfo.Margin.Height - refTeachInfo.Margin.Height);
            //float bW = Math.Abs(inspTeachInfo.Blot.Width - refTeachInfo.Blot.Width);
            //float bL = Math.Abs(inspTeachInfo.Blot.Height - refTeachInfo.Blot.Height);

            //float ar = Math.Min(inspTeachInfo.Area, refTeachInfo.Area) / Math.Max(inspTeachInfo.Area, refTeachInfo.Area);
            //float mW = Math.Min(inspTeachInfo.Margin.Width, refTeachInfo.Margin.Width) / Math.Max(inspTeachInfo.Margin.Width, refTeachInfo.Margin.Width);
            //float mL = Math.Min(inspTeachInfo.Margin.Height, refTeachInfo.Margin.Height) / Math.Max(inspTeachInfo.Margin.Height, refTeachInfo.Margin.Height);
            //float bW = Math.Min(inspTeachInfo.Blot.Width, refTeachInfo.Blot.Width) / Math.Max(inspTeachInfo.Blot.Width, refTeachInfo.Blot.Width);
            //float bL = Math.Min(inspTeachInfo.Blot.Height, refTeachInfo.Blot.Height) / Math.Max(inspTeachInfo.Blot.Height, refTeachInfo.Blot.Height);

            return Feature.Sub(inspTeachInfo.feature, refTeachInfo.feature);
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "ID", this.id.ToString());
            XmlHelper.SetValue(element, "Use", this.use.ToString());
            this.feature.Export(element, "Feature");
        }

        public static TeachInfo Load(XmlElement element, string key=null)
        {
            TeachInfo teachInfo = new TeachInfo();
            teachInfo.Import(element,key);
            return teachInfo;
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            this.id = Convert.ToInt32(XmlHelper.GetValue(element, "ID", "-1"));
            this.use = Convert.ToBoolean(XmlHelper.GetValue(element, "Use", "false"));
            this.feature = Feature.Load(element, "Feature");
        }
    }

    public class PatternInfo : IExportable, IImportable
    {
        // Shape Info
        ShapeInfo shapeInfo;

        // Teaching Info
        TeachInfo teachInfo;

        public ShapeInfo ShapeInfo
        {
            get { return shapeInfo; }
            set { shapeInfo = value; }
        }

        public TeachInfo TeachInfo
        {
            get { return teachInfo; }
            set { teachInfo = value; }
        }

        public static PatternInfo Load(XmlElement element, string key=null)
        {
            PatternInfo patternInfo = new PatternInfo();
            patternInfo.Import(element, key);
            return patternInfo;
        }

        private PatternInfo()
        { }

        public PatternInfo(ShapeInfo shapeInfo, TeachInfo teachingInfo)
        {
            this.shapeInfo = shapeInfo;
            this.teachInfo = teachingInfo;
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }
            
            shapeInfo.Export(element, "ShapeInfo");
            
            teachInfo.Export(element, "TeachInfo");
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            this.shapeInfo = ShapeInfo.Load(element, "ShapeInfo");
            this.teachInfo = TeachInfo.Load(element, "TeachInfo");
        }
    }

    public class PatternInfoGroup : IExportable, IImportable
    {
        List<ShapeInfo> shapeInfoList = null;
        TeachInfo teachInfo;

        public AlgoImage MakeAverageShapeImage(AlgoImage WholePatternImage)
        {
            //foreach ( ShapeInfo shape in shapeInfoList)
              AlgoImage MostImage = null;
            List<AlgoImage> listimage = new List<AlgoImage>();

            int Xbias = (int)(teachInfo.Feature.Margin.Width *0.6) ;
            int Ybias = (int)(teachInfo.Feature.Margin.Height * 0.6);
            int width = 0;
            int height = 0;
            int maxW = 0, maxH = 0;
            //foreach (ShapeInfo shape in shapeInfoList)
            //{
            //    if (maxW < shape.BaseRect.Width) maxW = (int)(shape.BaseRect.Width + 0.5);
            //    if (maxH < shape.BaseRect.Height) maxH = (int)(shape.BaseRect.Height + 0.5);
            //}
            //todo average add median
            maxW = (int)(teachInfo.Feature.Blot.Width+0.5f);
            maxH = (int)(teachInfo.Feature.Blot.Height+0.5f);

            width = maxW + Xbias * 2;
            height = maxH + Ybias * 2;

            int sX, sY, eX, eY;
            int ccc = 0;
            foreach (ShapeInfo shape in shapeInfoList)
            {
                ccc++;
                if (ccc > 100) break;
                sX = (int)(shape.CenterPT.X - (float)width / 2 + 0.5f);
                sY = (int)(shape.CenterPT.Y - (float)height / 2 + 0.5f);
                eX = sX + width;
                eY = sY + height;
                if (sX < 0 || sY < 0 || eX >= WholePatternImage.Width || eY >= WholePatternImage.Height) //이미지 사이즈를 벗어나면 제외함.
                    continue;
                Rectangle rc = new Rectangle(sX, sY, width, height);
                var cropImage = WholePatternImage.GetSubImage(rc);
                listimage.Add(cropImage);

                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(WholePatternImage) as OpenCvImageProcessing;
            MostImage = ipc.GetAverageFilteredImage(listimage);
            //MostImage.Save("c:\\Test\\Most.bmp");
            return MostImage;
        }
        public string debug = "";

        public int Id
        {
            get { return teachInfo.Id; }
        }

        public List<ShapeInfo> ShapeInfoList
        {
            get { return shapeInfoList; }
        }

        public TeachInfo TeachInfo
        {
            get { return teachInfo; }
        }
        
        private PatternInfoGroup() { }

        public PatternInfoGroup(int id, List<PatternInfo> patternInfoList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("L,T,R,B,A,MW,ML,BW,BL");
            patternInfoList.ForEach(f => sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                f.ShapeInfo.BaseRect.Left, f.ShapeInfo.BaseRect.Top, f.ShapeInfo.BaseRect.Right, f.ShapeInfo.BaseRect.Bottom,
                f.TeachInfo.Feature.Area, f.TeachInfo.Feature.Margin.Width, f.TeachInfo.Feature.Margin.Height, f.TeachInfo.Feature.Blot.Width, f.TeachInfo.Feature.Blot.Height)));
            debug = sb.ToString();

            shapeInfoList = patternInfoList.ConvertAll(f => f.ShapeInfo);

            List<PatternInfo> target = patternInfoList.FindAll(f => f.TeachInfo.IsValid);
            if (target.Count > 0)
            {
                float sz = (float)target.Average(f => f.TeachInfo.Feature.Area);

                float mW = (float)target.Average(f => f.TeachInfo.Feature.Margin.Width);
                float mL = (float)target.Average(f => f.TeachInfo.Feature.Margin.Height);

                float bW = (float)target.Average(f => f.TeachInfo.Feature.Blot.Width);
                float bL = (float)target.Average(f => f.TeachInfo.Feature.Blot.Height);

                teachInfo = new TeachInfo(id, sz, new SizeF(mW, mL), new SizeF(bW, bL));
            }
            else
                teachInfo = new TeachInfo(id, 0, SizeF.Empty, SizeF.Empty);
        }

        public PatternInfoGroup(PatternInfoGroup group)
        {
            this.CopyFrom(group);
        }

        public void CopyFrom(PatternInfoGroup src)
        {
            this.shapeInfoList.Clear();
            this.shapeInfoList.AddRange(src.shapeInfoList);
            this.teachInfo = src.teachInfo.Clone();
        }

        public void Offset(int x, int y)
        {
            for (int i = 0; i < shapeInfoList.Count; i++)
                shapeInfoList[i].Offset(x, y);
        }

        public void Sort()
        {
            // max x
            int maxX = shapeInfoList.Max(f => f.BaseRect.Right);

            // y-axis Sort
            List<ShapeInfo> yOrderedList = shapeInfoList.OrderBy(f => f.BaseRect.Top).ToList(); //오름차순

            //
            int idx = 0;
            int limit = 0;
            List<ShapeInfo> orderedList = new List<ShapeInfo>();

            while (yOrderedList.Count > 0)
            {
                Rectangle rect = Rectangle.FromLTRB(0, yOrderedList[0].BaseRect.Top, maxX, yOrderedList[0].BaseRect.Bottom);  //rect= 첫번째요소 위치의 가로줄 영역.. //기울어진 양으로 인해 에러를 유발할수 있겠으나, 마진영역이 흡수함.

                List<ShapeInfo> subList = yOrderedList.Where(f => f.BaseRect.IntersectsWith(rect)).ToList();  //rect영역에 오버랩되는 서브리스트
                yOrderedList = yOrderedList.Skip(subList.Count).ToList(); //찾은 오버랩 영억의 데이터의 갯수 만큼 제거.(정렬되어있으므로, 결국 그놈들이 제거됨)

                subList = subList.OrderBy(f => f.BaseRect.Left).ToList(); //오버랩 영역의 데이터를, 좌측 위치로 정렬.
                orderedList.AddRange(subList); //정렬된 가로줄 데이터를 추가.
            }

            System.Diagnostics.Debug.Assert(shapeInfoList.Count == orderedList.Count);
            shapeInfoList = orderedList;
        }

        public ShapeOfInterest GetShapeOfInterest(Rectangle fovRect, bool checkNeighbor)
        {
            PointF fovCenter = DrawingHelper.CenterPoint(fovRect);
            List<ShapeInfo> shapInfoList = this.ShapeInfoList.OrderBy(f => MathHelper.GetLength(fovCenter, DrawingHelper.CenterPoint(f.BaseRect))).ToList();

            TeachInfo teachInfo = this.TeachInfo;
            foreach (ShapeInfo shapeInfo in shapInfoList)
            {
                if (checkNeighbor)
                {
                    if (Array.TrueForAll(shapeInfo.Neighborhood, f => (f != null)) == false)
                        continue;

                    // 주변 패턴 중 적어도 두개는 동일 패턴이어야 한다.
                    int same = shapeInfo.Neighborhood.Count(f => shapInfoList.Contains(f));
                    if (same < 2)
                        continue;
                }

                Rectangle rect = shapeInfo.BaseRect;
                Array.ForEach(shapeInfo.Neighborhood, f => 
                {
                    if (f != null)
                        rect = Rectangle.Union(rect, f.BaseRect);
                });
                rect.Inflate((int)(teachInfo.Feature.Margin.Width / 2), (int)(teachInfo.Feature.Margin.Height / 2));

                // fovRect와 rect가 가로/세로 비율이 맞도록 rect 크기 조절
                Point centerPt = Point.Round(DrawingHelper.CenterPoint(rect));
                //float fovRatio = (float)fovRect.Height / (float)fovRect.Width;
                float fovRatio = (float)1800 / (float)3200;
                float roiRatio = (float)rect.Height / (float)rect.Width;
                Size newRoiSize = new Size();
                if (roiRatio > fovRatio)
                {
                    newRoiSize = new Size((int)(rect.Height / fovRatio), rect.Height);
                }
                else
                {
                    newRoiSize = new Size(rect.Width, (int)(rect.Width * fovRatio));
                }
                //Debug.Assert(newRoiSize.Width <= fovRect.Width && newRoiSize.Height <= fovRect.Height);

                Rectangle aroundRect = DrawingHelper.FromCenterSize(centerPt, newRoiSize);

                //if (Rectangle.Intersect(aroundRect, fovRect) != aroundRect)
                //    continue;

                aroundRect = Rectangle.Intersect(aroundRect, fovRect);
                if (aroundRect.Width==0 || aroundRect.Height==0)
                    continue;

                return new ShapeOfInterest(aroundRect, shapeInfo);
            }

            return new ShapeOfInterest(Rectangle.Empty, null);
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            foreach (ShapeInfo shapeInfo in shapeInfoList)
                shapeInfo.Export(element, "ShapeInfo");
            teachInfo.Export(element, "TeachInfo");
            XmlHelper.SetValue(element, "Debug", debug);
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            shapeInfoList = new List<ShapeInfo>();
            XmlNodeList xmlNodeList = element.GetElementsByTagName("ShapeInfo");
            foreach (XmlElement sheetInfoElement in xmlNodeList)
            {
                ShapeInfo shapeInfo = ShapeInfo.Load(sheetInfoElement);
                shapeInfoList.Add(shapeInfo);
            }
            teachInfo = TeachInfo.Load(element, "TeachInfo");
            debug = XmlHelper.GetValue(element, "Debug", "");
        }

        internal static PatternInfoGroup Load(XmlElement xmlElement, string key = null)
        {
            PatternInfoGroup patternInfoGroup = new PatternInfoGroup();
            patternInfoGroup.Import(xmlElement, key);
            return patternInfoGroup;
        }

        internal ShapeInfo GetCenterShapeInfo(Rectangle fovRect)
        {
            PointF centerPt = DrawingHelper.CenterPoint(fovRect);
            List<ShapeInfo> orderedList = (List<ShapeInfo>)this.shapeInfoList.OrderBy(f => MathHelper.GetLength(DrawingHelper.CenterPoint(f.BaseRect), centerPt)).ToList();
            return orderedList[0];
        }
    }

    public class PatternInfoGroupList : List<PatternInfoGroup>
    {
        public void Sort()
        {
            foreach (PatternInfoGroup group in this)
            {
                group.Sort(); //이미지 좌표 x,y 오름차순 정렬
            }

            //면적 내림차순.
            this.Sort((f, g) =>
            {
                double a = f.ShapeInfoList.Count * f.ShapeInfoList.Average(x => x.Area);
                double b = g.ShapeInfoList.Count * g.ShapeInfoList.Average(x => x.Area);
                return b.CompareTo(a);
            });
        }

        public void UpdateId()
        {

            for (int i = 0; i < this.Count; i++)
                this[i].TeachInfo.ChangeId(i);
        }
    }

    public class MatchResult
    {
        PatternInfo inspPatternInfo;
        PatternInfo refPatternInfo;

        public PatternInfo InspPatternInfo
        {
            get { return inspPatternInfo; }
            set { inspPatternInfo = value; }
        }

        public PatternInfo RefPatternInfo
        {
            get { return refPatternInfo; }
            set { refPatternInfo = value; }
        }

        public MatchResult(PatternInfo inspPatternInfo, PatternInfo refPatternInfo)
        {
            this.inspPatternInfo = inspPatternInfo;
            this.refPatternInfo = refPatternInfo;
        }
    }
    
    public class ProcessResult : IExportable, IImportable
    {
        PatternInfo trainPatternInfo;
        PatternInfo inspPatternInfo;
        InspectParam inspectParam;
        Feature? offsetValue = null;

        public PatternInfo TrainPatternInfo
        {
            get { return trainPatternInfo; }
        }

        public PatternInfo InspPatternInfo
        {
            get { return inspPatternInfo; }
        }

        public InspectParam InspectParam
        {
            get { return inspectParam; }
        }

        public Feature OffsetValue
        {
            get
            {
                if (offsetValue != null)
                    return offsetValue.Value;

                if (inspPatternInfo.TeachInfo.IsValid && trainPatternInfo.TeachInfo.Inspectable)
                    return Feature.Sub(inspPatternInfo.TeachInfo.Feature, trainPatternInfo.TeachInfo.Feature);
                else
                    return new Feature();
            }
            set { offsetValue = value; }
        }

        public bool IsInspected
        {
            get { return this.inspPatternInfo.TeachInfo.IsValid && this.trainPatternInfo.TeachInfo.Inspectable; }
        }

        public static ProcessResult Load(XmlElement element, string key=null)
        {
            ProcessResult processResult = new ProcessResult();
            processResult.Import(element, key);
            return processResult;
        }

        private ProcessResult() { }

        public ProcessResult(PatternInfo trainPatternInfo, PatternInfo inspPatternInfo, InspectParam inspectParam)
        {
            this.trainPatternInfo = trainPatternInfo;
            this.inspPatternInfo = inspPatternInfo;
            this.inspectParam = inspectParam;
        }

        public bool IsBlotGood
        {
            get
            {
                if (IsInspected == false)
                    return false;

                Feature offsetValue = OffsetValue;
                float bW, bL;

                {
                    bW = Math.Abs(offsetValue.Blot.Width);
                    bL = Math.Abs(offsetValue.Blot.Height);
                }

                bool res = true //aR < inspectParam.OffsetRange.Area
                        && bW < inspectParam.OffsetRange.Blot.Width && bL < inspectParam.OffsetRange.Blot.Height;

                return res;
            }
        }

        public bool IsMarginGood
        {
            get
            {
                if (IsInspected == false)
                    return false;

                Feature offsetValue = OffsetValue;
                float mW, mL;

                if (inspectParam.IsRelativeOffset)
                {
                    mW = (Math.Abs(offsetValue.Margin.Width) / trainPatternInfo.TeachInfo.Feature.Margin.Width) * 100;
                    mL = (Math.Abs(offsetValue.Margin.Height) / trainPatternInfo.TeachInfo.Feature.Margin.Height) * 100;
                }
                else
                {
                    mW = Math.Abs(offsetValue.Margin.Width);
                    mL = Math.Abs(offsetValue.Margin.Height);
                }

                bool res = true //aR < inspectParam.OffsetRange.Area
                        && mW < inspectParam.OffsetRange.Margin.Width && mL < inspectParam.OffsetRange.Margin.Height;

                return res;
            }
        }


        public bool IsGood
        {
            get
            {
                if (IsInspected == false)
                    return false;

                Feature offsetValue = OffsetValue;
                float aR, bW, bL, mW, mL;

                if (inspectParam.IsRelativeOffset)
                {
                    aR = (Math.Abs(offsetValue.Area) / trainPatternInfo.TeachInfo.Feature.Area) * 100;
                    mW = (Math.Abs(offsetValue.Margin.Width) / trainPatternInfo.TeachInfo.Feature.Margin.Width) * 100;
                    mL = (Math.Abs(offsetValue.Margin.Height) / trainPatternInfo.TeachInfo.Feature.Margin.Height) * 100;
                }
                else
                {
                    aR = Math.Abs(offsetValue.Area);
                    mW = Math.Abs(offsetValue.Margin.Width);
                    mL = Math.Abs(offsetValue.Margin.Height);
                }
                bW = Math.Abs(offsetValue.Blot.Width);
                bL = Math.Abs(offsetValue.Blot.Height);

                bool res = true //aR < inspectParam.OffsetRange.Area
                        && mW < inspectParam.OffsetRange.Margin.Width && mL < inspectParam.OffsetRange.Margin.Height
                        && bW < inspectParam.OffsetRange.Blot.Width && bL < inspectParam.OffsetRange.Blot.Height;

                return res;
            }
        }

        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            this.trainPatternInfo.Export(element, "TrainPatternInfo");
            
            this.inspPatternInfo.Export(element, "InspPatternInfo");
            
            this.inspectParam.Export(element, "InspParam");
        }
        
        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }            
            this.trainPatternInfo = PatternInfo.Load(element, "TrainPatternInfo");
            
            this.inspPatternInfo = PatternInfo.Load(element, "InspPatternInfo");

            this.inspectParam = InspectParam.Load(element, "InspParam");
        }
    }

    public class ProcessResultList : IExportable, IImportable
    {
        ImageD image = null;
        List<ProcessResult> processResultList = null;
        List<Rectangle> defectRectList = null;
        int interestResultId = -1;

        public ImageD Image
        {
            get { return image; }
        }

        public List<ProcessResult> ResultList
        {
            get { return processResultList; }
        }

        public List<Rectangle> DefectRectList
        {
            get {
                Debug.WriteLine("▣ ## DefectRectList------");
                return defectRectList;
            }
        }
        
        public int InterestResultId
        {
            get { return interestResultId; }
            set { interestResultId = value; }
        }

        public ProcessResult InterestProcessResult
        {
            get { return (interestResultId < 0 ? null : processResultList[this.interestResultId]); }
        }

        private ProcessResultList() { }

        public ProcessResultList(ImageD image)
        {
            this.image = image;
            this.processResultList = new List<ProcessResult>();
            this.defectRectList = new List<Rectangle>();
        }

        public void SetImage(AlgoImage algoImage)
        {
            //if (image.Size != algoImage.Size)
            //    return;

            image = algoImage.ToImageD();
        }

        public void Export(XmlElement element, string subKey=null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "InterestResultIndex", interestResultId.ToString());
            foreach (ProcessResult processResult in this.processResultList)
            {
                processResult.Export(element, "ProcessResult");
            }
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            interestResultId = Convert.ToInt32(XmlHelper.GetValue(element, "InterestResultIndex", "-1"));

            XmlNodeList nodeList = element.GetElementsByTagName("ProcessResult");
            foreach(XmlElement subElement in nodeList)
            {
                ProcessResult processResult = ProcessResult.Load(subElement);
                this.processResultList.Add(processResult);
            }
        }

        public static ProcessResultList Load(XmlElement xmlElement, string key=null)
        {
            ProcessResultList processResultList = new ProcessResultList();
            processResultList.Import(xmlElement, key);
            return processResultList;
        }

        public Rectangle GetMaxSizeDefectRect()
        {
            Debug.WriteLine("▣ GetMaxSizeDefectRect");
            try
            {
                Func<Rectangle, double> func = new Func<Rectangle, double>(f => Math.Sqrt(f.Width * f.Width + f.Height * f.Height));
                int nCount = this.defectRectList.Count;
                if (this.defectRectList.Count == 0)
                    return Rectangle.Empty;

                double maxDefLength = this.defectRectList.Max(func); //todo msong
                return this.defectRectList.Find(f => func(f) == maxDefLength);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("-----"+ ex.ToString());
                return Rectangle.Empty;
            }
            finally
            {

            }
        }

        //public void Save(string path)
        //{
        //    AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
        //    algoImage.Save(Path.Combine(path, "Processed_Image.bmp"));
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
        //    //imageProcessing.DrawRect(algoImage, Rectangle.FromLTRB(100, 100, algoImage.Width - 100, algoImage.Height - 100), 128, false);
        //    foreach (KeyValuePair<Features, ProcessResultItem> pair in this)
        //    {
        //        Features key = pair.Key;
        //        ProcessResultItem value = pair.Value;
        //        imageProcessing.DrawRect(algoImage, key.s.rectangle, 128, false);
        //    }

        //    algoImage.Save(Path.Combine(path, "Processed_Rect.bmp"));
        //    algoImage.Dispose();
        //}
    }

    public struct ProcessResultItem
    {
        int margineL;
        int margineW;
        int blotL;
        int blotW;

        public int MargineL
        {
            get { return margineL; }
        }

        public int MargineW
        {
            get { return margineW; }
        }

        public int BlotL
        {
            get { return blotL; }
        }

        public int BlotW
        {
            get { return blotW; }
        }

        public ProcessResultItem(int margineL, int margineW, int blotL, int blotW)
        {
            this.margineL = margineL;
            this.margineW = margineW;
            this.blotL = blotL;
            this.blotW = blotW;
        }
    }
}
