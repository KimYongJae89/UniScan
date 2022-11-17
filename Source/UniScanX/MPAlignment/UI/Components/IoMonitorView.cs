using DynMvp.Base;
using DynMvp.Device.Dio;
using ReaLTaiizor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanX.MPAlignment.Devices.DIO;
using DynMvp.Devices.Dio;
using UniEye.Base.Device;

namespace UniScanX.MPAlignment.UI.Components
{
    public partial class IoMonitorView : Form
    {
        private MPPortMap portMap;
        private DigitalIoHandler digitalIoHandler;
        bool IsRunning = false;
        private bool[] outputValueChecker;


        public IoMonitorView(DigitalIoHandler digitalIoHandler, MPPortMap portMap)
        {
            InitializeComponent();
            this.portMap = portMap;
            this.digitalIoHandler = digitalIoHandler;
           
            InitPortTable();
            DoubleBuffered(dgvOutPorts, true);
            DoubleBuffered(dgvInPorts, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        void InitPortTable()
        {
            dgvInPorts.Rows.Clear();
            dgvOutPorts.Rows.Clear();

            var digitalIo = digitalIoHandler.Get(0);
            if (digitalIo == null)
                return;

            int numInPorts = digitalIo.NumInPort;
            int index = 0;
            List<string> inportNames = portMap.inPorts.GetPortNames(0, numInPorts);
            foreach (string portName in inportNames)
            {
                dgvInPorts.Rows.Add(index, portName);
                index++;
            }

            int numOutPorts = digitalIo.NumOutPort;

            index = 0;
            List<string> outportNames = portMap.outPorts.GetPortNames(0, numInPorts);
            foreach (string portName in outportNames)
            {
                dgvOutPorts.Rows.Add(index, portName);
                index++;
            }
            outputValueChecker = new bool[index];
        }

        public void Run()
        {
            if (IsRunning)
            {
                LogHelper.Debug(LoggerType.IO, "IoMonitorView already running");
                return;
            }
            IsRunning = true;
            UpdateData();
        }

        async void UpdateData()
        {
            while(true)
            {
                this.SuspendLayout();

                int deviceIndex = 0;
                var digitalIo = digitalIoHandler.Get(deviceIndex);
                if (digitalIo == null)
                    return;

                int numInPorts = digitalIo.NumInPort;

                for (int i = 0; i < dgvInPorts.Rows.Count; i++)
                {
                    uint inputValue;

                    inputValue = digitalIoHandler.ReadInputGroup(0, 0);

                    bool value = ((inputValue >> i) & 0x1) == 1;
                    if (value)
                    {
                        dgvInPorts.Rows[i].Cells[2].Value = Properties.Resources.add_48;
                    }
                    else
                    {
                        dgvInPorts.Rows[i].Cells[2].Value = Properties.Resources.delete_48;
                    }
                }

                int numOutPorts = digitalIo.NumOutPort;
                uint outputValue = digitalIoHandler.ReadOutputGroup(0,0);
                for (int i = 0; i < dgvOutPorts.Rows.Count; i++)
                {
                    bool value = ((outputValue >> i) & 0x1) == 1;
                    outputValueChecker[i] = value;
                    if (value)
                    {
                        dgvOutPorts.Rows[i].Cells[2].Value = Properties.Resources.add_48;
                    }
                    else
                    {
                        dgvOutPorts.Rows[i].Cells[2].Value = Properties.Resources.delete_48;
                    }
                }
                this.ResumeLayout();
                await Task.Delay(50).ConfigureAwait(true);
            }
        }

        private void IoMonitorView_Load(object sender, EventArgs e)
        {
            Run();
        }

        private void dgvOutPorts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2)
                return;

            int index = e.RowIndex;

            bool value = outputValueChecker[index];

            string portName = "";
            object cellValue = dgvOutPorts.Rows[e.RowIndex].Cells[1].Value;
            if (cellValue != null)
            {
                portName = cellValue.ToString();
            }
            IoPort ioPort = new IoPort(portName, 0, 0);
            ioPort.Set(e.RowIndex, 0, 0);

            LogHelper.Debug(LoggerType.IO, String.Format("Write Output : {0} -> {1}", e.RowIndex, (!value).ToString()));
            digitalIoHandler.DigitalIoList[0].WriteOutputPort(0, ioPort.PortNo, !value);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReceiveOnce_Click(object sender, EventArgs e)
        {
            //convUnit.ReceiveOnce();
        }

        private void btnReceiveBackOnce_Click(object sender, EventArgs e)
        {
            //convUnit.ReceiveBack();
        }

        private void btnEjectPcb_Click(object sender, EventArgs e)
        {
            //convUnit.EjectPCB();
        }

        private void btnStopReceive_Click(object sender, EventArgs e)
        {
            //convUnit.StopReceive();
        }

        private void btnReceiveMulti_Click(object sender, EventArgs e)
        {
            //convUnit.ReceiveMulti();
        }

        private void btnReceiveBackMulti_Click(object sender, EventArgs e)
        {
            //convUnit.ReceiveBackMulti();
        }

        private void btnStartFlush_Click(object sender, EventArgs e)
        {
            //convUnit.StartFlush();
        }

        private void btnStopFlush_Click(object sender, EventArgs e)
        {
            //convUnit.StopFlush();
        }

     
    }
}
