using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanG.MachineIF;

namespace UniScanG.Module.Monitor.MachineIF
{
    public enum MelsecProtocol
    {
        // 검사기 상태
        SET_VISION_GRAVURE_MONITORING_READY,
        SET_VISION_GRAVURE_MONITORING_RUN,

        SET_VISION_GRAVURE_MONITORING_RESULT,
        SET_VISION_GRAVURE_MONITORING_MARGIN_W,
        SET_VISION_GRAVURE_MONITORING_MARGIN_L,
        SET_VISION_GRAVURE_MONITORING_BLOT_W,
        SET_VISION_GRAVURE_MONITORING_BLOT_L,
        SET_VISION_GRAVURE_MONITORING_DEFECT_W,
        SET_VISION_GRAVURE_MONITORING_DEFECT_L,
    };

    public class MelsecProtocolList : UniScanG.MachineIF.MachineIfProtocolListG
    {
        public MelsecProtocolList() : base(typeof(UniScanG.MachineIF.MelsecProtocolCommon), typeof(UniScanG.Module.Monitor.MachineIF.MelsecProtocol)) { }

        public MelsecProtocolList(UniScanG.MachineIF.MachineIfProtocolListG melsecProtocolList) : base(melsecProtocolList) { }

        public override MachineIfProtocolList Clone()
        {
            MelsecProtocolList melsecProtocolList = new MelsecProtocolList(this);
            return melsecProtocolList;
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(MelsecProtocol));
            foreach(Enum value in values)
            {
                if (!dic.ContainsKey(value))
                    dic.Add(value, new MelsecMachineIfProtocol(value));

                Enum key = value;
                MelsecMachineIfProtocol melsecMachineIfProtocol = this.dic[key] as MelsecMachineIfProtocol;
                if (melsecMachineIfProtocol == null)
                    continue;

                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_READY:
                        address = "D1800"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RUN:
                        address = "D1801"; sizeWord = 1; isReadCommand = false; break;

                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RESULT:
                        address = "D1802"; sizeWord = 1; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_W:
                        address = "D1804"; sizeWord = 2; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_L:
                        address = "D1806"; sizeWord = 2; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_W:
                        address = "D1808"; sizeWord = 2; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_L:
                        address = "D1810"; sizeWord = 2; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_W:
                        address = "D1812"; sizeWord = 2; isReadCommand = false; break;
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_L:
                        address = "D1814"; sizeWord = 2; isReadCommand = false; break;
                    
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                //if (isValid == true)
                {
                    melsecMachineIfProtocol.Use = isValid;
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                }
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
                MachineIfProtocol value = null;
                if (ok)
                {
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
