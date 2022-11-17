using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.UI.InspectionPanel;
using UniEye.Base.UI.ParamControl;

namespace UniEye.Base.UI
{
    public interface IUiControlPanel
    {
        void UpdateTab(bool trained);
        string GetCurrentTabKey();
        void EnableTab(string key);

        void DisableTab(string key);
        void ChangeTab(string key);
    }

    public class UiChanger
    {
        public virtual IMainForm CreateMainForm()
        {
            return new DefaultMainForm();
        }

        public virtual IReportPanel CreateReportPanel()
        {
            return new ReportPanel();
        }

        public virtual IInspectionPanel CreateInspectionPanel(int index)
        {
            SingleStepInspectionPanel singleStepInspectionPanel= new SingleStepInspectionPanel();
            singleStepInspectionPanel.Dock = DockStyle.Fill;
            return singleStepInspectionPanel;
        }

        public virtual string[] GetStepTypeNames()
        {
            return Enum.GetNames(typeof(DefaultStepType));
        }

        public virtual ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtender();
        }

        public virtual IDefectReportPanel CreateDefectReportPanel()
        {
            DefectReportPanel defectReportPanel = new DefectReportPanel();

            defectReportPanel.Location = new System.Drawing.Point(620, 47);
            defectReportPanel.Name = "DefectReportPanel";
            defectReportPanel.Size = new System.Drawing.Size(683, 489);
            defectReportPanel.TabIndex = 177;
            defectReportPanel.TabStop = false;

            return defectReportPanel;
        }

        public virtual ISettingPage CreateSettingPage()
        {
            SettingPage settingPage = new SettingPage();

            settingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            settingPage.Dock = System.Windows.Forms.DockStyle.Fill;
            settingPage.Location = new System.Drawing.Point(0, 313);
            settingPage.Name = "settingPage";
            settingPage.Size = new System.Drawing.Size(466, 359);
            settingPage.TabIndex = 0;

            return settingPage;
        }

        public virtual void BuildAdditionalAlgorithmTypeMenu(ModellerPage modellerPage, ToolStripItemCollection dropDownItems)
        {

        }

        public virtual IUiControlPanel CreateUiControlPanel()
        {
            return null;
        }

        public virtual void EnableTargetParamControls(TargetParamControl targetParamControl)
        {
            if (OperationSettings.Instance().UseSingleTargetModel == false)
            {
                targetParamControl.panelTarget.Visible = true;
            }
        }

        public virtual void SetupVisionParamControl(VisionParamControl visionParamControl)
        {
            visionParamControl.AddAlgorithmParamControl(new PatternMatchingParamControl());
            visionParamControl.AddAlgorithmParamControl(new BinaryCounterParamControl());
            visionParamControl.AddAlgorithmParamControl(new BrightnessCheckerParamControl());
            visionParamControl.AddAlgorithmParamControl(new ColorCheckerParamControl());
            visionParamControl.AddAlgorithmParamControl(new WidthCheckerParamControl());
            visionParamControl.AddAlgorithmParamControl(new CircleCheckeParamControl());
            visionParamControl.AddAlgorithmParamControl(new BlobCheckeParamControl());
            visionParamControl.AddAlgorithmParamControl(new BarcodeReaderParamControl());
            if (OperationSettings.Instance().ImagingLibrary == ImagingLibrary.MatroxMIL)
                visionParamControl.AddAlgorithmParamControl(new MilCharReaderParamControl());
            else
                visionParamControl.AddAlgorithmParamControl(new CharReaderParamControl());
            visionParamControl.AddAlgorithmParamControl(new RectCheckerParamControl());
            visionParamControl.AddAlgorithmParamControl(new DepthCheckerParamControl());
        }

        public virtual void ChangeModellerMenu(ModellerPage modellerPage)
        {

        }

        public virtual string[] GetProbeNames()
        {
            return null;
        }
    }
}
