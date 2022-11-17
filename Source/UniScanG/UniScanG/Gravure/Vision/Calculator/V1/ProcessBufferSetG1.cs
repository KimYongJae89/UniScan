using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.Calculator.V1
{
    public class ProcessBufferSetG1 : ProcessBufferSetG
    {
        public AlgoImage CalculatorInsp { get => calculatorInsp; }
        protected AlgoImage calculatorInsp = null;

        public AlgoImage[] CalculatorTemp { get => calculatorTemp; }
        protected AlgoImage[] calculatorTemp = null;

        public ProcessBufferSetG1(float scaleFactor, bool isMultiLayer, int width, int height) : base(scaleFactor, isMultiLayer, width, height) { }

        public override void BuildBuffers(bool halfScale)
        {
            base.BuildBuffers(halfScale);

            ImagingLibrary calculatorLibType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).LibraryType;
            ImageType calculatorImgType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).ImageType;
            ImagingLibrary detectorLibType = AlgorithmBuilder.GetStrategy(Detector.Detector.TypeName).LibraryType;
            ImageType detectorImgType = AlgorithmBuilder.GetStrategy(Detector.Detector.TypeName).ImageType;
            bool isHeterogenous = (calculatorLibType != detectorLibType) || (calculatorImgType != detectorImgType);

            int bufferDepth = this.isMultiLayer ? 4 : 1;

            width = (int)(width * scaleFactor);
            height = (int)(height * scaleFactor);

            if (scaleFactor > 1 || isHeterogenous)
                bufferList.Add(calculatorInsp = ImageBuilder.Build(CalculatorBase.TypeName, width, height));

            if (scaleFactor < 1 && isHeterogenous)
                bufferList.Add(scaledImage = ImageBuilder.Build(Detector.Detector.TypeName, width, height));

            if (scaleFactor > 1 || isHeterogenous)
            {
                bufferList.Add(scaledImage = ImageBuilder.Build(detectorLibType, ImageType.Grey, width, height));
                bufferList.Add(calculatorInsp = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            }

            calculatorTemp = new AlgoImage[bufferDepth];
            for (int i = 0; i < bufferDepth; i++)
                bufferList.Add(calculatorTemp[i] = ImageBuilder.Build(CalculatorBase.TypeName, width, height));

            bufferList.Add(calculatorResultGray = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            bufferList.Add(calculatorResultBinal = ImageBuilder.Build(CalculatorBase.TypeName, width, height));

            if (isHeterogenous)
                bufferList.Add(detectorInsp = ImageBuilder.Build(Detector.Detector.TypeName, width, height));


            //imageProcessing = new ImageProcessing[0];
        }

        public void SetPrevBitmap(Bitmap bitmap)
        {
            this.prevBitmap = bitmap;
        }

        public override void Upload(DebugContext debugContext)
        {
            Vision.AlgorithmCommon.GetInspImage(this.algoImage, this.scaledImage, this.calculatorInsp, this.scaleFactor);
        }

        public override void Download()
        {
            if (this.detectorInsp != null)
                DynMvp.Vision.ImageConverter.Convert(this.calculatorResultGray, this.detectorInsp);
        }

        public override void Clear()
        {
            base.Clear();

            calculatorInsp?.Clear();
            Array.ForEach(this.calculatorTemp, f => f?.Clear());
        }
    }
}
