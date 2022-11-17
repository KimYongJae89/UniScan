using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Device.Serial;
using System.Threading;

namespace DynMvp.Devices.UI
{
    public partial class SerialEncoderPanel : UserControl
    {
        Dictionary<Enum, string> dataSource = new Dictionary<Enum, string>();
        ThreadHandler threadHandler = null;

        public SerialEncoder SerialEncoder { get => this.serialEncoder; }
        SerialEncoder serialEncoder;

        public SerialEncoderInfo SerialEncoderInfo { get => (SerialEncoderInfo)this.serialEncoder.DeviceInfo; }

        public SerialEncoderPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.MinimumSize = new Size(480, 640);

            this.Disposed += SerialEncoderPanel_Disposed;
            
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(buttonAp, "Get Position");
            toolTip.SetToolTip(buttonCp, "Clear Position");
            toolTip.SetToolTip(buttonPc, "Pulse Count");
            toolTip.SetToolTip(buttonCc, "Clear Count");
        }

        public void Initialize(SerialEncoder serialEncoder)
        {
            this.serialEncoder = serialEncoder;

            buttonPc.Visible = (serialEncoder is SerialEncoderV107);
            buttonCc.Visible = (serialEncoder is SerialEncoderV107);
        }

        private void SerialEncoderPanel_Disposed(object sender, EventArgs e)
        {
            if (threadHandler != null)
            {
                threadHandler.Stop();
                ThreadManager.RemoveThread(threadHandler);
            }

            UpdateInfo();
        }

        private void SerialEncoderPanel_Load(object sender, EventArgs e)
        {
            this.umPerPulse.DataBindings.Add(new Binding("Value", SerialEncoderInfo, "InputResolution", false, DataSourceUpdateMode.OnPropertyChanged));

            UpdateDataSoruce();

            UpdateDataGrid();

            threadHandler = new ThreadHandler("SerialEncoderPanel", new System.Threading.Thread(WorkingThread), false);
            ThreadManager.AddThread(threadHandler);
            threadHandler.WorkingThread.Start();
        }

        private void UpdateDataGrid()
        {
            dataGridView1.Columns.Clear();
            DataGridViewColumn colName = new DataGridViewTextBoxColumn();
            colName.ReadOnly = true;
            colName.HeaderText = "Name";
            dataGridView1.Columns.Add(colName);

            DataGridViewColumn colValue = new DataGridViewTextBoxColumn();
            colValue.ReadOnly = false;
            colValue.HeaderText = "Value";
            dataGridView1.Columns.Add(colValue);

            dataGridView1.RowCount = dataSource.Count;

            dataGridView1.Invalidate();
        }

        private void UpdateDataSoruce()
        {
            lock (dataSource)
            {
                dataSource.Clear();

                if (serialEncoder.IsCompatible(SerialEncoderV105.ECommand.AP))
                    ExcuteCommand(SerialEncoderV105.ECommand.AP);

                if (serialEncoder.IsCompatible(SerialEncoderV107.ECommandV2.PC))
                    ExcuteCommand(SerialEncoderV107.ECommandV2.PC);

                if (serialEncoder.IsCompatible(SerialEncoderV105.ECommand.GR))
                    ExcuteCommand(SerialEncoderV105.ECommand.GR);
            }
        }

        private void ExcuteCommand(string packetString)
        {
            string excuteResult = serialEncoder.ExcuteCommand(packetString);
            string[] token = excuteResult?.Split(',');
            UpdateValue(token);

            if (token != null)
            {
                // send ok
                AppendLogText("TX >>    [ OK ] : " + packetString);

                if (token.Length > 0)
                {
                    // recive ok
                    StringBuilder sb = new StringBuilder();
                    sb.Append(token[0]);
                    for (int i = 1; i < token.Length; i++)
                        sb.AppendFormat(",{0}", token[i]);
                    string tokenString = sb.ToString().Trim();
                    AppendLogText("   << RX [ OK ] : " + tokenString);
                }
                else
                {
                    // recive fail
                    AppendLogText("   << RX [FAIL] : ");
                }
            }
            else
            {
                // send Fail
                AppendLogText("TX >>    [FAIL] : " + packetString);

            }
        }

        private delegate void AppendLogTextDelegate(string v);
        private void AppendLogText(string v)
        {
            if (textBox1.IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new AppendLogTextDelegate(AppendLogText), v);
                return;
            }

            textBox1.AppendText("\r\n" + v);
        }

        private void ExcuteCommand(Enum en, params string[] args)
        {
            if (serialEncoder.IsCompatible(en))
            {
                //serialEncoder.ExcuteCommand(en, args);
                string packetString = serialEncoder.MakePacket(en.ToString(), args);
                ExcuteCommand(packetString);
            }
        }

        private void WorkingThread()
        {
            while (threadHandler.RequestStop == false)
            {
                if (checkBoxAutoUpdate.Checked)
                {
                    ExcuteCommand(SerialEncoderV105.ECommand.AP);
                    ExcuteCommand(SerialEncoderV107.ECommandV2.PC);
                }
                Thread.Sleep(500);
            }
        }

        private void UpdateValue(string[] token)
        {
            if (token == null)
                return;

            //for (int i = 0; i < token.Count(); i++)
            //{
            //    string s = string.Join("", token[i].Where(char.IsLetter));
            //    if (serialEncoder.IsCompatible(s))
            //    {
            //        Enum e = serialEncoder.GetCommand(s);

            //        lock (dataSource)
            //        {
            //            switch (e)
            //            {
            //                case SerialEncoderV105.ECommand.AR:
            //                    dataSource[e] = string.Format("{0},{1}", token[i + 1], token[i + 2]);
            //                    break;
            //                default:
            //                    dataSource[e] = token[i + 1];
            //                    break;
            //            }
            //        }
            //    }
            //}

            int i = 0;
            while (i < token.Length)
            {
                string comm = token[i];
                if (!serialEncoder.IsCompatible(comm))
                {
                    i++;
                    continue;
                }

                Enum e = serialEncoder.GetCommand(comm);
                //int next = Array.FindIndex(token, i + 1, f => serialEncoder.IsCompatible(f));
                //int next = i + ((comm == SerialEncoder.ECommand.AR.ToString()) ? 2 : 1);
                int next = i + serialEncoder.GetArgumentSize(e) + 1;
                if (next < 0)
                    next = token.Length;

                string[] values = token.Skip(i + 1).Take(next - i - 1).ToArray();

                lock (dataSource)
                    dataSource[e] = string.Join(",", values);

                i = next;
            }

            dataGridView1.RowCount = dataSource.Count;
            dataGridView1.Invalidate();

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            Type type = SerialEncoderInfo.GetType();
            lock (dataSource)
            {
                Dictionary<Enum, string>.Enumerator en = dataSource.GetEnumerator();
                while (en.MoveNext())
                {
                    KeyValuePair<Enum, string> pair = en.Current;

                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pair.Key.ToString());
                    if (propertyInfo != null)
                        propertyInfo.SetValue(SerialEncoderInfo, pair.Value);
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button == buttonEn0)
                ExcuteCommand(SerialEncoderV105.ECommand.EN, "0");
            else if (button == buttonEn1)
                ExcuteCommand(SerialEncoderV105.ECommand.EN, "1");
            else if (button == buttonIn0)
                ExcuteCommand(SerialEncoderV105.ECommand.IN, "0");
            else if (button == buttonIn1)
                ExcuteCommand(SerialEncoderV105.ECommand.IN, "1");
            else if (button == buttonGr)
                ExcuteCommand(SerialEncoderV105.ECommand.GR);
            else if (button == buttonCp)
                ExcuteCommand(SerialEncoderV105.ECommand.CP);
            else if (button == buttonCc)
                ExcuteCommand(SerialEncoderV107.ECommandV2.CC);

            //ExcuteCommand(SerialEncoderV105.ECommand.GR);
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            lock (dataSource)
            {
                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = dataSource.ElementAt(e.RowIndex).Key;
                        break;
                    case 1:
                        e.Value = dataSource.ElementAt(e.RowIndex).Value;
                        break;
                }
            }
        }

        private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            string[] token = null;
            switch (e.ColumnIndex)
            {
                case 1:
                    Enum en = dataSource.ElementAt(e.RowIndex).Key;
                    //SerialEncoder.ECommand key = dataSource.ElementAt(e.RowIndex).Key;
                    string value = e.Value.ToString();
                    ExcuteCommand(en, value);
                    break;
            }

            token = serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.GR).Split(',');
            UpdateValue(token);
        }

        private void ManualCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string packetString = manualCommand.Text;
                manualCommand.Text = "";
                ExcuteCommand(packetString);
            }
        }
    }
}

