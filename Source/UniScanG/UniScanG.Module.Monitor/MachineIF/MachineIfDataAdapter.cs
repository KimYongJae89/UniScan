using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.MachineIF;
using UniScanG.MachineIF;

namespace UniScanG.Module.Monitor.MachineIF
{
    internal class MachineIfDataAdapterMM : UniScanG.Gravure.MachineIF.MelsecMachineIfDataAdapter
    {
        MelsecMachineIfProtocolSet setVitionStateProtocolSet;

        public MachineIfDataAdapterMM(MachineIfData machineIfData) : base(machineIfData)
        {
            this.setVitionStateProtocolSet = GetProtocolSet(UniScanGMelsecProtocol.SET_VISION_STATE,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_READY,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RUN,

                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RESULT,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_W,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_L,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_W,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_L,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_W,
                 MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_L
                 );
        }

        public override void Read()
        {
            if (!this.machineIfData.IsConnected)
                return;

            Read(this.getMachineStateProtocolSet);
        }

        public override void Write()
        {
            if (!this.machineIfData.IsConnected)
                return;

            Write(this.setVitionStateProtocolSet);
        }

        protected override string MakeArgument(Tuple<Enum, int, int>[] tuples)
        {
            MachineIfData machineIfData = this.machineIfData as MachineIfData;
            string[] aa = Array.ConvertAll<Tuple<Enum, int, int>, string>(tuples, f =>
            {
                string format = string.Format("X{0}", f.Item3 * 2);
                switch (f.Item1)
                {
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_READY:
                        return (machineIfData.SET_VISION_GRAVURE_MONITORING_READY ? 1 : 0).ToString(format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RUN:
                        return (machineIfData.SET_VISION_GRAVURE_MONITORING_RUN ? 1 : 0).ToString(format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_RESULT:
                        return (machineIfData.SET_VISION_GRAVURE_MONITORING_RESULT ? 1 : 0).ToString(format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_W:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_W, 10, format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_MARGIN_L:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_L, 10, format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_W:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_W, 10, format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_BLOT_L:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_L, 10, format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_W:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_W, 10, format);
                    case MelsecProtocol.SET_VISION_GRAVURE_MONITORING_DEFECT_L:
                        return GetWordSwapString(machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_L, 10, format);
                    default:
                        return new string('0', f.Item3);
                }
            });

            return string.Join("", aa);
        }
        private string GetWordSwapString(float value, float mul, string format)
        {
            char[] chars = ((int)(value * mul)).ToString(format).ToArray();
            char[] subChars = new char[4];
            for (int i = 0; i < chars.Length / 8; i++)
            {
                int i0 = i * 4;
                int i1 = (i + 1) * 4;
                Array.Copy(chars, i0, subChars, 0, 4);
                Array.Copy(chars, i1, chars, i0, 4);
                Array.Copy(subChars, 0, chars, i1, 4);
            }
            return new string(chars);
        }
    }
}
