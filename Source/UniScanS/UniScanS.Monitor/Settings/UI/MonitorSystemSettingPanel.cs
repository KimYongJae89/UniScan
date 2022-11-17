using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DynMvp.Base;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.UI;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.UI;

namespace UniScanS.Common.Settings.Monitor.UI
{
    public partial class MonitorSystemSettingPanel : UserControl, ICustomConfigPage
    {
        bool onUpdateData = false;
        FovSettingForm fovSettingForm = null;
        
        public MonitorSystemSettingPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitComponent();
        }

        public void InitComponent()
        {
            labelVncViewerPath.Text = StringManager.GetString(this.GetType().FullName, labelVncViewerPath.Text);
        }

        public void UpdateData()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;
            
            vncViewerPath.Text = MonitorSystemSettings.Instance().VncPath;

            inspectorInfoGridView.Rows.Clear();

            foreach (InspectorInfo inspectorInfo in MonitorSystemSettings.Instance().InspectorInfoList)
            {
                int index = inspectorInfoGridView.Rows.Add(inspectorInfo.CamIndex, inspectorInfo.Path);
                inspectorInfoGridView.Rows[index].Tag = inspectorInfo;
            }
                
            
            onUpdateData = false;
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

        public void SaveData()
        {
            MonitorSystemSettings.Instance().VncPath = vncViewerPath.Text;

            MonitorSystemSettings.Instance().InspectorInfoList.Clear();

            foreach (DataGridViewRow row in inspectorInfoGridView.Rows)
            {
                bool isEmpty = false;
                foreach (DataGridViewTextBoxCell cell in row.Cells)
                {
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()) == true)
                    {
                        isEmpty = true;
                        break;
                    }
                }

                if (isEmpty == true)
                    continue;

                InspectorInfo inspectorInfo = new InspectorInfo();
                inspectorInfo.CamIndex = int.Parse(row.Cells[0].Value.ToString());
                inspectorInfo.Path = (string)row.Cells[1].Value;
                
                inspectorInfo.Address = AddressManager.GetInspectorAddress(inspectorInfo.CamIndex, 0);
                if (row.Tag != null)
                    inspectorInfo.Fov = ((InspectorInfo)row.Tag).Fov;

                MonitorSystemSettings.Instance().InspectorInfoList.Add(inspectorInfo);
            }

            MonitorSystemSettings.Instance().Save();
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            MachineIfForm form = new MachineIfForm(MonitorSystemSettings.Instance().ServerSetting);
            form.ShowDialog();
        }

        private void buttonFov_Click(object sender, EventArgs e)
        {
            List<InspectorInfo> inspectorInfoList = new List<InspectorInfo>();

            foreach (DataGridViewRow row in inspectorInfoGridView.Rows)
            {
                InspectorInfo inspectorInfo = null;

                if (row.Tag != null)
                    inspectorInfo = (InspectorInfo)row.Tag;
                else
                {
                    bool isEmpty = false;
                    foreach (DataGridViewTextBoxCell cell in row.Cells)
                    {
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()) == true)
                        {
                            isEmpty = true;
                            break;
                        }
                    }

                    if (isEmpty == true)
                        continue;

                    inspectorInfo.CamIndex = int.Parse(row.Cells[0].Value.ToString());
                
                    row.Tag = inspectorInfo;
                }

                inspectorInfoList.Add(inspectorInfo);
            }

            fovSettingForm = new FovSettingForm(inspectorInfoList);
            fovSettingForm.ShowDialog();

            if (fovSettingForm.DialogResult == DialogResult.OK)
            {
                foreach (InspectorInfo info in inspectorInfoList)
                {
                    
                    foreach (DataGridViewRow row in inspectorInfoGridView.Rows)
                    {
                        if (row.Tag == null)
                            continue;
                        
                        InspectorInfo inspectorInfo = (InspectorInfo)row.Tag;
                        if (inspectorInfo.CamIndex == info.CamIndex)
                            inspectorInfo.Fov = inspectorInfo.Fov;
                    }
                }
            }
        }
    }
}
