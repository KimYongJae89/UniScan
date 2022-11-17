using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Settings;

namespace UniScanG.Module.Controller.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            // in
            InEmergency, InDoorOpenR, InDoorOpenL, InAirPressure,   // TestBed
            InLaserAlive, InLaserReady, InLaserMarkDone, InLaserError, InLaserOutOfMeanderRange, InLaserLotClearDone, InLaserMarkGood, InLaserDecelMarkFault,   // Laser
            InVisionNg00, InVisionNg01, InVisionNg10, InVisionNg11,
            InStickerSensor,    // StickerSensor

            // out
            OutDoorOpen, OutVaccumOn, OutIonizerSol, OutAirFan, OutRoomLight, OutIonizer, OutTowerRed, OutTowerYellow, OutTowerGreen, OutTowerBuzzer,   // TestBed
            OutLaserAlive, OutLaserEmergency, OutLaserReset, OutLaserRun, OutLaserMark, OutLaserLotClear, OutLaserNotuse, // Laser
            OutPowerIM1A, OutPowerIM1B, OutPowerIM2A, OutPowerIM2B, OutPowerStopImg,
        }

        public PortMap() : base()
        {
            this.InPortList.ClearPort();
            this.OutPortList.ClearPort();
            Initialize(typeof(IoPortName));
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            // OutLaserNg -> OutLaserMark 이름 변경
            ChangeName(xmlElement, "Output", "OutLaserNg", "OutLaserMark");
            //IoPort outMarkPort = this.OutPortList.GetIoPort(IoPortName.OutLaserMark.ToString());
            //if (outMarkPort != null && outMarkPort.PortNo == IoPort.UNUSED_PORT_NO)
            //{
            //    XmlElement outPortElement = xmlElement["Outport"];
            //    XmlNodeList xmlNodeList = outPortElement.GetElementsByTagName("Port");
            //    foreach (XmlElement element in xmlNodeList)
            //    {
            //        if (element.ChildNodes[0].InnerText == "OutLaserNG")
            //        {
            //            outMarkPort.LoadXml(element);
            //            outMarkPort.Name = IoPortName.OutLaserMark.ToString();
            //            Save();
            //            break;
            //        }
            //    }
            //}

            // InLaserNotMeanderDetect -> InLaserMarkGood 이름 변경
            ChangeName(xmlElement, "Input", "InLaserNotMeanderDetect", "InLaserMarkGood");
        }

        private void ChangeName(XmlElement xmlElement, string direction, string oldName, string newName)
        {
            Tuple<string, PortList> tuple = null;
            if (direction.StartsWith("In")) tuple = new Tuple<string, PortList>("Inport", this.InPortList);
            if (direction.StartsWith("Out")) tuple = new Tuple<string, PortList>("Outport", this.OutPortList);
            if (tuple == null) return;

            IoPort newIoPort = tuple.Item2.GetIoPort(newName);
            if (newIoPort != null && newIoPort.PortNo == IoPort.UNUSED_PORT_NO)
            {
                XmlElement portElement = xmlElement[tuple.Item1];
                if (portElement == null)
                    return;

                XmlNodeList xmlNodeList = portElement.GetElementsByTagName("Port");
                foreach (XmlElement xmlNode in xmlNodeList)
                {
                    XmlNodeList xmlNodeElementList = xmlNode.GetElementsByTagName("Name");
                    if (xmlNodeElementList.Count == 0)
                        continue;

                    foreach (XmlElement xmlNodeElement in xmlNodeElementList)
                    {
                        if (xmlNodeElement.InnerText == oldName)
                        {
                            newIoPort.LoadXml(xmlNode);
                            newIoPort.Name = newName.ToString();
                            Save();
                            return;
                        }
                    }
                }
            }
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
