using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision
{
    public class ProcessTimeLog
    {
        public enum ProcessTimeLogItem { Align, Align2, Build, Calculate, Binalize, Detection, Classification, Extend_StopImg, Extend_Margin, Extend_Transform, MAX_VALUE }
        Dictionary<ProcessTimeLogItem, List<long>> dic = null;
        Dictionary<ProcessTimeLogItem, int> parallelUnit = null;

        public ProcessTimeLog()
        {
            this.dic = new Dictionary<ProcessTimeLogItem, List<long>>();
            this.parallelUnit = new Dictionary<ProcessTimeLogItem, int>();
            Array array = Enum.GetValues(typeof(ProcessTimeLogItem));
            foreach (Enum e in array)
                dic.Add((ProcessTimeLogItem)e, new List<long>());
        }

        public double Get(ProcessTimeLogItem item)
        {
            double milisec = -1;
            if (this.dic[item].Count > 0)
            {
                int parallelUnit = -1;
                if (this.parallelUnit.ContainsKey(item))
                    parallelUnit = this.parallelUnit[item];

                if (parallelUnit < 0)
                    milisec = this.dic[item].Average();
                else if (parallelUnit == 0)
                    milisec = this.dic[item].Sum();
                else
                {
                    milisec = 0;
                    for (int i = 0; i < this.dic[item].Count; i += parallelUnit)
                    {
                        int dst = Math.Min(this.dic[item].Count, i + parallelUnit);
                        double mSec = this.dic[item].GetRange(i, dst - i).Average();
                        milisec += mSec;
                    }
                }
            }
            return milisec;
        }

        public void SetSerial(ProcessTimeLogItem item)
        {
            if (!this.parallelUnit.ContainsKey(item))
                this.parallelUnit.Add(item, -1);
            this.parallelUnit[item] = -1;   
        }

        public void SetParallel(ProcessTimeLogItem item, int parallelUnit)
        {
            if (!this.parallelUnit.ContainsKey(item))
                this.parallelUnit.Add(item, parallelUnit);
            this.parallelUnit[item] = parallelUnit;
        }

        public void Add(ProcessTimeLogItem item, long milsec)
        {
            List<long> list = this.dic[item];
            lock (list)
                list.Add(milsec);
        }

        public static string GetExportHeader()
        {
            Array array = Enum.GetValues(typeof(ProcessTimeLogItem));
            string[] strings = new string[array.Length - 1];
            for (int i = 0; i < array.Length - 1; i++)
                strings[i] = array.GetValue(i).ToString();
            return string.Join(",", strings);
        }

        public string GetExportData()
        {
            Array array = Enum.GetValues(typeof(ProcessTimeLogItem));
            double[] values = new double[array.Length - 1];
            for (int i = 0; i < array.Length - 1; i++)
                values[i] = Get((ProcessTimeLogItem)i);
            return string.Join(",", Array.ConvertAll(values, f => f.ToString("F01")));
        }

        public string GetData()
        {
            Array array = Enum.GetValues(typeof(ProcessTimeLogItem));
            Tuple<string, double>[] tuples = new Tuple<string, double>[array.Length - 1];
            for (int i = 0; i < array.Length - 1; i++)
                tuples[i] = new Tuple<string, double>(array.GetValue(i).ToString(), Get((ProcessTimeLogItem)i));
            return string.Join(",", Array.ConvertAll(tuples, f => $"{f.Item1}: {f.Item2.ToString("F01")}"));
        }

        public string GetToolStripText()
        {
            StringBuilder sb = new StringBuilder();

            Array array = Enum.GetValues(typeof(ProcessTimeLogItem));
            for (int i = 0; i < array.Length - 1; i++)
            {
                ProcessTimeLogItem item = (ProcessTimeLogItem)i;
                double time = Get(item);
                if (time >= 0)
                {
                    string headChar = this.parallelUnit[item] == 0 || this.parallelUnit[item] == 1 ? $"S{this.parallelUnit[item]:00}" : $"P{this.parallelUnit[item]:00}";
                    sb.AppendLine($"[{headChar}]{item}: {time:F02}");
                }
            }

            return sb.ToString().Trim();
        }

        public int GetTotalTimeMs()
        {
            double totalTimeMs = this.dic.Keys.ToList().Sum(f => Get(f));
            return (int)totalTimeMs;
        }
    }

    public class DebugContextG : DebugContext
    {
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        DateTime dateTime;

        public string LotNo { get => lotNo; set => lotNo = value; }
        string lotNo = "";

        public string FrameId { get => frameId; set => frameId = value; }
        string frameId = "";

        public int PatternId { get => patternId; set => patternId = value; }
        int patternId= -1;

        public bool IsSticker { get => isSticker; set => isSticker = value; }
        bool isSticker;

        public int RegionId { get => regionId; set => regionId = value; }
        int regionId = -1;

        public int LineSetId { get => lineSetId; set => lineSetId = value; }
        int lineSetId = -1;

        public int LineId { get => lineId; set => lineId = value; }
        int lineId = -1;

        public int BlobId { get => blobId; set => blobId = value; }
        int blobId = -1;

        public ProcessTimeLog ProcessTimeLog { get => this.processTimeLog; }
        ProcessTimeLog processTimeLog;

        public override string FullPath => GetFullPath();

        public DebugContextG(bool saveDebugImage, string path) : base(saveDebugImage, path)
        {
            Clear();
            this.processTimeLog = new ProcessTimeLog();
        }

        public DebugContextG(DebugContext debugContext) : base(debugContext)
        {
            if (debugContext is DebugContextG)
            {
                this.CopyFrom((DebugContextG)debugContext);
                //this.processTimeLog = ((DebugContextG)debugContext).processTimeLog;
            }
            else
            {
                Clear();
                this.processTimeLog = new ProcessTimeLog(); // reference pointer
            }
        }

        public DebugContextG Clone()
        {
            return new DebugContextG(this);
        }

        private void Clear()
        {
            this.dateTime = DateTime.Now;
            this.lotNo = "";
            this.patternId = -1;
            this.isSticker = false;
            this.regionId = -1;
            this.lineSetId = -1;
            this.lineId = -1;
            this.blobId = -1;
        }

        private void CopyFrom(DebugContextG debugContextG)
        {
            this.dateTime = debugContextG.dateTime;
            this.lotNo = debugContextG.lotNo;
            this.patternId = debugContextG.patternId;
            this.isSticker = debugContextG.isSticker;
            this.regionId = debugContextG.regionId;
            this.lineSetId = debugContextG.lineSetId;
            this.lineId = debugContextG.lineId;
            this.blobId = debugContextG.blobId;

            this.processTimeLog = debugContextG.processTimeLog;
        }

        private string GetFullPath()
        {
            List<string> fullPathList = new List<string>();
            fullPathList.Add(base.path);

            if (!string.IsNullOrEmpty(this.lotNo))
                fullPathList.Add(this.lotNo);

            if (!string.IsNullOrEmpty(this.frameId))
            {
                fullPathList.Add("Frames");
            }
            else if (this.isSticker)
            {
                fullPathList.Add("Sticker");
            }
            else
            {
                if (this.regionId >= 0)
                    fullPathList.Add(this.regionId.ToString());

                if (this.blobId >= 0)
                {
                    fullPathList.Add(string.Format("BLB{0}", this.blobId));
                }
                else
                {
                    if (this.lineSetId >= 0)
                        fullPathList.Add(this.lineSetId.ToString());

                    if (this.lineId >= 0)
                        fullPathList.Add(this.lineId.ToString());
                }
            }

            return Path.Combine(fullPathList.ToArray());
        }

        public override string ToString()
        {
            return this.processTimeLog.GetToolStripText();
        }
    }
}
