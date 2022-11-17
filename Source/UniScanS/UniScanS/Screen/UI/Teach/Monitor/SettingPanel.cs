using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using System.Reflection;
using DynMvp.Base;
using UniScanS.Screen.Inspect;
using UniScanS.Screen.Data;
using UniScanS.Data.UI;
using UniScanS.Data;
using DynMvp.Authentication;

namespace UniScanS.Screen.UI.Teach.Monitor
{
    public partial class SettingPanel : UserControl, IMultiLanguageSupport, IModelListener, IUserHandlerListener
    {
        bool onUpdateData = false;

        public SettingPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);

            UserChanged();
            UpdateData();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonTurn_Click(object sender, EventArgs e)
        {

        }

        private void UpdateData()
        {
            useAlarmOutput.Checked = MonitorSetting.Instance().UseAlarmOutput;
            signalTime.Value = MonitorSetting.Instance().SignalTime;
            errorNum.Value = MonitorSetting.Instance().ErrorNum;
            UpdateAlarmList();
        }

        private void lightValue_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            Model model = SystemManager.Instance().CurrentModel;
            if (model == null)
                return;

            model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)topLightValue.Value;
            model.LightParamSet.LightParamList[0].LightValue.Value[1] = (int)topLightValue.Value;
            model.LightParamSet.LightParamList[0].LightValue.Value[2] = (int)topLightValue.Value;

            if (SystemManager.Instance().InspectRunner is InspectRunnerMonitorS)
            {
                InspectRunnerMonitorS inspectRunner = (InspectRunnerMonitorS)SystemManager.Instance().InspectRunner;
                inspectRunner.UpdateLightCtrl(true);
            }

            SystemManager.Instance().ModelManager.SaveModel(model);
        }

        private void bottomLightValue_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            Model model = SystemManager.Instance().CurrentModel;

            if (model == null)
                return;

            model.LightParamSet.LightParamList[0].LightValue.Value[3] = (int)bottomLightValue.Value;

            if (SystemManager.Instance().InspectRunner is InspectRunnerMonitorS)
            {
                InspectRunnerMonitorS inspectRunner = (InspectRunnerMonitorS)SystemManager.Instance().InspectRunner;
                inspectRunner.UpdateLightCtrl(true);
            }

            SystemManager.Instance().ModelManager.SaveModel(model);
        }

        public void ModelChanged()
        {
            onUpdateData = true;

            Model model = SystemManager.Instance().CurrentModel;
            if (model != null)
            {
                topLightValue.Value = model.LightParamSet.LightParamList[0].LightValue.Value[0];
                bottomLightValue.Value = model.LightParamSet.LightParamList[0].LightValue.Value[3];
            }

            onUpdateData = false;
        }

        public void ModelTeachDone()
        {

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            MasterSettingForm masterSettingForm = new MasterSettingForm();
            masterSettingForm.ShowDialog();
        }

        private void UpdateAlarmList()
        {
            alarmCheckerGridView.Rows.Clear();

            int index = 1;

            foreach (AlarmChecker alarmChecker in MonitorSetting.Instance().AlarmCheckerList)
            {
                int rowIndex = alarmCheckerGridView.Rows.Add(index, alarmChecker.AlarmType, alarmChecker.AlarmIOType.ToString());
                alarmCheckerGridView.Rows[rowIndex].Tag = alarmChecker;
                foreach (DataGridViewCell cell in alarmCheckerGridView.Rows[rowIndex].Cells)
                    cell.ToolTipText = alarmChecker.ToString();

                index++;
            }
        }

        private void buttonAddDefectOutput_Click(object sender, EventArgs e)
        {
            AlarmCheckerForm alarmCheckerForm = new AlarmCheckerForm();
            if (alarmCheckerForm.ShowDialog() == DialogResult.OK)
            {
                AlarmChecker alarmChecker = alarmCheckerForm.CurAlarmChecker;
                MonitorSetting.Instance().AlarmCheckerList.Add(alarmChecker);
            }

            MonitorSetting.Instance().Save();
            UpdateAlarmList();
        }

        private void buttonDeleteDefectOutput_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in alarmCheckerGridView.SelectedRows)
            {
                MonitorSetting.Instance().AlarmCheckerList.Remove((AlarmChecker)row.Tag);
            }

            MonitorSetting.Instance().Save();
            UpdateAlarmList();
        }

        private void useDefectOutput_CheckedChanged(object sender, EventArgs e)
        {
            MonitorSetting.Instance().UseAlarmOutput = useAlarmOutput.Checked;
            MonitorSetting.Instance().Save();
        }

        private void signalTime_ValueChanged(object sender, EventArgs e)
        {
            MonitorSetting.Instance().SignalTime = (int)signalTime.Value;
            MonitorSetting.Instance().Save();
        }

        private void buttonEditDefectOutput_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in alarmCheckerGridView.SelectedRows)
            {
                AlarmCheckerForm alarmCheckerForm = new AlarmCheckerForm((AlarmChecker)row.Tag);
                if (alarmCheckerForm.ShowDialog() == DialogResult.OK)
                {
                    MonitorSetting.Instance().Save();
                }
            }

            UpdateAlarmList();
        }

        private void errorNum_ValueChanged(object sender, EventArgs e)
        {
            MonitorSetting.Instance().ErrorNum = (int)errorNum.Value;
            MonitorSetting.Instance().Save();
        }

        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            layoutMaster.Visible = UserHandler.Instance().CurrentUser.IsSuperAccount;
        }
    }
}
