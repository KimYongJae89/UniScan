using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Extender
{
    public class WatcherResult : AlgorithmResultG
    {
        public WatcherResult() : base("WatcherResult") { }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance();

            // 시트 1장에 대한 검사 결과 저장.
            string fileName = Path.Combine(path, CSVFileName);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("WATCHER PARMA"));
            stringBuilder.AppendLine(string.Format("Offset,{0}", this.OffsetFound.Width, this.OffsetFound.Height));//티칭값

            stringBuilder.AppendLine(string.Format("WATCHER RESULT"));
            stringBuilder.AppendLine(string.Format("StartTime,{0}", this.StartTime.ToString(AlgorithmResultG.DateTimeFormat)));//검사값
            stringBuilder.AppendLine(string.Format("SpandTime,{0}", this.SpandTime));//검사값

            stringBuilder.AppendLine(string.Format("WATCHER LIST"));
            stringBuilder.AppendLine(string.Format("Count,{0}", this.sheetSubResultList.Count));//티칭값
            stringBuilder.AppendLine(FoundedObjInPattern.GetExportHeader());

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
            int camIndex = client == null ? 0 : client.GetCamIndex();
            for (int i = 0; i < this.sheetSubResultList.Count; i++)
            {
                FoundedObjInPattern subResult = (FoundedObjInPattern)this.sheetSubResultList[i];
                subResult.Index = i;
                subResult.CamIndex = camIndex;
                string exportString = subResult.ToExportData();
                exportString = exportString.Replace('\t', ',');
                stringBuilder.AppendLine(exportString);
            }
            File.AppendAllText(fileName, stringBuilder.ToString());

            //Parallel.For(0, SheetSubResultList.Count, idx =>
            for (int i = 0; i < this.sheetSubResultList.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                FoundedObjInPattern subResult = this.sheetSubResultList[i];
                if (subResult.Image != null)
                {
                    lock (subResult.Image)
                        //ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("V{0}.jpg", i)));
                        ImageHelper.SaveImage(subResult.Image, Path.Combine(path, subResult.ImageFileName));
                }
            }
            //);
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, CSVFileName);
            if (File.Exists(fileName) == false)
                return false;

            List<string> lines = File.ReadAllLines(fileName).ToList();

            int startIdxResult = lines.FindIndex(f => f == "WATCHER RESULT");
            if (startIdxResult >= 0)
            {
                this.StartTime = DateTime.ParseExact(lines[startIdxResult + 1].Split(',')[1], AlgorithmResultG.DateTimeFormat, null, System.Globalization.DateTimeStyles.None);
                this.spandTime = TimeSpan.Parse(lines[startIdxResult + 2].Split(',')[1]);
            }

            int startIdxList = lines.FindIndex(f => f == "WATCHER LIST");
            if (startIdxList >= 0)
            {
                string[] token = lines[startIdxList + 1].Split(',');
                int count = int.Parse(token[1]);
                for (int i = 0; i < count; i++)
                {
                    string str = lines[startIdxList + i + 3];
                    FoundedObjInPattern obj = FoundedObjInPattern.Create(str.Split(',').FirstOrDefault());
                    obj.FromExportData(str);

                    //string imageFile = Path.Combine(path, string.Format("V{0}.jpg", i));
                    string imageFile = Path.Combine(path, obj.ImageFileName);
                    if(File.Exists(imageFile))
                    {
                        Bitmap image = (Bitmap)ImageHelper.LoadImage(imageFile);
                        if (image != null)
                        {
                                obj.Image = (Bitmap)image.Clone();
                            //if (image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                            //    obj.Image = ImageHelper.MakeGrayscale(image);
                            //else
                            //    obj.Image = (Bitmap)image.Clone();
                        }
                        image?.Dispose();
                    }

                    this.sheetSubResultList.Add(obj);
                }
            }

            return true;
        }
    }

    
}
