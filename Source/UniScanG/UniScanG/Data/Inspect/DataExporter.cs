using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using DynMvp.Vision;
using UniScanG.Common.Data;
using DynMvp.Base;
using System.IO;

namespace UniScanG.Data.Inspect
{
    public class DataExporterG : DynMvp.Data.DataExporter
    {
        public static string CacheFileName = "Cache.csv";
        static System.IO.StreamWriter sw;
        static string swPath;

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //LogHelper.Error(LoggerType.Operation, string.Format("DataExporterG::Export - {0}", inspectionResult.ResultPath));
            //FileHelper.ClearFolder(inspectionResult.ResultPath);
            try
            {
                Directory.CreateDirectory(inspectionResult.ResultPath);

                string csvFile = Path.Combine(inspectionResult.ResultPath, AlgorithmResultG.CSVFileName);
                if (File.Exists(csvFile))
                    File.Delete(csvFile);

                using (StreamWriter writer = new StreamWriter(csvFile, false))
                {
                    writer.WriteLine($"StartTime,{inspectionResult.InspectionStartTime.ToString(AlgorithmResultG.DateTimeFormat)}");
                    writer.WriteLine($"EndTime,{inspectionResult.InspectionEndTime.ToString(AlgorithmResultG.DateTimeFormat)}");
                }

                foreach (KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
                {
                    string key = pair.Key;
                    IExportable exportable = pair.Value as IExportable;
                    exportable?.Export(inspectionResult.ResultPath, cancellationToken);
                }

                AppendChahe(inspectionResult);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("DataExporterG::Export - {0}, {1}", ex.Message, ex.StackTrace));
            }
        }

        private void AppendChahe(DynMvp.InspData.InspectionResult inspectionResult)
        {
            bool isContainsKey = inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName);
            if (!isContainsKey)
                return;

            string path = System.IO.Path.GetDirectoryName(inspectionResult.ResultPath);
            if (swPath != path)
            {
                sw?.Dispose();
                FileStream fs = new FileStream(System.IO.Path.Combine(path, DataExporterG.CacheFileName), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                sw = new System.IO.StreamWriter(fs) { AutoFlush = true };
                swPath = path;
            }

            lock (sw)
            {
                MergeSheetResult mergeSheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as MergeSheetResult;
                string str = mergeSheetResult.GetExportString();
                sw.WriteLine(str);
            }
        }
    }

    //public class InspectorDataExporterG : DynMvp.Data.DataExporter
    //{
    //    public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
    //    {
    //        FileHelper.ClearFolder(inspectionResult.ResultPath);
    //        foreach(KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
    //        {
    //            string key = pair.Key;
    //            IExportable exportable = pair.Value as IExportable;
    //            exportable?.Export(inspectionResult.ResultPath, cancellationToken);
    //        }
    //    }
    //}

    //public class MonitorDataExporterG : DynMvp.Data.DataExporter
    //{
    //    public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
    //    {
    //        FileHelper.ClearFolder(inspectionResult.ResultPath);
    //        foreach (KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
    //        {
    //            string key = pair.Key;
    //            IExportable exportable = pair.Value as IExportable;
    //            exportable?.Export(inspectionResult.ResultPath, cancellationToken);
    //        }
    //    }
    //}
}
