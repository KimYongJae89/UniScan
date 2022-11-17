using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data.UI;
using DynMvp.Data;
using DynMvp.Base;

namespace DynMvp.Data.UI
{
    public partial class SelectResultValueForm : Form    
    {
        private string objectName = "";
        public string ObjectName
        {
            get { return objectName; }
        }

        private string valueName = "";
        public string ValueName
        {
            get { return valueName; }
        }

        private Model model;
        public Model Model
        {
            set { model = value; }
        }

        private ObjectTree objectTree;

        public SelectResultValueForm()
        {
            InitializeComponent();

            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            this.objectTree = new ObjectTree();

            // 
            // objectTree
            // 
            this.objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTree.Location = new System.Drawing.Point(0, 0);
            this.objectTree.Margin = new System.Windows.Forms.Padding(5);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(294, 468);
            this.objectTree.TabIndex = 0;
            this.objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTree_AfterSelect);

            // 
            // splitContainer.Panel1
            // 
        
            this.panelTree.Controls.Add(this.objectTree);

            buttonOk.Enabled = false;
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            objectName = "";

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                if (obj is string)
                {
                    TreeNode parentNode = objectTree.SelectedNode.Parent;
                    if (parentNode.Tag != null)
                    {
                        objectName = ((Probe)parentNode.Tag).FullId;
                        valueName = obj.ToString();
                    }
                }
                else if (obj is Probe)
                {
                    objectName = ((Probe)obj).FullId;
                    valueName = "Result";
                }
                else if (obj is Target)
                {
                    objectName = ((Target)obj).Name;
                    valueName = "Result";
                }
            }

            if (objectName != "")
                buttonOk.Enabled = true;
            else
                buttonOk.Enabled = false;
        }

        private void SelectResultValueForm_Load(object sender, EventArgs e)
        {
            objectTree.Initialize(model);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
