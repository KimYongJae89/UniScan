using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.StillImage.MachineIF
{
    public enum UniScanMMachineIfStillImageCommand
    {
        SET_VISION_STATE,
        SET_STILLIMAGE_READY,
        SET_STILLIMAGE_RUN,
        SET_STILLIMAGE,
        SET_STILLIMAGE_GOOD,
        SET_MARGIN_W,
        SET_MARGIN_L,
        SET_BLOT_W,
        SET_BLOT_L,
        SET_DEFECT_W,
        SET_DEFECT_L    // StillImage
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.StillImage.MachineIF.UniScanMMachineIfStillImageCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(UniScanMMachineIfStillImageCommand));
            foreach (Enum value in values)
            {
                MachineIfProtocol mp = this.dic[value];
                SetDefault(mp, machineIfType);
            }

            for (int i = 0; i < this.dic.Count; i++)
            {
                
            }
        }

        private void SetDefault(MachineIfProtocol mp, MachineIfType machineIfType)
        {
            if (machineIfType == MachineIfType.Melsec)
                SetDefaultMelsec(mp);
            else if(machineIfType == MachineIfType.AllenBreadley)
                SetDefaultAB(mp);
        }

        private void SetDefaultAB(MachineIfProtocol mp)
        {
            string tagName = "c_STOPIMGtoPLC";
            int offsetByte4 = 0; // 1 = 4byte
            int sizeByte4 = 0; // 1 = 4byte
            bool use = true;
            bool isWriteable = true;

            switch (mp.Command)
            {
                case UniScanMMachineIfStillImageCommand.SET_VISION_STATE:
                    offsetByte4 = 0; sizeByte4 = 2; break;
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_READY:
                    offsetByte4 = 1; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_RUN:
                    offsetByte4 = 2; sizeByte4 = 1; break;

                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE:
                    offsetByte4 = 3; sizeByte4 = 7; break;
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_GOOD:
                    offsetByte4 = 3; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_MARGIN_W:
                    offsetByte4 = 4; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_MARGIN_L:
                    offsetByte4 = 5; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_BLOT_W:
                    offsetByte4 = 6; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_BLOT_L:
                    offsetByte4 = 7; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_DEFECT_W:
                    offsetByte4 = 8; sizeByte4 = 1; break;
                case UniScanMMachineIfStillImageCommand.SET_DEFECT_L:
                    offsetByte4 = 9; sizeByte4 = 1; break;
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

        private void SetDefaultMelsec(MachineIfProtocol mp)
        {
            string address;
            int sizeWord;
            bool isReadCommand;
            bool isValid = true;

            switch (mp.Command)
            {
                case UniScanMMachineIfStillImageCommand.SET_VISION_STATE:
                    address = "D1800"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_READY:
                    address = "D1800"; sizeWord = 1; isReadCommand = false; break;      
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_RUN:
                    address = "D1801"; sizeWord = 1; isReadCommand = false; break;

                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE:
                    address = "D1802"; sizeWord = 13; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_GOOD:
                    address = "D1802"; sizeWord = 1; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_MARGIN_W:
                    address = "D1804"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_MARGIN_L:
                    address = "D1806"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_BLOT_W:
                    address = "D1808"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_BLOT_L:
                    address = "D1810"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_DEFECT_W:
                    address = "D1812"; sizeWord = 2; isReadCommand = false; break;
                case UniScanMMachineIfStillImageCommand.SET_DEFECT_L:
                    address = "D1814"; sizeWord = 2; isReadCommand = false; break;

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
    }
}
