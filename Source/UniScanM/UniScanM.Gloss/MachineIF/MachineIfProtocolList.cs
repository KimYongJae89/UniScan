using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.Gloss.MachineIF
{
    public enum UniScanMMachineIfGlossCommand
    {
        GET_START = 0,
        GET_PAUSE = 1,
        GET_ROLL_POSITION = 2,
        GET_LOT = 3,
        GET_MODEL = 4,
        SET_VISION_STATE,
        SET_GLOSS,
        SET_TOTAL_GLOSS,
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.Gloss.MachineIF.UniScanMMachineIfGlossCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(UniScanMMachineIfGlossCommand));
            foreach (Enum value in values)
            {
                MachineIfProtocol mp = this.dic[value];
                SetDefault(mp, machineIfType);
            }

            if (machineIfType == MachineIfType.AllenBreadley)
            {
                AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = this.dic[UniScanMMachineIfCommonCommand.GET_MACHINE_STATE] as AllenBreadleyMachineIfProtocol;
                allenBreadleyMachineIfProtocol.Use = true;
                allenBreadleyMachineIfProtocol.TagName = "Unieye_Write";
                allenBreadleyMachineIfProtocol.OffsetByte4 = 0;
                allenBreadleyMachineIfProtocol.SizeByte4 = 150;
                allenBreadleyMachineIfProtocol.IsWriteable = false;
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
                    case UniScanMMachineIfGlossCommand.GET_START:
                        address = "D6800"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfGlossCommand.GET_PAUSE:
                        address = "D6801"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfGlossCommand.GET_ROLL_POSITION:
                        address = "D2688"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanMMachineIfGlossCommand.GET_LOT:
                        address = "D6020"; sizeWord = 6; isReadCommand = true; break;
                    case UniScanMMachineIfGlossCommand.GET_MODEL:
                        address = "D6100"; sizeWord = 11; isReadCommand = true; break;

                    case UniScanMMachineIfGlossCommand.SET_GLOSS:
                        address = "D6802"; sizeWord = 24; isReadCommand = false; break;
                    case UniScanMMachineIfGlossCommand.SET_TOTAL_GLOSS:
                        address = "D6802"; sizeWord = 4; isReadCommand = false; break;

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
            else if (machineIfType == MachineIfType.AllenBreadley)
            {
                string tagName = "Unieye_G_Read";
                int offsetByte4 = 0; // 1 = 4byte
                int sizeByte4 = 0; // 1 = 4byte
                bool use = true;
                bool isWriteable = true;

                switch (mp.Command)
                {
                    case UniScanMMachineIfGlossCommand.SET_VISION_STATE:
                        offsetByte4 = 0; sizeByte4 = 3; break;
                    case UniScanMMachineIfGlossCommand.SET_GLOSS:
                        offsetByte4 = 20; sizeByte4 = 23; break;
                    case UniScanMMachineIfGlossCommand.SET_TOTAL_GLOSS:
                        offsetByte4 = 40; sizeByte4 = 3; break;
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
