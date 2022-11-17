using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public class ProductionManager : UniEye.Base.Data.ProductionManager
    {
        public ProductionManager(string defaultPath) : base(defaultPath) { }

        public new UniScanM.Data.Production CurProduction
        {
            get { return (UniScanM.Data.Production)curProduction; }
        }

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.ModelDescription modelDescription, string lotNo)
        {
            ModelDescription modelDescriptionM = modelDescription as ModelDescription;
            return list.Find(p => p.Name == modelDescriptionM.Name && ((Production)p).Paste == modelDescriptionM.Paste && p.LotNo == lotNo);
        }

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.ModelDescription modelDescription, DateTime date, string lotNo)
        {
            ModelDescription modelDescriptionM = modelDescription as ModelDescription;
            return list.Find(p => p.Name == modelDescriptionM.Name && ((Production)p).Paste == modelDescriptionM.Paste && p.StartTime.Date == date.Date && p.LotNo == lotNo);
        }

        public Production GetProduction(UniScanM.Data.InspectionResult inspectionResult)
        {
            return (Production)list.LastOrDefault(f =>
            (f is Production) &&
            (f.Name == inspectionResult.ModelName) &&
            (f.LotNo == inspectionResult.LotNo) &&
            (((Production)f).Worker == inspectionResult.Wroker));
        }

        public override DynMvp.Data.ProductionBase GetLastProduction(DynMvp.Data.Model model)
        {
            Model modelM = (Model)model;
            return list.FindLast(p => p.Name == model.Name && ((Production)p).Paste == modelM.ModelDescription.Paste);
        }


        public int LotExistCount(string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            int productionCount = list.Count(f =>
            {
                Production p = (Production)f;
                bool same = (p.Name == name && ((Production)p).Worker == worker && ((Production)p).Paste == paste);
                if (same == false)
                    return false;

                string[] tokens = p.LotNo.Split('_');
                return tokens[0] == lotNo;
            });
            return productionCount;

            Production production = (Production)list.FindLast(prod =>
            {
                Production p = (Production)prod;
                bool same = (p.Name == name && ((Production)p).Worker == worker && ((Production)p).Paste == paste && ((Production)p).Mode == mode);
                if (same == false)
                    return false;

                string[] tokens = p.LotNo.Split('_');
                return tokens[0] == lotNo;
            });

            if (production == null)
                return 0;

            string[] tokens2 = production.LotNo.Split('_');
            int idx = 0;
            if (tokens2.Count() > 1 && int.TryParse(tokens2[1], out int lotIndex))
                idx = (tokens2.Length == 1) ? 1 : Convert.ToInt32(tokens2[1]) + 1; //0base-name... lotNo > lotNo_1 > lotNo_2 > lotNo_3 > ....
            //if (production.RewinderSite != rewinderSite) idx++; //its ambiguous, so ignore because how operate field
            return idx;
        }


        public Production LotChange(string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            Production production = GetProduction(name, worker, lotNo, paste, rewinderSite, mode);
            if (production == null)
            {
                production = CreateProduction(name, worker, lotNo, paste, mode, rewinderSite);
                lock (list)
                    list.Add(production);
                this.Save(PathSettings.Instance().Result);
            }

            curProduction = production;
            return production;
        }

        private Production GetProduction(string name, string worker, string lotNo, string paste, int rewinderSite, string mode)
        {
            return (Production)list.Find(p => p.Name == name && ((Production)p).Worker == worker && ((Production)p).Paste == paste && p.LotNo == lotNo && ((Production)p).Mode == mode && ((Production)p).RewinderSite == rewinderSite);
        }

        public virtual Production CreateProduction(string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            Production newProduction = new Production(name, worker, lotNo, paste, mode, rewinderSite);
            return newProduction;
        }

        public override DynMvp.Data.ProductionBase CreateProduction(XmlElement productionElement)
        {
            Production production = new Production(productionElement);
            return production;
        }
    }
}