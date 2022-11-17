using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using DynMvp.Base;

namespace UniScanG.Gravure.UI.Inspect
{
    public delegate void ListBox_SelectedIndexChangedDelegate(int panelIndex, int rowIndex);
    public partial class InspectObserverPanel : UserControl
    {
        const int maxItems = 10000;
        List<Tuple<DateTime, TimeSpan, int>> tupleList = null;
        int panelIndex = -1;

        public ListBox_SelectedIndexChangedDelegate listBox_SelectedIndexChangedDelegate = null;

        public InspectObserverPanel(string name, int panelIndex)
        {
            InitializeComponent();

            tupleList = new List<Tuple<DateTime, TimeSpan, int>>();

            this.panelIndex = panelIndex;
            this.name.Text = name;
        }

        public delegate void ClearDelegate();
        public void Clear()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ClearDelegate(Clear));
                return;
            }

            try
            {
                comboBox1.Items.Clear();
                listBox.Items.Clear();

                tupleList?.Clear();

                count.Text = "0";
                time.Text = "00:00:00.000";
                term.Text = TimeSpan.Zero.ToString(@"ss\.fff");
                chart.Series[0].Points.Clear();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, string.Format("InspectObserverPanel::Clear() - {0}", ex.Message));
            }
        }

        public void AddData(int sheetNo)
        {
            DateTime dt = DateTime.Now;
            TimeSpan ts = TimeSpan.Zero;

            if (tupleList.Count > 0)
                ts = dt - tupleList.Last().Item1;

            Tuple<DateTime, TimeSpan, int> tuple = new Tuple<DateTime, TimeSpan, int>(dt, ts, sheetNo);

            string listBoxItem = string.Format("{0:00000} : [{1}] {2}", tupleList.Count,DateTime.Now.ToString("HH:mm:ss.fff"), tuple.Item3);
            string timeItem = tuple.Item1.ToString(@"HH\:mm\:ss\.fff");
            string termItem = tuple.Item2.ToString(@"ss\.fff");
            double chartItem = tuple.Item2.TotalMilliseconds;

            lock (tupleList)
                tupleList.Add(tuple);

            string countItem = tupleList.Count().ToString();

            if(this.Visible)
                ShowData(listBoxItem, countItem, timeItem, termItem, chartItem);
        }

        private delegate void ShowDataDelegate(string listBoxItem, string countItem, string timeItem, string termItem, double chartItem);
        private void ShowData(string listBoxItem, string countItem, string timeItem, string termItem, double chartItem)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ShowDataDelegate(ShowData), listBoxItem, countItem, timeItem, termItem, chartItem);
                return;
            }

            if (tupleList.Count % maxItems == 1)
            {
                int newRow = comboBox1.Items.Add(string.Format("{0:00000} - {1:00000}", tupleList.Count - 1, tupleList.Count + maxItems - 2));
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 2)
                {
                    comboBox1.SelectedIndex = newRow;
                    listBox.Items.Clear();
                    chart.Series[0].Points.Clear();
                }
            }

            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                listBox.SelectedIndex = listBox.Items.Add(listBoxItem);                
                chart.Series[0].Points.AddY(chartItem);
            }

            count.Text = countItem;
            time.Text = timeItem;
            term.Text = termItem;
        }

        private delegate void SelectRowDelegate(int rowIndex, bool pageAutoChange);
        public void SelectRow(int rowIndex, bool pageAutoChange)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SelectRowDelegate(SelectRow), rowIndex, pageAutoChange);
                return;
            }

            int pageNo = rowIndex / maxItems;
            int rowNo = rowIndex % maxItems;

            if (pageAutoChange)
                comboBox1.SelectedIndex = pageNo;

            if (comboBox1.SelectedIndex == pageNo)
            {
                listBox.SelectedIndex = Math.Min(rowNo, listBox.Items.Count - 1);
            }
        }

        public int Exist(int sheetNo)
        {
            return tupleList.FindIndex(f => f.Item3 == sheetNo);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_SelectedIndexChangedDelegate != null)
            {
                int sheetNo;
                if (comboBox1.SelectedIndex < 0)
                    sheetNo = listBox.SelectedIndex;
                else
                    sheetNo = comboBox1.SelectedIndex * maxItems + listBox.SelectedIndex;

                listBox_SelectedIndexChangedDelegate(panelIndex, sheetNo);
            }
        }

        internal int GetSheetNo(int rowIndex)
        {
            return tupleList[rowIndex].Item3;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;

            int srcIndx = comboBox1.SelectedIndex * maxItems;
            int dstIndx = (comboBox1.SelectedIndex + 1) * maxItems - 1;
            int dstIndx2 = Math.Min(dstIndx, tupleList.Count-1);

            listBox.Items.Clear();
            chart.Series[0].Points.Clear();
            for (int i = srcIndx; i <= dstIndx2; i++)
            {
                listBox.Items.Add(string.Format("{0:00000} : {1}", i, tupleList[i].Item3));
                chart.Series[0].Points.Add(tupleList[i].Item2.TotalMilliseconds);
            }
        }

        private void toolStripMenuSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CVS(*.csv) files|*.csv";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(this.name.Text);
                sb.AppendLine("No,Date,Time,Milisec,Term");
                lock (tupleList)
                {
                    tupleList.ForEach(f =>
                    {
                        string date = f.Item1.ToString("yyyy-MM-dd");
                        string time = f.Item1.ToString("HH:mm:ss");
                        string milisec = f.Item1.ToString("fff");
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", f.Item3,date,time,milisec,f.Item2.ToString("fff") ));
                    });
                }

                File.WriteAllText(dlg.FileName, sb.ToString());

            }
        }
    }
}
