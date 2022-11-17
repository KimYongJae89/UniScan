using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.RCI.Calculator
{
    internal struct WhatToNameThis
    {
        public int TargetStart { get; set; }
        public int ModelStart { get; set; }
        public int Length { get; set; }
    }

    public static class RangeSplitter
    {
        private static WhatToNameThis[] SplitRange(int target, int model)
        {
            List<WhatToNameThis> list = new List<WhatToNameThis>();
            WhatToNameThis item = new WhatToNameThis();
            float step = model * 1f / target;
            int offset = 0;
            for (int from = 0; from < target; from++)
            {
                int to = (int)Math.Round(from * step);

                if (from == (to - offset))
                {
                    item.Length++;
                }
                else
                {
                    list.Add(item);
                    offset = to - from;

                    item.TargetStart = from;
                    item.ModelStart = from + offset;
                    item.Length = 1;
                }
            }
            list.Add(item);

            return list.ToArray();
        }

        internal static WhatToNameThis[] SplitRange2(int target, int model)
        {
            if (target == model)
                return new WhatToNameThis[] { new WhatToNameThis() { TargetStart = 0, ModelStart = 0, Length = target } };

            List<WhatToNameThis> list = new List<WhatToNameThis>();
            WhatToNameThis item = new WhatToNameThis();
            float groups = Math.Abs(target - model) + 1;
            float step = target / groups;
            bool inflate = (model > target);

            int oldOffset = 0;
            for (int from = 0; from < target; from++)
            {
                int offset = (int)(from / step);

                if (!inflate)
                    offset *= -1;
                int to = from + offset;

                if (oldOffset == offset)
                {
                    item.Length++;
                }
                else
                {
                    list.Add(item);
                    oldOffset = offset;

                    item.TargetStart = from;
                    item.ModelStart = to;
                    item.Length = 1;
                }
            }
            list.Add(item);

            return list.ToArray();
        }

        internal static WhatToNameThis[] SplitRange3(int target, int model)
        {
            // 항상 2개로 나누기...??
            if (false)
            {
                int groupCnt = 2;
                float stepTarget = target * 1f / groupCnt;
                int[] lengths = new int[groupCnt];
                for (int i = 0; i < groupCnt; i++)
                    lengths[i] = (int)(Math.Round((i + 1) * stepTarget) - Math.Round(i * stepTarget));
                Debug.Assert(lengths.Sum() == target);

                int overlap = (model - target) / (groupCnt - 1);
                WhatToNameThis[] splits = new WhatToNameThis[groupCnt];

                for (int i = 0; i < groupCnt; i++)
                {
                    int startTarget = (int)Math.Round(i * stepTarget);
                    int startModel = startTarget + overlap * i;

                    splits[i] = new WhatToNameThis()
                    {
                        Length = lengths[i],
                        TargetStart = startTarget,
                        ModelStart = startModel
                    };

                    if (i > 0)
                        Debug.Assert(splits[i - 1].TargetStart + splits[i - 1].Length == splits[i].TargetStart);
                }
                return splits;
            }
            else
            {
                int groupCnt = Math.Abs(target - model) + 1;
                float stepTarget = target * 1f / groupCnt;

                int[] lengths = new int[groupCnt];
                for (int i = 0; i < groupCnt; i++)
                    lengths[i] = (int)(Math.Round((i + 1) * stepTarget) - Math.Round(i * stepTarget));
                Debug.Assert(lengths.Sum() == target);

                int overlap = (model - target) / (groupCnt - 1);
                WhatToNameThis[] splits = new WhatToNameThis[groupCnt];

                for (int i = 0; i < groupCnt; i++)
                {
                    int startTarget = (int)Math.Round(i * stepTarget);
                    int startModel = startTarget + overlap * i;

                    splits[i] = new WhatToNameThis()
                    {
                        Length = lengths[i],
                        TargetStart = startTarget,
                        ModelStart = startModel
                    };

                    if (i > 0)
                        Debug.Assert(splits[i - 1].TargetStart + splits[i - 1].Length == splits[i].TargetStart);
                }

                return splits;
            }
        }

    }
}
