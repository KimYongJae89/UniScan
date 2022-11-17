using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanM.MachineIF
{
    public enum UniScanMMachineIfCommonCommand
    {
        GET_MACHINE_STATE,
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
    };

    public abstract class MachineIfProtocolList : UniEye.Base.MachineInterface.MachineIfProtocolList
    {
        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public MachineIfProtocolList(params Type[] protocolListType) : base(protocolListType) { }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(UniScanMMachineIfCommonCommand));
            foreach(Enum value in values)
            {
                MachineIfProtocol mp = this.dic[value];
                SetDefault(mp, machineIfType);
            }
        }

        private void SetDefault(MachineIfProtocol mp, MachineIfType machineIfType)
        {
            if (machineIfType == MachineIfType.Melsec)
            {
                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (mp.Command)
                {
                    case UniScanMMachineIfCommonCommand.GET_MACHINE_STATE:
                        address = "D1600"; sizeWord = 200; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_START_STILLIMAGE:
                        address = "D1600"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_START_COLORSENSOR:
                        address = "D1601"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_START_EDMS:
                        address = "D1602"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_START_PINHOLE:
                        address = "D1603"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_START_RVMS:
                        address = "D1604"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_TARGET_SPEED:
                        address = "D1605"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_PRESENT_SPEED:
                        address = "D1606"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_PRESENT_POSITION:
                        address = "D1608"; sizeWord = 2; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_LOT:
                        address = "D1610"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_MODEL:
                        address = "D1620"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_WORKER:
                        address = "D1630"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_PASTE:
                        address = "D1640"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_ROLL_DIAMETER:
                        address = "D1650"; sizeWord = 2; isReadCommand = true; break;
                    case UniScanMMachineIfCommonCommand.GET_REWINDER_CUT:
                        address = "D1652"; sizeWord = 1; isReadCommand = true; break;
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                if (isValid == true)
                {
                    MelsecMachineIfProtocol melsecMachineIfProtocol = mp as MelsecMachineIfProtocol;
                    if (melsecMachineIfProtocol == null)
                        return;

                    melsecMachineIfProtocol.Use = true;
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                }
            }
            else if(machineIfType == MachineIfType.AllenBreadley)
            {
                string tagName = "c_PLCtoUniEye";
                int offsetByte4 = 0; // 1 = 4byte
                int sizeByte4 = 0; // 1 = 4byte
                bool use = true;
                bool isWriteable = false;

                switch (mp.Command)
                {
                    case UniScanMMachineIfCommonCommand.GET_MACHINE_STATE:
                        offsetByte4 = 0; sizeByte4 = 53; break;

                    case UniScanMMachineIfCommonCommand.GET_START_STILLIMAGE:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_START_COLORSENSOR:
                        offsetByte4 = 1; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_START_EDMS:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_START_PINHOLE:
                        offsetByte4 = 3; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_START_RVMS:
                        offsetByte4 = 4; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_TARGET_SPEED:
                        offsetByte4 = 5; sizeByte4 = 1; break;
                    case UniScanMMachineIfCommonCommand.GET_PRESENT_SPEED:
                        offsetByte4 = 6; sizeByte4 = 2; break;
                    case UniScanMMachineIfCommonCommand.GET_PRESENT_POSITION:
                        offsetByte4 = 8; sizeByte4 = 2; break;
                    case UniScanMMachineIfCommonCommand.GET_LOT:
                        offsetByte4 = 10; sizeByte4 = 10; break;
                    case UniScanMMachineIfCommonCommand.GET_MODEL:
                        offsetByte4 = 20; sizeByte4 = 10; break;
                    case UniScanMMachineIfCommonCommand.GET_WORKER:
                        offsetByte4 = 30; sizeByte4 = 10; break;
                    case UniScanMMachineIfCommonCommand.GET_PASTE:
                        offsetByte4 = 40; sizeByte4 = 10; break;
                    case UniScanMMachineIfCommonCommand.GET_ROLL_DIAMETER:
                        offsetByte4 = 50; sizeByte4 = 2; break;
                    case UniScanMMachineIfCommonCommand.GET_REWINDER_CUT:
                        offsetByte4 = 52; sizeByte4 = 1; break;
                    default:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                }

                if (use == true)
                {
                    AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfProtocol)mp;
                    allenBreadleyMachineIfProtocol.Use = use;
                    allenBreadleyMachineIfProtocol.TagName = tagName;
                    allenBreadleyMachineIfProtocol.OffsetByte4 = offsetByte4;
                    allenBreadleyMachineIfProtocol.SizeByte4 = sizeByte4;
                    allenBreadleyMachineIfProtocol.IsWriteable = isWriteable;
                }
            }
        }

        public override MachineIfProtocol GetProtocol(Enum command)
        {
            if (command == null)
                return GetProtocol(UniScanMMachineIfCommonCommand.GET_MACHINE_STATE);

            return base.GetProtocol(command);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");

            foreach (XmlElement subElement in xmlNodeList)
            {
                Enum key = this.GetEnum(XmlHelper.GetValue(subElement, "Command", ""));
                if(key!=null)
                {
                    MachineIfProtocol value = this.dic[key];
                    value.Load(subElement, "Protocol");
                }
            }
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<Enum, MachineIfProtocol> pair in this.dic)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
                xmlElement.AppendChild(subElement);

                XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
                pair.Value.Save(subElement, "Protocol");
            }
        }
    }
}
