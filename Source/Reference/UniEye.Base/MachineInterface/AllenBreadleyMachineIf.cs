using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    public class AllenBreadleyMachineIfProtocol : MachineIfProtocol
    {
        public string TagName { get; set; }
        public int OffsetByte4 { get; set; }
        public int SizeByte4 { get; set; }
        public bool IsWriteable { get; set; }

        public override bool IsValid => !this.Use || (this.Use && (this.SizeByte4 > 0) && !string.IsNullOrEmpty(TagName));

        public AllenBreadleyMachineIfProtocol(Enum command) : base(command, false, 2000) { }
        public AllenBreadleyMachineIfProtocol(Enum command, bool use, int waitResponceMs) : base(command, use, waitResponceMs) { }
        
        public override MachineIfProtocol Clone()
        {
            return new AllenBreadleyMachineIfProtocol(this.command, this.use, this.waitResponceMs)
            {
                TagName = this.TagName,
                OffsetByte4 = this.OffsetByte4,
                SizeByte4 = this.SizeByte4,
                IsWriteable = this.IsWriteable
            };
        }

        protected override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);

            XmlHelper.SetValue(element, "TagName", this.TagName);
            XmlHelper.SetValue(element, "OffsetByte4", this.OffsetByte4);
            XmlHelper.SetValue(element, "SizeByte4", this.SizeByte4);
            XmlHelper.SetValue(element, "IsWriteable", this.IsWriteable);
        }

        protected override void LoadXml(XmlElement element)
        {
            base.LoadXml(element);

            this.TagName = XmlHelper.GetValue(element, "TagName", this.TagName);
            this.OffsetByte4 = XmlHelper.GetValue(element, "OffsetByte4", this.OffsetByte4);
            this.SizeByte4 = XmlHelper.GetValue(element, "SizeByte4", this.SizeByte4);
            this.IsWriteable = XmlHelper.GetValue(element, "IsWriteable", this.IsWriteable);
        }

        public override string ToString()
        {
            return $"{this.command}_{TagName}_{OffsetByte4}_{SizeByte4}";
        }
    }
    class AllenBreadleyMachineIfVirtual : MachineIf, IVirtualMachineIf
    {
        public override bool IsConnected => this.isConnected;
        bool isConnected = true;

        public AllenBreadleyMachineIfVirtual(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        public void SetStateConnect(bool connect)
        {
            this.isConnected = connect;
        }

        public override void Release() { }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            return false;
        }
    }

    class AllenBreadleyMachineIf : MachineIf
    {
        ThreadHandler connectionCheckThread;

        // key: tagName, value: <tagIndex, dataSize, dataCount, accessLocker>
        Dictionary<string, Tuple<int, int, int, object>> tagList = new Dictionary<string, Tuple<int, int, int, object>>();
        object locker = new object();

        delegate IntPtr D_InitConsol();
        delegate void D_DisposeConsol();
        D_InitConsol InitConsol;
        D_DisposeConsol DisposeConsol;

        delegate string D_GetErrorString(int errCode);
        D_GetErrorString GetErrorString;

        delegate void D_InitPLC(string ipAddress, string cpuType, string path, int debugLevel);
        delegate void D_ReleasePLC();
        D_InitPLC InitPLC;
        D_ReleasePLC ReleasePLC;

        delegate void D_Register(int iTagIndex, string strTagName, int iDataType, int iCount);
        delegate void D_RegisterStr(int iTagIndex, string strTagName);
        delegate void D_UnRegister(int iTagIndex);
        D_Register Register;
        D_RegisterStr RegisterStr;
        D_UnRegister UnRegister;

        delegate bool D_IsConnected(int timeoutMs);
        D_IsConnected IsConnectedFunc;

        delegate void D_ReadDataS08(sbyte[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataU08(byte[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataS16(short[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataU16(ushort[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataS32(int[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataU32(uint[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataS64(long[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataU64(ulong[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataR32(float[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate void D_ReadDataR64(double[] value, int iTagIndex, int iArrayCount, int iTimeout);
        delegate string D_ReadString(int iTagIndex, int iTimeout);
        D_ReadDataS08 ReadDataS08;
        D_ReadDataU08 ReadDataU08;
        D_ReadDataS16 ReadDataS16;
        D_ReadDataU16 ReadDataU16;
        D_ReadDataS32 ReadDataS32;
        D_ReadDataU32 ReadDataU32;
        D_ReadDataS64 ReadDataS64;
        D_ReadDataU64 ReadDataU64;
        D_ReadDataR32 ReadDataR32;
        D_ReadDataR64 ReadDataR64;
        D_ReadString ReadString;

        delegate void D_SetTagValueS08(int iTagIndex, int iOffset, int iDataType, sbyte value);
        delegate void D_SetTagValueU08(int iTagIndex, int iOffset, int iDataType, byte value);
        delegate void D_SetTagValueS16(int iTagIndex, int iOffset, int iDataType, short value);
        delegate void D_SetTagValueU16(int iTagIndex, int iOffset, int iDataType, ushort value);
        delegate void D_SetTagValueS32(int iTagIndex, int iOffset, int iDataType, int value);
        delegate void D_SetTagValueU32(int iTagIndex, int iOffset, int iDataType, uint value);
        delegate void D_SetTagValueS64(int iTagIndex, int iOffset, int iDataType, long value);
        delegate void D_SetTagValueU64(int iTagIndex, int iOffset, int iDataType, ulong value);
        delegate void D_SetTagValueF32(int iTagIndex, int iOffset, int iDataType, float value);
        delegate void D_SetTagValueF64(int iTagIndex, int iOffset, int iDataType, double value);
        delegate void D_WriteString(int iTagIndex, int iTimeout, string @string);
        D_SetTagValueS08 SetTagValueS08;
        D_SetTagValueU08 SetTagValueU08;
        D_SetTagValueS16 SetTagValueS16;
        D_SetTagValueU16 SetTagValueU16;
        D_SetTagValueS32 SetTagValueS32;
        D_SetTagValueU32 SetTagValueU32;
        D_SetTagValueS64 SetTagValueS64;
        D_SetTagValueU64 SetTagValueU64;
        D_SetTagValueF32 SetTagValueF32;
        D_SetTagValueF64 SetTagValueF64;
        D_WriteString WriteString;

        delegate void D_WriteData(int iTagIndex, int iTimeout);
        D_WriteData WriteData;

        public override bool IsConnected => this.isConnected;
        bool isConnected = false;

        public static MachineIf Create(MachineIfSetting machineIfSetting)
        {
            if (machineIfSetting.IsVirtualMode)
                //return new AllenBreadleyMachineIf(machineIfSetting);
                return new AllenBreadleyMachineIfVirtual(machineIfSetting);

            return new AllenBreadleyMachineIf(machineIfSetting);
        }

        public AllenBreadleyMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            Assembly asm = Assembly.LoadFrom("ABPLC_TagCommW.dll");
            Type myStaticClassType = asm.GetExportedTypes().First(f => f.Name == "PLC");

            this.InitConsol = (D_InitConsol)Delegate.CreateDelegate(typeof(D_InitConsol), myStaticClassType.GetMethod("InitConsol"), true);
            this.DisposeConsol = (D_DisposeConsol)Delegate.CreateDelegate(typeof(D_DisposeConsol), myStaticClassType.GetMethod("DisposeConsol"), true);

            this.GetErrorString = (D_GetErrorString)Delegate.CreateDelegate(typeof(D_GetErrorString), myStaticClassType.GetMethod("GetErrorString"), true);

            this.InitPLC = (D_InitPLC)Delegate.CreateDelegate(typeof(D_InitPLC), myStaticClassType.GetMethod("InitPLC"), true);
            this.ReleasePLC = (D_ReleasePLC)Delegate.CreateDelegate(typeof(D_ReleasePLC), myStaticClassType.GetMethod("ReleasePLC"), true);

            this.Register = (D_Register)Delegate.CreateDelegate(typeof(D_Register), myStaticClassType.GetMethod("Register", new Type[] { typeof(int), typeof(string), typeof(int), typeof(int) }), true);
            this.RegisterStr = (D_RegisterStr)Delegate.CreateDelegate(typeof(D_RegisterStr), myStaticClassType.GetMethod("RegisterStr"), true);
            this.UnRegister = (D_UnRegister)Delegate.CreateDelegate(typeof(D_UnRegister), myStaticClassType.GetMethod("UnRegister"), true);

            this.IsConnectedFunc = (D_IsConnected)Delegate.CreateDelegate(typeof(D_IsConnected), myStaticClassType.GetMethod("IsConnected", new Type[] { typeof(int) }), true);

            this.ReadDataS08 = (D_ReadDataS08)Delegate.CreateDelegate(typeof(D_ReadDataS08), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(sbyte[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataU08 = (D_ReadDataU08)Delegate.CreateDelegate(typeof(D_ReadDataU08), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(byte[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataS16 = (D_ReadDataS16)Delegate.CreateDelegate(typeof(D_ReadDataS16), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(short[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataU16 = (D_ReadDataU16)Delegate.CreateDelegate(typeof(D_ReadDataU16), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(ushort[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataS32 = (D_ReadDataS32)Delegate.CreateDelegate(typeof(D_ReadDataS32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(int[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataU32 = (D_ReadDataU32)Delegate.CreateDelegate(typeof(D_ReadDataU32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(uint[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataS64 = (D_ReadDataS64)Delegate.CreateDelegate(typeof(D_ReadDataS64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(long[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataU64 = (D_ReadDataU64)Delegate.CreateDelegate(typeof(D_ReadDataU64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(ulong[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataR32 = (D_ReadDataR32)Delegate.CreateDelegate(typeof(D_ReadDataR32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(float[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadDataR64 = (D_ReadDataR64)Delegate.CreateDelegate(typeof(D_ReadDataR64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(double[]), typeof(int), typeof(int), typeof(int) }), true);
            this.ReadString = (D_ReadString)Delegate.CreateDelegate(typeof(D_ReadString), myStaticClassType.GetMethod("ReadString"), true);

            this.SetTagValueS08 = (D_SetTagValueS08)Delegate.CreateDelegate(typeof(D_SetTagValueS08), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(sbyte) }), true);
            this.SetTagValueU08 = (D_SetTagValueU08)Delegate.CreateDelegate(typeof(D_SetTagValueU08), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(byte) }), true);
            this.SetTagValueS16 = (D_SetTagValueS16)Delegate.CreateDelegate(typeof(D_SetTagValueS16), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(short) }), true);
            this.SetTagValueU16 = (D_SetTagValueU16)Delegate.CreateDelegate(typeof(D_SetTagValueU16), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(ushort) }), true);
            this.SetTagValueS32 = (D_SetTagValueS32)Delegate.CreateDelegate(typeof(D_SetTagValueS32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }), true);
            this.SetTagValueU32 = (D_SetTagValueU32)Delegate.CreateDelegate(typeof(D_SetTagValueU32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(uint) }), true);
            this.SetTagValueS64 = (D_SetTagValueS64)Delegate.CreateDelegate(typeof(D_SetTagValueS64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(long) }), true);
            this.SetTagValueU64 = (D_SetTagValueU64)Delegate.CreateDelegate(typeof(D_SetTagValueU64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(ulong) }), true);
            this.SetTagValueF32 = (D_SetTagValueF32)Delegate.CreateDelegate(typeof(D_SetTagValueF32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(float) }), true);
            this.SetTagValueF64 = (D_SetTagValueF64)Delegate.CreateDelegate(typeof(D_SetTagValueF64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(double) }), true);
            this.WriteString = (D_WriteString)Delegate.CreateDelegate(typeof(D_WriteString), myStaticClassType.GetMethod("WriteString"), true);

            this.WriteData = (D_WriteData)Delegate.CreateDelegate(typeof(D_WriteData), myStaticClassType.GetMethod("WriteData"), true);

            //MethodInfo myStaticMethodInfo2 = myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
            //var myStaticMethodResult = myStaticMethodInfo.Invoke(null, null);
        }

        public override void Initialize()
        {
            AllenBreadleyMachineIfSetting settings = (AllenBreadleyMachineIfSetting)machineIfSetting;

            this.InitPLC.Invoke(settings.PLC_IPADDRESS, settings.CPU_TYPE, settings.PLC_PATH, 0);

            List<AllenBreadleyMachineIfProtocol> list = settings.MachineIfProtocolList.Dic.Values.Cast<AllenBreadleyMachineIfProtocol>().ToList();
            list.RemoveAll(f => !f.IsValid || !f.Use);
            var groups = list.GroupBy(f => f.TagName);
            foreach(var group in groups)
            {
                string tagName = group.Key;
                int tagIndex = this.tagList.Count;
                int dataType = sizeof(Int32);
                int count = group.Max(f => f.OffsetByte4 + f.SizeByte4);

                this.tagList.Add(tagName, new Tuple<int, int, int, object>(tagIndex, dataType, count, this.locker));
                
                //Register(index, group.Key, sizeof(Int32), group.Max(f => f.OffsetByte4 + f.SizeByte4));
                //this.UnRegister(i);
            }

            this.connectionCheckThread = new ThreadHandler("ConnectionCheckThread", new System.Threading.Thread(ConnectionCheckThreadProc));
            this.connectionCheckThread.Start();
        }

        private void ConnectionCheckThreadProc()
        {
            while (!this.connectionCheckThread.RequestStop)
            {
                this.isConnected = IsConnectedFunc2();
                Thread.Sleep(5000);
                //if(!this.isConnected)
            }
        }

        public override void Release()
        {
            this.connectionCheckThread.Stop();
            this.ReleasePLC.Invoke();
        }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfProtocol)protocol;
            string tagName = allenBreadleyMachineIfProtocol.TagName;
            int tagId = this.tagList[tagName].Item1;
            int tagData = this.tagList[tagName].Item2;
            int tagCount = this.tagList[tagName].Item3;
            object locker = this.tagList[tagName].Item4;

            lock (locker)
            {
                try
                {
                    Register(tagId, tagName, tagData, tagCount);
                    System.Threading.Thread.Sleep(200);

                    int offsetByte4 = allenBreadleyMachineIfProtocol.OffsetByte4;
                    Int32[] resultValue;
                    if (args != null && args.Length > 0)
                    // 쓰기명령
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            if (args[i] != null)
                                this.SetTagValueS32(tagId, offsetByte4 + i, sizeof(Int32), Convert.ToInt32(args[i], 16));
                        }
                        this.WriteData(tagId, protocol.WaitResponceMs);
                        resultValue = new int[0];
                    }
                    else
                    // 읽기명령
                    {
                        resultValue = new Int32[allenBreadleyMachineIfProtocol.SizeByte4];
                        this.ReadDataS32(resultValue, tagId, allenBreadleyMachineIfProtocol.SizeByte4, protocol.WaitResponceMs);
                    }

                    if (protocolResponce != null || protocolResponce.IsResponced == false)
                    {
                        string recivedData = string.Join("", resultValue.Select(f => f.ToString("X08")));
                        protocolResponce.SetRecivedData(recivedData, true, null);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Network, ex);
                    return false;
                }
                finally
                {
                    this.UnRegister(tagId);
                    System.Threading.Thread.Sleep(200);
                }
            }
        }

        private bool IsConnectedFunc2()
        {
            //return IsConnectedFunc(1000);

            if (this.tagList.Count < 0)
                return false;

            string tagName = this.tagList.First().Key;
            int tagId = this.tagList[tagName].Item1;
            int tagData = this.tagList[tagName].Item2;
            int tagCount = this.tagList[tagName].Item3;
            object locker = this.tagList[tagName].Item4;

            return Send(tagId, tagName, tagData, tagCount, 5000, new int[0]);
        }

        private bool Send(int tagId, string tagName, int tagData, int tagCount, int timeoutMs, int[] data)
        {
            lock (locker)
            {
                Register(tagId, tagName, tagData, tagCount);
                System.Threading.Thread.Sleep(250);

                int sizeByte4 = data.Length * 4;
                try
                {
                    this.ReadDataS32(data, tagId, sizeByte4, timeoutMs);
                    return true;
                }
                catch (Exception ex)
                {
                    if (this.isConnected == true)
                        LogHelper.Error(LoggerType.Network, ex);
                    return false;
                }
                finally
                {
                    UnRegister(tagId);
                    System.Threading.Thread.Sleep(250);
                }
            }
        }
    }

    public class AllenBreadleyMachineIfSetting : TcpIpMachineIfSetting
    {
        public string PLC_IPADDRESS { get => this.tcpIpInfo.IpAddress; set => this.tcpIpInfo.IpAddress = value; } // PLC IP Address
        public string CPU_TYPE { get; set; } = "LGX"; // CPU TYPE
        public string PLC_PATH { get; set; } = "1,0"; // PLC Path

        public AllenBreadleyMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        public AllenBreadleyMachineIfSetting() : base(MachineIfType.AllenBreadley) { }

        public override MachineIfSetting Clone()
        {
            AllenBreadleyMachineIfSetting newSettings = new AllenBreadleyMachineIfSetting(this);
            return newSettings;
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            //this.PLC_IPADDRESS = XmlHelper.GetValue(xmlElement, "IPADDRESS", this.PLC_IPADDRESS);
            this.CPU_TYPE = XmlHelper.GetValue(xmlElement, "CPUTYPE", this.CPU_TYPE);
            this.PLC_PATH = XmlHelper.GetValue(xmlElement, "PATH", this.PLC_PATH);
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            //XmlHelper.SetValue(xmlElement, "IPADDRESS", this.PLC_IPADDRESS);
            XmlHelper.SetValue(xmlElement, "CPUTYPE", this.CPU_TYPE);
            XmlHelper.SetValue(xmlElement, "PATH", this.PLC_PATH);
        }
    }

}
