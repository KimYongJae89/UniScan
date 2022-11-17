using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.IO;
using System;
using UniScanM.StillImage.Data;
using DynMvp.Base;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class SheetFinder
    {
        static Dictionary<int, SheetFinder> _instance = new Dictionary<int, SheetFinder>();

        public static SheetFinder Create(int version)
        {
            SheetFinder instance = null;
            if (_instance.ContainsKey(version)==false)
            {
                switch (version)
                {
                    case 0:
                        instance = new SheetFinderV1();
                        break;
                }

                _instance.Add(version, instance);
            }
            else
            {
                instance = _instance[version];
            }
            
            return instance;
        }

        public static Point GetInspCenter(Size sheetSize)
        {
            //return GetFovRectTest(sheetSize);

            // Get FOV Conter
            float yPos = ((Data.Model)SystemManager.Instance().CurrentModel).FovYPos * sheetSize.Height;
            Point centerPoint = new Point(sheetSize.Width / 2, (int)Math.Round(yPos));

            return centerPoint;
        }

        public static Rectangle GetInspRect(Size sheetSize, Size fovSize)
        {
            Point center = SheetFinder.GetInspCenter(sheetSize);
            return DrawingHelper.FromCenterSize(center, fovSize);
        }

        private Rectangle GetFovRectTest(Size sheetSize)
        {
            Rectangle roiRect = Rectangle.FromLTRB(0, 0, sheetSize.Width, (int)(sheetSize.Height / 3.5f));
            return roiRect;
        }

        public abstract Rectangle FindSheet(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult);
    }

    public class SheetFinderV1 : SheetFinder
    {
        static int count = 0;
        // 점프패턴을 찾고 2개의 점프패턴 사이의 인쇄패턴 사이즈를 반환
        private Rectangle GetSheetRect(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
            count++;
            InspectionResult stillResult = inspectionResult as InspectionResult;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "SheetFinder"));
            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;


            //int skipRange = 500;
            //AlgoImage algoImage2 = null;
            try
            {
                //algoImage2 = algoImage.GetSubImage(Rectangle.FromLTRB(0, skipRange, algoImage.Width, algoImage.Height));
                //algoImage2.Save("algoImage2.bmp", debugContext);

                // Projection
                float[] projection = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);

                //todo 프로젝션의 히스토그램. 가장 넓은 그룹의 평균으로..,,
                // Projection Binarize
                float threshold = projection.Average() * 1.3f;
                LogHelper.Debug(LoggerType.Inspection, $"SheetFinderV1::GetSheetRect - Threshold: {threshold}");
                //float threshold = (projection.Max() + projection.Min()) / 2;
                if (threshold > 220 || threshold < 20) // 인쇄패턴이 없거나... 조명이 너무 밝거나, 어둡거나, 꺼져있거나.
                {
                    LogHelper.Debug(LoggerType.Inspection, $"SheetFinderV1::GetSheetRect - Return: Rectangle.Empty");
                    return Rectangle.Empty;
                }
                //이진화
                float[] binaryProjection = new float[projection.Length];
                Parallel.For(0, projection.Length, i =>
                {
                    binaryProjection[i] = (projection[i] >= threshold ? 255 : 0);
                });

#if DEBUG
                string projectionCSV = String.Join("\n", projection.Select(v => v.ToString()).ToArray());
                //Debug.WriteLine(projectionCSV);
                System.IO.File.WriteAllText(@"d:\projection.csv", projectionCSV);
                string projectionCSV2 = String.Join("\n", binaryProjection.Select(v => v.ToString()).ToArray());
                //Debug.WriteLine(projectionCSV);
                System.IO.File.WriteAllText(@"d:\projection2.csv", projectionCSV2);
#endif
                //구형파 찾아서 리스트에 넣기 (Square wave, Pulse wave)
                List<Point> foundPointList = new List<Point>();
                Point temppoint = new Point(-1, -1);
                List<Point> debugPointList = new List<Point>();
                for (int i = 0; i < binaryProjection.Length; i++)
                {
                    //X : High 시작점 -- Y:High 끝점
                    if (binaryProjection[i] == 0) //falling or zero
                    {
                        if (temppoint.X >= 0 && temppoint.Y >= 0 )
                        {
                            foundPointList.Add(temppoint);
                        }
                        temppoint.X = temppoint.Y = -1;
                    }
                    else  //255 // Rising or high
                    {
                        if (temppoint.X == -1)
                            temppoint.X = i;
                        temppoint.Y = i;
                    }
                    debugPointList.Add(temppoint);
                }
                //일정 두께 이하 버리기////////////////////////////////////////////////////////////////
                int removepixelWidth = (int)(2.0 *1000 / pelSize.Height);//2mm 이하는 버림..
                foundPointList.RemoveAll(f => (f.Y - f.X) < removepixelWidth);

                // 가까운것끼리 합치기  //점프패턴이 운없게 2개 이상으로 분리된경우 대비, 서로 합치기
                removepixelWidth = (int)(14.0 * 1000 / pelSize.Height);
                for (int i=0; i< foundPointList.Count-1; i++)
                {
                    int dist = foundPointList[i + 1].X - foundPointList[i].Y;  //다음꺼 시작이랑  2000픽셀( 14.0mm 거리에 있으면 합치기)
                    if (dist < removepixelWidth)
                    {
                        foundPointList[i] = new Point(foundPointList[i].X, foundPointList[i + 1].Y); //현재꺼 합쳐진걸로...다시 저장
                        foundPointList.RemoveAt(i + 1); //다음꺼 삭제.
                        i--; //this is  important
                    }
                }

                // for Save Debug
                if (debugContext.SaveDebugImage)
                {
                    AlgoImage resizeImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width / 5, algoImage.Height / 5);
                    imageProcessing.Resize(algoImage, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), string.Format("resized_{0}.bmp",count), debugContext);
                    resizeImage.Dispose();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Projection,Binarize,Edge(Src),Edge(Dst),Edge(len),Th,Dist"));
                    for (int i = 0; i < binaryProjection.Length; i++)
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", 
                            projection[i], binaryProjection[i], debugPointList[i].X, debugPointList[i].Y, 
                            (debugPointList[i].Y - debugPointList[i].X), threshold));
                    DebugHelper.SaveText(sb.ToString(), string.Format("Projection{0}.txt", count), debugContext);
                    sb.Clear();
                    foreach(var val in foundPointList)       
                        sb.AppendLine(val.ToString());
                    
                    DebugHelper.SaveText(sb.ToString(), string.Format("foundPointList{0}.txt", count), debugContext);
   
                }

                Rectangle sheetRect = Rectangle.Empty;
                //int left, top, right, bottom;
                if (foundPointList.Count == 2)
                {
                    int src = (foundPointList[0].X + foundPointList[0].Y) / 2;
                    int dst = (foundPointList[1].X + foundPointList[1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, src, algoImage.Width, dst);

                    if (stillResult != null)
                    {
                        stillResult.PrintingPeriod = Math.Abs(foundPointList[0].X - foundPointList[1].X) * pelSize.Height /1000;
                        stillResult.PrintingLength = Math.Abs(foundPointList[0].Y - foundPointList[1].X) * pelSize.Height /1000;
                    }
                    
                }
                else if (foundPointList.Count > 2)
                {
                    List<int> sheetLength = new List<int>();
                    foundPointList.Aggregate((f, g) =>
                    {
                        int dist = g.X - f.X;
                        sheetLength.Add(dist);
                        return g;
                    });
                    int maxLen = sheetLength.Max();
                    int maxIdx = sheetLength.FindIndex(f => f == maxLen);
                    int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;//Math.Min(foundPointList[maxIdx].Y, foundPointList[maxIdx + 1].Y);
                    int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;// Math.Max(foundPointList[maxIdx].X, foundPointList[maxIdx + 1].X);
                    //int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;
                    //int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, start, algoImage.Width, enddd);

                    if (stillResult != null)
                    {
                        stillResult.PrintingPeriod = Math.Abs(foundPointList[maxIdx].X - foundPointList[maxIdx + 1].X) * pelSize.Height /1000;
                        stillResult.PrintingLength = Math.Abs(foundPointList[maxIdx].Y - foundPointList[maxIdx + 1].X) * pelSize.Height /1000;
                    }
                    //algoImage.Save(@"d:\temp\tt.bmp");
                }
                foundPointList.Clear();

                if (debugContext.SaveDebugImage &&  sheetRect.Width > 0 && sheetRect.Height > 0)
                {
                    AlgoImage onePatternImage = algoImage.GetSubImage(sheetRect);
                    AlgoImage resizeImage = ImageBuilder.Build(onePatternImage.LibraryType, onePatternImage.ImageType, onePatternImage.Width / 5, onePatternImage.Height / 5);
                    imageProcessing.Resize(onePatternImage, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), string.Format("OnePattern_{0}.bmp", count), debugContext);
                    resizeImage.Dispose();
                }

                LogHelper.Debug(LoggerType.Inspection, $"SheetFinderV1::GetSheetRect - Return: {sheetRect}");
                return sheetRect;
            }
            finally
            {
                //algoImage2?.Dispose();
            }

        }

        public override Rectangle FindSheet(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
      
            Rectangle sheetRect = GetSheetRect(algoImage, inspectionResult);
            Model model = SystemManager.Instance().CurrentModel as Model;
            if (model.SheetHeigthPx > 0)
            {
                float grabLengthMin = model.SheetHeigthPx * 0.95f;
                float grabLengthMax = model.SheetHeigthPx * 1.05f;
                LogHelper.Debug(LoggerType.Inspection, $"SheetFinderV1::FindSheet - Min: {grabLengthMin}, Max: {grabLengthMax}, Length: {sheetRect.Height}");
                if (grabLengthMin < sheetRect.Height && sheetRect.Height < grabLengthMax)
                    return sheetRect;
                else
                    return Rectangle.Empty;
            }
            return sheetRect;
        }
    }

    public class SheetFinderV2 : SheetFinder
    {
        static int count = 0;
        private Rectangle GetSheetRect(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
            InspectionResult stillResult = inspectionResult as InspectionResult;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "SheetFinder"));
            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

 
            try
            {
                  // Projection
                float[] projection = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);

                string projectionCSV = String.Join("\n", projection.Select(v => v.ToString()).ToArray());
                //Debug.WriteLine(projectionCSV);
                //System.IO.File.WriteAllText(@"d:\projection.csv", projectionCSV);

                // Projection Binarize
                float threshold = projection.Average() * 1.2f;
                //float threshold = (projection.Max() + projection.Min()) / 2;
                if (threshold > 220 || threshold < 20) // 인쇄패턴이 없거나... 조명이 너무 밝거나, 어둡거나, 꺼져있거나.
                {
                    return Rectangle.Empty;
                }

                float[] projection2 = new float[projection.Length];
                Parallel.For(0, projection.Length, i =>
                {
                    projection2[i] = (projection[i] >= threshold ? 255 : 0);
                });
                //for (int i = 0; i < projection2.Length; i++)

                // Average Rising-edge Distance ( invalidate of upper 10% and lower 10%)
                List<int> distList = new List<int>();
                int edgeDist = 0;
                projection2.Aggregate((f, g) =>
                {
                    if (f != g && edgeDist > 0)
                    {
                        distList.Add(edgeDist);
                        edgeDist = 0;
                    }
                    else
                        edgeDist++;

                    return g;
                });
                //int skipCount = (int)(distList.Count * 0.1);
                //int takeCount = distList.Count - 2 * skipCount;
                //distList = distList.Skip(skipCount).Take(takeCount).ToList();
                if (distList.Count == 0)
                    return Rectangle.Empty;

                double avgDist = distList.Average();
                List<Point> foundPointList = new List<Point>();
                Point temppoint = new Point(-1, -1);
                List<Point> debugPointList = new List<Point>();
                for (int i = 0; i < projection2.Length; i++)
                {
                    //X : High 시작점 -- Y:High 끝점
                    if (projection2[i] == 0) //falling or zero
                    {
                        if (temppoint.X >= 0 && temppoint.Y >= 0 && (temppoint.Y - temppoint.X) > 2 * avgDist)
                        {
                            foundPointList.Add(temppoint);
                        }
                        temppoint.X = temppoint.Y = -1;
                    }
                    else  //255 // Rising or high
                    {
                        if (temppoint.X == -1)
                            temppoint.X = i;
                        temppoint.Y = i;
                    }
                    debugPointList.Add(temppoint);
                }
                //가장ㄴ 넓은 면적을 차지하는 전극(어두운)것의 평균값 이상으로 설정
                //일정 두께 이하 버리기////////////////////////////////////////////////////////////////
                foundPointList.RemoveAll(f => (f.Y - f.X) < avgDist * 5);
                //일정 두께 이상 근접 거리는 합치기. 점프패턴내의 노이즈 제거////////////////////////////
                ///


                // 여기 이상함. 점프패턴 근처에 7.0mm 이내의 high가 있으면 합쳐지게 됨.
                // 가까운것끼리 합치기  
                for (int i = 0; i < foundPointList.Count - 1; i++)
                {
                    int dist = foundPointList[i + 1].X - foundPointList[i].Y;  //다음꺼 시작이랑  1000픽셀( 7.0mm 거리에 있으면 합치기)
                    if (dist < 1000)
                    {
                        foundPointList[i] = new Point(foundPointList[i].X, foundPointList[i + 1].Y); //현재꺼 합쳐진걸로...다시 저장
                        foundPointList.RemoveAt(i + 1); //다음꺼 삭제.
                    }
                }

                // for Save Debug
                count++;
                if (debugContext.SaveDebugImage)
                {
                    AlgoImage resizeImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width / 5, algoImage.Height / 5);
                    imageProcessing.Resize(algoImage, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), string.Format("resized_{0}.bmp", count), debugContext);
                    resizeImage.Dispose();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Projection,Binarize,Edge(Src),Edge(Dst),Edge(len),Th,Dist"));
                    for (int i = 0; i < projection2.Length; i++)
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", projection[i], projection2[i], debugPointList[i].X, debugPointList[i].Y, (debugPointList[i].Y - debugPointList[i].X), threshold, avgDist));
                    DebugHelper.SaveText(sb.ToString(), "Projection.txt", debugContext);
                }

                Rectangle sheetRect = Rectangle.Empty;
                //int left, top, right, bottom;
                if (foundPointList.Count == 2)
                {
                    int src = (foundPointList[0].X + foundPointList[0].Y) / 2;
                    int dst = (foundPointList[1].X + foundPointList[1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, src, algoImage.Width, dst);

                    if (stillResult != null)
                    {
                        stillResult.PrintingPeriod = Math.Abs(foundPointList[0].X - foundPointList[1].X) * pelSize.Height / 1000;
                        stillResult.PrintingLength = Math.Abs(foundPointList[0].Y - foundPointList[1].X) * pelSize.Height / 1000;
                    }

                }
                else if (foundPointList.Count > 2)
                {
                    List<int> sheetLength = new List<int>();
                    foundPointList.Aggregate((f, g) =>
                    {
                        int dist = g.X - f.X;
                        sheetLength.Add(dist);
                        return g;
                    });
                    int maxLen = sheetLength.Max();
                    int maxIdx = sheetLength.FindIndex(f => f == maxLen);
                    int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;//Math.Min(foundPointList[maxIdx].Y, foundPointList[maxIdx + 1].Y);
                    int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;// Math.Max(foundPointList[maxIdx].X, foundPointList[maxIdx + 1].X);
                    //int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;
                    //int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, start, algoImage.Width, enddd);

                    if (stillResult != null)
                    {
                        stillResult.PrintingPeriod = Math.Abs(foundPointList[maxIdx].X - foundPointList[maxIdx + 1].X) * pelSize.Height / 1000;
                        stillResult.PrintingLength = Math.Abs(foundPointList[maxIdx].Y - foundPointList[maxIdx + 1].X) * pelSize.Height / 1000;
                    }
                    //algoImage.Save(@"d:\temp\tt.bmp");
                }
                foundPointList.Clear();

                if (debugContext.SaveDebugImage && sheetRect.Width > 0 && sheetRect.Height > 0)
                {
                    AlgoImage onePatternImage = algoImage.GetSubImage(sheetRect);
                    AlgoImage resizeImage = ImageBuilder.Build(onePatternImage.LibraryType, onePatternImage.ImageType, onePatternImage.Width / 5, onePatternImage.Height / 5);
                    imageProcessing.Resize(onePatternImage, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), string.Format("OnePattern_{0}.bmp", count), debugContext);
                    resizeImage.Dispose();
                }

                return sheetRect;
            }
            finally
            {
                //algoImage2?.Dispose();
            }

        }

        public override Rectangle FindSheet(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {

            Rectangle sheetRect = GetSheetRect(algoImage, inspectionResult);
            Model model = SystemManager.Instance().CurrentModel as Model;
            if (model.SheetHeigthPx > 0)
            {
                float grabLengthMin = model.SheetHeigthPx * 0.95f;
                float grabLengthMax = model.SheetHeigthPx * 1.05f;
                if (grabLengthMin < sheetRect.Height && sheetRect.Height < grabLengthMax)
                    return sheetRect;
                else
                    return Rectangle.Empty;
            }
            return sheetRect;
        }
    }


}
