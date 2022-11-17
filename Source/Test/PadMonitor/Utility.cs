using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Base;
using System.Windows;
using DynMvp.Data.UI;
using DynMvp.Vision.OpenCv;
using System.Diagnostics;

namespace PadMonitor
{
    class Utility
    {
    }

    public class ZoomDataInfo
    {
        public ZoomDataInfo(AlgoImage image, BlobRect blobrect)
        {
            this.image = image;
            this.blobrect = blobrect;
        }
        public AlgoImage image = null;
        public BlobRect blobrect = null;
    }
}
