using System;
using System.Windows.Forms;

namespace UniScanG.Common.Monitor.UI
{
    enum MonitoringPanelMode
    {
        Grab, Defect
    }

    public partial class MonitoringPanel : UserControl
    {
        MonitoringPanelMode panelMode = MonitoringPanelMode.Grab;

        DefectPanel defectPanel;
        //GrabImagePanel

        public MonitoringPanel()
        {
            InitializeComponent();
        }

        private void defect_Click(object sender, EventArgs e)
        {
            panelMode = MonitoringPanelMode.Defect;
        }

        private void UpdatePanel()
        {
            panelView.Controls.Clear();
            switch (panelMode)
            {
                case MonitoringPanelMode.Grab:
                    panelView.Controls.Add(defectPanel);
                    break;
            }

            panelView.Invalidate();
        }
    }
}
