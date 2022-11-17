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
using UniScanM.EDMSW.Operation;
using UniScanM.EDMSW.Settings;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.EDMSW.UI
{
    public partial class InspectionPanelLeft_Length : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        List<ProfilePanel_W> profilePanelList = new List<ProfilePanel_W>();
 
        public InspectionPanelLeft_Length()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            EDMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            InitResultViewPanel();
            StringManager.AddListener(this);
        }
        
        public void InitResultViewPanel()
        {
            ProfilePanel_W W100 = new ProfilePanel_W(Data.DataType_Length.W100);
            ProfilePanel_W W101 = new ProfilePanel_W(Data.DataType_Length.W101);
            ProfilePanel_W W102 = new ProfilePanel_W(Data.DataType_Length.W102);
            ProfilePanel_W L100 = new ProfilePanel_W(Data.DataType_Length.L100);
            ProfilePanel_W L200 = new ProfilePanel_W(Data.DataType_Length.L200);
            ProfilePanel_W LDIFF = new ProfilePanel_W(Data.DataType_Length.LDIFF);

            profilePanelList.Add(W100);
            profilePanelList.Add(W101);
            profilePanelList.Add(W102);
            profilePanelList.Add(L100);
            profilePanelList.Add(L200);
            profilePanelList.Add(LDIFF);

            layoutPanel.Controls.Add(W100, 0, 0);
            layoutPanel.Controls.Add(W101, 1, 0);
            layoutPanel.Controls.Add(W102, 2, 0);
            layoutPanel.Controls.Add(L100, 0, 1);
            layoutPanel.Controls.Add(L200, 1, 1);
            layoutPanel.Controls.Add(LDIFF, 2, 1);

            UpdatePanel();
        }

        public void UpdatePanel()
        {
            profilePanelList.ForEach(panel => panel.Initialize());

            //int newColumnCount = (EDMSSettings.Instance().SheetOnlyMode ? 1 : 3);
            int newColumnCount = 3;
            if (layoutPanel.ColumnCount != newColumnCount)
            {
                layoutPanel.ColumnCount = newColumnCount;
                SystemManager.Instance().InspectRunner.ResetState();

                //if (EDMSSettings.Instance().SheetOnlyMode)
                //    profilePanelList.ForEach(f => f.Visible = 
                //    (f.DataType == Data.DataType_Length.FilmEdge || f.DataType == Data.DataType_Length.FilmEdge_0));
                //else
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
        //        maxGap += 0.1 * (4 - rest);

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

            if (inspectionResult.Judgment == Judgment.Skip)
                return;

            if (edmsInspectionResult.State != Data.State_EDMS.Inspecting)
                return;

            if (edmsInspectionResult.TotalLengthData != null)
                foreach (ProfilePanel_W profilePanel in profilePanelList)
                    profilePanel.AddChartData(new DataPoint((float)edmsInspectionResult.RollDistance, 
                        (float)edmsInspectionResult.TotalLengthData[(int)profilePanel.DataType]));
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public void ClearPanel()
        {
           
            profilePanelList.ForEach(panel => panel.ClearPanel());
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
    }
}
