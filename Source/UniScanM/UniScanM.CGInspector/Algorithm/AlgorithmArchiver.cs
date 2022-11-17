using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using UniScanM.CGInspector.Algorithm.Glass;

namespace UniScanM.CGInspector.Algorithm
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<DynMvp.Vision.Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>(new DynMvp.Vision.Algorithm[]
            {
                new GlassAlgorithm()
            });
        }
        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == GlassAlgorithm.TypeName)
                return new GlassAlgorithm();
            return base.CreateAlgorithm(algorithmType);
        }
    }
}
