using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using UnieyeLauncher.UI;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    class PatchSettings : SubSettings
    {

        public LaunchSettings LauncherSetting { get=>this.launcherSetting; }
        LaunchSettings launcherSetting = new LaunchSettings();

        public ArchiveSettings ArchiverSetting { get => archiverSetting; }
        ArchiveSettings archiverSetting = new ArchiveSettings();

        public PatchSettings() : base(true) { }

        protected override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.ArchiverSetting.Load(xmlElement, "BackupSetting");
        }

        protected override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            this.ArchiverSetting.Save(xmlElement, "BackupSetting");
        }
    }


    class PatchOperator : Operator
    {
        const string PatchDoneFileName = "PatchDate";
        const string PatchDoneTempFileName = "~PatchDate";

        public LaunchOperator Launcher => this.launcher;
        LaunchOperator launcher;

        public ArchiveOperator Archiver => this.archiver;
        ArchiveOperator archiver;

        private PatchSettings patchSetting;

        public override bool Use => this.patchSetting.Use;

        public bool IsProcessable => this.patchSetting.Use && File.Exists(PatchDoneFile);

        public string PatchDoneFile => Path.Combine(this.workingDirectory, "Update", PatchOperator.PatchDoneFileName);
        public string PatchDoneTempFile => Path.Combine(this.workingDirectory, "Update", PatchOperator.PatchDoneTempFileName);

        public PatchOperator(PatchSettings patchSetting) : base()
        {
            this.patchSetting = patchSetting;

            this.launcher = new LaunchOperator(patchSetting.LauncherSetting);
            this.archiver = new ArchiveOperator(patchSetting.ArchiverSetting);
        }

        public void Process(bool launchExist, bool watchdogState, string targetExe)
        {
            if (!this.IsProcessable)
                return;

            this.IsActive = true;

            OnEvent(EventType.Message, "Patch Activate");

            if (launchExist)
            {
                if (File.Exists(PatchDoneTempFile))
                    File.Delete(PatchDoneTempFile);
                File.Move(PatchDoneFile, PatchDoneTempFile);

                // 런처가 있으면, 런처를 실행
                ProgressForm progressForm = new ProgressForm("Wait", false, new DoWorkEventHandler(WaitPatch), true, targetExe);
                Program.MainForm.Invoke(new System.Windows.Forms.MethodInvoker(() => progressForm.ShowDialog(Program.MainForm)));

                string[] exeFiles = Directory.GetFiles(this.launcher.Update, "*.exe", SearchOption.TopDirectoryOnly);
                string[] files = Array.FindAll(exeFiles, f => f.Contains(Program.ProcessName));
                if (files.Length > 0)
                {
                    //this.launcher.Run(files[0], launchExist.ToString(), "true", targetExe, System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
                    string pathName = files[0];
                    string[] args = new string[] { launchExist.ToString(), (true).ToString(), targetExe, System.Diagnostics.Process.GetCurrentProcess().Id.ToString() };
                    this.launcher.Run(pathName, args);
                    System.Windows.Forms.MessageBox.Show($"Excute New Launcher Program, soon.");
                }
            }
            else
            {
                // 런처가 없으면 새 파일 복사하고 백업 실행.
                new ProgressForm("Patch", true, new DoWorkEventHandler(ProcessCopy), true).ShowDialog(Program.MainForm);
                ProcessClear();

                if (this.archiver.Use)
                {
                    // how to archive??
                    new ProgressForm("BackUp", true, new DoWorkEventHandler(this.archiver.StartArchive), true, targetExe).ShowDialog(Program.MainForm);
                }
            }
            this.IsActive = false;
        }

        private void WaitPatch(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, "");

            int step = 50;
            for (int i = 0; i < step; i++)
            {
                if (worker.CancellationPending)
                    break;

                System.Threading.Thread.Sleep(100);
                worker.ReportProgress((int)((i + 1) * 100f / step), "");
            }
        }

        public void ProcessCopy(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, "");

            DirectoryInfo srcInfo = new DirectoryInfo(this.Update);
            DirectoryInfo dstInfo = new DirectoryInfo(this.workingDirectory);
            bool recursive = true;

            FileInfo[] srcFiles = srcInfo.GetFiles("*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < srcFiles.Length; i++)
            {
                FileInfo f = srcFiles[i];
                FileInfo dstFileInfo = new FileInfo(f.FullName.Replace(srcInfo.FullName, dstInfo.FullName));
                if (!dstFileInfo.Directory.Exists)
                    dstFileInfo.Directory.Create();

                for (int j = 0; j < 10; j++)
                {
                    try
                    {
                        f.CopyTo(dstFileInfo.FullName, true);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(500);
                    }
                }
                worker.ReportProgress((int)((i + 1) * 100f / srcFiles.Length), "");
            }
        }

        public void ProcessClear()
        {
            AppHandler.Clear(new DirectoryInfo(this.Update));
        }
    }
}
