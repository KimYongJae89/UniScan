using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniScan.UI
{
    public partial class ReportPage : UserControl, IMainTabPage, IReportPage
    {
        List<DirectoryInfo> findedLotList = new List<DirectoryInfo>();
        bool onUpdateList = false;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ReportPage()
        {
            InitializeComponent();
            btnSearch.Text = StringManager.GetString(this.GetType().FullName, btnSearch.Text);            

            VisibleChangeControl();

            Initialize();

            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";
        }

        public void EnableControls(UserType user)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
                RefreshReportPage();
                findModel.Text = "";
                //ModelAutoSelector();

                Search();
            }
        }

        public void Initialize()
        {

        }

        private void RefreshModelList()
        {
            modelList.Rows.Clear();

            int index = 1;
            foreach (ModelDescription md in SystemManager.Instance().ModelManager)
            {
                modelList.Rows.Add(index, md.Name, md.RegistrationDate.ToString("yyyy-MM-dd"));
                index++;
            }

            totalModel.Text = string.Format("Total : {0}", index - 1);
            modelList.Sort(modelList.Columns[2], System.ComponentModel.ListSortDirection.Descending);
        }

        public void RefreshReportPage()
        {
            SystemManager.Instance().ModelManager.Refresh();
            RefreshModelList();
        }

        private void VisibleChangeControl()
        {
            
        }

        private void GetLotList()
        {

        }

        private void Search()
        {
            if (onUpdateList == true)
                return;

            onUpdateList = true;
            findedLotList.Clear();

            this.lotNoList.Rows.Clear(); //모델이 바뀌면 데이터 리스트 초기화           

            List<string> modelPathList = new List<string>();

            DateTime startTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
            DateTime endTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            if (Directory.Exists(PathSettings.Instance().Result) == true)
            {
                string[] dateDirs = Directory.GetDirectories(PathSettings.Instance().Result);

                DateTime folderDateTime;
                foreach (string dateStr in dateDirs)
                {
                    if (DateTime.TryParse(dateStr, out folderDateTime) == false)
                        continue;

                    if (folderDateTime.Date >= startTime.Date && folderDateTime <= endTime.Date)
                    {
                        string[] modelDirs = Directory.GetDirectories(Path.Combine(PathSettings.Instance().Result, dateStr));

                        foreach (string modelName in modelDirs)
                        {
                            if (findModel.Text != "" && !modelName.Contains(findModel.Text.ToString().ToUpper()))
                            {
                                continue;
                            }

                            modelPathList.Add(Path.Combine(PathSettings.Instance().Result, dateStr, modelName));
                        }
                    }
                }
            }

            foreach (string modelPath in modelPathList)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(modelPath);
                //List<DirectoryInfo> searchDirector = new List<DirectoryInfo>();

                if (directoryInfo.Exists == true)
                {
                    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                    {
                        findedLotList.Add(directory);
                    }
                }
            }

            foreach (DirectoryInfo directory in findedLotList)
            {
                int rowIndex = lotNoList.Rows.Add(directory.Parent.Parent.Name, directory.Parent.Name, directory.Name);
                lotNoList.Rows[rowIndex].Tag = directory.FullName;
            }

            totalLot.Text = string.Format("Total : {0}", lotNoList.Rows.Count);
            onUpdateList = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search();
            SerialDataGridViewCellEvent();
        }

        public void ModelAutoSelector()
        {
            Model curModel = SystemManager.Instance().CurrentModel;

            if (curModel == null)
            {
                findModel.Text = "";
                return;
            }
                
            if (curModel.IsEmpty() == true)
            {
                findModel.Text = "";
                return;
            }

            findModel.Text = curModel.Name;
        }
        
        private List<string> GetImagePathList(string resultPath, string surfix = "")
        {
            List<string> searchPathList = new List<string>();

            List<string> imagePathList = new List<string>();
            //for (int i = 0; i < viewIndex; i++)
            {
                //string imageName = String.Format("Image_C{0:00}_S{1:000}_L00{2}", i, , surfix);
                //imagePathList.Add(string.Format("{0}\\{1}.bmp", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\{1}.jpeg", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\{1}.png", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\Image_C{1:00}_S{2:000}.3d", resultPath, i, Convert.ToInt32(stepNo.Text)));
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
            if (lotNoList.SelectedRows.Count < 1)
                return;

            findLotNo.Text = (string)lotNoList.SelectedRows[0].Cells[2].Value;
        }

        private void modelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void SerialDataGridViewCellEvent()
        {
            if (onUpdateList == true)
                return;

            if (lotNoList.SelectedRows.Count <= 0)
                return;
            
            string fullPath = lotNoList.SelectedRows[0].Tag.ToString();

            // Update Data
        }

        private void findLotNo_TextChanged(object sender, EventArgs e)
        {
            onUpdateList = true;

            if (string.IsNullOrEmpty(findLotNo.Text) == true)
                return;

            lotNoList.Rows.Clear();

            foreach (DirectoryInfo directory in findedLotList)
            {
                if (directory.Name.Contains(findLotNo.Text))
                {
                    int rowIndex = lotNoList.Rows.Add(directory.Parent.Parent.Name, directory.Parent.Name, directory.Name);
                    lotNoList.Rows[rowIndex].Tag = directory.FullName;
                }
            }

            onUpdateList = false;
        }

        private void findModel_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void lotNoList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (lotNoList.SelectedRows.Count < 1)
                return;

            findLotNo.Text = (string)lotNoList.SelectedRows[0].Cells[2].Value;
            //SerialDataGridViewCellEvent();
        }

        private void modelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ModelListClick();
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            ModelListClick();
        }

        private void ModelListClick()
        {
            if (modelList.SelectedRows.Count == 0)
                return;

            findModel.Text = modelList.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
