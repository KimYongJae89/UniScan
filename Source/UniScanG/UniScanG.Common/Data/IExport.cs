using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanG.Common.Data
{
    public interface IExportable
    {
        void Export(string path, CancellationToken cancellationToken);
        bool Import(string path);
    }
}
