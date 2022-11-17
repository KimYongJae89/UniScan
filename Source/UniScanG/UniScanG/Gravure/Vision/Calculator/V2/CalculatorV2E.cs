using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.Calculator.V2
{
    [StructLayout(LayoutKind.Sequential)]
    struct ImageData
    {
        IntPtr ptr;

        [MarshalAs(UnmanagedType.I4)]
        int pitch;

        public ImageData(IntPtr ptr, int pitch)
        {
            this.ptr = ptr;
            this.pitch = pitch;
        }
    }

    public static class CalculatorV2E
    {
        //#if DEBUG
        //        const string dllName = "UniScanG.VisionD.dll";
        //#else
        //        const string dllName = "UniScanG.Vision.dll";
        //#endif
        const string dllName = "UniScanG.Vision.dll";

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void GvCalcV2E(ImageData src, ImageData refL, ImageData refR, ImageData refE, ImageData diff, ImageData thMap, ImageData bin, [MarshalAs(UnmanagedType.U1)]byte ofsL, byte ofsR, [MarshalAs(UnmanagedType.I4)]int width, int heigth);

        public static void InspectLines(AlgoImage src, AlgoImage refL, AlgoImage refR, AlgoImage refE, AlgoImage diff, AlgoImage thMap, AlgoImage bin, byte ofsL, byte ofsR, DebugContextG debugContextG)
        {
            ImageData srcData = new ImageData(src.Ptr, src.PitchLib);
            ImageData refLData = new ImageData(refL.Ptr, refL.PitchLib);
            ImageData refRData = new ImageData(refR.Ptr, refR.PitchLib);
            ImageData refEData = new ImageData(IntPtr.Zero, 0);

            ImageData diffData = new ImageData(diff.Ptr, diff.PitchLib);
            ImageData thMapData = new ImageData(thMap.Ptr, thMap.PitchLib);
            ImageData binData = new ImageData(bin.Ptr, bin.PitchLib);

            if (refE != null)
                refEData = new ImageData(refE.Ptr, refE.PitchLib);

            int width = src.Width;
            int height  = src.Height;

            diff.Clear();
            GvCalcV2E(srcData, refLData, refRData, refEData, diffData, thMapData, binData, ofsL, ofsR, width, height);
        }
    }
}
