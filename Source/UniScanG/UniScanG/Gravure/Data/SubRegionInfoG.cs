using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Data
{
    class SubRegionInfoG
    {
        public Rectangle[,] PatRegionList
        {
            get { return this.patRegionList; }
        }

        public List<Rectangle> InspRegionList
        {
            get { return this.inspRegionList; }
        }

        public List<Point> SkipRegions
        {
            get { return this.skipPoints; }
        }

        Rectangle[,] patRegionList;
        List<Rectangle> inspRegionList;
        List<Point> skipPoints;
    }
}
