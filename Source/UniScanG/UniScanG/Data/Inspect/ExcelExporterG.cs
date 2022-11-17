using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;
using Infragistics.Documents.Excel;

namespace UniScanG.Data.Inspect
{
    public class ExcelExporterG:DataExporterG
    {
        public void Initialize()
        {

        }

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            Workbook workbook1 = new Workbook();
            Worksheet worksheet = workbook1.Worksheets.Add("Sheet1");

            // Make some column headers
            worksheet.Rows[1].Cells[1].Value = "Morning";
            worksheet.Rows[1].Cells[2].Value = "Afternoon";
            worksheet.Rows[1].Cells[3].Value = "Evening";

            // Create a merged region from column 1 to column 3 that will be a header to the column headers
            WorksheetMergedCellsRegion mergedRegion1 = worksheet.MergedCellsRegions.Add(0, 1, 0, 3);

            // Set the value of the merged region
            mergedRegion1.Value = "Day 1";

            // Set the cell alignment of the middle cell in the merged region.
            // Since a cell and its merged region shared a cell format, this will 
            // ultimately set the format of the merged region
            worksheet.Rows[0].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;

            workbook1.Save(@"D:\temp\workbook1.xlsx");
        }

        public void Save(string filePath)
        {

        }
    }
}
