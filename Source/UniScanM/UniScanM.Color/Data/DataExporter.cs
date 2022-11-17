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

namespace UniScanM.ColorSens.Data
{
    // Index,Time,Sheet Brightness,Judge,LSL,USL,Referenc
    internal enum ResultHeader { Index, Date, Time, Distance, Brightness, Judge, LSL, USL, Referenc, MAX_COUNT }
    internal enum ExcelHeader { DATE, TIME, RollPos, InspectionNo, Brightness, Result,ImageName}
    public class ReportDataExporter : UniScanM.Data.DataExporter
    {
        public ReportDataExporter() : base()
        {
            this.row_begin = 8;
        }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult colorResult = inspectionResult as InspectionResult;

            string[] tokens = new string[(int)ResultHeader.MAX_COUNT];
            tokens[(int)ResultHeader.Index] = colorResult.InspectionNo;
            tokens[(int)ResultHeader.Date] = colorResult.InspectionStartTime.ToString("yyyyMMdd");
            tokens[(int)ResultHeader.Time] = colorResult.InspectionStartTime.ToString("HHmmss");
            tokens[(int)ResultHeader.Distance] = colorResult.RollDistance.ToString();
            tokens[(int)ResultHeader.Brightness] = colorResult.SheetBrightness.ToString();
            tokens[(int)ResultHeader.Judge] = colorResult.IsGood().ToString();
            tokens[(int)ResultHeader.LSL] = colorResult.Lowerlimit.ToString();
            tokens[(int)ResultHeader.USL] = colorResult.Uppperlimit.ToString();
            tokens[(int)ResultHeader.Referenc] = colorResult.ReferenceBrightness.ToString();
            string aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
        }

        protected override void SaveImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            InspectionResult colorResult = inspectionResult as InspectionResult;
            if (inspectionResult.Judgment == Judgment.Accept && OperationSettings.Instance().SaveDebugImage==false)
            {
                //colorResult.GrabImageList[0].Dispose();
                return;
            }

            string fileName = string.Format("{0}.jpg", inspectionResult.InspectionNo);
            if (skipImageSave)
            {
                inspectionResult.DisplayBitmapSaved = IMAGE_SAVE_SKIPPED;
            }
            else if (colorResult.GrabImageList.Count > 0)
            {
                var  image  = colorResult.GrabImageList[0].ToBitmap();
                ImageHelper.SaveImage(image, Path.Combine(resultPath, fileName));
                //colorResult.GrabImageList[0].Dispose();
                //image?.Dispose();
                //ImageHelper.SaveImage(colorResult.DisplayBitmap, Path.Combine(resultPath, fileName));
                inspectionResult.DisplayBitmapSaved = fileName;
            }

            //colorResult.GrabImageList[0].Dispose();
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_ColorSensor.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //this.WriteCsvHeader(stringBuilder, typeof(ResultHeader));
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[1].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Rows[4].Cells[1].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("yyyy.MM.dd HH:mm:ss"); // End Time
            workbook.Worksheets[0].Rows[4].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            //int writeRow = logSheet.Rows.Count();
            Worksheet logSheet = workbook.Worksheets[sheetNo];
            InspectionResult colorInspectionResult = inspectionResult as InspectionResult;
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.DATE].Value = colorInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd"); // Date 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.TIME].Value = colorInspectionResult.InspectionStartTime.ToString("HH:mm:ss"); // time 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.RollPos].Value = colorInspectionResult.RollDistance; //Inspection NO
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.InspectionNo].Value = colorInspectionResult.InspectionNo; //Inspection NO
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Brightness].Value = colorInspectionResult.SheetBrightness.ToString("0.00"); //
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Result].Value = JudgementString.GetString(colorInspectionResult.Judgment);

            if (string.IsNullOrEmpty(colorInspectionResult.DisplayBitmapSaved) == false)
                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.ImageName].Value = colorInspectionResult.DisplayBitmapSaved;

            return 1;
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
                StreamReader sr = new StreamReader(resultFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] tokens = line.Split(',');
                    if (tokens.Length != (int)ResultHeader.MAX_COUNT)
                        continue;

                    int index;
                    if (int.TryParse(tokens[(int)ResultHeader.Index], out index) == false)
                        continue;

                    UniScanM.ColorSens.Data.InspectionResult colorResult = new UniScanM.ColorSens.Data.InspectionResult();
                    colorResult.ResultPath = this.ResultPath;

                    string inspectionNo = tokens[(int)ResultHeader.Index];
                    string date = tokens[(int)ResultHeader.Date];
                    string time = tokens[(int)ResultHeader.Time];
                    string distance = tokens[(int)ResultHeader.Distance];
                    string brightness = tokens[(int)ResultHeader.Brightness];
                    string judge = tokens[(int)ResultHeader.Judge];
                    string lsl = tokens[(int)ResultHeader.LSL];
                    string usl = tokens[(int)ResultHeader.USL];
                    string reference = tokens[(int)ResultHeader.Referenc];

                    colorResult.InspectionNo = inspectionNo;
                    colorResult.InspectionStartTime = DateTime.ParseExact(string.Format("{0}_{1}", date, time), "yyyyMMdd_HHmmss", null);
                    colorResult.RollDistance = (int)Math.Round(float.Parse(distance));
                    colorResult.SheetBrightness = float.Parse(brightness);
                    colorResult.Lowerlimit = float.Parse(lsl);
                    colorResult.Uppperlimit = float.Parse(usl);
                    colorResult.ReferenceBrightness = float.Parse(reference);
                    if (bool.Parse(judge))
                        colorResult.SetGood();
                    else
                        colorResult.SetDefect();
                    
                    this.inspectionResultList.Add(colorResult);
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
