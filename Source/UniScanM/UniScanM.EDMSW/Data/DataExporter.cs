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
using UniScanM.EDMSW.Data;
using DynMvp.Vision;
using UniEye.Base.Settings;
using Infragistics.Documents.Excel;
using DynMvp.InspData;
//using UniScanM.Data;

namespace UniScanM.EDMSW.Data
{
    internal enum ResultHeader { Index, Date, Time, Distance,
        //FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0,
        T100, T101, T102, T103, T104, T105, //left
        T200, T201, T202, T203, T204, T205, //right
        W100, W101, W102,
        L100, L200, LDIFF,
        Result, MAX_COUNT }

    internal enum ExcelHeader { INDEX, DATE, TIME, RollPos,
        //FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0,
        T100, T101, T102, T103, T104, T105, //left
        T200, T201, T202, T203, T204, T205, //right
        W100, W101, W102,
        L100, L200, LDIFF,

        Result, ImageName }

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
            UniScanM.EDMSW.Data.InspectionResult edmsResult = inspectionResult as UniScanM.EDMSW.Data.InspectionResult;

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

            //left
            double[] resultArray = edmsInspectionResult.TotalEdgePositionResultLeft;
            tokens[(int)ResultHeader.T100] = resultArray[(int)DataType.FilmEdge].ToString();
            tokens[(int)ResultHeader.T101] = resultArray[(int)DataType.Coating_Film].ToString();
            tokens[(int)ResultHeader.T102] = resultArray[(int)DataType.Printing_Coating].ToString();
            tokens[(int)ResultHeader.T103] = resultArray[(int)DataType.FilmEdge_0].ToString();
            tokens[(int)ResultHeader.T104] = resultArray[(int)DataType.PrintingEdge_0].ToString();
            tokens[(int)ResultHeader.T105] = resultArray[(int)DataType.Printing_FilmEdge_0].ToString();

            //right
            resultArray = edmsInspectionResult.TotalEdgePositionResultRight;
            tokens[(int)ResultHeader.T200] = resultArray[(int)DataType.FilmEdge].ToString();
            tokens[(int)ResultHeader.T201] = resultArray[(int)DataType.Coating_Film].ToString();
            tokens[(int)ResultHeader.T202] = resultArray[(int)DataType.Printing_Coating].ToString();
            tokens[(int)ResultHeader.T203] = resultArray[(int)DataType.FilmEdge_0].ToString();
            tokens[(int)ResultHeader.T204] = resultArray[(int)DataType.PrintingEdge_0].ToString();
            tokens[(int)ResultHeader.T205] = resultArray[(int)DataType.Printing_FilmEdge_0].ToString();

            //Length
            double []  LengthData =edmsInspectionResult.TotalLengthData;
            tokens[(int)ResultHeader.W100] = LengthData[(int)DataType_Length.W100].ToString();
            tokens[(int)ResultHeader.W101] = LengthData[(int)DataType_Length.W101].ToString();
            tokens[(int)ResultHeader.W102] = LengthData[(int)DataType_Length.W102].ToString();
            tokens[(int)ResultHeader.L100] = LengthData[(int)DataType_Length.L100].ToString();
            tokens[(int)ResultHeader.L200] = LengthData[(int)DataType_Length.L200].ToString();
            tokens[(int)ResultHeader.LDIFF] = LengthData[(int)DataType_Length.LDIFF].ToString();

            //
            tokens[(int)ResultHeader.Result] = edmsInspectionResult.Judgment.ToString();
            string aLine = string.Join(",", tokens);
            stringBuilder.AppendLine(aLine);
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_EDMS-W.xlsx";
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

            Data.InspectionResult edmsInspectionResult = inspectionResult as UniScanM.EDMSW.Data.InspectionResult;

            //int writeRow = logSheet.Rows.Count();
            Worksheet logSheet = workbook.Worksheets[sheetNo];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.INDEX].Value = int.Parse(edmsInspectionResult.InspectionNo);
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.DATE].Value = edmsInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd");
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.TIME].Value = edmsInspectionResult.InspectionStartTime.ToString("HH:mm:ss");
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.RollPos].Value = edmsInspectionResult.RollDistance;

            //left
            double[] resultArray = edmsInspectionResult.TotalEdgePositionResultLeft;
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T100].Value = resultArray[(int)DataType.FilmEdge];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T101].Value = resultArray[(int)DataType.Coating_Film];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T102].Value = resultArray[(int)DataType.Printing_Coating];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T103].Value = resultArray[(int)DataType.FilmEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T104].Value = resultArray[(int)DataType.PrintingEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T105].Value = resultArray[(int)DataType.Printing_FilmEdge_0];
            
            //right
            resultArray = edmsInspectionResult.TotalEdgePositionResultRight;
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T200].Value = resultArray[(int)DataType.FilmEdge];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T201].Value = resultArray[(int)DataType.Coating_Film];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T202].Value = resultArray[(int)DataType.Printing_Coating];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T203].Value = resultArray[(int)DataType.FilmEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T204].Value = resultArray[(int)DataType.PrintingEdge_0];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.T205].Value = resultArray[(int)DataType.Printing_FilmEdge_0];

            //length
            resultArray = edmsInspectionResult.TotalLengthData;
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.W100].Value = resultArray[(int)DataType_Length.W100];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.W101].Value = resultArray[(int)DataType_Length.W101];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.W102].Value = resultArray[(int)DataType_Length.W102];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.L100].Value = resultArray[(int)DataType_Length.L100];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.L200].Value = resultArray[(int)DataType_Length.L200];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.LDIFF].Value = resultArray[(int)DataType_Length.LDIFF];

            //
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

                        //left
                        float filmEdge = float.Parse(tokens[(int)ResultHeader.T100]);
                        float coating_Film = float.Parse(tokens[(int)ResultHeader.T101]);
                        float printing_Coating = float.Parse(tokens[(int)ResultHeader.T102]);
                        float filmEdge_0 = float.Parse(tokens[(int)ResultHeader.T103]);
                        float printingEdge_0 = float.Parse(tokens[(int)ResultHeader.T104]);
                        float printing_FilmEdge_0 = float.Parse(tokens[(int)ResultHeader.T105]);
                        edmsInspectionResult.TotalEdgePositionResultLeft = new double[] { filmEdge, coating_Film, printing_Coating, filmEdge_0, printingEdge_0, printing_FilmEdge_0 };

                        //right
                        filmEdge = float.Parse(tokens[(int)ResultHeader.T200]);
                        coating_Film = float.Parse(tokens[(int)ResultHeader.T201]);
                        printing_Coating = float.Parse(tokens[(int)ResultHeader.T202]);
                        filmEdge_0 = float.Parse(tokens[(int)ResultHeader.T203]);
                        printingEdge_0 = float.Parse(tokens[(int)ResultHeader.T204]);
                        printing_FilmEdge_0 = float.Parse(tokens[(int)ResultHeader.T205]);
                        edmsInspectionResult.TotalEdgePositionResultRight = new double[] { filmEdge, coating_Film, printing_Coating, filmEdge_0, printingEdge_0, printing_FilmEdge_0 };

                        //
                        float W100 = float.Parse(tokens[(int)ResultHeader.W100]);
                        float W101 = float.Parse(tokens[(int)ResultHeader.W101]);
                        float W102 = float.Parse(tokens[(int)ResultHeader.W102]);

                        float L100 = float.Parse(tokens[(int)ResultHeader.L100]);
                        float L200 = float.Parse(tokens[(int)ResultHeader.L200]);
                        float LDIFF = float.Parse(tokens[(int)ResultHeader.LDIFF]);
                        edmsInspectionResult.TotalLengthData = new double[] { W100, W101, W102, L100, L200, LDIFF };


                        //
                        DynMvp.InspData.Judgment judgment = (DynMvp.InspData.Judgment)Enum.Parse(typeof(DynMvp.InspData.Judgment), tokens[(int)ResultHeader.Result]);

                        edmsInspectionResult.InspectionNo = inspectionNo;
                        edmsInspectionResult.InspectionStartTime = date + time;
                        edmsInspectionResult.RollDistance = rollDistance;
                        edmsInspectionResult.Judgment = judgment;

                        this.inspectionResultList.Add(edmsInspectionResult);
                    }

                    this.inspectionResultList.RemoveAll(f => f.Judgment == Judgment.Skip);
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
