using System.Timers;
using UniScanM.Gloss;
using UniScanM.Gloss.MachineIF;

namespace WPF.UniScanCM.Manager
{
    public static class AliveService
    {
        private static Timer AliveCheckTimer { get; set; }
        public static bool Heart { get; private set; } = true;

        // 점등을 하는 경우 쓰는 함수
        public static void StartAliveCheckTimer(int interval = 500)
        {
            AliveCheckTimer = new Timer();
            AliveCheckTimer.Interval = interval;
            AliveCheckTimer.Elapsed += AliveCheckTimer_Elapsed;
            AliveCheckTimer.Start();
        }

        public static void StopAliveCheckTimer()
        {
            if (AliveCheckTimer != null)
                AliveCheckTimer.Stop();
        }

        private static void AliveCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //var plcDataExporter = SystemManager.Instance().DataExporterList.Find(x => x is MachineIfDataExporter) as MachineIfDataExporter;
            //if (plcDataExporter != null)
            //{
            //    plcDataExporter.ExportVisionState(Heart);
            //}
            Heart = !Heart;
        }
    }
}
