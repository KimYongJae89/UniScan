using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.UI;
using System.Xml;
using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System.Diagnostics;

namespace DynMvp.Vision
{
    public class BlobBox
    {
        int size = 0;

        public bool selectCenterPt = false;

        public double[] leftArray = new double[0];
        public double[] rightArray = new double[0];
        public double[] topArray = new double[0];
        public double[] bottomArray = new double[0];

        public double[] centerXArray = new double[0];
        public double[] centerYArray = new double[0];

        public double[] areaArray = new double[0];
        public double[] labelNumArray = new double[0];
        public double[] sigmaValueArray = new double[0];
        public double[] minValueArray = new double[0];
        public double[] maxValueArray = new double[0];
        public double[] meanValueArray = new double[0];
        public double[] compactnessArray = new double[0];
        public double[] convexAreaArray = new double[0];
        public double[] convexFillRatioArray = new double[0];
        public double[] aspectRetioArray = new double[0];
        public double[] rectangularityArray = new double[0];
        public double[] roughnessArray = new double[0];

        public double[] minFeretArray = new double[0];
        public double[] maxFeretArray = new double[0];
        public double[] isTouchBorder = new double[0];

        //RotateRect
        public double[] rotateRectAngle = new double[0];
        public double[] rotateRectX1 = new double[0];
        public double[] rotateRectX2 = new double[0];
        public double[] rotateRectX3 = new double[0];
        public double[] rotateRectX4 = new double[0];
        public double[] rotateRectY1 = new double[0];
        public double[] rotateRectY2 = new double[0];
        public double[] rotateRectY3 = new double[0];
        public double[] rotateRectY4 = new double[0];
        public double[] rotateRectWidth = new double[0];
        public double[] rotateRectHeight = new double[0];
        public double[] rotateRectCenterX = new double[0];
        public double[] rotateRectCenterY = new double[0];
        public MIL_INT[] numberOfHoles = new MIL_INT[0];

        public BlobBox(int size)
        {
            Init(size);
        }

        public void Resize(int newSize)
        {
            if (this.size >= newSize)
                return;

            Init(newSize);
        }

        private void Init(int size)
        {
            this.size = size;
            leftArray = new double[size];
            rightArray = new double[size];
            topArray = new double[size];
            bottomArray = new double[size];

            centerXArray = new double[size];
            centerYArray = new double[size];

            areaArray = new double[size];
            labelNumArray = new double[size];
            sigmaValueArray = new double[size];
            minValueArray = new double[size];
            maxValueArray = new double[size];
            meanValueArray = new double[size];
            compactnessArray = new double[size];
            convexAreaArray = new double[size];
            convexFillRatioArray = new double[size];
            aspectRetioArray = new double[size];
            rectangularityArray = new double[size];
            roughnessArray = new double[size];

            minFeretArray = new double[size];
            maxFeretArray = new double[size];

            isTouchBorder = new double[size];

            //RotateRect
            rotateRectAngle = new double[size];
            rotateRectX1 = new double[size];
            rotateRectX2 = new double[size];
            rotateRectX3 = new double[size];
            rotateRectX4 = new double[size];
            rotateRectY1 = new double[size];
            rotateRectY2 = new double[size];
            rotateRectY3 = new double[size];
            rotateRectY4 = new double[size];
            rotateRectWidth = new double[size];
            rotateRectHeight = new double[size];
            rotateRectCenterX = new double[size];
            rotateRectCenterY = new double[size];
            numberOfHoles = new MIL_INT[size];
        }

        public List<BlobRect> GetBlobRect()
        {
            List<BlobRect> blobRects = new List<BlobRect>();
            for (int i = 0; i < this.size; i++)
                blobRects.Add(GetBlobRect(i));
            return blobRects;
        }

        private BlobRect GetBlobRect(int i)
        {
            BlobRect blobRect = new BlobRect();
            blobRect.LabelNumber = (int)labelNumArray[i];
            blobRect.Area = (long)areaArray[i];
            blobRect.BoundingRect = new RectangleF((float)leftArray[i], (float)topArray[i], (float)(rightArray[i] - leftArray[i] + 1), (float)(bottomArray[i] - topArray[i] + 1));

            if (selectCenterPt)
                blobRect.CenterPt = new PointF((float)centerXArray[i], (float)centerYArray[i]);
            else
                blobRect.CenterPt = DrawingHelper.CenterPoint(blobRect.BoundingRect);
            blobRect.CalcCenterOffset();

            blobRect.SigmaValue = (float)sigmaValueArray[i];
            blobRect.MinValue = (float)minValueArray[i];
            blobRect.MaxValue = (float)maxValueArray[i];
            blobRect.MeanValue = (float)meanValueArray[i];
            blobRect.Compactness = (float)compactnessArray[i];
            blobRect.NumberOfHoles = (int)numberOfHoles[i];
            blobRect.ConvexArea = (float)convexAreaArray[i];
            blobRect.ConvexFillRatio = (float)convexFillRatioArray[i];
            blobRect.AspectRetio = (float)aspectRetioArray[i];
            blobRect.Rectangularity = (float)rectangularityArray[i];
            blobRect.Roughness = (float)roughnessArray[i];
            blobRect.MinFeretDiameter = (float)minFeretArray[i];
            blobRect.MaxFeretDiameter = (float)maxFeretArray[i];

            blobRect.Elongation = (float)(maxFeretArray[i] / minFeretArray[i]);
            blobRect.IsTouchBorder = isTouchBorder[i] == 1 ? true : false;

            blobRect.RotateWidth = (float)rotateRectWidth[i];
            blobRect.RotateHeight = (float)rotateRectHeight[i];
            blobRect.RotateAngle = (float)rotateRectAngle[i];
            blobRect.RotateXArray = new float[] { (float)rotateRectX1[i], (float)rotateRectX2[i], (float)rotateRectX4[i], (float)rotateRectX3[i] };
            blobRect.RotateYArray = new float[] { (float)rotateRectY1[i], (float)rotateRectY2[i], (float)rotateRectY4[i], (float)rotateRectY3[i] };
            blobRect.RotateCenterPt = new PointF((float)rotateRectCenterX[i], (float)rotateRectCenterY[i]);

            return blobRect;
        }
    }

    public class BlobRect
    {
        public float Area { get => area; set => area = value; }
        float area;

        public float Circularity { get => circularity; set => circularity = value; }
        float circularity;

        public RectangleF BoundingRect { get => boundingRect; set => boundingRect = value; }
        RectangleF boundingRect;

        public float[] RotateXArray { get => rotateXArray; set => rotateXArray = value; }
        float[] rotateXArray;

        public float[] RotateYArray { get => rotateYArray; set => rotateYArray = value; }
        float[] rotateYArray;

        public float RotateWidth { get => rotateWidth; set => rotateWidth = value; }
        float rotateWidth;

        public float RotateHeight { get => rotateHeight; set => rotateHeight = value; }
        float rotateHeight;

        public float RotateAngleRad => (float)(rotateAngle / 180 * Math.PI);
        public float RotateAngle { get => rotateAngle; set => rotateAngle = value; }
        float rotateAngle;

        public PointF RotateCenterPt { get => rotateCenterPt; set => rotateCenterPt = value; }
        PointF rotateCenterPt;

        public PointF CenterPt { get => centerPt; set => centerPt = value; }
        PointF centerPt;

        public PointF CenterOffset { get => centerOffset; set => centerOffset = value; }
        PointF centerOffset;
        
        public int LabelNumber { get => labelNumber; set => labelNumber = value; }
        int labelNumber;

        public float SigmaValue { get => sigmaValue; set => sigmaValue = value; }
        float sigmaValue;

        public float MaxValue { get => maxValue; set => maxValue = value; }
        float maxValue;

        public float MinValue { get => minValue; set => minValue = value; }
        float minValue;

        public float MeanValue { get => meanValue; set => meanValue = value; }
        float meanValue;

        public float Compactness { get => compactness; set => compactness = value; }
        float compactness;

        public float Rectangularity { get => rectangularity; set => rectangularity = value; }
        float rectangularity;

        public float Roughness { get => roughness; set => roughness = value; }
        float roughness;

        public int NumberOfHoles { get => numberOfHoles; set => numberOfHoles = value; }
        int numberOfHoles;

        public float ConvexArea { get => convexArea; set => convexArea = value; }
        float convexArea;

        public float ConvexFillRatio { get => convexFillRatio; set => convexFillRatio = value; }
        float convexFillRatio;

        public float AspectRetio { get => aspectRetio; set => aspectRetio = value; }
        float aspectRetio;

        public float MinFeretDiameter { get => minFeretDiameter; set => minFeretDiameter = value; }
        float minFeretDiameter;

        public float MaxFeretDiameter { get => maxFeretDiameter; set => maxFeretDiameter = value; }
        float maxFeretDiameter;

        public bool IsTouchBorder { get => isTouchBorder; set => isTouchBorder = value; }
        bool isTouchBorder;

        public float Elongation { get => elongation; set => elongation = value; }
        float elongation;

        public float AreaRatio
        {
            get { return this.area / (this.boundingRect.Width * this.boundingRect.Height) * 100; }
        }

        public void CalcCenterOffset()
        {
            PointF boundCenter = Base.DrawingHelper.CenterPoint(boundingRect);
            centerOffset = new PointF(boundCenter.X - centerPt.X, boundCenter.Y - centerPt.Y);
        }

        public void MoveOffset(int x, int y) { MoveOffset(new Point(x, y)); }
        public void MoveOffset(Point offset)
        {
            boundingRect.Offset(offset);
            centerPt = PointF.Add(centerPt, (Size)offset);
        }

        public static BlobRect operator +(BlobRect blobRect1, BlobRect blobRect2)
        {
            BlobRect mergeBlobRect = new BlobRect();

            float areaRatio1 = blobRect1.Area / (blobRect1.Area + blobRect2.Area);
            float areaRatio2 = 1.0f - areaRatio1;
            
            mergeBlobRect.Area = blobRect1.Area + blobRect2.Area;
            mergeBlobRect.BoundingRect = RectangleF.Union(blobRect1.BoundingRect, blobRect2.BoundingRect);
            mergeBlobRect.CenterPt = new PointF(blobRect1.CenterPt.X * areaRatio1 + blobRect2.CenterPt.X * areaRatio2, blobRect1.CenterPt.Y * areaRatio1 + blobRect2.CenterPt.Y * areaRatio2);
            //mergeBlobRect.Circularity = blobRect1.Circularity * areaRatio1 + blobRect2.Circularity * areaRatio2;
            mergeBlobRect.SigmaValue = blobRect1.SigmaValue * areaRatio1 + blobRect2.SigmaValue * areaRatio2;
            mergeBlobRect.compactness = (blobRect1.compactness + blobRect2.compactness) / 2.0f;

            mergeBlobRect.MinValue = Math.Min(blobRect1.MinValue, blobRect2.MinValue);
            mergeBlobRect.MaxValue = Math.Max(blobRect1.MaxValue, blobRect2.MaxValue);

            return mergeBlobRect;
        }

        public BlobRect Clone()
        {
            BlobRect clone = new BlobRect();
            clone.Copy(this);

            return clone;
        }

        public void Copy(BlobRect srcBlob)
        {
            this.area = srcBlob.area;
            this.circularity = srcBlob.circularity;
            this.boundingRect = srcBlob.boundingRect;
            this.rotateXArray = (float[])srcBlob.rotateXArray.Clone();
            this.rotateYArray = (float[])srcBlob.rotateYArray.Clone();
            this.rotateWidth = srcBlob.rotateWidth;
            this.rotateHeight = srcBlob.rotateHeight;
            this.rotateAngle = srcBlob.rotateAngle;
            this.rotateCenterPt = srcBlob.rotateCenterPt;
            this.centerPt = srcBlob.centerPt;
            this.centerOffset = srcBlob.centerOffset;
            this.labelNumber = srcBlob.labelNumber;
            this.sigmaValue = srcBlob.sigmaValue;
            this.minValue = srcBlob.minValue;
            this.maxValue = srcBlob.maxValue;
            this.meanValue = srcBlob.meanValue;
            this.compactness = srcBlob.compactness;
            this.rectangularity = srcBlob.rectangularity;
            this.roughness = srcBlob.roughness;
            this.numberOfHoles = srcBlob.numberOfHoles;
            this.convexArea = srcBlob.convexArea;
            this.convexFillRatio = srcBlob.convexFillRatio;
            this.aspectRetio = srcBlob.aspectRetio;

            this.minFeretDiameter = srcBlob.minFeretDiameter;
            this.maxFeretDiameter = srcBlob.maxFeretDiameter;
            this.isTouchBorder = srcBlob.isTouchBorder;
            this.elongation = srcBlob.elongation;
        }

        public virtual void SaveXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(sumElement);
                SaveXml(sumElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "area", this.area);
            XmlHelper.SetValue(xmlElement, "circularity", this.circularity);
            XmlHelper.SetValue(xmlElement, "boundingRect", this.boundingRect);
            XmlHelper.SetValue(xmlElement, "RotateXArray", this.rotateXArray);
            XmlHelper.SetValue(xmlElement, "RotateYArray", this.rotateYArray);
            XmlHelper.SetValue(xmlElement, "RotateWidth", this.rotateWidth);
            XmlHelper.SetValue(xmlElement, "RotateHeight", this.rotateHeight);
            XmlHelper.SetValue(xmlElement, "RotateAngle", this.rotateAngle);
            XmlHelper.SetValue(xmlElement, "RotateCenterPt", this.rotateCenterPt);

            XmlHelper.SetValue(xmlElement, "centerPt", this.centerPt);
            XmlHelper.SetValue(xmlElement, "centerOffset", this.centerOffset);
            XmlHelper.SetValue(xmlElement, "labelNumber", this.labelNumber);
            XmlHelper.SetValue(xmlElement, "sigmaValue", this.sigmaValue);
            XmlHelper.SetValue(xmlElement, "minValue", this.minValue);
            XmlHelper.SetValue(xmlElement, "maxValue", this.maxValue);
            XmlHelper.SetValue(xmlElement, "meanValue", this.meanValue);
            XmlHelper.SetValue(xmlElement, "compactness", this.compactness);
            XmlHelper.SetValue(xmlElement, "rectangularity", this.rectangularity);
            XmlHelper.SetValue(xmlElement, "roughness", this.roughness);
            XmlHelper.SetValue(xmlElement, "numberOfHoles", this.numberOfHoles);
            XmlHelper.SetValue(xmlElement, "ConvexArea", this.convexArea);
            XmlHelper.SetValue(xmlElement, "ConvexFillRatio", this.convexFillRatio);
            XmlHelper.SetValue(xmlElement, "aspectRetio", this.aspectRetio);

            XmlHelper.SetValue(xmlElement, "minFeretDiameter", this.minFeretDiameter);
            XmlHelper.SetValue(xmlElement, "maxFeretDiameter", this.maxFeretDiameter);
            XmlHelper.SetValue(xmlElement, "IsTouchBorder", this.isTouchBorder);
        }

        public virtual void LoadXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement[key];
                if (sumElement != null)
                    LoadXml(sumElement);
                return;
            }

            XmlHelper.GetValue(xmlElement, "area", this.area, ref this.area);
            XmlHelper.GetValue(xmlElement, "circularity", this.circularity, ref this.circularity);
            XmlHelper.GetValue(xmlElement, "boundingRect", ref this.boundingRect);
            XmlHelper.GetValue(xmlElement, "RotateXArray", ref this.rotateXArray);
            XmlHelper.GetValue(xmlElement, "RotateYArray", ref this.rotateYArray);
            XmlHelper.GetValue(xmlElement, "RotateWidth", this.rotateWidth, ref this.rotateWidth);
            XmlHelper.GetValue(xmlElement, "RotateHeight", this.rotateHeight, ref this.rotateHeight);
            XmlHelper.GetValue(xmlElement, "RotateAngle", this.rotateAngle, ref this.rotateAngle);
            XmlHelper.GetValue(xmlElement, "RotateCenterPt", ref this.rotateCenterPt);

            XmlHelper.GetValue(xmlElement, "centerPt", ref this.centerPt);
            XmlHelper.GetValue(xmlElement, "centerOffset", ref this.centerOffset);
            XmlHelper.GetValue(xmlElement, "labelNumber", this.labelNumber, ref this.labelNumber);
            XmlHelper.GetValue(xmlElement, "sigmaValue", this.sigmaValue, ref this.sigmaValue);
            XmlHelper.GetValue(xmlElement, "minValue", this.minValue, ref this.minValue);
            XmlHelper.GetValue(xmlElement, "maxValue", this.maxValue, ref this.maxValue);
            XmlHelper.GetValue(xmlElement, "meanValue", this.meanValue, ref this.meanValue);
            XmlHelper.GetValue(xmlElement, "compactness", this.compactness, ref this.compactness);
            XmlHelper.GetValue(xmlElement, "rectangularity", this.rectangularity, ref this.rectangularity);
            XmlHelper.GetValue(xmlElement, "roughness", this.roughness, ref this.roughness);
            XmlHelper.GetValue(xmlElement, "numberOfHoles", this.numberOfHoles, ref this.numberOfHoles);
            XmlHelper.GetValue(xmlElement, "ConvexArea", this.convexArea, ref this.convexArea);
            XmlHelper.GetValue(xmlElement, "ConvexFillRatio", this.convexFillRatio, ref this.convexFillRatio);
            XmlHelper.GetValue(xmlElement, "aspectRetio", this.aspectRetio, ref this.aspectRetio);

            //XmlHelper.GetValue(xmlElement, "WaistLength", this.waistLength, ref this.waistLength);

            XmlHelper.GetValue(xmlElement, "minFeretDiameter", this.minFeretDiameter, ref this.minFeretDiameter);
            XmlHelper.GetValue(xmlElement, "maxFeretDiameter", this.maxFeretDiameter, ref this.maxFeretDiameter);
            XmlHelper.GetValue(xmlElement, "IsTouchBorder", this.isTouchBorder, ref this.isTouchBorder);
        }
    }

    public class BlobRectList : IDisposable
    {
        bool isReached = false;
        public bool IsReached
        {
            get { return isReached; }
            set { isReached = value; }
        }

        private List<BlobRect> blobRectList = new List<BlobRect>();

        public int Count => blobRectList.Count;

        public BlobRect this[int i] => this.blobRectList[i];

        public List<BlobRect> GetList()
        {
            return blobRectList;
        }

        public BlobRect[] GetArray()
        {
            return blobRectList.ToArray();
        }

        public BlobRect[] GetArray(Predicate<BlobRect> predicate)
        {
            return blobRectList.FindAll(predicate).ToArray();
        }

        public void SetBlobRectList(List<BlobRect> blobRectList)
        {
            Debug.Assert(blobRectList.Min(f => f.BoundingRect.Width) > 0 && blobRectList.Min(f => f.BoundingRect.Height) > 0);
            this.blobRectList = blobRectList;
        }

        public void Append(BlobRect blobRect)
        {
            Debug.Assert(blobRect.BoundingRect.Width > 0 && blobRect.BoundingRect.Height > 0);
            blobRectList.Add(blobRect);
        }

        public void Append(List<BlobRect> blobRectList)
        {
            Debug.Assert(blobRectList.Min(f => f.BoundingRect.Width) > 0 && blobRectList.Min(f => f.BoundingRect.Height) > 0);
            this.blobRectList.AddRange(blobRectList);
        }

        public IEnumerator<BlobRect> GetEnumerator()
        {
            return blobRectList.GetEnumerator();
        }

        public void Clear()
        {
            blobRectList.Clear();
        }

        public BlobRect GetMaxAreaBlob()
        {
            if (blobRectList.Count == 0)
                return null;

            return blobRectList.OrderByDescending(x => x.Area).First();
        }

        public RectangleF GetUnionRect()
        {
            BlobRect maxBlobRect = GetMaxAreaBlob();
            if (maxBlobRect == null)
                return new RectangleF();

            RectangleF unionRect = maxBlobRect.BoundingRect;

            foreach (BlobRect blobRect in blobRectList)
            {
                unionRect = RectangleF.Union(unionRect, blobRect.BoundingRect);
            }

            return unionRect;
        }

        public virtual void Dispose()
        {
            //foreach (BlobRect blobRect in blobRectList)
            //    blobRect.Dispose();
        }

        public void MoveOffset(Point offset)
        {
            foreach (BlobRect blobRect in blobRectList)
                blobRect.MoveOffset(offset);
        }

        public void Select()
        {

        }
    }

    public class DrawBlobOption
    {
        bool selectBlob = false;
        public bool SelectBlob
        {
            get { return selectBlob; }
            set { selectBlob = value; }
        }

        bool selectBlobContour = false;
        public bool SelectBlobContour
        {
            get { return selectBlobContour; }
            set { selectBlobContour = value; }
        }

        bool selectHoles = false;
        public bool SelectHoles
        {
            get { return selectHoles; }
            set { selectHoles = value; }
        }

        bool selectHolesContour = false;
        public bool SelectHolesContour
        {
            get { return selectHolesContour; }
            set { selectHolesContour = value; }
        }
    }

    public class ResconstructParam
    {
        public bool IsGrayImage { get; set; } = false;
        public bool ForeGroundZero { get; set; } = false;
        public bool AllIncluded { get; set; } = false;
    }

    public class BlobParam
    {
        public int MaxCount { get; set; }

        public int TimeoutMs { get; set; } = -1;

        public bool SelectArea { get; set; }
        public double AreaMin { get; set; }
        public double AreaMax { get; set; }

        public bool SelectBoundingRect { get; set; }
        public double BoundingRectMinX { get; set; }
        public double BoundingRectMinY { get; set; }
        public double BoundingRectMaxX { get; set; }
        public double BoundingRectMaxY { get; set; }

        public bool SelectRotateRect { get; set; }
        public double RotateWidthMin { get; set; }
        public double RotateWidthMax { get; set; }
        public double RotateHeightMin { get; set; }
        public double RotateHeightMax { get; set; }


        public bool SelectCenterPt { get; set; }

        public bool SelectLabelValue { get; set; }

        public bool SelectNumberOfHoles { get; set; }


        public bool IsGrayScale { get; set; } = true;

        public bool SelectGrayMinValue { get; set; }

        public bool SelectGrayMeanValue { get; set; }

        public bool SelectGrayMaxValue { get; set; }

        public bool SelectSigmaValue { get; set; }
        public double SigmaMin { get; set; }

        public bool SelectCompactness { get; set; }

        public bool SelectRoughness { get; set; }

        public bool SelectAspectRatio { get; set; }

        public bool SelectRectangularity { get; set; }

        public bool SelectSawToothArea { get; set; }

        public bool EraseBorderBlobs { get; set; }

        public bool SelectBorderBlobs { get; set; }

        public bool SelectFeretDiameter { get; set; }

        public bool SelectElongation { get; set; }

        public bool SelectIsTouchBorder { get; set; }

        public bool Connectivity4 { get; set; } = true;


        public BlobParam()
        {
            MaxCount = 0;
            SelectArea = true;
            SelectBoundingRect = true;
        }
    }
}
