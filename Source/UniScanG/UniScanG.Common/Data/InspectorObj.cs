using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using UniEye.Base.Data;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Common.Data
{
    public enum CommState
    {
        CONNECTED, // Socket 연결됨
        DISCONNECTED, // Socket 연결 안됨
        OFFLINE // Ping 응답없음
    }

    public enum JobState
    {
        Idle, Run, Done, Error
    }

    public struct JobStruct
    {
        public bool IsJobError => this.jobState == JobState.Error;
        public bool IsJobDone => this.jobState == JobState.Done;
        public bool IsJobRunning => this.jobState == JobState.Run;

        public JobState JobState { get => this.jobState; }
        private JobState jobState;

        public ExchangeCommand JobCommand => this.jobCommand;
        ExchangeCommand jobCommand;

        public object[] JobResult { get => this.jobResult; }
        private object[] jobResult;

        public JobStruct(JobState jobState, ExchangeCommand jobCommand)
        {
            this.jobState = jobState;
            this.jobCommand = jobCommand;
            this.jobResult = null;
        }

        public void SetState(JobState jobState, ExchangeCommand jobCommand, object[] jobResult)
        {
            this.jobState = jobState;
            this.jobCommand = jobCommand;
            this.jobResult = jobResult;
        }

        public void ResetState()
        {
            this.jobState = JobState.Idle;
            this.jobCommand = ExchangeCommand.None;
            this.jobResult = null;
        }
    }

    public class InspectorObj
    {
        private ModelManager modelManager;
        public ModelManager ModelManager
        {
            get { return modelManager; }
        }

        public InspectorInfo Info => this.info;
        private InspectorInfo info;

        public NetworkDrive NetworkDrive => this.networkDrive;
        NetworkDrive networkDrive;

        public ThreadHandler PingThread { get; private set; } = null;

        public CommState CommState { get; set; } = CommState.OFFLINE;

        public bool IsConnected => this.CommState == CommState.CONNECTED;

        public InspectState InspectState { get; set; } = InspectState.Done;

        public OpState OpState { get; set; } = OpState.Idle;

        public string OpMessage { get; set; }

        public string LocaledOpMessage => StringManager.GetString(this.GetType().FullName, this.OpMessage);

        public JobStruct JobState => this.jobState;

        public void AddLoadFactor(float loadFactor)
        {
            lock (this.LoadFactorList)
            {
                DateTime now = DateTime.Now;
                if (loadFactor < 0)
                {
                    this.LoadFactorList.Clear();
                    return;
                }

                if (!this.LoadFactorList.ContainsKey(now))
                    this.LoadFactorList.Add(now, loadFactor);
            }
        }

        JobStruct jobState = new JobStruct();

        public JobStruct ModelSelectState => this.modelSelectState;
        JobStruct modelSelectState = new JobStruct();

        public SortedList<DateTime, float> LoadFactorList { get; private set; } 

        public InspectorObj(InspectorInfo inspectorInfo)
        {
            this.info = inspectorInfo;

            this.CommState = CommState.OFFLINE;

            this.PingThread = new ThreadHandler($"PingThread_{this.Info.GetName()}", new Thread(new ThreadStart(PingThreadProc)) { IsBackground = false }, false);
            this.modelManager = SystemManager.Instance().CreateModelManager();

            this.networkDrive = new NetworkDrive();

            this.LoadFactorList = new SortedList<DateTime, float>();

            Initialize();
        }

        private void PingThreadProc()
        {
            PingOptions options = new PingOptions()
            {
                DontFragment = true
            };
            Ping ping = new Ping();

            while (this.PingThread.RequestStop == false)
            {
                if (this.CommState != CommState.CONNECTED)
                {
                    PingReply reply = ping.Send(this.info.IpAddress, 500, new byte[0], options);
                    if (reply.Status == IPStatus.Success)
                    {
                        if (this.CommState == CommState.OFFLINE)
                            this.CommState = CommState.DISCONNECTED;
                    }
                    else
                    {
                        if (this.CommState == CommState.DISCONNECTED)
                            this.CommState = CommState.OFFLINE;
                    }
                }
                Thread.Sleep(5000);
            }
        }

        ~InspectorObj()
        {
            this.networkDrive.DisconnectNetworkDrive();
        }

        public void Initialize()
        {
            this.PingThread.Start();
            int code = 0;
            //new DynMvp.UI.Touch.SimpleProgressForm(message).Show(() =>
            //{
            //    // 여기서 말고, 검사기 연결되면 연결함.
            //    if (this.info.IpAddress != "127.0.0.1")
            //    {
            //        code = this.networkDrive.ConnectNetworkDrive(null, this.info.Path, this.info.UserId, this.info.UserPw);
            //        if (code != 0)
            //            return;
            //    }
            //    this.modelManager.Init(Path.Combine(this.info.Path, "Model"));
            //});

            if (code != 0)
                throw new AlarmException(ErrorSectionSystem.Instance.Initialize.InvalidSetting, ErrorLevel.Fatal,
                    "{0} Connection Fault - {1}", new object[] { this.info.GetName(), NetworkDrive.GetErrorString(code) }, "Check the Network State and ID/PW");
        }

        public bool Exist(ModelDescription modelDescription)
        {
            return modelManager.IsModelExist(modelDescription);
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            modelManager.DeleteModel(modelDescription);
        }

        public void NewModel(ModelDescription modelDescription)
        {
            if (modelManager.IsModelExist(modelDescription) == true)
                return;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();
            model.Modified = true;
            //AlgorithmPool.Instance().BuildAlgorithmPool();

            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);
        }

        public void Refesh()
        {
            modelManager.Refresh();
        }

        public bool IsTrained(ModelDescription modelDescription)
        {
            try
            {
                //modelManager.Refresh();
            }
            catch (IOException e)
            {
                return true;
            }

            ModelDescription getModelDescription = modelManager.GetModelDescription(modelDescription);

            if (getModelDescription == null)
            {
                return false;

                ModelDescription clone = (ModelDescription)modelDescription.Clone();
                NewModel(clone);
                getModelDescription = modelManager.GetModelDescription(clone);
            }
            return getModelDescription.IsTrained;
        }

        public bool IsInspectable(int sheetNo)
        {
            if (this.info.ClientIndex < 0 || sheetNo < 0)
                return true;

            return (sheetNo % 2) == this.info.ClientIndex;
        }

        public Bitmap GetPreviewImage(ModelDescription modelDescription)
        {
            string imagePath = modelManager.GetPreviewImagePath(modelDescription, "");

            if (!File.Exists(imagePath))
                imagePath = imagePath.Replace(".jpg", ".bmp");

            if (!File.Exists(imagePath))
                return null;

            return (Bitmap)ImageHelper.LoadImage(imagePath);
        }

        public void SetJobState(JobState jobState, ExchangeCommand jobCommand, object[] jobResult)
        {
            switch (jobCommand)
            {
                case ExchangeCommand.M_CREATE:
                case ExchangeCommand.M_SELECT:
                case ExchangeCommand.M_RESELECT:
                case ExchangeCommand.M_CLOSE:
                case ExchangeCommand.M_DELETE:
                    this.modelSelectState.SetState(jobState, jobCommand, jobResult);
                    break;
            }

            this.jobState.SetState(jobState, jobCommand, jobResult);
        }

        public void ResetJobState()
        {
            this.jobState.ResetState();
        }

        public void SetModelSelectState(JobState jobState, object[] jobResult)
        {
            this.modelSelectState.SetState(jobState, ExchangeCommand.M_SELECT, jobResult);
        }

        public void ResetModelSelectState()
        {
            this.modelSelectState.ResetState();
        }
    }
}
