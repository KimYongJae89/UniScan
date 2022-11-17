using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using DynMvp.Vision;

namespace UniEye.Base.Inspect
{
    /// <summary>
    /// 검사 객체 관리 안 함. None-Overlap(검사 중 검사요청 무시)
    /// </summary>
    public class DirectTriggerInspectRunner : InspectRunner
    {
        public IProcesser Processer
        {
            get { return processer; }
            set { processer = value; }
        }

        protected IProcesser processer = null;
        
        public DirectTriggerInspectRunner() : base()
        {
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            //AlgoImage algoImage = null;
            try
            {
                ImageD imageD = imageDevice.GetGrabbedImage(ptr);
                Debug.Assert(imageD != null);

                inspectionResult.GrabImageList.Add(imageD.Clone());
                //algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                //Debug.Assert(algoImage != null);

                inspectRunnerExtender.OnPreInspection();

                this.PreInspect();
                this.processer.Process(imageD, inspectionResult, inspectionOption);
                this.PostInspect();

                inspectRunnerExtender.OnPostInspection();

                //algoImage.Dispose();

                ProductInspected(inspectionResult);
            }
            finally
            {
                //algoImage?.Dispose();
            }
        }
    }
}
