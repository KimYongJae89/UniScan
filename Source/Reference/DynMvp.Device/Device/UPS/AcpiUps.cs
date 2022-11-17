using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Device.Device.UPS
{
    public class AcpiUpsSetting : UpsSetting
    {
        public AcpiUpsSetting() : base(UpsType.ACPI) { }
    }

    public class AcpiUps : Ups
    {
        public AcpiUps(AcpiUpsSetting upsSetting) : base(upsSetting) { }

        public override SystemPowerStatus GetPowerState()
        {
            PowerStatus ps = SystemInformation.PowerStatus;

            bool isBattaryExist = (ps.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery);
            bool isPowerOnline = (ps.PowerLineStatus == PowerLineStatus.Online);
            int battaryLifePercent = (int)(ps.BatteryLifePercent * 100);
            int battaryRemainTimeSec = ps.BatteryLifeRemaining;

            SystemPowerStatus status = new SystemPowerStatus(isBattaryExist, isPowerOnline, battaryLifePercent, battaryRemainTimeSec);
            return status;
        }
    }
}
