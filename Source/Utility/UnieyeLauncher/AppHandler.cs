using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnieyeLauncher
{
    static class AppHandler
    {

        public static bool IsAppRun(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        public static bool LockFileExist(string basePath)
        {
            string lockFile = Path.Combine(basePath, "Temp", "~UniEye.lock");
            return File.Exists(lockFile);
        }

        public static void StartApp(string exeFiles, string arguments = "")
        {
            Process proc = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/C \"{exeFiles}\" {arguments}",
                WorkingDirectory = Path.GetDirectoryName(exeFiles),
                WindowStyle = ProcessWindowStyle.Hidden
            });

            //ProcessStartInfo processStartInfo = new ProcessStartInfo(exeFiles);
            //processStartInfo.UseShellExecute = false;
            //processStartInfo.WorkingDirectory = Path.GetDirectoryName(exeFiles);
            //processStartInfo.Arguments = arguments;
            //Process.Start(processStartInfo);
        }

        public static void StartApp(string[] exeFiles)
        {
            Array.ForEach(exeFiles, f => 
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(f);
                processStartInfo.UseShellExecute = false;
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(f);
                Process.Start(processStartInfo);
            });
        }

        public static void KillApp(string[] processesNames)
        {
            List<Process> processList = new List<Process>();
            Array.ForEach(processesNames, f => processList.AddRange( Process.GetProcessesByName(f)));
            KillApp(processList.ToArray());
        }

        public static void KillApp(Process[] processes)
        {
            Array.ForEach(processes, f =>
            {
                if (!f.HasExited)
                    f.Kill();
            });

            List<Process> processNameList = processes.ToList();
            while (processNameList.Count > 0)
            {
                Thread.Sleep(200);
                processNameList.RemoveAll(f => f.HasExited);
            }
        }

        public static void MoveAllFiles(DirectoryInfo srcInfo, DirectoryInfo dstInfo, bool recursive)
        {
            FileInfo[] srcFiles = srcInfo.GetFiles("*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Array.ForEach(srcFiles, f =>
            {
                FileInfo dstFileInfo = new FileInfo(f.FullName.Replace(srcInfo.FullName, dstInfo.FullName));
                if (dstFileInfo.Exists)
                    dstFileInfo.Delete();

                f.MoveTo(dstFileInfo.FullName);         
            });
        }

        public static void MoveAllDirectories(DirectoryInfo srcInfo, DirectoryInfo dstInfo, bool recursive)
        {
            DirectoryInfo[] src = srcInfo.GetDirectories("*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Array.ForEach(src, f =>
            {
                DirectoryInfo dst = new DirectoryInfo(f.FullName.Replace(srcInfo.FullName, dstInfo.FullName));
                if (dst.Exists)
                    dst.Delete(true);
                Thread.Sleep(100);

                f.MoveTo(dst.FullName);
            });
        }

        public static void Clear(DirectoryInfo srcInfo)
        {
            Array.ForEach(srcInfo.GetFiles(), f => f.Delete());
            Array.ForEach(srcInfo.GetDirectories(), f => f.Delete(true));
        }

        public static void Clear(DirectoryInfo srcInfo, string filter)
        {
            Array.ForEach(srcInfo.GetFiles(filter), f => f.Delete());
        }
    }
}
