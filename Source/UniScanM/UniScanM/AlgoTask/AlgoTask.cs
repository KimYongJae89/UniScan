using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;

namespace UniScanM.AlgoTask
{
    public class GrabBufferPool
    {
        int nextSectionIndex;
        ConcurrentQueue<AlgoImage> queue;
        public GrabBufferPool(int idx)
        {
            this.nextSectionIndex = idx;
            this.queue = new ConcurrentQueue<AlgoImage>();
        }
    }
}
