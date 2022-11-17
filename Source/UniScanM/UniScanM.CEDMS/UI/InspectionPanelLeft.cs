using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.InspData;
using UniScanM.CEDMS.UI.Chart;
using UniScanM.CEDMS.Settings;
using DynMvp.Base;
using DynMvp.UI;

namespace UniScanM.CEDMS.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        List<ProfilePanel> profilePanelInFeedList = new List<ProfilePanel>();
        List<ProfilePanel> profilePanelOutFeedList = new List<ProfilePanel>();

        int curDistance = int.MinValue;

        ChartSetting inFeedChartSetting = new ChartSetting();
        ChartSetting outFeedChartSetting = new ChartSetting();

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            CEDMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            InitResultViewPanel();
            StringManager.AddListener(this);
        }

        public void InitResultViewPanel()
        {
            inFeedChartSetting.UseLineStop = CEDMSSettings.Instance().InFeedUseLineStop;
            inFeedChartSetting.LineStopLower = CEDMSSettings.Instance().InFeedLineStopLower;
            inFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().InFeedLineStopUpper;
            inFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().InFeedUseLineWarning;
            inFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().InFeedLineWarningLower;
            inFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().InFeedLineWarningUpper;
            inFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().InFeedXAxisDisplayLength;
            inFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().InFeedXAxisDisplayTotalLength;
            inFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().InFeedXAxisInterval;
            inFeedChartSetting.YAxisRange = CEDMSSettings.Instance().InFeedYAxisRange;
            inFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().InFeedYAxisUnit;
            inFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().InFeedYAxisInterval;
            inFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().InFeedUseTotalCenterLine;

            outFeedChartSetting.UseLineStop = CEDMSSettings.Instance().OutFeedUseLineStop;
            outFeedChartSetting.LineStopLower = CEDMSSettings.Instance().OutFeedLineStopLower;
            outFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().OutFeedLineStopUpper;
            outFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().OutFeedUseLineWarning;
            outFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().OutFeedLineWarningLower;
            outFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().OutFeedLineWarningUpper;
            outFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().OutFeedXAxisDisplayLength;
            outFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().OutFeedXAxisDisplayTotalLength;
            outFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().OutFeedXAxisInterval;
            outFeedChartSetting.YAxisRange = CEDMSSettings.Instance().OutFeedYAxisRange;
            outFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().OutFeedYAxisUnit;
            outFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().OutFeedYAxisInterval;
            outFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().OutFeedUseTotalCenterLine;

            inFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            inFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            inFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            inFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            inFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            inFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            inFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            inFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            inFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            inFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            outFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            outFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            outFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            outFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            outFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            outFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            outFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            outFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            outFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            outFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            ProfilePanel normalInFeedPanel = new ProfilePanel("Recent Length", false, false, inFeedChartSetting, new ProfileOption(false, false, false));
            ProfilePanel normalOutFeedPanel = new ProfilePanel("Recent Length", false, false, outFeedChartSetting, new ProfileOption(false, false, false));

            ProfilePanel totalInFeedPanel = new ProfilePanel("Total Length", true, false, inFeedChartSetting, new ProfileOption(false, false, false));
            ProfilePanel totalOutFeedPanel = new ProfilePanel("Total Length", true, false, outFeedChartSetting, new ProfileOption(false, false, false));

            profilePanelInFeedList.Add(normalInFeedPanel);
            profilePanelInFeedList.Add(totalInFeedPanel);

            profilePanelOutFeedList.Add(normalOutFeedPanel);
            profilePanelOutFeedList.Add(totalOutFeedPanel);

            VibrationViewPanel.Controls.Add(normalInFeedPanel, 0, 1);
            VibrationViewPanel.Controls.Add(normalOutFeedPanel, 1, 1);
            VibrationViewPanel.Controls.Add(totalInFeedPanel, 0, 2);
            VibrationViewPanel.Controls.Add(totalOutFeedPanel, 1, 2);

            UpdatePanel();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            CEDMS.Data.InspectionResult cedmsInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;

            // 마지막 측정한 거리를 가지고 있다가 현재 거리와 같은 거리거나 값이 적으면 (장비가 동작중이 아닐 경우) 측정 스킵
            if (cedmsInspectionResult.RollDistance <= curDistance)
                return;
            else
                curDistance = cedmsInspectionResult.RollDistance;

            if (cedmsInspectionResult.InFeed != null)
            {
                UiHelper.SetControlText(inFeedRaw, cedmsInspectionResult.InFeed.YRaw.ToString("F3"));

                if (cedmsInspectionResult.ResetZeroing)
                    profilePanelInFeedList.ForEach(panel => panel.ClearPanel());
                else if (cedmsInspectionResult.ZeroingComplete)
                    profilePanelInFeedList.ForEach(panel => panel.AddValue(cedmsInspectionResult.InFeed));
            }

            if (cedmsInspectionResult.OutFeed != null)
            {
                UiHelper.SetControlText(outFeedRaw, cedmsInspectionResult.OutFeed.YRaw.ToString("F3"));

                if (cedmsInspectionResult.ResetZeroing)
                    profilePanelOutFeedList.ForEach(panel => panel.ClearPanel());
                else if (cedmsInspectionResult.ZeroingComplete)
                    profilePanelOutFeedList.ForEach(panel => panel.AddValue(cedmsInspectionResult.OutFeed));
            }
        }
        
        private void UpdatePanel()
        {
            inFeedChartSetting.UseLineStop = CEDMSSettings.Instance().InFeedUseLineStop;
            inFeedChartSetting.LineStopLower = CEDMSSettings.Instance().InFeedLineStopLower;
            inFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().InFeedLineStopUpper;
            inFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().InFeedUseLineWarning;
            inFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().InFeedLineWarningLower;
            inFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().InFeedLineWarningUpper;
            inFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().InFeedXAxisDisplayLength;
            inFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().InFeedXAxisDisplayTotalLength;
            inFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().InFeedXAxisInterval;
            inFeedChartSetting.YAxisRange = CEDMSSettings.Instance().InFeedYAxisRange;
            inFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().InFeedYAxisUnit;
            inFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().InFeedYAxisInterval;
            inFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().InFeedUseTotalCenterLine;

            outFeedChartSetting.UseLineStop = CEDMSSettings.Instance().OutFeedUseLineStop;
            outFeedChartSetting.LineStopLower = CEDMSSettings.Instance().OutFeedLineStopLower;
            outFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().OutFeedLineStopUpper;
            outFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().OutFeedUseLineWarning;
            outFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().OutFeedLineWarningLower;
            outFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().OutFeedLineWarningUpper;
            outFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().OutFeedXAxisDisplayLength;
            outFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().OutFeedXAxisDisplayTotalLength;
            outFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().OutFeedXAxisInterval;
            outFeedChartSetting.YAxisRange = CEDMSSettings.Instance().OutFeedYAxisRange;
            outFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().OutFeedYAxisUnit;
            outFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().OutFeedYAxisInterval;
            outFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().OutFeedUseTotalCenterLine;

            inFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            inFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            inFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            inFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            inFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            inFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            inFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            inFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            inFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            inFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            outFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            outFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            outFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            outFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            outFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            outFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            outFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            outFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            outFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            outFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            profilePanelInFeedList.ForEach(panel => panel.Initialize(inFeedChartSetting));
            profilePanelOutFeedList.ForEach(panel => panel.Initialize(outFeedChartSetting));
        }

        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            UiHelper.SetControlText(inFeedRaw, "");
            UiHelper.SetControlText(outFeedRaw, "");

            profilePanelInFeedList.ForEach(panel => panel.ClearPanel());
            profilePanelOutFeedList.ForEach(panel => panel.ClearPanel());
            profilePanelInFeedList.ForEach(panel => panel.Initialize());
            profilePanelOutFeedList.ForEach(panel => panel.Initialize());

            curDistance = int.MinValue;
        }

        public void Initialize() { }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection()
        {
            UiHelper.SetControlText(inFeedRaw, "");
            UiHelper.SetControlText(outFeedRaw, "");
        }

        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
    }
}
