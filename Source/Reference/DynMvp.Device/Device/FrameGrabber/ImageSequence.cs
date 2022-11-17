using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.FrameGrabber
{
    public delegate void ScanDoneDelegate();
    public delegate void ImageScannedDelegate(ImageD image);

    public abstract class ImageSequence
    {
        public ScanDoneDelegate ScanDone;
        public ImageScannedDelegate ImageScanned;

        protected int imageIndex = 0;
        protected List<ImageD> imageList = new List<ImageD>();
        public List<ImageD> ImageList
        {
            get { return imageList; }
        }

        public abstract void Initialize(Camera camera);
        public abstract void Scan(int numImage);
        public abstract bool IsScanDone();
        public abstract void Stop();
    }
}
