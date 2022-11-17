using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniScanS.Data
{
    public class ProductionManagerS : UniEye.Base.Data.ProductionManager
    {

        public ProductionManagerS(string dafaultPath) : base(dafaultPath)
        {

        }

        public void LotChange(DynMvp.Data.Model model, string[] args)
        {
            ProductionS production = (ProductionS)GetProduction(model.ModelDescription, args[1]);
            if (production == null)
            {
                production = (ProductionS)CreateProduction(model, args[1]);

                lock (list)
                    list.Add(production);
            }

            if (args.Count() > 2)
                production.SheetIndex = Convert.ToInt32(args[2]);

            this.curProduction = production;
        }

        public override ProductionBase GetLastProduction(DynMvp.Data.Model model)
        {
            Model.Model modelG = (Model.Model)model;
            return list.ConvertAll(p => (ProductionS)p).FindLast(p => p.Name == modelG.Name && p.Paste == modelG.ModelDescription.Paste && p.Thickness == modelG.ModelDescription.Thickness.ToString());
        }

        public override ProductionBase GetProduction(DynMvp.Data.ModelDescription modelDescription, DateTime dateTime, string lotNo)
        {
            UniScanS.Data.Model.ModelDescriptionS modelDescriptionS = (UniScanS.Data.Model.ModelDescriptionS)modelDescription;
            return list.Find(p => p.Equals(modelDescriptionS) && p.LotNo == lotNo);
        }

        public override ProductionBase CreateProduction(DynMvp.Data.Model model, string lotNo = "")
        {
            UniScanS.Data.Model.Model modelG = (UniScanS.Data.Model.Model)model;

            Production newProduction;

            if (model == null)
                newProduction = new ProductionS("", DateTime.Now, "", "", lotNo);
            else
                newProduction = new ProductionS(modelG.Name, DateTime.Now, modelG.ModelDescription.Thickness.ToString(), modelG.ModelDescription.Paste, lotNo);
            
            newProduction.StartTime = DateTime.Now;

            return newProduction;
        }
    }
}
