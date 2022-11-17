using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public partial class ObjectTree : TreeView
    {
        String inspectionStepName = "InspectionStep";
        
        public String InspectionStepName
        {
            set { inspectionStepName = value; }
        }

        bool includeTargetType = true;
        public bool IncludeTargetType
        {
            get { return includeTargetType; }
            set { includeTargetType = value; }
        }

        public ObjectTree()
        {
            InitializeComponent();
            inspectionStepName = StringManager.GetString(this.GetType().FullName, inspectionStepName);
        }

        public void Initialize(Model model)
        {
            TreeNode modelNode = Nodes.Add(StringManager.GetString(this.GetType().FullName, "Current Model"));

            foreach (InspectionStep inspectionStep in model)
            {
                BuildInspectionStep(inspectionStep, modelNode);
            }

            if (model.MasterModel != null)
            {
                modelNode = Nodes.Add(StringManager.GetString(this.GetType().FullName, "Master Model"));

                foreach (InspectionStep inspectionStep in model.MasterModel)
                {
                    BuildInspectionStep(inspectionStep, modelNode);
                }
            }

            if (includeTargetType)
            {
                List<string> targetTypeList = new List<string>();
                model.GetTargetTypes(targetTypeList);

                BuildTargetType(targetTypeList, modelNode);
            }
        }

        private void BuildTargetType(List<string> targetTypeList, TreeNode modelNode)
        {
            TreeNode targetTypeRootNode = modelNode.Nodes.Add(StringManager.GetString(this.GetType().FullName, "TargetTypes"));

            foreach (string targetType in targetTypeList)
            {
                TreeNode targetTypeNode = targetTypeRootNode.Nodes.Add(targetType);
                targetTypeNode.Tag = "TargetType." + targetType;
            }
        }

        public void BuildInspectionStep(InspectionStep inspectionStep, TreeNode modelNode)
        {
            TreeNode inspectionStepNode = modelNode.Nodes.Add(String.Format("{0} {1}", inspectionStepName, inspectionStep.StepNo + 1));

            foreach (TargetGroup targetGroup in inspectionStep)
            {
                BuildTargetGroupTree(targetGroup, inspectionStepNode);
            }
        }

        public void BuildTargetGroupTree(TargetGroup targetGroup, TreeNode inspectionStepNode)
        {
            TreeNode targetGroupNode = inspectionStepNode.Nodes.Add(String.Format(StringManager.GetString(this.GetType().FullName, "TargetGroup")+"{0}", targetGroup.GroupId));
            targetGroupNode.Tag = targetGroup;

            foreach (Target target in targetGroup)
            {
                BuildTargetTree(target, targetGroupNode);
            }
        }

        public void BuildTargetTree(Target target, TreeNode targetGroupNode)
        {
            TreeNode targetNode = targetGroupNode.Nodes.Add(target.Name);
            targetNode.Tag = target;

            foreach (Probe probe in target)
            {
                BuildProbeTree(probe, targetNode);
            }
        }

        public void BuildProbeTree(Probe probe, TreeNode targetNode)
        {
            TreeNode probeNode = targetNode.Nodes.Add(probe.Name);
            probeNode.Tag = probe;

            List<ProbeResultValue> resultValueList = probe.GetResultValues();
            foreach (ProbeResultValue probeResultValue in resultValueList)
            {
                TreeNode probeResultValueNode = probeNode.Nodes.Add(probeResultValue.Name);
                probeResultValueNode.Tag = "ResultValue." + probeResultValue.Name;
            }
        }
    }
}
