using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;

namespace UniEye.Base.Settings.UI
{
    public partial class MachineIfTestForm : Form
    {
        private delegate void AddGridDataRowDelegate(string adr, string d1, string r1, string d2, string r2);

        string[] deviceCodes = new string[] { "A", "B", "C", "D" };
        Task task = null;
        CancellationTokenSource cancellationTokenSource = null;
        AddGridDataRowDelegate AddGridDataRow = null;

        string dCode;
        uint startAddr, endAddr;
        int repeat;
        int sendFail = 0;
        int sendSuccess = 0;
        DateTime dateTime = DateTime.Now;

        public MachineIfTestForm()
        {
            InitializeComponent();

            AddGridDataRow = MachineIfTestForm_AddGridDataRow;
        }

        internal void Initialize()
        {

        }

        private void MachineIfTestForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(deviceCodes);
            textBox1.Text = "0000";
            textBox2.Text = "2FFF";
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                StopTest();
            }
            else
            {
                if (Verify() == false)
                {
                    MessageForm.Show(null, "Check parameter");
                    return;
                }
                StartTest();
            }
        }

        private void StartTest()
        {
            dataGridView1.Rows.Clear();
            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(TaskProc, cancellationTokenSource.Token);
            task.Start();
            //buttonStartStop.Text = "Start";
        }

        private void StopTest()
        {
            cancellationTokenSource.Cancel();
            while (task != null && (task.IsCompleted == false && task.IsCompleted == false))
            {
                Thread.Sleep(10);
            }

            //buttonStartStop.Text = "End";
        }

        private bool Verify()
        {
            try
            {
                dCode = comboBox1.Text.Substring(0, 1);
                if (dCode.All(f => char.IsLetter(f)) == false)
                    return false;

                int dec = checkBoxHexadecimal.Checked ? 16 : 10;
                startAddr = Convert.ToUInt32(textBox1.Text, dec);
                endAddr = Convert.ToUInt32(textBox2.Text, dec);
                repeat = Convert.ToInt32(textBox3.Text);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private delegate void TestProcDoneDelegate(object sender, EventArgs e);
        private void TaskProc()
        {
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            
            MachineIfProtocol machineIfProtocol = machineIf.MachineIfSetting.MachineIfProtocolList.GetProtocol((Enum)null);
            Enum command = machineIfProtocol.Command;

            string[] testData = { "FFFF", "0000" };
            sendSuccess = sendFail = 0; ;
            dateTime = DateTime.Now;

            for (int r = 0; r < repeat; r++)
            {
                for (uint adr = startAddr; adr <= endAddr; adr++)
                {
                    string[] verifyData = new string[2];
                    string address = string.Format("{0}{1}", dCode, adr);
                    for (int i = 0; i < 2; i++)
                    //Test 0x0000/0xffff
                    {
                        for (int j = 0; j < 2; j++)
                        // Write/Verify
                        {
                            MelsecMachineIfProtocol melsecMachineIfProtocol = new MelsecMachineIfProtocol(command, true, 500, address, j == 1, 1);
                            string arg = testData[i];

                            if (j == 0)
                                arg = testData[i];

                            MachineIfProtocolResponce machineIfProtocolResponce = machineIf.SendCommand(melsecMachineIfProtocol, arg);
                            if (j == 1)
                            {
                                if (machineIfProtocolResponce.IsResponced)
                                {
                                    verifyData[i] = machineIfProtocolResponce.ReciveData;
                                    if (testData[i] == verifyData[i])
                                        sendSuccess++;
                                    else
                                        sendFail++;
                                }
                            }
                        }
                    }
                    AddGridDataRow(address, testData[0], verifyData[0], testData[1], verifyData[1]);
                    if (cancellationTokenSource.IsCancellationRequested)
                        break;
                }
            }

            this.task = null;
            StopTest();
        }

        private delegate int AddRowDelegate(DataGridViewRow row);

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (task == null)
                return;

            double spd = (this.sendSuccess / (DateTime.Now - dateTime).TotalSeconds);
            toolStripStatusLabel1.Text = string.Format("Sent: {0}", this.sendSuccess + this.sendFail);
            toolStripStatusLabel2.Text = string.Format("Fail: {0}", this.sendFail);
            toolStripStatusLabel3.Text = string.Format("Spd: {0:F1}", spd);
        }

        private void MachineIfTestForm_AddGridDataRow(string adr, string d1, string r1, string d2, string r2)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(this.dataGridView1);
            newRow.SetValues(adr, d1, r1, d2, r2, (d1 == r1 && d2 == r2));

            int lastDisplayedRowIndex = dataGridView1.DisplayedRowCount(false) + dataGridView1.FirstDisplayedScrollingRowIndex;
            bool scroll = (lastDisplayedRowIndex == dataGridView1.Rows.Count - 1);

            if (InvokeRequired)
            {
                BeginInvoke(new AddGridDataRow2Delegate(AddGridDataRow2), newRow, scroll);
            }
            else
            {
                AddGridDataRow2(newRow, scroll);
            }
        }

        private delegate void AddGridDataRow2Delegate(DataGridViewRow dataGridViewRow, bool scroll);

        private void buttonBinary_Click(object sender, EventArgs e)
        {

        }

        private void AddGridDataRow2(DataGridViewRow dataGridViewRow, bool scroll)
        {
            int newRowIdx = dataGridView1.Rows.Add(dataGridViewRow);
            if (scroll)
                dataGridView1.FirstDisplayedScrollingRowIndex = newRowIdx;
        }

    }
}
