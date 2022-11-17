using DataCollector.UI;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollector
{
    public partial class MainForm : Form
    {
        string monitorPath;
        string edmsPath;
        string colorSensorPath;
        string rvmsPath;
        string pinHolePath;

        Workbook collectWorkbook;

        string pathFileName = "DataCollectorPath.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            string resultFileName = Path.Combine(Settings.Instance().Result, String.Format("Result_{0}.xlsx", curTime.ToString("yyyyMMdd")));

            if (File.Exists(resultFileName) == false)
            {
                string tempResultFileName = Path.Combine(Settings.Instance().Result, String.Format("TempResult_{0}.xlsx", curTime.ToString("yyyyMMdd")));
                if (File.Exists(tempResultFileName) == true)
                {
                    File.Move(tempResultFileName, resultFileName);
                    collectWorkbook = Workbook.Load(resultFileName);
                }
                else
                {
                    string templateFileName = Path.Combine(Settings.Instance().Result, "RawDataTemplate_Collection.xlsx");
                    if (File.Exists(templateFileName) == false)
                        return;

                    collectWorkbook = Workbook.Load(templateFileName);
                }
            }
            else
            {
                collectWorkbook = Workbook.Load(resultFileName);
            }

            ultraSpreadsheet1.Workbook = collectWorkbook;

            LoadPath();

            dataCollectionTimer.Start();
        }

        private void dataCollectionTimer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            string resultFileName = Path.Combine(Settings.Instance().Result, String.Format("Result_{0}.xlsx", curTime.ToString("yyyyMMdd")));
            string tempResultFileName = Path.Combine(Settings.Instance().Result, String.Format("TempResult_{0}.xlsx", curTime.ToString("yyyyMMdd")));

            if (File.Exists(resultFileName) == false)
            {
                string templateFileName = Path.Combine(Settings.Instance().Result, "RawDataTemplate_Collection.xlsx");
                if (File.Exists(templateFileName) == false)
                    return;

                collectWorkbook = Workbook.Load(templateFileName);

                ultraSpreadsheet1.Workbook = collectWorkbook;
            }

            Worksheet collectLogSheet = collectWorkbook.Worksheets["LOG"];
            object value = collectLogSheet.Rows[3].Cells[8].Value;
            int numData = Convert.ToInt32(value);
            int collectReadRow = Convert.ToInt32(numData) + 9;

            collectLogSheet.Rows[collectReadRow].Cells[0].Value = curTime.ToString("yyyy.MM.dd"); // Start Time
            collectLogSheet.Rows[collectReadRow].Cells[1].Value = curTime.ToString("HH:mm:ss"); // Start Time

            Task taskMonitor = new Task(() => { AppendMonitorData(collectLogSheet, collectReadRow); });
            Task taskEDMS = new Task(() => { AppendEDMSData(collectLogSheet, collectReadRow); });
            Task taskColorSensor = new Task(() => { AppendColorSensorData(collectLogSheet, collectReadRow); });
            Task taskRVMS = new Task(() => { AppendRVMSData(collectLogSheet, collectReadRow); });
            Task taskPinHole = new Task(() => { AppendPinHoleData(collectLogSheet, collectReadRow); });

            taskMonitor.Start();
            taskEDMS.Start();
            taskColorSensor.Start();
            taskRVMS.Start();
            taskPinHole.Start();

            taskMonitor.Wait();
            taskEDMS.Wait();
            taskColorSensor.Wait();
            taskRVMS.Wait();
            taskPinHole.Wait();

            Task[] taskList = new Task[] { taskMonitor, taskEDMS, taskColorSensor, taskRVMS, taskPinHole };
            Task.WaitAll(taskList, 400);

            //AppendMonitorData(collectLogSheet, collectReadRow);            
            //AppendEDMSData(collectLogSheet, collectReadRow);
            //AppendColorSensorData(collectLogSheet, collectReadRow);
            //AppendRVMSData(collectLogSheet, collectReadRow);
            //AppendPinHoleData(collectLogSheet, collectReadRow);

            if (collectReadRow < ultraSpreadsheet1.ActivePane.RowScrollRegion.StartIndex || collectReadRow > ultraSpreadsheet1.ActivePane.RowScrollRegion.EndIndex)
            {
                ultraSpreadsheet1.ActivePane.ScrollRowIntoView(collectReadRow);
            }

            collectLogSheet.Rows[3].Cells[8].Value = numData + 1;

            collectWorkbook.Save(tempResultFileName);

            try
            {
                File.Delete(resultFileName);
                File.Move(tempResultFileName, resultFileName);
            }
            catch (IOException)
            {

            }
        }

        private void AppendMonitorData(Worksheet collectLogSheet, int collectReadRow)
        {
            string lastResultPath = Path.Combine(monitorPath, "Result\\LastResult.csv");
            string lastResultPathBackup = Path.Combine(monitorPath, "Result\\LastResultBackup.csv");

            // 파일이 없을 시 백업 파일을 확인해본다. 있으면 읽고, 없으면 리턴
            if (File.Exists(lastResultPath) == false)
            {
                lastResultPath = Path.Combine(monitorPath, "Result\\LastResultBackup.csv");

                if (File.Exists(lastResultPath) == false)
                    return;
            }

            string[] data = File.ReadAllText(lastResultPath).Split(',');

            for (int i = 0; i < data.Length; i++)
                collectLogSheet.Rows[collectReadRow].Cells[2 + i].Value = data[i];

            // 데이터를 전부 써준 뒤 백업 파일로 바꿔준다.
            if (lastResultPath != lastResultPathBackup)
            {
                File.Delete(lastResultPathBackup);
                File.Move(lastResultPath, lastResultPathBackup);
            }
        }

        private void AppendEDMSData(Worksheet collectLogSheet, int collectReadRow)
        {
            string lastResultPath = Path.Combine(edmsPath, "Result\\LastResult.csv");
            string lastResultPathBackup = Path.Combine(edmsPath, "Result\\LastResultBackup.csv");

            // 파일이 없을 시 백업 파일을 확인해본다. 있으면 읽고, 없으면 리턴
            if (File.Exists(lastResultPath) == false)
            {
                lastResultPath = Path.Combine(edmsPath, "Result\\LastResultBackup.csv");

                if (File.Exists(lastResultPath) == false)
                    return;
            }

            string[] data = File.ReadAllText(lastResultPath).Split(',');

            for (int i = 0; i < data.Length; i++)
                collectLogSheet.Rows[collectReadRow].Cells[12 + i].Value = data[i];

            // 데이터를 전부 써준 뒤 백업 파일로 바꿔준다.
            if (lastResultPath != lastResultPathBackup)
            {
                File.Delete(lastResultPathBackup);
                File.Move(lastResultPath, lastResultPathBackup);
            }
        }

        private void AppendColorSensorData(Worksheet collectLogSheet, int collectReadRow)
        {
            string lastResultPath = Path.Combine(colorSensorPath, "Result\\LastResult.csv");
            string lastResultPathBackup = Path.Combine(colorSensorPath, "Result\\LastResultBackup.csv");

            // 파일이 없을 시 백업 파일을 확인해본다. 있으면 읽고, 없으면 리턴
            if (File.Exists(lastResultPath) == false)
            {
                lastResultPath = Path.Combine(colorSensorPath, "Result\\LastResultBackup.csv");

                if (File.Exists(lastResultPath) == false)
                    return;
            }

            string[] data = File.ReadAllText(lastResultPath).Split(',');

            for (int i = 0; i < data.Length - 2; i++)
            {
                if (string.IsNullOrEmpty(data[2 + i]) == false)
                    collectLogSheet.Rows[collectReadRow].Cells[19 + i].Value = data[2 + i];
            }

            // 데이터를 전부 써준 뒤 백업 파일로 바꿔준다.
            if (lastResultPath != lastResultPathBackup)
            {
                File.Delete(lastResultPathBackup);
                File.Move(lastResultPath, lastResultPathBackup);
            }
        }

        private void AppendRVMSData(Worksheet collectLogSheet, int collectReadRow)
        {
            string lastResultPath = Path.Combine(rvmsPath, "Result\\LastResult.csv");
            string lastResultPathBackup = Path.Combine(rvmsPath, "Result\\LastResultBackup.csv");

            // 파일이 없을 시 백업 파일을 확인해본다. 있으면 읽고, 없으면 리턴
            if (File.Exists(lastResultPath) == false)
            {
                lastResultPath = Path.Combine(rvmsPath, "Result\\LastResultBackup.csv");

                if (File.Exists(lastResultPath) == false)
                    return;
            }

            string[] data = File.ReadAllText(lastResultPath).Split(',');

            for (int i = 0; i < data.Length; i++)
                collectLogSheet.Rows[collectReadRow].Cells[22 + i].Value = data[i];

            // 데이터를 전부 써준 뒤 백업 파일로 바꿔준다.
            if (lastResultPath != lastResultPathBackup)
            {
                File.Delete(lastResultPathBackup);
                File.Move(lastResultPath, lastResultPathBackup);
            }
        }

        private void AppendPinHoleData(Worksheet collectLogSheet, int collectReadRow)
        {
            string lastResultPath = Path.Combine(pinHolePath, "Result\\LastResult.csv");
            string lastResultPathBackup = Path.Combine(pinHolePath, "Result\\LastResultBackup.csv");

            // 파일이 없을 시 백업 파일을 확인해본다. 있으면 읽고, 없으면 리턴
            if (File.Exists(lastResultPath) == false)
            {
                lastResultPath = Path.Combine(pinHolePath, "Result\\LastResultBackup.csv");

                if (File.Exists(lastResultPath) == false)
                    return;
            }

            string[] data = File.ReadAllText(lastResultPath).Split(',');

            for (int i = 0; i < data.Length - 5; i++)
                collectLogSheet.Rows[collectReadRow].Cells[30 + i].Value = data[5 + i];

            // 데이터를 전부 써준 뒤 백업 파일로 바꿔준다.
            if (lastResultPath != lastResultPathBackup)
            {
                File.Delete(lastResultPathBackup);
                File.Move(lastResultPath, lastResultPathBackup);
            }
        }



        private void buttonSetting_Click(object sender, EventArgs e)
        {
            SettingForm form = new SettingForm(monitorPath, edmsPath, colorSensorPath, rvmsPath, pinHolePath);

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.monitorPath = form.MonitorPath;
                this.edmsPath = form.EDMSPath;
                this.colorSensorPath = form.ColorSensorPath;
                this.rvmsPath = form.RVMSPath;
                this.pinHolePath = form.PinHolePath;

                SavePath();
            }
        }

        private void SavePath()
        {
            if (File.Exists(pathFileName) == true)
                File.Delete(pathFileName);

            using (FileStream fileStream = File.Create(pathFileName)) { }

            string pathDirectorys = "";

            pathDirectorys += monitorPath + "/";
            pathDirectorys += edmsPath + "/";
            pathDirectorys += colorSensorPath + "/";
            pathDirectorys += rvmsPath + "/";
            pathDirectorys += pinHolePath + "/";

            File.AppendAllText(pathFileName, pathDirectorys);
        }

        private void LoadPath()
        {
            if (File.Exists(pathFileName) != true)
            {
                using (FileStream fileStream = File.Create(pathFileName)) { }

                File.AppendAllText(pathFileName, @"C:\/C:\/C:\/C:\/C:\");
            }

            string pathDirectory = File.ReadAllText(pathFileName);

            string[] pathDirectorys = pathDirectory.Split('/');

            monitorPath = pathDirectorys[0];
            edmsPath = pathDirectorys[1];
            colorSensorPath = pathDirectorys[2];
            rvmsPath = pathDirectorys[3];
            pinHolePath = pathDirectorys[4];
        }

        private void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception exception) { }
            }
        }
    }
}
