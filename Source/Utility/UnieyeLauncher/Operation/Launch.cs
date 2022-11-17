using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    class LaunchSettings : SubSettings
    {
        public string FileName { get => fileName; set => fileName = value; }
        string fileName = "";

        public LaunchSettings() : base(true) { }

        protected override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.fileName = xmlElement["FileName"].InnerText;
        }

        protected override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlElement subElementFileName = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement("FileName"));
            subElementFileName.InnerText = fileName;
        }
    }

    class LaunchOperator : Operator
    {
        LaunchSettings subSettingLaunch;

        public override bool Use => true;
        public string LaunchTarget { get => this.launchTarget; set => this.launchTarget = value; }
        string launchTarget;

        public LaunchOperator(LaunchSettings subSettingLaunch) : base()
        {
            this.subSettingLaunch = subSettingLaunch;
        }

        internal bool Run(string pathName, params string[] args)
        {
            string fullPath = Path.Combine(this.workingDirectory, pathName);
            if (File.Exists(fullPath))
            {
                string arg = string.Join(" ", args);
                WriteLog($"Launcher::Run - {fullPath} {arg}");
                AppHandler.StartApp(fullPath, arg);
                this.launchTarget = Path.GetFileNameWithoutExtension(fullPath);

                return true;
            }

            return false;
        }

        internal void Kill(string[] processName)
        {
            AppHandler.KillApp(processName);
        }

        internal void Stop()
        {
            if (!string.IsNullOrEmpty(launchTarget))
            {
                WriteLog($"Launcher::Stop - {launchTarget}");
                AppHandler.KillApp(new string[] { launchTarget });
            }
        }
    }
}
