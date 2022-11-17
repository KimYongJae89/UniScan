using DynMvp.Base;
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
using UniScan.Common.UI;

namespace CanvasPanelTest
{
    public partial class Form1 : Form
    {
        //CanvasPanel canvasPanel = null;
        Form2 overLayInfo = null;
        WPFCanvasPanel wPFCanvasPanel;
        public Form1()
        {
            InitializeComponent();

            wPFCanvasPanel = new WPFCanvasPanel();
            elementHost1.Child = wPFCanvasPanel;
        }

        private void canvasPanel_FigureFocused(Figure figure)
        {
            if(figure!=null)
            {
                this.overLayInfo = new Form2((string)figure.Tag);
                this.overLayInfo.StartPosition = FormStartPosition.Manual;
                this.overLayInfo.Location = MousePosition;
                this.overLayInfo.Show();
            }
            else
            {
                this.overLayInfo.Hide();
                this.overLayInfo.Dispose();
                this.overLayInfo = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap(*.bmp)|*.bmp";
            if(openFileDialog.ShowDialog()== DialogResult.OK)
            {
                Image2D image2D = new Image2D(openFileDialog.FileName);
                Bitmap bitmap = image2D.ToBitmap();
                wPFCanvasPanel.UpdateImage(bitmap);
                bitmap.Dispose();
                image2D.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.wPFCanvasPanel.ZoomFit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //canvasPanel.ShowToolbar = !canvasPanel.ShowToolbar;
        }
    }
}
