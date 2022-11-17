using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
//using UniScanM.Data;
using DynMvp.Base;
using System.IO;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.UI.Touch;
using UniScanM.ColorSens.Data;
using DynMvp.Authentication;

namespace UniScanM.ColorSens.UI
{
    public partial class ReportPage : UserControl, IMainTabPage, IReportPage, IMultiLanguageSupport
    {
        List<float> list_SheetBrightness = new List<float>();
        List<DateTime> list_GraphX_Time = new List<DateTime>();

        public ReportPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        private void ReportPanel_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy. MM. dd.";
            dateTimePicker1.Value = DateTime.Now.Date - new TimeSpan(1, 0, 0, 0);
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy. MM. dd.";
            dateTimePicker2.Value = DateTime.Now.Date;
            //UpdateList1();


            Random rand = new Random();
            chart_FullTime.Series["graphdata"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart_FullTime.Series["graphdata"].Points.Add(rand.Next(100));
            }
        }

        private void buttonStartSearch_Click(object sender, EventArgs e)
        {
            this.dataGridViewSearch.Rows.Clear();
            List<ColorSens.Data.InspectionResult> inspectionResultList = SearchResult(dateTimePicker1.Value, dateTimePicker2.Value.AddDays(1));

            foreach (ColorSens.Data.InspectionResult inspectionResult in inspectionResultList)
            {
                string[] directories = inspectionResult.ResultPath.Split('\\').Reverse().ToArray();
                
                string lotNo = directories[0];
                string runMode = directories[1];
                string model = directories[2];
                string startDate = directories[3];

                int sequenceResultCount = -1;
                if (inspectionResult.ExtraResult.ContainsKey("SequenceResultCount"))
                    sequenceResultCount = (int)inspectionResult.ExtraResult["SequenceResultCount"];
            
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewSearch, startDate, model, runMode, lotNo);
                dataGridViewRow.Tag = inspectionResult;

                this.dataGridViewSearch.Rows.Add(dataGridViewRow);
            }
        }
        public List<InspectionResult> SearchResult(DateTime fromDate, DateTime toDate)
        {
            string resultPath = PathSettings.Instance().Result;

            List<InspectionResult> inspectionResultList =
                Search(resultPath, fromDate, toDate).ConvertAll<InspectionResult>(f => (InspectionResult)f);
            return inspectionResultList;
        }
        
        private void dataGridViewSearch_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridViewResult.Rows.Clear();
            if (dataGridViewSearch.SelectedRows.Count == 0)
                return;

            ColorSens.Data.InspectionResult inspectionResult = (ColorSens.Data.InspectionResult)dataGridViewSearch.SelectedRows[0].Tag;
            if (inspectionResult == null)
                return;

            list_SheetBrightness.Clear();
            list_GraphX_Time.Clear();
            string resultFile = Path.Combine(inspectionResult.ResultPath, "Result.csv");

            try
            {
                StreamReader sr = new StreamReader(resultFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] token = line.Split(',');
                    int index;
                    if (int.TryParse(token[0], out index) == false)
                        continue;

                    TimeSpan dateTime = new TimeSpan(int.Parse(token[1].Substring(0, 2)), int.Parse(token[1].Substring(2, 2)), int.Parse(token[1].Substring(4, 2)));

                    ColorSens.Data.InspectionResult subInspectionResult = new ColorSens.Data.InspectionResult();
                    subInspectionResult.ResultPath = inspectionResult.ResultPath;

                    subInspectionResult.InspectionNo = index.ToString();
                    subInspectionResult.InspectionStartTime = inspectionResult.InspectionStartTime;// + dateTime;

                    float brightness = float.Parse(token[2]);
                    bool resultok = token[3] == "OK" ? true: false;


                    list_SheetBrightness.Add(brightness);
                    list_GraphX_Time.Add(subInspectionResult.InspectionStartTime);


                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    dataGridViewRow.CreateCells(
                        dataGridViewResult, 
                        index, 
                        string.Format("{0:F2}", brightness),
                        resultok ? "OK" : "NG");
                    if (resultok == false)
                    {
                        dataGridViewRow.DefaultCellStyle.BackColor = Color.Red;
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.White;
                    }
                    dataGridViewRow.Tag = subInspectionResult;
                    this.dataGridViewResult.Rows.Add(dataGridViewRow);
                }
                chart_FullTime.Series["graphdata"].Points.DataBindXY(list_GraphX_Time, list_SheetBrightness);
            }
            catch(IOException ex)
            {
                MessageForm.Show(null, ex.Message);

            }
        }

        private void dataGridViewResult_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResult.SelectedRows.Count == 0)
                return;

            ImageD imageD = null;
            ColorSens.Data.InspectionResult inspectionResult = (ColorSens.Data.InspectionResult)dataGridViewResult.SelectedRows[0].Tag;
            if (inspectionResult.GrabImageList.Count == 0)
            {
                string imagePath = Path.Combine(inspectionResult.ResultPath, string.Format("{0}.jpg", inspectionResult.InspectionNo));
                if (File.Exists(imagePath))
                {
                    imageD = new Image2D();
                    imageD.LoadImage(imagePath);
                    inspectionResult.GrabImageList.Add(imageD);
                }
            }
            else
                imageD = inspectionResult.GrabImageList[0];

            resultView.Image = imageD?.ToBitmap();

            //SystemManager.Instance().MainForm.MonitoringPage.InspectionPanel.ProductInspected(inspectionResult);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void RefreshReportPage()
        {
            throw new NotImplementedException();
        }

        public void ModelAutoSelector()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void EnableControls(UserType User)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }

        private void buttonExplorer_Click(object sender, EventArgs e)
        {
            string path = PathSettings.Instance().Result.Replace("Result", "Report");
            System.Diagnostics.Process.Start(path);
        }

        public List<InspectionResult> Search(string resultPath, DateTime start, DateTime end)
        {
            string modelName = null;
            List<InspectionResult> inspectionResultList = new List<InspectionResult>();

            // StartDate \ ModelName \ RunMode \ (StartTime or Lot)

            // Filter Date
            List<string> dateFilterd = SearchAt(resultPath, new Predicate<string>(f =>
            {
                DateTime dtDate = new DateTime();
                
                bool result = DateTime.TryParseExact(f, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dtDate);
                
                return result == true && start <= dtDate && dtDate <= end;
            }));

            // Filter Model Name
            List<string> modelFilterd = new List<string>();
            foreach (string s in dateFilterd)
            {
                if (string.IsNullOrEmpty(modelName))
                    modelFilterd.AddRange(SearchAt(s, new Predicate<string>(f => true)));
                else
                    modelFilterd.AddRange(SearchAt(s, new Predicate<string>(f => f == modelName)));
            }

            List<string> pathList = new List<string>();
            foreach (string s in modelFilterd)
            {
                List<string> runModeList = SearchAt(s, new Predicate<string>(f => true));
                foreach (string t in runModeList)
                {
                    pathList.AddRange(SearchAt(t, new Predicate<string>(f => true)));
                }
            }

            foreach (string path in pathList)
            {
                if (File.Exists(Path.Combine(path, "Result.csv")))
                {
                    InspectionResult inspectionResult = new InspectionResult();
                    inspectionResult.ResultPath = path;
                    inspectionResultList.Add(inspectionResult);
                }
            }
            return inspectionResultList;
        }

        private List<string> SearchAt(string searchPath, Predicate<string> predicate)
        {
            List<string> result = new List<string>();
            string[] directorys = Directory.GetDirectories(searchPath);

            foreach (string directory in directorys)
            {
                string name = Path.GetFileNameWithoutExtension(directory);
                if (predicate(name))
                    result.Add(directory);
            }
            return result;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
