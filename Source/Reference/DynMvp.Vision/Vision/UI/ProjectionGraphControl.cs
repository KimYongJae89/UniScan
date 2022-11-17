using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.UI
{
    public enum ProjectionGraphDirection
    {
        xProjection, yProjection
    }

    public partial class ProjectionGraphControl : UserControl
    {
        float[] data;
        public float[] Data
        {
            get { return data; }
            set
            {
                data = value;

                if (data != null && minValue == 0 && maxValue == 0)
                {
                    minValue = data.Min();
                    maxValue = data.Max();
                }
            }
        }

        float[] data2;
        public float[] Data2
        {
            get { return data2; }
            set
            {
                data2 = value;

                if (data2 != null && minValue2 == 0 && maxValue2 == 0)
                {
                    minValue2 = data2.Min();
                    maxValue2 = data2.Max();
                }
            }
        }

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

        float minValue2;
        public float MinValue2
        {
            get { return minValue2; }
            set { minValue2 = value; }
        }

        float maxValue2;
        public float MaxValue2
        {
            get { return maxValue2; }
            set { maxValue2 = value; }
        }

        ProjectionGraphDirection direction;
        public ProjectionGraphDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public ProjectionGraphControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (data == null)
                return;

            Graphics g = e.Graphics;

            if (direction == ProjectionGraphDirection.xProjection)
                DrawXProjection(g);
            else
                DrawYProjection(g);

            base.OnPaint(e);
        }

        private void DrawXProjection(Graphics g)
        {
            float scaleX = ((float)Size.Width) / (data.Length - 1);
            float scaleY = ((float)Size.Height) / (maxValue - minValue);

            for (int i = 0; i < data.Length-1; i++)
            {
                g.DrawLine(new Pen(Color.Red), new PointF(i * scaleX, data[i] * scaleY - minValue), new PointF((i+1) * scaleX, data[i+1] * scaleY - minValue));
            }

            if (data2 != null)
            {
                float scaleX2 = ((float)Size.Width) / (data2.Length - 1);
                float scaleY2 = ((float)Size.Height) / (maxValue2 - minValue2);

                for (int i = 0; i < data2.Length - 1; i++)
                {
                    g.DrawLine(new Pen(Color.Blue), new PointF(i * scaleX2, data2[i] * scaleY2 - minValue2), new PointF((i + 1) * scaleX2, data2[i + 1] * scaleY2 - minValue2));
                }
            }
        }

        private void DrawYProjection(Graphics g)
        {
            float scaleX = ((float)Size.Width) / (maxValue - minValue);
            float scaleY = ((float)Size.Height) / (data.Length - 1);

            for (int i = 0; i < data.Length - 1; i++)
            {
                g.DrawLine(new Pen(Color.Red), new PointF(data[i] * scaleX - minValue, i * scaleY), new PointF(data[i + 1] * scaleX - minValue, (i + 1) * scaleY));
            }

            if (data2 != null)
            {
                float scaleX2 = ((float)Size.Width) / (maxValue2 - minValue2);
                float scaleY2 = ((float)Size.Height) / (data2.Length - 1);

                for (int i = 0; i < data2.Length - 1; i++)
                {
                    g.DrawLine(new Pen(Color.Blue), new PointF(data2[i] * scaleX2 - minValue2, i * scaleY2), new PointF(data2[i + 1] * scaleX2 - minValue2, (i + 1) * scaleY2));
                }
            }

        }

        private void ProjectionGraphControl_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ProjectionGraphControl_Load(object sender, EventArgs e)
        {

        }

        private void ProjectionGraphControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Histogram histogram = new Histogram();

            if (e.X < Size.Width / 2)
            {
                histogram.Create(data, 10, 2);
            }
            else
            {
                histogram.Create(data2, 10, 10);
            }

            HistogramView histogramView = new HistogramView();
            histogramView.Histogram = histogram;
            histogramView.Show();
        }
    }
}
