using DynMvp.Base;
using Infragistics.Win.UltraWinTabControl;
using MLCCS.Operation.UI.Inspector;
using MLCCS.Operation.UI.Monitor;
using MLCCS.Settings;
using SamsungElectro.Operation.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base;
using UniEye.Base.Data;

namespace MLCCS.Operation.UI.Monitor
{
    public class MonitoringUIController : UIController
    {
        protected override void InitPartialComponent()
        {
            remoteTeachingPage = new RemoteTeachingPage();

            remoteTeachingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            remoteTeachingPage.Dock = DockStyle.Fill;
            remoteTeachingPage.Location = new System.Drawing.Point(0, 313);
            remoteTeachingPage.Name = "Remote Teaching Page";
            remoteTeachingPage.Size = new System.Drawing.Size(466, 359);
            remoteTeachingPage.TabIndex = 0;

            tabMain.Tabs[GetTabName(MainTabKey.Teach)].TabPage.Controls.Add(remoteTeachingPage);
            
            LogHelper.Debug(LoggerType.StartUp, "Init Remote Teaching Page.");

            monitoringPage = new MLCCSMonitoringPage();

            monitoringPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            monitoringPage.Dock = DockStyle.Fill;
            monitoringPage.Location = new System.Drawing.Point(0, 313);
            monitoringPage.Name = "Monitoring Page";
            monitoringPage.Size = new System.Drawing.Size(466, 359);
            monitoringPage.TabIndex = 0;

            tabMain.Tabs[GetTabName(MainTabKey.Inspect)].TabPage.Controls.Add(monitoringPage);

            LogHelper.Debug(LoggerType.StartUp, "Init Monitoring Page.");

            LogHelper.Debug(LoggerType.StartUp, "End Monitoring Component.");
        }

        private void CheckTraind()
        {
            if (SystemManager.Instance().CurrentModel == null || SystemManager.Instance().CurrentModel.IsEmpty() == true)
            {
                UpdateTab(false);
                return;
            }

            bool allTrained = true;
            foreach (InspectorObj inspector in MonitoringManager.Instance().InspectorList)
            {
                string modelPath = System.IO.Path.Combine(inspector.Info.Path, "Model", SystemManager.Instance().CurrentModel.Name);

                bool trained = true;
                
                if (inspector.State != InspectorState.Disconnected)
                    trained = MLCCSUtil.Instance().IsTrained(modelPath);

                if (trained == false)
                {
                    allTrained = false;
                    break;
                }
            }
            
            UpdateTab(allTrained);
        }

        private bool IsNeedSync(string key)
        {
            bool need = key == MLCCSUtil.Instance().GetTabKey(MainTabKey.Model) ||
                key == MLCCSUtil.Instance().GetTabKey(MainTabKey.Teach) ||
                key == MLCCSUtil.Instance().GetTabKey(MainTabKey.Inspect);

            return need;
        }

        public void StartClient(int clientNo)
        {
            if (SystemState.Instance().OnInspectOrWaitOrPause)
                MonitoringPage.StopInspection();

            SystemManager.Instance().CurrentModel = null;

            CheckTraind();

            ChangeTab(MainTabKey.Model);
        }

        public override void SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "Start SelectedTabChanged");

            string opString = string.Format("({0}) => ({1})", e.PreviousSelectedTab?.Key, e.Tab.Key);
            LogHelper.Info(LoggerType.Operation, opString);

            if (e.PreviousSelectedTab != null)
                TabVisibleChanged(e.PreviousSelectedTab.TabPage, false);

            TabVisibleChanged(e.Tab.TabPage, true);

            if (IsNeedSync(e.Tab.Key) == true)
                MonitoringManager.Instance().SendCommand(MonitoringCommand.ChangeTab, e.Tab.Key.ToString());
        }

        public override void UpdateTab(bool trained)
        {
            if (SystemManager.Instance().CurrentModel == null)
            {
                DisableTab(MainTabKey.Inspect);
                DisableTab(MainTabKey.Teach);
                
                return;
            }

            EnableTab(MainTabKey.Teach);

            if (trained == true)
            {
                MonitoringManager.Instance().SendCommand(MonitoringCommand.ChangeTab, MLCCSUtil.Instance().GetTabKey(MainTabKey.Inspect));

                EnableTab(MainTabKey.Inspect);
            }
        }

        public override bool ChangeTab(MainTabKey key)
        {
            if (tabMain.Tabs[key.ToString()].Visible == true && tabMain.Tabs[key.ToString()].Enabled == true)
            {
                tabMain.Tabs[key.ToString()].Selected = true;
                MonitoringManager.Instance().SendCommand(MonitoringCommand.ChangeTab, MLCCSUtil.Instance().GetTabKey(key));
                return true;
            }

            return false;
        }
    }
}
