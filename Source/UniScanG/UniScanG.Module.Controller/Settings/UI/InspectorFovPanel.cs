using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.UI;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings.UI;

namespace UniScanG.Module.Controller.Settings.Monitor.UI
{
    public partial class InspectorFovPanel : UserControl
    {
        bool onUpdateData = false;

        InspectorInfo inspectorInfo = null;
        public InspectorInfo InspectorInfo
        {
            get { return inspectorInfo; }
        }

        Image2D image;
        public Image2D Image
        {
            get { return image; }
        }

        CanvasPanel canvasPanel = null;

        public InspectorFovPanel(InspectorInfo inspectorInfo)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.inspectorInfo = inspectorInfo;

            canvasPanel = new CanvasPanel() { FastMode = true };
            canvasPanel.SetPanMode();
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.MouseDblClicked += new MouseDblClickedDelegate(f => f.ZoomFit());

            canvasPanel.Dock = DockStyle.Fill;
            panelImage.Controls.Add(canvasPanel);
        }

        ~InspectorFovPanel()
        {
            Release();
        }

        public delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            if (onUpdateData == true)
                return;

            onUpdateData = true;

            left.Value = (Decimal)inspectorInfo.Fov.Left;
            top.Value = (Decimal)inspectorInfo.Fov.Top;
            right.Value = (Decimal)inspectorInfo.Fov.Right;
            bottom.Value = (Decimal)inspectorInfo.Fov.Bottom;

            width.Value = (Decimal)inspectorInfo.Fov.Width;
            height.Value = (Decimal)inspectorInfo.Fov.Height;

            UpdateRegion();

            onUpdateData = false;
        }

        private void UpdateRegion()
        {
            canvasPanel.TempFigures.Clear();

            int fidgureWidth = (int)(5);
            canvasPanel.TempFigures.AddFigure(new RectangleFigure(inspectorInfo.Fov, new Pen(Color.Yellow, fidgureWidth)));
            canvasPanel.Invalidate(true);

            width.Value = (decimal)Math.Max(0, inspectorInfo.Fov.Width);
            height.Value = (decimal)Math.Max(0, inspectorInfo.Fov.Height);
        }

        public void UpdateRegion(Rectangle rect)
        {
            canvasPanel.TempFigures.Clear();
            int fidgureWidth = (int)(5);
            canvasPanel.TempFigures.AddFigure(new RectangleFigure(rect, new Pen(Color.Red, fidgureWidth)));

            canvasPanel.Invalidate(true);
        }

        private void Release()
        {
            if (image != null)
                image.Dispose();
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                //dlg.InitialDirectory = Environment.CurrentDirectory;
                dlg.Filter = "Supported Files(*.png,*.bmp)|*.png;*.bmp|PNG File(*.png)|*.png|BMP File(*.bmp)|*.bmp";
                string initialDirectory = Path.GetFullPath(Path.Combine(this.inspectorInfo.Path, "Model"));
#if !DEBUG
                if (Directory.Exists(initialDirectory))
                    dlg.InitialDirectory = initialDirectory;
#endif

                DialogResult dialogResult = dlg.ShowDialog(this);
                //DialogResult dialogResult = UiHelper.ShowSTADialog(dlg);
                if (dialogResult == DialogResult.OK)
                {
                    new DynMvp.UI.Touch.SimpleProgressForm().Show(() =>
                    {
                        Release();
                        image = new Image2D(dlg.FileName);
                        Bitmap bitmap = null;
                        if (image.Height > 30120)
                            bitmap = image.ClipImage(new Rectangle(0, 0, image.Width, 30120)).ToBitmap();
                        else
                            bitmap = image.ToBitmap();

                        canvasPanel.UpdateImage(bitmap);
                        UpdateData();
                    });
                }
                dlg.Dispose();
            }
            Console.WriteLine("buttonLoadImage_Click Done");
        }

        private void fov_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            int width = Math.Max(0, (int)(right.Value - left.Value));
            int height = Math.Max(0, (int)(bottom.Value - top.Value));

            //inspectorInfo.Fov = new Rectangle((int)left.Value, (int)top.Value, width, height);
            inspectorInfo.Fov = Rectangle.FromLTRB((int)left.Value, (int)top.Value, (int)right.Value, (int)bottom.Value);

            UpdateRegion();
        }
    }
}
