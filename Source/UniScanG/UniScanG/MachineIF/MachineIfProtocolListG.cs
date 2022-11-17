using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanG.MachineIF
{
    public enum MelsecProtocolCommon
    {
        // 설비 상태
        //GET_MACHINE_STATE,
        GET_START_STILLIMAGE,
        GET_START_COLORSENSOR,
        GET_START_EDMS,
        GET_START_PINHOLE,
        GET_START_RVMS,
        GET_TARGET_SPEED,
        GET_PRESENT_SPEED,
        GET_PRESENT_POSITION,
        GET_LOT,
        GET_MODEL,
        GET_WORKER,
        GET_PASTE,
        GET_ROLL_DIAMETER,
        GET_REWINDER_CUT,
        GET_START_GRAVURE_INSPECTOR,
        GET_START_GRAVURE_ERASER,
    };

    public abstract class MachineIfProtocolListG : MachineIfProtocolList
    {
        public MachineIfProtocolListG(MachineIfProtocolListG melsecProtocolList) : base(melsecProtocolList) { }
        public MachineIfProtocolListG(params Type[] protocolListType) : base(protocolListType) { }

        protected void SetDefault(MachineIfProtocol machineIfProtocol, MachineIfType machineIfType)
        {
            MelsecProtocolCommon key = (MelsecProtocolCommon)machineIfProtocol.Command;
            if (machineIfType == MachineIfType.Melsec)
            {
                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    //case MelsecProtocolCommon.GET_MACHINE_STATE:
                    //    address = "D1600"; sizeWord = 100; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_STILLIMAGE:
                        address = "D1600"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_COLORSENSOR:
                        address = "D1601"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_EDMS:
                        address = "D1602"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_PINHOLE:
                        address = "D1603"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_RVMS:
                        address = "D1604"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_TARGET_SPEED:
                        address = "D1605"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_PRESENT_SPEED:
                        address = "D1606"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_PRESENT_POSITION:
                        address = "D1608"; sizeWord = 2; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_LOT:
                        address = "D1610"; sizeWord = 10; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_MODEL:
                        address = "D1620"; sizeWord = 10; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_WORKER:
                        address = "D1630"; sizeWord = 10; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_PASTE:
                        address = "D1640"; sizeWord = 10; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_ROLL_DIAMETER:
                        address = "D1650"; sizeWord = 2; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_REWINDER_CUT:
                        address = "D1652"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR:
                        address = "D1653"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_GRAVURE_ERASER:
                        address = "D1654"; sizeWord = 1; isReadCommand = true; break;

                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)machineIfProtocol;
                melsecMachineIfProtocol.Use = isValid;
                melsecMachineIfProtocol.Address = address;
                melsecMachineIfProtocol.SizeWord = sizeWord;
                melsecMachineIfProtocol.IsReadCommand = isReadCommand;
            }
            else if (machineIfType == MachineIfType.IO)
            {
                IoMachineIfProtocol ioMachineIfProtocol = (IoMachineIfProtocol)machineIfProtocol;
            }
            else if (machineIfType == MachineIfType.AllenBreadley)
            {
                string tagName = "c_PLCtoUniEye";
                int offsetByte4; // 1 = 4byte
                int sizeByte4; // 1 = 4byte
                bool use = true;
                bool isWriteable = false;
                switch (key)
                {
                    //case MelsecProtocolCommon.GET_MACHINE_STATE:
                    //    address = "D1600"; sizeWord = 100; isReadCommand = true; break;
                    case MelsecProtocolCommon.GET_START_STILLIMAGE:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_COLORSENSOR:
                        offsetByte4 = 1; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_EDMS:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_PINHOLE:
                        offsetByte4 = 3; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_RVMS:
                        offsetByte4 = 4; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_TARGET_SPEED:
                        offsetByte4 = 5; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_PRESENT_SPEED:
                        offsetByte4 = 6; sizeByte4 = 2; break;
                    case MelsecProtocolCommon.GET_PRESENT_POSITION:
                        offsetByte4 = 8; sizeByte4 = 2; break;
                    case MelsecProtocolCommon.GET_LOT:
                        offsetByte4 = 10; sizeByte4 = 10; break;
                    case MelsecProtocolCommon.GET_MODEL:
                        offsetByte4 = 20; sizeByte4 = 10; break;
                    case MelsecProtocolCommon.GET_WORKER:
                        offsetByte4 = 30; sizeByte4 = 10; break;
                    case MelsecProtocolCommon.GET_PASTE:
                        offsetByte4 = 40; sizeByte4 = 10; break;
                    case MelsecProtocolCommon.GET_ROLL_DIAMETER:
                        offsetByte4 = 50; sizeByte4 = 2; break;
                    case MelsecProtocolCommon.GET_REWINDER_CUT:
                        offsetByte4 = 52; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR:
                        offsetByte4 = 53; sizeByte4 = 1; break;
                    case MelsecProtocolCommon.GET_START_GRAVURE_ERASER:
                        offsetByte4 = 54; sizeByte4 = 1; break;

                    default:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                }

                AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfProtocol)machineIfProtocol;
                allenBreadleyMachineIfProtocol.Use = use;
                allenBreadleyMachineIfProtocol.TagName = tagName;
                allenBreadleyMachineIfProtocol.OffsetByte4 = offsetByte4;
                allenBreadleyMachineIfProtocol.SizeByte4 = sizeByte4;
                allenBreadleyMachineIfProtocol.IsWriteable = isWriteable;
            }
        }

        public override void CopyFrom(MachineIfProtocolList machineIfProtocolList)
        {
            base.CopyFrom(machineIfProtocolList);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(MelsecProtocolCommon));
            foreach (Enum key in values)
            {
                if (!this.dic.ContainsKey(key))
                    continue;

                MachineIfProtocol machineIfProtocol = this.dic[key];
                SetDefault(machineIfProtocol, machineIfType);
            }
        }

        public override UniEye.Base.MachineInterface.MachineIfProtocol GetProtocol(Enum command)
        {
            //if (command == null)
            //    return GetProtocol(MelsecProtocolCommon.GET_MACHINE_STATE);

            return base.GetProtocol(command);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            var dd = this.dic[MelsecProtocolCommon.GET_START_STILLIMAGE];

            foreach (XmlElement subElement in xmlNodeList)
            {
                MelsecProtocolCommon key;
                bool ok = Enum.TryParse<MelsecProtocolCommon>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                if (ok)
                    this.dic[key].Load(subElement, "Protocol");
            }
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<Enum, UniEye.Base.MachineInterface.MachineIfProtocol> pair in this.dic)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
                xmlElement.AppendChild(subElement);

                XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
                pair.Value.Save(subElement, "Protocol");
            }
        }
    }
}
