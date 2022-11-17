using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GlossSensorTest
{

    public class ExcelTemplate
    {
        protected string FilePath { get; set; }
        protected string TemplateFilePath { get; set; }

        private Application excelApp;
        private Workbook workbook;
        private Worksheet worksheet;

        public ExcelTemplate()
        {

        }

        public ExcelTemplate(string filePath, string templateFilePath = "")
        {
            excelApp = new Application();
            FilePath = filePath;

            if (templateFilePath != "")
                workbook = excelApp.Workbooks.Open(templateFilePath);
            else
                workbook = excelApp.Workbooks.Add();

            TemplateFilePath = templateFilePath;
        }

        public virtual void Dispose()
        {
            workbook?.Close();
            excelApp?.Quit();

            ReleaseExcelObject(worksheet);
            ReleaseExcelObject(workbook);
            ReleaseExcelObject(excelApp);
        }

        // Worksheet를 선택한다.
        public virtual void SelectSheet(string sheetName, bool isCreate = false)
        {
            if (worksheet != null)
                ReleaseExcelObject(worksheet);

            bool isExist = false;
            foreach (Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Name == sheetName)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist)
            {
                worksheet = workbook.Worksheets.get_Item(sheetName);
            }
            else if (isCreate)
            {
                worksheet = workbook.Sheets.Add(After: workbook.Sheets[workbook.Sheets.Count]);
                worksheet.Name = sheetName;
            }
        }

        public void SetChartRange(string startRange)
        {
            Range chartRange;
            Range last = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            ChartObjects xlCharts = (ChartObjects)worksheet.ChartObjects(Type.Missing);
            ChartObject myChart = (ChartObject)xlCharts.Add(500, 20, 800, 500);
            Chart chartPage = myChart.Chart;

            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column;

            chartRange = worksheet.get_Range(startRange, last);
            chartPage.ChartType = XlChartType.xlLine;
            chartPage.SetSourceData(chartRange);


            worksheet.Columns.AutoFit();
        }

        public virtual void WriteData(int row, int col, object data)
        {
            if (worksheet != null)
                worksheet.Cells[row, col] = data;
        }

        /****
         * startRow : 데이터가 시작되는 열 번호
         * startCol : 데이터가 시작되는 행 번호
         * dataTuple : 영역 데이터
         * - Item1 : Row
         * - Item2 : Column
         * - Item3 : Data
         */
        public virtual void WriteRangeData(int startRow, int startCol, List<Tuple<int, int, object>> dataTuple)
        {
            if (worksheet != null && dataTuple.Count > 0)
            {
                startRow++;
                startCol++;

                int index = 0;
                foreach (var tuple in dataTuple)
                {
                    int row = tuple.Item1 + startRow;
                    int col = tuple.Item2 + startCol;

                    Range range = worksheet.Cells[row, col];
                    range.Value = tuple.Item3;
                }
                worksheet.Columns.AutoFit();
            }
        }
       
        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
            }
        }

        public virtual void Save()
        {
            excelApp.DisplayAlerts = false;
            workbook?.SaveAs(FilePath, XlFileFormat.xlWorkbookDefault);
        }
    }
}
