using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public class Production : DynMvp.Data.ProductionBase
    {
        public string Mode { get => mode; set => mode = value; }
        public string Paste { get => paste; set => paste = value; }
        public string Worker { get => worker; set => worker = value; }
        public int StartPosition { get => startPosition; set => startPosition = value; }
        public int EndPosition { get => endPosition; set => endPosition = value; }
        public int LastStartPosition { get => lastStartPosition; set => lastStartPosition = value; }
        public float Value { get => value; set => this.value = value; }
        public int RewinderSite { get => rewinderSite; set => rewinderSite = value; }
        public int LastInspectionNo { get => lastInspectionNo; }

        public override string ResultPath => GetResultPath();

        string mode;
        string paste;
        string worker;
        int startPosition = -1;
        int endPosition = -1;
        int lastStartPosition = -1;
        float value = float.NaN;
        int rewinderSite = -1;

        int lastInspectionNo = -1;

        public Production(string name, string worker, string lotNo, string paste, string mode, int rewinderSite) : base(name,DateTime.Now, lotNo)
        {
            this.paste = paste;
            this.worker = worker;
            this.mode = mode;
            this.rewinderSite = rewinderSite;

            this.lastInspectionNo = 0;

            Reset();
        }

        public Production(XmlElement element) : base(element)
        {

        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            worker = XmlHelper.GetValue(productionElement, "Worker", "");
            paste = XmlHelper.GetValue(productionElement, "Paste", "");
            mode = XmlHelper.GetValue(productionElement, "Mode", "");
            startPosition = Convert.ToInt32(XmlHelper.GetValue(productionElement, "StartPosition", "-1"));
            lastStartPosition = Convert.ToInt32(XmlHelper.GetValue(productionElement, "LastStartPosition", "-1"));
            endPosition = Convert.ToInt32(XmlHelper.GetValue(productionElement, "EndPosition", "-1"));
            value = Convert.ToSingle(XmlHelper.GetValue(productionElement, "Value", float.NaN));
            rewinderSite = Convert.ToInt32(XmlHelper.GetValue(productionElement, "RewinderSite", "-1"));
            lastInspectionNo = XmlHelper.GetValue(productionElement, "LastInspectionNo", -1);
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "Worker", worker);
            XmlHelper.SetValue(productionElement, "Paste", paste);
            XmlHelper.SetValue(productionElement, "Mode", mode);
            XmlHelper.SetValue(productionElement, "StartPosition", startPosition);
            XmlHelper.SetValue(productionElement, "LastStartPosition", lastStartPosition);
            XmlHelper.SetValue(productionElement, "EndPosition", endPosition);
            XmlHelper.SetValue(productionElement, "Value", value);
            XmlHelper.SetValue(productionElement, "RewinderSite", rewinderSite);
            XmlHelper.SetValue(productionElement, "LastInspectionNo", lastInspectionNo);            
        }

        public override string GetModelPath()
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(this.name) as ModelDescription;
            if (md != null)
                return SystemManager.Instance().ModelManager.GetModelPath(md);
            return null;
        }

        public override string GetResultPath()
        {
            string resultPath = System.IO.Path.Combine(PathSettings.Instance().Result, this.StartTime.ToString("yyyyMMdd"), this.Name, this.mode, this.LotNo);
            return resultPath;
        }

        public override void Update(DynMvp.InspData.InspectionResult inspectionResult)
        {
            UniScanM.Data.InspectionResult inspectionResultM = inspectionResult as UniScanM.Data.InspectionResult;
            if (inspectionResultM == null)
                return;

            lock (this)
            {
                this.AddTotal();
                if (inspectionResultM.IsGood())
                    this.AddGood();
                else
                    this.AddNG();

                this.EndPosition = Math.Max(this.EndPosition, inspectionResultM.RollDistance);

                int inspectionNo;
                if (int.TryParse(inspectionResultM.InspectionNo, out inspectionNo))
                    this.lastInspectionNo = Math.Max(this.lastInspectionNo, inspectionNo);
            }
        }
    }
}
