using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanM.Data;
using UniScanM.Gloss.Operation;
using UniScanM.Gloss.Service;
using UniScanM.Gloss.Settings;
using UniScanM.Settings;

namespace UniScanM.Gloss.MachineIF
{
    public class PLCInspectStarter : UniScanM.Operation.PLCInspectStarter
    {
        public PLCInspectStarter() : base()
        {
        }

        public override void CreateMelsecMonitor()
        {
            MachineIf mi = SystemManager.Instance().DeviceBox.MachineIf;

            if (mi != null)
            {
                int ioBytes = mi.MachineIfSetting.MachineIfType == MachineIfType.AllenBreadley ? 4 : 2;
                melsecMonitor = new MelsecMonitor(ioBytes);
            }
        }

        public override void Start()
        {
            isInspecting = false;
            RequestStop = false;
            WorkingThread = new System.Threading.Thread(ThreadProc);
            WorkingThread.Priority = ThreadPriority.Lowest;
            WorkingThread.Start();
        }

        protected override void ThreadProc()//********************************************************// ThreadProc()
        {
            //SetFirstValue();

            Thread.Sleep(100);

            while (RequestStop == false)
            {
                Thread.Sleep(250);
                if (ErrorManager.Instance().IsAlarmed() == true)
                {
                    continue;  //??
                }

                //<개선>여기서 한방에 공통사항 다 읽어서 멤버변수에 저장
                //이후 멤버변수를 체크하여 콜백 실행 결정 -> 읽기 네트워크 부하를 줄일수 있음.
                melsecMonitor.ReadMachineState();
                //쓰기도 모아서 한방에?
                if (melsecMonitor is MelsecMonitor glossMelsecMonitor)
                {
                    glossMelsecMonitor.WriteVisionState();
                }

                //0.0 only when autostart mode
                if (startMode == StartMode.Auto)
                {
                    if (isInspecting == false)
                    {
                        //0.주기적으로 Ready 신호 쏘기
                        //SetReadySignal(true); //0: off or manual, 1: on and Auto-mode

                        //1. Gloss Start 감시
                        Check_StartSignal();
                    }
                    //2. 이미 자동시작되어있는 running 중일때임.
                    else
                    {
                        //3. Stop 신호 체크
                        Check_StopSignal();

                        if (melsecMonitor.State.LotNo != preLotNo) // 로트 번호 바뀜
                        {
                            Check_LotChange();// 리와인뜼 컸꽈 깥은 똥짞
                        }

                        //5. SetRunning Signal  //쓰기 한방에? result??
                        //SetRunningSignal(true); //0: idle, 1: run@ only auto-mode, Not Manual-Mode
                    }
                }//if

                //preRewinderCut = melsecMonitor.State.RewinderCut;
                preLotNo = melsecMonitor.State.LotNo;

                //SetVisionState();
            }// while
            isInspecting = false;
        }//*************************************************************************************8   ThreadProc

        public override void SetReadySignal(bool imReady = true) //0: off or manual, 1: on and Auto-mode
        {
            //if(Auto-mode)
            //string mode = automode ? "1" : "0";
            //PLC가 읽고 clear 함으로 매뉴얼 모드일때는 굳이 쓸필요없음.
            //SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfGlossCommand.SET_GLOSS_READY, "1");  // Update PLC
        }

        public override void SetRunningSignal(bool running = true) //0: idle, 1: run@ only auto-mode, Not Manual-Mode
        {
            //SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfGlossCommand.SET_GLOSS_RUN, "1");  // Update PLC
        }

        public override void Check_StartSignal()
        {
            if (GetAutoStartSignal() == true && GetUnwinderCutSignal() == false)
            {
                bool ok = PreStartInspect(true);
                if (ok)
                {
                    if (OnStartInspection != null)
                    {
                        ok = OnStartInspection.Invoke();//Start
                    }
                }
                isInspecting = ok;
            }
        }

        public override void Check_StopSignal()
        {
            if (GetAutoStartSignal() == false || GetUnwinderCutSignal() == true)
            {
                OnStopInspection?.Invoke();//Stop
                isInspecting = false;
            }
        }

        public void Check_LotChange()
        {
            PreStartInspect(true);

            OnLotChanged?.Invoke();
        }

        public override bool GetAutoStartSignal()
        {
            return melsecMonitor.State.GlossOnStart;
        }

        public bool GetUnwinderCutSignal()
        {
            return melsecMonitor.State.UnwinderCut;
        }

        public string GetLotNo(bool autoStart)
        {
            string lotNo = string.Empty;
            if (autoStart)
            {
                lotNo = melsecMonitor.State.LotNo;
            }
            else
            {
                lotNo = GlossSettings.Instance().ManualLotNo = CheckLotNoService.CheckLotNo(GlossSettings.Instance().ManualLotNo);
            }

            if (string.IsNullOrEmpty(lotNo))
            {
                lotNo = string.Format("NewLot.{0}", DateTime.Now.ToString("yyMMddHHmm"));
            }

            return lotNo;
        }

        public override bool PreStartInspect(bool autoStart)//todo ProductionManager
        {
            string modelName = GetModelName();
            string paste = GetPaste();
            string lotNo = GetLotNo(autoStart);
            int rewinderSite = (int)GetRewinderSite();
            int startPosition = (int)GetPosition();
            string worker = GetWorker();
            double lineSpeed = GetLineSpeedSv();
            if (autoStart && (lineSpeed > UniScanMSettings.Instance().MaximumLineSpeed))
            {
                return false;
            }

            SystemManager.Instance().LoadDefaultModel();
            if (SystemManager.Instance().ProductionManager != null)
            {
                int sameCount = SystemManager.Instance().ProductionManager.LotExistCount(modelName, worker, lotNo, paste, (autoStart ? "Auto" : "Manual"), rewinderSite);
                //if (sameCount > 0)
                //    lotNo = string.Format("{0}_{1}", lotNo, sameCount);  //0base-name... lotNo > lotNo_1 > lotNo_2 > lotNo_3 > ....

                if (sameCount == 0)
                {
                    var dataExporter = SystemManager.Instance().DataExporterList.Find(x => x is DataExporter) as DataExporter;
                    dataExporter.ReportFileName = string.Format("{0}.xlsx", lotNo);

                    UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.LotChange(modelName, worker, lotNo, paste, (autoStart ? "Auto" : "Manual"), rewinderSite);
                    lock (production)
                    {
                        production.LastStartPosition = startPosition;

                        if (production.StartPosition < 0)
                        {
                            production.StartPosition = startPosition;
                        }

                        SystemManager.Instance().InspectStarter.OnLotChanged?.Invoke();
                    }
                }
            }

            return true;
        }

        protected override void SetFirstValue()
        {
            string mergeString =
                string.Format("{0}{1}{2}{3}{4}{5}",
                string.Format("{0:X04}", (short)(0))
                , string.Format("{0:X04}", (short)(0))
                , string.Format("{0:X04}", (short)(0))
                , "0000"
                , MelsecDataConverter.WInt((int)(0))
                , MelsecDataConverter.WInt((int)(0))
                );

            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfGlossCommand.SET_GLOSS, mergeString);
        }
    }
}
