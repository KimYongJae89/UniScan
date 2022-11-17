using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public abstract class CameraBufferTag
    {
        public DateTime DateTime { get; protected set; }

        public UInt64 FrameId { get; protected set; }

        /// <summary>
        /// LineScan 카메라의 경우 버퍼 크기와 프레임 크기가 다를 수 있다 -> 그랩하다 중단한 경우.
        /// </summary>
        public Size FrameSize { get; protected set; }

        public CameraBufferTag(UInt64 frameId, Size frameSize)
        {
            this.DateTime = DateTime.Now;
            this.FrameId = frameId;
            this.FrameSize = frameSize;
        }

        public override string ToString()
        {
            return $"Frame {this.FrameId}";
        }
    }
}
