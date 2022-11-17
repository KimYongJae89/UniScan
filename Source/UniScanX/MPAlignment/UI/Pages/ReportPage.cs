using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DynMvp.Base;
using System.IO;

namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class ReportPage : UserControl
    {
        //PgAdminHelper pgAdminHelper;
        //List<SearchSummaryResult> currentSummarys;
        public ReportPage()
        {
            InitializeComponent();
  //          pgAdminHelper = new PgAdminHelper(SystemConfig.Instance.PgAdminInfo);
            cmbProductList.SelectedIndex = 0;
        }

        private async void btnInspect_Click(object sender, EventArgs e)
        {
 //           var searcher = pgAdminHelper.DbSearcher;
 //           currentSummarys = await Task.Run(() => searcher.GetResult(dtpSearchStart.Value, dtpSearchEnd.Value));
 //           var goodCountSummary = await Task.Run(()=> currentSummarys.Where(data => data.Judgment == DynMvp.Data.Judgment.Good).ToList());

 //           UpdateSummaryText(currentSummarys.Count, goodCountSummary.Count);

 //           dgvSearchResult.AutoGenerateColumns = false;
 //           dgvSearchResult.DataSource = currentSummarys;

 //           void UpdateSummaryText(int total, int good)
 //           {
 //               int ng = total - good;
 //               double totlaYield = GetYield(total, good);
 //               double ngYield = GetYield(total, total - good);

 //               lblResultTotal.Text = $"{total}({totlaYield} %)";
 //               lblResultGood.Text = $"{good}({totlaYield} %)";
 //               lblResultNg.Text = $"{ng}({ngYield} %)";
 //           }
        }

        double GetYield(int total, int counter)
        {
            double yield= 0f;
            try
            {
                yield = (total / counter) * 100;
            }
            catch
            {
                yield = 00.00;
            }
            return yield;
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            string startTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:01");
            string endTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            dtpSearchStart.Value = DateTime.Parse(startTime);
            dtpSearchEnd.Value = DateTime.Parse(endTime);
        }

        private void dgvSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            //int rowindex = dgvSearchResult.CurrentRow.Index;
            //string inspectionNo = dgvSearchResult.CurrentRow.Cells[0].Value.ToString();
            //var resultPath = currentSummarys.Where(x => x.InspectionNo == inspectionNo).Select(path => path.ResultPath).ToList();
            //if(resultPath.Count > 0)
            //{
            //    string imagePath = Path.Combine(resultPath[0], "FovImage.jpg");
            //    if (File.Exists(imagePath))
            //    {
            //        if (picImageResult.Image != null)
            //        {
            //            picImageResult.Image.Dispose();
            //            picImageResult.Image = null;
            //        }
            //        picImageResult.Image = ImageHelper.LoadImage(imagePath);

            //    }
            //}
        }
    }
}
