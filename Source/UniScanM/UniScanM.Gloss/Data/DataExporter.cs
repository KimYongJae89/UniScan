using DynMvp.Base;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UniEye.Base.Settings;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Data
{
    internal enum ResultHeader
    {
        Index = 0, StartDateTime = 1, EndDateTime = 2, RollPosition = 3,
        GlossMin = 4, GlossMax = 5, GlossAvg = 6, GlossDev = 7,
        DistanceMin = 8, DistanceMax = 9, DistanceAvg = 10, DistanceDev = 11,
        RobotPosition = 12, GlossValue = 62, DistanceValue = 112,
        MAX_COUNT = 162
    }

    internal enum ReportInfoHeaderCol
    {
        StartTime = 1, EndTime = 3, LotNo = 5,
        RobotStartPos = 1, RobotEndPos = 3, ValidStartPos = 5, ValidEndPos = 7,
        SheetWidth = 1, StepCount = 5, AvgCount = 7,
        FarDist = 1, NearLeftDist = 2, NearRightDist = 3, XDegree = 4, YDegree = 5, ReferenceMirror = 6,
    }

    internal enum ReportInfoHeaderRow
    {
        StartTime = 3, EndTime = 3, LotNo = 3,
        RobotStartPos = 4, RobotEndPos = 4, ValidStartPos = 4, ValidEndPos = 4,
        SheetWidth = 5, StepCount = 5, AvgCount = 5,
        BFartDist = 8, BNearLeftDist = 8, BNearRightDist = 8, BXDegree = 8, BYDegree = 8, BReferenceMirror = 8,
        CFarDist = 9, CNearLeftDist = 9, CNearRightDist = 9, CXDegree = 9, CYDegree = 9, CReferenceMirror = 9,
        DFarDist = 10, DNearLeftDist = 10, DNearRightDist = 10, DXDegree = 10, DYDegree = 10, DReferenceMirror = 10,
    }

    internal enum ReportHeader
    {
        Index = 0, StartDateTime = 1, EndDateTime = 2, RollPosition = 3,
        GlossMin = 4, GlossMax = 5, GlossAvg = 6, GlossDev = 7,
        DistanceMin = 8, DistanceMax = 9, DistanceAvg = 10, DistanceDev = 11,
        RobotPosition = 12, GlossValue = 62, DistanceValue = 112,
        MAX_COUNT = 162
    }

    public class ReportDataExporter : UniScanM.Data.DataExporter
    {
        public ReportDataExporter() : base()
        {
            this.row_begin = 13;
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_Gloss.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder) { }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult glossInspectionResult = inspectionResult as InspectionResult;
            GlossSettings glossSetting = GlossSettings.Instance();

            string[] tokens = new string[(int)ResultHeader.MAX_COUNT];

            tokens[(int)ResultHeader.Index] = glossInspectionResult.InspectionNo;
            tokens[(int)ResultHeader.StartDateTime] = glossInspectionResult.GlossScanData.StartTime.ToString("yyyy.MM.dd - HH:mm:ss.fff");
            tokens[(int)ResultHeader.EndDateTime] = glossInspectionResult.GlossScanData.EndTime.ToString("yyyy.MM.dd - HH:mm:ss.fff");
            tokens[(int)ResultHeader.RollPosition] = glossInspectionResult.GlossScanData.RollPosition.ToString();

            tokens[(int)ResultHeader.GlossMin] = glossInspectionResult.GlossScanData.MinGloss.ToString();
            tokens[(int)ResultHeader.GlossMax] = glossInspectionResult.GlossScanData.MaxGloss.ToString();
            tokens[(int)ResultHeader.GlossAvg] = glossInspectionResult.GlossScanData.AvgGloss.ToString();
            tokens[(int)ResultHeader.GlossDev] = glossInspectionResult.GlossScanData.DevGloss.ToString();

            tokens[(int)ResultHeader.DistanceMin] = glossInspectionResult.GlossScanData.MinDistance.ToString();
            tokens[(int)ResultHeader.DistanceMax] = glossInspectionResult.GlossScanData.MaxDistance.ToString();
            tokens[(int)ResultHeader.DistanceAvg] = glossInspectionResult.GlossScanData.AvgDistance.ToString();
            tokens[(int)ResultHeader.DistanceDev] = glossInspectionResult.GlossScanData.DevDistance.ToString();

            for (int i = 0; i < glossSetting.StepCount; i++)
            {
                tokens[(int)ResultHeader.RobotPosition + i] = glossInspectionResult.GlossScanData.GlossDatas[i].X.ToString();
                tokens[(int)ResultHeader.GlossValue + i] = glossInspectionResult.GlossScanData.GlossDatas[i].Y.ToString();
                tokens[(int)ResultHeader.DistanceValue + i] = glossInspectionResult.GlossScanData.GlossDatas[i].Distance.ToString();
            }

            string aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
            LogHelper.Debug(LoggerType.Operation, "AppendResult Complete");
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            GlossSettings setting = GlossSettings.Instance();
            var InspectRunner = SystemManager.Instance().InspectRunner as Operation.InspectRunner;
            var calData = InspectRunner.CalibrationData;

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.StartTime].Cells[(int)ReportInfoHeaderCol.StartTime].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.LotNo].Cells[(int)ReportInfoHeaderCol.LotNo].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.RobotStartPos].Cells[(int)ReportInfoHeaderCol.RobotStartPos].Value = setting.SelectedGlossScanWidth.Start;
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.RobotEndPos].Cells[(int)ReportInfoHeaderCol.RobotEndPos].Value = setting.SelectedGlossScanWidth.End;
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.ValidStartPos].Cells[(int)ReportInfoHeaderCol.ValidStartPos].Value = setting.SelectedGlossScanWidth.ValidStart;
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.ValidEndPos].Cells[(int)ReportInfoHeaderCol.ValidEndPos].Value = setting.SelectedGlossScanWidth.ValidEnd;

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.SheetWidth].Cells[(int)ReportInfoHeaderCol.SheetWidth].Value = setting.SelectedGlossScanWidth.Name;
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.StepCount].Cells[(int)ReportInfoHeaderCol.StepCount].Value = setting.StepCount;
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.AvgCount].Cells[(int)ReportInfoHeaderCol.AvgCount].Value = setting.AvgCount;

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BFartDist].Cells[(int)ReportInfoHeaderCol.FarDist].Value = setting.FarFromMirrorSensorValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BNearLeftDist].Cells[(int)ReportInfoHeaderCol.NearLeftDist].Value = setting.NearToMirrorSensorLeftSideValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BNearRightDist].Cells[(int)ReportInfoHeaderCol.NearRightDist].Value = setting.NearToMirrorSensorRightSideValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BXDegree].Cells[(int)ReportInfoHeaderCol.XDegree].Value = setting.AxisXDiffDegree.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BYDegree].Cells[(int)ReportInfoHeaderCol.YDegree].Value = setting.AxisYDiffDegree.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.BReferenceMirror].Cells[(int)ReportInfoHeaderCol.ReferenceMirror].Value = setting.BlackMirrorValue.ToString("F2");

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CFarDist].Cells[(int)ReportInfoHeaderCol.FarDist].Value = calData.FarFromMirrorSensorValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CNearLeftDist].Cells[(int)ReportInfoHeaderCol.NearLeftDist].Value = calData.NearToMirrorSensorLeftSideValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CNearRightDist].Cells[(int)ReportInfoHeaderCol.NearRightDist].Value = calData.NearToMirrorSensorRightSideValue.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CXDegree].Cells[(int)ReportInfoHeaderCol.XDegree].Value = calData.AxisXDiffDegree.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CYDegree].Cells[(int)ReportInfoHeaderCol.YDegree].Value = calData.AxisYDiffDegree.ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.CReferenceMirror].Cells[(int)ReportInfoHeaderCol.ReferenceMirror].Value = calData.BeforeBlackMirrorValue.ToString("F2");

            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DFarDist].Cells[(int)ReportInfoHeaderCol.FarDist].Value = (setting.FarFromMirrorSensorValue - calData.FarFromMirrorSensorValue).ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DNearLeftDist].Cells[(int)ReportInfoHeaderCol.NearLeftDist].Value = (setting.NearToMirrorSensorLeftSideValue - calData.NearToMirrorSensorLeftSideValue).ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DNearRightDist].Cells[(int)ReportInfoHeaderCol.NearRightDist].Value = (setting.NearToMirrorSensorRightSideValue - calData.NearToMirrorSensorRightSideValue).ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DXDegree].Cells[(int)ReportInfoHeaderCol.XDegree].Value = (setting.AxisXDiffDegree - calData.AxisXDiffDegree).ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DYDegree].Cells[(int)ReportInfoHeaderCol.YDegree].Value = (setting.AxisYDiffDegree - calData.AxisYDiffDegree).ToString("F3");
            workbook.Worksheets[0].Rows[(int)ReportInfoHeaderRow.DReferenceMirror].Cells[(int)ReportInfoHeaderCol.ReferenceMirror].Value = (setting.BlackMirrorValue - calData.BeforeBlackMirrorValue).ToString("F2");
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Rows[3].Cells[3].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("yyyy.MM.dd - HH:mm:ss.fff");
            
            GlossSettings glossSetting = GlossSettings.Instance();
            InspectionResult glossInspectionResult = inspectionResult as InspectionResult;
            Worksheet logSheet = workbook.Worksheets[sheetNo];
            
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.Index].Value = glossInspectionResult.InspectionNo;
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.StartDateTime].Value = glossInspectionResult.GlossScanData.StartTime.ToString("yyyy.MM.dd - HH:mm:ss.fff");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.EndDateTime].Value = glossInspectionResult.GlossScanData.EndTime.ToString("yyyy.MM.dd - HH:mm:ss.fff");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.RollPosition].Value = glossInspectionResult.GlossScanData.RollPosition.ToString();

            logSheet.Rows[rowNo].Cells[(int)ReportHeader.GlossMin].Value = glossInspectionResult.GlossScanData.MinGloss.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.GlossMax].Value = glossInspectionResult.GlossScanData.MaxGloss.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.GlossAvg].Value = glossInspectionResult.GlossScanData.AvgGloss.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.GlossDev].Value = glossInspectionResult.GlossScanData.DevGloss.ToString("F3");

            logSheet.Rows[rowNo].Cells[(int)ReportHeader.DistanceMin].Value = glossInspectionResult.GlossScanData.MinDistance.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.DistanceMax].Value = glossInspectionResult.GlossScanData.MaxDistance.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.DistanceAvg].Value = glossInspectionResult.GlossScanData.AvgDistance.ToString("F3");
            logSheet.Rows[rowNo].Cells[(int)ReportHeader.DistanceDev].Value = glossInspectionResult.GlossScanData.DevDistance.ToString("F3");

            for (int i = 0; i < glossSetting.StepCount; i++)
            {
                logSheet.Rows[rowNo].Cells[(int)ReportHeader.RobotPosition + i].Value = glossInspectionResult.GlossScanData.GlossDatas[i].X.ToString("F3");
                logSheet.Rows[rowNo].Cells[(int)ReportHeader.GlossValue + i].Value = glossInspectionResult.GlossScanData.GlossDatas[i].Y.ToString("F3");
                logSheet.Rows[rowNo].Cells[(int)ReportHeader.DistanceValue + i].Value = glossInspectionResult.GlossScanData.GlossDatas[i].Distance.ToString("F3");
            }
            
            return 1;
        }

        protected override void SaveImage(string targetPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
        }

        protected override void OpenFiles(string resultPath, string reportPath, bool forceUpdate = false)
        {
            if (resultPath != null && ((this.resultPath != resultPath) || forceUpdate))
            {
                bool needAppendHeader = false;

                if (File.Exists(resultPath) == false)
                {
                    needAppendHeader = true;
                }

                try
                {
                    SaveResultFile(this.resultPath);

                    string openFilePath = Path.Combine(resultPath, "temp.csv");
                    this.resultFileStream = new StreamWriter(openFilePath, true, Encoding.ASCII);
                    this.resultPath = resultPath;

                    //string openFilePath = Path.Combine(resultPath, "temp.csv");
                    //this.resultFileStream = new StreamWriter(openFilePath, true, Encoding.ASCII);
                    //SaveResultFile(resultPath);

                    //this.resultPath = openFilePath;

                    if (needAppendHeader)
                    {
                        StringBuilder sb = new StringBuilder();
                        WriteCsvHeader(sb);
                        this.resultFileStream.Write(sb);
                    }
                }
                catch (Exception e)
                {
                    this.resultFileStream?.Dispose();
                    this.resultFileStream = null;
                    this.resultPath = "";
                }
            }

            if (reportPath != null && ((reportWorkbookPath != reportPath) || forceUpdate))
            {
                string loadFile = Path.Combine(reportPath, "temp.xlsx");
                bool needAppendHeader = false;
                if (File.Exists(loadFile) == false)
                {
                    needAppendHeader = true;
                    loadFile = GetTemplatePathName();  //★ RawDataTemplate_EDMS.xlsx
                }

                try
                {
                    //SaveReportFile();

                    if (File.Exists(loadFile) == true)
                    {
                        reportWorkbook = Workbook.Load(loadFile); //todo 보관 파일의 크기는 0일 수 없습니다.
                        this.reportWorkbookPath = reportPath;
                        //디버깅중 비정상종료되면 loadFile(temp.xlsx)가 깨져있어서 제대로 Workbook.Load(...) 안먹음...//실제 발생하나? 
                        //System.ArgumentOutOfRangeException: 'Index was out of range. It must be non-negative and less than the size of the collection.
                        string rowCounts = reportWorkbook.Worksheets[0].Rows[0].Cells[0].Value?.ToString();
                        if (int.TryParse(rowCounts, out this.excelRowOffset) == false)
                            this.excelRowOffset = 0;

                        if (needAppendHeader)
                            AppendSheetHeader(reportWorkbook);
                    }
                    else
                    {
                        this.reportWorkbook = null;
                        this.reportWorkbookPath = "";
                    }
                }
                catch (Exception ex)
                {
                    this.reportWorkbook = null;
                    this.reportWorkbookPath = "";
                }
            }
        }

        internal void SaveLotTotalResult(List<InspectionResult> inspectionResultList)
        {
            if (inspectionResultList == null || inspectionResultList.Count == 0)
                return;

            var production = SystemManager.Instance().ProductionManager.CurProduction;
            DateTime date = production.StartTime;
            string month = date.ToString("yyyy-MM");
            string resultPath = PathSettings.Instance().Result;
            string reportPath = Path.Combine(resultPath.Replace("Result", "Report"), month);
            string strFileName = String.Format("{0}\\{1}.csv", reportPath, month);

            List<GlossScanData> glossScanDataList = inspectionResultList.ConvertAll(x => x.GlossScanData);
            float gMin = glossScanDataList.Min(x => x.MinGloss);
            float gMax = glossScanDataList.Max(x => x.MaxGloss);
            float gAvg = glossScanDataList.Average(x => x.AvgGloss);
            float dMin = glossScanDataList.Min(x => x.MinDistance);
            float dMax = glossScanDataList.Max(x => x.MaxDistance);
            float dAvg = glossScanDataList.Average(x => x.AvgDistance);

            var gloassDataList = new List<float>();
            var distanceDataList = new List<float>();
            foreach (var glossData in glossScanDataList)
            {
                gloassDataList.AddRange(glossData.GlossDatas.ConvertAll<float>(x => x.Y));
                distanceDataList.AddRange(glossData.GlossDatas.ConvertAll<float>(x => x.Distance));
            }
            float gDev = DynMvp.Vision.DataProcessing.StdDev(gloassDataList.ToArray());
            float dDev = DynMvp.Vision.DataProcessing.StdDev(distanceDataList.ToArray());

            if (Directory.Exists(reportPath) == false)
                Directory.CreateDirectory(reportPath);

            if (File.Exists(strFileName) == false)
                using (FileStream fs = File.Create(strFileName)) { }

            if (File.Exists(strFileName) == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName, true))
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    stringBuilder.Append(String.Format("LOCALTIME,"));
                    stringBuilder.Append(String.Format("MODELNAME,"));
                    stringBuilder.Append(String.Format("LOTNAME,"));
                    stringBuilder.Append(String.Format("STARTPOSITION,"));
                    stringBuilder.Append(String.Format("ENDPOSITION,"));
                    stringBuilder.Append(String.Format("TOTALDISTANCE,"));
                    stringBuilder.Append(String.Format("GLOSS_MIN,"));
                    stringBuilder.Append(String.Format("GLOSS_MAX,"));
                    stringBuilder.Append(String.Format("GLOSS_AVG,"));
                    stringBuilder.Append(String.Format("GLOSS_DEV,"));
                    stringBuilder.Append(String.Format("SENSOR_MIN,"));
                    stringBuilder.Append(String.Format("SENSOR_MAX,"));
                    stringBuilder.Append(String.Format("SENSOR_AVG,"));
                    stringBuilder.Append(String.Format("SENSOR_DEV,"));

                    stringBuilder.AppendLine();

                    stringBuilder.Append(String.Format("{0},", production.StartTime.ToString("yyyy.MM.dd HH:mm:ss")));
                    stringBuilder.Append(String.Format("{0},", production.Name));
                    stringBuilder.Append(String.Format("{0},", production.LotNo));
                    stringBuilder.Append(String.Format("{0:0.000},", production.LastStartPosition));
                    stringBuilder.Append(String.Format("{0:0.000},", production.EndPosition));
                    stringBuilder.Append(String.Format("{0:0.000},", production.EndPosition - production.LastStartPosition));
                    stringBuilder.Append(String.Format("{0:0.000},", gMin));
                    stringBuilder.Append(String.Format("{0:0.000},", gMax));
                    stringBuilder.Append(String.Format("{0:0.000},", gAvg));
                    stringBuilder.Append(String.Format("{0:0.000},", gDev));
                    stringBuilder.Append(String.Format("{0:0.000},", dMin));
                    stringBuilder.Append(String.Format("{0:0.000},", dMax));
                    stringBuilder.Append(String.Format("{0:0.000},", dAvg));
                    stringBuilder.Append(String.Format("{0:0.000},", dDev));

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();

                    file.Write(stringBuilder);
                    file.Flush();
                }
            }
        }
    }

    public class DataImporter : UniScanM.Data.DataImporter
    {
        protected override bool Import()
        {
            string resultFile = Path.Combine(this.ResultPath, this.resultFileName);
            if (File.Exists(resultFile) == false)
                return false;

            GlossSettings glossSetting = GlossSettings.Instance();
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
                    string startDate = tokens[(int)ResultHeader.StartDateTime];
                    string endDate = tokens[(int)ResultHeader.EndDateTime];
                    DateTime.TryParseExact(startDate, "yyyy.MM.dd - HH:mm:ss.ff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tempStartDate);
                    DateTime.TryParseExact(endDate, "yyyy.MM.dd - HH:mm:ss.ff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tempEndDate);
                    float rollPosition = Convert.ToSingle(tokens[(int)ResultHeader.RollPosition]);

                    float gMin = Convert.ToSingle(tokens[(int)ResultHeader.GlossMin]);
                    float gMax = Convert.ToSingle(tokens[(int)ResultHeader.GlossMax]);
                    float gAvg = Convert.ToSingle(tokens[(int)ResultHeader.GlossAvg]);
                    float gDev = Convert.ToSingle(tokens[(int)ResultHeader.GlossDev]);

                    float dMin = Convert.ToSingle(tokens[(int)ResultHeader.DistanceMin]);
                    float dMax = Convert.ToSingle(tokens[(int)ResultHeader.DistanceMax]);
                    float dAvg = Convert.ToSingle(tokens[(int)ResultHeader.DistanceAvg]);
                    float dDev = Convert.ToSingle(tokens[(int)ResultHeader.DistanceDev]);

                    List<float> XList = new List<float>();
                    List<float> YList = new List<float>();
                    List<float> DistanceList = new List<float>();
                    List<GlossData> glossDatas = new List<GlossData>();

                    for (int i = 0; i < glossSetting.StepCount; i++)
                    {
                        XList.Add(Convert.ToSingle(tokens[(int)ResultHeader.RobotPosition + i]));
                        YList.Add(Convert.ToSingle(tokens[(int)ResultHeader.GlossValue + i]));
                        DistanceList.Add(Convert.ToSingle(tokens[(int)ResultHeader.DistanceValue + i]));
                        glossDatas.Add(new GlossData(XList[i], YList[i], DistanceList[i]));
                    }
                    
                    GlossScanData glossScanData = new GlossScanData();
                    glossScanData.StartTime = tempStartDate;
                    glossScanData.EndTime = tempEndDate;
                    glossScanData.RollPosition = rollPosition;

                    glossScanData.MinGloss = gMin;
                    glossScanData.MaxGloss = gMax;
                    glossScanData.AvgGloss = gAvg;
                    glossScanData.DevGloss = gDev;

                    glossScanData.MinDistance = dMin;
                    glossScanData.MaxDistance = dMax;
                    glossScanData.AvgDistance = dAvg;
                    glossScanData.DevDistance = dDev;

                    glossScanData.GlossDatas = glossDatas;

                    InspectionResult glossResult = new InspectionResult();
                    glossResult.ResultPath = this.ResultPath;
                    glossResult.InspectionNo = inspectionNo;
                    glossResult.GlossScanData = glossScanData;

                    this.inspectionResultList.Add(glossResult);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class ManualDataExporter
    {
        StreamWriter resultFileStream = null;
        string resultPath = "";
        protected string ResultFileName { get => "Result.csv"; }

        public enum ManualType { OnceMeasure, LongRun, Repeat, Degree };
        internal enum ManualResultHeader
        {
            ManualType,
            StartDateTime, /*EndDateTime ,*/
            MeasureValue, LaserDistance, RepeatCount, Name, XDegree, YDegree, BlackMirror,
            MAX_COUNT = 50
        }

        public void OpenFiles(string resultPath)
        {
            if (resultPath != null && (this.resultPath != resultPath))
            {
                try
                {
                    SaveResultFile(this.resultPath);

                    string openFilePath = Path.Combine(resultPath, "temp.csv");
                    this.resultFileStream = new StreamWriter(openFilePath, true, Encoding.ASCII);
                    this.resultPath = resultPath;
                }
                catch
                {
                }
            }
        }

        private void SaveFiles()
        {
            try
            {
                SaveResultFile();
            }
            catch (IOException ex) { }
        }

        private void SaveResultFile(string resultPath = null)
        {
            if (resultPath == null)
                resultPath = this.resultPath;

            if (this.resultFileStream == null)
                return;

            try
            {
                string resultSrcFile = Path.Combine(resultPath, "temp.csv");
                string resultFile = Path.Combine(resultPath, this.ResultFileName);
                lock (this.resultFileStream)
                    this.resultFileStream.Flush();

                if (File.Exists(resultFile))
                {
                    File.SetAttributes(resultFile, FileAttributes.Normal);
                    File.Delete(resultFile);
                }

                File.Copy(resultSrcFile, resultFile, true);
                File.SetAttributes(resultFile, FileAttributes.Normal);
            }
            catch (IOException ex) { }
        }
        StringBuilder stringBuilder = new StringBuilder();

        public void AppendCSV(ManualType manualType, string measureResult, string laserDistance, string name = "", string repeatCount = "")
        {
            if (this.resultFileStream == null)
                return;

            AppendResult(stringBuilder, manualType, measureResult, laserDistance, name, repeatCount);
            lock (this.resultFileStream)
                this.resultFileStream.Write(stringBuilder);

            SaveFiles();
        }

        public void AppendHeader(ManualType manualType)
        {
            if (this.resultFileStream == null)
                return;

            stringBuilder.Clear();

            string[] tokens = new string[(int)ManualResultHeader.MAX_COUNT];
            string aLine;
            switch (manualType)
            {
                case ManualType.OnceMeasure:
                case ManualType.LongRun:
                    stringBuilder.AppendLine();
                    tokens[(int)ManualResultHeader.ManualType] = ManualResultHeader.ManualType.ToString();
                    tokens[(int)ManualResultHeader.StartDateTime] = ManualResultHeader.StartDateTime.ToString();
                    tokens[(int)ManualResultHeader.MeasureValue] = ManualResultHeader.MeasureValue.ToString();
                    tokens[(int)ManualResultHeader.LaserDistance] = ManualResultHeader.LaserDistance.ToString();
                    break;
                case ManualType.Repeat:
                    stringBuilder.AppendLine();
                    tokens[(int)ManualResultHeader.ManualType] = ManualResultHeader.ManualType.ToString();
                    tokens[(int)ManualResultHeader.StartDateTime] = ManualResultHeader.StartDateTime.ToString();
                    tokens[(int)ManualResultHeader.MeasureValue] = ManualResultHeader.MeasureValue.ToString();
                    tokens[(int)ManualResultHeader.LaserDistance] = ManualResultHeader.LaserDistance.ToString();
                    tokens[(int)ManualResultHeader.Name] = ManualResultHeader.Name.ToString();
                    tokens[(int)ManualResultHeader.RepeatCount] = ManualResultHeader.RepeatCount.ToString();
                    break;
                case ManualType.Degree:
                    stringBuilder.AppendLine();
                    tokens[0] = ManualResultHeader.ManualType.ToString();
                    tokens[1] = ManualResultHeader.StartDateTime.ToString();
                    tokens[2] = "BlackMirrorGlossValue";
                    tokens[3] = "Degree";
                    tokens[4] = "TopLeftDistance";
                    tokens[5] = "TopRightDistance";
                    tokens[6] = "BottomLeftDistance";
                    tokens[7] = "BottomRightDistance";
                    tokens[8] = "Diff Axis X Degree";
                    tokens[9] = "Diff Axis Y Degree";
                    break;
            }
            aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);

            lock (this.resultFileStream)
                this.resultFileStream.Write(stringBuilder);

            //SaveFiles();
        }

        public void AppendResult(StringBuilder stringBuilder, ManualType manualType, string measureResult, string laserDistance, string name = "", string repeatCount = "")
        {
            stringBuilder.Clear();

            string[] tokens = new string[(int)ManualResultHeader.MAX_COUNT];
            string aLine;

            switch (manualType)
            {
                case ManualType.OnceMeasure:
                case ManualType.LongRun:
                    tokens[(int)ManualResultHeader.ManualType] = manualType.ToString();
                    tokens[(int)ManualResultHeader.StartDateTime] = DateTime.Now.ToString("yyyy.MM.dd.HH:mm:ss.fff");
                    tokens[(int)ManualResultHeader.MeasureValue] = measureResult;
                    tokens[(int)ManualResultHeader.LaserDistance] = laserDistance;
                    break;
                case ManualType.Repeat:
                    tokens[(int)ManualResultHeader.ManualType] = manualType.ToString();
                    tokens[(int)ManualResultHeader.StartDateTime] = DateTime.Now.ToString("yyyy.MM.dd.HH:mm:ss.fff");
                    tokens[(int)ManualResultHeader.MeasureValue] = measureResult;
                    tokens[(int)ManualResultHeader.LaserDistance] = laserDistance;
                    tokens[(int)ManualResultHeader.Name] = name;
                    tokens[(int)ManualResultHeader.RepeatCount] = repeatCount;
                    break;
                case ManualType.Degree:
                    tokens[(int)ManualResultHeader.ManualType] = manualType.ToString();
                    tokens[(int)ManualResultHeader.StartDateTime] = DateTime.Now.ToString("yyyy.MM.dd.HH:mm:ss.fff");
                    tokens[2] = measureResult;
                    tokens[3] = name;
                    tokens[4] = GlossSettings.Instance().FarFromMirrorSensorValue.ToString();
                    tokens[5] = GlossSettings.Instance().FarFromMirrorSensorValue.ToString();
                    tokens[6] = GlossSettings.Instance().NearToMirrorSensorLeftSideValue.ToString();
                    tokens[7] = GlossSettings.Instance().NearToMirrorSensorRightSideValue.ToString();
                    tokens[8] = GlossSettings.Instance().AxisXDiffDegree.ToString();
                    tokens[9] = GlossSettings.Instance().AxisYDiffDegree.ToString();
                    break;
            }
            aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
        }
    }

}
