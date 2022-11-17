using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Common.Util
{
    public static class Colors
    {
        public static Color Good
        {
            get { return Color.LightGreen; }
        }

        public static Color NG
        {
            get { return Color.Red; }
        }

        public static Color Trained
        {
            get { return Color.LightGreen; }
        }

        public static Color Untrained
        {
            get { return Color.Red; }
        }

        public static Color NotExist
        {
            get { return Color.FromArgb(150, 150, 150); }
        }

        public static Color Alarm
        {
            get { return Color.Crimson; }
        }

        public static Color Wait
        {
            get { return Color.FromArgb(140, 203, 255); ; }
        }

        public static Color Idle
        {
            get { return Color.LightGray; }
        }

        public static Color Run
        {
            get { return Color.LightGreen; }
        }

        public static Color Connected
        {
            get { return Color.LightGreen; }
        }

        public static Color Disconnected
        {
            get { return Color.Red; }
        }

        public static Color Teach
        {
            get { return Color.Yellow; }
        }
    }
}
