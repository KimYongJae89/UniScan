using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace UniScanM.Pinhole.UI.MenuPage
{
    public partial class ResultTable : UserControl, IMultiLanguageSupport
    {
        Timer updateTimer = new Timer();
        System.Threading.ManualResetEvent updateQuary = new System.Threading.ManualResetEvent(false);

        public System.Threading.ManualResetEvent UpdateQuary
        {
            get { return this.updateQuary; }
        }

        public ResultTable()
        {
            InitializeComponent();
            //dataGridViewResultCount.Rows.Add();
            //dataGridViewResultCount.Rows.Add();

            StringManager.AddListener(this);

            this.updateTimer = new Timer();
            this.updateTimer.Interval = 200;
            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.updateTimer.Stop();

            if(UpdateQuary.WaitOne(0))
            {
                UpdateTable();
                UpdateQuary.Reset();
            }

            this.updateTimer.Start();
        }

        IAsyncResult asyncUpdatePage = null;
        private delegate void UpdateTableDelegate();
        private void UpdateTable()
        {
            //Debug.WriteLine("UpdateTable()..111.");
            if (InvokeRequired)
            {
                if (asyncUpdatePage != null)
                {
                    if (asyncUpdatePage.IsCompleted == false)
                        return;
                }
                asyncUpdatePage = BeginInvoke(new UpdateTableDelegate(UpdateTable));
                return;
            }
            //Debug.WriteLine("UpdateTable()..222.");
            Pinhole.Data.Production production = (Pinhole.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            if (production != null)
            {
                lock (production)
                {
                    totalSum.Text = production.TotalNum.ToString();
                    pinholeSum.Text = production.TotalPinholeNum.ToString();
                    dustSum.Text = production.TotalDustNum.ToString();
                    totalCam1.Text = production.Num1.ToString();
                    pinholeCam1.Text = production.PinholeNum1.ToString();
                    dustCam1.Text = production.DustNum1.ToString();
                    totalCam2.Text = production.Num2.ToString();
                    pinholeCam2.Text = production.PinholeNum2.ToString();
                    dustCam2.Text = production.DustNum2.ToString();
                    sectionCam1.Text = production.Section1.ToString();
                    sectionCam2.Text = production.Section2.ToString();
                }
                Update();
            }

            /*
            dataGridViewResultCount.Rows.Add(inspectResult.DeviceIndex + 1, inspectResult.SectionIndex, inspectResult.GetTotalDefectCount(), inspectResult.GetDefectCount(Data.DefectType.Pinhole), inspectResult.GetDefectCount(Data.DefectType.Dust));

            //dataGridViewResultCount.Refresh();
            if (dataGridViewResultCount.RowCount > 0)
                dataGridViewResultCount.FirstDisplayedScrollingRowIndex = dataGridViewResultCount.RowCount - 1;
                */
        }

        public void ResultTableRefresh()
        {
            //dataGridViewResultCount.Refresh();
        }

        private void ResultTable_SizeChanged(object sender, EventArgs e)
        {
            //this.SuspendLayout();

            //labelTotal.Dock = DockStyle.Fill;
            //labelTotalPinHole.Dock = DockStyle.Fill;
            //labelTotalDirty.Dock = DockStyle.Fill;
            //labelCamera1Total.Dock = DockStyle.Fill;
            //labelCamera1PinHole.Dock = DockStyle.Fill;
            //labelCamera1Dirty.Dock = DockStyle.Fill;
            //labelCamera2Total.Dock = DockStyle.Fill;
            //labelCamera2PinHole.Dock = DockStyle.Fill;
            //labelCamera2Dirty.Dock = DockStyle.Fill;

            //this.ResumeLayout();

            //labelTotal.Dock = DockStyle.None;
            //labelTotalPinHole.Dock = DockStyle.None;
            //labelTotalDirty.Dock = DockStyle.None;
            //labelCamera1Total.Dock = DockStyle.None;
            //labelCamera1PinHole.Dock = DockStyle.None;
            //labelCamera1Dirty.Dock = DockStyle.None;
            //labelCamera2Total.Dock = DockStyle.None;
            //labelCamera2PinHole.Dock = DockStyle.None;
            //labelCamera2Dirty.Dock = DockStyle.None;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
