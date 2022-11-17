using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using UniScanWPF.Screen.PinHoleColor.Device;

namespace UniScanWPF.Screen.PinHoleColor.Device
{
    public class IoMonitor : INotifyPropertyChanged
    {
        bool machineState = false;

        IoPort inMachinePort;
        IoPort inRollingPort;

        IoPort outRunPort;
        IoPort outSyncPort;
        IoPort outPinHolePort;
        IoPort outColorPort;

        Brush idleBrush;
        Brush inBrush;
        Brush outBrush;

        Brush machineRunBrush;
        Brush machineRollingBrush;

        Brush sensorRunBrush;
        Brush sensorSyncBrush;
        Brush sensorPinHoleBrush;
        Brush sensorColorBrush;

        public Brush MachineRunBrush
        {
            get => machineRunBrush;
            set
            {
                if (machineRunBrush != value)
                {
                    machineRunBrush = value;
                    OnPropertyChanged("MachineRunBrush");
                }
            }
        }
        public Brush MachineRollingBrush
        {
            get => machineRollingBrush;
            set
            {
                if (machineRollingBrush != value)
                {
                    machineRollingBrush = value;
                    OnPropertyChanged("MachineRollingBrush");
                }
            }
        }
        public Brush SensorRunBrush
        {
            get => sensorRunBrush;
            set
            {
                if (sensorRunBrush != value)
                {
                    sensorRunBrush = value;
                    OnPropertyChanged("SensorRunBrush");
                }
            }
        }

        public Brush SensorSyncBrush
        {
            get => sensorSyncBrush;
            set
            {
                if (sensorSyncBrush != value)
                {
                    sensorSyncBrush = value;
                    OnPropertyChanged("SensorSyncBrush");
                }
            }
        }

        public Brush SensorPinHoleBrush
        {
            get => sensorPinHoleBrush;
            set
            {
                if (sensorPinHoleBrush != value)
                {
                    sensorPinHoleBrush = value;
                    OnPropertyChanged("SensorPinHoleBrush");
                }
            }
        }
        public Brush SensorColorBrush
        {
            get => sensorColorBrush;
            set
            {
                if (sensorColorBrush != value)
                {
                    sensorColorBrush = value;
                    OnPropertyChanged("SensorColorBrush");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IoMonitor()
        {
            idleBrush = new SolidColorBrush(Colors.White);
            inBrush = new SolidColorBrush(Colors.LightGreen);
            outBrush = new SolidColorBrush(Colors.Crimson);

            DispatcherTimer timer = new DispatcherTimer();    //객체생성

            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            PortMapBase portMap = SystemManager.Instance().DeviceBox.PortMap;
            inMachinePort = portMap.GetInPort(PortMap.IoPortName.InMachine);
            inRollingPort = portMap.GetInPort(PortMap.IoPortName.InRolling);

            outRunPort = portMap.GetOutPort(PortMap.IoPortName.OutRun);
            outSyncPort = portMap.GetOutPort(PortMap.IoPortName.OutSync);
            outPinHolePort = portMap.GetOutPort(PortMap.IoPortName.OutPinHole);
            outColorPort = portMap.GetOutPort(PortMap.IoPortName.OutColor);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DigitalIoHandler ioHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            bool curState = ioHandler.ReadInput(inMachinePort);

            MachineRunBrush = curState ? inBrush : idleBrush;
            
            SensorRunBrush = ioHandler.ReadOutput(outRunPort) ? inBrush : idleBrush;
            SensorSyncBrush = ioHandler.ReadOutput(outSyncPort) ? inBrush : idleBrush;
            SensorPinHoleBrush = ioHandler.ReadOutput(outPinHolePort) ? outBrush : idleBrush;
            SensorColorBrush = ioHandler.ReadOutput(outColorPort) ? outBrush : idleBrush;

            if (machineState == false && curState == true)
                SystemManager.Instance().InspectRunner.EnterWaitInspection();
            else if (machineState == true && curState == false)
                SystemManager.Instance().InspectRunner.ExitWaitInspection();

            machineState = curState;
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
