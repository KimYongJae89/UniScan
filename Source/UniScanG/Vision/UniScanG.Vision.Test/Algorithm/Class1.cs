using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Gravure.Vision.Calculator;

namespace UniScanG.Vision.Test.Algorithm
{
    public enum AlgoLibrary { MIL, SIMD }
    class DebugContext
    {
        DateTime beginMeasure = DateTime.MinValue;

        public string DebugPath { get => this.debugPath; set => this.debugPath = value; }
        string debugPath = @"C:\temp";

        public List<Tuple<DateTime, string, double>> TupleList => tupleList;
        List<Tuple<DateTime, string, double>> tupleList = new List<Tuple<DateTime, string, double>>();

        private void AddMessage(DateTime dateTime, string message, double timeMs = 0)
        {
            tupleList.Add(new Tuple<DateTime, string, double>(DateTime.Now, message, timeMs));
        }

        public void AddMessage(string message)
        {
            AddMessage(DateTime.Now, message);
        }

        public void BeginMeasure(string message)
        {
            DateTime now = DateTime.Now;
            beginMeasure = now;
            AddMessage(now, string.Format("BeginMeasure: {0}", message));
        }

        public void EndMeasure(string message)
        {
            DateTime now = DateTime.Now;
            double spendTimeMs = (now - this.beginMeasure).TotalMilliseconds;
            AddMessage(now, string.Format("EndMeasure: {0}", message), spendTimeMs);
        }
    }

    class Class1
    {
        public CalculatorParam CalculatorParam => this.calculatorParam;
        CalculatorParam calculatorParam = null;

        public Image2D Image2D => this.image2D;
        Image2D image2D = null;

        public bool Load(string dataFilePath, string imageFilePath)
        {
            AlgorithmPool algorithmPool = new AlgorithmPool();
            algorithmPool.Initialize(new UniScanG.Gravure.AlgorithmArchiver());
            algorithmPool.Load(dataFilePath);
            this.calculatorParam = algorithmPool.GetAlgorithm(CalculatorBase.TypeName).Param as CalculatorParam;

            this.image2D = new Image2D(imageFilePath);
            this.image2D.ConvertFromData();
            return true;
        }

        public bool IsLoaded => this.image2D != null && this.calculatorParam != null;

        public Image2D DoProcess(int v, AlgoLibrary algoLibrary,bool parallel, DebugContext debugContext)
        {
            Image2D image2D = null;
            // whole Image subtract
            using (Process process = Process.CreateProcess(algoLibrary))
            {
                image2D = process.DoProcess(v, this, parallel, debugContext);
            }
            return image2D;
        }
    }
}
