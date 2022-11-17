using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class ImagePanel : UserControl
    {
        public ImagePanel()
        {
            InitializeComponent();

            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
        }

        int viewRectWidth, viewRectHeight; // view window width and height

        float zoom = 1.0f;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                if (value < 0.001f)
                    value = 0.001f;

                zoom = value;

                displayScrollbar();
                setScrollbarValues();
                Invalidate();
            }
        }

        Size canvasSize = new Size(60, 40);
        public Size CanvasSize
        {
            get { return canvasSize; }
            set
            {
                canvasSize = value;
                displayScrollbar();
                setScrollbarValues();
                Invalidate();
            }
        }

        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set 
            {
                image = value;
                displayScrollbar();
                setScrollbarValues(); 
                Invalidate();
            }
        }

        InterpolationMode interMode = InterpolationMode.HighQualityBilinear;
        public InterpolationMode InterpolationMode
        {
            get{return interMode;}
            set{interMode=value;}
        }

        protected override void OnLoad(EventArgs e)
        {
            displayScrollbar();
            setScrollbarValues();
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            displayScrollbar();
            setScrollbarValues();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
             base.OnPaint(e);

            //draw image
            if(image!=null)
            {
                Rectangle srcRect,distRect;
                Point pt=new Point((int)(hScrollBar.Value/zoom),(int)(vScrollBar.Value/zoom));
                if (canvasSize.Width * zoom < viewRectWidth && canvasSize.Height * zoom < viewRectHeight)
                    srcRect = new Rectangle(0, 0, canvasSize.Width, canvasSize.Height);  // view all image
                else srcRect = new Rectangle(pt, new Size((int)(viewRectWidth / zoom), (int)(viewRectHeight / zoom))); // view a portion of image

                distRect=new Rectangle((int)(-srcRect.Width/2),-srcRect.Height/2,srcRect.Width,srcRect.Height); // the center of apparent image is on origin
 
                Matrix mx=new Matrix(); // create an identity matrix
                mx.Scale(zoom,zoom); // zoom image
                mx.Translate(viewRectWidth/2.0f,viewRectHeight/2.0f, MatrixOrder.Append); // move image to view window center

                Graphics g=e.Graphics;
                g.InterpolationMode=interMode;
                g.Transform=mx;
                g.DrawImage(image,distRect,srcRect, GraphicsUnit.Pixel);
            }

        }

        private void displayScrollbar()
        {
            viewRectWidth = this.Width;
            viewRectHeight = this.Height;

            if (image != null) canvasSize = image.Size;

            // If the zoomed image is wider than view window, show the HScrollBar and adjust the view window
            if (viewRectWidth > canvasSize.Width*zoom)
            {
                hScrollBar.Visible = false;
                viewRectHeight = Height;
            }
            else
            {
                hScrollBar.Visible = true;
                viewRectHeight = Height - hScrollBar.Height;
            }

            // If the zoomed image is taller than view window, show the VScrollBar and adjust the view window
            if (viewRectHeight > canvasSize.Height*zoom)
            {
                vScrollBar.Visible = false;
                viewRectWidth = Width;
            }
            else
            {
                vScrollBar.Visible = true;
                viewRectWidth = Width - vScrollBar.Width;
            }

            // Set up scrollbars
            hScrollBar.Location = new Point(0, Height - hScrollBar.Height);
            hScrollBar.Width = viewRectWidth;
            vScrollBar.Location = new Point(Width - vScrollBar.Width, 0);
            vScrollBar.Height = viewRectHeight;
        }

        private void setScrollbarValues()
        {
            // Set the Maximum, Minimum, LargeChange and SmallChange properties.
            this.vScrollBar.Minimum = 0;
            this.hScrollBar.Minimum = 0;

            // If the offset does not make the Maximum less than zero, set its value. 
            if ((canvasSize.Width * zoom - viewRectWidth) > 0)
            {
                this.hScrollBar.Maximum =(int)( canvasSize.Width * zoom) - viewRectWidth;
            }
            // If the VScrollBar is visible, adjust the Maximum of the 
            // HSCrollBar to account for the width of the VScrollBar.  
            if (this.vScrollBar.Visible)
            {
                this.hScrollBar.Maximum += this.vScrollBar.Width;
            }
            this.hScrollBar.LargeChange = this.hScrollBar.Maximum / 10;
            this.hScrollBar.SmallChange = this.hScrollBar.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.hScrollBar.Maximum += this.hScrollBar.LargeChange;

            // If the offset does not make the Maximum less than zero, set its value.    
            if ((canvasSize.Height * zoom - viewRectHeight) > 0)
            {
                this.vScrollBar.Maximum = (int)(canvasSize.Height * zoom) - viewRectHeight;
            }

            // If the HScrollBar is visible, adjust the Maximum of the 
            // VSCrollBar to account for the width of the HScrollBar.
            if (this.hScrollBar.Visible)
            {
                this.vScrollBar.Maximum += this.hScrollBar.Height;
            }
            this.vScrollBar.LargeChange = this.vScrollBar.Maximum / 10;
            this.vScrollBar.SmallChange = this.vScrollBar.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.vScrollBar.Maximum += this.vScrollBar.LargeChange;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }
    }
}
