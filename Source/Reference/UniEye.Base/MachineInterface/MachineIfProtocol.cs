using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    public class MachineIfProtocolWithArguments
    {
        public MachineIfProtocol MachineIfProtocol => this.machineIfProtocol;
        MachineIfProtocol machineIfProtocol;

        public string[] Arguments => this.arguments;
        string[] arguments;

        public MachineIfProtocolWithArguments(MachineIfProtocol machineIfProtocol, string[] arguments)
        {
            this.machineIfProtocol = machineIfProtocol;
            this.arguments = arguments;
        }

        public override string ToString()
        {
            string args = null;
            if (this.arguments != null)
                args = string.Join(",", this.arguments);

            if (!string.IsNullOrEmpty(args))
                return string.Format("{0},{1}", machineIfProtocol.ToString(), args);
            else
                return machineIfProtocol.ToString();
        }
    }

    public abstract class MachineIfProtocol
    {
        public string Name => this.command.ToString();

        public Enum Command { get => this.command; }
        protected Enum command;

        public bool Use { get => this.use; set => this.use = value; }
        protected bool use = false;

        public int WaitResponceMs { get => this.waitResponceMs; set => this.waitResponceMs = value; }
        protected int waitResponceMs = 2000;

        public static MachineIfProtocol Create(MachineIfType machineIfType, Enum command)
        {
            MachineIfProtocol machineIfProtocol = null;
            switch (machineIfType)
            {
                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    machineIfProtocol = new TcpIpMachineIfProtocol(command);
                    break;
                case MachineIfType.Melsec:
                    machineIfProtocol = new MelsecMachineIfProtocol(command);
                    break;
                case MachineIfType.IO:
                    machineIfProtocol = new IoMachineIfProtocol(command);
                    break;
            }
            return machineIfProtocol;
        }

        public abstract bool IsValid { get; }

        public MachineIfProtocol(Enum command, bool use, int waitResponceMs)
        {
            this.command = command;
            this.use = use;
            this.waitResponceMs = waitResponceMs;
        }

        protected virtual void SaveXml(XmlElement element)
        {
            XmlHelper.SetValue(element, "Use", this.use);
            XmlHelper.SetValue(element, "WaitResponceMs", this.waitResponceMs);
        }

        public void Save(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Save(subElement);
                return;
            }

            SaveXml(element);
        }

        protected virtual void LoadXml(XmlElement element)
        {
            this.use = XmlHelper.GetValue(element, "Use", this.use);
            this.waitResponceMs = XmlHelper.GetValue(element, "WaitResponceMs", this.waitResponceMs);
            if (this.waitResponceMs < 0)
                this.waitResponceMs = 400;
        }

        public void Load(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Load(subElement);
                return;
            }

            LoadXml(element);
        }

        public override string ToString()
        {
            return command?.ToString();
        }

        public abstract MachineIfProtocol Clone();
    }
    
    public class MachineIfProtocolResponce
    {
        MachineIfProtocol sentProtocol;
        string reciveData;
        bool isGood = false;
        bool isResponced = false;
        ReceivedPacket receivedPacket = null;
        ManualResetEvent onResponce = new ManualResetEvent(false);
        
        public bool IsResponced
        {
            get { return isResponced; }
        }

        public MachineIfProtocol SentProtocol
        {
            get { return sentProtocol; }
        }

        public string ReciveData
        {
            get { return reciveData; }
        }

        public bool IsGood
        {
            get { return isResponced && isGood /*&& !string.IsNullOrEmpty(reciveData)*/; }
        }

        public ReceivedPacket ReceivedPacket { get => receivedPacket; }

        public MachineIfProtocolResponce(MachineIfProtocol sentProtocol)
        {
            this.sentProtocol = sentProtocol;
        }

        public void SetRecivedData(string reciveData, bool isGood, ReceivedPacket receivedPacket)
        {
            //if (string.IsNullOrEmpty(reciveData))
            //    return;

            this.reciveData = reciveData;
            this.isGood = isGood;
            this.isResponced = true;
            this.receivedPacket = receivedPacket;
            this.onResponce.Set();
        }

        public bool WaitResponce(int timeoutMs = -1)
        {
            if (timeoutMs < 0)
                timeoutMs = this.SentProtocol.WaitResponceMs;

            return onResponce.WaitOne(timeoutMs);
        }

        /// <summary>
        /// Ascii Byte -> Unicode String
        /// </summary>
        /// <returns></returns>
        public string Convert2String()
        {
            if (this.isResponced == false)
                return null;

            char[] chars = new char[reciveData.Length / 2];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)Convert.ToByte(reciveData.Substring(i * 2, 2), 16);
            return new string(chars).Trim('\0');
        }

        public string Convert2StringLE()
        {
            if (this.isResponced == false)
                return null;

            char[] chars = new char[reciveData.Length/2];
            for (int i = 0; i < chars.Length; i+=2)
            {
                int idx1 = (i + 1) * 2;
                if (idx1 + 1 < chars.Length)
                    chars[i] = (char)Convert.ToByte(reciveData.Substring(idx1, 2),  16);

                int idx2 = (i) * 2;
                if (idx2 + 1 < chars.Length)
                    chars[i + 1] = (char)Convert.ToByte(reciveData.Substring(idx2, 2), 16);
            }
            string converted = new string(chars).Trim('\0');
            return converted;
        }
    }
}
