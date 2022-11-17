using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.UI.Graph;

namespace ProfilePanelTest
{
    public partial class Form1 : Form
    {
        UniScanM.UI.Graph.ProfilePanel profilePanel = null;
        ProfileOption profileOption = null;

        public Form1()
        {
            InitializeComponent();

            this.profileOption = new ProfileOption(2, 1);
            this.profilePanel = new UniScanM.UI.Graph.ProfilePanel("DefaultTitle", this.profileOption);
            this.profilePanel.Dock = DockStyle.Fill;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.SelectedObject = this.profileOption;

            this.tableLayoutPanel1.Controls.Add(profilePanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(propertyGrid, 1, 0);
        }

        float x = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            float data = (float)Math.Sin(Math.PI * x) * (1 + (float)Math.Cos(Math.PI / 2.3 * x));
            profilePanel.AddValue(0, x, data);
            x += 0.01f;
        }
    }
}
