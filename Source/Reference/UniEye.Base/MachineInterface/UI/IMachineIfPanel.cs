using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface.UI
{
    interface IMachineIfPanel
    {
        void Initialize();
        void Initialize(MachineIfSetting machineIfSetting);
        void SetDefault();
        bool Verify();
        void Apply();
    }
}
