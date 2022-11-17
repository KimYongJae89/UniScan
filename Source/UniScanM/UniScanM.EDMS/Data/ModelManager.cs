using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Data;
using DynMvp.UI;

namespace UniScanM.EDMS.Data
{
    public class ModelManager : UniScanM.Data.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new UniScanM.EDMS.Data.Model();
        } 
    }
}
