using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScan.Data
{
    public delegate void ScanValueAdded(float value);

    public class ThicknessData
    {
        List<PointF> valueList = new List<PointF>();
        public List<PointF> ValueList { get { return valueList; } }

        float max;
        public float Max { get { return max; } set { max = value; } }

        float min;
        public float Min { get { return min; }
            set { min = value; }
        }

        float average;
        public float Average { get { return average; }
            set { average = value; }
        }

        float range;
        public float Range { get { return range; }
            set { range = value; }
        }

        float stdDev;
        public float StdDev { get { return stdDev; }
            set { stdDev = value; }
        }

        public PointF this[int i]
        {
            get { return valueList[i]; }
        }

        public void Add(float position,  float value)
        {
            valueList.Add(new PointF(position, value));
        }

        public void Clear()
        {
            valueList.Clear();
        }

        public int Count()
        {
            return valueList.Count();
        }

        public void CalcValues()
        {
            List<float> thicknessList = new List<float>();

            for (int i = 0; i < valueList.Count; i++)
            {
                thicknessList.Add(valueList[i].Y);
            }

            max = thicknessList.Max();
            min = thicknessList.Min();
            average = thicknessList.Average();

            range = Max - min;
            stdDev = DynMvp.Vision.DataProcessing.StdDev(thicknessList.ToArray());
        }

        internal string GetStatString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}", max, min, average, range, stdDev);
        }
    }

    public class ScanData
    {
        int index;
        public int Index { get { return index; } set { index = value; } }

        public float SheetMin { get { return sheetThicknessData.Min; } }
        public float SheetMax { get { return sheetThicknessData.Max; } }
        public float SheetAverage { get { return sheetThicknessData.Average; } }

        public float PetMin { get { return petThicknessData.Min; } }
        public float PetMax { get { return petThicknessData.Max; } }
        public float PetAverage { get { return petThicknessData.Average; } }

        public float TrendPosition { get { return index; } }// 임시 코드
        
        DateTime startTime;
        ThicknessData sheetThicknessData = new ThicknessData();
        public ThicknessData SheetThicknessData { get { return sheetThicknessData; } }

        ThicknessData petThicknessData = new ThicknessData();
        public ThicknessData PetThicknessData { get { return petThicknessData; } }

        public ScanData()
        {
            startTime = DateTime.Now;
        }

        public void AddValue(float position, float sheetThickness, float petThickness)
        {
            sheetThicknessData.Add(position, sheetThickness);
            sheetThicknessData.CalcValues();

            petThicknessData.Add(position, sheetThickness);
            petThicknessData.CalcValues();
        }

        public void LoadData(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            startTime = DateTime.Parse(lines[0]);

            sheetThicknessData.Clear();
            petThicknessData.Clear();

            for (int i = 0; i < lines.Count(); i++)
            {
                string[] valueStr = lines[i].Split(',');
                sheetThicknessData.Add(Convert.ToSingle(valueStr[0]), Convert.ToSingle(valueStr[1]));
                petThicknessData.Add(Convert.ToSingle(valueStr[0]), Convert.ToSingle(valueStr[2]));
            }

            sheetThicknessData.CalcValues();
            petThicknessData.CalcValues();
        }

        public void SaveData(string fileName)
        {
            List<string> lines = new List<string>();

            lines.Add(startTime.ToString());

            for (int i = 0; i < sheetThicknessData.Count(); i++)
                lines.Add(String.Format("{0},{1},{2}", sheetThicknessData[i].X.ToString(),  sheetThicknessData[i].Y.ToString(), petThicknessData[i].Y.ToString()));
        }

        public int Count()
        {
            return sheetThicknessData.Count();
        }

        public string GetStatString()
        {
            return sheetThicknessData.GetStatString() + ", " + petThicknessData.GetStatString();
        }
    }

    class LotResult
    {
        string modelName;
        string lotNo;
        DateTime startTime;
        List<ScanData> scanDataList = new List<ScanData>();

        string lotDataPath;

        public LotResult(Model model, string lotNo)
        {
            modelName = model.Name;
            startTime = DateTime.Now;
            this.lotNo = lotNo;

            lotDataPath = Path.Combine(PathSettings.Instance().Result, modelName, startTime.ToString("yyyyMMddHHmmss"), lotNo);

            Directory.CreateDirectory(lotDataPath);
        }

        public LotResult(string lotDataPath)
        {
            this.lotDataPath = lotDataPath;
        }

        public void AddScanData(ScanData scanData)
        {
            scanDataList.Add(scanData);
            scanData.Index = scanDataList.Count();

            string scanFileName = Path.Combine(lotDataPath, scanData.Index.ToString("00000"), ".dat");
            scanData.SaveData(scanFileName);

            string statFileName = Path.Combine(lotDataPath, "stat.dat");
            if (File.Exists(statFileName) == false)
                File.WriteAllText(statFileName, scanData.GetStatString() + "\n");
            else
                File.AppendAllText(statFileName, scanData.GetStatString() + "\n");
        }
    }
}
