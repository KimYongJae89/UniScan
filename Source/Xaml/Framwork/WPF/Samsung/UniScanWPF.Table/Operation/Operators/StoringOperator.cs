using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Helper;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.Operation.Operators
{
    public class StoringOperator : Operator
    {
        const string reportTamplateFile = "RawDataTemplate_Offline.xlsx";

        Task saveTask = null;
        public StoringOperator()
        {
        }

        public override bool Initialize(ResultKey resultKey, int totalProgressSteps, CancellationTokenSource cancellationTokenSource)
        {
            return true;
        }

        public static List<LoadItem> Load(Production production)
        {
            string resultPath = production.GetResultPath();

            List<LoadItem> list = new List<LoadItem>();

            for (int i = 0; i < production.Count; i++)
            {
                // resultPath 는 1-base
                string subResultPath = Path.Combine(resultPath, (i + 1).ToString());
                list.Add(new LoadItem(subResultPath));
            }

            // 가장 최근것이 1번
            list.Reverse();

            return list;
        }

        public void Save(List<ExtractOperatorResult> extractOperatorResultList, List<CanvasDefect> defectList)
        {
            if (extractOperatorResultList == null || extractOperatorResultList.Count == 0 || defectList == null)
                return;

            if (saveTask != null && saveTask.IsCompleted == false)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    WpfControlLibrary.UI.SimpleProgressWindow simpleProgressWindow = new WpfControlLibrary.UI.SimpleProgressWindow(LocalizeHelper.GetString("Wait Previous Save Task Done..."));
                    simpleProgressWindow.Show(() =>
                    {
                        saveTask.Wait();
                    }, this.cancellationTokenSource);
                });
            }

            this.saveTask = Task.Factory.StartNew(() => StartSave(extractOperatorResultList, defectList));
        }

        private void StartSave(List<ExtractOperatorResult> extractOperatorResultList, List<CanvasDefect> canvasDefectList)
        {
            Production production = SystemManager.Instance().ProductionManager.CurProduction;
            ResultKey resultKey = extractOperatorResultList.First().ResultKey;

            base.Initialize(resultKey, 300, cancellationTokenSource);

            OperatorState = OperatorState.Run;

            production.AddCount(canvasDefectList);
            SystemManager.Instance().ProductionManager.Save();

            // resultPath 는 1-base
            int repeatCount = resultKey.Production.Count;
            string resultPath = production.GetResultPath();
            if (!Directory.Exists(resultPath))
                Directory.CreateDirectory(resultPath);

            string reportFolder = Path.Combine(PathSettings.Instance().Result.Replace("Result", "Report"), resultKey.DateTime.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);

            string marginFolder = Path.Combine(PathSettings.Instance().Result.Replace("Result", "Margin"), resultKey.DateTime.ToString("yyyy"), resultKey.DateTime.ToString("MM"));
            if (!Directory.Exists(marginFolder))
                Directory.CreateDirectory(marginFolder);

            this.CurProgressSteps = 0;

            bool retry = false;
            do
            {
                try
                {
                    LogHelper.Debug(LoggerType.Operation, "Save Margin Start");
                    string path = Path.Combine(marginFolder, $@"{resultKey.DateTime.Month:00}.temp");
                    SaveMargin(path, resultKey, canvasDefectList);
                    retry = false;
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Error, $"StoringOperator::StartSave - Margin Data Save Fail. {ex.Message}");
                    string message = $"Margin Data Save Fail.{Environment.NewLine}{ex.Message}{Environment.NewLine}Retry?";
                    retry = (MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                }
                LogHelper.Debug(LoggerType.Operation, "Save Margin End");
            } while (retry);
            this.CurProgressSteps = 100;


            do
            {
                try
                {
                    LogHelper.Debug(LoggerType.Operation, "Save Result Start");
                    SaveResult(Path.Combine(resultPath, repeatCount.ToString()), canvasDefectList, extractOperatorResultList);
                    retry = false;
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Error, $"StoringOperator::StartSave - Review Data Save Fail. {ex.Message}");
                    string message = $"Review Data Save Fail.{Environment.NewLine}{ex.Message}{Environment.NewLine}Retry?";
                    retry = (MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                }
                LogHelper.Debug(LoggerType.Operation, "Save Result End");
            } while (retry);
            this.CurProgressSteps = 200;

            do
            {
                try
                {
                    LogHelper.Debug(LoggerType.Operation, "Save Report Start");
                    string reportPath = Path.Combine(reportFolder, string.Format("{0}_{1}.xlsx", resultKey.Model.Name, resultKey.LotNo));
                    SaveReport(reportPath, repeatCount, resultKey, canvasDefectList, extractOperatorResultList);
                    retry = false;
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Error, $"StoringOperator::StartSave - Excel Data Save Fail. {ex.Message}");
                    string message = $"Excel Data Save Fail.{Environment.NewLine}{ex.Message}{Environment.NewLine}Retry?";
                    retry = (MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                }
                LogHelper.Debug(LoggerType.Operation, "Save Report End");
            } while (retry);

        
            this.CurProgressSteps = 300;

            OperatorState = OperatorState.Idle;
        }

        public void SaveReport(string targetPath, int repeatCount, ResultKey resultKey, List<CanvasDefect> canvasDefectList, List<ExtractOperatorResult> extractOperatorResultList)
        {
            string templateFilePaht = Path.Combine(PathSettings.Instance().Result, reportTamplateFile);
            //#if DEBUG
            //            string debugTemplateFilePath = Path.Combine(@"D:\Project_UniScan\UniScan\Runtime\Result", reportTamplateFile);
            //            if (File.Exists(debugTemplateFilePath))
            //                templateFilePaht = debugTemplateFilePath;
            //#endif
            if (File.Exists(templateFilePaht) == false)
                throw new Exception("No Template File Exist");
            
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Sheets sheets = null;
            Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                System.Diagnostics.Debug.WriteLine(string.Format("ExcelApp.Version is {0}", excelApp.Version));

                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(templateFilePaht);
                sheets = workbook.Worksheets;
                worksheet = sheets.get_Item("Sheet1");

                // 제목 -> B4 -> 4,2
                worksheet.Cells[4, 2].Value = resultKey.DateTime.ToString("yyyy.MM.dd HH:mm:ss");
                worksheet.Cells[4, 5].Value = resultKey.Production.Name;
                worksheet.Cells[4, 8].Value = resultKey.Production.LotNo;

                // 티칭값 : B7 -> 7,2
                InspectOperatorSettings inspectOperatorSettings = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
                worksheet.Cells[7, 2].Value = inspectOperatorSettings.PatternLower;
                worksheet.Cells[7, 3].Value = inspectOperatorSettings.PatternUpper;
                worksheet.Cells[7, 4].Value = inspectOperatorSettings.PatternMinDefectSize;
                worksheet.Cells[8, 2].Value = inspectOperatorSettings.MarginLower;
                worksheet.Cells[8, 3].Value = inspectOperatorSettings.MarginUpper;
                worksheet.Cells[8, 4].Value = inspectOperatorSettings.MarginMinDefectSize;
                worksheet.Cells[9, 4].Value = inspectOperatorSettings.DiffThreshold;

                //누적개수 : G7 -> 7,7
                worksheet.Cells[7, 7].Value = resultKey.Production.PatternCount;
                worksheet.Cells[7, 8].Value = resultKey.Production.MarginCount;
                worksheet.Cells[7, 9].Value = resultKey.Production.ShapeCount;
                worksheet.Cells[7, 10].Value = resultKey.Production.PatternCount + resultKey.Production.MarginCount + resultKey.Production.ShapeCount;

                // 불량개수 : G8 -> 8,7
                int[] defCount = new int[]
                {
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Pattern) || f.Defect.ResultObjectType.Equals(DefectType.CircularPattern)),
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Margin) || f.Defect.ResultObjectType.Equals(DefectType.CircularMargin)),
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Shape))
                };

                worksheet.Cells[8, 7].Value = defCount[0];
                worksheet.Cells[8, 8].Value = defCount[1];
                worksheet.Cells[8, 9].Value = defCount[2];
                worksheet.Cells[8, 10].Value = defCount.Sum();

                // 시트 길이 : B12 -> 12,2
                List<LengthMeasure> validLengthMeasureList = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Length)).ConvertAll(f => f.Defect as LengthMeasure).FindAll(f => f.IsValid);
                List<MeanderMeasure> meanderMeasure = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Meander)).ConvertAll(f => f.Defect as MeanderMeasure);
                List<LengthMeasure> verticalLength = validLengthMeasureList.FindAll(f => f.Direction == DynMvp.Vision.Direction.Vertical);
                List<LengthMeasure> horizontalLength = validLengthMeasureList.FindAll(f => f.Direction == DynMvp.Vision.Direction.Horizontal);
                for (int i = 0; i < 3; i++)
                {
                    if (horizontalLength.Count > i)
                        worksheet.Cells[12, 2 + i].Value = horizontalLength[i].LengthMm;

                    if (verticalLength.Count > i)
                        worksheet.Cells[13, 2 + i].Value = verticalLength[i].LengthMm;

                    if (meanderMeasure.Count > i)
                    {
                        worksheet.Cells[14, 2 + i].Value = meanderMeasure[i].SheetPrintDistMm;
                        worksheet.Cells[15, 2 + i].Value = meanderMeasure[i].CoatPrintDistMm;
                    }
                }

                // 마진길이 : G19 -> 19,7
                //extractOperatorResultList.FindAll(f=>f is )
                List<ExtraMeasure> extraMeasureList = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Extra)).ConvertAll(f => f.Defect as ExtraMeasure);
                extraMeasureList.ForEach(f => 
                {
                    int idx = extraMeasureList.IndexOf(f);
                    string name = f.MarginMeasurePos.Name;
                    string w = f.Width;
                    string l = f.Height;

                    worksheet.Cells[19 + idx, 7].Value = idx;
                    worksheet.Cells[19 + idx, 8].Value = name;
                    worksheet.Cells[19 + idx, 9].Value = w;
                    worksheet.Cells[19 + idx, 10].Value = l;
                });

                // 불량 목록
                int row = 19;
                for (int i = 0; i < canvasDefectList.Count; i++)
                {
                    Defect defect = canvasDefectList[i].Defect as Defect;
                    if (defect != null)
                    {
                        worksheet.Cells[row, 1].Value = i + 1;
                        worksheet.Cells[row, 2].Value = defect.Length;
                        worksheet.Cells[row, 3].Value = defect.DefectType == DefectType.Shape ? 0 : defect.DiffValue;
                        worksheet.Cells[row, 4].Value = defect.DefectType.ToString();
                        row++;
                    }

                    this.CurProgressSteps = 200 + (int)Math.Round((i + 1) * 1.0 / canvasDefectList.Count * 100);  // 200 ~ 300
                }

                // 저장
                worksheet.Name = repeatCount.ToString();

                if (File.Exists(targetPath))
                {
                    Workbook workbook2 = excelApp.Workbooks.Open(targetPath);
                    Sheets sheets2 = workbook2.Sheets;
                    Worksheet worksheet2 = (Worksheet)sheets2[1];

                    worksheet.Copy(worksheet2);
                    workbook2.Save();
                    workbook2.Close();

                    ReleaseObject(worksheet2);
                    ReleaseObject(sheets2);
                    ReleaseObject(workbook2);

                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet2);
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets2);
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook2);
                }
                else
                {
                    workbook.SaveAs(targetPath);
                }

                workbook.Close(false);
            }
            finally
            {
                excelApp?.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(sheets);
                ReleaseObject(workbook);
                ReleaseObject(workbooks);
                ReleaseObject(excelApp);

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        /// <summary>
        /// 엑셀 object에 빈값(null) 채움.
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(object obj)
        {
            if (obj != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        }

        private void SaveResult(string path, List<CanvasDefect> canvasDefectList, List<ExtractOperatorResult> extractOperatorResultList)
        {
            SystemManager.Instance().OperatorManager.ScanOperator.Settings.Save(Path.Combine(path, "ScanSetting.xml"));
            SystemManager.Instance().OperatorManager.ExtractOperator.Settings.Save(Path.Combine(path, "ExtractSetting.xml"));
            SystemManager.Instance().OperatorManager.InspectOperator.Settings.Save(Path.Combine(path, "InspectSetting.xml"));

            string defectPath = Path.Combine(path, "Defect");
            if (Directory.Exists(defectPath) == false)
                Directory.CreateDirectory(defectPath);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement resultElement = xmlDocument.CreateElement("", "Result", "");
            xmlDocument.AppendChild(resultElement);

            for (int i = 0; i < canvasDefectList.Count; i++)
            {
                XmlElement defectElement = xmlDocument.CreateElement("", string.Format("Defect{0}", i), "");
                resultElement.AppendChild(defectElement);

                CanvasDefect canvasDefect = canvasDefectList[i];
                canvasDefect.Save(defectElement);

                try
                {
                    if (canvasDefectList[i].Defect is Defect)
                    {
                        WPFImageHelper.SaveBitmapSource(Path.Combine(defectPath, string.Format("{0}.png", i + 1)), (canvasDefectList[i].Defect as Defect)?.Image);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("StoringOperator::SaveResult - {0},{1}", i, ex.Message));
                }
                this.CurProgressSteps = 0 + (int)Math.Round((i + 1) * 1.0 / canvasDefectList.Count * 100); // 0->100
            }

            XmlDocument scanDocument = new XmlDocument();
            XmlElement scanResultElement = scanDocument.CreateElement("", "Result", "");
            scanDocument.AppendChild(scanResultElement);
            for (int i = 0; i < extractOperatorResultList.Count; i++)
            {
                var extractResult = extractOperatorResultList[i];
                var scanResult = extractResult?.ScanOperatorResult;
                if (extractResult == null || scanResult == null)
                    continue;

                string bitmapPath = Path.Combine(path, string.Format("{0}.png", scanResult.FlowPosition));
                WPFImageHelper.SaveBitmapSource(bitmapPath, scanResult.TopLightBitmap);

                //WPFImageHelper.SaveBitmapSource(
                // Path.Combine(resultPath, string.Format("{0}Mask.png",
                // scanResult.FlowPosition)),
                // extractResult.MaskBufferBitmap);

                //WPFImageHelper.SaveBitmapSource(
                //Path.Combine(resultPath, string.Format("{0}Sheet.png",
                //scanResult.FlowPosition)),
                //extractResult.SheetBufferBitmap);

                XmlHelper.SetValue(scanResultElement, string.Format("X{0}", scanResult.FlowPosition), scanResult.CanvasAxisPosition.Position[0]);
                XmlHelper.SetValue(scanResultElement, string.Format("Y{0}", scanResult.FlowPosition), scanResult.CanvasAxisPosition.Position[1]);
                XmlHelper.SetValue(scanResultElement, string.Format("W{0}", scanResult.FlowPosition), scanResult.PixelPositionX);

                XmlHelper.SetValue(scanResultElement, string.Format("C{0}", scanResult.FlowPosition), extractResult.PatternCount);
                XmlHelper.SetValue(scanResultElement, string.Format("H{0}", scanResult.FlowPosition), extractResult.SheetRect);
                this.CurProgressSteps = 100 + (int)Math.Round((i + 1) * 1.0 / extractOperatorResultList.Count * 100); // 100->200
            }

            string scanXmlPath = Path.Combine(path, "Scan.xml");
            XmlHelper.Save(scanDocument, scanXmlPath);

            string xmlPath = Path.Combine(defectPath, "Defect.xml");
            XmlHelper.Save(xmlDocument, xmlPath);
        }


        private void SaveMargin(string xlsxFile, ResultKey resultKey, List<CanvasDefect> canvasDefectList)
        {
            string templateXlsx = Path.Combine(PathSettings.Instance().Result, "RawDataTemplate_OfflineMargin.xlsx");
            bool saveAs = (File.Exists(xlsxFile) == false);
            
            if (File.Exists(templateXlsx) == false)
                throw new Exception("No xlsx File Exist");

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Sheets sheets = null;
            Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                System.Diagnostics.Debug.WriteLine(string.Format("ExcelApp.Version is {0}", excelApp.Version));

                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(saveAs ? templateXlsx : xlsxFile);
                sheets = workbook.Worksheets;
                worksheet = sheets.get_Item("Sheet1");

                if(saveAs)
                {
                    string title = $"{resultKey.DateTime.ToString("yyyy. MM.")} 마진 측정 데이터";
                    worksheet.Cells[1, 2].Value = title;
                }

                int iRow = 22;
                while (worksheet.Cells[iRow, 2].Value != null)
                    iRow++;

                string[] values = new string[13];
                values[0] = resultKey.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                values[1] = resultKey.LotNo;
                values[2] = resultKey.Model.Name;

                string[] extraValues = SystemManager.Instance().OperatorManager.ResultCombiner.ExtraValues;
                values[3] = extraValues[1];

                string[] designed = extraValues[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (designed.Length == 1)
                {
                    values[4] = values[8] = designed[0];
                }
                else
                {
                    values[4] = designed[0].Substring(1).Trim();
                    values[8] = designed[1].Substring(1).Trim();
                }

                string[] mins = extraValues[2].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                values[5] = mins[0].Substring(1).Trim();
                if (mins.Length > 1)
                    values[9] = mins[1].Substring(1).Trim();

                string[] maxs = extraValues[3].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                values[6] = maxs[0].Substring(1).Trim();
                if (maxs.Length > 1)
                    values[10] = maxs[1].Substring(1).Trim();

                string[] means = extraValues[4].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                values[7] = means[0].Substring(1).Trim();
                if (means.Length>1)
                    values[11] = means[1].Substring(1).Trim();

                values[12] = extraValues[5];

                //Array.Copy(extraValues, 0, values, 3, 6);

                for (int i = 0; i < values.Length; i++)
                    worksheet.Cells[iRow, i + 2].Value = values[i];

                Names names = worksheet.Parent.Names;
                int iCount = names.Count;
                for (int i = 0; i < iCount; i++)
                {
                    Name name = names.Item(i + 1);
                    string refer = name.RefersToLocal;
                    string[] tokens = refer.Split('$');
                    tokens[tokens.Length - 1] = iRow.ToString();
                    name.RefersToLocal = string.Join("$", tokens);
                }

                if (saveAs)
                {
                    workbook.SaveAs(xlsxFile);
                }
                else
                {
                    workbook.Save();
                }

                string patentPath = Path.GetDirectoryName(xlsxFile);
                string backupFile = Path.Combine(patentPath, $@"{resultKey.DateTime.Month:00}_{resultKey.DateTime.ToString("yyyyMMdd_HHmmss")}.xlsx");
                workbook.SaveAs(backupFile);
            }
            finally
            {
                excelApp?.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(sheets);
                ReleaseObject(workbook);
                ReleaseObject(workbooks);
                ReleaseObject(excelApp);
            }


            //using (StreamWriter sw = new StreamWriter(csvFile, true))
            //{
            //    if (sw.BaseStream.Length == 0)
            //        sw.WriteLine("DateTime, RollNo, Name, Designed, Spec, Minimum, Maximum, Average, Judgement");

            //    string[] values = new string[9];
            //    values[0] = resultKey.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //    values[1] = resultKey.LotNo;
            //    values[2] = resultKey.Model.Name;

            //    string[] extraValues = SystemManager.Instance().OperatorManager.ResultCombiner.ExtraValues;
            //    Array.Copy(extraValues, 0, values, 3, 6);
            //    sw.WriteLine(string.Join(", ", values));
            //}
        }

        public ImageSource DrawingDefect(ImageSource bitmapSource, InspectOperatorResult inspectOperatorResult)
        {
            var visual = new DrawingVisual();

            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, bitmapSource.Width, bitmapSource.Height));

                for (int i = 0; i < inspectOperatorResult.DefectList.Count; i++)
                {
                    //BlobRect blobRect = inspectOperatorResult.DefectList[i].DefectBlob;
                    IResultObject resObj = inspectOperatorResult.DefectList[i];
                    SolidColorBrush brush = resObj.GetBrush();
                    //switch (inspectOperatorResult.DefectList[i].DefectType)
                    //{
                    //    case DefectType.Pattern:
                    //        brush = Brushes.Red;
                    //        break;
                    //    case DefectType.Margin:
                    //        brush = Brushes.Blue;
                    //        break;
                    //    case DefectType.Shape:
                    //        brush = Brushes.Gold;
                    //        break;
                    //}

                    //Point position = new Point(inspectOperatorResult.DefectList[i].DefectBlob.BoundingRect.X / 10.0, inspectOperatorResult.DefectList[i].DefectBlob.BoundingRect.Y / 10.0 - 48);
                    System.Windows.Point position = resObj.GetPoints(10)[0];
                    drawingContext.DrawText(
                       new FormattedText(string.Format("{0}", i), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("malgun gothic"), 32, brush), position);

                    StreamGeometry streamGeometry = new StreamGeometry();
                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        //geometryContext.BeginFigure(new Point(blobRect.RotateXArray[0] / 10.0, blobRect.RotateYArray[0] / 10.0), true, true);

                        //PointCollection points = new PointCollection { 
                        //   new Point(blobRect.RotateXArray[1] / 10.0, blobRect.RotateYArray[1] / 10.0),
                        //new Point(blobRect.RotateXArray[2] / 10.0, blobRect.RotateYArray[2] / 10.0),
                        //new Point(blobRect.RotateXArray[3] / 10.0, blobRect.RotateYArray[3] / 10.0)};

                        //geometryContext.PolyLineTo(points, true, true);

                        System.Windows.Point[] points = resObj.GetPoints(10);
                        geometryContext.BeginFigure(points[0], true, true);
                        geometryContext.PolyLineTo(points, true, true);
                    }

                    brush = new SolidColorBrush(brush.Color);
                    brush.Opacity = 0.25;
                    drawingContext.DrawGeometry(brush, new Pen(brush, 1), streamGeometry);
                }
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bitmapSource.Width, (int)bitmapSource.Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);
            return renderTargetBitmap;
        }


    }

    public class StoringOperatorSettings : OperatorSettings
    {
        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Storing");
        }

        public override void Load(XmlElement xmlElement)
        {

        }

        public override void Save(XmlElement xmlElement)
        {

        }
    }

    public class LoadItem
    {
        public string RootPath { get => this.rootPath; }
        string rootPath;

        public List<ExtractOperatorResult> ExtractOperatorResultList { get => this.extractOperatorResultList; }
        List<ExtractOperatorResult> extractOperatorResultList;

        public List<CanvasDefect> CanvasDefectList { get => this.canvasDefectList; }
        List<CanvasDefect> canvasDefectList;

        public List<OperatorSettings> OperatorSettingList { get => this.operatorSettingList; }
        List<OperatorSettings> operatorSettingList;

        public bool IsLoaded { get => extractOperatorResultList != null && canvasDefectList != null && operatorSettingList != null; }

        public LoadItem(string rootPath)
        {
            this.rootPath = rootPath;
        }

        BackgroundWorker worker;
        CancellationToken cancellationToken;

        public bool Load(BackgroundWorker worker)
        {
            this.cancellationToken = new CancellationToken(false);
            this.worker = worker;
            return Load();
        }

        public bool Load(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            this.worker = null;
            return Load();
        }

        private bool Load()
        {
            if (string.IsNullOrEmpty(this.rootPath))
                return false;

            try
            {
                FileInfo copiedFileInfo = new FileInfo(Path.Combine(this.rootPath,"..", "Copied"));
                if (copiedFileInfo.Exists)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(this.rootPath);
                    this.rootPath = Path.Combine(File.ReadAllText(copiedFileInfo.FullName), directoryInfo.Name);
                }

                ReportProgress(0);

                // 티칭 정보 로드 0~5
                this.operatorSettingList = LoadTeachData(this.rootPath);
                ReportProgress(5);

                // 불량 정보 로드 5 ~ 95
                string defectPath = Path.Combine(this.rootPath, "Defect");
                string xmlPath = Path.Combine(defectPath, "Defect.xml");
                XmlDocument xmlDocument = XmlHelper.Load(xmlPath);
                XmlElement resultElement = xmlDocument?["Result"];
                if (resultElement == null)
                    throw new Exception("Result Element is not exist");

                this.canvasDefectList = new List<CanvasDefect>();
                for (int i = 0; i < resultElement.ChildNodes.Count; i++)
                {
                    if (i % 100 == 0)
                        ReportProgress((int)(5 + (i + 1) * (90.0 / resultElement.ChildNodes.Count)));

                    XmlElement xmlElement = (XmlElement)resultElement.ChildNodes[i];
                    if (xmlElement.Name.Contains("Defect") == false)
                        continue;

                    string imagePath = Path.Combine(defectPath, string.Format("{0}.png", i + 1));
                    CanvasDefect canvasDefect = new CanvasDefect(imagePath, xmlElement);
                    this.canvasDefectList.Add(canvasDefect);
                }
                ReportProgress(95);


                // 스캔 정보 로드 95 ~ 100
                string scanXmlPath = Path.Combine(this.rootPath, "Scan.xml");
                bool mul = (new FileInfo(scanXmlPath).CreationTime < new DateTime(2019, 3, 12, 15, 0, 0));
                XmlDocument scanDocument = XmlHelper.Load(scanXmlPath);
                XmlElement scanResultElement = scanDocument?["Result"];

                if (scanResultElement == null)
                    throw new Exception("Scan Element is not exist");

                this.extractOperatorResultList = new List<ExtractOperatorResult>();
                for (int j = 0; j < DeveloperSettings.Instance.ScanNum; j++)
                {
                    ReportProgress((int)(95 + (j + 1) * (5.0 / DeveloperSettings.Instance.ScanNum)));

                    BitmapSource source = WPFImageHelper.LoadBitmapSource(Path.Combine(this.rootPath, string.Format("{0}.png", j)));

                    AxisPosition axisPosition = new AxisPosition(2);
                    axisPosition.Position[0] = XmlHelper.GetValue(scanResultElement, string.Format("X{0}", j), 0.0f);
                    axisPosition.Position[1] = XmlHelper.GetValue(scanResultElement, string.Format("Y{0}", j), 0.0f);

                    if (mul)
                    {
                        axisPosition.Position[0] *= 0.2f;
                        axisPosition.Position[1] *= 0.2f;
                    }

                    int pixelPositionX = XmlHelper.GetValue(scanResultElement, string.Format("W{0}", j), 0);

                    System.Drawing.Rectangle sheetRect = System.Drawing.Rectangle.Empty;
                    sheetRect = XmlHelper.GetValue(scanResultElement, string.Format("H{0}", j), sheetRect);

                    ScanOperatorResult scanResult = new ScanOperatorResult(null, null, source, j, null, null, null, axisPosition, pixelPositionX);
                    ExtractOperatorResult extractResult = new ExtractOperatorResult(null, scanResult, null, null, null, null, sheetRect, null, null, 1);
                    this.extractOperatorResultList.Add(extractResult);
                }


                return true;
            }
            catch (Exception ex)
            {
                this.extractOperatorResultList = null;
                this.canvasDefectList = null;
                this.operatorSettingList = null;
                return false;
            }
        }

        private void ReportProgress(int progress)
        {
            if(this.worker!=null)
            {
                if (this.worker.CancellationPending)
                    throw new OperationCanceledException();

                this.worker.ReportProgress(progress);
            }
            else
            {
                this.cancellationToken.ThrowIfCancellationRequested();
            }
        }

        private static List<OperatorSettings> LoadTeachData(string path)
        {
            ScanOperatorSettings scanOperatorSettings = new ScanOperatorSettings();
            scanOperatorSettings.Load(Path.Combine(path, "ScanSetting.xml"));

            ExtractOperatorSettings extractOperatorSettings = new ExtractOperatorSettings();
            extractOperatorSettings.Load(Path.Combine(path, "ExtractSetting.xml"));

            InspectOperatorSettings inspectOperatorSettings = new InspectOperatorSettings();
            inspectOperatorSettings.Load(Path.Combine(path, "InspectSetting.xml"));

            return new UniScanWPF.Table.Operation.OperatorSettings[] { scanOperatorSettings, extractOperatorSettings, inspectOperatorSettings }.ToList();
        }
    }
}
