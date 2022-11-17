using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using ZedGraph;

namespace DynMvp.Vision.UI
{
    public partial class ProjectionView : Form
    {
        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        DataSet dataSet1 = new DataSet();
        public DataSet DataSet1
        {
            get { return dataSet1; }
            set { dataSet1 = value; }
        }

        DataSet dataSet2 = new DataSet();
        public DataSet DataSet2
        {
            get { return dataSet2; }
            set { dataSet2 = value; }
        }

        ProjectionGraphControl xProjectionGraph;
        ProjectionGraphControl yProjectionGraph;

        public ProjectionView()
        {
            xProjectionGraph = new ProjectionGraphControl();
            yProjectionGraph = new ProjectionGraphControl();

            InitializeComponent();

            saveImage.Text = StringManager.GetString(this.GetType().FullName,saveImage.Text);

            this.tableLayoutPanel1.Controls.Add(this.xProjectionGraph, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.yProjectionGraph, 1, 0);

            this.xProjectionGraph.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.xProjectionGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xProjectionGraph.Location = new System.Drawing.Point(0, 313);
            this.xProjectionGraph.Name = "xProjectionGraph";
            this.xProjectionGraph.Size = new System.Drawing.Size(466, 359);
            this.xProjectionGraph.TabIndex = 0;
            this.xProjectionGraph.Direction = ProjectionGraphDirection.xProjection;

            this.yProjectionGraph.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.yProjectionGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yProjectionGraph.Location = new System.Drawing.Point(0, 313);
            this.yProjectionGraph.Name = "yProjectionGraph";
            this.yProjectionGraph.Size = new System.Drawing.Size(466, 359);
            this.yProjectionGraph.TabIndex = 0;
            this.yProjectionGraph.Direction = ProjectionGraphDirection.yProjection;
        }

        private void ProjectionView_Load(object sender, EventArgs e)
        {
            this.imageBox.Image = Image;

            xProjectionGraph.Data = dataSet1.XProjection;
            xProjectionGraph.MaxValue = dataSet1.MaxValue;
            xProjectionGraph.MinValue = dataSet1.MinValue;
            xProjectionGraph.Data2 = dataSet2.XProjection;
            xProjectionGraph.MaxValue2 = dataSet2.MaxValue;
            xProjectionGraph.MinValue2 = dataSet2.MinValue;
            xProjectionGraph.Invalidate();

            yProjectionGraph.Data = dataSet1.YProjection;
            yProjectionGraph.MaxValue = dataSet1.MaxValue;
            yProjectionGraph.MinValue = dataSet1.MinValue;
            yProjectionGraph.Data2 = dataSet2.YProjection;
            yProjectionGraph.MaxValue2 = dataSet2.MaxValue;
            yProjectionGraph.MinValue2 = dataSet2.MinValue;
            yProjectionGraph.Invalidate();
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.imageBox.Image.Save(dialog.FileName);
            }
        }
    }

    public class DataSet
    {
        float minValue;
        public float MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        float maxValue;
        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        float[] xProjection;
        public float[] XProjection
        {
            get { return xProjection; }
            set { xProjection = value; }
        }

        float[] yProjection;
        public float[] YProjection
        {
            get { return yProjection; }
            set { yProjection = value; }
        }
    }
}
