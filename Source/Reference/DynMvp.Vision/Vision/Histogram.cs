using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    class Histogram
    {
        int numStep;
        float minValue;
        float maxValue;

        int[] histogramData;
        public int[] HistogramData
        {
            get { return histogramData; }
            set { histogramData = value; }
        }

        public void Create(int numStep, float minValue, float maxValue)
        {
            this.numStep = numStep;
            this.minValue = minValue;
            this.maxValue = maxValue;

            histogramData = new int[numStep];
        }

        public void Create(float[] datum, int numStep)
        {
            this.numStep = numStep;
            histogramData = new int[numStep];

            minValue = datum.Min();
            maxValue = datum.Max();

            float stepDistance = (maxValue - minValue)/numStep;

            foreach (float data in datum)
            {
                int index = GetIndex(data);
                histogramData[index]++;
            }
        }

        public void Create(float[] datum, int numStep, float stepDistance)
        {
            this.numStep = numStep;
            histogramData = new int[numStep];

            float averageValue = datum.Average();
            
            minValue = averageValue - (numStep / 2) * stepDistance;
            maxValue = averageValue + (numStep / 2) * stepDistance;

            foreach (float data in datum)
            {
                int index = GetIndex(data);
                histogramData[index]++;
            }
        }

        public void Add(float[] datum, bool ignoreOutboundValue)
        {
            foreach (float data in datum)
            {
                if (ignoreOutboundValue == true)
                {
                    if (data > maxValue || data < minValue)
                        continue;
                }

                int index = GetIndex(data);
                histogramData[index]++;
            }
        }

        private int GetIndex(float value)
        {
            float stepDistance = (maxValue - minValue) / numStep;

            int index = (int)((value - minValue) / stepDistance);
            if (index >= numStep || index < 0)
            {
                if (index >= numStep)
                    index = numStep - 1;
                if (index < 0)
                    index = 0;
            }

            return index;
        }

        public int GetHistogramCount(float value)
        {
            int index = GetIndex(value);
            return histogramData[index];
        }

        public int GetMaxCount()
        {
            return histogramData.Max();
        }
    }
}
