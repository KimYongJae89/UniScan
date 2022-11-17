using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices.Dio;
using UniEye.Base.Device;
using System.Xml;

namespace UniScanX.MPAlignment.Devices.DIO
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            // in
            InOrgSW, InEmergency, InDoorOpen, 
            // out
            OutORGLamp, OutDoorMagnet, OutLineLaser, OutDoorBypass,
            OutBackLight, OutTowerRed, OutTowerYellow, OutTowerGreen,
            OutTowerBuzzer
        }

        public PortMap() //: base()
        {
            this.InPortList.ClearPort();
            this.OutPortList.ClearPort();
            Initialize(typeof(IoPortName));
        }

        public override IoPort[] GetTowerLampPort()
        {
            return new IoPort[]
            {
                GetOutPort(IoPortName.OutTowerRed),
                GetOutPort(IoPortName.OutTowerYellow),
                GetOutPort(IoPortName.OutTowerGreen),
                GetOutPort(IoPortName.OutTowerBuzzer)
            };
        }
    }

}