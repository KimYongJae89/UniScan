using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Vision.Test.Algorithm
{
    class MilProcess : Process
    {
        public static MIL_ID AllocGreyImage(IntPtr intPtr, Size size, long pitch)
        {
            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            MIL_ID image = MIL.M_NULL;
            if (intPtr == IntPtr.Zero)
                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size.Width, size.Height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            else
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, size.Width, size.Height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)intPtr, MIL.M_NULL);
            return image;
        }

        public static MIL_ID[] AllocGreyImage(IntPtr intPtr, Size size, long pitch, int count)
        {
            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            MIL_ID[] image = new MIL_ID[count];

            for (int i = 0; i < count; i++)
            {
                if (intPtr == IntPtr.Zero)
                    image[i] = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size.Width, size.Height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
                else
                    image[i] = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, size.Width, size.Height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)intPtr, MIL.M_NULL);
            }
            return image;
        }

        public static void FreeGreyImage(MIL_ID milId)
        {
            MIL.MbufFree(milId);
        }

        public static void Save(MIL_ID milId, string path)
        {
            MIL_INT fileFormat = MIL.M_BMP;
            MIL.MbufExport(path, fileFormat, milId);
        }

        public static void FreeGreyImage(MIL_ID[] milId)
        {
            Array.ForEach(milId, f => MIL.MbufFree(f));
        }

        public static Image2D ToImage2D(MIL_ID milId)
        {
            int width = (int)MIL.MbufInquire(milId, MIL.M_SIZE_X);
            int pitch = ((int)(width + 3) / 4) * 4;
            int height = (int)MIL.MbufInquire(milId, MIL.M_SIZE_Y);
            byte[] milBuf = new byte[width * height];
            MIL.MbufGet(milId, milBuf);

            Image2D image2D = new Image2D(width, height, 1, pitch, milBuf);
            return image2D;
        }

        public static MIL_ID GetChild(MIL_ID milId, Rectangle rectangle)
        {
            return MIL.MbufChild2d(milId, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, MIL.M_NULL);
        }

        public static void Clear(MIL_ID milId, double color)
        {
            MIL.MbufClear(milId, color);
        }

        public static IntPtr GetHostAddress(MIL_ID milId)
        {
            return MIL.MbufInquire(milId, MIL.M_HOST_ADDRESS, MIL.M_NULL);
        }

        public static int GetPitch(MIL_ID milId)
        {
            return (int)MIL.MbufInquire(milId, MIL.M_PITCH_BYTE, MIL.M_NULL);
        }

        public override Image2D Process1(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext)
        {
            MIL_ID milSrc = AllocGreyImage(intPtr, size, pitch);
            MIL_ID milDst = AllocGreyImage(IntPtr.Zero, size, pitch);

            debugContext.BeginMeasure("Binarize");
            MIL.MimBinarize(milSrc, milDst, MIL.M_FIXED + MIL.M_GREATER, 50, MIL.M_NULL);
            debugContext.EndMeasure("Binarize");

            Image2D image2D = ToImage2D(milDst);

            FreeGreyImage(milDst);
            FreeGreyImage(milSrc);

            return image2D;
        }


        public override Image2D Process2(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext)
        {
            Tuple<Size, Rectangle[]> roi = GetRoi(size);
            Size roiSize = roi.Item1;
            Rectangle[] rectangles = roi.Item2;

            MIL_ID milSrc = AllocGreyImage(intPtr, size, pitch);
            MIL_ID milDst = AllocGreyImage(IntPtr.Zero, size, pitch);
            MIL_ID[] milBuf = AllocGreyImage(IntPtr.Zero, roiSize, pitch, 5);

            debugContext.BeginMeasure("Subtract");
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = parallel ? -1 : 1 };
            Parallel.For(0, rectangles.Length, parallelOptions, i =>
            {
                Rectangle[] rects = new Rectangle[]
                {
                    i ==0 ? rectangles[i+2] : rectangles[i-1],
                    rectangles[i],
                    i ==rectangles.Length-1 ? rectangles[i-2] : rectangles[i+1]
                };

                MIL_ID[] milIds = Array.ConvertAll(rects, f => MIL.MbufChild2d(milSrc, f.X, f.Y, f.Width, f.Height, MIL.M_NULL));
                MIL_ID milId = MIL.MbufChild2d(milDst, rects[1].X, rects[1].Y, rects[1].Width, rects[1].Height, MIL.M_NULL);

                MIL.MimArith(milIds[1], milIds[0], milBuf[0], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milIds[1], milIds[2], milBuf[1], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], milBuf[1], milBuf[2], MIL.M_MIN);

                MIL.MimArith(milIds[0], milIds[1], milBuf[0], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milIds[2], milIds[1], milBuf[1], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], milBuf[1], milBuf[3], MIL.M_MIN);

                MIL.MimArith(milBuf[2], milBuf[3], milBuf[4], MIL.M_MAX);
                MIL.MimBinarize(milBuf[4], milId, MIL.M_FIXED + MIL.M_GREATER, 50, MIL.M_NULL);

                FreeGreyImage(milId);
                FreeGreyImage(milIds);
            });
            debugContext.EndMeasure("Subtract");

            Image2D image2D = ToImage2D(milDst);
            FreeGreyImage(milBuf);
            FreeGreyImage(milDst);
            FreeGreyImage(milSrc);

            return image2D;
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

            MIL_ID milSrc = AllocGreyImage(intPtr, size, pitch);
            MIL_ID milDst = AllocGreyImage(IntPtr.Zero, size, pitch);
            MIL_ID milDst2 = AllocGreyImage(IntPtr.Zero, size, pitch);

            MIL_ID[] milBuf = AllocGreyImage(IntPtr.Zero, roiSize, pitch, 5);
            MIL_ID milEdge = AllocGreyImage(IntPtr.Zero, roiSize, pitch);
            MIL.MbufClear(milEdge, ofsE);
            MIL_ID milRefT = AllocGreyImage(IntPtr.Zero, size, pitch);
            MIL.MbufClear(milRefT, ofsTh);

            debugContext.BeginMeasure("Simulate");
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = parallel ? -1 : 1 };
            Parallel.For(0, rectangles.Length, parallelOptions, i =>
            {
                Rectangle[] rects = new Rectangle[]
                {
                    i ==0 ? rectangles[i+2] : rectangles[i-1],
                    rectangles[i],
                    i ==rectangles.Length-1 ? rectangles[i-2] : rectangles[i+1]
                };

                MIL_ID[] milIds = Array.ConvertAll(rects, f => MIL.MbufChild2d(milSrc, f.X, f.Y, f.Width, f.Height, MIL.M_NULL));
                MIL_ID milId = MIL.MbufChild2d(milDst, rects[1].X, rects[1].Y, rects[1].Width, rects[1].Height, MIL.M_NULL);
                MIL_ID milId2 = MIL.MbufChild2d(milDst2, rects[1].X, rects[1].Y, rects[1].Width, rects[1].Height, MIL.M_NULL);
                MIL_ID milTh = MIL.MbufChild2d(milRefT, rects[1].X, rects[1].Y, rects[1].Width, rects[1].Height, MIL.M_NULL);

                MIL.MimArith(milIds[1], milIds[0], milBuf[0], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], ofsL, milBuf[0], MIL.M_SUB_CONST + MIL.M_SATURATION);
                MIL.MimArith(milIds[1], milIds[2], milBuf[1], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[1], ofsR, milBuf[1], MIL.M_SUB_CONST + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], milBuf[1], milBuf[2], MIL.M_MIN);

                MIL.MimArith(milIds[0], milIds[1], milBuf[0], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], ofsL, milBuf[0], MIL.M_SUB_CONST + MIL.M_SATURATION);
                MIL.MimArith(milIds[2], milIds[1], milBuf[1], MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimArith(milBuf[1], ofsR, milBuf[1], MIL.M_SUB_CONST + MIL.M_SATURATION);
                MIL.MimArith(milBuf[0], milBuf[1], milBuf[3], MIL.M_MIN);

                MIL.MimArith(milBuf[2], milBuf[3], milBuf[4], MIL.M_MAX);
                MIL.MimArith(milBuf[4], milEdge, milId, MIL.M_SUB + MIL.M_SATURATION);

                MIL.MimArith(milId, milTh, milId2, MIL.M_SUB + MIL.M_SATURATION);
                MIL.MimBinarize(milId2, milId2, MIL.M_FIXED + MIL.M_GREATER, 0, MIL.M_NULL);

                FreeGreyImage(milTh);
                FreeGreyImage(milId2);
                FreeGreyImage(milId);
                FreeGreyImage(milIds);
            });
            debugContext.EndMeasure("Simulate");

            MilProcess.Save(milDst, @"C:\temp\MIL_P3_Dst.bmp");
            MilProcess.Save(milDst2, @"C:\temp\MIL_P3_Dst2.bmp");

            Image2D image2D = ToImage2D(milDst2);
            FreeGreyImage(milRefT);
            FreeGreyImage(milEdge);
            FreeGreyImage(milBuf);
            FreeGreyImage(milDst2);
            FreeGreyImage(milDst);
            FreeGreyImage(milSrc);

            return image2D;
        }
    }
}

