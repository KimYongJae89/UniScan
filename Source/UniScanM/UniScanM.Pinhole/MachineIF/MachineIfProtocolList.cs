using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.Pinhole.MachineIF
{
    public enum UniScanMMachineIfPinholeCommand
    {
        SET_VISION_STATE,
        SET_PINHOLE_READY,
        SET_PINHOLE_RUN,
        SET_PINHOLE,
        SET_PINHOLE_GOOD,
        SET_SUM_DEFECT
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList() : base(typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.Pinhole.MachineIF.UniScanMMachineIfPinholeCommand)) { }

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
                    case UniScanMMachineIfPinholeCommand.SET_VISION_STATE:
                        address = "D1700"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfPinholeCommand.SET_PINHOLE:
                        address = "D1702"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfPinholeCommand.SET_PINHOLE_READY:
                        address = "D1700"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfPinholeCommand.SET_PINHOLE_RUN:
                        address = "D1701"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfPinholeCommand.SET_PINHOLE_GOOD:
                        address = "D1702"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfPinholeCommand.SET_SUM_DEFECT:
                        address = "D1703"; sizeWord = 1; isReadCommand = false; break;
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                if (isValid == true)
                {
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                }
            }
        }
    }
}
