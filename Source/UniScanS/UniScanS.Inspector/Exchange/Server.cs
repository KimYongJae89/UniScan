using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.Inspector;
using UniScanS.Inspector.Data;

namespace UniScanS.Inspector.Exchange
{
    class Server : TcpIpMachineIfServer
    {
        List<SlaveObj> slaveList = new List<SlaveObj>();
        internal List<SlaveObj> SlaveList
        {
            get { return slaveList; }
        }

        public Server(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            foreach (InspectorInfo inspectorInfo in InspectorSystemSettings.Instance().SlaveInfoList)
            {
                SlaveInfo slaveInfo = new SlaveInfo();
                slaveInfo.Path = inspectorInfo.Path;

                slaveList.Add(new SlaveObj(slaveInfo));
            }

            //Initialize();
            //Start();
        }

        public override void Initialize()
        {
            base.Initialize();
            //Start();
        }
    }
}
