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
using DynMvp.Vision;
using UniEye.Base.Settings;
using Infragistics.Documents.Excel;
using UniScanM.Data;

namespace UniScanM.RVMS.Data
{
    internal enum ResultHeader { Index, Date, Time, RollPos, ManSideRaw, ManSideZero, GearSideRaw, GearSideZero, BeforePattern, AfterPattern, Result, MAX_COUNT }
    internal enum ExcelHeader { Date, Time, GearSideZero, GearSideRaw, ManSideZero, ManSideRaw, BeforePattern, AfterPattern, Result }
    public class ReportDataExporter : UniScanM.Data.DataExporter
    {
        public ReportDataExporter() : base()
        {
            this.row_begin = 7;
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_RVMS.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //this.WriteCsvHeader(stringBuilder, typeof(ResultHeader));

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Date Report");
            //sb.AppendLine(string.Format("Start Date,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy-MM-dd")));
            //sb.AppendLine(string.Format("Start Time,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("HH:mm:ss")));
            //sb.AppendLine(string.Format("Lot No.,{0}", SystemManager.Instance().ProductionManager.CurProduction.LotNo));
            //sb.AppendLine("No,Date,Time,Judgement,ManSideRaw,ManSideZero,GearSideRaw,GearSideZero,BeforePattern,AfterPattern");

            //File.WriteAllText(resultFile, sb.ToString(), Encoding.Default);
        }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult rvmsInspectionResult = inspectionResult as InspectionResult;
            string[] tokens = new string[(int)ResultHeader.MAX_COUNT];
            
            tokens[(int)ResultHeader.Index] = rvmsInspectionResult.InspectionNo;
            tokens[(int)ResultHeader.Date] = rvmsInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd");
            tokens[(int)ResultHeader.Time] = rvmsInspectionResult.InspectionStartTime.ToString("HH:mm:ss.ff");
            tokens[(int)ResultHeader.RollPos] = rvmsInspectionResult.RollDistance.ToString();

            tokens[(int)ResultHeader.ManSideRaw] = tokens[(int)ResultHeader.ManSideZero] = "0";
            if (rvmsInspectionResult.ManSide != null)
            {
                tokens[(int)ResultHeader.ManSideRaw] = rvmsInspectionResult.ManSide.YRaw.ToString("F4");
                tokens[(int)ResultHeader.ManSideZero] = rvmsInspectionResult.ManSide.Y.ToString("F4");
            }

            tokens[(int)ResultHeader.GearSideRaw] = tokens[(int)ResultHeader.GearSideZero] = "0";
            if (rvmsInspectionResult.ManSide != null)
            {
                tokens[(int)ResultHeader.GearSideRaw] = rvmsInspectionResult.GearSide.YRaw.ToString("F4");
                tokens[(int)ResultHeader.GearSideZero] = rvmsInspectionResult.GearSide.Y.ToString("F4");
            }

            tokens[(int)ResultHeader.BeforePattern] = "0";
            if (rvmsInspectionResult.BeforePattern != null)
                tokens[(int)ResultHeader.BeforePattern] = rvmsInspectionResult.BeforePattern.Y.ToString("F3");

            tokens[(int)ResultHeader.AfterPattern] = "0";
            if (rvmsInspectionResult.AffterPattern != null)
                tokens[(int)ResultHeader.AfterPattern] = rvmsInspectionResult.AffterPattern.Y.ToString("F3");

            tokens[(int)ResultHeader.Result] = inspectionResult.Judgment.ToString();

            string aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[1].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[5].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Rows[3].Cells[3].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[7].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            InspectionResult rvmsInspResult = inspectionResult as InspectionResult;

            Worksheet logSheet = workbook.Worksheets[sheetNo];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Date].Value = rvmsInspResult.InspectionStartTime.ToString("yyyy.MM.dd"); // Date 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Time].Value = rvmsInspResult.InspectionStartTime.ToString("HH:mm:ss"); // Date 

            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.GearSideZero].Value = rvmsInspResult.GearSide == null ? "0" : rvmsInspResult.GearSide.Y.ToString("F4"); // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.GearSideRaw].Value = rvmsInspResult.GearSide == null ? "0": rvmsInspResult.GearSide.YRaw.ToString("F4");
            
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.ManSideZero].Value = rvmsInspResult.ManSide == null ? "0" : rvmsInspResult.ManSide.Y.ToString("F4"); // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.ManSideRaw].Value = rvmsInspResult.ManSide == null ? "0" : rvmsInspResult.ManSide.YRaw.ToString("F4");
           
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.BeforePattern].Value = rvmsInspResult.BeforePattern == null ? "0" : rvmsInspResult.BeforePattern.Y.ToString("F3"); // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.AfterPattern].Value = rvmsInspResult.AffterPattern == null ? "0" : rvmsInspResult.AffterPattern.Y.ToString("F3"); // Insp Zone 

            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Result].Value = JudgementString.GetString(rvmsInspResult.Judgment);

            return 1;
        }

        protected override void SaveImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
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

                    string inspectionNo = tokens[(int)ResultHeader.Index];
                    string date = tokens[(int)ResultHeader.Date];
                    string time = tokens[(int)ResultHeader.Time];

                    float manSideRaw = float.Parse(tokens[(int)ResultHeader.ManSideRaw]);
                    float manSideZero = float.Parse(tokens[(int)ResultHeader.ManSideZero]);
                    float gearSideRaw = float.Parse(tokens[(int)ResultHeader.GearSideRaw]);
                    float gearSideZero = float.Parse(tokens[(int)ResultHeader.GearSideZero]);
                    float bPattern = float.Parse(tokens[(int)ResultHeader.BeforePattern]);
                    float aPattern = float.Parse(tokens[(int)ResultHeader.AfterPattern]);

                    Judgment judgment;
                    bool ok = Enum.TryParse(tokens[(int)ResultHeader.Result], out judgment);
                    if (ok == false)
                        judgment = bool.Parse(tokens[(int)ResultHeader.Result]) ? Judgment.Accept : Judgment.Reject;

                    InspectionResult rvmsResult = new InspectionResult();
                    rvmsResult.ResultPath = this.ResultPath;
                    rvmsResult.InspectionNo = inspectionNo;
                    rvmsResult.InspectionStartTime = DateTime.ParseExact(string.Format("{0}_{1}", date, time), "yyyy.MM.dd_HH:mm:ss.ff", null);

                    rvmsResult.ManSide = new ScanData(rvmsResult.InspectionStartTime, manSideZero, manSideRaw, 0);
                    rvmsResult.GearSide = new ScanData(rvmsResult.InspectionStartTime, gearSideZero, gearSideRaw,  0);
                    rvmsResult.BeforePattern = new ScanData(rvmsResult.InspectionStartTime, bPattern, 0, 0);
                    rvmsResult.AffterPattern= new ScanData(rvmsResult.InspectionStartTime, aPattern, 0, 0);

                    rvmsResult.Judgment = judgment;
                    
                    this.inspectionResultList.Add(rvmsResult);
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
