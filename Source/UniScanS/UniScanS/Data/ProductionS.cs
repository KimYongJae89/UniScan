using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Data;
using UniScanS.Screen.Data;

namespace UniScanS.Data
{
    public class ProductionS : Production
    {
        int sheetIndex;
        public int SheetIndex
        {
            get { return sheetIndex; }
            set { sheetIndex = value; }
        }

        string thickness;
        public string Thickness
        {
            get { return thickness; }
        }

        string paste;
        public string Paste
        {
            get { return paste; }
        }

        public int TotalDefectNum
        {
            get { return sheetAttackNum + poleNum + dielectricNum + pinHoleNum + shapeNum; }
        }

        public int TotalDefectPatternNum
        {
            get { return sheetAttackPatternNum + polePatternNum + dielectricPatternNum + pinHolePatternNum + shapePatternNum; }
        }

        int sheetAttackNum;
        public int SheetAttackNum
        {
            get { return sheetAttackNum; }
        }

        int poleNum;
        public int PoleNum
        {
            get { return poleNum; }
        }
        
        int dielectricNum;
        public int DielectricNum
        {
            get { return dielectricNum; }
        }

        int pinHoleNum;
        public int PinHoleNum
        {
            get { return pinHoleNum; }
        }

        int shapeNum;
        public int ShapeNum
        {
            get { return shapeNum; }
        }

        int sheetAttackPatternNum;
        public int SheetAttackPatternNum
        {
            get { return sheetAttackPatternNum; }
        }

        int polePatternNum;
        public int PolePatternNum
        {
            get { return polePatternNum; }
        }

        int dielectricPatternNum;
        public int DielectricPatternNum
        {
            get { return dielectricPatternNum; }
        }

        int pinHolePatternNum;
        public int PinHolePatternNum
        {
            get { return pinHolePatternNum; }
        }

        int shapePatternNum;
        public int ShapePatternNum
        {
            get { return shapePatternNum; }
        }

        public ProductionS(string name, DateTime dateTime, string thickness, string paste, string lotNo) : base(name, dateTime, lotNo)
        {
            this.thickness = thickness;
            this.paste = paste;
        }

        public override void Reset()
        {
            base.Reset();

            sheetIndex = 0;

            sheetAttackNum = 0;
            poleNum = 0;
            dielectricNum = 0;
            pinHoleNum = 0;
            shapeNum = 0;

            sheetAttackPatternNum = 0;
            polePatternNum = 0;
            dielectricPatternNum = 0;
            pinHolePatternNum = 0;
            shapePatternNum = 0;
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            sheetIndex = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetIndex", "0"));

            thickness = XmlHelper.GetValue(productionElement, "Thickness", "");
            paste = XmlHelper.GetValue(productionElement, "Paste", "");

            sheetAttackNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetAttackNum", "0"));
            poleNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PoleNum", "0"));
            dielectricNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "DielectricNum", "0"));
            pinHoleNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PinHoleNum", "0"));
            shapeNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "ShapeNum", "0"));

            sheetAttackPatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetAttackPatternNum", "0"));
            polePatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PolePatternNum", "0"));
            dielectricPatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "DielectricPatternNum", "0"));
            pinHolePatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PinHolePatternNum", "0"));
            shapePatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "ShapePatternNum", "0"));
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "SheetIndex", sheetIndex.ToString());

            XmlHelper.SetValue(productionElement, "Thickness", thickness);
            XmlHelper.SetValue(productionElement, "Paste", paste);

            XmlHelper.SetValue(productionElement, "SheetAttackNum", sheetAttackNum.ToString());
            XmlHelper.SetValue(productionElement, "PoleNum", poleNum.ToString());
            XmlHelper.SetValue(productionElement, "DielectricNum", dielectricNum.ToString());
            XmlHelper.SetValue(productionElement, "PinHoleNum", pinHoleNum.ToString());
            XmlHelper.SetValue(productionElement, "ShapeNum", shapeNum.ToString());

            XmlHelper.SetValue(productionElement, "SheetAttackPatternNum", sheetAttackPatternNum.ToString());
            XmlHelper.SetValue(productionElement, "PolePatternNum", polePatternNum.ToString());
            XmlHelper.SetValue(productionElement, "DielectricPatternNum", dielectricPatternNum.ToString());
            XmlHelper.SetValue(productionElement, "PinHolePatternNum", pinHolePatternNum.ToString());
            XmlHelper.SetValue(productionElement, "ShapePatternNum", shapePatternNum.ToString());
        }

        public void Update(SheetResult sheetResult)
        {
            if (sheetResult.Good)
                AddGood();
            else
                AddNG();

            if (sheetResult.SheetAttackList.Count > 0)
            {
                sheetAttackNum += sheetResult.SheetAttackList.Count;
                sheetAttackPatternNum++;
            }
            
            if (sheetResult.PoleList.Count > 0)
            {
                poleNum += sheetResult.PoleList.Count;
                polePatternNum++;
            }

            if (sheetResult.DielectricList.Count > 0)
            {
                dielectricNum += sheetResult.DielectricList.Count;
                dielectricPatternNum++;
            }

            if (sheetResult.PinHoleList.Count > 0)
            {
                pinHoleNum += sheetResult.PinHoleList.Count;
                pinHolePatternNum++;
            }

            if (sheetResult.ShapeList.Count > 0)
            {
                shapeNum += sheetResult.ShapeList.Count;
                shapePatternNum++;
            }
        }
    }
}
