using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Data.UI
{
    public partial class FigurePropertyForm : Form
    {
        Figure figure = null;
        public Figure Figure
        {
            get { return figure; }
            set { figure = value; }
        }

        Font font = null;
        Pen pen = null;
        SolidBrush brush = null;
        Color textColor;
        StringAlignment alignment;

        public FigurePropertyForm()
        {
            InitializeComponent();

            labelPosition.Text = StringManager.GetString(this.GetType().FullName,labelPosition.Text);
            labelWidth.Text = StringManager.GetString(this.GetType().FullName,labelWidth.Text);
            labelLineColor.Text = StringManager.GetString(this.GetType().FullName,labelLineColor.Text);
            lineThicknessLabel.Text = StringManager.GetString(this.GetType().FullName,lineThicknessLabel.Text);
            labelBackgroundColor.Text = StringManager.GetString(this.GetType().FullName,labelBackgroundColor.Text);
            labelTextColor.Text = StringManager.GetString(this.GetType().FullName,labelTextColor.Text);
            labelFont.Text = StringManager.GetString(this.GetType().FullName,labelFont.Text);
            alignmentLabel.Text = StringManager.GetString(this.GetType().FullName,alignmentLabel.Text);
            okButton.Text = StringManager.GetString(this.GetType().FullName,okButton.Text);
            cancelButton.Text = StringManager.GetString(this.GetType().FullName,cancelButton.Text);

        }

        private void GetRectProperty(Figure figure)
        {
            RotatedRect rectangle = figure.GetRectangle();
            txtWidth.Text = rectangle.Width.ToString();
            txtHeight.Text = rectangle.Height.ToString();
            txtPositionX.Text = rectangle.X.ToString();
            txtPositionY.Text = rectangle.Y.ToString();
        }
        private void GetProperty(Figure figure)
        {
            if (figure is TextFigure)
            {
                TextFigure textFigure = figure as TextFigure;

                if (font == null)
                {
                    font = (Font)textFigure.FigureProperty.Font.Clone();
                    textColor = textFigure.FigureProperty.TextColor;
                    alignment = textFigure.FigureProperty.Alignment;
                    
                }
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                if (pen == null)
                    pen = (Pen)figure.FigureProperty.Pen.Clone();

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    if (brush == null)
                        brush = (SolidBrush)figure.FigureProperty.Brush.Clone();
                }
            }
        }

        private void SetProperty(Figure figure)
        {
            if (figure is TextFigure)
            {
                TextFigure textFigure = figure as TextFigure;

                textFigure.FigureProperty.Font = (Font)font.Clone();
                textFigure.FigureProperty.TextColor = textColor;
                textFigure.FigureProperty.Alignment = alignment;
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                figure.FigureProperty.Pen = (Pen)pen.Clone();

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    figure.FigureProperty.Brush = (SolidBrush)brush.Clone();
                }
            }
        }
        private void SetRectProperty(Figure figure)
        {
            if (txtWidth.Text.ToString() != "" && txtHeight.Text.ToString() != "" && txtPositionX.Text.ToString() != "" && txtPositionY.Text.ToString() != "")
            {
                RotatedRect rectangle = figure.GetRectangle();
                rectangle.Width = float.Parse(txtWidth.Text.ToString());
                rectangle.Height = float.Parse(txtHeight.Text.ToString());
                rectangle.X = float.Parse(txtPositionX.Text.ToString());
                rectangle.Y = float.Parse(txtPositionY.Text.ToString());
                figure.SetRectangle(rectangle);
            }
        }

        private void FigurePropertyForm_Load(object sender, EventArgs e)
        {
            if (figure is FigureGroup)
            {
                FigureGroup figureGroup = figure as FigureGroup;
                foreach (Figure subFigure in figureGroup)
                {
                    GetProperty(subFigure);
                }
            }
            else
            {
                GetProperty(figure);
                GetRectProperty(figure);
            }

            if (font != null)
            {
                switch (alignment)
                {
                    case StringAlignment.Near:
                        alignmentCombo.SelectedIndex = 0;
                        break;
                    case StringAlignment.Center:
                        alignmentCombo.SelectedIndex = 1;
                        break;
                    case StringAlignment.Far:
                        alignmentCombo.SelectedIndex = 2;
                        break;
                }
                textColorBox.BackColor = textColor;
                labelSampleText.Font = font;
            }
            else
            {
                textColorBox.Enabled = false;
                buttonSelectFont.Enabled = false;
                alignmentCombo.Enabled = false;
            }
            
            if (pen != null)
            {
                lineThicknessEdit.Value = (int)pen.Width;
                lineColorBox.BackColor = pen.Color;
            }
            else
            {
                lineThicknessEdit.Enabled = false;
                lineColorBox.Enabled = false;
            }

            if (brush != null)
            {
                backgroundColorBox.BackColor = brush.Color;
            }
            else
            {
                backgroundColorBox.Enabled = false;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (font != null)
            {
                font = (Font)font.Clone(); //  new Font(font.FontFamily, (float)fontSizeEdit.Value, font.Style);

                switch (alignmentCombo.SelectedIndex)
                {
                    case 0:
                        alignment = StringAlignment.Near;
                        break;
                    case 1:
                        alignment = StringAlignment.Center;
                        break;
                    case 2:
                        alignment = StringAlignment.Far;
                        break;
                }

                textColor = textColorBox.BackColor;
            }

            if (pen != null)
                pen = new Pen(lineColorBox.BackColor, (float)lineThicknessEdit.Value);

            if (brush != null)
                brush = new SolidBrush(backgroundColorBox.BackColor);

            if (figure is FigureGroup)
            {
                FigureGroup figureGroup = figure as FigureGroup;
                foreach (Figure subFigure in figureGroup)
                {
                    SetProperty(subFigure);
                    SetRectProperty(subFigure);
                }
            }
            else
            {
                SetProperty(figure);
                SetRectProperty(figure);
            }

            Close();
        }



        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lineColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = lineColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lineColorBox.BackColor = dlg.Color;
            }
        }

        private void backgroundColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = backgroundColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                backgroundColorBox.BackColor = dlg.Color;
            }
        }

        private void textColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = textColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textColorBox.BackColor = dlg.Color;
            }
        }

        private void buttonSelectFont_Click(object sender, EventArgs e)
        {
            if (font == null)
                return;

            FontDialog dialog = new FontDialog();
            dialog.ShowColor = false;
            dialog.ShowEffects = false;
            dialog.AllowVerticalFonts = false;
            dialog.Font = font;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                font = dialog.Font;
                labelSampleText.Font = font;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
