using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastCanvasPanel
{
    public partial class Form1 : Form
    {
        DrawBox2 canvasPanel = null;
        public Form1()
        {
            InitializeComponent();

            this.canvasPanel = new DrawBox2();
            this.canvasPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(this.canvasPanel, 0, 0);
        }
    }
}
