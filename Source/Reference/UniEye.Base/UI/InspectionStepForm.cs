using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices.MotionController;
using System;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public partial class InspectionStepForm : Form
    {
        InspectionStep inspectionStep;

        public InspectionStepForm()
        {
            InitializeComponent();
            labelStepName.Text = StringManager.GetString(this.GetType().FullName,labelStepName.Text);
            labelRobotPosition.Text = StringManager.GetString(this.GetType().FullName,labelRobotPosition.Text);
            columnAxis.HeaderText = StringManager.GetString(this.GetType().FullName,columnAxis.HeaderText);
            columnPosition.HeaderText = StringManager.GetString(this.GetType().FullName,columnPosition.HeaderText);
            okButton.Text = StringManager.GetString(this.GetType().FullName,okButton.Text);
            cancelButton.Text = StringManager.GetString(this.GetType().FullName,cancelButton.Text);
            refreshButton.Text = StringManager.GetString(this.GetType().FullName,refreshButton.Text);

            stepType.DataSource = SystemManager.Instance().UiChanger.GetStepTypeNames();
        }

        public void Initialize(InspectionStep inspectionStep, string[] stepNames = null)
        {
            this.inspectionStep = inspectionStep;
            if (stepNames != null)
                stepType.DataSource = stepNames;
        }

        private void InspectionStepForm_Load(object sender, EventArgs e)
        {
            if (inspectionStep == null)
                return;

            stepName.Text = inspectionStep.Name;
            stepType.SelectedIndex = inspectionStep.StepType;

            AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;

            if (robotStage != null)
            {
                positionGridView.Rows.Clear();
                foreach (Axis axis in robotStage.UniqueAxisList)
                {
                    positionGridView.Rows.Add(axis.Name);
                }
            }

            if (inspectionStep.AlignedPosition == null)
            {
                if (robotStage != null)
                    UpdatePosition(robotStage.GetActualPos());
            }
            else
            {
                UpdatePosition(inspectionStep.AlignedPosition);
            }
        }

        void UpdatePosition(AxisPosition axisPosition)
        { 
            int index = 0;
            foreach (float value in axisPosition.Position)
            {
                positionGridView.Rows[index].Cells[1].Value = value;
                index++;
            }
        }

        void MovePosition()
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;
            if (robotStage == null)
                return;

            int countRow = positionGridView.Rows.Count;
            int i = 0;
            float[] pos = new float[countRow];
            foreach (DataGridViewRow row in positionGridView.Rows)
            {
                pos[i++] = float.Parse((string)row.Cells[1].Value);
            }

            AxisPosition axisPosition = new AxisPosition(pos);
            robotStage.Move(axisPosition);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;
            //UpdatePosition(robotStage.GetActualPos());
            UpdatePosition(robotStage.GetCommandPos());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            inspectionStep.Name = stepName.Text;
            inspectionStep.StepType = stepType.SelectedIndex;

            AxisPosition axisPosition = new AxisPosition(positionGridView.Rows.Count);

            for (int i = 0; i < axisPosition.NumAxis; i++)
            {
                axisPosition[i] = Convert.ToSingle(positionGridView.Rows[i].Cells[1].Value.ToString());
            }

            //inspectionStep.AlignedPosition = axisPosition;
            inspectionStep.BasePosition = axisPosition;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            MovePosition();
        }
    }
}
