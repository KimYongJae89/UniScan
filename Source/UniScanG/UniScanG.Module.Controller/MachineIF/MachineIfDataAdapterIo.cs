using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor;

namespace UniScanG.Module.Controller.MachineIF
{
    public class IoMachineIfDataAdapter : UniScanG.Gravure.MachineIF.IoMachineIfDataAdapter
    {
        IoMachineIfProtocolSet setVisionStateProtocolSet;

        public IoMachineIfDataAdapter(MachineIfData machineIfData) : base(machineIfData)
        {
            this.setVisionStateProtocolSet = GetProtocolSet(UniScanGMelsecProtocolSet.SET_VISION_STATE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_READY,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL
             );
        }

        public override void Read()
        {
            MachineIfData machineIfData = (MachineIfData)this.machineIfData;

            if (!machineIfData.IsConnected)
                return;

            foreach (KeyValuePair<IoMachineIfProtocol, Tuple<Enum, int, bool>[]> pair in this.getMachineStateProtocolSet.Dictionary)
            {
                uint value = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInputGroup(pair.Key.DeviceNo, pair.Key.GroupNo);
                Parse(value, pair.Value);
            }

        }

        protected override void Parse(uint dioValue, Tuple<Enum, int, bool>[] tuples)
        {
            base.Parse(dioValue, tuples);

            MachineIfData machineIfData = (MachineIfData)this.machineIfData;

            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_READY, ref machineIfData.SET_VISION_GRAVURE_INSP_READY, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING, ref machineIfData.SET_VISION_GRAVURE_INSP_RUNNING, tuples);

            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT, ref machineIfData.SET_VISION_GRAVURE_INSP_RESULT, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN, tuples);

            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN, tuples);
            Parse(dioValue, MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE, ref machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE, tuples);
        }

        public override void Write()
        {
            foreach (KeyValuePair<IoMachineIfProtocol, Tuple<Enum, int, bool>[]> pair in this.setVisionStateProtocolSet.Dictionary)
            {
                DioValue dioValue = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput();
                uint value = dioValue.GetValue(pair.Key.DeviceNo, pair.Key.GroupNo);

                uint mask;
                uint newValue = MakeArgument(pair.Value, out mask);

                uint writeValue = (uint)((value & ~mask) | newValue);
                DioValue newDioValue = new DioValue();
                newDioValue.AddValue(pair.Key.DeviceNo, pair.Key.GroupNo, writeValue);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(newDioValue);
            }
        }

        protected override uint MakeArgument(Tuple<Enum, int, bool>[] tuples, out uint mask)
        {
            MachineIfData machineIfData = this.machineIfData as MachineIfData;

            uint value = 0;
            uint tempMask = 0;
            Array.ForEach(tuples, f =>
            {
                tempMask |= ((uint)0x01 << f.Item2);

                bool b = GetValue(machineIfData, f.Item1) == (f.Item3 ? false : true);
                uint v = (uint)(b ? ((uint)0x01 << f.Item2) : 0);
                value |= v;
            });
            mask = tempMask;
            return value;
        }

        private bool GetValue(MachineIfData machineIfData, Enum item1)
        {
            switch (item1)
            {
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_READY:
                    return machineIfData.SET_VISION_GRAVURE_INSP_READY;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING:
                    return machineIfData.SET_VISION_GRAVURE_INSP_RUNNING;

                case MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT:
                    return machineIfData.SET_VISION_GRAVURE_INSP_RESULT;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_ALL > 0 ? true : false;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_NG > 0 ? true : false;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_PINHOLE > 0 ? true : false;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_COATING > 0 ? true : false;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK > 0 ? true : false;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT:
                    return machineIfData.SET_VISION_GRAVURE_INSP_CNT_NOPRINT > 0 ? true : false;

                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE;
                case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL:
                    return machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL;

                case MelsecProtocol.GET_FORCE_GRAVURE_ERASER:
                    return machineIfData.GET_FORCE_GRAVURE_ERASER;

                case MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY:
                    return machineIfData.SET_VISION_GRAVURE_ERASER_READY;
                case MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING:
                    return machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING;
                case MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE:
                    return machineIfData.SET_VISION_GRAVURE_ERASER_CNT_ERASE > 0 ? true : false;
            }

            return false;
        }
    }

}
