using DynMvp.Base;
using DynMvp.Device;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Inspect;

namespace UniEye.Base.MachineInterface
{
    public enum MachineIfType
    {
        None, TcpServer, TcpClient, Melsec, IO, AllenBreadley
    }

    public abstract class MachineIfProtocolList
    {
        public Type[] ProtocolListType => this.protocolListType;
        Type[] protocolListType;

        public Dictionary<Enum, MachineIfProtocol> Dic => this.dic;
        protected Dictionary<Enum, MachineIfProtocol> dic = new Dictionary<Enum, MachineIfProtocol>();

        public static MachineIfProtocolList List { get; private set; } = null;
        public static void Set(MachineIfProtocolList list)
        {
            MachineIfProtocolList.List = list;
        }

        public MachineIfProtocolList(params Type[] protocolListType)
        {
            Debug.Assert(protocolListType != null && protocolListType.Length > 0);

            this.protocolListType = protocolListType;
            for (int i = 0; i < protocolListType.Length; i++)
                AddProtocolList(protocolListType[i]);
        }

        public MachineIfProtocolList(MachineIfProtocolList machineIfProtocolList) : this(machineIfProtocolList.protocolListType)
        {
            var enumerator = machineIfProtocolList.dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (dic.ContainsKey(enumerator.Current.Key))
                    dic[enumerator.Current.Key] = enumerator.Current.Value.Clone();
            }
        }

        private void AddProtocolList(Type protocolListType)
        {
            //this.dic.Clear();
            Array array = Enum.GetValues(protocolListType);
            foreach (Enum e in array)
            {
                this.dic.Add(e, null);
            }
        }

        public abstract MachineIfProtocolList Clone();

        public virtual void CopyFrom(MachineIfProtocolList machineIfProtocolList)
        {
            this.protocolListType = (Type[])machineIfProtocolList.protocolListType.Clone();

            this.dic.Clear();
            var v = machineIfProtocolList.dic.GetEnumerator();
            while (v.MoveNext())
            {
                this.dic.Add(v.Current.Key, v.Current.Value.Clone());
            }
        }

        public Enum GetEnum(string command)
        {
            foreach (Type type in this.ProtocolListType)
            {
                if (type.IsEnumDefined(command))
                {
                    Enum e = (Enum)Enum.Parse(type, command);
                    return e;
                }
            }
            return null;
        }

        public virtual void Initialize(MachineIfType machineIfType)
        {
            Func<KeyValuePair<Enum, MachineIfProtocol>, MachineIfProtocol> func = new Func<KeyValuePair<Enum, MachineIfProtocol>, MachineIfProtocol>(f =>
            {
                switch (machineIfType)
                {
                    case MachineIfType.TcpClient:
                    case MachineIfType.TcpServer:
                        return new TcpIpMachineIfProtocol(f.Key);
                    case MachineIfType.Melsec:
                        return new MelsecMachineIfProtocol(f.Key);
                    case MachineIfType.IO:
                        return new IoMachineIfProtocol(f.Key);
                    case MachineIfType.AllenBreadley:
                        return new AllenBreadleyMachineIfProtocol(f.Key);
                    default:
                        return null;
                }
            });

            this.dic = this.dic.ToDictionary(f => f.Key, f => func(f));
        }

        protected abstract void LoadXml(XmlElement xmlElement);
        public void Load(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement[subKey];
                Load(subElement);
                return;
            }

            LoadXml(xmlElement);
        }

        protected abstract void SaveXml(XmlElement xmlElement);
        public void Save(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            SaveXml(xmlElement);
        }

        public MachineIfProtocol[] GetProtocols()
        {
            List<MachineIfProtocol> list = this.dic.Values.ToList();
            list.RemoveAll(f => f == null);
            return list.ToArray();
        }

        public virtual MachineIfProtocol GetProtocol(string e)
        {
            foreach(KeyValuePair<Enum, MachineIfProtocol> pair in this.dic)
            {
                if (pair.Key.ToString() == e)
                    return pair.Value.Clone();
            }
            return null;
        }

        public virtual MachineIfProtocol GetProtocol(Enum e)
        {
            return this.dic[e]?.Clone();
        }
    }

    public abstract class MachineIfSetting 
    {
        public string Name { get => this.name; set => this.name = value; }
        string name= "";

        public bool IsVirtualMode { get => this.isVirtualMode; set => this.isVirtualMode = value; }
        bool isVirtualMode = false;

        public MachineIfType MachineIfType { get => this.machineIfType; set => this.machineIfType = value; }
        protected MachineIfType machineIfType = MachineIfType.None;

        public MachineIfProtocolList MachineIfProtocolList { get => this.machineIfProtocolList; set => this.machineIfProtocolList = value; }
        protected MachineIfProtocolList machineIfProtocolList;

        protected MachineIfSetting(MachineIfSetting machineIfSetting)
        {
            this.CopyFrom(machineIfSetting);
        }

        protected MachineIfSetting(MachineIfType machineIfType)
        {
            this.machineIfType = machineIfType;
        }

        public static MachineIfSetting LoadSettings(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return null;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                return LoadSettings(subElement);
            }

            MachineIfType machineIfType = XmlHelper.GetValue(xmlElement, "MachineIfType", MachineIfType.None);
            MachineIfSetting machineIfSetting = Create(machineIfType);
            if (machineIfSetting != null)
                machineIfSetting.Load(xmlElement);

            return machineIfSetting;
        }

        public abstract MachineIfSetting Clone();

        public virtual void CopyFrom(MachineIfSetting src)
        {
            this.name = src.name;
            this.isVirtualMode = src.isVirtualMode;
            this.machineIfType = src.machineIfType;
            this.machineIfProtocolList = src.machineIfProtocolList?.Clone();
        }

        protected abstract void SaveXml(XmlElement xmlElement);
        public void Save(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement xmlSubElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(xmlSubElement);
                Save(xmlSubElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "MachineIfType", machineIfType.ToString());
            XmlHelper.SetValue(xmlElement, "IsVirtualMode", isVirtualMode.ToString());
            
            if (machineIfProtocolList != null)
                machineIfProtocolList.Save(xmlElement, "ProtocolList");
            XmlHelper.SetValue(xmlElement, "Name", name);

            SaveXml(xmlElement);
        }

        protected abstract void LoadXml(XmlElement xmlElement);
        public void Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement xmlSubElement = xmlElement[key];
                Load(xmlSubElement);
                return;
            }

            this.name = XmlHelper.GetValue(xmlElement, "Name", this.name);
            this.machineIfType = XmlHelper.GetValue(xmlElement, "MachineIfType", MachineIfType.None);
            this.isVirtualMode = XmlHelper.GetValue(xmlElement, "IsVirtualMode", this.isVirtualMode);

            this.machineIfProtocolList.Load(xmlElement, "ProtocolList");

            LoadXml(xmlElement);
        }

        internal static MachineIfSetting Create(MachineIfType machineIfType)
        {
            MachineIfSetting machineIfSetting = null;
            switch (machineIfType)
            {
                case MachineIfType.None:
                    machineIfSetting = null;
                    break;
                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    machineIfSetting = new TcpIpMachineIfSetting(machineIfType);
                    break;
                case MachineIfType.Melsec:
                    machineIfSetting = new MelsecMachineIfSetting();
                    break;
                case MachineIfType.IO:
                    machineIfSetting = new IoMachineIfSetting();
                    break;
                case MachineIfType.AllenBreadley:
                    machineIfSetting = new AllenBreadleyMachineIfSetting();
                    break;
            }

            if (machineIfSetting != null)
            {
                MachineIfProtocolList list = MachineIfProtocolList.List;
                list.Initialize(machineIfType);
                machineIfSetting.machineIfProtocolList = list;
            }

            return machineIfSetting;
        }
    }

    public interface IVirtualMachineIf
    {
        void SetStateConnect(bool connect);
    }

    public abstract class MachineIf 
    {
        public MachineIfSetting MachineIfSetting { get => this.machineIfSetting; }
        protected MachineIfSetting machineIfSetting = null;
        protected MachineIfProtocolResponce protocolResponce = null;

        bool isIdle = true;
        public bool IsIdle { get => isIdle; set => isIdle = value; }

        public abstract bool IsConnected { get; }
        public bool IsVirtual => this is IVirtualMachineIf;


        List<MachineIfExecuter> executerList = new List<MachineIfExecuter>();
        
        public virtual void Initialize() { }

        public abstract void Release();

        public static MachineIf Create(MachineIfSetting machineIfSetting, bool isVirtual)
        {
            //if (isVirtual)
            //    machineIfSetting.IsVirtualMode = true;

            switch (machineIfSetting.MachineIfType)
            {
                case MachineIfType.None:
                    return null;
                case MachineIfType.TcpClient:
                    return new TcpIpMachineIfClient(machineIfSetting);
                case MachineIfType.TcpServer:
                    return new TcpIpMachineIfServer(machineIfSetting);
                case MachineIfType.Melsec:
                    return MelsecMachineIf.Create(machineIfSetting);
                case MachineIfType.IO:
                    return IoMachineIf.Create(machineIfSetting);
                case MachineIfType.AllenBreadley:
                    return AllenBreadleyMachineIf.Create(machineIfSetting);
            }
            return null;
        }

        public MachineIf(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = machineIfSetting;
        }
        
        public void AddExecuter(MachineIfExecuter executer)
        {
            executerList.Add(executer);
        }

        public void AddExecuters(MachineIfExecuter[] executers)
        {
            executerList.AddRange(executers);
        }

        public ExecuteResult ExecuteCommand(MachineIfProtocolWithArguments commandString)
        {
            ExecuteResult executeResult = new ExecuteResult();
            //if (String.IsNullOrEmpty(commandString) == true)
            //    return executeResult;

            LogHelper.Debug(LoggerType.Network, $"MachineIf::ExecuteCommand - Name: {commandString.MachineIfProtocol.Name}, Args: {string.Join(",", commandString.Arguments)}");
            try
            {
                foreach (MachineIfExecuter executer in executerList)
                {
                    executeResult = executer.ExecuteCommand(commandString);
                    if (executeResult.Success == true)
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Network, $"MachineIf::ExecuteCommand - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            return executeResult;
        }

        protected abstract bool Send(MachineIfProtocol protocol, params string[] args);

        //public abstract void SendCommand(byte[] bytes);

        public MachineIfProtocolResponce SendCommand(MachineIfProtocol protocol, params string[] args)
        {
            int timeoutMs = protocol.WaitResponceMs;

            //Monitor.TryEnter(this, 500);
            isIdle = false;
            lock (this)
            {
                protocolResponce = new MachineIfProtocolResponce(protocol);
                if (protocol.Use)
                {
                    bool ok = this.Send(protocol, args);
                    if (ok)
                    {
                        if (timeoutMs != 0 && protocolResponce.WaitResponce(timeoutMs) == false)
                        {
                            LogHelper.Error(LoggerType.Network, string.Format("MachineIf::SendCommand - WaitResponce Timeout. {0}", protocol.Command == null ? "" : protocol.Command.ToString()));
                            //Debug.WriteLine("MachineIf::Send Timeout");
                        }
                    }
                }
                isIdle = true;
                return protocolResponce;
            }
            isIdle = true;
        }

        public MachineIfProtocolResponce SendCommand(Enum command, params string[] args)
        {
            MachineIfProtocol machineIfProtocol = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList.GetProtocol(command);
            //if (timeout >= 0)
            //    machineIfProtocol.WaitResponceMs = timeout;

            return SendCommand(machineIfProtocol, args);
        }

        public MachineIfProtocolResponce SendCommand(MachineIfProtocolList machineIfProtocolList, Enum command, params string[] args)
        {
            string logString = string.Format("MachineIf::SendCommand - Command: {0}, Args: {1}", command, args == null ? "null" : string.Join(";", args));
            LogHelper.Debug(LoggerType.Network, logString);
            Console.WriteLine(logString);

            MachineIfProtocol machineIfProtocol = machineIfProtocolList.GetProtocol(command);
            return SendCommand(machineIfProtocol, args);
        }
    }
}
