using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Vision.Test.Algorithm
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

    class SIMDProcess : Process
    {
        const string dllName = "UniScanG.Vision.dll";

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void Subtract(IntPtr pSrc1, IntPtr pSrc2, IntPtr pDst, int iWidth, int iHeigth, int iPitch);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void SubMinSubMinMaxBin(IntPtr src, IntPtr refL, IntPtr refR, IntPtr dst, [MarshalAs(UnmanagedType.I4)]int width, int heigth, int pitch);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void Binarize(IntPtr pSrc, IntPtr pDst, int iWidth, int iHeigth, int iPitch);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void GvCalcV2E(ImageData src, ImageData refL, ImageData refR, ImageData refE, ImageData dst, ImageData thMap, ImageData bin, [MarshalAs(UnmanagedType.U1)]byte ofsL, byte ofsR, [MarshalAs(UnmanagedType.I4)] int iWidth, int iHeigth);


        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void CalibrateLine(ImageData src, ImageData dst, ImageData mul100, [MarshalAs(UnmanagedType.I4)]int width, int heigth);

        public override Image2D Process1(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext)
        {
            Image2D dstImage = new Image2D(size.Width, size.Height, 1, pitch);
            dstImage.ConvertFromData();

            debugContext.BeginMeasure("Binarize");
            Binarize(intPtr, dstImage.DataPtr, size.Width, size.Height, pitch);
            debugContext.EndMeasure("Binarize");

            return dstImage;
        }

        public override Image2D Process2(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext)
        {
            Tuple<Size, Rectangle[]> roi = GetRoi(size);
            Size roiSize = roi.Item1;
            Rectangle[] rectangles = roi.Item2;

            IntPtr srcPtr, refLPtr, refRPtr, dstPtr;
            Image2D dstImage = new Image2D(size.Width, size.Height, 1, pitch);
            dstImage.ConvertFromData();

            debugContext.BeginMeasure("Subtract");
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = parallel ? -1 : 1 };
            Parallel.For(0, rectangles.Length, parallelOptions, i =>
            {
                dstPtr = dstImage.DataPtr + (rectangles[i].X);
                srcPtr = intPtr + (rectangles[i].X);
                refLPtr = intPtr + (rectangles[i == 0 ? (i + 2) : (i - 1)].X);
                refRPtr = intPtr + (rectangles[i == rectangles.Length - 1 ? (i - 2) : (i + 1)].X);

                SubMinSubMinMaxBin(srcPtr, refLPtr, refRPtr, dstPtr, roiSize.Width, roiSize.Height, pitch);
            });

            debugContext.EndMeasure("Subtract");
            return dstImage;
        }

        public override Image2D Process3(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext)
        {
            byte ofsL = 5;
            byte ofsR = 15;
            byte ofsE = 10;
            byte ofsTh = 30;

            Tuple<Size, Rectangle[]> roi = GetRoi(size);
            Size roiSize = roi.Item1;
            Rectangle[] rectangles = roi.Item2;

            int milSrc = (int)MilProcess.AllocGreyImage(intPtr, size, pitch);
            int milDst = (int)MilProcess.AllocGreyImage(IntPtr.Zero, size, pitch);
            int milEdge = (int)MilProcess.AllocGreyImage(IntPtr.Zero, roiSize, pitch);
            MilProcess.Clear(milEdge, ofsE);
            int milTh = (int)MilProcess.AllocGreyImage(IntPtr.Zero, size, pitch);
            MilProcess.Clear(milTh, ofsTh);
            int milDst2 = (int)MilProcess.AllocGreyImage(IntPtr.Zero, size, pitch);

            debugContext.BeginMeasure("Simulate");
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = parallel ? -1 : 1 };
            Parallel.For(0, rectangles.Length, parallelOptions, i =>
            {
                Rectangle[] rects = new Rectangle[]
                {
                    rectangles[i],
                    i ==0 ? rectangles[i+2] : rectangles[i-1],
                    i ==rectangles.Length-1 ? rectangles[i-2] : rectangles[i+1]
                };

                int src = (int)MilProcess.GetChild(milSrc, rects[0]);
                int refL = (int)MilProcess.GetChild(milSrc, rects[1]);
                int refR = (int)MilProcess.GetChild(milSrc, rects[2]);
                int dst = (int)MilProcess.GetChild(milDst, rects[0]);
                int refT = (int)MilProcess.GetChild(milTh, rects[0]);
                int dst2 = (int)MilProcess.GetChild(milDst2, rects[0]);

                int pitchSrc = MilProcess.GetPitch(src);
                int pitchRefL = MilProcess.GetPitch(refL);
                int pitchRefR = MilProcess.GetPitch(refR);
                int pitchRefE = MilProcess.GetPitch(milEdge);
                int pitchDst = MilProcess.GetPitch(dst);
                int pitchRefT = MilProcess.GetPitch(refT);
                int pitchDst2 = MilProcess.GetPitch(dst2);

                IntPtr srcPtr = MilProcess.GetHostAddress(src);
                IntPtr refLPtr = MilProcess.GetHostAddress(refL);
                IntPtr refRPtr = MilProcess.GetHostAddress(refR);
                IntPtr refEPtr = MilProcess.GetHostAddress(milEdge);
                IntPtr dstPtr = MilProcess.GetHostAddress(dst);
                IntPtr refTPtr = MilProcess.GetHostAddress(refT);
                IntPtr dst2Ptr = MilProcess.GetHostAddress(dst2);

                ImageData idSrc = new ImageData(srcPtr, pitchSrc);
                ImageData idRefL = new ImageData(refLPtr, pitchRefL);
                ImageData idRefR = new ImageData(refRPtr, pitchRefR);
                ImageData idRefE = new ImageData(refEPtr, pitchRefE);
                ImageData idDst = new ImageData(dstPtr, pitchDst);
                ImageData idRefT = new ImageData(refTPtr, pitchRefT);
                ImageData idDst2 = new ImageData(dst2Ptr, pitchDst2);

                // Pitch가 이미지마다 다르다.....
                GvCalcV2E(idSrc, idRefL, idRefR, idRefE, idDst, idRefT, idDst2, ofsL, ofsR, roiSize.Width, roiSize.Height);

                MilProcess.FreeGreyImage(src);
                MilProcess.FreeGreyImage(refL);
                MilProcess.FreeGreyImage(refR);
                MilProcess.FreeGreyImage(dst);
                MilProcess.FreeGreyImage(refT);
                MilProcess.FreeGreyImage(dst2);
            });
            debugContext.EndMeasure("Simulate");

            MilProcess.Save(milDst, @"C:\temp\SIMD_P3_Dst.bmp");
            MilProcess.Save(milDst2, @"C:\temp\SIMD_P3_Dst2.bmp");

            Image2D result = MilProcess.ToImage2D(milDst2);
            MilProcess.FreeGreyImage(milTh);
            MilProcess.FreeGreyImage(milEdge);
            MilProcess.FreeGreyImage(milDst2);
            MilProcess.FreeGreyImage(milDst);
            MilProcess.FreeGreyImage(milSrc);
            return result;

            //Image2D dstImage = new Image2D(size.Width, size.Height, 1, pitch);
            //dstImage.ConvertFromData();

            //Image2D edgeImage = new Image2D(roiSize.Width, roiSize.Height, 1, pitch);
            //edgeImage.ConvertFromData();
            //edgeImage.ImageData.Clear(ofsE);

            //IntPtr srcPtr, dstPtr, refLPtr, refRPtr, refEPtr;
            //debugContext.BeginMeasure("Simulate");
            //for (int i = 0; i < rectangles.Length; i++)
            //{
            //    srcPtr = intPtr + (rectangles[i].X);
            //    dstPtr = dstImage.ImageData.DataPtr + (rectangles[i].X);

            //    refLPtr = intPtr + (rectangles[i == 0 ? (i + 2) : (i - 1)].X);
            //    refRPtr = intPtr + (rectangles[i == rectangles.Length - 1 ? (i - 2) : (i + 1)].X);
            //    refEPtr = edgeImage.ImageData.DataPtr + (rectangles[i].X);

            //    GvCalcV2E(srcPtr, dstPtr, refLPtr, refRPtr, refEPtr, ofsL, ofsR, roiSize.Width, roiSize.Height, pitch);
            //}
            //debugContext.EndMeasure("Simulate");

            //edgeImage.Dispose();

            //return dstImage;
        }

        public static void Multiply()
        {
            int col = 1784;
            int row = 10;
            byte[] srcData = new byte[col * row];
            byte[] dstData = new byte[col * row];
            byte[] mulData = new byte[col];

            for (int i = 0; i < col; i++)
            {
                mulData[i] = (byte)(200);
            }

            for (int r = 0; r < row; r++)
                for (int c = 0; c < col; c++)
                    srcData[r * col + c] = (byte)(r * col + c + 0);

            GCHandle gcHandleSrc = GCHandle.Alloc(srcData, GCHandleType.Pinned);
            GCHandle gcHandleDst = GCHandle.Alloc(dstData, GCHandleType.Pinned);
            GCHandle gcHandleMul = GCHandle.Alloc(mulData, GCHandleType.Pinned);

            IntPtr srcPtr = gcHandleSrc.AddrOfPinnedObject();
            IntPtr dstPtr = gcHandleDst.AddrOfPinnedObject();
            IntPtr mulPtr = gcHandleMul.AddrOfPinnedObject();

            ImageData src = new ImageData(srcPtr, col * sizeof(byte));
            ImageData dst = new ImageData(dstPtr, col * sizeof(byte));
            ImageData mul = new ImageData(mulPtr, col * sizeof(byte));

            CalibrateLine(src, dst, mul,  col, row);

            gcHandleMul.Free();
            gcHandleDst.Free();
            gcHandleSrc.Free();
        }

        public static void SIMDTEST()
        {
            MatroxHelper.InitApplication();

            Image2D srcImage = new Image2D(@"D:\temp\src.bmp");
            Image2D prevImage = new Image2D(@"D:\temp\refL.bmp");
            Image2D nextImage = new Image2D(@"D:\temp\refR.bmp");
            Image2D edgeImage = new Image2D(@"D:\temp\edge.bmp");

            AlgoImage src = ImageBuilder.Build(ImagingLibrary.MatroxMIL, srcImage, ImageType.Grey);
            AlgoImage prev = ImageBuilder.Build(ImagingLibrary.MatroxMIL, prevImage, ImageType.Grey);
            AlgoImage next = ImageBuilder.Build(ImagingLibrary.MatroxMIL, nextImage, ImageType.Grey);
            AlgoImage edge = ImageBuilder.Build(ImagingLibrary.MatroxMIL, edgeImage, ImageType.Grey);

            AlgoImage diff = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, srcImage.Size);
            AlgoImage bin = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, srcImage.Size);
            AlgoImage thMap = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, srcImage.Size);

            UniScanG.Gravure.Vision.Calculator.V2.CalculatorV2E.InspectLines(src, prev, next, edge, diff, thMap, bin, 0, 0, null);
        }
    }
}
