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
using DynMvp.Data.UI;

namespace UniScanS.Data.UI
{
    public partial class AlarmForm : Form
    {
        bool onUpdateData = false;
        AlarmChecker currentAlarmChecker = null;

        CanvasPanel canvasPanel;

        public AlarmForm()
        {
            InitializeComponent();

            canvasPanel = new CanvasPanel(true, System.Drawing.Drawing2D.InterpolationMode.Bilinear);
            canvasPanel.ShowToolbar = false;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.Dock = DockStyle.Fill;
            panelCanvas.Controls.Add(canvasPanel);
        }

        public void UpdateData(AlarmChecker alarmChecker)
        {
            onUpdateData = true;
            currentAlarmChecker = alarmChecker;
            type.Text = alarmChecker.AlarmType.ToString();

            List<MergeSheetResult> resultList = alarmChecker.ResultList;

            List<DataGridViewRow> sheetRowList = new List<DataGridViewRow>();

            int ngNum = 0;
            foreach (MergeSheetResult result in resultList)
            {
                if (result.Good == false)
                    ngNum++;

                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = result.Index };
                DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = result.DefectNum };
                row.Cells.Add(indexCell);
                row.Cells.Add(qtyCell);
                row.Tag = result;

                sheetRowList.Add(row);
            }

            total.Text = resultList.Count.ToString();
            ng.Text = ngNum.ToString();
            ratio.Text = string.Format("{0} %", (((float)ngNum / (float)resultList.Count) * 100.0f));

            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(sheetRowList.ToArray());

            onUpdateData = false;

            SelectSheet();
        }

        private void sheetList_SelectionChanged(object sender, EventArgs e)
        {
            SelectSheet();
        }

        private void sheetList_Click(object sender, EventArgs e)
        {
            SelectSheet();
        }

        private void SelectSheet()
        {
            if (onUpdateData == true)
                return;

            if (sheetList.SelectedRows.Count == 0)
                return;

            onUpdateData = true;

            defectList.Rows.Clear();

            int rowHeight = (defectList.Height - defectList.ColumnHeadersHeight) / 4;
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            MergeSheetResult result = (MergeSheetResult)sheetList.SelectedRows[0].Tag;

            canvasPanel.UpdateImage(result.PrevImage);
            canvasPanel.WorkingFigures.Clear();

            foreach (SheetSubResult subResult in result.SheetSubResultList)
            {
                DataGridViewRow subResultRow = new DataGridViewRow();
                subResultRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewTextBoxCell camCell = new DataGridViewTextBoxCell() { Value = subResult.CamIndex };
                subResultRow.Cells.Add(camCell);

                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = StringManager.GetString(this.GetType().FullName, subResult.DefectType.ToString()) };

                switch (subResult.DefectType)
                {
                    case DefectType.SheetAttack:
                        typeCell.Style.ForeColor = Color.Maroon;
                        break;
                    case DefectType.Pole:
                        typeCell.Style.ForeColor = Color.Red;
                        break;
                    case DefectType.Dielectric:
                        typeCell.Style.ForeColor = Color.Blue;
                        break;
                    case DefectType.PinHole:
                        typeCell.Style.ForeColor = Color.DarkMagenta;
                        break;
                    case DefectType.Shape:
                        typeCell.Style.ForeColor = Color.DarkGreen;
                        break;
                }

                subResultRow.Cells.Add(typeCell);

                subResultRow.Cells.Add(new DataGridViewTextBoxCell() { Value = subResult.ToString() });
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = subResult.Image };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                subResultRow.Cells.Add(imageCell);
                subResultRow.Height = rowHeight;
                subResultRow.Tag = subResult;

                rowList.Add(subResultRow);

                canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(10, SystemTypeSettings.Instance().ResizeRatio));
            }

            canvasPanel.Invalidate();

            defectList.Rows.AddRange(rowList.ToArray());

            onUpdateData = false;

            SelectDefect();
        }

        private void defectList_Click(object sender, EventArgs e)
        {
            SelectDefect();
        }

        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            SelectDefect();
        }

        private void SelectDefect()
        {
            if (onUpdateData == true)
                return;

            if (defectList.SelectedRows.Count == 0)
                return;

            SheetSubResult subResult = (SheetSubResult)defectList.SelectedRows[0].Tag;

            canvasPanel.TempFigures.Clear();
            canvasPanel.TempFigures.AddFigure(subResult.GetFigure(25, SystemTypeSettings.Instance().ResizeRatio));

            if (currentAlarmChecker is SamePointAlarmChecker)
                canvasPanel.TempFigures.AddFigure(subResult.GetFigure(25, SystemTypeSettings.Instance().ResizeRatio, false, AlgorithmSetting.Instance().DefectDistance));

            canvasPanel.Invalidate();
        }

        private void AlarmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentAlarmChecker != null)
            {
                currentAlarmChecker.Reset();
                currentAlarmChecker = null;
            }

            this.Hide();

            e.Cancel = true;
        }

        public void Popup()
        {
            if (MonitorSetting.Instance().UseAlarmOutput == true)
            {
                switch (currentAlarmChecker.AlarmIOType)
                {
                    case AlarmIOType.Alarm:
                        IoPort ioAlarmPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanS.Screen.Device.PortMap.IoPortName.OutAlarm);
                        if (ioAlarmPort != null)
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioAlarmPort, true);
                            Thread.Sleep(MonitorSetting.Instance().SignalTime);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioAlarmPort, false);
                        }
                        break;
                    case AlarmIOType.NG:
                        IoPort ioNgPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanS.Screen.Device.PortMap.IoPortName.OutStop);
                        if (ioNgPort != null)
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioNgPort, true);
                            Thread.Sleep(MonitorSetting.Instance().SignalTime);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioNgPort, false);
                        }
                        break;
                }
            }

            this.Show();
        }
    }
}