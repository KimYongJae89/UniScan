using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Common.Data;
using UniScanG;
using UniScanG.Inspect;

namespace UniScanG.Gravure.UI.Inspect.Monitor
{
    public partial class InspectObserver : Form, IInspectObserver
    {
        InspectorObj[] inspectorObjs = null;
        List<Tuple<int, int, InspectObserverPanel>> panelList = null;
        int doneCount = 0;
        string title = "";
        public InspectObserver(InspectorObj[] inspectorObjs)
        {
            InitializeComponent();

            this.inspectorObjs = inspectorObjs;
            this.title = this.Text;

            this.panelList = new List<Tuple<int, int, InspectObserverPanel>>();

            tableLayoutPanel1.ColumnCount = inspectorObjs.Length;
            float width = 0;
            float height = 0;

            for (int i = 0; i < inspectorObjs.Length; i++)
            {
                InspectorObj inspectorObj = inspectorObjs[i];
                int camIdx = inspectorObj.Info.CamIndex;
                int clientIdx = inspectorObj.Info.ClientIndex;
                InspectObserverPanel inspectObserverPanel = new InspectObserverPanel(inspectorObj.Info.GetName(), i) { listBox_SelectedIndexChangedDelegate = listBox_SelectedIndexChanged };
                this.panelList.Add(new Tuple<int, int, InspectObserverPanel>(camIdx, clientIdx, inspectObserverPanel));

                tableLayoutPanel1.ColumnStyles[i].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[i].Width = inspectObserverPanel.Width + inspectObserverPanel.Margin.Left + inspectObserverPanel.Margin.Right + tableLayoutPanel1.Padding.Left + tableLayoutPanel1.Padding.Right;
                tableLayoutPanel1.RowStyles[0].Height = inspectObserverPanel.Height + inspectObserverPanel.Margin.Top + inspectObserverPanel.Margin.Bottom + tableLayoutPanel1.Padding.Top + tableLayoutPanel1.Padding.Bottom;

                width += tableLayoutPanel1.ColumnStyles[i].Width;
                height = Math.Max(height, tableLayoutPanel1.RowStyles[0].Height);
                this.tableLayoutPanel1.Controls.Add(this.panelList[i].Item3, i, 0);
            }

            tableLayoutPanel1.Width =(int)width;
            tableLayoutPanel1.Height =(int)height;

            ChangeTitle();

            //this.Show();
        }

        public void AddData(int index, int subIndex, int sheetNo)
        {
            try
            {
                InspectObserverPanel inspectObserverPanel = this.panelList.Find(f => f.Item1 == index && f.Item2 == subIndex)?.Item3;
                if (inspectObserverPanel != null)
                {
                    inspectObserverPanel.AddData(sheetNo);
                    CheckDone(index, sheetNo);
                }
            }
            catch { }
            finally { }
        }

        public void Clear()
        {
            try
            {
                this.panelList.ForEach(f => f.Item3.Clear());
                this.doneCount = 0;
                ChangeTitle();
            }
            catch { }
            finally { }
        }

        private void CheckDone(int idx, int sheetNo)
        {
            //InspectObserverPanel srcPanel = this.panelList[idx];
            //InspectObserverPanel dstPanel = this.panelList[(idx + 2) % 4];
            //int targetRowIndex = dstPanel.Exist(sheetNo);
            //if (targetRowIndex >= 0)
            //{
            //    //lock (title)
            //    //{
            //    //lockSelectionChange = true;
            //    dstPanel.SelectRow(targetRowIndex, true);
            //    //srcPanel.SelectRow(srcPanel.Exist(sheetNo));
            //    //lockSelectionChange = false;
            //    lock (this)
            //        doneCount++;
            //    //}
            //    ChangeTitle();
            //}
        }

        private delegate void ChangeTitleDelegate();
        private void ChangeTitle()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ChangeTitleDelegate(ChangeTitle));
                return;
            }

            lock (this)
                this.Text = string.Format("{0} - Inspected Count {1}", title, doneCount);
        }

        private void InspectObserver_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void listBox_SelectedIndexChanged(int panelIndex, int rowIndex)
        {
            //if (rowIndex < 0)
            //    return;

            //InspectObserverPanel curPanel = this.panelList[panelIndex];
            //int sheetNo = curPanel.GetSheetNo(rowIndex);

            //int targetIdx = (panelIndex + 2) % 4;
            //InspectObserverPanel panel = this.panelList[targetIdx];
            //int targetRowIndex = panel.Exist(sheetNo);
            //panel.SelectRow(targetRowIndex, true);
        }
    }
}
