using System;
using System.Collections;
using System.Collections.Generic;

namespace UniScanM.Gloss.Data
{
    public class GlossScanData : IEnumerable
    {
        #region 생성자
        public GlossScanData(float targetValue = 0)
        {
            StartTime = DateTime.Now;
            MinGloss = targetValue;
            MaxGloss = targetValue;
            AvgGloss = targetValue;
        }
        #endregion

        #region 열거형
        IEnumerator IEnumerable.GetEnumerator() { return GlossDatas.GetEnumerator(); }
        #endregion

        #region 속성
        public float MinGloss { get; set; } = 0;

        public float MaxGloss { get; set; } = 0;

        public float AvgGloss { get; set; } = 0;

        public float DevGloss { get; set; } = 0;

        public float MinDistance { get; set; } = 0;

        public float MaxDistance { get; set; } = 0;

        public float AvgDistance { get; set; } = 0;

        public float DevDistance { get; set; } = 0;

        public float RollPosition { get; set; } = 0;

        public List<GlossData> GlossDatas { get; set; } = new List<GlossData>();

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Count => GlossDatas.Count;
        #endregion

        #region 인덱서
        public GlossData this[int i] { get => GlossDatas[i]; set => GlossDatas[i] = value; }
        #endregion

        #region 메서드
        public void Clear()
        {
            MinGloss = 0;
            MaxGloss = 0;
            AvgGloss = 0;
            DevGloss = 0;
            MinDistance = 0;
            MaxDistance = 0;
            AvgDistance = 0;
            DevDistance = 0;
            RollPosition = 0;
            GlossDatas.Clear();
        }

        public GlossScanData Clone()
        {
            GlossScanData scanData = new GlossScanData();

            scanData.StartTime = this.StartTime;
            scanData.EndTime = this.EndTime;
            scanData.MinGloss = this.MinGloss;
            scanData.MaxGloss = this.MaxGloss;
            scanData.AvgGloss = this.AvgGloss;
            scanData.DevGloss = this.DevGloss;
            scanData.MinDistance = this.MinDistance;
            scanData.MaxDistance = this.MaxDistance;
            scanData.AvgDistance = this.AvgDistance;
            scanData.DevDistance = this.DevDistance;
            scanData.RollPosition = this.RollPosition;

            foreach (var glossData in GlossDatas)
                scanData.GlossDatas.Add(glossData.Clone());

            return scanData;
        }
        #endregion
    }
}