using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.MachineIF;

namespace UniScanG.Gravure.MachineIF
{
    public class AllenBreadleyMachineIfProtocolSet : AllenBreadleyMachineIfProtocol
    {
        // this.Name: 통합 명령 이름
        // this.Address: 통합 시작주소

        // Key.Name: 통합 명령 이름
        // Key.Address: 연속된 주소의 시작
        // Key.Size: 연속된 주소의 크기

        // Value[i].Item1: 연속된 주소를 이루는 명령의 이름
        // Value[i].Item2: 연속된 주소 내 하위 명령의 위치
        // Value[i].Item2: 연속된 주소 내 하위 명령의 크기
        public Dictionary<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]> Dictionary => this.dictionary;
        Dictionary<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]> dictionary;

        public Dictionary<Enum, FieldInfo> EnumFieldPair => this.enumFieldPair;
        Dictionary<Enum, FieldInfo> enumFieldPair = new Dictionary<Enum, FieldInfo>();

        public AllenBreadleyMachineIfProtocolSet(Enum command) : base(command)
        {
            this.dictionary = new Dictionary<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]>();
        }

        internal void Add(AllenBreadleyMachineIfProtocol protocol, List<AllenBreadleyMachineIfProtocol> protocolList)
        {
            List<AllenBreadleyMachineIfProtocol> orderBy = protocolList.OrderBy(f => f.OffsetByte4).ToList();
            IEnumerable<Tuple<Enum, int, int>> enumerable = orderBy.Select(f => new Tuple<Enum, int, int>(f.Command, f.OffsetByte4, f.SizeByte4));
            this.dictionary.Add(protocol, enumerable.ToArray());
        }
    }

    public abstract class ABMachineIfDataAdapter : MachineIfDataAdapterG
    {
        AllenBreadleyMachineIfProtocolSet c_PLCtoUniEye;

        public ABMachineIfDataAdapter(MachineIfDataBase machineIfData) : base(machineIfData)
        {
            this.c_PLCtoUniEye = GetProtocolSet(ProtocolSet.GET_MACHINE_STATE, "c_PLCtoUniEye",
                MelsecProtocolCommon.GET_START_STILLIMAGE,
                MelsecProtocolCommon.GET_START_COLORSENSOR,
                MelsecProtocolCommon.GET_START_EDMS,
                MelsecProtocolCommon.GET_START_PINHOLE,
                MelsecProtocolCommon.GET_START_RVMS,
                MelsecProtocolCommon.GET_TARGET_SPEED,
                MelsecProtocolCommon.GET_PRESENT_SPEED,
                MelsecProtocolCommon.GET_PRESENT_POSITION,
                MelsecProtocolCommon.GET_LOT,
                MelsecProtocolCommon.GET_MODEL,
                MelsecProtocolCommon.GET_WORKER,
                MelsecProtocolCommon.GET_PASTE,
                MelsecProtocolCommon.GET_ROLL_DIAMETER,
                MelsecProtocolCommon.GET_REWINDER_CUT,
                MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR,
                MelsecProtocolCommon.GET_START_GRAVURE_ERASER);

            List<System.Reflection.FieldInfo> fieldInfoList = this.MachineIfData.GetType().GetFields().ToList();
            foreach (MelsecProtocolCommon command in Enum.GetValues(typeof(MelsecProtocolCommon)))
                this.c_PLCtoUniEye.EnumFieldPair.Add(command, fieldInfoList.Find(f => f.Name == command.ToString()));
        }

        protected AllenBreadleyMachineIfProtocolSet GetProtocolSet(Enum command, string tagName, params Enum[] subCommand)
        {
            MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
            List<AllenBreadleyMachineIfProtocol> protocolList = list.GetProtocols().Cast<AllenBreadleyMachineIfProtocol>().ToList();
            protocolList.RemoveAll(f => !f.IsValid || !f.Use || f.TagName != tagName || !subCommand.Contains(f.Command));

            AllenBreadleyMachineIfProtocolSet set = new AllenBreadleyMachineIfProtocolSet(command)
            {
                Use = true
            };

            // Write
            List<AllenBreadleyMachineIfProtocol> writeableProtocolList = protocolList.FindAll(f => f.IsWriteable);
            if (writeableProtocolList.Count > 0)
            {
                AllenBreadleyMachineIfProtocol protocolW = new AllenBreadleyMachineIfProtocol(null)
                {
                    WaitResponceMs = writeableProtocolList.Max(f => f.WaitResponceMs),
                    TagName = tagName,
                    Use = true,
                    OffsetByte4 = writeableProtocolList.Min(f => f.OffsetByte4),
                    SizeByte4 = writeableProtocolList.Max(f => f.OffsetByte4 + f.SizeByte4),
                    IsWriteable = true
                };
                set.Add(protocolW, protocolList);

                protocolList.RemoveAll(f => writeableProtocolList.Contains(f));
            }

            // Read
            if (protocolList.Count > 0)
            {
                AllenBreadleyMachineIfProtocol protocolR = new AllenBreadleyMachineIfProtocol(null)
                {
                    WaitResponceMs = protocolList.Max(f => f.WaitResponceMs),
                    TagName = tagName,
                    Use = protocolList.Count > 0,
                    OffsetByte4 = protocolList.Count == 0 ? 0 : protocolList.Min(f => f.OffsetByte4),
                    SizeByte4 = protocolList.Count == 0 ? 0 : protocolList.Max(f => f.OffsetByte4 + f.SizeByte4),
                    IsWriteable = false
                };
                set.Add(protocolR, protocolList);
            }

            return set;
        }

        public override void Read()
        {
            Read(this.c_PLCtoUniEye);
        }

        protected void Read(AllenBreadleyMachineIfProtocolSet set)
        {
            foreach (KeyValuePair<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]> pair in set.Dictionary)
            {
                if (pair.Key.IsWriteable)
                    continue;

                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(pair.Key);
                responce.WaitResponce();

                if (responce.IsGood && responce.IsResponced)
                    Parse(set.EnumFieldPair, responce, pair.Value);
            }
        }

        protected void Parse(Dictionary<Enum, FieldInfo> pairs, MachineIfProtocolResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            int l = responce.ReciveData.Length / 8;
            Int32[] values = new int[l];
            for (int i = 0; i < l; i++)
                values[i] = Convert.ToInt32(responce.ReciveData.Substring(i * 8, 8), 16);

            foreach (KeyValuePair<Enum, System.Reflection.FieldInfo> pair in pairs)
                Parse(values, pair.Value, Array.Find(tuples, f => f.Item1.Equals(pair.Key)));
        }

        protected void Parse(int[] values, FieldInfo fieldInfo, Tuple<Enum, int, int> tuple)
        {
            if (tuple == null)
                return;

            if (fieldInfo.FieldType.Name == "String")
            {
                List<Byte> byteList = new List<byte>();
                for (int i = 0; i < tuple.Item3; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(values[tuple.Item2 + i]);
                    byteList.AddRange(bytes.Reverse());
                }
                string strr = new string(byteList.ConvertAll<char>(f => (char)f).ToArray()).Trim('\0');
                fieldInfo.SetValue(this.machineIfData, strr);
            }
            else
            {
                int value = values[tuple.Item2];
                object var = Convert.ChangeType(value, fieldInfo.FieldType);
                //if ((MelsecProtocolCommon)tuple.Item1 == MelsecProtocolCommon.GET_TARGET_SPEED)
                //    var = (float)var / 10;
                //if ((MelsecProtocolCommon)tuple.Item1 == MelsecProtocolCommon.GET_PRESENT_SPEED)
                //    var = (float)var / 10;
                //if ((MelsecProtocolCommon)tuple.Item1 == MelsecProtocolCommon.GET_ROLL_DIAMETER)
                //    var = (float)var / 100;

                fieldInfo.SetValue(this.machineIfData, var);
            }
        }

        public override void Write()
        {
            Write(this.c_PLCtoUniEye);
        }

        protected void Write(AllenBreadleyMachineIfProtocolSet set)
        {
            foreach (KeyValuePair<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]> pair in set.Dictionary)
            {
                if (!pair.Key.IsWriteable)
                    continue;

                string[] arguments = GetArgument(pair, set.EnumFieldPair);
                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(pair.Key, arguments);
                responce.WaitResponce();
            }
        }

        private string[] GetArgument(KeyValuePair<AllenBreadleyMachineIfProtocol, Tuple<Enum, int, int>[]> pair,
            Dictionary<Enum, FieldInfo> enumFieldPair)
        {
            int[] arguments = new int[pair.Value.Max(f => f.Item2 + f.Item3)];

            Array.ForEach(pair.Value, f =>
            {
                if (enumFieldPair.ContainsKey(f.Item1))
                {
                    FieldInfo fieldInfo = enumFieldPair[f.Item1];
                    object value = fieldInfo.GetValue(this.machineIfData);
                    if (fieldInfo.FieldType.Name == "String")
                    {
                        char[] chars = ((string)value).ToArray();
                        Buffer.BlockCopy(chars, 0, arguments, f.Item2 * 4, chars.Length);
                    }
                    else
                    {
                        arguments[f.Item2] = (Int32)Convert.ChangeType(value, typeof(Int32));
                    }
                }
            });

            //return arguments;
            return arguments.Select(f => f.ToString("X08")).ToArray();
        }
    }
}
