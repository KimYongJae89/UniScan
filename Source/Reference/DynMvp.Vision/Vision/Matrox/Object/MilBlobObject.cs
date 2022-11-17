using DynMvp.Devices;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision.Matrox.Object
{
    public class MilBlobObject : IDisposable, MilObject
    {
        public System.Diagnostics.StackTrace StackTrace { get; private set; }

        protected MIL_ID id = MIL.M_NULL;
        public MIL_ID Id
        {
            get { return id; }
        }

        StackTrace stackTrace = null;

        public MilBlobObject()
        {
            Build();
        }

        protected virtual void Build()
        {
            MIL_ID dummy = MIL.M_NULL;
            this.id = MIL.MblobAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref dummy);
            MilObjectManager.Instance.AddObject(this);
        }

        public void Dispose()
        {
            MilObjectManager.Instance.ReleaseObject(this);
        }

        public void Free()
        {
            if (id != MIL.M_NULL)
            {
                MIL.MblobFree(id);
                id = MIL.M_NULL;
            }
        }

        public void AddTrace()
        {
#if DEBUG
            this.stackTrace = new StackTrace();
#endif
        }
    }

    public class MilBlobResult : MilBlobObject, MilObject
    {
        protected override void Build()
        {
            MIL_ID dummy = MIL.M_NULL;
            this.id = MIL.MblobAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref dummy);
            MilObjectManager.Instance.AddObject(this);
        }
    }

    public class MilBlobRectList : BlobRectList
    {
        MilBlobResult blobResult;
        public MilBlobResult BlobResult
        {
            get { return blobResult; }
        }

        public MilBlobRectList()
        {
            this.blobResult = new MilBlobResult();
        }

        public MilBlobRectList(MilBlobResult blobResult)
        {
            this.blobResult = blobResult;
        }

        ~MilBlobRectList()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (blobResult != null)
                blobResult.Dispose();

            blobResult = null;
        }
    }
}
