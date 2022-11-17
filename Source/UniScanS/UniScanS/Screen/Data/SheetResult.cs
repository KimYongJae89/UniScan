using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Settings;
using UniScanS.Screen.Vision;
using UniScanS.Screen.Vision.Detector;

namespace UniScanS.Screen.Data
{
    public enum DefectType
    {
        Total, SheetAttack, Pole, Dielectric, PinHole, Shape
    }

    public class SheetResult : AlgorithmResult, IExportable
    {
        protected SheetErrorType sheetErrorType = SheetErrorType.None;
        public SheetErrorType SheetErrorType
        {
            get { return sheetErrorType; }
            set { sheetErrorType = value; }
        }

        protected Bitmap prevImage = null;
        public Bitmap PrevImage
        {
            get { return prevImage; }
            set { prevImage = value; }
        }

        protected List<SheetSubResult> sheetAttackList = new List<SheetSubResult>();
        public List<SheetSubResult> SheetAttackList
        {
            get { return sheetAttackList; }
            set { sheetAttackList = value; }
        }

        protected List<SheetSubResult> poleList = new List<SheetSubResult>();
        public List<SheetSubResult> PoleList
        {
            get { return poleList; }
            set { poleList = value; }
        }

        protected List<SheetSubResult> dielectricList = new List<SheetSubResult>();
        public List<SheetSubResult> DielectricList
        {
            get { return dielectricList; }
            set { dielectricList = value; }
        }

        protected List<SheetSubResult> pinHoleList = new List<SheetSubResult>();
        public List<SheetSubResult> PinHoleList
        {
            get { return pinHoleList; }
            set { pinHoleList = value; }
        }

        protected List<ShapeResult> shapeList = new List<ShapeResult>();

        public SheetResult(string algorithmName = "") : base(algorithmName)
        {
        }

        public List<ShapeResult> ShapeList
        {
            get { return shapeList; }
            set { shapeList = value; }
        }

        public int DefectNum
        {
            get
            {
                return sheetAttackList.Count + poleList.Count + dielectricList.Count
                  + pinHoleList.Count + shapeList.Count;
            }
        }

        public List<SheetSubResult> SheetSubResultList
        {
            get
            {
                List<SheetSubResult> sheetSubResult = new List<SheetSubResult>();
                sheetSubResult.AddRange(sheetAttackList);
                sheetSubResult.AddRange(poleList);
                sheetSubResult.AddRange(dielectricList);
                sheetSubResult.AddRange(pinHoleList);
                sheetSubResult.AddRange(shapeList);
                return sheetSubResult;
            }
        }

        public SheetResult() : base(SheetInspector.TypeName)
        {

        }

        public void Copy(SheetResult sheetResult)
        {
            this.sheetErrorType = sheetResult.sheetErrorType;
            this.SpandTime = sheetResult.SpandTime;
            this.prevImage = sheetResult.prevImage;
            this.sheetAttackList = sheetResult.sheetAttackList;
            this.poleList = sheetResult.poleList;
            this.dielectricList = sheetResult.dielectricList;
            this.pinHoleList = sheetResult.pinHoleList;
            this.shapeList = sheetResult.shapeList;

            this.good = sheetResult.DefectNum == 0 ? true : false;
        }

        public void Copy(SheetResult sheetResult, int camIndex)
        {
            this.sheetErrorType = sheetResult.sheetErrorType;
            this.SpandTime = sheetResult.SpandTime;
            this.prevImage = sheetResult.prevImage;
            this.sheetAttackList.AddRange(sheetResult.sheetAttackList.FindAll(r => r.CamIndex == camIndex));
            this.poleList.AddRange(sheetResult.poleList.FindAll(r => r.CamIndex == camIndex));
            this.dielectricList.AddRange(sheetResult.dielectricList.FindAll(r => r.CamIndex == camIndex));
            this.pinHoleList.AddRange(sheetResult.pinHoleList.FindAll(r => r.CamIndex == camIndex));
            this.shapeList.AddRange(sheetResult.shapeList.FindAll(r => r.CamIndex == camIndex));

            this.good = sheetResult.DefectNum == 0 ? true : false;
        }

        public void AddSheetSubResult(List<SheetSubResult> sheetAttackList, List<SheetSubResult> poleList,
            List<SheetSubResult> dielectricList, List<SheetSubResult> pinHoleList, List<ShapeResult> shapeList)
        {
            this.sheetAttackList.AddRange(sheetAttackList);
            this.poleList.AddRange(poleList);
            this.dielectricList.AddRange(dielectricList);
            this.pinHoleList.AddRange(pinHoleList);
            this.shapeList.AddRange(shapeList);
        }

        public void RemoveAllIntersectResult()
        {
            lock (this)
            {
                List<SheetSubResult> tempList = new List<SheetSubResult>();

                tempList.AddRange(sheetAttackList);
                tempList.AddRange(poleList);
                tempList.AddRange(dielectricList);
                tempList.AddRange(pinHoleList);
                tempList.AddRange(shapeList);

                tempList = tempList.OrderByDescending(result => result.Area).ToList();

                for (int srcIndex = 0; srcIndex < tempList.Count; srcIndex++)
                {
                    RectangleF region = tempList[srcIndex].Region;
                    region.Inflate(AlgorithmSetting.Instance().DefectDistance, AlgorithmSetting.Instance().DefectDistance);

                    for (int destIndex = srcIndex + 1; destIndex < tempList.Count; destIndex++)
                    {
                        if (region.IntersectsWith(tempList[destIndex].Region) == true)
                        {
                            tempList.RemoveAt(destIndex);
                            destIndex--;
                        }
                    }
                }

                sheetAttackList.Clear();
                poleList.Clear();
                dielectricList.Clear();
                pinHoleList.Clear();
                shapeList.Clear();

                for (int i = 0; i < Math.Min(tempList.Count, AlgorithmSetting.Instance().MaxDefectNum); i++)
                {
                    switch (tempList[i].DefectType)
                    {
                        case DefectType.SheetAttack:
                            sheetAttackList.Add(tempList[i]);
                            break;
                        case DefectType.Pole:
                            poleList.Add(tempList[i]);
                            break;
                        case DefectType.Dielectric:
                            dielectricList.Add(tempList[i]);
                            break;
                        case DefectType.PinHole:
                            pinHoleList.Add(tempList[i]);
                            break;
                        case DefectType.Shape:
                            shapeList.Add((ShapeResult)tempList[i]);
                            break;
                    }
                }
            }
        }

        public static SheetResult operator +(SheetResult sheetResult1, SheetResult sheetResult2)
        {
            SheetResult sheetResult = new SheetResult("");

            sheetResult.sheetAttackList.AddRange(sheetResult1.sheetAttackList);
            sheetResult.poleList.AddRange(sheetResult1.poleList);
            sheetResult.dielectricList.AddRange(sheetResult1.dielectricList);
            sheetResult.pinHoleList.AddRange(sheetResult1.pinHoleList);
            sheetResult.shapeList.AddRange(sheetResult1.shapeList);

            sheetResult.sheetAttackList.AddRange(sheetResult2.sheetAttackList);
            sheetResult.poleList.AddRange(sheetResult2.poleList);
            sheetResult.dielectricList.AddRange(sheetResult2.dielectricList);
            sheetResult.pinHoleList.AddRange(sheetResult2.pinHoleList);
            sheetResult.shapeList.AddRange(sheetResult2.shapeList);

            sheetResult.SpandTime = sheetResult1.SpandTime > sheetResult2.SpandTime ? sheetResult1.SpandTime : sheetResult2.SpandTime;

            if (sheetResult1.sheetErrorType != SheetErrorType.None)
                sheetResult.sheetErrorType = sheetResult1.sheetErrorType;
            else if (sheetResult2.sheetErrorType != SheetErrorType.None)
                sheetResult.sheetErrorType = sheetResult2.sheetErrorType;

            return sheetResult;
        }

        public void Offset(int x, int y)
        {
            if (x == 0 && y == 0)
                return;

            float resizeReatio = SystemTypeSettings.Instance().ResizeRatio;

            sheetAttackList.ForEach(result => result.Offset(x, y, resizeReatio));
            poleList.ForEach(result => result.Offset(x, y, resizeReatio));
            dielectricList.ForEach(result => result.Offset(x, y, resizeReatio));
            pinHoleList.ForEach(result => result.Offset(x, y, resizeReatio));
            shapeList.ForEach(result => result.Offset(x, y, resizeReatio));
        }

        public static string AppendPostString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Inspect Time");
            stringBuilder.AppendLine();

            stringBuilder.Append("Cam\tIndex\tType\tX(pixel)\tY(pixel)\tWidth(pixel)\tHeight(pixel)\tArea(pixel)\tCalX(um)\tCalY(um)\tCalLength(um)\t");
            stringBuilder.AppendLine();
            stringBuilder.Append("Lower\tUpper\tCompactness\tElongation");
            stringBuilder.AppendLine();
            stringBuilder.Append("Area\tWidth\tHeight\tOffsetX\tOffsetY");
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        public virtual void Export(string key, string path)
        {
            ImageHelper.SaveImage(prevImage, Path.Combine(path, string.Format("Prev.Jpg")));
            
            string fileName = Path.Combine(path, string.Format("{0}.csv", key));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(AppendPostString());
            stringBuilder.AppendFormat("{0}\t{1}", this.SpandTime, this.sheetErrorType.ToString());
            stringBuilder.AppendLine();

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            int subIndex = 1;
            foreach (SheetSubResult subResult in SheetSubResultList)
            {
                subResult.Index = subIndex;
                subResult.CamIndex = client.GetCamIndex() + 1;
                stringBuilder.Append(subResult.ToExportString());
                stringBuilder.AppendLine();
                ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("{0}.bmp", subIndex)));
                subIndex++;
            }
            
            File.WriteAllText(fileName, stringBuilder.ToString()); 
        }
        
        public virtual void Import(string key, string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", key));

            if (File.Exists(fileName) == false)
                 return;

            string imageName = Path.Combine(path, "Prev.jpg");
            if (File.Exists(imageName) == true)
            {
                Bitmap image = (Bitmap)ImageHelper.LoadImage(imageName);
                prevImage = ImageHelper.MakeGrayscale(image);
            }

            string[] lines = File.ReadAllLines(fileName);
            lines = lines.Skip(4).ToArray();

            string[] firstLine = lines[0].Split('\t').ToArray();
            SpandTime = TimeSpan.Parse(firstLine[0]);

            if (firstLine.Length > 1)
                Enum.TryParse(firstLine[1], out sheetErrorType);

            lines = lines.Skip(1).ToArray();

            object lockObj = new object();
            foreach (string line in lines)
            //Parallel.ForEach(lines, line =>
            {
                string[] splitLine = line.Split('\t').ToArray();

                string subImageName = Path.Combine(path, string.Format("{0}.bmp", splitLine[1]));
                Bitmap subImage = null;
                if (File.Exists(subImageName) == true)
                    subImage = (Bitmap)ImageHelper.LoadImage(subImageName);
                    
                DefectType defectType = SheetSubResult.GetDefectType(splitLine[2]);
                
                SheetSubResult sheetSubResult;
                if (defectType == DefectType.Shape)
                    sheetSubResult = new ShapeResult("");
                else
                    sheetSubResult = new SheetSubResult("");
                
                sheetSubResult.Image = subImage;

                sheetSubResult.FromExportData(splitLine);

                lock (lockObj)
                {
                    switch (defectType)
                    {
                        case DefectType.SheetAttack:
                            sheetAttackList.Add(sheetSubResult);
                            break;
                        case DefectType.Pole:
                            poleList.Add(sheetSubResult);
                            break;
                        case DefectType.Dielectric:
                            dielectricList.Add(sheetSubResult);
                            break;
                        case DefectType.PinHole:
                            pinHoleList.Add(sheetSubResult);
                            break;
                        case DefectType.Shape:
                            shapeList.Add((ShapeResult)sheetSubResult);
                            break;
                    }
                }
            }
        }

        public void Clear()
        {
            sheetAttackList.Clear();
            poleList.Clear();
            dielectricList.Clear();
            pinHoleList.Clear();
            shapeList.Clear();
        }
    }
}
