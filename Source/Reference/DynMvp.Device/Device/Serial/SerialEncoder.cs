using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Device.Serial;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;

namespace DynMvp.Device.Serial
{
    public class SerialEncoderInfo : SerialDeviceInfo
    {
        public double InputResolution { get => inputResolution; set => inputResolution = value; }
        double inputResolution = 7;

        //public double OutputResolution { get => outputResolution; set => outputResolution = value; }
        //double outputResolution = 14;

        //public double MulResolution { get => mulResolution; set => mulResolution = value; }
        //double mulResolution = 4;

        // Base
        public string DL { get => this.dl; set => this.dl = value; } // 트리거 딜레이 시간 (클럭 수)
        string dl = "0";

        public string PW { get => this.pw; set => this.pw = value; }  // 트리거 펄스 폭 (클럭 수). 
        string pw = "10"; // 50clk * 20ns/clk = 1.0us

        public string DV { get => this.dv; set => this.dv = value; }  // 입력/출력 분주비
        string dv = "8";

        public string ED { get => this.ed; set => this.ed = value; } // 위치값 증감 방향 (0-역방향 / 1-정방향)
        string ed = "1";

        public string OS { get => this.os; set => this.os = value; } // 트리거 출력 상 (0-Active High / 1- Active Low)
        string os = "0";

        public string AR { get => this.ar; set => this.ar = value; } // 출력 활성화 범위 지정
        string ar = "0,0";

        // V1.07
        public string CY { get => this.cy; set => this.cy = value; } // 펄스 누적 주기
        string cy = "20000000";  // 20000000 * 20ns = 0.4s = 400ms

        // V1.09
        public string DS { get => this.ds; set => this.ds = value; } // 트리거 출력 방향 설정. 
        string ds = "0"; // 0-무관, 1-정방향, 2-역방향


        public SerialEncoderInfo() : base(ESerialDeviceType.SerialEncoder) { }

        public override SerialDevice CreateSerialDevice(bool virtualMode)
        {
            if (virtualMode)
                this.SerialPortInfo.PortName = "Virtual";

            return SerialEncoder.Create(this);
        }

        public override SerialDeviceInfo Clone()
        {
            SerialEncoderInfo serialEncoderInfo = new SerialEncoderInfo();
            serialEncoderInfo.CopyFrom(this);
            return serialEncoderInfo;
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            SerialEncoderInfo serialEncoderInfo = (SerialEncoderInfo)serialDeviceInfo;

            base.CopyFrom(serialDeviceInfo);

            this.inputResolution = serialEncoderInfo.inputResolution;
            // Base
            this.dl = serialEncoderInfo.dl;
            this.pw = serialEncoderInfo.pw; // 50clk * 20ns/clk = 1.0us
            this.dv = serialEncoderInfo.dv;
            this.ed = serialEncoderInfo.ed;
            this.os = serialEncoderInfo.os;
            this.ar = serialEncoderInfo.ar;

            // V1.07
            this.cy = serialEncoderInfo.cy;  // 20000000 * 20ns = 0.4s = 400ms

            // V1.09
            this.ds = serialEncoderInfo.ds; // 0-무관, 1-정방향, 2-역방향
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            XmlHelper.SetValue(xmlElement, "InputResolution", this.inputResolution);

            XmlHelper.SetValue(xmlElement, "DL", this.dl);
            XmlHelper.SetValue(xmlElement, "PW", this.pw);
            XmlHelper.SetValue(xmlElement, "DV", this.dv);
            XmlHelper.SetValue(xmlElement, "ED", this.ed);
            XmlHelper.SetValue(xmlElement, "OS", this.os);
            XmlHelper.SetValue(xmlElement, "AR", this.ar);
            XmlHelper.SetValue(xmlElement, "CY", this.cy);
            XmlHelper.SetValue(xmlElement, "DS", this.ds);
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            this.inputResolution = XmlHelper.GetValue(xmlElement, "InputResolution", this.inputResolution);

            this.dl = XmlHelper.GetValue(xmlElement, "DL", this.dl);
            this.pw = XmlHelper.GetValue(xmlElement, "PW", this.pw);
            this.dv = XmlHelper.GetValue(xmlElement, "DV", this.dv);
            this.ed = XmlHelper.GetValue(xmlElement, "ED", this.ed);
            this.os = XmlHelper.GetValue(xmlElement, "OS", this.os);
            this.ar = XmlHelper.GetValue(xmlElement, "AR", this.ar);
            this.cy = XmlHelper.GetValue(xmlElement, "CY", this.cy);
            this.ds = XmlHelper.GetValue(xmlElement, "DS", this.ds);

            if (!this.ar.Contains(","))
                this.ar = "0,0";

            // 호환성..
            float outputResolution = XmlHelper.GetValue(xmlElement, "OutputResolution", -1f);
            float mulResolution = XmlHelper.GetValue(xmlElement, "MulResolution", -1f);

            if (outputResolution > 0 && mulResolution > 0)
                this.dv = ((uint)Math.Ceiling(outputResolution / inputResolution * mulResolution)).ToString();
        }

        public override string GetPortFindString()
        {
            return "vr\r\n";
        }

        public override bool IsPortFound(byte[] responce)
        {
            string msg = Encoding.Default.GetString(responce).Trim();
            if (msg == "ER,000")
                return true;

            string[] tokens = msg.Split(',');
            if (tokens.Length != 2)
                return false;

            if (tokens[0] != "VR")
                return false;

            return float.TryParse(tokens[1], out float ver);
        }

        public override PacketParser CreatePacketParser()
        {
            SimplePacketParser packetParser = new SimplePacketParser();
            packetParser.EndChar = new byte[2] { (byte)'\r', (byte)'\n' };

            //this.EncodePacket = packetParser.EncodePacket;
            //this.DecodePacket = packetParser.DecodePacket;

            return packetParser;
        }

        public override string GetDeviceString()
        {
            return this.DeviceType.ToString();
        }
    }

    public abstract class SerialEncoder : SerialDevice
    {
        protected List<Type> commTypeList = new List<Type>();

        public abstract string Version { get; }

        public SerialEncoder(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            this.commTypeList = new List<Type>();
        }

        public abstract double GetSpeedPlsPerMs();

        public abstract bool IsCompatible(string command);
        public bool IsCompatible(Enum command)
        {
            return IsCompatible(command.ToString());
        }

        public abstract int GetArgumentSize(Enum command);

        public static SerialEncoder Create(SerialDeviceInfo deviceInfo)
        {
            if (deviceInfo.SerialPortInfo.IsVirtual)
                return new SerialEncoderVirtual(deviceInfo);

            SerialEncoder serialEncoder = null;
            string version = GetVersion(deviceInfo);
            switch (version)
            {
                case "1.05":
                    serialEncoder = new SerialEncoderV105(deviceInfo);
                    break;

                case "1.06":
                case "1.07":
                    serialEncoder = new SerialEncoderV107(deviceInfo);
                    break;

                case "1.08":
                case "1.09":
                default:
                    serialEncoder = new SerialEncoderV109(deviceInfo);
                    break;
            }

            if (serialEncoder == null)
            {
                throw new AlarmException(ErrorCodeSerial.Instance.FailToInitialize, ErrorLevel.Fatal,
                    deviceInfo.DeviceName, "Fail To Initialize", null, "");
                //ErrorManager.Instance().Report(ErrorCodeSerial.Instance.InvalidType, ErrorLevel.Fatal, deviceInfo.DeviceName, errorMessage);
                //serialEncoder = new SerialEncoderVirtual(deviceInfo);
            }

            return serialEncoder;
        }

        private static string GetVersion(SerialDeviceInfo deviceInfo)
        {
            //SimplePacketParser spp = new SimplePacketParser();
            //spp.EndChar = new byte[] { (byte)'\r', (byte)'\n' };
            SerialDevice serialDevice = new SerialDevice(deviceInfo);
            if (serialDevice.Initialize() == false)
                throw new AlarmException(ErrorCodeSerial.Instance.FailToInitialize, ErrorLevel.Fatal, deviceInfo.DeviceName, "PORT Open Fail.", null, "");

            string recive = serialDevice.ExcuteCommand("VR");
            if (recive == "\n")
                recive = serialDevice.ExcuteCommand("VR");
            string[] token = recive?.Split(',');
            serialDevice.Release();

            if (token == null)
                throw new AlarmException(ErrorCodeSerial.Instance.Timeout, ErrorLevel.Fatal, deviceInfo.DeviceName, "Timeout", null, "");
            else if (token.Length != 2)
                throw new AlarmException(ErrorCodeSerial.Instance.FailToReadValue, ErrorLevel.Fatal, deviceInfo.DeviceName, "Unknown Responce", null, "");
            else if (token[0] == "ER")
                return "1.05";
            else
                return token[1].Trim();
        }
    }

    public class SerialEncoderV105 : SerialEncoder
    {
        public enum ECommand
        {
            DL, // 트리거 딜레이 시간 (클럭 수)
            PW, // 트리거 펄스 폭 (클럭 수)
            DV, // 입력/출력 분주비
            FQ, // 가상모드 출력 주파수 (클럭 수)

            ED, // 위치값 증감 방향 (0-역방향 / 1-정방향)
            OS, // 트리거 출력 상 (0-Active High / 1- Active Low)
            EN, // 출력 활성화 (0-Disable / 1-Enable)
            AR, // 출력 활성화 범위 지정
            IN, // 가상모드 적용 (0-Disable / 1-Enable)

            EI,
            EF,
            ES,
            EC,
            EP,

            AP, // 현재 위치 조회
            CP, // 현재 위치 클리어
            GR, // 설정 파라메터 조회
        }

        /// <summary>
        /// PosDiff per Ms
        /// </summary>
        double[] speedBuffer = new double[9];
        int speedBufferIdx = 0;
        ThreadHandler threadHandler = null;

        public override string Version
        {
            get { return "1.05"; }
        }

        public override Enum GetCommand(string command)
        {
            ECommand res;
            if (Enum.TryParse<ECommand>(command, out res))
                return res;
            return null;
        }

        public override int GetArgumentSize(Enum command)
        {
            if (Enum.IsDefined(typeof(ECommand), command.ToString()))
            {
                ECommand comm = (ECommand)command;
                return (comm == ECommand.AR ? 2 : 1);
            }
            return -1;
        }

        public override bool IsCompatible(string command)
        {
            return Enum.GetNames(typeof(ECommand)).Contains(command);
        }

        public SerialEncoderV105(SerialDeviceInfo deviceInfo) : base(deviceInfo) { }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                ExcuteCommand(ECommand.CP);
                ExcuteCommand(ECommand.IN, "0");
                ExcuteCommand(ECommand.EN, "0");

                SerialEncoderInfo info = (SerialEncoderInfo)this.deviceInfo;
                ExcuteCommand(ECommand.DL, info.DL);
                ExcuteCommand(ECommand.PW, info.PW);
                ExcuteCommand(ECommand.DV, info.DV);
                ExcuteCommand(ECommand.ED, info.ED);
                ExcuteCommand(ECommand.OS, info.OS);
                ExcuteCommand(ECommand.AR, info.AR);

                StartSpeedMeasureThread();
            }
            return ok;
        }

        public bool ExcuteAndVerify(Predicate<string> predicate, Enum e, params string[] args)
        {
            if (predicate == null)
                predicate = new Predicate<string>(f => f.StartsWith(e.ToString()));

            string responce = ExcuteCommand(e, args);
            if (string.IsNullOrEmpty(responce))
                return false;

            return predicate(responce);
        }

        protected virtual void StartSpeedMeasureThread()
        {
            threadHandler = new ThreadHandler("SerialEncoderV105", new Thread(SpeedMeasureProc));
            threadHandler.Start();
        }

        public override void Release()
        {
            threadHandler?.Stop(1000);
            threadHandler = null;
            base.Release();
        }

        public override string MakePacket(string command, params string[] args)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append(command.ToString());
            //foreach (string arg in args)
            //{
            //    sb.AppendFormat(",{0}", arg);
            //}
            //return sb.ToString();

            List<string> stringList = new List<string>(args);
            stringList.Insert(0, command);
            return string.Join(",", stringList.ToArray());
        }

        public uint GetPositionPls()
        {
            string[] token = ExcuteCommand(ECommand.AP)?.Split(',');
            uint pos;
            if (token == null || token.Length < 2 || (uint.TryParse(token[1], out pos) == false))
                return 0;
            return pos;
        }

        private void SpeedMeasureProc()
        {
            DateTime startTime = DateTime.Now;
            DateTime prevDateTime = DateTime.Now;
            long prevPos = GetPositionPls();
            System.Diagnostics.Stopwatch commStopwatch = new System.Diagnostics.Stopwatch();

            while (threadHandler.RequestStop == false)
            {
                Thread.Sleep(20);
                long curPos = 0;
                commStopwatch.Restart();
                try
                {
                    //curPos = GetPositionPls();
                }
                catch (TimeoutException)
                {
                    continue;
                }
                DateTime curDateTime = DateTime.Now;
                commStopwatch.Stop();
                long commTime = commStopwatch.ElapsedMilliseconds;

                //long posDiff = Math.Abs(curPos - prevPos);
                //if (Math.Abs(posDiff) > uint.MaxValue / 4)    // overflow or underflow
                //{
                //    prevDateTime = curDateTime;
                //    prevPos = curPos;
                //    continue;
                //}
                long posDiff = curPos - prevPos;
                if (posDiff >= 0)
                {
                    double timeDiff = (curDateTime - prevDateTime).TotalMilliseconds;
                    double curVel = posDiff / timeDiff; // [pulse/milisec]
                                                        //LogHelper.Debug(LoggerType.Grab, string.Format("curVelosity: {0} = {1} / {2}, CommTime: {3}", curVel, posDiff, timeDiff, commTime));

                    // Median Filter
                    this.speedBuffer[this.speedBufferIdx] = curVel;
                    this.speedBufferIdx = (this.speedBufferIdx + 1) % speedBuffer.Count();
                }
                prevDateTime = curDateTime;
                prevPos = curPos;
            }
        }

        private double Median(double[] filterBuffer)
        {
            double[] sortedFilterBuffer = (double[])filterBuffer.Clone();
            Array.Sort(sortedFilterBuffer);

            int halfLen = filterBuffer.Length / 2;
            if ((filterBuffer.Length % 2) == 1)
                return sortedFilterBuffer[halfLen];
            else
                return (sortedFilterBuffer[halfLen] + sortedFilterBuffer[halfLen + 1]) / 2;
        }

        public override double GetSpeedPlsPerMs()
        {
            return Median(this.speedBuffer);
        }
    }

    public class SerialEncoderVirtual : SerialEncoderV105
    {
        Dictionary<ECommand, string> dic = new Dictionary<ECommand, string>();
        ThreadHandler virtualModeThread = null;

        public SerialEncoderVirtual(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            deviceInfo.SerialPortInfo.PortName = "Virtual";

            dic.Add(ECommand.AP, "0");
            dic.Add(ECommand.DL, "0");
            dic.Add(ECommand.PW, "0");
            dic.Add(ECommand.DV, "0");
            dic.Add(ECommand.FQ, "0");
            dic.Add(ECommand.ED, "0");
            dic.Add(ECommand.OS, "0");
            dic.Add(ECommand.EN, "0");
            dic.Add(ECommand.AR, "0,1000000");
            dic.Add(ECommand.IN, "0");
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                virtualModeThread = new ThreadHandler("SerialEncoderVirtual", new Thread(Thread_WorkingThread));
                virtualModeThread.WorkingThread.Start();
            }
            return ok;
        }

        public override void Release()
        {
            base.Release();
            virtualModeThread?.Stop();
            virtualModeThread = null;
        }

        private void Thread_WorkingThread()
        {
            // 80 [m/m]
            // 1.333 [m/s]
            // 7 [um/line]
            // 1142857.142857143 [line/1000ms]
            long virtualCount = 0;
            while (virtualModeThread.RequestStop == false)
            {
                Thread.Sleep(10);

                double step = double.Parse(dic[ECommand.FQ]);
                step /= 100;
                if (step == 0)
                    step = 80.0 / 60.0 / 0.000007 / 100.0;

                if (dic[ECommand.IN] == "1")
                {
                    virtualCount++;
                    dic[ECommand.AP] = ((long)(virtualCount * step)).ToString();
                }
            }
        }

        protected override bool SendCommand(string v)
        {
            Task.Factory.StartNew(() =>
            {
                ProcessCommand(v);
                ReceivedPacket receivedPacket = CreateReceivedPacket(v);
                this.serialPortEx.PacketHandler.PacketParser.OnDataReceived(receivedPacket);
            });
            return true;
        }

        private void ProcessCommand(string v)
        {
            string[] token = v.Trim().Split(',');
            ECommand command = (ECommand)Enum.Parse(typeof(ECommand), token[0]);

            switch (command)
            {
                case ECommand.CP:
                    dic[ECommand.AP] = "0";
                    break;
                case ECommand.AR:
                    dic[command] = token[1] + "," + token[2];
                    break;
                case ECommand.AP:
                case ECommand.GR:
                    break;
                default:
                    dic[command] = token[1];
                    break;
            }
        }

        private ReceivedPacket CreateReceivedPacket(string wirtePacket)
        {
            StringBuilder sb = new StringBuilder();
            string[] token = wirtePacket.Trim().Split(',');
            ECommand command = (ECommand)Enum.Parse(typeof(ECommand), token[0]);
            switch (command)
            {
                case ECommand.CP:
                case ECommand.AP:
                    sb.Append(string.Format("{0},{1}", ECommand.AP, dic[ECommand.AP]));
                    break;

                case ECommand.GR:
                    sb.Append(string.Format("{0},{1},", ECommand.DL, dic[ECommand.DL]));
                    sb.Append(string.Format("{0},{1},", ECommand.PW, dic[ECommand.PW]));
                    sb.Append(string.Format("{0},{1},", ECommand.DV, dic[ECommand.DV]));
                    sb.Append(string.Format("{0},{1},", ECommand.FQ, dic[ECommand.FQ]));
                    sb.Append(string.Format("{0},{1},", ECommand.ED, dic[ECommand.ED]));
                    sb.Append(string.Format("{0},{1},", ECommand.OS, dic[ECommand.OS]));
                    sb.Append(string.Format("{0},{1},", ECommand.EN, dic[ECommand.EN]));
                    sb.Append(string.Format("{0},{1},", ECommand.AR, dic[ECommand.AR]));
                    sb.Append(string.Format("{0},{1}", ECommand.IN, dic[ECommand.IN]));
                    break;

                default:
                    sb.Append(wirtePacket);
                    break;
            }
            byte[] packet = this.serialPortEx.PacketHandler.PacketParser.EncodePacket(sb.ToString());
            string packetString = sb.ToString();
            ReceivedPacket receivedPacket = new ReceivedPacket(packet);

            return receivedPacket;
        }
    }

    public class SerialEncoderV107 : SerialEncoderV105
    {
        public enum ECommandV2
        {
            // Send
            CY, // 펄스 누적 주기 설정
            PC, // 누적 펄스 값 읽기
            CC, // 누적 펄스 클리어

            // Recive
            RC  // PC명령 응답
        }

        public override string Version
        {
            get { return "1.07"; }

        }

        public override Enum GetCommand(string command)
        {
            Enum e = base.GetCommand(command);
            if (e == null)
            {
                ECommandV2 res;
                if (Enum.TryParse<ECommandV2>(command, out res))
                    return res;
                return null;
            }
            return e;
        }

        public override int GetArgumentSize(Enum command)
        {
            int size = base.GetArgumentSize(command);
            if (size >= 0)
                return size;

            if (Enum.IsDefined(typeof(ECommandV2), command.ToString()))
            {
                ECommandV2 comm = (ECommandV2)command;
                return 1;
            }

            return -1;
        }

        public override bool IsCompatible(string command)
        {
            if (base.IsCompatible(command))
                return true;

            return Enum.GetNames(typeof(ECommandV2)).Contains(command);
        }

        public SerialEncoderV107(SerialDeviceInfo deviceInfo) : base(deviceInfo) { }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                ExcuteCommand(ECommandV2.CC);

                SerialEncoderInfo info = (SerialEncoderInfo)this.deviceInfo;
                ExcuteCommand(ECommandV2.CY, info.CY.ToString());
            }
            return ok;
        }

        protected override void StartSpeedMeasureThread() { }

        public override double GetSpeedPlsPerMs()
        {
            string[] paramArray = ExcuteCommand(ECommand.GR)?.Split(',');
            if (paramArray == null)
                return -1;

            int idx = Array.IndexOf(paramArray, "CY");
            if (idx < 0)
                return -1;

            if (!int.TryParse(paramArray[idx + 1], out int timeClk))
                return -1;

            double timeMs = timeClk / 50e6 * 1000;

            string[] token = ExcuteCommand(ECommandV2.PC)?.Split(',');
            if (token == null || token.Length != 2)
                return -1;

            long pls = long.Parse(token[1]);

            return pls / timeMs;
        }
    }

    public class SerialEncoderV109 : SerialEncoderV107
    {
        public enum ECommandV3
        {
            // Send
            DS, // 트리거 출력 방향 설정. 0-무관, 1-정방향, 2-역방향
        }

        public SerialEncoderV109(SerialDeviceInfo deviceInfo) : base(deviceInfo) { }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                SerialEncoderInfo info = (SerialEncoderInfo)this.deviceInfo;
                ExcuteCommand(ECommandV3.DS, info.DS.ToString());
            }
            return ok;
        }

        public override Enum GetCommand(string command)
        {
            Enum e = base.GetCommand(command);
            if (e == null)
            {
                ECommandV3 res;
                if (Enum.TryParse<ECommandV3>(command, out res))
                    return res;
                return null;
            }
            return e;
        }

        public override int GetArgumentSize(Enum command)
        {
            int size = base.GetArgumentSize(command);
            if (size >= 0)
                return size;

            if (Enum.IsDefined(typeof(ECommandV3), command.ToString()))
            {
                ECommandV3 comm = (ECommandV3)command;
                return 1;
            }

            return -1;
        }

        public override bool IsCompatible(string command)
        {
            if (base.IsCompatible(command))
                return true;

            return Enum.GetNames(typeof(ECommandV3)).Contains(command);
        }
    }
}
