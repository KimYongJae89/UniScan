using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;

namespace UniScanM.UI.ControlPanel
{
    public partial class MachineIfPanel : UserControl
    {
        MachineIfSetting machineIfSetting;
        bool onTask;

        public MachineIfPanel()
        {
            InitializeComponent();
            onTask = false;
        }


        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = machineIfSetting;

            // Initialize Columns
            DataGridViewColumn[] dataGridViewColumns = new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn {ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "Command"},
                new DataGridViewTextBoxColumn {ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "Address"},
                new DataGridViewCheckBoxColumn {ReadOnly = false, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "ShowString"},
                new DataGridViewTextBoxColumn {ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "Value"},
                new DataGridViewTextBoxColumn {ReadOnly = false, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "Set"},
                new DataGridViewTextBoxColumn {ReadOnly = false, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, HeaderText = "Result"}
            };

            dataGridView1.Columns.AddRange(dataGridViewColumns);
        }

        private void SettingPageMachineIfPanel_Load(object sender, EventArgs e)
        {
            if (machineIfSetting == null)
                return;

            switch (machineIfSetting.MachineIfType)
            {
                case MachineIfType.Melsec:
                    foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
                    {
                        MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)pair.Value;
                        if (melsecMachineIfProtocol.Use)
                        {
                            DataGridViewRow aRow = new DataGridViewRow();
                            aRow.CreateCells(dataGridView1, pair.Key, melsecMachineIfProtocol.Address, false, "");
                            if (melsecMachineIfProtocol.IsReadCommand)
                            {
                                aRow.Cells[4].Value = "Not.Supported";
                                aRow.Cells[4].ReadOnly = true;
                            }
                            dataGridView1.Rows.Add(aRow);
                        }
                    }
                    break;

                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
                    {
                        TcpIpMachineIfProtocol tcpIpMachineIfProtocol = (TcpIpMachineIfProtocol)pair.Value;
                        dataGridView1.Rows.Add(pair.Key, tcpIpMachineIfProtocol.Use, tcpIpMachineIfProtocol.WaitResponceMs);
                    }
                    break;
            }
            timerUpdate.Start();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            timerUpdate.Stop();
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            this.Parent.Text = string.Format("MachineIfTest Form - {0}", machineIf.IsConnected ? "Connected" : "Disconnected");

            if (this.onTask == false)
            {
                Task.Run(() =>            
                {
                    this.onTask = true;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        Enum command = (Enum)row.Cells[0].Value;
                        MelsecMachineIfProtocol protocol = (MelsecMachineIfProtocol)machineIf.MachineIfSetting.MachineIfProtocolList.GetProtocol(command);
                        MelsecMachineIfProtocol melsecMachineIfProtocol =
                            new MelsecMachineIfProtocol(protocol.Command, protocol.Use, protocol.WaitResponceMs, protocol.Address, true, protocol.SizeWord);

                        MachineIfProtocolResponce responce = machineIf.SendCommand(melsecMachineIfProtocol);
                        responce.WaitResponce();
                        UpdateValueCell(row, 3, responce);
                    }
                    this.onTask = false;
                });
            }
            timerUpdate.Interval = 100;
            timerUpdate.Start();
        }

        private delegate void UpdateValueCellDelegate(DataGridViewRow row, int column, MachineIfProtocolResponce responce);
        private void UpdateValueCell(DataGridViewRow row, int column, MachineIfProtocolResponce responce)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateValueCellDelegate(UpdateValueCell), row, column, responce);
                return;
            }

            string valueString;
            if (responce.IsResponced == false)
                valueString = "!! No RESPONCED !!";
            else if (responce.IsGood == false)
                valueString = "!! ERROR !!";
            else
            {
                bool isString = (bool)row.Cells[2].Value;
                if (isString)
                    valueString = responce.Convert2String();
                else
                    valueString = responce.ReciveData;
            }
            row.Cells[column].Value = valueString;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4)
                return;

            Enum command = (Enum)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList.GetProtocol(command);

            MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(melsecMachineIfProtocol,
                new string[] { (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value });

            UpdateValueCell(dataGridView1.Rows[e.RowIndex],5, responce);
        }
    }
}