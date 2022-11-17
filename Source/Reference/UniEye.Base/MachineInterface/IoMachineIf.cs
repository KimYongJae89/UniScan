using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    public class IoMachineIfProtocol : MachineIfProtocol
    {
        public int DeviceNo { get => this.ioPort.DeviceNo; set => this.ioPort.DeviceNo = value; }
        
        public int GroupNo { get => this.ioPort.GroupNo; set => this.ioPort.GroupNo = value; }
        
        public int PortNo { get => this.ioPort.PortNo; set => this.ioPort.PortNo = value; }
        
        public bool ActiveLow { get => this.ioPort.ActiveLow; set => this.ioPort.ActiveLow = value; }
        
        public IoPort IoPort => this.ioPort;
        IoPort ioPort;

        public override bool IsValid => (!this.use) || (this.use && PortNo >= 0);

        public IoMachineIfProtocol(Enum command) : base(command, false, 500)
        {
            this.ioPort = IoPort.UnkownPort.Clone();
        }

        public IoMachineIfProtocol(Enum command, bool use, int waitResponceMs, IoPort ioPort) : base(command, use, waitResponceMs)
        {
            this.ioPort = ioPort.Clone();
        }

        public override MachineIfProtocol Clone()
        {
            return new IoMachineIfProtocol(this.command, this.use, this.waitResponceMs, this.ioPort.Clone());
        }

        protected override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);

            this.ioPort.SaveXml(element, "IoPort");
        }

        protected override void LoadXml(XmlElement element)
        {
            base.LoadXml(element);

            this.ioPort.LoadXml(element, "IoPort");
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}_{3}", this.command, this.DeviceNo, this.GroupNo, this.PortNo);
        }
    }

    //public enum IoMachineIfCommand { OutVisionAlive, InMachineAlive, OutVisionReadyPort, InTriggerPort, OutOnWorking, OutCompletePort, OutResultNgPort, InCommandDone };
    public enum ActiveLevel { Low, High, Toggle };
    //public class IoMachineIfProtocolListItem : ProtocolListItem
    //{
    //    ActiveLevel activeLevel = ActiveLevel.High;
    //    IoPort ioPort;

    //    public ActiveLevel ActiveLevel
    //    {
    //        get { return activeLevel; }
    //        set { activeLevel = value; }
    //    }

    //    public IoPort IoPort
    //    {
    //        get { return ioPort; }
    //        set { ioPort = value; }
    //    }

    //    protected override void SaveXml(XmlElement element)
    //    {
    //        XmlHelper.SetValue(element, "ActiveLevel", activeLevel.ToString());
    //        ioPort.SaveXml(element);
    //    }

    //    protected override void LoadXml(XmlElement element)
    //    {
    //        activeLevel = (ActiveLevel)Enum.Parse(typeof(ActiveLevel), XmlHelper.GetValue(element, "ActiveLevel", activeLevel.ToString()));
    //        ioPort = IoPort.Load(element);
    //    }
    //}

    //public class IoMachineIfProcotolHandler : MachineIfProtocolHandler
    //{
    //    public IoMachineIfProcotolHandler() : base()
    //    {
    //    }

    //    public override MachineIfProtocol GetProtocol(Enum command)
    //    {
    //        IoMachineIfProtocol item = (IoMachineIfProtocol)machineIfProtocolList.GetProtocol(command);
    //        IoPort ioPort = item.IoPort;
    //        return item;
    //    }

    //    //public override void Load(XmlElement xmlElement, string subKey = null)
    //    //{
    //    //    if (xmlElement == null)
    //    //        return;

    //    //    if (string.IsNullOrEmpty(subKey) == false)
    //    //    {
    //    //        XmlElement subElement = xmlElement[subKey];
    //    //        Save(subElement, null);
    //    //        return;
    //    //    }

    //    //    XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
    //    //    foreach (XmlElement subElement in xmlNodeList)
    //    //    {
    //    //        Enum e = (Enum)Enum.Parse(this.commandType, XmlHelper.GetValue(subElement, "Command", ""));
    //    //        IoPort ioPort = IoPort.Load(subElement, "IoPort");

    //    //        dictionary.Add(e, ioPort);
    //    //    }
    //    //}

    //    //public override void Save(XmlElement xmlElement, string subKey = null)
    //    //{
    //    //    if (xmlElement == null)
    //    //        return;

    //    //    if (string.IsNullOrEmpty(subKey) == false)
    //    //    {
    //    //        XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
    //    //        xmlElement.AppendChild(subElement);
    //    //        Save(subElement, null);
    //    //        return;
    //    //    }

    //    //    foreach (KeyValuePair<Enum, IoPort> pair in dictionary)
    //    //    {
    //    //        XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
    //    //        xmlElement.AppendChild(subElement);

    //    //        XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
    //    //        pair.Value.SaveXml(xmlElement, "IoPort");
    //    //    }
    //    //}
    //}

    public class IoMachineIfExcuter : MachineIfExecuter
    {
        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            throw new NotImplementedException();
        }
    }

    public class IoMachineIfSetting : MachineIfSetting
    {
        public IoMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        public IoMachineIfSetting() : base(MachineIfType.IO) { }

        public override MachineIfSetting Clone()
        {
            IoMachineIfSetting ioMachineIfSetting = new IoMachineIfSetting(this);
            return ioMachineIfSetting;
        }

        public override void CopyFrom(MachineIfSetting src)
        {
            base.CopyFrom(src);

            IoMachineIfSetting ioMachineIfSetting = (IoMachineIfSetting)src;
        }

        protected override void LoadXml(XmlElement xmlElement) { }

        protected override void SaveXml(XmlElement xmlElement) { }
    }

    public class IoMachineIf : MachineIf
    {
        DigitalIoHandler digitalIoHandler;

        public override bool IsConnected => true;

        public static IoMachineIf Create(MachineIfSetting machineIfSetting)
        {
            if (machineIfSetting.IsVirtualMode)
                return new IoMachineIfVirtual(machineIfSetting);

            return new IoMachineIf(machineIfSetting);
        }

        public IoMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            this.AddExecuter(new IoMachineIfExcuter());
        }

        public override void Initialize() { }

        public override void Release() { }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            IoMachineIfProtocol ioMachineIfProcotol = (IoMachineIfProtocol)protocol;
            this.digitalIoHandler.WriteOutput(ioMachineIfProcotol.IoPort, bool.Parse(args[0]));
            return true;
        }

        //public override void SendCommand(byte[] bytes)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class IoMachineIfVirtual : IoMachineIf, IVirtualMachineIf
    {
        public override bool IsConnected => this.isConnected;
        bool isConnected = true;

        public IoMachineIfVirtual(MachineIfSetting machineIfSetting) : base(machineIfSetting)        {        }

        public void SetStateConnect(bool connect)
        {
            this.isConnected = connect;
        }
    }
}
