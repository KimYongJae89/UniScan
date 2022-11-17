using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.UI
{
    public partial class HistogramView : Form
    {
        Histogram histogram;
        internal Histogram Histogram
        {
            get { return histogram; }
            set { histogram = value; }
        }

        //ZedGraphControl graphControl = new ZedGraphControl();

        public HistogramView()
        {
            InitializeComponent();

            //this.graphControl.BackColor = System.Drawing.SystemColors.ButtonFace;
            //this.graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.graphControl.Location = new System.Drawing.Point(0, 313);
            //this.graphControl.Name = "graphControl";
            //this.graphControl.Size = new System.Drawing.Size(466, 359);
            //this.graphControl.TabIndex = 0;

            this.Controls.Clear();

            //this.Controls.Add(this.graphControl);
        }

        private void HistogramView_Load(object sender, EventArgs e)
        {
            //PointPairList dataPointPairList = new PointPairList();
            //for (int i = 0; i < histogram.HistogramData.Count(); i++)
            //{
            //    dataPointPairList.Add(i, histogram.HistogramData[i]);
            //}

            //graphControl.GraphPane.AddBar("Height", dataPointPairList, Color.Red);

            //graphControl.GraphPane.YAxis.Scale.Max = histogram.HistogramData.Max();
            //graphControl.GraphPane.YAxis.Scale.Min = histogram.HistogramData.Min();
            //graphControl.GraphPane.XAxis.Scale.Max = histogram.HistogramData.Count();
            //graphControl.GraphPane.XAxis.Scale.Min = 0;
        }
    }
}
