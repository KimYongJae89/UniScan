using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.ColorSens.MachineIF
{
    public enum UniScanMMachineIfColorSensorCommand
    {
        SET_VISION_STATE,
        SET_COLORSENSOR_READY,
        SET_COLORSENSOR_RUN,

        SET_COLORSENSOR,
        SET_COLORSENSOR_NG,
        SET_SHEET_BRIGHTNESS
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList(MachineIfProtocolList list) : base(list) { }
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.ColorSens.MachineIF.UniScanMMachineIfColorSensorCommand)) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(UniScanMMachineIfColorSensorCommand));
            foreach (Enum value in values)
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
                    case UniScanMMachineIfColorSensorCommand.SET_VISION_STATE:
                        address = "D1710"; sizeWord = 2; isReadCommand = false; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_READY:
                        address = "D1710"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_RUN:
                        address = "D1711"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR:
                        address = "D1712"; sizeWord = 2; isReadCommand = false; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_NG:
                        address = "D1712"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_SHEET_BRIGHTNESS:
                        address = "D1713"; sizeWord = 1; isReadCommand = false; break;
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
                string tagName = "c_COLORtoPlc";
                int offsetByte4 = 0; // 1 = 4byte
                int sizeByte4 = 0; // 1 = 4byte
                bool use = true;
                bool isWriteable = true;

                switch (mp.Command)
                {
                    case UniScanMMachineIfColorSensorCommand.SET_VISION_STATE:
                        offsetByte4 = 0; sizeByte4 = 2; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_READY:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_RUN:
                        offsetByte4 = 1; sizeByte4 = 1; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR:
                        offsetByte4 = 2; sizeByte4 = 2; break;

                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_NG:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case UniScanMMachineIfColorSensorCommand.SET_SHEET_BRIGHTNESS:
                        offsetByte4 = 3; sizeByte4 = 1; break;
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
