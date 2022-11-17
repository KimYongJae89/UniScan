using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public enum ColorRangeType
    {
        Red, Green, Blue, Hue, Saturation, Luminance
    }

    public partial class ColorRangeSlider : UserControl
    {
        ColorRangeType colorRangeType;
        public ColorRangeType ColorRangeType
        {
            get { return colorRangeType; }
            set { colorRangeType = value; }
        }

        int minValue;
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public ColorRangeSlider()
        {
            InitializeComponent();
        }

        private void ColorRangeSlider_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
