using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Fiducial;

namespace UniScanG.Gravure.Vision.Calculator.V2
{
    public class InBarAligner
    {
        FiducialFinder[] fiducialFinders;
        PatternMatchingParam patternMatchingParam;

        public InBarAligner(int speed)
        {
            this.fiducialFinders = new FiducialFinder[0];
            this.patternMatchingParam = new PatternMatchingParam()
            {
                SpeedType = speed
            };
        }

        public void Initialize(AlignInfo[] alignInfos)
        {
            int count = alignInfos.Length;

            this.fiducialFinders = new FiducialFinder[count];
            for (int i = 0; i < count; i++)
            {
                this.fiducialFinders[i] = new FiducialFinder();
                this.fiducialFinders[i].Initialize(alignInfos[i], this.patternMatchingParam);
            }
        }

        public void Dispose()
        {
            if (this.fiducialFinders != null)
                Array.ForEach(this.fiducialFinders, f => f.Dispose());
            this.fiducialFinders = null;
        }

        internal OffsetStruct Align(AlgoImage fullImage, Point adjLocation, float inBarAlignScore, DebugContextG debugContextG)
        {
            bool debugOffsetLog = AdditionalSettings.Instance().DebugOffsetLog;
            List<OffsetStruct> list = new List<OffsetStruct>();
            for (int i = 0; i < this.fiducialFinders.Length; i++)
            {
                OffsetStruct offsetStruct = fiducialFinders[i].Align(fullImage, adjLocation, inBarAlignScore, debugOffsetLog, debugContextG);
                if (offsetStruct.IsGood)
                    list.Add(offsetStruct);
            }

            string subGoodString = string.Join("/", list.Select(f => f.Position));
            OffsetStruct overallOffsetStruct;
            switch (list.Count)
            {
                case 0:
                    overallOffsetStruct = new OffsetStruct(false, subGoodString, PointF.Empty, PointF.Empty, Size.Empty, 0, null);
                    break;
                case 1:
                    overallOffsetStruct = new OffsetStruct(true, subGoodString, list[0].BaseF, list[0].OffsetF, Size.Empty, list[0].Score, list[0].ImageD);
                    break;
                case 2:
                    overallOffsetStruct = new OffsetStruct(true, subGoodString, list[0].BaseF, list[0].OffsetF, new SizeF(DrawingHelper.Subtract(list[1].OffsetF, list[0].OffsetF)), Math.Min(list[0].Score, list[1].Score), list[0].ImageD);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return overallOffsetStruct;
        }
    }

  
}
