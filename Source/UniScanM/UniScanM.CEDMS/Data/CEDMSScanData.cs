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
using UniScanM.CEDMS.Settings;

namespace UniScanM.CEDMS.Data
{
    public class CEDMSScanData
    {
        int nowDistance;
        public int NowDistance { get => nowDistance; set => nowDistance = value; }

        float y;
        public float Y { get => y; set => y = value; }

        float yRaw;
        public float YRaw { get => yRaw; set => yRaw = value; }

        float yOffset;
        public float YOffset { get => yOffset; set => yOffset = value; }

        public CEDMSScanData(int distance, float y, float yRaw, float yOffset)
        {
            this.nowDistance = distance;
            this.y = y;
            this.yRaw = yRaw;
            this.yOffset = yOffset;
        }

        public CEDMSScanData Clone2()
        {
            return new CEDMSScanData(nowDistance, y, YRaw, YOffset);
        }
    }
}
