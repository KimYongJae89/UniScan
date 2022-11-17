using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data.UI;
using DynMvp.Data;
using DynMvp.Base;

namespace DynMvp.Data.Forms
{
    public partial class ComputeParamControl : UserControl
    {
        private UserControl selectedAlgorithmParamControl = null;
        ComputeItem computeProbeItem;
        ComputeProbe selectedProbe;
        ObjectTree objectTree;
        Model model;
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }


        string algorithmType;
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }
        private double length;
        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public ComputeParamControl()
        {
            InitializeComponent();
            AlgorithmType = "Compute Probe";
            this.objectTree = new ObjectTree();
            UpdateComputeTypeToList(); // compute타입의 리스트를 추가한다.

            labelType.Text = StringManager.GetString(this.GetType().FullName,labelType.Text);
            labelTarget1.Text = StringManager.GetString(this.GetType().FullName,labelTarget1.Text);
            labelTarget2.Text = StringManager.GetString(this.GetType().FullName,labelTarget2.Text);
        }
        public void Initialize(Model model)
        {
            this.model = model;
        }
        private void UpdateComputeTypeToList()
        {
            computeTypeList.DataSource = Enum.GetValues(typeof(ComputeType));
        }
        private void buttonTarget1_Click(object sender, EventArgs e)
        {
            ShowSelectTree(this.txtTarget1);
        }
        private void buttonTarget2_Click(object sender, EventArgs e)
        {
            ShowSelectTree(this.txtTarget2);
        }
        private void ShowSelectTree(TextBox textBox)
        {
            SelectResultValueForm form = new SelectResultValueForm();
            form.Model = model;
            objectTree.Initialize(model);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string valueName = form.ValueName.Replace("ResultValue.", "");
                textBox.Text = valueName;
            }
        }
        public void SetSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "ComputeProbeParamControl - SetSelectedProbe");

            
            //ComputeProbe computeProbe = (ComputeProbe)probe
            //if (computeProbe.InspAlgorithm.GetAlgorithmType() == WidthChecker.TypeName)
            //{
            //    selectedProbe = visionProbe;
            //    widthChecker = (WidthChecker)visionProbe.InspAlgorithm;
            //    UpdateData();
            //}
            //else
            //    throw new InvalidOperationException();

        }
        private void ShowAlgorithmParamControl(string algorithmType)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            if (selectedAlgorithmParamControl != null)
                selectedAlgorithmParamControl.Hide();

            //this.algorithmParamPanel.Controls.Clear();
            //this.algorithmParamPanel.Controls.Add(selectedAlgorithmParamControl);

            selectedAlgorithmParamControl.Show();

            if (selectedProbe != null)
            {
                ((IAlgorithmParamControl)selectedAlgorithmParamControl).ClearSelectedProbe();
            }
        }

        private void ComputeParamControl_Load(object sender, EventArgs e)
        {
            objectTree.Initialize(model);            
        }
    }
}
