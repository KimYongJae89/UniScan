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
using UniScanM.Gloss.UI.Chart;
using UniScanM.Gloss.Settings;
using DynMvp.Base;
using DynMvp.UI;

namespace UniScanM.Gloss.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        int curDistance = int.MinValue;

        ChartSetting ProfileChartSetting { get; set; } = new ChartSetting();
        ChartSetting TrendChartSetting { get; set; } = new ChartSetting();
        ProfilePanel ProfilePanel { get; set; }
        TrendPanel TrendPanel { get; set; }

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            GlossSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            InitResultViewPanel();
            StringManager.AddListener(this);
        }

        public void InitResultViewPanel()
        {
            ProfileChartSetting.UseLineStop = GlossSettings.Instance().UseProfileLineStop;
            ProfileChartSetting.LineStopRange = GlossSettings.Instance().ProfileLineStopRange;
            ProfileChartSetting.UseLineWarning = GlossSettings.Instance().UseProfileLineWarning;
            ProfileChartSetting.LineWarningRange = GlossSettings.Instance().ProfileLineWarningRange;
            ProfileChartSetting.XAxisInterval = GlossSettings.Instance().ProfileXAxisInterval;
            ProfileChartSetting.YAxisRange = GlossSettings.Instance().ProfileYAxisRange;
            ProfileChartSetting.YAxisInterval = GlossSettings.Instance().ProfileYAxisInterval;

            ProfileChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            ProfileChartSetting.BackColor = GlossSettings.Instance().BackColor;
            ProfileChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            ProfileChartSetting.SubGraphColor = GlossSettings.Instance().OverlayGraphColor;
            ProfileChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            ProfileChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            ProfileChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            ProfileChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            ProfileChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            ProfileChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            ProfileChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;
            ProfileChartSetting.OverlayCount = GlossSettings.Instance().OverlayCount;

            ProfileChartSetting.GlossScanWidth = GlossSettings.Instance().SelectedGlossScanWidth?.Clone();

            TrendChartSetting.UseLineStop = GlossSettings.Instance().UseTrendLineStop;
            TrendChartSetting.LineStopRange = GlossSettings.Instance().TrendLineStopRange;
            TrendChartSetting.UseLineWarning = GlossSettings.Instance().UseTrendLineWarning;
            TrendChartSetting.LineWarningRange = GlossSettings.Instance().TrendLineWarningRange;
            TrendChartSetting.XAxisInterval = GlossSettings.Instance().TrendXAxisInterval;
            TrendChartSetting.YAxisRange = GlossSettings.Instance().TrendYAxisRange;
            TrendChartSetting.YAxisInterval = GlossSettings.Instance().TrendYAxisInterval;

            TrendChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            TrendChartSetting.BackColor = GlossSettings.Instance().BackColor;
            TrendChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            TrendChartSetting.SubGraphColor = GlossSettings.Instance().TrendbarGraphColor;
            TrendChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            TrendChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            TrendChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            TrendChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            TrendChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            TrendChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            TrendChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;

            TrendChartSetting.GlossScanWidth = GlossSettings.Instance().SelectedGlossScanWidth?.Clone();

            ProfilePanel = new ProfilePanel("Gloss Profile", false, ProfileChartSetting, new ChartOption(false, false, false));
            TrendPanel = new TrendPanel("Gloss Trend", false, TrendChartSetting, new ChartOption(false, false, false));
            
            VibrationViewPanel.Controls.Add(ProfilePanel, 0, 1);
            VibrationViewPanel.Controls.Add(TrendPanel, 0, 2);

            UpdatePanel();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            Gloss.Data.InspectionResult GlossInspectionResult = (Gloss.Data.InspectionResult)inspectionResult;
            
            if (GlossInspectionResult.GlossScanData != null)
            {
                UiHelper.SetControlText(glossRaw, GlossInspectionResult.GlossScanData.AvgGloss.ToString("F3"));

                ProfilePanel.AddScanData(GlossInspectionResult.GlossScanData);
                TrendPanel.AddScanData(GlossInspectionResult.GlossScanData);

                ProfilePanel.DisplayResult();
                TrendPanel.DisplayResult();
            }
        }
        
        private void UpdatePanel()
        {
            ProfileChartSetting.UseLineStop = GlossSettings.Instance().UseProfileLineStop;
            ProfileChartSetting.LineStopRange = GlossSettings.Instance().ProfileLineStopRange;
            ProfileChartSetting.UseLineWarning = GlossSettings.Instance().UseProfileLineWarning;
            ProfileChartSetting.LineWarningRange = GlossSettings.Instance().ProfileLineWarningRange;
            ProfileChartSetting.XAxisInterval = GlossSettings.Instance().ProfileXAxisInterval;
            ProfileChartSetting.YAxisRange = GlossSettings.Instance().ProfileYAxisRange;
            ProfileChartSetting.YAxisInterval = GlossSettings.Instance().ProfileYAxisInterval;

            ProfileChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            ProfileChartSetting.BackColor = GlossSettings.Instance().BackColor;
            ProfileChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            ProfileChartSetting.SubGraphColor = GlossSettings.Instance().OverlayGraphColor;
            ProfileChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            ProfileChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            ProfileChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            ProfileChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            ProfileChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            ProfileChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            ProfileChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;
            ProfileChartSetting.OverlayCount = GlossSettings.Instance().OverlayCount;

            ProfileChartSetting.GlossScanWidth = GlossSettings.Instance().SelectedGlossScanWidth;

            TrendChartSetting.UseLineStop = GlossSettings.Instance().UseTrendLineStop;
            TrendChartSetting.LineStopRange = GlossSettings.Instance().TrendLineStopRange;
            TrendChartSetting.UseLineWarning = GlossSettings.Instance().UseTrendLineWarning;
            TrendChartSetting.LineWarningRange = GlossSettings.Instance().TrendLineWarningRange;
            TrendChartSetting.XAxisInterval = GlossSettings.Instance().TrendXAxisInterval;
            TrendChartSetting.YAxisRange = GlossSettings.Instance().TrendYAxisRange;
            TrendChartSetting.YAxisInterval = GlossSettings.Instance().TrendYAxisInterval;

            TrendChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            TrendChartSetting.BackColor = GlossSettings.Instance().BackColor;
            TrendChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            TrendChartSetting.SubGraphColor = GlossSettings.Instance().TrendbarGraphColor;
            TrendChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            TrendChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            TrendChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            TrendChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            TrendChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            TrendChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            TrendChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;

            TrendChartSetting.GlossScanWidth = GlossSettings.Instance().SelectedGlossScanWidth;

            ProfilePanel.Initialize(ProfileChartSetting);
            TrendPanel.Initialize(TrendChartSetting);
        }

        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            UiHelper.SetControlText(glossRaw, "");

            ProfilePanel.ClearPanel();
            ProfilePanel.Initialize(ProfileChartSetting);
            TrendPanel.ClearPanel();
            TrendPanel.Initialize(TrendChartSetting);

            curDistance = int.MinValue;
        }

        public void Initialize() { }
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
