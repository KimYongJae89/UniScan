using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
//using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScanG.Common.Settings;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Detector
{
    public class DetectorResult : AlgorithmResultG
    {
        public DetectorResult() : base("DetectorResult")
        {
            this.StartTime = DateTime.Now;
            if (resultCollector == null)
                resultCollector = new Gravure.Data.ResultCollector();
        }

        //public override void Union(AlgorithmResultG sheetResult)
        //{
        //    base.Union(sheetResult);

        //    if (sheetResult is DetectorResult)
        //        this.postProcessed |= sheetResult.PostProcessed;
        //}

        public override void Export(string path, CancellationToken cancellationToken)
        {
            DetectorParam detectorParam = Detector.DetectorParam;
            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance();

            // 시트 1장에 대한 검사 결과 저장.
            string fileName = Path.Combine(path, CSVFileName);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("DETECTION TEACH"));
            stringBuilder.AppendLine(string.Format("MaximumDefectCount,{0}", detectorParam.MaximumDefectCount));//티칭값
            stringBuilder.AppendLine(string.Format("MinBlackDefectArea,{0}", detectorParam.MinBlackDefectLength));//티칭값
            stringBuilder.AppendLine(string.Format("MinWhiteDefectArea,{0}", detectorParam.MinWhiteDefectLength));//티칭값
            stringBuilder.AppendLine(string.Format("Offset,{0},{1}", this.offsetFound.Width, this.offsetFound.Height));//티칭값

            stringBuilder.AppendLine(string.Format("DETECTION RESULT"));
            stringBuilder.AppendLine(string.Format("StartTime,{0}", this.StartTime.ToString(AlgorithmResultG.DateTimeFormat)));//검사값
            stringBuilder.AppendLine(string.Format("SpandTime,{0}", this.spandTime));//검사값
            stringBuilder.AppendLine(string.Format("SheetSize,{0},{1}", this.sheetSize.Width, sheetSize.Height));//검사값
            stringBuilder.AppendLine(string.Format("SheetSizePx,{0},{1}", this.sheetSizePx.Width, sheetSizePx.Height));//검사값
            stringBuilder.AppendLine(string.Format("DefectCount,{0}", this.sheetSubResultList.Count));//검사값
            stringBuilder.AppendLine(string.Format("LaserBurned,{0}", this.postProcessed));//검사값

            stringBuilder.AppendLine(string.Format("DETECTION LIST"));
            stringBuilder.AppendLine(DefectObj.GetExportHeader());

            //IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
            //int camIndex = client == null ? 0 : client.GetCamIndex();
            for (int i = 0; i < this.sheetSubResultList.Count; i++)
            {
                DefectObj subResult = (DefectObj)SheetSubResultList[i];
                //subResult.Index = i;
                //subResult.CamIndex = camIndex;
                string exportString = subResult.ToExportData();
                exportString = exportString.Replace('\t', ',');
                stringBuilder.AppendLine(exportString);
            }
            File.AppendAllText(fileName, stringBuilder.ToString());

            for (int i = 0; i < SheetSubResultList.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Gravure.Data.DefectObj subResult = (Gravure.Data.DefectObj)SheetSubResultList[i];
                lock (subResult.Image)
                {
                    ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("D{0}.jpg", subResult.Index)));
                    ImageHelper.SaveImage(subResult.BufImage, Path.Combine(path, string.Format("D{0}B.jpg", subResult.Index)));
                }
            }
            //);
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, CSVFileName);

            if (File.Exists(fileName) == false)
                return false;

            try
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                int startIdxTeach = lines.FindIndex(f => f == "DETECTION TEACH");
                if (startIdxTeach >= 0)
                {
                    //DetectorParam detectorParam = new DetectorParam();
                    //detectorParam.MaximumDefectCount = int.Parse(lines[startIdxTeach + 1].Split(',')[1]);
                    //detectorParam.MinBlackDefectLength = int.Parse(lines[startIdxTeach + 2].Split(',')[1]);
                    //detectorParam.MinWhiteDefectLength = int.Parse(lines[startIdxTeach + 3].Split(',')[1]);

                    string[] offsetFoundedToken = lines[startIdxTeach + 4].Split(',');
                    this.offsetFound.Width = int.Parse(offsetFoundedToken[1]);
                    this.offsetFound.Height = int.Parse(offsetFoundedToken[2]);
                }

                int startIdxResult = lines.FindIndex(f => f == "DETECTION RESULT");
                if (startIdxResult >= 0)
                {
                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        string[] tokens = lines[startIdxResult + i + 1].Split(',');
                        if (/*tokens.Length == 2 && */tokens[0] == "SpandTime")
                        {
                            this.spandTime = TimeSpan.Parse(tokens[1]);
                        }
                        else if (/*tokens.Length == 3 &&*/ tokens[0] == "SheetSize")
                        {
                            this.sheetSize.Width = float.Parse(tokens[1]);
                            this.sheetSize.Height = float.Parse(tokens[2]);
                        }
                        else if (/*tokens.Length == 3 &&*/ tokens[0] == "SheetSizePx")
                        {
                            this.sheetSizePx.Width = int.Parse(tokens[1]);
                            this.sheetSizePx.Height = int.Parse(tokens[2]);
                        }
                        else if (/*tokens.Length == 2 &&*/ tokens[0] == "DefectCount")
                        {
                            int size = int.Parse(tokens[1]);
                            this.SheetSubResultList = new List<UniScanG.Data.FoundedObjInPattern>(size);
                        }
                        else if (/*tokens.Length == 2 &&*/ tokens[0] == "LaserBurned")
                        {
                            this.postProcessed = bool.Parse(tokens[1]);
                        }
                        else if (/*tokens.Length == 2 &&*/ tokens[0] == "StartTime")
                        {
                            this.StartTime = DateTime.ParseExact(tokens[1], AlgorithmResultG.DateTimeFormat, null, System.Globalization.DateTimeStyles.None);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                int startIdxList = lines.FindIndex(f => f == "DETECTION LIST");
                if (startIdxList >= 0)
                {
                    for(int i=0; i< this.SheetSubResultList.Capacity; i++)
                    {
                        int lineNo = startIdxList + i + 2;
                        if (lineNo >= lines.Count)
                            break;

                        string line = lines[lineNo];
                        try
                        {
                            Data.DefectObj ssr = new Data.DefectObj(false);
                            bool ok = ssr.FromExportData(line);
                            if (ok == false)
                                break;

                            ssr.ImagePath = Path.Combine(path, string.Format("D{0}.jpg", ssr.Index));
                            if (!File.Exists(ssr.ImagePath))
                                ssr.ImagePath = Path.Combine(path, string.Format("{0}.jpg", ssr.Index));

                            ssr.BufImagePath = Path.Combine(path, string.Format("D{0}B.jpg", ssr.Index));
                            if (!File.Exists(ssr.BufImagePath))
                                ssr.BufImagePath = Path.Combine(path, string.Format("{0}B.jpg", ssr.Index));

                            this.sheetSubResultList.Add(ssr);
                        }
                        catch
                        {
                            break;
                        }
                    }
                    //int index = 0;
                    //while (startIdxList < lines.Count)
                    //{
                    //    index++;
                    //    int lineNo = startIdxList + index + 1;
                    //    if (lineNo >= lines.Count)
                    //        break;

                    //    string line = lines[startIdxList + index + 1];
                    //    string[] token = line.Split(new char[] { ',', '\t' });
                    //    try
                    //    {
                    //        Data.SheetSubResult ssr = new Data.SheetSubResult();
                    //        bool ok = ssr.FromExportData(line);

                    //        if (ok == false)
                    //            break;

                    //        this.sheetSubResultList.Add(ssr);
                    //    }
                    //    catch
                    //    {
                    //        break;
                    //    }
                    //}

                    //Parallel.ForEach(this.sheetSubResultList, f =>
                    //{
                    //    f.ImagePath = Path.Combine(path, string.Format("D{0}.jpg", f.Index));
                    //    if (!File.Exists(f.ImagePath))
                    //        f.ImagePath = Path.Combine(path, string.Format("{0}.jpg", f.Index));

                    //    f.BufImagePath = Path.Combine(path, string.Format("D{0}B.jpg", f.Index));
                    //    if (!File.Exists(f.BufImagePath))
                    //        f.BufImagePath = Path.Combine(path, string.Format("{0}B.jpg", f.Index));
                    //});

                    this.good = this.sheetSubResultList.Count == 0;
                }
            }
            catch { }
            return true;
        }
    }
}
