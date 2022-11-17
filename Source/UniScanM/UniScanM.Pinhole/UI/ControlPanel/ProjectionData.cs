using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.DataVisualization;

namespace UniScanM.Pinhole.UI.ControlPanel
{
    public partial class ProjectionData : UserControl
    {
        ScatterLineSeries seriesLine;

        List<PointF> profileList = new List<PointF>();
        private int offset = 300;
        int index = 0;

        public ProjectionData(int index)
        {
            InitializeComponent();

            this.index = index;
            chartProfile.ChartAreas[0].AxisX.Minimum = 0;
            chartProfile.ChartAreas[0].AxisX.Maximum = 2048;
            chartProfile.ChartAreas[0].AxisY.Minimum = 0;
            
            chartProfile.ChartAreas[0].AxisY.Maximum = 256;
            chartProfile.Series[0].XValueMember = "X";
            chartProfile.Series[0].YValueMembers = "Y";
        }

        public void UpdateData(float[] data)
        {
            //chartProfile.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            profileList.Clear();
            for (int i = 0; i < data.Length;i++)
            {
                profileList.Add(new PointF(i - offset, data[i]));
            }

            chartProfile.DataSource = profileList;
            chartProfile.DataBind();
            chartProfile.Invalidate();
        }

        private void ProjectionData_Load(object sender, EventArgs e)
        {

        }
    }
}
