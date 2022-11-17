using DynMvp.Devices.Dio;
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
    public abstract class IoMachineIfDataAdapter : MachineIfDataAdapterG
    {
        protected IoMachineIfProtocolSet getMachineStateProtocolSet;

        protected abstract uint MakeArgument(Tuple<Enum, int, bool>[] tuples, out uint mask);

        public IoMachineIfDataAdapter(MachineIfDataBase machineIfData) : base(machineIfData)
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

        protected IoMachineIfProtocolSet GetProtocolSet(Enum command, params Enum[] subCommand)
        {
            MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
            MachineIfProtocol[] machineIfProtocols = list.GetProtocols();
            IoMachineIfProtocol[] protocols = Array.FindAll(machineIfProtocols, f => f.Use && subCommand.Contains(f.Command)).Cast<IoMachineIfProtocol>().ToArray();

            IoMachineIfProtocolSet protocolSet = new IoMachineIfProtocolSet(command);
            List<IGrouping<int, IoMachineIfProtocol>> dList = protocols.GroupBy(f => f.DeviceNo).ToList();
            dList.ForEach(f =>
            {
                List<IGrouping<int, IoMachineIfProtocol>> gList = f.GroupBy(g => g.GroupNo).ToList();
                gList.ForEach(g =>
                {
                    Tuple<Enum, int, bool>[] tuples = g.Select(h => new Tuple<Enum, int, bool>(h.Command, h.PortNo, h.ActiveLow)).ToArray();

                    IoMachineIfProtocol protocol = new IoMachineIfProtocol(tuples.First().Item1);
                    protocol.DeviceNo = f.Key;
                    protocol.GroupNo = g.Key;

                    protocolSet.AddProtocol(protocol, tuples);
                });
            });

            return protocolSet;
        }

        protected virtual void Parse(uint dioValue, Tuple<Enum, int, bool>[] tuples)
        {
            MachineIfData machineIfData = (MachineIfData)this.machineIfData;

            Parse(dioValue, MelsecProtocolCommon.GET_START_STILLIMAGE, ref machineIfData.GET_START_STILLIMAGE, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_COLORSENSOR, ref machineIfData.GET_START_COLORSENSOR, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_EDMS, ref machineIfData.GET_START_EDMS, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_PINHOLE, ref machineIfData.GET_START_PINHOLE, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_RVMS, ref machineIfData.GET_START_RVMS, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_TARGET_SPEED, ref machineIfData.GET_TARGET_SPEED, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_PRESENT_SPEED, ref machineIfData.GET_PRESENT_SPEED, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_PRESENT_POSITION, ref machineIfData.GET_PRESENT_POSITION, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_LOT, ref machineIfData.GET_LOT, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_MODEL, ref machineIfData.GET_MODEL, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_PASTE, ref machineIfData.GET_PASTE, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_ROLL_DIAMETER, ref machineIfData.GET_ROLL_DIAMETER, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_REWINDER_CUT, ref machineIfData.GET_REWINDER_CUT, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR, ref machineIfData.GET_START_GRAVURE_INSPECTOR, tuples);
            Parse(dioValue, MelsecProtocolCommon.GET_START_GRAVURE_ERASER, ref machineIfData.GET_START_GRAVURE_ERASER, tuples);
        }

        private uint GetV(uint dioValue, int shift)
        {
            return (uint)((dioValue >> shift) & 0x01);
        }

        protected void Parse(uint dioValue, Enum e, ref int value, Tuple<Enum, int, bool>[] tuples)
        {
            Tuple<Enum, int, bool> tuple = Array.Find(tuples, f => f.Item1.Equals(e));
            if (tuple == null)
                return;

            uint v = GetV(dioValue, tuple.Item2);
            value = (int)v;
        }

        protected void Parse(uint dioValue, Enum e, ref bool value, Tuple<Enum, int, bool>[] tuples)
        {
            Tuple<Enum, int, bool> tuple = Array.Find(tuples, f => f.Item1.Equals(e));
            if (tuple == null)
                return;

            uint v = GetV(dioValue, tuple.Item2);
            value = (v == (tuple.Item3 ? 0 : 1));
        }

        protected void Parse(uint dioValue, Enum e, ref float value, Tuple<Enum, int, bool>[] tuples)
        {
            Tuple<Enum, int, bool> tuple = Array.Find(tuples, f => f.Item1.Equals(e));
            if (tuple == null)
                return;

            uint v = GetV(dioValue, tuple.Item2);
            value = (float)v;
        }

        protected void Parse(uint dioValue, Enum e, ref string value, Tuple<Enum, int, bool>[] tuples)
        {
            Tuple<Enum, int, bool> tuple = Array.Find(tuples, f => f.Item1.Equals(e));
            if (tuple == null)
                return;

            uint v = GetV(dioValue, tuple.Item2);
            value = v.ToString();
        }

        public class IoMachineIfProtocolSet : IoMachineIfProtocol
        {
            // 동일한 DeviceNo, GroupNo를 가진 명령끼리 묶음.

            // this.Name: 통합명령 이름

            // Key.Name[i]: 그룹 이름
            // Key.DeviceNo: 그룹의 DeviceNo
            // Key.GroupNo: 그룹의 GroupNo

            // Value[i].Item1: 그룹 내 목록의 이름
            // Vlaue[i].Item2: 그룹 내 목록의 DIO Value 오프셋

            public Dictionary<IoMachineIfProtocol, Tuple<Enum, int, bool>[]> Dictionary => this.dictionary;
            Dictionary<IoMachineIfProtocol, Tuple<Enum, int, bool>[]> dictionary;

            public IoMachineIfProtocolSet(Enum command) : base(command)
            {
                this.dictionary = new Dictionary<IoMachineIfProtocol, Tuple<Enum, int, bool>[]>();
            }

            public void AddProtocol(IoMachineIfProtocol protocol, Tuple<Enum, int, bool>[] tuples)
            {
                dictionary.Add(protocol, tuples);
            }
        }
    }
}