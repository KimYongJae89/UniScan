using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.InspData;
using DynMvp.Inspection;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;

using UniEye.Base.Device;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using DynMvp.UI.Touch;
using UniEye.Base.MachineInterface;
using UniEye.Base.UI;
using UniEye.Base.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace UniEye.Base
{
    public delegate bool SystemManagerSetupDoneDelegate();

    public class SystemManager
    {
        private struct SYSTEMTIME { public ushort wYear, wMonth, wDayOfWeek, wDay, wHour, wMinute, wSecond, wMilliseconds; }

        [DllImport("kernel32.dll")] private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")] private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        public SystemManagerSetupDoneDelegate OnSetupDone = null;

        public event UI.OnModelChangedDelegate OnModelChanged;

        protected static SystemManager _instance = null;
        public static SystemManager Instance()
        {
            return _instance;
        }

        //protected static SystemManager _instance2 = null;
        //public static SystemManager Instance2
        //{
        //    get
        //    {
        //        //if ( _instance2 == null )
        //        //{
        //        //    _instance2 = new SystemManager();
        //        //}
        //        //return _instance2;
        //        return _instance2 ?? new SystemManager();
        //    }
        //}

        public static void SetInstance(SystemManager systemManager)
        {
            _instance = systemManager;
        }

        protected IMainForm mainForm;
        public IMainForm MainForm
        {
            get { return mainForm; }
            set { mainForm = value; }
        }

        protected DeviceBox deviceBox;
        public DeviceBox DeviceBox
        {
            get { return deviceBox; }
        }

        protected DeviceController deviceController;
        public DeviceController DeviceController
        {
            get { return deviceController; }
        }

        protected InspectRunner inspectRunner;
        public InspectRunner InspectRunner
        {
            get { return inspectRunner; }
        }


        //public MachineIfProtocolList MachineIfProtocolList => this.deviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;

        protected Data.ModelManager modelManager;
        public Data.ModelManager ModelManager
        {
            get { return modelManager; }
        }

        protected AlgorithmArchiver algorithmArchiver;
        public AlgorithmArchiver AlgorithmArchiver
        {
            get { return algorithmArchiver; }
        }

        protected Model currentModel;
        public Model CurrentModel
        {
            get { return currentModel; }
            set { currentModel = value; }
        }

        protected InspectionResult inspectionResult;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
        }
        
        string lastResultPath;

        protected List<DataExporter> dataExporterList = new List<DataExporter>();
        public List<DataExporter> DataExporterList
        {
            get { return dataExporterList; }
        }

        protected UiChanger uiChanger;
        public UiChanger UiChanger
        {
            get { return uiChanger; }
        }

        protected List<ThreadHandler> dataManagerList = new List<ThreadHandler>();
        public List<ThreadHandler> DataManagerList
        {
            get { return dataManagerList; }
        }

        protected ProductionManagerBase productionManager;
        public ProductionManagerBase ProductionManager
        {
            get { return productionManager; }
            set { productionManager = value; }
        }

        public PowerManager PowerManager { get; protected set; }

        bool onScanImage = false;
        public bool OnScanImage
        {
            get { return onScanImage; }
            set { onScanImage = value; }
        }

        protected ProgressForm progressForm;

        private bool livemode = false;
        public bool LiveMode
        {
            get { return livemode; }
            set { livemode = value; }
        }

        bool alarmState = false;

        //public virtual PortMap CreatePortMap()
        //{
        //    return new PortMap();
        //}

        //private bool onInspectionPage = false;
        //public bool OnInspectionPage
        //{
        //    get { return onInspectionPage; }
        //    set { onInspectionPage = value; }
        //}
        
        public virtual string[] GetSystemTypeNames()
        {
            return new string[] { "" };
        }

        public SystemManager()
        {
        }

        public virtual SystemManager GetSystemManager()
        {
            return _instance;
        }

        public void Init(Data.ModelManager modelManager, UiChanger uiChanger, AlgorithmArchiver algorithmArchiver,
            DeviceBox deviceBox, DeviceController deviceController, ProductionManagerBase productionManager, PowerManager powerManager)
        {
            this.uiChanger = uiChanger;
            this.modelManager = modelManager;
            this.algorithmArchiver = algorithmArchiver;
            this.deviceBox = deviceBox;
            this.deviceController = deviceController;
            this.productionManager = productionManager;
            this.PowerManager = powerManager;

            LoadAdditialSettings();
        }

        //public void Init(Data.ModelManager modelManager, UiChanger uiChanger, AlgorithmArchiver algorithmArchiver,
        //    DeviceBox deviceBox, DeviceController deviceController)
        //{
        //    this.uiChanger = uiChanger;
        //    this.modelManager = modelManager;
        //    this.algorithmArchiver = algorithmArchiver;
        //    this.deviceBox = deviceBox;
        //    this.deviceController = deviceController;

        //    LoadAdditialSettings();

        //    //mainForm = uiChanger.CreateMainForm();
        //}

        protected virtual void LoadAdditialSettings()
        {
            AdditionalSettings.CreateInstance();
            AdditionalSettings.Instance().Load();
        }

        public bool Setup()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init SplashForm");
            
            if (OperationSettings.Instance().UseUserManager)
            {
                LogInForm loginForm = new LogInForm();
                loginForm.ShowDialog();
                if (loginForm.DialogResult == DialogResult.Cancel)
                    return false;

                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
            }
            
            SplashForm form = new SplashForm("");
            form.ConfigAction = SplashConfigAction;
            form.SetupAlgorithmStrategyAction = this.SetupAlgorithmStrategy;
            form.SetupAction = SplashSetupAction;
            form.title.Text = CustomizeSettings.Instance().ProgramTitle;
            if (File.Exists(PathSettings.Instance().CompanyLogo) == true)
                form.companyLogo.Image = new Bitmap(PathSettings.Instance().CompanyLogo);
            if (File.Exists(PathSettings.Instance().ProductLogo) == true)
                form.productLogo.Image = new Bitmap(PathSettings.Instance().ProductLogo);

            string copyright = CustomizeSettings.Instance().Copyright;
            //if (string.IsNullOrEmpty(copyright))
            //    form.copyrightText.Text = string.Format("@2019 UniEye, All right reserved.");
            //else
            form.copyrightText.Text = string.Format("@{0}, All right reserved.", copyright);
            form.title.Text = CustomizeSettings.Instance().Title;
            Configuration.Initialize(PathSettings.Instance().Config, PathSettings.Instance().Temp, 7, false, true, 1);
            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");

            form.ShowDialog();

            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");

            if (form.DialogResult == DialogResult.Abort)
            {
                return false;
            }
            
            bool ok = true;
            if (OnSetupDone != null)
                ok = OnSetupDone();
            return ok;
        }


        private void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
                reportProgress.ReportProgress(percentage, StringManager.GetString(this.GetType().FullName,message));
        }
        
        public bool SetupAlgorithmStrategy(IReportProgress reportProgress)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start Setup Algorithm Strategy");

            BuildAlgorithmStrategy();
            SelectAlgorithmStrategy();
            if (AlgorithmBuilder.LicenseErrorCount > 0)
                throw new Exception("License Authorize Fail");

            if (AlgorithmBuilder.IsUseMatroxMil())
            {
                return MatroxHelper.InitApplication(OperationSettings.Instance().UseNonPagedMem, OperationSettings.Instance().UseCuda);
            }
            return true;
        }

        public virtual void BuildAlgorithmStrategy()
        {
            switch (OperationSettings.Instance().ImagingLibrary)
            {
                case ImagingLibrary.CognexVisionPro:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize Vision Pro Algorithms");
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.CognexVisionPro, "VxPatMax|VxPatQuick"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.CognexVisionPro, "VxDimensioning"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BarcodeReader.TypeName, ImagingLibrary.CognexVisionPro, "VxSymbol"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CharReader.TypeName, ImagingLibrary.CognexVisionPro, "VxOCR"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(LineChecker.TypeName, ImagingLibrary.CognexVisionPro, "VxDimensioning"));
                    break;
                case ImagingLibrary.MatroxMIL:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize MIL Algorithms");
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.MatroxMIL, "PAT"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(EdgeDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CircleDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.MatroxMIL, "CAL"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BlobChecker.TypeName, ImagingLibrary.MatroxMIL, "BLOB"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(LineDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BarcodeReader.TypeName, ImagingLibrary.MatroxMIL, "CODE"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CharReader.TypeName, ImagingLibrary.MatroxMIL, "OCR"));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(LineChecker.TypeName, ImagingLibrary.MatroxMIL, ""));
                    break;
                case ImagingLibrary.OpenCv:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize OpenCV Algorithms");
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(EdgeDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CircleDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BlobChecker.TypeName, ImagingLibrary.OpenCv, ""));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(LineDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    break;
                case ImagingLibrary.Custom:
                    string fileName = String.Format("{0}\\AlgorithmStrategy.xml", PathSettings.Instance().Config);
                    AlgorithmBuilder.InitStrategy(fileName);
                    break;
            }

            if (OperationSettings.Instance().ImagingLibrary != ImagingLibrary.Custom)
            {
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BinaryCounter.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(BrightnessChecker.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(RectChecker.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CircleChecker.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalibrationChecker.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(DepthChecker.TypeName, ImagingLibrary.OpenCv, ""));
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(ColorChecker.TypeName, ImagingLibrary.OpenCv, ""));
            }
        }

        public virtual void SelectAlgorithmStrategy()
        {
        }

        public virtual bool SplashConfigAction(IReportProgress reportProgress)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (loginForm.LogInUser.IsSuperAccount)
                {
                    ConfigForm form = new ConfigForm();
                    form.InitSystemType(SystemManager.Instance().GetSystemTypeNames(), OperationSettings.Instance().SystemType);
                    form.InitCustomConfigPage(SystemManager.Instance().GetCustomConfigPage());
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        return SystemManager.Instance().ApplySystemSettings();
                    }
                }
                else
                {
                    MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "You don't have proper permission."));
                }
            }
            return true;
        }

        public virtual bool ApplySystemSettings()
        {
            MachineSettings.Instance().Load();
            Util.ApplicationHelper.InitStringTable();
            return true;
        }

        void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SplashSetupAction(progressForm);
        }

        protected virtual bool SplashSetupAction(IReportProgress reportProgress)
        {
            try
            {
                

                DoReportProgress(reportProgress, 10, "Initialize Model List");

                modelManager?.Refresh(PathSettings.Instance().Model);

                // 이거 여기서 하면 안 됨
                // 알고리즘 리스팅 후 MIL APP 초기화하면
                // SplashForm 소멸시 Thread가 죽으면서 MIL 오류남.
                // SetupAlgorithmStrategy();


                AlgorithmPool.Instance().Initialize(algorithmArchiver);

                DoReportProgress(reportProgress, 20, "Initialize Machine");

                //                try
                //                {
                deviceBox?.Initialize(reportProgress);
                deviceController?.Initialize(deviceBox);
                //                }
                //#if !DEBUG
                //                catch (DllNotFoundException ex)
                //                {
                //                    string message = string.Format("DllNotFoundException\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                //                    MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                    return false;
                //                }
                //                catch (FileNotFoundException ex)
                //                {
                //                    string message = string.Format("FileNotFoundException\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                //                    MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                    return false;
                //                }
                //                catch (Exception ex)
                //                {
                //                    StringBuilder sb = new StringBuilder();
                //                    sb.AppendLine("!!Exception!!");
                //                    sb.AppendLine(ex.TargetSite.Name);
                //                    sb.AppendLine(ex.GetType().ToString());
                //                    sb.AppendLine(ex.Message);
                //                    sb.AppendLine(ex.StackTrace);
                //                    string message = sb.ToString();
                //                    MessageBox.Show(message);

                //                    return false;
                //                }
                //#endif
                //                finally { }

                DoReportProgress(reportProgress, 40, "Create Inspector Processor Extender");
                InitalizeInspectRunner();

                DoReportProgress(reportProgress, 70, "Start Image Copy");

                if (OperationSettings.Instance().UseRemoteBackup)
                {
                    StartImageCopy();
                }
                else
                {
                    InitializeResultManager();
                }

                DoReportProgress(reportProgress, 80, "Create Data Exporter");
                InitializeDataExporter();

                DoReportProgress(reportProgress, 90, "Init Power Manager");
                InitializePowerManager();

                DoReportProgress(reportProgress, 90, "Init Additional Units");
                InitializeAdditionalUnits();
            }
            //#if !DEBUG
            //            catch (Exception ex)
            //            {
            //                DoReportProgress(reportProgress, 100, ex.Message);
            //                reportProgress.SetLastError(ex.Message);

            //                string message = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            //                MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //                return false;
            //            }
            //#endif
            finally { }
            DoReportProgress(reportProgress, 90, "End of Initialize");

            return true;
        }

        public virtual void InitializeResultManager()
        {
            //dataManagerList.Add(new DataRemover(DataStoringType.Seq, PathSettings.Instance().Result, OperationSettings.Instance().ResultStoringDays, "yyyy-MM-dd", false));
            dataManagerList.Add(new DataRemover(OperationSettings.Instance().ResultStoringDays, OperationSettings.Instance().MinimumFreeSpace, SystemManager.Instance().ProductionManager, new DirectoryInfo(PathSettings.Instance().Log)));

            if (OperationSettings.Instance().ResultCopyDays > 0)
                dataManagerList.Add(new DynMvp.Data.DataCopier(SystemManager.Instance().productionManager, OperationSettings.Instance().ResultCopyDays, OperationSettings.Instance().MinimumFreeSpace));

        }

        public virtual void InitializePowerManager()
        {
            if (this.PowerManager == null)
                return;

            this.PowerManager.OnPowerStateOffline += SystemManager.Instance().inspectRunner.ExitWaitInspection;
            this.PowerManager.OnBattLevelCritical += PowerManager.ShutDown;

            this.PowerManager.Initialize();
            this.PowerManager.Start();
        }

        public virtual MachineIfExecuter[] CreateMachineIfExecuter()
        {
            return new MachineIfExecuter[0];
        }

        public virtual void InitializeMachineIfProcotolHandler()
        {
            throw new NotImplementedException();
        }

        public virtual void InitializeAdditionalUnits() { }

        public void InitalizeInspectRunner()
        {
            inspectRunner = CreateInspectRunner();
            inspectRunner.InspectRunnerExtender = GetInspectRunnerExtender();
        }

        public virtual InspectRunner CreateInspectRunner()
        {
            return new DirectTriggerInspectRunner();
        }

        public virtual InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new InspectRunnerExtender();
        }

        public virtual void Release()
        {
            this.PowerManager.Stop();

            this.currentModel?.CloseModel();

            this.inspectRunner?.Dispose();

            this.deviceController?.Release();

            this.deviceBox?.Release();

            LogHelper.Debug(LoggerType.Shutdown, "Machine is released.");

            LogHelper.Debug(LoggerType.Shutdown, "IoMonitor is released.");

            ThreadManager.StopAllThread();
            ThreadManager.WaitAllDead(1000);
            try
            {
                MatroxHelper.FreeApplication();
            }
            catch(Exception e)
            {
                LogHelper.Debug(LoggerType.Shutdown, $"MatroxHelper.FreeApplication is failed. - {e}");
            }
            LogHelper.Debug(LoggerType.Shutdown, "All Thread are dead.");
        }

        public bool IsCurrentModel(string modelName)
        {
            return (currentModel != null && currentModel.Name == modelName);
        }

        public virtual void InitializeDataExporter()
        {
            DataExporter dataExporter = null;

            dataExporter = new TextProductOverviewDataExport(PathSettings.Instance().Result);
            dataExporterList.Add(dataExporter);

            dataExporter = new TextProductResultDataExport();
            dataExporterList.Add(dataExporter);
        }

        public virtual bool LoadModel(ModelDescription modelDescription)
        {
            try
            {
                currentModel = modelManager.LoadModel(modelDescription, progressForm);
                if (currentModel == null)
                    return false;

                if (deviceController != null )
                    deviceController.OnModelLoaded(currentModel);

                OnModelChanged?.Invoke();
            }
            catch (InvalidModelNameException)
            {
                currentModel = null;
                return false;
            }

            return true;
        }

        public void CloseModel()
        {
            deviceController.OnModelClose(currentModel);
            currentModel?.CloseModel();
            currentModel = null;
        }

        public void StartImageCopy()
        {
            Process[] slideShowProcess = Process.GetProcessesByName("ImageCopyPM");
            if (slideShowProcess.Count() == 0)
            {
                string fileName = Path.Combine(Environment.CurrentDirectory + "\\ImageCopyPM.exe");

                if (File.Exists(fileName) == true)
                    Process.Start(fileName);
            }
        }

        public void ExportData(InspectionResult inspectionResult, CancellationTokenSource cancellationTokenSource = null)
        {
            lastResultPath = inspectionResult.ResultPath;

            if (Directory.Exists(lastResultPath) == false)
                Directory.CreateDirectory(lastResultPath);

            if (cancellationTokenSource != null)
            {
                foreach (DataExporter dataExporter in dataExporterList)
                    dataExporter.Export(inspectionResult, cancellationTokenSource.Token);
            }
            else
            {
                foreach (DataExporter dataExporter in dataExporterList)
                    dataExporter.Export(inspectionResult);
            }
        }

        public string GetImagePath()
        {
            string imagePath;

            if (String.IsNullOrEmpty(lastResultPath) == false && Directory.Exists(lastResultPath))
                imagePath = lastResultPath;
            else
                imagePath = Path.Combine(currentModel.ModelPath, "Image");

            imagePath = Path.GetFullPath(imagePath);
            return imagePath;
        }

        public virtual bool EnterPauseInspection()
        {
            return true;
        }

        /// <summary>
        /// 검사 대기 상태로 들어갈 때 호출
        /// </summary>
        public virtual bool EnterWaitInspection()
        {
            return true;
        }

        /// <summary>
        /// 여러 Trigger Source로 부터 검사를 시작할 때
        /// </summary>
        public virtual bool StartInspection()
        {
            return true;
        }

        public virtual bool StartScan(string scanImagePath)
        {
            return true;
        }
        
        
        /// <summary>
        /// 검사 대기 상태를 해제할 때 호출
        /// </summary>
        public virtual void ExitWaitInspection()
        {

        }
        
        public virtual ICustomConfigPage GetCustomConfigPage()
        {
            return null;
        }

        public bool SetSystemDateTIme(DateTime dateTime)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (ushort)dateTime.Year;
            st.wMonth = (ushort)dateTime.Month;
            st.wDay = (ushort)dateTime.Day;

            st.wHour = (ushort)dateTime.Hour;
            st.wMinute = (ushort)dateTime.Minute;
            st.wSecond = (ushort)dateTime.Second;
            st.wMilliseconds = (ushort)dateTime.Millisecond;

            return SetSystemTime(ref st) != 0;
        }
    }
}
