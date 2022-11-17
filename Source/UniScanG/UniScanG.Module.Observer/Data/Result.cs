using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Module.Observer.Data
{
    internal class Result
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime InspectTime { get; set; } = DateTime.Now;
        public string Lot { get; set; } = "";
        public int No { get; set; } = -1;

        public Dictionary<string, Bitmap> List { get; } = new Dictionary<string, Bitmap>();

        public Result(Result prevResult)
        {
            this.InspectTime = DateTime.Now;

            if (prevResult == null)
            {
                this.StartTime = this.InspectTime;
                this.Lot = "";
                this.No = 0;
            }
            else
            {
                this.StartTime = prevResult.StartTime;
                this.Lot = prevResult.Lot;
                this.No = prevResult.No + 1;
            }
        }

        public string GetPath()
        {
            string timePath = this.StartTime.ToString("yyyy_MM_dd");
            string lotPath = $"{this.StartTime.ToString("HH_mm")}_{this.Lot}";
            return Path.Combine(timePath, lotPath);
        }

    }
}
