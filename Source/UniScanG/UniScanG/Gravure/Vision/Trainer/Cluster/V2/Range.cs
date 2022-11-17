using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UniScanG.Gravure.Vision.Trainer.Cluster.V2
{
    class RangeCollection
    {
        List<Range> rangeList;

        public int Count => rangeList.Count;

        public RangeCollection()
        {
            this.rangeList = new List<Range>();
        }

        public void Add(Range range)
        {
            List<Range> includeList = this.rangeList.FindAll(f => f.IsInclude(range));
            if (includeList.Count > 1)
                includeList = includeList.OrderBy(f => Math.Abs(f.Center - range.Center)).ToList();

            Range include = includeList.FirstOrDefault();
            if (include == null)
                this.rangeList.Add(range);
            else
                include.Add(range);

            this.rangeList.Sort((f, g) => f.Src.CompareTo(g.Src));
        }

        public Range[] GetRanges()
        {
            return this.rangeList.OrderBy(f => f.Src).ToArray();
        }

        public bool IsCross()
        {
            for (int i = 0; i < this.rangeList.Count - 1; i++)
                if (this.rangeList[i].Dst > this.rangeList[i + 1].Src)
                    return true;
            return false;
        }

        public void Split()
        {
            List<float> list = new List<float>();
            list.Add(this.rangeList.First().Src);
            this.rangeList.ForEach(f =>
            {
                list.Add((f.Src + f.Dst) / 2);
            });
            list.Add(this.rangeList.Last().Dst);
            list.Sort();

            List<Range> splitRangeList = new List<Range>();
            list.Aggregate((f, g) =>
            {
                splitRangeList.Add(new Range(f, g));
                return g;
            });

            this.rangeList = splitRangeList;
        }
    }

    class Range
    {
        public float Length => this.dst - this.src;

        public float Center => this.src + (Length - 1) / 2f;

        public float Src => this.src;
        float src;

        public float Dst => this.dst;
        float dst;

        public int Count => this.count;
        int count;

        public Range(float src, float dst)
        {
            this.src = src;
            this.dst = dst;
            this.count = 0;
        }

        public void Add(Range range)
        {
            //this.count++;
            //this.src = Math.Min(this.src, range.src);
            //this.dst = Math.Max(this.dst, range.dst);
            float src = this.src * this.count;
            float dst = this.dst * this.count;
            this.count++;
            this.src = (src + range.src) / count;
            this.dst = (dst + range.dst) / count;
        }

        public bool IsInclude(Range range)
        {
            return MathHelper.IsInRange(range.Center, this.src, this.dst);
        }
    }
}
