using AutoParamTuner.Base;
using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Vision;

namespace AutoParamTuner.Tuner
{
    class Inspecter
    {
        Control parent;

        public Inspecter(Control parent)
        {
            this.parent = parent;
        }

        public TunerResult Inspect(UniScanG.Data.Model.Model model, Image2D image, Calibration calibration)
        {
            model.CalculatorModelParam.UseMultiData = false;

            AlgoImage fullImage = null, scaleImage = null;
            ProcessBufferSetG bufferSet = null;

            try
            {
                Algorithm[] algorithms = new Algorithm[3]
                {
                    AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName),
                    AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName),
                    AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName)
                };

                if (!Array.TrueForAll(algorithms, f => f != null))
                    throw new InvalidDataException(StringManager.GetString("Algorithm is Not Ready."));

                bool isMultiLayerBuffer = false;
                float scaleFactorF = model.ScaleFactorF;

                DebugContextG debugContextG = new DebugContextG(new DebugContext(false, PathSettings.Instance().Temp));
                debugContextG.LotNo = "ModellerPageTestInspect";

                Exception bufferInitEx = null;
                new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready")).Show(parent, () =>
                {
                    try
                    {
                        fullImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image, ImageType.Grey);
                        LogHelper.Debug(LoggerType.Inspection, string.Format("ModellerPageExtenderG::Inspect - fullImageSize: {0}", fullImage.Size));

                        //fullImage = ImageBuilder.Build(Calculator.TypeName, currentImage);

                        CalculatorParam calculatorParam = algorithms[0].Param as CalculatorParam;
                        DetectorParam detectorParam = algorithms[1].Param as DetectorParam;
                        WatcherParam watcherParam = algorithms[2].Param as WatcherParam;

                        bufferSet = ((CalculatorBase)algorithms[0]).CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, fullImage.Width, fullImage.Height);
                        bufferSet.BuildBuffers(false);
                        if (!bufferSet.IsBuilded)
                            throw new Exception("Buffer Build Fail");
                        Array.ForEach(algorithms, f => f.PrepareInspection());
                    }
                    catch (Exception ex)
                    {
                        bufferInitEx = ex;
                        LogHelper.Error(LoggerType.Error, string.Format("Exception in ModellerPageExtenderG::Inspect - {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
                    }
                });

                if (bufferInitEx != null)
                {
                    bufferSet.Dispose();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(StringManager.GetString("Buffer Initialize Fail."));
                    sb.AppendLine(bufferInitEx.Message);
                    throw new InvalidDataException(sb.ToString());
                }

                InspectionResult inspectionResult = new UniScanG.Data.Inspect.InspectionResult();
                inspectionResult.InspectionStartTime = DateTime.Now;

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                Stopwatch sw = new Stopwatch();
                SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));
                Exception exception = null;
                inspectorForm.Show(parent, new Action(() =>
                {
                    try
                    {
                        sw.Start();

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Upload"));
                        Stopwatch swUpload = Stopwatch.StartNew();
                        bufferSet.Upload(fullImage, debugContextG);
                        inspectionResult.SetOffsetStruct(bufferSet.OffsetStructSet);
                        swUpload.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Upload: {0}[ms]", swUpload.ElapsedMilliseconds));
                        SheetInspectParam inspectParam = new SheetInspectParam(model, bufferSet, calibration, debugContextG);

                        inspectParam.TestInspect = true;
                        inspectParam.CancellationToken = cancellationTokenSource.Token;

                        string algorithmTypeCalc = algorithms[0].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeCalc));
                        Stopwatch swCalc = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeCalc, algorithms[0].Inspect(inspectParam));
                        swCalc.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeCalc, swCalc.ElapsedMilliseconds));

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Download"));
                        Stopwatch swDownload = Stopwatch.StartNew();
                        bufferSet.Download();
                        swDownload.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Download: {0}[ms]", swDownload.ElapsedMilliseconds));

                        string algorithmTypeDetect = algorithms[1].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeDetect));
                        Stopwatch swDetect = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeDetect, algorithms[1].Inspect(inspectParam));
                        swDetect.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeDetect, swDetect.ElapsedMilliseconds));

                        string algorithmTypeWatch = algorithms[2].GetAlgorithmType();
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, algorithmTypeWatch));
                        Stopwatch swWatch = Stopwatch.StartNew();
                        inspectionResult.AlgorithmResultLDic.Add(algorithmTypeWatch, algorithms[2].Inspect(inspectParam));
                        swWatch.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - {0}: {1}[ms]", algorithmTypeWatch, swWatch.ElapsedMilliseconds));

                        sw.Stop();
                        DynMvp.ConsoleEx.WriteLine(string.Format("ModellerPageExtenderG::Inspect - Total: {0}[ms]", sw.ElapsedMilliseconds));
                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Generate Preview"));
                        
                        //if (false && this.targetRegionInfo != null)
                        //{
                        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(bufferSet.AlgoImage);
                        //    Rectangle rect = this.targetRegionInfo.Region;

                        //    using (AlgoImage subAlgoImage = bufferSet.AlgoImage.Clip(rect))
                        //    {
                        //        //subAlgoImage.Save(@"C:\temp\AlgoImage.bmp");

                        //        using (AlgoImage subImage = bufferSet.EdgeMapImage.Clip(rect))
                        //        {
                        //            //subImage.Save(@"C:\temp\EdgeMapImage.bmp");
                        //            ip.Clipping(subImage, subImage, 0, 0, 1, 50);
                        //            //subImage.Save(@"C:\temp\EdgeMapImage2.bmp");
                        //            ip.Add(subAlgoImage, subImage, subAlgoImage);
                        //        }

                        //        using (AlgoImage subImage = bufferSet.CalculatorResultGray.GetSubImage(rect))
                        //        {
                        //            //subImage.Save(@"C:\temp\CalculatorResultGray.bmp");
                        //            ip.Add(subAlgoImage, subImage, subAlgoImage);
                        //        }

                        //        using (AlgoImage subImage = bufferSet.CalculatorResultBinal.GetSubImage(rect))
                        //        {
                        //            //subImage.Save(@"C:\temp\CalculatorResultBinal.bmp");
                        //            ip.Add(subAlgoImage, subImage, subAlgoImage);
                        //        }

                        //        subAlgoImage.Save(@"C:\temp\AddImage.bmp");
                        //    }
                        //}
                        bufferSet.Clear();

                        inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Done"));
                        inspectionResult.InspectionEndTime = DateTime.Now;
                        inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;
                        inspectionResult.UpdateJudgement();

                        Thread.Sleep(1000);
                    }
                    catch (OperationCanceledException) { }
                    catch(Exception ex)
                    {
                        exception = ex;
                    }
                }), cancellationTokenSource);

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    SimpleProgressForm waitForm = new SimpleProgressForm();
                    waitForm.Show(() => inspectorForm.Task.Wait());
                    return null;
                }

                Array.ForEach(algorithms, f => f.ClearInspection());
                LogHelper.Debug(LoggerType.Inspection, string.Format("InsepctionTime {0} {1}", debugContextG.PatternId, debugContextG.ProcessTimeLog.GetData()));

                if (exception != null)
                {
                    StringBuilder sb = new StringBuilder();
                    do
                    {
                        sb.AppendLine(exception.Message);
                    } while ((exception = exception.InnerException) != null);
                    MessageBox.Show(sb.ToString());
                }
                return new TunerResult(inspectionResult);
            }
            finally
            {
                scaleImage?.Dispose();
                fullImage?.Dispose();
                bufferSet?.Dispose();
            }
        }
    }
}
