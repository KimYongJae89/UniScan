using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using UnieyeLauncher.Operation;

namespace UnieyeLauncher
{
    public delegate void ShoweHideDelegate(bool visible);
    public delegate void ShowBalloonTipDelegate(string tipText);
    public delegate void OnLaunchableFilesUpdated();

    internal class LauncherProcess
    {
        public event ShowBalloonTipDelegate ShowBalloonTip;
        public event ShoweHideDelegate ShowHide;
        public event OnLaunchableFilesUpdated OnLaunchableFilesUpdated;

        public bool IsRun => this.operators.Any(f => f.IsRun);

        public bool IsActive => this.operators.Any(f => f.IsActive);

        public UniEyeLauncherSetting Setting { get; private set; }

        public RemoteOperator RemoteOperator { get; private set; }

        public LaunchOperator LaunchOperator { get; private set; }

        public ArchiveOperator ArchiveOperator { get; private set; }

        public WatchDogOperator WatchDogOperator { get; private set; }

        public PatchOperator PatchOperator { get; private set; }

        public UPSOperator UPSOperator { get; private set; }

        public string[] ExcutableFileNames { get; private set; }

        Operator[] operators = null;

        System.Timers.Timer timer;
        string currentDirectory;

        public LauncherProcess(string currentDirectory, string targetExe)
        {
            this.currentDirectory = currentDirectory;

            UpdateLaunchableFiles();

            FileInfo configFileInfo = new FileInfo(Path.Combine(currentDirectory, UniEyeLauncherSetting.SettingFileNameS));
            if (configFileInfo.Exists)
            {
                this.Setting = UniEyeLauncherSetting.LoadSerialize(configFileInfo.FullName);
            }
            else
            {
                FileInfo settingFile = new FileInfo(Path.Combine(currentDirectory, UniEyeLauncherSetting.SettingFileName));
                if (!settingFile.Exists)
                {
                    FileInfo settingFile2 = new FileInfo(Path.Combine(currentDirectory, UniEyeLauncherSetting.SettingFileName2));
                    if (settingFile2.Exists)
                        File.Move(settingFile2.FullName, settingFile.FullName);
                }

                this.Setting = new UniEyeLauncherSetting();
                this.Setting.Load(settingFile.FullName);
                this.Setting.SaveSerialize(configFileInfo.FullName);
            }

            

            this.RemoteOperator = new RemoteOperator(this.Setting.RemoteSettings);
            this.RemoteOperator.EventHandler += Operator_EventHandler;

            if (!string.IsNullOrEmpty(targetExe))
                this.Setting.LaunchSettings.FileName = targetExe;

            this.LaunchOperator = new LaunchOperator(this.Setting.LaunchSettings);
            this.LaunchOperator.EventHandler += Operator_EventHandler;
            
            this.WatchDogOperator = new WatchDogOperator(this.Setting.WatchdogSettings);
            this.WatchDogOperator.EventHandler += Operator_EventHandler;

            this.ArchiveOperator = new ArchiveOperator(this.Setting.ArchiveSettings);
            this.ArchiveOperator.EventHandler += Operator_EventHandler;

            this.PatchOperator = new PatchOperator(this.Setting.PatchSettings);
            this.PatchOperator.EventHandler += Operator_EventHandler;

            this.UPSOperator = new UPSOperator(this.Setting.UPSSettings);
            this.UPSOperator.EventHandler += Operator_EventHandler;

            this.operators = new Operator[] { };
            this.timer = new System.Timers.Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            this.timer.Start();
            this.RemoteOperator.Start();
        }


        public void Operator_EventHandler(Operator sender, EventType eventType, string message)
        {
            switch (eventType)
            {
                case EventType.Start:
                    if (sender is WatchDogOperator)
                        ShowHide?.Invoke(false);
                    break;
                case EventType.Stop:
                    if(sender is WatchDogOperator)
                        ShowHide?.Invoke(true);
                    break;
                case EventType.Message:
                    ShowBalloonTip?.Invoke(message);
                    break;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                if (this.PatchOperator.Use && this.PatchOperator.IsProcessable)
                {
                    bool watchdogState = this.WatchDogOperator.IsRun;
                    this.WatchDogOperator.Stop();

                    string[] exeFiles = Directory.GetFiles(this.PatchOperator.Update, "*.exe", SearchOption.AllDirectories);
                    List<string> processNameList = Array.ConvertAll(exeFiles, f => Path.GetFileNameWithoutExtension(f)).ToList();

                    string currentProcessName = Program.ProcessName;
                    bool launcherExist = processNameList.Exists(f => f.Contains(currentProcessName));
                    if (launcherExist)
                        processNameList.RemoveAll(f => f.Contains(currentProcessName));

                    this.LaunchOperator.Kill(processNameList.ToArray());

                    if (processNameList.Count == 0 && !this.Setting.Silent)
                    {
                        Form form = Program.MainForm;
                        form.Invoke(new MethodInvoker(() => MessageBox.Show(form, Properties.Resources.NoExeMessage, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)));
                    }

                    string targetExe = "";
                    if (!string.IsNullOrEmpty(this.LaunchOperator.LaunchTarget))
                    {
                        targetExe = $"{this.LaunchOperator.LaunchTarget}.exe";
                    }
                    this.PatchOperator.Process(launcherExist, watchdogState, targetExe);

                    if (!launcherExist)
                    {
                        this.WatchDogOperator.Start();
                    }
                }
                else if (this.WatchDogOperator.IsRun)
                {
                    this.WatchDogOperator.Process();
                }
                UpdateLaunchableFiles();

                this.UPSOperator.CheckUpsStatus();
            }
            finally
            {
                timer.Start();
            }
        }

        private void UpdateLaunchableFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.currentDirectory, "Bin"));

            List<string> excutableFileNameList = new List<string>();
            if (directoryInfo.Exists)
                excutableFileNameList.AddRange(Array.ConvertAll(directoryInfo.GetFiles("*.exe"), f => Path.GetFileNameWithoutExtension(f.Name)));

            string[] excutableFileNames = excutableFileNameList.ToArray();
            if (!Compare(this.ExcutableFileNames, excutableFileNames))
            {
                this.ExcutableFileNames = excutableFileNames;
                this.OnLaunchableFilesUpdated?.Invoke();
            }
        }
        
        private bool Compare(string[] a, string[] b)
        {
            if (a == null || b == null)
                return false;

            bool exact = true;
            if (a.Length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                    exact &= (a[i] == b[i]);
            }
            else
            {
                exact = false;
            }
            return exact;
        }

        internal void SaveSetting()
        {
            //this.Setting.Save(Path.Combine(this.currentDirectory, UniEyeLauncherSetting.SettingFileName));

            this.Setting.SaveSerialize(Path.Combine(this.currentDirectory, UniEyeLauncherSetting.SettingFileNameS));
            
        }

        internal bool Launch(string launchTarget, params string[] args)
        {
            if (!this.Setting.LaunchSettings.Use)
                return false;

            string fullPath = Path.Combine(this.currentDirectory, "Bin", string.Format("{0}.exe", launchTarget));
            if (File.Exists(fullPath))
            {
                if (AppHandler.IsAppRun(launchTarget))
                    Operator.WriteLog($"LauncherProcess::Launch - launchTarget [{launchTarget}] is already running");
                else
                    this.LaunchOperator.Run(fullPath, args);

                this.WatchDogOperator.Start(fullPath);
                return true;
            }
            return false;
        }

        internal void Kill(string launchTarget = "")
        {
            this.WatchDogOperator.Stop();
            this.LaunchOperator.Stop();
        }
    }
}
