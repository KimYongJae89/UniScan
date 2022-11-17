using DynMvp.Device.Daq.Sensor.UsbSensorGloss_60;
using GlossSensorTest.DatabaseManager;
using GlossSensorTest.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using MsWorkbook = Microsoft.Office.Interop.Excel.Workbook;
using MsWorksheet = Microsoft.Office.Interop.Excel.Worksheet;
using System.Windows.Forms;
using System.IO;
using DynMvp.UI.Touch;
using System.Collections;
using System.Globalization;
using DynMvp.Device.Device.Serial.Sensor.CD22_15_485;
using DynMvp.Device.Serial;

namespace GlossSensorTest
{
    public partial class Form1 : Form
    {
        SerialSensorCD22_15_485 serialSensor;

        public Form1()
        {
            InitializeComponent();
            DatabaseInitialize();

            SerialSensorInfo serialSensorInfo = new DynMvp.Device.Serial.SerialSensorInfo();
            serialSensorInfo.DeviceName = "SerialSensorCD22_15_485";
            serialSensorInfo.SensorType = SerialSensorType.CD22_15_485;
            serialSensorInfo.SerialPortInfo = new DynMvp.Devices.Comm.SerialPortInfo("COM5", /*1250000*/ 115200);

            this.serialSensor = new SerialSensorCD22_15_485(serialSensorInfo);

            //this.serialSensor = SerialSensorSC_HG1_485.Create(serialSensorInfo);
            this.serialSensor.Initialize();
        }

        private void DatabaseInitialize()
        {
            DbConnectionInfo dbConnectionInfo = new DbConnectionInfo(
                                    Settings.Default.DbSource,
                                    Settings.Default.DbPort,
                                    Settings.Default.DbPath,
                                    Settings.Default.DbUserID,
                                    Settings.Default.DbPassword);
            var connection = MeasureDbConnectionFactory.Create(dbConnectionInfo);
            MeasureResultHandler.Instance.Initialize(new MeasureDbSource(connection));
        }

        bool IsLongRunProc = true;
        bool IsOpen = false;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (IsOpen == false)
            {
                StringBuilder result = new StringBuilder();

                List<GlossSensorInfo> deviceList = UsbSensorGloss60.ListDevices();

                if (deviceList == null || deviceList.Count() == 0)
                {
                    MessageBox.Show("Device를 찾을 수 없습니다.");
                    return;
                }

                foreach (var device in deviceList)
                {
                    UsbSensorGloss60.Open(device.PortNo);
                    MessageBox.Show("Com [" + device.PortNo.ToString() + "] : Open Success");
                }

                IsOpen = true;
            }
            else
            {
                MessageBox.Show("이미 오픈되어있습니다..");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }

            if (IsOpen)
            {
                UsbSensorGloss60.Close();
                MessageBox.Show("Close Success");

                IsOpen = false;
            }
        }

        public bool CheckingSpecialText(string txt)
        {
            string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);
            return rex.IsMatch(txt);
        }

        private void btnOnceMeasure_Click(object sender, EventArgs e)
        {
            if (CheckingSpecialText(txtSampleName.Text))
            {
                MessageBox.Show("특수문자를 제외해주세요");
                return;
            }

            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }

            float[] datas = new float[1];
            this.serialSensor.GetData(1, datas);
            lblLaserValue.Text = datas[0].ToString();

            string result = UsbSensorGloss60.Measure().ToString();
            lblValue.Text = result.ToString();

            MeasureInfo measureInfo = new MeasureInfo(result.ToString(), DateTime.Now, txtSampleName.Text, datas[0].ToString(), txtTilting.Text);
            MeasureResultHandler.Instance.AddMeasure(measureInfo);
            MeasureResultHandler.Instance.Save();
        }

        int repeatCnt;
        private void btnRepeat_Click(object sender, EventArgs e)
        {
            if (CheckingSpecialText(txtSampleName.Text))
            {
                MessageBox.Show("특수문자를 제외해주세요");
                return;
            }

            repeatCnt = 0;
            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }
            string result = "";

            Task task = new Task(new Action(RepeatProc));
            task.Start();
        }

        private void RepeatProc()
        {
            string result = "";
            string name = txtSampleName.Text;
            string height = txtTilting.Text;
            while (true)
            {
                repeatCnt++;

                float[] datas = new float[1];
                this.serialSensor.GetData(1, datas);
                SetLaserText(datas[0].ToString());

                result = UsbSensorGloss60.Measure().ToString();
                SetGlossText(result);
                MeasureInfo measureInfo = new MeasureInfo(result, DateTime.Now, txtSampleName.Text, datas[0].ToString(), txtTilting.Text);
                MeasureResultHandler.Instance.AddMeasure(measureInfo);

                if (repeatCnt == this.MeasureCnt.Value)
                {
                    MeasureResultHandler.Instance.Save();
                    MessageBox.Show("Measure Finish");
                    break;
                }
            }
        }

        private void btnLongRunStart_Click(object sender, EventArgs e)
        {
            if (CheckingSpecialText(txtSampleName.Text))
            {
                MessageBox.Show("특수문자를 제외해주세요");
                return;
            }

            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }

            IsLongRunProc = true;
            Task task = new Task(new Action(LongRunProc));
            task.Start();
        }

        private void LongRunProc()
        {
            string result = "";
            while (IsLongRunProc == true)
            {
                float[] datas = new float[1];
                this.serialSensor.GetData(1, datas);
                SetLaserText(datas[0].ToString());

                result = UsbSensorGloss60.Measure().ToString();
                SetGlossText(result);
                MeasureInfo measureInfo = new MeasureInfo(result, DateTime.Now, txtSampleName.Text, datas[0].ToString(), txtTilting.Text);
                MeasureResultHandler.Instance.AddMeasure(measureInfo);
            }
        }

        private void btnLongRunStop_Click(object sender, EventArgs e)
        {
            if (IsLongRunProc == false)
                return;

            IsLongRunProc = false;
            MeasureResultHandler.Instance.Save();
            MessageBox.Show("Measure Finish");
        }

        delegate void SetLaserTextCallback(string text);
        private void SetLaserText(string text)
        {
            if (this.lblLaserValue.InvokeRequired)
            {
                SetLaserTextCallback d = new SetLaserTextCallback(SetLaserText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblLaserValue.Text = text;
            }
        }

        delegate void SetGlossTextCallback(string text);
        private void SetGlossText(string text)
        {
            if (this.lblValue.InvokeRequired)
            {
                SetLaserTextCallback d = new SetLaserTextCallback(SetGlossText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblValue.Text = text;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MeasureResultHandler.Instance.Load();
            var measureList = MeasureResultHandler.Instance.GetMeasures(startDate.Value, endDate.Value);

            if (measureList.Count != 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = string.Format("{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                dlg.Filter = "Excel(*.xlsx)|*.xlsx";

                if (dlg.ShowDialog() == DialogResult.OK)
                    ExportExcel(dlg.FileName, measureList);
            }
            else
            {
                MessageBox.Show("기록이 존재하지않습니다.");
            }
        }

        private void ExportExcel(string fileName, List<MeasureInfo> measureList)
        {
            List<Tuple<int, int, object>> dataList = new List<Tuple<int, int, object>>();

            ExcelTemplate excel = new ExcelTemplate(fileName);
            excel.SelectSheet("MeasureData", true);

            int rowIndex = 0;
            int resultNum = 1;
            int numColumn = 0;
            int sampleColumn = 1;
            int heightColumn = 2;
            int dateColumn = 3;
            int measureColumn = 4;

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Excel Export...");
            simpleProgressForm.Show(() =>
            {
                dataList.Add(new Tuple<int, int, object>(rowIndex, numColumn, "Measure Num"));
                dataList.Add(new Tuple<int, int, object>(rowIndex, sampleColumn, "Sample Name"));
                dataList.Add(new Tuple<int, int, object>(rowIndex, heightColumn, "Height"));
                dataList.Add(new Tuple<int, int, object>(rowIndex, measureColumn, "Measure Value"));
                dataList.Add(new Tuple<int, int, object>(rowIndex, dateColumn, "Measure Time"));
                rowIndex++;

                foreach (var measurePair in measureList)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex, sampleColumn, resultNum++));
                    dataList.Add(new Tuple<int, int, object>(rowIndex, numColumn, measurePair.SampleName));
                    dataList.Add(new Tuple<int, int, object>(rowIndex, heightColumn, measurePair.Distance));
                    dataList.Add(new Tuple<int, int, object>(rowIndex, measureColumn, measurePair.Measure));
                    dataList.Add(new Tuple<int, int, object>(rowIndex, dateColumn, measurePair.MeasureDate));
                    rowIndex++;
                }
                excel.WriteRangeData(1, 1, dataList);
                excel.SetChartRange("d2");
                excel.Save();
                excel.Dispose();
            });
            MessageBox.Show("Excport Complete!!!!");
        }

        private void btnLaserOff_Click(object sender, EventArgs e)
        {
            serialSensor.LaserOff();
        }
        private void btnLaserInit_Click(object sender, EventArgs e)
        {
            serialSensor.ExcuteInitiallize();
        }
        private void btnLaserOn_Click(object sender, EventArgs e)
        {
            serialSensor.LaserOn();
        }

        private void btnLaserMeasure_Click(object sender, EventArgs e)
        {
            float[] datas = new float[1];
            this.serialSensor.GetData(1, datas);
            lblLaserValue.Text = datas[0].ToString();
        }
    }
}
