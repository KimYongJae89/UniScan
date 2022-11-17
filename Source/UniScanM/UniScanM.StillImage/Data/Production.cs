using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.StillImage.Data
{
    public class Production : UniScanM.Data.Production
    {
        int marginDefectCnt;
        int blotDefectCnt;
        int pinholeDefectCnt;

        public int MarginDefectCnt { get => marginDefectCnt; set => marginDefectCnt = value; }
        public int BlotDefectCnt { get => blotDefectCnt; set => blotDefectCnt = value; }
        public int PinholeDefectCnt { get => pinholeDefectCnt; set => pinholeDefectCnt = value; }

        public Production(string name, string worker, string lotNo, string paste, string mode, int rewinderSite) : base(name, worker, lotNo, paste, mode, rewinderSite)
        {
            blotDefectCnt = 0;
            marginDefectCnt = 0;
            pinholeDefectCnt = 0;
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
            blotDefectCnt = XmlHelper.GetValue(productionElement, "BlotDefectCnt", blotDefectCnt);
            marginDefectCnt = XmlHelper.GetValue(productionElement, "MarginDefectCnt", marginDefectCnt);
            pinholeDefectCnt = XmlHelper.GetValue(productionElement, "PinholeDefectCnt", pinholeDefectCnt);

        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

                XmlHelper.SetValue(productionElement, "BlotDefectCnt", blotDefectCnt.ToString());
                XmlHelper.SetValue(productionElement, "MarginDefectCnt", marginDefectCnt.ToString());
                XmlHelper.SetValue(productionElement, "PinholeDefectCnt", pinholeDefectCnt.ToString());
        }

        public override void Update(DynMvp.InspData.InspectionResult inspectionResult)
        {
            base.Update(inspectionResult);

            Data.InspectionResult stillImageInspectionResult = inspectionResult as Data.InspectionResult;
            if (stillImageInspectionResult != null && 
                stillImageInspectionResult.ProcessResultList != null && 
                stillImageInspectionResult.ProcessResultList.InterestProcessResult!=null)
            {
                //base.Update(stillImageInspectionResult);
                if (!stillImageInspectionResult.ProcessResultList.InterestProcessResult.IsMarginGood)
                    this.marginDefectCnt++;

                if (!stillImageInspectionResult.ProcessResultList.InterestProcessResult.IsBlotGood)
                    this.blotDefectCnt++;

                if (stillImageInspectionResult.ProcessResultList.DefectRectList.Count > 0)
                    this.pinholeDefectCnt++;
            }
        }
    }
}
