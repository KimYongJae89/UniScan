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

namespace DynMvp.Device.UI
{
    public partial class MotionSpeedForm : Form
    {
        AxisConfiguration axisConfiguration;
        AxisHandler curAxisHandler;
        AxisParam axisParam = new AxisParam();

        public MotionSpeedForm()
        {
            InitializeComponent();

            labelAxisHandler.Text = StringManager.GetString(this.GetType().FullName,labelAxisHandler.Text);
            labelAxisNo.Text = StringManager.GetString(this.GetType().FullName,labelAxisNo.Text);
            labelJog.Text = StringManager.GetString(this.GetType().FullName,labelJog.Text);
            okbutton.Text = StringManager.GetString(this.GetType().FullName,okbutton.Text);

        }

        public void Intialize(AxisConfiguration axisConfiguration, AxisHandler curAxisHandler = null)
        {
            this.axisConfiguration = axisConfiguration;
            this.curAxisHandler = curAxisHandler;
        }

        private void MotionControlForm_Load(object sender, EventArgs e)
        {
            int axisHandlerIndex = 0;
            int index = 0;
            foreach (AxisHandler axisHandler in axisConfiguration)
            {
                comboAxisHandler.Items.Add(axisHandler);

                if (curAxisHandler != null && axisHandler == curAxisHandler)
                    axisHandlerIndex = index;
                index++;
            }

            comboAxisHandler.SelectedIndex = axisHandlerIndex;
            movingStep.SelectedIndex = 2;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)(Convert.ToInt32(movingStep.Text)));
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)(Convert.ToInt32(movingStep.Text) * (-1)));
        }

        private void MoveAxis(float pos)
        {
            curAxisHandler.RelativeMove(comboAxis.Text, pos);
        }

        private void axisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            Axis axis = (Axis)comboAxis.Items[selectedIndex];

            axisParam = axis.AxisParam;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void MotionControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modal == false)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            axisConfiguration.SaveConfiguration();

            if (Modal == true)
                Close();
            else
                Hide();
        }

        private void comboAxisHandler_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxisHandler.SelectedIndex;
            curAxisHandler = (AxisHandler)comboAxisHandler.Items[selectedIndex];

            for (int i = 0; i < curAxisHandler.NumAxis; i++)
                comboAxis.Items.Add(curAxisHandler[i]);

            comboAxis.SelectedIndex = 0;
        }
    }
}
