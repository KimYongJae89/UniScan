using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Comm;

namespace UniScanG.Inspector.Comm
{
    internal class Executer : MachineIfExecuter
    {
        protected override MachineIfProtocol ParseOperator(string command)
        {
            throw new NotImplementedException();
        }

        protected override bool Execute(MachineIfProtocol machineIfProtocol)
        {
            throw new NotImplementedException();
        }
    }
}
