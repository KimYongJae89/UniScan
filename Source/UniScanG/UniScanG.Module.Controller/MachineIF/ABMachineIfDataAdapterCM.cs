using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.MachineIF;
using System.Reflection;

namespace UniScanG.Module.Controller.MachineIF
{
    class ABMachineIfDataAdapterCM : ABMachineIfDataAdapter
    {
        AllenBreadleyMachineIfProtocolSet c_PItoPLC;

        public ABMachineIfDataAdapterCM(MachineIfDataBase machineIfData) : base(machineIfData)
        {
            this.c_PItoPLC = GetProtocolSet(UniScanGMelsecProtocolSet.SET_VISION_STATE, "c_PItoPLC",
             MelsecProtocol.SET_VISION_GRAVURE_INSP_READY,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P,
             MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N,
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

            List<System.Reflection.FieldInfo> fieldInfoList = this.MachineIfData.GetType().GetFields().ToList();
            foreach (MelsecProtocol command in Enum.GetValues(typeof(MelsecProtocol)))
                this.c_PItoPLC.EnumFieldPair.Add(command, fieldInfoList.Find(f => f.Name == command.ToString()));
        }

        public override void Read()
        {
            base.Read();

            Read(this.c_PItoPLC);
        }

        public override void Write()
        {
            base.Write();

            Write(this.c_PItoPLC);
        }
    }
}
