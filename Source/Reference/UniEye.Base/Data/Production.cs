using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
using DynMvp.InspData;

namespace UniEye.Base.Data
{
    public class Production : DynMvp.Data.ProductionBase
    {
        public Production(string name, DateTime dateTime, string lotNo) : base(name, dateTime, lotNo) { }
        public Production(XmlElement xmlElement) : base(xmlElement) { }

        public override string ResultPath => PathManager.GetResultPath(this.Name, this.StartTime, this.LotNo);

        public override void Update(InspectionResult sheetResult)
        {
            AddTotal();
            switch (sheetResult.Judgment)
            {
                case Judgment.Accept:
                case Judgment.Warn:
                    AddGood();
                    break;
                case Judgment.FalseReject:
                case Judgment.Reject:
                    AddNG();
                    break;
                case Judgment.Skip:
                    AddPass();
                    break;
            }
        }

        public override string GetModelPath()
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(this.name);
            if (md != null)
                return SystemManager.Instance().ModelManager.GetModelPath(md);
            return null;
        }

        public override string GetResultPath()
        {
            return ResultPath;
        }
    }


    public interface IProductionListener
    {
        void ProductionChanged();
    }


    public class ProductionManager : DynMvp.Data.ProductionManagerBase
    {
        List<IProductionListener> productionChangedListenerList = new List<IProductionListener>();

        public ProductionManager(string defaultPath) : base(defaultPath) { }

        public void AddListener(IProductionListener listener)
        {
            productionChangedListenerList.Add(listener);
        }

        public void RemoveProduction(int storingDays)
        {
            lock (list)
                list.RemoveAll(p => DateTime.Now.Day - p.StartTime.Day > storingDays);
        }


        public override ProductionBase CreateProduction(Model model, string lotNo)
        {
            return new Production(model == null ? "" : model.Name,DateTime.Now, lotNo);
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new Production(productionElement);
        }
    }

}
