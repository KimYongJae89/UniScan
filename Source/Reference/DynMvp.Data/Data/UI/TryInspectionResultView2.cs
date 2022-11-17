using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data;
using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;

namespace DynMvp.Data.UI
{
    public delegate void TryInspectionResultCellClickedDelegate();

    public partial class TryInspectionResultView2 : UserControl
    {
        public TryInspectionResultCellClickedDelegate TryInspectionResultCellClicked = null;

        private TeachHandlerProbe teachHandlerProbe = null;
        public TeachHandlerProbe TeachHandlerProbe
        {
            get { return teachHandlerProbe; }
            set{ teachHandlerProbe = value; }
        }

        private InspectionResult targetInspectionResult;

        bool showProbeOwner = false;
        public bool ShowProbeOwner
        {
            get { return showProbeOwner; }
            set { showProbeOwner = value; }
        }

        public TryInspectionResultView2()
        {
            InitializeComponent();
        }

        public void ClearResult()
        {
            webBrowser.DocumentText = "";
            resultGrid.Rows.Clear();
        }

        public void SetResult(InspectionResult targetInspectionResult)
        {
            resultGrid.Rows.Clear();
            this.targetInspectionResult = targetInspectionResult;

            tabControlMain.SelectedTab = tabControlMain.Tabs[0];

            foreach (ProbeResult probeResult in targetInspectionResult)
            {
                int rowIndex = 0;
                
                if (String.IsNullOrEmpty(probeResult.Probe.Name))
                    rowIndex = resultGrid.Rows.Add(probeResult.Probe.Target.Name, probeResult.Probe.Id, JudgementString.ToLocaleString(probeResult.Judgment), probeResult.ShortResultMessage);
                else
                    rowIndex = resultGrid.Rows.Add(probeResult.Probe.Name, "", JudgementString.ToLocaleString(probeResult.Judgment), probeResult.ShortResultMessage);

                resultGrid.Rows[rowIndex].Tag = probeResult;
                if (probeResult.Judgment == Judgment.Reject)
                    resultGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
            }
        }

        private void resultGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DynMvp.UI.MessageBuilder message = new DynMvp.UI.MessageBuilder();
            ProbeResult probeResult = (ProbeResult)resultGrid.Rows[e.RowIndex].Tag;
            if (probeResult == null)
                return;

            probeResult.BuildResultMessage(message);

            webBrowser.DocumentText = HtmlMessageBuilder.Build(message);

            tabControlMain.SelectedTab = tabControlMain.Tabs[1];
        }

        private void resultGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            ProbeResult probeResult = (ProbeResult)resultGrid.Rows[e.RowIndex].Tag;

            if (teachHandlerProbe != null)
            {
                teachHandlerProbe.ClearSelection();
                teachHandlerProbe.Select(probeResult.Probe);
                
            }

            if (TryInspectionResultCellClicked != null)
            {
                TryInspectionResultCellClicked();
            }
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabControlMain.Tabs[0];
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            //webBrowser.DocumentText.
        }
    }
}
