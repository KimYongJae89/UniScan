using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Data
{
    internal enum ResultHeader
    {
        // 검사 정보
        Index, Date, Time, Lot, RollPos, InspZone, 

        // 이미지 정보
        ImageW, ImageH, pelSizeX, pelSizeY,

        // 마진 검사 정보
        MarginTargetW, MarginTargetH, MarginSizeW, MarginSizeH,
        MarginRectX, MarginRectY, MarginRectW, MarginRectH,
        MarginResult,
        
        // 번짐 검사 정보
        BlotTargetW, BlotTargetH, BlotSizeW, BlotSizeH,
        BlotRectX, BlotRectY, BlotRectW, BlotRectH,
        BlotResult,

        // 이물 검사 정보
        DefectCount,
        DefectResult,

        // 전체 정보
        OverallResult,

        DefectRects,

        MAX_COUNT
    }

    public class DataArchiver : InspResultArchiver
    {
        static string RESULT_FILENAME = "Result.csv";
        public InspectionResult Load(string dataPath)
        {
            string resultFile = Path.Combine(dataPath, RESULT_FILENAME);
            if (!File.Exists(resultFile))
                return null;

            string tempResultFile = Path.Combine(dataPath, string.Format("_{0}", RESULT_FILENAME));
            File.Copy(resultFile, tempResultFile);
            string[] lines = File.ReadAllLines(tempResultFile);
            File.Delete(tempResultFile);

            if (lines.Length < (int)ResultHeader.MAX_COUNT)
                return null;

            Inspect.InspectionResult inspectionResult = new Inspect.InspectionResult();
            try
            {
                inspectionResult.InspectionNo = lines[(int)ResultHeader.Index];
                inspectionResult.InspectionStartTime = DateTime.ParseExact(lines[(int)ResultHeader.Date], "yyyy/MM/dd", null);
                inspectionResult.InspectionStartTime += TimeSpan.ParseExact(lines[(int)ResultHeader.Time], "hh\\:mm\\:ss", null);
                inspectionResult.LotNo = lines[(int)ResultHeader.Lot];
                inspectionResult.RollPos = float.Parse(lines[(int)ResultHeader.RollPos]);
                inspectionResult.ZoneIndex = int.Parse(lines[(int)ResultHeader.InspZone]);

                inspectionResult.InspImageSize = new System.Drawing.Size(int.Parse(lines[(int)ResultHeader.ImageW]), int.Parse(lines[(int)ResultHeader.ImageH]));
                inspectionResult.InspPelSize = new System.Drawing.SizeF(int.Parse(lines[(int)ResultHeader.pelSizeX]), int.Parse(lines[(int)ResultHeader.pelSizeY]));

                inspectionResult.MarginTarget = new System.Drawing.SizeF(float.Parse(lines[(int)ResultHeader.MarginTargetW]), float.Parse(lines[(int)ResultHeader.MarginTargetH]));
                inspectionResult.MarginSize = new System.Drawing.SizeF(float.Parse(lines[(int)ResultHeader.MarginSizeW]), float.Parse(lines[(int)ResultHeader.MarginSizeH]));
                inspectionResult.MarginRect = new System.Drawing.Rectangle(
                    int.Parse(lines[(int)ResultHeader.MarginRectX]), int.Parse(lines[(int)ResultHeader.MarginRectY]),
                    int.Parse(lines[(int)ResultHeader.MarginRectW]), int.Parse(lines[(int)ResultHeader.MarginRectH]));
                inspectionResult.MarginResult = bool.Parse(lines[(int)ResultHeader.MarginResult]);

                inspectionResult.BlotTarget = new System.Drawing.SizeF(float.Parse(lines[(int)ResultHeader.BlotTargetW]), float.Parse(lines[(int)ResultHeader.BlotTargetH]));
                inspectionResult.BlotSize = new System.Drawing.SizeF(float.Parse(lines[(int)ResultHeader.BlotSizeW]), float.Parse(lines[(int)ResultHeader.BlotSizeH]));
                inspectionResult.BlotRect = new System.Drawing.Rectangle(
                    int.Parse(lines[(int)ResultHeader.BlotRectX]), int.Parse(lines[(int)ResultHeader.BlotRectY]),
                    int.Parse(lines[(int)ResultHeader.BlotRectW]), int.Parse(lines[(int)ResultHeader.BlotRectH]));
                inspectionResult.BlotResult = bool.Parse(lines[(int)ResultHeader.BlotResult]);
                
                inspectionResult.Defects= new Defect[int.Parse(lines[(int)ResultHeader.DefectCount])];
                inspectionResult.DefectResult = bool.Parse(lines[(int)ResultHeader.DefectResult]);

                inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), lines[(int)ResultHeader.OverallResult]);

                for (int i = 0; i < inspectionResult.Defects.Length; i++)
                {
                    string[] tokens = lines[i + (int)ResultHeader.DefectRects].Split(',');
                    int x = int.Parse(tokens[0]);
                    int y = int.Parse(tokens[1]);
                    int w = int.Parse(tokens[2]);
                    int h = int.Parse(tokens[3]);
                    float umW = float.Parse(tokens[4]);
                    float umH = float.Parse(tokens[5]);
                    inspectionResult.Defects[i] = new Defect(new System.Drawing.Rectangle(x, y, w, h), new System.Drawing.SizeF(umW, umH));
                }
            }
            catch (FormatException)
            {
                return null;
            }

            return inspectionResult;
        }


        public void Save(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            string targetDir = Path.Combine(inspectionResult.ResultPath, inspectionResult.InspectionNo);
            Directory.CreateDirectory(targetDir);

            string targetFile = Path.Combine(targetDir, RESULT_FILENAME);
            if (File.Exists(targetFile))
                File.Delete(targetFile);

            FileStream writeFileStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write);
            AppendResult(inspectionResult, writeFileStream);

            Task flushTask = writeFileStream.FlushAsync();
            ImageD imageD = inspectionResult.GrabImageList.FirstOrDefault();
            imageD.SaveImage(Path.Combine(targetDir, "GrabImage.jpg"));
            flushTask.Wait();
        }

        private void AppendHeader(FileStream writeFileStream)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Date Report");
            sb.AppendLine(string.Format("Start Date,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy-MM-dd")));
            sb.AppendLine(string.Format("Start Time,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("HH:mm:ss")));

            sb.AppendLine(string.Join(",", Enum.GetNames(typeof(ResultHeader))));

            byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
            writeFileStream.Write(bytes, 0, bytes.Length);
        }

        private void AppendResult(InspectionResult inspectionResult, FileStream writeFileStream)
        {
            Inspect.InspectionResult inspectionResult2 = inspectionResult as Inspect.InspectionResult;
            string[] tokens = new string[(int)ResultHeader.MAX_COUNT+1];

            tokens[(int)ResultHeader.Index] = inspectionResult2.InspectionNo;
            tokens[(int)ResultHeader.Date] = inspectionResult2.InspectionStartTime.ToString("yyyy/MM/dd");
            tokens[(int)ResultHeader.Time] = inspectionResult2.InspectionStartTime.ToString("HH:mm:ss");
            tokens[(int)ResultHeader.Lot] = inspectionResult2.LotNo;
            tokens[(int)ResultHeader.RollPos] = inspectionResult2.RollPos.ToString("F2");
            tokens[(int)ResultHeader.InspZone] = inspectionResult2.ZoneIndex.ToString();

            tokens[(int)ResultHeader.ImageW] = inspectionResult2.InspImageSize.Width.ToString();
            tokens[(int)ResultHeader.ImageH] = inspectionResult2.InspImageSize.Height.ToString();
            tokens[(int)ResultHeader.pelSizeX] = inspectionResult2.InspPelSize.Width.ToString();
            tokens[(int)ResultHeader.pelSizeY] = inspectionResult2.InspPelSize.Height.ToString();

            tokens[(int)ResultHeader.MarginTargetW] = inspectionResult2.MarginTarget.Width.ToString();
            tokens[(int)ResultHeader.MarginTargetH] = inspectionResult2.MarginTarget.Height.ToString();
            tokens[(int)ResultHeader.MarginSizeW] = inspectionResult2.MarginSize.Width.ToString();
            tokens[(int)ResultHeader.MarginSizeH] = inspectionResult2.MarginSize.Height.ToString();
            tokens[(int)ResultHeader.MarginRectX] = inspectionResult2.MarginRect.X.ToString();
            tokens[(int)ResultHeader.MarginRectY] = inspectionResult2.MarginRect.Y.ToString();
            tokens[(int)ResultHeader.MarginRectW] = inspectionResult2.MarginRect.Width.ToString();
            tokens[(int)ResultHeader.MarginRectH] = inspectionResult2.MarginRect.Height.ToString();
            tokens[(int)ResultHeader.MarginResult] = inspectionResult2.MarginResult.ToString();

            tokens[(int)ResultHeader.BlotTargetW] = inspectionResult2.BlotTarget.Width.ToString();
            tokens[(int)ResultHeader.BlotTargetH] = inspectionResult2.BlotTarget.Height.ToString();
            tokens[(int)ResultHeader.BlotSizeW] = inspectionResult2.BlotSize.Width.ToString();
            tokens[(int)ResultHeader.BlotSizeH] = inspectionResult2.BlotSize.Height.ToString();
            tokens[(int)ResultHeader.BlotRectX] = inspectionResult2.BlotRect.X.ToString();
            tokens[(int)ResultHeader.BlotRectY] = inspectionResult2.BlotRect.Y.ToString();
            tokens[(int)ResultHeader.BlotRectW] = inspectionResult2.BlotRect.Width.ToString();
            tokens[(int)ResultHeader.BlotRectH] = inspectionResult2.BlotRect.Height.ToString();
            tokens[(int)ResultHeader.BlotResult] = inspectionResult2.BlotResult.ToString();

            tokens[(int)ResultHeader.DefectCount] = inspectionResult2.DefectCount.ToString();
            tokens[(int)ResultHeader.DefectResult] = inspectionResult2.DefectResult.ToString();

            tokens[(int)ResultHeader.OverallResult] = inspectionResult2.Judgment.ToString();

            int count = inspectionResult2.DefectCount;
            string[] tokens2 = new string[count];
            for (int i = 0; i < count; i++)
                tokens2[i] = string.Format("{0},{1},{2},{3},{4},{5}"
                    , inspectionResult2.Defects[i].Rectangle.X
                    , inspectionResult2.Defects[i].Rectangle.Y
                    , inspectionResult2.Defects[i].Rectangle.Width
                    , inspectionResult2.Defects[i].Rectangle.Height
                    , inspectionResult2.Defects[i].SizeUm.Width
                    , inspectionResult2.Defects[i].SizeUm.Height
                    );
            tokens[(int)ResultHeader.DefectRects] = string.Join(Environment.NewLine, tokens2);

            byte[] bytes = Encoding.Default.GetBytes(string.Join(Environment.NewLine, tokens));

            writeFileStream.Write(bytes, 0, bytes.Length);
        }
    }
}
