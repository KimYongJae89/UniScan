using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace UniScanG
{
    public static class Program
    {
        public static Dictionary<string, string> CommandLineArgs { get; private set; } = new Dictionary<string, string>();

        public static bool ParseArgs(string[] args)
        {
            LogHelper.Debug(LoggerType.StartUp, $"Program::ParseArgs - args: {string.Join(" / ", args)}");
            if (!LicenseManager.Exist(LicenseManager.ELicenses.EnvArgs))
                return true;

            ParseArgs2(args.Skip(1).ToArray());
            
            if(CommandLineArgs.ContainsKey("-?"))
            {
                List<string> list = new List<string>();
                list.Add("-Restart");
                list.Add("-CameraConfiguration [name]");
                string msg = string.Join(Environment.NewLine, list);
                System.Windows.MessageBox.Show(msg);
                return false;
            }

            if (CommandLineArgs.ContainsKey("--"))
                return true;

            if (CommandLineArgs.ContainsKey("-RESTART"))
            {
                Process process = Process.GetCurrentProcess();
                Process[] processes = Array.FindAll(Process.GetProcessesByName(process.ProcessName), f => f.Id != process.Id);
                Array.ForEach(processes, f => f.Kill());
                Thread.Sleep(1000);
            }

            if (CommandLineArgs.ContainsKey("-CAMERACONFIGURATION"))
                DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag = CommandLineArgs["-CAMERACONFIGURATION"];

            foreach (KeyValuePair<string, string> pair in CommandLineArgs)
                LogHelper.Debug(LoggerType.StartUp, $"Program::ParseArgs - ( \"{pair.Key}\" / \"{pair.Value}\" )");

            return true;
        }

        private static void ParseArgs2(string[] args)
        {
            //"-restart":
            //"-cameracalibration":
            CommandLineArgs.Clear();

            int begin = Array.FindIndex(args, f => f.StartsWith("-"));
            while (begin >= 0 && begin < args.Length)
            {
                int end = Array.FindIndex(args, begin + 1, f => f.StartsWith("-"));
                if (end < 0)
                    end = args.Length;

                string key = args[begin].ToUpper();
                string value = "";
                int subLen = end - begin - 1;
                if (subLen > 0)
                    value = string.Join(" ", args, begin + 1, subLen);
                CommandLineArgs.Add(key, value.ToUpper());
                begin = end;
            }
        }
    }
}
