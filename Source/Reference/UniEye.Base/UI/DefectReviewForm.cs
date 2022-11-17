using System;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.InspData;
using DynMvp.Data;
using UniEye.Base.Data;

namespace UniEye.Base.UI
{
    public enum ReportMode
    {
        Review, Report
    }

    public interface IDefectReportPanel
    {
        void Initialize(ReportMode reportMode, Model model, InspectionResult inspectionResult);
        void MovePrev();
        void MoveNext();
        void SetGood();
        void SetDefect();
    }

    public partial class DefectReviewForm : Form
    {
        private InspectionResult inspectionResult;
//        private Machine machine;

        IDefectReportPanel defectReportPanel;
        public IDefectReportPanel DefectReportPanel
        {
            set { defectReportPanel = value; }
        }

        private int curIndex = 0;

        public DefectReviewForm()
        {
            InitializeComponent();

            defectReportPanel = SystemManager.Instance().UiChanger.CreateDefectReportPanel();

            UserControl userControl = (UserControl)defectReportPanel;
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;

            this.panelReport.Controls.Add(userControl);

            btnAlarmOff.Text = StringManager.GetString(this.GetType().FullName,btnAlarmOff.Text);
        }

        public void Initialize(InspectionResult inspectionResult)
        {
            defectReportPanel.Initialize(ReportMode.Review, SystemManager.Instance().CurrentModel, inspectionResult);

            this.inspectionResult = inspectionResult;

            UpdateResult();
        }

        private void ProbeDefectProcessForm_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdateResult()
        {
            int defectCount = inspectionResult.GetProbeDefectCount();
            numDefectTarget.Text = defectCount.ToString();
            if (defectCount == 0)
            {
                labelResult.BackColor = Color.LightGreen;
                labelResult.Text = StringManager.GetString(this.GetType().FullName, "Good");
            }
            else
            {
                labelResult.BackColor = Color.Red;
                labelResult.Text = StringManager.GetString(this.GetType().FullName, "NG");
            }
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            LogHelper.Info(LoggerType.Operation, "Defect Process - Retry Inspection");
        }

        private void probeNgButton_Click(object sender, EventArgs e)
        {
            LogHelper.Info(LoggerType.Operation, "Defect Process - Set NG");
            defectReportPanel.SetDefect();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void labelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.MovePrev();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.MoveNext();
        }

        private void probeGoodButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.SetGood();
        }

        private void btnAlarmOff_Click(object sender, EventArgs e)
        {
            SystemState.Instance().Alarmed = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            inspectionResult.UpdateJudgement();

            DialogResult result;
            if (inspectionResult.IsGood())
                result = MessageForm.Show(this, StringManager.GetString(this.GetType().FullName, "Do you want to close this form?"), MessageFormType.YesNo);
            else
                result = MessageForm.Show(this, StringManager.GetString(this.GetType().FullName, "Are you sure you want to process as all NG?"), MessageFormType.YesNo);

            if (result == DialogResult.No)
                return;

            Close();
        }
    }
}
