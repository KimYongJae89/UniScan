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
using UniScanM.MachineIF;
using UniScanM.Operation;
using UniScanM.Settings;

namespace UniScanM.Operation
{
    public enum PLCIndexPos
    {
        StillImage_OnStart = 0, ColorSenseor_OnStart = 2, Gloss_OnStart = 2, EDMS_OnStart = 4, CEDMS_OnStart = 4, Pinhole_OnStart = 6, RVMS_OnStart = 8,
        SP_Speed = 10, PV_Speed = 12, PV_Postion = 16,
        LotNo = 20, Model = 40, Worker = 60, Paste = 80,
        RollDia = 100, RewinderCut = 104, UnwinderCut = 104, RVMSPreOven = 300, RVMSAfterOven = 304,
        MAX_BYTES = 306
    }

    public delegate void MelsecInfoMationChanged(MachineState state);

    [SecurityPermission(SecurityAction.Demand)]
    public class MelsecMonitor : IDisposable
    {
        protected int ioBytes = 2; // word=2Bytes
        //protected IpcClientChannel ipcClientChannel = null;

        protected Random virtualRandom = new Random();
        protected DateTime lastReadTime = DateTime.MinValue;

        protected object lockstate = new object();
        protected MachineState state = null;
        public MachineState State { get => state; }
        public MachineState StateCopy
        {
            get
            {
                MachineState copy = null;
                lock (lockstate)
                    copy = state.DeepCopy();
                return copy;
            }
        }

        //public MelsecInfoMationChanged MelsecInfoMationChanged;

        public MelsecMonitor(int ioBytes = 2)
        {
            //this.ipcClientChannel = new IpcClientChannel();
            //ChannelServices.RegisterChannel(ipcClientChannel, false);
            //RemotingConfiguration.RegisterWellKnownClientType(typeof(MachineState), "ipc://remote/GAE");

            this.state = new MachineState();
            this.ioBytes = ioBytes;
        }

        public virtual void ReadMachineState()
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
#if REMOTE

                    //int line = 1000;
                    //if (state.PvPosition < line)
                    //    state.StillImageOnStart = true;
                    //else state.StillImageOnStart = false;

                    //if (state.PvPosition > line+30)
                    //    state.PvPosition = 0;

#endif

                    double rand1 = virtualRandom.NextDouble() * 0.1 - 0.05;
                    double rand2 = virtualRandom.NextDouble() * 0.1 - 0.05;

                    state.Rvms_BeforePattern = 48000 + (int)Math.Round(rand1 * 100);
                    state.Rvms_AfterPattern = 48000 + (int)Math.Round(rand2 * 100);
                }
                else if (machineIf.IsIdle)
                {
                    MachineIfProtocolResponce protocolResponce = machineIf?.SendCommand(UniScanMMachineIfCommonCommand.GET_MACHINE_STATE);
                    if (protocolResponce != null && protocolResponce.IsGood)
                        ProcessPacket(protocolResponce.ReciveData);
                }
                this.lastReadTime = curTime;
            }
        }

        public virtual void ProcessPacket(string receivedData, ReceivedPacket packet = null)
        {
            try
            {
                lock (lockstate)
                {
                    if (this.ioBytes == 2)
                    {
                        byte[] data = StringHelper.HexStringToByteArray(receivedData);

                        state.StillImageOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.StillImage_OnStart, data));
                        state.ColorSensorOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.ColorSenseor_OnStart, data));
                        state.EdmsOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.EDMS_OnStart, data));
                        state.PinholeOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.Pinhole_OnStart, data));
                        state.RvmsOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.RVMS_OnStart, data));
                        state.CedmsOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.CEDMS_OnStart, data));
                        state.GlossOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.Gloss_OnStart, data));

                        state.SpSpeed = MelsecDataConverter.GetShort((int)PLCIndexPos.SP_Speed, data) / 10.0;
                        state.PvSpeed = MelsecDataConverter.GetShort((int)PLCIndexPos.PV_Speed, data) / 10.0;
                        state.PvPosition = MelsecDataConverter.GetInt((int)PLCIndexPos.PV_Postion, data);
                        state.RollDia = MelsecDataConverter.GetInt((int)PLCIndexPos.RollDia, data) / 100.0;

                        string modelName = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.Model, 20, data).Trim();
                        state.ModelName = modelName;

                        string lotNo = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.LotNo, 20, data).Trim();
                        state.LotNo = lotNo;

                        string worker = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.Worker, 20, data).Trim();
                        state.Worker = worker;

                        state.RewinderCut = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.RewinderCut, data));
                        state.UnwinderCut = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.UnwinderCut, data));
                        state.Rvms_BeforePattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSPreOven, data);
                        state.Rvms_AfterPattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSAfterOven, data);
                    }
                    else if (this.ioBytes == 4)
                    {
                        byte[] data = ABDataConverter.Str2Byte(receivedData);
                        state.StillImageOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.StillImage_OnStart * 2, data));
                        state.ColorSensorOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.ColorSenseor_OnStart * 2, data));
                        state.EdmsOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.EDMS_OnStart * 2, data));
                        state.PinholeOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.Pinhole_OnStart * 2, data));
                        state.RvmsOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.RVMS_OnStart * 2, data));
                        state.CedmsOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.CEDMS_OnStart * 2, data));
                        state.GlossOnStart = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.Gloss_OnStart * 2, data));

                        state.SpSpeed = ABDataConverter.GetInt((int)PLCIndexPos.SP_Speed * 2, data) / 10.0;
                        state.PvSpeed = ABDataConverter.GetInt((int)PLCIndexPos.PV_Speed * 2, data) / 10.0;
                        state.PvPosition = ABDataConverter.GetInt((int)PLCIndexPos.PV_Postion * 2, data);
                        state.RollDia = ABDataConverter.GetInt((int)PLCIndexPos.RollDia * 2, data) / 100.0;

                        string modelName = ABDataConverter.GetString((int)PLCIndexPos.Model * 2, 20 * 2, data).Trim();
                        state.ModelName = modelName;

                        string lotNo = ABDataConverter.GetString((int)PLCIndexPos.LotNo * 2, 20 * 2, data).Trim();
                        state.LotNo = lotNo;

                        string worker = ABDataConverter.GetString((int)PLCIndexPos.Worker * 2, 20 * 2, data).Trim();
                        state.Worker = worker;

                        state.RewinderCut = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.RewinderCut * 2, data));
                        state.UnwinderCut = Convert.ToBoolean(ABDataConverter.GetInt((int)PLCIndexPos.UnwinderCut * 2, data));
                        //state.Rvms_BeforePattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSPreOven * 2, data);
                        //state.Rvms_AfterPattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSAfterOven * 2, data);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "ProcessPacket - Responce buffer length is invalid.");
            }
        }

        public virtual void Dispose()
        {

        }

        protected virtual void Displaynotify()
        {
            try
            {
                AlarmException alarmException = new AlarmException(ErrorSectionSystem.Instance.Comms.Disconnected, ErrorLevel.Error,
                    "Printer Disconnected", null, "");
                ErrorManager.Instance().Report(alarmException);

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

public class MachineState
{
    // UseStillImage = 0, UseColorSenseor = 2, UseEDMS = 4, UsePinhole = 6, UseRVMS = 8
    // SP_Speed = 10, PV_Speed = 12, PV_Postion = 14,
    // LotNo = 18, Model = 38, Worker = 58, Paste = 78,
    // RollDia = 98, RewinderCut = 102

    bool isConnected = false;
    public bool IsConnected { get => isConnected; set => isConnected = value; }

    bool stillImageOnStart = false;
    public bool StillImageOnStart {
        get => stillImageOnStart;
        set => stillImageOnStart = value;
    }

    bool colorSensorOnStart = false;
    public bool ColorSensorOnStart { get => colorSensorOnStart; set => colorSensorOnStart = value; }

    bool edmsOnStart = false;
    public bool EdmsOnStart { get => edmsOnStart; set => edmsOnStart = value; }

    bool pinholeOnStart = false;
    public bool PinholeOnStart { get => pinholeOnStart; set => pinholeOnStart = value; }

    bool rvmsOnStart = false;
    public bool RvmsOnStart { get => rvmsOnStart; set => rvmsOnStart = value; }

    bool cedmsOnStart = false;
    public bool CedmsOnStart { get => cedmsOnStart; set => cedmsOnStart = value; }

    bool glossOnStart = false;
    public bool GlossOnStart { get => glossOnStart; set => glossOnStart = value; }

    // 모델이 있는 프로그램들을 위한 변수 (Gloss)
    bool autoModelChange = false;
    public bool AutoModelChange { get => autoModelChange; set => autoModelChange = value; }

    //  Target SetPoint speed m/min
    double spSpeed = 0;
    public double SpSpeed { get => spSpeed; set => spSpeed = value; }

    //speed m/min
    double pvSpeed = 0;
    public double PvSpeed { get => pvSpeed; set => pvSpeed = value; }

    // m 
    double spPosition = 0;
    public double SpPosition { get => spPosition; set => spPosition = value; }

    //m
    double pvPosition = 0;
    public double PvPosition { get => pvPosition; set => pvPosition = value; }

    // mm
    int spWidth = 0;
    public int SpWidth { get => spWidth; set => spWidth = value; }

    string lotNo = "0A12345BCD"; // model format
    public string LotNo
    {
        get
        {
            return lotNo;
            //if (string.IsNullOrEmpty(lotNo))
            //    lotNo = string.Format("Unkown/{0}", DateTime.Now.ToString("yyMMddHHmm"));

            //else if ( lotNo.IndexOf("Unkown/") >= 0)
            //    lotNo = string.Format("Unkown/{0}", DateTime.Now.ToString("yyMMddHHmm"));

            //return lotNo;
        }
        set { lotNo = value; }
    }

    string modelName = "ABCDEFG00-ABC-AB00CD";
    public string ModelName { get => modelName; set => modelName = value; }

    string worker = "Worker";
    public string Worker { get => worker; set => worker = value; }

    string paste = "ABC00E";
    public string Paste { get => paste; set => paste = value; }

    double rollDia = 150;
    public double RollDia { get => rollDia; set => rollDia = value; }

    bool rewinderCut = false;
    public bool RewinderCut { get => rewinderCut; set => rewinderCut = value; }

    bool unwinderCut = false;
    public bool UnwinderCut { get => unwinderCut; set => unwinderCut = value; }

    int rvms_BeforePattern = 0;
    public int Rvms_BeforePattern { get => rvms_BeforePattern; set => rvms_BeforePattern = value; }

    int rvms_AfterPattern = 0;
    public int Rvms_AfterPattern { get => rvms_AfterPattern; set => rvms_AfterPattern = value; }

    public MachineState()
    {
#if DEBUG
        SpSpeed = PvSpeed = 33;
#endif
    }
    public MachineState(MachineState org)
    {
        this.isConnected = org.isConnected;
        this.stillImageOnStart = org.stillImageOnStart;
        this.colorSensorOnStart = org.colorSensorOnStart;
        this.edmsOnStart = org.edmsOnStart;
        this.pinholeOnStart = org.pinholeOnStart;
        this.rvmsOnStart = org.rvmsOnStart;
        this.cedmsOnStart = org.cedmsOnStart;
        this.glossOnStart = org.glossOnStart;
        this.autoModelChange = org.autoModelChange;

        this.spSpeed = org.spSpeed;
        this.pvSpeed = org.pvSpeed;
        this.spPosition = org.spPosition;
        this.pvPosition = org.pvPosition;

        this.spWidth = org.spWidth;
        this.lotNo = String.Copy(org.lotNo);
        this.modelName = String.Copy(org.modelName);
        this.worker = String.Copy(org.worker);
        this.paste = String.Copy(org.paste);

        this.rollDia = org.rollDia;
        this.rewinderCut = org.rewinderCut;
        this.unwinderCut = org.unwinderCut;
        this.rvms_BeforePattern = org.rvms_BeforePattern;
        this.rvms_AfterPattern = org.rvms_AfterPattern;
    }


    public void Set()
    {
        stillImageOnStart = false;
        colorSensorOnStart = true;
        edmsOnStart = true;
        pinholeOnStart = true;
        rvmsOnStart = true;
        cedmsOnStart = true;
        glossOnStart = true;
        autoModelChange = true;
        rewinderCut = true;
        unwinderCut = true;
    }

    public void Reset()
    {
        stillImageOnStart = false;
        colorSensorOnStart = false;
        edmsOnStart = false;
        pinholeOnStart = false;
        rvmsOnStart = false;
        cedmsOnStart = false;
        glossOnStart = false;
        autoModelChange = false;
        rewinderCut = false;
        unwinderCut = false;
    }

    public void Toggle()
    {
        stillImageOnStart = !stillImageOnStart;
        colorSensorOnStart = !colorSensorOnStart;
        edmsOnStart = !edmsOnStart;
        pinholeOnStart = !pinholeOnStart;
        rvmsOnStart = !rvmsOnStart;
        glossOnStart = !glossOnStart;
        autoModelChange = !autoModelChange;
        cedmsOnStart = !cedmsOnStart;
        rewinderCut = !rewinderCut;
        unwinderCut = !unwinderCut;
    }

    public virtual void Load()
    {
        // 현재는 필요 없어 보이나 마지막 러닝 상태 굳이 확인 한다면 넣어주세요
    }

    public MachineState DeepCopy()
    {
        MachineState copy = new MachineState(this);
        return copy;
    }
}

