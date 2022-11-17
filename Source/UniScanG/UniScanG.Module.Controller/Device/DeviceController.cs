using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using DynMvp.Device.Device.MotionController;
//using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using UniScanG.Common.Settings;
using UniScanG.Module.Controller.Settings.Monitor;
using UniScanG.Module.Controller.Device.Laser;
using UniScanG.Gravure.Settings;
using UniEye.Base.MachineInterface;
using UniEye.Base.Data;
using UniScanG.Module.Controller.Device.Stage;
using UniScanG.Module.Controller.Device.Sensor;
using UniScanG.Gravure.Device;
using UniScanG.Module.Controller.MachineIF;
using DynMvp.Device.Device.Light;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace UniScanG.Module.Controller.Device
{
    public class DeviceController : DeviceControllerG
    {
        bool lastStartState = false;
        float lastLineSpdMm = 0;

        public HanbitLaser HanbitLaser { get => this.hanbitLaser; }
        HanbitLaser hanbitLaser = null;

        public TestbedStage TestbedStage { get => this.testbedStage; }
        TestbedStage testbedStage = null;

        public StickerSensor StickerSensor { get => this.stickerSensor; }
        StickerSensor stickerSensor = null;

        public DeviceController()
        {
        }

        private void ProductionManager_OnLotChanged()
        {
            hanbitLaser?.HanbitLaserExtender?.LotClear();
        }

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);

            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;

            MachineIfSetting machineIfSetting = deviceBox.MachineIf?.MachineIfSetting;
            UniScanG.MachineIF.MachineIfDataAdapterG adapter = null;
            switch (machineIfSetting?.MachineIfType)
            {
                case null:
                    adapter = new UniScanG.MachineIF.MachineIfDataAdapterG(new UniScanG.Module.Controller.MachineIF.MachineIfData());
                    break;
                case MachineIfType.Melsec:
                    adapter = new UniScanG.Module.Controller.MachineIF.MelsecMachineIfDataAdapterCM(new UniScanG.Module.Controller.MachineIF.MachineIfData());
                    break;
                case MachineIfType.IO:
                    adapter = new UniScanG.Module.Controller.MachineIF.IoMachineIfDataAdapter(new UniScanG.Module.Controller.MachineIF.MachineIfData());
                    break;
                case MachineIfType.AllenBreadley:
                    adapter = new UniScanG.Module.Controller.MachineIF.ABMachineIfDataAdapterCM(new UniScanG.Module.Controller.MachineIF.MachineIfData());
                    break;
            }

            System.Diagnostics.Debug.Assert(adapter != null);
            this.machineIfMonitor = new MachineIfMonitor(adapter);
            this.machineIfMonitor.OnUpdated += MachineIfMonitor_OnUpdated;
            this.machineIfMonitor.Start();


            if (MonitorSystemSettings.Instance().UseTestbedStage)
            {
                this.testbedStage = new TestbedStage(this);
                this.testbedStage.Initialize(deviceBox);
            }

            if (MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.None)
            {
                this.hanbitLaser = HanbitLaser.Create(this);
                AdditionalSettings.Instance().AdditionalSettingChangedDelegate += new UniEye.Base.Settings.AdditionalSettingChangedDelegate(() =>
                 {
                     this.hanbitLaser.HanbitLaserExtender.UseFromLocal = AdditionalSettings.Instance().LaserSetting.Use;
                 });
                this.hanbitLaser.Initialize(deviceBox);
            }

            if (MonitorSystemSettings.Instance().UseStickerSensor)
            {
                this.stickerSensor = new StickerSensor(this);
                this.stickerSensor.OnStickerSensed = new OnStickerSensed((f) =>
                {
                    ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                        "Sticker", "Coating Sticker Detected.", null);
                    lock (this.tupleList)
                        this.tupleList.Add(new Tuple<DateTime, string>(f, "Sticker"));
                });
                AdditionalSettings.Instance().AdditionalSettingChangedDelegate += new UniEye.Base.Settings.AdditionalSettingChangedDelegate(() =>
                {
                    this.stickerSensor.UseFromLocal = AdditionalSettings.Instance().LaserSetting.StickerSensorSetting.Use;
                });
                this.stickerSensor.Initialize(deviceBox);
            }
        }

        private void MachineIfMonitor_OnUpdated()
        {
            float lineSpdMm = this.machineIfMonitor.MachineIfData.GET_PRESENT_SPEED_REAL;
            bool startState = this.machineIfMonitor.MachineIfData.GET_START_GRAVURE_INSPECTOR;
            if (this.lastLineSpdMm != lineSpdMm || this.lastStartState!= startState)
            {
                this.lastLineSpdMm = lineSpdMm;
                this.lastStartState = startState;
                string[] param = new string[] { this.lastStartState.ToString(), this.lastLineSpdMm.ToString() };
                SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.C_SPD, param);
            }

            if (this.testbedStage != null)
            {
                if (this.testbedStage.GetActualVelMpm() == 0)
                    this.testbedStage.SetConvSpeed(this.machineIfMonitor.MachineIfData.GET_PRESENT_SPEED_REAL);
            }
        }

        public override TowerLampState towerLamp_GetDynamicState()
        {
            if (this.testbedStage != null)
                return this.testbedStage.GetTowerlampState();

            return base.towerLamp_GetDynamicState();
        }

        public override bool OnEnterWaitInspection(params object[] args)
        {
            //try
            //{
                bool all = args.Length == 0;
                bool stage = all || args.Contains("Stage");
                bool laser = all || args.Contains("Laser");
                bool encoder = all || args.Contains("Encoder");
                bool light = all || args.Contains("Light");

                if (this.testbedStage != null && stage)
                {
                    this.testbedStage.LockDoor(true);
                    this.testbedStage.UpdateUtility(true);
                    this.testbedStage.UpdateAxisHandler(true);
                }

                if (this.hanbitLaser != null && laser)
                {
                    bool laserUse = this.hanbitLaser.HanbitLaserExtender.Use;
                    if (this.hanbitLaser != null && laserUse)
                    {
                        this.hanbitLaser.HanbitLaserExtender.ClearDone();
                        this.hanbitLaser.HanbitLaserExtender.Start();
                    }
                }

                if (this.stickerSensor != null)
                {
                    this.stickerSensor.UseFromLocal = AdditionalSettings.Instance().LaserSetting.StickerSensorSetting.Use;
                    this.stickerSensor.Start();
                }

                if (encoder)
                    UpdateSerialEncoder(true);

                if (light)
                    UpdateLightCtrl(true);

                return base.OnEnterWaitInspection(args);
            //}
            //catch (AlarmException ex)
            //{
            //    Stop();
            //    ErrorManager.Instance().Report(ex);
            //    return false;
            //}
        }

        public override bool OnExitWaitInspection()
        {
            if (!base.OnExitWaitInspection())
                return false;

            //SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_RESULT_GRAVURE_INSP, "0");
            //SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_NORDEF, "0");

            Stop();

            //MachineIF.MachineIfData machineIfData = this.machineIfMonitor.MachineIfData as MachineIF.MachineIfData;
            //machineIfData?.ClearVisionData();
            //if (machineIfData != null)
            //{
            //    machineIfData.SET_VISION_GRAVURE_INSP_RESULT = false;  // 0(false)이면 정상. 1(true)이면 불량.
            //    machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN = false;
            //    machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF = false;
            //}
            return true;
        }

        private void Stop()
        {
            this.stickerSensor?.Stop();
            this.HanbitLaser?.HanbitLaserExtender.Stop();
            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);

            this.testbedStage?.UpdateAxisHandler(false);
            this.testbedStage?.UpdateUtility(false);
            this.testbedStage?.LockDoor(false);
        }

        private void UpdateSerialEncoder(bool enable)
        {
            SerialDeviceHandler sdh = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialEncoder se = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder) as SerialEncoder;
            if (se != null)
            {
                string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.EN, enable ? "1" : "0")?.Split(',');

                // 응답 확인이 필요하다....
            }
        }

        int lightOnCount = 0;
        private void UpdateLightCtrl(bool enable)
        {
            LightCtrlHandler lch = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            for (int i = 0; i < lch.Count; i++)
            {
                LightCtrl lightCtrl = lch.GetLightCtrl(i);
                if (lightCtrl != null)
                {
                    if (enable)
                    {
                        //LightParam lightParam = GetLightParam(serialLightCtrl);
                        LightParam lightParam = GetLightParam(SystemManager.Instance().CurrentModel.LightParamSet);
                        if (lightParam == null)
                            throw new AlarmException(ErrorCodeLight.Instance.FailToWriteValue, ErrorLevel.Error,
                                "Light Value is not set.", null, "Check the light Value");

                        lightCtrl.TurnOn(lightParam.LightValue);
                        lightOnCount++;
                    }
                    else
                    {
                        lightOnCount--;
                        if (lightOnCount <= 0)
                        {
                            lightCtrl.TurnOff();
                            lightOnCount = 0;
                        }
                    }
                }
            }
        }

        private LightParam GetLightParam(SerialLightCtrl serialLightCtrl)
        {
            LightParam lightParam = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0];

            // Auto-Adjust
            float spd = this.machineIfMonitor.MachineIfData.GET_PRESENT_SPEED_REAL;
            if (spd > 0 && Array.TrueForAll(lightParam.LightValue.Value, f => f == 0))
            {
                float mul = Math.Min(2, (0.5f / 30f) * (spd - 10f) + 1.5f);
                int tempValue = (int)Math.Round(spd * mul);
                //int tempValue = (int)Math.Round(p.LineSpeedMpm * 2);
                //int tempValue = (int)Math.Round(3 * p.LineSpeedMpm - 40);
                byte lightValue = (byte)Math.Max(Math.Min(tempValue, byte.MaxValue), 10);

                lightParam = new LightParam(serialLightCtrl.NumChannel, 0);
                for (int j = 0; j < serialLightCtrl.NumChannel - 1; j++)
                    lightParam.LightValue.Value[j] = lightValue;
            }

            return lightParam;
        }

        public LightParam GetLightParam(LightParamSet lightParamSet)
        {
            try
            {
                LightParam lightParam;

                if (lightParamSet.LightParamList.Count == 0)
                    return null;

                float lineSpeed = this.machineIfMonitor == null ? -1 : this.machineIfMonitor.MachineIfData.GET_TARGET_SPEED_REAL;
                if (lineSpeed < 0)
                    return null;

                lightParam = GetSameLightParam(lightParamSet.LightParamList, lineSpeed);
                if (lightParam == null)
                {
                    LightParam upperLightParam = GetUpperLightParam(lightParamSet.LightParamList, lineSpeed);
                    LightParam lowerLightParam = GetLowerLightParam(lightParamSet.LightParamList, lineSpeed);

                    if (upperLightParam != null && lowerLightParam != null)
                    // 두 부분 선형 보간
                    {
                        lightParam = GetLightParam(lineSpeed, lowerLightParam, upperLightParam);
                    }
                    else if (lowerLightParam == null)
                    // 빠른 두 개로 선형보간
                    {
                        LightParam upperLightParam2 = GetUpperLightParam(lightParamSet.LightParamList, float.Parse(upperLightParam.Name));
                        if (upperLightParam2 != null)
                            lightParam = GetLightParam(lineSpeed, upperLightParam, upperLightParam2);
                        else
                            lightParam = GetLightParam(lineSpeed, new LightParam(lightParamSet.NumLight, 0) { Name = "0" }, upperLightParam);
                    }
                    else if (upperLightParam == null)
                    // 느린 두 개로 선형보간
                    {
                        LightParam lowerLightParam2 = GetLowerLightParam(lightParamSet.LightParamList, float.Parse(lowerLightParam.Name));
                        if (lowerLightParam2 != null)
                            lightParam = GetLightParam(lineSpeed, lowerLightParam2, lowerLightParam);
                        else
                            lightParam = GetLightParam(lineSpeed, new LightParam(lightParamSet.NumLight, 0) { Name = "0" }, lowerLightParam);
                    }
                }

                if (lightParam != null)
                    lightParam = (LightParam)lightParam.Clone();

                // 조명값 1/4 으로... (X,Y Binning)
                if (DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag == "FASTMODE")
                {
                    for (int i = 0; i < lightParam.LightValue.Value.Length; i++)
                        lightParam.LightValue.Value[i] = lightParam.LightValue.Value[i] / 4;
                }

                return lightParam;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, ex.Message);
                throw new AlarmException(ErrorCodeLight.Instance.FailToWriteValue, ErrorLevel.Error,
                    "Light Value Calc Fail.", null, "Check the light Value");
                return new LightParam(lightParamSet.NumLight, 0);
            }
        }

        private LightParam GetLightParam(float lineSpeed, LightParam lowerLightParam, LightParam upperLightParam)
        {
            // 두 부분을 선형 보간
            float lowerSpeed = float.Parse(lowerLightParam.Name);
            float upperSpeed = float.Parse(upperLightParam.Name);
            int numLight = Math.Min(lowerLightParam.LightValue.NumLight, upperLightParam.LightValue.NumLight);
            LightParam newLightParam = new LightParam(numLight);
            newLightParam.Name = lineSpeed.ToString("F1");
            for (int i = 0; i < numLight; i++)
                newLightParam.LightValue.Value[i] = (int)Math.Round(lowerLightParam.LightValue.Value[i] + (upperLightParam.LightValue.Value[i] - lowerLightParam.LightValue.Value[i]) / (upperSpeed - lowerSpeed) * (lineSpeed - lowerSpeed));

            return newLightParam;
        }

        private LightParam GetSameLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.Find(f =>
            {
                float nameValue = float.Parse(f.Name);
                return nameValue == lineSpeed;
            });
        }

        private LightParam GetUpperLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.Find(f =>
            {
                float nameValue = float.Parse(f.Name);
                return nameValue > 0 && nameValue > lineSpeed;
            });
        }

        private LightParam GetLowerLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.FindLast(f =>
            {
                float nameValue = float.Parse(f.Name);
                return nameValue > 0 && nameValue < lineSpeed;
            });
        }


        public override void Release()
        {
            Stop();
            base.Release();
        }

        // 스티커 IO 수신 후 레이저 출력까지 딜레이 시간 계산
        protected override double CalculateOutputDelayMs(float lineSpdMpm)
        {
            double lineSpdMps = Math.Max(0, lineSpdMpm / 60.0);
            if (lineSpdMps == 0)
                return -1;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            double distanceM = additionalSettings.LaserSetting.DistanceM;
            double safeDistanceM = additionalSettings.LaserSetting.StickerSensorSetting.BeforeEraseLengthM;
            double validDistanceM = Math.Max(distanceM - safeDistanceM, 0);

            return validDistanceM / lineSpdMps * 1000;
        }

        // 레이저 출력 High 유지시간 계산
        protected override int GetOutputHoldTimeMs(float lineSpdMpm)
        {
            double lineSpdMps = Math.Max(0, lineSpdMpm / 60.0);
            if (lineSpdMps <= 0)
                return 0;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            double distanceM = additionalSettings.LaserSetting.StickerSensorSetting.EraseLengthM;
            return (int)Math.Round(distanceM / lineSpdMps * 1000);
        }

        protected override bool SetIo(bool active)
        {
            if (this.hanbitLaser != null && this.hanbitLaser.HanbitLaserExtender.IsSetRun)
            {
                this.hanbitLaser.HanbitLaserExtender.SetSNG(active);
                return true;
            }

            return false;
        }

        public override void Shutdown(string imName, bool restart)
        {
            Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            Common.Data.InspectorObj obj = server.GetInspectorList().Find(f => f.Info.GetName() == imName);
            if (obj == null)
                return;

            if (obj.Info.IpAddress == "127.0.0.1")
                return;

            string flag = restart ? "-r" : "-s";
            string[] commands = new string[]
            {
                    $@"net use \\{obj.Info.IpAddress} ""{obj.Info.UserPw}"" /user:{obj.Info.UserId}",
                    $@"shutdown {flag} -f -t 0 -m {obj.Info.IpAddress}",
                    $@"net use /d \\{obj.Info.IpAddress}"
            };
            //System.Windows.Forms.MessageBox.Show(string.Join(Environment.NewLine, commands));

            ProcessStartInfo pri = new ProcessStartInfo();
            Process pro = new Process();

            pri.FileName = @"cmd.exe";
            pri.CreateNoWindow = true;
            pri.UseShellExecute = false;
            pri.Verb = "runas";

            pri.RedirectStandardInput = true;                //표준 출력을 리다이렉트
            pri.RedirectStandardOutput = true;
            pri.RedirectStandardError = true;

            pro.StartInfo = pri;
            pro.Start();   //어플리케이션 실
            
            //if (!obj.IsConnected)
                pro.StandardInput.WriteLine(commands[0]);

            pro.StandardInput.WriteLine(commands[1]);

            //if (!obj.IsConnected)
            pro.StandardInput.WriteLine(commands[2]);

            pro.StandardInput.Close();
            string output = pro.StandardOutput.ReadToEnd();
            string error = pro.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
                System.Windows.Forms.MessageBox.Show(error, obj.Info.IpAddress);
            pro.WaitForExit();

            Thread.Sleep(1000);
        }

        public override void Startup(string imName)
        {
            IoPort imPowerPort = GetIoPowerPort(imName);
            if (imPowerPort == null || imPowerPort.PortNo < 0)
                return;

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            digitalIoHandler.SetOutputActive(imPowerPort);
            Thread.Sleep(1000);
            digitalIoHandler.SetOutputDeactive(imPowerPort);
            Thread.Sleep(1000);
        }

        private IoPort[] GetIoPowerPorts()
        {
            UniEye.Base.Device.DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            List<IoPort> list = new List<IoPort>();
            list.Add(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM1A));
            list.Add(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM1B));
            list.Add(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM2A));
            list.Add(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM2B));
            list.RemoveAll(f => f == null);
            return list.ToArray();
        }

        private IoPort GetIoPowerPort(string imName)
        {
            UniEye.Base.Device.DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            if (imName == "IM1" || imName == "IM1A")
                return deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM1A);
            else if (imName == "IM1B")
                return deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM1B);
            else if (imName == "IM2" || imName == "IM2A")
                return deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM2A);
            else if (imName == "IM2B")
                return deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutPowerIM2B);

            return null;
        }

        public override void Launch(string imName, string[] args)
        {
            Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            Common.Data.InspectorObj obj = server.GetInspectorList().Find(f => f.Info.GetName() == imName);

            string imBin = "UniScanG.Module.Inspector";
            string message = $"{imName};RUN;{imBin};{string.Join(" ", args)}";
            LogHelper.Debug(LoggerType.Device, $"DeviceController::Launch - imName: {imName}, args: {string.Join("/", args)}");
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(obj.Info.IpAddress), 7803);
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.17"), 7803);
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Send(bytes, bytes.Length, iPEndPoint);
                udpClient.Close();
            }            
        }


        public override void InitializeInspectModule(System.Windows.Forms.IWin32Window parent, string imName)
        {
            List<string> argList = new List<string>();
            argList.Add("-Restart");

            foreach (var arg in UniScanG.Program.CommandLineArgs)
            {
                argList.Add(arg.Key);
                if (!string.IsNullOrEmpty(arg.Value))
                    argList.Add(arg.Value);
            }

            Action action = new Action(() =>
            {
                Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                SystemManager.Instance().DeviceController.Launch(imName, argList.ToArray());
            });

            if (parent != null)
                new DynMvp.UI.Touch.SimpleProgressForm().Show(parent, action);
            else
                action.Invoke();
        }

    }
}
