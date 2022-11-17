using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Settings;

namespace UniScanG.Module.Controller.Device.Laser
{
    class IMElementCollection
    {
        List<IMElement> imElementList;
        int clientCnt = 0;

        public IMElementCollection()
        {
            this.imElementList = new List<IMElement>();
            this.clientCnt = 0;
        }

        public void Add(IMElement imElement)
        {
            if (!this.imElementList.Contains(imElement))
                this.imElementList.Add(imElement);
            UpdateClinetCnt();
        }

        public void Remove(IMElement imElement)
        {
            if (this.imElementList.Contains(imElement))
                this.imElementList.Remove(imElement);
            UpdateClinetCnt();
        }

        private void UpdateClinetCnt()
        {
            List<int> clientList = new List<int>();
            this.imElementList.ForEach(f =>
            {
                if (!clientList.Contains(f.ClientNo))
                    clientList.Add(f.ClientNo);
            });

            this.clientCnt = clientList.Count;
        }

        public IMElement Find(int cameraIndex, int clientIndex)
        {
            return imElementList.Find(f => f.CameraNo == cameraIndex && f.ClientNo == clientIndex);
        }

        public IMElement Find(int portNo)
        {
            return imElementList.Find(f => f.PortNo == portNo);
        }

        public bool[] GetNgState(int ignoreTimeMs)
        {
            bool[] ngState = new bool[this.clientCnt];

            for (int i = 0; i < this.clientCnt; i++)
            {
                List<IMElement> iMElementList = this.imElementList.FindAll(f => f.ClientNo == i);


                bool state = false;

                bool isAnyActive = iMElementList.Any(f => f.IsActive);

                if (isAnyActive)
                // 하나라도 On이면 
                // (가장 최근 활성화 시간 - 가장 오래된 활성화 시간) < LimitTime 이면 무시함.
                {
                    TimeSpan ts = iMElementList.Max(f => f.LastActiveDateTime) - iMElementList.Min(f => f.LastActiveDateTime);
                    state = (ts.TotalMilliseconds > ignoreTimeMs);
                }
                else
                // 다 Off면 off
                {
                    state = false;
                }
                ngState[i] = state;
            }
            return ngState;
        }
    }

    class IMElement
    {
        public int PortNo => this.portNo;
        int portNo;

        public int CameraNo => this.cameraNo;
        int cameraNo;
        public int ClientNo => this.clientNo;
        int clientNo;

        public DateTime LastActiveDateTime => this.lastActiveDateTime;
        DateTime lastActiveDateTime;
        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (value)
                    this.lastActiveDateTime = DateTime.Now;
                this.isActive = value;
            }
        }
        bool isActive;

        public IMElement(int portNo, int cameraNo, int clientNo)
        {
            this.portNo = portNo;
            this.cameraNo = cameraNo;
            this.clientNo = clientNo;
            this.lastActiveDateTime = DateTime.MinValue;
            this.isActive = false;
        }
    }

    public class HanbitLaserExtender
    {
        HanbitLaser hanbitLaser = null;

        IoPort c2lRun = null;
        IoPort[] l2cVisionNg = new IoPort[4];
        bool initialized = false;

        IMElementCollection imElementCollection = new IMElementCollection();

        public bool IsStartRequest { get => isStartRequest; }
        bool isStartRequest;

        public bool UseFromLocal
        {
            get => useFromLocal;
            set
            {
                if (this.useFromLocal != value)
                {
                    bool oldUse = Use;
                    AppendLog("Inspector", UseOrNot(value));

                    this.useFromLocal = value;
                    AppendLog(oldUse, Use);

                    OnStateChanged();
                }
            }
        }
        bool useFromLocal;

        public bool UseFromRemote
        {
            get => useFromRemote;
            set
            {
                if (this.useFromRemote != value)
                {
                    bool oldUse = Use;
                    AppendLog("Printer", UseOrNot(value));

                    this.useFromRemote = value;
                    AppendLog(oldUse, Use);

                    OnStateChanged();
                }
            }
        }
        bool useFromRemote;

        //public bool Use { get => (useFromLocal && useFromRemote); }
        public bool Use
        {
            get
            {
                switch (AdditionalSettings.Instance().LaserSetting.PrinterUseLogic)
                {
                    default:
                    case LaserSetting.EPrinterUseLogic.None:
                        return this.useFromLocal;
                    case LaserSetting.EPrinterUseLogic.And:
                        return this.useFromLocal && this.useFromRemote;
                    case LaserSetting.EPrinterUseLogic.Or:
                        return this.useFromLocal || this.useFromRemote;
                }
            }
        }

        public bool IsRunable => (Use && IsPrepared && this.isStartRequest);

        public bool IsPrepared { get => this.hanbitLaser.IsConnected && !this.hanbitLaser.IsError && this.hanbitLaser.IsReady; }

        public bool[] NgVision => this.ngVision;
        bool[] ngVision = new bool[0];

        public bool NgPrinter => this.ngPrinter;
        bool ngPrinter = false;

        public bool NgSensor => this.ngSensor;
        bool ngSensor = false;

        public int ReqCount { get => reqCount; set => this.reqCount = value; }
        int reqCount = 0;

        public int DoneCount { get => doneCount; set => this.doneCount = value; }
        int doneCount = 0;

        public int OverCount { get => overCount; set => this.overCount = value; }
        int overCount = 0;

        public int GoodCount { get => goodCount; set => this.goodCount = value; }
        int goodCount = 0;

        public bool IsSetRun { get => isSetRun; }
        bool isSetRun;

        public int IgnoreTimeMs { get => ignoreTimeMs; set => this.ignoreTimeMs = value; }
        int ignoreTimeMs = 0;

        public static string UseOrNot(bool v)
        {
            return v ? "Use" : "Unuse";
        }

        public static string EnableOrNot(bool v)
        {
            return v ? "Enable" : "Disable";
        }

        public void AppendLog(string value)
        {
            string logString = string.Format("Laser Setting Changed. ({0})", value);

            ErrorManager.Instance().Report(new AlarmException(ErrorCodeLaser.Instance.Information, ErrorLevel.Info,
                logString, null, ""));
        }

        public void AppendLog(string source, string value)
        {
            string logString = string.Format("Laser Setting Changed. ({0}, {1})", source, value);

            ErrorManager.Instance().Report(new AlarmException(ErrorCodeLaser.Instance.Information, ErrorLevel.Info,
                logString, null, ""));
        }

        private void AppendLog(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                string logString = string.Format("{0} Laser Device.", newValue ? "Enable" : "Disable");

                ErrorManager.Instance().Report(new AlarmException(ErrorCodeLaser.Instance.Information, ErrorLevel.Info,
                    logString, null, ""));
            }
        }

        public void OnDoneChanged()
        {
            if (this.hanbitLaser.IsDone)
            {
                this.doneCount++;
                if (this.doneCount > (this.reqCount + this.overCount))
                    this.overCount++;

                if (this.isSetRun)
                {
                    ProductionG production = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                    production?.IncreseEraseNum();

                    UpdateNgSignal();
                }
            }
        }

        public void OnDoneGoodChanged()
        {
            if (this.hanbitLaser.IsMGood)
            {
                this.goodCount++;

                if (this.isSetRun)
                {
                    ProductionG production = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                    production?.IncreseEraseGood();
                }
            }
        }

        public void ClearDone()
        {
            this.reqCount = this.overCount = this.doneCount = this.goodCount = 0;
            UpdateNgSignal();
        }

        public HanbitLaserExtender(HanbitLaser hanbitLaser)
        {
            this.initialized = false;
            this.hanbitLaser = hanbitLaser;
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmState;

            DeviceBox deviceBox = (DeviceBox)SystemManager.Instance().DeviceBox;
            DeviceController deviceController = (DeviceController)SystemManager.Instance().DeviceController;
            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            this.c2lRun = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserRun);
            deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.c2lRun, IoEventHandlerDirection.OutBound) { OnChanged = C2LRun_OnChanged });

            Dictionary<int, List<int>> portValuePairDic = new Dictionary<int, List<int>>();
            for (int i = 0; i < 4; i++)
            {
                this.l2cVisionNg[i] = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InVisionNg00 + i);
                if (this.l2cVisionNg[i].PortNo == IoPort.UNUSED_PORT_NO)
                    continue;

                deviceController.AddIoEventHandler(new IoEventHandler(digitalIoHandler, this.l2cVisionNg[i], IoEventHandlerDirection.InBound) { OnChanged = l2cVisionNg_OnChanged });

                int cameraNo = (i / 2);
                int clientNo = (i % 2);
                this.imElementCollection.Add(new IMElement(this.l2cVisionNg[i].PortNo, cameraNo, clientNo));
            }

            this.ngVision = new bool[0];
            this.initialized = true;
        }

        private bool C2LRun_OnChanged(IoEventHandler eventSource)
        {
            this.isSetRun = eventSource.IsActivate;
            OnStateChanged();
            return true;
        }

        private bool l2cVisionNg_OnChanged(IoEventHandler eventSource)
        {
            if (!this.isSetRun)
                return true;

            this.imElementCollection.Find(eventSource.IoPort.PortNo).IsActive = eventSource.IsActivate;
            UpdateNgSignal();
            return true;
        }

        private void ErrorManager_OnResetAlarmState()
        {
            Task.Run(() =>
            {
                this.hanbitLaser.SetReset(true);
                System.Threading.Thread.Sleep(500);
                this.hanbitLaser.SetReset(false);

                this.OnStateChanged();
            });
        }

        public bool Start()
        {
            this.isStartRequest = true;

            this.ignoreTimeMs = 0;

            // 패턴 길이 460mm. 1개 패턴 지나가는 시간의 0.5배 미만의 신호는 무시.
            ProductionG production = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (production != null)
                this.ignoreTimeMs = (int)Math.Round(1000f / (production.LineSpeedMpm / 60 / 0.46f) * 0.5f);

            if (Use)
            {
                if (!IsPrepared)
                {
                    throw new AlarmException(ErrorCodeLaser.Instance.InvalidState, ErrorLevel.Error, "Laser Device is not prepared.", null, "");
                    return false;
                }

                //SetRun(true);
            }
            OnStateChanged();
            return true;
        }

        public bool Stop()
        {
            this.isStartRequest = false;
            OnStateChanged();
            this.hanbitLaser.SetMark(false);

            return true;
        }

        public void OnStateChanged()
        {
            if (this.isSetRun)
            // 레이저 작동 중
            {
                // OOR
                if (this.hanbitLaser.IsError)
                {
                    if (this.hanbitLaser.IsOutofMeanderRange)
                        ErrorManager.Instance().Report(ErrorCodeLaser.Instance.OutOfRange, ErrorLevel.Warning, "Laser Device \'Out of Meander Range\' Warning", null);
                    else if (this.hanbitLaser.IsDecelMarkFault)
                        ErrorManager.Instance().Report(ErrorCodeLaser.Instance.FailToDetection, ErrorLevel.Warning, "Laser Device \'Decel Mark Fault\' Warning", null);
                    //else
                    //    ErrorManager.Instance().Report(ErrorSections.Machine, ErrorSubSections.CommonReason, ErrorLevel.Warning, StringManager.GetString("Laser Device Warning"));
                }

                // Ready
                if (!this.hanbitLaser.IsReady)
                    ErrorManager.Instance().Report(ErrorCodeLaser.Instance.InvalidState, ErrorLevel.Warning, "Laser Device is changed to Manual State.", null);

                // Alive timeout
                if (!this.hanbitLaser.IsConnected)
                    ErrorManager.Instance().Report(ErrorCodeLaser.Instance.Timeout, ErrorLevel.Warning, "Laser Device Alive Timeout.", null);
            }
            else if (this.isStartRequest && SystemState.Instance().OpState == UniEye.Base.Data.OpState.Inspect)
            // 레이저 안 작동 중. 검사 상태
            {
                if (Use && !IsPrepared)
                    ErrorManager.Instance().Report(ErrorCodeLaser.Instance.InvalidState, ErrorLevel.Warning, "Laser Device is not prepared.", null);
            }

            bool alarmed = !ErrorManager.Instance().IsCleared();
            SetRun(IsRunable && !alarmed);
            if (this.useFromLocal)
                this.hanbitLaser.SetNotUse(!Use || !IsRunable || alarmed);
            UpdateNgSignal();
        }

        public void LotClear()
        {
            this.hanbitLaser.SetLotClear(true);
        }

        public void SetRun(bool active)
        {
            if (this.c2lRun != null && this.c2lRun.PortNo != IoPort.UNUSED_PORT_NO)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lRun, active);
            else
                this.isSetRun = active;
        }

        public void SetVNG(int cameraNo, int clientNo, bool active)
        {
            //if (!this.isSetRun)
            //    return;

            this.imElementCollection.Find(cameraNo, clientNo).IsActive = active;
            UpdateNgSignal();
        }

        public bool IsSetVNG(int cameraNo, int clientNo)
        {
            return this.imElementCollection.Find(cameraNo, clientNo).IsActive;
        }

        public void SetPNG(bool active)
        {
            if (this.ngPrinter != active)
            {
                this.ngPrinter = active;
                UpdateNgSignal();
            }
        }

        public void SetSNG(bool active)
        {
            if (this.ngSensor != active)
            {
                this.ngSensor = active;
                UpdateNgSignal();
            }
        }

        public void UpdateNgSignal()
        {
            if (!this.initialized)
                return;

            if (this.isSetRun)
            {
                bool[] ngVisions = this.imElementCollection.GetNgState(this.ignoreTimeMs);

                if (this.ngVision.Length != ngVisions.Length)
                    this.ngVision = ngVisions;

                for (int i = 0; i < this.ngVision.Length; i++)
                {
                    if (!this.ngVision[i] && ngVisions[i])
                    {
                        this.reqCount++;
                        ProductionG production = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                        production?.IncreseEraseReq();
                    }
                }

                this.ngVision = ngVisions;
            }

            bool visionNg = (this.reqCount + this.overCount > this.doneCount);
            bool pringerNg = this.ngPrinter;
            bool sensorNg = this.ngSensor;

            bool active = ((visionNg || pringerNg || sensorNg) && this.isSetRun);
           
            if(this.isStartRequest)
                this.hanbitLaser.SetMark(active);
        }
    }

}
