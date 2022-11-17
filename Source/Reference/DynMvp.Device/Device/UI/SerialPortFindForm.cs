using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class SerialPortFindForm : Form
    {
        enum EColumn { Port, BaudRate, Result };

        DataTable DataTable { get; set; }
        BindingSource BindingSource { get; set; }
        CancellationTokenSource CancellationTokenSource { get; set; }

        public ISerialDeviceInfo SerialDeviceInfo { get; private set; }
        public string[] ScanPorts { get; private set; }
        public int[] ScanBaudRates { get; private set; }

        public string SelectedPort { get; set; }
        public int SelectedBaudRate { get; set; }

        public SerialPortFindForm(ISerialDeviceInfo serialDeviceInfo, string[] scanPorts, int[] scanBaudRates)
        {
            InitializeComponent();

            this.SerialDeviceInfo = serialDeviceInfo;
            this.ScanPorts = scanPorts;
            this.ScanBaudRates = scanBaudRates;

            this.dataGridView.MultiSelect = false;
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoGenerateColumns = true;
            //this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            //    DataPropertyName = "Port",
            //    HeaderText = "Port"
            //});
            //this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            //    DataPropertyName = "BaudRate",
            //    HeaderText = "BaudRate"
            //});
            //this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            //    DataPropertyName = "Result",
            //    HeaderText = "Result"
            //});

            this.DataTable = new DataTable();
            this.DataTable.Columns.Add(EColumn.Port.ToString());
            this.DataTable.Columns.Add(EColumn.BaudRate.ToString());
            this.DataTable.Columns.Add(EColumn.Result.ToString());

            this.BindingSource = new BindingSource();
            BindingSource.DataSource = this.DataTable;
            this.dataGridView.DataSource = this.BindingSource;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1)
            {
                DataRowView rowView = (DataRowView)this.dataGridView.SelectedRows[0].DataBoundItem;
                string port = (string)rowView.Row[EColumn.Port.ToString()];
                int baudRate = int.Parse((string)rowView.Row[EColumn.BaudRate.ToString()]);
                string result = (string)rowView.Row[EColumn.Result.ToString()];

                if (result != "OK")
                {
                    DialogResult dialogResult = MessageBox.Show(this, $"Port:{port}, Baud:{baudRate} is not Founded. Continue?", "", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                        return;
                }

                this.SelectedPort = port;
                this.SelectedBaudRate = baudRate;

                this.CancellationTokenSource.Cancel();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.CancellationTokenSource.Cancel();
            this.DialogResult = DialogResult.Abort;
        }

        private void SerialPortFindForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Com Scan for {this.SerialDeviceInfo.GetDeviceString()}";
            this.showFoundOnly.Checked = true;

            string portFindString = null;
            try
            {
                portFindString = this.SerialDeviceInfo.GetPortFindString();
            }
            catch (Exception ex) { }

            if (string.IsNullOrEmpty(portFindString))
            {
                MessageBox.Show(this, $"{this.SerialDeviceInfo.GetDeviceString()} is not support port scan");
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            this.CancellationTokenSource = new CancellationTokenSource();
            Task task = Task.Run(new Action(FindPort), this.CancellationTokenSource.Token);
        }

        private void FindPort()
        {
            int loopMax = this.ScanPorts.Length * this.ScanBaudRates.Length;
            UiHelper.SetProgressBarMinMax(this.progressBar, 0, loopMax);

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            PacketParser packetParser = this.SerialDeviceInfo.CreatePacketParser();
            packetParser.DataReceived += new DataReceivedDelegate((f) =>
            {
                Console.WriteLine($"SerialPortSettingForm::FindPort - DataReceived.");
                Console.WriteLine($"SerialPortSettingForm::FindPort - {string.Join("/", f.ReceivedData.Select(g => g.ToString("X02")))}");
                bool isPortFound = this.SerialDeviceInfo.IsPortFound(f.ReceivedData);
                if (isPortFound)
                    manualResetEvent.Set();
            });

            string protFindTest = this.SerialDeviceInfo.GetPortFindString();
            if (string.IsNullOrEmpty((protFindTest)))
            {
                MessageBox.Show(this, $"{this.SerialDeviceInfo.GetDeviceString()} is not support.");
                return;
            }

            SerialPortInfo serialPortInfo = this.SerialDeviceInfo.SerialPortInfo.Clone();
            byte[] dummyBytes = packetParser.EncodePacket("");

            for (int i = 0; i < this.ScanPorts.Length; i++)
            {
                string portName = this.ScanPorts[i];
                serialPortInfo.PortName = portName;

                for (int j = 0; j < this.ScanBaudRates.Length; j++)
                {
                    int baudRate = this.ScanBaudRates[j];
                    serialPortInfo.BaudRate = baudRate;

                    UiHelper.SetProgressBarValue(this.progressBar, this.ScanBaudRates.Length * i + j);

                    if (this.CancellationTokenSource.IsCancellationRequested)
                        continue;

                    using (SerialPortEx serialPortEx = new SerialPortEx(packetParser))
                    {
                        try
                        {
                            manualResetEvent.Reset();

                            serialPortEx.Open("", serialPortInfo);

                            // 포트 오픈 전 Dummy Data가 수신측 버퍼에 쌓여있을 수 있음 -> EndChars로 클리어.
                            if (dummyBytes.Length > 0)
                                serialPortEx.WritePacket(dummyBytes, 0, dummyBytes.Length);

                            serialPortEx.StartListening();

                            Console.WriteLine($"SerialPortSettingForm::FindPort - {portName}_{baudRate}, {protFindTest}");
                            serialPortEx.WritePacket(protFindTest);

                            bool isFounded = manualResetEvent.WaitOne(200);
                            AddBindingSource(portName, baudRate, isFounded ? "OK" : "");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"SerialPortSettingForm::FindPort - {ex.GetType().Name}: {ex.Message}");
                            AddBindingSource(portName, baudRate, "ERR");
                        }
                    }
                    UiHelper.SetProgressBarValue(this.progressBar, this.ScanBaudRates.Length * i + j + 1);
                }
            }
            UiHelper.SetProgressBarValue(this.progressBar, loopMax);
        }

        private delegate void AddBindingSourceDelegate(string portName, int baudRate, string result);
        private void AddBindingSource(string portName, int baudRate, string result)
        {
            if(this.dataGridView.InvokeRequired)
            {
                this.dataGridView.Invoke(new AddBindingSourceDelegate(AddBindingSource), portName, baudRate, result);
                return;
            }

            //this.DataTable.Columns.Add("Port");
            //this.DataTable.Columns.Add("BaudRate");
            //this.DataTable.Columns.Add("Result");
            DataRow row = this.DataTable.NewRow();
            row[EColumn.Port.ToString()] = portName;
            row[EColumn.BaudRate.ToString()] = baudRate;
            row[EColumn.Result.ToString()] = result;
            this.DataTable.Rows.Add(row);
            
            //this.BindingSource.ResetBindings(false);
        }

        private void showFoundOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();

            if (showFoundOnly.Checked)
            {
                //this.dataGridView.DataSource = this.BindingSource.SupportsFiltering;
                 this.BindingSource.Filter = "Result = 'OK'";
            }
            else
            {
                this.BindingSource.Filter = null;
            }
        }
    }
}
