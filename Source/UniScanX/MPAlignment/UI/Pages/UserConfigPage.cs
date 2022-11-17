using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanX.MPAlignment.Settings;
using DynMvp.Devices.UI;
using DynMvp.UI.Touch;
using DynMvp.Devices.MotionController;

namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class UserConfigPage : UserControl
    {
        public UserConfigPage()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            //nudLightStableTime.Value = OperationConfig.Instance.Time.StatbleTime;

            ////컨베이어
            //nudBeforMachineToEntry.Value = OperationConfig.Instance.ConveyorTimeOut.BeforeToEntryTimeout;
            //nudEntryToSlowDown.Value = OperationConfig.Instance.ConveyorTimeOut.EntryToSlowDownTimeout;
            //nudSlowdownToInspectReady.Value = OperationConfig.Instance.ConveyorTimeOut.SlowDownToInspectReadyTimeout;
            //nudInspectReadyToEject.Value = OperationConfig.Instance.ConveyorTimeOut.InsepctReadyToEjectTimeout;
            //nudEjectToNextMachine.Value = OperationConfig.Instance.ConveyorTimeOut.EjectToNextMachineTimeout;
            //nudFlushTimeout.Value = OperationConfig.Instance.ConveyorTimeOut.FlushTimeout;
            //tgsReviewMode.Checked = OperationConfig.Instance.UseReviewMode;
            //tgsUseSmema.Checked = OperationConfig.Instance.UseSMEMA;

            //txtResultPath.Text = UserConfig.Instance.UserPaths.ResultPath;
        }

        void Save()
        {
            //OperationConfig.Instance.Time.StatbleTime = (int)nudLightStableTime.Value;

            ////컨베이어
            //OperationConfig.Instance.ConveyorTimeOut.BeforeToEntryTimeout = (int)nudBeforMachineToEntry.Value;
            //OperationConfig.Instance.ConveyorTimeOut.EntryToSlowDownTimeout = (int)nudEntryToSlowDown.Value;
            //OperationConfig.Instance.ConveyorTimeOut.SlowDownToInspectReadyTimeout = (int)nudSlowdownToInspectReady.Value;
            //OperationConfig.Instance.ConveyorTimeOut.InsepctReadyToEjectTimeout = (int)nudInspectReadyToEject.Value;
            //OperationConfig.Instance.ConveyorTimeOut.EjectToNextMachineTimeout = (int)nudEjectToNextMachine.Value;
            //OperationConfig.Instance.ConveyorTimeOut.FlushTimeout = (int)nudFlushTimeout.Value;

            //OperationConfig.Instance.UseReviewMode = tgsReviewMode.Checked;
            //OperationConfig.Instance.UseSMEMA = tgsUseSmema.Checked;

            //OperationConfig.Instance.Save(SystemConfig.Instance.ConfigPath);

            //UserConfig.Instance.UserPaths.ResultPath = txtResultPath.Text;
            //UserConfig.Instance.Save(SystemConfig.Instance.ConfigPath);
        }

        private void btnApplyConfig_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnResultSet_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtResultPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button_MotionSetting_Click(object sender, EventArgs e)
        {
            AxisConfiguration axisConfiguration = SystemManager.Instance().DeviceBox.AxisConfiguration;
            if (axisConfiguration.Count == 0)
            {
                MessageForm.Show(null, "There is no Axis");
                return;
            }
            //((MainForm)SystemManager.Instance().MainForm).ChangeStartMode(StartMode.Stop);
            MotionControlForm motionControlForm = new MotionControlForm();
            motionControlForm.Intialize(axisConfiguration);
            motionControlForm.Show();
        }
    }
}
