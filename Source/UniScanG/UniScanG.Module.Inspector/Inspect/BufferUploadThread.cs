using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanG.Module.Inspector.Inspect
{
    public class BufferUploadThread : ThreadHandler
    {
        public BufferUploadThread(string name, ThreadStart threadStart) : base(name)
        {
            this.workingThread = new Thread(threadStart);
        }
    }
}
