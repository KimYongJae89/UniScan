using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.CEDMS.Data
{
    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        List<float> curValueList = new List<float>();
        public List<float> CurValueList
        {
            get { return curValueList; }
        }

        bool resetZeroing;
        public bool ResetZeroing
        {
            get { return resetZeroing; }
            set { resetZeroing = value; }
        }

        bool zeroingComplete;
        public bool ZeroingComplete
        {
            get { return zeroingComplete; }
            set { zeroingComplete = value; }
        }

        float inFeedZeroingValue;
        public float InFeedZeroingValue
        {
            get { return inFeedZeroingValue; }
            set { inFeedZeroingValue = value; }
        }

        float outFeedZeroingValue;
        public float OutFeedZeroingValue
        {
            get { return outFeedZeroingValue; }
            set { outFeedZeroingValue = value; }
        }

        int zeroingNum;
        public int ZeroingNum
        {
            get { return zeroingNum; }
            set { zeroingNum = value; }
        }

        bool zeroingStable;
        public bool ZeroingStable
        {
            get { return zeroingStable; }
            set { zeroingStable = value; }
        }

        double zeroingVariance;
        public double ZeroingVariance
        {
            get { return zeroingVariance; }
            set { zeroingVariance = value; }
        }

        CEDMSScanData inFeed;
        public CEDMSScanData InFeed
        {
            get { return inFeed; }
            set { inFeed = value; }
        }

        CEDMSScanData outFeed;
        public CEDMSScanData OutFeed
        {
            get { return outFeed; }
            set { outFeed = value; }
        }

        private double lineSpeed;
        public double LineSpeed { get => lineSpeed; set => lineSpeed = value; }

        DateTime firstTime;
        public DateTime FirstTime { get => firstTime; set => firstTime = value; }

        int elapsedTime;
        public int ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    }
}
