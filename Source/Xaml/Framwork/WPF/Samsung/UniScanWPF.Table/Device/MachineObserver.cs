using DynMvp.Base;
using DynMvp.Device.Device.Dio;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Device;
using UniScanWPF.Table.Operation;

namespace UniScanWPF.Table.Device
{
    public class MachineObserver : IDisposable
    {
        MachineObserverSettings settings = new MachineObserverSettings();

        IoPort ioFan;
        IoPort ioLock;
        IoPort ioRed;
        IoPort ioYellow;
        IoPort ioGreen;

        Thread observeThread;
        AxisHandler axisHandler;

        CancellationTokenSource source;
        IOBox ioBox;
        MotionBox motionBox;

        public IOBox IoBox { get => ioBox; }
        public MotionBox MotionBox { get => motionBox; }

        public MachineObserver()
        {
            ioBox = new IOBox();
            motionBox = new MotionBox();

            settings = new MachineObserverSettings();

            settings.Load();

            observeThread = new Thread(ThreadProc);
            observeThread.IsBackground = true;
            observeThread.Priority = ThreadPriority.Highest;
            observeThread.Start();

            PortMap portMap = SystemManager.Instance().DeviceBox.PortMap as PortMap;
            if (portMap != null)
            {
                ioFan = portMap.GetOutPort(PortMap.OutPortName.OutFan);
                ioLock = portMap.GetOutPort(PortMap.OutPortName.OutDoorLock);
                ioRed = portMap.GetOutPort(PortMap.OutPortName.OutLampRed);
                ioYellow = portMap.GetOutPort(PortMap.OutPortName.OutLampYellow);
                ioGreen = portMap.GetOutPort(PortMap.OutPortName.OutLampGreen);
            }

            axisHandler = SystemManager.Instance().DeviceController.RobotStage;
        }

        public void ThreadProc()
        {
            source = new CancellationTokenSource();
            bool inOnPower = true;
            while (source.IsCancellationRequested == false)
            {
                motionBox.Read();
                ioBox.Read();

                if (ioFan != null)
                {
                    if (ioBox.InOnDoor1 == false || ioBox.InOnDoor2 == false)
                    {
                        // 문 열림. 팬 끔.
                        if (SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(ioFan) == true)
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioFan, false);
                    }
                    else
                    {
                        // 문 닫힘. 팬 켬.
                        if (SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(ioFan) == false)
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioFan, true);
                    }
                }

                if (axisHandler != null)
                {
                    if (ErrorManager.Instance().IsAlarmed() == false)
                    {
                        if (ioBox.InOnEmergency)
                        {
                            ErrorManager.Instance().Report(ErrorSectionSafety.Instance.EmergencySwitch.Information, ErrorLevel.Error,
                                "Machine", "Emergency SW Pushed.", null);
                        }

                        if (motionBox.OnMove == true && (ioBox.InOnDoor1 == false || ioBox.InOnDoor2 == false))
                        {
                            ErrorManager.Instance().Report(ErrorSectionSafety.Instance.DoorOpen.Information, ErrorLevel.Error,
                                axisHandler.Name, "Door Open", null);
                        }
                    }

                    if (ErrorManager.Instance().IsAlarmed() == true)
                    {
                        axisHandler.EmergencyStop();
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioLock, false);
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioFan, false);
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioRed, true);
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioYellow, false);
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioGreen, false);

                        SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                    }

                    if (motionBox.OnMove == false && inOnPower == false && ioBox.InOnPower == true)
                    {
                        if (ErrorManager.Instance().IsAlarmed() == true)
                        {
                            ErrorManager.Instance().ResetAllAlarm();
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioRed, false);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioGreen, true);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioYellow, false);
                        }


                        if (ioBox.InOnDoor1 == true && ioBox.InOnDoor2 == true)
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioLock, true);
                            MachineOperator.MoveHome(5000, null, new CancellationTokenSource());
                        }
                    }
                }

                if (ErrorManager.Instance().IsAlarmed() == false)
                {
                    SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioRed, false);
                    if (SystemManager.Instance().OperatorManager != null)
                    {
                        if (SystemManager.Instance().OperatorManager.IsRun)
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioLock, true);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioGreen, true);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioYellow, false);
                        }
                        else
                        {
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioLock, false);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioGreen, false);
                            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioYellow, true);
                        }
                    }
                }

                inOnPower = ioBox.InOnPower;

                Thread.Sleep(settings.CycleTime == 0 ? 100 : settings.CycleTime);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    observeThread.Abort();
                }

                observeThread = null;

                disposedValue = true;
            }
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class MachineObserverSettings
    {
        private string fileName = String.Format(@"{0}\MachineObserverSettings.xml", PathSettings.Instance().Config);

        int cycleTime;

        public int CycleTime { get => cycleTime; set => cycleTime = value; }

        public void Load()
        {
            bool ok = false;
            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Settings"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
            }
            finally
            {
                if (ok == false)
                    Save();
            }
        }

        public void Save()
        {
            if (Directory.Exists(PathSettings.Instance().Config) == false)
                Directory.CreateDirectory(PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(fileName);
        }

        public void Load(XmlElement xmlElement)
        {
            cycleTime = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CycleTime", "100"));
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "CycleTime", cycleTime.ToString());
        }
    }
}
