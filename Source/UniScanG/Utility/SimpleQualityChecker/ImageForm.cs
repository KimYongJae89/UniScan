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

namespace SimpleQualityChecker
{
    public partial class ImageForm : Form
    {
        string fileName;

        string mLoc;
        string pxValue;

        bool bitmapFlag = false;
        Bitmap bitmap;
        Bitmap bitmap2;
        int scale;
        double stdDev;
        CanvasPanel canvasPanel = new CanvasPanel();

        public ImageForm()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel()
            {
                Dock = DockStyle.Fill
            };

            this.canvasPanel.MouseMove += new MouseEventHandler((s,e)=>
            {
                CanvasPanel canvasPanel = (CanvasPanel)s;
                Rectangle imagePt = new Rectangle(canvasPanel.PointToImage(e.Location), Size.Empty);
                Rectangle rect = new Rectangle(Point.Empty, bitmap.Size);
                if (rect.IntersectsWith(imagePt))
                {
                    Color color = canvasPanel.GetPixel(imagePt.Location);

                    this.mLoc = $"X:{imagePt.X * scale}, Y:{imagePt.Y * scale}";
                    this.pxValue = $"V:{color.GetBrightness() * 255:F0}";
                    UpdateTitle();
                }
            });

            this.canvasPanel.MouseDoubleClick += new MouseEventHandler((s, e) =>
            {
                CanvasPanel canvasPanel = (CanvasPanel)s;
                if (e.Button == MouseButtons.Left)
                    canvasPanel.ZoomFit();
            });

            this.canvasPanel.MouseClick += new MouseEventHandler((s, e) =>
            {
                CanvasPanel canvasPanel = (CanvasPanel)s;
                if (e.Button == MouseButtons.Right)
                {
                    bitmapFlag = !bitmapFlag;
                    if (bitmapFlag)
                        canvasPanel.UpdateImage(bitmap2);
                    else
                        canvasPanel.UpdateImage(bitmap);
                }
            });


            this.Controls.Add(this.canvasPanel);
        }

        public void SetData(string fileName, Bitmap bitmap, Bitmap bitmap2,int scale, double stdDev)
        {
            this.fileName = fileName;
            this.bitmap = bitmap;
            this.bitmap2 = bitmap2;
            this.scale = scale;
            this.stdDev = stdDev;
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            UpdateTitle();
            this.canvasPanel.UpdateImage(bitmap);
            this.canvasPanel.ZoomFit();
            this.canvasPanel.SetPanMode();
        }

        private void UpdateTitle()
        {
            this.Text = $"{this.fileName} - {stdDev:F2} - {this.mLoc} - {this.pxValue}";
            
        }
    }
}
