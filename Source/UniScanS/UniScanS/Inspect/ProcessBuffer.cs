using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Common.Settings;

namespace UniScanS.Inspect
{
    public delegate void ProcessBufferReturnedDelegate();

    public class ProcessBufferManager : IDisposable
    {
        ImageDevice imageDevice;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
            set { imageDevice = value; }
        }

        List<ProcessBufferSet> imageBufferSetList = new List<ProcessBufferSet>();

        public ProcessBufferReturnedDelegate ProcessBufferReturnedDelegate;

        public void AddProcessBufferSet(ImageDevice imageDevice, ProcessBufferSet imageBufferSet)
        {
            imageBufferSetList.Add(imageBufferSet);
        }

        public void Dispose()
        {
            foreach (ProcessBufferSet imageBufferSet in imageBufferSetList)
                imageBufferSet.Dispose();

            imageBufferSetList.Clear();
        }

        public ProcessBufferSet Request(ImageDevice imageDevice)
        {
            ProcessBufferSet imageBufferSet = null;

            lock (imageBufferSetList)
            {
                imageBufferSet = imageBufferSetList.Find(f => f.IsUsing ==  false);

                if (imageBufferSet != null)
                {
                    imageBufferSet.IsUsing = true;
                    Debug.WriteLine("Buffer request Success");
                    imageBufferSet.Clear();
                }
                else
                {
                    Debug.WriteLine("Buffer request fail");
                }
            }

            return imageBufferSet;
        }

        public void Return(ProcessBufferSet imageBufferSet)
        {
            lock (imageBufferSetList)
            {
                if (imageBufferSet == null)
                {
                    Debug.WriteLine("Buffer return fail");
                    imageBufferSetList.ForEach(b => b.IsUsing = false);
                }
                else
                {
                    ProcessBufferSet processBufferSet = imageBufferSetList.Find(b => b == imageBufferSet);
                    if (processBufferSet != null)
                        processBufferSet.IsUsing = false;
                }
            }
        }
        
    }

    public class ProcessBufferSet : IDisposable
    {
        private bool isUsing;
        public bool IsUsing
        {
            get { return isUsing; }
            set { isUsing = value; }
        }
        
        protected List<AlgoImage> bufferList = new List<AlgoImage>();
        
        //Alloc
        AlgoImage fiducial;
        public AlgoImage Fiducial
        {
            get { return fiducial; }
            set { fiducial = value; }
        }

        //Preview
        AlgoImage interestP;
        public AlgoImage InterestP
        {
            get { return interestP; }
            set { interestP = value; }
        }

        public ProcessBufferSet(string algorithmTypeName, int width, int height)
        {
            bufferList = new List<AlgoImage>();
            BuildBuffers(algorithmTypeName, width, height);
        }

        public void Dispose()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Dispose();
        }

        public virtual void Clear()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Clear();
        }

        protected virtual void BuildBuffers(string algorithmTypeName, int width, int height)
        {
            fiducial = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height);
            bufferList.Add(fiducial);
            float resizeReatio = SystemTypeSettings.Instance().ResizeRatio;
            int previewWidth = (int)Math.Truncate(width * resizeReatio);
            int previewHeight = (int)Math.Truncate(height * resizeReatio);

            interestP = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, previewWidth, previewHeight);
            bufferList.Add(interestP);
        }
    }
}
