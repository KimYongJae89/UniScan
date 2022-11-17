using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Matrox.MatroxImagingLibrary;
using DynMvp.Base;
using System.Diagnostics;
using DynMvp.Authentication;

namespace DynMvp.Devices
{
    public class MatroxHelper
    {
        static int initCount = 0;

        public static MIL_ID ApplicationId { get => applicationId; }
        static MIL_ID applicationId = MIL.M_NULL;

        public static MIL_ID HostSystemId { get => hostSystemId; }
        static MIL_ID hostSystemId = MIL.M_NULL;

        public static MIL_ID GpuSystemId { get => gpuSystemId; }
        static MIL_ID gpuSystemId = MIL.M_NULL;

        public static MIL_INT Version { get => version; }
        static MIL_INT version = MIL.M_NULL;

        public static bool UseNonPagedMem { get => useNonPagedMem; }
        static bool useNonPagedMem = false;

        public static bool UseGpuSystem { get => useGpuSystem; }
        static bool useGpuSystem = false;

        public static bool IsAppInitialized => applicationId != MIL.M_NULL;

        public static bool InitApplication(bool useNonPagedMem, bool useGpuSystem)
        {
            MatroxHelper.useNonPagedMem = useNonPagedMem;
            MatroxHelper.useGpuSystem = useGpuSystem;
            return InitApplication();
        }

        public static bool InitApplication()
        {
            try
            {
                LogHelper.Debug(LoggerType.StartUp, "Initialize MIL Applications");

                if (applicationId == MIL.M_NULL)
                {
                    MIL.MappAlloc(MIL.M_DEFAULT, ref applicationId);
                    MIL.MsysAlloc(MIL.M_DEFAULT, MIL.M_SYSTEM_HOST, MIL.M_DEV0, MIL.M_DEFAULT, ref hostSystemId);
                    if (useGpuSystem)
                        MIL.MsysAlloc(MIL.M_DEFAULT, MIL.M_SYSTEM_GPU, MIL.M_DEV1, MIL.M_DEFAULT, ref gpuSystemId);

                    version = MIL.MappInquire(MIL.M_VERSION);

#if DEBUG
                    MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);
#else
                    MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
#endif
                }
                initCount++;

                return applicationId != MIL.M_NULL;
            }
            catch (Exception ex)
            {
                throw new AlarmException(ErrorSectionSystem.Instance.Initialize.Initialize, ErrorLevel.Fatal, ex.Message, null, null);
            }
        }

        public static void ShowMessageOnError() => MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);
        public static void ThrowExceptionOnError() => MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_THROW_EXCEPTION);

        public static void FreeApplication()
        {
            if (initCount == 0)
                return;

            initCount--;

            if (initCount == 0)
            {
                LogHelper.Debug(LoggerType.StartUp, "Free MIL Applications");

                MilObjectManager.Instance.RemoveAllObject();

                if (gpuSystemId != MIL.M_NULL)
                {
                    MIL.MsysFree(gpuSystemId);
                    gpuSystemId = MIL.M_NULL;
                }

                if (hostSystemId != MIL.M_NULL)
                {
                    MIL.MsysFree(hostSystemId);
                    hostSystemId = MIL.M_NULL;
                }

                if (applicationId != MIL.M_NULL)
                {
                    MIL.MappFree(applicationId);
                    applicationId = MIL.M_NULL;
                }
            }
        }

        public static bool LicenseExist(string licenseString)
        {
            bool init = InitApplication();
            if (init == false)
                return false;

            bool result = false;

            long licenseValue = 0;
            MIL.MappInquire(MIL.M_LICENSE_MODULES, ref licenseValue);

            string[] licenseTypes = licenseString.Split(';');

            foreach (string licenseType in licenseTypes)
            {
                if (licenseType == "PAT")
                {
                    result = (licenseValue & MIL.M_LICENSE_PAT) != 0;
                }
                else if (licenseType == "EDGE")
                {
                    result = (licenseValue & MIL.M_LICENSE_EDGE) != 0;
                }
                else if (licenseType == "IM")
                {
                    result = (licenseValue & MIL.M_LICENSE_IM) != 0;
                }
                else if (licenseType == "MEAS")
                {
                    result = (licenseValue & MIL.M_LICENSE_MEAS) != 0;
                }
                else if (licenseType == "BLOB")
                {
                    result = (licenseValue & MIL.M_LICENSE_BLOB) != 0;
                }
                else if (licenseType == "CAL")
                {
                    result = (licenseValue & MIL.M_LICENSE_CAL) != 0;
                }
                else if (licenseType == "CODE")
                {
                    result = (licenseValue & MIL.M_LICENSE_CODE) != 0;
                }
                else if (licenseType == "OCR")
                {
                    result = (licenseValue & MIL.M_LICENSE_OCR) != 0;
                }
            }

            FreeApplication();
            return result;
        }

        public static bool LicenseExist()
        {
            bool init = InitApplication();
            if (init == false)
                return false;

            long licenseValue = 0;
            MIL.MappInquire(MIL.M_LICENSE_MODULES, ref licenseValue);
            FreeApplication();

            return (licenseValue > 0);
        }
    }

    public class MatroxBuffer : MilObject
    {
        public StackTrace StackTrace { get; private set; }

        MIL_ID milId;

        int dimension;
        public int Dimension
        {
            get { return dimension; }
        }

        public IntPtr Ptr
        {
            get
            {
                if (milId == MIL.M_NULL)
                    return IntPtr.Zero;
                else
                    return (IntPtr)MIL.MbufInquire(milId, MIL.M_HOST_ADDRESS);
            }
        }

        public MatroxBuffer(long size)
        {
            dimension = 1;
            long attribute = MIL.M_ARRAY;
            if (MatroxHelper.UseNonPagedMem)
                attribute += MIL.M_NON_PAGED;

            milId = MIL.MbufAlloc1d(MIL.M_DEFAULT_HOST, size, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            MIL.MbufClear(milId, 0);
            MilObjectManager.Instance.AddObject(this);
        }

        public MatroxBuffer(long width, long height)
        {
            dimension = 2;
            long attribute = MIL.M_ARRAY;
            if (MatroxHelper.UseNonPagedMem)
                attribute += MIL.M_NON_PAGED;

            milId = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            MIL.MbufClear(milId, 0);
            MilObjectManager.Instance.AddObject(this);
        }

        public void Dispose()
        {
            MilObjectManager.Instance.ReleaseObject(this);
        }

        public void Free()
        {
            if (milId != MIL.M_NULL)
                MIL.MbufFree(milId);
            milId = MIL.M_NULL;
        }

        public void AddTrace()
        {
#if DEBUG
            this.StackTrace = new StackTrace();
#endif
        }
    }
}
