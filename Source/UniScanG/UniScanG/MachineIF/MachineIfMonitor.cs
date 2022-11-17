using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.MachineIF
{
    public delegate void OnUpdatedDelegate();

    public abstract class MachineIfMonitor
    {
        public event OnUpdatedDelegate OnUpdated;

        ThreadHandler thread;

        public DateTime LastUpdateTime => this.lastUpdateTime;
        protected DateTime lastUpdateTime;

        public MachineIfData MachineIfData => (MachineIfData)this.adapter.MachineIfData;

        public UniScanG.MachineIF.MachineIfDataAdapterG Adapter => this.adapter;
        protected UniScanG.MachineIF.MachineIfDataAdapterG adapter;


        float virtualAccel = 2.0f;
        float virtualDecel = -8.0f;

        public float VirtualMaxLotLength { get => this.virtualMaxLotLength; set => this.virtualMaxLotLength = value; }
        float virtualMaxLotLength = 3000.0f;

        private bool autoStarted = false;

        public MachineIfMonitor(UniScanG.MachineIF.MachineIfDataAdapterG adapter)
        {
            this.lastUpdateTime = DateTime.Now;
            this.adapter = adapter;
        }


        public void Start()
        {
            if (this.thread == null)
            {
                this.thread = new ThreadHandler("MachineIfMonitor", new System.Threading.Thread(ThreadProc));

                if (SystemManager.Instance().DeviceBox.MachineIf != null)
                    this.thread.Start();
            }
        }

        public void Stop()
        {
            this.thread?.Stop();

            this.thread = null;
        }

        private void ThreadProc()
        {
            bool isVirtual = SystemManager.Instance().DeviceBox.MachineIf.IsVirtual;
            while (!this.thread.RequestStop)
            {
                try
                {
                    bool wasConnect = this.adapter.MachineIfData.IsConnected;
                    bool isConnect = SystemManager.Instance().DeviceBox.MachineIf.IsConnected;
                    if (wasConnect && !isConnect)
                    // 연결 되어 있다가 끊어짐.
                    {
                        ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Disconnected, ErrorLevel.Info, "Printer", "Printer Disconnected.", null, ""));
                        MachineIfData.Reset();
                    }
                    else if (isConnect && !wasConnect)
                    // 끊겨 있다가 연결됨.
                    {
                        ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected, ErrorLevel.Info, "Printer", "Printer Connected.", null, ""));
                    }
                    MachineIfData.IsConnected = isConnect;

                    if (isConnect)
                    {
                        if (isVirtual)
                        {
                            VirtualRead();
                            VirtualWrite();
                        }
                        else
                        {
                            Read();
                            Write();
                        }
                    }

                    OnUpdated?.Invoke();
                    System.Threading.Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, ex);
                }
            }
        }

        public void Read()
        {   
            this.adapter.Read();
            PropagateData();

            this.lastUpdateTime = DateTime.Now;
        }

        public void VirtualRead()
        {
            if (!this.adapter.MachineIfData.IsConnected)
                return;

            MachineIfData machineIfData = this.MachineIfData;

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now - this.lastUpdateTime;

            bool isDecel = (machineIfData.GET_PRESENT_POSITION > virtualMaxLotLength);
            float targetSpd = isDecel ? 0 : MachineIfData.GET_TARGET_SPEED_REAL;
            float spdDiff = targetSpd - MachineIfData.GET_PRESENT_SPEED_REAL;
            if (Math.Round(spdDiff, 1) == 0)
            {
                machineIfData.GET_PRESENT_SPEED_REAL = targetSpd;

                if (isDecel)
                {
                    ChangeRewinder();
                }
                else if (!autoStarted)
                {
                    machineIfData.GET_START_GRAVURE_INSPECTOR = true;
                    autoStarted = true;
                }
            }
            else
            {
                float acc = (float)Math.Round(((spdDiff >= 0 ? Math.Min(spdDiff, virtualAccel) : Math.Max(spdDiff, virtualDecel)) * timeSpan.TotalSeconds), 1);
                machineIfData.GET_PRESENT_SPEED_REAL = Math.Max(0, machineIfData.GET_PRESENT_SPEED_REAL + acc);

                machineIfData.GET_START_GRAVURE_INSPECTOR = false;
                autoStarted = false;
            }

            machineIfData.GET_PRESENT_POSITION += (float)(machineIfData.GET_PRESENT_SPEED_REAL * timeSpan.TotalMinutes);
            machineIfData.GET_START_GRAVURE_ERASER = machineIfData.GET_START_GRAVURE_INSPECTOR;

            PropagateData();

            this.lastUpdateTime = now;
        }

        private void ChangeRewinder()
        {
            MachineIfData machineIfData = this.MachineIfData;

            machineIfData.GET_REWINDER_CUT = !machineIfData.GET_REWINDER_CUT;
            machineIfData.GET_PRESENT_POSITION = 0;

            string lot = machineIfData.GET_LOT;
            if (!string.IsNullOrEmpty(lot))
            {
                int count = 0;
                string body = lot;
                string[] tokens = lot.Split('-');
                if (tokens.Length > 1)
                {
                    body = string.Join("-", tokens.Take(tokens.Length - 1));
                    if (int.TryParse(tokens.LastOrDefault(), out count))
                        count++;
                }
                machineIfData.GET_LOT = string.Format("{0}-{1}", body, count);
            }
        }

        /// <summary>
        /// 읽은 MachineIfData데이터를 각 디바이스에 적용함.
        /// </summary>
        public abstract void PropagateData();


        public void Write()
        {
            ApplyData();
            this.adapter.Write();
        }

        public void VirtualWrite()
        {
            if (!this.adapter.MachineIfData.IsConnected)
                return;

            ApplyData();
        }

        /// <summary>
        /// 쓰기 전, 각 디바이스의 상태를 MachineIfData에 저장함.
        /// </summary>
        public abstract void ApplyData();
    }

}
