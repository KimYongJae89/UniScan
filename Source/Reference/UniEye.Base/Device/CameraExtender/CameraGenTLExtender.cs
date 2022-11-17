//using DynMvp.Base;
//using DynMvp.Device.Device.FrameGrabber;
//using DynMvp.Devices.FrameGrabber;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UniEye.Base.Settings;

//namespace UniEye.Base.Device.CameraExtender
//{
//    public class CameraGenTLExtender : CameraGenTL, ICameraExtender
//    {
//        protected List<AlgoImage> algoImageList = null;

//        ~CameraGenTLExtender()
//        {
//            Release();
//        }

//        public override void Release()
//        {
//            base.Release();
//            ReleaseBuffer();
//        }

//        public override bool UpdateBuffer(int width, int height, uint scanLines, uint count, bool forceRealloc = false)
//        {
//            bool ok = base.UpdateBuffer(width, height, scanLines, count, forceRealloc);
//            if (ok)
//            {
//                List<ImageD> bufferImageList = this.GetImageBufferList();
//                algoImageList = new List<AlgoImage>();
//                int bufferId = 0;
//                foreach (ImageD bufferImage in bufferImageList)
//                {
//                    CameraBufferTag tag = new CameraBufferTag(bufferId);

//                    AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, bufferImage, ImageType.Gpu);
//                    algoImage.Tag = bufferImage.Tag = tag;

//                    algoImageList.Add(algoImage);
//                    bufferId++;
//                }
//            }
//            return ok;
//        }

//        public override void ReleaseBuffer()
//        {
//            base.ReleaseBuffer();
//            if (algoImageList != null)
//            {
//                foreach (AlgoImage algoImage in algoImageList)
//                    algoImage.Dispose();

//                algoImageList.Clear();
//            }
//        }

//        public void UpdateBufferTag(object tagSource)
//        {
//            Euresys.ScopedBuffer buffer = tagSource as Euresys.ScopedBuffer;

//            IntPtr ptr;
//            UInt64 tsMs;
//            UInt64 frameId;

//            buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out ptr);
//            buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_TIMESTAMP, out tsMs);
//            buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_FRAMEID, out frameId);

//            CameraBufferTag tag = (CameraBufferTag)grabbedImageList[GetBufferIndex(ptr)].Tag;
//            tag.TimeStamp = tsMs;
//            tag.FrameId = frameId;

//        }

//        public AlgoImage GetAlgoImage(IntPtr ptr)
//        {
//            int bufferIndex = GetBufferIndex(ptr);
//            System.Diagnostics.Debug.Assert(bufferIndex >= 0);

//            LogHelper.Debug(LoggerType.Grab, string.Format("CameraGenTLExtender::GetAlgoImage - {0}", bufferIndex));
//            return algoImageList[bufferIndex];
//        }
//    }
//}
