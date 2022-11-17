using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Common.Settings;

namespace UniScanG.Inspect
{
    public delegate void ProcessBufferReturnedDelegate();

    public class ProcessBufferManager : IDisposable
    {
        int nextId = -1;

        ImageDevice imageDevice;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
            set { imageDevice = value; }
        }
        
        List<ProcessBufferSet> imageBufferSetList = new List<ProcessBufferSet>();

        public int Count { get { return imageBufferSetList.Count; } }
        public int UsingCount { get { return imageBufferSetList.FindAll(f=>f.IsUsing).Count; } }
        public float UsingCountP { get { return (UsingCount * 100f / Count); } }

        public ProcessBufferReturnedDelegate ProcessBufferReturnedDelegate;

        public void AddProcessBufferSet(ProcessBufferSet imageBufferSet)
        {
            this.nextId = 0;
            imageBufferSetList.Add(imageBufferSet);
        }

        public void Dispose()
        {
            LogHelper.Debug(LoggerType.Inspection, "ProcessBufferManager::Dispose");
            foreach (ProcessBufferSet imageBufferSet in imageBufferSetList)
                imageBufferSet.Dispose();

            imageBufferSetList.Clear();
        }

        public ProcessBufferSet Request()
        {
            ProcessBufferSet imageBufferSet = null;
            int curId = -1;
            lock (imageBufferSetList)
            {
                // rollingId 이후에서 탐색
                curId = imageBufferSetList.FindIndex(nextId, f => f.IsUsing == false);
                if (curId < 0)
                {
                    // rollingId 이전에서 탐색
                    curId = imageBufferSetList.FindIndex(0, nextId, f => f.IsUsing == false);
                }

                if (curId >= 0)
                {
                    // UsingCountP 가 특정 %를 넘으면 null 리턴..
                    imageBufferSet = imageBufferSetList[curId];
                    imageBufferSet.IsUsing = true;
                    LogHelper.Debug(LoggerType.Operation, string.Format("ProcessBufferManager::Request - OK. Index {0}, {1:F2}%", curId, UsingCountP));
                    this.nextId = (curId + 1) % imageBufferSetList.Count;
                }
            }

            if (imageBufferSet == null)
            {
                LogHelper.Debug(LoggerType.Operation, string.Format("ProcessBufferManager::Request - Fail. {0:F2}%", UsingCountP));
                return null;
            }

            imageBufferSet.Clear();

            return imageBufferSet;
        }

        public void Return(ProcessBufferSet imageBufferSet)
        {
            if (imageBufferSet == null)
                return;

            if (!imageBufferSet.IsUsing)
                return;

            lock (imageBufferSetList)
            {
                imageBufferSet.IsUsing = false;
                int index = imageBufferSetList.IndexOf(imageBufferSet);
                LogHelper.Debug(LoggerType.Operation, string.Format("ProcessBufferManager::Return - OK. Index {0}, {1:F2}%", index, UsingCountP));
            }
        }

        public void ReturnAll()
        {
            LogHelper.Debug(LoggerType.Operation, "ProcessBufferManager::ReturnAll");
            lock (imageBufferSetList)
                imageBufferSetList.ForEach(f => Return(f));
        }

        public bool WaitDisposable(int timeoutMs = -1)
        {
            TimeOutTimer tot = new TimeOutTimer();
            if (timeoutMs >= 0)
                tot.Start(timeoutMs);
            while (this.imageBufferSetList.Exists(f => f.IsUsing))
            {
                Thread.Sleep(100);
                if (tot.TimeOut)
                    return false;
            }
            return true;
        }
    }

    public abstract class ProcessBufferSet : IDisposable
    {
        private bool isUsing;
        public bool IsUsing
        {
            get { return isUsing; }
            set { isUsing = value; }
        }

        protected string algorithmTypeName = "";
        protected int width = 0;
        protected int height = 0;

        public virtual bool IsDone { get => true; }

        public bool IsBuilded { get => this.bufferList.TrueForAll(f => f.IsAllocated); }

        protected List<AlgoImage> bufferList = new List<AlgoImage>();
        
        public ProcessBufferSet()
        {

        }

        public ProcessBufferSet(string algorithmTypeName, int width, int height)
        {
            this.algorithmTypeName = algorithmTypeName;
            this.width = width;
            this.height = height;

            bufferList = new List<AlgoImage>();
            //BuildBuffers(algorithmTypeName, width, height);
        }

        public virtual void Dispose()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Dispose();
            bufferList.Clear();
        }

        public virtual void Clear()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Clear();
        }

        public virtual void WaitDone() { }

        public abstract void BuildBuffers(bool halfScale);
    }
}
