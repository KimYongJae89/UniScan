using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Module.Monitor.MachineIF
{
    public class MachineIfData: UniScanG.MachineIF.MachineIfData
    {
        // Controller -> Printer
        public bool SET_VISION_GRAVURE_MONITORING_READY;
        public bool SET_VISION_GRAVURE_MONITORING_RUN;

        public bool SET_VISION_GRAVURE_MONITORING_RESULT;
        public float SET_VISION_GRAVURE_MONITORING_MARGIN_W;
        public float SET_VISION_GRAVURE_MONITORING_MARGIN_L;
        public float SET_VISION_GRAVURE_MONITORING_BLOT_W;
        public float SET_VISION_GRAVURE_MONITORING_BLOT_L;
        public float SET_VISION_GRAVURE_MONITORING_DEFECT_W;
        public float SET_VISION_GRAVURE_MONITORING_DEFECT_L;

        public override void Reset()
        {
            base.Reset();

            // Controller -> Printer
            this.SET_VISION_GRAVURE_MONITORING_READY = false;
            this.SET_VISION_GRAVURE_MONITORING_RUN = false;

            this.SET_VISION_GRAVURE_MONITORING_RESULT = false;
            this.SET_VISION_GRAVURE_MONITORING_MARGIN_W = 0;
            this.SET_VISION_GRAVURE_MONITORING_MARGIN_L = 0;
            this.SET_VISION_GRAVURE_MONITORING_BLOT_W = 0;
            this.SET_VISION_GRAVURE_MONITORING_BLOT_L = 0;
            this.SET_VISION_GRAVURE_MONITORING_DEFECT_W = 0;
            this.SET_VISION_GRAVURE_MONITORING_DEFECT_L = 0;
        }
    }
}
