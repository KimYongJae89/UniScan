using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using UniScanS.Screen.Vision;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Screen.Data;
using DynMvp.Devices.Dio;
using DynMvp.UI;
using UniScanS.Common.Settings;

namespace UniScanS.Data.UI
{
    public partial class ErrorForm : Form, IMultiLanguageSupport
    {
        ErrorChecker curErrorChecker;

        public ErrorForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);
        }
        
        public void UpdateData(ErrorChecker errorChecker)
        {
            curErrorChecker = errorChecker;

            List<DataGridViewRow> sheetRowList = new List<DataGridViewRow>();
            
            foreach (Tuple<int, SheetErrorType> tuple in errorChecker.ErrorList)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = tuple.Item1 };
                DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = tuple.Item2 };
                row.Cells.Add(indexCell);
                row.Cells.Add(qtyCell);
                row.Tag = tuple;

                sheetRowList.Add(row);
            }
            
            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(sheetRowList.ToArray());
        }
        
        private void AlarmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (curErrorChecker != null)
                curErrorChecker.Reset();

            this.Hide();

            e.Cancel = true;
        }
        
        public void Popup()
        {
            IoPort ioAlarmPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanS.Screen.Device.PortMap.IoPortName.OutStop);
            if (ioAlarmPort != null)
            {
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioAlarmPort, true);
                Thread.Sleep(MonitorSetting.Instance().SignalTime);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioAlarmPort, false);
            }

            this.Show();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}