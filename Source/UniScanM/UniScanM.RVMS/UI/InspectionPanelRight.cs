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
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;
using UniScanM.RVMS.Settings;
using UniScanM.RVMS.UI.Chart;
using DynMvp.UI.Touch;
using UniScanM.RVMS.Data;
using UniEye.Base.MachineInterface;
using System.Threading;
using UniScanM.RVMS.MachineIF;
using UniScanM.MachineIF;
using UniScanM.RVMS.Operation;

namespace UniScanM.RVMS.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        ProfilePanel patternBeforePanel;
        ProfilePanel patternAfterPanel;
        
        public InspectionPanelRight()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitResultViewPanel();

            patternBeforePanel.Visible = patternAfterPanel.Visible = RVMSSettings.Instance().VisibilityPlGraph;

            RVMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
        }

        public void InitResultViewPanel()
        {
            patternBeforePanel = new ProfilePanel("Before Pattern", false, true, false, null, new ProfileOption(false, false,true));
            patternAfterPanel = new ProfilePanel("After Pattern", false, true, false, null, new ProfileOption(false, false, true));

            panelBefore.Controls.Add(patternBeforePanel);
            panelAfter.Controls.Add(patternAfterPanel);
        }
         
        public void UpdatePanel()
        {
            patternBeforePanel.Visible = patternAfterPanel.Visible = RVMSSettings.Instance().VisibilityPlGraph;
            patternAfterPanel.Initialize();
            patternBeforePanel.Initialize();

        }

        private void Clear()
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void ProductInspectedDelegate(DynMvp.InspData.InspectionResult inspectionResult);
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;
            this.SuspendLayout();

            if (rvmsInspectionResult.ResetZeroing)
            {
                patternBeforePanel.ClearPanel();
                patternAfterPanel.ClearPanel();
            }
            else
            {
                if (rvmsInspectionResult.BeforePattern != null)
                    patternBeforePanel.AddValue(rvmsInspectionResult.BeforePattern);

                if (rvmsInspectionResult.AffterPattern != null)
                    patternAfterPanel.AddValue(rvmsInspectionResult.AffterPattern);
            }
            UiHelper.SuspendDrawing(labelState);

            if (SystemState.Instance().OpState != OpState.Idle)
            {
                if (rvmsInspectionResult.ZeroingComplate == false)
                {
                    RVMSSettings rvmsSetting = (RVMSSettings)RVMSSettings.Instance();
                    if (rvmsInspectionResult.ZeroingStable)
                    {
                        labelState.Text = string.Format("{0} ({1}/{2})", StringManager.GetString("Zeroing"), rvmsInspectionResult.ZeroingNum, rvmsSetting.DataCountForZeroSetting);
                        labelState.BackColor = Color.Gold;
                        labelState.ForeColor = Color.Black;
                    }
                    else
                    {
                        double threshold = Math.Abs(rvmsSetting.LineStopUpper + rvmsSetting.LineStopLower);
                        labelState.Text = string.Format("{0} ({1:F2}/{2:F2})", StringManager.GetString("ORG. Unstable"), rvmsInspectionResult.ZeroingVariance, threshold);
                        labelState.BackColor = Color.Red;
                        labelState.ForeColor = Color.White;
                    }
                    
                    progressBarZeroing.Value = (int)(((float)rvmsInspectionResult.ZeroingNum / (float)RVMSSettings.Instance().DataCountForZeroSetting) * 100.0f);
                }
                else
                {
                    labelState.Text = StringManager.GetString("Measure");
                    labelState.BackColor = Color.Green;
                    labelState.ForeColor= Color.White;
                }
            }
            UiHelper.ResumeDrawing(labelState);
            this.ResumeLayout();
        }

        public void Initialize() { }
        delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            labelState.Text = StringManager.GetString("State");
            labelState.BackColor = Color.Black;

            patternBeforePanel.ClearPanel();
            patternAfterPanel.ClearPanel();
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if(InvokeRequired)
            {
                this.Invoke(new OpStateChangedDelegate(OpStateChanged),curOpState, prevOpState);
                return;
            }

            if(curOpState == OpState.Idle)
            {
                labelState.ForeColor = SystemColors.ControlText;
                labelState.BackColor = SystemColors.Control;
                labelState.Text = "";
                progressBarZeroing.Value = 0;
                return;
            }
        }


        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        private void buttonStateReset_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ResetState();
        }
    }
}
