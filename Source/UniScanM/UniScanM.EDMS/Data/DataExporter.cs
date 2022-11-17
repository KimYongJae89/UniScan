using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
//using DynMvp.InspData;
using System.Threading;
using System.Xml;
using DynMvp.Base;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using UniEye.Base;
using System.Diagnostics;
using UniScanM.EDMS.Data;
using DynMvp.Vision;
using UniEye.Base.Settings;
using Infragistics.Documents.Excel;
using DynMvp.InspData;
//using UniScanM.Data;

namespace UniScanM.EDMS.Data
{
    internal enum ResultHeader { Index, Date, Time, Distance, FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0, Result, MAX_COUNT }
    internal enum ExcelHeader { INDEX, DATE, TIME, RollPos, FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0, Result, ImageName }

    public class ReportDataExporter : UniScanM.Data.DataExporter
    {
        public ReportDataExporter() : base()
        {
            this.row_begin = 8;
        }
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //System.Diagnostics.Debug.WriteLine("Call: UniScanM.Data.DataExporter::Export");
            UniScanM.Data.InspectionResult uniScanMInspectionResult = inspectionResult as UniScanM.Data.InspectionResult;
            UniScanM.EDMS.Data.InspectionResult edmsResult = inspectionResult as UniScanM.EDMS.Data.InspectionResult;

            if (edmsResult.State == State_EDMS.Inspecting)
            {
                if (edmsResult.Judgment != Judgment.Skip)
                {
                    lock (this.inspectionResultTupleList)
                        this.inspectionResultTupleList.Add(new Tuple<UniScanM.Data.InspectionResult, CancellationToken>(uniScanMInspectionResult, cancellationToken));
                }
            }
        }
        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult edmsInspectionResult = inspectionResult as InspectionResult;
            Settings.EDMSSettings edmsSetting = Settings.EDMSSettings.Instance();

            string[] tokens = new string[(int)ResultHeader.MAX_COUNT];
            tokens[(int)ResultHeader.Index] = edmsInspectionResult.InspectionNo;
            tokens[(int)ResultHeader.Date] = edmsInspectionResult.InspectionStartTime.ToString("yyyyMMdd");
            tokens[(int)ResultHeader.Time] = edmsInspectionResult.InspectionStartTime.ToString("HHmmss");
            tokens[(int)ResultHeader.Distance] = edmsInspectionResult.RollDistance.ToString();

            double[] resultArray = edmsInspectionResult.TotalEdgePositionResult;

            tokens[(int)ResultHeader.FilmEdge] = resultArray[(int)DataType.FilmEdge].ToString();
            tokens[(int)ResultHeader.Coating_Film] = resultArray[(int)DataType.Coating_Film].ToString();
            tokens[(int)ResultHeader.Printing_Coating] = resultArray[(int)DataType.Printing_Coating].ToString();
            tokens[(int)ResultHeader.FilmEdge_0] = resultArray[(int)DataType.FilmEdge_0].ToString();
            tokens[(int)ResultHeader.PrintingEdge_0] = resultArray[(int)DataType.PrintingEdge_0].ToString();
            tokens[(int)ResultHeader.Printing_FilmEdge_0] = resultArray[(int)DataType.Printing_FilmEdge_0].ToString();

            tokens[(int)ResultHeader.Result] = edmsInspectionResult.Judgment.ToString();

            string aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_EDMS.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //base.WriteCsvHeader(stringBuilder, typeof(ResultHeader));

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Date Report");
            //sb.AppendLine(string.Format("Start Date,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy-MM-dd")));
            //sb.AppendLine(string.Format("Start Time,{0}", SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("HH:mm:ss")));
            //sb.AppendLine(string.Format("Lot No.,{0}", SystemManager.Instance().ProductionManager.CurProduction.LotNo));
            //sb.AppendLine("Index,Date,Time,Film[um],Coating[um],Printing[um],Film (from 0)[um],Printing (from 0)[um],Printing - Film (from 0)[um]");
            //File.WriteAllText(resultFile, sb.ToString(), Encoding.Default);
        }

        protected override void SaveImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            InspectionResult edmsInspectionResult = inspectionResult as InspectionResult;

            int inspectionNo = 0;
            int.TryParse(inspectionResult.InspectionNo, out inspectionNo);

            // Accept라도 저장함
            bool isMeasureState = edmsInspectionResult.State == State_EDMS.Inspecting;
            int interval = Settings.EDMSSettings.Instance().ImageSavingInterval;

            if (isMeasureState == false) return; //검사중이 아니면 저장안함.
            if (OperationSettings.Instance().SaveDebugImage == false)  // 설정: 저장안함
            {
                if(inspectionResult.Judgment == Judgment.Accept) return; //정상일 경우 건너뛰고, 저장안함으로 설정해도, 불량일경우 저장
            }
            else /////////////////////////////////////////////////////////설정: 저장함
            {
                if (inspectionResult.Judgment == Judgment.Accept)//정상 이미지일경우
                {
                    if (interval <= 0) return; //잘못 설정하면 저장안함 
                    if(inspectionNo % interval != 0) return; //저장 건너뛰기
                }
                else
                {
                    //불량이면 저장
                }                
            }
            ////////////////////////////////////////////////////////////////////////////////////////////
            string fileName = string.Format("{0}.jpg", inspectionResult.InspectionNo);
            if(skipImageSave)
            {
                inspectionResult.DisplayBitmapSaved = IMAGE_SAVE_SKIPPED;
            }
            else if (edmsInspectionResult.DisplayBitmap != null)
            {
                ImageHelper.SaveImage(edmsInspectionResult.DisplayBitmap, Path.Combine(resultPath, fileName));
                inspectionResult.DisplayBitmapSaved = fileName;
            }

            //-----------------------------------
            //Debug.WriteLine(LoggerType.Debug, string.Format("Gc Status: {0}", status.ToString()));
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[2].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[7].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Workbook.Worksheets[0].Rows[3].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("yyyy.MM.dd HH:mm:ss"); // End Time
            workbook.Worksheets[0].Workbook.Worksheets[0].Rows[3].Cells[9].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            Data.InspectionResult edmsInspectionResult = inspectionResult as UniScanM.EDMS.Data.InspectionResult;
   
            double[] resultArray = edmsInspectionResult.TotalEdgePositionResult;

            //int writeRow = logSheet.Rows.Count();
            Worksheet logSheet = workbook.Worksheets[sheetNo];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.INDEX].Value = int.Parse(edmsInspectionResult.InspectionNo);
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.DATE].Value = edmsInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd");
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.TIME].Value = edmsInspectionResult.InspectionStartTime.ToString("HH:mm:ss");
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.RollPos].Value = edmsInspectionResult.RollDistance;
                          
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.FilmEdge].Value = resultArray[(int)DataType.FilmEdge];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Coating_Film].Value = resultArray[(int)DataType.Coating_Film];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Printing_Coating].Value = resultArray[(int)DataType.Printing_Coating];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.FilmEdge_0].Value = resultArray[(int)DataType.FilmEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.PrintingEdge_0].Value = resultArray[(int)DataType.PrintingEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Printing_FilmEdge_0].Value = resultArray[(int)DataType.Printing_FilmEdge_0];
                          
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Result].Value = JudgementString.GetString(edmsInspectionResult.Judgment);

            if (string.IsNullOrEmpty(edmsInspectionResult.DisplayBitmapSaved) == false)
                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.ImageName].Value = edmsInspectionResult.DisplayBitmapSaved;

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
                /*
                StreamReader sr = new StreamReader(resultFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {/*/
                using (FileStream fs = File.Open(resultFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))//버퍼드스트림 꼬옥~ 쓰자...
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    { //*/



                        string[] tokens = line.Split(',');
                        if (tokens.Length != (int)ResultHeader.MAX_COUNT)
                            continue;

                        int index;
                        if (int.TryParse(tokens[(int)ResultHeader.Index], out index) == false)
                            continue;

                        InspectionResult edmsInspectionResult = new InspectionResult();
                        edmsInspectionResult.ResultPath = this.ResultPath;

                        string inspectionNo = tokens[(int)ResultHeader.Index];
                        DateTime date = DateTime.ParseExact(tokens[(int)ResultHeader.Date], "yyyyMMdd", null);
                        TimeSpan time = TimeSpan.ParseExact(tokens[(int)ResultHeader.Time], "hhmmss", null);
                        int rollDistance = int.Parse(tokens[(int)ResultHeader.Distance]);

                        float filmEdge = float.Parse(tokens[(int)ResultHeader.FilmEdge]);
                        float coating_Film = float.Parse(tokens[(int)ResultHeader.Coating_Film]);
                        float printing_Coating = float.Parse(tokens[(int)ResultHeader.Printing_Coating]);
                        float filmEdge_0 = float.Parse(tokens[(int)ResultHeader.FilmEdge_0]);
                        float printingEdge_0 = float.Parse(tokens[(int)ResultHeader.PrintingEdge_0]);
                        float printing_FilmEdge_0 = float.Parse(tokens[(int)ResultHeader.Printing_FilmEdge_0]);
                        DynMvp.InspData.Judgment judgment = (DynMvp.InspData.Judgment)Enum.Parse(typeof(DynMvp.InspData.Judgment), tokens[(int)ResultHeader.Result]);

                        edmsInspectionResult.InspectionNo = inspectionNo;
                        edmsInspectionResult.InspectionStartTime = date + time;
                        edmsInspectionResult.RollDistance = rollDistance;

                        edmsInspectionResult.TotalEdgePositionResult = new double[] { filmEdge, coating_Film, printing_Coating, filmEdge_0, printingEdge_0, printing_FilmEdge_0 };

                        edmsInspectionResult.Judgment = judgment;

                        this.inspectionResultList.Add(edmsInspectionResult);
                    }

                    this.inspectionResultList.RemoveAll(f => f.Judgment == Judgment.Skip);

                    if (float.IsNaN(this.Production.Value))
                    // Production의 Value 값 업데이트 필요함..
                    {
                        float max = (float)this.inspectionResultList.Max(f => ((InspectionResult)f).TotalEdgePositionResult[(int)Data.DataType.Printing_FilmEdge_0]);
                        float min = (float)this.inspectionResultList.Min(f => ((InspectionResult)f).TotalEdgePositionResult[(int)Data.DataType.Printing_FilmEdge_0]);
                        float value = max - min;
                        this.Production.Value = value;
                        SystemManager.Instance().ProductionManager.Save();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            //if (InvokeRequired)
            //{
            //    Invoke(new UpdateDataDelegate(UpdateData), path);
            //    return;
            //}

            //string filePath = Path.Combine(path, "Result.csv");

            //if (File.Exists(filePath) == false)
            //    return;

            //string[] lines = File.ReadAllLines(filePath);

            //lines = lines.Skip(Math.Min(lines.Length, 5)).ToArray();

            //List<ChartData> t100DataList = new List<ChartData>();
            //List<ChartData> t101DataList = new List<ChartData>();
            //List<ChartData> t102DataList = new List<ChartData>();
            //List<ChartData> t103DataList = new List<ChartData>();
            //List<ChartData> t104DataList = new List<ChartData>();
            //List<ChartData> t105DataList = new List<ChartData>();

            //string format = "yyyyMMdd.HHmmss.fff";
            //MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            //foreach (string line in lines)
            //{
            //    DateTime curTime;

            //    string[] lineToken = line.Split(',');

            //    if (DateTime.TryParseExact(lineToken[0], format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out curTime) == false)
            //        continue;

            //    float distance;
            //    if (float.TryParse(lineToken[9], out distance) == false)
            //        continue;

            //    t100DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[3])));
            //    t101DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[4])));
            //    t102DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[5])));
            //    t103DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[6])));
            //    t104DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[7])));
            //    t105DataList.Add(new ChartData(distance, Convert.ToSingle(lineToken[8])));
            //}

            //profilePanelList.ForEach(panel => panel.Initialize());
            //profilePanelList.ForEach(panel => panel.ClearPanel());

            //t100DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t101DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t102DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t103DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t104DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t105DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));

            //t100.AddChartDataList(t100DataList);
            //t101.AddChartDataList(t101DataList);
            //t102.AddChartDataList(t102DataList);
            //t103.AddChartDataList(t103DataList);
            //t104.AddChartDataList(t104DataList);
            //t105.AddChartDataList(t105DataList);

            //profilePanelList.ForEach(panel => panel.DisplayResult());
        }
            }
}
