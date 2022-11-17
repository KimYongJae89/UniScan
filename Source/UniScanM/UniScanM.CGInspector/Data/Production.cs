using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.CGInspector.Data
{
    public class Production : UniScanM.Data.Production
    {
        public Production(string name, string worker, string lotNo, string paste, string mode, int rewinderSite) : base(name, worker, lotNo, paste, mode, rewinderSite)
        {
        }

        public void AddResult(InspectionResult inspectionResult)
        {

        }

        public Production(XmlElement element) : base(element)
        {

        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);
        }

        public override void Update(DynMvp.InspData.InspectionResult inspectionResult)
        {
            base.Update(inspectionResult);
        }
    }
}
