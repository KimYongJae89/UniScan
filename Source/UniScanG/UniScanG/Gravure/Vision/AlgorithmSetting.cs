using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision.LengthVariation;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.Vision
{
    public enum ESheetFinderVersion { GRVE, RVOS };
    public enum ETrainerVersion { V1, V2 , RCI };
    public enum ECalculatorVersion { V1, V2, V3_RCI };
    public enum EDetectorVersion { V1, V2 };
    public class AlgorithmSetting
    {
        static AlgorithmSetting _instance;
        public static AlgorithmSetting Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmSetting();

            return _instance;
        }

        public ESheetFinderVersion SheetFinderVersion { get => this.sheetFinderVersion; set => this.sheetFinderVersion = value; }
        ESheetFinderVersion sheetFinderVersion;

        public ETrainerVersion TrainerVersion { get => this.trainerVersion; set => this.trainerVersion = value; }
        ETrainerVersion trainerVersion;

        public ECalculatorVersion CalculatorVersion { get => this.calculatorVersion; set => this.calculatorVersion = value; }
        ECalculatorVersion calculatorVersion;

        public EDetectorVersion DetectorVersion { get => this.detectorVersion; set => this.detectorVersion = value; }
        EDetectorVersion detectorVersion;


        public SheetFinderBaseParam SheetFinderBaseParam => this.sheetFinderBaseParam;
        SheetFinderBaseParam sheetFinderBaseParam;

        public CalculatorParam CalculatorParam => this.calculatorParam;
        CalculatorParam calculatorParam;

        public DetectorParam DetectorParam => this.detectorParam;
        DetectorParam detectorParam;

        public WatcherParam WatcherParam => this.watcherParam;
        WatcherParam watcherParam;

        public TrainerParam TrainerParam => this.trainerParam;
        TrainerParam trainerParam;

        public LengthVariationParam LengthVariationParam => this.lengthVariationParam;
        LengthVariationParam lengthVariationParam;

        public RCI.RCIGlobalOptions RCIGlobalOptions { get; private set; }

        // 스티커 검출 기능
        public bool UseExtSticker { get => this.useExtSticker; set => this.useExtSticker = value; }
        bool useExtSticker;

        // 관찰 기능
        public bool UseExtObserve { get => this.useExtObserve; set => this.useExtObserve = value; }
        bool useExtObserve;

        // 정지화상 기능
        public bool UseExtStopImg { get => this.useExtStopImg; set => this.useExtStopImg = value; }
        bool useExtStopImg;

        // 마진측정 기능
        public bool UseExtMargin { get => this.useExtMargin; set => this.useExtMargin = value; }
        bool useExtMargin;

        // 패턴변형 기능
        public bool UseExtTransfrom { get => this.useExtTransfrom; set => this.useExtTransfrom = value; }
        bool useExtTransfrom;

        public int InspBufferCount { get => this.inspBufferCount; set => this.inspBufferCount = value; }
        int inspBufferCount;


        public float PoleValueMul { get => poleValueMul; set => poleValueMul = value; }
        float poleValueMul;

        public float DielectValueMul { get => dielectValueMul; set => dielectValueMul = value; }
        float dielectValueMul;

        public AlgorithmSetting()
        {
            this.inspBufferCount = 10;

            this.sheetFinderBaseParam = null;
            this.calculatorParam = new CalculatorParam(false);
            this.detectorParam = new DetectorParam(false);
            this.watcherParam = new WatcherParam(false);
            this.trainerParam = new TrainerParam(false);
            this.RCIGlobalOptions = new RCI.RCIGlobalOptions();
            this.lengthVariationParam = new LengthVariationParam();

            this.sheetFinderVersion = ESheetFinderVersion.GRVE;
            this.calculatorVersion = ECalculatorVersion.V2;
            this.detectorVersion = EDetectorVersion.V2;
            this.trainerVersion = ETrainerVersion.V1;

            this.useExtSticker = true;
            this.useExtObserve = false;
            this.useExtStopImg = false;
            this.useExtMargin = false;
            this.useExtTransfrom = false;

            try
            {
                Load();
            }
            catch
            {
                Save();
            }
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
            {
                Save();
                return;
            }

            XmlElement xmlElement= xmlDocument["Algorithm"];
            if (xmlElement == null)
                return;

            this.calculatorParam.UseSticker = XmlHelper.GetValue(xmlElement, "StickerDiffLow", this.calculatorParam.UseSticker);
            this.calculatorParam.StickerDiffLow = XmlHelper.GetValue(xmlElement, "StickerDiffLow", this.calculatorParam.StickerDiffLow);
            this.calculatorParam.StickerDiffHigh = XmlHelper.GetValue(xmlElement, "StickerDiffHigh", this.calculatorParam.StickerDiffHigh);

            this.calculatorParam.UseMultiThread = XmlHelper.GetValue(xmlElement, "UseMultiThread", this.calculatorParam.UseMultiThread);
            
            this.detectorParam.MinBlackDefectLength = XmlHelper.GetValue(xmlElement, "MinBlackDefectLength", this.detectorParam.MinBlackDefectLength);
            this.detectorParam.MinWhiteDefectLength = XmlHelper.GetValue(xmlElement, "MinWhiteDefectLength", this.detectorParam.MinWhiteDefectLength);
            this.detectorParam.CriterionLength = XmlHelper.GetValue(xmlElement, "CriterionLength", this.detectorParam.CriterionLength);
            this.detectorParam.MaximumDefectCount = XmlHelper.GetValue(xmlElement, "MaxDefectNum", this.detectorParam.MaximumDefectCount);

            this.watcherParam.MonitoringPeriod = XmlHelper.GetValue(xmlElement, "MonitoringPeriod", this.watcherParam.MonitoringPeriod);

            this.poleValueMul = XmlHelper.GetValue(xmlElement, "PoleValueMul", this.poleValueMul);
            this.dielectValueMul = XmlHelper.GetValue(xmlElement, "DielectValueMul", this.dielectValueMul);

            this.sheetFinderVersion = XmlHelper.GetValue(xmlElement, "SheetFinderVersion", this.sheetFinderVersion);
            this.trainerVersion = XmlHelper.GetValue(xmlElement, "TrainerVersion", this.trainerVersion);
            this.calculatorVersion = XmlHelper.GetValue(xmlElement, "CalculatorVersion", this.calculatorVersion);
            this.detectorVersion = XmlHelper.GetValue(xmlElement, "DetectorVersion", this.detectorVersion);

#if DEBUG
            //this.calculatorVersion = ECalculatorVersion.V3_RCI;
            //this.trainerVersion = ETrainerVersion.RCI;
#endif

            this.inspBufferCount = XmlHelper.GetValue(xmlElement, "InspBufferCount", this.inspBufferCount);

            this.useExtSticker = XmlHelper.GetValue(xmlElement, "UseExtSticker", this.useExtSticker);
            this.useExtObserve = XmlHelper.GetValue(xmlElement, "UseExtObserve", this.useExtObserve);
            this.useExtStopImg = XmlHelper.GetValue(xmlElement, "UseExtStopImg", this.useExtStopImg);
            this.useExtMargin = XmlHelper.GetValue(xmlElement, "UseExtMargin", this.useExtMargin);
            this.useExtTransfrom = XmlHelper.GetValue(xmlElement, "UseExtTransfrom", this.useExtTransfrom);

            if (this.sheetFinderBaseParam == null)
            {
                switch (this.sheetFinderVersion)
                {
                    case ESheetFinderVersion.GRVE:
                        this.sheetFinderBaseParam = new SheetFinder.SheetBase.SheetFinderV2Param();
                        break;
                    case ESheetFinderVersion.RVOS:
                        this.sheetFinderBaseParam = new SheetFinder.ReversOffset.SheetFinderRVOSParam();
                        break;
                }
            }

            this.sheetFinderBaseParam?.LoadParam(xmlElement, "SheetFinderBaseParam");
            this.trainerParam.LoadParam(xmlElement, "TrainerParam");
            this.calculatorParam.LoadParam(xmlElement, "CalculatorParam");
            this.detectorParam.LoadParam(xmlElement, "DetectorParam");
            this.watcherParam.LoadParam(xmlElement, "WatcherParam");
            this.RCIGlobalOptions.LoadParam(xmlElement, "RCIGlobalOptions");
            this.lengthVariationParam.LoadParam(xmlElement, "LengthVariationParam");

            if (LicenseManager.IsInitialized)
            {
                GetLicenses();
            }
            else
            {
                SetLicenses();
                LicenseManager.Save();
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement xmlElement = xmlDocument.CreateElement("Algorithm");
            xmlDocument.AppendChild(xmlElement);

            this.sheetFinderBaseParam?.SaveParam(xmlElement, "SheetFinderBaseParam");
            this.calculatorParam.SaveParam(xmlElement, "CalculatorParam");
            this.detectorParam.SaveParam(xmlElement, "DetectorParam");
            this.watcherParam.SaveParam(xmlElement, "WatcherParam");
            this.trainerParam.SaveParam(xmlElement, "TrainerParam");
            this.RCIGlobalOptions.SaveParam(xmlElement, "RCIGlobalOptions");
            this.lengthVariationParam.SaveParam(xmlElement, "LengthVariationParam");

            XmlHelper.SetValue(xmlElement, "PoleValueMul", this.poleValueMul);
            XmlHelper.SetValue(xmlElement, "DielectValueMul", this.dielectValueMul);

            XmlHelper.SetValue(xmlElement, "SheetFinderVersion", this.sheetFinderVersion);
            XmlHelper.SetValue(xmlElement, "TrainerVersion", this.trainerVersion);
            XmlHelper.SetValue(xmlElement, "CalculatorVersion", this.calculatorVersion);
            XmlHelper.SetValue(xmlElement, "DetectorVersion", this.detectorVersion);

            XmlHelper.SetValue(xmlElement, "UseExtSticker", this.useExtSticker);
            XmlHelper.SetValue(xmlElement, "UseExtObserve", this.useExtObserve);
            XmlHelper.SetValue(xmlElement, "UseExtStopImg", this.useExtStopImg);
            XmlHelper.SetValue(xmlElement, "UseExtMargin", this.useExtMargin);
            XmlHelper.SetValue(xmlElement, "UseExtTransfrom", this.useExtTransfrom);

            XmlHelper.SetValue(xmlElement, "InspBufferCount", this.inspBufferCount);

            XmlHelper.Save(xmlDocument, configFileName);

            if (LicenseManager.IsInitialized)
            {
                SetLicenses();
                LicenseManager.Save();
            }
        }

        private void SetLicenses()
        {
            //LicenseManager.Clear();
            //LicenseManager.Set("ExtSticker", this.useExtSticker);
            LicenseManager.Set("ExtObserve", this.useExtObserve);
            LicenseManager.Set("ExtStopImg", this.useExtStopImg);
            LicenseManager.Set("ExtMargin", this.useExtMargin);
            LicenseManager.Set("ExtTransfrom", this.useExtTransfrom);
        }

        private void GetLicenses()
        {
            //this.useExtSticker = LicenseManager.Exist("ExtSticker");
            this.useExtObserve = LicenseManager.Exist("ExtObserve");
            this.useExtStopImg = LicenseManager.Exist("ExtStopImg");
            this.useExtMargin = LicenseManager.Exist("ExtMargin");
            this.useExtTransfrom = LicenseManager.Exist("ExtTransfrom");
        }
    }
}
