using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Device.MotionController.UI
{
    public partial class AxisConfigurationForm : Form
    {
        int preSelectedIndex = -1;
        AxisConfiguration axisConfiguration;
        MotionList motionList;
        

        bool lockUpdate = false;

        public AxisConfigurationForm()
        {
            InitializeComponent();

            moveUpButton.Text = StringManager.GetString(this.GetType().FullName,moveUpButton.Text);
            moveDownButton.Text = StringManager.GetString(this.GetType().FullName,moveDownButton.Text);
            homeMoveButton.Text = StringManager.GetString(this.GetType().FullName,homeMoveButton.Text);
            jogPlusButton.Text = StringManager.GetString(this.GetType().FullName,jogPlusButton.Text);
            jogMinusButton.Text = StringManager.GetString(this.GetType().FullName,jogMinusButton.Text);
            setOriginOffsetButton.Text = StringManager.GetString(this.GetType().FullName,setOriginOffsetButton.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        public void Initialize(AxisConfiguration axisConfiguration, MotionList motionList)
        {
            lockUpdate = true;

            this.axisConfiguration = axisConfiguration;
            this.motionList = motionList;

            ColumnType.DataSource =Enum.GetNames(typeof(AxisHandlerType));
            columnAxisName.DataSource = Enum.GetNames(typeof(AxisName));

            columnMotionName.Items.Clear();
            foreach (Motion motion in motionList)
            {
                columnMotionName.Items.Add(motion.Name);
            }

            UpdateHandlerList();

            if(axisHandlerList.Rows.Count>0)
                axisHandlerList.Rows[0].Selected = true;

            UpdateAxisList();

            lockUpdate = false;

            positionUpdateTimer.Start();

            preSelectedIndex = 0;
        }

        private void UpdateHandlerList()
        {
            axisHandlerList.Rows.Clear();
            foreach (AxisHandler axisHandler in axisConfiguration)
            {
                int index = axisHandlerList.Rows.Add(axisHandler.Name, axisHandler.HandlerType.ToString());
                axisHandlerList.Rows[index].Tag = axisHandler;
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveAxisHandler();
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(axisList);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(axisList);
        }

        Motion GetMotion()
        {
            object value = axisList.SelectedRows[0].Cells[1].Value;
            if (value == null)
                return null;

            return motionList.GetMotion(value.ToString());
        }

        int GetAxisNo()
        {
            object value = axisList.SelectedRows[0].Cells[2].Value;
            if (value == null)
                return 0;

            return Convert.ToInt32(value.ToString());
        }
        
        private void jogPlusButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                bool isOn = motion.IsServoOn(axisNo);
                if (isOn)
                {
                    MovingParam movingParam = new MovingParam("", 1000, 100, 100, 15000, 0);
                    motion.ContinuousMove(axisNo, movingParam, false);
                }
            }
        }

        private void jogMinusButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                bool isOn = motion.IsServoOn(axisNo);
                if (isOn)
                {
                    MovingParam movingParam = new MovingParam("", 1000, 100, 100, 15000, 0);
                    motion.ContinuousMove(axisNo, movingParam, true);
                }
            }
        }


        private void jogButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                bool isOn = motion.IsServoOn(axisNo);
                if (isOn)
                {
                    motion.StopMove(axisNo);
                }
            }
        }

        private void SaveAxisHandler()
        {
            axisConfiguration.Clear();
            for(int i=0; i< axisHandlerList.Rows.Count;i++)
            {
                AxisHandler axisHandler = (AxisHandler)axisHandlerList.Rows[i].Tag;
                axisConfiguration.Add(axisHandler);
            }
            //if (preSelectedIndex > -1)
            //{
            //    AxisHandler preAxisHandler = (AxisHandler)axisHandlerList.Rows[preSelectedIndex].Tag;
            //    preAxisHandler.Clear();

            //    for (int i = 0; i < axisList.Rows.Count; i++)
            //    {
            //        DataGridViewCellCollection cells = axisList.Rows[i].Cells;
            //        if (cells[0].Value == null)
            //            break;

            //        string axisName = cells[0].Value.ToString();
            //        Motion motion = motionList.GetMotion(cells[1].Value.ToString());
            //        int axisNo = Convert.ToInt32(cells[2].Value.ToString());
            //        float originPulse = 0;
            //        if (cells[3].Value != null)
            //        {
            //            originPulse = Convert.ToSingle(cells[3].Value.ToString());
            //        }

            //        if (motion != null)
            //        {
            //            Axis axis = (Axis)axisList.Rows[i].Tag;
            //            if (axis == null)
            //                axis = preAxisHandler.AddAxis(axisName, motion, axisNo);
            //            else
            //            {
            //                preAxisHandler.AddAxis(axis);
            //                axis.Update(axisName, motion, axisNo);
            //            }

            //            axis.AxisParam.OriginPulse = originPulse;
            //        }
            //    }
            //}
        }

        private void UpdateAxisList()
        {
            axisList.Rows.Clear();

            if (axisHandlerList.SelectedRows.Count == 0)
                return;

            AxisHandler axisHandler = (AxisHandler)axisHandlerList.SelectedRows[0].Tag;
            if (axisHandler == null)
                return;

            for (int i = 0; i < axisHandler.NumAxis; i++)
            {
                Axis axis = axisHandler[i];

                string motionName = "";
                if (axis.Motion != null)
                {
                    motionName = axis.Motion.Name;
                }

                int rowIndex = axisList.Rows.Add(axis.Name, motionName, axis.AxisNo, axis.AxisParam.MicronPerPulse, axis.AxisParam.OriginPulse);
                axisList.Rows[rowIndex].Tag = axis;
            }
        }

        private void axisHandlerList_SelectionChanged(object sender, EventArgs e)
        {
            if (axisHandlerList.SelectedRows.Count == 0 || lockUpdate == true)
                return;

            //UpdateAxisHandler();
            UpdateAxisList();
        }

        private void setOriginOffsetButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                MovingParam movingParam = new MovingParam("", 1000, 100, 100, 15000, 0);
                float axisPosition = motion.GetActualPos(axisNo);

                axisList.SelectedRows[0].Cells[3].Value = axisPosition.ToString();
            }
        }

        private void homeMoveButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                bool isOn = motion.IsServoOn(axisNo);
                if (isOn)
                {
                    HomeParam homeParam = new HomeParam();
                    homeParam.HighSpeed = new MovingParam("", 5000, 100, 100, 15000, 0);
                    homeParam.MediumSpeed = new MovingParam("", 3000, 100, 100, 10000, 0);
                    homeParam.FineSpeed = new MovingParam("", 1000, 100, 100, 5000, 0);
                    
                    //motion.HomeMove(axisNo, homeParam);
                    SimpleProgressForm form = new SimpleProgressForm();
                    form.Show(() => motion.HomeMove(axisNo, homeParam));
                    
                }
            }
        }

        private void positionUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                position.Text = motion.GetCommandPos(axisNo).ToString();
                survoButton.Text = motion.IsServoOn(axisNo) ? "OFF" : "ON";
            }
        }

        private void survoButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                bool isOn = motion.IsServoOn(axisNo);
                motion.TurnOnServo(axisNo, !isOn);
                //survoButton.Text = string.Format("Survo {0}", survoStateOn ? "OFF" : "ON");
            }
        }

        private void AxisConfigurationForm_Load(object sender, EventArgs e)
        {
            //ColumnType.Items.Add("RobotStage");
            //ColumnType.Items.Add("Convayor");
        }

        private void axisHandlerList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.lockUpdate)
                return;

            if (e.RowIndex < 0)
                return;

            AxisHandler axisHandler = (AxisHandler)axisHandlerList.Rows[e.RowIndex].Tag;
            axisHandler.Name = (string)axisHandlerList.Rows[e.RowIndex].Cells[0].Value;
            axisHandler.HandlerType = (AxisHandlerType)Enum.Parse(typeof(AxisHandlerType),(string)axisHandlerList.Rows[e.RowIndex].Cells[1].Value);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AxisHandler axisHandler = new AxisHandler("New");
            int newIdx =axisHandlerList.Rows.Add(axisHandler.Name, axisHandler.HandlerType.ToString());
            axisHandlerList.Rows[newIdx].Tag = axisHandler;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axisHandlerList.SelectedRows.Count == 1)
            {
                int index = axisHandlerList.SelectedRows[0].Index;
                axisHandlerList.Rows.RemoveAt(index);
            }
        }

        private void axisList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
                return;

            if (this.lockUpdate)
                return;

            AxisHandler axisHandler = (AxisHandler)this.axisHandlerList.SelectedRows[0].Tag;
            int rowIndex = axisList.SelectedRows[0].Index;

            DataGridViewCellCollection cells = axisList.Rows[rowIndex].Cells;
            if (cells[0].Value == null || cells[1].Value == null || cells[2].Value == null)
                return;

            string axisName = cells[0].Value.ToString();

            Motion motion = motionList.GetMotion(cells[1].Value.ToString());
            int axisNo = Convert.ToInt32(cells[2].Value.ToString());
            double umPerPulse = cells[3].Value == null ? 0 : Convert.ToDouble(cells[3].Value);
            float originPulse = cells[4].Value == null ? 0 : Convert.ToSingle(cells[4].Value);

            if (umPerPulse <= 0)
                return ;

            if (motion != null)
            {
                Axis axis = (Axis)axisList.Rows[rowIndex].Tag;
                if (axis == null)
                    axis = axisHandler.AddAxis(axisName, motion, axisNo);

                axis.Update(axisName, motion, axisNo);

                axis.AxisParam.MicronPerPulse = umPerPulse;
                axis.AxisParam.OriginPulse = originPulse;
            }
        }

        private void toolStripMenuItemAddAxis_Click(object sender, EventArgs e)
        {
            AxisHandler axisHandler = (AxisHandler)this.axisHandlerList.SelectedRows[0].Tag;
            if (axisHandler == null)
                return;

            Axis axis = axisHandler.AddAxis("", null, 0);
            UpdateAxisList();

        }

        private void toolStripMenuItemDeleteAxis_Click(object sender, EventArgs e)
        {
            AxisHandler axisHandler = (AxisHandler)this.axisHandlerList.SelectedRows[0].Tag;
            if (axisHandler == null)
                return;

            Axis axis = (Axis)this.axisList.SelectedRows[0].Tag;
            if (axis == null)
                return;

            axisHandler.RemoveAxisAt(this.axisList.SelectedRows[0].Index);
            UpdateAxisList();
        }

        private void axisList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void axisHandlerList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}    
