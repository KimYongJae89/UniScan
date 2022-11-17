using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public interface IExportable
    {
        void Export(XmlElement element, string subKey = null);
    }

    public interface IImportable
    {
        void Import(XmlElement element, string subKey = null);
    }

    public abstract class DataExporter : DynMvp.Data.DataExporter
    {
        protected int row_begin = 0;
        protected int ROW_BEGIN { get => row_begin; }

        protected const string IMAGE_SAVE_SKIPPED = "Image Save Skipped";
        protected int MAX_SHEET_ROWS = Workbook.GetMaxRowCount(WorkbookFormat.Excel2007);// 1048576;

        protected string resultFileName = "Result.csv";
        public string ResultFileName { get => resultFileName; set => resultFileName = value; }

        protected string reportFileName = "Report.xlsx";
        public string ReportFileName { get => reportFileName; set => reportFileName = value; }

        protected StreamWriter resultFileStream = null;
        protected string resultPath = "";

        protected Workbook reportWorkbook = null;
        protected string reportWorkbookPath = "";

        List<int> averageCountList = null;
        protected List<Tuple<InspectionResult, CancellationToken>> inspectionResultTupleList = null;
        ThreadHandler excelSaveThred = null;

        protected int excelRowOffset = 0;

        public DataExporter()
        {
            this.averageCountList = new List<int>();

            this.inspectionResultTupleList = new List<Tuple<InspectionResult, CancellationToken>>();
            this.excelSaveThred = new ThreadHandler("ExcelSaveThred", new Thread(SaveProc), false);
            this.excelSaveThred.Start();
        }

        ~DataExporter()
        {
            this.excelSaveThred?.Stop();
        }

        protected abstract void WriteCsvHeader(StringBuilder stringBuilder);
        protected void WriteCsvHeader(StringBuilder sb, Type resultHeader)
        {
            sb.AppendLine("Date Report");
            sb.AppendLine(string.Format("Start Date,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy-MM-dd")));
            sb.AppendLine(string.Format("Start Time,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("HH:mm:ss")));
            sb.AppendLine(string.Format("Lot No,{0}", SystemManager.Instance().ProductionManager.CurProduction.LotNo));

            Array arr = Enum.GetValues(resultHeader);
            foreach (Enum e in arr)
                sb.Append(string.Format("{0},", e.ToString()));
            sb.AppendLine();

            //File.WriteAllText(resultFile, sb.ToString(), Encoding.Default);
            //File.SetAttributes(resultFile, FileAttributes.Hidden);
        }

        protected abstract void AppendSheetHeader(Workbook workbook);
        protected abstract int AppendSheetData(Workbook workbook,int sheetNo, int rowNo, InspectionResult inspectionResult);

        protected abstract void AppendResult(StringBuilder stringBuilder, InspectionResult inspectionResult);
        protected abstract string GetTemplateName();
                                           
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //System.Diagnostics.Debug.WriteLine("Call: UniScanM.Data.DataExporter::Export");
            InspectionResult uniScanMInspectionResult = inspectionResult as InspectionResult;
            lock (this.inspectionResultTupleList)
                this.inspectionResultTupleList.Add(new Tuple<InspectionResult, CancellationToken>(uniScanMInspectionResult, cancellationToken));
        }

        private void AppendCSV(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (this.resultFileStream == null)
                return;

            StringBuilder sb = new StringBuilder();

            AppendResult(sb, inspectionResult);
            lock (this.resultFileStream)
                this.resultFileStream.Write(sb);
        }

        protected abstract void SaveImage(string targetPath, bool skipImageSave, InspectionResult inspectionResult, CancellationToken cancellationToken);

        private void AppendExcel(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (this.reportWorkbook == null)
                return;

            lock (this.reportWorkbook)
            {
                int targetRow = excelRowOffset + this.row_begin;
                int sheetNo = targetRow / MAX_SHEET_ROWS;
                int rowNo = targetRow % MAX_SHEET_ROWS;

                int nextSheetNo = targetRow / MAX_SHEET_ROWS + 1;
                string sheetName = string.Format("LOG{0}", sheetNo == 0 ? "" : (sheetNo+1).ToString());
                string nextSheetName = string.Format("LOG{0}", nextSheetNo == 0 ? "" : (nextSheetNo+1).ToString());

                if (reportWorkbook.Worksheets.Exists(nextSheetName) == false)
                {
                    Worksheet nextWorksheet = reportWorkbook.Worksheets.Add(nextSheetName);
                    foreach (WorksheetColumn sourceColumn in this.reportWorkbook.Worksheets[0].Columns)
                    {
                        WorksheetColumn destinationColumn = nextWorksheet.Columns[sourceColumn.Index];
                        destinationColumn.CellFormat.SetFormatting(CreateFormatCopy(reportWorkbook, sourceColumn.CellFormat));
                        destinationColumn.Width = sourceColumn.Width;
                        destinationColumn.Hidden = sourceColumn.Hidden;
                    }
                }

                Worksheet targetWorksheet = reportWorkbook.Worksheets[sheetName];

                int newRowCount = AppendSheetData(reportWorkbook, sheetNo,rowNo, inspectionResult);
                excelRowOffset += newRowCount;
                reportWorkbook.Worksheets[0].Rows[0].Cells[0].Value = excelRowOffset.ToString();
            }
        }

        private void SaveProc()
        {
            while (this.excelSaveThred.RequestStop == false)
            {
                if (this.inspectionResultTupleList.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                List<Tuple<InspectionResult, CancellationToken>> inspectionResultList = new List<Tuple<InspectionResult, CancellationToken>>();
                lock (this.inspectionResultTupleList)
                {
                    inspectionResultList.AddRange(this.inspectionResultTupleList);
                    this.inspectionResultTupleList.Clear();
                }

                Debug.WriteLine(string.Format("DataExporter::SaveProc - Save {0} item(s)", inspectionResultList.Count));

                this.averageCountList.Add(inspectionResultList.Count);
                if (this.averageCountList.Count > 10)
                    this.averageCountList.RemoveRange(0, Math.Max(0, this.averageCountList.Count - 10));
                bool skipImageSave = this.averageCountList.Average() > 20;

                //System.Diagnostics.Debug.WriteLine(string.Format("Save Start: {0} EA. SkipImageSave {1}", inspectionResultList.Count, skipImageSave));
                DateTime startDt = DateTime.Now;
                while (inspectionResultList.Count > 0)
                {
                    Tuple<InspectionResult, CancellationToken> inspectionResultTuple = inspectionResultList[0];
                    inspectionResultList.RemoveAt(0);

                    InspectionResult inspectionResult = inspectionResultTuple.Item1;
                    CancellationToken cancellationToken = inspectionResultTuple.Item2;

                    OpenFiles(inspectionResult.ResultPath, inspectionResult.ReportPath);

                    // Save to result
                    SaveImage(inspectionResult.ResultPath, skipImageSave, inspectionResult, cancellationToken);
                    AppendCSV(inspectionResult, cancellationToken);

                    // Save to report
                    //SaveImage(inspectionResult.ReportPath, skipImageSave, inspectionResult, cancellationToken); //쓸데없이 용량만 많이 막아서 일단 저장 않기로함
                    AppendExcel(inspectionResult, cancellationToken);
                }
                //System.Diagnostics.Debug.WriteLine(string.Format("Append Done. {0} ms", (DateTime.Now - startDt).TotalMilliseconds));

                try
                {
                    SaveFiles();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("DataExporter::SaveProc Exception - {0}", ex.Message));
                }
                Debug.WriteLine(string.Format("Save Done. {0} ms", (DateTime.Now - startDt).TotalMilliseconds));
                //Thread.Sleep(1000);
            }
        }

        private void SaveFiles()
        {
            try
            {
                SaveResultFile();
                SaveReportFile();
            }
            catch (IOException ex) { }
        }

        protected void SaveResultFile(string resultPath = null)
        {
            if (resultPath == null)
                resultPath = this.resultPath;

            if (this.resultFileStream == null)
                return;

            try
            {
                string resultSrcFile = Path.Combine(resultPath, "temp.csv");
                string resultFile = Path.Combine(resultPath,this.ResultFileName);
                lock (this.resultFileStream)
                    this.resultFileStream.Flush();

                if (File.Exists(resultFile))
                {
                    File.SetAttributes(resultFile, FileAttributes.Normal);
                    File.Delete(resultFile);
                }

                File.Copy(resultSrcFile, resultFile,true);
                File.SetAttributes(resultFile, FileAttributes.Normal);
            }
            catch (IOException ex) { }
        }

        protected void SaveReportFile(string reportPath=null)
        {
            if (reportPath == null)
                reportPath = this.reportWorkbookPath;

            if (this.reportWorkbook == null)
                return;

            try
            {
                string reportSrcFile = Path.Combine(reportPath, "temp.xlsx");
                lock (reportWorkbook)
                    reportWorkbook.Save(reportSrcFile);

                string reportFile = Path.Combine(reportPath, this.ReportFileName);

                if (File.Exists(reportFile))
                {
                    File.SetAttributes(reportFile, FileAttributes.Normal);
                    File.Delete(reportFile);
                }

                File.Copy(reportSrcFile, reportFile,true);
                File.SetAttributes(reportFile, FileAttributes.Normal);
            }
            catch (IOException ex) { }
        }

        IWorksheetCellFormat CreateFormatCopy(Workbook workbook, IWorksheetCellFormat sourceCellFormat)
        {
            IWorksheetCellFormat copy = workbook.CreateNewWorksheetCellFormat();
            copy.SetFormatting(sourceCellFormat);
            return copy;
        }

        protected virtual void OpenFiles(string resultPath, string reportPath, bool forceUpdate = false)
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
                    SaveReportFile();

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
                catch(Exception ex)
                {
                    this.reportWorkbook = null;
                    this.reportWorkbookPath = "";
                }
            }
        }

        protected string GetTemplatePathName()
        {
            string reportTemplateName = GetTemplateName();
            string rawDataTemplatePath = Path.Combine(PathSettings.Instance().Result, reportTemplateName);
#if DEBUG
            string tt= Path.Combine(@"D:\Project_UniScan\UniScan\Runtime\Result", reportTemplateName);
            if (File.Exists(tt))
                rawDataTemplatePath = tt;
#endif
            return rawDataTemplatePath;
        }

        //private void SaveLastResult(string resultPath, DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        //{
        //    string lastResultPath = Path.Combine(PathSettings.Instance().Result, "LastResult.csv");

        //    if (File.Exists(lastResultPath) == false)
        //    {
        //        string tempLastResultPath = Path.Combine(PathSettings.Instance().Result, "TempLastResult.csv");

        //        StringBuilder sb = new StringBuilder();
        //        AppendResult(sb, inspectionResult);
        //        File.WriteAllText(tempLastResultPath, sb.ToString());

        //        File.Move(tempLastResultPath, lastResultPath);
        //    }
        //}
    }

    public abstract class DataImporter
    {
        public string ResultPath { get { return this.production?.GetResultPath(); } }

        public Production Production { get => this.production; }
        Production production;

        protected string resultFileName = "Result.csv";
        protected List<InspectionResult> inspectionResultList;
        public List<InspectionResult> InspectionResultList { get { return inspectionResultList; } }

        public int Count { get { return inspectionResultList == null ? 0 : inspectionResultList.Count; } }

        public DataImporter()
        {
            this.inspectionResultList = new List<InspectionResult>();
        }

        public InspectionResult GetInspectionResult(int i)
        {
            return inspectionResultList[i];
        }

        public bool Import(Production production, string resultFileName = "Result.csv")
        {
            this.inspectionResultList.Clear();

            this.production = production;
            this.resultFileName = resultFileName;

            string resultFile = Path.Combine(this.production.GetResultPath(), this.resultFileName);
            if (File.Exists(resultFile) == false)
                return false;

            this.inspectionResultList.Clear();
            bool ok = false;

            try
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() =>
                {
                    ok = Import();
                });
            }
            catch (Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
            return ok;
        }
        protected abstract bool Import();
    }
}
