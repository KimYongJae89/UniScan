using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanWPF.Table.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum InPortName
        {
            InEmergency, InSpare, InPower, InAir, InDoor1, InDoor2, InCylinderUp, InCylinderDown
        }

        public enum OutPortName
        {
            OutGrab, OutLampRed, OutLampYellow, OutLampGreen, OutFan, OutDoorLock, OutCylinderUp, OutCylinderDown
        }

        public PortMap() : base()
        {
            Clear();
            Initialize(typeof(InPortName), 16);
            Initialize(typeof(OutPortName), 16);
        }

        public void Initialize(Type ioPortNameType, int offset)
        {
            AddInPorts(ioPortNameType, offset);
            AddOutPorts(ioPortNameType, offset);
        }

        private void AddInPorts(Type ioPortNameType, int offset)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            int index = offset;
            foreach (string name in names)
                if (name.Substring(0, 2) == "In")
                    AddInPort(new IoPort(name, index++));
        }

        private void AddOutPorts(Type ioPortNameType, int offset)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            int index = offset;
            foreach (string name in names)
                if (name.Substring(0, 3) == "Out")
                    AddOutPort(new IoPort(name, index++));
        }
    }
}
