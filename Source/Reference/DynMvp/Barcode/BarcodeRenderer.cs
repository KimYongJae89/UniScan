using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Barcode
{
    public enum BarcodeRendererType
    {
        Code128
    }

    public abstract class BarcodeRenderer
    {
        public static BarcodeRenderer GetBarcodeRenderer(BarcodeRendererType barcodeRendererType)
        {
            return new Code128Renderer();
        }

        public abstract Image GetBarcodeImage(string inputData);
    }
}
