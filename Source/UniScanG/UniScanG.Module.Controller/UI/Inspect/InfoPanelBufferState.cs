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
using UniScanG.Module.Controller.Inspect;
using DynMvp.Device.Serial;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Gravure.Device;
using DynMvp.Base;

namespace UniScanG.Module.Controller.UI.Inspect
{
    public partial class InfoPanelBufferState : UserControl, IInfoPanelBufferState, IMultiLanguageSupport
    {
        SerialEncoder serialEncoder = null;
        bool encoderSpeedMeasure = false;
        MachineIfData machineIfData;

        public InfoPanelBufferState()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            StringManager.AddListener(this);

            this.serialEncoder = (SerialEncoder)SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f is SerialEncoder);
            this.machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
        }

        public void UpdateBufferState()
        {
            InspectRunnerMonitorG inspectRunner = SystemManager.Instance().InspectRunnerG as InspectRunnerMonitorG;
            if (inspectRunner != null)
            {
                List<Task> transferTaskList = inspectRunner.TransferTaskList;
                List<Task> waitTaskList = inspectRunner.WaitTaskList;

                int transferTaskCount = 0;
                int waitTaskCount = 0;

                lock (transferTaskList)
                    transferTaskCount = transferTaskList.Count();

                lock (waitTaskList)
                    waitTaskCount = waitTaskList.Count();

                DynMvp.UI.UiHelper.SetControlText(bufferUsageTransfer, string.Format("{0} / {1}", transferTaskCount, transferTaskCount + waitTaskCount));
            }
        }

        public void Clear()
        {
            DynMvp.UI.UiHelper.SetControlText(bufferUsageTransfer, "0");
        }

        public string GetLineSpdPv()
        {
            string spd = "";
            if (encoderSpeedMeasure && this.serialEncoder != null)
            {
                SerialEncoderInfo info = (SerialEncoderInfo)this.serialEncoder.DeviceInfo;
                float plsPerMs = (float)(this.serialEncoder.GetSpeedPlsPerMs());
                if (plsPerMs >= 0)
                {
                    float presentSpeedMpm = (float)(plsPerMs * info.InputResolution * 1000f * 60f / 1e6f);
                    spd = string.Format("E{0:F1}", presentSpeedMpm);
                }
            }
            else
            {
                float presentSpeedMpm = this.machineIfData.GET_PRESENT_SPEED_REAL;
                spd = presentSpeedMpm.ToString("F1");
            }
            return spd;
        }

        public string GetLineSpdSv()
        {
            return this.machineIfData.GET_TARGET_SPEED_REAL.ToString("F1");
        }

        public void TogCurLineSpdSpdType()
        {
            encoderSpeedMeasure = !encoderSpeedMeasure;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void InfoPanelBufferState_Load(object sender, EventArgs e)
        {
        }
    }
}
