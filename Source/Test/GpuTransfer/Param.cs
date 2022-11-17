using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpuTransfer
{
    class Param
    {
        public bool UseIntPtr { get; set; }
        public int Iterate { get; set; } = 1;
        public int Width { get; set; } = 17824;
        public int Heigth { get; set; } = 35000;
        public int SizeByte => Width * Heigth;
        public double SizeGByte => Width * Heigth / 1024.0 / 1024.0 / 1024.0;
        public Size Size => new Size(Width, Heigth);

        public Param()
        {
            this.UseIntPtr = Properties.Settings.Default.UseIntPtr;
            this.Width = Properties.Settings.Default.Width;
            this.Heigth = Properties.Settings.Default.Height;
            this.Iterate = Properties.Settings.Default.Iterate;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.UseIntPtr = this.UseIntPtr;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.Height = this.Heigth;
            Properties.Settings.Default.Iterate = this.Iterate;
            Properties.Settings.Default.Save();
        }
    }
}
