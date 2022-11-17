using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Data;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public delegate void ApplyAllDelegate(RegionInfoG regionInfo);
    public partial class RegionEditor : UserControl
    {
        public RegionInfoG RegionInfo
        {
            get { return this.regionInfo; }
            set {
                this.regionInfo = value;
                this.regionInfoWork = (RegionInfoG)value.Clone();
            }
        }

        public ImageD TrainImage { get { return trainImage; } set { trainImage = value; } }
        public ImageD MajorPatternImage { get { return majorPatternImage; } set { majorPatternImage = value; } }

        public GroupDirection GroupDirection
        {
            get { return this.groupDirection; }
            set { this.groupDirection = value; }
        }

        public ApplyAllDelegate ApplyAll
        {
            get { return this.ApplyAllDelegate; }
            set { this.ApplyAllDelegate = value; }
        }

        ImageD trainImage;
        ImageD majorPatternImage;
        Calibration calibration = null;

        RegionInfoG regionInfo = null;
        RegionInfoG regionInfoWork = null;
        GroupDirection groupDirection;
        ApplyAllDelegate ApplyAllDelegate = null;

        CanvasPanel canvasPanel = null;
        bool clickEditMode = false;

        public RegionEditor(ImageD trainImage, ImageD majorPatternImage, Calibration calibration)
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.FastMode = true;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.canvasPanel.MouseClicked = canvasPanel_MouseClicked;
            this.canvasPanel.FigureSelected += canvasPanel_FigureSelected;
            this.canvasPanel.FigureRemoved += canvasPanel_FigureRemoved;
            this.canvasPanel.FigureModified += canvasPanel_FigureModified;
            this.panelImage.Controls.Add(this.canvasPanel);

            this.trainImage = trainImage;
            this.majorPatternImage = majorPatternImage;
            this.calibration = calibration;
        }

        private void canvasPanel_FigureSelected(List<Figure> figureList)
        {
            if (figureList.Count == 1)
            {
                if (figureList[0].Tag is Tuple<string, Rectangle>)
                {
                    Tuple<string, Rectangle> tag = (Tuple<string, Rectangle>)figureList[0].Tag;
                    UpdateStatusLabel(tag);
                }
            }
        }

        private void UpdateStatusLabel(Tuple<string, Rectangle> figureTag)
        {
            this.toolStripStatusLabelPx.Text = string.Format("W: {0}[px] / H:{1}[px]", figureTag.Item2.Width, figureTag.Item2.Height);

            if (calibration != null)
            {
                SizeF sizeUm = calibration.PixelToWorld(figureTag.Item2.Size);
                this.toolStripStatusLabelUm.Text = string.Format("W: {0:F3}[mm] / H:{1:F3}[mm]", sizeUm.Width/1000, sizeUm.Height/1000);
            }
            //UiHelper.SetControlText(this.toolStripStatusLabel1, string.Format("W: {0}[px] / H:{1}[px]", figureTag.Item2.Width, figureTag.Item2.Height));
        }

        private void canvasPanel_FigureModified(List<Figure> figureList)
        {
            figureList.ForEach(f =>
            {
                if (f.Tag is Tuple<string, Rectangle>)
                    ModifyRect(f, f.GetRectangle().ToRectangle());
            });
        }

        private void ModifyRect(Figure figure, Rectangle rectangle)
        {
            Tuple<string, Rectangle> tag = (Tuple<string, Rectangle>)figure.Tag;
            List<Rectangle> list = null;
            switch (tag.Item1)
            {
                case "Pass":
                    list = this.regionInfoWork.PassRectList;
                    break;
                case "Block":
                    list = this.regionInfoWork.BlockRectList;
                    break;
            }

            if (list != null)
            {
                int idx = list.IndexOf(tag.Item2);
                if (idx >= 0)
                {
                    Tuple<string, Rectangle> newTag = new Tuple<string, Rectangle>(tag.Item1, list[idx] = rectangle);
                    figure.Tag = newTag;
                    UpdateStatusLabel(newTag);
                }
            }
        }

        private void canvasPanel_MouseClicked(CanvasPanel canvasPanel, PointF point, ref bool processingCancelled)
        {
            if (clickEditMode)
            {
                if (this.regionInfoWork.InspectElementList.Count == 0)
                    return;

                Point leftTop = Point.Empty;
                leftTop.X = this.regionInfoWork.InspectElementList.Min(f => f.Rectangle.X);
                leftTop.Y = this.regionInfoWork.InspectElementList.Min(f => f.Rectangle.Y);
                PointF adjClickPt = point;

                for (int y = 0; y < this.regionInfoWork.AdjPatRegionList.GetLength(0); y++)
                {
                    for (int x = 0; x < this.regionInfoWork.AdjPatRegionList.GetLength(1); x++)
                    {
                        Rectangle rect = this.regionInfoWork.AdjPatRegionList[y, x];
                        if (rect.Contains(Point.Round(adjClickPt)))
                        {
                            Point pt = new Point(x, y);
                            int idx = this.regionInfoWork.DontcarePatLocationList.FindIndex(f => f.Equals(pt));

                            if (idx < 0)
                                this.regionInfoWork.DontcarePatLocationList.Add(pt);
                            else
                                this.regionInfoWork.DontcarePatLocationList.RemoveAt(idx);
                        }
                    }
                }
            }
            UpdateFigure();
        }


        private void canvasPanel_FigureRemoved(List<Figure> figureList)
        {
            List<Tuple<string, Rectangle>> tagList = figureList.Select(f => f.Tag as Tuple<string, Rectangle>).ToList();
            tagList.ForEach(f =>
            {
                List<Rectangle> list = null;
                switch (f.Item1)
                {
                    case "Pass":
                        list = this.regionInfoWork.PassRectList;
                        break;
                    case "Block":
                        list = this.regionInfoWork.BlockRectList;
                        break;
                    case "Cretical":
                        list = this.regionInfoWork.CreticalPointList;
                        break;
                }
                if (list != null)
                    list.Remove(f.Item2);
            });
            UpdateFigure();
        }

        private void RegionEditor_Load(object sender, EventArgs e)
        {
            UpdateData();
            UpdateImage();
            UpdateFigure();

            if (SystemManager.Instance().CurrentModel != null)
                SystemManager.Instance().CurrentModel.Modified = true;
        }

        private ImageD GetCurrentImage()
        {
            if (showTrainImage.Checked)
                return this.trainImage;
            else if (showPatternImage.Checked)
                return this.majorPatternImage;

            return null;
        }

        private void UpdateImage()
        {
            ImageD currentImageD = GetCurrentImage();
            Bitmap bitmap = currentImageD?.ToBitmap();
            this.canvasPanel.UpdateImage(bitmap);
            this.canvasPanel.ZoomFit();
            //bitmap?.Dispose();
        }

        private void UpdateData()
        {
            oddEvenPair.Checked = this.regionInfoWork.OddEvenPair;
            this.editMode.Checked = clickEditMode;
        }

        private void UpdateFigure()
        {
            this.canvasPanel.WorkingFigures.Clear();
            this.canvasPanel.BackgroundFigures.Clear();

            FigureGroup figure = (FigureGroup)regionInfoWork.GetFigure(true);
            List<Figure> selectableFigureList = figure.FigureList.FindAll(f => f.Selectable);
            List<Figure> nonSelectableFigureList = figure.FigureList.FindAll(f => !f.Selectable);

            if (!this.clickEditMode)
            {
                this.canvasPanel.WorkingFigures.AddFigure(selectableFigureList.ToArray());
            }
            else
            {
                this.canvasPanel.BackgroundFigures.AddFigure(nonSelectableFigureList.ToArray());
            }

            this.canvasPanel.Invalidate();
        }

        private void RegionEditor_SizeChanged(object sender, EventArgs e)
        {
            this.canvasPanel.ZoomFit();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Apply();
            MessageForm.Show(null, "Apply Success");
        }

        private void Apply()
        {
            //this.regionInfo.Dispose();
            
            this.regionInfo = (RegionInfoG)this.regionInfoWork.Clone();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
            MessageForm.Show(null, "Reset Success");
            UpdateData();
            UpdateFigure();
        }

        private void Reset()
        {
            this.regionInfoWork = (RegionInfoG)this.regionInfo.Clone();
        }

        private void buttonApplyAll_Click(object sender, EventArgs e)
        {
            Apply();
            ApplyAllDelegate(this.regionInfo);
            MessageForm.Show(null, "Apply All Success");
        }

        private void Image_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "BITMAP(*.bmp) Files|*.bmp|JPEG(*.jpg) Files|*.jpg";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                this.canvasPanel.Image.Save(dlg.FileName);
            }
        }

        private void loadImage_Click(object sender, EventArgs e)
        {
            ImageD currentImage = GetCurrentImage();
            if (currentImage == null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BITMAP(*.bmp) Files|*.bmp|JPEG(*.jpg) Files|*.jpg";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            ImageD loadImage = new Image2D();
            loadImage.LoadImage(dlg.FileName);

            Rectangle currentImageRect = new Rectangle(Point.Empty, currentImage.Size);
            Rectangle loadImageRect = new Rectangle(Point.Empty, loadImage.Size);
            AdjustSize(ref currentImageRect, ref loadImageRect);

            currentImage.Clear();
            currentImage.CopyFrom(loadImage, loadImageRect, loadImage.Pitch, currentImageRect.Location);

            UpdateImage();
        }

        private void AdjustSize(ref Rectangle rect1, ref Rectangle rect2)
        {
            PointF center1 = DrawingHelper.CenterPoint(rect1);
            PointF center2 = DrawingHelper.CenterPoint(rect2);
            Size minSize = new Size(Math.Min(rect1.Width, rect2.Width), Math.Min(rect1.Height, rect2.Height));

            rect1 = Rectangle.Round(DrawingHelper.FromCenterSize(center1, minSize));
            rect2 = Rectangle.Round(DrawingHelper.FromCenterSize(center2, minSize));
        }

        private void oddEvenPair_CheckedChanged(object sender, EventArgs e)
        {
            this.regionInfoWork.OddEvenPair = oddEvenPair.Checked;
        }

        private void editMode_CheckedChanged(object sender, EventArgs e)
        {
            this.clickEditMode = this.editMode.Checked;
            UpdateFigure();
        }

        private void addPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickEditMode)
                return;

            Point pt = Cursor.Position;
            Point cPt = this.canvasPanel.PointToClient(pt);
            Point iPt = this.canvasPanel.PointToImage(cPt);
            //Point iPt2 = this.canvasPanel.PointToImage(pt);
            Rectangle passRect = DrawingHelper.FromCenterSize(iPt, new Size(50, 50));
            this.regionInfoWork.PassRectList.Add(passRect);
            UpdateFigure();
        }

        private void addBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickEditMode)
                return;

            Point pt = Cursor.Position;
            Point cPt = this.canvasPanel.PointToClient(pt);
            Point iPt = this.canvasPanel.PointToImage(cPt);
            //Point iPt2 = this.canvasPanel.PointToImage(pt);
            Rectangle blkRect = DrawingHelper.FromCenterSize(iPt, new Size(50, 50));
            this.regionInfoWork.BlockRectList.Add(blkRect);
            UpdateFigure();
        }

        private void toolStripStatusLabel_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm("Size? (um)", "5000");
            inputForm.ValidCheckFunc = new InputFormValidCheckFunc(f => 
            {
                float ff;
                return float.TryParse(f, out ff);
            });

            if (inputForm.ShowDialog(this) != DialogResult.OK)
                return;

            float size = float.Parse(inputForm.InputText);
            SizeF newSizeUm = new SizeF(size, size);
            SizeF newSizePx = this.calibration.WorldToPixel(newSizeUm);

            Figure[] selectedFigures = this.canvasPanel.GetSelectedFigures();
            Array.ForEach(selectedFigures, f =>
           {
               if (f.Tag is Figure)
                   f = (Figure)f.Tag;

               if (f.Tag is Tuple<string, Rectangle>)
               {
                   RectangleF rect = ((Tuple<string, Rectangle>)f.Tag).Item2;
                   SizeF diff = DrawingHelper.Subtract(rect.Size, newSizePx);
                   rect.Inflate(-diff.Width / 2, -diff.Height / 2);
                   ModifyRect(f, Rectangle.Round(rect));
               }
           });
            UpdateFigure();
        }
    }
}
