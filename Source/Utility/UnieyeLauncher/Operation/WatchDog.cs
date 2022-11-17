using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    class WatchdogSettings : SubSettings
    {
        public WatchdogSettings() : base(true) { }

        protected override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            List<string> targetExeFileNameList = new List<string>();
            XmlElement subElements = xmlElement["TargetExeFileNames"];
            if (subElements != null)
            {
                foreach (XmlElement subElement in subElements)
                {
                    if (subElement.Name == "TargetExeFile")
                        targetExeFileNameList.Add(subElement.InnerText);
                }
            }
        }

        protected override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
        }
    }

    class WatchdogItem
    {
        public bool Enable { get => enable; set => enable = value; }
        bool enable;

        public string ProcessName { get => processName; set => processName = value; }
        string processName;

        public string FilePathName { get => filePathName; set => filePathName = value; }
        string filePathName;

        public WatchdogItem(string processName, string filePathName)
        {
            this.enable = true;
            this.processName = processName;
            this.filePathName = filePathName;
        }

    }

    class WatchDogOperator : Operator
    {
        WatchdogSettings watchdogSetting;
        List<WatchdogItem> watchdogItemList;

        public override bool Use => this.watchdogSetting.Use;

        public WatchDogOperator(WatchdogSettings watchdogSetting) : base()
        {
            this.watchdogSetting = watchdogSetting;
            this.watchdogItemList = new List<WatchdogItem>();
        }

        public void Start(string filePathName, string processName = "")
        {
            if (string.IsNullOrEmpty(processName))
                processName = Path.GetFileNameWithoutExtension(filePathName);

            this.watchdogItemList.Clear();
            this.watchdogItemList.Add(new WatchdogItem(processName, filePathName));

            Start();
        }

        public void Start()
        {
            OnEvent(EventType.Start, "Watchdog Started");
            this.IsRun = true;
        }

        public void Stop()
        {
            OnEvent(EventType.Stop, "Watchdog Stopped");
            this.IsRun = false;
        }

        public void Process()
        {
            try
            {
                this.watchdogItemList.ForEach(f =>
                {
                    bool isEnable = f.Enable;
                    bool isAppRun = AppHandler.IsAppRun(f.ProcessName);
                    bool isAppDown = !isAppRun && AppHandler.LockFileExist(this.workingDirectory);

                    if (!isAppRun && !isAppDown)
                        f.Enable = false;
                    else if (isEnable && !isAppRun && isAppDown)
                    {
                        this.IsActive = true;
                        OnEvent(EventType.Message, "Watchdog Activate");
                        AppHandler.StartApp(f.FilePathName);
                        this.IsActive = false;
                    }
                });

                this.watchdogItemList.RemoveAll(f => f.Enable == false);
                if (this.watchdogItemList.Count == 0)
                    Stop();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

    }
}
