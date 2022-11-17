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

namespace UniScanM.CEDMS.Data
{
    internal enum ResultHeader { Index, Date, Time, RollPos, InFeedRaw, /*InFeedZero,*/ OutFeedRaw, /*OutFeedZero,*/ Result, LineSpeed, MAX_COUNT }
    internal enum ExcelHeader { Date, Time, RollPos , InFeedRaw, /*InFeedZero,*/ OutFeedRaw, /*OutFeedZero,*/ Result, LineSpeed }
    public class ReportDataExporter : UniScanM.Data.DataExporter
    {
        public ReportDataExporter() : base()
        {
            this.row_begin = 7;
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_CEDMS.xlsx";
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
            InspectionResult cedmsInspectionResult = inspectionResult as InspectionResult;
            string[] tokens = new string[(int)ResultHeader.MAX_COUNT];
            
            tokens[(int)ResultHeader.Index] = cedmsInspectionResult.InspectionNo;
            tokens[(int)ResultHeader.Date] = cedmsInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd");
            tokens[(int)ResultHeader.Time] = cedmsInspectionResult.InspectionStartTime.ToString("HH:mm:ss.ff");
            tokens[(int)ResultHeader.RollPos] = cedmsInspectionResult.RollDistance.ToString();

            //tokens[(int)ResultHeader.InFeedRaw] = tokens[(int)ResultHeader.InFeedZero] = "0";
            if (cedmsInspectionResult.InFeed != null)
            {
                tokens[(int)ResultHeader.InFeedRaw] = cedmsInspectionResult.InFeed.YRaw.ToString("F4");
                //tokens[(int)ResultHeader.InFeedZero] = cedmsInspectionResult.InFeed.Y.ToString("F4");
            }

            //tokens[(int)ResultHeader.OutFeedRaw] = tokens[(int)ResultHeader.OutFeedZero] = "0";
            if (cedmsInspectionResult.OutFeed != null)
            {
                tokens[(int)ResultHeader.OutFeedRaw] = cedmsInspectionResult.OutFeed.YRaw.ToString("F4");
                //tokens[(int)ResultHeader.OutFeedZero] = cedmsInspectionResult.OutFeed.Y.ToString("F4");
            }

            tokens[(int)ResultHeader.Result] = inspectionResult.Judgment.ToString();
            tokens[(int)ResultHeader.LineSpeed] = cedmsInspectionResult.LineSpeed.ToString();

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
            //workbook.Worksheets[0].Rows[3].Cells[7].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            InspectionResult cedmsInspResult = inspectionResult as InspectionResult;

            Worksheet logSheet = workbook.Worksheets[sheetNo];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Date].Value = cedmsInspResult.InspectionStartTime.ToString("yyyy.MM.dd"); // Date 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Time].Value = cedmsInspResult.InspectionStartTime.ToString("HH:mm:ss"); // Time 

            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.RollPos].Value = cedmsInspResult.RollDistance; // RollPosition 

            //logSheet.Rows[rowNo].Cells[(int)ExcelHeader.InFeedZero].Value = cedmsInspResult.InFeed == null ? "0" : cedmsInspResult.InFeed.Y.ToString("F4"); // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.InFeedRaw].Value = cedmsInspResult.InFeed == null ? "0" : cedmsInspResult.InFeed.YRaw.ToString("F4");

            //logSheet.Rows[rowNo].Cells[(int)ExcelHeader.OutFeedZero].Value = cedmsInspResult.OutFeed == null ? "0" : cedmsInspResult.OutFeed.Y.ToString("F4"); // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.OutFeedRaw].Value = cedmsInspResult.OutFeed == null ? "0" : cedmsInspResult.OutFeed.YRaw.ToString("F4");

            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Result].Value = JudgementString.GetString(cedmsInspResult.Judgment);
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.LineSpeed].Value = cedmsInspResult.LineSpeed.ToString("F4");

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
                int curLength = int.MinValue;
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

                    int rollPos = int.Parse(tokens[(int)ResultHeader.RollPos]);
                    float inFeedRaw = float.Parse(tokens[(int)ResultHeader.InFeedRaw]);
                    //float inFeedZero = float.Parse(tokens[(int)ResultHeader.InFeedZero]);
                    float outFeedRaw = float.Parse(tokens[(int)ResultHeader.OutFeedRaw]);
                    //float outFeedZero = float.Parse(tokens[(int)ResultHeader.OutFeedZero]);

                    Judgment judgment;
                    bool ok = Enum.TryParse(tokens[(int)ResultHeader.Result], out judgment);
                    if (ok == false)
                        judgment = bool.Parse(tokens[(int)ResultHeader.Result]) ? Judgment.Accept : Judgment.Reject;

                    InspectionResult cedmsResult = new InspectionResult();
                    cedmsResult.ResultPath = this.ResultPath;
                    cedmsResult.InspectionNo = inspectionNo;
                    cedmsResult.InspectionStartTime = DateTime.ParseExact(string.Format("{0}_{1}", date, time), "yyyy.MM.dd_HH:mm:ss.ff", null);

                    cedmsResult.InFeed = new CEDMSScanData(rollPos, 0, inFeedRaw,  0);
                    cedmsResult.OutFeed = new CEDMSScanData(rollPos, 0, outFeedRaw, 0);

                    cedmsResult.RollDistance = rollPos;
                    cedmsResult.Judgment = judgment;

                    if (cedmsResult.RollDistance > curLength)
                    {
                        this.inspectionResultList.Add(cedmsResult);
                        curLength = rollPos;
                    }
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
