using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Vision;

namespace MIL_vs_CUDA.Processer
{
    public enum ProcesserType { MIL, EMGU }

    internal class Processer
    {
        public AddLogDelegate OnLogAdded = null;

        public bool SaveResultImage { get => this.saveResultImage; set => saveResultImage = value; }
        bool saveResultImage = false;

        public bool IsBusy { get => isBusy; }
        bool isBusy = false;

        public Processer()
        {
        }

        public ProcessOutput Process(ProcessInput processInput)
        {
            this.isBusy = true;

            ProcessOutput processOutput = new ProcessOutput(processInput.Name);
            AlgoImage algoImage = null;
            UniScanG.Data.Inspect.InspectionResult inspectionResult = new UniScanG.Data.Inspect.InspectionResult();

            try
            {
                AddLog(processOutput, LogItemType.Start, string.Format("{0} - Process Started", processInput.Name), true);

                Image2D image2D = processInput.Image2D;
                algoImage = BuildImage(image2D, processInput.ProcesserType);
                ProcessBufferSetG bufferSet = processInput.ProcessBufferSet;
                if(bufferSet==null)
                {
                    AddLog(processOutput, LogItemType.Error, "Buffer Check Fail");
                    return processOutput;
                }
                AddLog(processOutput, LogItemType.Build, "Image Builded");

                DebugContext debugContext = new DebugContext();
                bufferSet.Upload(algoImage, debugContext);
                AddLog(processOutput, LogItemType.Transfer, "Image Uploaded");

                SheetInspectParam inspectParam = new SheetInspectParam(null, bufferSet, null, debugContext);
                inspectParam.TestInspect = true;

                Algorithm calculator = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
                bool parallel = ((CalculatorParam)(calculator.Param)).UseMultiThread;
                if (processInput.UseParallel != System.Windows.Forms.CheckState.Indeterminate)
                    ((CalculatorParam)(calculator.Param)).UseMultiThread = processInput.UseParallel == System.Windows.Forms.CheckState.Checked;

                CalculatorResult calculatorResult = (CalculatorResult)calculator.Inspect(inspectParam);
                inspectionResult.AlgorithmResultLDic.Add(calculator.GetAlgorithmType(), calculatorResult);
                ((CalculatorParam)(calculator.Param)).UseMultiThread = parallel;
                AddLog(processOutput, LogItemType.Process_Calc, "Calculated");

                bufferSet.Download();
                AddLog(processOutput, LogItemType.Transfer, "Image Downloaded");

                Algorithm detector = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);
                DetectorResult detectorResult = (DetectorResult)detector.Inspect(inspectParam);
                inspectionResult.AlgorithmResultLDic.Add(detector.GetAlgorithmType(), detectorResult);
                processOutput.DetectsCount = detectorResult == null ? 0 : detectorResult.SheetSubResultList.Count;
                AddLog(processOutput, LogItemType.Process_Detect, "Detected");

                bufferSet.WaitDone();
                processOutput.InspectionResult = inspectionResult;

                if (this.saveResultImage)
                {
                    bufferSet.CalculatorResultGray.Save(@"C:\temp\MIL_vs_CUDA.bmp");
                    AddLog(processOutput, LogItemType.Save, @"Save Done (C:\temp\MIL_vs_CUDA.bmp)");
                }

                algoImage.Dispose();
                AddLog(processOutput, LogItemType.Dispose, "Image Disposed");
            }
            catch (Exception ex)
            {
                AddLog(processOutput, LogItemType.End, string.Format("Exception Occur! {0}", ex.Message));
            }
            finally
            {
                AddLog(processOutput, LogItemType.End, "Process Done!");

                algoImage?.Dispose();
                this.isBusy = false;
            }
            return processOutput;
        }

        private void AddLog(ProcessOutput processOutput, LogItemType logItemType, string message, bool isStartLog=false)
        {
            DateTime dateTime = DateTime.Now;
            processOutput.AddLog(logItemType,message, dateTime);
            OnLogAdded?.Invoke(logItemType,message, dateTime, isStartLog);
        }

        private AlgoImage BuildImage(Image2D image2D, ProcesserType processerType)
        {
            return ImageBuilder.Build(ImagingLibrary.MatroxMIL, image2D, ImageType.Grey); ;
            AlgoImage algoImage = null;
            switch (processerType)
            {
                case ProcesserType.MIL:
                    algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image2D, ImageType.Grey);
                    break;
                case ProcesserType.EMGU:
                    algoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2D, ImageType.Gpu);
                    break;
            }
            return algoImage;
        }
    }
}
