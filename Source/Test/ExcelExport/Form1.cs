using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSaveExcelFile_Click(object sender, EventArgs e)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add("LOG");
            WorksheetCell cell = worksheet.Rows[0].Cells[0];
            cell.Value = "Test";
            cell.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
            cell.CellFormat.BottomBorderColorInfo = Color.Black;
            cell.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            cell.CellFormat.RightBorderColorInfo = Color.Black;
            worksheet.Columns[0].Width = 5000;

            WorksheetCell cell2 = worksheet.Rows[1].Cells[0];
            cell2.Value = "Test";

            int count = workbook.Worksheets["LOG"].Rows.Count();

            workbook.Save("Test.xls");
        }

        private void btnAppendRow_Click(object sender, EventArgs e)
        {
            //Workbook workbook = Workbook.Load("Test.xls");

            //Worksheet logSheet = workbook.Worksheets["LOG"];
            //int count = logSheet.Rows.Count();
            //logSheet.Rows[count].Cells[0].Value = "Test";
            //logSheet.Rows[count].Cells[1].Value = "Test";

            //logSheet.MergedCellsRegions.Add(count, 1, count, 3);

            //workbook.Save("Test.xls");

            Workbook workbook2 = Workbook.Load("Test4.xlsx");
            Worksheet logSheet2 = workbook2.Worksheets["LOG"];
            int index = Convert.ToInt32(logSheet2.Rows[3].Cells[10].Value) + 6;
            logSheet2.Rows[index].Cells[0].Value = "Test";
            logSheet2.Rows[index].Cells[1].Value = "Test";

            int count = index - 5;
            logSheet2.Rows[3].Cells[10].Value = count;

            workbook2.Save("Test4.xlsx");
        }

        private void btnLoadExcelFile_Click(object sender, EventArgs e)
        {
            Workbook workbook = Workbook.Load("Test2.xlsx");

            ultraGrid1.DataSource = workbook.Worksheets["LOG"];
        }
    }
}
