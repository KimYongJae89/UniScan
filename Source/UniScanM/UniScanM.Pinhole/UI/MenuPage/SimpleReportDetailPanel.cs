using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;
using DynMvp.Base;
using System.IO;
using System.Reflection;
using UniScanM.Pinhole.UI.MenuPage;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.UI;
using UniScanM.Pinhole.Data;
using UniScanM.Pinhole.Settings;
using System.Diagnostics;
using UniScanM.UI;
//using UniScanM.Data;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.UI.Touch;

namespace UniScanM.Pinhole.UI.MenuPage
{
    public partial class SimpleReportDetailPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        ReportDefectList reportDefectList = new ReportDefectList();
        string reportPath;

        DataTable table = new DataTable();
        private object dataGridViewResult;

        CanvasPanel defectView1;
        CanvasPanel defectView2;
        CanvasPanel[] defectViews;
        List<DataGridViewRow> sectionResultList = new List<DataGridViewRow>();

        public string AxisYFormat => "{F1}";

        public SimpleReportDetailPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            InitView();

            defectViews = new CanvasPanel[2] { defectView1, defectView2 };

            StringManager.AddListener(this);
        }
        
        public void InitView()
        {
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanel Start.");
            defectView1 = new CanvasPanel(true);
            defectView1.ShowCenterGuide = false;
            defectView1.Dock = DockStyle.Fill;
            defectView1.MouseDoubleClick += DefectView_MouseDoubleClick;

            defectView2 = new CanvasPanel(true);
            defectView2.ShowCenterGuide = false;
            defectView2.Dock = DockStyle.Fill;
            defectView2.MouseDoubleClick += DefectView_MouseDoubleClick;

            layoutDefectView.Controls.Add(defectView1, 0, 0);
            layoutDefectView.Controls.Add(defectView2, 1, 0);
            reportDefectListPanel.Controls.Add(reportDefectList);

            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanelEnd.");
        }

        private void DefectView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CanvasPanel canvasPanel = (CanvasPanel)sender;
            if (canvasPanel.Image == null)
                return;

            string imageFile = (string)canvasPanel.Tag;

            if (File.Exists(imageFile))
                System.Diagnostics.Process.Start(imageFile);
            else
                MessageForm.Show(null, string.Format(StringManager.GetString("Can not Fouund Image. [{0}]"), imageFile));
        }

        public bool ShowNgOnlyButton()
        {
            return false;
        }

        delegate void UpdateListDelegate(string path);
        private void UpdateList(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateListDelegate(UpdateList), path);
                return;
            }

        }
        
        private void chkNgOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSectionList();
        }

        void UpdateSectionList()
        {
            //UiHelper.SuspendDrawing(sectionList);
            sectionList.Rows.Clear();       
            sectionList.Rows.AddRange(sectionResultList.ToArray());

            //UiHelper.ResumeDrawing(sectionList);
        }
        private void UpdateList()
        {
            if (sectionList.SelectedRows.Count == 0)
                return;

            List<InspectionResult> inspectionResultList = sectionList.SelectedRows[0].Tag as List<InspectionResult>;
            if (inspectionResultList == null)
                return;

            Array.ForEach(defectViews, f =>
            {
                f.UpdateImage(null);
                f.WorkingFigures.Clear();
                f.Invalidate();
            });

            reportDefectList.Clear();
            for (int i = 0; i < inspectionResultList.Count; i++)
            {
                InspectionResult inspectionResult = inspectionResultList[i];
                int devIdx = inspectionResult.DeviceIndex;

                string displayImageFile = Path.Combine(inspectionResult.ResultPath, "DispImage", inspectionResult.GetDispImageName());
                if (File.Exists(displayImageFile) == false)
                    displayImageFile = Path.Combine(inspectionResult.ResultPath, "DispImage", inspectionResult.GetDispImageNameOld());

                if (File.Exists(displayImageFile) == false)
                    continue;

                this.defectViews[devIdx].Clear();
                Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(displayImageFile);
                defectViews[devIdx].UpdateImage(bitmap);
                defectViews[devIdx].Tag = displayImageFile;
                defectViews[devIdx].ZoomFit();

                List<DefectInfo> sectionDefectList = inspectionResult.LastDefectInfoList;
                FigureGroup figureGroup = new FigureGroup();
                foreach (DefectInfo defectInfo in sectionDefectList)
                {
                    Color defectColor = (defectInfo.DefectType == Data.DefectType.Dust ? Color.Red : Color.Orange);
                    Figure figure = defectInfo.GetDefectMark(defectColor, 0.2f);
                    figure.Selectable = false;
                    figureGroup.AddFigure(figure);
                }
                defectViews[devIdx].WorkingFigures.AddFigure(figureGroup.FigureList.ToArray());
                defectViews[devIdx].ZoomFit();
                defectViews[devIdx].Invalidate();
                reportDefectList.AddDefect(inspectionResult.ResultPath, sectionDefectList);
            }
        }
        private void sectionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //UpdateList();
        }

        public void Initialize() { }
        public void Clear()
        {
            sectionList.Rows.Clear();
            reportDefectList.Clear();

            defectView1.Clear();
            defectView2.Clear();

            defectView1.UpdateImage(null);
            defectView2.UpdateImage(null);
        }
        public void Search(DynMvp.Data.ProductionBase production) { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void SimpleReportDetailPanel_Load(object sender, EventArgs e)
        {

        }

        private void buttonShortCut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(reportPath))
                return;

            if(Directory.Exists(reportPath)==false)
            {
                MessageForm.Show(null, string.Format(StringManager.GetString("Report Path [{0}] is not exist"), reportPath));
                return;
            }
            Process.Start(reportPath);
        }

        public void UpdateData(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), dataImporter, showNgOnly);
                return;
            }

            sectionResultList.Clear();

            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            if (dataImporter.Count > 0)
            {
                inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
                if (showNgOnly)
                    inspectionResultList.RemoveAll(f => f.IsGood());
            }

            if (inspectionResultList.Count > 0)
            {
                int minSection = inspectionResultList.Min(f => ((InspectionResult)f).SectionIndex);
                int maxSection = inspectionResultList.Max(f => ((InspectionResult)f).SectionIndex);

                string okString = StringManager.GetString("OK");
                string skipString = StringManager.GetString("SKIP");
                string ngString = StringManager.GetString("NG");

                for (int section = minSection; section <= maxSection; section++)
                {
                    List<InspectionResult> sectionList = inspectionResultList.FindAll(f => ((InspectionResult)f).SectionIndex == section);
                    if (sectionList.Count == 0)
                        continue;

                    DataGridViewRow row = new DataGridViewRow();

                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = section });
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = sectionList.Sum(f => f.NumDefect) });
                    row.Tag = sectionList;

                    sectionResultList.Add(row);
                }
            }
            UpdateSectionList();

            reportPath = dataImporter.ResultPath.Replace("Result", "Report");
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        public bool InitializeChart(Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Distance [m]");
            chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Defects [EA]");

            chart.ChartAreas[0].AxisX.TitleFont = font;
            chart.ChartAreas[0].AxisY.TitleFont = font;
            chart.Legends[0].Font = font;

            chart.Series.Clear();
            chart.Series.Add(new Series { Name = "defects", ChartType = SeriesChartType.Line, BorderWidth = 3, LegendText = "Defects", YAxisType = AxisType.Primary });

            bool useDefaultReportPanel = true;

            return useDefaultReportPanel;
        }

        public Dictionary<string, List<DataPoint>> GetChartData(int srcPos, int dstPos, UniScanM.Data.DataImporter dataImporter)
        {
            List<DataPoint> dpList = new List<DataPoint>();

            List<InspectionResult> inspectionResult = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
            if (inspectionResult.Count > 0)
            {
                int minDist = srcPos;
                int maxDist = dstPos;
                for (int i = minDist; i <= maxDist; i++)
                {
                    List<InspectionResult> sameDistance = inspectionResult.FindAll(f => f.RollDistance == i);
                    if(sameDistance.Count==0)
                    {
                        DataPoint dp = new DataPoint(i, 0);
                        dpList.Add(dp);
                    }
                    else
                    {
                        int minSection = sameDistance.Min(f => f.SectionIndex);
                        int maxSection = sameDistance.Max(f => f.SectionIndex);
                        int sectionCount = maxSection - minSection + 1;
                        for (int j = 0; j < sectionCount; j++)
                        {
                            List<InspectionResult> sameSection = sameDistance.FindAll(f => f.SectionIndex == j + minSection);
                            if (sameSection.Count > 0)
                            {
                                float newDistance = i + j * 1.0f / sectionCount;
                                float defects = sameSection.Sum(f => f.NumDefect);

                                DataPoint dp = new DataPoint(newDistance, defects);
                                dpList.Add(dp);
                            }
                        }
                    }
                }
            }

            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("Brightness", dpList);
            return result;
        }

        private void sectionList_SelectionChanged(object sender, EventArgs e)
        {
            UpdateList();
        }
    }
}
