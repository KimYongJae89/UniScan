using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;

namespace DynMvp.Devices
{
    public enum TriggerSourceType
    {
        None, Direct, Io, Serial, TcpIpClient, TcpIpServer, Button, Fins
    }

    public struct TriggerResult
    {
        public int stepNo;
        public bool operation;
        public bool inspection;
    }

    public enum VisionState
    {
        Idle, VisionReady, Inspect, Complete
    }

    public enum MachineState
    {
        Idle, Trigger0, Trigger1, MachineDone
    }

    public delegate TriggerResult TriggerHandler(string additionalInfo = "");
    public delegate void StateChangedDelegate();

    public abstract class TriggerSource
    {
        List<Camera> cameraList = new List<Camera>();

        bool onScanImage;
        public bool OnScanImage
        {
            get { return onScanImage; }
            set { onScanImage = value; }
        }

        VisionState visionState = VisionState.Idle;
        public VisionState VisionState
        {
            get { return visionState; }
            set
            {
                visionState = value;
                if (StateChanged != null)
                    StateChanged();
            }
        }

        MachineState machineState = MachineState.Idle;
        public MachineState MachineState
        {
            get { return machineState; }
            set
            {
                machineState = value;
                if (StateChanged != null)
                    StateChanged();
            }
        }

        public TriggerHandler Trigger;
        public StateChangedDelegate StateChanged;

        public abstract void SetupPacketHandler(byte[] startChar, byte[] endChar);
        public abstract void StartProcess(bool scanMode);
        public abstract void StopProcess();
    }

    public class SerialTriggerSource : TriggerSource
    {
        SerialPortEx serialPortEx;

        public SerialTriggerSource(SerialPortEx serialPortEx)
        {
            this.serialPortEx = serialPortEx;

            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataReceived;

            serialPortEx.PacketHandler.PacketParser = packetParser;
        }

        public override void SetupPacketHandler(byte[] startChar, byte[] endChar)
        {
            SimplePacketParser simplePacketParser = (SimplePacketParser)serialPortEx.PacketHandler.PacketParser; ;
            simplePacketParser.StartChar = startChar;
            simplePacketParser.EndChar = endChar;
        }

        public override void StartProcess(bool scanMode)
        {
            serialPortEx.StartListening();
        }

        public override void StopProcess()
        {
            serialPortEx.StopListening();
        }

        public void DataReceived(ReceivedPacket receivedPacket)
        {
            if (Trigger != null)
                Trigger(Encoding.Default.GetString(receivedPacket.ReceivedData));
        }
    }

    public class TcpIpServerTriggerSource : TriggerSource
    {
        SimpleServerSocket simpleSeverSocket;

        public TcpIpServerTriggerSource(SimpleServerSocket simpleServerSocket)
        {
            this.simpleSeverSocket = simpleServerSocket;
        }

        public override void SetupPacketHandler(byte[] startChar, byte[] endChar)
        {
            SimplePacketParser simplePacketParser = (SimplePacketParser )simpleSeverSocket.ListeningPacketHandler.PacketParser; ;
            simplePacketParser.StartChar = startChar;
            simplePacketParser.EndChar = endChar;
        }

        public override void StartProcess(bool scanMode)
        {
            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataReceived;

            simpleSeverSocket.ListeningPacketHandler.PacketParser = packetParser;
        }

        public override void StopProcess()
        {
            simpleSeverSocket.ListeningPacketHandler.PacketParser = null;
        }

        public void DataReceived(ReceivedPacket receivedPacket)
        {
            if (Trigger != null)
                Trigger(Encoding.Default.GetString(receivedPacket.ReceivedData));
        }
    }

    public class TcpIpClientTriggerSource : TriggerSource
    {
        SinglePortSocket simpleSocket;

        public TcpIpClientTriggerSource(SinglePortSocket simpleSocket)
        {
            this.simpleSocket = simpleSocket;
        }

        public override void SetupPacketHandler(byte[] startChar, byte[] endChar)
        {
            SimplePacketParser simplePacketParser = (SimplePacketParser)simpleSocket.PacketHandler.PacketParser;
            simplePacketParser.StartChar = startChar;
            simplePacketParser.EndChar = endChar;
        }

        public override void StartProcess(bool scanMode)
        {
            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataReceived;

            simpleSocket.PacketHandler.PacketParser = packetParser;
        }

        public override void StopProcess()
        {
            simpleSocket.PacketHandler.PacketParser = null;
        }

        public void DataReceived(ReceivedPacket receivedPacket)
        {
            if (Trigger != null)
                Trigger(Encoding.Default.GetString(receivedPacket.ReceivedData));
        }
    }

    public class FinsTriggerSource : TriggerSource
    {
        FinsMonitor finsMonitor;

        public FinsTriggerSource(FinsMonitor finsMonitor)
        {
            this.finsMonitor = finsMonitor;
        }

        public override void SetupPacketHandler(byte[] startChar, byte[] endChar)
        {

        }

        public override void StartProcess(bool scanMode)
        {
            if (finsMonitor != null)
            {
                finsMonitor.StartListening(scanMode);
            }
        }

        public override void StopProcess()
        {
            if (finsMonitor != null)
            {
                finsMonitor.StopListening();
            }
        }
    }

    public class IoTriggerSource : TriggerSource
    {
        DigitalIoHandler digitalIoHandler;
        IoMonitor ioMonitor;

        IoPort visionReady;
        IoPort complete1;
        IoPort complete2;
        IoPort triggerPort1;
        IoPort triggerPort2;
        IoPort machineDonePort;
        IoPort resultNg;

        int lastTriggerNo;

        public IoTriggerSource(DigitalIoHandler digitalIoHandler)
        {
            this.digitalIoHandler = digitalIoHandler;
            ioMonitor = new IoMonitor(digitalIoHandler, null);
            ioMonitor.ProcessInitial += ProcessIoInit;
            ioMonitor.ProcessInputChanged += ProcessIo;
        }

        public void SetupPort(IoPort triggerPort1)
        {
            this.triggerPort1 = triggerPort1;
        }

        public void SetupPort(IoPort visionReady, IoPort complete1, IoPort complete2, IoPort triggerPort1, IoPort triggerPort2, IoPort machineDonePort, IoPort resultNg)
        {
            this.visionReady = visionReady;
            this.complete1 = complete1;
            this.complete2 = complete2;
            this.triggerPort1 = triggerPort1;
            this.triggerPort2 = triggerPort2;
            this.machineDonePort = machineDonePort;
            this.resultNg = resultNg;

            SetIdleState();
        }

        bool ProcessIoInit(DioValue value)
        {
            return true;
        }

        bool ProcessIo(DioValue oldValue, DioValue newValue)
        {
            bool trigger1 = IoMonitor.Check(newValue, triggerPort1);
            bool trigger2 = IoMonitor.Check(newValue, triggerPort2);
            bool machineDone = IoMonitor.Check(newValue, machineDonePort);

            if (trigger1)
            {
                LogHelper.Debug(LoggerType.IO, "Trigger 1 Activated");
                TriggerActivated(0);
            }
            else if (trigger2)
            {
                LogHelper.Debug(LoggerType.IO, "Trigger 2 Activated");
                TriggerActivated(1);
            }
            else if (machineDone)
            {
                LogHelper.Debug(LoggerType.IO, "Machine Done Activated");
                MachineDoneActivated();
            }

            return true;
        }

        private void TriggerActivated(int triggerNo)
        {
            VisionState = VisionState.Inspect;

            if (Trigger == null)
                return;
            if (OnScanImage)
                return;

            if (visionReady.PortNo != -1)
            {
                // 검사가 진행중이더라도 Vision Ready는 끄지 않음.

                //digitalIoHandler.WriteOutput(visionReady, false);

                if (triggerNo == 0)
                {
                    Trigger("START");
                    MachineState = MachineState.Trigger0;
                }
                else
                {
                    MachineState = MachineState.Trigger1;
                }

                TriggerResult triggerResult = Trigger(String.Format("TRIG,{0}", triggerNo));

                LogHelper.Debug(LoggerType.Inspection, String.Format("Trigger No : {0} / Result : {1}", triggerNo, triggerResult.inspection));

                Thread.Sleep(50);

                lastTriggerNo = triggerNo;
            }
            else
            {
                Trigger("TRIG,0");
            }
        }

        private void SetReadyState()
        {
            LogHelper.Debug(LoggerType.Inspection, "Set Ready State");

            VisionState = VisionState.VisionReady;

            digitalIoHandler.WriteOutput(complete1, false);
            digitalIoHandler.WriteOutput(complete2, false);
            digitalIoHandler.WriteOutput(resultNg, false);

            digitalIoHandler.WriteOutput(visionReady, true);
        }

        private void SetIdleState()
        {
            LogHelper.Debug(LoggerType.Inspection, "Set Idle State");

            VisionState = VisionState.Idle;

            digitalIoHandler.WriteOutput(complete1, false);
            digitalIoHandler.WriteOutput(complete2, false);
            digitalIoHandler.WriteOutput(resultNg, false);

            digitalIoHandler.WriteOutput(visionReady, false);
        }

        private void MachineDoneActivated()
        {
            MachineState = MachineState.MachineDone;

            if (lastTriggerNo == 1)
            {
                Thread.Sleep(500);
                Trigger("END");
            }

            SetReadyState();

            if (lastTriggerNo == 1)
                Trigger("FINALIZE");
        }

        public override void SetupPacketHandler(byte[] startChar, byte[] endChar)
        {

        }

        public override void StartProcess(bool scanMode)
        {
            if (ioMonitor != null)
            {
                if (visionReady != null)
                    SetReadyState();

                ioMonitor.Start();
            }
        }

        public override void StopProcess()
        {
            if (ioMonitor != null)
            {
                SetIdleState();

                ioMonitor.Stop();
            }
        }
    }
}
