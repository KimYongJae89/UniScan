using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.MachineIF;
using UniScanG.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor;

namespace UniScanG.Module.Controller.MachineIF
{
    public class MelsecMachineIfDataAdapterCM : MelsecMachineIfDataAdapter
    {
        MelsecMachineIfProtocolSet setVisionStateProtocolSet;
        MelsecMachineIfProtocolSet getEraserStateProtocolSet;
        MelsecMachineIfProtocolSet setEraserStateProtocolSet;

        public MelsecMachineIfDataAdapterCM(MachineIfData machineIfData) : base(machineIfData)
        {
            this.setVisionStateProtocolSet = GetProtocolSet(UniScanGMelsecProtocolSet.SET_VISION_STATE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_READY,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_SHTLEN
             );

            if (MonitorSystemSettings.Instance().UseLaserBurner == LaserMode.Virtual)
            {
                this.getEraserStateProtocolSet = GetProtocolSet(UniScanGMelsecProtocolSet.GET_ERASER_STATE,
                    MelsecProtocol.GET_FORCE_GRAVURE_ERASER
                    );

                this.setEraserStateProtocolSet = GetProtocolSet(UniScanGMelsecProtocolSet.SET_ERASER_STATE,
                    MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY,
                    MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING,
                    MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE
                    );
            }
        }

        public override void Read()
        {
            MachineIfData machineIfData = (MachineIfData)this.machineIfData;

            if (!machineIfData.IsConnected)
                return;

            Read(this.getMachineStateProtocolSet);
            if (this.getEraserStateProtocolSet != null)
                Read(this.getEraserStateProtocolSet);
        }

        public override void Parse(MachineIfProtocolResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            base.Parse(responce, tuples);

            MachineIfData machineIfData = this.machineIfData as MachineIfData;
            Parse<bool>(responce.ReciveData, MelsecProtocol.GET_FORCE_GRAVURE_ERASER, ref machineIfData.GET_FORCE_GRAVURE_ERASER, tuples);
        }

        public override void Write()
        {
            MachineIfData machineIfData = (MachineIfData)this.machineIfData;
            if (!machineIfData.IsConnected)
                return;

            Write(this.setVisionStateProtocolSet);
            if (this.setEraserStateProtocolSet != null)
                Write(this.setEraserStateProtocolSet);
        }

        protected override string MakeArgument(Tuple<Enum, int, int>[] tuples)
        {
            MachineIfData machineIfData = this.machineIfData as MachineIfData;
            string[] aa = Array.ConvertAll<Tuple<Enum, int, int>, string>(tuples, f =>
            {
                string format = string.Format("X{0}", f.Item3 * 2);
                string v;
                switch (f.Item1)
                {
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_READY:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_READY ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_RUNNING ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_RESULT ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN ? 1 : 0).ToString(format); break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_ALL.ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_NG.ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_PINHOLE.ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_COATING.ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK.ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT:
                        v = machineIfData.SET_VISION_GRAVURE_INSP_CNT_NOPRINT.ToString(format); break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL:
                        v = (machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL ? 1 : 0).ToString(format); break;

                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE:
                        v = ((int)Math.Round(machineIfData.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE)).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_SHTLEN:
                        v = ((int)Math.Round(machineIfData.SET_VISION_GRAVURE_INSP_INFO_SHTLEN)).ToString(format); break;

                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY:
                        v = (machineIfData.SET_VISION_GRAVURE_ERASER_READY ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING:
                        v = (machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING ? 1 : 0).ToString(format); break;
                    case MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE:
                        v = machineIfData.SET_VISION_GRAVURE_ERASER_CNT_ERASE.ToString(string.Format("X{0}", f.Item3)); break;
                    default:
                        v = new string('0', f.Item3); break;
                }

                // Word Reverse 옵션화 필요함...
                //if (f.Item3 > 1)
                //{
                //    string[] tokens = new string[v.Length / 4];
                //    for (int i = 0; i < tokens.Length; i++)
                //        tokens[i] = v.Substring(i * 4, 4);
                //    string vv = string.Join("", tokens.Reverse());
                //    return vv;
                //}
                return v;
            });


            return string.Join("", aa);
        }
    }

}
