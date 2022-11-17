using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using UniScanG.Module.Monitor.Vision;

namespace UniScanG.Module.Monitor.Data
{
    public class AlgorithmArchiver: DynMvp.Vision.AlgorithmArchiver
    {
        public override List<Algorithm> GetDefaultAlgorithm()
        {
            return new Algorithm[] { CreateAlgorithm(MyAlgorithm.TypeName)}.ToList();
        }

        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == MyAlgorithm.TypeName)
                return new MyAlgorithm();

            return base.CreateAlgorithm(algorithmType);
        }
    }
}
