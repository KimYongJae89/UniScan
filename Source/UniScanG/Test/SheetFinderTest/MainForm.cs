using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.SheetFinder.ReversOffset;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Inspect;
using UniScanG.Vision;

namespace SheetFinderTest
{
    public delegate void GapFoundedDelegate(int frameNo, Rectangle fiducialRect);
    public delegate void PracticeGrabbedDelegate(ImageD image, int no, Size size, Point basePoint, object tag);
    public partial class MainForm : Form
    {
        Practice practice = null;
        CanvasPanel canvasPanelFOld = new CanvasPanel();
        CanvasPanel canvasPanelFNew = new CanvasPanel();
        CanvasPanel canvasPanelS = new CanvasPanel();
        SheetFinderBaseParam param = null;
        Image2D image2D = null;
        int lastFrameNo = -1;
        public string DebugPath { get; set; } = @"C:\temp";

        public MainForm()
        {
            InitializeComponent();

            //this.param = new SheetFinderV2Param { BoundaryHeight = 1500, ProjectionBinalizeMul = 1.1f, BaseXSearchDir = BaseXSearchDir.Left2Right };
            //this.param = new SheetFinderRVOSParam { BoundaryHeight = 1000, BaseXSearchDir = BaseXSearchDir.Left2Right };

            canvasPanelFOld = new CanvasPanel();
            canvasPanelFOld.Dock = DockStyle.Fill;
            canvasPanelFOld.FastMode = true;
            canvasPanelFOld.SetPanMode();
            canvasPanelFOld.MouseDblClicked += new MouseDblClickedDelegate(f => f.ZoomFit());
            panelFrameOld.Controls.Add(canvasPanelFOld);

            canvasPanelFNew = new CanvasPanel();
            canvasPanelFNew.Dock = DockStyle.Fill;
            canvasPanelFNew.FastMode = true;
            canvasPanelFNew.SetPanMode();
            canvasPanelFNew.MouseDblClicked += new MouseDblClickedDelegate(f => f.ZoomFit());
            panelFrameNew.Controls.Add(canvasPanelFNew);

            canvasPanelS = new CanvasPanel();
            canvasPanelS.Dock = DockStyle.Fill;
            canvasPanelS.FastMode = true;
            canvasPanelS.SetPanMode();
            canvasPanelS.MouseDblClicked += new MouseDblClickedDelegate(f => f.ZoomFit());
            panelPattern.Controls.Add(canvasPanelS);

            frameHeight.Minimum = 0;
            frameHeight.Maximum = int.MaxValue;

            sheetFinderSelector.Items.Add(new SheetFinderV2Param { BoundaryHeightMm = 21, ProjectionBinalizeMul = 1.1f, BaseXSearchDir = BaseXSearchDir.Left2Right });
            sheetFinderSelector.Items.Add(new SheetFinderRVOSParam { BoundaryHeightMm = 14, BaseXSearchDir = BaseXSearchDir.Left2Right });
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            canvasPanelFNew.ZoomFit();
            canvasPanelFOld.ZoomFit();
            canvasPanelS.ZoomFit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.frameHeight.Value = 15000; //this.image2D.Height;
            this.debugPath.DataBindings.Add(new Binding("Text", this, "DebugPath"));
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (practice == null)
            {
                if (this.image2D == null)
                    return;

                if (this.param == null)
                    return;

                ClearResult();

                practice = new Practice();
                practice.FrameGrabbed = practice_FrameGrabbed;
                practice.GapFounded = practice_GapFound;
                practice.SheetFounded = practice_SheetFound;
                OperationSettings.Instance().SaveDebugImage = checkBox1.Checked;
                practice.Start(this.image2D, this.param, (int)frameHeight.Value,(float)pixelRes.Value, DebugPath);
            }
            else
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() =>
                {
                    practice.Stop();
                });
                practice = null;
            }
        }

        private void practice_FrameGrabbed(ImageD foundImage, int no, Size size, Point basePoint, object tag)
        {
            Bitmap bitmap = foundImage.Resize(0.1f).ToBitmap();

            //canvasPanelFOld.WorkingFigures.Clear();
            //canvasPanelFOld.WorkingFigures.AddFigure(canvasPanelFNew.WorkingFigures);
            canvasPanelFOld.WorkingFigures.Clear();
            canvasPanelFOld.WorkingFigures.AddFigure(canvasPanelFNew.WorkingFigures.FigureList.ToArray());
            canvasPanelFOld.UpdateImage(canvasPanelFNew.Image, canvasPanelFOld.Image == null);
            canvasPanelFOld.Invalidate();
            //canvasPanelFOld.ZoomFit();
            //canvasPanelFOld.SetPanMode();

            canvasPanelFNew.WorkingFigures.Clear();
            canvasPanelFNew.WorkingFigures.AddFigure(new TextFigure(no.ToString(), Point.Empty, new Font("Arial", 150), Color.Red, StringAlignment.Far, StringAlignment.Near));
            canvasPanelFNew.UpdateImage(bitmap, canvasPanelFNew.Image == null);
            canvasPanelFNew.Invalidate();

            lastFrameNo = no;
            //canvasPanelFNew.ZoomFit();
            //canvasPanelFNew.SetPanMode();
        }

        private void practice_GapFound(int frameNo, Rectangle fiducialRect)
        {
            CanvasPanel canvasPanel = null;
            if (frameNo == lastFrameNo)
                canvasPanel = canvasPanelFNew;
            else if (frameNo == lastFrameNo - 1)
                canvasPanel = canvasPanelFOld;

            if (canvasPanel == null)
                return;

            Rectangle rect = DrawingHelper.Div(fiducialRect, 10);
            canvasPanel.WorkingFigures.AddFigure(new RectangleFigure(rect, new Pen(Color.Red)));
            canvasPanel.Invalidate();
            //canvasPanel.Update();
        }

        private void practice_SheetFound(ImageD foundImage, int no, Size size, Point basePoint, object tag)
        {
            //canvasPanelS.UpdateImage(foundImage.ToBitmap());
            //canvasPanelS.ZoomFit();
            AddResult(foundImage, no, size, basePoint, tag);

            PointF pt0 = DrawingHelper.Div(new PointF(basePoint.X, 0), 10);
            PointF pt1 = DrawingHelper.Div(new PointF(basePoint.X, size.Height), 10);
            canvasPanelS.WorkingFigures.AddFigure(new LineFigure(pt0, pt1, new Pen(Color.Yellow)));
            canvasPanelS.Invalidate();
        }


        private delegate void ClearResultDelegate();
        private void ClearResult()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearResultDelegate(ClearResult));
                return;
            }
            this.canvasPanelFNew.UpdateImage(null);
            this.canvasPanelFOld.UpdateImage(null);
            this.canvasPanelS.UpdateImage(null);
            dataGridView.Rows.Clear();
        }

        private void AddResult(ImageD imageD, int no, Size size, Point basePoint, object tag)
        {
            if (InvokeRequired)
            {
                Invoke(new PracticeGrabbedDelegate(AddResult), imageD, no, size, basePoint, tag);
                return;
            }

            List<int> list = tag as List<int>;
            string joinString = string.Join("/", list.ToArray());

            bool moveLastRow = false;
            if (dataGridView.SelectedRows.Count > 0)
                moveLastRow = dataGridView.Rows.Count == dataGridView.SelectedRows[0].Index + 1;
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView);
            row.Cells[0].Value = no;
            row.Cells[1].Value = size.Height;
            row.Cells[2].Value = $"{basePoint.X}, {basePoint.Y}";
            row.Cells[3].Value = joinString;
            int newIndex = dataGridView.Rows.Add(row);
            if (moveLastRow)
            {
                dataGridView.Rows[newIndex].Selected = true;
                dataGridView.FirstDisplayedScrollingRowIndex = newIndex;
            }

            Bitmap bitmap = imageD.ToBitmap();
            canvasPanelS.WorkingFigures.Clear();
            canvasPanelS.WorkingFigures.AddFigure(new TextFigure(no.ToString(), Point.Empty, new Font("Arial", 150), Color.Blue, StringAlignment.Far, StringAlignment.Near));
            canvasPanelS.UpdateImage(bitmap, canvasPanelS.Image == null);
            canvasPanelS.Invalidate();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string defaultFileName = @"D:\UniScan\Gravure_Inspector\VirtualImage\440-10BFSF511-GA03\Image_C00_S000_L00.bmp";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image|*.png;*.bmp|PNG|*.png|BMP|*.bmp|ALL|*.*";

            if (File.Exists(defaultFileName))
                dlg.FileName = defaultFileName;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    if (this.image2D == null)
                        this.image2D = new Image2D();

                    this.image2D.LoadImage(dlg.FileName);
                    //if (this.image2D.NumBand > 1)
                    //    this.image2D = this.image2D.GetGrayImage();
                    Bitmap bitmap = this.image2D.ToBitmap();
                    //if (this.image2D.Height < 30120)
                    //    bitmap = this.image2D.ToBitmap();
                    //else
                    //    bitmap = this.image2D.ClipImage(new Rectangle(0, 0, this.image2D.Width, 30120)).ToBitmap();

                    //bitmap.Save(@"C:\temp\bitmapbitmap.bmp");
                    canvasPanelS.UpdateImage(bitmap);
                    canvasPanelS.ZoomFit();
                    canvasPanelS.Invalidate();

                    //this.image2D.SaveImage(@"C:\temp\this.image2D.bmp");

                });
            }
        }

        private void buttonOpenDebugPath_Click(object sender, EventArgs e)
        {
            string debugPath = this.debugPath.Text;
            if (Directory.Exists(debugPath))
                Process.Start(debugPath);
        }

        private void sheetFinderSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.param = (SheetFinderBaseParam)sheetFinderSelector.SelectedItem;
            this.propertyGrid1.SelectedObject = this.param;
        }
    }
}
