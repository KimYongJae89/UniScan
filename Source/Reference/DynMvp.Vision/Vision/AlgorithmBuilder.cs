using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;

using DynMvp.Base;

using DynMvp.Vision.OpenCv;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
//using DynMvp.Vision.Cognex;
using DynMvp.Vision.Planbss;

using System.IO;

namespace DynMvp.Vision
{
    public class AlgorithmStrategy
    {
        string algorithmType;
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        ImagingLibrary libraryType;
        public ImagingLibrary LibraryType
        {
            get { return libraryType; }
            set { libraryType = value; }
        }

        ImageType imageType;
        public ImageType ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }

        string subLibraryType;
        public string SubLibraryType
        {
            get { return subLibraryType; }
            set { subLibraryType = value; }
        }

        bool enabled = false;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public AlgorithmStrategy(string algorithmType, ImagingLibrary libraryType, string subLibraryType, ImageType imageType = ImageType.Grey)
        {
            this.algorithmType = algorithmType;
            this.libraryType = libraryType;
            this.imageType = imageType;
            this.subLibraryType = subLibraryType;
        }

        public override string ToString()
        {
            return algorithmType;
        }

        public override bool Equals(object obj)
        {
            AlgorithmStrategy algorithmStrategy = obj as AlgorithmStrategy;
            if (algorithmStrategy == null)
                return false;

            return this.algorithmType == algorithmStrategy.algorithmType
                && this.libraryType == algorithmStrategy.libraryType
                && this.imageType == algorithmStrategy.imageType
                && this.subLibraryType == algorithmStrategy.subLibraryType;
        }

        public override int GetHashCode()
        {
            return this.algorithmType.GetHashCode();
        }
    }

    public class AlgorithmBuilder
    {
        static int licenseErrorCount = 0;
        public static int LicenseErrorCount
        {
            get { return licenseErrorCount; }
        }

        static List<AlgorithmStrategy> algorithmStrategyList = new List<AlgorithmStrategy>();
        static OpenEVisionImageProcessing openEVisionImageProcessing = new OpenEVisionImageProcessing();
        static MilImageProcessing milImageProcessing = new MilImageProcessing();
        //static CognexImageProcessing cognexImageProcessing = new CognexImageProcessing();
        static OpenCvImageProcessing openCvImageProcessing = new OpenCvImageProcessing();

        public static void ClearStrategyList()
        {
            algorithmStrategyList.Clear();
        }

        public static void AddStrategy(AlgorithmStrategy strategy)
        {
            if(!algorithmStrategyList.Contains(strategy))
                algorithmStrategyList.Add(strategy);
        }

        public static void RemoveStrategy(AlgorithmStrategy strategy)
        {
            if (algorithmStrategyList.Contains(strategy))
                algorithmStrategyList.Remove(strategy);
        }

        public static bool IsUseMatroxMil()
        {
            return algorithmStrategyList.Exists(f => f.LibraryType == ImagingLibrary.MatroxMIL);
        }

        public static bool IsUseCognexVisionPro()
        {
            return algorithmStrategyList.Exists(f => f.LibraryType == ImagingLibrary.CognexVisionPro);
        }

        public static void InitStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Init Algorithm Strategy");

            if (File.Exists(strategyFileName) == true)
            {
                LoadStrategy(strategyFileName);
            }
            else
            {
                SaveStrategy(strategyFileName);
            }
        }

        private static void LoadStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Algorithm Strategy");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(strategyFileName);

            XmlElement strategyListElement = xmlDocument.DocumentElement;
            foreach (XmlElement strategyElement in strategyListElement)
            {
                if (strategyElement.Name == "AlgorithmStrategy")
                {
                    string algorithmType = XmlHelper.GetValue(strategyElement, "AlgorithmType", "PatternMatching");
                    AlgorithmStrategy algorithmStrategy = GetStrategy(algorithmType);
                    if (algorithmStrategy != null)
                    {
                        algorithmStrategy.LibraryType = (ImagingLibrary)Enum.Parse(typeof(ImagingLibrary), XmlHelper.GetValue(strategyElement, "LibraryType", "OpenCv"));
                        algorithmStrategy.SubLibraryType = XmlHelper.GetValue(strategyElement, "SubLibraryType", "");
                    }
                }
            }
        }

        public static void SaveStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Algorithm Strategy");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement strategyListElement = xmlDocument.CreateElement("", "AlgorithmStrategyList", "");
            xmlDocument.AppendChild(strategyListElement);

            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                XmlElement strategyElement = xmlDocument.CreateElement("", "AlgorithmStrategy", "");
                strategyListElement.AppendChild(strategyElement);

                XmlHelper.SetValue(strategyElement, "AlgorithmType", algorithmStrategy.AlgorithmType);
                XmlHelper.SetValue(strategyElement, "LibraryType", algorithmStrategy.LibraryType.ToString());
                XmlHelper.SetValue(strategyElement, "SubLibraryType", algorithmStrategy.SubLibraryType);
            }

            xmlDocument.Save(strategyFileName);
        }

        public static bool IsAlgorithmEnabled(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                return findedStrategy.Enabled;
            }

            return false;
        }

        public static void SetAlgorithmEnabled(string algorithmType, bool enabled)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                findedStrategy.Enabled = enabled;
            }
            else
            {
                LogHelper.Error(LoggerType.Error, String.Format("Algorithm License Error : {0}", algorithmType));
                licenseErrorCount++;
            }
        }

        public static AlgorithmStrategy GetStrategy(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = algorithmStrategyList.Find(delegate (AlgorithmStrategy strategy) { return strategy.AlgorithmType == algorithmType; });
            if (findedStrategy != null)
            {
                if (LicenseManager.LicenseExist(findedStrategy.LibraryType, findedStrategy.SubLibraryType) == true)
                    return findedStrategy;
            }

            return null;
        }

        public static Pattern CreatePattern()
        {
            //LogHelper.Debug(LoggerType.Operation, "Pattern::CreatePattern");

            AlgorithmStrategy strategy = GetStrategy(PatternMatching.TypeName);

            if (strategy == null)
                return new OpenCv.OpenCvPattern();

            return CreatePattern(strategy.LibraryType);
        }

        public static Pattern CreatePattern(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                case ImagingLibrary.CognexVisionPro:
                    throw new NotImplementedException();
                case ImagingLibrary.EuresysOpenEVision:
                    return new OpenEVisionPattern();
                case ImagingLibrary.MatroxMIL:
                    return new MilPattern();
                case ImagingLibrary.OpenCv:
                default:
                    return new OpenCvPattern();
            }
        }


        public static LineDetector CreateLineDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(LineDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    //return new CognexLineDetector();
                    throw new NotImplementedException();
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilLineDetector();
            }

            return new OpenCvLineDetector();
        }

        public static EdgeDetector CreateEdgeDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(EdgeDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    //return new CognexEdgeDetector();
                    throw new NotImplementedException();
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilEdgeDetector();
            }

            return new OpenCvEdgeDetector();
        }

        public static CharReader CreateCharReader()
        {
            AlgorithmStrategy strategy = GetStrategy(CharReader.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    //return new CognexCharReader();
                    throw new NotImplementedException();
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilCharReader();
            }

            return new OpenCvCharReader();
        }

        public static BarcodeReader CreateBarcodeReader()
        {
            AlgorithmStrategy strategy = GetStrategy(BarcodeReader.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    //return new CognexBarcodeReader();
                    throw new NotImplementedException();
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilBarcodeReader();
            }

            return new OpenCvBarcodeReader();
        }

        public static ColorMatchChecker CreateColorMatchChecker()
        {
            AlgorithmStrategy strategy = GetStrategy(ColorMatchChecker.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    //return new CognexColorMatchChecker();
                    throw new NotImplementedException();
            }

            return new OpenCvColorMatchChecker();
        }

        public static CircleDetector CreateCircleDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(CircleDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilCircleDetector();
            }

            return new OpenCvCircleDetector();
        }

        public static Calibration CreateCalibration()
        {
            AlgorithmStrategy strategy = GetStrategy(Calibration.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return new MilCalibration();
                else if (strategy.LibraryType == ImagingLibrary.OpenCv)
                    return new OpenCvCalibration();
            }
            return new OpenCvCalibration();

#pragma warning disable CS0162 // 접근할 수 없는 코드가 있습니다.
            return null;
#pragma warning restore CS0162 // 접근할 수 없는 코드가 있습니다.
        }

        public static CirclePositionDetector CreateCirclePositionDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(CirclePositionDetector.TypeName);

            if (strategy != null)
            {

            }

            return new CirclePositionDetector();
        }

        public static ImageProcessing GetImageProcessing(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                case ImagingLibrary.EuresysOpenEVision:
                    return openEVisionImageProcessing;
                case ImagingLibrary.MatroxMIL:
                    return milImageProcessing;
                case ImagingLibrary.CognexVisionPro:
                    //return cognexImageProcessing;
                    throw new NotImplementedException();
            }

            return openCvImageProcessing;
        }

        public static ImageProcessing GetImageProcessing(AlgoImage algoImage)
        {
             return GetImageProcessing(algoImage.LibraryType);
        }
    }
}
