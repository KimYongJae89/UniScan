using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollector.UI
{
    public enum SystemType
    {
        Monitor, EDMS, ColorSensor, RVMS, PinHole
    }

    public partial class SettingForm : Form
    {
        string monitorPath;
        public string MonitorPath { get => monitorPath; set => monitorPath = value; }

        string edmsPath;
        public string EDMSPath { get => edmsPath; set => edmsPath = value; }

        string colorSensorPath;
        public string ColorSensorPath { get => colorSensorPath; set => colorSensorPath = value; }

        string rvmsPath;
        public string RVMSPath { get => rvmsPath; set => rvmsPath = value; }

        string pinHolePath;
        public string PinHolePath { get => pinHolePath; set => pinHolePath = value; }



        public SettingForm(string monitorPath, string edmsPath, string colorSensorPath, string rvmsPath, string pinHolePath)
        {
            InitializeComponent();

            this.MonitorPath = monitorPath;
            this.EDMSPath = edmsPath;
            this.ColorSensorPath = colorSensorPath;
            this.RVMSPath = rvmsPath;
            this.PinHolePath = pinHolePath;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            txtMonitorPath.Text = MonitorPath;
            txtEDMSPath.Text = EDMSPath;
            txtColorSensorPath.Text = ColorSensorPath;
            txtRVMSPath.Text = RVMSPath;
            txtPinHolePath.Text = PinHolePath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MonitorPath = txtMonitorPath.Text;
            EDMSPath = txtEDMSPath.Text;
            ColorSensorPath = txtColorSensorPath.Text;
            RVMSPath = txtRVMSPath.Text;
            PinHolePath = txtPinHolePath.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
