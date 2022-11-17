using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Common.Data
{
    public interface IExportable
    {
        void Export(string name, string path);
    }
}
