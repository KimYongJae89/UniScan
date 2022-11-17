using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.Pinhole.Data
{
    public class Production : UniScanM.Data.Production
    {
        int[] sectionIndex;
        int[] pinHoleDefect;
        int[] dustDefect;

        public int Section1
        {
            get { return sectionIndex.First(); }
        }

        public int Section2
        {
            get { return sectionIndex.Last(); }
        }

        public int TotalNum
        {
            get
            {
                if ((pinHoleDefect.Sum() + dustDefect.Sum()) >= 37767)
                    CountReset();
                return pinHoleDefect.Sum() + dustDefect.Sum();
            }
        }

        void CountReset()
        {
            for(int i = 0; i < SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count; i++)
            {
                pinHoleDefect[i] = 1;
                dustDefect[i] = 1;
            }
        }

        public int TotalPinholeNum
        {
            get { return pinHoleDefect.Sum(); }
        }

        public int TotalDustNum
        {
            get { return dustDefect.Sum(); }
        }
        
        public int Num1
        {
            get { return pinHoleDefect.First() + dustDefect.First(); }
        }

        public int PinholeNum1
        {
            get { return pinHoleDefect.First(); }
        }

        public int DustNum1
        {
            get { return dustDefect.First(); }
        }

        public int Num2
        {
            get { return pinHoleDefect.Last() + dustDefect.Last(); }
        }

        public int PinholeNum2
        {
            get { return pinHoleDefect.Last(); }
        }

        public int DustNum2
        {
            get { return dustDefect.Last(); }
        }

        //labelTotal.Text = model.TotalDefect.ToString();
        //labelTotalPinHole.Text = model.TotalPinHoleDefect.ToString();
        //labelTotalDirty.Text = model.TotalDustDefect.ToString();
        //labelCamera1Total.Text = model.CamDefect[0].ToString();
        //labelCamera1PinHole.Text = model.PinHoleDefect[0].ToString();
        //labelCamera1Dirty.Text = model.DustDefect[0].ToString();
        //labelCamera2Total.Text = model.CamDefect[1].ToString();
        //labelCamera2PinHole.Text = model.PinHoleDefect[1].ToString();
        //labelCamera2Dirty.Text = model.DustDefect[1].ToString();
        //labelCam1Section.Text = model.SectionIndex[0].ToString();
        //labelCam2Section.Text = model.SectionIndex[1].ToString();

        public Production(string name, string worker, string lotNo, string paste, string mode, int rewinderSite) : base(name, worker, lotNo, paste, mode, rewinderSite)
        {
            int count = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;
            sectionIndex = new int[count];
            pinHoleDefect = new int[count];
            dustDefect = new int[count];
        }

        public void AddResult(InspectionResult inspectionResult)
        {
            sectionIndex[inspectionResult.DeviceIndex] = inspectionResult.SectionIndex;
            pinHoleDefect[inspectionResult.DeviceIndex] += inspectionResult.LastDefectInfoList.Count(defect => defect.DefectType == DefectType.Pinhole);
            dustDefect[inspectionResult.DeviceIndex] += inspectionResult.LastDefectInfoList.Count(defect => defect.DefectType == DefectType.Dust);
        }

        public Production(XmlElement element) : base(element)
        {

        }

        public override void Load(XmlElement productionElement)
        {
            int count = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;
            sectionIndex = new int[count];
            pinHoleDefect = new int[count];
            dustDefect = new int[count];

            base.Load(productionElement);
            for (int i = 0; i < count; i++)
            {
                sectionIndex[i] = Convert.ToInt32(XmlHelper.GetValue(productionElement, string.Format("SectionIndex{0}", i), "0"));
                pinHoleDefect[i] = Convert.ToInt32(XmlHelper.GetValue(productionElement, string.Format("PinHoleDefect{0}", i), "0"));
                dustDefect[i] = Convert.ToInt32(XmlHelper.GetValue(productionElement, string.Format("DustDefect{0}", i), "0"));
            }
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            for (int i = 0; i < SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count; i++)
            {
                XmlHelper.SetValue(productionElement, string.Format("SectionIndex{0}", i), sectionIndex[i]);
                XmlHelper.SetValue(productionElement, string.Format("PinHoleDefect{0}", i), pinHoleDefect[i]);
                XmlHelper.SetValue(productionElement, string.Format("DustDefect{0}", i), dustDefect[i]);
            }
        }

        public override void Update(DynMvp.InspData.InspectionResult inspectionResult)
        {
            base.Update(inspectionResult);

            InspectionResult pinholeInspectionResult = inspectionResult as InspectionResult;
            this.AddResult(pinholeInspectionResult);
        }
    }
}
