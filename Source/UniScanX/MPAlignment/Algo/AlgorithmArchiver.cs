using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;

namespace UniScanX.MPAlignment.Algo
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<DynMvp.Vision.Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>(new DynMvp.Vision.Algorithm[]
            {
                new MPAlgorithm()
            });
        }
        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == MPAlgorithm.TypeName)
                return new MPAlgorithm();

            return base.CreateAlgorithm(algorithmType);
        }
    }
}
