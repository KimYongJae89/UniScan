using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.MotionController;
using System.Threading;

namespace DynMvp.Device.UI
{
    public partial class Joystick3AxisControl : UserControl, IJoystickControl
    {
        private AxisHandler axisHandler;
        private AxisPosition currentPosition;

        Thread continueousMoveThread = null;
        int stepOffset = 0;
        int axisNo = -1;

        bool startMove = false;

        public Joystick3AxisControl()
        {
            InitializeComponent();

            moveStep.Text = "1000";

            continueousMoveThread = new Thread(continueousMoveThreadProc);
        }

        public void InitControl()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Size = new System.Drawing.Size(409, 523);
            TabIndex = 8;
            TabStop = false;
        }

        private void continueousMoveThreadProc()
        {
            while (startMove)
            {
                currentPosition[axisNo] += stepOffset;
                axisHandler.StartMove(currentPosition);
                while (!axisHandler.IsMoveDone());
            }
        }

        public void Initialize(AxisHandler axisHandler)
        {
            this.axisHandler = axisHandler;
            currentPosition = new AxisPosition();
        }

        public void MoveAxis(int axisNo, int direction)
        {
            currentPosition = axisHandler.GetCommandPos();
            //currentPosition = axisHandler.GetActualPos();

            stepOffset = direction * Convert.ToInt32(moveStep.Text);
            this.axisNo = axisNo;

            if (checkStepMove.Checked)
            {   
                currentPosition[axisNo] += stepOffset;
                axisHandler.Move(currentPosition);
            }
            else
            {
                startMove = true;
                continueousMoveThread = new Thread(continueousMoveThreadProc);
                continueousMoveThread.Start();
            }
        }

        public void StopAxis()
        {
            startMove = false;
            axisHandler.StopMove();
        }

        private void buttonAll_MouseUp(object sender, MouseEventArgs e)
        {
            if (startMove)
            {
                StopAxis();
            }
        }

        private void buttonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.X, (int)Direction.Negative);
        }

        private void buttonRight_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.X, (int)Direction.Positive);
        }

        private void buttonUp_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.Y, (int)Direction.Positive);
        }
  
        private void buttonDown_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.Y, (int)Direction.Negative);
        }

        private void buttonFoward_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.Z, (int)Direction.Negative);
        }

        private void buttonBack_MouseDown(object sender, MouseEventArgs e)
        {
            MoveAxis((int)AxisName.Z, (int)Direction.Positive);
        }
    }
}