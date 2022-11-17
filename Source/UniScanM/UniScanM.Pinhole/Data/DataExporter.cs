using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using System.Threading;
using System.Xml;
using DynMvp.Base;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using UniEye.Base;
using System.Diagnostics;
using UniScanM.Data;
using DynMvp.Vision;
using UniEye.Base.Settings;
using Infragistics.Documents.Excel;

namespace UniScanM.Pinhole.Data
{
    // Index,Time,Sheet Brightness,Judge,LSL,USL,Referenc
    internal enum ResultHeader { Index, Date, Time, RollDistance, Device, Section, DefectNo, PixelPosX, PixelPosY, DefectRectX, DefectRectY, DefectRectW, DefectRectH, DefectRealPosX, DefectRealPosY, DefectType, ImageFileName, MAX_COUNT }
    internal enum ExcelHeader { DATE, TIME, RollPos, Section, Device, Defect, SizeW, SizeH, ImageName }
    public class DataExporter : UniScanM.Data.DataExporter
    {
        public DataExporter() : base()
        {
            this.row_begin = 7;
        }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult pinholeResult = inspectionResult as InspectionResult;
            foreach (DefectInfo defectInfo in pinholeResult.LastDefectInfoList)
            {
                string[] tokens = new string[(int)ResultHeader.MAX_COUNT];
                //tokens[(int)ResultHeader.Index] = pinholeResult.InspectionNo;
                tokens[(int)ResultHeader.Date] = pinholeResult.InspectionStartTime.ToString("yyyyMMdd");
                tokens[(int)ResultHeader.Time] = pinholeResult.InspectionStartTime.ToString("HHmmss");
                tokens[(int)ResultHeader.RollDistance] = pinholeResult.RollDistance.ToString();
                tokens[(int)ResultHeader.Device] = pinholeResult.DeviceIndex.ToString();
                tokens[(int)ResultHeader.Section] = pinholeResult.SectionIndex.ToString();
                tokens[(int)ResultHeader.PixelPosX] = defectInfo.PixelPosition.X.ToString();
                tokens[(int)ResultHeader.PixelPosY] = defectInfo.PixelPosition.Y.ToString();
                tokens[(int)ResultHeader.DefectNo] = defectInfo.DefectNo.ToString();
                tokens[(int)ResultHeader.DefectRectX] = defectInfo.BoundingRect.X.ToString();
                tokens[(int)ResultHeader.DefectRectY] = defectInfo.BoundingRect.Y.ToString();
                tokens[(int)ResultHeader.DefectRectW] = defectInfo.BoundingRect.Width.ToString();
                tokens[(int)ResultHeader.DefectRectH] = defectInfo.BoundingRect.Height.ToString();
                tokens[(int)ResultHeader.DefectRealPosX] = defectInfo.RealPosition.X.ToString();
                tokens[(int)ResultHeader.DefectRealPosY] = defectInfo.RealPosition.Y.ToString();

                tokens[(int)ResultHeader.DefectType] = defectInfo.DefectType.ToString();
                if(string.IsNullOrEmpty(defectInfo.Path)==false)
                    tokens[(int)ResultHeader.ImageFileName] = defectInfo.GetImageFileName();

                string aLine = string.Join(",", tokens);
                stringBuilder.AppendLine(aLine);
            }
        }

        protected override void SaveImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (inspectionResult.Judgment == Judgment.Accept && OperationSettings.Instance().SaveDebugImage == false)
                return;

            SaveDispImage(resultPath, skipImageSave,inspectionResult);
            SaveSubIamge(resultPath, skipImageSave,inspectionResult);
        }

        private void SaveDispImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult pinholeResult = inspectionResult as InspectionResult;
            string fileName = pinholeResult.GetDispImageName();
            string fullPath = Path.Combine(resultPath, "DispImage", fileName);
            if(skipImageSave)
            {
                inspectionResult.DisplayBitmapSaved = IMAGE_SAVE_SKIPPED;
            }else if (pinholeResult.SaveBitmap != null)
            {
                ImageHelper.SaveImage(pinholeResult.SaveBitmap, fullPath);
                inspectionResult.DisplayBitmapSaved = fileName;
            }
        }

        private void SaveSubIamge(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult pinholeResult = inspectionResult as InspectionResult;
            if (pinholeResult.NumDefect == 0)
                return;

            //if (Directory.Exists(ResultPath) == false)
            //    Directory.CreateDirectory(ResultPath);

            foreach (DefectInfo defectInfo in pinholeResult.LastDefectInfoList)
            {
                string path = string.Format(@"{0}\{1}", resultPath, defectInfo.GetImageFileName());
                if (skipImageSave)
                {
                    defectInfo.Path = IMAGE_SAVE_SKIPPED;
                }
                else
                {
                    if (defectInfo.ClipImage != null)
                        ImageHelper.SaveImage(defectInfo.ClipImage, path);
                    defectInfo.Path = path;
                }
                //defectInfo.ClipImage.Save(path, ImageFormat.Bmp);
            }
        }  

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_Pinhole.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //this.WriteCsvHeader(stringBuilder, typeof(ResultHeader));
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[1].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[5].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Rows[3].Cells[3].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("HH:mm:ss.fff"); // End Time
            workbook.Worksheets[0].Rows[3].Cells[7].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            if (inspectionResult.IsGood())
                return 0;

            InspectionResult pinholeResult = inspectionResult as InspectionResult;
            for (int i = 0; i < pinholeResult.LastDefectInfoList.Count; i++)
            {
                int writeSheet = sheetNo;
                int writeRow = rowNo + i;
                if (writeRow > MAX_SHEET_ROWS)
                {
                    writeSheet++;
                    writeRow -= MAX_SHEET_ROWS;
                }

                if (writeSheet >= workbook.Worksheets.Count())
                    continue;

                Worksheet logSheet = workbook.Worksheets[sheetNo];
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.DATE].Value = pinholeResult.InspectionEndTime.ToString("yyyy.MM.dd"); // Date
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.TIME].Value = pinholeResult.InspectionEndTime.ToString("HH:mm:ss.fff"); // Time
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.RollPos].Value = pinholeResult.RollDistance.ToString("0");
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.Section].Value = pinholeResult.SectionIndex.ToString("0");
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.Device].Value = pinholeResult.DeviceIndex.ToString("0");

                DefectInfo defectInfo = pinholeResult.LastDefectInfoList[i];
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.Defect].Value = defectInfo.DefectType.ToString();
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.SizeW].Value = defectInfo.BoundingRect.Width.ToString();
                logSheet.Rows[writeRow].Cells[(int)ExcelHeader.SizeH].Value = defectInfo.BoundingRect.Height.ToString();

                if(string.IsNullOrEmpty(defectInfo.Path) == false)
                    logSheet.Rows[writeRow].Cells[(int)ExcelHeader.ImageName].Value = defectInfo.Path;
            }

            return pinholeResult.LastDefectInfoList.Count;
        }
    }
    public class DataImporter : UniScanM.Data.DataImporter
    {
        protected override bool Import()
        {
            string resultFile = Path.Combine(this.ResultPath, this.resultFileName);
            if (File.Exists(resultFile) == false)
                return false;
            try
            {
                DefectInfoList defectInfoList = new DefectInfoList();

                StreamReader sr = new StreamReader(resultFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] tokens = line.Split(',');
                    if (tokens.Length != (int)ResultHeader.MAX_COUNT)
                        continue;

                    // 핀홀은 Index가 없음.
                    //int index;
                    //if (int.TryParse(tokens[(int)ResultHeader.Index], out index) == false)
                    //    continue;
                    //string inspectionNo = tokens[(int)ResultHeader.Index];
                    DateTime dateTime = DateTime.ParseExact(tokens[(int)ResultHeader.Date], "yyyyMMdd", null).Add(TimeSpan.ParseExact(tokens[(int)ResultHeader.Time], @"hhmmss", null));
                    int rollDistance = int.Parse(tokens[(int)ResultHeader.RollDistance]);
                    int deviceIndex = int.Parse(tokens[(int)ResultHeader.Device]);
                    int sectionIndex = int.Parse(tokens[(int)ResultHeader.Section]);
                    float pixelPosX = float.Parse(tokens[(int)ResultHeader.PixelPosX]);
                    float pixelPosY = float.Parse(tokens[(int)ResultHeader.PixelPosY]);

                    int defectNo = int.Parse(tokens[(int)ResultHeader.DefectNo]);
                    float defectRectX = float.Parse(tokens[(int)ResultHeader.DefectRectX]);
                    float defectRectY = float.Parse(tokens[(int)ResultHeader.DefectRectY]);
                    float defectRectW = float.Parse(tokens[(int)ResultHeader.DefectRectW]);
                    float defectRectH = float.Parse(tokens[(int)ResultHeader.DefectRectH]);
                    float defectRealPosX = float.Parse(tokens[(int)ResultHeader.DefectRealPosX]);
                    float defectRealPosY = float.Parse(tokens[(int)ResultHeader.DefectRealPosY]);
                    string defectType = tokens[(int)ResultHeader.DefectType];
                    string imageFileName = tokens[(int)ResultHeader.ImageFileName];

                    RectangleF boundingRect = new RectangleF(defectRectX, defectRectY, defectRectW, defectRectH);
                    PointF pixelPos = new PointF(pixelPosX, pixelPosY);
                    PointF defectRealPos= new PointF(defectRealPosX, defectRealPosY);
                    Data.DefectType type = (Data.DefectType)Enum.Parse(typeof(Data.DefectType), defectType);

                    defectInfoList.Add(new DefectInfo(deviceIndex, sectionIndex,defectNo, boundingRect, pixelPos, defectRealPos, type, imageFileName, rollDistance.ToString()));
                }

                while (defectInfoList.Count > 0)
                {
                    int src = defectInfoList.FindIndex(f => f.DefectNo == 0);
                    if (src < 0)
                        return false;

                    int dst = defectInfoList.FindIndex(src + 1, f => f.DefectNo == 0);
                    if (dst < src)
                        dst = defectInfoList.Count;

                    int subCount = dst - src;
                    List<DefectInfo> subList = defectInfoList.GetRange(src, subCount);
                    defectInfoList.RemoveRange(src, subCount);

                    InspectionResult pinholeResult = new InspectionResult();
                    pinholeResult.ResultPath = this.ResultPath;
                    pinholeResult.RollDistance = Convert.ToInt32(subList[0].PvPos);
                    pinholeResult.DeviceIndex = subList[0].CameraIndex;
                    pinholeResult.SectionIndex = subList[0].SectionIndex;
                    pinholeResult.LastDefectInfoList.AddRange(subList);
                    pinholeResult.NumDefect = subList.Count;
                    pinholeResult.Judgment = Judgment.Reject;

                    this.inspectionResultList.Add(pinholeResult);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
