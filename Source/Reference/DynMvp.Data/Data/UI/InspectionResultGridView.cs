using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.UI;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public partial class InspectionResultGridView : UserControl
    {
        bool showDefectOnly;
        public bool ShowDefectOnly
        {
            get { return showDefectOnly; }
            set { showDefectOnly = value; }
        }

        private InspectionResult inspectionResult = null;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        private InspectionResult lastInspectionResult = null;
        public InspectionResult LastInspectionResult
        {
            get { return lastInspectionResult; }
            set { lastInspectionResult = value; }
        }

        public InspectionResultGridView()
        {
            InitializeComponent();

            // language change
            ColumnProbeType.HeaderText = StringManager.GetString(this.GetType().FullName,ColumnProbeType.HeaderText);
            ColumnValue.HeaderText = StringManager.GetString(this.GetType().FullName,ColumnValue.HeaderText);
            ColumnResult.HeaderText = StringManager.GetString(this.GetType().FullName,ColumnResult.HeaderText);
            ColumnStandard.HeaderText = StringManager.GetString(this.GetType().FullName,ColumnStandard.HeaderText);
        }

        private void InspectionResultForm_Load(object sender, EventArgs e)
        {
            if (inspectionResult == null)
                return;

            foreach (ProbeResult probeResult in inspectionResult)
            {
                if (showDefectOnly == true && probeResult.Judgment != Judgment.Reject)
                    continue;

                Probe probe = probeResult.Probe;

                int index = defectGridResultList.Rows.Add(probe.Target.TargetGroup.InspectionStep.StepName, 
                        probe.Target.TargetGroup.GroupId, probe.Target.Id, probe.Id, probe.Target.Name, probe.GetProbeTypeShortName(), JudgementString.ToLocaleString(probeResult.Judgment));

                if (probeResult.Judgment != Judgment.Accept)
                    defectGridResultList.Rows[index].Cells[ColumnResult.Index].Style.BackColor = Color.Red;
                else
                    defectGridResultList.Rows[index].Cells[ColumnResult.Index].Style.BackColor = Color.Green;
                defectGridResultList.Rows[index].Tag = probeResult;
            }
        }

        private void defectGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            ProbeResult probeResult = (ProbeResult)defectGridResultList.Rows[e.RowIndex].Tag;
            Probe probe = probeResult.Probe;

            dataGridCurrentResultValue.Rows.Clear();
            foreach (ProbeResultValue probeResultValue in probeResult.ResultValueList)
            {
                string standardString = "";
                if (String.IsNullOrEmpty(probeResultValue.DesiredString) == false)
                    standardString = probeResultValue.DesiredString;
                else if (probeResultValue.Lcl != probeResultValue.Ucl)
                    standardString = String.Format("{0:0.00} ~ {1:0.00}", probeResultValue.Lcl, probeResultValue.Ucl);

                if (probeResultValue.Value is string)
                {
                    dataGridCurrentResultValue.Rows.Add(probeResultValue.Name, probeResultValue.Value, standardString);
                }
                else
                {
                    dataGridCurrentResultValue.Rows.Add(probeResultValue.Name, String.Format("{0:0.00}", probeResultValue.Value), standardString);
                }
            }
        }
    }
}
