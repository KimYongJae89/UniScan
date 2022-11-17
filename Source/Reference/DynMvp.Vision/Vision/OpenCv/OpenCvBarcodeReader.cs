using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision.OpenCv
{
    class OpenCvBarcodeReader : BarcodeReader
    {
        public override Algorithm Clone()
        {
            OpenCvBarcodeReader barcodeReader = new OpenCvBarcodeReader();
            barcodeReader.CopyFrom(this);

            return barcodeReader;
        }

        public override AlgorithmResult Read(AlgoImage algoImage, RectangleF clipRect, DebugContext debugContext)
        {
            AlgorithmResult barcodeReaderResult = CreateAlgorithmResult();

            return barcodeReaderResult;
        }
    }
}
