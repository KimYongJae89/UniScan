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
using DynMvp.Base;
using DynMvp.Vision;

namespace RCITest.Controls
{
    public partial class ImageControl : UserControl, INotifyPropertyChanged
    {
        public event OnViewPortChangedDelegate OnZoomScaleChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool FixedKey { get; private set; }

        public string Key
        {
            get => this.key;
            private set
            {
                this.key = value;
                OnPropChanged("Key");
            }
        }
        string key;

        public CanvasPanel CanvasPanel { get; set; }

        CustomChartControl chartH;
        CustomChartControl chartV;
        int chartSize = 0;
        public int ImageScale { get; private set; } = 10;

        public ImageD ImageD { get; private set; }

        public ImageControl()
        {
            InitializeComponent();

            this.FixedKey = false;
            this.Key = "";
            this.labelKey.DataBindings.Add(new Binding("Text", this, "Key", false, DataSourceUpdateMode.OnPropertyChanged));

            this.tableLayoutPanel1.Controls.Add(this.CanvasPanel = new CanvasPanel() { Dock = DockStyle.Fill }, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartH = new CustomChartControl() { Dock = DockStyle.Fill, IsRotate90 = false }, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chartV = new CustomChartControl() { Dock = DockStyle.Fill, IsRotate90 = true }, 1, 0);

            this.CanvasPanel.OnViewPortChanged += CanvasPanel_OnZoomScaleChanged;
        }

        public ImageControl(string key) : this()
        {
            this.FixedKey = true;
            this.Key = key;
        }

        private delegate void OnPropChangedDelegate(string name);
        private void OnPropChanged(string name)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new OnPropChangedDelegate(OnPropChanged), name);
                return;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void CanvasPanel_OnZoomScaleChanged(CanvasPanel canvasPanel)
        {
            RectangleF viewPort = canvasPanel.ViewPort;
            RectangleF mul = DrawingHelper.Mul(viewPort, this.ImageScale);
            this.chartH.SetAxis(mul.Left, mul.Right);
            this.chartV.SetAxis(mul.Top, mul.Bottom);

            this.OnZoomScaleChanged?.Invoke(canvasPanel);
        }

        private void ImageControl_Load(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.RowStyles[1].Height = chartSize;
            this.tableLayoutPanel1.ColumnStyles[1].Width = chartSize;

            this.CanvasPanel.SetPanMode();
            this.CanvasPanel.MouseDoubleClick += new MouseEventHandler((s, a) => ((CanvasPanel)s).ZoomFit());
        }

        public void Set(string key, ImageD imageD)
        {
            if (!this.FixedKey)
                this.Key = key;

            this.ImageD = imageD?.Clone();
            if (imageD == null)
            {
                this.CanvasPanel.UpdateImage(null);
                return;
            }

            using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
            {
                ImageProcessing ip = Program.ImageProcessing;
                Size resize = DrawingHelper.Div(algoImage.Size, this.ImageScale);

                using (AlgoImage resizeAlgoImage = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Grey, resize))
                {
                    ip.Resize(algoImage, resizeAlgoImage);
                    this.CanvasPanel.UpdateImage(resizeAlgoImage.ToBitmap(), true);
                }

                float[] projW = ip.Projection(algoImage, Direction.Horizontal, ProjectionType.Mean);
                this.chartH.SetData(projW);

                float[] projH = ip.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);
                this.chartV.SetData(projH);
            }
            UpdateSize();
        }

        private void ImageControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(UpdateSize));
                return;
            }

            if (this.ImageD == null)
                return;

            Size imgBoxMax = new Size(this.tableLayoutPanel1.Width - chartSize, this.tableLayoutPanel1.Height - chartSize);
            if (imgBoxMax.Width <= 0 || imgBoxMax.Height <= 0)
                return;

            float imgRatio = this.ImageD.Width * 1f / this.ImageD.Height;
            float boxRatio = imgBoxMax.Width * 1f / imgBoxMax.Height;
            if (imgRatio > boxRatio)
            // image Width에 맞게 Cell[0,0] Width 조정.
            {
                this.tableLayoutPanel1.ColumnStyles[0].Width = imgBoxMax.Width;
                this.tableLayoutPanel1.RowStyles[0].Height = imgBoxMax.Width / imgRatio;
            }
            else
            // image Heigth에 맞게 Cell[0,0] Height 조정.
            {
                this.tableLayoutPanel1.RowStyles[0].Height = imgBoxMax.Height;
                this.tableLayoutPanel1.ColumnStyles[0].Width = imgBoxMax.Height * boxRatio;
            }
            this.CanvasPanel.ZoomFit();
        }
    }
}
