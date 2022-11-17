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

namespace UniScanM.CGInspector.Data
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
            WriteCsvHeader(stringBuilder, typeof(ResultHeader));
        }

        protected override void AppendResult(StringBuilder stringBuilder, UniScanM.Data.InspectionResult inspectionResult)
        {
            InspectionResult stopImageInspectionResult = (InspectionResult)inspectionResult;

            
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

            
        }

        protected override void AppendSheetHeader(Workbook workbook)
        {
            workbook.Worksheets[0].Rows[3].Cells[4].Value = SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyy.MM.dd HH:mm:ss");
            workbook.Worksheets[0].Rows[3].Cells[9].Value = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
        }

        protected override int AppendSheetData(Workbook workbook, int sheetNo, int rowNo, UniScanM.Data.InspectionResult inspectionResult)
        {
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
