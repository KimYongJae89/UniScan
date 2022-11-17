using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UniScanM.MachineIF;
using UniEye.Base.MachineInterface;
using System.Xml;
using DynMvp.Base;

namespace UniScanM.CEDMS.MachineIF
{
    public enum UniScanMMachineIfCEDMSCommand
    {
        SET_VISION_STATE,
        SET_CEDMS_READY,
        SET_CEDMS_RUN,
        SET_CEDMS,
        SET_CEDMS_GOOD,
        SET_INFEED_MODULUS,
        SET_OUTFEED_MODULUS,
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.CEDMS.MachineIF.UniScanMMachineIfCEDMSCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList)        {        }

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
                    case UniScanMMachineIfCEDMSCommand.SET_VISION_STATE:
                        address = "D1720"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfCEDMSCommand.SET_CEDMS_READY:
                        address = "D1720"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfCEDMSCommand.SET_CEDMS_RUN:
                        address = "D1721"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfCEDMSCommand.SET_CEDMS:
                        address = "D1722"; sizeWord = 3; isReadCommand = false; break;
                    case UniScanMMachineIfCEDMSCommand.SET_CEDMS_GOOD:
                        address = "D1722"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfCEDMSCommand.SET_INFEED_MODULUS:
                        address = "D1723"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfCEDMSCommand.SET_OUTFEED_MODULUS:
                        address = "D1724"; sizeWord = 1; isReadCommand = false; break;
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
