using DynMvp.Device.Serial;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Gloss.Settings;
using WPF.UniScanCM.Manager;

namespace UniScanM.Gloss.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap)
        {
        }

        public override void PostInitialize()
        {
            // IO 설정
            var outputPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanM.Gloss.Device.PortMap.IoPortName.OutMC);
            outputPort.PortNo = GlossSettings.Instance().OutMCPort;
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(outputPort, true);

            // AliveService 켜기
            AliveService.StartAliveCheckTimer(2000);
        }
    }
}
