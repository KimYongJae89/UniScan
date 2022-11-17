using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniScanM.Data;
using UniScanM.Gloss.MachineIF;
using UniScanM.MachineIF;
using UniScanM.Operation;
using UniScanM.Settings;

namespace UniScanM.Gloss.Operation
{
    public enum GlossPLCIndexPos
    {
        GET_START_MACHINE = 0,
        GET_START_COATING = 1,
        GET_START_THICKNESS = 2,
        GET_START_GLOSS = 3,

        GET_AUTO_MODEL_COATING = 4,
        GET_AUTO_MODEL_THICKNESS = 5,
        GET_AUTO_MODEL_GLOSS = 6,

        GET_TARGET_WIDTH = 7,
        GET_TARGET_SPEED = 30,
        GET_PRESENT_SPEED = 31,
        GET_TARGET_POSITION = 32,
        GET_PRESENT_POSITION = 33,
        GET_TARGET_THICKNESS = 34,
        GET_DEFECT_NG_COUNT = 35,

        GET_LOT = 60,
        GET_MODEL = 70,
        GET_WORKER = 80,
        GET_PASTE = 90
    }


    [SecurityPermission(SecurityAction.Demand)]
    public class MelsecMonitor : UniScanM.Operation.MelsecMonitor, IDisposable
    {
        private int readCount = 0;
        private int currentCount = 1;

        public MelsecMonitor(int ioBytes = 2) : base(ioBytes)
        {
            // PNT PLC의 경우 모든 주소가 다 붙어있지 않고 멀리 있어서 카운트를 세 가면서 PLC 데이터를 읽어왔음.
            readCount = Enum.GetNames(typeof(UniScanMMachineIfGlossCommand)).Count();
        }

        public override void ReadMachineState()
        {
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null)
                return;

            if (state.IsConnected && machineIf.IsConnected == false)
                Displaynotify();    // 연결 끊어짐

            DateTime curTime = DateTime.Now;

            lock (lockstate)
            {
                state.IsConnected = machineIf.IsConnected;

                if (state.IsConnected == false)
                {
                    state.Reset();
                }
                else if (machineIf is IVirtualMachineIf)
                {
                    double timeSpanMin = (curTime - this.lastReadTime).TotalMinutes;
                    double distance = state.SpSpeed * timeSpanMin;
                    state.PvPosition += distance;

                    if (state.PvPosition > 1000) //todo if (state.PvPosition > 345) 
                    {
                        state.RewinderCut = state.RewinderCut ? false : true;
                        state.PvPosition = 0;
                    }
                }
                else if (SystemManager.Instance().DeviceBox.MachineIf.IsIdle)
                {
                    if (machineIf.MachineIfSetting.MachineIfType == MachineIfType.Melsec)
                    {
                        UniScanMMachineIfGlossCommand command = (UniScanMMachineIfGlossCommand)currentCount;
                        if (command != UniScanMMachineIfGlossCommand.SET_GLOSS && command != UniScanMMachineIfGlossCommand.SET_TOTAL_GLOSS)
                        {
                            MachineIfProtocolResponce protocolResponce = machineIf?.SendCommand(command);
                            if (protocolResponce != null && protocolResponce.IsGood)
                                ProcessPacket(protocolResponce.ReciveData);
                        }
                        currentCount = (currentCount + 1) % readCount;
                    }
                    else if (machineIf.MachineIfSetting.MachineIfType == MachineIfType.AllenBreadley)
                    {
                        MachineIfProtocolResponce protocolResponce = machineIf?.SendCommand(UniScanMMachineIfCommonCommand.GET_MACHINE_STATE);
                        if (protocolResponce != null && protocolResponce.IsGood)
                            ProcessPacket(protocolResponce.ReciveData);
                    }
                }
                this.lastReadTime = curTime;
            }
        }
        
        public void WriteVisionState()
        {
            if (SystemManager.Instance().DataExporterList.Find(x => x is MachineIfDataExporter) is MachineIfDataExporter machineIfDataExporter)
                machineIfDataExporter.ExportVisionState();
        }

        public override void ProcessPacket(string receivedData, ReceivedPacket packet = null)
        {
            try
            {
                lock (lockstate)
                {
                    if (this.ioBytes == 2) // Melsec
                    {
                        byte[] data = StringHelper.HexStringToByteArray(receivedData);

                        switch (currentCount)
                        {
                            case 0: state.GlossOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort(0, data)); break;
                            case 1: state.UnwinderCut = Convert.ToBoolean(MelsecDataConverter.GetShort(0, data)); break;
                            case 2: state.PvPosition = MelsecDataConverter.GetShort(0, data); break;
                            case 3: state.LotNo = MelsecDataConverter.GetString_LittleEndian(0, 12, data).Trim(); break;
                            case 4: state.ModelName = MelsecDataConverter.GetString_LittleEndian(0, 22, data).Trim(); break;
                            default: break;
                        }
                    }
                    else if (this.ioBytes == 4) // AB
                    {
                        byte[] data = ABDataConverter.Str2Byte(receivedData);
                        state.GlossOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_START_GLOSS * 4, data));

                        state.SpWidth = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_TARGET_WIDTH * 4, data);

                        state.SpSpeed = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_TARGET_SPEED * 4, data) / 10.0f;
                        state.PvSpeed = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_PRESENT_SPEED * 4, data) / 10.0f;
                        state.SpPosition = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_TARGET_POSITION * 4, data) / 1.0f;
                        state.PvPosition = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_PRESENT_POSITION * 4, data) / 1.0f;

                        state.AutoModelChange = Convert.ToBoolean(ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_AUTO_MODEL_GLOSS * 4, data));

                        string lotNo = ABDataConverter.GetString((int)GlossPLCIndexPos.GET_LOT * 4, 10 * 4, data).Trim();
                        state.LotNo = lotNo;

                        string modelName = ABDataConverter.GetString((int)GlossPLCIndexPos.GET_MODEL * 4, 10 * 4, data).Trim();
                        state.ModelName = modelName;

                        int worker = ABDataConverter.GetInt((int)GlossPLCIndexPos.GET_WORKER * 4, data);
                        state.Worker = worker.ToString();

                        string paste = ABDataConverter.GetString((int)GlossPLCIndexPos.GET_PASTE * 4, 10 * 4, data).Trim();
                        state.Paste = paste;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "ProcessPacket - Responce buffer length is invalid.");
            }
        }

        public override void Dispose()
        {

        }

        protected override void Displaynotify()
        {
            try
            {
                //NotifyIcon notifyIcon = new NotifyIcon();
                //notifyIcon.Text = "Export Datatable Utlity";
                //notifyIcon.Visible = true;
                //notifyIcon.BalloonTipTitle = "Error Our System PLC disconnected.";
                //notifyIcon.BalloonTipText = "Click Here to see details";
                //notifyIcon.ShowBalloonTip(10000);
                //notifyIcon.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}

