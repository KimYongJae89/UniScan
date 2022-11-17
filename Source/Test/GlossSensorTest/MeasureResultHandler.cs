using GlossSensorTest.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlossSensorTest
{
    public class MeasureResultHandler
    {
        static MeasureResultHandler instance = null;
        public static MeasureResultHandler Instance => instance ?? (instance = new MeasureResultHandler());

        private IMeasureSource _measureSource;

        ICollection<MeasureInfo> _measures = new List<MeasureInfo>();
        public IEnumerable<MeasureInfo> Measures => _measures;

        public void Initialize(IMeasureSource measureSource)
        {
            _measureSource = measureSource;
            _measureSource?.LoadMeasures(_measures);
        }

        public bool AddMeasure(MeasureInfo measure)
        {
            _measures.Add(measure);

            return true;
        }

        public bool RemoveMeasure(MeasureInfo measure)
        {
            _measures.Remove(measure);

            return true;
        }


        public void Save()
        {
            _measureSource?.SaveMeasures(_measures);
        }

        public void Load()
        {
            _measureSource?.LoadMeasures(_measures);
        }

        public List<MeasureInfo> GetMeasures(DateTime startDate, DateTime endDate)
        {
            List<MeasureInfo> measureInfos = new List<MeasureInfo>();

            foreach (MeasureInfo measure in _measures)
            {
                if (measure.MeasureDate >= startDate - TimeSpan.FromDays(1) && measure.MeasureDate <= endDate)
                    measureInfos.Add(measure);
            }

            return measureInfos;
        }
    }

    public class MeasureInfo : ICloneable
    {
        [Key]
        public DateTime MeasureDate { get; set; }
        public string SampleName { get; set; }
        public string Measure { get; set; }
        public string Distance { get; set; }
        public string Tilting { get; set; }

        public MeasureInfo() { }

        public MeasureInfo(string measure, DateTime measureDate, string sampleName, string distance, string tilting = "")
        {
            this.MeasureDate = measureDate;
            this.Measure = measure;
            this.SampleName = sampleName;
            this.Distance = distance;
            this.Tilting = tilting;
        }

        public object Clone()
        {
            var clone = new MeasureInfo();
            var properties = typeof(MeasureInfo).GetProperties();
            foreach (var prop in properties)
                prop.SetValue(clone, prop.GetValue(this));

            return clone;
        }
    }
}
