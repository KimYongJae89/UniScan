using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public delegate bool TriggerDelegate(string message);
    public delegate void ClearInspectionResultDelegate();
    public delegate void ScanFinishedDelegate();

    public class FinsMonitor
    {
        public TriggerDelegate Trigger;
        public ClearInspectionResultDelegate ClearInspectionResult;
        public ScanFinishedDelegate ScanFinished;

        FinsInfo finsInfo;

        int plcNodeNo = 0;
        int pcNodeNo = 0;
        SinglePortSocket monitoringSocket;
        FinsPacketParser packetParser;
        Thread monitoringThread;
        bool stopThreadFlag = false;

        bool jobFinish = true;
        bool onProcessing = false;
        bool requestPlcState = true;
        bool inspectionReady = false;
        bool inspectionCompleted = false;
        bool unitResult = false;

        bool scanMode = false;

        int pcStepIndex = 0;

        string totalResult;
        public string TotalResult
        {
            set { totalResult = value; }
        }

        int inspectionState;

        bool resultSent;
        public bool ResultSent
        {
            get { return resultSent; }
            set { resultSent = value; }
        }

        LoggingTarget loggingTarget = null;
        public LoggingTarget LoggingTarget
        {
            get { return loggingTarget; }
            set { loggingTarget = value; }
        }

        string lastPcStateData = "";
        byte lastPlcStateData = 0;
        Stopwatch socketTimer = new Stopwatch();

        public FinsMonitor()
        {
        }

        public void Setup(FinsInfo finsInfo, SinglePortSocket monitoringSocket)
        {
            this.finsInfo = finsInfo;
            this.monitoringSocket = monitoringSocket;
        }

        public void ResetState()
        {
            inspectionState = 0;
        }

        public void StartListening(bool scanMode)
        {
            if (monitoringSocket != null)
            {
                LogHelper.Debug(LoggerType.Network, "Start Fins Monitor");

                inspectionReady = true;
                inspectionCompleted = false;

                this.scanMode = scanMode;
                stopThreadFlag = false;
                monitoringSocket.ClearBuffer();

                monitoringThread = new Thread(new ThreadStart(MonitoringProc));
                monitoringThread.IsBackground = true;
                monitoringThread.Start();
            }
        }

        private void MonitoringProc()
        {
            while (stopThreadFlag == false || onProcessing == true)
            {
                if (onProcessing == true)
                {
                    if (socketTimer.ElapsedMilliseconds > 10000)
                    {
                        LogHelper.Debug(LoggerType.Network, "Packet Failed.");

                        packetParser = null;
                        onProcessing = false;
                        socketTimer.Reset();
                    }
                    Thread.Sleep(50);
                    continue;
                }

                if (plcNodeNo == 0 || pcNodeNo == 0)
                {
                    LogHelper.Debug(LoggerType.Network, "Request Node Address");

                    packetParser = new FinsPacketParser();
                }
                else
                {
                    if (String.IsNullOrEmpty(totalResult) == false)
                    {
                        LogHelper.Debug(LoggerType.Network, "Send Result");

                        packetParser = new FinsPacketParser(finsInfo.NetworkNo, plcNodeNo, finsInfo.NetworkNo, pcNodeNo, FinsDataType.DM_Word, finsInfo.ResultAddress, 0, totalResult);
                    }
                    else if (requestPlcState)
                    {
                        packetParser = new FinsPacketParser(finsInfo.NetworkNo, plcNodeNo, finsInfo.NetworkNo, pcNodeNo, FinsDataType.DM_Word, finsInfo.PlcStateAddress, 0, 2);
                        requestPlcState = false;
                    }
                    else
                    {
                        string pcStateData = GetPcState();
                        if (pcStateData != lastPcStateData)
                        {
                            LogHelper.Debug(LoggerType.Network, "Write PC State : " + pcStateData);

                            packetParser = new FinsPacketParser(finsInfo.NetworkNo, plcNodeNo, finsInfo.NetworkNo, pcNodeNo, FinsDataType.DM_Word, finsInfo.PcStateAddress, 0, pcStateData);

                            lastPcStateData = pcStateData;
                        }
                        requestPlcState = true;
                    }
                }

                if (packetParser != null)
                {
                    onProcessing = true;

                    packetParser.DataReceived += DataReceived;
                    monitoringSocket.SendCommand(packetParser);

                    socketTimer.Restart();
                }

                Thread.Sleep(100);
            }
        }

        public void StopListening()
        {
            scanMode = false;
            inspectionReady = false;
            inspectionCompleted = false;

            stopThreadFlag = true;

            if (monitoringSocket != null)
            {
                while (monitoringThread.IsAlive)
                    Thread.Sleep(100);
            }

            monitoringThread = null;
        }

        public void DataReceived(ReceivedPacket receivedPacket)
        {
            FinsReceivedPacket finsReceivedPacket = (FinsReceivedPacket)receivedPacket;
            if (finsReceivedPacket.RequestCommand == FinsCommandType.RequestAddress)
            {
                plcNodeNo = finsReceivedPacket.PlcNodeNo;
                pcNodeNo = finsReceivedPacket.PcNodeNo;

                LogHelper.Debug(LoggerType.Network, String.Format("Node Number : PLC - {0} / PC - {1}", plcNodeNo, pcNodeNo));
            }
            else if (finsReceivedPacket.RequestCommand == FinsCommandType.ReadData)
            {
                if (finsReceivedPacket.DataAddress == finsInfo.PlcStateAddress)
                {
                    LogHelper.Debug(LoggerType.Network, "Read PLC State Data is completed.");

                    if (lastPlcStateData != finsReceivedPacket.BinaryData[1])
                    {
                        LogHelper.Debug(LoggerType.Network, "Read PLC State : {0:X2}" + finsReceivedPacket.BinaryData[1]);

                        lastPlcStateData = finsReceivedPacket.BinaryData[1];
                        PlcStateReceived(finsReceivedPacket);
                    }
                }
            }
            else if (finsReceivedPacket.RequestCommand == FinsCommandType.WriteData)
            {
                if (finsReceivedPacket.DataAddress == finsInfo.PcStateAddress)
                {
                    LogHelper.Debug(LoggerType.Network, "Writing PC State Data is completed.");

                    if (inspectionCompleted == true)
                    {
                        inspectionReady = true;
                        inspectionCompleted = false;
                    }

                    resultSent = false;
                    jobFinish = false;
                    unitResult = true;
                }
                else if (finsReceivedPacket.DataAddress == finsInfo.ResultAddress)
                {
                    LogHelper.Debug(LoggerType.Network, "Writing Result Data is completed.");

                    totalResult = "";
                    resultSent = true;
                }
            }

            monitoringSocket.PacketHandler.PacketParser = null;

            socketTimer.Stop();

            packetParser = null;
            onProcessing = false;
        }

        private string GetPcState()
        {
            bool ito1stGood = false;
            bool ito1stNg = false;
            bool ito2ndGood = false;
            bool ito2ndNg = false;

            int plcStepIndex = 0;
            if (inspectionCompleted == true)
            {
                plcStepIndex = pcStepIndex;

                if (plcStepIndex > 1)
                {
                    plcStepIndex--;
                }
                else
                {
                    if (plcStepIndex == 0)
                    {
                        ito1stGood = unitResult; ito1stNg = !unitResult;
                    }
                    else if (plcStepIndex == 1)
                    {
                        ito2ndGood = unitResult; ito2ndNg = !unitResult;
                    }
                    plcStepIndex = 0;
                }
            }

            bool alarmBlock = false;

            bool dataSent1 = false;
            bool dataSent2 = false;
            if (inspectionState != 0)
            {
                if (resultSent)
                {
                    if (inspectionState == 1)
                        dataSent1 = true;
                    else
                        dataSent2 = true;

                    inspectionState = 0;
                }
                else
                {
                    alarmBlock = true;
                }
            }

            bool waitInspectionTrigger = (monitoringThread != null);

            byte pcState1 = Convert.ToByte((inspectionReady ? 0x1 : 0) + (inspectionCompleted ? 0x2 : 0) +
                (waitInspectionTrigger ? 0x4 : 0) + (ito1stGood ? 0x8 : 0) + (ito1stNg ? 0x10 : 0) + (ito2ndGood ? 0x20 : 0) + (ito2ndNg ? 0x40 : 0) + (dataSent1 ? 0x80 : 0));
            byte pcState2 = Convert.ToByte((dataSent2 ? 0x1 : 0) + (jobFinish ? 0x2 : 0) + (alarmBlock ? 0x4 : 0));

            return String.Format("{0:X02}{1:X02}00{2:X02}", pcState2, pcState1, plcStepIndex);
        }

        private void PlcStateReceived(FinsReceivedPacket finsReceivedPacket)
        {
            inspectionCompleted = false;
            unitResult = false;

            bool resetReceived = ((finsReceivedPacket.BinaryData[1] & 0x1) == 1);
            bool triggerReceivedFid1 = (((finsReceivedPacket.BinaryData[1] >> 0x1) & 0x1) == 1);
            bool triggerReceivedFid2 = (((finsReceivedPacket.BinaryData[1] >> 0x2) & 0x1) == 1);
            bool triggerReceivedStep1 = (((finsReceivedPacket.BinaryData[1] >> 0x3) & 0x1) == 1);
            bool triggerReceivedStep2 = (((finsReceivedPacket.BinaryData[1] >> 0x4) & 0x1) == 1);
            bool dataReadStep1 = (((finsReceivedPacket.BinaryData[1] >> 0x5) & 0x1) == 1);
            bool dataReadStep2 = (((finsReceivedPacket.BinaryData[1] >> 0x6) & 0x1) == 1);

            bool triggerReceived = triggerReceivedFid1 || triggerReceivedFid2 || triggerReceivedStep1 || triggerReceivedStep2;
            int stepIndex = finsReceivedPacket.BinaryData[2] * 256 + finsReceivedPacket.BinaryData[3];

            if (triggerReceived)
            {
                if (triggerReceivedFid1)
                    stepIndex = 0;
                else if (triggerReceivedFid2)
                    stepIndex = 1;
                else
                    stepIndex++;

                if (scanMode == false && triggerReceivedFid2 == true)
                {
                    LogHelper.Debug(LoggerType.Network, "Trigger START");
                    Trigger("START");
                }

                // 본딩 후 검사시, 이전의 검사 결과를 재사용하여 검사  결과를 저장한다.
                if (stepIndex == 2 && triggerReceivedStep2 == true)
                {
                    if (ClearInspectionResult != null)
                        ClearInspectionResult();
                }

                string triggerMsg = String.Format("TRIG,{0}", stepIndex);
                LogHelper.Debug(LoggerType.Network, triggerMsg);

                bool result = Trigger(triggerMsg);

                inspectionReady = false;
                inspectionCompleted = true;
                pcStepIndex = stepIndex;
                unitResult = result;
            }

            if ((dataReadStep1 == true || dataReadStep2 == true) && inspectionState == 0)
            {
                if (scanMode == false)
                {
                    if (dataReadStep1 == true)
                        inspectionState = 1;
                    else
                        inspectionState = 2;

                    LogHelper.Debug(LoggerType.Network, "Trigger END");
                    Trigger("END");
                }
                else
                {
                    if (ScanFinished != null)
                        ScanFinished();

                    jobFinish = true;
                }
            }
        }
    }
}
