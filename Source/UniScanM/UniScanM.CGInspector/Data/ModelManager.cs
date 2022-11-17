using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Data;
using DynMvp.UI;

namespace UniScanM.CGInspector.Data
{
    public class ModelManager : UniScanM.Data.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new UniScanM.CGInspector.Data.Model();
        }
    }
}
