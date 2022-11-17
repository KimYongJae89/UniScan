using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanG.Settings;
using UniScanG.UI.Model;
using System.Windows.Forms;
using UniScanG.Common;
using System;
using UniScanG.UI;
using System.Collections.Generic;
using UniScanG.Common.Data;
using UniScanG.UI.Teach.Monitor;
using UniScanG.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.Common.Util;
using UniScanG.Gravure.UI.Teach;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Common.Settings;
using UniScanG.Gravure.UI;
using UniScanG.Module.Controller.Settings.Monitor;
using System.IO;
using DynMvp.Data;
using UniScanG.Module.Controller.Device.Laser;
using UniScanG.Module.Controller.Device.Stage;
using UniScanG.Module.Controller.Device.Printer;
using UniScanG.UI.Inspect;
using UniScanG.Module.Controller.UI.Teach;
using UniScanG.Gravure.UI.Inspect;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.UI.Setting;

namespace UniScanG.Module.Controller.UI
{
    public class MonitorUiChangerG : MonitorUiChanger
    {
        public MonitorUiChangerG()
        {
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            if (uiControlPanel == null)
            {
                this.inspect = CreateInspectPage();
                this.model = CreateModelPage();
                this.teach = CreateTeachPage();
                this.report = CreateReportPage();
                this.setting = CreateSettingPage();
                this.log = CreateLogPage();

                uiControlPanel = new MainTabPanel((Control)inspect, (Control)model, (Control)teach, (Control)report, (Control)setting, (Control)log);
            }

            return uiControlPanel;
        }

        public override IMainTabPage CreateReportPage()
        {
            bool useLaserBurner = (MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None);
            return this.report = new UniScanG.UI.Report.ReportPage(useLaserBurner);
        }

        public override Control CreateInfoBufferPanel()
        {
            IInfoPanelBufferState infoPanelBufferState = new UI.Inspect.InfoPanelBufferState();
            return new Gravure.UI.Inspect.InfoPanel(infoPanelBufferState, MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None);
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.DefectPanel() { Dock = DockStyle.Right };
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanG.Gravure.UI.Setting.SettingPage(new UI.Settings.SettingPageExtender());
        }

        public override List<Control> GetTeachButtons(IVncContainer teachPage)
        {
            List<Control> controlList = new List<Control>();

            controlList.Add(new LightSettingButton());
            controlList.Add(new AutoTeachButton());

            if (MonitorSystemSettings.Instance().UseTestbedStage)
                controlList.Add(new TestbedStageControlButton());

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_TEACH, inspector, teachPage.ProcessStarted, teachPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }

        public override IReportPanel CreateReportPanel()
        {
            return new UniScanG.Gravure.UI.Report.ReportPanel(MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None);
        }

        public override IMainTabPage CreateInspectPage()
        {
            InspectPage inspectPage = new InspectPage();

            inspectPage.RepeatedDefectAlarmForm = new RepeatedDefectAlarmForm(inspectPage);

            //form.WindowState = FormWindowState.Minimized;
            //form.WindowState = FormWindowState.Normal;

            this.inspect = inspectPage;
            return this.inspect;
        }

        public override Control CreateTeachSettingPanel()
        {
            return null;
            //IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            //List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0);

            //TableLayoutPanel panel = new TableLayoutPanel();
            //panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //panel.AutoSize = true;
            //panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ////panel.Dock = DockStyle.Fill;
            //panel.ColumnCount = 1;
            //panel.RowCount = inspectorObjList.Count;
            //for (int i = 0; i < inspectorObjList.Count; i++)
            //{
            //    SettingPanel settingPanel = new SettingPanel(i);
            //    panel.Controls.Add(settingPanel, 0, i);
            //}
            //return panel;
        }

        public override List<IStatusStripPanel> GetStatusStrip()
        {
            bool canImsPowerOn = MonitorSystemSettings.Instance().EnableImsPowControl;
            List<IStatusStripPanel> statusStripList = new List<IStatusStripPanel>();

            if (SystemManager.Instance().DeviceBox.MachineIf != null)
                statusStripList.Add(new PrinterStatusStripPanel() { Dock = DockStyle.Right });

            // Right align
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj info in server.GetInspectorList())
            {
                InspectorStatusStripPanel statusStripPanel = new InspectorStatusStripPanel(info, canImsPowerOn);
                statusStripPanel.Dock = DockStyle.Right;
                statusStripList.Add(statusStripPanel);
            }

            // Left align
            DriveInfo[] driveInfos = DynMvp.Data.DataCopier.BackupDriveInfos;
            foreach (DriveInfo driveInfo in driveInfos)
            {
                VolumePanel volumePanel = new VolumePanel(driveInfo);
                volumePanel.Dock = DockStyle.Left;
                statusStripList.Insert(0, volumePanel);
            }

            if (MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None)
            {
                HanbitLaser hanbitLaser = ((Device.DeviceController)SystemManager.Instance().DeviceController).HanbitLaser;
                statusStripList.Add(new LaserStatusStripPanel(hanbitLaser) { Dock = DockStyle.Right });
            }


            return statusStripList;
        }

        public override IInspectExtraPanel CreateExtraInfoPanel()
        {
            if (AlgorithmSetting.Instance().UseExtMargin)
                return new Inspect.MarginPanel();
            return null;
        }

        public override IInspectExtraPanel CreateExtraImagePanel()
        {
            return new Inspect.ExtraImagePanel();
        }

        public override IInspectExtraPanel CreateExtraLengthPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.LengthChart();
        }
    }
}
