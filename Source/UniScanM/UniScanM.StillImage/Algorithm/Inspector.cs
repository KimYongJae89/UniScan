using System;
using System.Collections.Generic;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using UniScanM.StillImage.Data;
using System.Drawing;
using System.IO;
using UniEye.Base.Settings;
using System.Windows;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class Inspector
    {
        public static Inspector Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new InspectorV1_BCI();
                case 1:
                    return new InspectorV2_TCI();
                default:
                    throw new Exception("Version is invalid");
            }
        }
        public abstract void Inspect(AlgoImage fovImage, InspectParam inspectParam, InspectionResult inspectionResult);
    }

    public class InspectorV1_BCI : Inspector
    {
        protected FeatureExtractor featureExtractor = null;

        public InspectorV1_BCI() : base()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }

        public override void Inspect(AlgoImage inspImage, InspectParam inspectParam, InspectionResult inspectionResult)
        {
            inspectionResult.SetGood();
            TeachData teachData = inspectionResult.TeachData;
            List<MatchResult> matchResultList = featureExtractor.Match(inspImage, teachData, inspectParam.MatchRatio);

            List<ProcessResult> processResults = new List<ProcessResult>();
            List<Rectangle> defectResults = new List<Rectangle>();
            foreach (MatchResult matchResult in matchResultList)
            {
                PatternInfo inspPatternInfo = matchResult.InspPatternInfo;
                PatternInfo refPatternInfo = matchResult.RefPatternInfo;
                if (refPatternInfo != null)
                // 티칭된 패턴 -> 패턴 Margin, Blot 검사
                {
                    if (refPatternInfo.TeachInfo.Inspectable == false)
                        continue;

                    ProcessResult patternResult = new ProcessResult(refPatternInfo, inspPatternInfo, inspectParam);
                    processResults.Add(patternResult);
                }
                else
                // 티칭되지 않은 패턴 -> 이물(Defect) 검사
                {
                    defectResults.Add(inspPatternInfo.ShapeInfo.BaseRect);
                }
            }
            ProcessResultList processResultList = new ProcessResultList(inspImage.ToImageD());
            processResultList.ResultList.AddRange(processResults);
            processResultList.DefectRectList.AddRange(defectResults);

            // 중앙에 가장 가까운 검사 패턴 찾기
            List<PatternInfo> list = processResults.ConvertAll<PatternInfo>(f => f.InspPatternInfo);
            PatternInfoGroup patternInfoGroup = new PatternInfoGroup(-1, list);
            ShapeOfInterest shapeOfInterest = patternInfoGroup.GetShapeOfInterest(new Rectangle(Point.Empty, inspImage.Size), false);

            if (shapeOfInterest.IsEmpty == false)
            {
                int ii = processResultList.ResultList.FindIndex(f => f.InspPatternInfo.ShapeInfo.Id == shapeOfInterest.ShapeInfo.Id);
                processResultList.InterestResultId = ii;
            }

            //사이즈 필터링
            //큰놈 제거
            if (inspectParam.InspectionSizeMax > 0)
            {
                int Wpix = (int)(inspectParam.InspectionSizeMax / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width + 0.5);
                int Hpix = (int)(inspectParam.InspectionSizeMax / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height + 0.5);
                processResultList.DefectRectList.RemoveAll(f => f.Width > Wpix || f.Height > Hpix);
            }
            //작은놈 제거
            if (inspectParam.InspectionSizeMin > 0)
            {
                int Wpix = (int)(inspectParam.InspectionSizeMin / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width + 0.5);
                int Hpix = (int)(inspectParam.InspectionSizeMin / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height + 0.5);
                processResultList.DefectRectList.RemoveAll(f => f.Width < Wpix && f.Height < Hpix);
            }

            //fovImage.Save(@"d:\temp\temp.bmp");
            inspectionResult.ProcessResultList = processResultList;
            inspectionResult.UpdateJudgement();
        }
    }
    public class InspectorV2_TCI : Inspector
    {
        protected FeatureExtractor featureExtractor = null;

        public InspectorV2_TCI() : base()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }
         
        public override void Inspect(AlgoImage inspImage, InspectParam inspectParam, InspectionResult inspectionResult)
        {
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, 
                Path.Combine(PathSettings.Instance().Temp, "Inspect.cs.Inspect()"+ inspectionResult.InspZone.ToString()));

            if (debugContext.SaveDebugImage)
            {
                if (Directory.Exists(debugContext.FullPath) == false)
                    Directory.CreateDirectory(debugContext.FullPath);
                string[] files = Directory.GetFiles(debugContext.FullPath);
                foreach (string file in files)
                    File.Delete(file);
            }

            inspectionResult.SetGood();
            TeachData teachData = inspectionResult.TeachData;
            //검사영역의 형상별 동일 티칭 형상 패어의 리스트, 이미지에서의 좌표(위치)는 동일하지 않음!!
            List<MatchResult> matchResultList = featureExtractor.Match(inspImage, teachData, inspectParam.MatchRatio, inspectionResult.InspZone);

            List<ProcessResult> processResults = new List<ProcessResult>();
            List<Rectangle> defectResults = new List<Rectangle>();
            foreach (MatchResult matchResult in matchResultList)
            {
                PatternInfo inspPatternInfo = matchResult.InspPatternInfo;
                PatternInfo refPatternInfo = null;
                if (matchResult.RefPatternInfo != null)
                // 티칭된 패턴 -> 패턴 Margin, Blot 검사  // 주패턴외에도 있음.
                {
                    refPatternInfo = matchResult.RefPatternInfo;
                    if (refPatternInfo.TeachInfo.Inspectable == false)
                        continue;

                    ProcessResult patternResult = new ProcessResult(refPatternInfo, inspPatternInfo, inspectParam);
                    processResults.Add(patternResult);
                }
                else 
                // 티칭되지 않은 패턴 -> 이물(Defect) 검사
                {
                    defectResults.Add(inspPatternInfo.ShapeInfo.BaseRect);
                }
            }
            //엣지영역 제외시키기 // 이미지의 경계에 걸친 블랍은 제외.
            int eX = inspImage.Width;// - 1; //하 참....
            int eY = inspImage.Height;// - 1;
            var innn = defectResults.RemoveAll(f => (f.Left == 0 | f.Right == eX | f.Top == 0 | f.Bottom == eY));

            //♥ Subtraction 검사하기 ♥
            BlobRectList blobRectList = inspectSubtraction(inspImage, matchResultList, inspectionResult, inspectParam, debugContext);
            var defectslist = blobRectList.GetList();
            int i = 0;
            foreach(var blob in defectslist) //
            {
                blob.LabelNumber = ++i;
            }
            foreach (var rect in defectResults) // 이물도 합치기...
            {
                BlobRect blob = new BlobRect();
                blob.BoundingRect = rect;
                blob.LabelNumber = ++i;
                defectslist.Add(blob);
            }
            //todo 디펙 근거리는 한 묶음으로 머지. 
            defectslist = Merge(defectslist, 10); //todo param

            if (debugContext.SaveDebugImage)
            {
                AlgoImage tempImage = inspImage.Clone();
                OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(inspImage) as OpenCvImageProcessing;
                foreach (var blob in defectslist)
                {
                    var rc = Rectangle.Round(blob.BoundingRect);
                    rc.Inflate(5, 5);
                    ipc.DrawRect(tempImage, rc, 128, false);
                }
                tempImage.Save("merged_defect.bmp", debugContext);
                tempImage.Dispose();
            }

            //디펙 결과에 반영.
            foreach (BlobRect blob in defectslist)
            {
                defectResults.Add( Rectangle.Round(blob.BoundingRect) );
            }            
           
            /////
            ProcessResultList processResultList = new ProcessResultList(inspImage.ToImageD());
            processResultList.ResultList.AddRange(processResults);
            processResultList.DefectRectList.AddRange(defectResults);

            // 중앙에 가장 가까운 검사 패턴 찾기
            List<PatternInfo> list = processResults.ConvertAll<PatternInfo>(f => f.InspPatternInfo);
            PatternInfoGroup patternInfoGroup = new PatternInfoGroup(-1, list);
            ShapeOfInterest shapeOfInterest = patternInfoGroup.GetShapeOfInterest(new Rectangle(Point.Empty, inspImage.Size), false);

            if (shapeOfInterest.IsEmpty == false)
            {
                int ii = processResultList.ResultList.FindIndex(f => f.InspPatternInfo.ShapeInfo.Id == shapeOfInterest.ShapeInfo.Id);
                processResultList.InterestResultId = ii; //***************************************************************
            }

            //사이즈 필터링
            //큰놈 제거
            if (inspectParam.InspectionSizeMax > 0)
            {
                int Wpix = (int)(inspectParam.InspectionSizeMax / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width +0.5);
                int Hpix = (int)(inspectParam.InspectionSizeMax / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height +0.5);
                processResultList.DefectRectList.RemoveAll(f => f.Width > Wpix || f.Height > Hpix);
            }
            //작은놈 제거
            if (inspectParam.InspectionSizeMin > 0)
            {
                int Wpix = (int)(inspectParam.InspectionSizeMin / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width + 0.5);
                int Hpix = (int)(inspectParam.InspectionSizeMin / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height + 0.5);
                processResultList.DefectRectList.RemoveAll(f => f.Width < Wpix && f.Height < Hpix);
            }

            if (debugContext.SaveDebugImage)
            {
                AlgoImage tempImage = inspImage.Clone();
                OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(inspImage) as OpenCvImageProcessing;
                foreach (var rect in processResultList.DefectRectList)
                {
                    var rc = Rectangle.Round(rect);
                    rc.Inflate(5, 5);
                    ipc.DrawRect(tempImage, rc, 128, false);
                }
                tempImage.Save("Remove_Small_Defect.bmp", debugContext);
                tempImage.Dispose();
            }

            //fovImage.Save(@"d:\temp\temp.bmp");
            inspectionResult.ProcessResultList = processResultList;
            inspectionResult.UpdateJudgement();
        }

        private BlobRectList inspectSubtraction(AlgoImage inspImage, List<MatchResult> MatchResultlist, InspectionResult inspectionResult, 
            InspectParam inspectParam, DebugContext debugContext)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(inspImage) as OpenCvImageProcessing;
            var OriginImage = inspImage;
            // inspectionResult.InspRectInSheet;
            // inspectionResult.SheetRectInFrame;

           // int minSize         = inspectParam.InspectionSizeMin;
            int edgeThreshold   = inspectParam.InspectionRefEdgeLevel;
            float fInspectmin   = inspectParam.InspectionLevelMin;
            float fInspectmax   = inspectParam.InspectionLevelMax;

            AlgoImage ReferenceImage        = inspectionResult.TeachData.MajorElectrodeImage;
            AlgoImage ReferenceEdgeImage    = inspectionResult.TeachData.MajorElectrodeEdgeImage;
            var ReferenceEdgeImageBiynay    = ReferenceEdgeImage.Clone();
            edgeThreshold                   = (int)(ipc.GetBinarizeValue(ReferenceEdgeImage)+0.5);

            ipc.Binarize(ReferenceEdgeImageBiynay, ReferenceEdgeImageBiynay , edgeThreshold, true);
            ipc.Erode(ReferenceEdgeImageBiynay, 2);
            ReferenceEdgeImageBiynay.Save("ReferenceEdgeImageBiynay.bmp", debugContext);

            var DiffAbsImage = ImageBuilder.BuildSameTypeSize(OriginImage);
            DiffAbsImage.Clear(0);
            var InspThresholdImage = ImageBuilder.BuildSameTypeSize(OriginImage);
            InspThresholdImage.Clear(255);

            int sX, sY, eX, eY;
            int ccc = 0;
            int dX = 30; //patmat search range x  //todo param
            int dY = 30; //patmat search range y  //todo param

            int RoiW = ReferenceEdgeImage.Width + dX;
            int RoiH = ReferenceEdgeImage.Height + dY;

            var RefThresholdImage = ipc.MakeThresholdImage(ReferenceImage, fInspectmin, fInspectmax); //검사에 쓸 문턱값

            //ref edge 이미지와  inspImage edge 이미지와 패턴매칭후 최적의 위치를 찾고
            //해당 위치에서 Ref이미지와 insp이미지 밝기 보정후 -> 차연산
            //해당위치에서 InspThresholdImage
            foreach (var match in MatchResultlist)
            {
                if (match.RefPatternInfo == null || match.RefPatternInfo.TeachInfo.Id != 0)//todo  주패턴만 처리
                    continue;

                var objCentPt = match.InspPatternInfo.ShapeInfo.CenterPT;
               
                ccc++;
                sX = (int)(objCentPt.X - (float)RoiW / 2 + 0.5f);
                sY = (int)(objCentPt.Y - (float)RoiH / 2 + 0.5f);
                eX = sX + RoiW;
                eY = sY + RoiH;
                if (sX < 0 || sY < 0 || eX >= OriginImage.Width || eY >= OriginImage.Height)//영역을 벗어나면 스킵
                    continue;
                Rectangle rc = new Rectangle(sX, sY, RoiW, RoiH);

                ///////////////위치보정후////////////////////////////////////////////////////////////////////////////////
                var srcImage = OriginImage.GetSubImage(rc);
                var sobelimage = srcImage.Clone();
                ipc.Sobel(srcImage, sobelimage);
                Point pt = ipc.MatchTemplatePos(sobelimage, ReferenceEdgeImage);

                sX += pt.X;
                sY += pt.Y;
                eX = sX + ReferenceEdgeImage.Width;
                eY = sY + ReferenceEdgeImage.Height;
                if (sX < 0 || sY < 0 || eX >= inspImage.Width || eY >= inspImage.Height) //영역을 벗어나면 스킵 // 여긴 논리적으로 걸릴수 없다.
                    continue;
                /////////////밝기 보정/////////////////////////////////////////////////////////////////////////////////
                Rectangle rc2 = new Rectangle(sX, sY, ReferenceEdgeImage.Width, ReferenceEdgeImage.Height);
                srcImage = OriginImage.GetSubImage(rc2);
                var destImage = DiffAbsImage.GetSubImage(rc2);
                float srcAvg = ipc.GetGreyAverage(srcImage);
                float refAvg = ipc.GetGreyAverage(ReferenceImage);
                var CalRefImage = ReferenceImage.Clone();
                ipc.Mul(ReferenceImage, CalRefImage, srcAvg/refAvg);
                ////////////차이값 계산/////////////////////////////////////////////////////////////////////////////////
                ipc.Subtract(srcImage, CalRefImage, destImage, true);
                ipc.And(destImage, ReferenceEdgeImageBiynay, destImage);
                ///////////동적문턱값 //////////////////////////////////////////////////////////////////////////////////
                var roi = InspThresholdImage.GetSubImage(rc2);
                roi.Copy(RefThresholdImage);               

            }//foreach (var match in MatchResultlist)

            InspThresholdImage.Save("InspThresholdImage.bmp", debugContext);
            DiffAbsImage.Save("DiffAbsImage.bmp", debugContext);
            //ipc.Binarize(DiffAbsImage, DiffAbsImage, inspectionLevel); 
            ipc.Binarize(DiffAbsImage, DiffAbsImage, InspThresholdImage);
            DiffAbsImage.Save("DiffAbsImageBinary.bmp", debugContext);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;
            BlobRectList result = ipc.Blob(DiffAbsImage, blobParam);

            // remove small defect //작거나 큰 디펙 지우기는 상위에서 처리 
            //result.GetList().RemoveAll(f => f.Area < minSize);
            //result.GetList().RemoveAll(f => f.BoundingRect.Width < minSize || f.BoundingRect.Height < minSize);
            
            if (debugContext.SaveDebugImage)
            {
                inspImage.Save("inspImage.bmp", debugContext);
                AlgoImage tempImage = DiffAbsImage.Clone();
                foreach (BlobRect blob in result.GetList())
                {
                    var rc = Rectangle.Round(blob.BoundingRect);
                    rc.Inflate(5,5);
                    ipc.DrawRect(tempImage, rc, 128, false);
                }
                tempImage.Save("DiffAbsImageBinary_Defect.bmp", debugContext);
                tempImage.Dispose();
            }

            return result;
        }

        private List<BlobRect> Merge(List<BlobRect> blobList, int radius)
        {
            foreach (var blob in blobList)
            {
                //blob.BoundingRect.Inflate(radius, radius);
                blob.BoundingRect = RectangleF.Inflate(blob.BoundingRect, radius, radius);
            }

            int mergeCount = 0;
            do
            {
                mergeCount = 0;
                foreach (var blob1 in blobList)
                {
                    if (blob1.LabelNumber == -1) continue;

                    foreach (var blob2 in blobList)
                    {
                        if (blob1.LabelNumber == blob2.LabelNumber) continue;
                        if (blob2.LabelNumber == -1) continue;

                        RectangleF rc1 = blob1.BoundingRect;
                        RectangleF rc2 = blob2.BoundingRect;
                        if (blob1.BoundingRect.IntersectsWith(blob2.BoundingRect))
                        {
                            blob1.BoundingRect = RectangleF.Union(blob1.BoundingRect, blob2.BoundingRect);
                            blob2.LabelNumber = -1;
                            mergeCount++;
                        }
                    }
                }
            } while(mergeCount > 0);

            blobList.RemoveAll(f => f.LabelNumber == -1);

            foreach (var blob in blobList)
            {
                //blob.BoundingRect.Inflate(-radius, -radius);
                blob.BoundingRect = RectangleF.Inflate(blob.BoundingRect, -radius, -radius);
            }
            return blobList;

        }//merge
    }
}
