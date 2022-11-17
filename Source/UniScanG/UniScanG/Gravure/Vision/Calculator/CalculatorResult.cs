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
using UniEye.Base.Settings;
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
using UniScanG.Common.Data;
using UniScanG.Common;
using UniScanG.Gravure.Vision.RCI.Calculator;

namespace UniScanG.Gravure.Vision.Calculator
{
    public class CalculatorResult : AlgorithmResultG
    {
        public OffsetStructSet OffsetSet { get => offsetSet; set => offsetSet = value; }
        protected OffsetStructSet offsetSet;

        public float[] PartialProjections => this.partialProjections;
        float[] partialProjections;

        public ImageD DebugImageD { get; set; }

        public CalculatorResult() : base("CalculatorResult")
        {
            this.offsetSet = new OffsetStructSet(0);

            if (resultCollector == null)
                resultCollector = new UniScanG.Gravure.Data.ResultCollector();
        }

        public override void Copy(AlgorithmResultG sheetResult)
        {
            base.Copy(sheetResult);

            CalculatorResult calculatorResult = sheetResult as CalculatorResult;
            if (calculatorResult != null)
            {
                this.prevImage = (Bitmap)calculatorResult.prevImage.Clone();
            }
        }

        public void SetPartialProjection(PartialProjection partialProjection)
        {
            this.partialProjections = new float[partialProjection.Length];
            Array.Copy(partialProjection.Datas, this.partialProjections, partialProjection.Length);
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;

            SizeF pelSize = new SizeF(14, 14);
            if (SystemManager.Instance() != null)
                pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault().PelSize;

            // 시트 1장에 대한 검사 결과 저장.
            string fileName = Path.Combine(path, CSVFileName);
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.AppendLine(string.Format("CALCULATOR TEACH"));
            //stringBuilder.AppendLine(string.Format("Sensitivity,{0}", calculatorParam.MultiSensitivity));//티칭값

            //stringBuilder.AppendLine(string.Format("REGION INFO"));
            //stringBuilder.AppendLine(string.Format("X,RealX,Y,RealY,Width,RealWidth,Height,RealHeight,PoleAvg,DielectricAvg"));
            //SizeF scale = new SizeF(pelSize.Width / 1000.0f, pelSize.Height / 1000.0f);
            //foreach (RegionInfoG regionInfo in calculatorParam.RegionInfoList)
            //{
            //    Rectangle regionRect = regionInfo.Region;
            //    regionRect.Offset(this.offsetSet.PatternOffset.Offset);
            //    stringBuilder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
            //        regionRect.X, regionRect.X * scale.Width,
            //        regionRect.Y, regionRect.Y * scale.Height,
            //        regionRect.Width, regionRect.Width * scale.Width,
            //        regionRect.Height, regionRect.Height * scale.Height,
            //        regionInfo.PoleAvg, regionInfo.DielectricAvg
            //        ));
            //}

            if (this.offsetSet.LocalOffsets != null)
            {
                stringBuilder.AppendLine(string.Format("REGION OFFSET"));
                stringBuilder.AppendLine(string.Format("Count, {0}", this.offsetSet.LocalOffsets.Length));
                stringBuilder.AppendLine(OffsetStruct.GetExportHeader());
                for (int i = 0; i < this.offsetSet.LocalOffsets.Length; i++)
                {
                    OffsetStruct localOffset = this.offsetSet.LocalOffsets[i];
                    if (localOffset == null)
                        localOffset = new OffsetStruct();

                    localOffset.ImageD?.SaveImage(Path.Combine(path, string.Format("localOffset.{0:D02}.Jpg", i)));
                    stringBuilder.AppendLine(localOffset.GetExportData());
                }
            }

            stringBuilder.AppendLine(string.Format("CALCULATOR RESULT"));
            stringBuilder.AppendLine(string.Format("PatternOffset,{0},{1}", this.offsetSet.PatternOffset.OffsetF.X, this.offsetSet.PatternOffset.OffsetF.Y));//검사값
            this.offsetSet.PatternOffset.ImageD?.SaveImage(Path.Combine(path, "PatternOffset.Jpg"));

            stringBuilder.AppendLine(string.Format("SheetSize,{0},{1}", this.sheetSize.Width, this.sheetSize.Height));//검사값
            stringBuilder.AppendLine(string.Format("SheetSizePx,{0},{1}", this.sheetSizePx.Width, this.sheetSizePx.Height));//검사값
            stringBuilder.AppendLine(string.Format("StartTime,{0}", this.StartTime.ToString(AlgorithmResultG.DateTimeFormat)));//검사값
            stringBuilder.AppendLine(string.Format("SpandTime,{0}", this.spandTime));//검사값
            stringBuilder.AppendLine(string.Format("PartialProjections,{0}", string.Join(";", this.PartialProjections?.Select(f => f.ToString()))));//검사값

            stringBuilder.AppendLine(string.Format("CALCULATOR LIST"));
            stringBuilder.AppendLine(string.Format("Count, {0}", this.sheetSubResultList.Count));
            stringBuilder.AppendLine(DefectObj.GetExportHeader());

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
            int camIndex = client == null ? 0 : client.GetCamIndex();
            for (int i = 0; i < this.sheetSubResultList.Count; i++)
            {
                DefectObj subResult = (DefectObj)this.sheetSubResultList[i];
                subResult.Index = i;
                subResult.CamIndex = camIndex;
                string exportString = subResult.ToExportData();
                exportString = exportString.Replace('\t', ',');
                stringBuilder.AppendLine(exportString);
                ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("S{0}.jpg", i)));
            }

            File.AppendAllText(fileName, stringBuilder.ToString());
            //base.Export(path);

            if (prevImage != null)
            {
                Bitmap bitmap = null;
                // 저장이 오래걸려서 화면 업데이트중 [개채를 다른곳에서 사용중] 예외 발생.
                lock (prevImage)
                    bitmap = (Bitmap)prevImage.Clone();
                ImageHelper.SaveImage(bitmap, Path.Combine(path, string.Format("Prev.Jpg")));
            }
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, CSVFileName);
            if (File.Exists(fileName) == false)
                return false;

            string prevImageName = Path.Combine(path, "Prev.jpg");
            if (File.Exists(prevImageName) == true)
            {
                Bitmap image = (Bitmap)ImageHelper.LoadImage(prevImageName);
                if (image != null)
                {
                    if (image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                        this.prevImage = ImageHelper.MakeGrayscale(image);
                    else
                        this.prevImage = image;
                }
                image?.Dispose();
            }
            else
            {
                //LogHelper.Error(LoggerType.Error, string.Format("DetectorResult::Import - prevImage {0} is noe exist", imageName));
            }

            string basePointImageName = Path.Combine(path, "PatternOffset.jpg");
            if (File.Exists(basePointImageName) == true)
            {
                this.offsetSet.PatternOffset.ImageD = new Image2D(basePointImageName);
                //using (Bitmap image = (Bitmap)ImageHelper.LoadImage(basePointImageName))
                //{
                //    if (image != null)
                //    {
                //        if (image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                //            this.patternOffset.ImageD = (ImageD)Image2D.ToImage2D(ImageHelper.MakeGrayscale(image));
                //        else
                //            this.patternOffset.ImageD = (ImageD)Image2D.ToImage2D(image);
                //    }
                //}
            }

            try
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                int startIdxResult = lines.FindIndex(f => f == "CALCULATOR RESULT");
                if (startIdxResult >= 0)
                {

                    this.offsetSet.PatternOffset.IsGood = true;
                    this.offsetSet.PatternOffset.OffsetF = new Point(
                        int.Parse(lines[startIdxResult + 1].Split(',')[1]),
                        int.Parse(lines[startIdxResult + 1].Split(',')[2]));

                    this.sheetSize = new SizeF(
                        float.Parse(lines[startIdxResult + 2].Split(',')[1]),
                        float.Parse(lines[startIdxResult + 2].Split(',')[2]));

                    this.sheetSizePx = new Size(
                        int.Parse(lines[startIdxResult + 3].Split(',')[1]),
                        int.Parse(lines[startIdxResult + 3].Split(',')[2]));

                    this.StartTime = DateTime.ParseExact(lines[startIdxResult + 4].Split(',')[1], AlgorithmResultG.DateTimeFormat, null, System.Globalization.DateTimeStyles.None);
                    this.spandTime = TimeSpan.Parse(lines[startIdxResult + 5].Split(',')[1]);

                    string[] tokens = lines.ElementAtOrDefault(startIdxResult + 6)?.Split(',');
                    if (tokens != null && tokens[0] == "PartialProjections")
                    {
                        this.partialProjections = tokens[1].Split(';').Select(f =>
                        {
                            if (float.TryParse(f, out float single))
                                return single;
                            return -1;
                        }).ToArray();
                    }
                    else
                    {
                        this.partialProjections = new float[0];
                    }
                }

                int startIdxOffset = lines.FindIndex(f => f == "REGION OFFSET");
                if (startIdxOffset >= 0)
                {
                    string[] token = lines[startIdxOffset + 1].Split(',');
                    int count = int.Parse(token[1]);
                    this.offsetSet.Resize(count);
                    for (int i = 0; i < count; i++)
                    {
                        //string[] tokens = lines[startIdxOffset + 3 + i].Split(',');
                        //bool isGood = bool.Parse(tokens[0]);
                        //int x = int.Parse(tokens[1]);
                        //int y = int.Parse(tokens[2]);
                        //float score = float.Parse(tokens[3]);

                        //Point offset = new Point(x, y);
                        //string localPointImageName = Path.Combine(path, string.Format("localOffset.{0:D02}.Jpg", i));
                        //ImageD localPointImage = File.Exists(localPointImageName) ? new Image2D(localPointImageName) : null;
                        //this.offsetSet.LocalOffsets[i] = new OffsetStruct(isGood, new Point(x, y), Size.Empty, score, localPointImage);

                        this.offsetSet.LocalOffsets[i] = OffsetStruct.FromExportData(lines[startIdxOffset + 3 + i]);
                        string localPointImageName = Path.Combine(path, string.Format("localOffset.{0:D02}.Jpg", i));
                        this.offsetSet.LocalOffsets[i].ImageD = File.Exists(localPointImageName) ? new Image2D(localPointImageName) : null;
                    }
                }

                int startIdxSticker = lines.FindIndex(f => f == "CALCULATOR LIST");
                if (startIdxSticker >= 0)
                {
                    string[] token = lines[startIdxSticker + 1].Split(',');
                    int count = int.Parse(token[1]);
                    for (int i = 0; i < count; i++)
                    {
                        Data.DefectObj ssr = new Data.DefectObj(false);
                        bool ok = ssr.FromExportData(lines[startIdxSticker + i + 3]);
                        if (!ok)
                            break;

                        ssr.ImagePath = Path.Combine(path, string.Format("S{0}.jpg", ssr.Index));
                        this.sheetSubResultList.Add(ssr);
                    }
                }

                this.good = this.sheetSubResultList.Count == 0;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("CalculatorResult::Import - {0}", ex.Message));
                return false;
            }
        }
    }
}
