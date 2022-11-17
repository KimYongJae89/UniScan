using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Operation;
using DynMvp.Base;
using DynMvp.UI;
using UniEye.Base.MachineInterface;
using UniScanM.Settings;

namespace UniScanM.UI
{
    public partial class PLCStatusPanel : UserControl
    {
        public bool IsGlossOnly { get; set; }

        Color offColor;
        public PLCStatusPanel()
        {
            InitializeComponent();

            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            this.Visible = machineIf != null;
            offColor = Color.LightGray;
        }

        private void PLCStatusPanel_Load(object sender, EventArgs e)
        {
            this.labelConnected.Visible = true;
            this.labelPinhole.Visible = !IsGlossOnly;
            this.labelRVMS.Visible = !IsGlossOnly;
            this.labelColorSensor.Visible = !IsGlossOnly;
            this.labelEDMS.Visible = !IsGlossOnly;
            this.labelCEDMS.Visible = !IsGlossOnly;
            this.labelStopImageOn.Visible = !IsGlossOnly;
            this.labelGloss.Visible = IsGlossOnly;
            this.labelRewinderCut.Visible = true;
            this.labelSPSpeed.Visible = true;
            this.labelPVSpeed.Visible = true;
            this.labelPVPos.Visible = true;

            this.labelModel.Visible = true;
            this.labelLot.Visible = true;
            this.labelWorker.Visible = true;
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {
            if (SystemManager.Instance().InspectStarter != null && SystemManager.Instance().InspectStarter is PLCInspectStarter)
            {
                MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor?.StateCopy;
                if (state == null)
                {
                    LogHelper.Debug(LoggerType.Operation, "Melsec machine state is null.");
                    return;
                }

                UpdateOnState(state);
                UpdateParameter(state);
            }
        }

        void UpdateOnState(MachineState state)
        {
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null)
                return;
            
            //labelConnected.BackColor = SystemManager.Instance().DeviceBox.MachineIf.IsConnected == true  ? Color.Green : offColor;
            labelConnected.BackColor = state.IsConnected == true  ? Color.Green : offColor;
            labelConnected.Text = state.IsConnected == true ? "Connected" : "Disconnected";

            //state.Toggle();

            labelPinhole.BackColor = state.PinholeOnStart == true ? Color.LightGreen : offColor;
            labelRVMS.BackColor = state.RvmsOnStart == true ? Color.LawnGreen : offColor;
            labelColorSensor.BackColor = state.ColorSensorOnStart == true ? Color.LimeGreen : offColor;
            labelEDMS.BackColor = state.EdmsOnStart == true ? Color.SpringGreen : offColor;
            labelCEDMS.BackColor = state.CedmsOnStart == true ? Color.SpringGreen : offColor;
            labelStopImageOn.BackColor = state.StillImageOnStart == true ? Color.Lime : offColor;
            labelGloss.BackColor = state.GlossOnStart == true ? Color.LawnGreen : offColor;

            // 리와인더 컷
            labelRewinderCut.BackColor = state.RewinderCut == true ? Color.YellowGreen : offColor;

            // 기타 정보
            labelModel.Text = string.Format(" {0} ", state.ModelName);
            labelLot.Text = string.Format(" {0} ", state.LotNo);
            labelWorker.Text = string.Format(" {0} ", state.Worker);
        }

        void UpdateParameter(MachineState state)
        {
            labelSPSpeed.Text = string.Format("SP Spd : {0:F1} m/min", state.SpSpeed);
            labelPVSpeed.Text = string.Format("PV Spd : {0:F1} m/min", state.PvSpeed);
            labelPVPos.Text = string.Format("PV Pos : {0:F0} m", state.PvPosition);

            Color bgColor = Control.DefaultBackColor;
            if (state.SpSpeed > UniScanMSettings.Instance().MaximumLineSpeed)
                bgColor = labelSPSpeed.BackColor == Control.DefaultBackColor ? Color.OrangeRed : Control.DefaultBackColor;
            labelSPSpeed.BackColor = bgColor;
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            IVirtualMachineIf virtualMachineIf = SystemManager.Instance().DeviceBox.MachineIf as IVirtualMachineIf;
            if (virtualMachineIf == null)
                return;

            MachineState state = null;
            if (SystemManager.Instance().InspectStarter != null && SystemManager.Instance().InspectStarter is PLCInspectStarter)
            {
                 state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.State;
                if (state == null) return;
            }
        
            ToolStripItem selItem = e.ClickedItem;

            if (selItem.Name == labelConnected.Name) { virtualMachineIf.SetStateConnect(!state.IsConnected); }
            else if (selItem.Name == labelStopImageOn.Name) { state.StillImageOnStart = !state.StillImageOnStart; }
            else if (selItem.Name == labelColorSensor.Name) { state.ColorSensorOnStart = !state.ColorSensorOnStart; }
            else if (selItem.Name == labelEDMS.Name) { state.EdmsOnStart = !state.EdmsOnStart; }
            else if (selItem.Name == labelCEDMS.Name) { state.CedmsOnStart = !state.CedmsOnStart; }
            else if (selItem.Name == labelGloss.Name) { state.GlossOnStart = !state.GlossOnStart; }
            else if (selItem.Name == labelPinhole.Name) { state.PinholeOnStart = !state.PinholeOnStart; }
            else if (selItem.Name == labelRVMS.Name) { state.RvmsOnStart = !state.RvmsOnStart; }
            else if(selItem.Name == labelPVSpeed.Name|| selItem.Name == labelSPSpeed.Name|| selItem.Name == labelPVPos.Name)
            {
                double newValue = 0.0f;
                if (selItem.Name == labelSPSpeed.Name) { newValue = state.SpSpeed; }
                else if (selItem.Name == labelPVSpeed.Name) { newValue = state.PvSpeed; }
                else if (selItem.Name == labelPVPos.Name) { newValue = state.PvPosition; }

                InputForm inputForm = new InputForm(StringManager.GetString("Enter New Value"), newValue.ToString("F1"));
                inputForm.ValidCheckFunc += new InputFormValidCheckFunc(f => double.TryParse(f, out newValue));
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    if (selItem.Name == labelSPSpeed.Name)      { state.SpSpeed = newValue; state.PvSpeed = newValue; }
                    else if (selItem.Name == labelPVSpeed.Name) { state.SpSpeed = newValue; state.PvSpeed = newValue; }
                    else if (selItem.Name == labelPVPos.Name) { state.PvPosition = (int)Math.Round(newValue); }
                }
            }
            else if (selItem.Name == labelRewinderCut.Name) { state.RewinderCut = !state.RewinderCut; state.PvPosition = 0; }
            else if (selItem.Name == labelModel.Name || selItem.Name == labelLot.Name || selItem.Name == labelWorker.Name)
            {
                string newValue = "";
                if (selItem.Name == labelModel.Name) { newValue = state.ModelName; }
                else if (selItem.Name == labelLot.Name) { newValue = state.LotNo; }
                else if (selItem.Name == labelWorker.Name) { newValue = state.Worker; }

                InputForm inputForm = new InputForm(StringManager.GetString("Enter New Value"), newValue);
                inputForm.ValidCheckFunc += new InputFormValidCheckFunc(f => f.Contains('\\') == false);
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    newValue = inputForm.InputText;
                    if (selItem.Name == labelModel.Name) { state.ModelName = newValue; }
                    else if (selItem.Name == labelLot.Name) { state.LotNo = newValue; }
                    else if (selItem.Name == labelWorker.Name) { state.Worker = newValue; }
                }
            }

            //revision data

        }
    }
}
