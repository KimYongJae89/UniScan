using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;
using UniScanG.Data.Model;

namespace UniScanG.Gravure.Data
{
    public class ProductionManagerG : UniScanG.Data.ProductionManager
    {
        public ProductionManagerG(string defaultPath) : base(defaultPath)
        {
            this.OnLotChanged += ProductionManagerG_OnLotChanged;
        }

        public override Production CreateProduction(UniScanG.Data.Model.ModelDescription modelDescription, string lotNo, RewinderZone rewinderZone, int subIndex = 0, float lineSpeed = 0)
        {
            return new ProductionG(modelDescription.Name, DateTime.Now, lotNo, rewinderZone, modelDescription.Thickness, modelDescription.Paste, subIndex, lineSpeed);
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new ProductionG(productionElement);
        }

        public override ProductionBase GetProduction(DynMvp.Data.ModelDescription modelDescription, string lotNo)
        {
            UniScanG.Data.Model.ModelDescription modelDescription2 = (UniScanG.Data.Model.ModelDescription)modelDescription;

            Production target = CreateProduction(modelDescription2, lotNo, RewinderZone.Unknowen);
            return list.Find(p =>
            {
                return target.Equals(p);
            });
        }

        public override ProductionBase GetProduction(DynMvp.Data.ModelDescription modelDescription, DateTime date, string lotNo)
        {
            UniScanG.Data.Model.ModelDescription modelDescription2 = (UniScanG.Data.Model.ModelDescription)modelDescription;

            Production target = CreateProduction(modelDescription2, lotNo, RewinderZone.Unknowen);
            return list.FindLast(p => target.Equals(p) && p.StartTime.Date == date.Date);
        }

        public override DynMvp.Data.ProductionBase GetLastProduction(DynMvp.Data.Model model)
        {
            UniScanG.Data.Model.ModelDescription modelDescription = (UniScanG.Data.Model.ModelDescription)model.ModelDescription;
            return list.LastOrDefault(p => ((ProductionG)p).Name == modelDescription.Name && ((ProductionG)p).Paste == modelDescription.Paste && ((ProductionG)p).Thickness == modelDescription.Thickness);
        }

        private void ProductionManagerG_OnLotChanged()
        {
            ProductionG curProduction = this.curProduction as ProductionG;
            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Info,
                "Lot Changed. Name: {0}, Rewinder: {1}", new object[] { curProduction.LotNo, curProduction.RewinderZone, curProduction }, ""));
        }
    }
}
