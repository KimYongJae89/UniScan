using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Data;

namespace UniScanM.Data
{
    public class DataRemover : DynMvp.Data.DataRemover
    {
        public DataRemover(int resultStoringDays, int minimumFreeSpaceP, ProductionManagerBase productionManager, DirectoryInfo logDataFolder) : base(resultStoringDays, minimumFreeSpaceP, productionManager, logDataFolder)
        {
        }

        protected override void RemoveAllDataExtend(string targetPath)
        {
            string reportPath = targetPath.Replace(@"Result\", @"Report\");
            if (Directory.Exists(reportPath))
            {
                //this.RemoveAllData(reportPath);
                FileHelper.ClearFolder(reportPath);
                this.RemoveUpperFolder(reportPath);
            }
        }
    }
}
