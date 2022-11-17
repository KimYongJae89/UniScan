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

namespace DynMvp.Data.UI
{
    public partial class TryInspectionResultView : UserControl
    {
        private InspectionResult selectedTargetInspectionResult;
        private InspectionResult lastTargetInspectionResult;

        bool showProbeOwner = false;
        public bool ShowProbeOwner
        {
            get { return showProbeOwner; }
            set { showProbeOwner = value; }
        }

        public TryInspectionResultView()
        {
            InitializeComponent();
        }

        private void InspectionResultForm_Load(object sender, EventArgs e)
        {

        }

        public void ClearResult()
        {
            resultText.Text = "";
            Invalidate();
            Update();
        }

        public void AddResult(InspectionResult selectedTargetInspectionResult, InspectionResult lastTargetInspectionResult)
        {
            this.selectedTargetInspectionResult = selectedTargetInspectionResult;
            this.lastTargetInspectionResult = lastTargetInspectionResult;

            if (lastTargetInspectionResult != null && lastTargetInspectionResult.Count() > 0)
            {
                resultText.AppendText("< Last Inspection Result >" + Environment.NewLine);
                foreach (ProbeResult probeResult in lastTargetInspectionResult)
                {
                    AppendProbeResult(probeResult);
                }

                resultText.AppendText(Environment.NewLine + "< Selected Inspection Result >" + Environment.NewLine);
            }

            foreach (ProbeResult probeResult in selectedTargetInspectionResult)
            {
                AppendProbeResult(probeResult);
            }
        }

        void AppendProbeResult(ProbeResult probeResult)
        {
            if (showProbeOwner)
            {
                string stepName = probeResult.Probe.Target.TargetGroup.InspectionStep.Name;
                if (string.IsNullOrEmpty(stepName))
                {
                    stepName = probeResult.Probe.Target.TargetGroup.InspectionStep.StepNo.ToString();
                }

                resultText.AppendText(String.Format("Step : {0} / Target Group {1} / Target {2}", stepName, probeResult.Probe.Target.TargetGroup.GroupId, probeResult.Probe.Target.Id));
            }
            resultText.AppendText(Environment.NewLine);

            resultText.AppendText(String.Format("Probe{0} [ {1} ]", probeResult.Probe.Id, JudgementString.ToLocaleString(probeResult.Judgment)));
            resultText.AppendText(Environment.NewLine);
            resultText.AppendText(probeResult.ToString());
        }

        public void UpdateResult(InspectionResult selectedTargetInspectionResult, InspectionResult lastInspectionResult)
        {
            ClearResult();

            this.selectedTargetInspectionResult = selectedTargetInspectionResult;

            if (lastTargetInspectionResult != null && lastTargetInspectionResult.Count() > 0)
            {
                resultText.AppendText("< Last Inspection Result >" + Environment.NewLine);
                foreach (ProbeResult probeResult in lastTargetInspectionResult)
                {
                    resultText.AppendText(String.Format("Probe{0} - {1} [ {2} ]", probeResult.Probe.Id, probeResult.ToString(), JudgementString.ToLocaleString(probeResult.Judgment)));
                    resultText.AppendText(Environment.NewLine);
                }

                resultText.AppendText(Environment.NewLine + "< Selected Inspection Result >" + Environment.NewLine);
            }

            foreach (ProbeResult probeResult in selectedTargetInspectionResult)
            {
                resultText.AppendText(String.Format("Probe{0} - {1} [ {2} ]\n", probeResult.Probe.Id, probeResult.ToString(), JudgementString.ToLocaleString(probeResult.Judgment)));
                resultText.AppendText(Environment.NewLine);
            }
        }
    }
}
