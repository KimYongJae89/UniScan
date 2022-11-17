using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanG.Common.Exchange
{
    public enum ExchangeCommand
    {
        None = 0, U_CHANGE, C_SYNC, C_SPD, C_LICENSE,
        I_READY = 100, I_TEACH, I_LIGHT, I_START, I_STOP, I_DONE, I_PAUSE, I_LOTCHANGE, I_LOADFACTOR,//Inspect
        M_CREATE = 200, M_SELECT , M_RESELECT,  M_REFRESH, M_CLOSE, M_DELETE, M_TEACH_DONE,// Model
        J_RUNNING = 300, J_DONE, J_ERROR, // Job
        C_CONNECTED = 400, C_DISCONNECTED, // Command
        S_IDLE = 500, S_OpWait, S_INSPECT, S_PAUSE, S_ALARM, S_TEACH, S_InspWAIT, S_RUN, S_DONE, S_OpErr, // State
        V_SHOW = 600, V_HIDE, V_INSPECT, V_MODEL, V_TEACH, V_REPORT, V_LOG, V_SETTING, V_DONE, // Visit

        // G용
        F_FOUNDED = 700, F_SET // Fiducial
    }

    public class ExchangeProtocolList : MachineIfProtocolList
    {
        public ExchangeProtocolList(params Type[] protocolListType) : base(protocolListType) { }

        public ExchangeProtocolList(MachineIfProtocolList machineIfProtocolList) : base(machineIfProtocolList) { }

        public override MachineIfProtocolList Clone()
        {
            return new ExchangeProtocolList(this);
        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            List<Enum> list = this.dic.Keys.ToList();

            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                ExchangeCommand key;
                bool ok = Enum.TryParse<ExchangeCommand>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                MachineIfProtocol value = null;
                if (ok)
                {
                    System.Diagnostics.Debug.Assert(list.Contains(key));
                    list.Remove(key);
                    value = this.dic[key];
                    if (value != null)
                    {
                        value.Load(subElement, "Protocol");
                        value.Use = true;
                        value.WaitResponceMs = 0;

                        this.dic[key] = value;
                    }
                }
            }

            list.ForEach(f =>
            {
                ExchangeCommand key = (ExchangeCommand)f;
                this.dic[key].Use = true;
                this.dic[key].WaitResponceMs = 0;
            });
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
