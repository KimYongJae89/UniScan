using DynMvp.Barcode;
using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml;
using UniEye.Base.Data;

namespace UniEye.Base.Settings
{
    public class OperationSettings
    {
        private string language = "English";
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string GetLocaleCode()
        {
            string localeCode = "";

            if (language == null || language == "" || language == "English")
                return "en-us";

            int startIndex = language.IndexOf("[");
            int endIndex = language.IndexOf("]");
            if (startIndex > -1 && endIndex > -1 && endIndex > startIndex)
            {
                localeCode = language.Substring(startIndex + 1, endIndex - startIndex - 1);
            }

            return localeCode;
        }

        private bool repeatInspection;
        public bool RepeatInspection
        {
            get { return repeatInspection; }
            set { repeatInspection = value; }
        }

        private bool saveDebugImage;
        public bool SaveDebugImage
        {
            get { return saveDebugImage; }
            set { saveDebugImage = value; }
        }

        private bool saveProbeImage;
        public bool SaveProbeImage
        {
            get { return saveProbeImage; }
            set { saveProbeImage = value; }
        }

        private bool saveTargetImage;
        public bool SaveTargetImage
        {
            get { return saveTargetImage; }
            set { saveTargetImage = value; }
        }

        private bool saveTargetGroupImage;
        public bool SaveTargetGroupImage
        {
            get { return saveTargetGroupImage; }
            set { saveTargetGroupImage = value; }
        }

        private ImageFormat targetGroupImageFormat = ImageFormat.Bmp;
        public ImageFormat TargetGroupImageFormat
        {
            get { return targetGroupImageFormat; }
            set { targetGroupImageFormat = value; }
        }

        private bool autoResetProductionCount;
        public bool AutoResetProductionCount
        {
            get { return autoResetProductionCount; }
            set { autoResetProductionCount = value; }
        }

        private int trackerSize = 10;
        public int TrackerSize
        {
            get { return trackerSize; }
            set { trackerSize = value; }
        }

        private string defaultUserId;
        public string DefaultUserId
        {
            get { return defaultUserId; }
            set { defaultUserId = value; }
        }

        private string defaultUserPassword;
        public string DefaultUserPassword
        {
            get { return defaultUserPassword; }
            set { defaultUserPassword = value; }
        }

        private int resultStoringDays=30;
        public int ResultStoringDays
        {
            get { return resultStoringDays; }
            set { resultStoringDays = value; }
        }

        private float resultCopyDays =-1;
        public float ResultCopyDays
        {
            get { return resultCopyDays; }
            set { resultCopyDays = value; }
        }

        private int minimumFreeSpace = 10;
        public int MinimumFreeSpace
        {
            get { return minimumFreeSpace; }
            set { minimumFreeSpace = value; }
        }

        private string systemType = "";
        public string SystemType
        {
            get { return systemType; }
            set { systemType = value; }
        }

        private ImagingLibrary imagingLibrary;
        public ImagingLibrary ImagingLibrary
        {
            get { return imagingLibrary; }
            set { imagingLibrary = value; }
        }

        private bool useNonPagedMem;
        public bool UseNonPagedMem
        {
            get { return useNonPagedMem; }
            set { useNonPagedMem = value; }
        }

        private bool useCuda;
        public bool UseCuda
        {
            get { return useCuda; }
            set { useCuda = value; }
        }
        
        private int cpuUsage;
        public int CpuUsage
        {
            get { return cpuUsage; }
            set { cpuUsage = value; }
        }

        private bool showCenterGuide;
        public bool ShowCenterGuide
        {
            get { return showCenterGuide; }
            set { showCenterGuide = value; }
        }

        private bool useRejectPusher;
        public bool UseRejectPusher
        {
            get { return useRejectPusher; }
            set { useRejectPusher = value; }
        }


        /// <summary>
        /// 사용자 계정 관리를 사용할 것인지 여부를 설정한다.
        /// LogInForm / Change User / User Manager 등이 활성화 되고, 권한에 따른 기능 설정이 변경되어야 한다.
        /// </summary>
        private bool useUserManager;
        public bool UseUserManager
        {
            get { return useUserManager; }
            set { useUserManager = value; }
        }

        private bool useUpdateMonitor;
        public bool UseUpdateMonitor
        {
            get { return useUpdateMonitor; }
            set { useUpdateMonitor = value; }
        }

        private bool showScore;
        public bool ShowScore
        {
            get { return showScore; }
            set { showScore = value; }
        }

        /// <summary>
        /// 검사 결과창에 표시되는 불량 이미지를 불량 위치가 표시된 이미지를 표시한다.
        /// 그려진   이미지가 없을 시 원본 이미지를 출력한다
        /// </summary>
        private bool showNGImage;
        public bool ShowNGImage
        {
            get { return showNGImage; }
            set { showNGImage = value; }
        }

        private bool useRemoteBackup;
        public bool UseRemoteBackup
        {
            get { return useRemoteBackup; }
            set { useRemoteBackup = value; }
        }

        private int imageCheckIntervalS = 3600;
        public int ImageCheckIntervalS
        {
            get { return imageCheckIntervalS; }
            set { imageCheckIntervalS = value; }
        }

        private bool useFixedInspectionStep;
        public bool UseFixedInspectionStep
        {
            get { return useFixedInspectionStep; }
            set { useFixedInspectionStep = value; }
        }

        // FPCB Aligner에서 사용됨. 단일 Step 티칭 후, 2단계에 걸쳐 검사를 수행한다.
        private bool useSingleStepModel;
        public bool UseSingleStepModel
        {
            get { return useSingleStepModel; }
            set { useSingleStepModel = value; }
        }

        private int numInspectionStep;
        public int NumInspectionStep
        {
            get { return numInspectionStep; }
            set { numInspectionStep = value; }
        }

        private bool useFiducialStep;
        public bool UseFiducialStep
        {
            get { return useFiducialStep; }
            set { useFiducialStep = value; }
        }

        private bool useDefectReview;
        public bool UseDefectReview
        {
            get { return useDefectReview; }
            set { useDefectReview = value; }
        }

        private DataPathType dataPathType = DataPathType.Model_Day;
        public DataPathType DataPathType
        {
            get { return dataPathType; }
            set { dataPathType = value; }
        }

        public bool UseHasp { get; set; } = true;

        private bool isAligner;
        public bool IsAligner
        {
            get { return isAligner; }
            set { isAligner = value; }
        }
        private int showFirstPageIndex = 1;
        public int ShowFirstPageIndex
        {
            get { return showFirstPageIndex; }
            set { showFirstPageIndex = value; }
        }

        Point centerGuidePos;
        public Point CenterGuidePos
        {
            get { return centerGuidePos; }
            set { centerGuidePos = value; }
        }

        int centerGuideThickness;
        public int CenterGuideThickness
        {
            get { return centerGuideThickness; }
            set { centerGuideThickness = value; }
        }

        /// <summary>
        /// 검사 완료 후 최종 이미지를 다시 획득하여 화면에 표시할지 여부를 설정한다.
        /// 일반적으로 Aligner와 같이 사용된다.
        /// </summary>
        private bool showFinalImage;
        public bool ShowFinalImage
        {
            get { return showFinalImage; }
            set { showFinalImage = value; }
        }

        private bool useSingleTargetModel;
        public bool UseSingleTargetModel
        {
            get { return useSingleTargetModel; }
            set { useSingleTargetModel = value; }
        }

        public bool saveResultFigure;
        public bool SaveResultFigure
        {
            get { return saveResultFigure; }
            set { saveResultFigure = value; }
        }

        public bool useSimpleLightParamForm;
        public bool UseSimpleLightParamForm
        {
            get { return useSimpleLightParamForm; }
            set { useSimpleLightParamForm = value; }
        }

        private BarcodeSettings barcode = new BarcodeSettings();
        public BarcodeSettings Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }

        private LogLevel logLevel = LogLevel.Debug;
        public LogLevel LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }
        
        static OperationSettings _instance;
        public static OperationSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new OperationSettings();
                _instance.Load();
            }

            return _instance;
        }
        
        protected OperationSettings()
        {
            numInspectionStep = 1;
        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\operation.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement operationElement = xmlDocument.CreateElement("", "Operation", "");
            xmlDocument.AppendChild(operationElement);

            XmlHelper.SetValue(operationElement, "SystemType", systemType);
            XmlHelper.SetValue(operationElement, "Language", language);
            XmlHelper.SetValue(operationElement, "TrackerSize", trackerSize.ToString());
            XmlHelper.SetValue(operationElement, "AutoResetProductionCount", autoResetProductionCount.ToString());
            XmlHelper.SetValue(operationElement, "DefaultUserId", defaultUserId);
            XmlHelper.SetValue(operationElement, "DefaultUserPassword", defaultUserPassword);
            XmlHelper.SetValue(operationElement, "SaveProbeImage", saveProbeImage.ToString());
            XmlHelper.SetValue(operationElement, "SaveTargetImage", saveTargetImage.ToString());
            XmlHelper.SetValue(operationElement, "SaveTargetGroupImage", saveTargetGroupImage.ToString());
            XmlHelper.SetValue(operationElement, "SaveResultFigure", saveResultFigure.ToString());
            XmlHelper.SetValue(operationElement, "TargetGroupImageFormat", targetGroupImageFormat.ToString());
            XmlHelper.SetValue(operationElement, "ResultStoringDays", resultStoringDays.ToString());
            XmlHelper.SetValue(operationElement, "ResultCopyDays", resultCopyDays.ToString());
            XmlHelper.SetValue(operationElement, "MinimumFreeSpace", minimumFreeSpace.ToString());
            XmlHelper.SetValue(operationElement, "ImagingLibrary", imagingLibrary.ToString());
            XmlHelper.SetValue(operationElement, "UseCuda", useCuda.ToString());
            XmlHelper.SetValue(operationElement, "UseNonPagedMem", useNonPagedMem.ToString());
            XmlHelper.SetValue(operationElement, "CpuUsage", cpuUsage.ToString());
            XmlHelper.SetValue(operationElement, "ShowFirstPageIndex", showFirstPageIndex.ToString());
            XmlHelper.SetValue(operationElement, "ShowCenterLine", showCenterGuide.ToString());
            XmlHelper.SetValue(operationElement, "UseRejectCylinder", useRejectPusher.ToString());
            XmlHelper.SetValue(operationElement, "ShowScore", showScore.ToString());
            XmlHelper.SetValue(operationElement, "ShowNGImage", showNGImage.ToString());
            XmlHelper.SetValue(operationElement, "UseRemoteBackup", useRemoteBackup.ToString());
            XmlHelper.SetValue(operationElement, "ImageCheckInterval", imageCheckIntervalS.ToString());
            XmlHelper.SetValue(operationElement, "UseFiducialStep", useFiducialStep.ToString());
            XmlHelper.SetValue(operationElement, "UseFixedInspectionStep", useFixedInspectionStep.ToString());
            XmlHelper.SetValue(operationElement, "UseSingleStepModel", useSingleStepModel.ToString());
            XmlHelper.SetValue(operationElement, "UseSingleTargetModel", useSingleTargetModel.ToString());
            XmlHelper.SetValue(operationElement, "NumInspectionStep", numInspectionStep.ToString());
            XmlHelper.SetValue(operationElement, "UseDefectReview", useDefectReview.ToString());
            XmlHelper.SetValue(operationElement, "DatePathType", dataPathType.ToString());
            XmlHelper.SetValue(operationElement, "IsAligner", isAligner.ToString());
            XmlHelper.SetValue(operationElement, "UseUserManager", useUserManager.ToString());
            XmlHelper.SetValue(operationElement, "DataPathType", dataPathType.ToString());
            XmlHelper.SetValue(operationElement, "UseSimpleLightControl", useSimpleLightParamForm.ToString());
            XmlHelper.SetValue(operationElement, "CenterGuidePosX", centerGuidePos.X.ToString());
            XmlHelper.SetValue(operationElement, "CenterGuidePosY", centerGuidePos.Y.ToString());
            XmlHelper.SetValue(operationElement, "CenterGuideThickness", centerGuideThickness.ToString());
            XmlHelper.SetValue(operationElement, "UseUpdateMonitor", useUpdateMonitor.ToString());
            XmlHelper.SetValue(operationElement, "LogLevel", logLevel.ToString());
            XmlHelper.SetValue(operationElement, "SaveDebugImage", saveDebugImage.ToString());
            XmlHelper.SetValue(operationElement, "UseHasp", UseHasp.ToString());

            XmlElement barcodeElement = xmlDocument.CreateElement("", "Barcode", "");
            operationElement.AppendChild(barcodeElement);

            barcode.Save(barcodeElement);
            
            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\operation.xml", PathSettings.Instance().Config);

            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Operation"];
                if (operationElement == null)
                    return;

                autoResetProductionCount = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "AutoResetProductionCount", "False"));

                defaultUserId = XmlHelper.GetValue(operationElement, "DefaultUserId", "");
                defaultUserPassword = XmlHelper.GetValue(operationElement, "DefaultUserPassword", "");

                systemType = XmlHelper.GetValue(operationElement, "SystemType", "");
                language = XmlHelper.GetValue(operationElement, "Language", "");
                trackerSize = Convert.ToInt32(XmlHelper.GetValue(operationElement, "TrackerSize", "10"));
                saveProbeImage = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "SaveProbeImage", "False"));
                saveTargetImage = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "SaveTargetImage", "False"));
                saveTargetGroupImage = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "SaveTargetGroupImage", "False"));
                saveResultFigure = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "SaveResultFigure", "False"));

                string imageFormat = XmlHelper.GetValue(operationElement, "TargetGroupImageFormat", "Bmp");
                if (imageFormat == "Jpeg")
                    targetGroupImageFormat = ImageFormat.Jpeg;
                else if (imageFormat == "Png")
                    targetGroupImageFormat = ImageFormat.Png;
                else
                    targetGroupImageFormat = ImageFormat.Bmp;

                resultStoringDays = Convert.ToInt32(XmlHelper.GetValue(operationElement, "ResultStoringDays", "30"));
                minimumFreeSpace = Convert.ToInt32(XmlHelper.GetValue(operationElement, "MinimumFreeSpace", "10"));
                resultCopyDays = Convert.ToSingle(XmlHelper.GetValue(operationElement, "ResultCopyDays", "-1"));
                imagingLibrary = (ImagingLibrary)Enum.Parse(typeof(ImagingLibrary), XmlHelper.GetValue(operationElement, "ImagingLibrary", "OpenCv"));
                useCuda = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseCuda", "false"));
                useNonPagedMem = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseNonPagedMem", "false"));
                cpuUsage = Convert.ToInt32(XmlHelper.GetValue(operationElement, "CpuUsage", "20"));
                showFirstPageIndex = Convert.ToInt32(XmlHelper.GetValue(operationElement, "ShowFirstPageIndex", "1"));
                showCenterGuide = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "ShowCenterLine", "False"));
                useRejectPusher = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseRejectCylinder", "False"));
                showScore = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "ShowScore", "False"));
                showNGImage = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "ShowNGImage", "False"));
                useRemoteBackup = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseRemoteBackup", "False"));
                imageCheckIntervalS = Convert.ToInt32(XmlHelper.GetValue(operationElement, "ImageCheckInterval", "3600"));
                useFiducialStep = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseFiducialStep", "False"));
                useFixedInspectionStep = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseFixedInspectionStep", "True"));
                useSingleTargetModel = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseSingleTargetModel", "false"));
                useSingleStepModel = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseSingleStepModel", "False"));
                numInspectionStep = Convert.ToInt32(XmlHelper.GetValue(operationElement, "NumInspectionStep", "1"));
                useDefectReview = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseDefectReview", "False"));
                dataPathType = (DataPathType)Enum.Parse(typeof(DataPathType), XmlHelper.GetValue(operationElement, "DataPathType", dataPathType.ToString()));
                isAligner = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "IsAligner", "False"));
                useUserManager = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseUserManager", "False"));
                useSimpleLightParamForm = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseSimpleLightControl", "False"));
                centerGuidePos.X = Convert.ToInt32(XmlHelper.GetValue(operationElement, "CenterGuidePosX", "0"));
                centerGuidePos.Y = Convert.ToInt32(XmlHelper.GetValue(operationElement, "CenterGuidePosY", "0"));
                centerGuideThickness = Convert.ToInt32(XmlHelper.GetValue(operationElement, "CenterGuideThickness", "1"));
                useUpdateMonitor = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "UseUpdateMonitor", "true"));
                saveDebugImage = Convert.ToBoolean(XmlHelper.GetValue(operationElement, "SaveDebugImage", "false"));
                UseHasp = XmlHelper.GetValue(operationElement, "UseHasp", true);

                logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), XmlHelper.GetValue(operationElement, "LogLevel", logLevel.ToString()));

                XmlElement barcodeElement = xmlDocument["Barcode"];
                if (barcodeElement != null)
                {
                    barcode.Load(barcodeElement);
                }

                PathManager.DataPathType = dataPathType;

                // Installer에서 설치한 언어 따라가기
                string p = System.IO.Path.Combine(PathSettings.Instance().Config, "InstallLanguage.txt");
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(p);
                if(fileInfo.Exists)
                {
                    string lang = System.IO.File.ReadAllText(fileInfo.FullName);
                    this.language = lang;
                    Save();
                    fileInfo.Delete();
                }
            }
            catch
            {
                Save();
            }
        }
    }
}
