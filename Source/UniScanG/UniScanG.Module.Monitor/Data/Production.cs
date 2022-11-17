using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniScanG.Data;

namespace UniScanG.Module.Monitor.Data
{
    public class Production : UniScanG.Data.Production
    {
        public int NgMargin => ngMargin;
        int ngMargin;

        public int NgBlot => ngBlot;
        int ngBlot;

        public int NgPinhole => ngDefect;
        int ngDefect;


        public Production(XmlElement productionElement) : base(productionElement)
        {
        }

        public Production(string name, DateTime dateTime, string lotNo, RewinderZone rewinderZone, float lineSpeed)
            : base(name, dateTime, lotNo, rewinderZone, 0, "", lineSpeed) { }

        public override void Update(InspectionResult inspectionResult)
        {
            //AddTotal();
            Inspect.InspectionResult inspectionResult2 = inspectionResult as Inspect.InspectionResult;

            switch (inspectionResult.Judgment)
            {
                case Judgment.Accept:
                case Judgment.Warn:
                    AddGood();
                    break;
                case Judgment.Reject:
                case Judgment.FalseReject:
                    AddNG();
                    if (!inspectionResult2.MarginResult)
                        this.ngMargin++;
                    if (!inspectionResult2.BlotResult)
                        this.ngBlot++;
                    if (inspectionResult2.DefectCount > 0)
                        this.ngDefect++;
                    break;
                case Judgment.Skip:
                    AddPass();
                    break;
                    
            }
            
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "NgMargin", ngMargin.ToString());
            XmlHelper.SetValue(productionElement, "NgBlot", ngBlot.ToString());
            XmlHelper.SetValue(productionElement, "NgDefect", ngDefect.ToString());
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            ngMargin = XmlHelper.GetValue(productionElement, "NgMargin", 0);
            ngBlot = XmlHelper.GetValue(productionElement, "NgBlot", 0);
            ngDefect = XmlHelper.GetValue(productionElement, "NgDefect", 0);
        }
    }

    public class ProductionManager : UniScanG.Data.ProductionManager
    {
        public ProductionManager(string defaultPath) : base(defaultPath) { }

        public override UniScanG.Data.Production CreateProduction(UniScanG.Data.Model.ModelDescription modelDescription, string lotNo, RewinderZone rewinderZone,int subIndex, float lineSpeed)
        {
            return new Production(modelDescription.Name,DateTime.Now, lotNo, rewinderZone, lineSpeed);
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new Production(productionElement);
        }
    }
}
