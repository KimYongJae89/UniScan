using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;

namespace UniScanG.MachineIF
{
    enum ProtocolSet { GET_MACHINE_STATE };

    public class MachineIfDataAdapterG : UniEye.Base.MachineInterface.MachineIfDataAdapter
    {
        public new MachineIfData MachineIfData => this.machineIfData as MachineIfData;

        public MachineIfDataAdapterG(UniEye.Base.MachineInterface.MachineIfDataBase machineIfData) : base(machineIfData) { }

        public override void Read() { }

        public override void Write() { }
    }
}
