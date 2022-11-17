using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.UI.Touch;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.UI;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;

namespace UniScanG.Module.Controller.Settings.Monitor.UI
{
    public partial class MonitorSystemSettingPanel : UserControl, ICustomConfigPage
    {
        bool onUpdateData = false;
        FovSettingForm fovSettingForm = null;

        public MonitorSystemSettingPanel()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }
        
        public void UpdateData()
        {
            if (this.onUpdateData == true)
                return;

            this.onUpdateData = true;

            this.vncViewerPath.Text = MonitorSystemSettings.Instance().VncPath;
            this.useTestbedStage.Checked = MonitorSystemSettings.Instance().UseTestbedStage;

            this.useExtSticker.Checked = Gravure.Vision.AlgorithmSetting.Instance().UseExtSticker;
            this.useExtObserve.Checked = Gravure.Vision.AlgorithmSetting.Instance().UseExtObserve;
            this.useExtStopImg.Checked = Gravure.Vision.AlgorithmSetting.Instance().UseExtStopImg;
            this.useExtMargin.Checked = Gravure.Vision.AlgorithmSetting.Instance().UseExtMargin;
            this.useExtTransform.Checked = Gravure.Vision.AlgorithmSetting.Instance().UseExtTransfrom;

            switch (MonitorSystemSettings.Instance().UseLaserBurner)
            {
                case LaserMode.None:
                    laserNone.Checked = true;
                    break;
                case LaserMode.Use:
                    laserUse.Checked = true;
                    break;
                case LaserMode.Virtual:
                    laserUseVirtual.Checked = true;
                    break;
            }
            this.useStickerSensor.Checked = MonitorSystemSettings.Instance().UseStickerSensor;
            this.enableImPowCon.Checked = MonitorSystemSettings.Instance().EnableImsPowControl;

            this.InspectorInfoGridView.Rows.Clear();

            foreach (InspectorInfo inspectorInfo in MonitorSystemSettings.Instance().InspectorInfoList)
            {
                int index = this.InspectorInfoGridView.Rows.Add(inspectorInfo.Use, inspectorInfo.CamIndex, inspectorInfo.ClientIndex, inspectorInfo.Path, inspectorInfo.UserId, inspectorInfo.UserPw);
                this.InspectorInfoGridView.Rows[index].Tag = inspectorInfo;
            }
            this.InspectorInfoGridView.AutoResizeColumns();

            this.onUpdateData = false;
        }

        //private void buttonAddInspectorInfo_Click(object sender, EventArgs e)
        //{
        //    InspectorInfoGridView.Rows.Add("0", "0", "", "Administrator", "");
        //}

        //private void buttonDeleteInspectorInfo_Click(object sender, EventArgs e)
        //{
        //    if (InspectorInfoGridView.SelectedCells.Count == 0)
        //        return;

        //    int selectedRowIndex = InspectorInfoGridView.SelectedCells[0].RowIndex;
        //    InspectorInfoGridView.Rows.RemoveAt(selectedRowIndex);
        //}

        public bool SaveData()
        {
            MonitorSystemSettings.Instance().VncPath = vncViewerPath.Text;
            MonitorSystemSettings.Instance().UseTestbedStage = useTestbedStage.Checked;
            MonitorSystemSettings.Instance().UseStickerSensor = useStickerSensor.Checked;
            MonitorSystemSettings.Instance().EnableImsPowControl = this.enableImPowCon.Checked;

            MonitorSystemSettings.Instance().UseLaserBurner = laserNone.Checked ? LaserMode.None : laserUse.Checked ? LaserMode.Use : LaserMode.Virtual;
            Gravure.Vision.AlgorithmSetting.Instance().UseExtSticker = this.useExtSticker.Checked;
            Gravure.Vision.AlgorithmSetting.Instance().UseExtObserve = this.useExtObserve.Checked;
            Gravure.Vision.AlgorithmSetting.Instance().UseExtStopImg = this.useExtStopImg.Checked;
            Gravure.Vision.AlgorithmSetting.Instance().UseExtMargin = this.useExtMargin.Checked;
            Gravure.Vision.AlgorithmSetting.Instance().UseExtTransfrom = this.useExtTransform.Checked;

            bool localMode = SystemTypeSettings.Instance().LocalExchangeMode;
            TcpIpInfo tcpIpInfo = new TcpIpInfo(localMode ? "127.0.0.1" : AddressManager.Instance().GetMonitorAddress(), AddressManager.Instance().GetMonitorLinteningPort());
            MonitorSystemSettings.Instance().ServerSetting.TcpIpInfo = tcpIpInfo;

            List<InspectorInfo> inspectorInfoList = MonitorSystemSettings.Instance().InspectorInfoList;
            inspectorInfoList.Clear();

            foreach (DataGridViewRow row in InspectorInfoGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool isEmpty = string.IsNullOrEmpty(row.Cells[1].Value?.ToString()) || string.IsNullOrEmpty(row.Cells[2].Value?.ToString());
                if (isEmpty)
                    continue;

                bool use = (bool)(row.Cells[0].Value ?? false);
                int camIndex = int.Parse(row.Cells[1].Value.ToString());
                int clientIndex = int.Parse(row.Cells[2].Value.ToString());
                string address = localMode ? "127.0.0.1" : AddressManager.Instance().GetInspectorAddress(camIndex, clientIndex);
                string path = row.Cells[3].Value?.ToString();
                string userId = row.Cells[4].Value?.ToString();
                string userPw = row.Cells[5].Value?.ToString();

                InspectorInfo inspectorInfo = (row.Tag as InspectorInfo ?? new InspectorInfo());
                inspectorInfo.Use = use;
                inspectorInfo.CamIndex = camIndex;
                inspectorInfo.ClientIndex = clientIndex;
                inspectorInfo.IpAddress = address;
                inspectorInfo.Path = string.IsNullOrEmpty(path) ? Path.Combine(@"\\", address, "UniScan", "Gravure_Inspector") : path;
                inspectorInfo.UserId = string.IsNullOrEmpty(userId) ? inspectorInfo.UserId : userId;
                inspectorInfo.UserPw = userPw == null ? inspectorInfo.UserPw : userPw;

                inspectorInfoList.Add(inspectorInfo);
            }

            inspectorInfoList.Sort((f, g) =>
            {
                int comp = f.CamIndex.CompareTo(g.CamIndex);
                if (comp == 0)
                    return f.ClientIndex.CompareTo(g.ClientIndex);
                return comp;
                //return f.GetName().CompareTo(g.GetName());
            });

            MonitorSystemSettings.Instance().Save();
            Gravure.Vision.AlgorithmSetting.Instance().Save();

            return true;
        }

        private void buttonFov_Click(object sender, EventArgs e)
        {
            //if (MessageForm.Show(this, "Save Changes?", MessageFormType.YesNo) == DialogResult.No)
            //    return;

            SaveData();

            List<InspectorInfo> inspectorInfoList = MonitorSystemSettings.Instance().InspectorInfoList;
            fovSettingForm = new FovSettingForm(inspectorInfoList);
            fovSettingForm.ShowDialog(this);
            if (fovSettingForm.DialogResult == DialogResult.OK)
            // Client==0 인 객체의 FOV를 Client!=0인 객체에게 전파
            {
                List<InspectorInfo> inspectorInfos = inspectorInfoList.FindAll(f => f.ClientIndex <= 0);
                foreach (InspectorInfo inspectorInfo in inspectorInfos)
                    inspectorInfoList.FindAll(f => f.CamIndex == inspectorInfo.CamIndex).ForEach(f => f.Fov = inspectorInfo.Fov);
                SaveData();
            }
            fovSettingForm.Dispose();
        }

        private void MonitorSystemSettingPanel_Load(object sender, EventArgs e)
        {
#if !DEBUG
            //this.groupBoxExtFunction.Enabled = false;
            this.useExtObserve.Enabled = false;
            this.useExtStopImg.Enabled = false;
            this.useExtMargin.Enabled = false;
            this.useExtTransform.Enabled = false;
#endif
        }

        private void MonitorSystemSettingPanel_SizeChanged(object sender, EventArgs e)
        {
        }
    }
}
