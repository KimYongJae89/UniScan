using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.MotionController;
using DynMvp.Base;

namespace DynMvp.Devices.UI
{
    public partial class RobotPositionForm : Form
    {
        private FovNavigator fovNavigator;
        private AxisPosition loadPosition = new AxisPosition();
        public AxisPosition LoadPosition
        {
            get { return loadPosition; }
            set 
            {
                if (value.IsEmpty)
                    loadPosition = new AxisPosition();
                else
                    loadPosition = value;
            }
        }

        private AxisPosition unloadPosition = new AxisPosition();
        public AxisPosition UnloadPosition
        {
            get { return unloadPosition; }
            set
            {
                if (value.IsEmpty)
                    unloadPosition = new AxisPosition();
                else
                    unloadPosition = value;
            }
        }

        private bool onUpdatingPosition = false;
        private AxisHandler axisHandler;

        public RobotPositionForm()
        {
            InitializeComponent();

            buttonMoveLoadPosition.Text = StringManager.GetString(this.GetType().FullName,buttonMoveLoadPosition.Text);
            buttonSetLoadPosition.Text = StringManager.GetString(this.GetType().FullName,buttonSetLoadPosition.Text);
            buttonMoveUnloadPosition.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUnloadPosition.Text);
            buttonSetUnloadPosition.Text = StringManager.GetString(this.GetType().FullName,buttonSetUnloadPosition.Text);
            groupBoxLoadPosition.Text = StringManager.GetString(this.GetType().FullName,groupBoxUnload.Text);
            groupBoxUnload.Text = StringManager.GetString(this.GetType().FullName,groupBoxUnload.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);


            this.fovNavigator = new FovNavigator();

            // 
            // fovNavigator
            // 
            this.fovNavigator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fovNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fovNavigator.Location = new System.Drawing.Point(3, 3);
            this.fovNavigator.Name = "fovNavigator";
            this.fovNavigator.Size = new System.Drawing.Size(409, 523);
            this.fovNavigator.TabIndex = 8;
            this.fovNavigator.TabStop = false;
            this.fovNavigator.Enable = true;
            this.fovNavigator.FovChanged = new FovChangedDelegate(fovNavigator_FovChanged);

            this.panelFovNavigator.Controls.Add(this.fovNavigator);
        }

        public void Initialize(AxisHandler axisHandler, AxisParam xAxisConfig, AxisParam yAxisConfig)
        {
            this.axisHandler = axisHandler; 
            this.fovNavigator.RobotStage = axisHandler;

            loadPosition = new AxisPosition();
            unloadPosition = new AxisPosition();
        }

        private void UpdatePositionValue(AxisPosition position)
        {
            onUpdatingPosition = true;

            xPosition.Value = (int)position[0];
            yPosition.Value = (int)position[1];

            onUpdatingPosition = false;
        }

        private void fovNavigator_FovChanged(int fovNo, PointF position)
        {
            AxisPosition movePosition = new AxisPosition();
            movePosition.SetPosition(position.X, position.Y, 0);

            axisHandler.Move(movePosition);

            UpdatePositionValue(movePosition);
        }

        private void buttonMoveLoadPosition_Click(object sender, EventArgs e)
        {
            axisHandler.Move(loadPosition);
            UpdatePositionValue(loadPosition);
        }

        private void buttonMoveUnloadPosition_Click(object sender, EventArgs e)
        {
            axisHandler.Move(unloadPosition);
            UpdatePositionValue(unloadPosition);
        }

        private void buttonSetLoadPosition_Click(object sender, EventArgs e)
        {
            loadPosition[0] = (float)xPosition.Value;
            loadPosition[1] = (float)yPosition.Value;
        }

        private void buttonSetUnloadPosition_Click(object sender, EventArgs e)
        {
            unloadPosition[0] = (float)xPosition.Value;
            unloadPosition[1] = (float)yPosition.Value;
        }

        private void position_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdatingPosition)
                return;

            AxisPosition movePosition = new AxisPosition();
            movePosition.SetPosition((float)xPosition.Value, (float)yPosition.Value, 0);

            axisHandler.Move(movePosition);
        }
    }
}
