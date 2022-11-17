using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UnieyeLauncher.Operation;
using UnieyeLauncher.UI;

namespace UnieyeLauncher
{
    static class Program
    {
        public static string ProcessName => "UnieyeLauncher";
        public static MainForm MainForm => mainForm;
        static MainForm mainForm;

        public static string WorkingDirectory;

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] strArgs = Environment.GetCommandLineArgs();
            // strArgs[0]: 실행 경로.
            // strArgs[1]: 패치 단계(패치파일 적용 전 true, 적용 후 false)
            // strArgs[2]: 런처 작동 중 업데이트 실행함.
            // strArgs[3]: 메인 exe 파일 이름.
            // strArgs[4]: 기존 런처의 PID.
            string pid = Process.GetCurrentProcess().Id.ToString();
#if DEBUG
            MessageBox.Show(null, string.Join("/", strArgs), pid);
            MessageBox.Show(null, Environment.CurrentDirectory, pid);
#else
#endif

            Program.WorkingDirectory = Path.GetDirectoryName(strArgs[0]);
            try
            {
                Program.WorkingDirectory = Path.GetDirectoryName(strArgs[0]);

                //Operator.WriteLog($"Program::Main - CommandLine: { Environment.CommandLine}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.GetType().Name}- {ex.Message}", "Operator.WriteLog");
            }
            bool launchExist = false, autoRun = true;
            string targetExe = "";
            int callerPid = -1;
            if (strArgs.Length >= 3)                // 런처에서 실행됨
            {
                bool ok = bool.TryParse(strArgs[1], out launchExist) && bool.TryParse(strArgs[2], out autoRun);
                if (strArgs.Length > 3)
                    targetExe = strArgs[3];
                if (strArgs.Length > 4)
                    callerPid = int.Parse(strArgs[4]);

                if (ok)
                {
                    PatchProcess(launchExist, autoRun, targetExe, callerPid);

                    if (launchExist)
                    {
                        MessageBox.Show("This message will be closed automatic");
                        return;
                    }
                    AddStartupMenu(true);
                }
            }

            bool bNew;
            string mutexName = GetMutexName();
            Mutex mutex = new Mutex(true, mutexName, out bNew);
            if (bNew == false)
            {
                //MessageBox.Show("UniEye Launcher is already running");
                return;
            }

#if DEBUG
            MessageBox.Show("Normally Started");
#endif

            targetExe = Path.GetFileNameWithoutExtension(targetExe);
            string vertion = GetVertion();
            DateTime buildDateTime = GetBuildDateTime();
            mainForm = new MainForm(vertion, buildDateTime, autoRun, targetExe);
            Application.Run(mainForm);

            mutex.ReleaseMutex();
        }

        internal static string GetMutexName()
        {
            string path = Path.GetFileName(Program.WorkingDirectory);
            //string path = WorkingDirectory;
            //List<char> invalidCharList = new List<char>(Path.GetInvalidPathChars());
            //invalidCharList.AddRange(new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.VolumeSeparatorChar });
            //invalidCharList.ForEach(f =>
            //{
            //    string str = new string(f, 1);
            //    path = path.Replace(new string(f, 1), "");
            //});
            return string.Format("UniEyeLauncher_{0}", path);
        }

        private static void PatchProcess(bool doPatch, bool watchdogState, string targetExe, int callerPid)
        {
            // 기존 런처 프로그램 종료
            if (callerPid < 0)
            {
                int curProcessId = Process.GetCurrentProcess().Id;
                List<Process> processes = Array.FindAll(Process.GetProcesses(), f => f.ProcessName.Contains(ProcessName)).ToList();
                processes.RemoveAll(f => f.Id == curProcessId);
                AppHandler.KillApp(processes.ToArray());
            }
            else
            {
                try
                {
                    Process caller = Process.GetProcessById(callerPid);
                    AppHandler.KillApp(new Process[] { caller });
                }
                catch (Exception) { }
            }

            if (doPatch)
                Program.WorkingDirectory = Path.GetDirectoryName(Program.WorkingDirectory);

            PatchOperator patcher = new PatchOperator(new PatchSettings());
            if (doPatch)
            {
                try
                {
                    // 복사 전 BAT파일은 제거함.
                    AppHandler.Clear(new DirectoryInfo(patcher.Update), "*.bat");

                    // 복사
                    new ProgressForm("Patch", true, new System.ComponentModel.DoWorkEventHandler(patcher.ProcessCopy), true).ShowDialog();

                    // 기존 런처 이름의 V2 제거
                    FileInfo launcherFile = new FileInfo(Path.Combine(Program.WorkingDirectory, "UnieyeLauncher.exe"));
                    FileInfo launcherV2File = new FileInfo(Path.Combine(Program.WorkingDirectory, "UnieyeLauncherV2.exe"));
                    if (launcherV2File.Exists)
                    {
                        if (launcherFile.Exists)
                            launcherFile.Delete();
                        File.Move(launcherV2File.FullName, launcherFile.FullName);
                    }

                    // 복사된 런처 실행
                    LaunchOperator launcher = new LaunchOperator(new LaunchSettings());
                    string processName = Program.ProcessName;
                    launcher.Run($"{processName}.exe", "false", watchdogState.ToString(), targetExe, Process.GetCurrentProcess().Id.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace.ToString());
                }
                finally
                {
            
                }
            }
            else
            {
                // 복사된 파일 제거
                patcher.ProcessClear();

                if (patcher.Archiver.Use)
                {
                    // how to archive??
                    ProgressForm progressForm = new ProgressForm("BackUp", true, new System.ComponentModel.DoWorkEventHandler(patcher.Archiver.StartArchive), true, targetExe);
                    progressForm.ShowDialog();
                }
            }
        }

        private static string GetVertion()
        {
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1];
            string[] tokens = strVersionText.Split('.');
            return string.Format("{0}.{1}", tokens[0], tokens[1]);
        }

        private static DateTime GetBuildDateTime()
        {
            //1. Assembly.GetExecutingAssembly().FullName의 값은  
            //'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' 
            //와 같다.  
            // DynMVP의 Build 날짜 가져옴
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터  
            //Build된 날짜까지의 총 일(Days) 수 이다. 
            int intDays = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            //3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된 
            //시간까지의 지나간 초(Second) 값 이다. 
            int intSeconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            //4. 시차조정 
            DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
                dtBuildDate = dtBuildDate.Add(daylingTime.Delta);

            return dtBuildDate;
        }


        internal static void AddStartupMenu(bool create)
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            DeleteShortcut(startupPath);
            if (create)
                CreateShortcut(startupPath);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DeleteShortcut(desktopPath);
            if (create)
                CreateShortcut(desktopPath);
        }

        internal static void CelarStartupMenu()
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            DeleteShortcutAll(startupPath);
        }

        internal static void CreateShortcut(string path)
        {
            string lnkName = GetMutexName();

            using (StreamWriter writer = new StreamWriter(path + "\\" + lnkName + ".url"))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string icon = app.Replace('\\', '/');
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine(@"URL=file:///" + icon);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + icon);
            }
        }

        internal static void DeleteShortcut(string path)
        {
            string lnkName = GetMutexName();

            DirectoryInfo dInfo = new DirectoryInfo(path);
            List<FileInfo> fInfoList = new List<FileInfo>();
            fInfoList.AddRange(dInfo.GetFiles("UnieyeLauncher.lnk"));
            fInfoList.AddRange(dInfo.GetFiles("UnieyeLauncherV2.lnk"));
            fInfoList.AddRange(dInfo.GetFiles("UnieyeLauncher.url"));
            fInfoList.AddRange(dInfo.GetFiles("UnieyeLauncherV2.url"));
            fInfoList.AddRange(dInfo.GetFiles($"{lnkName}.url"));
            fInfoList.ForEach(f => f.Delete());
        }

        internal static void DeleteShortcutAll(string path)
        {
            string lnkName = GetMutexName();

            DirectoryInfo dInfo = new DirectoryInfo(path);
            List<FileInfo> fInfoList = new List<FileInfo>();
            fInfoList.AddRange(dInfo.GetFiles("*.lnk"));
            fInfoList.AddRange(dInfo.GetFiles("*.url"));
            fInfoList.ForEach(f => f.Delete());
        }
    }
}
