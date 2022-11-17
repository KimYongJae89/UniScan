using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Vision
{
    public class AlgorithmArchiver
    {
        public virtual List<Algorithm> GetDefaultAlgorithm()
        {
            return new List<Algorithm>();
        }

        public void LoadAlgorithm(XmlElement algorithmElement, out Algorithm algorithm)
        {
            string algorithmPoolName = XmlHelper.GetValue(algorithmElement, "AlgorithmPoolName", "");
            string algorithmType = XmlHelper.GetValue(algorithmElement, "AlgorithmType", "");

            algorithm = AlgorithmPool.Instance().GetAlgorithm(algorithmPoolName);
            if (algorithm == null)
                algorithm = CreateAlgorithm(algorithmType);

            if (algorithm == null)
            {
                return;
                //throw new InvalidDataException(String.Format("Can't create algorithm [{0}]", algorithmType));
            }

            algorithm.Enabled = AlgorithmBuilder.IsAlgorithmEnabled(algorithmType);
            algorithm.AlgorithmName = XmlHelper.GetValue(algorithmElement, "AlgorithmName", "");

            XmlElement filterListElement = algorithmElement["FilterList"];
            if (filterListElement != null)
            {
                algorithm.FilterList.Clear();

                foreach (XmlElement filterElement in filterListElement)
                {
                    if (filterElement.Name == "Filter")
                    {
                        FilterType filterType = (FilterType)Enum.Parse(typeof(FilterType), XmlHelper.GetValue(filterElement, "FilterType", "EdgeExtraction"));

                        IFilter filter = FilterFactory.CreateFilter(filterType);
                        filter.LoadParam(filterElement);
                        algorithm.FilterList.Add(filter);
                    }
                }
            }
            else
            {
                bool useEdgeExtraction = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseEdgeExtraction", "False"));
                if (useEdgeExtraction)
                {
                    algorithm.FilterList.Add(new EdgeExtractionFilter(3));
                }
                bool useAverageFilter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseAverageFilter", "False"));
                if (useAverageFilter)
                {
                    algorithm.FilterList.Add(new AverageFilter());
                }
                bool useHistogramEqualization = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseHistogramEqualization", "False"));
                if (useHistogramEqualization)
                {
                    algorithm.FilterList.Add(new HistogramEqualizationFilter());
                }

                bool useBinarization = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseBinarization", "False"));
                if (useBinarization)
                {
                    BinarizationType binarizationType = (BinarizationType)Enum.Parse(typeof(BinarizationType), XmlHelper.GetValue(algorithmElement, "BinarizationType", "SingleThreshold"));
                    int thresholdLower = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdLower", "128"));
                    int thresholdUpper = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdUpper", "128"));
                    algorithm.FilterList.Add(new BinarizeFilter(binarizationType, thresholdLower, thresholdUpper));
                }

                int numErode = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "NumErode", "0"));
                if (numErode > 0)
                {
                    algorithm.FilterList.Add(new MorphologyFilter(MorphologyType.Erode, numErode));
                }

                int numDilate = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "NumDilate", "0"));
                if (numDilate > 0)
                {
                    algorithm.FilterList.Add(new MorphologyFilter(MorphologyType.Dilate, numDilate));
                }
            }

            algorithm.LoadParam(algorithmElement);
        }

        public static void SaveAlgorithm(XmlElement algorithmElement, Algorithm algorithm, bool fromAlgorithmPool = false)
        {
            if (algorithm.IsAlgorithmPoolItem == true && fromAlgorithmPool == false)
            {
                XmlHelper.SetValue(algorithmElement, "AlgorithmPoolName", algorithm.AlgorithmName);
            }

            XmlElement filterListElement = algorithmElement.OwnerDocument.CreateElement("FilterList");
            algorithmElement.AppendChild(filterListElement);

            foreach (IFilter filter in algorithm.FilterList)
            {
                XmlElement filterElement = filterListElement.OwnerDocument.CreateElement("Filter");
                filterListElement.AppendChild(filterElement);

                XmlHelper.SetValue(filterElement, "FilterType", filter.GetFilterType().ToString());
                filter.SaveParam(filterElement);
            }

            algorithm.SaveParam(algorithmElement);
        }

        public virtual Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == PatternMatching.TypeName)
                return new PatternMatching();
            else if (algorithmType == BinaryCounter.TypeName)
                return new BinaryCounter();
            else if (algorithmType == BrightnessChecker.TypeName)
                return new BrightnessChecker();
            else if (algorithmType == WidthChecker.TypeName)
                return new WidthChecker();
            else if (algorithmType == LineChecker.TypeName)
                return new LineChecker();
            else if (algorithmType == CircleChecker.TypeName)
                return new CircleChecker();
            else if (algorithmType == BlobChecker.TypeName)
                return new BlobChecker();
            else if (algorithmType == BarcodeReader.TypeName)
                return AlgorithmBuilder.CreateBarcodeReader();
            else if (algorithmType == CharReader.TypeName)
                return AlgorithmBuilder.CreateCharReader();
            else if (algorithmType == ColorChecker.TypeName)
                return new ColorChecker();
            else if (algorithmType == ColorMatchChecker.TypeName)
                return AlgorithmBuilder.CreateColorMatchChecker();
            else if (algorithmType == RectChecker.TypeName)
                return new RectChecker();
            else if (algorithmType == CalibrationChecker.TypeName)
                return new CalibrationChecker();
            else if (algorithmType == DepthChecker.TypeName)
                return new DepthChecker();

                throw new InvalidDataException("Algorithm type is empty");
            return null;
        }
    }
}
