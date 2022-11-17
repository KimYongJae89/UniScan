using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using UniScanG.Module.Observer.Vision;
using DynMvp.Data.UI;

namespace UniScanG.Module.Observer.Panel
{
    public partial class ImagePanel : UserControl
    {
        CanvasPanel canvasPanel;

        public ImagePanel()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.ShowCenterGuide = false;

            this.panelCanvas.Controls.Add(canvasPanel);
        }

        private delegate void UpdateTextDelegate(string text);
        public void UpdateText(string text)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateTextDelegate(UpdateText), text);
                return;
            }

            this.label1.Text = text;
        }

        public void UpdateImage(Bitmap bitmap)
        {
            this.canvasPanel.UpdateImage(bitmap);
            this.canvasPanel.ZoomRange(new RectangleF(PointF.Empty, bitmap.Size));
            //this.canvasPanel.ZoomFit();
        }

        //private delegate void UpdateResultDelegate(float marginL, float marginT, float marginR, float marginB);
        //internal void UpdateResult(float marginL, float marginT, float marginR, float marginB)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new UpdateResultDelegate(UpdateResult),  marginL,  marginT,  marginR,  marginB);
        //        return;
        //    }

        //    this.marginL.Text = marginL < 0?"": marginL.ToString("0.0");
        //    this.marginT.Text = marginT < 0 ? "" : marginT.ToString("0.0");
        //    this.marginR.Text = marginR < 0 ? "" : marginR.ToString("0.0");
        //    this.marginB.Text = marginB < 0 ? "" : marginB.ToString("0.0");
        //}

        public void Clear()
        {
            this.canvasPanel.UpdateImage(null);
            //this.UpdateResult(-1, -1, -1, -1);
        }
    }
}
