using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum FilterType
    {
        None, EdgeExtraction, Average, HistogramEqualization, Binarize, Morphology, Subtraction, Mask
    }

    public interface IFilter
    {
        IFilter Clone();
        FilterType GetFilterType();
        void Filter(AlgoImage algoImage);
        void LoadParam(XmlElement filterElement);
        void SaveParam(XmlElement filterElement);
        bool NeedMasterImage();
        void SetMasterImage(AlgoImage algoImage, bool temporaty=false);
        void ClearMasterImage();
    }

    public class FilterFactory
    {
        public static IFilter CreateFilter(FilterType filterType)
        {
            IFilter filter = null;

            switch (filterType)
            {
                case FilterType.EdgeExtraction:
                    filter = new EdgeExtractionFilter();
                    break;
                case FilterType.Average:
                    filter = new AverageFilter();
                    break;
                case FilterType.HistogramEqualization:
                    filter = new HistogramEqualizationFilter();
                    break;
                case FilterType.Binarize:
                    filter = new BinarizeFilter();
                    break;
                case FilterType.Morphology:
                    filter = new MorphologyFilter();
                    break;
                case FilterType.Subtraction:
                    filter = new SubtractionFilter();
                    break;
                case FilterType.Mask:
                    filter = new MaskFilter();
                    break;
            }
            return filter;
        }
    }
            
    public class EdgeExtractionFilter : IFilter
    {
        int kernelSize;
        public int KernelSize
        {
            get { return kernelSize; }
            set { kernelSize = value; }
        }

        public EdgeExtractionFilter(int kernelSize = 3)
        {
            this.kernelSize = kernelSize;
        }

        public IFilter Clone()
        {
            return new EdgeExtractionFilter(kernelSize);
        }

        public FilterType GetFilterType()
        {
            return FilterType.EdgeExtraction;
        }

        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            imageProcessing.Sobel(algoImage, kernelSize);
        }

        public void LoadParam(XmlElement filterElement)
        {
            kernelSize = Convert.ToInt32(XmlHelper.GetValue(filterElement, "KernelSize", "3"));
        }

        public void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "KernelSize", kernelSize.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            throw new NotImplementedException();
        }

        public void ClearMasterImage()
        {
            throw new NotImplementedException();
        }
    }

    public class AverageFilter : IFilter
    {
        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            imageProcessing.Average(algoImage);
        }

        public IFilter Clone()
        {
            return new AverageFilter();
        }

        public FilterType GetFilterType()
        {
            return FilterType.Average;
        }

        public void LoadParam(XmlElement filterElement)
        {
        }

        public void SaveParam(XmlElement filterElement)
        {
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            throw new NotImplementedException();
        }

        public void ClearMasterImage()
        {
            throw new NotImplementedException();
        }
    }

    public class HistogramEqualizationFilter : IFilter
    {
        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            imageProcessing.HistogramStretch(algoImage);
        }

        public IFilter Clone()
        {
            return new HistogramEqualizationFilter();
        }

        public FilterType GetFilterType()
        {
            return FilterType.HistogramEqualization;
        }

        public void LoadParam(XmlElement filterElement)
        {
        }

        public void SaveParam(XmlElement filterElement)
        {
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            throw new NotImplementedException();
        }

        public void ClearMasterImage()
        {
            throw new NotImplementedException();
        }
    }

    public class BinarizeFilter : IFilter
    {
        private BinarizationType binarizationType;
        public BinarizationType BinarizationType
        {
            get { return binarizationType; }
            set { binarizationType = value; }
        }

        private int thresholdLower;
        public int ThresholdLower
        {
            get { return thresholdLower; }
            set { thresholdLower = value; }
        }

        private int thresholdUpper;
        public int ThresholdUpper
        {
            get { return thresholdUpper; }
            set { thresholdUpper = value; }
        }

        public BinarizeFilter(BinarizationType binarizationType = BinarizationType.SingleThreshold, int thresholdLower = 100, int thresholdUpper = 200)
        {
            this.binarizationType = binarizationType;
            this.thresholdLower = thresholdLower;
            this.thresholdUpper = thresholdUpper;
        }

        public IFilter Clone()
        {
            return new BinarizeFilter(binarizationType, thresholdLower, thresholdUpper);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Binarize;
        }

        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            imageProcessing.Binarize(algoImage, binarizationType, thresholdLower, thresholdUpper);
        }

        public void LoadParam(XmlElement filterElement)
        {
            binarizationType = (BinarizationType)Enum.Parse(typeof(BinarizationType), XmlHelper.GetValue(filterElement, "BinarizationType", "SingleThreshold"));
            thresholdLower = Convert.ToInt32(XmlHelper.GetValue(filterElement, "ThresholdLower", "128"));
            thresholdUpper = Convert.ToInt32(XmlHelper.GetValue(filterElement, "ThresholdUpper", "128"));
        }

        public void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "BinarizationType", binarizationType.ToString());
            XmlHelper.SetValue(filterElement, "ThresholdLower", thresholdLower.ToString());
            XmlHelper.SetValue(filterElement, "ThresholdUpper", thresholdUpper.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            throw new NotImplementedException();
        }

        public void ClearMasterImage()
        {
            throw new NotImplementedException();
        }
    }

    public enum MorphologyType
    {
        Erode, Dilate, Open, Close
    }

    public class MorphologyFilter : IFilter
    {
        private MorphologyType morphologyType;
        public MorphologyType MorphologyType
        {
            get { return morphologyType; }
            set { morphologyType = value; }
        }

        private int numIteration;
        public int NumIteration
        {
            get { return numIteration; }
            set { numIteration = value; }
        }

        public MorphologyFilter(MorphologyType morphologyType = MorphologyType.Erode, int numIteration = 3)
        {
            this.morphologyType = morphologyType;
            this.numIteration = numIteration;
        }

        public IFilter Clone()
        {
            return new MorphologyFilter(morphologyType, numIteration);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Morphology;
        }

        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            switch(morphologyType)
            {
                case MorphologyType.Erode:
                    imageProcessing.Erode(algoImage, numIteration);
                    break;
                case MorphologyType.Dilate:
                    imageProcessing.Dilate(algoImage, numIteration);
                    break;
                case MorphologyType.Open:
                    imageProcessing.Open(algoImage, numIteration);
                    break;
                case MorphologyType.Close:
                    imageProcessing.Close(algoImage, numIteration);
                    break;
            }
        }

        public void LoadParam(XmlElement filterElement)
        {
            morphologyType = (MorphologyType)Enum.Parse(typeof(MorphologyType), XmlHelper.GetValue(filterElement, "MorphologyType", "Erode"));
            numIteration = Convert.ToInt32(XmlHelper.GetValue(filterElement, "NumIteration", "1"));
        }

        public void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "MorphologyType", morphologyType.ToString());
            XmlHelper.SetValue(filterElement, "NumIteration", numIteration.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            throw new NotImplementedException();
        }

        public void ClearMasterImage()
        {
            throw new NotImplementedException();
        }
    }

    public enum SubtractionType
    {
        Absolute, SetZero
    }
    public class SubtractionFilter : IFilter
    {
        private SubtractionType subtractionType;
        public SubtractionType SubtractionType
        {
            get { return subtractionType; }
            set { subtractionType = value; }
        }

        AlgoImage subtractionImage = null;
        public AlgoImage SubtractionImage
        {
            get { return subtractionImage; }
        }

        bool isTrained = false;
        public bool IsTrained
        {
            get { return isTrained; }
        }

        public static string TypeName
        {
            get { return "Subtraction"; }
        }

        bool useInvert = false;
        public bool UseInvert
        {
            get { return useInvert; }
            set { useInvert = value; }
        }

        public SubtractionFilter(SubtractionType subtractionType = SubtractionType.Absolute)
        {
            this.subtractionType = subtractionType;
        }

        public IFilter Clone()
        {
            return new SubtractionFilter(subtractionType);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Subtraction;
        }

        public void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            if (this.subtractionImage != null)
            {
                if (this.SubtractionType == SubtractionType.SetZero)
                {
                    if (useInvert)
                    {
                        imageProcessing.Subtract(algoImage, this.subtractionImage, algoImage);
                    }
                    else
                    {
                        imageProcessing.Subtract(this.subtractionImage, algoImage, algoImage);
                    }
                }
                else
                {
                    AlgoImage subImage1 = ImageBuilder.Build(this.GetFilterType().ToString(), algoImage.ImageType, algoImage.Width, algoImage.Height);
                    AlgoImage subImage2 = ImageBuilder.Build(this.GetFilterType().ToString(), algoImage.ImageType, algoImage.Width, algoImage.Height);
                   
                    imageProcessing.Subtract(this.subtractionImage, algoImage, subImage1);
                    imageProcessing.Subtract(algoImage, this.subtractionImage, subImage2);
                    imageProcessing.Or(subImage1, subImage2, algoImage);

                    subImage1.Dispose();
                    subImage2.Dispose();
                }
            }
        }

        public void LoadParam(XmlElement filterElement)
        {
            subtractionType = (SubtractionType)Enum.Parse(typeof(SubtractionType), XmlHelper.GetValue(filterElement, "SubtractionType", subtractionType.ToString()));
            useInvert = bool.Parse(XmlHelper.GetValue(filterElement, "UseInvert", useInvert.ToString()));

            string bitmapString = XmlHelper.GetValue(filterElement, "SubtractionImage", "");
            if (!string.IsNullOrEmpty(bitmapString))
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(bitmapString);
                if (bitmap != null)
                {
                    ImageType imageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(filterElement, "SubtractionImageType", ImageType.Grey.ToString()));

                    ImageD tempImage = Image2D.FromBitmap(bitmap);
                    subtractionImage = ImageBuilder.Build(this.GetFilterType().ToString(), tempImage, imageType);

                    isTrained = true;
                }
            }
        }

        public void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "SubtractionType", subtractionType.ToString());
            XmlHelper.SetValue(filterElement, "UseInvert", useInvert.ToString());

            if (this.isTrained)
            {
                string bitmapString = ImageHelper.BitmapToBase64String(subtractionImage.ToImageD().ToBitmap());
                XmlHelper.SetValue(filterElement, "SubtractionImage", bitmapString);

                XmlHelper.SetValue(filterElement, "SubtractionImageType", subtractionImage.ImageType.ToString());
            }
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public bool NeedMasterImage()
        {
            if (isTrained == false)
                return true;
            return false;
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            this.subtractionImage = algoImage;
            if (!temporaty)
            {
                isTrained = true;
            }
        }

        public void ClearMasterImage()
        {
            if (this.subtractionImage != null)
                this.subtractionImage.Dispose();

            this.subtractionImage = null;
            isTrained = false;
        }
    }


    public class MaskFilter : IFilter
    {
        AlgoImage maskImage = null;
        public AlgoImage MaskImage
        {
            get { return maskImage; }
        }

        FigureGroup maskFigure = new FigureGroup();
        private bool isTrain = false;
        public bool IsTrain
        {
            get { return isTrain; }
        }

        public FigureGroup MaskFigure
        {
            get { return maskFigure; }
        }



        public IFilter Clone()
        {
            MaskFilter maskFilter = new MaskFilter();
            if (this.maskImage != null)
            {
                maskFilter.maskImage = this.maskImage.Clone();
                maskFilter.isTrain = this.isTrain;
            }
            return maskFilter;
        }

        public void Filter(AlgoImage algoImage)
        {
            if (maskImage == null)
            {
                return;
            }

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            
            if ((algoImage.Width != maskImage.Width) || (algoImage.Height != maskImage.Height))
                return;

            imageProcessing.And(algoImage, maskImage, algoImage);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Mask;
        }

        public void LoadParam(XmlElement filterElement)
        {
            string imageString = XmlHelper.GetValue(filterElement, "MaskImage", "");
            if (!string.IsNullOrEmpty(imageString))
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
                if (bitmap != null)
                {
                    ImageType imageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(filterElement, "MaskImageType", ImageType.Grey.ToString()));
                    maskImage = ImageBuilder.Build(this.GetFilterType().ToString(), Image2D.FromBitmap(bitmap), imageType);
                    isTrain = true;
                }
            }
        }

        public bool NeedMasterImage()
        {
            if (IsTrain)
            {
                return false;
            }
            return true;
        }

        public void SaveParam(XmlElement filterElement)
        {
            if (isTrain)
            {
                string imageString = ImageHelper.BitmapToBase64String(maskImage.ToImageD().ToBitmap());
                XmlHelper.SetValue(filterElement, "MaskImage", imageString);

                XmlHelper.SetValue(filterElement, "MaskImageType", maskImage.ImageType.ToString());
            }
        }

        public override string ToString()
        {
            return StringManager.GetString(this.GetType().FullName,GetFilterType().ToString());
        }

        public void SetMasterImage(AlgoImage algoImage, bool temporaty = false)
        {
            maskImage = algoImage;
            if (!temporaty)
            {
                isTrain = true;
            }
        } 

        public void ClearMasterImage()
        {
            if (this.maskImage != null)
                this.maskImage.Dispose();

            this.maskImage = null;
            isTrain = false;
        }
    }
}
