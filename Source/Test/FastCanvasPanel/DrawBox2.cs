using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastCanvasPanel
{
    public partial class DrawBox2 : UserControl
    {
        Bitmap bitmap = null;
        Rectangle viewPort;

        public DrawBox2()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap bitmap)
        {

        }
    }
}
