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
using static UniScanM.RVMS.MachineIF.MachineIfProtocolList;
using UniScanM.RVMS.MachineIF;
using UniScanM.MachineIF;
using UniScanM.RVMS.Operation;
using static DynMvp.UI.UiHelper;

namespace UniScanM.RVMS.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        ProfilePanel normalManSidePanel;
        ProfilePanel normalGearSidePanel;

        ProfilePanel totalManSidePanel;
        ProfilePanel totalGearSidePanel;

        bool useCenterLine;
        public bool UseCenterLine { get => useCenterLine; set => useCenterLine = value; }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }
      
        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitResultViewPanel();

            RVMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            StringManager.AddListener(this);
        }

        public void InitResultViewPanel()
        {
            normalGearSidePanel = new ProfilePanel("Certain Time", false, false, false, null, new ProfileOption(false, false,false));
            normalManSidePanel = new ProfilePanel("Certain Time", false, false, false, null, new ProfileOption(false, false, false));
            totalGearSidePanel = new ProfilePanel("Total Time", true, false, false, null, new ProfileOption(false, false, false));
            totalManSidePanel = new ProfilePanel("Total Time", true, false, false, null, new ProfileOption(false, false, false));

            VibrationViewPanel.Controls.Add(normalGearSidePanel, 0, 1);
            VibrationViewPanel.Controls.Add(normalManSidePanel, 1, 1);
            VibrationViewPanel.Controls.Add(totalGearSidePanel, 0, 2);
            VibrationViewPanel.Controls.Add(totalManSidePanel, 1, 2);
        }

        public void UpdatePanel()
        {
            normalGearSidePanel.Initialize();
            normalManSidePanel.Initialize();
            totalGearSidePanel.Initialize();
            totalManSidePanel.Initialize();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;

            if (rvmsInspectionResult.ManSide != null)
            {
                UiHelper.SetControlText(manRaw, rvmsInspectionResult.ManSide.YRaw.ToString("F3"));
                if (rvmsInspectionResult.ResetZeroing)
                {
                    normalManSidePanel.ClearPanel();
                    totalManSidePanel.ClearPanel();
                }
                else if(rvmsInspectionResult.ZeroingComplate)
                {
                    normalManSidePanel.AddValue(rvmsInspectionResult.ManSide);
                    totalManSidePanel.AddValue(rvmsInspectionResult.ManSide);
                }
            }

            if (rvmsInspectionResult.GearSide != null)
            {
                UiHelper.SetControlText(gearRaw, rvmsInspectionResult.GearSide.YRaw.ToString("F3"));
                if (rvmsInspectionResult.ResetZeroing)
                {
                    normalGearSidePanel.ClearPanel();
                    totalGearSidePanel.ClearPanel();
                }
                else if (rvmsInspectionResult.ZeroingComplate)
                {
                    normalGearSidePanel.AddValue(rvmsInspectionResult.GearSide);
                    totalGearSidePanel.AddValue(rvmsInspectionResult.GearSide);
                }
            }
        }
        
        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            UiHelper.SetControlText(manRaw, "");
            UiHelper.SetControlText(gearRaw, "");

            normalManSidePanel.ClearPanel();
            normalGearSidePanel.ClearPanel();
            totalGearSidePanel.ClearPanel();
            totalManSidePanel.ClearPanel();

            normalManSidePanel.Initialize();
            normalGearSidePanel.Initialize();
            totalGearSidePanel.Initialize();
            totalManSidePanel.Initialize();    
        }

        public void Initialize() { }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection()
        {
            UiHelper.SetControlText(manRaw, "");
            UiHelper.SetControlText(gearRaw, "");
        }

        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
    }
}
