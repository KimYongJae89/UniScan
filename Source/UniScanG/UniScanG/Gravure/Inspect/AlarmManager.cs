using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.MachineIF;

namespace UniScanG.Gravure.Inspect
{
    public class AlarmManager
    {
        public float Lower10 { get; private set; }
        public float Middle80 { get; private set; }
        public float Upper10 { get; private set; }

        public float Diff10 => Math.Max(this.Upper10 - this.Middle80, this.Middle80 - this.Lower10);

        public float Mean { get; private set; }
        public float StdDev { get; private set; }
        public float Min { get; private set; }
        public float Max { get; private set; }
        public float Range => Max - Min;

        // 패턴길이
        protected List<float> patternLengthList = new List<float>();

        public AlarmManager() { }

        public virtual void ClearSignal() { }

        public virtual void UpdateSignal() { }

        public virtual void ClearData()
        {
            lock (this.patternLengthList)
            {
                this.patternLengthList.Clear();
                UpdatePatternLength();
            }

            //UpdateSignal();
        }

        public virtual void AddResult(InspectionResult inspectionResult)
        {
            //LogHelper.Debug(LoggerType.Inspection, string.Format("AlarmManager::AddResult"));

            if (!int.TryParse(inspectionResult.InspectionNo, out int patternNo))
                return;

            bool result = inspectionResult.IsGood();

            if (!inspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
                return;

            AlgorithmResultG algorithmResultG = inspectionResult.AlgorithmResultLDic[Detector.TypeName] as AlgorithmResultG;

            float height = algorithmResultG.SheetSize.Height;
            lock (this.patternLengthList)
            {
                this.patternLengthList.Add(height);
                this.patternLengthList.Sort();
                UpdatePatternLength();
            }
        }

        protected void UpdatePatternLength()
        {
            int sp1 = this.patternLengthList.Count / 10;
            int sp2 = this.patternLengthList.Count - sp1;
            if (sp1 > 0 && sp2 > sp1)
            {
                this.Lower10 = this.patternLengthList.GetRange(0, sp1).Average();
                this.Middle80 = this.patternLengthList.GetRange(sp1, sp2 - sp1).Average();
                this.Upper10 = this.patternLengthList.GetRange(sp2, sp1).Average();

                MathHelper.StdDev(this.patternLengthList.ToArray(), out double mean, out double stdDev);

                this.Mean = (float)mean;
                this.StdDev = (float)stdDev;
                this.Min = this.patternLengthList.Min();
                this.Max = this.patternLengthList.Max();
    }
            else
            {
                this.Upper10 = this.Middle80 = this.Lower10 = 0;
                this.Mean = this.StdDev = this.Min = this.Max = 0;
            }
        }
    }    
}
