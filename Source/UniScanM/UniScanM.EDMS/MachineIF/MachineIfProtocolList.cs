using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.EDMS.MachineIF
{
    public enum UniScanMMachineIfEDMSCommand
    {
        SET_VISION_STATE,
        SET_EDMS_READY,
        SET_EDMS_RUN,

        SET_EDMS,
        SET_EDMS_GOOD,
        SET_BASIC_EDGE,
        SET_ROLL_COATING_EDGE,
        SET_COATING_PRINTING_EDGE,
        SET_ROLL_PRINTING_EDGE,
        SET_TOTAL_EDGE,
        SET_FILM_PRINTING_EDGE,
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.EDMS.MachineIF.UniScanMMachineIfEDMSCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(UniScanMMachineIfEDMSCommand));
            foreach (Enum value in values)
            {
                MachineIfProtocol mp = this.dic[value];
                SetDefault(mp, machineIfType);
            }
        }

        private void SetDefault(MachineIfProtocol mp, MachineIfType machineIfType)
        {
            if(machineIfType == MachineIfType.Melsec)
            {
                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (mp.Command)
                {
                    case UniScanMMachineIfEDMSCommand.SET_VISION_STATE:
                        address = "D1720"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_READY:
                        address = "D1720"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_RUN:
                        address = "D1721"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfEDMSCommand.SET_EDMS:
                        address = "D1722"; sizeWord = 7; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_GOOD:
                        address = "D1722"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_BASIC_EDGE:
                        address = "D1723"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_COATING_EDGE:
                        address = "D1724"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_COATING_PRINTING_EDGE:
                        address = "D1725"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_PRINTING_EDGE:
                        address = "D1726"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_TOTAL_EDGE:
                        address = "D1727"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_FILM_PRINTING_EDGE:
                        address = "D1728"; sizeWord = 1; isReadCommand = false; break;
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                if (isValid == true)
                {
                    MelsecMachineIfProtocol melsecMachineIfProtocol = mp as MelsecMachineIfProtocol;
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                    melsecMachineIfProtocol.Use = true;
                }
            }
            else if(machineIfType == MachineIfType.AllenBreadley)
            {
                string tagName = "c_EDMStoPLC";
                int offsetByte4 = 0; // 1 = 4byte
                int sizeByte4 = 0; // 1 = 4byte
                bool use = true;
                bool isWriteable = true;

                switch (mp.Command)
                {
                    case UniScanMMachineIfEDMSCommand.SET_VISION_STATE:
                        offsetByte4 = 0; sizeByte4 = 2; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_READY:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_RUN:
                        offsetByte4 = 1; sizeByte4 = 1; break;

                    case UniScanMMachineIfEDMSCommand.SET_EDMS:
                        offsetByte4 = 2; sizeByte4 = 7; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_GOOD:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_BASIC_EDGE:
                        offsetByte4 = 3; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_COATING_EDGE:
                        offsetByte4 = 4; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_COATING_PRINTING_EDGE:
                        offsetByte4 = 5; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_PRINTING_EDGE:
                        offsetByte4 = 6; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_TOTAL_EDGE:
                        offsetByte4 = 7; sizeByte4 = 1; break;
                    case UniScanMMachineIfEDMSCommand.SET_FILM_PRINTING_EDGE:
                        offsetByte4 = 8; sizeByte4 = 1; break;
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
    }
}
