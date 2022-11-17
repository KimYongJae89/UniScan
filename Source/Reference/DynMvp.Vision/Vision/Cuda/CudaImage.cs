using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;

namespace DynMvp.Vision.Cuda
{
    public static class CudaMethods
    {
        const string DllFileName = "cuCUDAs.dll";

        public enum EdgeSearchDirection
        {
            Horizontal = 0,
            Vertical
        };


        [DllImport(DllFileName)]
        private static extern int CUDA_DEVICE_LAST_ERROR_LEN();

        [DllImport(DllFileName)]
        private static extern void CUDA_DEVICE_LAST_ERROR(StringBuilder sb, int len);

        public static string CUDA_DEVICE_LAST_ERROR2()
        {
            int len = CUDA_DEVICE_LAST_ERROR_LEN();
            StringBuilder sb = new StringBuilder(len);
            CUDA_DEVICE_LAST_ERROR(sb, sb.Capacity);
            return sb.ToString();
        }

        [DllImport(DllFileName)]
        public static extern int CUDA_DEVICE_COUNT();
        [DllImport(DllFileName)]
        public static extern void CUDA_DEVICE_RESET();
        [DllImport(DllFileName)]
        public static extern void CUDA_DEVICE_SELECT(int gpuNo);
        [DllImport(DllFileName)]
        public static extern IntPtr CUDA_DEVICE_NAME(int gpuNo);

        [DllImport(DllFileName)]
        public static extern void CUDA_THREAD_NUM(int threadNum);
        [DllImport(DllFileName)]
        public static extern UInt32 CUDA_CREATE_IMAGE(int width, int height, int depth);
        [DllImport(DllFileName)]
        public static extern void CUDA_SET_IMAGE(UInt32 image, IntPtr pImageBuffer);
        [DllImport(DllFileName)]
        public static extern void CUDA_FREE_IMAGE(UInt32 image);
        [DllImport(DllFileName)]
        public static extern void CUDA_GET_IMAGE(UInt32 image, IntPtr pDstBuffer);
        [DllImport(DllFileName)]
        public static extern void CUDA_SET_ROI(UInt32 image, double x, double y, double width, double height);
        [DllImport(DllFileName)]
        public static extern void CUDA_CLEAR_IMAGE(UInt32 image);
        [DllImport(DllFileName)]
        public static extern void CUDA_RESET_ROI(UInt32 image);
        [DllImport(DllFileName)]
        public static extern void CUDA_CREATE_PROFILE(UInt32 srcImage);

        [DllImport(DllFileName)]
        public static extern void CUDA_CREATE_LABEL_BUFFER(UInt32 srcImage);

        // Edge Detect
        [DllImport(DllFileName)]
        public static extern bool CUDA_EDGE_DETECT(UInt32 srcImage, EdgeSearchDirection dir, int threshold, ref int startPos, ref int endPos);

        // Binarize
        [DllImport(DllFileName)]
        public static extern void CUDA_BINARIZE(UInt32 srcImage, UInt32 dstImage, float lower, float upper, bool inverse = false);
        [DllImport(DllFileName)]
        public static extern void CUDA_BINARIZE_LOWER(UInt32 srcImage, UInt32 dstImage, float lower, bool inverse = false);
        [DllImport(DllFileName)]
        public static extern void CUDA_BINARIZE_UPPER(UInt32 srcImage, UInt32 dstImage, float upper, bool inverse = false);

        // Vertical adaptive
        [DllImport(DllFileName)]
        public static extern void CUDA_ADAPTIVE_BINARIZE(UInt32 srcImage, UInt32 dstImage, float lower, float upper, bool inverse = false);
        [DllImport(DllFileName)]
        public static extern void CUDA_ADAPTIVE_BINARIZE_LOWER(UInt32 srcImage, UInt32 dstImage, float lower, bool inverse = false);
        [DllImport(DllFileName)]
        public static extern void CUDA_ADAPTIVE_BINARIZE_UPPER(UInt32 srcImage, UInt32 dstImage, float upper, bool inverse = false);

        // Blob
        [DllImport(DllFileName)]
        public static extern int CUDA_LABELING(UInt32 binImage);
        [DllImport(DllFileName)]
        public static extern bool CUDA_BLOBING(UInt32 binImage, UInt32 srcImage, int count, 
            UInt32[] areaArray, UInt32[] xMinArray, UInt32[] xMaxArray, UInt32[] yMinArray, UInt32[] yMaxArray,
            byte[] vMinArray, byte[] vMaxArray, float[] vMeanArray);

        // Morphology
        [DllImport(DllFileName)]
        public static extern void CUDA_MORPHOLOGY_ERODE(UInt32 srcImage, UInt32 dstImage, int maskSize);
        [DllImport(DllFileName)]
        public static extern void CUDA_MORPHOLOGY_DILATE(UInt32 srcImage, UInt32 dstImage, int maskSize);
        [DllImport(DllFileName)]
        public static extern void CUDA_MORPHOLOGY_OPEN(UInt32 srcImage, UInt32 dstImage, int maskSize);
        [DllImport(DllFileName)]
        public static extern void CUDA_MORPHOLOGY_CLOSE(UInt32 srcImage, UInt32 dstImage, int maskSize);

        [DllImport(DllFileName)]
        public static extern void CUDA_MATH_XOR(UInt32 srcImage1, UInt32 srcImage2, UInt32 dstImage);
    }

    public abstract class CudaImage : AlgoImage
    {
        public static int CUDA_GPU_NO = 0;

        protected UInt32 imageId;
        protected int width;
        protected int height;
        protected int pitch;
        protected int channel;

        public UInt32 ImageID => imageId;
        public override int Width => width;
        public override int Height => height;
        public override int Pitch => pitch;
        public override bool IsAllocated => this.imageId != 0;

        public abstract void Alloc(int width, int height);
        public abstract void Alloc(int width, int height, IntPtr dataPtr);

        public static IntPtr ToIntPtr(Array buffer)
        {
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            IntPtr temp = pinnedArray.AddrOfPinnedObject();

            pinnedArray.Free();

            return temp;
        }

        public CudaImage()
        {
            imageId = 0;
            width = 0;
            height = 0;
            pitch = 0;
            channel = 1;
        }

        protected static int SizeOfType(Type type)
        {
            var dm = new DynamicMethod("SizeOfType", typeof(int), new Type[] { });
            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, type);
            il.Emit(OpCodes.Ret);
            return (int)dm.Invoke(null, null);
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(imageId > 0);
            Debug.Assert(bytes.Length == width * height);

            byte[] imageBuffer = new byte[width * height];
            GCHandle pinnedArray = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);

            IntPtr dataPtr = pinnedArray.AddrOfPinnedObject();
            CudaMethods.CUDA_GET_IMAGE(imageId, dataPtr);

            pinnedArray.Free();
        }

        protected abstract Array GetData();

        public Array CloneData()
        {
            if (imageId == 0)
                return null;

            return GetData();
        }

        public abstract void Put(IntPtr ptr);

        public override void PutByte(byte[] data)
        {
            Put(CudaImage.ToIntPtr(data));
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            Put(ptr);
        }

        protected override void Disposing()
        {
            CudaMethods.CUDA_FREE_IMAGE(imageId);

            imageId = 0;
            width = 0;
            height = 0;
            pitch = 0;
            channel = 1;
        }
        
        public override ImageD ToImageD()
        {
            Image2D image = new Image2D();
            byte[] buffer = GetByte();

            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            image.Initialize(width, height, 1, pitch, gcHandle.AddrOfPinnedObject());

            gcHandle.Free();

            return image;
        }
        
        public override AlgoImage Clip(Rectangle rectangle)
        {
            return null;
        }

        public override void Save(string fileName)
        {

        }

        public override void Clear(byte initVal = 0)
        {
            CudaMethods.CUDA_CLEAR_IMAGE(imageId);
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {

        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {

        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            dstImage = null;
        }

        public override IntPtr GetImagePtr()
        {
            return IntPtr.Zero;
        }
        
    }

    public class CudaDepthImage<T> : CudaImage
    {
        T[] imageData;

        public override int BitPerPixel => throw new NotImplementedException();

        public CudaDepthImage() : base()
        {
            imageData = null;
        }

        protected override Array GetData()
        {
            if (imageData == null)
                imageData = new T[width * height];

            GCHandle pinnedArray = GCHandle.Alloc(imageData, GCHandleType.Pinned);

            IntPtr dataPtr = pinnedArray.AddrOfPinnedObject();
            CudaMethods.CUDA_GET_IMAGE(imageId, dataPtr);

            pinnedArray.Free();

            return imageData;
        }

        public override void Alloc(int width, int height)
        {
            this.width = width;
            this.height = height;

            pitch = width;
            channel = 1;

            imageId = CudaMethods.CUDA_CREATE_IMAGE(width, height, SizeOfType(typeof(T)));
        }

        public override void Alloc(int width, int height, IntPtr dataPtr)
        {
            Alloc(width, height);
            imageData = new T[width * height];
            IntPtr intPtr = CudaImage.ToIntPtr(imageData);
            Put(intPtr);
        }
        
        public override void Put(IntPtr intPtr)
        {
            CudaMethods.CUDA_SET_IMAGE(imageId, intPtr);
        }

        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    CudaDepthImage<T> newImage = new CudaDepthImage<T>();
        //    newImage.Alloc(width, height);
        //    newImage.PutByte(CloneByte());

        //    return newImage;
        //}

        public override AlgoImage Clone()
        {
            CudaDepthImage<T> newImage = new CudaDepthImage<T>();
            newImage.Alloc(width, height);
            newImage.PutByte(GetByte());

            return newImage;
        }
    }
}
