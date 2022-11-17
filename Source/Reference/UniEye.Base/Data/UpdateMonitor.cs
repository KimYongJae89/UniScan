using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.Data
{
    public delegate void UpdateRequiredDelegate();
    public class WatchdogMonitor : ThreadHandler
    {
        public UpdateRequiredDelegate OnUpdateRequireChanged;

        private int checkTimeInterval;
        private string path;
        private string[] fileNames;
        private bool updateRequired = false;
        private int checkTimeIntervalMs;

        public bool UpdateRequired
        {
            get { return updateRequired; }
        }

        public WatchdogMonitor(int checkTimeIntervalMs = 2000, string path = null):base("WatchdogMonitor")
        {
            this.checkTimeIntervalMs = checkTimeIntervalMs;

            if (path == null)
                path = PathSettings.Instance().Update;
            this.path = path;

            fileNames = new string[]
            {
                "DynMvp.dll",
                "UmxService.dll",
                "DynMvp.Data.dll",
                "DynMvp.Device.dll",
                "DynMvp.Vision.dll",
                "DynMvp.Component.dll",
                "UniEye.Base.dll",
                "UniEye.exe",
                "MLCCPrintScan.exe"
            };
        }

        public void StartMonotering()
        {
            this.RequestStop = false;
            this.WorkingThread = new System.Threading.Thread(CheckUpdateProc);

            this.WorkingThread.Start();
        }
        
        private void CheckUpdateProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "UpdateMonitor::CheckUpdateProc Begin");
            while (this.RequestStop == false)
            {
                if (string.IsNullOrEmpty(path))
                    path = PathSettings.Instance().Update;

                foreach (string fileName in fileNames)
                {
                    string curFileVer = Path.Combine(Environment.CurrentDirectory, fileName);
                    string newFileVer = Path.Combine(path, fileName);
                    if (File.Exists(curFileVer) && File.Exists(newFileVer))
                    {
                        DateTime curFileWriteTime = File.GetLastWriteTime(curFileVer);
                        DateTime newFileWriteTime = File.GetLastWriteTime(newFileVer);
                        if (DateTime.Compare(curFileWriteTime, curFileWriteTime) > 0)
                        {
                            if (OnUpdateRequireChanged != null)
                                OnUpdateRequireChanged();
                        }
                    }
                }
                Thread.Sleep(checkTimeIntervalMs);
            }
            LogHelper.Debug(LoggerType.StartUp, "UpdateMonitor::CheckUpdateProc Finish");
        }
    }
}
