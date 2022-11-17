using DynMvp.Base;
using DynMvp.Device.Device.UPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniEye.Base.Data
{
    public delegate void PowerManagerEventDelegate();

    public class PowerManager: ThreadHandler
    {
        public event PowerManagerEventDelegate OnPowerStateOffline;
        public event PowerManagerEventDelegate OnBattLevelCritical;

        Ups ups;
        SystemPowerStatus lastPowerStatus;

        public PowerManager() : base("PowerManager") { }

        public void Initialize()
        {
            this.ups = SystemManager.Instance().DeviceBox.Ups;
            if (this.ups == null)
                return;

            this.lastPowerStatus = new SystemPowerStatus(true, true, 100, -1);

            this.WorkingThread = new System.Threading.Thread(ThreadProc);
            this.requestStop = false;
        }

        private void ThreadProc()
        {
            int creticalState = 0;
            while (!this.requestStop)
            {
                Thread.Sleep(1000);

                SystemPowerStatus powerStatus = this.ups.GetPowerState();
                if (powerStatus != null)
                {
                    if (lastPowerStatus.IsBattaryExist && !powerStatus.IsBattaryExist)
                    {
                        ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.InvalidState, ErrorLevel.Error, "UPS", "UPS Battary Status Fault.", null, ""));
                    }

                    if (lastPowerStatus.IsPowerOnline && !powerStatus.IsPowerOnline)
                    {
                        ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.InvalidState, ErrorLevel.Error, "UPS", "UPS Power State Offline.", null, ""));
                        this.OnPowerStateOffline?.Invoke();
                    }

                    if (IsNormalState(powerStatus))
                        creticalState = -1;

                    creticalState++;
                    if (creticalState == 5)
                    {
                        ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.InvalidState, ErrorLevel.Error, "UPS", "UPS Battary Level Critical.", null, ""));
                        this.OnBattLevelCritical?.Invoke();
                    }
                    lastPowerStatus = powerStatus;
                }
            }
        }

        private bool IsNormalState(SystemPowerStatus systemPowerStatus)
        {
            return systemPowerStatus.IsPowerOnline || (systemPowerStatus.BattaryLifePercent > this.ups.UpsSetting.BattaryCriticalLevel);
        }

        private bool IsTurnOff(bool last, bool current)
        {
            return (last && !current);
        }

        internal static void ShutDown()
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-s -f -t 5");
        }
    }
}
