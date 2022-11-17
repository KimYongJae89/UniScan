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
using UniScanG.Data;

namespace UniScanG.UI.Etc
{
    public partial class ContextInfoForm : Form, IMultiLanguageSupport
    {
        FoundedObjInPattern sheetSubResult = null;

        public ContextInfoForm()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        private void ContextInfoForm_Load(object sender, EventArgs e)
        {
        }

        public void CanvasPanel_MouseClicked(PointF point, ref bool processingCancelled)
        {
            Hide();
        }

        public void CanvasPanel_MouseLeaved(CanvasPanel canvasPanel)
        {
            Hide();
        }

        public void CanvasPanel_MouseLeaved()
        {
            Hide();
        }

        internal void CanvasPanel_FigureClicked(Figure figure, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                CanvasPanel_FigureFocused(figure);
        }

        public void CanvasPanel_FigureFocused(Figure figure)
        {
            if (figure == null)
            {
                this.sheetSubResult = null;
                Hide();
            }
            else if (figure.Tag is FoundedObjInPattern)
            {                
                this.sheetSubResult = (FoundedObjInPattern)figure.Tag;
                this.Location = Point.Add(MousePosition, new Size(0, 10));
                Rectangle screenRect= System.Windows.Forms.Screen.GetBounds(this.Location);
                if (this.Right > screenRect.Right)
                    this.Location = Point.Add(MousePosition, new Size(-this.Size.Width, 10));
                defectType.Text = sheetSubResult.GetDefectType().GetLocalString();
                infoText.Text = sheetSubResult.ToString();
                image.Image = sheetSubResult.Image;
                Show();
            }
        }


        public void CanvasPanel_FigureMouseLeaved(Figure figure)
        {
            Hide();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void image_Click(object sender, EventArgs e)
        {
            if (image.Image == sheetSubResult.BufImage)
                image.Image = sheetSubResult.Image;
            else
                image.Image = sheetSubResult.BufImage;

        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            Bitmap bt = new Bitmap(this.Width,this.Height);

            Graphics g = Graphics.FromImage(bt);
            Point screenPt = PointToScreen(this.Location);

            //g.CopyFromScreen(screenPt, Point.Empty, this.Size);
            g.CopyFromScreen(this.Location, Point.Empty, this.Size);

            Clipboard.SetImage((Image)bt);

            System.IO.Directory.CreateDirectory(@"C:\temp\");
            bt.Save(string.Format(@"C:\temp\{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")));
            this.Hide();
            
        }

        private void ContextInfoForm_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
