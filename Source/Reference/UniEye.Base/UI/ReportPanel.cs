using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class ReportPanel : UserControl, IReportPanel
    {
        List<DrawBox> imageList = new List<DrawBox>();
        public List<DrawBox> ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        int viewIndex;
        public int ViewIndex
        {
            get { return viewIndex; }
            set { viewIndex = value; }
        }

        bool onUpdateList = false;

        public ReportPanel()
        {
            InitializeComponent();
            labelTotal.Text = StringManager.GetString(this.GetType().FullName, labelTotal.Text);
            labelNG.Text = StringManager.GetString(this.GetType().FullName, labelNG.Text);
            labelGood.Text = StringManager.GetString(this.GetType().FullName, labelGood.Text);
            searchNg.Text = StringManager.GetString(this.GetType().FullName, searchNg.Text);
            searchGood.Text = StringManager.GetString(this.GetType().FullName, searchGood.Text);
            labelStart.Text = StringManager.GetString(this.GetType().FullName, labelStart.Text);
            labelEnd.Text = StringManager.GetString(this.GetType().FullName, labelEnd.Text);
            labelModel.Text = StringManager.GetString(this.GetType().FullName, labelModel.Text);
            this.viewIndex = CustomizeSettings.Instance().NumOfResultView;
            labelModel.Text = StringManager.GetString(this.GetType().FullName, labelModel.Text);
            labelStart.Text = StringManager.GetString(this.GetType().FullName, labelStart.Text);
            labelEnd.Text = StringManager.GetString(this.GetType().FullName, labelEnd.Text);
            searchGood.Text = StringManager.GetString(this.GetType().FullName, searchGood.Text);
            searchNg.Text = StringManager.GetString(this.GetType().FullName, searchNg.Text);
            searchFalseReject.Text = StringManager.GetString(this.GetType().FullName, searchFalseReject.Text);
            btnSearch.Text = StringManager.GetString(this.GetType().FullName, btnSearch.Text);

            AddPictureBox();
            VisibleChangeControl();
        }

        public void Initialize()
        {
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            RefreshReportPage();
        }

        public void RefreshReportPage()
        {
            SystemManager.Instance().ModelManager.Refresh();

            modelCombo.Items.Clear();

            foreach (ModelDescription md in SystemManager.Instance().ModelManager)
            {
                modelCombo.Items.Add(md.Name);
            }
            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";

            startHour.SelectedIndex = 0;
            endHour.SelectedIndex = 23;
        }

        private void VisibleChangeControl()
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (modelCombo.SelectedItem == null)
            {
                MessageBox.Show("Model is not selected");
                return;
            }

            onUpdateList = true;

            int goodCnt = 0;
            int ngCnt = 0;

            serialDataGridView.Columns.Clear();

            serialDataGridView.ColumnCount = 2;
            serialDataGridView.ColumnHeadersVisible = true;
            serialDataGridView.Columns[0].Name = "Serial No.";
            serialDataGridView.Columns[1].Name = "Result";            

            string modelName = modelCombo.SelectedItem.ToString();

            bool searchAll = (searchGood.Checked == searchNg.Checked);
            DateTime startTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, Convert.ToInt32(startHour.Text), 0, 0);
            DateTime endTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, Convert.ToInt32(endHour.Text), 59, 59);            
            string defaultPath = PathSettings.Instance().Result + @"\" + modelName;
            
            DirectoryInfo directoryInfo = new DirectoryInfo(defaultPath);
            List<DirectoryInfo> searchDirector = new List<DirectoryInfo>();

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                if(startTime.Date <= directory.CreationTime.Date && directory.CreationTime.Date <= endTime.Date)
                {
                    searchDirector.Add(directory);
                }
            }
            if (searchDirector.Count == 0)
            {
                MessageForm.Show(ParentForm, "Not found result data.");
                return;
            }

            List<FileInfo> resultFiles = new List<FileInfo>();
            List<string> dailyResultFiles = new List<string>();

            foreach (DirectoryInfo directory in searchDirector)
            {
                foreach(DirectoryInfo di in directory.GetDirectories())
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        if (file == null)
                            continue;
                        if (file.Name == "result.csv")
                        {
                            resultFiles.Add(file);
                            dailyResultFiles.Add(file.FullName);
                        }
                    }
                }
            }
            //모든 파일을 불러 온다
            foreach (string dailyResultFile in dailyResultFiles)
            {
                using (StreamReader sr = File.OpenText(dailyResultFile))
                {
                    string lineStr;
                    List<string> tempList = new List<string>();
                    while ((lineStr = sr.ReadLine()) != null)
                    {
                        tempList.Add(lineStr);
                    }
                    string[] words = tempList[1].Split(new char[] { ',' });
                    string serialNo = words[1].Trim();
                    string tempInspectionTime = words[2].Trim();
                    DateTime inspectionTime = DateTime.Parse(tempInspectionTime);
                    string resultStr = words[3].Trim();
                    
                    if (resultStr == "Reject")
                        resultStr = "NG";
                    if (resultStr == "Accept")
                        resultStr = "OK";
                    if (resultStr == "FalseReject")
                        resultStr = "OverKill";

                    if (searchAll == false)
                    {
                        if (searchNg.Checked && resultStr != "NG")
                            continue;

                        if (searchGood.Checked && resultStr != "OK")
                            continue;

                        if (searchFalseReject.Checked && resultStr != "OverKill")
                            continue;
                    }
                    if (startTime <= inspectionTime && inspectionTime <= endTime)
                    {
                        int rowIndex = serialDataGridView.Rows.Add(serialNo, resultStr);

                        serialDataGridView.Rows[rowIndex].Tag = inspectionTime;

                        if (resultStr == "NG")
                        {
                            serialDataGridView.Rows[rowIndex].Cells[1].Style.BackColor = Color.Red;
                            ngCnt++;
                        }
                        else
                        {
                            serialDataGridView.Rows[rowIndex].Cells[1].Style.BackColor = Color.LightGreen;
                            goodCnt++;
                        }
                    }
                }
            }

            productionTotal.Text = (ngCnt + goodCnt).ToString();
            productionNg.Text = ngCnt.ToString();
            productionGood.Text = goodCnt.ToString();

            serialDataGridView.Sort(serialDataGridView.Columns[0], ListSortDirection.Descending);

            onUpdateList = false;
        }

        public void ModelAutoSelector()
        {
            Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel.IsEmpty() == true)
                return;

            if (modelCombo.Text != curModel.Name)
            {
                modelCombo.SelectedItem = curModel.Name;
            }
        }

        private void serialDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (serialDataGridView.SelectedRows.Count == 0)
                return;

            int rowIndex = serialDataGridView.SelectedRows[0].Index;
            string resultPath = GetResultPath(rowIndex);

            if (String.IsNullOrEmpty(resultPath) == false)
            {
                SystemManager.Instance().MainForm.ModifyTeaching(resultPath);
            }
        }

        string GetResultPath(int rowIndex)
        {
            string modelName = modelCombo.SelectedItem.ToString();
            string serialNo = serialDataGridView.Rows[rowIndex].Cells[0].Value.ToString();
            DateTime inspectionTime = (DateTime)serialDataGridView.Rows[rowIndex].Tag;

            string date = inspectionTime.ToString("yyyyMMdd");
            string time = serialNo.Substring(8, 2);

            return String.Format("{0}\\{1}\\{2}\\{3}", PathSettings.Instance().Result, modelName, date, serialNo);
        }

        private List<string> GetImagePathList(string resultPath, string surfix = "")
        {
            List<string> searchPathList = new List<string>();

            List<string> imagePathList = new List<string>();
            for (int i = 0; i < viewIndex; i++)
            {
                string imageName = String.Format("Image_C{0:00}_S{1:000}_L00{2}", i, Convert.ToInt32(stepNo.Text), surfix);
                imagePathList.Add(string.Format("{0}\\{1}.bmp", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\{1}.jpeg", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\{1}.png", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\Image_C{1:00}_S{2:000}.3d", resultPath, i, Convert.ToInt32(stepNo.Text)));
            }

            string imagePath = "";
            foreach (string searchImagePath in imagePathList)
            {
                if (File.Exists(searchImagePath) == true)
                {
                    LogHelper.Debug(LoggerType.Operation, "Search Path - " + searchImagePath);
                    searchPathList.Add(searchImagePath);
                }
            }
            imagePathList.Clear();

            LogHelper.Debug(LoggerType.Operation, "Found path - " + imagePath);

            return searchPathList;
        }

        private void serialDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        private void modelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.serialDataGridView.Rows.Clear(); //모델이 바뀌면 데이터 리스트 초기화
            if(MachineSettings.Instance().NumCamera > 0)
            {
                foreach (DrawBox drawBox in imageList)
                    drawBox.UpdateImage( null);
            }

            int count = SystemManager.Instance().CurrentModel.InspectionStepList.Count();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    stepNo.Items.Add(i);

                stepNo.Text = "0";
            }
        }

        private void AddPictureBox()
        {
            int numOfResultView = CustomizeSettings.Instance().NumOfResultView;

            resultImageTable.ColumnStyles.Clear();
            resultImageTable.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));
            resultImageTable.ColumnCount = numCount;
            resultImageTable.RowCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));

            for (int i = 0; i < resultImageTable.ColumnCount; i++)
            {
                resultImageTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / resultImageTable.ColumnCount));
            }

            for (int i = 0; i < resultImageTable.RowCount; i++)
            {
                resultImageTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / resultImageTable.RowCount));
            }

            float widthPct = (float)100 / numOfResultView;

            resultImageTable.Controls.Clear();
            resultImageTable.Dock = DockStyle.None;

            for (int i = 0; i < numOfResultView; i++)
            {
                imageList.Add(new DrawBox());

                imageList[i].Size = new Size(1024, 768);
                imageList[i].Dock = DockStyle.Fill;

                int rowIndex = i / numCount;
                int colIndex = i % numCount;

                resultImageTable.Controls.Add(imageList[i], colIndex, rowIndex);
            }

            resultImageTable.Dock = DockStyle.Fill;
        }

        private void serialDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        private void SerialDataGridViewCellEvent()
        {
            if (serialDataGridView.SelectedRows.Count == 0 || onUpdateList)
                return;

            int rowIndex = serialDataGridView.SelectedRows[0].Index;

            String resultPath = GetResultPath(rowIndex);

            List<string> imagePathList = new List<string>();
            if (OperationSettings.Instance().ShowNGImage == false)
                imagePathList = GetImagePathList(resultPath);
            else
            {
                imagePathList = GetImagePathList(resultPath, "R");
                if (imagePathList.Count == 0) //그려진   이미지가 없을 시 원본 이미지를 출력한다
                {
                    imagePathList = GetImagePathList(resultPath);
                }
            }

            if (imagePathList.Count > 0)
            {
                for (int i = 0; i < Math.Min(CustomizeSettings.Instance().NumOfResultView, imagePathList.Count); i++)
                {
                    if (File.Exists(imagePathList[i]))
                    {
                        imageList[i].UpdateImage((Bitmap)Image.FromFile(imagePathList[i]));
                        imageList[i].MouseDoubleClicked = null;
                    }
                }
            }
        }

        private void stepNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void Search(ProductionBase production)
        {
            throw new NotImplementedException();
        }

        public void Export(ProductionBase[] productions)
        {
            throw new NotImplementedException();
        }
    }
}
