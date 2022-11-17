using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanG.Module.Controller.MachineIF
{
    public enum MelsecProtocol
    {
        // 검사기 상태
        //SET_VISION_STATE_GRAVURE_INSP,
        SET_VISION_GRAVURE_INSP_READY,
        SET_VISION_GRAVURE_INSP_RUNNING,

        //SET_VISION_RESULT_GRAVURE_INSP,
        SET_VISION_GRAVURE_INSP_RESULT,
        SET_VISION_GRAVURE_INSP_NG_REPDEF_P,
        SET_VISION_GRAVURE_INSP_NG_REPDEF_N,
        SET_VISION_GRAVURE_INSP_NG_REPDEF_B,
        SET_VISION_GRAVURE_INSP_NG_REPDEF,
        SET_VISION_GRAVURE_INSP_NG_NORDEF,
        SET_VISION_GRAVURE_INSP_NG_SHTLEN,

        SET_VISION_GRAVURE_INSP_CNT_ALL,
        SET_VISION_GRAVURE_INSP_CNT_NG,
        SET_VISION_GRAVURE_INSP_CNT_PINHOLE,
        SET_VISION_GRAVURE_INSP_CNT_COATING,
        SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK,
        SET_VISION_GRAVURE_INSP_CNT_NOPRINT,

        SET_VISION_GRAVURE_INSP_NG_DEFCNT,
        SET_VISION_GRAVURE_INSP_NG_MARGIN,
        SET_VISION_GRAVURE_INSP_NG_STRIPE,
        SET_VISION_GRAVURE_INSP_NG_CRITICAL,

        SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE,
        SET_VISION_GRAVURE_INSP_INFO_SHTLEN,

        // 레이저 상태
        //GET_ERASER_STATE,
        GET_FORCE_GRAVURE_ERASER,

        // 레이저 상태
        //SET_ERASER_STATE,
        SET_VISION_GRAVURE_ERASER_READY,
        SET_VISION_GRAVURE_ERASER_RUNNING,
        SET_VISION_GRAVURE_ERASER_CNT_ERASE,
    };

    public class MachineProtocolList : UniScanG.MachineIF.MachineIfProtocolListG
    {
        public MachineProtocolList() : base(typeof(UniScanG.MachineIF.MelsecProtocolCommon), typeof(UniScanG.Module.Controller.MachineIF.MelsecProtocol)) { }

        public MachineProtocolList(MachineProtocolList melsecProtocolList) : base(melsecProtocolList) { }

        public override MachineIfProtocolList Clone()
        {
            MachineProtocolList melsecProtocolList = new MachineProtocolList(this);
            return melsecProtocolList;
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(MelsecProtocol));
            foreach (Enum key in values)
            {
                if (!this.dic.ContainsKey(key))
                    continue;

                MachineIfProtocol machineIfProtocol = this.dic[key];
                SetDefault(machineIfProtocol, machineIfType);
            }
        }

        protected new void SetDefault(MachineIfProtocol machineIfProtocol, MachineIfType machineIfType)
        {
            MelsecProtocol key = (MelsecProtocol)machineIfProtocol.Command;

            if (machineIfType == MachineIfType.Melsec)
            {
                MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)machineIfProtocol;

                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_READY:
                        address = "D1850"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING:
                        address = "D1851"; sizeWord = 1; isReadCommand = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT:
                        address = "D1852"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P:
                        address = "D1853"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N:
                        address = "D1854"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B:
                        address = "D1865"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF:
                        address = "D1866"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF:
                        address = "D1855"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN:
                        address = "D1856"; sizeWord = 1; isReadCommand = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL:
                        address = "D1857"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG:
                        address = "D1858"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE:
                        address = "D1859"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING:
                        address = "D1860"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK:
                        address = "D1861"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT:
                        address = "D1862"; sizeWord = 1; isReadCommand = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT:
                        address = "D0000"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN:
                        address = "D0000"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE:
                        address = "D0000"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL:
                        address = "D1864"; sizeWord = 1; isReadCommand = false; isValid = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE:
                        address = "D1820"; sizeWord = 1; isReadCommand = false; isValid = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_SHTLEN:
                        address = "D0000"; sizeWord = 1; isReadCommand = false; isValid = false; break;

                    case MelsecProtocol.GET_FORCE_GRAVURE_ERASER:
                        address = "D1880"; sizeWord = 1; isReadCommand = true; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY:
                        address = "D1890"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING:
                        address = "D1891"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE:
                        address = "D1892"; sizeWord = 1; isReadCommand = false; break;

                    default:
                        address = ""; sizeWord = 0; isReadCommand = true; isValid = false; break;
                }

                melsecMachineIfProtocol.Use = isValid;
                melsecMachineIfProtocol.Address = address;
                melsecMachineIfProtocol.SizeWord = sizeWord;
                melsecMachineIfProtocol.IsReadCommand = isReadCommand;
            }
            else if (machineIfType == MachineIfType.IO)
            {
                UniEye.Base.Device.PortMap pm = (UniEye.Base.Device.PortMap)SystemManager.Instance().DeviceBox.PortMap;
                IoMachineIfProtocol ioMachineIfProtocol = (IoMachineIfProtocol)machineIfProtocol;
                //ioMachineIfProtocol.IoPort.DeviceNo;
                //ioMachineIfProtocol.IoPort.GroupNo;
                //ioMachineIfProtocol.IoPort.PortNo;
                //ioMachineIfProtocol.IoPort.ActiveLow;
            }
            else if(machineIfType == MachineIfType.AllenBreadley)
            {
                string tagName = "c_PItoPLC";
                int offsetByte4; // 1 = 4byte
                int sizeByte4; // 1 = 4byte
                bool use = true;
                bool isWriteable = true;
                switch (key)
                {
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_READY:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING:
                        offsetByte4 = 1; sizeByte4 = 1; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P:
                        offsetByte4 = 3; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N:
                        offsetByte4 = 4; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF:
                        offsetByte4 = 5; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN:
                        offsetByte4 = 6; sizeByte4 = 1; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL:
                        offsetByte4 = 7; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG:
                        offsetByte4 = 8; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE:
                        offsetByte4 = 9; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING:
                        offsetByte4 = 10; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK:
                        offsetByte4 = 11; sizeByte4 = 1; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT:
                        offsetByte4 = 12; sizeByte4 = 1; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_SHTLEN:
                        offsetByte4 = 13; sizeByte4 = 1; use = true; break;
                    //case MelsecProtocol.GET_ERASER_STATE:
                    //    address = "D1880"; sizeWord = 1; isReadCommand = true; break;
                    case MelsecProtocol.GET_FORCE_GRAVURE_ERASER:
                        offsetByte4 = 30; sizeByte4 = 1; use = false; isWriteable = false; break;

                    //case MelsecProtocol.SET_ERASER_STATE:
                    //    address = "D1890"; sizeWord = 3; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY:
                        offsetByte4 = 40; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING:
                        offsetByte4 = 41; sizeByte4 = 0; use = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE:
                        offsetByte4 = 42; sizeByte4 = 0; use = false; break;

                    default:
                        offsetByte4 = 0; sizeByte4 = 0; break;
                }

                AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfProtocol)machineIfProtocol;
                allenBreadleyMachineIfProtocol.Use = use;
                allenBreadleyMachineIfProtocol.TagName = tagName;
                allenBreadleyMachineIfProtocol.OffsetByte4 = offsetByte4;
                allenBreadleyMachineIfProtocol.SizeByte4 = sizeByte4;
                allenBreadleyMachineIfProtocol.IsWriteable = isWriteable;
            }
        }

        public override MachineIfProtocol GetProtocol(Enum command)
        {
            //if (command == null)
            //    return GetProtocol(MelsecProtocol.GET_MACHINE_STATE);

            return base.GetProtocol(command);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                MelsecProtocol key;
                bool ok = Enum.TryParse<MelsecProtocol>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                if (ok)
                {
                    MachineIfProtocol value = null;
                    value = this.dic[key];
                    value.Load(subElement, "Protocol");
                }
            }
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

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
