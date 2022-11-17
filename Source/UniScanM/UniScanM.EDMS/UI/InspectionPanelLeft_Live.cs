using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.Devices.Light;
using UniScanM.EDMS.Operation;
using UniScanM.EDMS.Settings;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.EDMS.UI
{
    public partial class InspectionPanelLeft_Live : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        List<ProfilePanel> profilePanelList = new List<ProfilePanel>();
        private bool isOn = true;

        public InspectionPanelLeft_Live()
        {
            InitializeComponent();
            this.SuspendLayout();
            this.Dock = DockStyle.Fill;
            
            EDMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            SetDoubleBuffered(this);

            InitResultViewPanel();
            this.ResumeLayout();
            StringManager.AddListener(this);
        }
        
        public void InitResultViewPanel()
        {

            ProfilePanel film = new ProfilePanel(Data.DataType.FilmEdge);
            ProfilePanel coating = new ProfilePanel(Data.DataType.Coating_Film);
            ProfilePanel printing = new ProfilePanel(Data.DataType.Printing_Coating);
            ProfilePanel film0 = new ProfilePanel(Data.DataType.FilmEdge_0);
            ProfilePanel printingEdge0 = new ProfilePanel(Data.DataType.PrintingEdge_0);
            ProfilePanel printingFilm0 = new ProfilePanel(Data.DataType.Printing_FilmEdge_0);

            profilePanelList.Add(film);
            profilePanelList.Add(coating);
            profilePanelList.Add(printing);
            profilePanelList.Add(film0);
            profilePanelList.Add(printingEdge0);
            profilePanelList.Add(printingFilm0);

            
            layoutPanel.SuspendLayout();

            layoutPanel.Controls.Add(film, 0, 0);
            layoutPanel.Controls.Add(coating, 1, 0);
            layoutPanel.Controls.Add(printing, 2, 0);
            layoutPanel.Controls.Add(film0, 0, 1);
            layoutPanel.Controls.Add(printingEdge0, 1, 1);
            layoutPanel.Controls.Add(printingFilm0, 2, 1);

            UpdatePanel();
            layoutPanel.ResumeLayout(false);
        }
        /*
        public void InitResultViewPanel2()
        {

            LivePanel film = new LivePanel(Data.DataType.FilmEdge);
            LivePanel coating = new LivePanel(Data.DataType.Coating_Film);
            LivePanel printing = new LivePanel(Data.DataType.Printing_Coating);
            LivePanel film0 = new LivePanel(Data.DataType.FilmEdge_0);
            LivePanel printingEdge0 = new LivePanel(Data.DataType.PrintingEdge_0);
            LivePanel printingFilm0 = new LivePanel(Data.DataType.Printing_FilmEdge_0);

            //profilePanelList.Add(film);
            //profilePanelList.Add(coating);
            //profilePanelList.Add(printing);
            //profilePanelList.Add(film0);
            //profilePanelList.Add(printingEdge0);
            //profilePanelList.Add(printingFilm0);


            layoutPanel.SuspendLayout();

            layoutPanel.Controls.Add(film, 0, 0);
            layoutPanel.Controls.Add(coating, 1, 0);
            layoutPanel.Controls.Add(printing, 2, 0);
            layoutPanel.Controls.Add(film0, 0, 1);
            layoutPanel.Controls.Add(printingEdge0, 1, 1);
            layoutPanel.Controls.Add(printingFilm0, 2, 1);

            UpdatePanel();
            layoutPanel.ResumeLayout(false);
        }*/

        public void UpdatePanel()
        {
            profilePanelList.ForEach(panel => panel.Initialize());

            int newColumnCount = (EDMSSettings.Instance().SheetOnlyMode ? 1 : 3);
            if (layoutPanel.ColumnCount != newColumnCount)
            {
                layoutPanel.ColumnCount = newColumnCount;
                SystemManager.Instance().InspectRunner.ResetState();
                if (EDMSSettings.Instance().SheetOnlyMode)
                    profilePanelList.ForEach(f => f.Visible = (f.DataType == Data.DataType.FilmEdge || f.DataType == Data.DataType.FilmEdge_0));
                else
                    profilePanelList.ForEach(f => f.Visible = true);
            }
        }
        //private delegate void CalulateAutoScaleValueDelegate();
        //private void CalulateAutoScaleValue()
        //{
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new CalulateAutoScaleValueDelegate(CalulateAutoScaleValue));
        //        return;
        //    }

        //    double maxGap = 0.4;
        //    double maxGapFrom0 = 0.04;
        //    foreach (MonitoringPanel monitoringPanel in monitoringPanelList)
        //    {
        //        if (monitoringPanel.PanelType == Data.DataType.FilmEdge || monitoringPanel.PanelType == Data.DataType.Coating_Film || monitoringPanel.PanelType == Data.DataType.Printing_Coating)
        //        {
        //            if (monitoringPanel.PositionList.Count == 0)
        //                return;

        //            double gap = (Math.Ceiling(monitoringPanel.PositionList.Max() * 10) / 10) - (Math.Truncate(monitoringPanel.PositionList.Min() * 10) / 10);
        //            if (gap > maxGap)
        //                maxGap = gap;
        //        }
        //        else if (monitoringPanel.PanelType == Data.DataType.FilmEdge_0 || monitoringPanel.PanelType == Data.DataType.PrintingEdge_0)
        //        {
        //            double gap = (Math.Ceiling(monitoringPanel.PositionList.Max() * 100) / 100) - (Math.Truncate(monitoringPanel.PositionList.Min() * 100) / 100);
        //            if (gap > maxGapFrom0)
        //                maxGapFrom0 = gap;
        //        }
        //    }

        //    int rest = (Convert.ToInt32(maxGap * 10.0) % 4);
        //    int restFrom0 = (Convert.ToInt32(maxGapFrom0 * 100.0) % 4);

        //    if (rest % 4 != 0)
        //        maxGap += 0.1 * (4- rest);

        //    if (restFrom0 % 4 != 0)
        //        maxGapFrom0 += 0.01 * (4 - restFrom0);

        //    foreach (MonitoringPanel monitoringPanel in monitoringPanelList)
        //    {
        //        if (monitoringPanel.PanelType == Data.DataType.FilmEdge || monitoringPanel.PanelType == Data.DataType.Coating_Film || monitoringPanel.PanelType == Data.DataType.Printing_Coating)
        //        {
        //            double medium = (monitoringPanel.PositionList.Max() + monitoringPanel.PositionList.Min()) / 2;
        //            double maxValue = Math.Ceiling((medium + maxGap / 2) * 10) / 10;
        //            double minValue = maxValue - maxGap;
        //            monitoringPanel.ChartMinMaxValue(minValue, maxValue);
        //        }
        //        else if (monitoringPanel.PanelType == Data.DataType.FilmEdge_0 || monitoringPanel.PanelType == Data.DataType.PrintingEdge_0)
        //        {
        //            double medium = (monitoringPanel.PositionList.Max() + monitoringPanel.PositionList.Min()) / 2;
        //            double maxValue = Math.Ceiling((medium + maxGapFrom0 / 2) * 100) / 100;
        //            double minValue = maxValue - maxGapFrom0;
        //            monitoringPanel.ChartMinMaxValue(minValue, maxValue);
        //        }
        //    }
        //}

        public void ProductInspected(InspectionResult inspectionResult)
        {
            Data.InspectionResult edmsInspectionResult = (Data.InspectionResult)inspectionResult;
            if (edmsInspectionResult.ResetZeroing)
                ClearPanel();

             if (edmsInspectionResult.State != Data.State_EDMS.Inspecting)
                return;

            layoutPanel.SuspendLayout();

            foreach (var profilePanel in profilePanelList)
            {
                profilePanel.AddChartData(
                    new DataPoint((float)edmsInspectionResult.RollDistance,
                                    (float)edmsInspectionResult.TotalEdgePositionResult[(int)profilePanel.DataType])
                                    );
            }

            layoutPanel.ResumeLayout(false);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public void ClearPanel()
        {
           
            profilePanelList.ForEach(panel => panel.ClearPanel());
            //profilePanelList.ForEach(panel => panel.Refresh());
            //profilePanelList.ForEach(panel => panel.Initialize());
        }

        public void ClearPanel(double rollDistance)
        {
            //profilePanelList.ForEach(panel => panel.ClearPanel2(rollDistance));
            //profilePanelList.ForEach(panel => panel.Refresh());
            //profilePanelList.ForEach(panel => panel.Initialize());
        }

        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
