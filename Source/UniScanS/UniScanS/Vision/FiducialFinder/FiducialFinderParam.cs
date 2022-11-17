using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanS.Inspect;
using UniScanS.Vision;

namespace UniScanS.Vision.FiducialFinder
{
    public class FiducialFinderParam : DynMvp.Vision.AlgorithmParam
    {
        protected int minScore;
        public int MinScore
        {
            get { return minScore; }
            set { minScore = value; }
        }

        protected int searchRangeHalfWidth;
        public int SearchRangeHalfWidth
        {
            get { return searchRangeHalfWidth; }
            set { searchRangeHalfWidth = value; }
        }

        protected int searchRangeHalfHeight;
        public int SearchRangeHalfHeight
        {
            get { return searchRangeHalfHeight; }
            set { searchRangeHalfHeight = value; }
        }

        public FiducialFinderParam()
        {
            searchRangeHalfWidth = 2000;
            searchRangeHalfHeight = 500;
            minScore = 60;
        }

        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
