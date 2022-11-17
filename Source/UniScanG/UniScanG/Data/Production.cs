using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Data;
using UniScanG.Data.Model;
using UniScanG.Gravure.Data;

namespace UniScanG.Data
{
    public enum RewinderZone { Unknowen = -1, ZoneA, ZoneB }

    public abstract class Production : UniEye.Base.Data.Production
    {
        public RewinderZone RewinderZone => rewinderZone;
        protected RewinderZone rewinderZone;

        public float Thickness => this.thickness;
        protected float thickness;

        public string Paste => this.paste;
        protected string paste;

        public float LineSpeedMpm { get; set; }

        public string Note { get => this.note; set => this.note = string.IsNullOrEmpty(value) ? "" : value; }
        protected string note = "";

        public Production(string name, DateTime dateTime, string lotNo, RewinderZone rewinderZone, float thickness, string paste, float lineSpeed) : base(name, dateTime, lotNo)
        {
            this.rewinderZone = rewinderZone;
            this.thickness = thickness;
            this.paste = paste;
            this.LineSpeedMpm = lineSpeed;
        }

        public Production(XmlElement productionElement) : base (productionElement)
        {

        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            this.rewinderZone = XmlHelper.GetValue(productionElement, "RewinderZone", this.rewinderZone);
            this.thickness = Convert.ToSingle(XmlHelper.GetValue(productionElement, "Thickness", ""));
            this.paste = XmlHelper.GetValue(productionElement, "Paste", "");
            this.LineSpeedMpm = XmlHelper.GetValue(productionElement, "LineSpeedMpm", this.LineSpeedMpm);
            this.note = XmlHelper.GetValue(productionElement, "Note", "");
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "RewinderZone", this.rewinderZone.ToString());
            XmlHelper.SetValue(productionElement, "Thickness", this.thickness);
            XmlHelper.SetValue(productionElement, "Paste", this.paste);
            XmlHelper.SetValue(productionElement, "LineSpeedMpm", this.LineSpeedMpm);
            XmlHelper.SetValue(productionElement, "Note", this.note);
        }

        public virtual string GetResultPath(string root)
        {
            return PathManager.GetResultPath(root, this.Name, this.StartTime, this.LotNo);
        }

        public override bool Equals(object obj)
        {
            Production production = obj as Production;
            if (production == null)
                return false;
                
            return
                base.Equals(obj) &&
                this.thickness == production.thickness &&
                this.paste == production.paste &&
                (this.rewinderZone == RewinderZone.Unknowen ? true : this.rewinderZone == production.rewinderZone);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.LotNo.GetHashCode();
        }
    }

    public abstract class ProductionManager : UniEye.Base.Data.ProductionManager
    {
        public ProductionManager(string defaultPath) : base(defaultPath)
        {
            this.Load();
        }

        public abstract Production CreateProduction(UniScanG.Data.Model.ModelDescription modelDescription, string lotNo, RewinderZone rewinderZone, int subIndex = 0, float lineSpeed = 0);

        private Production BuildProduction(Model.ModelDescription modelDescription, string lotNo, RewinderZone rewinderZone, int subIndex)
        {
            Production production = (Production)this.CreateProduction(modelDescription, lotNo, rewinderZone, subIndex);
            lock (this.list)
            {
                this.list.Add(production);
                this.Save();
            }

            return production;
        }

        public ProductionBase[] GetProductions(UniScanG.Data.Model.ModelDescription modelDescription, DateTime date, string lotNo, RewinderZone rewinderZone)
        {
            ProductionBase targetProduction = CreateProduction(modelDescription, lotNo, rewinderZone);
            List<ProductionBase> findAll = list.FindAll(p => targetProduction.Equals(p) && p.StartTime.Date == date.Date);
            return list.FindAll(p => targetProduction.Equals(p)).ToArray();
        }

        public ProductionBase LotChange(UniScanG.Data.Model.Model model, DateTime date, string lotNo, RewinderZone rewinderZone, float lineSpeed)
        {
            ProductionBase[] productions = this.GetProductions(model.ModelDescription, date, lotNo, rewinderZone);
            Production production = (Production)productions.LastOrDefault(f => !f.UserRemoveFlag);
            if (production == null)
            {
                int subIndex = productions.Length == 0 ? 0 : productions.Max(f => ((Gravure.Data.ProductionG)f).SubIndex) + 1;
                production = BuildProduction(model.ModelDescription, lotNo, rewinderZone, subIndex);
            }
            production.LineSpeedMpm = lineSpeed;

            base.LotChange(production);
            
            return production;
        }
    }
}
