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
using UniScanM.Data;
using Infragistics.Documents.Excel;

namespace UniScanM.StillImage.Data
{
    internal enum ExcelHeader { Index, Date, Time, RollPos, InspZone, Width, Height, MarginW, MarginH, BlotW, BlotH, DefectW, DefectH, Result, FileName }
    internal enum ResultHeader { Index, Date, Time, RollPos, InspPos, InspZone, pelSizeX, pelSizeY,
        SheetX, SheetY, SheetW, SheetH,
        InspX, InspY, InspW, InspH,
        RoiX, RoiY, RoiW, RoiH,
        BlotRectX, BlotRectY, BlotRectW, BlotRectH,
        MarginRectX, MarginRectY, MarginRectW, MarginRectH,
        InspArea, InspMarginW, InspMarginL, InspBlotW, InspBlotL,
        OffsArea, OffsMarginW, OffsMarginL, OffsBlotW, OffsBlotL,
        DefectW, DefectH, DefectC, Result, PrintingPeriod, PrintingLength,
        MAX_COUNT }

    public class DataExporter : UniScanM.Data.DataExporter
    {
        public DataExporter() : base()
        {
            this.row_begin = 8;
        }

        protected override string GetTemplateName()
        {
            return "RawDataTemplate_StillImage.xlsx";
        }

        protected override void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //WriteCsvHeader(stringBuilder, typeof(ResultHeader));
        }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult stopImageInspectionResult = (InspectionResult)inspectionResult;

            int defectCount = 0;
            Size defectSize = new Size(0, 0);
            Rectangle blotRect = new Rectangle(0, 0, 0, 0);
            Rectangle marginRect = new Rectangle(0, 0, 0, 0);
            Feature inspectFeature = new Feature { Area = 0, Margin = new SizeF(0, 0), Blot = new SizeF(0, 0) };
            Feature offsetFeature = new Feature { Area = 0, Margin = new SizeF(0, 0), Blot = new SizeF(0, 0) };

            ProcessResult processResult = stopImageInspectionResult.ProcessResultList?.InterestProcessResult;
            if (processResult != null)
            {
                inspectFeature = processResult.InspPatternInfo.TeachInfo.Feature;
                offsetFeature = processResult.OffsetValue;

                blotRect = processResult.InspPatternInfo.ShapeInfo.BaseRect;
                marginRect = Rectangle.Inflate(blotRect, (int)inspectFeature.Margin.Width, (int)inspectFeature.Margin.Height);
            }

            List<Rectangle> defectRectList = stopImageInspectionResult.ProcessResultList?.DefectRectList;
            if (defectRectList != null && defectRectList.Count > 0)
            {
                defectCount = defectRectList.Count;
                defectSize = new Size(defectRectList.Max(f => f.Width), defectRectList.Max(f => f.Height));
            }
            string[] results = new string[(int)ResultHeader.MAX_COUNT];

            results[(int)ResultHeader.Index] = inspectionResult.InspectionNo;
            results[(int)ResultHeader.Date] = inspectionResult.InspectionStartTime.ToString("yyyyMMdd");
            results[(int)ResultHeader.Time] = inspectionResult.InspectionStartTime.ToString("HHmmss");
            results[(int)ResultHeader.RollPos] = stopImageInspectionResult.RollDistance.ToString();
            results[(int)ResultHeader.InspPos] = stopImageInspectionResult.InspZone.ToString();
            results[(int)ResultHeader.InspZone] = stopImageInspectionResult.InspZone.ToString();// inspectionResult.ExtraResult["InspectSequence"].ToString();
            results[(int)ResultHeader.pelSizeX] = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width.ToString();
            results[(int)ResultHeader.pelSizeY] = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height.ToString();
            results[(int)ResultHeader.SheetX] = stopImageInspectionResult.SheetRectInFrame.X.ToString();
            results[(int)ResultHeader.SheetY] = stopImageInspectionResult.SheetRectInFrame.Y.ToString();
            results[(int)ResultHeader.SheetW] = stopImageInspectionResult.SheetRectInFrame.Width.ToString();
            results[(int)ResultHeader.SheetH] = stopImageInspectionResult.SheetRectInFrame.Height.ToString();
            results[(int)ResultHeader.InspX] = stopImageInspectionResult.InspRectInSheet.X.ToString();
            results[(int)ResultHeader.InspY] = stopImageInspectionResult.InspRectInSheet.Y.ToString();
            results[(int)ResultHeader.InspW] = stopImageInspectionResult.InspRectInSheet.Width.ToString();
            results[(int)ResultHeader.InspH] = stopImageInspectionResult.InspRectInSheet.Height.ToString();
            results[(int)ResultHeader.RoiX] = "0";// stopImageInspectionResult.RoiRectInFov.X.ToString();
            results[(int)ResultHeader.RoiY] = "0";// stopImageInspectionResult.RoiRectInFov.Y.ToString();
            results[(int)ResultHeader.RoiW] = "0";//stopImageInspectionResult.RoiRectInFov.Width.ToString();
            results[(int)ResultHeader.RoiH] = "0";//stopImageInspectionResult.RoiRectInFov.Height.ToString();
            results[(int)ResultHeader.BlotRectX] = blotRect.X.ToString();
            results[(int)ResultHeader.BlotRectY] = blotRect.Y.ToString();
            results[(int)ResultHeader.BlotRectW] = blotRect.Width.ToString();
            results[(int)ResultHeader.BlotRectH] = blotRect.Height.ToString();
            results[(int)ResultHeader.MarginRectX] = marginRect.X.ToString();
            results[(int)ResultHeader.MarginRectY] = marginRect.Y.ToString();
            results[(int)ResultHeader.MarginRectW] = marginRect.Width.ToString();
            results[(int)ResultHeader.MarginRectH] = marginRect.Height.ToString();
            results[(int)ResultHeader.InspArea] = inspectFeature.Area.ToString();
            results[(int)ResultHeader.InspMarginW] = inspectFeature.Margin.Width.ToString();
            results[(int)ResultHeader.InspMarginL] = inspectFeature.Margin.Height.ToString();
            results[(int)ResultHeader.InspBlotW] = inspectFeature.Blot.Width.ToString();
            results[(int)ResultHeader.InspBlotL] = inspectFeature.Blot.Height.ToString();
            results[(int)ResultHeader.OffsArea] = offsetFeature.Area.ToString();
            results[(int)ResultHeader.OffsMarginW] = offsetFeature.Margin.Width.ToString();
            results[(int)ResultHeader.OffsMarginL] = offsetFeature.Margin.Height.ToString();
            results[(int)ResultHeader.OffsBlotW] = offsetFeature.Blot.Width.ToString();
            results[(int)ResultHeader.OffsBlotL] = offsetFeature.Blot.Height.ToString();
            results[(int)ResultHeader.DefectW] = defectSize.Width.ToString();
            results[(int)ResultHeader.DefectH] = defectSize.Height.ToString();
            results[(int)ResultHeader.DefectC] = defectCount.ToString();
            results[(int)ResultHeader.Result] = stopImageInspectionResult.Judgment.ToString();
            //Printing Length....
            results[(int)ResultHeader.PrintingPeriod] = stopImageInspectionResult.PrintingPeriod.ToString();
            results[(int)ResultHeader.PrintingLength] = stopImageInspectionResult.PrintingLength.ToString();
 
            string aLine = string.Join(",", results);
            stringBuilder.AppendLine(aLine);
        }

        protected override void SaveImage(string resultPath, bool skipImageSave, UniScanM.Data.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (inspectionResult.IsGood())
                return;

            Data.InspectionResult stillImageInspectionResult = (Data.InspectionResult)inspectionResult;

            // Full Image
            if (skipImageSave)
            {
                inspectionResult.DisplayBitmapSaved = IMAGE_SAVE_SKIPPED;
                return;
            }

            string fileName = stillImageInspectionResult.GetFullImageFileName();
            string imgFile = Path.Combine(resultPath, fileName);
            ImageD saveImage = GetDrawImage(stillImageInspectionResult);
            if (saveImage != null)
            {
                saveImage.SaveImage(imgFile, ImageFormat.Jpeg);
                saveImage.Dispose();
                inspectionResult.DisplayBitmapSaved = fileName;
            }

            // Defect Image
            ProcessResultList processResultList = stillImageInspectionResult.ProcessResultList;
            if(processResultList !=null)
            for (int i = 0; i < processResultList.DefectRectList.Count; i++)
            {
                Rectangle defectRect = processResultList.DefectRectList[i];
                defectRect.Inflate(20, 20);
                defectRect = Rectangle.Intersect(defectRect, new Rectangle(Point.Empty, processResultList.Image.Size));

                imgFile = Path.Combine(resultPath, stillImageInspectionResult.GetDefectImageFileName(i));
                saveImage = processResultList.Image.ClipImage(defectRect);
                if (saveImage != null)
                {
                    saveImage.SaveImage(imgFile, ImageFormat.Jpeg);
                    saveImage.Dispose();
                }
            }
            //string imgFile = Path.Combine(resultPath, string.Format("{0}.jpg", inspectionResult.InspectionNo));
            //ImageD saveImage = GetDrawImage(inspectionResult as InspectionResult);
            //saveImage?.SaveImage(imgFile, ImageFormat.Jpeg);
            //saveImage?.Dispose();
        }

        protected ImageD GetDrawImage(InspectionResult inspectionResult)
        {
            Bitmap displayBitmap = inspectionResult.DisplayBitmap;
            Rectangle displayImageRect = inspectionResult.DisplayImageRect;
            if (displayBitmap == null || displayImageRect.Width == 0 || displayImageRect.Height == 0)
                return null;

            ImageD imageD = Image2D.FromBitmap(displayBitmap.Clone(displayImageRect, displayBitmap.PixelFormat));
            //imageD.SaveImage(@"d:\Temp\imageD.bmp");
            AlgoImage grayAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
            //grayAlgoImage.Save(@"d:\Temp\grayAlgoImage.bmp");
            //AlgoImage colorAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Color, grayAlgoImage.Size);
            //DynMvp.Vision.ImageConverter.Convert(grayAlgoImage, colorAlgoImage);
            AlgoImage colorAlgoImage = grayAlgoImage.ConvertTo(ImageType.Color);
            //colorAlgoImage.Save(@"d:\Temp\colorAlgoImage.bmp");
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(colorAlgoImage);

            ProcessResult interestProcessResult = inspectionResult.ProcessResultList?.InterestProcessResult;
            if (interestProcessResult != null)
            {
                // Draw Blot
                Color colorBlot = interestProcessResult.IsBlotGood ? Color.LightGreen : Color.Red;
                Rectangle blotRect = inspectionResult.BlotRectInInsp;
                blotRect.Offset(-displayImageRect.X, -displayImageRect.Y);
                ip.DrawRect(colorAlgoImage, blotRect, colorBlot.ToArgb(), false);

                // Draw Margine
                Color colorMargin = interestProcessResult.IsMarginGood ? Color.Green : Color.Red;
                Rectangle marginRect = inspectionResult.MarginRectInInsp;
                marginRect.Offset(-displayImageRect.X, -displayImageRect.Y);
                ip.DrawRect(colorAlgoImage, marginRect, colorMargin.ToArgb(), false);
            }

            // Draw Defects
            for (int i = 0; i < inspectionResult.ProcessResultList?.DefectRectList.Count; i++)
            {
                Rectangle defectRect = inspectionResult.ProcessResultList.DefectRectList[i];
                defectRect.Inflate(10, 10);
                defectRect.Offset(-displayImageRect.X, -displayImageRect.Y);
                defectRect = Rectangle.Intersect(defectRect, new Rectangle(Point.Empty, colorAlgoImage.Size));
                ip.DrawRect(colorAlgoImage, defectRect, Color.Red.ToArgb(), false);
            }

            ImageD drawImage = colorAlgoImage.ToImageD();
            colorAlgoImage.Dispose();
            grayAlgoImage.Dispose();
            imageD.Dispose();
            drawImage.SaveImage(@"d:\Temp\drawImage.bmp");
            return drawImage;
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[9].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
            workbook.Worksheets[0].Rows[4].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.LastUpdateTime.ToString("HH:mm:ss:fff"); // End Time
            workbook.Worksheets[0].Rows[4].Cells[9].Value = SystemManager.Instance().ProductionManager.CurProduction.NgRatio.ToString("F2");

            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

            Data.InspectionResult stillImageInspectionResult = (Data.InspectionResult)inspectionResult;

            Worksheet logSheet = workbook.Worksheets[sheetNo];
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Index].Value = stillImageInspectionResult.InspectionNo;
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Date].Value = stillImageInspectionResult.InspectionStartTime.ToString("yyyy.MM.dd"); // Date 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Time].Value = stillImageInspectionResult.InspectionStartTime.ToString("HH:mm:ss"); // Date 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.InspZone].Value = stillImageInspectionResult.InspZone + 1; // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.RollPos].Value = stillImageInspectionResult.RollDistance; // Insp Zone 

            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Width].Value = stillImageInspectionResult.SheetRectInFrame.Width * pelSize.Width / 1000; // Insp Zone 
            logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Height].Value = stillImageInspectionResult.SheetRectInFrame.Height * pelSize.Height / 1000; // Insp Zone 

            ProcessResultList processResultList = stillImageInspectionResult.ProcessResultList;
            if (processResultList != null)
            {
                ProcessResult processResult = processResultList.InterestProcessResult;
                if (processResult != null)
                {
                    Feature inspectFeature = processResult.InspPatternInfo.TeachInfo.Feature.Mul(pelSize);
                    Feature offsetFeature = processResult.OffsetValue.Mul(pelSize);

                    logSheet.Rows[rowNo].Cells[(int)ExcelHeader.MarginW].Value = inspectFeature.Margin.Width; // Insp Zone 
                    logSheet.Rows[rowNo].Cells[(int)ExcelHeader.MarginH].Value = inspectFeature.Margin.Height; // Insp Zone 
                    logSheet.Rows[rowNo].Cells[(int)ExcelHeader.BlotW].Value = offsetFeature.Blot.Width; // Insp Zone 
                    logSheet.Rows[rowNo].Cells[(int)ExcelHeader.BlotH].Value = offsetFeature.Blot.Height; // Insp Zone 

                    int defectCount = processResultList.DefectRectList.Count;
                }
                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.Result].Value = JudgementString.GetString(stillImageInspectionResult.Judgment);
                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.FileName].Value = stillImageInspectionResult.GetFullImageFileName();

                Rectangle maxDefectRect = processResultList.GetMaxSizeDefectRect();

                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.DefectW].Value = maxDefectRect.Width; // Insp Zone 
                logSheet.Rows[rowNo].Cells[(int)ExcelHeader.DefectH].Value = maxDefectRect.Height; // Insp Zone 
            }

            return 1;
        }
    }

    public class DataImporter : UniScanM.Data.DataImporter
    {
        public DataImporter() : base() { }

        protected override bool Import()
        {
            string resultFile = Path.Combine(this.ResultPath, this.resultFileName);
            if (File.Exists(resultFile) == false)
                return false;            
            try
            {
                float rollPossss = 0;
                StreamReader sr = new StreamReader(resultFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] tokens = line.Split(',');
                    if (tokens.Length <  (int)ResultHeader.Result+1)
                        continue;
                    
                    int index;
                    if (int.TryParse(tokens[(int)ResultHeader.Index], out index) == false)
                        continue;

                    //TimeSpan dateTime = new TimeSpan(int.Parse(token[1].Substring(0, 2)), int.Parse(token[1].Substring(2, 2)), int.Parse(token[1].Substring(4, 2)));
                    string dt = string.Format("{0}_{1}", tokens[(int)ResultHeader.Date], tokens[(int)ResultHeader.Time]);
                    DateTime dateTime = DateTime.ParseExact(dt, "yyyyMMdd_HHmmss", null);
                    int inspPos = int.Parse(string.IsNullOrEmpty(tokens[(int)ResultHeader.InspPos])?"0": tokens[(int)ResultHeader.InspPos]);
                    int inspZone = int.Parse(tokens[(int)ResultHeader.InspZone]);
                    int rollPos = int.Parse(tokens[(int)ResultHeader.RollPos]);
                    float umPerPxW = float.Parse(tokens[(int)ResultHeader.pelSizeX]);
                    float umPerPxH = float.Parse(tokens[(int)ResultHeader.pelSizeY]);
                    int sheetX = int.Parse(tokens[(int)ResultHeader.SheetX]);
                    int sheetY = int.Parse(tokens[(int)ResultHeader.SheetY]);
                    int sheetW = int.Parse(tokens[(int)ResultHeader.SheetW]);
                    int sheetH = int.Parse(tokens[(int)ResultHeader.SheetH]);
                    int fovX = int.Parse(tokens[(int)ResultHeader.InspX]);
                    int fovY = int.Parse(tokens[(int)ResultHeader.InspY]);
                    int fovW = int.Parse(tokens[(int)ResultHeader.InspW]);
                    int fovH = int.Parse(tokens[(int)ResultHeader.InspH]);
                    int roiX = int.Parse("0");
                    int roiY = int.Parse("0");
                    int roiW = int.Parse("0");
                    int roiH = int.Parse("0");

                    int blotRectX = int.Parse(tokens[(int)ResultHeader.BlotRectX]);
                    int blotRectY = int.Parse(tokens[(int)ResultHeader.BlotRectY]);
                    int blotRectW = int.Parse(tokens[(int)ResultHeader.BlotRectH]);
                    int blotRectH = int.Parse(tokens[(int)ResultHeader.BlotRectH]);

                    int marginRectX = int.Parse(tokens[(int)ResultHeader.MarginRectX]);
                    int marginRectY = int.Parse(tokens[(int)ResultHeader.MarginRectY]);
                    int marginRectW = int.Parse(tokens[(int)ResultHeader.MarginRectW]);
                    int marginRectH = int.Parse(tokens[(int)ResultHeader.MarginRectH]);

                    int inspArea = int.Parse(tokens[(int)ResultHeader.InspArea]);
                    int inspMarginW = int.Parse(tokens[(int)ResultHeader.InspMarginW]);
                    int inspMarginL = int.Parse(tokens[(int)ResultHeader.InspMarginL]);
                    int inspBlotW = int.Parse(tokens[(int)ResultHeader.InspBlotW]);
                    int inspBlotL = int.Parse(tokens[(int)ResultHeader.InspBlotL]);

                    float offsetArea = float.Parse(tokens[(int)ResultHeader.OffsArea]);
                    float offsetMarginW = float.Parse(tokens[(int)ResultHeader.OffsMarginW]);
                    float offsetMarginL = float.Parse(tokens[(int)ResultHeader.OffsMarginL]);
                    float offsetBlotW = float.Parse(tokens[(int)ResultHeader.OffsBlotW]);
                    float offsetBlotL = float.Parse(tokens[(int)ResultHeader.OffsBlotL]);

                    int defectW = int.Parse(tokens[(int)ResultHeader.DefectW]);
                    int defectH = int.Parse(tokens[(int)ResultHeader.DefectH]);
                    int defectC = int.Parse(tokens[(int)ResultHeader.DefectC]);

                    Judgment judgment;
                    bool ok = Enum.TryParse(tokens[(int)ResultHeader.Result], out judgment);
                    if(ok==false)
                    {
                        if (tokens[(int)ResultHeader.Result] == "GOOD" || tokens[(int)ResultHeader.Result] == "NG")
                            judgment = tokens[(int)ResultHeader.Result] == "GOOD" ? Judgment.Accept : Judgment.Reject;
                        else
                            judgment = Judgment.Skip;
                    }
                    double PrintingPeriod = -1, PrintingLength = -1;
                    if (tokens.Length == (int)ResultHeader.MAX_COUNT)
                    {
                        PrintingPeriod = double.Parse(tokens[(int)ResultHeader.PrintingPeriod]);
                        PrintingLength = double.Parse(tokens[(int)ResultHeader.PrintingLength]);
                    }

                    Data.InspectionResult inspectionResult = new Data.InspectionResult();
                    inspectionResult.PrintingPeriod = PrintingPeriod; //////////////////////
                    inspectionResult.PrintingLength = PrintingLength;////////////////////////
                    
                    inspectionResult.ResultPath = this.ResultPath;
                    //inspectionResult .LotNo = inspectionResult.LotNo;

                    inspectionResult.InspectionNo = index.ToString();
                    inspectionResult.InspectionStartTime = dateTime;
                    //inspectionResult.AddExtraResult("InspectPosition", inspPos);

                    inspectionResult.InspZone = inspZone;
                    inspectionResult.AddExtraResult("RollDistance", rollPos);
                    //inspectionResult.AddExtraResult("RollDistance", (int)rollPossss);
                    //rollPossss += 0.5f;

                    inspectionResult.SheetRectInFrame = new Rectangle(sheetX, sheetY, sheetW, sheetH);
                    inspectionResult.InspRectInSheet = new Rectangle(fovX, fovY, fovW, fovH);
                    inspectionResult.BlotRectInInsp = new Rectangle(blotRectX, blotRectY, blotRectW, blotRectH);
                    inspectionResult.MarginRectInInsp = new Rectangle(marginRectX, marginRectY, marginRectW, marginRectH);

                    inspectionResult.ProcessResultList = new Data.ProcessResultList(null);

                    for (int i = 0; i < defectC; i++)
                        inspectionResult.ProcessResultList.DefectRectList.Add(new Rectangle(0, 0, defectW, defectH));

                    Feature feature = new Feature
                    {
                        Area = inspArea,
                        Margin = new SizeF(inspMarginW, inspMarginL),
                        Blot = new SizeF(offsetBlotW, offsetBlotL)
                    }.Mul(new SizeF(umPerPxW, umPerPxH));

                    Feature offsetFeature = new Feature
                    {
                        Area = offsetArea,
                        Margin = new SizeF(offsetMarginW, offsetMarginL),
                        Blot = new SizeF(offsetBlotW, offsetBlotL)
                    }.Mul(new SizeF(umPerPxW, umPerPxH));

                    Feature inspFeature = new Feature
                    {
                        Area = inspArea,
                        Margin = new SizeF(inspMarginW, inspMarginL),
                        Blot = new SizeF(inspBlotW, inspBlotL)
                    }.Mul(new SizeF(umPerPxW, umPerPxH));

                    inspectionResult.AddExtraResult("OffsetFeature", offsetFeature);
                    inspectionResult.AddExtraResult("InspFeature", inspFeature);
                    inspectionResult.AddExtraResult("Result", feature);

                    inspectionResult.Judgment = judgment;
                    this.inspectionResultList.Add(inspectionResult);
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
