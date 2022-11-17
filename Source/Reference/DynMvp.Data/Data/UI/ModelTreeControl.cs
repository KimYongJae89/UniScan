using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data;
using DynMvp.Base;

namespace DynMvp.Data.UI
{
    public delegate void TargetClickedDelegate(Target target);
    public delegate void TargetDoubleClickedDelegate(Target target);

    public partial class ModelTreeControl : UserControl
    {
        private Model model;
        public Model Model
        {
            set { model = value; }
        }

        public ModelTreeControl()
        {
            InitializeComponent();
        }

        private TargetClickedDelegate targetClicked;
        public TargetClickedDelegate TargetClicked
        {
            set { targetClicked = value; }
        }

        private TargetDoubleClickedDelegate targetDoubleClicked;
        public TargetDoubleClickedDelegate TargetDoubleClicked
        {
            set { targetDoubleClicked = value; }
        }

        public void Update(Model model)
        {
            objectTree.Nodes.Clear();

            this.model = model;

            TreeNode modelNode = objectTree.Nodes.Add("Current Model");
            foreach (InspectionStep inspectionStep in model)
            {
                BuildInspectionStepTree(inspectionStep, modelNode);
            }

            if (model.MasterModel != null)
            {
                modelNode = objectTree.Nodes.Add("Master Model");
                foreach (InspectionStep inspectionStep in model.MasterModel)
                {
                    BuildInspectionStepTree(inspectionStep, modelNode);
                }
            }
        }

        public void BuildInspectionStepTree(InspectionStep inspectionStep, TreeNode modelNode)
        {
            TreeNode targetGroupNode = modelNode.Nodes.Add(String.Format("InspectionStep{0}", inspectionStep.StepNo));

            foreach (TargetGroup targetGroup in inspectionStep)
            {
                BuildTargetGroupTree(targetGroup, modelNode);
            }
        }

        public void BuildTargetGroupTree(TargetGroup targetGroup, TreeNode modelNode)
        {
            TreeNode targetGroupNode = modelNode.Nodes.Add(String.Format("TargetGroup{0}", targetGroup.GroupId));

            foreach (Target target in targetGroup)
            {
                TreeNode targetNode = targetGroupNode.Nodes.Add(target.Name);
                targetNode.Tag = target;
            }
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Target target = (Target)objectTree.SelectedNode.Tag;

            if (target != null)
            {
                if (targetImage.Image != null)
                    targetImage.Image.Dispose();

                targetImage.Image = target.Image.ToBitmap();

                if (targetClicked != null)
                    targetClicked(target);
            }
            else
            {
                targetImage.Image = null;
            }
        }

        private void objectTree_DoubleClick(object sender, EventArgs e)
        {
            Target target = (Target)objectTree.SelectedNode.Tag;

            if (target != null && targetDoubleClicked != null)
            {
                targetDoubleClicked(target);
            }
        }

        public Target GetSelectedTarget()
        {
            return (Target)objectTree.SelectedNode.Tag;
        }
    }
}
