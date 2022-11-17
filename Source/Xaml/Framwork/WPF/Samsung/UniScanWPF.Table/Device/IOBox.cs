using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UniScanWPF.Table.Device
{
    public class IOBox : INotifyPropertyChanged
    {
        DigitalIoHandler ioHandler;

        SolidColorBrush greyBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush greenBrush = new SolidColorBrush(Colors.LightGreen);
        SolidColorBrush redBrush = new SolidColorBrush(Colors.Crimson);
        SolidColorBrush yellowBrush = new SolidColorBrush(Colors.Gold);

        IoPort inEmergency;
        IoPort inPower;
        IoPort inDoor1;
        IoPort inDoor2;
        IoPort inCylinderUp;
        IoPort inCylinderDown;

        bool inOnEmergency;
        bool inOnDoor1;
        bool inOnDoor2;
        bool inOnPower;
        bool inOnCylinderUp;
        bool inOnCylinderDown;

        SolidColorBrush inEmergencyBrush;
        SolidColorBrush inDoorBrush1;
        SolidColorBrush inDoorBrush2;
        SolidColorBrush inPowerBrush;
        SolidColorBrush inCylinderUpBrush;
        SolidColorBrush inCylinderDownBrush;
        
        IoPort outGrab;
        IoPort outLampRed;
        IoPort outLampYellow;
        IoPort outLampGreen;
        IoPort outDoorLock;
        IoPort outCylinderUp;
        IoPort outCylinderDown;
        IoPort outFan;

        bool outOnGrab;
        bool outOnLampRed;
        bool outOnLampYellow;
        bool outOnLampGreen;
        bool outOnDoorLock;
        bool outOnCylinderUp;
        bool outOnCylinderDown;
        bool outOnFan;

        SolidColorBrush outGrabBrush;
        SolidColorBrush outLampRedBrush;
        SolidColorBrush outLampYellowBrush;
        SolidColorBrush outLampGreenBrush;
        SolidColorBrush outDoorLockBrush;
        SolidColorBrush outCylinderUpBrush;
        SolidColorBrush outCylinderDownBrush;
        SolidColorBrush outFanBrush;

        public IoPort OutGrab => this.outGrab;
        public IoPort OutLampRed => this.outLampRed;
        public IoPort OutLampYellow => this.outLampYellow;
        public IoPort OutLampGreen => this.outLampGreen;
        public IoPort OutDoorLock => this.outDoorLock;
        public IoPort OutCylinderUp => this.outCylinderUp;
        public IoPort OutCylinderDown => this.outCylinderDown;
        public IoPort OutFan => this.outFan;


        public IoPort InEmergency => this.inEmergency;
        public IoPort InPower => this.inPower;
        public IoPort InDoor1 => this.inDoor1;
        public IoPort InDoor2 => this.inDoor2;
        public IoPort InCylinderUp => this.inCylinderUp;
        public IoPort InCylinderDown => this.inCylinderDown;

        public bool InOnEmergency
        {
            get => inOnEmergency;
            set
            {
                if (inOnEmergency != value)
                {
                    inOnEmergency = value;
                    inEmergencyBrush = inOnEmergency == true ? redBrush : greyBrush;
                    OnPropertyChanged("InEmergencyBrush");
                }
            }
        }

        public bool OutOnFan
        {
            get => outOnFan;
            set
            {
                if (outOnFan != value)
                {
                    outOnFan = value;
                    outFanBrush = outOnFan ? greenBrush : greyBrush;
                    OnPropertyChanged("OutFanBrush");
                }
            }
        }


        public bool InOnDoor1
        {
            get => inOnDoor1;
            set
            {
                if (inOnDoor1 != value)
                {
                    inOnDoor1 = value;
                    inDoorBrush1 = inOnDoor1 == true ? greenBrush : greyBrush;
                    OnPropertyChanged("InDoorBrush1");
                }
            }
        }

        public bool InOnDoor2
        {
            get => inOnDoor2;
            set
            {
                if (inOnDoor2 != value)
                {
                    inOnDoor2 = value;
                    inDoorBrush2 = inOnDoor2 == true ? greenBrush : greyBrush;
                    OnPropertyChanged("InDoorBrush2");
                }
            }
        }

        public bool InOnPower
        {
            get => inOnPower;
            set
            {
                if (inOnPower != value)
                {
                    inOnPower = value;
                    inPowerBrush = inOnPower == true ? greenBrush : greyBrush;
                    OnPropertyChanged("InPowerBrush");
                }
            }
        }

        public bool InOnCylinderUp
        {
            get => inOnCylinderUp;
            set
            {
                if (inOnCylinderUp != value)
                {
                    inOnCylinderUp = value;
                    inCylinderUpBrush = inOnCylinderUp == true ? greenBrush : greyBrush;
                    OnPropertyChanged("InCylinderUpBrush");
                }
            }
        }

        public bool InOnCylinderDown
        {
            get => inOnCylinderDown;
            set
            {
                if (inOnCylinderDown != value)
                {
                    inOnCylinderDown = value;
                    inCylinderDownBrush = inOnCylinderDown == true ? greenBrush : greyBrush;
                    OnPropertyChanged("InCylinderDownBrush");
                }
            }
        }

        public bool OutOnGrab
        {
            get => outOnGrab;
            set
            {
                if (outOnGrab != value)
                {
                    outOnGrab = value;
                    outGrabBrush = outOnGrab == true ? greenBrush : greyBrush;
                    OnPropertyChanged("OutGrabBrush");
                }
            }
        }

        public bool OutOnLampRed
        {
            get => outOnLampRed;
            set
            {
                if (outOnLampRed != value)
                {
                    outOnLampRed = value;
                    outLampRedBrush = outOnLampRed == true ? redBrush : greyBrush;
                    OnPropertyChanged("OutLampRedBrush");
                }
            }
        }

        public bool OutOnLampYellow
        {
            get => outOnLampYellow;
            set
            {
                if (outOnLampYellow != value)
                {
                    outOnLampYellow = value;
                    outLampYellowBrush = outOnLampYellow == true ? yellowBrush : greyBrush;
                    OnPropertyChanged("OutLampYellowBrush");
                }
            }
        }

        public bool OutOnLampGreen
        {
            get => outOnLampGreen;
            set
            {
                if (outOnLampGreen != value)
                {
                    outOnLampGreen = value;
                    outLampGreenBrush = outOnLampGreen == true ? greenBrush : greyBrush;
                    OnPropertyChanged("OutLampGreenBrush");
                }
            }
        }

        public bool OutOnDoorLock
        {
            get => outOnDoorLock;
            set
            {
                if (outOnDoorLock != value)
                {
                    outOnDoorLock = value;
                    outDoorLockBrush = outOnDoorLock == true ? greenBrush : greyBrush;
                    OnPropertyChanged("OutDoorLockBrush");
                }
            }
        }

        public bool OutOnCylinderUp
        {
            get => outOnCylinderUp;
            set
            {
                if (outOnCylinderUp != value)
                {
                    outOnCylinderUp = value;
                    outCylinderUpBrush = outOnCylinderUp == true ? greenBrush : greyBrush;
                    OnPropertyChanged("OutCylinderUpBrush");
                }
            }
        }

        public bool OutOnCylinderDown
        {
            get => outOnCylinderDown;
            set
            {
                outOnCylinderDown = value;
                outCylinderDownBrush = outOnCylinderDown == true ? greenBrush : greyBrush;
                OnPropertyChanged("OutCylinderDownBrush");
            }
        }

        public SolidColorBrush InEmergencyBrush { get => inEmergencyBrush; }
        public SolidColorBrush InDoorBrush1 { get => inDoorBrush1; }
        public SolidColorBrush InDoorBrush2 { get => inDoorBrush2; }
        public SolidColorBrush InPowerBrush { get => inPowerBrush; }
        public SolidColorBrush InCylinderUpBrush { get => inCylinderUpBrush; }
        public SolidColorBrush InCylinderDownBrush { get => inCylinderDownBrush; }

        public SolidColorBrush OutGrabBrush { get => outGrabBrush; }
        public SolidColorBrush OutLampRedBrush { get => outLampRedBrush; }
        public SolidColorBrush OutLampYellowBrush { get => outLampYellowBrush; }
        public SolidColorBrush OutLampGreenBrush { get => outLampGreenBrush; }
        public SolidColorBrush OutDoorLockBrush { get => outDoorLockBrush; }
        public SolidColorBrush OutCylinderUpBrush { get => outCylinderUpBrush; }
        public SolidColorBrush OutCylinderDownBrush { get => outCylinderDownBrush; }
        public SolidColorBrush OutFanBrush { get => outFanBrush; }
        

        public event PropertyChangedEventHandler PropertyChanged;

        public IOBox()
        {
            ioHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            if (SystemManager.Instance().DeviceBox.PortMap != null)
            {
                PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;

                inEmergency = portMap.GetInPort(PortMap.InPortName.InEmergency);
                inDoor1 = portMap.GetInPort(PortMap.InPortName.InDoor1);
                inDoor2 = portMap.GetInPort(PortMap.InPortName.InDoor2);
                inPower = portMap.GetInPort(PortMap.InPortName.InPower);
                inCylinderUp = portMap.GetInPort(PortMap.InPortName.InCylinderUp);
                inCylinderDown = portMap.GetInPort(PortMap.InPortName.InCylinderDown);

                outGrab = portMap.GetOutPort(PortMap.OutPortName.OutGrab);
                outLampRed = portMap.GetOutPort(PortMap.OutPortName.OutLampRed);
                outLampYellow = portMap.GetOutPort(PortMap.OutPortName.OutLampYellow);
                outLampGreen = portMap.GetOutPort(PortMap.OutPortName.OutLampGreen);
                outDoorLock = portMap.GetOutPort(PortMap.OutPortName.OutDoorLock);
                outCylinderUp = portMap.GetOutPort(PortMap.OutPortName.OutCylinderUp);
                outCylinderDown = portMap.GetOutPort(PortMap.OutPortName.OutCylinderDown);
                outFan = portMap.GetOutPort(PortMap.OutPortName.OutFan);
            }

            InOnEmergency = true;
            InOnDoor1 = true;
            InOnDoor2 = true;
            InOnPower = true;
            InOnCylinderUp = true;
            InOnCylinderDown = true;

            OutOnGrab = true;
            OutOnLampRed = true;
            OutOnLampYellow = true;
            OutOnLampGreen = true;
            OutOnDoorLock = true;
            OutOnCylinderUp = true;
            OutOnCylinderDown = true;
        }

        public void Read()
        {
            if (ioHandler != null)
            {
                uint inputValue = ioHandler.ReadInputGroup(0, 0);
                InOnEmergency = ((inputValue >> inEmergency.PortNo) & 0x1) == 1;
                InOnDoor1 = ((inputValue >> inDoor1.PortNo) & 0x1) == 0;
                InOnDoor2 = ((inputValue >> inDoor2.PortNo) & 0x1) == 0;
                InOnPower = ((inputValue >> inPower.PortNo) & 0x1) == 1;
                InOnCylinderUp = ((inputValue >> inCylinderUp.PortNo) & 0x1) == 1;
                InOnCylinderDown = ((inputValue >> inCylinderDown.PortNo) & 0x1) == 1;

                uint outputValue = ioHandler.ReadOutputGroup(0, 0);
                OutOnGrab = ((outputValue >> outGrab.PortNo) & 0x1) == 1;
                OutOnLampRed = ((outputValue >> outLampRed.PortNo) & 0x1) == 1;
                OutOnLampYellow = ((outputValue >> outLampYellow.PortNo) & 0x1) == 1;
                OutOnLampGreen = ((outputValue >> outLampGreen.PortNo) & 0x1) == 1;
                OutOnDoorLock = ((outputValue >> outDoorLock.PortNo) & 0x1) == 1;
                OutOnCylinderUp = ((outputValue >> outCylinderUp.PortNo) & 0x1) == 1;
                OutOnCylinderDown = ((outputValue >> outCylinderDown.PortNo) & 0x1) == 1;
                OutOnFan = ((outputValue >> outFan.PortNo) & 0x1) == 1;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
