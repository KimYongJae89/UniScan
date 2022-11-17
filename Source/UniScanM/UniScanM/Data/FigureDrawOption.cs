using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Data
{
    public struct DrawSet
    {
        Pen pen;
        Brush brush;

        public Pen Pen
        {
            get { return pen; }
        }

        public Brush Brush
        {
            get { return brush; }
        }

        public DrawSet(Pen pen, Brush brush)
        {
            this.pen = pen;
            this.brush = brush;
        }
    }

    public struct FontSet
    {
        Font font;
        Color color;

        public Font Font
        {
            get { return font; }
        }

        public Color Color
        {
            get { return color; }
        }

        public FontSet(Font font, Color color)
        {
            this.font = font;
            this.color = color;
        }
    }

    public struct FigureDrawOptionProperty
    {
        bool showFigure;
        DrawSet good;
        DrawSet ng;
        DrawSet invalid;

        bool showText;
        FontSet fontSet;

        public bool ShowFigure
        {
            get { return showFigure; }
            set { showFigure = value; }
        }

        public DrawSet Good
        {
            get { return good; }
            set { good = value; }
        }

        public DrawSet Ng
        {
            get { return ng; }
            set { ng = value; }
        }

        public DrawSet Invalid
        {
            get { return invalid; }
            set { invalid = value; }
        }

        public bool ShowText
        {
            get { return showText; }
            set { showText = value; }
        }

        public FontSet FontSet
        {
            get { return fontSet; }
            set { fontSet = value; }
        }
    }

    public class FigureDrawOption: DynMvp.UI.FigureDrawOption
    {
        bool patternConnection = false;

        FigureDrawOptionProperty teachResult;
        FigureDrawOptionProperty processResult;

        public bool PatternConnection
        {
            get { return patternConnection; }
            set { patternConnection = value; }
        }

        public FigureDrawOptionProperty TeachResult
        {
            get { return teachResult; }
            set { teachResult = value; }
        }

        public FigureDrawOptionProperty ProcessResult
        {
            get { return processResult; }
            set { processResult = value; }
        }
    }
}
