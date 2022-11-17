using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Gravure.Data
{
    public class ResultCollector : UniScanG.Data.ResultCollector
    {
        public AlgorithmResultG Collect(string path)
        {
            MergeSheetResult mergeSheetResult = new MergeSheetResult(-1, 1, path, false);

            string fileName = Path.Combine(path, AlgorithmResultG.CSVFileName);
            using (StreamReader reader = new StreamReader(fileName))
            {              
                mergeSheetResult.StartTime = DateTime.ParseExact(reader.ReadLine().Split(',')[1], AlgorithmResultG.DateTimeFormat, null);
                mergeSheetResult.EndTime = DateTime.ParseExact(reader.ReadLine().Split(',')[1], AlgorithmResultG.DateTimeFormat, null);
            }

            CalculatorResult cr = new CalculatorResult();
                cr.Import(path);

            DetectorResult dr = new DetectorResult();
                dr.Import(path);

            WatcherResult wr = new WatcherResult();
                wr.Import(path);

            mergeSheetResult.SpandTimes.Add(CalculatorBase.TypeName, cr.SpandTime);
            mergeSheetResult.SpandTimes.Add(Detector.TypeName, dr.SpandTime);
            mergeSheetResult.SpandTimes.Add(Watcher.TypeName, wr.SpandTime);
            mergeSheetResult.UpdateSpandTime();

            mergeSheetResult.PrevImage = cr.PrevImage;
            mergeSheetResult.OffsetStructSet[0] = cr.OffsetSet;
            mergeSheetResult.SheetSizePx = cr.SheetSizePx;
            mergeSheetResult.PartialProjection = new float[1][];
            mergeSheetResult.PartialProjection[0] = new float[cr.PartialProjections.Length];
            Array.Copy(cr.PartialProjections, mergeSheetResult.PartialProjection[0], cr.PartialProjections.Length);

            mergeSheetResult.SheetSize = dr.SheetSize;
            mergeSheetResult.PostProcessed = dr.PostProcessed;

            mergeSheetResult.SheetSubResultList.AddRange(cr.SheetSubResultList);
            mergeSheetResult.SheetSubResultList.AddRange(dr.SheetSubResultList);
            mergeSheetResult.SheetSubResultList.AddRange(wr.SheetSubResultList);

            for (int i = 0; i < mergeSheetResult.SheetSubResultList.Count; i++)
                mergeSheetResult.SheetSubResultList[i].Index = i;

            LogHelper.Debug(LoggerType.Inspection, string.Format("ResultCollector::Collect - Path: {0}, Judgement: {1}, SpandTime: {2:F0}", path, !mergeSheetResult.IsNG, mergeSheetResult.SpandTime.TotalMilliseconds));

            return mergeSheetResult;
        }


        public AlgorithmResultG CreateSheetResult(int index, int subCount, string path)
        {
            return new MergeSheetResult(index, subCount, path, false);
        }
    }
}
