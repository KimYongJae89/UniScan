using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;

namespace DynMvp.Data.UI
{
    public partial class MaskEditor : Form
    {
        DrawBox drawBox;
        bool modified = false;
        bool onUpdate = false;

        ImageD maskImage;
        FigureType addFigureType;

        public MaskEditor()
        {
            InitializeComponent();

            drawBox = new DrawBox();

            this.drawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawBox.Location = new System.Drawing.Point(246, 0);
            this.drawBox.Name = "DrawBox";
            this.drawBox.Size = new System.Drawing.Size(511, 507);
            this.drawBox.TabIndex = 1;
            this.drawBox.TabStop = false;
            this.drawBox.Enable = true;
            this.drawBox.FigureAdd = new FigureAddDelegate(drawBox_FigureAdd);
            this.drawBox.FigureMoved = new FigureMovedDelegate(drawBox_FigureMoved);
            this.drawBox.FigureSelected = new FigureSelectedDelegate(drawBox_FigureSelected);
            this.drawBox.FigureCopy = new FigureCopyDelegate(drawBox_FigureCopy);
            this.drawBox.FigureGroup = new FigureGroup();

            //this.Controls.Add(drawBox);
            //MaskEditor_Fill_Panel.Controls.Add(drawBox);
            MaskEditor_Fill_Panel.ClientArea.Controls.Add(drawBox);
            //MaskEditor_Fill_Panel.Dock = DockStyle.Left;
            //MaskEditor_Fill_Panel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;

            btnOK.Text = StringManager.GetString(this.GetType().FullName, btnOK);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel);
            toolStripButtonCircle.Text = StringManager.GetString(this.GetType().FullName, toolStripButtonCircle);
            toolStripButtonRectangle.Text = StringManager.GetString(this.GetType().FullName, toolStripButtonRectangle);
            toolStripButtonDelete.Text = StringManager.GetString(this.GetType().FullName, toolStripButtonDelete);
        }

        public void SetImage(Image2D image)
        {
            if (drawBox.Image != null)
                drawBox.Image.Dispose();

            maskImage = image;
            drawBox.UpdateImage(maskImage.ToBitmap());
        }

        public ImageD GetImage()
        {
            return maskImage;
        }

        public void SetMaskFigures(FigureGroup figureGroup)
        {
            drawBox.FigureGroup = figureGroup;
        }

        private void toolStripSplitButtonAddFigure_ButtonClick(object sender, EventArgs e)
        {
            if (drawBox.AddFigureMode)
            {
                SetAddFigureMode(false);
            }
            else
            {
                SetAddFigureMode(true);
            }
        }

        private void toolStripButtonCircle_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Ellipse);
        }

        private void toolStripButtonRectangle_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Rectangle);
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Line);
        }

        private void toolStripButtonFreeLine_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Polygon);
        }

        private void SetAddFigureMode(FigureType figureType)
        {
            if (addFigureType == figureType)
            {
                //addFigureType = FigureType.None;
                drawBox.AddFigureMode = false;
                drawBox.TrackerShape = FigureType.Rectangle;
            }
            else
            {
                addFigureType = figureType;
                drawBox.TrackerShape = figureType;
                drawBox.AddFigureMode = true;
            }

            UpdateButtonState();
        }

        private void SetAddFigureMode(bool addState)
        {
            drawBox.AddFigureMode = addState;
            if(addState)
            drawBox.TrackerShape = addFigureType;
            else
                drawBox.TrackerShape = FigureType.Rectangle;
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            foreach(ToolStripMenuItem item in toolStripSplitButtonAddFigure.DropDownItems)
            {
                item.Checked = false;
            }

            toolStripSplitButtonAddFigure.BackColor = SystemColors.Control;
            MaskEditor_Fill_Panel.Cursor = Cursors.Default;
            if (drawBox.AddFigureMode)
            {
                toolStripSplitButtonAddFigure.BackColor = Color.LightGreen;
                MaskEditor_Fill_Panel.Cursor = Cursors.Cross;
            }

            switch (addFigureType)
            {
                case FigureType.Ellipse:
                    toolStripButtonCircle.Checked = true;
                    break;
                case FigureType.Rectangle:
                    toolStripButtonRectangle.Checked = true;
                    break;
                case FigureType.Line:
                    toolStripButtonLine.Checked = true;
                    break;
                case FigureType.Polygon:
                    toolStripButtonFreeLine.Checked = true;
                    break;
            }
        }

        private void drawBox_FigureMoved(List<Figure> figureList)
        {

        }

        private void drawBox_FigureCopy(List<Figure> figureList)
        {
            drawBox.ResetSelection();

            foreach (Figure figure in figureList)
            {
                Figure newFigure = (Figure)figure.Clone();

                RotatedRect rectangle = figure.GetRectangle();
                newFigure.SetRectangle(rectangle);

                drawBox.FigureGroup.AddFigure(newFigure);

                drawBox.SelectFigure(newFigure);
            }
        }

        private void drawBox_FigureSelected(Figure figure, bool select)
        {

        }

        private void drawBox_FigureAdd(List< PointF > pointList, FigureType figureType)
        {
            //drawBox.AddFigureMode = false;
            //this.Cursor = Cursors.Default;

            if (pointList.Count<=1)
                return;
            PointF pt1 = pointList.First();
            PointF pt2 = pointList.Last();
            RectangleF rectangle = RectangleF.FromLTRB(
                Math.Min(pt1.X, pt2.X),
                Math.Min(pt1.Y, pt2.Y),
                Math.Max(pt1.X, pt2.X),
                Math.Max(pt1.Y, pt2.Y));

            Figure figure = null;
            switch (addFigureType)
            {
                case FigureType.Rectangle:
                    figure = new RectangleFigure(rectangle, new Pen(Color.Red), new SolidBrush(Color.Red));
                    break;
                case FigureType.Ellipse:
                    figure = new EllipseFigure(rectangle, new Pen(Color.Red), new SolidBrush(Color.Red));
                    break;
                case FigureType.Line:
                    figure = new LineFigure(pt1, pt2, new Pen(Color.Red, 5));
                    break;
                case FigureType.Polygon:
                    figure = new PolygonFigure(pointList, false, new Pen(Color.Red, 5), null);
                    ((PolygonFigure)figure).IsEditable = false;
                    break;
            }
            drawBox.FigureGroup.AddFigure(figure);
            modified = true;
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            drawBox.RemoveSelectedFigure();
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            string debugString = "Apply drawing mask figure to image?";
            DialogResult dialogResult = MessageBox.Show(debugString, "Question", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            ApplyMaskFigure();
        }

        private void ApplyMaskFigure()
        {
            AlgoImage algoMaskImage = ImageBuilder.Build("", maskImage, (maskImage.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            //algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));

            ImageD maksImage2 = GetMaskImage();
            //maksImage2.SaveImage(@"D:\maksImage2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            AlgoImage algoMaskImage2 = ImageBuilder.Build("", maksImage2, (maksImage2.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            //algoMaskImage2.Save(@"algoMaskImage2.bmp", new DebugContext(true, @"D:\"));

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoMaskImage);
            imageProcessing.Or(algoMaskImage, algoMaskImage2, algoMaskImage);
            maskImage = algoMaskImage.ToImageD();

            drawBox.FigureGroup.Clear();
            UpdateData();
            modified = false;
        }
        private ImageD GetMaskImage()
        {
            Bitmap maskImage = new Bitmap(drawBox.Image.Width, drawBox.Image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            ImageHelper.Clear(maskImage, 0);

            FigureGroup figureGroup = this.drawBox.FigureGroup;
            //if (figureGroup.FigureExist == false)
            //{

            //    return Image2D.ToImage2D(maskImage);
            //}

            Graphics g = Graphics.FromImage(maskImage);
            figureGroup.SetTempBrush(new SolidBrush(Color.White));
            figureGroup.Draw(g, new CoordTransformer(), true);
            figureGroup.ResetTempProperty();
            g.Dispose();

            //ImageHelper.SaveImage(maskImage, @"d:\maskImage.bmp");
            maskImage = ImageHelper.MakeGrayscale(maskImage);
            AlgoImage algoImage = ImageBuilder.Build("", Image2D.FromBitmap(maskImage), ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            imageProcessing.Binarize(algoImage, 1);
            return algoImage.ToImageD();
            //return Image2D.ToImage2D(maskImage);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                string message = "Mask is modified. apply?";
                DialogResult dialogResult = MessageBox.Show(message, "Question", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                {
                    ApplyMaskFigure();
                }else if(dialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void toolStripButtonInvert_Click(object sender, EventArgs e)
        {
            AlgoImage algoMaskImage = ImageBuilder.Build("", maskImage, (maskImage.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoMaskImage);
            imageProcessing.Not(algoMaskImage, algoMaskImage);
            algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));
            maskImage = algoMaskImage.ToImageD();

            UpdateData();
        }

        private void UpdateData()
        {
            if (onUpdate)
            {
                return;
            }

            onUpdate = true;

            drawBox.UpdateImage( maskImage.ToBitmap());

            onUpdate = false;
        }
    }
}
