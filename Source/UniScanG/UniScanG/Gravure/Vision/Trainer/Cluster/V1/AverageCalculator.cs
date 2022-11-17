using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.Trainer.Cluster
{
    class AverageCalculator
    {
        public float TotalSum => this.totalSum;
        float totalSum = 0;

        public int TotalCount => this.totalCount;
        int totalCount = 0;

        public float MaxValue => maxValue;
        float maxValue = 0;

        public float MinValue => minValue;
        float minValue = 0;

        public float Average => this.totalSum / this.totalCount;

        public AverageCalculator() { }

        public void Clear()
        {
            this.totalSum = 0;
            this.totalCount = 0;
        }

        public void Add(float value)
        {
            this.minValue = totalCount == 0 ? value : Math.Min(this.minValue, value);
            this.maxValue = totalCount == 0 ? value : Math.Max(this.maxValue, value);
            totalSum += value;
            totalCount++;
        }
    }
}
