using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PadMonitor
{
    public partial class OverViewer : Form
    {
        public OverViewer()
        {
            InitializeComponent();
        }
        public void UpdateImage(Bitmap bitmap)
        {
            pictureBox1.Image = bitmap;
            Invalidate();
        }
    }
}
