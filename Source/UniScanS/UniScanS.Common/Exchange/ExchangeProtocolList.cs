using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanS.Common.Exchange
{
    public enum ExchangeCommand
    {
        None = 0, U_CHANGE,
        I_START = 100, I_STOP, I_DONE, I_ENTER_PAUSE, I_EXIT_PAUSE, I_LOTCHANGE,//Inspect
        M_SELECT = 200, M_REFRESH, M_CLOSE, M_TEACH_DONE, M_PARAM, M_RESET,// Model
        J_DONE = 300, J_ERROR, // Job
        C_CONNECTED = 400, C_DISCONNECTED, //Comm
        S_IDLE = 500, S_INSPECT, S_PAUSE, S_ALARM, S_TEACH, S_WAIT, S_RUN, S_DONE,//State
        V_INSPECT = 600, V_MODEL, V_TEACH, V_REPORT, V_SETTING, V_DONE, //Visit

        //G용
        F_FOUNDED = 700, F_SET // Fiducial
    }

    public class ExchangeProtocolList : MachineIfProtocolList
    {
        public ExchangeProtocolList(params Type[] protocolListType) : base(protocolListType) { }


        public ExchangeProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override UniEye.Base.MachineInterface.MachineIfProtocolList Clone()
        {
            return new ExchangeProtocolList(this);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                ExchangeCommand key;
                bool ok = Enum.TryParse<ExchangeCommand>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                MachineIfProtocol value = null;
                if (ok) 
                {
                    value = this.dic[key];
                    if (value != null)
                    {
                        value.Load(subElement, "Protocol");
                         this.dic[key] = value;
                    }
                }
            }
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<Enum, MachineIfProtocol> pair in this.dic)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
                xmlElement.AppendChild(subElement);

                XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
                pair.Value?.Save(subElement, "Protocol");
            }
        }

    }
}
