using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.Devices;
using UniScanG.Common.Data;
using UniScanG;
using UniScanG.Inspect;

namespace UniScanG.Gravure.UI.Inspect.Inspector
{
    public partial class InspectObserver : Form, IInspectObserver
    {
        List<InspectObserverPanel> panelList = null;

        public InspectObserver()
        {
            InitializeComponent();

            this.panelList = new List<InspectObserverPanel>();

            this.panelList.Add(new InspectObserverPanel("Grabbed", 0));
            this.panelList.Add(new InspectObserverPanel("Inspected", 1));

            this.tableLayoutPanel1.Controls.Add(this.panelList[0], 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelList[1], 1, 0);
        }

        public void AddData(int index, int subIndex, int sheetNo)
        {
            try
            {
                this.panelList[index].AddData(sheetNo);
                int ind = this.panelList[index].Exist(sheetNo);
                this.panelList[index].SelectRow(ind, false);
            }
            finally { }
        }

        public void Clear()
        {
            try
            {
                lock (this.panelList)
                    this.panelList.ForEach(f => f.Clear());
            }
            finally { }
        }

        private void InspectObserver_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void InspectObserver_Load(object sender, EventArgs e)
        {

        }
    }
}
