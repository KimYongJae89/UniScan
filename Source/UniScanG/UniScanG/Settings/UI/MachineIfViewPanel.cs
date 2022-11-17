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

namespace UniScanG.Settings.UI
{
    public partial class MachineIfViewPanel : UserControl
    {
        MachineIfSetting machineIfSetting;
        bool onTask;

        public MachineIfViewPanel()
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

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            timerUpdate.Stop();
            bool isConnected = SystemManager.Instance().DeviceBox.MachineIf.IsConnected;

            this.Parent.Text = string.Format("Machine Interface {0} - {1}", SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfType.ToString(), isConnected ? "Connected" : "Disconnected");
            if (this.onTask == false && isConnected)
            {
                Task.Run(() =>            
                {
                    this.onTask = true;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (this.Visible == false)
                            break;

                        Enum command = (Enum)row.Cells[0].Value;
                        MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
                        MelsecMachineIfProtocol protocol = (MelsecMachineIfProtocol)list.GetProtocol(command);
                        MelsecMachineIfProtocol melsecMachineIfProtocol =
                            new MelsecMachineIfProtocol(protocol.Command, protocol.Use, protocol.WaitResponceMs, protocol.Address, true, protocol.SizeWord);

                        MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(melsecMachineIfProtocol);
                        responce?.WaitResponce();

                        UpdateValueCell(row, 3, responce);
                    }
                    this.onTask = false;
                });
            }
            timerUpdate.Interval = 500;
            if (this.IsDisposed == false)
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

            if (responce == null)
            {
                responce = row.Tag as MachineIfProtocolResponce;
                if (responce == null)
                    return;
            }

            string valueString = "";
            DynMvp.Devices.Comm.ReceivedPacket receivedPacket = responce.ReceivedPacket;
            if (responce.IsResponced == false)
                valueString = "!! No RESPONCED !!";
            else if (responce.IsGood == false)
                valueString = string.Format("!! {0} !!", receivedPacket?.ErrCode);
            else
            {
                bool isString = (bool)row.Cells[2].Value;
                if (isString)
                    valueString = responce.Convert2String();
                else
                    valueString = responce.ReciveData;
            }
            row.Tag = responce;
            row.Cells[column].Value = valueString;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                // 체크박스 클릭 후 Cell 이동해야만 이벤트 발생함.
                UpdateValueCell(dataGridView1.Rows[e.RowIndex], 3, null);
            }
            else if (e.ColumnIndex == 4)
            {
                Enum command = (Enum)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList.GetProtocol(command).Clone();
                string[] args = new string[] { (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value };
                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(melsecMachineIfProtocol, args);
                responce.WaitResponce();
                
                UpdateValueCell(dataGridView1.Rows[e.RowIndex], 5, responce);
            }
        }
        
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                // 체크박스 클릭 순간 반응하도록...
                e.Cancel = true;
                bool checke = (bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !checke;
                //UpdateValueCell(dataGridView1.Rows[e.RowIndex], 3, null);
            }
        }
    }
}