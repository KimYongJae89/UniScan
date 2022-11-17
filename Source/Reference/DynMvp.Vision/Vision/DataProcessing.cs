using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public static class DataProcessing
    {
        public static float[] Differential(float[] datum)
        {
            int size = datum.Count() - 1;
            float[] derivative = new float[datum.Count() - 1];

            for (int i = 0; i < size; i++)
            {
                derivative[i] = datum[i + 1] - datum[i];
            }

            return derivative;
        }

        public static float StdDev(float[] datum)
        {
            float squareSum = 0;
            float sum = 0;

            foreach (float data in datum)
            {
                squareSum += data * data;
                sum += data;
            }

            float average = sum / datum.Count();
            return (float)Math.Sqrt(squareSum / datum.Count() - average * average);
        }
    }
}
