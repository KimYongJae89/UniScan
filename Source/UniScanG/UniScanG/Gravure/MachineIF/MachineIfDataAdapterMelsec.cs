using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.MachineIF;

namespace UniScanG.Gravure.MachineIF
{
    public abstract class MelsecMachineIfDataAdapter : MachineIfDataAdapterG
    {
        protected MelsecMachineIfProtocolSet getMachineStateProtocolSet;

        protected abstract string MakeArgument(Tuple<Enum, int, int>[] tuples);

        public MelsecMachineIfDataAdapter(UniEye.Base.MachineInterface.MachineIfDataBase machineIfData) : base(machineIfData)
        {
            this.getMachineStateProtocolSet = GetProtocolSet(ProtocolSet.GET_MACHINE_STATE,
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
        }

        protected MelsecMachineIfProtocolSet GetProtocolSet(Enum command, params Enum[] subCommand)
        {
            MachineIfProtocol[] machineIfProtocols = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList.GetProtocols();

            // 명렁어 -> 프로토콜 변환
            List<MelsecMachineIfProtocol> wholeProtocol = Array.FindAll(machineIfProtocols, f => f.Use && subCommand.Contains(f.Command)).Cast<MelsecMachineIfProtocol>().ToList();

            // 정렬
            wholeProtocol.Sort(new Comparison<MelsecMachineIfProtocol>((f,g)=> f.Address.CompareTo(g.Address)));

            //wholeProtocol.Sort(new MelsecMachineIfProtocolComp());

            MelsecMachineIfProtocolSet protocolSet = new MelsecMachineIfProtocolSet(command);
            if (wholeProtocol.Count == 0)
                return protocolSet;

            // 연결된 주소끼리 그룹화
            List<List<MelsecMachineIfProtocol>> subProtocolList = new List<List<MelsecMachineIfProtocol>>();
            subProtocolList.Add(new List<MelsecMachineIfProtocol>());
            subProtocolList[0].Add(wholeProtocol[0]);
            wholeProtocol.Aggregate((f, g) =>
            {
                int nextAddr = int.Parse(f.Address.Substring(1)) + f.SizeWord;
                if (f.IsReadCommand != g.IsReadCommand || int.Parse(g.Address.Substring(1)) != nextAddr)
                    subProtocolList.Add(new List<MelsecMachineIfProtocol>());
                subProtocolList.LastOrDefault().Add(g);
                return g;
            });

            // 각 그룹별 프로토콜 생성
            subProtocolList.ForEach(f =>
            {
                List<Tuple<Enum, int, int>> tupleList = new List<Tuple<Enum, int, int>>();
                int baseAddr = int.Parse(f[0].Address.Substring(1));
                f.ForEach(g =>
                {
                    int addr = int.Parse(g.Address.Substring(1));
                    int offset = (addr - baseAddr) * 2;
                    int size = g.SizeWord * 2;
                    tupleList.Add(new Tuple<Enum, int, int>(g.Command, offset, size));
                });

                if (tupleList.Count > 0)
                {
                    MelsecMachineIfProtocol subProtocol = new MelsecMachineIfProtocol(null);
                    Debug.Assert(f.TrueForAll(g => g.IsReadCommand == f[0].IsReadCommand));
                    subProtocol.WaitResponceMs = f.Max(g => g.WaitResponceMs);
                    subProtocol.IsReadCommand = f[0].IsReadCommand;
                    subProtocol.Address = f[0].Address;
                    subProtocol.SizeWord = tupleList.Sum(g => g.Item3) / 2;
                    subProtocol.Use = true;
                    protocolSet.AddProtocol(subProtocol, tupleList.ToArray());
                }
            });

            return protocolSet;
        }

        protected void Read(MelsecMachineIfProtocolSet melsecMachineIfProtocolSet)
        {
            foreach (KeyValuePair<MelsecMachineIfProtocol, Tuple<Enum, int, int>[]> pair in melsecMachineIfProtocolSet.Dictionary)
            {
                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(pair.Key);
                responce.WaitResponce();

                if (responce.IsGood && responce.IsResponced)
                    Parse(responce, pair.Value);
            }
        }

        public virtual void Parse(MachineIfProtocolResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            MachineIfData machineIfData = (MachineIfData)this.machineIfData;

            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_STILLIMAGE, ref machineIfData.GET_START_STILLIMAGE, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_COLORSENSOR, ref machineIfData.GET_START_COLORSENSOR, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_EDMS, ref machineIfData.GET_START_EDMS, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_PINHOLE, ref machineIfData.GET_START_PINHOLE, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_RVMS, ref machineIfData.GET_START_RVMS, tuples);
            Parse(responce.ReciveData, MelsecProtocolCommon.GET_TARGET_SPEED, ref machineIfData.GET_TARGET_SPEED, tuples);
            Parse(responce.ReciveData, MelsecProtocolCommon.GET_PRESENT_SPEED, ref machineIfData.GET_PRESENT_SPEED, tuples);
            Parse(responce.ReciveData, MelsecProtocolCommon.GET_PRESENT_POSITION, ref machineIfData.GET_PRESENT_POSITION, tuples);
            Parse<string>(responce.ReciveData, MelsecProtocolCommon.GET_LOT, ref machineIfData.GET_LOT, tuples);
            Parse<string>(responce.ReciveData, MelsecProtocolCommon.GET_MODEL, ref machineIfData.GET_MODEL, tuples);
            Parse<string>(responce.ReciveData, MelsecProtocolCommon.GET_WORKER, ref machineIfData.GET_WORKER, tuples);
            Parse<string>(responce.ReciveData, MelsecProtocolCommon.GET_PASTE, ref machineIfData.GET_PASTE, tuples);

            Parse(responce.ReciveData, MelsecProtocolCommon.GET_ROLL_DIAMETER, ref machineIfData.GET_ROLL_DIAMETER, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_REWINDER_CUT, ref machineIfData.GET_REWINDER_CUT, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR, ref machineIfData.GET_START_GRAVURE_INSPECTOR, tuples);
            Parse<bool>(responce.ReciveData, MelsecProtocolCommon.GET_START_GRAVURE_ERASER, ref machineIfData.GET_START_GRAVURE_ERASER, tuples);
        }

        private bool Parse(string data, Enum command, ref float value, Tuple<Enum, int, int>[] tuples, float div)
        {
            Tuple<Enum, int, int> tuple = Array.Find(tuples, f => f.Item1.Equals(command));
            if (tuple == null)
                return false;

            try
            {
                value = Parse<int>(data, tuple.Item2 * 2, tuple.Item3 * 2) / div;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool Parse<T>(string data, Enum command, ref T value, Tuple<Enum, int, int>[] tuples)
        {
            Tuple<Enum, int, int> tuple = Array.Find(tuples, f => f.Item1.Equals(command));
            if (tuple == null)
                return false;

            try
            {
                value = Parse<T>(data, tuple.Item2 * 2, tuple.Item3 * 2);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"MelsecMachineIfDataAdapter::Parse - {ex.GetType().Name} - {ex.Message} - {command}", true);
                return false;
            }
        }

        private T Parse<T>(string reciveData, int start, int count)
        {
            string hexBytes = reciveData;
            string hexData = hexBytes.Substring(start, count);

            Type type = typeof(T);
            if (type.Name == "String")
            {
                int len = hexData.Length / 2;
                char[] chars = new char[len];
                for (int i = 0; i < len; i += 2)
                {
                    chars[i] = (char)Convert.ToByte(hexData.Substring((i + 1) * 2, 2), 16);
                    chars[i + 1] = (char)Convert.ToByte(hexData.Substring(i * 2, 2), 16);
                }
                string str = new string(chars).Trim(' ', '\0');
                return (T)Convert.ChangeType(str, typeof(T));
            }
            else
            {
                char[] convertChars = new char[count];
                int wards = count / 4;
                for (int i = 0; i < wards; i++)
                {
                    for (int j = 0; j < 4; j++)
                        convertChars[4 * i + j] = hexData[4 * (wards - 1 - i) + j];
                }
                string convertString = string.Concat(convertChars);
                uint decInt = uint.Parse(convertString, System.Globalization.NumberStyles.AllowHexSpecifier);
                return (T)Convert.ChangeType(decInt, typeof(T));
            }
        }

        protected void Write(MelsecMachineIfProtocolSet melsecMachineIfProtocolSet)
        {
            foreach (KeyValuePair<MelsecMachineIfProtocol, Tuple<Enum, int, int>[]> pair in melsecMachineIfProtocolSet.Dictionary)
            {
                string arg = MakeArgument(pair.Value);
                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(pair.Key, arg);
                responce.WaitResponce();
            }
        }
    }

    public class MelsecMachineIfProtocolSet : MelsecMachineIfProtocol
    {
        // this.Name: 통합명령 이름
        // this.Address: [Empty]

        // Key.Name: [Empty]
        // Key.Address: 연속된 주소의 시작
        // Key.Size: 연속된 주소의 크기

        // Value[i].Item1: 연속된 주소를 이루는 명령의 이름
        // Value[i].Item2: 연속된 주소 내 하위 명령의 위치
        // Value[i].Item2: 연속된 주소 내 하위 명령의 크기

        public Dictionary<MelsecMachineIfProtocol, Tuple<Enum, int, int>[]> Dictionary => this.dictionary;
        Dictionary<MelsecMachineIfProtocol, Tuple<Enum, int, int>[]> dictionary;

        public MelsecMachineIfProtocolSet(Enum command) : base(command)
        {
            this.dictionary = new Dictionary<MelsecMachineIfProtocol, Tuple<Enum, int, int>[]>();
        }

        public void AddProtocol(MelsecMachineIfProtocol protocol, Tuple<Enum, int, int>[] tuples)
        {
            dictionary.Add(protocol, tuples);
        }
    }
}
