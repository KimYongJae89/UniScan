using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;

namespace UniScanG.Module.Inspector.Settings.Inspector.UI
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
            this.labelCamIndex.Text = StringManager.GetString(this.GetType().FullName, this.labelCamIndex.Text);
            this.labelClientIndex.Text = StringManager.GetString(this.GetType().FullName, this.labelClientIndex.Text);

            this.sheetFindVersion.DataSource = Enum.GetValues(typeof(ESheetFinderVersion));
            this.calculatorVersion.DataSource = Enum.GetValues(typeof(ECalculatorVersion));
            this.detectorVersion.DataSource = Enum.GetValues(typeof(EDetectorVersion));
            this.trainerVersion.DataSource = Enum.GetValues(typeof(ETrainerVersion));
        }

        public void UpdateData()
        {
            int camIndex = InspectorSystemSettings.Instance().CamIndex;
            int clientIndex = InspectorSystemSettings.Instance().ClientIndex;

            this.camIndex.Value = camIndex;
            this.clientIndex.Value = clientIndex;
            this.ipAddress.Text = AddressManagerG.Instance().GetInspectorAddress(camIndex, clientIndex);

            this.splitExactPattern.Checked = InspectorSystemSettings.Instance().SplitExactPattern;

            this.sheetFindVersion.SelectedItem = AlgorithmSetting.Instance().SheetFinderVersion;
            this.calculatorVersion.SelectedItem = AlgorithmSetting.Instance().CalculatorVersion;
            this.detectorVersion.SelectedItem = AlgorithmSetting.Instance().DetectorVersion;
            this.trainerVersion.SelectedItem = AlgorithmSetting.Instance().TrainerVersion;

            this.useExtSticker.Checked = AlgorithmSetting.Instance().UseExtSticker;
            this.useExtObserve.Checked = AlgorithmSetting.Instance().UseExtObserve;
            this.useExtStopImg.Checked = AlgorithmSetting.Instance().UseExtStopImg;
            this.useExtMargin.Checked = AlgorithmSetting.Instance().UseExtMargin;
            this.useExtTransform.Checked = AlgorithmSetting.Instance().UseExtTransfrom;
        }

        public bool SaveData()
        {
            InspectorSystemSettings.Instance().CamIndex = (int)this.camIndex.Value;
            InspectorSystemSettings.Instance().ClientIndex = (int)this.clientIndex.Value;
            InspectorSystemSettings.Instance().SplitExactPattern = this.splitExactPattern.Checked;

            AlgorithmSetting.Instance().SheetFinderVersion = (ESheetFinderVersion)this.sheetFindVersion.SelectedItem;
            AlgorithmSetting.Instance().CalculatorVersion = (ECalculatorVersion)this.calculatorVersion.SelectedItem;
            AlgorithmSetting.Instance().DetectorVersion = (EDetectorVersion)this.detectorVersion.SelectedItem;
            AlgorithmSetting.Instance().TrainerVersion = (ETrainerVersion)this.trainerVersion.SelectedItem;

            AlgorithmSetting.Instance().UseExtSticker = this.useExtSticker.Checked;
            AlgorithmSetting.Instance().UseExtObserve = this.useExtObserve.Checked;
            AlgorithmSetting.Instance().UseExtStopImg = this.useExtStopImg.Checked;
            AlgorithmSetting.Instance().UseExtMargin = this.useExtMargin.Checked;
            AlgorithmSetting.Instance().UseExtTransfrom = this.useExtTransform.Checked;

            bool localMode = SystemTypeSettings.Instance().LocalExchangeMode;
            TcpIpInfo tcpIpInfo = new TcpIpInfo(localMode ? "127.0.0.1" : AddressManager.Instance().GetMonitorAddress(), AddressManager.Instance().GetMonitorLinteningPort());
            InspectorSystemSettings.Instance().ClientSetting.TcpIpInfo = tcpIpInfo;

            AlgorithmSetting.Instance().Save();
            InspectorSystemSettings.Instance().Save();
            return true;
        }

        private void InspectorSystemSettingPanel_Load(object sender, EventArgs e)
        {
#if !DEBUG
            //this.groupBoxExtFunction.Enabled = false;
            this.useExtObserve.Enabled = false;
            this.useExtStopImg.Enabled = false;
            this.useExtMargin.Enabled = false;
            this.useExtTransform.Enabled = false;
#endif
        }

        private void InspectorIndex_ValueChanged(object sender, EventArgs e)
        {
            this.ipAddress.Text = AddressManagerG.Instance().GetInspectorAddress((int)camIndex.Value, (int)clientIndex.Value);

        }
    }
}
