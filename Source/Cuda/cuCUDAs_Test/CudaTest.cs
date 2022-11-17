using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuCUDAs_Test
{
    class CudaTest
    {
        public static void SelectDevice(int deviceId)
        {
            try
            {
                CudaMethods.CUDA_DEVICE_SELECT(deviceId);
                CudaMethods.CUDA_DEVICE_RESET();
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                string err = CudaMethods.CUDA_DEVICE_LAST_ERROR2();
                throw new Exception(err);
            }
        }

        internal static void ResetDevice()
        {
            try
            {
                CudaMethods.CUDA_DEVICE_RESET();
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                string err = CudaMethods.CUDA_DEVICE_LAST_ERROR2();
                throw new Exception(err);
            }
        }

        internal static uint Alloc(Size size)
        {
            try
            {
                return CudaMethods.CUDA_CREATE_IMAGE(size.Width, size.Height, sizeof(byte));
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                string err = CudaMethods.CUDA_DEVICE_LAST_ERROR2();
                throw new Exception(err);
            }
        }

        internal static void Free(uint id)
        {
            try
            {
                CudaMethods.CUDA_FREE_IMAGE(id);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                string err = CudaMethods.CUDA_DEVICE_LAST_ERROR2();
                throw new Exception(err);
            }
        }
    }
}
