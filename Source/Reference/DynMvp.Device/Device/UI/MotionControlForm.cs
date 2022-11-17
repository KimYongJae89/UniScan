using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Devices.MotionController;
using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.UI
{
    public partial class MotionControlForm : Form
    {
        AxisConfiguration axisConfiguration;
        AxisHandler curAxisHandler;
        AxisParam axisParam = new AxisParam();

        public MotionControlForm()
        {
            InitializeComponent();

            labelAxisHandler.Text = StringManager.GetString(this.GetType().FullName, labelAxisHandler);
            labelAxisNo.Text = StringManager.GetString(this.GetType().FullName, labelAxisNo);
            labelPosition.Text = StringManager.GetString(this.GetType().FullName, labelPosition);
            labelRelative.Text = StringManager.GetString(this.GetType().FullName, labelRelative);
            moveButton.Text = StringManager.GetString(this.GetType().FullName, moveButton);
            originButton.Text = StringManager.GetString(this.GetType().FullName, originButton);
            buttonFindLimit.Text = StringManager.GetString(this.GetType().FullName, buttonFindLimit);
            okbutton.Text = StringManager.GetString(this.GetType().FullName, okbutton);
        }

        public void Intialize(AxisConfiguration axisConfiguration)
        {
            this.axisConfiguration = axisConfiguration;
        }

        private void MotionControlForm_Load(object sender, EventArgs e)
        {
            foreach (AxisHandler axisHandler in axisConfiguration)
            {
                comboAxisHandler.Items.Add(axisHandler);
            }

            comboAxisHandler.SelectedIndex = 0;
            movingStep.SelectedIndex = 2;
            paramPropertyGrid.SelectedObject = axisParam;

            checkTimer.Start();
        }

        private void comboAxisHandler_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxisHandler.SelectedIndex;
            curAxisHandler = (AxisHandler)comboAxisHandler.Items[selectedIndex];

            comboAxis.Items.Clear();
            for (int i = 0; i < curAxisHandler.NumAxis; i++)
                comboAxis.Items.Add(curAxisHandler[i]);

            comboAxis.SelectedIndex = 0;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)position.Value, true);
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveAxis(Convert.ToInt32(movingStep.Text), false);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveAxis(-Convert.ToInt32(movingStep.Text), false);
        }

        private void MoveAxis(float pos, bool abs)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            if (axis.IsMoveDone() == false)
            {
                axis.StopMove();
                return;
            }

            if (abs == false)
                pos += axis.GetActualPos();

            axis.StartMove(pos, null);
        }

        private void originButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            if (axis.IsMoveDone() == false)
                return;

            originButton.Enabled = false;
            axis.HomeMove();
            originButton.Enabled = true;
        }

        private void axisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            position.Value = 0;
            axisParam = axis.AxisParam;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void MotionControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            axisConfiguration.SaveConfiguration();
            //if (Modal == false)
            //{
            //    e.Cancel = true;
            //    Hide();
            //}
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            Close();
            //if (Modal == true)
            //    Close();
            //else
            //    Hide();
        }

        private void buttonFindLimit_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            if (axis.IsMoveDone() == false)
                return;

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                MovingParam movingParam = axisParam.MovingParam.Clone();
                movingParam.MaxVelocity /= 2;
                axis.ContinuousMove(movingParam, true);

                TimeOutTimer timeOutTimer = new TimeOutTimer();
                timeOutTimer.Start(20000);
                while (axis.IsNegativeOn() == false)
                {
                    if (timeOutTimer.TimeOut == true)
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.CantFindNegLimit, ErrorLevel.Error,
                            axis.Name, "Can't Find Neg Limit.", null);
                        return;
                    }

                    Thread.Sleep(10);
                }
                timeOutTimer.Reset();

                axis.StopMove();

                Thread.Sleep(1000);
                axis.AxisParam.NegativeLimit = axis.GetActualPulse() /*+ offset*/;

                axis.ContinuousMove(movingParam);

                timeOutTimer.Start(20000);
                while (axis.IsPositiveOn() == false)
                {
                    if (timeOutTimer.TimeOut == true)
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.CantFindPosLimit, ErrorLevel.Error,
                            axis.Name, "Can't Find Pos Limit.", null);
                        return;
                    }

                    Thread.Sleep(10);
                }

                axis.StopMove();
                Thread.Sleep(1000);

                axis.AxisParam.PositiveLimit = axis.GetActualPulse() /*- offset*/;
                axis.Move(0);
            });

            axisParam = axis.AxisParam;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {
            Axis axis = (Axis)comboAxis.Items[comboAxis.SelectedIndex];

            MotionStatus motionStatus = axis.GetMotionStatus();

            labelOrg.BackColor = (motionStatus.origin ? Color.LightGreen : Color.Transparent);
            labelEz.BackColor = (motionStatus.ez ? Color.LightGreen : Color.Transparent);
            labelEmg.BackColor = (motionStatus.emg ? Color.Red : Color.Transparent);
            labelInp.BackColor = (motionStatus.inp ? Color.LightGreen : Color.Transparent);
            labelAlarm.BackColor = (motionStatus.alarm ? Color.Red : Color.Transparent);
            labelLimitPos.BackColor = (motionStatus.posLimit ? Color.Red : Color.Transparent);
            labelLimitNeg.BackColor = (motionStatus.negLimit ? Color.Red : Color.Transparent);
            labelRun.BackColor = (motionStatus.run ? Color.LightGreen : Color.Transparent);
            labelErr.BackColor = (motionStatus.err ? Color.Red : Color.Transparent);
            labelHome.BackColor = (motionStatus.home ? Color.LightGreen : Color.Transparent);
            labelHomeOk.BackColor = (motionStatus.homeOk ? Color.LightGreen : Color.Transparent);
            labelCClr.BackColor = (motionStatus.cClr ? Color.LightGreen : Color.Transparent);
            labelSon.BackColor = (motionStatus.servoOn ? Color.LightGreen : Color.Transparent);
            labelARst.BackColor = (motionStatus.aRst ? Color.LightGreen : Color.Transparent);

            labelCurPositionP.Text = axis.GetActualPulse().ToString() + "[pls]";
            labelCurPositionU.Text = axis.GetActualPos().ToString() + "[um]";
        }

        private void labelAlarm_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            axis.ResetAlarm();

        }

        private void labelSon_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            bool on = axis.IsServoOn();
            axis.TurnOnServo(!on);
        }

        private void labelHomeOk_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            bool on = axis.Motion.IsMoveDone(axis.AxisNo);
            try
            {
                if (on)
                    axis.Motion.ClearHomeDone(axis.AxisNo);
            }
            catch { }
        }
    }
}
