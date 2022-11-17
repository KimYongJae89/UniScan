using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using Infragistics.Documents.Excel;
using UniEye.Base.Settings;
using UniScanM.Pinhole.Settings;

namespace UniScanM.Pinhole.Data
{
    public enum DefectType
    {
        Invaild = -1, 
        Pinhole, Dust, ReCheckTop,ReCheckBottom
    }

    public class DefectInfo
    {
        int defectIndex; // 전체 불량 개수
        int cameraIndex;
        int sectionIndex;
        int defectNo; // 섹션 내의 불량 번호
        int minValue;
        int maxValue;

        PointF pixelPosition;
        PointF realPosition;
        DefectType defectType;
        RectangleF boundingRect;
        RectangleF realRect;
        Bitmap clipImage;
        string path;

        string pvPos = "";

        RectangleF interestedRect;
        public RectangleF InterestedRect { get => interestedRect; set => interestedRect = value; }

        public int DefectIndex { get => defectIndex; set => defectIndex = value; }
        public PointF PixelPosition { get => pixelPosition; }
        public PointF RealPosition { get => realPosition; }
        public int SectionIndex { get => sectionIndex; }
        public int CameraIndex { get => cameraIndex; }
        public DefectType DefectType { get => defectType; }
        public RectangleF BoundingRect { get => boundingRect; }
        public Bitmap ClipImage { get => clipImage; set => clipImage = value; }
        public int DefectNo { get => defectNo; }
        public string Path { get => path; set => path = value; }
        public string PvPos { get => pvPos; set => pvPos = value; }

        public DefectInfo(int cameraIndex, int sectionIndex, int defectNo, RectangleF boundingRect, PointF pixelPosition, PointF realPosition, DefectType defectType, int minValue, int maxValue, Bitmap clipImage)
        {
            this.cameraIndex = cameraIndex;
            this.sectionIndex = sectionIndex;
            this.defectNo = defectNo;
            this.boundingRect = boundingRect;
            this.pixelPosition = pixelPosition;
            this.realPosition = realPosition;
            this.defectType = defectType;
            this.clipImage = clipImage;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public DefectInfo(int cameraIndex, int sectionIndex, int defectNo, RectangleF boundingRect, PointF pixelPosition, PointF realPosition, DefectType defectType, string path, string pvPos)
        {
            this.cameraIndex = cameraIndex;
            this.sectionIndex = sectionIndex;
            this.defectNo = defectNo;
            this.boundingRect = boundingRect;
            this.pixelPosition = pixelPosition;
            this.realPosition = realPosition;
            this.defectType = defectType;
            this.path = path;
            this.pvPos = pvPos;
        }

        public DefectInfo Clone()
        {
            return new DefectInfo(cameraIndex, sectionIndex, defectNo, boundingRect, pixelPosition, realPosition, defectType, minValue, maxValue, clipImage);
        }

        public string GetImageFileName()
        {
            return String.Format("S{0:000000}_C{1}_D{2:000}.bmp", SectionIndex, CameraIndex + 1, DefectNo);
        }

        public override string ToString()
        {
            return String.Format("x : {0}\ny : {1}\nw : {2}, h : {3}\nmin : {4}\nmax : {5}", realPosition.X, realPosition.Y, boundingRect.Width, boundingRect.Height, minValue, maxValue);
        }

        public Figure GetDefectMark(Color defectColor, float resizeRatio)
        {
            RectangleF rectangleF = RectangleF.Empty;
            rectangleF.X = boundingRect.X * resizeRatio;
            rectangleF.Y = boundingRect.Y * resizeRatio;
            rectangleF.Width = boundingRect.Width * resizeRatio;
            rectangleF.Height = boundingRect.Height * resizeRatio;
            return new CrossFigure(rectangleF, new Pen(defectColor, PinholeSettings.Instance().DefectPenSize));
        }
    }

    public class DefectInfoList : List<DefectInfo>
    {
    }

    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        //const int MaxSubItem = 100000;
        const int MaxSubItem = 1000;

        int deviceIndex;
        int sectionIndex;
        
        int numDefect;
        int defectIndex;
        DefectInfoList lastDefectInfoList = new DefectInfoList();
        RectangleF interestRegion;
        RectangleF skipRegion;
 
        public int DeviceIndex { get => deviceIndex; set => deviceIndex = value; }
        public int SectionIndex { get => sectionIndex; set => sectionIndex = value; }
        public ImageFormat Imageformat { get; private set; }
        public int NumDefect { get => numDefect; set => numDefect = value; }
        public DefectInfoList LastDefectInfoList { get => lastDefectInfoList; set => lastDefectInfoList = value; }
        public RectangleF InterestRegion { get => interestRegion; set => interestRegion = value; }
        public RectangleF SkipRegion { get => skipRegion; set => skipRegion = value; }

        static System.IO.StreamWriter dataFile = null;
        static System.IO.StreamWriter collectorFile = null;

        static int totalDefectCount;
        public static int TotalDefectCount
        {
            get
            {
                if(totalDefectCount > short.MaxValue)
                {
                    totalDefectCount = 0;
                }
                return totalDefectCount;
            }
        }
                
            
        public new DefectInfo this[int key]
        {
            get
            {
                return lastDefectInfoList[key];
            }
        }

        public InspectionResult()
        {

        }

        public string GetDispImageNameOld()
        {
            return String.Format("Image_{0}_{1:000000}.bmp", this.DeviceIndex, this.SectionIndex);
        }

        public string GetDispImageName()
        {
            return String.Format("Image_S{0:000000}_C{1}_.bmp", this.SectionIndex, this.DeviceIndex + 1);
        }

        public void AddDefectInfo(Data.InspectionResult inspectionResult)
        {
//            lock (this)
            {
                lastDefectInfoList.AddRange(inspectionResult.lastDefectInfoList.ToArray());

                numDefect = lastDefectInfoList.Count;
            }
        }

        public void AddDefectInfo(DefectInfo defectInfo)
        {
//            lock (this)
            {
                AddDefectInfoInner(defectInfo);
            }
        }

        public void AddDefectInfoInner(DefectInfo defectInfo) 
        {
            lastDefectInfoList.Add(defectInfo);
            numDefect++;
        }


        public int GetDefectCount(DefectType defectType)
        {
            int defectCount = 0;
            foreach(DefectInfo defectInfo in lastDefectInfoList)
            {
                if (defectInfo.DefectType == defectType)
                    defectCount++;
            }
            return defectCount;
        }

        public int GetTotalDefectCount()
        {
            return lastDefectInfoList.Count;
        }

        public override bool IsGood()
        {
            return this.Judgment == Judgment.Accept;
        }
    }
}
