using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UniScanM.MachineIF;
using UniEye.Base.MachineInterface;
using System.Xml;
using DynMvp.Base;

namespace UniScanM.RVMS.MachineIF
{
    public enum UniScanMMachineIfRVMSCommand
    {
        SET_VISION_STATE,
        SET_RVMS_READY,
        SET_RVMS_RUN,
        SET_RVMS,
        SET_RVMS_GOOD,
        SET_GEAR_VIBRATION_ZERORING,
        SET_MAN_VIBRATION_ZERORING,
        SET_GEAR_VIBRATION_MODULUS,
        SET_MAN_VIBRATION_MODULUS,
        GET_PATTERN_LENGTH_BEFORE,
        GET_PATTERN_LENGTH_AFTER
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.RVMS.MachineIF.UniScanMMachineIfRVMSCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            for (int i = 0; i < this.dic.Count; i++)
            {
                Enum key = dic.ElementAt(i).Key;
                MelsecMachineIfProtocol melsecMachineIfProtocol = this.dic[key] as MelsecMachineIfProtocol;
                if (melsecMachineIfProtocol == null)
                    continue;

                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case UniScanMMachineIfRVMSCommand.SET_VISION_STATE:
                        address = "D1740"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_RVMS:
                        address = "D1742"; sizeWord = 8; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_RVMS_READY:
                        address = "D1740"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfRVMSCommand.SET_RVMS_RUN:
                        address = "D1741"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_RVMS_GOOD:
                        address = "D1742"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_GEAR_VIBRATION_ZERORING:
                        address = "D1743"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_MAN_VIBRATION_ZERORING:
                        address = "D1744"; sizeWord = 1; isReadCommand = false; break;
                        //두개똑같아요???
                    case UniScanMMachineIfRVMSCommand.SET_GEAR_VIBRATION_MODULUS:
                        address = "D1746"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.SET_MAN_VIBRATION_MODULUS:
                        address = "D1748"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.GET_PATTERN_LENGTH_BEFORE:
                        address = "D1750"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfRVMSCommand.GET_PATTERN_LENGTH_AFTER:
                        address = "D1752"; sizeWord = 2; isReadCommand = false; break;
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                if (isValid == true)
                {
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                    melsecMachineIfProtocol.Use = true;
                }
            }
        }
    }
}
