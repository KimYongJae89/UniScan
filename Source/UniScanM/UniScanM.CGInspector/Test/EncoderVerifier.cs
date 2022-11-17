using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniScanM.CGInspector.Test
{
    public partial class EncoderVerifier : Form
    {
        private class ListItem
        {
            public AxisPosition axisPosition;
            public string[] serialResponce;
            public ListItem(AxisPosition axisPosition, string[] serialResponce)
            {
                this.axisPosition = axisPosition;
                this.serialResponce = serialResponce;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(axisPosition.ToString());
                Array.ForEach(serialResponce, f => sb.AppendFormat(",{0}",f));
                string str = sb.ToString();
                return str.Trim(',');
            }
        }

        AxisHandler axisHandler = null;
        SerialDevice serialDevice = null;

        List<ListItem> listItemList = null;
        Thread thread = null;
        bool onRunning = false;
        bool onSaving = false;
        public EncoderVerifier()
        {
            InitializeComponent();

            listItemList = new List<ListItem>();
        }

        public void Initialize(AxisHandler axisHandler, SerialDevice serialDevice)
        {
            this.axisHandler = axisHandler;
            this.serialDevice = serialDevice;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (SystemState.Instance().OpState != OpState.Idle)
                return;

            if (onRunning)
                return;

            if (this.numericUpDown1.Value <= 0)
                return;

            // Init
            AxisPosition initPos = new AxisPosition(axisHandler.NumAxis);
            axisHandler.SetPosition(initPos);
            serialDevice.ExcuteCommand(SerialEncoderV105.ECommand.CP);

            this.dataGridView1.RowCount = 0;
            listItemList.Clear();

            thread = new Thread(Proc);
            onRunning = true;
            thread.Start();
        }

        private void Proc()
        {
            AxisPosition resPos = new AxisPosition(axisHandler.NumAxis);
            resPos.Add((float)this.numericUpDown1.Value);

            MovingParam movingParam = new MovingParam("", 100, 100, 100, 1000, 0);
            while (onRunning)
            {
                axisHandler.RelativeMove(resPos, movingParam);
                Thread.Sleep(100);

                AxisPosition axisPos = axisHandler.GetActualPos();
                string[] serialDeviceResponce = serialDevice.ExcuteCommand(SerialEncoderV105.ECommand.AP)?.Split(',');

                listItemList.Add(new ListItem(axisPos, serialDeviceResponce));

                UpdateGrid();

                Thread.Sleep(100);
            }
        }

        private delegate void UpdateGridDelegate();
        private void UpdateGrid()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateGridDelegate(UpdateGrid));
                return;
            }

            int visibleLastRow = this.dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed);
            bool autoScroll = (this.dataGridView1.RowCount-1 == visibleLastRow);

            this.dataGridView1.RowCount = listItemList.Count;
            if (autoScroll)
            {
                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.RowCount - 1;
                this.dataGridView1.Rows[this.dataGridView1.RowCount - 1].Selected = true;
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.onRunning = false;
            axisHandler.StopMove();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (listItemList.Count == 0)
                return;

            if (onSaving)
                return;

            string initFilePath = Path.GetFullPath(Path.Combine(PathSettings.Instance().Temp, "EncoderVerifier.txt"));

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text(*.txt)|*.txt";
            dlg.InitialDirectory = Path.GetDirectoryName(initFilePath);
            dlg.FileName = Path.GetFileName(initFilePath);

            if (UiHelper.ShowSTADialog(dlg) == DialogResult.Cancel)
                return;

            onSaving = true;
            Task task = new Task(() =>
            {
                StringBuilder sb = new StringBuilder();
                lock (listItemList)
                {
                    for (int i = 0; i < listItemList.Count; i++)
                    {
                        ListItem item = listItemList[i];
                        sb.AppendLine(string.Format("{0},{1}", i, item.ToString()));
                    }
                }
                File.WriteAllText(dlg.FileName, sb.ToString());
                MessageForm.Show(null, "GOOOOOD");
                onSaving = false;
            });
            task.Start();

        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ListItem item = listItemList[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = e.RowIndex + 1;
                    break;
                case 1:
                    e.Value = item.axisPosition;
                    break;
                case 2:
                    {
                        StringBuilder sb = new StringBuilder();
                        Array.ForEach(item.serialResponce, f => sb.AppendFormat("{0},",f));
                        e.Value = sb.ToString().Trim(',');
                    }
                    break;
            }
        }
    }
}
