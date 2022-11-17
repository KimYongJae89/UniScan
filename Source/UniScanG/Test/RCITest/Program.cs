using DynMvp;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Vision.Calculator;

namespace RCITest
{
    static class Program
    {
        internal static DynMvp.Vision.ImagingLibrary ImagingLibrary => DynMvp.Vision.ImagingLibrary.MatroxMIL;

        internal static ImageProcessing ImageProcessing => imageProcessing;
        private static ImageProcessing imageProcessing = ImageProcessing.Create(ImagingLibrary);

        private static Dictionary<string, Stopwatch> stopwatchs = new Dictionary<string, Stopwatch>();
        private static int consoleIndent = 0;

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //MyConsole.ShowConsole();
            ConsoleEx.Alloc();
            ConsoleEx.AppendFrom = false;

            PowerStatus ps = SystemInformation.PowerStatus;

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary, ""));

            Application.Run(new Form1());

            //MyConsole.HideConsole();
            //ConsoleEx.Free();
        }

        private static void BeginTimeCheck(string key, string message, bool restart, bool writeConsole)
        {
            if (string.IsNullOrEmpty(key))
                throw new Exception("BeginTimeCheck - key is empty");

            if (!stopwatchs.ContainsKey(key))
                stopwatchs.Add(key, new Stopwatch());

            Stopwatch sw = stopwatchs[key];
            if (sw.IsRunning)
                throw new Exception("BeginTimeCheck - Timer is already running");

            if(restart)
                sw.Reset();

            sw.Start();

            if (writeConsole)
            {
                string indent = new string('\t', consoleIndent);
                //MyConsole.WriteLine($"{indent}[{key}]{message} - Begin");
                ConsoleEx.WriteLine($"{indent}[{key}]{message} - Begin");
                consoleIndent++;
            }
        }

        private static long EndTimeCheck(string key, string message, bool writeConsole, bool remove)
        {
            if (!stopwatchs.ContainsKey(key))
                throw new Exception("EndTimeCheck - key is not exist");

            Stopwatch sw = stopwatchs[key];
            Debug.Assert(sw.IsRunning);
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;

            if (writeConsole)
            {
                consoleIndent--;
                string indent = new string('\t', consoleIndent);
                //MyConsole.WriteLine($"{indent}[{key}]{message} - End ({sw.ElapsedMilliseconds}[ms])");
                ConsoleEx.WriteLine($"{indent}[{key}]{message} - End ({elapsedMilliseconds }[ms])");
            }

            if (remove)
                stopwatchs.Remove(key);

            return elapsedMilliseconds;
        }

        internal static void BeginTimeCheck(string key, string message="")
        {
            BeginTimeCheck(key, message, true, true);
        }

        internal static long EndTimeCheck(string key, string message = "")
        {
            return EndTimeCheck(key, message, true, true);
        }

        internal static int GetCheckedTime(string key)
        {
            if (!stopwatchs.ContainsKey(key))
                throw new Exception("EndTimeCheck - not exist key");

            return (int)stopwatchs[key].ElapsedMilliseconds;
        }

        internal static void PrintCheckedTime(string key, string message = "")
        {
            if (!stopwatchs.ContainsKey(key))
                throw new Exception("EndTimeCheck - not exist key");

            Stopwatch sw = stopwatchs[key];

            string indent = new string('\t', consoleIndent);
            ConsoleEx.WriteLine($"{indent}[{key}]{message} - ({sw.ElapsedMilliseconds}[ms])");
        }

        internal static void PrintAllCheckedTime()
        {
            foreach (var v in stopwatchs)
            {
                Stopwatch sw = v.Value;

                string indent = new string('\t', consoleIndent);
                ConsoleEx.WriteLine($"{indent}[{v.Key}] - ({sw.ElapsedMilliseconds}[ms])");
            }
        }

        internal static void TimeCheck(string key, string message, Action action)
        {
            BeginTimeCheck(key, message, true, true);
            try
            {
                action.Invoke();
            }
            finally { }
            EndTimeCheck(key, message, true, true);
        }

        internal static void ResetAllTimer()
        {
            foreach (var f in stopwatchs.Values)
                f.Stop();

            stopwatchs.Clear();
        }

        internal static T TimeCheck<T>(string key, string message, Func<T> func)
        {
            BeginTimeCheck(key, message, true, true);
            T result = func.Invoke();
            EndTimeCheck(key, message, true, true);
            return result;
        }

        internal static void AccumulateTimeCheck(string key, Action action)
        {
            BeginTimeCheck(key, "", false, false);
            action.Invoke();
            EndTimeCheck(key, "", false, false);
        }

        internal static T AccumulateTimeCheck<T>(string key, Func<T> func)
        {
            BeginTimeCheck(key, "", false, false);
            T result = func.Invoke();
            EndTimeCheck(key, "", false, false);
            return result;
        }

        internal static void ClearTimer()
        {
            var vv =  stopwatchs.ToList().FindAll(f => f.Value.IsRunning).ToArray();
            Debug.Assert(stopwatchs.All(f => !f.Value.IsRunning));
            stopwatchs.Clear();
        }
    }
}
