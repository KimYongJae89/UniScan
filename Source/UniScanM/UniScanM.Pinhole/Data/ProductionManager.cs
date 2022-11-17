using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
using UniScanM.Data;

namespace UniScanM.Pinhole.Data
{
    public class ProductionManager : UniScanM.Data.ProductionManager
    {
        public ProductionManager(string defaultPath) : base(defaultPath)
        {
        }
        
        public override DynMvp.Data.ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new Production(productionElement);
        }

        public override UniScanM.Data.Production CreateProduction(string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            Production newProduction = new Production(name, worker, lotNo, paste, mode, rewinderSite);
            return newProduction;
        }
    }
}
