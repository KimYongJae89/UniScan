using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.EDMSW.MachineIF
{
    public enum UniScanMMachineIfEDMSCommand
    {
        SET_VISION_STATE,
        SET_EDMS_READY,
        SET_EDMS_RUN,
        SET_EDMS,
        SET_EDMS_GOOD,

        SET_T100,
        SET_T101,
        SET_T102,
        SET_T103,
        SET_T104,
        SET_T105,

        SET_T200,
        SET_T201,
        SET_T202,
        SET_T203,
        SET_T204,
        SET_T205,

        SET_W100,
        SET_W101,
        SET_W102,
        SET_L100,
        SET_L200,
        SET_LDIFF
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.EDMSW.MachineIF.UniScanMMachineIfEDMSCommand)) { }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new MachineIfProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            for(int i=0; i< this.dic.Count;i++)
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
                    case UniScanMMachineIfEDMSCommand.SET_VISION_STATE:
                        address = "D1720"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_READY:
                        address = "D1720"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_RUN:
                        address = "D1721"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfEDMSCommand.SET_EDMS:
                        address = "D1722"; sizeWord = 25; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_GOOD:
                        address = "D1722"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfEDMSCommand.SET_T100 :
                        address = "D1723"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T101:
                        address = "D1724"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T102:
                        address = "D1725"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T103:
                        address = "D1726"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T104:
                        address = "D1727"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T105:
                        address = "D1728"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfEDMSCommand.SET_T200:
                        address = "D1729"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T201:
                        address = "D1730"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T202:
                        address = "D1731"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T203:
                        address = "D1732"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T204:
                        address = "D1733"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_T205:
                        address = "D1734"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanMMachineIfEDMSCommand.SET_W100:
                        address = "D1735"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_W101:
                        address = "D1737"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_W102:
                        address = "D1739"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_L100:
                        address = "D1741"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_L200:
                        address = "D1743"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_LDIFF:
                        address = "D1745"; sizeWord = 2; isReadCommand = false; break;

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
