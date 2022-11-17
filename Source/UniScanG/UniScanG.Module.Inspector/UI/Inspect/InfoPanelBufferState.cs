using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.UI.Inspect;
using UniScanG.Module.Inspector.Inspect;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Base;
using UniScanG.Gravure.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Data;
using Infragistics.Win.UltraWinProgressBar;

namespace UniScanG.Module.Inspector.UI.Inspect
{
    public partial class InfoPanelBufferState : UserControl, IInfoPanelBufferState, IMultiLanguageSupport
    {
        DateTime lastDateTime = DateTime.MinValue;
        List<UltraProgressBar> progressBarList = new List<UltraProgressBar>();

        public InfoPanelBufferState()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            StringManager.AddListener(this);
        }

        public void Clear()
        {
            UiHelper.SetUltraProgreessBarValue(bufferUsageGrab, 0, true);
            UiHelper.SetUltraProgreessBarValue(bufferUsageInsp, 0, true);
            UiHelper.SetControlText(bufferUsageTransfer, "0");
            UiHelper.SetUltraProgreessBarValue(loadUsagePrep, 0, false);
        }

        public void UpdateBufferState()
        {
            InspectRunnerInspectorG inspectRunner = SystemManager.Instance().InspectRunner as InspectRunnerInspectorG;
            if (inspectRunner == null)
                return;

            int? grabBufferLength = SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList.FirstOrDefault()?.GetImageBufferCount();
            if (grabBufferLength.HasValue)
                UiHelper.SetUltraProgreessBarMinMax(bufferUsageGrab, 0, grabBufferLength.Value);

            int grabBufferUsage = inspectRunner.GrabProcesser.GetBufferCount();
            UiHelper.SetUltraProgreessBarValue(bufferUsageGrab, grabBufferUsage, true);

            int inspBufferLength = inspectRunner.ProcessBufferManager.Count;
            UiHelper.SetUltraProgreessBarMinMax(bufferUsageInsp, 0, inspBufferLength);

            int inspBufferUsage = inspectRunner.ProcessBufferManager.UsingCount;
            UiHelper.SetUltraProgreessBarValue(bufferUsageInsp, inspBufferUsage, true);

            int waitBufferCnt = inspectRunner.GrabbedQueue.Count;
            UiHelper.SetControlText(bufferUsageTransfer, string.Format("{0}", waitBufferCnt));

            {
                BufferUploadThread thread = inspectRunner.BufferUploadThread;
                if (thread != null && thread.IsRunning)
                {
                    float factor = thread.GetLoadFactor();
                    UiHelper.SetUltraProgreessBarValue(loadUsagePrep, (int)Math.Round(factor * 1000f), false);
                }

                int units = inspectRunner.InspectUnitManager.Count;
                tableLayoutPanel1.RowCount = units + 4;
                if (tableLayoutPanel1.RowCount > tableLayoutPanel1.RowStyles.Count)
                {
                    tableLayoutPanel1.RowStyles.Clear();
                    for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                }

                List<UltraProgressBar> list = new List<UltraProgressBar>(this.progressBarList);
                for (int i = 0; i < units; i++)
                {
                    InspectUnit unit = inspectRunner.InspectUnitManager.ElementAtOrDefault(i);
                    if (unit == null)
                        continue;

                    UltraProgressBar bar = this.progressBarList.Find(f => f.Name == unit.Name);
                    if (bar == null)
                        bar = AddUltraProgressBar(unit.Name, i + 4);

                    float factor = unit.GetLoadFactor();
                    UiHelper.SetUltraProgreessBarValue(bar, (int)Math.Round(factor * 1000f), false);
                    list.Remove(bar);
                }
                list.ForEach(f => UiHelper.SetUltraProgreessBarValue(f, 0, false));

                if (SystemState.Instance().IsInspectOrWait)
                {
                    DateTime now = DateTime.Now;
                    if ((now - lastDateTime).TotalSeconds > 1)
                        // 1초에 한번씩
                    {
                        InspectorOperator inspectorOperator = (InspectorOperator)SystemManager.Instance().ExchangeOperator;
                        int camera = inspectorOperator.GetCamIndex();
                        int client = inspectorOperator.GetClientIndex();
                        float loadFactor = inspectRunner.GetLoadFactor();
                        SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.I_LOADFACTOR, camera.ToString(), client.ToString(), loadFactor.ToString());
                        lastDateTime = now;
                    }
                }
            }
        }

        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar AddUltraProgressBar(string name, int row)
        {
            Label label = new Label()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0),
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Text = name,
            };
            tableLayoutPanel1.Controls.Add(label, 0, row);

            UltraProgressBar bar = new UltraProgressBar()
            {
                Name = name,
                Minimum = 0,
                Maximum = 1000,
                Value = 0,
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
            };
            tableLayoutPanel1.Controls.Add(bar, 1, row);
            progressBarList.Add(bar);
            return bar;
        }

        //private delegate void UpdateUltraProgreessBarDelegate(Infragistics.Win.UltraWinProgressBar.UltraProgressBar progressBar, int maxValue, float curValue);
        //private void UpdateUltraProgreessBar(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int maxValue, float curValue)
        //{
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new UpdateUltraProgreessBarDelegate(UpdateUltraProgreessBar), ultraProgressBar, maxValue, curValue);
        //        return;
        //    }

        //    if (maxValue > 0)
        //    {
        //        ultraProgressBar.Maximum = maxValue;
        //        ultraProgressBar.Value = (int)Math.Min(curValue, maxValue);

        //        float load = (maxValue == 0) ? 0 : curValue * 100.0f / maxValue;
        //        ultraProgressBar.Text = string.Format("{0:0.0}% ({1}/{2})", load, curValue, maxValue);
        //    }
        //    else
        //    {
        //        maxValue = ultraProgressBar.Maximum;
        //        ultraProgressBar.Value = (int)Math.Min(curValue, maxValue);

        //        float load = (maxValue == 0) ? 0 : curValue * 100.0f / maxValue;
        //        ultraProgressBar.Text = string.Format("{0:0.0}%", load);
        //    }
        //}

        public string GetLineSpdPv()
        {
            string spd = "";
            DynMvp.Devices.ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList.FirstOrDefault();
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (imageDevice != null && calibration != null)
            {
                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                float grabLinesPerSec = imageDevice.GrabPerSec * imageDevice.ImageSize.Height;
                double umPerSec = grabLinesPerSec * pelSize.Height;
                float presentSpeedMpm = (float)(umPerSec / 1000 / 1000 * 60);
                spd = presentSpeedMpm.ToString("F1");
            }

            return spd;
        }

        public void TogCurLineSpdSpdType()
        {
   
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public string GetLineSpdSv()
        {
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            return productionG?.LineSpeedMpm.ToString("F1");
        }
    }
}
