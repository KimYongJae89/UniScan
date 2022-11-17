using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.MachineInterface.UI
{
    public partial class ProtocolPanel : UserControl, IMachineIfPanel
    {
        MachineIfSetting machineIfSetting;

        public ProtocolPanel()
        {
            InitializeComponent();

        }

        public void SetDefault()
        {
            machineIfSetting.MachineIfProtocolList.Initialize(machineIfSetting.MachineIfType);
        }

        public void Apply()
        {

        }

        public void Initialize()
        {
            Initialize(this.machineIfSetting);
        }

        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = machineIfSetting;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Command",
                ReadOnly = true,
                DataPropertyName = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Use",
                DataPropertyName = "Use",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "WaitTimeMs",
                DataPropertyName = "WaitResponceMs",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });


            switch (machineIfSetting.MachineIfType)
            {
                case MachineIfType.None:
                case MachineIfType.TcpServer:
                case MachineIfType.TcpClient:
                    break;
                case MachineIfType.Melsec:
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Address",
                        DataPropertyName = "Address",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });

                    dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn()
                    {
                        HeaderText = "IsReadCommand",
                        DataPropertyName = "IsReadCommand",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "SizeWord",
                        DataPropertyName = "SizeWord",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });

                    dataGridView1.DataSource = machineIfSetting.MachineIfProtocolList.Dic.Values.Cast<MelsecMachineIfProtocol>().ToList();
                    break;

                case MachineIfType.IO:
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Device",
                        DataPropertyName = "DeviceNo",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Group",
                        DataPropertyName = "GroupNo",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Port",
                        DataPropertyName = "PortNo",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn()
                    {
                        HeaderText = "ActiveLow",
                        DataPropertyName = "ActiveLow",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.DataSource = machineIfSetting.MachineIfProtocolList.Dic.Values.Cast<IoMachineIfProtocol>().ToList();
                    break;

                case MachineIfType.AllenBreadley:
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Tag Name",
                        DataPropertyName = "TagName",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Offset[DWORD]",
                        DataPropertyName = "OffsetByte4",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Size[DWORD]",
                        DataPropertyName = "SizeByte4",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                    dataGridView1.DataSource = machineIfSetting.MachineIfProtocolList.Dic.Values.Cast<AllenBreadleyMachineIfProtocol>().ToList();
                    break;
            }



            dataGridView1.AutoResizeColumns();
        }

        private void AddMelsecColumns()
        {
            if (machineIfSetting.MachineIfProtocolList != null)
            {
                foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
                {
                    MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)pair.Value;

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1, pair.Key, melsecMachineIfProtocol.Use, melsecMachineIfProtocol.WaitResponceMs, melsecMachineIfProtocol.Address, melsecMachineIfProtocol.IsReadCommand, melsecMachineIfProtocol.SizeWord);
                    row.Tag = melsecMachineIfProtocol;

                    dataGridView1.Rows.Add(row);
                }
            }
        }

        public bool Verify()
        {
            foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
            {
                if (!pair.Value.IsValid)
                    return false;
            }
            return true;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
