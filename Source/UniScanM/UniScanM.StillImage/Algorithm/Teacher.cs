using DynMvp.Vision;
using UniScanM.StillImage.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using DynMvp.Base;
using DynMvp.Vision.OpenCv;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class Teacher 
    {
        protected FeatureExtractor featureExtractor = null;

        public static Teacher Create(int version)
        {
            switch (version)
            {
                case 0:
                    //BCI(Blob Comparison Inspection)
                    return new TeacherV1_BCI();
                case 1:
                    //TCI(Template Comparison Inspection) 정지화상 + 규칙성을 몰라도, Bolb 위치 잘못되면 검사도.
                    return new TeacherV2_TCI();

            }
            return null;
        }
        
        public abstract void Teach(AlgoImage algoImage, InspectionResult inspectionResult);
    }
    public class TeacherV1_BCI : Teacher
    {
        public TeacherV1_BCI()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }

        public override void Teach(AlgoImage sheetImage, InspectionResult inspectionResult)
        {
            TeachData teachData = featureExtractor.Extract(sheetImage);
            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);
            if (teachData.TeachDone)
            {
                PatternInfoGroup majorPatternInfoGroup = teachData.PatternInfoGroupList.Find(f => f.TeachInfo.Inspectable);
                if (majorPatternInfoGroup != null)
                {
                    float inspSizeW = Math.Min(imageRect.Width, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Width) * 3);
                    float inspSizeH = Math.Min(imageRect.Height, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Height) * 3);
                    float sizeN = Math.Max(inspSizeW, inspSizeH);
                    float size = sizeN < imageRect.Width ? sizeN : imageRect.Width; // 이미지 사이즈를 벗어나지 않도록
                    Size fovSize = Size.Round(new SizeF(size, size));
                    teachData.InspSize = fovSize;

                    Rectangle inspRect = SheetFinder.GetInspRect(sheetImage.Size, fovSize);

                    ShapeInfo centerShapeInfo = majorPatternInfoGroup.GetCenterShapeInfo(inspRect);

                    //int sameNeighbor = centerShapeInfo.Neighborhood.Count(f => ShapeInfo.IsSimilar(centerShapeInfo, f, 0.8f));
                    //if (sameNeighbor == 4)
                    //    teachData.IsInspectable = true;

                    bool left = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[0], 0.8f);
                    bool right = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[2], 0.8f);
                    teachData.IsInspectable = left && right;

                    if (centerShapeInfo != null)
                    {
                        Point newCenterPt = Point.Round(DrawingHelper.CenterPoint(centerShapeInfo.BaseRect));
                        inspRect = Rectangle.Intersect(imageRect, DrawingHelper.FromCenterSize(newCenterPt, fovSize));
                    }
                    inspectionResult.InspRectInSheet = inspRect;

                    AlgoImage displayImage = sheetImage.GetSubImage(inspRect);
                    inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                    displayImage.Dispose();

                    inspectionResult.SetGood();
                }
            }

            if (inspectionResult.InspRectInSheet.IsEmpty)
            {
                Point clipRectCenter = SheetFinder.GetInspCenter(sheetImage.Size);
                int aSize = Math.Min(sheetImage.Width, sheetImage.Height);
                Rectangle clipRect = Rectangle.Round(DrawingHelper.FromCenterSize(clipRectCenter, new Size(aSize, aSize)));
                clipRect.Intersect(imageRect);
                inspectionResult.InspRectInSheet = clipRect;

                AlgoImage displayImage = sheetImage.GetSubImage(clipRect);
                inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                displayImage.Dispose();

                inspectionResult.SetDefect();
            }

            inspectionResult.TeachData = teachData;
        }
    }
    public class TeacherV2_TCI : Teacher
    {
        public TeacherV2_TCI()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }

        public override void Teach(AlgoImage sheetImage, InspectionResult inspectionResult)
        {
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, 
                Path.Combine(PathSettings.Instance().Temp, "Teacher.cs.Teach()" + inspectionResult.InspZone.ToString()));
            if (debugContext.SaveDebugImage)
            {
                if (Directory.Exists(debugContext.FullPath) == false)
                    Directory.CreateDirectory(debugContext.FullPath);
                string[] files = Directory.GetFiles(debugContext.FullPath);
                foreach (string file in files)
                    File.Delete(file);
            }

            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(sheetImage) as OpenCvImageProcessing;

            TeachData teachData = featureExtractor.Extract(sheetImage, inspectionResult.InspZone);
            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);
            if (teachData.TeachDone)
            {
                //면적과 허리길이로 0.1 마진으로 그룹핑된것..// 상위 List >>면적 내림차순. //하위 List >> 이미지 좌표 x,y 오름차순 정렬
                // 위와 같이 정렬된 첫번째 그룹은 = Major Electrode....(주요-인쇄전극 형상임)
                PatternInfoGroup majorPatternInfoGroup = teachData.PatternInfoGroupList.Find(f => f.TeachInfo.Inspectable);
                if (majorPatternInfoGroup != null)
                {
                    float inspSizeW = Math.Min(imageRect.Width, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Width) * 3);
                    float inspSizeH = Math.Min(imageRect.Height, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Height) * 3);
                    float sizeN = Math.Max(inspSizeW, inspSizeH);
                    float size = sizeN < imageRect.Width ? sizeN : imageRect.Width; // 이미지 사이즈를 벗어나지 않도록
                    Size fovSize = Size.Round(new SizeF(size, size));
                    teachData.InspSize = fovSize;

                    //주전극 평균 이미지 구하기 ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
                    teachData.MajorElectrodeImage = majorPatternInfoGroup.MakeAverageShapeImage(sheetImage);
                    //todo 주전근 엣지 이미지 구하기
                    teachData.MajorElectrodeEdgeImage = teachData.MajorElectrodeImage.Clone();
                    ipc.Sobel(teachData.MajorElectrodeImage, teachData.MajorElectrodeEdgeImage);

                    //todo 주전극 엣지 이진화 이미지 구하기XXXX
                    if (debugContext.SaveDebugImage)
                    {
                        teachData.MajorElectrodeImage.Save("MajorElectrodeImage.bmp",debugContext);
                        teachData.MajorElectrodeEdgeImage.Save("MajorElectrodeEdgeImage.bmp", debugContext);
                    }

                    //가로 가운데, 세로 설정된 위치를 중심으로하는 FOV size 크기 Rect 설정
                    Rectangle inspRect = SheetFinder.GetInspRect(sheetImage.Size, fovSize);
                    // 위 inspRect위치의 중심에 가장 가까운 majorPattern의 ShapeInfo 선정
                    ShapeInfo centerShapeInfo = majorPatternInfoGroup.GetCenterShapeInfo(inspRect);

                    //int sameNeighbor = centerShapeInfo.Neighborhood.Count(f => ShapeInfo.IsSimilar(centerShapeInfo, f, 0.8f));
                    //if (sameNeighbor == 4)
                    //    teachData.IsInspectable = true;

                    bool left = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[0], 0.8f);
                    bool right = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[2], 0.8f);
                    teachData.IsInspectable = left && right;

                    if (centerShapeInfo != null)
                    {
                        Point newCenterPt = Point.Round(DrawingHelper.CenterPoint(centerShapeInfo.BaseRect));
                        inspRect = Rectangle.Intersect(imageRect, DrawingHelper.FromCenterSize(newCenterPt, fovSize));
                    }
                    inspectionResult.InspRectInSheet = inspRect;

                    AlgoImage displayImage = sheetImage.GetSubImage(inspRect);
                    inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                    displayImage.Dispose();

                    inspectionResult.SetGood();
                }//if (majorPatternInfoGroup != null)
            }//if (teachData.TeachDone)

            if (inspectionResult.InspRectInSheet.IsEmpty)//검사영역 확정이 안되면!! 그냥 가운데 Width by width 사각형 영역을 따서 취함
            {
                Point clipRectCenter = SheetFinder.GetInspCenter(sheetImage.Size);
                int aSize = Math.Min(sheetImage.Width, sheetImage.Height);
                Rectangle clipRect = Rectangle.Round(DrawingHelper.FromCenterSize(clipRectCenter, new Size(aSize, aSize)));
                clipRect.Intersect(imageRect);
                inspectionResult.InspRectInSheet = clipRect;

                AlgoImage displayImage = sheetImage.GetSubImage(clipRect);
                inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                displayImage.Dispose();

                inspectionResult.SetDefect();
            }
            //티칭 데이터 넘김(저장)
            inspectionResult.TeachData = teachData;
        }
    }
}
