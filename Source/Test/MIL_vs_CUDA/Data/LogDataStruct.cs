using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIL_vs_CUDA.Data
{
    public struct LogDataStruct
    {
        public bool IsStart { get => isStart; }
        bool isStart;

        public DateTime Datetime { get => datetime; }
        DateTime datetime;

        public string Message { get => message; }
        string message;

        public double SpendTimeMs { get => spendTimeMs; }
        double spendTimeMs;

        public double TotalSpendTimeMs { get => totalSpendTimeMs; }
        double totalSpendTimeMs;

        public LogDataStruct(DateTime datetime, string message, double spendTimeMs, double totalSpendTimeMs)
        {
            this.isStart = false;
            this.datetime = datetime;
            this.message = message;
            this.spendTimeMs = spendTimeMs;
            this.totalSpendTimeMs = totalSpendTimeMs;
        }

        public LogDataStruct(DateTime datetime, string message, double spendTimeMs, double totalSpendTimeMs, bool isStartLog)
        {
            this.isStart = isStartLog;
            this.datetime = datetime;
            this.message = message;
            this.spendTimeMs = spendTimeMs;
            this.totalSpendTimeMs = totalSpendTimeMs;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", datetime.ToString("yyyy.MM.dd HH:mm:ss.fff"), message, spendTimeMs, totalSpendTimeMs);
        }
    }
}
