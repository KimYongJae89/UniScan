using DynMvp.Base;
using DynMvp.Devices.Comm;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using UniEye.Base.Settings;
using UniScanM.RVMS.Settings;

namespace UniScanM.RVMS.Data
{
    public class ScanData
    {
        DateTime x;
        public DateTime X { get => x; set => x = value; }

        float y;
        public float Y { get => y; set => y = value; }

        float yRaw;
        public float YRaw { get => yRaw; set => yRaw = value; }

        float yOffset;
        public float YOffset { get => yOffset; set => yOffset = value; }
        
        int nowDistance;
        public int NowDistance { get => nowDistance; set => nowDistance = value; }

        DateTime inspectStartedTime;
        public DateTime InspectStartedTime { get => inspectStartedTime; set => inspectStartedTime = value; }


        public ScanData(DateTime x, float y, float yRaw, float yOffset)
        {
            this.x = x;
            this.y = y;
            this.yRaw = yRaw;
            this.yOffset = yOffset;
        }

        public ScanData(DateTime x, float y, float yRaw, float yOffset, DateTime inspectStartedTime)
        {
            this.x = x;
            this.y = y;
            this.yRaw = yRaw;
            this.yOffset = yOffset;
            this.inspectStartedTime = inspectStartedTime;
        }

        public ScanData(int distance, float y, float yRaw, float yOffset)
        {
            this.nowDistance = distance;
            this.y = y;
            this.yRaw = yRaw;
            this.yOffset = yOffset;
        }

        public ScanData Clone()
        {
            return new ScanData(x, y, YRaw, YOffset);
        }

        public ScanData Clone2()
        {
            return new ScanData(nowDistance, y, YRaw, YOffset);
        }
    }
}
