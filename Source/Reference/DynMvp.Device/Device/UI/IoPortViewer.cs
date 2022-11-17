using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.Dio;
using DynMvp.Base;

namespace DynMvp.Devices.UI
{
    public partial class IoPortViewer : Form
    {
        private bool intialized = false;

        private PortMapBase portMap;
        private DigitalIoHandler digitalIoHandler;
        private bool[] outputValueChecker;
        private bool[] inputValueChecker;

        public IoPortViewer(DigitalIoHandler digitalIoHandler, PortMapBase portMap)
        {        
            InitializeComponent();
                                 
            this.inportTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.inportTable_CellMouseDown);
            this.inportTable.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.inportTable_CellMouseUp);

            labelDigitalIo.Text = StringManager.GetString(this.GetType().FullName,labelDigitalIo.Text);
            labelInput.Text = StringManager.GetString(this.GetType().FullName,labelInput.Text);
            labelOutput.Text = StringManager.GetString(this.GetType().FullName,labelOutput.Text);
            closeButton.Text = StringManager.GetString(this.GetType().FullName,closeButton.Text);
           
            this.digitalIoHandler = digitalIoHandler;
            this.portMap = portMap;

            int index = 1;
            foreach (IDigitalIo digitalIo in digitalIoHandler)
            { 
                comboBoxDigitalIo.Items.Add(String.Format("{0} : {1}", index, digitalIo.GetName()));
                index++;
            }

            if (comboBoxDigitalIo.Items.Count > 0)
                comboBoxDigitalIo.SelectedIndex = 0;

            InitGroupCombo();

            intialized = true;
        }

        private void IoPortViewer_Load(object sender, EventArgs e)
        {
            InitInPortTable();
            InitOutPortTable();

            timer.Start();
        }

        private void InitGroupCombo()
        {
            comboBoxInPortGroup.Items.Clear();
            comboBoxOutPortGroup.Items.Clear();

            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
                return;

            int numInportGroup = digitalIo.GetNumInPortGroup();
            if (numInportGroup > 0)
            {
                for (int i = 0; i < numInportGroup; i++)
                    comboBoxInPortGroup.Items.Add(String.Format("Group {0} ", i));
                comboBoxInPortGroup.SelectedIndex = 0;
            }

            int numOutportGroup = digitalIo.GetNumOutPortGroup();
            if (numOutportGroup > 0)
            {
                for (int i = 0; i < numOutportGroup; i++)
                    comboBoxOutPortGroup.Items.Add(String.Format("Group {0} ", i));
                comboBoxOutPortGroup.SelectedIndex = 0;
            }
        }

        private void InitInPortTable()
        {
            inportTable.Rows.Clear();

            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
                return;

            int groupIndex = comboBoxInPortGroup.SelectedIndex;
            int numInPorts = digitalIo.GetNumInPort();
            this.inputValueChecker = new bool[numInPorts];

            List<string> inportNames = portMap.InPortList.GetPortNames(deviceIndex, groupIndex, numInPorts);
            for (int i = 0; i < inportNames.Count; i++)
                inportTable.Rows.Add(i, inportNames[i], global::DynMvp.Device.Properties.Resources.led_off);
        }

        private void InitOutPortTable()
        {
            outportTable.Rows.Clear();

            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
                return;

            int groupIndex = comboBoxOutPortGroup.SelectedIndex;
            int numOutPorts = digitalIo.GetNumOutPort();
            this.outputValueChecker = new bool[numOutPorts];

            List<string> outportNames = portMap.OutPortList.GetPortNames(deviceIndex, groupIndex, numOutPorts);
            for (int i = 0; i < outportNames.Count; i++)
                outportTable.Rows.Add(i, outportNames[i], global::DynMvp.Device.Properties.Resources.Output_Off);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Visible == true)
                UpdatePortTable();
        }

        private void UpdatePortTable()
        {
            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
                return;

            int inPortGroupIndex = comboBoxInPortGroup.SelectedIndex;
            if (inPortGroupIndex >= 0)
            {
                int numInPorts = digitalIo.GetNumInPort();
                uint inputValue = digitalIoHandler.ReadInputGroup(deviceIndex, inPortGroupIndex);
                for (int i = 0; i < inportTable.Rows.Count; i++)
                {
                    bool value = ((inputValue >> i) & 0x1) == 1;

                    if (this.inputValueChecker[i] != value)
                    {
                        this.inputValueChecker[i] = value;
                        if (this.inputValueChecker[i])
                            inportTable.Rows[i].Cells[2].Value = global::DynMvp.Device.Properties.Resources.led_on;
                        else
                            inportTable.Rows[i].Cells[2].Value = global::DynMvp.Device.Properties.Resources.led_off;
                    }
                }
            }

            int outPortGroupIndex = comboBoxOutPortGroup.SelectedIndex;
            if (outPortGroupIndex >= 0)
            {
                int numOutPorts = digitalIo.GetNumOutPort();
                uint outputValue = digitalIoHandler.ReadOutputGroup(deviceIndex, outPortGroupIndex);
                for (int i = 0; i < outportTable.Rows.Count; i++)
                {
                    bool value = ((outputValue >> i) & 0x1) == 1;
                    if (this.outputValueChecker[i] != value)
                    {
                        this.outputValueChecker[i] = value;
                        if (this.outputValueChecker[i])
                            outportTable.Rows[i].Cells[2].Value = global::DynMvp.Device.Properties.Resources.Output_On;
                        else
                            outportTable.Rows[i].Cells[2].Value = global::DynMvp.Device.Properties.Resources.Output_Off;

                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Modal == true)
                Close();
            else
                Hide();
        }

        private void outportTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                int index = e.RowIndex;

                bool value = outputValueChecker[index];

                string portName = "";
                object cellValue = outportTable.Rows[e.RowIndex].Cells[1].Value;
                if (cellValue != null)
                {
                    portName = cellValue.ToString();
                }
                IoPort ioPort = new IoPort(portName);
                ioPort.Set(e.RowIndex, comboBoxOutPortGroup.SelectedIndex, comboBoxDigitalIo.SelectedIndex);

                LogHelper.Debug(LoggerType.IO, String.Format("Write Output : {0} -> {1}", e.RowIndex, (!value).ToString()));
                digitalIoHandler.WriteOutput(ioPort, !value);
            }
        }

        private void IoPortViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modal == false)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void comboBoxDigitalIo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
                InitGroupCombo();
        }

        private void inportTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                int index = e.RowIndex;

                bool value = inputValueChecker[index];

                string portName = "";
                object cellValue = inportTable.Rows[e.RowIndex].Cells[1].Value;
                if (cellValue != null)
                {
                    portName = cellValue.ToString();
                }
                IoPort ioPort = new IoPort(portName);
                ioPort.Set(e.RowIndex, comboBoxDigitalIo.SelectedIndex);

                LogHelper.Debug(LoggerType.IO, String.Format("Virtual Input : {0} -> {1}", e.RowIndex, (!value).ToString()));
                digitalIoHandler.WriteInput(ioPort, !value);
            }
        }

        private void inportTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void comboBoxInPortGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
                InitInPortTable();
        }

        private void comboBoxOutPortGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
                InitOutPortTable();
        }
    }
}
