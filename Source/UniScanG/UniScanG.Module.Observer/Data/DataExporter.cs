using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Module.Observer.Vision;

namespace UniScanG.Module.Observer.Data
{
    public class DataExporter
    {
        enum DataExporterHeader { DATETIME, TITLE, MARGIN_LEFT, MARGIN_TOP, MARGIN_RIGHT, MARGIN_BOTTOM, COUNT_MAX }
        public void ExportData(string resultPath, string fileName, MarginCheckerResult marginCheckerResult)
        {
            if (Directory.Exists(resultPath) == false)
                Directory.CreateDirectory(resultPath);

            string file = Path.Combine(resultPath, fileName);
            if (File.Exists(file) == false)
            {
                bool ok = AppendHeader(file);
                if (ok == false)
                    return;
            }

            string[] tokens = new string[(int)DataExporterHeader.COUNT_MAX];
            tokens[(int)DataExporterHeader.DATETIME] = marginCheckerResult.InspectStartTime.ToString("yyyy.MM.dd HH:mm:ss");
            tokens[(int)DataExporterHeader.TITLE] = marginCheckerResult.Title;
            tokens[(int)DataExporterHeader.MARGIN_LEFT] = marginCheckerResult.RealMargin.Left.ToString("F0");
            tokens[(int)DataExporterHeader.MARGIN_TOP] = marginCheckerResult.RealMargin.Top.ToString("F0");
            tokens[(int)DataExporterHeader.MARGIN_RIGHT] = marginCheckerResult.RealMargin.Right.ToString("F0");
            tokens[(int)DataExporterHeader.MARGIN_BOTTOM] = marginCheckerResult.RealMargin.Bottom.ToString("F0");

            string concat = string.Join(",", tokens);
            try
            {
                File.AppendAllText(file, string.Format("{0}\r\n", concat));
            }
            catch (IOException) { }
            }

        private bool AppendHeader(string file)
        {
            string[] tokens = new string[(int)DataExporterHeader.COUNT_MAX];
            for (int i = 0; i < (int)DataExporterHeader.COUNT_MAX; i++)
                tokens[i] = ((DataExporterHeader)i).ToString();

            string join = string.Format("{0}\r\n", string.Join(",", tokens));
            try
            {
                File.WriteAllText(file, join);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
