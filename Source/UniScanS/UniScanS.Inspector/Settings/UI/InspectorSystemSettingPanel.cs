using System;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.UI;

namespace UniScanS.Common.Settings.Inspector.UI
{
    public partial class InspectorSystemSettingPanel : UserControl, ICustomConfigPage
    {
        public InspectorSystemSettingPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InItComponent();
        }

        public void InItComponent()
        {
            labelCamIndex.Text = StringManager.GetString(this.GetType().FullName, labelCamIndex.Text);
            labelClientIndex.Text = StringManager.GetString(this.GetType().FullName, labelClientIndex.Text);
        }

        public void UpdateData()
        {
            camIndex.Value = InspectorSystemSettings.Instance().CamIndex;
            clientIndex.Value = InspectorSystemSettings.Instance().ClientIndex;
        }

        public void SaveData()
        {
            InspectorSystemSettings.Instance().CamIndex = (int)camIndex.Value;
            InspectorSystemSettings.Instance().ClientIndex = (int)clientIndex.Value;
            
            TcpIpInfo tcpIpInfo = new TcpIpInfo(AddressManager.GetMonitorAddress(), 6000);

            InspectorSystemSettings.Instance().ClientSetting.TcpIpInfo = tcpIpInfo;

            InspectorSystemSettings.Instance().Save();
        }

        private void clientIndex_ValueChanged(object sender, EventArgs e)
        {
            if (clientIndex.Value == 0)
                groupMaster.Show();
            else
                groupMaster.Hide();
        }

        private void buttonAddInspectorInfo_Click(object sender, EventArgs e)
        {
            inspectorInfoGridView.Rows.Add("0", "");
        }

        private void buttonDeleteInspectorInfo_Click(object sender, EventArgs e)
        {
            if (inspectorInfoGridView.SelectedCells.Count == 0)
                return;

            int selectedRowIndex = inspectorInfoGridView.SelectedCells[0].RowIndex;
            inspectorInfoGridView.Rows.RemoveAt(selectedRowIndex);
        }
    }
}
