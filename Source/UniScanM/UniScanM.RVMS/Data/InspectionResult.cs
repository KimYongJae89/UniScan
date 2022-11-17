using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.RVMS.Data
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

        bool zeroingComplate;
        public bool ZeroingComplate
        {
            get { return zeroingComplate; }
            set { zeroingComplate = value; }
        }

        float gearZeroingValue;
        public float GearZeroingValue
        {
            get { return gearZeroingValue; }
            set { gearZeroingValue = value; }
        }

        float manZeroingValue;
        public float ManZeroingValue
        {
            get { return manZeroingValue; }
            set { manZeroingValue = value; }
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

        ScanData beforePattern;
        public ScanData BeforePattern
        {
            get { return beforePattern; }
            set { beforePattern = value; }
        }

        ScanData affterPattern;
        public ScanData AffterPattern
        {
            get { return affterPattern; }
            set { affterPattern = value; }
        }

        ScanData manSide;
        public ScanData ManSide
        {
            get { return manSide; }
            set { manSide = value; }
        }

        ScanData gearSide;
        public ScanData GearSide
        {
            get { return gearSide; }
            set { gearSide = value; }
        }

        DateTime firstTime;
        public DateTime FirstTime { get => firstTime; set => firstTime = value; }

        int elapsedTime;
        public int ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        
    }
}
