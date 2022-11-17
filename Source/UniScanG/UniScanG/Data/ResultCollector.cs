using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Data
{
    public interface ResultCollector
    {
        AlgorithmResultG Collect(string path);
        AlgorithmResultG CreateSheetResult(int index, int subCount, string path);
    }
}
