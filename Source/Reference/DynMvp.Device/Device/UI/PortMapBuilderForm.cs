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
    public partial class PortMapBuilderForm : Form
    {
        private IDigitalIo digitalIo;
        private PortMapBase portMap;

        public PortMapBuilderForm(IDigitalIo digitalIo, PortMapBase portMap)
        {        
            InitializeComponent();

            labelInput.Text = StringManager.GetString(this.GetType().FullName,labelInput.Text);
            labelOutput.Text = StringManager.GetString(this.GetType().FullName,labelOutput.Text);
            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            this.digitalIo = digitalIo;
            this.portMap = portMap;
            //this.portMap.Load();
        }

        private void PortMapBuilderForm_Load(object sender, EventArgs e)
        {
            columnInputDeviceNo.Items.Add(0);
            columnOutputDeviceNo.Items.Add(0);

            //IDigitalIo digitalIo = digitalIoHandler.Get(0);
            if (digitalIo != null)
            {
                List<string> inputPortComboData = new List<string>();

                inputPortComboData.Add("None");

                int numInPort = digitalIo.GetNumInPort();
                for (int inputPort = 0; inputPort < numInPort; inputPort++)
                    inputPortComboData.Add(inputPort.ToString());

                columnInputPortNo.DataSource = inputPortComboData;
                
                List<string> outputPortComboData = new List<string>();

                outputPortComboData.Add("None");

                int numOutPort = digitalIo.GetNumOutPort();
                for (int outputPort = 0; outputPort < numOutPort; outputPort++)
                    outputPortComboData.Add(outputPort.ToString());

                columnOutputPortNo.DataSource = outputPortComboData;

                foreach(IoPort ioPort  in portMap.InPortList)
                {
                    if (ioPort != null)
                    {
                        string portNoString = "";
                        if (ioPort.PortNo == -1)
                            portNoString = "None";
                        else
                            portNoString = ioPort.PortNo.ToString();

                        if (inputPortComboData.Contains(portNoString) == false)
                            portNoString = "None";

                        inportTable.Rows.Add(ioPort.Name, 0, portNoString, ioPort.ActiveLow);
                    }
                    else
                    {
                        inportTable.Rows.Add("", 0, "None", false);
                    }
                }

                foreach (IoPort ioPort in portMap.OutPortList)
                {
                    if (ioPort != null)
                    {
                        string portNoString = "";
                        if (ioPort.PortNo == -1)
                            portNoString = "None";
                        else
                            portNoString = ioPort.PortNo.ToString();

                        if (outputPortComboData.Contains(portNoString) == false)
                            portNoString = "None";

                        outportTable.Rows.Add(ioPort.Name, 0, portNoString, ioPort.ActiveLow);
                    }
                    else
                    {
                        outportTable.Rows.Add("", 0, "None", false);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            PortList[] portLists = new PortList[] { portMap.InPortList, portMap.OutPortList };
            DataGridView[] portViews = new DataGridView[] { inportTable, outportTable };
            
            for (int i = 0; i < 2; i++)
            {
                portLists[i].Clear();
                for (int j = 0; j < portViews[i].Rows.Count; j++)
                {
                    string name = "";
                    if (portViews[i].Rows[j].Cells[0].Value != null)
                        name = portViews[i].Rows[j].Cells[0].Value.ToString();

                    int deviceNo = 0;
                    if (portViews[i].Rows[j].Cells[1].Value != null)
                        deviceNo = (int)portViews[i].Rows[j].Cells[1].Value;

                    int portNo = IoPort.UNUSED_PORT_NO;
                    if (portViews[i].Rows[j].Cells[2].Value.ToString() != "None")
                        portNo = Convert.ToInt32(portViews[i].Rows[j].Cells[2].Value);

                    bool invert = false;
                    //if (i == 0)
                    invert = Convert.ToBoolean(portViews[i].Rows[j].Cells[3].Value);

                    portLists[i].SetPort(portViews[i].Rows[j].Cells[0].Value.ToString(), Convert.ToInt32(portViews[i].Rows[j].Cells[1].Value), portNo, invert);
                }
            }

            //portMap.Save();
        }
    }
}
